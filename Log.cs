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
#endregion

namespace CommonUtils;

internal class Log : CustomLogger
{
	public static Log Instance
	{
		get
		{
			instance ??= new Log();
			instance.Initialize();
			return instance;
		}
	}
	private static Log? instance;

	public override string ModName => Plugin.Name;
	public override bool EnableLog => Plugin.ForceLog;
	public override LogSeverity CurrentSeverity { get; } = LogSeverity.Development;
	public override bool isDevMod => Plugin.DebugMode;


	#region Log
	internal static void LogDevelopment(object Message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
	{
		Instance.BaseLogDevelopment(Message, memberName, filePath, lineNumber);
	}
	internal static void LogDebug(object Message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
	{
		Instance.BaseLogDebug(Message, memberName, filePath, lineNumber);
	}
	internal static void LogInfo(object Message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
	{
		Instance.BaseLogInfo(Message, memberName, filePath, lineNumber);
	}
	internal static void LogMessage(object Message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
	{
		Instance.BaseLogMessage(Message, memberName, filePath, lineNumber);
	}
	internal static void LogWarning(object Message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
	{
		Instance.BaseLogWarning(Message, memberName, filePath, lineNumber);
	}
	internal static void LogError(object Message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
	{
		Instance.BaseLogError(Message, memberName, filePath, lineNumber);
	}
	internal static void LogException(object Message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
	{
		Instance.BaseLogException(Message, memberName, filePath, lineNumber);
	}
	internal static void LogFatal(object Message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
	{
		Instance.BaseLogFatal(Message, memberName, filePath, lineNumber);
	}
	#endregion

}