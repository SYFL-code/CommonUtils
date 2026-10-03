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
	#region PhysicalObject
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
	#endregion

	#region Food
	// 玩家胃里的食物格数
	public static float? PlayerStomachFood(Player player)
	{
		if (player == null || player.playerState == null) return null;
		int FoodInt = 0;
		FoodInt = player.FoodInStomach;
		float FoodFloat = player.playerState.quarterFoodPoints * 0.25f;
		return FoodInt + FoodFloat;
		/*if (player.FoodInStomach == player.playerState.foodInStomach)
		{
			FoodInt = player.FoodInStomach;
		}*/
	}

	// 从玩家胃里扣除指定数量的 ¼ 格食物
	public static bool SubtractQuarterFood(Player player, int quartersToRemove)
	{
		if (quartersToRemove <= 0) return false;
		if (player == null || player.room == null) return false;

		int Deduct = 0;

		var hud = player.room.game.cameras[0]?.hud;
		var meter = hud?.foodMeter;
		var sound = SoundID.HUD_Food_Meter_Deplete_Plop_A;

		for (int i = 0; i < quartersToRemove; i++)
		{
			if (player.playerState.quarterFoodPoints > 0)
			{
				// 扣 ¼ 格
				player.playerState.quarterFoodPoints--;
			}
			else if (player.FoodInStomach > 0)
			{
				// 整格扣除
				player.SubtractFood(1);
				player.playerState.quarterFoodPoints = 3;
			}
			else
			{
				break;
			}

			// 每扣一次都刷新 UI
			Deduct += 1;
			hud?.PlaySound(sound);
			meter?.Update();
			meter?.quarterPipShower?.Reset();
		}
		return Deduct == quartersToRemove;
	}
	#endregion

	#region Find
	// 寻找当前房间的生物
	public static List<Creature> FindCreature(Room room,
		List<Creature>? exclude = null, List<Type>? excludeTypes = null, bool includeDead = false,
		Func<Creature, bool>? includeFunc = null, Action<List<Creature>, Creature>? addAction = null)
	{
		List<Creature> creatures = [];
		excludeTypes ??= [typeof(Player), typeof(Fly)];

		if (room == null || room.abstractRoom == null || room.abstractRoom.creatures == null || room.abstractRoom.creatures.Count <= 0)
		{
			return creatures;
		}

		// 遍历当前房间所有生物
		foreach (AbstractCreature abstractCreature in room.abstractRoom.creatures)
		{
			Creature c = abstractCreature.realizedCreature;

			if (c == null ||
				c.mainBodyChunk == null)
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
				if (excludeTypes.Any(t => t.IsAssignableFrom(c.GetType())))
				{
					continue;
				}
			}
			if (!includeDead && c.dead)// 死亡的生物
			{
				continue;
			}
			if (includeFunc != null && !includeFunc(c))
			{
				continue;
			}


			if (addAction != null)
			{
				addAction(creatures, c);
			}
			else
			{
				creatures.Add(c);
			}
		}
		return creatures;
	}

	// 寻找当前房间中距离自身最近的生物
	public static Creature? FindNearestCreature(Vector2 centerPos, Room room,
		List<Creature>? exclude = null, List<Type>? excludeTypes = null, bool includeDead = false)
	{
		Creature? nearest = null;        // 最近生物对象
		float minSqrDistance = float.MaxValue;  // 最小平方距离


		Action<List<Creature>, Creature> addAction = (creatures, c) =>
		{
			// 计算位置差（目标位置 - 自身位置）
			Vector2 offset = c.mainBodyChunk.pos - centerPos;
			float sqrDistance = offset.sqrMagnitude;

			// 检查是否为更近的生物
			if (sqrDistance < minSqrDistance)
			{
				minSqrDistance = sqrDistance;
				nearest = c;
			}
		};
		FindCreature(room, exclude, excludeTypes, includeDead, null, addAction);

		return nearest;
	}

	public static List<Creature> FindCreaturesInCone(Vector2 sourcePos, Vector2 direction, Room room, float halfAngleDeg, float maxRadius,
		List<Creature>? exclude = null, List<Type>? excludeTypes = null, bool includeDead = false)
	{
		Func<Creature, bool> includeFunc = (c) =>
		{
			Vector2 toTarget = c.mainBodyChunk.pos - sourcePos;
			float dist = toTarget.magnitude;
			//float dist = Vector2.Distance(sourcePos, c.mainBodyChunk.pos);
			if (dist > maxRadius) return false;
			if (dist < 0.001f) return true;

			// 角度检测
			Vector2 dirToTarget = toTarget / dist;
			float angle = Vector2.Angle(direction, dirToTarget);
			if (angle > halfAngleDeg) return false;

			// 视线检测（地形阻挡）
			if (!IsLineOfSight(room, sourcePos, c.mainBodyChunk.pos))
				return false;

			return true;
		};
		return FindCreature(room, exclude, excludeTypes, includeDead, includeFunc, null);
	}

	public static void SortDistance(this List<Creature> creatures, Vector2 centerPos)
	{
		if (creatures == null || creatures.Count == 0)
			return;
		creatures.Sort((a, b) =>
		{
			float da = (a.mainBodyChunk.pos - centerPos).sqrMagnitude;
			float db = (b.mainBodyChunk.pos - centerPos).sqrMagnitude;
			return da.CompareTo(db);
		});
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
	#endregion

	#region GetRoomWaterColor
	public static Color GetRoomWaterColor(AbstractRoom abstractRoom)
	{
		if (abstractRoom == null || abstractRoom.world == null)
		{
			return Color.white;
		}

		try
		{
			RoomSettings? settings = null;
			if (abstractRoom.realizedRoom != null)
			{
				settings = abstractRoom.realizedRoom.roomSettings;
			}
			if (settings == null)
			{
				settings = new RoomSettings(null, WorldLoader.RoomNameManipulator(abstractRoom.FileName, abstractRoom.world.game), abstractRoom.world.region,
					template: false, firstTemplate: false, abstractRoom.world.game?.TimelinePoint, abstractRoom.world.game);
			}
			if (settings == null)
			{
				return Color.white;
			}
			Texture2D paletteTex = LoadRoomPalette(settings.Palette);
			if (paletteTex == null)
			{
				return Color.white;
			}
			Color waterColor = Color.Lerp(paletteTex.GetPixel(4, 15), paletteTex.GetPixel(4, 7), 0.5f);
			return waterColor;
		}
		catch
		{
			return Color.white;
		}
	}

	private static Texture2D LoadRoomPalette(int paletteNumber)
	{
		Texture2D texture = new Texture2D(32, 16, TextureFormat.ARGB32, mipChain: false);

		string path = AssetManager.ResolveFilePath(
			"palettes" + Path.DirectorySeparatorChar +
			"palette" + paletteNumber.ToString(CultureInfo.InvariantCulture) + ".png"
		);

		try
		{
			AssetManager.SafeWWWLoadTexture(ref texture, "file:///" + path, clampWrapMode: false, crispPixels: true);
		}
		catch
		{
			path = AssetManager.ResolveFilePath("palettes" + Path.DirectorySeparatorChar + "palette-1.png");
			AssetManager.SafeWWWLoadTexture(ref texture, "file:///" + path, clampWrapMode: false, crispPixels: true);
		}

		texture.Apply(updateMipmaps: false);
		return texture;
	}
	#endregion

	#region 文件
	private static string? _cachedModRoot;
	public static string ModRoot
	{
		get
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
	}
	public static string GetStringsPath(string? language = null)
	{
		string langDir = Path.Combine(ModRoot, "text", "text_" + (language ?? lang));
		string path = Path.Combine(langDir, "strings.txt");

		Directory.CreateDirectory(langDir);

		if (!File.Exists(path))
		{
			using (File.Create(path)) { } // 立即释放句柄
		}
		return path;
	}
	public static string GameRoot
	{
		get
		{
			return AppDomain.CurrentDomain.BaseDirectory;
		}
	}
	#endregion

	#region Translate
	public static RainWorld RainWorld => Custom.rainWorld;
	public static InGameTranslator inGameTranslator => RainWorld.inGameTranslator;
	public static InGameTranslator Translator => inGameTranslator;
	public static InGameTranslator Trans => inGameTranslator;
	public static string lang => LocalizationTranslator.LangShort(inGameTranslator.currentLanguage);

	extension(string originalName)
	{
		public string Translate => Translator.Translate(originalName);
	}
	extension(IEnumerable<string> originalNames)
	{
		public string[] Translate
		{
			get
			{
				List<string> items = [];
				foreach (string name in originalNames)
				{
					items.Add(Translator.Translate(name));
				}
				return items.ToArray();
			}
		}
		public ListItem[] ToListItem
		{
			get
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
		}
	}
	#endregion
}
