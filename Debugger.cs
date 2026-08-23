#region using
using BepInEx;
using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

// <DefineConstants>ENDERPEARL</DefineConstants>
#if ENDERPEARL
using EnderPearl;
#elif EXTENSIONLIB
using ExtensionLib;
#elif TRANSLATOR
using Translator;
#else
//
#endif

#endregion

namespace CommonUtils;

public static class Debugger
{
    private enum ArrayType { None, Float, Bool, Vector }
	private enum InputState { None, SelectingIndex, EnteringValue }

	private static ArrayType _selectedType = ArrayType.None;
	private static int _selectedIndex = -1;
	private static InputState _state = InputState.None;

	// 公共访问方法
	public static float GetFloat(int index, float defaultValue = default, string name = "")
	{
		_floats[index].Item2 = name;
		return _floats[index].Item1 ??= defaultValue;
	}
	public static bool GetBool(int index, bool defaultValue = default, string name = "")
	{
		_bools[index].Item2 = name;
		return _bools[index].Item1 ??= defaultValue;
	}
	public static Vector2 GetVector(int index, Vector2 defaultValue = default, string name = "")
	{
		_Vectors[index].Item2 = name;
		return _Vectors[index].Item1 ??= defaultValue;
	}

	public static void SetFloat(int index, float value, string name = "")
		=> _floats[index] = (value, name);
	public static void SetBool(int index, bool value, string name = "")
		=> _bools[index] = (value, name);
	public static void SetVector(int index, Vector2 value, string name = "")
		=> _Vectors[index] = (value, name);

	// 数据存储
	private static (float?, string)[] _floats = new (float?, string)[10];
	private static (bool?, string)[] _bools = new (bool?, string)[10];
	private static (Vector2?, string)[] _Vectors = new (Vector2?, string)[10];

	// 输入缓存
	private static string _inputBuffer = "";

	#region 标记
	// 用 object 作为 Value，存一个静态占位符即可
	private static readonly ConditionalWeakTable<object, object> _markedInstances = new();
	private static readonly object MarkerValue = new object();

	// 标记某个实例
	public static object Mark(object instance)
	{
		// 如果 instance 已存在，直接返回旧值；不存在则调用回调创建新值
		_markedInstances.GetValue(instance, _ => MarkerValue);
		return MarkerValue;
	}

	// 检查某个实例是否被标记
	public static bool IsMarked(object instance)
	{
		return _markedInstances.TryGetValue(instance, out _);
	}

	//public static bool IsMarked(object instance, object type, RainWorldGame? game = null)
	//{
	//	bool isMarked = false;

	//	if (IsMarked(instance))
	//	{
	//		isMarked = true;
	//	}

	//	if (type is bool forceMark)
	//	{
	//		if (forceMark)
	//			Mark(instance);
	//		else
	//			_markedInstances.Remove(instance);

	//		return forceMark;
	//	}
	//	else if (type is Vector2 pos)
	//	{
	//		if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
	//		{
	//			Vector2 mouse = new Vector2(Futile.mousePosition.x, Futile.mousePosition.y) + (game?.cameras[0].pos ?? Vector2.zero);
	//			float distance = Vector2.Distance(pos, mouse);

	//			Log.LogInfo($"distance: {distance}");
	//			if (distance < 6f)
	//			{
	//				if (Input.GetMouseButton(0))
	//				{
	//					Log.LogInfo($"鼠标左键按住中");

	//					Mark(instance);
	//					isMarked = true;
	//				}
	//				if (Input.GetMouseButton(1))
	//				{
	//					Log.LogInfo($"鼠标右键按住中");

	//					_markedInstances.Remove(instance);
	//					isMarked = false;
	//				}
	//			}
	//		}
	//	}
	//	return isMarked;
	//}
	public static bool IsMarked(object instance, object type)
	{
		if (type is bool forceMark)
		{
			if (forceMark)
				Mark(instance);
			else
				_markedInstances.Remove(instance);

			return forceMark;
		}

		//if (type is Vector2 worldPos)
		//{
		//	RainWorldGame? game = GlobalVar.game;

		//	Vector2 camPos = (game?.cameras != null && game.cameras.Length > 0)
		//		? game.cameras[0].pos
		//		: Vector2.zero;

		//	Vector2 mouseWorld = new Vector2(Futile.mousePosition.x, Futile.mousePosition.y) + camPos;
		//	float distance = Vector2.Distance(worldPos, mouseWorld);

		//	if (GetBool(9, false, "Debug Distance"))
		//	{
		//		Log.LogDebug($"distance:{distance}");
		//	}
		//	if (distance < GetFloat(9, 6f, "Debug Distance Threshold"))
		//	{
		//		bool leftBtn = Input.GetMouseButton(0);
		//		bool rightBtn = Input.GetMouseButton(1);

		//		if (leftBtn)
		//		{
		//			Log.LogDebug($"鼠标左键按住中");
		//			Mark(instance);
		//			return true;
		//		}

		//		if (rightBtn)
		//		{
		//			Log.LogDebug($"鼠标右键按住中");
		//			_markedInstances.Remove(instance);
		//			return false;
		//		}
		//	}

		//	// 不在范围内或未点击时，返回当前已存在的标记状态
		//	return IsMarked(instance);
		//}
		return IsMarked(instance);
	}
    #endregion

