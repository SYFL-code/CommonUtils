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
using Translator;
using UnityEngine;
using UnityEngine.UI;
using Watcher;
using static Player.ObjectGrabability;
using static SlugBase.Features.FeatureTypes;
#endregion

public static class Helper
{

	public static void SetObjectPosition(PhysicalObject obj, Vector2 newPos)
	{
		if (obj is not Creature)
		{
			ReleaseAllGrasps(obj);
		}
		if (obj is Player player)
		{
			if (player.tongue != null)
			{
				player.tongue.resetRopeLength();
				player.tongue.mode = Player.Tongue.Mode.Retracted;
				player.tongue.rope.Reset();
			}
			for (int num12 = 0; num12 < 2; num12++)
			{
				//player.bodyChunks[num12].vel = Custom.DegToVec(UnityEngine.Random.value * 360f) * 12f;
				player.bodyChunks[num12].pos = newPos;
				player.bodyChunks[num12].lastPos = newPos;
			}
			return;
		}
		int num = 0;
		for (; ; )
		{
			int num2 = num;
			int? num3;
			if (obj == null)
			{
				num3 = null;
			}
			else
			{
				BodyChunk[] bodyChunks = obj.bodyChunks;
				num3 = (bodyChunks != null) ? new int?(bodyChunks.Length) : null;
			}
			int? num4 = num3;
			if (!(num2 < num4.GetValueOrDefault() & num4 != null))
			{
				break;
			}
			if (obj != null && obj.bodyChunks[num] != null)
			{
				obj.bodyChunks[num].pos = newPos;
				obj.bodyChunks[num].lastPos = newPos;
				obj.bodyChunks[num].lastLastPos = newPos;
				obj.bodyChunks[num].vel = default(Vector2);
				if (obj is PlayerCarryableItem playerCarryableItem)
				{
					playerCarryableItem.lastOutsideTerrainPos = null;
				}
			}
			num++;
		}
	}

	public static void ReleaseAllGrasps(PhysicalObject obj)
	{
		if (((obj != null) ? obj.grabbedBy : null) != null && obj != null)
		{
			for (int i = obj.grabbedBy.Count - 1; i >= 0; i--)
			{
				Creature.Grasp grasp = obj.grabbedBy[i];
				if (grasp != null)
				{
					grasp.Release();
				}
			}
		}
		if (obj is Creature creature)
		{
			if (obj is Player player)
			{
				Player.SlugOnBack slugOnBack = player.slugOnBack;
				if (slugOnBack != null)
				{
					slugOnBack.DropSlug();
				}
				Player onBack = player.onBack;
				if (onBack != null)
				{
					Player.SlugOnBack slugOnBack2 = onBack.slugOnBack;
					if (slugOnBack2 != null)
					{
						slugOnBack2.DropSlug();
					}
				}
				player.slugOnBack = null;
				player.onBack = null;
				Player.SpearOnBack spearOnBack = player.spearOnBack;
				if (spearOnBack != null)
				{
					spearOnBack.DropSpear();
				}
			}
			creature.LoseAllGrasps();
		}
	}

	public static void SuperHardSetPosition(Player player, Vector2 pos)
	{
		Vector2 firstChunkOldPos = player.firstChunk.pos;
		List<Vector2> offset = [];

		PlayerGraphics? playerGraphics = player.graphicsModule as PlayerGraphics;

		Vector2 firstDrawPositions = new();
		Vector2[,] drawOffset = new Vector2[10, 4];
		if (playerGraphics != null)
		{
			firstDrawPositions = playerGraphics.drawPositions[0, 0];
			drawOffset = playerGraphics.drawPositions;
		}


		for (int i = 0; i < player.bodyChunks.Length; i++)
		{
			offset.Add(player.bodyChunks[i].pos - firstChunkOldPos);

			if (playerGraphics != null)
			{
				for (int j = 0; j < 2; j++)
				{
					drawOffset[i, j] = playerGraphics.drawPositions[i, j] - firstDrawPositions;
				}
			}
		}

		for (int i = 0; i < player.bodyChunks.Length; i++)
		{
			player.bodyChunks[i].HardSetPosition(pos + offset[i]);
			//player.bodyChunks[i].HardSetPosition(pos);
			if (playerGraphics != null)
			{
				for (int j = 0; j < 2; j++)
				{
					playerGraphics.drawPositions[i, j] = pos + drawOffset[i, j];
				}
			}
		}
		player.bodyChunks[1].pos.x = player.bodyChunks[0].pos.x - 1f;
		if (playerGraphics != null)
		{
			if (playerGraphics.bodyParts.Length > 0)
			{
				Vector2 firstBodyPartsPos = playerGraphics.bodyParts[0].pos;
				for (int i = 0; i < playerGraphics.bodyParts.Length; i++)
				{
					BodyPart bodyPart = playerGraphics.bodyParts[i];
					Vector2 bodyPartOffset = bodyPart.pos - firstChunkOldPos;
					bodyPart.pos = pos + bodyPartOffset;
					bodyPart.lastPos = pos + bodyPartOffset;
				}
			}
		}
		if (player.tongue != null)
		{
			if (player.tongue.Attached)
			{
				player.tongue.Release();
			}
			player.tongue.pos = player.mainBodyChunk.pos;
			player.tongue.lastPos = player.mainBodyChunk.lastPos;
			player.tongue.rope.Reset(pos);
			if (playerGraphics != null)
			{
				foreach (PlayerGraphics.RopeSegment ropeSegment in playerGraphics.ropeSegments)
				{
					ropeSegment.pos = pos;
					ropeSegment.lastPos = pos;
				}
			}
		}
		foreach (Creature.Grasp grasp in player.grasps)
		{
			if (grasp?.grabbed?.bodyChunks != null)
			{
				BodyChunk[] bodyChunks = grasp.grabbed.bodyChunks;
				for (int l = 0; l < bodyChunks.Length; l++)
				{
					bodyChunks[l].HardSetPosition(pos);
				}
				GraphicsModule graphicsModule = grasp.grabbed.graphicsModule;
				if (graphicsModule != null)
				{
					graphicsModule.Reset();
				}
			}
		}
	}



