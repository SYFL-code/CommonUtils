#region using
using BepInEx.Logging;
using RWCustom;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
#endregion

namespace CommonUtils;

public abstract class CustomLogger
{
	public ManualLogSource? Logger;

	private bool isInitialized;

	public abstract bool EnableLog { get; }
	public virtual LogSeverity CurrentSeverity { get; } = LogSeverity.Development;
	public abstract bool isDevMod { get; }
	public virtual string ModName => "MyMod";

	#region 日志文件
	public string? _baseLogsDirectory;
	public virtual string BaseLogsDirectory
	{
		get
		{
			try
			{
				_baseLogsDirectory ??= Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, ModName, "Logs");
				return _baseLogsDirectory;
			}
			catch (Exception ex)
			{
				Logger?.LogError($"Fail to get base Dir: {ex}");
				return Path.Combine("C:\\", ModName, "Logs");
			}
		}
	}
	// 输出日志文件
	public virtual string OutputLogFilePath => Path.Combine(BaseLogsDirectory, "output_log.txt");
	#endregion

	public void Initialize()
	{
		try
		{
			// 创建 BepInEx 日志源
			Logger = BepInEx.Logging.Logger.CreateLogSource(ModName);

			if (!Directory.Exists(BaseLogsDirectory))
			{
				Directory.CreateDirectory(BaseLogsDirectory);
				Logger?.LogDebug($"Create base Log Dir: {BaseLogsDirectory}");
			}

			if (!File.Exists(OutputLogFilePath))
			{
				File.WriteAllText(OutputLogFilePath, $"# Output Log File - created at {DateTime.Now:yyyy-MM-dd HH:mm:ss}{Environment.NewLine}");
				Logger?.LogDebug($"Create Output Log File: {OutputLogFilePath}");
			}

			isInitialized = true;

			Logger?.LogMessage($"{ModName} file system init complete");
		}
		catch (Exception e)
		{
			Logger?.LogError($"Fail to init file system: {e}");
		}
	}
	private void EnsureInitialized()
	{
		if (!isInitialized)
		{
			Initialize();
		}
	}


	// 内存中的日志文本缓存（累积所有日志条目）
	public string LogText = "";
	// 上次成功保存到文件的日志文本快照
	private string lastLogText = "";
	// 日志条目列表
	public List<string> Logs = [];

	// 日志文本
	public string LogTextValue
	{
		get
		{
			return LogText;
		}
		set
		{
			if (LogText != value)
			{
				LogText = value;
				SaveLogText();
			}
		}
	}


	// 当日志文本被追加时触发的事件。
	// 可供外部订阅（如游戏内实时日志面板 UI）。
	public event Action<string>? OnAppendLogText;

	// 追加日志文本
	public void AppendLogText(string text)
	{
		if (!string.IsNullOrEmpty(text))
		{
			try
			{
				EnsureInitialized();

				if (!string.IsNullOrEmpty(LogText))
				{
					LogText = LogText + Environment.NewLine + text;
				}
				else
				{
					LogText = text;
				}

				Logs.Add(text);

				this.OnAppendLogText?.Invoke(text);

				SaveLogText();
			}
			catch (Exception e)
			{
				BaseLogError($"Fail to add log: {e}");
			}
		}
	}

	// 保存日志文本
	private void SaveLogText()
	{
		if (isInitialized)
		{
			if (LogText != lastLogText)
			{
				try
				{
					string content = $"# Output Log File - Last update at {DateTime.Now:yyyy-MM-dd HH:mm:ss}{Environment.NewLine}";

					content += LogText;

					File.WriteAllText(OutputLogFilePath, content, Encoding.UTF8);

					lastLogText = LogText;
				}
				catch (Exception e)
				{
					Logger?.LogError($"Fail to save Output Log: {e}");
				}
			}
		}
	}

	// 清空日志文本和日志文件
	public void ClearLogText()
	{
		LogText = "";
		SaveLogText();
		BaseLogMessage("Clear text in output log file");
	}
	// 强制立即保存所有日志到文件
	public void ForceSaveAll()
	{
		SaveLogText();
		BaseLogMessage("All text are saved forcely");
	}
	public void WriteText()
	{
		ForceSaveAll();
	}

	#region BaseLog
	public void BaseLogDevelopment(object Message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
	{
		BaseLog(Message, LogSeverity.Development, memberName, filePath, lineNumber);
	}
	public void BaseLogDebug(object Message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
	{
		BaseLog(Message, LogSeverity.Debug, memberName, filePath, lineNumber);
	}
	public void BaseLogInfo(object Message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
	{
		BaseLog(Message, LogSeverity.Info, memberName, filePath, lineNumber);
	}
	public void BaseLogMessage(object Message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
	{
		BaseLog(Message, LogSeverity.Message, memberName, filePath, lineNumber);
	}
	public void BaseLogWarning(object Message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
	{
		BaseLog(Message, LogSeverity.Warning, memberName, filePath, lineNumber);
	}
	public void BaseLogError(object Message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
	{
		BaseLog(Message, LogSeverity.Error, memberName, filePath, lineNumber);
	}
	public void BaseLogException(object Message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
	{
		BaseLog(Message, LogSeverity.Exception, memberName, filePath, lineNumber);
	}
	public void BaseLogFatal(object Message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
	{
		BaseLog(Message, LogSeverity.Fatal, memberName, filePath, lineNumber);
	}
	#endregion

	public enum LogSeverity
	{
		Fatal,
		Exception,
		Error,
		Warning,
		Message,
		Info,
		Debug,
		Development,
	}

	public void BaseLog(object Message, LogSeverity severity, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
	{
		EnsureInitialized();

		if (!ShouldLog(severity))
			return;

		if (isInitialized && Logger != null)
		{
			object translatedMessage = (Message is string s) ? Translate(s): Message;

			string className = Path.GetFileNameWithoutExtension(filePath);

			bool logPath = false;
			string type;

			switch (severity)
			{
				case LogSeverity.Development:
					if (!isDevMod)
						return;
					Logger.LogDebug(translatedMessage);
					type = "[DEVELOPMENT]";

					break;
				case LogSeverity.Debug:
					Logger.LogDebug(translatedMessage);
					type = "[DEBUG]";

					break;
				case LogSeverity.Warning:
					Logger.LogWarning(translatedMessage);
					type = "[WARN]";

					break;
				case LogSeverity.Error:
					Logger.LogError($"[{className}.{memberName}:{lineNumber}] {translatedMessage}({filePath})");
					//Logger.LogError($"{Path.GetFileName(filePath)}({lineNumber}) - {memberName}:{translatedMessage}({filePath})");

					type = "[ERROR]";
                    logPath = true;
                    break;
				case LogSeverity.Fatal:
					Logger.LogFatal(translatedMessage);

					type = "[FATAL]";
                    logPath = true;
                    break;
				case LogSeverity.Exception:
					if (Message is Exception ex)
					{
						string message = Translate($"An error has occurred from {ex.Source} at: {ex}: {ex.Message}\n" +
							$"-----Stack Trace-----\n{ex.StackTrace}\n" +
							$"-----Target Site-----\n{ex.TargetSite}\n" +
							$"-----Inner Exception-----\n{ex.InnerException}");


						Logger.LogError($"[{className}.{memberName}:{lineNumber}] {memberName}({filePath})");
						//Logger.LogError($"{Path.GetFileName(filePath)}({lineNumber}) - {memberName}:{message}({filePath})");

						type = "[EXCEPTION]";
					}
					else
					{
						Logger.LogFatal($"{Path.GetFileName(filePath)}({lineNumber}) - {memberName}:{translatedMessage}({filePath})");

						type = "[FATAL]";
					}
					logPath = true;
					break;

				case LogSeverity.Message:
					Logger.LogMessage(translatedMessage);

					type = "[MESSAGE]";
					break;
				case LogSeverity.Info:
					Logger.LogInfo(translatedMessage);

					type = "[INFO]";
					break;
				default:
					Logger.LogInfo(translatedMessage);

					type = "[INFO]";
					break;
			}

			AppendLogText($"{type} [{DateTime.Now:HH:mm:ss}] " +
				$"[{className}.{memberName}:{lineNumber}] {translatedMessage}" + (logPath ? $"({filePath})" : ""));

			//AppendLogText($"{type} {DateTime.Now:HH:mm:ss} - " +
			//	$"{Path.GetFileName(filePath)}({lineNumber}) - {memberName}: {translatedMessage}" + (logPath ? $"({filePath})" : ""));
		}
	}

	private bool ShouldLog(LogSeverity severity)
	{
		if (!EnableLog) return false;

		return CurrentSeverity >= severity;
	}

	public string Translate(string text)
	{
		try
		{
			string TranslateText = Custom.rainWorld?.inGameTranslator?.Translate(text) ?? text;

			return (string.IsNullOrEmpty(TranslateText) || TranslateText == "!NO TRANSLATION!")
				? text : TranslateText;
		}
		catch (Exception ex)
		{
			Logger?.LogWarning($"{ex}");
		}
		return text;
	}

}