	public static void SetUpdate()
	{
        Plugin.OnUpdate += Update;
    }
    public static void Update()
	{
		if (Plugin.DebugMode)
		{
			// 1. 选择类型
			if (Input.GetKeyDown(KeyCode.Slash))
			{
				//Log.LogInfo($"ParticleEffectType:{MyConfig.ParticleEffectType}");
				Log.LogInfo($"已安装的模组数量: {ModManager.InstalledMods.Count}");
				// 遍历所有已安装的模组
				foreach (ModManager.Mod mod in ModManager.InstalledMods)
				{
					// 获取原始名称和描述（注意：这里拿的是未经过翻译覆写的原始值）
					string originalName = mod.name;
					string originalDesc = mod.description;
					string id = mod.id;


					// 输出到 BepInEx 的控制台日志
					Log.LogInfo($"ID: {id} | 名称: {originalName} | 简介: {originalDesc}");

					// 或者，你想直接保存到文件：
					File.AppendAllText("模组列表备份.txt", $"{id}|{originalName}|{originalDesc}\n");
				}
			}

			// 1. 选择类型
			if (Input.GetKeyDown(KeyCode.Semicolon))
			{
				_selectedType = ArrayType.Float;
				_state = InputState.SelectingIndex;
				Log.LogInfo("选择: Float 数组");
			}
			else if (Input.GetKeyDown(KeyCode.Quote))
			{
				_selectedType = ArrayType.Bool;
				_state = InputState.SelectingIndex;
				Log.LogInfo("选择: Bool 数组");
			}
			else if (Input.GetKeyDown(KeyCode.Backslash))
			{
				_selectedType = ArrayType.Vector;
				_state = InputState.SelectingIndex;
				Log.LogInfo("选择: Vector2 数组");
			}
			else if (Input.GetKeyDown(KeyCode.RightBracket))
			{
				_selectedType = ArrayType.None;
				_state = InputState.None;
				_selectedIndex = -1;
				Log.LogInfo("取消选择");
				return;
			}
			if (Input.GetKeyDown(KeyCode.LeftBracket))
			{
				PrintAllArrays();
			}

			// 2. 选择索引
			if (_state == InputState.SelectingIndex)
			{
				for (int i = 0; i < 10; i++)
				{
					KeyCode key = KeyCode.Alpha0 + i;
					if (Input.GetKeyDown(key))
					{
						_selectedIndex = i;
						_state = InputState.EnteringValue;
						_inputBuffer = "";
						Log.LogInfo($"选择索引: {i}, 当前值: {GetCurrentValue()}");
						//Log.LogInfo($"");
						return;
					}
				}
			}

			// 3. 输入值
			if (_state == InputState.EnteringValue && _selectedIndex >= 0)
			{
				HandleInput();
			}
		}
	}

