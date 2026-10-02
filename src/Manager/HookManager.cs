using CommonUtils.Misc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;

namespace CommonUtils.Core;

[Credits("选择部分")]
// 初始化所有已注册的钩子（Hooks）。
// 根据钩子是否要求单线程执行，分别采用并行或串行方式初始化。
public static class HookManager
{
	public static Dictionary<string, HookData> datas = [];
	private static readonly object _dataLock = new();

	public const int Top = int.MaxValue;

	public static void Initialize()
	{
		// 并行挂钩
		List<KeyValuePair<string, HookData>> parallelHooks = datas.Where(kv => !kv.Value.RequireSingleThread).ToList();

		if (parallelHooks.Count > 0)
		{
			// 并行选项
			ParallelOptions options = new ParallelOptions
			{
				// 最大并行度
				MaxDegreeOfParallelism = 15
			};
			Parallel.ForEach(parallelHooks, options, delegate (KeyValuePair<string, HookData> kv)
			{
				HookData data = kv.Value;

				if (data.InitializeHooks != null && !data.isInitialized)
				{
					try
					{
						HookData hookData = data;
						lock (hookData)
						{
							if (!data.isInitialized)
							{
								data.InitializeHooks();
								data.isInitialized = true;
								Log.LogDebug($"Initialized hooks for {kv.Key}");
							}
						}
					}
					catch (Exception ex)
					{
						Log.LogError($"Error initializing {kv.Key}: {ex.Message}");
					}
				}
			});
		}


		List<KeyValuePair<string, HookData>> singleThreadHooks = datas.Where(kv => kv.Value.RequireSingleThread)
			.OrderByDescending(kv => kv.Value.Priority).ToList();
		foreach (KeyValuePair<string, HookData> kv in singleThreadHooks)
		{
			HookData data = kv.Value;

			if (data.InitializeHooks != null && !data.isInitialized)
			{
				try
				{
					data.InitializeHooks();
					data.isInitialized = true;
					Log.LogDebug($"Initialized {kv.Key} (single-threaded), priority {data.Priority})");
				}
				catch (Exception ex)
				{
					Log.LogError($"Error initializing {kv.Key}: {ex.Message}");
				}
			}
		}
	}
	public static void UnInitializeAll()
	{
		var uninitHooks = datas.Where(kv => kv.Value.isInitialized)
			.OrderBy(kv => kv.Value.Priority)
			.ToList();

		// 串行注销（保证顺序和线程安全）
		foreach (var kv in uninitHooks)
		{
			var data = kv.Value;

			if (data.UnInitializeHooks != null)
			{
				try
				{
					data.UnInitializeHooks();
					data.isInitialized = false;
					Log.LogDebug($"Uninitialized {kv.Key}");
				}
				catch (Exception ex)
				{
					Log.LogError($"Error uninitializing {kv.Key}: {ex.Message}");
				}
			}
		}

		// 清空字典（需加锁）
		lock (_dataLock)
		{
			datas.Clear();
		}
	}


	//private static int _hookCounter = 0;

	public static void Register(bool RequireSingleThread = true, [CallerLineNumber] int Priority = 0,
		Action? Hook = null, Action? UnHook = null,
		[CallerArgumentExpression(nameof(Hook))] string hookExpression = "",
		[CallerArgumentExpression(nameof(UnHook))] string UnHookExpression = "",
		[CallerFilePath] string filePath = "")
	{
		try
		{
			HookData data = new HookData
			{
				RequireSingleThread = RequireSingleThread,
				Priority = Priority,
				InitializeHooks = Hook,
				UnInitializeHooks = UnHook
			};

			int arrowIndex = hookExpression.IndexOf("=>");
			string hookCode = arrowIndex >= 0 ? hookExpression[(arrowIndex + "=>".Length)..].Trim() : hookExpression;

			string[] hookCodeParts = Regex.Split(hookCode, @"\+=");
			string hookName = hookCodeParts.Length > 1 ? hookCodeParts[1].Trim() : "";
			string[] hookNameParts = Regex.Split(hookName, @"\.");
			string className = hookNameParts.Length > 0 ? hookNameParts[0].Trim() : "null";
			//string className = Path.GetFileNameWithoutExtension(filePath);

			if (hookExpression.Contains("+="))
			{
				string ReplaceExpression = hookExpression.Replace("+=", "-=");
				if (ReplaceExpression != UnHookExpression)
				{
					Log.LogError($"Hook and UnHook do not match!!");
					Log.LogError($"hookExpression:{hookExpression}");
					Log.LogError($"UnHookExpression:{UnHookExpression}");
					return;
				}
			}
			if (hookExpression == UnHookExpression)
			{
				Log.LogError($"Hook is equal to UnHook!!");
				Log.LogError($"hookExpression:{hookExpression}");
				Log.LogError($"UnHookExpression:{UnHookExpression}");
				return;
			}

			Log.LogDebug($"hookCode:{hookCode} #{Priority}");
			Register($"{hookCode} ({className})", data);
		}
		catch (Exception ex)
		{
			Log.LogError($"Error in Register: {ex.Message}");
		}
	}
	public static void Register(string ID, HookData data)
	{
		if (data == null)
		{
			throw new ArgumentNullException("Hook data can not be null");
		}
		data.ID = ID;

		if (!datas.Keys.Contains(ID))
		{
			datas.Add(ID, data);
		}
		else
		{
			Log.LogError("Hook already registered: " + ID);
		}
	}
	public static void Unregister(string ID)
	{
		lock (_dataLock)
		{
			if (datas.TryGetValue(ID, out var data))
			{
				// 先注销再移除
				if (data.UnInitializeHooks != null && data.isInitialized)
				{
					try
					{
						data.UnInitializeHooks();
						data.isInitialized = false;
						Log.LogDebug($"Unregistered {ID}");
					}
					catch (Exception ex)
					{
						Log.LogError($"Error unregistering {ID}: {ex.Message}");
					}
				}
				datas.Remove(ID);
			}
			else
			{
				Log.LogError($"Hook not registered: {ID}");
				return;
			}
		}
	}

	public class HookData
	{
		// 要求单线程。
		// 若为 true，则在 Initialize 中会串行执行；
		// 若为 false，则允许并行初始化以提高启动速度。
		public bool RequireSingleThread { get; set; } = true;
		// 优先级
		// 数值越小，优先级越高
		public int Priority { get; set; } = 0;

		public string ID = string.Empty;

		// 标记该钩子是否已经初始化，防止重复执行。
		public bool isInitialized;

		// 初始化钩子的委托，由外部注册者提供具体逻辑。
		public Action? InitializeHooks;
		// 反初始化钩子的委托
		public Action? UnInitializeHooks;
	}
}