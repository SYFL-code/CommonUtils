#region using
using BepInEx;
using CommonUtils;
using CoralBrain;
using Expedition;
using Fisobs.Core;
using HUD;
using ImprovedInput;
using JollyCoop;
using JollyCoop.JollyMenu;
using Menu;
using Menu.Remix.MixedUI;
using MonoMod.RuntimeDetour;
using MoreSlugcats;
using Newtonsoft.Json;
using Noise;
using RWCustom;
using SlugBase;
using SlugBase.DataTypes;
using SlugBase.Features;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Watcher;
using static Player.ObjectGrabability;
using static SlugBase.Features.FeatureTypes;
#endregion

namespace CommonUtils.Core;

public static class StringHelper
{
	// 取左 N 个字符
	public static string Left(this string str, int length)
	{
		if (string.IsNullOrEmpty(str)) return str;
		if (length <= 0) return string.Empty;
		return str.Length <= length ? str : str.Substring(0, length);
	}
	// 取右 N 个字符
	public static string Right(this string str, int length)
	{
		if (string.IsNullOrEmpty(str)) return str;
		if (length <= 0) return string.Empty;
		return str.Length <= length ? str : str.Substring(str.Length - length, length);
	}

	// 替换行尾符
	public static string ReplaceLineEndings(this string s, string lineEndings = "\r\n")
	{
		if (string.IsNullOrEmpty(s)) return s;
		return s.Replace("\r\n", "\n")
				.Replace("\r", "\n")
				.Replace("\n", lineEndings);
	}

	// 截断
	public static string Truncate(this string str, int maxLength, string ellipsis = "...")
	{
		if (string.IsNullOrEmpty(str) || str.Length <= maxLength) return str;
		return str.Substring(0, maxLength) + ellipsis;
	}

	// 首字母大写
	public static string Capitalize(this string str)
	{
		if (string.IsNullOrEmpty(str)) return str;
		return char.ToUpper(str[0]) + str.Substring(1);
	}

	// 去 BOM
	public static string TrimBOM(this string str)
	{
		if (!string.IsNullOrEmpty(str) && str[0] == '\uFEFF')
			return str.Substring(1);
		return str;
	}

	// 统一为正斜杠（跨平台）
	public static string ToForwardSlash(this string path) => path.Replace('\\', '/');
	// 统一为反斜杠（Windows 显示）
	public static string ToBackSlash(this string path) => path.Replace('/', '\\');

	// 不为 null 或空（Length == 0）
	public static bool IsNotNullOrEmpty(this string? str) => !string.IsNullOrEmpty(str);
	// 不为 null 或空（Length == 0） 或空白（只包含空格、制表符等）
	public static bool IsNotNullOrWhiteSpace(this string? str) => !string.IsNullOrWhiteSpace(str);
}