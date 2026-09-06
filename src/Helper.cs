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

	/// <summary> 寻找当前房间中距离自身最近的生物 </summary>
	public static Creature? FindNearestCreature(Vector2 centerPos, Room room,
		List<Creature>? exclude = null, List<Type>? excludeTypes = null, bool excludeDead = true)
	{
		Creature? nearest = null;        // 最近生物对象
		float minSqrDistance = float.MaxValue;  // 最小平方距离
		excludeTypes ??= [typeof(Player), typeof(Fly)];

		if (room == null || room.abstractRoom == null || room.abstractRoom.creatures == null || room.abstractRoom.creatures.Count <= 0)
		{
			return null;
		}

		// 遍历当前房间所有生物
		foreach (AbstractCreature abstractCreature in room.abstractRoom.creatures)
		{
			Creature c = abstractCreature.realizedCreature;

			if (c == null ||
				c.mainBodyChunk == null ||
				c.mainBodyChunk.pos == null)
			{
				continue;
			}
			if (exclude != null)
			{
				if (exclude.Contains(c))
				{
					continue;
				}
			}
			if (excludeTypes.Count > 0)
			{
				if (excludeTypes.Contains(c.GetType()))
				{
					continue;
				}
			}
			if (excludeDead)// 死亡的生物
			{
				if (c.dead)
				{
					continue;
				}
			}

			// 计算位置差（目标位置 - 自身位置）
			Vector2 offset = c.mainBodyChunk.pos - centerPos;
			float sqrDistance = offset.sqrMagnitude;

			// 检查是否为更近的生物
			if (sqrDistance < minSqrDistance)
			{
				minSqrDistance = sqrDistance;
				nearest = c;
			}
		}
		return nearest;
	}

	public static List<Creature> FindCreaturesInCone(Vector2 sourcePos, Vector2 direction, Room room, float halfAngleDeg, float maxRadius,
		List<Creature>? exclude = null, List<Type>? excludeTypes = null, bool excludeDead = true)
	{
		List<Creature> candidates = [];
		if (room == null || room.abstractRoom == null) return candidates;
		excludeTypes ??= [typeof(Player), typeof(Fly)];

		float halfAngleRad = halfAngleDeg * Mathf.Deg2Rad;

		foreach (AbstractCreature absCreature in room.abstractRoom.creatures)
		{
			Creature c = absCreature.realizedCreature;

			if (c == null) continue;
			if (c.mainBodyChunk == null) continue;
			if (c.dead && excludeDead) continue;
			if (excludeTypes.Contains(c.GetType())) continue;
			if (exclude != null && exclude.Contains(c)) continue;

			Vector2 toTarget = c.mainBodyChunk.pos - sourcePos;
			float dist = toTarget.magnitude;
            //float dist = Vector2.Distance(sourcePos, c.mainBodyChunk.pos);
            if (dist > maxRadius) continue;

			// 角度检测
			Vector2 dirToTarget = toTarget / dist;
			float angle = Vector2.Angle(direction, dirToTarget);
			if (angle > halfAngleRad * Mathf.Rad2Deg) continue;

			// 视线检测（地形阻挡）
			if (!IsLineOfSight(room, sourcePos, c.mainBodyChunk.pos))
				continue;

			candidates.Add(c);
		}

		return candidates;
	}

	// 检测两点之间是否有地形阻挡
	public static bool IsLineOfSight(Room room, Vector2 start, Vector2 end)
	{
		bool isTerrain;
		Vector2 adjustedEnd = Trace(start, end, room, out isTerrain);
		// 如果有地形阻挡，返回 false
		return !isTerrain;
	}
	// 检测路径是否碰撞地形
	public static Vector2 Trace(Vector2 start, Vector2 end, Room room, out bool isTerrain)
	{
		Vector2 Direction = Custom.DegToVec(Custom.AimFromOneVectorToAnother(start, end));

		// 检测起点到终点之间是否碰撞地形
		// 返回值intVector为碰撞的格子坐标（若无碰撞则返回null）
		IntVector2? intVector = SharedPhysics.RayTraceTilesForTerrainReturnFirstSolid(room, start, end);

		if (intVector != null)
		{
			// 标记碰撞到地形
			isTerrain = true;

			// 计算修正后的终点位置
			// 方案：取碰撞格子的中心坐标，并向反方向微调7单位
			return room.MiddleOfTile(intVector.Value) - (Direction * 7f);
		}

		// 无碰撞时保持原始终点
		isTerrain = false;
		return start;
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
	//public static string? currentLang;
	//private static Dictionary<string, string> _dict = [];
	//private static Dictionary<string, string> Dict
	//{
	//    get
	//    {
	//        if (currentLang != LocalizationTranslator.LangShort(Translator.currentLanguage))
	//        {
	//            currentLang = LocalizationTranslator.LangShort(Translator.currentLanguage);

	//            string path = MyOptions.GetTranslatorPath();
	//            if (File.Exists(path))
	//            {
	//                _dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(path)) ?? [];
	//            }
	//            else
	//            {
	//                Log.LogError("找不到语言文件: " + currentLang);

	//                path = MyOptions.GetTranslatorPath("eng");
	//                if (File.Exists(path))
	//                {
	//                    _dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(path)) ?? [];
	//                }
	//                else
	//                {
	//                    Log.LogError("找不到默认语言文件: eng");
	//                    _dict = [];
	//                }
	//            }
	//            return _dict;
	//        }
	//        else
	//        {
	//            return _dict;
	//        }
	//    }
	//}
	//public static string T(string key)
	//{
	//    return Dict.TryGetValue(key, out var val) ? val : key;
	//}
	//public static string T(string key, object arg0) => string.Format(T(key), arg0);
	//public static string T(string key, object arg0, object arg1) => string.Format(T(key), arg0, arg1);
	//public static string T(string key, params object[] args) => string.Format(T(key), args);
	#endregion

	#region String
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
	#endregion
}
