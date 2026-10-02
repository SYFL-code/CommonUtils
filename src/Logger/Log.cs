#region using
using BepInEx.Logging;
using Kittehface.Build;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;
#endregion

namespace CommonUtils.Core;

public class Log : CustomLogger
{
    public static Log Instance
	{
		get
		{
			if (instance == null)
			{
				instance ??= new Log();
				instance.Initialize();
			}
			return instance;
		}
	}
	private static Log? instance;

	//public required string Name;
	//public required bool ForceLog;
	//public required bool DebugMode;

	public override string ModName => Plugin.Name;
	public override bool EnableLog => Plugin.ForceLog;
	public override LogSeverity CurrentSeverity { get; } = LogSeverity.Development;
	public override bool isDevMod => Plugin.DebugMode;


	#region Log
	internal static void LogDevelopment<T>(T Message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
	{
		Instance.BaseLogDevelopment(Message, memberName, filePath, lineNumber);
	}
	internal static void LogDebug<T>(T Message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
	{
		Instance.BaseLogDebug(Message, memberName, filePath, lineNumber);
	}
	internal static void LogInfo<T>(T Message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
	{
		Instance.BaseLogInfo(Message, memberName, filePath, lineNumber);
	}
	internal static void LogMessage<T>(T Message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
	{
		Instance.BaseLogMessage(Message, memberName, filePath, lineNumber);
	}
	internal static void LogWarning<T>(T Message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
	{
		Instance.BaseLogWarning(Message, memberName, filePath, lineNumber);
	}
	internal static void LogError<T>(T Message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
	{
		Instance.BaseLogError(Message, memberName, filePath, lineNumber);
	}
	internal static void LogException<T>(T Message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
	{
		Instance.BaseLogException(Message, memberName, filePath, lineNumber);
	}
	internal static void LogFatal<T>(T Message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
	{
		Instance.BaseLogFatal(Message, memberName, filePath, lineNumber);
	}
	#endregion

	public static void Assert<T>(bool condition, T message, [CallerArgumentExpression("condition")] string expression = "",
	[CallerMemberName] string caller = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
	{
		if (!condition)
			Instance.BaseLog($"断言失败:{message}, {expression}: {condition}", LogSeverity.Error, caller, filePath, lineNumber);
	}

	#region 参数
	// 1个参数
	public static void LogVar<T1>(LogSeverity severity, T1 v1, [CallerMemberName] string caller = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0,
		[CallerArgumentExpression(nameof(v1))] string n1 = "")
		 => Instance.BaseLog($"{n1}:{v1}", severity, caller, filePath, lineNumber);
	// 2个参数
	public static void LogVar<T1, T2>(LogSeverity severity, T1 v1, T2 v2, [CallerMemberName] string caller = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0,
		[CallerArgumentExpression(nameof(v1))] string n1 = "", [CallerArgumentExpression(nameof(v2))] string n2 = "")
		 => Instance.BaseLog($"{n1}:{v1}, {n2}:{v2}", severity, caller, filePath, lineNumber);
	// 3个参数
	public static void LogVar<T1, T2, T3>(LogSeverity severity, T1 v1, T2 v2, T3 v3, [CallerMemberName] string caller = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0,
		[CallerArgumentExpression(nameof(v1))] string n1 = "", [CallerArgumentExpression(nameof(v2))] string n2 = "", [CallerArgumentExpression(nameof(v3))] string n3 = "")
		 => Instance.BaseLog($"{n1}:{v1}, {n2}:{v2}, {n3}:{v3}", severity, caller, filePath, lineNumber);
	// 4个参数
	public static void LogVar<T1, T2, T3, T4>(LogSeverity severity, T1 v1, T2 v2, T3 v3, T4 v4, [CallerMemberName] string caller = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0,
		[CallerArgumentExpression(nameof(v1))] string n1 = "", [CallerArgumentExpression(nameof(v2))] string n2 = "", [CallerArgumentExpression(nameof(v3))] string n3 = "",
		[CallerArgumentExpression(nameof(v4))] string n4 = "")
		 => Instance.BaseLog($"{n1}:{v1}, {n2}:{v2}, {n3}:{v3}, {n4}:{v4}", severity, caller, filePath, lineNumber);
	// 5个参数
	public static void LogVar<T1, T2, T3, T4, T5>(LogSeverity severity, T1 v1, T2 v2, T3 v3, T4 v4, T5 v5, [CallerMemberName] string caller = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0,
		[CallerArgumentExpression(nameof(v1))] string n1 = "", [CallerArgumentExpression(nameof(v2))] string n2 = "", [CallerArgumentExpression(nameof(v3))] string n3 = "",
		[CallerArgumentExpression(nameof(v4))] string n4 = "", [CallerArgumentExpression(nameof(v5))] string n5 = "")
		 => Instance.BaseLog($"{n1}:{v1}, {n2}:{v2}, {n3}:{v3}, {n4}:{v4}, {n5}:{v5}", severity, caller, filePath, lineNumber);

	// 1个参数
	public static void LogVar<T1>(T1 v1, LogSeverity severity = LogSeverity.Info, [CallerMemberName] string caller = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0,
		[CallerArgumentExpression(nameof(v1))] string n1 = "")
		 => Instance.BaseLog($"{n1}:{v1}", severity, caller, filePath, lineNumber);
	// 2个参数
	public static void LogVar<T1, T2>(T1 v1, T2 v2, LogSeverity severity = LogSeverity.Info, [CallerMemberName] string caller = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0,
		[CallerArgumentExpression(nameof(v1))] string n1 = "", [CallerArgumentExpression(nameof(v2))] string n2 = "")
		 => Instance.BaseLog($"{n1}:{v1}, {n2}:{v2}", severity, caller, filePath, lineNumber);
	// 3个参数
	public static void LogVar<T1, T2, T3>(T1 v1, T2 v2, T3 v3, LogSeverity severity = LogSeverity.Info, [CallerMemberName] string caller = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0,
		[CallerArgumentExpression(nameof(v1))] string n1 = "", [CallerArgumentExpression(nameof(v2))] string n2 = "", [CallerArgumentExpression(nameof(v3))] string n3 = "")
		 => Instance.BaseLog($"{n1}:{v1}, {n2}:{v2}, {n3}:{v3}", severity, caller, filePath, lineNumber);
	// 4个参数
	public static void LogVar<T1, T2, T3, T4>(T1 v1, T2 v2, T3 v3, T4 v4, LogSeverity severity = LogSeverity.Info, [CallerMemberName] string caller = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0,
		[CallerArgumentExpression(nameof(v1))] string n1 = "", [CallerArgumentExpression(nameof(v2))] string n2 = "", [CallerArgumentExpression(nameof(v3))] string n3 = "",
		[CallerArgumentExpression(nameof(v4))] string n4 = "")
		 => Instance.BaseLog($"{n1}:{v1}, {n2}:{v2}, {n3}:{v3}, {n4}:{v4}", severity, caller, filePath, lineNumber);
	// 5个参数
	public static void LogVar<T1, T2, T3, T4, T5>(T1 v1, T2 v2, T3 v3, T4 v4, T5 v5, LogSeverity severity = LogSeverity.Info, [CallerMemberName] string caller = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0,
		[CallerArgumentExpression(nameof(v1))] string n1 = "", [CallerArgumentExpression(nameof(v2))] string n2 = "", [CallerArgumentExpression(nameof(v3))] string n3 = "",
		[CallerArgumentExpression(nameof(v4))] string n4 = "", [CallerArgumentExpression(nameof(v5))] string n5 = "")
		 => Instance.BaseLog($"{n1}:{v1}, {n2}:{v2}, {n3}:{v3}, {n4}:{v4}, {n5}:{v5}", severity, caller, filePath, lineNumber);
	#endregion

	public override void BaseLog<T>(T Message, LogSeverity severity, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
	{
		EnsureInitialized();

		if (!ShouldLog(severity)) return;

		if (isInitialized && Logger != null)
		{
			string className = Path.GetFileNameWithoutExtension(filePath);

			string translatedMsg = (Message is string s) ? Translate(s) : Message?.ToString() ?? "null";
			string Msg = $"{Plugin.version}|{DateTime.Now:HH:mm:ss}[{className.Left(3)}.{memberName.Left(4)}:{lineNumber}]{translatedMsg}";
			/*
			v01|13:34:26[Plu.OnEn:135]Mod OnEnable!
			*/

			bool logPath = false;
			string typeTag;


			InvokeOnAppendLog(Msg);

			switch (severity)
			{
				case LogSeverity.Development:
					if (!isDevMod) return;
					Logger.LogDebug(Msg);
					typeTag = "[DEV]";
					break;
				case LogSeverity.Debug:
					Logger.LogDebug(Msg);
					typeTag = "[DEB]";
					break;
				case LogSeverity.Info:
					Logger.LogInfo(Msg);
					typeTag = "[INF]";
					break;
				case LogSeverity.Message:
					Logger.LogMessage(Msg);
					typeTag = "[MES]";
					break;
				case LogSeverity.Warning:
					Logger.LogWarning(Msg);
					typeTag = "[WAR]";
					break;
				case LogSeverity.Error:
					Logger.LogError(Msg);
					typeTag = "[ERR]";
					logPath = true;
					break;
				case LogSeverity.Fatal:
					Logger.LogFatal(Msg);
					typeTag = "[FAT]";
					logPath = true;
					break;
				case LogSeverity.Exception:
					Logger.LogError(Msg);
					typeTag = "[EXC]";
					logPath = true;
					break;
				default:
					Logger.LogInfo(Msg);
					typeTag = "[INF]";
					break;
			}

			string fileLog = $"{typeTag} {Plugin.version}|{DateTime.Now:HH:mm:ss}[{className}.{memberName}:{lineNumber}] {translatedMsg}" +
							 (logPath ? $" ({filePath})" : "");
			/*
			[INF]v01|13:34:26[Plugin.OnEnable:135] Mod OnEnable!
			*/

			AppendLogText(fileLog);
		}
	}

}