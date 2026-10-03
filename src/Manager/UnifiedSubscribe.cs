using HarmonyLib;
using MonoMod.RuntimeDetour;
using MonoMod.RuntimeDetour.HookGen;
using RWCustom;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static CommonUtils.Core.UnifiedHooks;
using static UnityEngine.EventSystems.EventTrigger;
namespace CommonUtils.Core;

public static class UnifiedSubscribe
{
	public static event On.Weapon.hook_HitSomething HitSomething
	{
		add
		{
			foreach (var m in targets_HitSomething())
				HookEndpointManager.Add(m, value);
		}
		remove
		{
			foreach (var m in targets_HitSomething())
				HookEndpointManager.Remove(m, value);
		}
	}
	public static event On.Creature.hook_Violence Violence
	{
		add
		{
			foreach (var m in targets_Violence())
				HookEndpointManager.Add(m, value);
		}
		remove
		{
			foreach (var m in targets_Violence())
				HookEndpointManager.Remove(m, value);
		}
	}
	public static event hook_IsWallClimber IsWallClimber
	{
		add
		{
			HookEndpointManager.Add(targets_IsWallClimber, value);
		}
		remove
		{
			HookEndpointManager.Remove(targets_IsWallClimber, value);
		}
	}
}
public static class UnifiedHooks
{
	public static IEnumerable<MethodBase> targets_HitSomething() => HookScanner.GetMethods(typeof(Weapon), nameof(Weapon.HitSomething));

	public static IEnumerable<MethodBase> targets_Violence() => HookScanner.GetMethods(typeof(Creature), nameof(Creature.Violence));

	public delegate bool orig_IsWallClimber(Lizard self);
	public delegate bool hook_IsWallClimber(orig_IsWallClimber orig, Lizard self);
	public static MethodBase targets_IsWallClimber => AccessTools.PropertyGetter(typeof(Lizard), nameof(Lizard.IsWallClimber));

}
public static class HookScanner
{
	static readonly List<Hook> _hooks = [];
	static readonly HashSet<string> _hooked = [];

	// 把 baseType 及其所有后代里名字为 methodName 的方法，全部挂到 handler 上。
	public static void Apply(Type baseType, string methodName, Type handlerType, string handlerName)
	{
		var handler = handlerType.GetMethod(handlerName,
			BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
			?? throw new InvalidOperationException($"Handler {handlerType.Name}.{handlerName} not found");

		Apply(baseType, methodName, handler);
	}
	// handler 的签名必须和原方法一致（含 self + orig）。
	public static void Apply(Type baseType, string methodName, MethodInfo handler)
	{
		static string HookKey(MethodBase m, MethodInfo handler)
			=> $"{Key(m)}->{handler.MetadataToken}";

		foreach (var m in GetMethods(baseType, methodName))
		{
			var key = HookKey(m, handler);
			if (!_hooked.Add(key)) continue;   // 已挂过

			try
			{
				_hooks.Add(new Hook(m, handler));
				Log.LogInfo($"[BulkHook] {m.DeclaringType.FullName}.{m.Name} -> {handler.DeclaringType.FullName}.{handler.Name}");
			}
			catch (Exception ex)
			{
				Log.LogError($"[BulkHook] fail {m.DeclaringType.FullName}.{m.Name}: {ex.Message}");
				_hooked.Remove(key);   // 失败回滚
			}
		}
	}

	public static void Undo()
	{
		foreach (var h in _hooks) h.Dispose();
		_hooks.Clear();
		_hooked.Clear();
		// _cache 不清，因为方法集合没变
	}

	// ---------- 查找与缓存 ----------

	static readonly Dictionary<string, List<MethodBase>> _cache = [];
	public static List<MethodBase> GetMethods(Type baseType, string methodName)
	{
		var key = $"{baseType.FullName}::{methodName}";
		if (_cache.TryGetValue(key, out var cached)) return cached;

		var list = FindOverrides(baseType, methodName).ToList();
		_cache[key] = list;
		return list;
	}

	static IEnumerable<MethodBase> FindOverrides(Type baseType, string methodName)
	{
		const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic
								 | BindingFlags.Instance | BindingFlags.Static
								 | BindingFlags.DeclaredOnly;

		// 先找基类方法，推导参数类型
		var baseMethod = baseType.GetMethods(flags).FirstOrDefault(x => x.Name == methodName);
		var paramTypes = baseMethod?.GetParameters().Select(p => p.ParameterType).ToArray();

		MethodInfo? Find(Type t, string name, BindingFlags f)
		{
			if (paramTypes == null) return null;
			return t.GetMethod(name, f, null, paramTypes, null);
		}

		var seen = new HashSet<string>();

		if (baseMethod != null)
		{
			seen.Add(Key(baseMethod));
			yield return baseMethod;
		}
		else
		{
			throw new InvalidOperationException($"Base method {baseType.FullName}.{methodName} not found");
		}

		foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
		{
			Type[] types;
			try
			{
				types = asm.GetTypes();
			}
			catch (ReflectionTypeLoadException e)
			{
				types = e.Types.Where(t => t != null).ToArray();
			}
			catch (NotSupportedException)
			{
				continue;
			}
			catch (Exception ex)
			{
				Log.LogWarning($"[BulkHook] skip {asm.GetName().Name}: {ex.GetType().Name}");
				continue;
			}

			foreach (var t in types)
			{
				if (t == null || t.IsInterface || t == baseType) continue;
				if (!baseType.IsAssignableFrom(t)) continue;

				var m = Find(t, methodName, flags);
				if (m == null || m.IsAbstract) continue;
				if (!seen.Add(Key(m))) continue;
				yield return m;
			}
		}
	}

	static string Key(MethodBase m)
		=> $"{m.DeclaringType.FullName}::{m.MetadataToken}";
}