	private static string _buildTime = File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location).ToString("mm:ss");
	private static void PrintAllArrays()
	{
		Log.LogInfo($"{_buildTime}========== 调试数组内容 ==========");

		PrintArray("Float", _floats);
		PrintArray("Bool", _bools);
		PrintArray("Vector2", _Vectors);

		Log.LogInfo($"{_buildTime}====================================");
	}

	private static void PrintArray<T>(string type, T[] array) where T : struct
	{
		bool hasValue = false;
		Log.LogInfo($"{type} 数组:");

		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] is ITuple tuple && tuple[0] != null)
			{
				Log.LogInfo($"{type}[{i}] = {array[i]}");

				hasValue = true;
			}
		}
		if (!hasValue)
			Log.LogInfo($"无");
	}


	private static void HandleInput()
	{
		switch (_selectedType)
		{
			case ArrayType.Float:
				HandleFloatInput();
				break;
			case ArrayType.Bool:
				HandleBoolInput();
				break;
			case ArrayType.Vector:
				HandleVectorInput();
				break;
		}
	}

	// 处理通用输入（退格 + 数字）
	private static bool HandleCommonInput(ref string buffer)
	{
		// 退格
		if (Input.GetKeyDown(KeyCode.Backspace) && buffer.Length > 0)
		{
			buffer = buffer.Substring(0, buffer.Length - 1);
			Log.LogInfo($"输入: {buffer}");
			return true;
		}

		// 数字输入
		if (Input.anyKeyDown)
		{
			string input = Input.inputString;

			if (!string.IsNullOrEmpty(input))
			{
				char c = input[0];
				if (char.IsDigit(c) || c == '.' || c == '-')
				{
					buffer += input;
					Log.LogInfo($"输入: {buffer}");
					return true;
				}
			}
		}

		return false;
	}

	private static void HandleFloatInput()
	{
		if (HandleCommonInput(ref _inputBuffer))
			return;

		// 回车确认
		if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
		{
			if (float.TryParse(_inputBuffer, NumberStyles.Any, CultureInfo.InvariantCulture, out float result))
			{
				_floats[_selectedIndex] = (result, _floats[_selectedIndex].Item2);
				Log.LogInfo($"float_{_selectedIndex} = {result}");
				_state = InputState.SelectingIndex;
				_inputBuffer = "";
			}
			else
			{
				Log.LogWarning($"无效数字: {_inputBuffer}");
				_inputBuffer = "";
			}
		}
	}

	private static void HandleBoolInput()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			_bools[_selectedIndex] = (true, _bools[_selectedIndex].Item2);
			Log.LogInfo($"bool_{_selectedIndex} = true");
			_state = InputState.SelectingIndex;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Alpha2))
		{
			_bools[_selectedIndex] = (false, _bools[_selectedIndex].Item2);
			Log.LogInfo($"bool_{_selectedIndex} = false");
			_state = InputState.SelectingIndex;
		}
	}

	private static void HandleVectorInput()
	{
		if (HandleCommonInput(ref _inputBuffer))
			return;

		// 按 , 设置 X
		if (Input.GetKeyDown(KeyCode.Comma))
		{
			if (float.TryParse(_inputBuffer, NumberStyles.Any, CultureInfo.InvariantCulture, out float result))
			{
				Vector2 vec = _Vectors[_selectedIndex].Item1 ?? default;  // 获取值
				vec.x = result;
				_Vectors[_selectedIndex] = (vec, _Vectors[_selectedIndex].Item2);  // 重新赋值

				Log.LogInfo($"Vec_{_selectedIndex}.x = {result}");
				_inputBuffer = "";
				Log.LogInfo("输入 Y 值");
			}
			else
			{
				Log.LogWarning($"无效数字: {_inputBuffer}");
				_inputBuffer = "";
			}
		}

		// 按 Enter 设置 Y
		if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
		{
			if (float.TryParse(_inputBuffer, NumberStyles.Any, CultureInfo.InvariantCulture, out float result))
			{
				Vector2 vec = _Vectors[_selectedIndex].Item1 ?? default;  // 获取值
				vec.y = result;
				_Vectors[_selectedIndex] = (vec, _Vectors[_selectedIndex].Item2);  // 重新赋值
				Log.LogInfo($"Vec_{_selectedIndex}.y = {result}");
				Log.LogInfo($"Vec_{_selectedIndex} = {_Vectors[_selectedIndex]}");
				_state = InputState.SelectingIndex;
				_inputBuffer = "";
			}
			else
			{
				Log.LogWarning($"无效数字: {_inputBuffer}");
				_inputBuffer = "";
			}
		}
	}

	private static string GetCurrentValue()
	{
					if (_selectedIndex < 0 || _selectedIndex >= 10)
				return "索引无效";

		return _selectedType switch
		{
			ArrayType.Float => (_floats[_selectedIndex].Item1 ?? default(float), _floats[_selectedIndex].Item2).ToString(),
			ArrayType.Bool => (_bools[_selectedIndex].Item1 ?? default(bool), _bools[_selectedIndex].Item2).ToString(),
			ArrayType.Vector => (_Vectors[_selectedIndex].Item1 ?? default(Vector2), _Vectors[_selectedIndex].Item2).ToString(),
			_ => "未知"
		};
	}



}
