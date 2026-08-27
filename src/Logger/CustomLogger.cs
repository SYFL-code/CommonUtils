#region using
using BepInEx.Logging;
using RWCustom;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using UnityEngine;
#endregion

namespace CommonUtils.Core;

public abstract class CustomLogger
{
	public ManualLogSource? Logger;

	protected bool isInitialized;
	private bool _disposed = false;

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
				_baseLogsDirectory ??= Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Logs", ModName);
				return _baseLogsDirectory;
			}
			catch (Exception ex)
			{
				Logger?.LogError($"Fail to get base Dir: {ex}");
				return Path.Combine("C:\\", "Logs", ModName);
			}
		}
	}
	// 输出日志文件
	public virtual string OutputLogFilePath => Path.Combine(BaseLogsDirectory, "output_log.txt");
	#endregion

	public void Initialize()
	{
		if (isInitialized) return;

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

			// 启动定时器：每 3 秒自动刷盘一次
			_flushTimer = new Timer(Flush, null, 3000, 3000);

			// 注册进程退出钩子，保证最后几条日志不丢失
			AppDomain.CurrentDomain.ProcessExit += (s, e) => { ForceSaveAll(); };
			AppDomain.CurrentDomain.UnhandledException += (s, e) => { ForceSaveAll(); };

			isInitialized = true;
			Logger?.LogMessage($"{ModName} file system init complete");
		}
		catch (Exception e)
		{
			Logger?.LogError($"Fail to init file system: {e}");
		}
	}
	public void EnsureInitialized()
	{
		if (!isInitialized)
		{
			Initialize();
		}
	}


	// 内存缓存与异步写入队列
	private readonly object _lock = new object();
	private readonly List<string> _memoryLogs = [];      // 用于 UI 显示（最多 200 条）
	private readonly Queue<string> _writeQueue = new Queue<string>();    // 待写入文件的队列
	private const int MaxMemoryLogs = 200;                               // 内存保留上限

	private Timer? _flushTimer;
	private bool _hasPendingWrites = false;
	private string _cachedLogText = "";                                 // 缓存拼接后的文本，避免每次 get 都遍历

	// 日志文本
	// ============ 属性：LogText（仅返回内存中的最近 200 条，用于 UI） ============
	public string LogTextValue
	{
		get
		{
			lock (_lock) return _cachedLogText;
		}
		set
		{
			if (value == null) return;
			lock (_lock)
			{
				_memoryLogs.Clear();
				if (!string.IsNullOrEmpty(value))
				{
					var lines = value.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
					foreach (var line in lines) _memoryLogs.Add(line);
					if (_memoryLogs.Count > MaxMemoryLogs)
						_memoryLogs.RemoveRange(0, _memoryLogs.Count - MaxMemoryLogs);
					_cachedLogText = string.Join(Environment.NewLine, _memoryLogs);
				}
				else
				{
					_cachedLogText = "";
				}
			}
		}
	}


	// 当日志文本被追加时触发的事件。
	public event Action<string>? OnAppendLogText;

	// 追加日志文本
	public void AppendLogText(string text)
	{
		if (string.IsNullOrEmpty(text)) return;
		EnsureInitialized();

		lock (_lock)
		{
			// 1. 写入内存缓存（供 UI 显示）
			_memoryLogs.Add(text);
			if (_memoryLogs.Count > MaxMemoryLogs)
			{
				_memoryLogs.RemoveAt(0); // 移除最旧的一条
			}

			// 2. 更新缓存字符串（用于 LogText 属性）
			_cachedLogText = string.Join(Environment.NewLine, _memoryLogs);

			// 3. 加入写入队列（异步落盘）
			_writeQueue.Enqueue(text);
			_hasPendingWrites = true;

			// 4. 如果队列积压超过 50 条，立即触发一次刷盘（防止内存队列无限增长）
			if (_writeQueue.Count >= 50)
			{
				// 注意：这里不直接调用 Flush，而是触发定时器立即回调，避免阻塞主线程
				_flushTimer?.Change(0, 3000);
			}
		}

		// 5. 触发 UI 事件（注意：如果订阅者更新 UI，需确保在主线程，此处仅触发）
		try
		{
			OnAppendLogText?.Invoke(text);
		}
		catch (Exception ex)
		{
			Logger?.LogError($"OnAppendLogText event error: {ex}");
		}

	}

	// ============ 定时刷盘 + 手动刷盘 ============
	private void Flush(object? state = null)
	{
		if (!isInitialized || _disposed) return;

		List<string>? logsToWrite = null;
		lock (_lock)
		{
			if (_writeQueue.Count == 0)
			{
				_hasPendingWrites = false;
				return;
			}

			// 取出当前队列中所有待写入条目
			logsToWrite = [.. _writeQueue];
			_writeQueue.Clear();
			_hasPendingWrites = false;
		}

		// 在锁外执行文件 IO（避免长时间占用锁）
		if (logsToWrite != null && logsToWrite.Count > 0)
		{
			try
			{
				// 追加写入文件（UTF-8 无 BOM，兼容 Unity）
				File.AppendAllLines(OutputLogFilePath, logsToWrite, Encoding.UTF8);
			}
			catch (Exception ex)
			{
				// 写文件失败至少打给 BepInEx 控制台
				Logger?.LogError($"Fail to flush logs to file: {ex}");
			}
		}
	}

	public void ClearLogText()
	{
		lock (_lock)
		{
			_memoryLogs.Clear();
			_cachedLogText = "";
			_writeQueue.Clear();
			_hasPendingWrites = false;
		}

		try
		{
			File.WriteAllText(OutputLogFilePath, $"# Output Log File - cleared at {DateTime.Now:yyyy-MM-dd HH:mm:ss}{Environment.NewLine}", Encoding.UTF8);
		}
		catch (Exception ex)
		{
			Logger?.LogError($"Fail to clear log file: {ex}");
		}
		BaseLogMessage("Clear text in output log file");
	}

	public void ForceSaveAll()
	{
		if (_disposed) return;
		// 取消定时器，一次性刷完所有
		_flushTimer?.Change(Timeout.Infinite, Timeout.Infinite);
		Flush();
		BaseLogMessage("All text are saved forcely");
	}

	#region BaseLog
	public void BaseLogDevelopment<T>(T Message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
	{
		BaseLog(Message, LogSeverity.Development, memberName, filePath, lineNumber);
	}
	public void BaseLogDebug<T>(T Message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
	{
		BaseLog(Message, LogSeverity.Debug, memberName, filePath, lineNumber);
	}
	public void BaseLogInfo<T>(T Message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
	{
		BaseLog(Message, LogSeverity.Info, memberName, filePath, lineNumber);
	}
	public void BaseLogMessage<T>(T Message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
	{
		BaseLog(Message, LogSeverity.Message, memberName, filePath, lineNumber);
	}
	public void BaseLogWarning<T>(T Message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
	{
		BaseLog(Message, LogSeverity.Warning, memberName, filePath, lineNumber);
	}
	public void BaseLogError<T>(T Message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
	{
		BaseLog(Message, LogSeverity.Error, memberName, filePath, lineNumber);
	}
	public void BaseLogException<T>(T Message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
	{
		BaseLog(Message, LogSeverity.Exception, memberName, filePath, lineNumber);
	}
	public void BaseLogFatal<T>(T Message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
	{
		BaseLog(Message, LogSeverity.Fatal, memberName, filePath, lineNumber);
	}
	#endregion

	public enum LogSeverity
	{
		Development, // 0
		Debug,       // 1
		Info,        // 2
		Message,     // 3
		Warning,     // 4
		Error,       // 5
		Exception,   // 6
		Fatal,       // 7
	}

	public virtual void BaseLog<T>(T Message, LogSeverity severity, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
	{
		EnsureInitialized();

		if (!ShouldLog(severity)) return;

		if (isInitialized && Logger != null)
		{
			string Msg = (Message is string s) ? Translate(s) : Message?.ToString() ?? "null";
			string className = Path.GetFileNameWithoutExtension(filePath);
			bool logPath = false;
			string typeTag;

			switch (severity)
			{
				case LogSeverity.Development:
					if (!isDevMod) return;
					Logger.LogDebug(Msg);
					typeTag = "[DEVELOPMENT]";
					break;
				case LogSeverity.Debug:
					Logger.LogDebug(Msg);
					typeTag = "[DEBUG]";
					break;
				case LogSeverity.Info:
					Logger.LogInfo(Msg);
					typeTag = "[INFO]";
					break;
				case LogSeverity.Message:
					Logger.LogMessage(Msg);
					typeTag = "[MESSAGE]";
					break;
				case LogSeverity.Warning:
					Logger.LogWarning(Msg);
					typeTag = "[WARN]";
					break;
				case LogSeverity.Error:
					Logger.LogError($"[{className}.{memberName}:{lineNumber}] {Msg}");
					typeTag = "[ERROR]";
					logPath = true;
					break;
				case LogSeverity.Fatal:
					Logger.LogFatal($"[{className}.{memberName}:{lineNumber}] {Msg}");
					typeTag = "[FATAL]";
					logPath = true;
					break;
				case LogSeverity.Exception:
					if (Message is Exception ex)
					{
						string msg = Translate(
							$"An error has occurred from {ex.Source} at: {ex}: {ex.Message}\n" +
							$"-----Stack Trace-----\n{ex.StackTrace}\n" +
							$"-----Target Site-----\n{ex.TargetSite}\n" +
							$"-----Inner Exception-----\n{ex.InnerException}");
						Logger.LogError($"[{className}.{memberName}:{lineNumber}] {msg}");
					}
					else
					{
						Logger.LogError($"[{className}.{memberName}:{lineNumber}] {Msg}");
					}
					typeTag = "[EXCEPTION]";
					logPath = true;
					break;
				default:
					Logger.LogInfo(Msg);
					typeTag = "[INFO]";
					break;
			}

			string fileLog = $"{typeTag} [{DateTime.Now:HH:mm:ss}] " +
							 $"[{className}.{memberName}:{lineNumber}] {Msg}" +
							 (logPath ? $" ({filePath})" : "");

			AppendLogText(fileLog);
		}
	}

	protected bool ShouldLog(LogSeverity severity)
	{
		if (!EnableLog) return false;

		return severity >= CurrentSeverity;
	}

	public string Translate(string text)
	{
		try
		{
			string TranslateText = Custom.rainWorld?.inGameTranslator?.Translate(text) ?? text;

			return (string.IsNullOrEmpty(TranslateText) || TranslateText == "!NO TRANSLATION!") ? text : TranslateText;
		}
		catch (Exception ex)
		{
			Logger?.LogWarning($"Translate error: {ex}");
		}
		return text;
	}

	public void Dispose()
	{
		if (_disposed) return;
		_disposed = true;

		ForceSaveAll();
		_flushTimer?.Dispose();
		_flushTimer = null;
		Logger?.LogMessage($"{ModName} logger disposed.");
	}

	~CustomLogger()
	{
		Dispose();
	}

}
