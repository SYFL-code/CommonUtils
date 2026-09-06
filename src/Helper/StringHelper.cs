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
	public static string Left(this string str, int length)
	{
		if (string.IsNullOrEmpty(str)) return str;
		return str.Length <= length ? str : str.Substring(0, length);
	}

	public static string ReplaceLineEndings(this string s, string lineEndings = "\r\n")
	{
		return s.Replace("\r\n", "\n")
				.Replace("\r", "\n")
				.Replace("\n", lineEndings);
	}
}