	#region 文件
	private static string? _cachedModRoot;
	public static string GetModRootPath()
	{
		if (_cachedModRoot != null) return _cachedModRoot;

		// 获取当前 DLL 所在目录
		string? dllDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

		// 从 DLL 目录逐级向上查找 modinfo.json
		string? dir = dllDir;
		while (dir != null)
		{
			if (File.Exists(Path.Combine(dir, "modinfo.json")))
			{
				_cachedModRoot = dir;
				Log.LogInfo($"模组根目录已缓存: {dir}");
				return dir;
			}
			dir = Path.GetDirectoryName(dir);
		}

		Log.LogError($"无法找到 modinfo.json, GUID: {Plugin.GUID}, DLL位置: {dllDir}");
		throw new FileNotFoundException($"无法找到 modinfo.json, GUID: {Plugin.GUID}, DLL位置: {dllDir}");
	}
	public static string GetStringsPath(string? language = null)
	{
		string langDir = Path.Combine(GetModRootPath(), "text", "text_" + (language ?? lang));
		string path = Path.Combine(langDir, "strings.txt");

		Directory.CreateDirectory(langDir);

		if (!File.Exists(path))
		{
			using (File.Create(path)) { } // 立即释放句柄
		}
		return path;
	}
	public static string GetGameRoot()
	{
		return AppDomain.CurrentDomain.BaseDirectory;
	}
	#endregion

	#region Translate
	public static RainWorld RainWorld => Custom.rainWorld;
	public static InGameTranslator inGameTranslator => RainWorld.inGameTranslator;
	public static InGameTranslator Translator => inGameTranslator;
	public static InGameTranslator Trans => inGameTranslator;
	public static string lang => LocalizationTranslator.LangShort(Trans.currentLanguage);

	public static string Tra(this string originalName)
	{
		return Translator.Translate(originalName);
	}
	public static string Translate(this string originalName)
	{
		return Translator.Translate(originalName);
	}
	public static string[] Translate(this IEnumerable<string> originalNames)
	{
		List<string> items = [];
		foreach (string name in originalNames)
		{
			items.Add(Translator.Translate(name));
		}
		return items.ToArray();
	}
	public static ListItem[] ToListItem(this IEnumerable<string> originalNames)
	{
		List<ListItem> items = [];

		int i = 0;
		foreach (string name in originalNames)
		{
			// name 作为实际值，displayName 使用翻译后的文本
			items.Add(new ListItem(name, Translator.Translate(name), i));
			i += 1;
		}
		return items.ToArray();
	}
    #endregion

    #region UITranslate
    public static string? currentLang;
    private static Dictionary<string, string> _dict = [];
    private static Dictionary<string, string> Dict
    {
        get
        {
            if (currentLang != LocalizationTranslator.LangShort(Translator.currentLanguage))
            {
                currentLang = LocalizationTranslator.LangShort(Translator.currentLanguage);

                string path = MyOptions.GetTranslatorPath();
                if (File.Exists(path))
                {
                    _dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(path)) ?? [];
                }
                else
                {
                    Log.LogError("找不到语言文件: " + currentLang);

                    path = MyOptions.GetTranslatorPath("eng");
                    if (File.Exists(path))
                    {
                        _dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(path)) ?? [];
                    }
                    else
                    {
                        Log.LogError("找不到默认语言文件: eng");
                        _dict = [];
                    }
                }
                return _dict;
            }
            else
            {
                return _dict;
            }
        }
    }
    public static string T(string key)
    {
        return Dict.TryGetValue(key, out var val) ? val : key;
    }
    public static string T(string key, object arg0) => string.Format(T(key), arg0);
    public static string T(string key, object arg0, object arg1) => string.Format(T(key), arg0, arg1);
    public static string T(string key, params object[] args) => string.Format(T(key), args);
    #endregion

    #region String
    public static string ReplaceLineEndings(this string s, string lineEndings = "\r\n")
	{
		return s.Replace("\r\n", "\n")
				.Replace("\r", "\n")
				.Replace("\n", lineEndings);
	}
	#endregion
}
