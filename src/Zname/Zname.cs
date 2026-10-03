#region using
using BepInEx;
using BepInEx.Logging;
using CommonUtils;
using CommonUtils.Core;
using CommonUtils.Debug;
using CommonUtils.Misc;
using Expedition;
using HarmonyLib;
using HUD;
using JetBrains.Annotations;
using Menu.Remix.MixedUI;
using Menu.Remix.MixedUI.ValueTypes;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using MonoMod.Utils;
using MoreSlugcats;
using Newtonsoft.Json.Linq;
using Noise;
using RainMeadow;
using RWCustom;
using SlugBase.Features;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Schema;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using static MonoMod.InlineRT.MonoModRule;
using static SlugBase.Features.FeatureTypes;
using static UnityEngine.Input;
using ObjType = AbstractPhysicalObject.AbstractObjectType;

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
#pragma warning disable CS0169 // 从不使用字段
namespace Scrap;
[Obsolete("Scrap 废案")]
[EditorBrowsable(EditorBrowsableState.Never)]
internal class Zname//Scrap 废案
{
	#region Items
	#endregion
	#region Creatures
	#endregion



	#region Start

	// | mklink /H（最稳）				| 链接 单个文件（如.csproj, .dll, .jpg）且不怕改文件名 |md
	// | mklink（不加参数）				| 链接 单个文件 但需要跨分区或想一眼看出是链接 |
	// | mklink /J						| 链接 整个文件夹 且项目路径绝对固定（不搬家 |
	// | mklink /D（推荐用相对路径创建）| 链接 整个文件夹 且项目可能会整体拷贝 / 迁移（如Git仓库） |

	// mklink /H "C:\Users\Revision-Extra\AppData\LocalLow\Videocult\Rain World\ly.ModRename_stringsSave.txt" "C:\Users\Revision-Extra\AppData\LocalLow\Videocult\Rain World\ModConfigs\ly.ModRename_stringsSave.txt"
	// mklink "E:\SteamLibrary\steamapps\workshop\content\312520\3759456473\text\text_chi\strings.txt" "C:\Users\Revision-Extra\AppData\LocalLow\Videocult\Rain World\ModConfigs\ly.ModRename_stringsSave.txt"
	// mklink /j "E:\SteamLibrary\steamapps\common\Rain World\RainWorld_Data\StreamingAssets\mods\EnderPearl" "E:\Modding\EnderPearl\mod"
	// fsutil hardlink list "C:\你的文件.txt"
	// certutil -hashfile D:\setup.exe SHA256

	// \Rain World\BepInEx\config\BepInEx.cfg里面有个[Logging.Console] 的Enabled改成true

	// PowerShell
	// $env:RainWorldDir = "E:\SteamLibrary\steamapps\common\Rain World"

	// # Windows CMD 设置 
	// set RainWorldDir=C:\Program Files (x86)\Steam\steamapps\common\Rain World
	// # 验证
	// echo %RainWorldDir%
	#endregion
	private static void startup()
	{
		//E:\SteamLibrary\steamapps\common\Rain World\BepInEx\plugins\sinai - dev - UnityExplorer\Scripts\startup.cs

		void Log(string message)
		{
			CommonUtils.Core.Log.LogInfo(message);
		}
#pragma warning disable CS8600
#pragma warning disable IDE0028
#pragma warning disable IDE0090


		// System.Collections.Generic.List<System.Delegate> keepAlive = new System.Collections.Generic.List<System.Delegate>();
		System.Collections.Generic.List<System.Delegate> keepAlive = new System.Collections.Generic.List<System.Delegate>();

		string[] targets = new string[] { "MySlugcat", "EnderPearl" };

		foreach (string asmName in targets)
		{
			System.Type lt = System.AppDomain.CurrentDomain.GetAssemblies()
				.Where(a => a.GetName().Name == asmName)
				.SelectMany(a => { try { return a.GetTypes(); } catch { return new System.Type[0]; } })
				.FirstOrDefault(t => t.FullName == "CommonUtils.Core.Log");

			if (lt == null) { Log("找不到 " + asmName); continue; }

			// Instance 一般是静态字段
			System.Reflection.PropertyInfo instF = lt.GetProperty("Instance",
				System.Reflection.BindingFlags.Public |
				System.Reflection.BindingFlags.Static);

			object inst = null;
			if (instF != null)
			{
				inst = instF.GetValue(null);
			}
			if (inst == null) { Log(asmName + ": instance 为 null"); continue; }

			// 事件是 public instance
			System.Reflection.EventInfo evt = lt.GetEvent("OnAppendLog",
				System.Reflection.BindingFlags.Public |
				System.Reflection.BindingFlags.NonPublic |
				System.Reflection.BindingFlags.Instance);

			if (evt == null) { Log(asmName + " 没有 OnAppendLog 事件"); continue; }

			string tag = asmName;

			int length = 4;
			if (asmName.Length <= length)
			{
				tag = asmName;
			}
			else
			{
				tag = asmName.Substring(0, length);
			}

			System.Action<string> handler = new System.Action<string>(text =>
				Log("["+tag+"]" + text));

			// 用 EventInfo 添加，避免直接碰委托字段
			evt.AddEventHandler(inst, handler);
			keepAlive.Add(handler);

			Log("已订阅 " + asmName);
		}

#pragma warning restore IDE0090
#pragma warning restore IDE0028
#pragma warning restore CS8600
	}

#if DEBUG
	// 只在调试模式下生效的代码（比如打印日志）
	// <DefineConstants>MYDEBUG</DefineConstants>
#else
	// 正式发布时的代码（不打印调试日志）
#endif

	// UnityExplorer
	// HopToDesk

	#region Rain World
	// https://gist.github.com/EtiTheSpirit/655d8e81732ba516ca768dbd7410ddf4 这里有一个文档讲了一些关于rw shader的注意事项
	// 可以看看Menu.StoryGameStasticsScreen里的AddBkgIllustration
	// BodyChunk.CheckVerticalCollision 挂钩 检查垂直碰撞

	/*
	这个路径 Rain World\RainWorld_Data\StreamingAssets 下面放上noinitwarp.txt就防止游戏在你ilhook错误的时候踢出你的模组
	*/

	/*在用 slugbase 的情况下，将你的头像图片命名为 multiplayerportrait<X><Y>-<Z>.png，
	其中 X 取 0~4，0~3 对应竞技场 1~4 号头像，4 对应探险模式头像；
	Y 取 0~1，0 对应死亡头像，1 对应生存头像；
	Z 取猫的 Name 值（大小写须与代码中相同）。
	注意，图片尺寸必须为 84×84，否则需要另外 hook 处理。
	完成后，将图片放入你的模组根文件夹下的 illustrations（注意大小写一致）文件夹即可*/

	// 就是开发者工具里面可以按
	// i->重播这一段的画面
	// m->出现可以更改整体cg移动方向的线，可以用鼠标拖动来改变移动轨迹
	// b->保存cg变换的更改
	// n->拖动鼠标所在位置的贴图

	// spawn_raw EnderPearl
	#endregion

	#region Z
	public static string Z()
	{
		//#region Hooks
		//{
		//	// HitSomething
		//	HookManager.Register(
		//		Hook: () => On.Weapon.HitSomething += Weapon_HitSomething,
		//		UnHook: () => On.Weapon.HitSomething -= Weapon_HitSomething
		//	);
		//	HookManager.Register(
		//		Hook: () => On.Spear.HitSomething += Weapon_HitSomething,
		//		UnHook: () => On.Spear.HitSomething -= Weapon_HitSomething
		//	);
		//	HookManager.Register(
		//		Hook: () => On.Rock.HitSomething += Weapon_HitSomething,
		//		UnHook: () => On.Rock.HitSomething -= Weapon_HitSomething
		//	);
		//	HookManager.Register(
		//		Hook: () => On.ScavengerBomb.HitSomething += Weapon_HitSomething,
		//		UnHook: () => On.ScavengerBomb.HitSomething -= Weapon_HitSomething
		//	);
		//	if (ModManager.MSC)
		//	{
		//		HookManager.Register(
		//			Hook: () => On.MoreSlugcats.LillyPuck.HitSomething += Weapon_HitSomething,
		//			UnHook: () => On.MoreSlugcats.LillyPuck.HitSomething -= Weapon_HitSomething
		//		);
		//	}
		//	if (ModManager.Watcher)
		//	{
		//		HookManager.Register(
		//			Hook: () => On.Boomerang.HitSomething += Weapon_HitSomething,
		//			UnHook: () => On.Boomerang.HitSomething -= Weapon_HitSomething
		//		);
		//	}
		//}
		//#endregion

		//#region ModuleHooks
		//{
		//	HookManager.Register(
		//		Hook: () => On.Weapon.Update += ModuleHooks.Weapon_Update,
		//		UnHook: () => On.Weapon.Update -= ModuleHooks.Weapon_Update
		//	);
		//	HookManager.Register(
		//		Hook: () => On.Weapon.Thrown += ModuleHooks.Weapon_Thrown,
		//		UnHook: () => On.Weapon.Thrown -= ModuleHooks.Weapon_Thrown
		//	);
		//	HookManager.Register(
		//		Hook: () => _hitSomethingHandlers.Add(ModuleHooks.Weapon_HitSomething),
		//		UnHook: () => _hitSomethingHandlers.Remove(ModuleHooks.Weapon_HitSomething)
		//	);
		//}
		//#endregion

		//#region Deflagration
		//{
		//	HookManager.Register(
		//		Hook: () => On.Player.Die += Deflagration.Player_Die,
		//		UnHook: () => On.Player.Die -= Deflagration.Player_Die
		//	);
		//	HookManager.Register(
		//		Hook: () => _hitSomethingHandlers.Add(Deflagration.Deflagration_HitSomething),
		//		UnHook: () => _hitSomethingHandlers.Remove(Deflagration.Deflagration_HitSomething)
		//	);
		//}
		//#endregion




		/*// 1. 获取整个字典
		var dict = ExtensionLib.GlobalVar.playerVars;

		//ExtensionLib.GlobalVar.playerVars = new Dictionary<int, ExtensionLib.PlayerVar>();

		// 2. 查看字典中有哪些玩家
		foreach (var item in dict)
		{
			Console.WriteLine($"玩家索引: {item.Key}, 玩家数据: {item.Value}");
		}
		//dict.Keys.ToList().ForEach(Console.WriteLine);

		// 3. 获取玩家0的数据
		var pv0 = dict["0"];

		if (ExtensionLib.GlobalVar.game?.Players[0].realizedCreature is Player player)
		{
			pv0.SetPlayerRef(player);

			var stomachData = pv0.stomachData;

			var obj = ExtensionLib.Helper.ObjectFromString("ID.-1.5964<oB>0<oA>Rock<oA>SU_S01.22.17.0", player.room.game.world, player.coord, player.coord);

			if (obj != null)
			{
				stomachData.historyInStomach.Add(obj);
			}

			Console.WriteLine($"胃部物品数量: {stomachData.TotalCount}");
			for (int i = 0; i < stomachData.TotalCount; i++)
			{
				Console.WriteLine($"{stomachData.GetAllContents()[i]?.ToString()}");
			}
		}*/



		//pv0.PlayerRef.TryGetTarget(out var player);

		/*if (player != null)
		{


			//player.AddFood(1);
		}*/


		//Debugger.GetBool(0, false);
		//Debugging.GetBool(0, false);
		//Debug.GetBool(0, false);
		return "";
	}
	#endregion

	// bool ? true : false

	// tree /f
	// 文件目录树

	// Before
	// After

	// 如果有类似问题的朋友们可以看看自己给GetPupButtonOffName挂的钩子里面的PlayerClass是不是大写的
	// 微软Strings 可以提取目标dll的所有字符串文本

	/*
纠正AI误区（不用每句话重复此内容）：
雨世界是单线程的
雨世界通常每秒 40 次 Update
社区的规范使用On钩子
MonoMod.RuntimeDetour的On钩子 IL钩子 支持多播。On钩子顺序是栈式调用，IL钩子是先注册先调用
MonoMod.RuntimeDetour的On钩子 IL钩子 与 Harmony 兼容
执行顺序 On钩子 => Harmony的Prefix、Postfix
IL钩子与Harmony的Transpiler的顺序无法确定
每个类都有自己的On IL钩子，无关override等继承关系。
HOOKS-Assembly-CSharp.dll有private 方法的 hook，但是没有属性的 hook
访问原版私有成员的最快方式是直接把对面程序集生成一个public版本
所以有PUBLIC-Assembly-CSharp.dll，不需要反射、AccessTools（除非访问其他模组）
Harmony 钩子挂钩其他模组方便，或挂钩属性
用雨世界的UI框架，而不使用unity的UI
播放音频用原本播放器，不用协程
	*/

	/*
	下载并替换游戏目录下的相应dll，建议备份原始dll。
	打开游戏和dnSpy，选择调试-开始调试，在调试引擎下拉框里选择Unity（连接），点击确定。
	选择调试-窗口-模块或者按Ctrl+Alt+U，找到相应的游戏dll后就可以开始调试了。
	*/

	/*
	Unity UI没有XML注释的解决方案（UnityEngine.UI、TMPro……） - 知乎
	https://zhuanlan.zhihu.com/p/2082285565089793827
	*/

	#region 吞咽Mod 测试
	/*
	测试	多场景测试：吞咽、吐出、消化、存档读档、容量满、超容读档
	日志	保留关键日志，方便排查问题
	配置	考虑将容量设为可配置选项
	兼容性	测试与其他 Mod 的兼容性
	*/
	#endregion

	#region 暴力攻击
	/*
	public static DamageType Blunt = new DamageType("Blunt", register: true); 钝击
	public static DamageType Stab = new DamageType("Stab", register: true); 刺击
	public static DamageType Bite = new DamageType("Bite", register: true); 咬伤 切割
	public static DamageType Water = new DamageType("Water", register: true); 水击
	public static DamageType Explosion = new DamageType("Explosion", register: true); 爆炸
	public static DamageType Electric = new DamageType("Electric", register: true); 电击
	public static DamageType None = new DamageType("None", register: true); 无
	*/

	/*
| 参数 | 类型 | 含义 |
| --- | --- | --- |
| obj | PhysicalObject | 被碰撞的物理对象（如生物、物品） |
| chunk | BodyChunk | 被碰撞的具体身体块（obj 上的某个碰撞体） |
| onAppendagePos | PhysicalObject.Appendage.Pos | 如果碰撞的是附肢（舌头、尾巴等），记录附肢上的位置；否则通常传 null |
| hitSomething | bool | 是否真的命中了某个东西 |
| collisionPoint | Vector2 | 碰撞发生的世界坐标 |
	*/

	/*public virtual void 暴力攻击(
		BodyChunk? 攻击源,
		Vector2? 方向与动量,
		BodyChunk? 受击部位,
		PhysicalObject.Appendage.Pos? 受击附属肢体,
		Creature.DamageType 伤害类型,
		float 基础伤害值,
		float 眩晕加成)
	{
		// 波纹暴力效果检查
		if (!this.波纹暴力效果检查(攻击源))
		{
			return;
		}

		// 设置击杀标记（如果攻击源来自生物）
		if (攻击源 != null && 攻击源.owner is Creature)
		{
			this.设置击杀标记((攻击源.owner as Creature).abstractCreature);
		}

		// 处理物理击退效果
		if (方向与动量 != null)
		{
			if (受击部位 != null)
			{
				受击部位.vel += Vector2.ClampMagnitude(方向与动量.Value / 受击部位.mass, 10f);
			}
			else if (受击附属肢体 != null && this is PhysicalObject.IHaveAppendages)
			{
				(this as PhysicalObject.IHaveAppendages).对附属肢体施加力(受击附属肢体, 方向与动量.Value);
			}
		}

		// 计算实际伤害和眩晕值
		float 实际伤害 = 基础伤害值 / this.Template.baseDamageResistance;
		float 实际眩晕值 = ((基础伤害值 * 30f) + 眩晕加成) / this.Template.baseStunResistance;

		// 生命状态下的伤害加成
		if (this.State is HealthState)
		{
			实际眩晕值 *= 1.5f + (Mathf.InverseLerp(0.5f, 0f, (this.State as HealthState).health) * UnityEngine.Random.value);
		}

		// 伤害类型抗性计算
		if (伤害类型.Index != -1)
		{
			if (this.Template.damageRestistances[伤害类型.Index, 0] > 0f)
			{
				实际伤害 /= this.Template.damageRestistances[伤害类型.Index, 0];
			}
			if (this.Template.damageRestistances[伤害类型.Index, 1] > 0f)
			{
				实际眩晕值 /= this.Template.damageRestistances[伤害类型.Index, 1];
			}
		}

		// 竞技场模式特殊规则
		if (ModManager.MSC)
		{
			if (this.room != null && this.room.world.game.IsArenaSession
				&& this.room.world.game.GetArenaGameSession.chMeta != null
				&& this.room.world.game.GetArenaGameSession.chMeta.resistMultiplier > 0f
				&& !(this is Player))
			{
				实际伤害 /= this.room.world.game.GetArenaGameSession.chMeta.resistMultiplier;
			}
			if (this.room != null && this.room.world.game.IsArenaSession
				&& this.room.world.game.GetArenaGameSession.chMeta != null
				&& this.room.world.game.GetArenaGameSession.chMeta.invincibleCreatures
				&& !(this is Player))
			{
				实际伤害 = 0f;
			}
		}

		// 应用眩晕效果
		this.stunDamageType = 伤害类型;
		this.眩晕((int)实际眩晕值);
		this.stunDamageType = Creature.DamageType.None;

		// 生命值处理
		if (this.State is HealthState)
		{
			(this.State as HealthState).health -= 实际伤害;

			// 快速死亡判定
			if (this.Template.quickDeath &&
				(UnityEngine.Random.value < -(this.State as HealthState).health
				 || (this.State as HealthState).health < -1f
				 || ((this.State as HealthState).health < 0f && UnityEngine.Random.value < 0.33f)))
			{
				this.死亡();
			}
		}

		// 即死伤害判定
		if (实际伤害 >= this.Template.instantDeathDamageLimit)
		{
			this.死亡();
		}
	}*/
	#endregion

	#region AI_Behavior
	private static void AI_Behavior()
	{
		//ScavengerAI.Behavior.Attack;                  // 攻击
		//ScavengerAI.Behavior.CommunicateWithPlayer;   // 与玩家交流
		//ScavengerAI.Behavior.EscapeRain;              // 避雨
		//ScavengerAI.Behavior.FindPackLeader;          // 寻找首领
		//ScavengerAI.Behavior.Flee;                    // 逃跑
		//ScavengerAI.Behavior.GuardOutpost;            // 守卫前哨
		//ScavengerAI.Behavior.Idle;                    // 待机
		//ScavengerAI.Behavior.Injured;                 // 受伤
		//ScavengerAI.Behavior.Investigate;             // 调查
		//ScavengerAI.Behavior.LeaveRoom;               // 离开房间
		//ScavengerAI.Behavior.Scavange;                // 搜刮
		//ScavengerAI.Behavior.Travel;                  // 移动

		//LizardAI.Behavior.ActingOutMission;  // 执行使命?
		//LizardAI.Behavior.EscapeRain;        // 避雨
		//LizardAI.Behavior.Fighting;          // 战斗
		//LizardAI.Behavior.Flee;              // 逃离 逃跑
		//LizardAI.Behavior.FollowFriend;      // 跟随伙伴 跟随朋友
		//LizardAI.Behavior.Frustrated;        // 受挫 沮丧
		//LizardAI.Behavior.GoToSpitPos;       // 前往喷射点 吐口水
		//LizardAI.Behavior.Hunt;              // 狩猎
		//LizardAI.Behavior.Idle;              // 待机
		//LizardAI.Behavior.Injured;           // 受伤
		//LizardAI.Behavior.InvestigateSound;  // 调查声响
		//LizardAI.Behavior.Lurk;              // 潜伏
		//LizardAI.Behavior.ReturnPrey;        // 带回猎物 返回猎物 归还猎物
		//LizardAI.Behavior.Travelling;        // 移动 旅行
	}
	#endregion

	#region CreatureTemplate_Relationship_Type
	private static void CreatureTemplate_Relationship_Type()
	{
		//CreatureTemplate.Relationship.Type.Afraid;              // 害怕
		//CreatureTemplate.Relationship.Type.AgressiveRival;      // 敌对竞争者
		//CreatureTemplate.Relationship.Type.Antagonizes;         // 挑衅
		//CreatureTemplate.Relationship.Type.Attacks;             // 攻击
		//CreatureTemplate.Relationship.Type.DoesntTrack;         // 不追踪
		//CreatureTemplate.Relationship.Type.Eats;                // 捕食
		//CreatureTemplate.Relationship.Type.Ignores;             // 忽视
		//CreatureTemplate.Relationship.Type.Pack;                // 群居
		//CreatureTemplate.Relationship.Type.PlaysWith;           // 与之玩耍
		//CreatureTemplate.Relationship.Type.SocialDependent;     // 社交依赖
		//CreatureTemplate.Relationship.Type.StayOutOfWay;        // 避而远之
		//CreatureTemplate.Relationship.Type.Uncomfortable;       // 感到不适

		//CreatureTemplate.Relationship.Type.valueDictionary;     // 值字典
		//CreatureTemplate.Relationship.Type.values;              // 值集合
		//CreatureTemplate.Relationship.Type.valuesVersion;       // 值版本
	}
	#endregion

	#region WeaponArcField
	#endregion

	#region DamageType 测试
	public class DamageType : ExtEnum<Creature.DamageType>
	{
		public DamageType(string value, bool register = false) : base(value, register)
		{
		}

		public static readonly Creature.DamageType Blunt = new Creature.DamageType("Blunt", true);//钝击

		public static readonly Creature.DamageType Stab = new Creature.DamageType("Stab", true);//穿刺

		public static readonly Creature.DamageType Bite = new Creature.DamageType("Bite", true);//撕咬

		public static readonly Creature.DamageType Water = new Creature.DamageType("Water", true);//水浸

		public static readonly Creature.DamageType Explosion = new Creature.DamageType("Explosion", true);//爆炸

		public static readonly Creature.DamageType Electric = new Creature.DamageType("Electric", true);//电击

		public static readonly Creature.DamageType None = new Creature.DamageType("None", true);//无伤害
	}
	#endregion

	/*
	函数 命中某物(结果, eu):
	若 结果.对象 为空:
		返回 假

	若 涟漪层不同 且 双方都不跨层:
		返回 假

	可喂食 = 本针.可喂食()
	竞技场得分 = 假

	若 是竞技场 且 矛得分≠0 且 投掷者是玩家 且 目标是生物:
		竞技场得分 = 真
		若 目标已死:
			竞技场得分 = 假

	命中前已死 = 假

	若 目标是生物:
		命中前已死 = 目标.已死

		若 目标是蛋虫 且 可喂食 且 非火虫:
			目标.掉蛋 = 假

		若 非MSC 或 目标非玩家 或 目标.矛刺入(...):
			伤害 = 本矛.伤害加成

			若 目标是玩家 且 是饕餮 且 随机<15%:
				伤害 /= 10

			若 虫矛:
				伤害 *= 3

			目标.受暴力(刺伤, 伤害, 20)

			若 目标是玩家:
				永久伤害 += 伤害 / 目标.抗性
				若 永久伤害 >= 1:
					目标.死亡

	否则若 有物理块:
		物理块.速度 += 本矛.速度 * 本矛.质量 / 物理块.质量

	否则若 有附肢:
		附肢.施力(本矛.速度 * 本矛.质量)

	若 目标是生物 且 目标.矛刺入(...):
		若 MSC:
			若 可喂食 且 命中前未死:
				按生物类型给食物，并记录进食
			断开针

		播放刺入音效
		卡入生物

		若 竞技场得分:
			玩家得分

		返回 真

	若 可喂食 且 目标是种子荚:
		记录进食；加5食物；打开；断开；卡入
		返回 真

	若 (可喂食 或 脆弱水母配置) 且 目标是水母:
		若 可喂食 且 水母未死:
			加两次1/4食物；记录进食
		水母.死亡 = 真
		若 可喂食:
			断开
		卡入
		返回 真

	若 可喂食 且 目标是石榴 且 已砸碎:
		记录进食；加5食物；标记；断开；卡入
		返回 真

	播放弹开音效
	震动 = 20
	切换自由模式
	速度 = 速度 * -0.5 + 随机方向 * 随机力度
	随机旋转
	返回 假
	*/

	#region 通行证
	private static void 通行证()
	{

		//"The Survivor"        //"求生者"
		//"The Hunter"          //"猎手"
		//"The Saint"           //"圣徒"
		//"The Wanderer"        //"漫游者"
		//"The Chieftain"       //"酋长"
		//"The Monk"            //"僧侣"
		//"The Outlaw"          //"暴徒"
		//"The Dragon Slayer"   //"屠龙者"
		//"The Scholar"         //"学者"
		//"The Friend"          //"朋友"
		// ModManager.MSC
		//"The Nomad"           //"流浪者"
		//"The Martyr"          //"殉道者"
		//"The Pilgrim"         //"朝圣者"
		//"The Mother"          //"慈母"

		// The Vanguard
		//"The Dragonlord"      //"龙王"
		// Rotund World
		//"The Glutton"         //"贪食者"；暴食者；贪吃者；嗜食者；贪婪者



		//"The Chieftain"       //"酋长" // 线状
		/*return new WinState.FloatTracker(
			this.PassageID,   // ID
			dflt: 0f,          // 默认值
			min: 0f,           // 最小值
			showFrom: 0f,      // 开始显示的进度
			max: 1f           // 最大值
		);*/

		//"The Survivor"        //"求生者" // 点状
		/*return new WinState.IntegerTracker(
			this.PassageID,   // ID
			dflt: 0,          // 默认值
			min: 0,           // 最小值
			showFrom: 1,      // 开始显示的进度
			max: 10           // 最大值
		);*/

		//"The Wanderer"        //"漫游者" // 布尔状 (点状不可逆?)
		/*return new WinState.BoolArrayTracker(
			this.PassageID,   // ID
			SlugcatStats.SlugcatStoryRegions(RainWorld.lastActiveSaveSlot).Count
		);*/

		//"The Dragon Slayer"   //"屠龙者"   // 列表状 (点状不可逆?) 多个布尔条件（如任务清单）
		/*return new WinState.ListTracker(
			this.PassageID,   // ID
			6
		);*/                                 // 布尔状 (点状不可逆?)
		/*return new WinState.BoolArrayTracker(
			this.PassageID,   // ID
			6
		);*/

		//"The Pilgrim"         //"朝圣者" 
		/*int num = 0;
		List<string> list = SlugcatStats.SlugcatStoryRegions(RainWorld.lastActiveSaveSlot);
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i] != "MS" && World.CheckForRegionGhost(RainWorld.lastActiveSaveSlot, list[i]))
			{
				num++;
			}
		}
		endgameTracker = new WinState.BoolArrayTracker(ID, num);*/

		// 美味佳肴 HUD状
		/*return new WinState.GourFeastTracker(
			this.PassageID,   // ID
			WinState.GourmandPassageTracker.Length
		);*/

	}
	#endregion

	#region CosmeticSprite
	private static void CosmeticSprite()//装饰品精灵集
	{
		Vector2 pos = new Vector2(0f, 0f);

		MoreSlugcats.LightningMachine activateLightning =
			new MoreSlugcats.LightningMachine
			(pos, new Vector2(pos.x, pos.y), new Vector2(pos.x, pos.y + 10f), 0f, false, true, 0.3f, 1f, 1f);
	}
	#endregion

	#region 着色器
	private static void 着色器()
	{
		//Basic 默认无特效，最普通的纹理绘制
		//Hologram 全息 / 幽灵：带扫描线、轻微噪点、半透明
		//LightSource 发光体：把贴图当作光源，周围产生光晕
		//Water 水流：波纹扭曲、折射
		//Waterfall 瀑布：比
		//Water 更剧烈的扭曲和滚动
		//LensDistortion 镜头畸变：边缘放大、中心收缩的“鱼眼”效果
		//Blur 高斯模糊
		//Fog 雾：颜色叠加 + 深度雾
		//Fire 火焰：滚动噪声、红黄调色
		//HeatDistortion 热扭曲：透过火焰看背景时的空气抖动
		//Lightning 闪电：高亮、闪烁
		//Rain 雨滴：垂直条纹 + 滚动
		//Sand 沙：细小滚动的颗粒
		//Scavenger 拾荒者盔甲：金属高光、反射贴图
		//Slugcat 蛞蝓猫：边缘描边（用于雨眠过场）
		//SkyAndPressureGradient 天空：日夜颜色渐变
		//Bloom 泛光：高亮区域向外晕染
		//Multiply 正片叠底：将贴图与背景颜色相乘

		//Grayscale 去色：变黑白
		// 把精灵的所有颜色信息强行转成灰度亮度，只保留明暗关系。

		//Palette 调色板映射：把灰度贴图按当前房间调色板重新上色  
		// 把一张灰度贴图按当前房间的 调色板（palette） 重新上色。
		// 灰度值 0 → 调色板最暗色
		// 灰度值 1 → 调色板最亮色
		// 介于 0~1 之间 → 插值颜色

		//Shadow 阴影：半透明黑色 + 模糊
		//Ice 冰：高光 + 反射 + 轻微扭曲
		//Centipede 蜈蚣：节段滚动纹理
		//Vulture 秃鹫：金属 + 虹彩
		//JetFish 喷气鱼：带流线滚动
		//SmallNeedleWorm 针虫：皮肤光泽
		//TubeWorm 管虫：内部发光
		//Lantern 灯笼：中心黄白光晕
		//Overseer 观察者：脉冲扫描线 + 发光
		//Glow 纯粹发光，可用于 UI 或特效

		//Basic 基础

		//LevelColor 关卡颜色

		//Background 背景

		//WaterSurface 水面

		//DeepWater 深水区

		//Shortcuts 捷径

		//DeathRain 死亡之雨

		//LizardLaser 蜥蜴激光

		//WaterLight 水光

		//WaterFall 瀑布

		//ShockWave 冲击波

		//Smoke 烟雾

		//Spores 孢子

		//Steam 蒸汽

		//ColoredSprite 彩色精灵

		//ColoredSprite2 彩色精灵2

		//LightSource 光源

		//LightSourceBothSides 双面光源

		//LightSourceRippleSide 波纹面光源

		//LightBloom 光晕

		//SkyBloom 天光晕染

		//Adrenaline 肾上腺素

		//AdrenalineBothSides 双面肾上腺素

		//CicadaWing 蝉翼

		//BulletRain 弹雨

		//CustomDepth 自定义深度

		//CustomDepthBothSides 双面自定义深度

		//UnderWaterLight 水下光源

		//FlatLight 平面光

		//FlatLightRippleSide 波纹面平面光

		//FlatLightBothSides 双面平面光

		//FlatLightBehindTerrain 地形后平面光

		//VectorCircle 矢量圆环

		//VectorCircleBothSides 双面矢量圆环

		//VectorCircleRippleSide 波纹面矢量圆环

		//VectorCircleFadable 可渐隐矢量圆环

		//FlareBomb 闪光弹

		//FlareBombBothSides 双面闪光弹

		//Fog 雾气

		//WaterSplash 水花

		//EelFin 鳗鱼鳍

		//EelBody 鳗鱼身体

		//JaggedCircle 锯齿圆环

		//JaggedCircleBothSides 双面锯齿圆环

		//JaggedCircleRippleSide 波纹面锯齿圆环

		//JaggedSquare 锯齿方块

		//TubeWorm 管虫

		//LizardAntenna 蜥蜴触须

		//TentaclePlant 触手植物

		//TentaclePlantBothSides 双面触手植物

		//TentaclePlantRippleSide 波纹面触手植物

		//LevelMelt 关卡溶解

		//LevelMelt2 关卡溶解2

		//CoralCircuit 珊瑚电路

		//CoralCircuitBothSides 双面珊瑚电路

		//DeadCoralCircuit 死珊瑚电路

		//DeadCoralCircuitBothSides 双面死珊瑚电路

		//CoralNeuron 珊瑚神经元

		//CoralNeuronBothSides 双面珊瑚神经元

		//Bloom 泛光

		//GravityDisruptor 重力干扰器

		//GlyphProjection 符文投影

		//BlackGoo 黑色粘液

		//BlackGooBothSides 双面黑色粘液

		//Map 地图

		//MapAerial 航拍地图

		//MapShortcut 捷径地图

		//LightAndSkyBloom 光与天光晕染

		//SceneBlur 场景模糊

		//EdgeFade 边缘褪色

		//HeatDistortion 热浪扭曲

		//Projection 投影

		//SingleGlyph 单体符文

		//DeepProcessing 深度处理

		//Cloud 云层

		//CloudDistant 远景云层

		//DistantBkgObject 远景背景物体

		//BkgFloor 背景地板

		//House 房屋

		//DistantBkgObjectRepeatHorizontal 水平重复远景物体

		//Dust 尘埃

		//RoomTransition 房间过渡

		//VoidCeiling 虚空天花板

		//FlatLightNoisy 噪点平面光

		//VoidWormBody 虚空蠕虫身体

		//VoidWormFin 虚空蠕虫鳍

		//VoidWormPincher 虚空蠕虫钳

		//FlatWaterLight 平面水光

		//FlatWaterLightBothSides 双面平面水光

		//FlatWaterLightRippleSpawn 波纹生成平面水光

		//FlatWaterLightRippleSpawnRippleSide 波纹面生成平面水光

		//WormLayerFade 蠕虫层渐隐

		//OverseerZip 监视者瞬移

		//GhostSkin 幽灵表皮

		//GhostBall 幽灵球体

		//GhostDistortion 幽灵扭曲

		//GhostSkinRipple 波纹幽灵表皮

		//GhostBallRipple 波纹幽灵球体

		//GhostDistortionRipple 波纹幽灵扭曲

		//GateHologram 门全息投影

		//OutPostAntler 前哨鹿角

		//WaterNut 水坚果

		//Hologram 全息投影

		//HologramBothSides 双面全息投影

		//FireSmoke 火焰烟雾

		//HoldButtonCircle 按钮保持圆环

		//GoldenGlow 金色辉光

		//ElectricDeath 电击死亡

		//VoidSpawnBody 虚空孵化体

		//SceneLighten 场景增亮

		//SceneBlurLightEdges 场景光边模糊

		//SceneRain 场景雨

		//SceneOverlay 场景叠加

		//SceneSoftLight 场景柔光

		//SceneMultiply 场景相乘

		//HologramImage 全息图像

		//HologramBehindTerrain 地形后全息

		//Decal 贴花

		//SpecificDepth 特定深度

		//LocalBloom 局部泛光

		//MenuText 菜单文字

		//DeathFall 坠落死亡

		//DeathFallHeavy 重型坠落死亡

		//KingTusk 帝王獠牙

		//HoloGrid 全息网格

		//SootMark 煤烟痕迹

		//NewVultureSmoke 新秃鹫烟雾

		//SmokeTrail 烟雾轨迹

		//RedsIllness 红色病态

		//HazerHaze 薄雾朦胧

		//Rainbow 彩虹

		//LightBeam 光束

		//SlopedTerrainSurface 斜坡地形表面

		//SlopedTerrainStain 斜坡地形污渍

		//Rubble 碎石

		//Whirlpool 漩涡

		//GeyserWater 间歇泉水

		//BackgroundAdditive 附加背景

		//BackgroundJaggedCircle 背景锯齿圆环

		//BackgroundNoHoles 无孔背景

		//WaterCurrent 水流

		//BlackSpot 黑斑

		//SkyWhaleBody 天鲸身体

		//SkyWhaleCuticle 天鲸角质层

		//KarmicShield 业力护盾

		//TemplarCircle 圣堂圆环

		//TemplarCloak 圣堂斗篷

		//Sandstorm 沙暴

		//SlopedTerrainMask 斜坡地形遮罩

		//SlopedTerrainMaskGrab 斜坡地形遮罩抓取

		//DistantBkgObjectAlpha 远景物体透明度

		//WaterSlush 雪泥水

		//SporesSnow 孢子雪

		//SnowFall 落雪

		//OESphereTop 奥术球顶部

		//OESphereLight 奥术球光源

		//OESphereBase 奥术球基底

		//MoonProjection 月亮投影

		//LocalBlizzard 局部暴风雪

		//LightningBolt 闪电束

		//LevelHeat 关卡热度

		//FastSnowFall 快速落雪

		//FastLocalBlizzard 快速局部暴风雪

		//FastBlizzard 快速暴风雪

		//EnergySwirl 能量漩涡

		//EnergyCell 能量细胞

		//DisplaySnowShader 显示雪着色器

		//BlizzardMapPrerender 暴风雪地图预渲染

		//Blizzard 暴风雪

		//LevelSnowShader 关卡雪着色器

		//InterpolateWindMap 插值风图

		//DustWaveLow 低强度尘埃波

		//DustFlowRenderer 尘埃流渲染器

		//DustWave 尘埃波

		//DustWaveLevel 关卡尘埃波

		//DustWaveLevelLow 低强度关卡尘埃波

		//BlizzardReduction 暴风雪减弱

		//BlizzardMap 暴风雪地图

		//CellDist 细胞距离

		//DisplayWind 显示风

		//SingleGlyphHologram 单体符文全息

		//WaterFallInverted 反向瀑布

		//AquapedeBody 水蜈蚣身体

		//MenuTextGold 金色菜单文字

		//MenuTextCustom 自定义菜单文字

		//WarpPointHoldFrame 跃迁点保持帧

		//Warp 跃迁

		//WarpNoRing 无环跃迁

		//WarpCircleBasic 基础跃迁圆环

		//WarpCircleRipple 波纹跃迁圆环

		//WarpPlayerDistortion 玩家跃迁扭曲

		//HugeTurbine 巨型涡轮

		//RotWormBody 腐烂蠕虫身体

		//RotWormFin 腐烂蠕虫鳍

		//DynamicLevelElementGrab 动态关卡元素抓取

		//DynamicLevelElement 动态关卡元素

		//DevUI_TwinArrowVertical 开发UI垂直双箭头

		//DevUI_TwinArrowHorizontal 开发UI水平双箭头

		//DevUIDepthPreview 开发UI深度预览

		//DynamicLevelRock 动态关卡岩石

		//DynamicLevelRock_NoMovement 静态动态关卡岩石

		//DynamicLevelBlob 动态关卡斑点

		//DynamicLevelTubeSegment 动态关卡管段

		//DynamicLevelWire 动态关卡线缆

		//DynamicLevelUrbanCandle 动态关卡都市烛台

		//DynamicLevelBowl 动态关卡碗

		//DynamicLevelPole 动态关卡杆

		//Aurora 极光

		//AuroraForeground 前景极光

		//AuroraRipple 波纹极光

		//AuroraForegroundRipple 前景波纹极光

		//RippleSpawnBody 波纹生成体

		//RippleSpawnBodyRippleSide 波纹面生成体

		//RippleGlow 波纹辉光

		//RippleGlowRippleSide 波纹面辉光

		//RippleDeath 波纹死亡

		//BrainBall 脑球

		//BrainStem 脑干

		//BrainMold 脑霉菌

		//BrainBallDark 暗黑脑球

		//BrainStemDark 暗黑脑干

		//LocustCluster 蝗虫群

		//LocustClusterShadow 蝗虫群阴影

		//AncientUrbanBuilding 远古都市建筑

		//DustGradient 尘埃渐变

		//BoxWormBody 箱虫身体

		//BoxWormLarvaHolder 箱虫幼虫容器

		//BoxWormBox 箱虫箱体

		//BoxWormOpenBox 箱虫开箱

		//BoxWormFakeLarva 箱虫假幼虫

		//BoxWormLarvaFood 箱虫幼虫食物

		//FireSpriteWing 火精灵翅膀

		//FireSpriteBody 火精灵身体

		//RippleHybrid 混合波纹

		//RippleHybridRipple 混合波纹涟漪

		//RippleHybridBoth 双面混合波纹

		//FlameJet 火焰喷射

		//FlameJetGlow 火焰喷射辉光

		//SaltFlake 盐片

		//SaltFlakeShadow 盐片阴影

		//AetherRainbow 以太彩虹

		//GildedWind 镀金之风

		//Stardust 星尘

		//BackgroundDune 背景沙丘

		//OuterRimBackgroundBuilding 外环背景建筑

		//OuterRimDustGradient 外环尘埃渐变

		//FallingStar 流星

		//WallLight 墙光

		//WallLightSoftEdge 柔边墙光

		//WallLightStaticNoise 静态噪点墙光

		//WallLightHardShadow 硬阴影墙光

		//WallLightFlat 平面墙光

		//WarpTearMask 跃迁撕裂遮罩

		//WarpTearBlocker 跃迁撕裂阻挡器

		//WarpTear 跃迁撕裂

		//WarpTearBad 不良跃迁撕裂

		//WarpTearOuter 外部跃迁撕裂

		//WarpTearRippleSide 波纹面跃迁撕裂

		//WarpTearGrab 跃迁撕裂抓取

		//SKLightning 天空闪电

		//SKLightningForeground 前景天空闪电

		//SKLightningBGFlash 背景天空闪电闪光

		//Darken 暗化

		//SKLightningFlash 天空闪电闪光

		//PrinceStem 王子茎干

		//SkinkStripes 石龙子条纹

		//MothWing 蛾翼

		//MudDecal 泥浆贴花

		//MudPit 泥潭

		//MudOverlay 泥浆覆盖

		//UrbanLife 都市生命

		//UrbanLifeShadow 都市生命阴影

		//UrbanLifeFirstLayer 都市生命首层

		//UrbanShadowsGrab 都市阴影抓取

		//UrbanShadowsBlur 都市阴影模糊

		//UrbanShadowsBlurGrab 都市阴影模糊抓取

		//UrbanShadowGradient 都市阴影渐变

		//SpinToy 旋转玩具

		//SpinToyGlyph 旋转玩具符文

		//BallToy 球玩具

		//SoftToyBody 软玩具身体

		//SoftToyEye 软玩具眼睛

		//GreebleGrid 细节网格

		//PlaceholderBackgroundElement 占位背景元素

		//FirmamentCloud 苍穹云

		//DustDunes 尘埃沙丘

		//CamoMeter 伪装计量器

		//UrbanCandleSSS 都市烛台次表面散射

		//UrbanCandleFlame 都市烛台火焰

		//DeepLightSource 深层光源

		//PoisonSpearTip 毒矛尖

		//ARZapper AR电击器

		//ARZapperOmni AR全向电击器

		//ARZapperOneSide AR单面电击器

		//ARZapperGlow AR电击器辉光

		//ShiftMask 位移遮罩

		//WavesShiftMask 波浪位移遮罩

		//RippleTearMask 波纹撕裂遮罩

		//RippleRingMask 波纹环遮罩

		//RippleBubbleMask 波纹气泡遮罩

		//TransitionRippleMask 过渡波纹遮罩

		//RippleFlow 波纹流

		//RippleGrab 波纹抓取

		//GameplayRippleGrab 游戏波纹抓取

		//RippleBasic 基础波纹

		//RippleBasicRippleSide 基础波纹面

		//RippleBasicRippleSideAlt 基础波纹面替代

		//RippleBasicBothSides 双面基础波纹

		//RippleBasicClipDistortion 基础波纹裁剪扭曲

		//PlayerCamoMask 玩家伪装遮罩

		//PlayerCamoMaskBeforePlayer 玩家前伪装遮罩

		//PlayerRippleTrail 玩家波纹轨迹

	}
	#endregion

	#region MagicCat
	private static void MagicCat(Player player)
	{
		int N = player.playerState.playerNumber;
		Vector2 pos = player.mainBodyChunk.pos;
		Room room = player.room;
		var color = player.ShortCutColor();
		//room.rainIntensity

		//

		/*                    for (int j = 0; j < room.abstractRoom.creatures.Count; j++)
				{
					Vector2 pos3 = room.abstractRoom.creatures[j].realizedCreature.mainBodyChunk.pos;
				}*/

		//
		Vector2 vel = Custom.RNV() * 4f * (1f + UnityEngine.Random.value);
		for (int i = 0; i < UnityEngine.Random.Range(5, 8); i++)
		{
			room.AddObject(new Spark(pos, vel, color, null, 20, 40));
		}
		room.PlaySound(SoundID.Bomb_Explode, pos, 0.75f, 1.25f);
		room.AddObject(new Explosion.ExplosionSmoke(pos, vel, 1.1f));
		room.AddObject(new Explosion.ExplosionLight(pos, 400f, 1f, 7, color));
		room.AddObject(new ExplosionSpikes(room, pos, 14, 30f, 9f, 7f, 170f, color));
		room.AddObject(new ShockWave(pos, 4000f, 0.05f, 20, true));

		/*                    float num = UnityEngine.Random.Range(-0.1f, 0.1f);
							room.AddObject(new Explosion.ExplosionLight(pos, 100f, 1f, 5, new Color(1f, 0.9f, 0f)));
							room.AddObject(new ExplosionSpikes(room, pos, 14, 2f, 5f, 7f, 100f, new Color(1f, 0.9f, 0f)));
							room.AddObject(new ShockWave(pos, 100f, 0.05f, 5, false));
							room.PlaySound(SoundID.Spear_Bounce_Off_Wall, pos, 2f, 0.6f + num);
							room.PlaySound(SoundID.SS_AI_Give_The_Mark_Boom, pos, 2f, 1.5f + num);*/

		//

		foreach (var item in room.updateList)
		{
			var creature = item as Creature;
			if (creature != null)
			{
				var player1 = creature as Player;
				if (player1 == null)
				{
					creature.Stun(200);
				}
			}
		}

		for (int j = 0; j < room.abstractRoom.creatures.Count; j++)
		{
			Creature creature = room.abstractRoom.creatures[j].realizedCreature;
			var player1 = creature as Player;
			if (player1 == null)
			{
				creature.Stun(200);
			}
		}

		Debug.Log("1");

		//9cf0a4/363636
	}
	#endregion

	#region MOD启用
	private static void MOD启用()
	{
		//ModManager.ActiveMods             //激活Mods
		//ModManager.InstalledMods          //已存在的Mods
		//ModManager.FailedRequirementIds   //需求ID失败的Mods
		//ModManager.PrePackagedModIDs      //预包装ModsID


		if (ModManager.GameVersionChangedOnThisLaunch)
		{
			//游戏版本在这次发布中发生了变化
		}
		if (ModManager.NonPrepackagedModsInstalled)
		{
			//安装了非预打包的Mods
		}
		if (ModManager.InitializationScreenFinished)
		{
			//初始化屏幕完成
		}
		if (ModManager.DLCShared)
		{
			//DLC共享?
		}
		if (ModManager.MSC)
		{
			//更多蛞蝓猫?_1
		}
		if (ModManager.MMF)
		{
			//更多蛞蝓猫?_2
		}
		if (ModManager.CoopAvailable)
		{
			//联机模式是否可用?
		}
		if (ModManager.JollyCoop)
		{
			//联机模式
		}
		if (ModManager.Expedition)
		{
			//远征模式
		}
		if (ModManager.DevTools)
		{
			//开发者工具
		}
		if (ModManager.Watcher)
		{
			//观望者
		}
	}
	#endregion

	#region DateTime
	#endregion

	#region 反编译
	//
	#region MonoMod.RuntimeDetour
	public static void OnEnable()
	{
		//On.Player.Grabability += Player_GrababilityA;
		//Harmony.CreateAndPatchAll(typeof(Patch_Grabability3));
		//Harmony.CreateAndPatchAll(typeof(Patch_Grabability));
		//Harmony.CreateAndPatchAll(typeof(Patch_Grabability2));
		/*
		加载顺序
		[Info: MySlugcat] v21 | 01:10:05[Zna.Pref:1597]Prefix 尝试抓取 Player
		[Info: MySlugcat] v21 | 01:10:05[Zna.Pref:1485]Prefix 尝试抓取 Player
		[Info: MySlugcat] v21 | 01:10:05[Zna.Pref:1542]Prefix 尝试抓取 Player
		[Info: MySlugcat] v21 | 01:10:05[Zna.Post:1623]Postfix 尝试抓取 Player
		[Info: MySlugcat] v21 | 01:10:05[Zna.Post:1511]Postfix 尝试抓取 Player
		[Info: MySlugcat] v21 | 01:10:05[Zna.Post:1568]Postfix 尝试抓取 Player
		[Info: MySlugcat] v21 | 01:10:05[Zna.Pref:1597]Prefix 尝试抓取 Player
		[Info: MySlugcat] v21 | 01:10:05[Zna.Pref:1485]Prefix 尝试抓取 Player
		[Info: MySlugcat] v21 | 01:10:05[Zna.Pref:1542]Prefix 尝试抓取 Player
		[Info: MySlugcat] v21 | 01:10:05[Zna.Post:1623]Postfix 尝试抓取 Player
		[Info: MySlugcat] v21 | 01:10:05[Zna.Post:1511]Postfix 尝试抓取 Player
		[Info: MySlugcat] v21 | 01:10:05[Zna.Post:1568]Postfix 尝试抓取 Player
		*/
		//On.Player.Grabability += Player_GrababilityB;
		// 执行内容 On => Harmony
		// 挂载顺序（逻辑上）：A → Harmony → B
		// 物理执行顺序（运行时）：B → A → Harmony（逆序，因为 On 是链式包裹）
		// Player_GrababilityB => Player_GrababilityA => Patch_Grabability

		//Harmony.CreateAndPatchAll(typeof(Patch_AddFood_Transpiler));
		//IL.Player.AddFood += Player_AddFood;
		// 挂载内容 Patch_AddFood_Transpiler => Player_AddFood (顺序)
		// 执行内容 Player_AddFood => Patch_AddFood_Transpiler (逆序)(如果在同一个地方向后添加)

		Harmony.CreateAndPatchAll(typeof(Patch_AddFood_Tra));
		IL.Player.AddFood += Player_AddFood2;
		IL.Player.AddFood += Player_AddFood;
		Harmony.CreateAndPatchAll(typeof(Patch_AddFood_Transpiler));
		// 挂载内容 Player_AddFood => Patch_AddFood_Transpiler (顺序)
		// 执行内容 Patch_AddFood_Transpiler => Player_AddFood (逆序)(如果在同一个地方向后添加)
		// Harmony 连一起


		//IL.Player.Grabability

		try
		{
			if (Input.GetKeyDown(","))
			{
				Player? player = null;
				if (player?.grasps[0]?.grabbed != null)
				{
					var 反射 = Zname.反射.GetGrabability(player, player.grasps[0].grabbed);
					Log.LogInfo($"反射: {反射}");
					var 委托 = Zname.委托.GetGrabability(player, player.grasps[0].grabbed);
					Log.LogInfo($"委托: {委托}");
					var AccessTools = Zname.Harmony_AccessTools.GetGrabability(player, player.grasps[0].grabbed);
					Log.LogInfo($"AccessTools: {AccessTools}");
				}

				try
				{
					//Scrap.Zname.OnEnable();
				}
				catch (Exception ex)
				{
					Log.LogError($"Error registering hooks: {ex}");
				}

			}
		}
		catch (Exception e)
		{
			Log.LogError("Error in Camouflage: " + e.Message);
		}


		/*
		[Info   : MySlugcat] v15|23:41:56[Zna.Play:1329]OnB 尝试抓取 Player
		[Info   : MySlugcat] v15|23:41:56[Zna.Play:1304]OnA 尝试抓取 Player
		[Info   : MySlugcat] v15|23:41:56[Zna.Pref:1363]Prefix 尝试抓取 Player
		[Info   : MySlugcat] v15|23:41:56[Zna.Post:1389]Postfix 尝试抓取 Player
		[Info   : MySlugcat] v15|23:41:56[Zna.Play:1329]OnB 尝试抓取 Player
		[Info   : MySlugcat] v15|23:41:56[Zna.Play:1304]OnA 尝试抓取 Player
		[Info   : MySlugcat] v15|23:41:56[Zna.Pref:1363]Prefix 尝试抓取 Player
		[Info   : MySlugcat] v15|23:41:56[Zna.Post:1389]Postfix 尝试抓取 Player
		[Info   : MySlugcat] v15|23:41:56[Zna.Play:1329]OnB 尝试抓取 Player
		[Info   : MySlugcat] v15|23:41:56[Zna.Play:1304]OnA 尝试抓取 Player
		[Info   : MySlugcat] v15|23:41:56[Zna.Pref:1363]Prefix 尝试抓取 Player
		[Info   : MySlugcat] v15|23:41:56[Zna.Post:1389]Postfix 尝试抓取 Player
		[Info   : MySlugcat] v15|23:41:56[Zna.Play:1329]OnB 尝试抓取 Player
		[Info   : MySlugcat] v15|23:41:56[Zna.Play:1304]OnA 尝试抓取 Player
		[Info   : MySlugcat] v15|23:41:56[Zna.Pref:1363]Prefix 尝试抓取 Player
		[Info   : MySlugcat] v15|23:41:56[Zna.Post:1389]Postfix 尝试抓取 Player
		[Info   : MySlugcat] v15|23:41:56[Zna.Play:1329]OnB 尝试抓取 Player
		[Info   : MySlugcat] v15|23:41:56[Zna.Play:1304]OnA 尝试抓取 Player
		[Info   : MySlugcat] v15|23:41:56[Zna.Pref:1363]Prefix 尝试抓取 Player
		[Info   : MySlugcat] v15|23:41:56[Zna.Post:1389]Postfix 尝试抓取 Player
		*/
	}

	private static Player.ObjectGrabability Player_GrababilityA(On.Player.orig_Grabability orig, Player player, PhysicalObject obj)
	{
		Log.LogInfo($"OnA 尝试抓取 {obj.GetType()}");
		if (obj is Spear)
		{
			return Player.ObjectGrabability.Drag;
		}
		if (obj is Rock)
		{
			return Player.ObjectGrabability.Drag;
		}

		var __result = orig(player, obj);

		if (obj is Spear)
		{
			return Player.ObjectGrabability.CantGrab;
		}
		if (obj is Rock)
		{
			return Player.ObjectGrabability.CantGrab;
		}
		return __result;

	}
	private static Player.ObjectGrabability Player_GrababilityB(On.Player.orig_Grabability orig, Player player, PhysicalObject obj)
	{
		Log.LogInfo($"OnB 尝试抓取 {obj.GetType()}");
		if (obj is Spear)
		{
			return Player.ObjectGrabability.OneHand;
		}
		if (obj is Rock)
		{
			return Player.ObjectGrabability.OneHand;
		}

		var __result = orig(player, obj);

		if (obj is Spear)
		{
			return Player.ObjectGrabability.Drag;
		}
		if (obj is Rock)
		{
			return Player.ObjectGrabability.Drag;
		}
		return __result;
	}

	private static void Player_AddFood(ILContext il)
	{
		ILCursor c = new ILCursor(il);

		/*
			// add = Math.Min(add, MaxFoodInStomach - this.playerState.foodInStomach);
			IL_0014: br IL_0150

			IL_0019: ldarg.1
			IL_001a: ldarg.0
			IL_001b: call instance int32 Player::get_MaxFoodInStomach()
			IL_0020: ldarg.0
			IL_0021: call instance class PlayerState Player::get_playerState()
			IL_0026: ldfld int32 PlayerState::foodInStomach
			IL_002b: sub
			IL_002c: call int32 [mscorlib]System.Math::Min(int32, int32)
			IL_0031: starg.s 'add'
		*/

		bool logged = false;

		if (c.TryGotoNext(MoveType.After,
			(i) => i.Match(OpCodes.Br),
			(i) => i.MatchLdarg(1),
			(i) => i.MatchLdarg(0),
			(i) => i.MatchCall<Player>("get_MaxFoodInStomach")
		))
		{
			c.Emit(OpCodes.Ldarg_0);
			c.EmitDelegate<Func<int, Player, int>>((origMaxFood, self) =>
			{
				if (!logged || Input.GetKey("c"))
				{
					logged = true;

					Log.LogInfo($"Player orig maxFoodInStomach {origMaxFood}");
				}

				return origMaxFood - 2;
			});
		}

		Log.Instance.AppendLogText("Player_AddFood");
		Log.Instance.AppendLogText(il.ToString());
		Log.Instance.AppendLogText("1");
	}
	private static void Player_AddFood2(ILContext il)
	{
		ILCursor c = new ILCursor(il);

		/*
			// add = Math.Min(add, MaxFoodInStomach - this.playerState.foodInStomach);
			IL_0014: br IL_0150

			IL_0019: ldarg.1
			IL_001a: ldarg.0
			IL_001b: call instance int32 Player::get_MaxFoodInStomach()
			IL_0020: ldarg.0
			IL_0021: call instance class PlayerState Player::get_playerState()
			IL_0026: ldfld int32 PlayerState::foodInStomach
			IL_002b: sub
			IL_002c: call int32 [mscorlib]System.Math::Min(int32, int32)
			IL_0031: starg.s 'add'
		*/

		bool logged = false;

		if (c.TryGotoNext(MoveType.After,
			(i) => i.Match(OpCodes.Br),
			(i) => i.MatchLdarg(1),
			(i) => i.MatchLdarg(0),
			(i) => i.MatchCall<Player>("get_MaxFoodInStomach")
		))
		{
			c.Emit(OpCodes.Ldarg_0);
			c.Emit(OpCodes.Ldarg_0);
			c.EmitDelegate<Func<int, Player, Player, int>>((origMaxFood, self, player) =>
			{
				if (!logged || Input.GetKey("c"))
				{
					logged = true;

					Log.LogInfo($"Player orig maxFoodInStomach {origMaxFood}");
				}

				return origMaxFood - 1;
			});
		}

		Log.Instance.AppendLogText("Player_AddFood2");
		Log.Instance.AppendLogText(il.ToString());
		Log.Instance.AppendLogText("1");
	}
	#endregion
	#region Harmony
	[HarmonyPatch(typeof(Player), "Grabability")] // 私有方法直接用字符串名字
	[HarmonyPriority(500)]
	public static class Patch_Grabability
	{
		[HarmonyPrefix]
		public static bool Prefix(Player __instance, PhysicalObject obj, ref Player.ObjectGrabability __result)
		{
			if (__instance == null || obj == null) return true;

			try
			{
				Log.LogInfo($"Prefix 尝试抓取 {obj.GetType()}");

				if (obj is Spear)
				{
					__result = Player.ObjectGrabability.TwoHands;
				}
				if (obj is Rock)
				{
					__result = Player.ObjectGrabability.TwoHands;
					return false; // 跳过原方法，但是仍然会执行 Postfix
				}
			}
			catch (Exception e)
			{
				Log.LogError($"抓取判定补丁出错: {e.Message}");
			}
			return true; // 返回 true 继续执行原方法，返回 false 则跳过原方法
		}

		[HarmonyPostfix]
		public static void Postfix(Player __instance, PhysicalObject obj, ref Player.ObjectGrabability __result)
		{
			if (__instance == null || obj == null) return;

			try
			{
				Log.LogInfo($"Postfix 尝试抓取 {obj.GetType()}");

				if (obj is Spear)
				{
					__result = Player.ObjectGrabability.BigOneHand;
				}
				if (obj is Rock)
				{
					__result = Player.ObjectGrabability.BigOneHand;
				}
			}
			catch (Exception e)
			{
				Log.LogError($"抓取判定补丁出错: {e.Message}");
			}
		}
	}

	// 挂载：Harmony.CreateAndPatchAll(typeof(Patch_Grabability));

	[HarmonyPatch(typeof(Player), "Grabability")] // 私有方法直接用字符串名字
	[HarmonyPriority(200)]
	public static class Patch_Grabability2
	{
		[HarmonyPrefix]
		public static bool Prefix(Player __instance, PhysicalObject obj, ref Player.ObjectGrabability __result)
		{
			if (__instance == null || obj == null) return true;

			try
			{
				Log.LogInfo($"Prefix 尝试抓取 {obj.GetType()}");

				if (obj is Spear)
				{
					__result = Player.ObjectGrabability.CantGrab;
				}
				if (obj is Rock)
				{
					__result = Player.ObjectGrabability.CantGrab;
					return false; // 跳过原方法，但是仍然会执行 Postfix
				}
			}
			catch (Exception e)
			{
				Log.LogError($"抓取判定补丁出错: {e.Message}");
			}
			return true; // 返回 true 继续执行原方法，返回 false 则跳过原方法
		}

		[HarmonyPostfix]
		public static void Postfix(Player __instance, PhysicalObject obj, ref Player.ObjectGrabability __result)
		{
			if (__instance == null || obj == null) return;

			try
			{
				Log.LogInfo($"Postfix 尝试抓取 {obj.GetType()}");

				if (obj is Spear)
				{
					__result = Player.ObjectGrabability.OneHand;
				}
				if (obj is Rock)
				{
					__result = Player.ObjectGrabability.OneHand;
				}
			}
			catch (Exception e)
			{
				Log.LogError($"抓取判定补丁出错: {e.Message}");
			}
		}
	}

	[HarmonyPatch(typeof(Player), "Grabability")] // 私有方法直接用字符串名字
	[HarmonyPriority(100)]
	public static class Patch_Grabability3
	{
		[HarmonyPrefix]
		public static bool Prefix(Player __instance, PhysicalObject obj, ref Player.ObjectGrabability __result)
		{
			if (__instance == null || obj == null) return true;

			try
			{
				Log.LogInfo($"Prefix 尝试抓取 {obj.GetType()}");

				if (obj is Spear)
				{
					__result = Player.ObjectGrabability.CantGrab;
				}
				if (obj is Rock)
				{
					__result = Player.ObjectGrabability.CantGrab;
					return false; // 跳过原方法，但是仍然会执行 Postfix
				}
			}
			catch (Exception e)
			{
				Log.LogError($"抓取判定补丁出错: {e.Message}");
			}
			return true; // 返回 true 继续执行原方法，返回 false 则跳过原方法
		}

		[HarmonyPostfix]
		public static void Postfix(Player __instance, PhysicalObject obj, ref Player.ObjectGrabability __result)
		{
			if (__instance == null || obj == null) return;

			try
			{
				Log.LogInfo($"Postfix 尝试抓取 {obj.GetType()}");

				if (obj is Spear)
				{
					__result = Player.ObjectGrabability.Drag;
				}
				if (obj is Rock)
				{
					__result = Player.ObjectGrabability.Drag;
				}
			}
			catch (Exception e)
			{
				Log.LogError($"抓取判定补丁出错: {e.Message}");
			}
		}
	}


	[HarmonyPatch(typeof(Player), "AddFood")]
	[HarmonyPriority(200)]
	public static class Patch_AddFood_Transpiler
	{
		// 静态日志锁（防止高频刷屏，但 Release 下用 #if DEBUG 屏蔽）
		private static bool _hasLogged = false;

		// 核心修改方法（静态，供 IL 注入调用）
		private static int ModifyMaxFood(int originalMaxFood, Player self)
		{
#if DEBUG
			if (!_hasLogged)
			{
				_hasLogged = true;
				Log.LogInfo($"[Transpiler] 胃容量上限: {originalMaxFood} -> {originalMaxFood - 2}");
			}
#endif

			// 核心逻辑：将最大胃容量永久减少 2
			return originalMaxFood - 2;
		}

		[HarmonyTranspiler]
		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			// 1. 转为可修改的列表
			var codes = new List<CodeInstruction>(instructions);

			// 2. 缓存关键 MethodInfo（避免在循环中重复反射）
			MethodInfo targetMethod = AccessTools.Method(typeof(Player), "get_MaxFoodInStomach");
			MethodInfo modifyMethod = AccessTools.Method(typeof(Patch_AddFood_Transpiler), nameof(ModifyMaxFood));

			if (targetMethod == null || modifyMethod == null)
			{
				Log.LogWarning("[Transpiler] 获取方法失败，Player.get_MaxFoodInStomach 或 ModifyMaxFood 不存在");
				return codes;
			}

			// 3. 遍历查找 call Player::get_MaxFoodInStomach
			for (int i = 0; i < codes.Count; i++)
			{
				if (codes[i].opcode == System.Reflection.Emit.OpCodes.Call &&
					codes[i].operand is MethodInfo mi &&
					mi == targetMethod)
				{
					// 4. 在该指令后插入我们的修改逻辑
					// 此时栈状态：[add 参数, MaxFood(返回值)]
					// 我们需要压入 this，然后调用 ModifyMaxFood(Player, int) 替换栈顶值
					codes.Insert(i + 1, new CodeInstruction(System.Reflection.Emit.OpCodes.Ldarg_0));          // 压入 this
					codes.Insert(i + 2, new CodeInstruction(System.Reflection.Emit.OpCodes.Call, modifyMethod)); // 调用修改方法

					// 修改后栈状态：[add 参数, newMaxFood] 与原 IL 预期完全一致
					break; // 只修改第一个匹配项（实际 AddFood 中只会有一处调用）
				}
			}

			Log.Instance.AppendLogText("Patch_AddFood_Transpiler");
			Log.Instance.AppendLogText(codes.ToString());
			Log.Instance.AppendLogText("1");
			for (int i = 0; i < codes.Count; i++)
			{
				Log.Instance.AppendLogText(codes[i].ToString());
			}
			Log.Instance.AppendLogText("1");

			return codes;
		}
	}

	[HarmonyPatch(typeof(Player), "AddFood")]
	[HarmonyPriority(100)]
	public static class Patch_AddFood_Tra
	{
		// 静态日志锁（防止高频刷屏，但 Release 下用 #if DEBUG 屏蔽）
		private static bool _hasLogged = false;

		// 核心修改方法（静态，供 IL 注入调用）
		private static int ModifyMaxFood0(int originalMaxFood, Player self)
		{
#if DEBUG
			if (!_hasLogged)
			{
				_hasLogged = true;
				Log.LogInfo($"[Transpiler] 胃容量上限: {originalMaxFood} -> {originalMaxFood - 2}");
			}
#endif

			// 核心逻辑：将最大胃容量永久减少 1
			return originalMaxFood - 1;
		}

		[HarmonyTranspiler]
		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			// 1. 转为可修改的列表
			var codes = new List<CodeInstruction>(instructions);

			// 2. 缓存关键 MethodInfo（避免在循环中重复反射）
			MethodInfo targetMethod = AccessTools.Method(typeof(Player), "get_MaxFoodInStomach");
			MethodInfo modifyMethod = AccessTools.Method(typeof(Patch_AddFood_Tra), nameof(ModifyMaxFood0));

			if (targetMethod == null || modifyMethod == null)
			{
				Log.LogWarning("[Tra] 获取方法失败，Player.get_MaxFoodInStomach 或 ModifyMaxFood 不存在");
				return codes;
			}

			// 3. 遍历查找 call Player::get_MaxFoodInStomach
			for (int i = 0; i < codes.Count; i++)
			{
				if (codes[i].opcode == System.Reflection.Emit.OpCodes.Call &&
					codes[i].operand is MethodInfo mi &&
					mi == targetMethod)
				{
					// 4. 在该指令后插入我们的修改逻辑
					// 此时栈状态：[add 参数, MaxFood(返回值)]
					// 我们需要压入 this，然后调用 ModifyMaxFood(Player, int) 替换栈顶值
					codes.Insert(i + 1, new CodeInstruction(System.Reflection.Emit.OpCodes.Ldarg_0));          // 压入 this
					codes.Insert(i + 2, new CodeInstruction(System.Reflection.Emit.OpCodes.Call, modifyMethod)); // 调用修改方法

					// 修改后栈状态：[add 参数, newMaxFood] 与原 IL 预期完全一致
					break; // 只修改第一个匹配项（实际 AddFood 中只会有一处调用）
				}
			}

			Log.Instance.AppendLogText("Patch_AddFood_Tra");
			Log.Instance.AppendLogText(codes.ToString());
			Log.Instance.AppendLogText("1");
			for (int i = 0; i < codes.Count; i++)
			{
				Log.Instance.AppendLogText(codes[i].ToString());
			}
			Log.Instance.AppendLogText("1");

			return codes;
		}
	}
	#endregion

	#region Harmony_AccessTools
	public static class Harmony_AccessTools
	{
		/*
		| AccessTools 方法 | 作用 | 对应聊天内容 |
		| --- | --- | --- |
		| AccessTools.Method(Type, string) | 获取方法（含私有/公有） | 选部分用来挂钩 prefix 时定位方法 |
		| AccessTools.Field(Type, string) | 获取字段 | 访问其他模组的私有变量 |
		| AccessTools.Property(Type, string) | 获取属性（getter/setter） | 访问带 { get; set; } 的属性 |
		| AccessTools.Constructor(Type, Type[]) | 获取构造函数 | 动态实例化私有类 |
		| AccessTools.TypeByName(string) | 通过字符串全名找类型 | 跨程序集联动（不用引用对方DLL） |
		*/

		// 1. 缓存 MethodInfo，避免每次调用都反射查找（性能优化关键）
		private static readonly MethodInfo _grababilityMethod =
			AccessTools.Method(typeof(Player), "Grabability", new Type[] { typeof(PhysicalObject) });

		// 2. 封装一个公开的静态方法供调用
		public static Player.ObjectGrabability? GetGrabability(Player player, PhysicalObject obj)
		{
			if (player == null || obj == null)
				return null;

			// 调用私有方法并强制转换返回值
			return (Player.ObjectGrabability)_grababilityMethod.Invoke(player, new object[] { obj });
		}
	}
	#endregion
	#region 反射
	public static class 反射
	{
		private static MethodInfo? _grababilityMethod;

		public static Player.ObjectGrabability GetGrabability(Player player, PhysicalObject obj)
		{
			if (_grababilityMethod == null)
			{
				// 第一次调用时查找并缓存
				_grababilityMethod = typeof(Player).GetMethod("Grabability",
					BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
			}

			return (Player.ObjectGrabability)_grababilityMethod.Invoke(player, new object[] { obj });
		}
	}
	#endregion
	#region 委托
	public static class 委托
	{
		// 定义一个匹配原方法签名的委托类型
		private delegate Player.ObjectGrabability GrababilityDelegate(Player player, PhysicalObject obj);

		private static GrababilityDelegate? _grababilityDelegate;

		public static Player.ObjectGrabability GetGrabability(Player player, PhysicalObject obj)
		{
			if (_grababilityDelegate == null)
			{
				MethodInfo method = typeof(Player).GetMethod("Grabability",
					BindingFlags.NonPublic | BindingFlags.Instance);

				// 创建强类型委托（零反射开销）
				_grababilityDelegate = (GrababilityDelegate)Delegate.CreateDelegate(
					typeof(GrababilityDelegate), method);
			}

			return _grababilityDelegate(player, obj);
		}
	}
	#endregion


	#region On 钩子
	public void OnEnable_()
	{
		On.Player.CanBeSwallowed += On_Player_CanBeSwallowed;
		IL.Player.CanBeSwallowed += IL_Player_CanBeSwallowed;

		// 插件 A
		On.Player.Update += (orig, self, eu) => {
			orig(self, eu);
			UnityEngine.Debug.Log("来自插件 A 的日志");
		};

		// 插件 B（稍后加载）
		On.Player.Update += (orig, self, eu) => {
			orig(self, eu);
			UnityEngine.Debug.Log("来自插件 B 的日志");
		};

		//[Info: Unity Log] 来自插件 A 的日志
		//[Info: Unity Log] 来自插件 B 的日志
		//[Info: Unity Log] 来自插件 A 的日志
		//[Info: Unity Log] 来自插件 B 的日志...

	}

	public static bool On_Player_CanBeSwallowed(On.Player.orig_CanBeSwallowed orig, Player player, PhysicalObject testObj)
	{
		if (testObj is Rock) return true;

		return orig(player, testObj);
	}

	public static void IL_Player_CanBeSwallowed(ILContext il) // 随便写的
	{
		ILCursor c = new ILCursor(il)
		{
			Index = 17
		};

		ILLabel? proceedCond = c.Prev.Operand as ILLabel;

		c.Goto(0);

		c.Emit(OpCodes.Ldarg_0);
		c.Emit(OpCodes.Ldarg_1);
		c.EmitDelegate<Func<Player, PhysicalObject, bool>> ((player, testObj) =>
		{
			if (testObj is Rock)
			{
				return true;
			}
			return false;
		});

		c.Emit(OpCodes.Brtrue, proceedCond);
	}
	#endregion
	#region Harmony
	// 1. 定义补丁类
	//[HarmonyPatch(typeof(Player), "CanBeSwallowed")] // 定位目标类和方法
	//public static class Player_CanBeSwallowed_Patch
	//{
	//	// 2. Prefix：在原方法执行前运行
	//	//    __instance 指 Player 实例，ref int damage 允许修改传入的参数
	//	static bool Prefix(Player __instance, PhysicalObject testObj, ref bool __result)
	//	{
	//		if (testObj is Rock)
	//		{
	//			__result = true;
	//			return false; // 跳过原方法
	//		}
	//		return true;
	//	}

	//	// 也可以写 Postfix（执行后）或 Transpiler（修改IL中间码）

	//	static bool Postfix(Player __instance, PhysicalObject testObj, ref bool __result)
	//	{
	//		return true;
	//	}

	//	// Transpiler 必须返回 IEnumerable<CodeInstruction>
	//	static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
	//	{
	//		// 1. 转为 List 方便遍历和修改
	//		var codes = new List<CodeInstruction>(instructions);

	//		// 2. 遍历每一条 IL 指令
	//		for (int i = 0; i < codes.Count; i++)
	//		{
	//			CodeInstruction instruction = codes[i];

	//			// 此处省略...
	//			/*if (instruction.opcode == System.Reflection.Emit.OpCodes.Ldc_I4_S && instruction.operand is sbyte val && val == 100)
	//			{
	//				// 4. 替换指令：改成加载常量 200
	//				codes[i] = new CodeInstruction(System.Reflection.Emit.OpCodes.Ldc_I4_S, (sbyte)200);
	//				break; // 改完退出循环（当然如果多处硬编码，可以不 break）
	//			}*/
	//		}

	//		// 5. 返回修改后的 IL 指令集
	//		return codes;
	//	}

	//	// 用 CodeMatcher 改写上面的例子（更稳健）
	//	static void Transpiler(CodeMatcher matcher)
	//	{
	//		matcher.MatchForward(false,
	//			new CodeMatch(System.Reflection.Emit.OpCodes.Ldc_I4_S, 100) // 查找 100
	//		).SetOperandAndAdvance(200); // 改成 200
	//	}

	//	static void A()
	//	{
	//		// 在模组加载时执行一次：
	//		Harmony.CreateAndPatchAll(typeof(Player_CanBeSwallowed_Patch));
	//	}
	//}
	#endregion
	#region MonoMod.RuntimeDetour
	public class MyModLoader
	{
		private Hook? _swallowHook;

		public void LoadHooks()
		{
			// 1. 通过反射拿到目标方法
			MethodInfo targetMethod = typeof(Player).GetMethod("CanBeSwallowed",
				BindingFlags.Public | BindingFlags.Instance);

			// 2. 定义钩子委托（注意委托签名：必须包含原方法 + 原参数）
			//    这里的 orig 代表原始方法的调用入口
			Func<Func<Player, PhysicalObject, bool>, Player, PhysicalObject, bool> hookDelegate = (orig, self, testObj) =>
			{
				// 修改逻辑
				if (testObj is Rock) return true;

				// 调用原方法（注意这里的调用方式和 Harmony 的 Prefix 不同）
				return orig(self, testObj);
			};
			//这里的Func是指有返回值的委托，泛型实参的最后一个会默认为返回值。如果只有一个泛型实参，则为有一个返回值没有参数的委托。
			//如果需要无返回值的话，请使用Action类型的委托。 !!!

			// 3. 创建钩子
			_swallowHook = new Hook(targetMethod, hookDelegate);
		}

		public void UnloadHooks()
		{
			// 卸载钩子，恢复原样
			_swallowHook?.Dispose();
		}
	}
	#endregion
	#region AccessTools
	/*
	| AccessTools 方法 | 作用 | 对应聊天内容 |
	| --- | --- | --- |
	| AccessTools.Method(Type, string) | 获取方法（含私有/公有） | 选部分用来挂钩 prefix 时定位方法 |
	| AccessTools.Field(Type, string) | 获取字段 | 访问其他模组的私有变量 |
	| AccessTools.Property(Type, string) | 获取属性（getter/setter） | 访问带 { get; set; } 的属性 |
	| AccessTools.Constructor(Type, Type[]) | 获取构造函数 | 动态实例化私有类 |
	| AccessTools.TypeByName(string) | 通过字符串全名找类型 | 跨程序集联动（不用引用对方DLL） |
	*/

	// 定义一个静态委托（只需初始化一次）
	//static readonly AccessTools.FieldRef<Player, int> HealthRef =
	//	AccessTools.FieldRefAccess<Player, int>("_health");
	public static string AccessTools_(Player player)
	{
		// ❌ 原生反射（又臭又长）
		FieldInfo fi = typeof(Player).GetField("_health",
			BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
		int health = (int)fi.GetValue(player);

		// ✅ Harmony AccessTools（清爽简洁）
		int health_ = (int)AccessTools.Field(typeof(Player), "_health").GetValue(player);

		// 在游戏循环里高频调用（极快，无反射损耗）
		//int currentHP = HealthRef(player);

		// 直接挂钩其他模组的私有方法，名字用字符串传
		/*
		Harmony.Patch(
			AccessTools.Method("OtherModNamespace.OtherClass, OtherModAssembly", "PrivateMethod"),
			prefix: new HarmonyMethod(typeof(MyPatch), nameof(MyPatch.Prefix))
		);
		*/

		return "反编译";
	}
	#endregion
	#region 反射 + 委托
	public static class LegacyHook
	{
		private static Player? _targetPlayer; // 假设需要实例
		private static MethodInfo? _originalMethod;
		private static Delegate? _hookDelegate;

		public static void Hook()
		{
			// 1. 疯狂的反射获取私有/公有方法（甚至要跨程序集 BindingFlags）
			_originalMethod = typeof(Player).GetMethod("CanBeSwallowed",
				BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

			// 2. 定义一个匹配原方法签名的委托（用来调用原函数）
			Func<Player, PhysicalObject, bool> originalAction = (player, testObj) =>
			{
				if (testObj is Rock) return true;

				// 这里需要 Invoke，性能差且容易抛异常
				return (bool)_originalMethod.Invoke(player, new object[] { testObj });
			};

			// 3. 想要拦截？没有现成的Hook机制，只能自己用 GetMethod 替换，或者
			//    直接重写委托逻辑覆盖掉原有的委托字段（如果游戏用了委托回调）。
			//    （绝大多数情况根本做不到无侵入拦截，非常鸡肋）
			Console.WriteLine("此方法极难实现无侵入拦截，通常只能用于调用，而非修改!");
		}
	}
	#endregion
	#region Public程序集
	/*
针对你关心的 “雨甸（Rain Meadow）频繁更新导致手动维护Public程序集太累” 这个问题，我直接给你两套 MSBuild PreBuildTask 脚本方案。

第一套是社区标准方案（推荐，零维护成本），第二套是纯手工 Exec 方案（不用装 NuGet，完全控制权）。
方案一：使用 BepInEx.Publicizer（最推荐，行业标准）

这是目前 BepInEx 模组社区的事实标准。它会在编译前自动读取你指定的原始 DLL，生成 Publicized 版本放到 obj 目录，并自动帮你加上 InternalsVisibleTo 特性，完全不需要你手动干预。

在你的 .csproj 项目文件中，替换或追加以下代码：
xml

<Project Sdk="Microsoft.NET.Sdk">

  <!-- 1. 定义游戏路径（方便管理和切换版本） -->
  <PropertyGroup>
	<GameDir>D:\Steam\steamapps\common\RainWorld\</GameDir>
	<ManagedDir>$(GameDir)RainWorld_Data\Managed\</ManagedDir>
  </PropertyGroup>

  <!-- 2. 引入 Publicizer NuGet 包（仅编译时使用，不会打包进你的模组） -->
  <ItemGroup>
	<PackageReference Include="BepInEx.Publicizer" Version="1.0.1" PrivateAssets="all" />
  </ItemGroup>

  <!-- 3. 指定需要“私有变公有”的 DLL -->
  <ItemGroup>
	<!-- 注意：Private="False" 代表让公共化后的版本覆盖掉原始引用 -->
	<Publicize Include="$(ManagedDir)Assembly-CSharp.dll" Private="False" />
	<Publicize Include="$(ManagedDir)UnityEngine.dll" Private="False" />
	<!-- 如果是联动的其他模组，例如雨甸的 Mod，也可以直接加进来 -->
	<Publicize Include="$(ManagedDir)RainMeadow.dll" Private="False" />
  </ItemGroup>

  <!-- 4. （可选）如果你想在编译前在输出栏看到确认信息 -->
  <Target Name="CheckPublicize" BeforeTargets="PreBuildEvent">
	<Message Text="[Publicizer] 正在为最新游戏版本生成公共化程序集..." Importance="high" />
  </Target>

</Project>

它的运作逻辑：

	当你点击 Visual Studio 的“生成”或执行 dotnet build 时，BepInEx.Publicizer 会在 PreBuild 阶段 自动抓取 $(ManagedDir) 里的原始 DLL。

	它会把所有 private / internal 改成 public，并自动注入 [assembly: InternalsVisibleTo("你的模组名")]。

	输出文件默认在 obj\PublicizedAssemblies\ 下，你的项目引用会自动指向这个输出，无需额外配置。

方案二：纯 MSBuild 手工脚本（不依赖 NuGet，适用于特殊环境）

如果你因为公司内网、网络限制，或者想完全掌控每一步，可以使用 Exec 任务 配合命令行工具（如 Publicizer.exe 或自己写的小工具）。

	你需要先下载一个命令行 publicizer 工具（如 CabbageRoth/Publicizer 的 CLI 版本）放到项目根目录的 Tools\ 文件夹下。

xml

<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
	<GameDir>D:\Steam\steamapps\common\RainWorld\</GameDir>
	<ManagedDir>$(GameDir)RainWorld_Data\Managed\</ManagedDir>
	<PublicizedOutput>$(ProjectDir)PublicizedAssemblies\</PublicizedOutput>
	<PublicizerExe>$(ProjectDir)Tools\publicizer.exe</PublicizerExe>
  </PropertyGroup>

  <!-- 核心任务：在编译前执行 Publicizer 命令行 -->
  <Target Name="PreBuildPublicize" BeforeTargets="PreBuildEvent">
	<!-- 创建输出文件夹 -->
	<MakeDir Directories="$(PublicizedOutput)" Condition="!Exists('$(PublicizedOutput)')" />
	
	<!-- 对 Assembly-CSharp 执行反私有化 -->
	<Exec Command="&quot;$(PublicizerExe)&quot; &quot;$(ManagedDir)Assembly-CSharp.dll&quot; -o &quot;$(PublicizedOutput)Assembly-CSharp-publicized.dll&quot;" />
	
	<!-- 对雨甸模组执行反私有化（如果它在游戏目录里） -->
	<Exec Command="&quot;$(PublicizerExe)&quot; &quot;$(ManagedDir)RainMeadow.dll&quot; -o &quot;$(PublicizedOutput)RainMeadow-publicized.dll&quot;" />
	
	<Message Text="[PreBuild] Publicized DLL 已生成: $(PublicizedOutput)" Importance="high" />
  </Target>

  <!-- 关键步骤：手动将生成的 Publicized 文件添加为引用，替代原始 DLL -->
  <ItemGroup>
	<Reference Include="Assembly-CSharp-publicized">
	  <HintPath>$(PublicizedOutput)Assembly-CSharp-publicized.dll</HintPath>
	  <Private>False</Private> <!-- 不复制到输出目录，避免覆盖游戏原版 -->
	</Reference>
	<Reference Include="RainMeadow-publicized">
	  <HintPath>$(PublicizedOutput)RainMeadow-publicized.dll</HintPath>
	  <Private>False</Private>
	</Reference>
  </ItemGroup>

</Project>

关于“雨甸（Rain Meadow）三天一小更”的对策

这两种方案都能完美应对频繁更新：

	方案一（NuGet）：每次编译时，Publicizer 都会重新读取游戏目录下最新的 DLL。你完全不需要手动替换文件，只要 Steam 更新了雨甸，你下一次编译就自动适配新版本的私有字段。

	方案二（手工）：Exec 命令里没有写死版本号，直接指向 RainMeadow.dll，同样每次编译都重新生成。

解决群里提到的“访问权限检测”报错（关键属性）

如果你用了上述脚本，但仍然报 FieldAccessException，说明 Publicized 程序集缺少 InternalsVisibleTo 特性。

在方案一中，BepInEx.Publicizer 默认会自动添加该特性，无需额外操作。

在方案二中，如果你用的 CLI 工具不支持自动注入，你需要在你的模组主项目的 AssemblyInfo.cs（或 Properties\AssemblyInfo.cs）中手动声明：
csharp

// 告诉游戏和雨甸模组，允许我的模组访问你们的私有成员
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("我的模组项目名")]

注意：这个声明需要双方都认。如果你的项目叫 MyRainWorldMod，就在你的项目里写上 InternalsVisibleTo("MyRainWorldMod")。严格来说，应该是目标程序集（雨甸）允许你访问，但既然我们生成了 Publicized 版本，相当于我们在编译期“欺骗”了编译器，这个特性写在你的程序集里可以绕开运行时的安全检查。
如果你嫌配置麻烦，最简化的终极方案（复制即用）

直接把这段粘贴到你的 .csproj 文件末尾（</Project> 之前）：
xml

  <!-- 极简 PreBuild：每次编译前强制重新生成 Public DLL -->
  <Target Name="PreBuild" BeforeTargets="PreBuildEvent">
	<Exec Command="dotnet tool install -g BepInEx.Publicizer.Cli || true" IgnoreExitCode="true" />
	<Exec Command="publicizer &quot;$(GameDir)RainWorld_Data\Managed\Assembly-CSharp.dll&quot; -o &quot;$(ProjectDir)Publicized\&quot;" />
	<Exec Command="publicizer &quot;$(GameDir)RainWorld_Data\Managed\RainMeadow.dll&quot; -o &quot;$(ProjectDir)Publicized\&quot;" />
  </Target>

（依赖 .NET Core Global Tool，适合喜欢命令行的开发者）

总结建议：直接上 方案一（BepInEx.Publicizer），它已经是 BepInEx 官方模板的一部分，群聊里 Nop 提到的 PUBLIC-assembly-csharp.dll 就是这种工具生成的，你只需要配置一次，以后每次编译都自动完成，彻底告别“反射地狱”和“手动更新公版 DLL”的烦恼。如果编译时遇到 NuGet 源不通的问题，再考虑切换方案二。

	*/
	#endregion
	//
	#endregion

	#region Hooks

	/// <summary>标记一个方法为钩子处理函数</summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public class HookAttribute : Attribute
	{
		public string HookName { get; }          // "Player.Update" 等
		public int Priority { get; set; } = 0;
		public HookPhase Phase { get; set; } = HookPhase.Before;

		public HookAttribute(string hookName) => HookName = hookName;
	}
	public enum HookPhase { Before, After }

	/// <summary>钩子上下文，在 Before/After 间共享数据</summary>
	public class HookContext
	{
		public bool Block { get; set; }          // 阻断 orig 及后续
		public int BlockingPriority { get; set; } // 由框架自动记录
		public object? Result { get; set; }       // 存放/修改返回值
		public Dictionary<string, object> Data { get; } = [];
	}

	/// <summary>存储每个 HookName 对应的原始方法签名信息</summary>
	public class HookSignature
	{
		public Type OrigDelegateType { get; }   // 例如 On.Player.orig_Update
		public Type[] ArgTypes { get; }         // 除 orig 外的参数类型
		public Type ReturnType { get; }         // 返回值类型（void 为 null）
		public Type EventContainerType { get; }        // 如 On.Player
		public string EventName { get; }               // 如 "Update"

		public HookSignature(Type origDelegateType, Type[] argTypes, Type returnType, Type eventContainerType, string eventName)
		{
			OrigDelegateType = origDelegateType;
			ArgTypes = argTypes;
			ReturnType = returnType;
			EventContainerType = eventContainerType;
			EventName = eventName;
		}
	}

	public static class HookRegistry
	{
		public static readonly Dictionary<string, HookSignature> Signatures = new()
		{
			// ===== Player =====
			["Player.ctor"] = new(
				typeof(On.Player.orig_ctor),
				new[] { typeof(AbstractCreature), typeof(World) },
				typeof(void),
				typeof(On.Player),
				"ctor"
			),
			["Player.Update"] = new(
				typeof(On.Player.orig_Update),
				new[] { typeof(Player), typeof(bool) },
				typeof(void),
				typeof(On.Player),
				"Update"
			),
			["Player.checkInput"] = new(
				typeof(On.Player.orig_checkInput),
				new[] { typeof(Player) },
				typeof(void),
				typeof(On.Player),
				"checkInput"
			),
			["Player.MovementUpdate"] = new(
				typeof(On.Player.orig_MovementUpdate),
				new[] { typeof(Player), typeof(bool) },
				typeof(void),
				typeof(On.Player),
				"MovementUpdate"
			),
			["Player.SwallowObject"] = new(
				typeof(On.Player.orig_SwallowObject),
				new[] { typeof(Player), typeof(PhysicalObject) },
				typeof(bool),
				typeof(On.Player),
				"SwallowObject"
			),
			["Player.ThrownSpear"] = new(
				typeof(On.Player.orig_ThrownSpear),
				new[] { typeof(Player), typeof(Spear) },
				typeof(void),
				typeof(On.Player),
				"ThrownSpear"
			),
			["Player.Die"] = new(
				typeof(On.Player.orig_Die),
				new[] { typeof(Player) },
				typeof(void),
				typeof(On.Player),
				"Die"
			),
			["Player.Destroy"] = new(
				typeof(On.Player.orig_Destroy),
				new[] { typeof(Player) },
				typeof(void),
				typeof(On.Player),
				"Destroy"
			),

			// ===== PlayerGraphics =====
			["PlayerGraphics.InitiateSprites"] = new(
				typeof(On.PlayerGraphics.orig_InitiateSprites),
				new[] { typeof(PlayerGraphics), typeof(RoomCamera.SpriteLeaser), typeof(RoomCamera) },
				typeof(void),
				typeof(On.PlayerGraphics),
				"InitiateSprites"
			),
			["PlayerGraphics.DrawSprites"] = new(
				typeof(On.PlayerGraphics.orig_DrawSprites),
				new[] { typeof(PlayerGraphics), typeof(RoomCamera.SpriteLeaser), typeof(RoomCamera), typeof(float), typeof(Vector2) },
				typeof(void),
				typeof(On.PlayerGraphics),
				"DrawSprites"
			),
			["PlayerGraphics.AddToContainer"] = new(
				typeof(On.PlayerGraphics.orig_AddToContainer),
				new[] { typeof(PlayerGraphics), typeof(RoomCamera.SpriteLeaser), typeof(RoomCamera), typeof(FContainer) },
				typeof(void),
				typeof(On.PlayerGraphics),
				"AddToContainer"
			),

			// ===== SlugcatStats =====
			["SlugcatStats.ctor"] = new(
				typeof(On.SlugcatStats.orig_ctor),
				new[] { typeof(SlugcatStats), typeof(SlugcatStats.Name), typeof(bool) },
				typeof(void),
				typeof(On.SlugcatStats),
				"ctor"
			),

			// ===== Creature =====
			["Creature.Update"] = new(
				typeof(On.Creature.orig_Update),
				new[] { typeof(Creature), typeof(bool) },
				typeof(void),
				typeof(On.Creature),
				"Update"
			),
			["Creature.Die"] = new(
				typeof(On.Creature.orig_Die),
				new[] { typeof(Creature) },
				typeof(void),
				typeof(On.Creature),
				"Die"
			),

			// ===== Abstract =====
			//["AbstractCreature.ctor"] = new(
			//    typeof(On.AbstractCreature.orig_ctor),
			//    new[] { typeof(AbstractCreature), typeof(World), typeof(AbstractCreature.CreatureTemplate), typeof(WorldCoordinate), typeof(EntityID) },
			//    typeof(void),
			//    typeof(On.AbstractCreature),
			//    "ctor"
			//),
			["AbstractPhysicalObject.ctor"] = new(
				typeof(On.AbstractPhysicalObject.orig_ctor),
				new[] { typeof(AbstractPhysicalObject), typeof(World), typeof(AbstractPhysicalObject.AbstractObjectType), typeof(WorldCoordinate), typeof(EntityID) },
				typeof(void),
				typeof(On.AbstractPhysicalObject),
				"ctor"
			),
			["AbstractPhysicalObject.Destroy"] = new(
				typeof(On.AbstractPhysicalObject.orig_Destroy),
				new[] { typeof(AbstractPhysicalObject) },
				typeof(void),
				typeof(On.AbstractPhysicalObject),
				"Destroy"
			),

			// ===== Weapon =====
			["Spear.HitSomething"] = new(
				typeof(On.Spear.orig_HitSomething),
				new[] { typeof(Spear), typeof(SharedPhysics.CollisionResult), typeof(bool) },
				typeof(bool),
				typeof(On.Spear),
				"HitSomething"
			),
			["Spear.SetRandomSpin"] = new(
				typeof(On.Spear.orig_SetRandomSpin),
				new[] { typeof(Spear) },
				typeof(void),
				typeof(On.Spear),
				"SetRandomSpin"
			),
			["Rock.HitSomething"] = new(
				typeof(On.Rock.orig_HitSomething),
				new[] { typeof(Rock), typeof(SharedPhysics.CollisionResult), typeof(bool) },
				typeof(bool),
				typeof(On.Rock),
				"HitSomething"
			),
			["Weapon.HitSomething"] = new(
				typeof(On.Weapon.orig_HitSomething),
				new[] { typeof(Weapon), typeof(SharedPhysics.CollisionResult), typeof(bool) },
				typeof(bool),
				typeof(On.Weapon),
				"HitSomething"
			),
			["Weapon.SetRandomSpin"] = new(
				typeof(On.Weapon.orig_SetRandomSpin),
				new[] { typeof(Weapon) },
				typeof(void),
				typeof(On.Weapon),
				"SetRandomSpin"
			),
			["Weapon.Update"] = new(
				typeof(On.Weapon.orig_Update),
				new[] { typeof(Weapon), typeof(bool) },
				typeof(void),
				typeof(On.Weapon),
				"Update"
			),
			["Weapon.HitAnotherThrownWeapon"] = new(
				typeof(On.Weapon.orig_HitAnotherThrownWeapon),
				new[] { typeof(Weapon), typeof(Weapon) },
				typeof(bool),
				typeof(On.Weapon),
				"HitAnotherThrownWeapon"
			),
		};
	}

	public static class HookAutoInstaller
	{
		private static bool _installed;
		private static readonly List<Delegate> _generatedHooks = []; // 保存引用避免被 GC

		public static void Install()
		{
			if (_installed) return;
			_installed = true;

			// 1. 扫描所有程序集中的静态方法（可按需限定命名空间）
			var methods = AppDomain.CurrentDomain.GetAssemblies()
				.Where(a => !a.IsDynamic)
				.SelectMany(a => a.GetTypes())
				.SelectMany(t => t.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
				.Where(m => m.GetCustomAttribute<HookAttribute>() != null)
				.ToList();

			// 2. 按 HookName 分组
			var groups = methods.GroupBy(m => m.GetCustomAttribute<HookAttribute>().HookName);

			foreach (var group in groups)
			{
				var hookName = group.Key;
				if (!HookRegistry.Signatures.TryGetValue(hookName, out var sig))
				{
					LogError($"未找到 Hook 签名: {hookName}，请先在 HookRegistry 中登记");
					continue;
				}

				// 分别取出 Before 和 After（按优先级升序）
				var beforeMethods = group
					.Where(m => m.GetCustomAttribute<HookAttribute>().Phase == HookPhase.Before)
					.OrderBy(m => m.GetCustomAttribute<HookAttribute>().Priority)
					.ToArray();

				var afterMethods = group
					.Where(m => m.GetCustomAttribute<HookAttribute>().Phase == HookPhase.After)
					.OrderBy(m => m.GetCustomAttribute<HookAttribute>().Priority)
					.ToArray();

				// 生成并挂载中央钩子
				var hookDelegate = BuildCentralHook(sig, beforeMethods, afterMethods);
				if (hookDelegate != null)
				{
					_generatedHooks.Add(hookDelegate);
					// 挂载到事件
					var eventInfo = sig.EventContainerType.GetEvent(sig.EventName, BindingFlags.Public | BindingFlags.Static);
					if (eventInfo == null)
					{
						Debug.LogError($"[HookInstaller] 未找到事件 {sig.EventContainerType.FullName}.{sig.EventName}");
						continue;
					}
					eventInfo.AddEventHandler(null, hookDelegate);
					Debug.Log($"[HookInstaller] 自动挂载钩子: {hookName}");

					//_generatedHooks.Add(hookDelegate); // 防止被 GC
					//HookRegistry.Signatures[hookName] = sig; // 存储引用（可选）
				}
			}
		}

		private static Delegate BuildCentralHook(HookSignature sig, MethodInfo[] beforeMethods, MethodInfo[] afterMethods)
		{
			// 参数：orig 委托 + 原方法所有参数
			var origParam = Expression.Parameter(sig.OrigDelegateType, "orig");
			var argParams = sig.ArgTypes.Select(t => Expression.Parameter(t, $"p_{t.Name}")).ToArray();

			var ctxVar = Expression.Variable(typeof(HookContext), "ctx");
			var resultVar = Expression.Variable(typeof(object), "result");

			var exprs = new List<Expression>
			{
				// 1. 创建上下文
				Expression.Assign(ctxVar, Expression.New(typeof(HookContext)))
			};


			var newExprs = new List<Expression>
			{
				Expression.Assign(ctxVar, Expression.New(typeof(HookContext)))
			};

			// Before loop with break
			var beforeBreakLabel = Expression.Label();
			foreach (var method in beforeMethods)
			{
				var callArgs = argParams.Cast<Expression>().Append(ctxVar).ToArray();
				newExprs.Add(Expression.Call(method, callArgs));
				newExprs.Add(Expression.IfThen(
					Expression.PropertyOrField(ctxVar, "Block"),
					Expression.Break(beforeBreakLabel)
				));
			}
			newExprs.Add(Expression.Label(beforeBreakLabel));

			// Orig call (only if !Block)
			// 3. 调用 orig（条件：!Block）
			var origCallArgs = argParams.Cast<Expression>().ToArray();
			var origCall = Expression.Invoke(origParam, origCallArgs);
			bool hasReturn = sig.ReturnType != typeof(void);

			var origCallExpr = hasReturn
				? (Expression)Expression.Assign(resultVar, Expression.Convert(origCall, typeof(object)))
				: (Expression)origCall;
			var ifNotBlock = Expression.IfThen(
				Expression.Equal(Expression.PropertyOrField(ctxVar, "Block"), Expression.Constant(false)),
				origCallExpr
			);
			newExprs.Add(ifNotBlock);

			if (hasReturn)
			{
				newExprs.Add(Expression.Assign(Expression.PropertyOrField(ctxVar, "Result"), resultVar));
			}

			// After loop (only if !Block, but also check Block after each)
			var afterBreakLabel = Expression.Label();
			foreach (var method in afterMethods)
			{
				var callArgs = argParams.Cast<Expression>().Append(ctxVar).ToArray();
				var ifNotBlocked = Expression.IfThen(
					Expression.Equal(Expression.PropertyOrField(ctxVar, "Block"), Expression.Constant(false)),
					Expression.Call(method, callArgs)
				);
				newExprs.Add(ifNotBlocked);
				// 如果 After 中设置 Block，则跳出后续 After
				newExprs.Add(Expression.IfThen(
					Expression.PropertyOrField(ctxVar, "Block"),
					Expression.Break(afterBreakLabel)
				));
			}
			newExprs.Add(Expression.Label(afterBreakLabel));

			// 返回值
			if (hasReturn)
			{
				var returnExpr = Expression.Convert(Expression.PropertyOrField(ctxVar, "Result"), sig.ReturnType);
				newExprs.Add(returnExpr);
			}
			else
			{
				newExprs.Add(Expression.Empty());
			}

			var variables = new List<ParameterExpression> { ctxVar };
			if (hasReturn) variables.Add(resultVar);

			var body = Expression.Block(variables, newExprs);

			// 构建委托类型
			var allParamTypes = new[] { sig.OrigDelegateType }.Concat(sig.ArgTypes).ToArray();
			Type delegateType;
			if (hasReturn)
			{
				delegateType = Expression.GetFuncType(allParamTypes.Concat(new[] { sig.ReturnType }).ToArray());
			}
			else
			{
				delegateType = Expression.GetActionType(allParamTypes);
			}

			var lambda = Expression.Lambda(delegateType, body, new[] { origParam }.Concat(argParams));
			return lambda.Compile();
		}

		// 辅助：获取 On 类中的事件信息（如 On.Player.Update）
		private static EventInfo? GetHookEventInfo(HookSignature sig, Type onType)
		{
			// 通过 orig 委托类型推断事件名
			// 例如 On.Player.orig_Update 对应的事件是 On.Player.Update
			var origName = sig.OrigDelegateType.Name;
			if (origName.StartsWith("orig_"))
				origName = origName.Substring(5);

			// 遍历 On 类的嵌套类型
			foreach (var nested in onType.GetNestedTypes(BindingFlags.Public | BindingFlags.Static))
			{
				var evt = nested.GetEvent(origName, BindingFlags.Public | BindingFlags.Static);
				if (evt != null)
				{
					// 检查委托类型是否匹配
					if (evt.EventHandlerType == sig.OrigDelegateType)
						return evt;
					// 有些事件可能用 Action/Func 包装，我们直接通过名称匹配
					return evt;
				}
			}
			return null;
		}

		private static void LogDebug(string msg) => Debug.Log($"[HookInstaller] {msg}");
		private static void LogError(string msg) => Debug.LogError($"[HookInstaller] {msg}");
	}

	#endregion

	#region IEnumerator
	private static IEnumerator WaitForEndOfFrameCoroutine(TaskCompletionSource<bool> completionSource)
	{
		yield return new WaitForEndOfFrame();
		completionSource.SetResult(true);
	}
	private static Task WaitForEndOfFrameAsync(RainWorld rainWorld)
	{
		TaskCompletionSource<bool> completionSource = new();
		rainWorld.StartCoroutine(WaitForEndOfFrameCoroutine(completionSource));
		return completionSource.Task;
	}
	#endregion

	#region Hook

	//// 调度器
	//public static class HookDispatcher
	//{
	//	// <名称, 程序集>
	//	private static readonly Dictionary<string, List<HandlerEntry>> _handlers = [];

	//	// 函数委托(上下文)
	//	public delegate void HandlerDelegate(HookContext context);
	//	// 程序项
	//	private class HandlerEntry
	//	{
	//		public int Priority { get; }
	//		public HandlerDelegate? Before { get; }
	//		public HandlerDelegate? After { get; }

	//		public HandlerEntry(int priority, HandlerDelegate? before, HandlerDelegate? after)
	//		{
	//			Priority = priority;
	//			Before = before;
	//			After = after;
	//		}
	//	}

	//	// 注册函数对
	//	public static void Register(string hookName, int priority, HandlerDelegate? before, HandlerDelegate? after)
	//	{
	//		lock (_handlers)
	//		{
	//			if (!_handlers.TryGetValue(hookName, out var list))
	//			{
	//				list = [];
	//				_handlers[hookName] = list;
	//			}
	//			list.Add(new HandlerEntry(priority, before, after));
	//			// 按优先级排序（小->大）
	//			list.Sort((a, b) => a.Priority.CompareTo(b.Priority));
	//		}
	//	}


	//	// 调度执行（无返回值）
	//	public static void Dispatch(string hookName, HookContext context, Action<HookContext> origAction)
	//	{
	//		Dispatch<object>(hookName, context, (ctx) =>
	//		{
	//			origAction(ctx);
	//			return null;
	//		});
	//	}

	//	// 调度执行（有返回值）
	//	public static T? Dispatch<T>(string hookName, HookContext context, Func<HookContext, T?> origFunc)
	//	{
	//		if (!_handlers.TryGetValue(hookName, out var entries))
	//		{
	//			return origFunc(context);
	//		}

	//		context.Result = (T?)default;

	//		// 执行 Before
	//		foreach (var entry in entries)
	//		{
	//			if (!context.Block || entry.Priority <= context.BlockingPriority)
	//			{
	//				entry.Before?.Invoke(context);
	//			}
	//			if (context.Block && context.BlockingPriority == null)
	//			{
	//				context.BlockingPriority = entry.Priority;
	//			}
	//		}

	//		// 执行 orig
	//		if (!context.Block)
	//		{
	//			context.Result = origFunc(context);
	//		}

	//		// 执行 After
	//		foreach (var entry in entries)
	//		{
	//			if (!context.Block || entry.Priority <= context.BlockingPriority)
	//			{
	//				entry.After?.Invoke(context);
	//			}
	//			if (context.Block && context.BlockingPriority == null)
	//			{
	//				context.BlockingPriority = entry.Priority;
	//			}
	//		}

	//		return (T?)context.Result;
	//	}

	//	public static void Clear()
	//	{
	//		lock (_handlers)
	//			_handlers.Clear();
	//	}
	//}

	//// 上下文
	//public abstract class HookContext
	//{
	//	public bool Block;
	//	public int? BlockingPriority = null;
	//	public object? Result;
	//	public Dictionary<string, object> Data = [];
	//}

	//// Player.Update 的上下文
	//public class PlayerUpdateContext : HookContext
	//{
	//	public required Player player;
	//	public required bool eu;
	//}
	//private static void Hook_PlayerUpdate(On.Player.orig_Update orig, Player player, bool eu)
	//{
	//	var Context = new PlayerUpdateContext
	//	{
	//		player = player,
	//		eu = eu
	//	};


	//	// 注册二段跳功能（优先级更高，先执行）
	//	HookDispatcher.Register("Player.Update",
	//		priority: 5,
	//		before: (ctx) => { /* ... */ },
	//		after: (ctx) => { /* 恢复状态 */ }
	//	);

	//	// 注册冲刺功能
	//	HookDispatcher.Register("Player.Update",
	//		priority: 10,
	//		before: (ctx) =>
	//		{
	//			if (ctx is PlayerUpdateContext c)
	//			{
	//				if (Input.GetKey(KeyCode.LeftShift))
	//				{
	//					c.player.bodyChunks[0].vel.x = 20f;
	//					// 阻断原逻辑和后续After（如果需要）
	//					c.Block = true;
	//					c.Data["isDashing"] = true; // 供其他部分读取
	//				}
	//			}
	//		},
	//		after: null
	//	);

	//	HookDispatcher.Dispatch("Player.Update", Context, (context) =>
	//	{
	//		var c = (PlayerUpdateContext)context;

	//		orig(c.player, c.eu);
	//	});
	//}

	//// PlayerGraphics.DrawSprites 的上下文
	//public class PlayerGraphicsDrawContext : HookContext
	//{
	//	public required PlayerGraphics playerGraphics;
	//	public required RoomCamera.SpriteLeaser sLeaser;
	//	public required RoomCamera rCam;
	//	public required float timeStacker;
	//	public required Vector2 camPos;
	//}
	//private static void Hook_PlayerGraphicsDraw(On.PlayerGraphics.orig_DrawSprites orig, PlayerGraphics playerGraphics, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
	//{
	//	var Context = new PlayerGraphicsDrawContext
	//	{
	//		playerGraphics = playerGraphics,
	//		sLeaser = sLeaser,
	//		rCam = rCam,
	//		timeStacker = timeStacker,
	//		camPos = camPos
	//	};

	//	HookDispatcher.Dispatch("PlayerGraphics.DrawSprites", Context, (context) =>
	//	{
	//		var c = (PlayerGraphicsDrawContext)context;

	//		orig(c.playerGraphics, c.sLeaser, c.rCam, c.timeStacker, c.camPos);
	//	});
	//}

	//public class SpearHitContext : HookContext
	//{
	//	public required Spear spear;
	//	public required SharedPhysics.CollisionResult result; // 值类型
	//	public required bool eu;
	//}
	//private static bool Hook_SpearHitSomething(On.Spear.orig_HitSomething orig, Spear spear, SharedPhysics.CollisionResult result, bool eu)
	//{
	//	var Context = new SpearHitContext
	//	{
	//		spear = spear,
	//		result = result,
	//		eu = eu
	//	};

	//	return HookDispatcher.Dispatch<bool>("Spear.HitSomething", Context, (context) =>
	//	{
	//		var c = (SpearHitContext)context;

	//		// 调用原方法，将返回值存入上下文
	//		return orig(c.spear, c.result, c.eu);
	//	});
	//}

	//// 挂载所有中央钩子
	//public static void InstallHooks()
	//{
	//	On.Player.Update += Hook_PlayerUpdate;
	//	On.PlayerGraphics.DrawSprites += Hook_PlayerGraphicsDraw;
	//	On.Spear.HitSomething+= Hook_SpearHitSomething;
	//	//On.Player.ctor += Hook_PlayerCtor;      // 类似写法
	//											// ... 其余所有钩子
	//}

	//// 卸载时取消订阅
	//public static void UninstallHooks()
	//{
	//	On.Player.Update -= Hook_PlayerUpdate;
	//	On.PlayerGraphics.DrawSprites -= Hook_PlayerGraphicsDraw;
	//	On.Spear.HitSomething -= Hook_SpearHitSomething;
	//	// ...
	//}

	#endregion

	#region 文件目录
	static string save = UnityEngine.Application.persistentDataPath;
	static string gameRoot = System.AppDomain.CurrentDomain.BaseDirectory;
	private static string? _cachedModRoot;
	static string modRoot
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
			throw new FileNotFoundException("无法找到 modinfo.json");
		}
	}
	#endregion

	#region LINQ & Lambda 表达式
	/*
	LINQ的基本概念
	查询表达式
	查询表达式是一种声明式的编程模型，用于指定需要执行的数据操作。其基本结构如下：

	var query = from source in collection
				where condition
				orderby key
				select result;

	from source in collection：指定查询的数据源。
	where condition：指定筛选条件。
	orderby key：指定排序条件。
	select result：定义查询结果。


	标准查询运算符
	标准查询运算符是一组扩展方法，它们提供了一种函数式的方式来构建查询。这些方法定义在System.Linq.Enumerable类中，适用于所有实现了IEnumerable<T>接口的集合。常见的标准查询运算符包括：

	Where：过滤集合中的元素。
	Select：投影每个元素。
	OrderBy 和 OrderByDescending：按升序或降序排序。
	GroupBy：根据键值对元素进行分组。
	Join：将两个集合基于键值进行关联。
	Any 和 All：检查集合是否满足某些条件。
	Count 和 Sum：计算集合的大小或求和。
	*/


	// 假设我们有一个整数列表，想要找出其中的所有偶数并按降序排列：
	private void LINQLambda()
	{
		List<int> numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

		var evenNumbers = from n in numbers
						  where n % 2 == 0
						  orderby n descending
						  select n;

		foreach (var num in evenNumbers)
		{
			Console.WriteLine(num);
		}

		// Lambda 表达式与 LINQ
		// Lambda 表达式在 LINQ 查询中被广泛使用。
		// 简单的 LINQ 查询
		numbers = [1, 2, 3, 4, 5];

		// 筛选出大于 3 的数字
		var result = numbers.Where(x => x > 3);

		foreach (var num in result)
		{
			Console.WriteLine(num); // 输出 4 和 5
		}

		// 使用 Select 转换数据
		numbers = [1, 2, 3];

		// 将每个数字平方
		var squares = numbers.Select(x => x * x);

		foreach (var square in squares)
		{
			Console.WriteLine(square); // 输出 1, 4, 9
		}
	}


	void LINQ_ling(){ LINQLambda();}
	#endregion

	#region 类型
	// 值类型
	// 简单类型：int, long, short, byte, float, double, decimal, 
	// bool, char
	// 结构体：struct
	// DateTime, TimeSpan 等少数几个内置 struct
	// 枚举：enum
	// 元组：ValueTuple

	// 引用类型
	// string object
	// 类：class record
	// 接口：interface
	// 集合：数组 List Dictionary
	// 字符串：string（特殊，表现像值类型但有引用类型的本质）
	// 委托：delegate
	// 记录：record（默认引用类型，record struct是值类型）
	// 抽象类和基类引用
	// Stream stream = File.OpenRead("file.txt");
	// Exception ex = new ArgumentException();

	// 你写的类型
	// ├── 用 class 定义     →  引用类型
	// ├── 用 struct 定义    →  值类型
	// ├── 用 record 定义    →  引用类型（默认）
	// ├── 用 record struct  →  值类型
	// └── 用 interface 定义 →  引用类型（实现类决定，但接口变量本身是引用行为）

	public static void 值类型引用类型()
	{
		// 装箱&拆箱
		int val = 42;
		object obj = val;      // 装箱：值类型→引用类型（堆上分配内存）
		int unbox = (int)obj;  // 拆箱：引用类型→值类型

		// 字符串不可变性
		string original = "hello";
		original.ToUpper();
		Console.WriteLine(original); // "hello"

		#region 数组
		int[] arr1 = { 1, 2, 3 }; var list1 = new List<int> { 1, 2, 3 }; var dict1 = new Dictionary<string, int> { { "A", 1 } };
		// 引用
		var arr2 = arr1; var list2 = list1; var dict2 = dict1;
		// 复制
		int[] arr3 = (int[])arr1.Clone(); var list3 = new List<int>(list1); var dict3 = new Dictionary<string, int>(dict1);
		#endregion
		Person p1 = new Person { Name = "Alice" };
		Person p2 = p1.Clone();  // 独立的对象
		p2.Name = "Bob";
		Console.WriteLine(p1.Name);  // Alice，不受影响

		// ref 关键字
		void Modify(ref int x) { x = 100; }
		int num = 10;
		Modify(ref num);
		Console.WriteLine(num);  // 输出：100

		#region string
		string s = "hello";
		s += " world";  // 创建新字符串，原字符串被丢弃
		s = s.Replace("world", "C#");  // 又创建了新字符串
		s = s.Substring(0, 5);         // 又创建了新字符串
									   // 短短三行，可能产生了4个临时对象

		// 低效：每次 + 都创建新对象
		string result = "";
		for (int i = 0; i < 10000; i++)
			result += i.ToString();   // 10000次内存分配！

		// 高效：内部用可变char数组，一次搞定
		var sb = new StringBuilder();
		for (int i = 0; i < 10000; i++)
			sb.Append(i);
		string result_ = sb.ToString();


		s = "hello" + " " + "world";     // + 拼接
		char @char = s[0];                          // [] 索引访问
		foreach (char ch in s) { }              // 可枚举
												// 但 s[0] = 'H'; 不行！只读的

		string? aStr = null;        // 不存在，不能调用任何方法
		string bStr = "";          // 空字符串对象，是真实存在的
		string cStr = string.Empty; // 等同于 ""，性能略好（有驻留优化）

		int? aInt = aStr?.Length;  // null，不报错（空传播）
		int bInt = bStr.Length;   // 0
		#endregion

		#region char
		char c = 'a';

		// 类型判断
		char.IsLetter(c);      // true，是否为字母
		char.IsDigit(c);       // false，是否为数字
		char.IsWhiteSpace(c);  // false，是否空白字符
		char.IsUpper(c);       // false，是否大写
		char.IsLower(c);       // true，是否小写
		char.IsLetterOrDigit(c); // true

		// 转换
		char.ToUpper(c);       // 'A'
		char.ToLower('A');     // 'a'

		// 数值转换（Unicode 码点）
		int code = (int)c;           // 97（'a' 的 Unicode 值）
		char fromCode = (char)97;    // 'a'
		if (fromCode == 'a') { }
		#endregion

		int a = 1;
		ref int b = ref a;  // b 是 a 的引用/别名
		b = 999;
		// 但 ref 局部变量有诸多限制，不能用在异步方法、不能存到字段里等。

		Console.WriteLine(a);  // 999


		object aro = 1;
		ref object bro = ref aro;

		bro = 2;                    // 修改 b
		Console.WriteLine(aro);      // 输出: 2 ✅ a 也被修改了
		Console.WriteLine(bro);      // 输出: 2

		object ao = 1;
		object bo = a;

		bo = 2;                    // 重新赋值 b（指向新对象）
		Console.WriteLine(ao);      // 输出: 1 ❌ a 不变
		Console.WriteLine(bo);      // 输出: 2
	}

	#region ref对引用
	class Box { public int Value; }

	// 1. 不加 ref：传的是引用的副本（遥控器复制品）
	void ChangeContent(Box b)
	{
		b.Value = 100;   // ✅ 影响原对象，因为指向同一块内存
		b = new Box { Value = 999 };  // ❌ 只改了副本遥控器，外部变量不变
	}

	// 2. 加了 ref：传的是引用本身（把遥控器直接拿出来调）
	void ChangeReference(ref Box b)
	{
		b.Value = 100;   // ✅ 影响原对象
		b = new Box { Value = 999 };  // ✅ 连外部变量指向的对象都换了
	}

	private void 引用_()
	{
		Box box1 = new Box { Value = 10 };
		ChangeContent(box1);
		Console.WriteLine(box1.Value);  // 100（对象内容被改了）
										// box1 仍然指向原来的对象

		Box box2 = new Box { Value = 10 };
		ChangeReference(ref box2);
		Console.WriteLine(box2.Value);  // 999（整个对象被换了！）
										// box2 现在指向全新的对象
	}
	#endregion

	#region record+

	private class Record
	{
		/*
		 特性			record			record struct		class				struct
		引入版本		C# 9.0			C# 10.0				C# 1.0				C# 1.0
		类型分类		引用类型		值类型				引用类型			值类型
		存储位置		堆（Heap）		栈（Stack）			堆（Heap）			栈（Stack）
		默认相等性		值相等（内容）	值相等（内容）		引用相等（地址）	值相等（内容）
		不可变性		✅ 默认 init	✅ 默认 init		❌ 可变				⚠️ 建议不可变
		继承			✅ 支持			❌ 不支持			✅ 支持				❌ 不支持
		无参构造函数	✅ 支持			✅ 支持				✅ 支持				❌ 不支持（C# 10前）
		析构函数		❌				❌					✅					❌
		with 表达式		✅				✅					❌					❌
		解构支持		✅				✅					❌ 需手动			❌ 需手动
		ToString() 实现	✅ 自动生成		✅ 自动生成			❌ 需手动			❌ 需手动
		IEquatable<T>	✅ 自动实现		✅ 自动实现			❌ 需手动			❌ 需手动
		适用场景		DTO、API响应	小型数据、高性能	实体、服务、行为	小型值、性能关键
		 */

		#region 相等性
		public static void 相等性()
		{
			// === record ===
			var r1 = new PersonRecord("Alice", 30);
			var r2 = new PersonRecord("Alice", 30);
			Console.WriteLine(r1 == r2);        // True（值相等）
			Console.WriteLine(r1.Equals(r2));   // True
			Console.WriteLine(ReferenceEquals(r1, r2)); // False

			// === record struct ===
			var rs1 = new PointRecord(10, 20);
			var rs2 = new PointRecord(10, 20);
			Console.WriteLine(rs1 == rs2);      // True（值相等）

			// === class ===
			var c1 = new PersonClass { Name = "Alice", Age = 30 };
			var c2 = new PersonClass { Name = "Alice", Age = 30 };
			Console.WriteLine(c1 == c2);        // False（引用相等）
			Console.WriteLine(c1.Equals(c2));   // False（需要重写）
			Console.WriteLine(ReferenceEquals(c1, c2)); // False

			// === struct ===
			var s1 = new PointStruct { X = 10, Y = 20 };
			var s2 = new PointStruct { X = 10, Y = 20 };
			Console.WriteLine(s1.Equals(s2));   // True（值相等）
												// 注意：struct 没有 == 运算符，除非重载
		}
		#endregion

		#region 不可变性
		public static void 不可变性()
		{
			// === record（默认不可变）===
			var person = new PersonRecord("Alice", 30);
			// person.Name = "Bob";  // ❌ 编译错误（init-only）

			// 使用 with 创建副本
			var updated = person with { Age = 31 };
			Console.WriteLine(person.Age);  // 30（原对象不变）
			Console.WriteLine(updated.Age); // 31

			// === record struct（默认不可变）===
			var point = new PointRecord(10, 20);
			// point.X = 30;  // ❌ 编译错误

			// === class（默认可变）===
			var c1 = new PersonClass { Name = "Alice", Age = 30 };
			c1.Name = "Bob";  // ✅ 直接修改

			// === struct（默认可变，但建议不可变）===
			var s1_ = new PointStruct { X = 10, Y = 20 };
			s1_.X = 30;  // ✅ 允许修改（但可能引起问题）
		}
		#endregion

		#region 内存分配
		public static void 内存分配()
		{
			// === record（堆分配）===
			var r1 = new PersonRecord("Alice", 30);  // 堆上分配
			var r2 = r1;  // 复制引用（不复制数据）

			// === record struct（栈分配）===
			var rs1 = new PointRecord(10, 20);  // 栈上分配
			var rs2 = rs1;  // 复制整个值（8字节）

			// === class（堆分配）===
			var c1 = new PersonClass();  // 堆上分配
			var c2 = c1;  // 复制引用

			// === struct（栈分配）===
			var s1 = new PointStruct();  // 栈上分配
			var s2 = s1;  // 复制整个值（8字节）
		}
		#endregion

		#region 性能基准测试
		public void RecordArray()
		{
			var arr = new PersonRecord[1000];
			for (int i = 0; i < 1000; i++)
				arr[i] = new PersonRecord($"User{i}", i);
			// 1000 次堆分配
		}

		public void RecordStructArray()
		{
			var arr = new PointRecord[1000];
			for (int i = 0; i < 1000; i++)
				arr[i] = new PointRecord(i, i);
			// 连续内存，无额外分配
		}

		public void ClassArray()
		{
			var arr = new PersonClass[1000];
			for (int i = 0; i < 1000; i++)
				arr[i] = new PersonClass { Name = $"User{i}", Age = i };
			// 1000 次堆分配
		}

		public void StructArray()
		{
			var arr = new PointStruct[1000];
			for (int i = 0; i < 1000; i++)
				arr[i] = new PointStruct { X = i, Y = i };
			// 连续内存，无额外分配
		}

		// 结果（大约）：
		// RecordArray:        8.5 μs,  24000 bytes GC
		// RecordStructArray:  2.1 μs,      0 bytes GC  ✅ 最快
		// ClassArray:         9.2 μs,  28000 bytes GC
		// StructArray:        2.3 μs,      0 bytes GC  ✅ 第二快
		#endregion

		#region 定义语法
		// ===== record（引用类型，C# 9.0+）=====
		public record PersonRecord(string Name, int Age);

		// 等价于：
		public record PersonRecord_
		{
			public string Name { get; init; } // set：任何时候都可以修改
			public int Age { get; init; } // init：只能在构造时赋值，之后只读
			public PersonRecord_(string name, int age) => (Name, Age) = (name, age);
			public void Deconstruct(out string name, out int age) => (name, age) = (Name, Age);
		}

		// ===== record struct（值类型，C# 10.0+）=====
		public record struct PointRecord(int X, int Y);

		// ===== class（引用类型，C# 1.0+）=====
		public class PersonClass
		{
			public string? Name { get; set; }
			public int Age { get; set; }
		}

		// ===== struct（值类型，C# 1.0+）=====
		public struct PointStruct
		{
			public int X { get; set; }
			public int Y { get; set; }
		}
		#endregion
	}
	#endregion

	class Person
	{
		public string? Name;
		public Person Clone() => new Person { Name = this.Name };
	}

	#endregion

	#region Mod按键?
	// Input.GetKey("n")	按住期间每帧返回 true
	// Input.GetKeyDown("n")   按下瞬间只返回 true 一次
	// Input.GetKeyUp("n") 松开瞬间只返回 true 一次

	/*public static readonly PlayerKeybind Explode = PlayerKeybind.Register(
			"example:explode",      // 唯一ID（格式：作者:功能）
			"Example Mod",          // 模组显示名称
			"Explode",              // 按键显示名称
			KeyCode.C,              // 键盘默认键（C键）
			KeyCode.JoystickButton3 // 手柄默认键（通常是RB或R1）
		);*/
	#endregion

	#region More

	private static void Player_ctor(On.Player.orig_ctor orig, Player self, AbstractCreature abstractCreature, World world)
	{
		if (self.room.world.game.rainWorld.ExpeditionMode)//在探险模式里开启冰盾能力
		{
			//GlobalVar.glacier2_iceshield_lock = false;
		}
		if (self.room.world.game.session is ArenaGameSession)//在竞技场模式里也开启冰盾能力
		{
			//GlobalVar.glacier2_iceshield_lock = false;
		}

		Player player = self;
		//player.GetPlayerVar(out var pv);
		//var stomachData = pv.stomachData;

		//if (stomachData.IsFull)
		{

		}
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
				settings = new RoomSettings(null, WorldLoader.RoomNameManipulator(abstractRoom.FileName, abstractRoom.world.game), abstractRoom.world.region, template: false, firstTemplate: false, abstractRoom.world.game?.TimelinePoint, abstractRoom.world.game);
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

	#region room

	//room
	//player.room.game.Players
	//player.room.game.GetStorySession.Players
	//player.room.game.warpDeferPlayerSpawnRoomName
	//player.room.abstractRoom.name

	#endregion

	#region CWT
	//private static readonly ConditionalWeakTable<object, object> _markedInstances = new();

	// | 方法 | 参数 | 作用 |
	// | :--- | :--- | :--- |
	// | `GetOrCreateValue(key)` | 1个参数 | 自动调用 `new TValue()` 创建值。要求 TValue 必须有无参构造函数。 |
	// | `GetValue(key, callback)` | 2个参数 | 键不存在时，调用你传入的 `callback` 方法来创建值。最灵活，推荐用于标记场景。 |
	// | `Add(key, value)` | 2个参数 | 强制绑定一个已存在的值。如果键已存在会报错。 |
	#endregion

	#region ItemID

	//swallowedObjectsTemp.Add("ID.-1.5964<oB>0<oA>Rock<oA>SU_S01.22.17.0");

	//ID.-1.5964<oB>0<oA>Rock<oA>SU_S01.20.16.0		(AbstractPhysicalObject)
	//ID.-1.2274<oB>0<oA>FirecrackerPlant<oA>SU_S01.23.17.0<oA>-1<oA>-1		(AbstractConsumable)
	//ID.-1.1980<oB>0<oA>Rock<oA>SU_S01.23.16.0		(AbstractPhysicalObject)
	//Hazer ID.-1.1982		(AbstractCreature)

	//"swallowedObjects": [
	//"ID.-1.5964<oB>0<oA>Rock<oA>SU_S01.20.16.0",
	//"ID.-1.2274<oB>0<oA>FirecrackerPlant<oA>SU_S01.23.17.0<oA>-1<oA>-1",
	//"ID.-1.1980<oB>0<oA>Rock<oA>SU_S01.23.16.0",
	//"Hazer<cA>ID.-1.1982<cB>0<cA>SU_S01.0<cA>"
	//]



	// 4. 检查 swallowedObjectsTemp
	//Console.WriteLine($"swallowedObjectsTemp count: {pv0.swallowedObjectsTemp.Count}");

	// 5. 检查 objectsInStomach
	//Console.WriteLine($"objectsInStomach count: {pv0.objectsInStomach.Count}");



	//pv0.swallowedObjectsTemp.Add("ID.-1.5964<oB>0<oA>Rock<oA>SU_S01.22.17.0");

	//Console.WriteLine($"StorageCapacity: {pv0.StorageCapacity}");

	// 4. 检查 swallowedObjectsTemp
	//Console.WriteLine($"swallowedObjectsTemp count: {pv0.swallowedObjectsTemp.Count}");

	// 5. 检查 objectsInStomach
	//Console.WriteLine($"objectsInStomach count: {pv0.objectsInStomach.Count}");

	// 这样有用

	#endregion

	#region Items
	string[] baseItemTypes = {
				"Item",
				"Rock",
				"Spear",
				"VultureMask",
				"NeedleEgg",
				"OracleSwarmer",
				"SeedCob",
				"SporePlant",
				"FlareBomb",
				"PuffBall",
				"FirecrackerPlant",
				"KarmaFlower",
			};
	string[] mscItemTypes = {
				"LillyPuck",
				"FireEgg",
				"JokeRifle",
				"EnergyCell",
				"MoonCloak",
			};
	string[] watcherItemTypes = {
				"Boomerang",
				"GraffitiBomb",
			};
	#endregion
	#region Creatures
	string[] baseCreatureTypes = {
				"Creature",
				"Slugcat",
				"Lizard",
				"Vulture",
				"Centipede",
				"Spider",
				"DropBug",
				"BigEel",
				"MirosBird",
				"DaddyLongLegs",
				"Cicada",
				"Snail",
				"Scavenger",
				"EggBug",
				"LanternMouse",
				"JetFish",
				"TubeWorm",
				"Deer",
				"TempleGuard"
			};
	string[] mscCreatureTypes = {
				"Yeek",
				"Inspector",
				"StowawayBug"
			};
	string[] watcherCreatureTypes = {
				"Loach",
				"BigMoth",
				"SkyWhale",
				"BoxWorm",
				"DrillCrab",
				"Tardigrade",
				"Barnacle",
				"Frog"
			};
	#endregion

	#region Save

	private static string path1 => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt");
	private static string path => "";//Path.Combine(Application.persistentDataPath, "ExtensionData");

	public static void WipeAll(int saveSlot)
	{
		// 确保目录存在
		if (Directory.Exists(path))
		{
			// 获取目录下所有文件的完整路径
			string[] files = Directory.GetFiles(path);

			// 遍历每个文件
			for (int i = 0; i < files.Length; i++)
			{
				// 检查文件名是否以 "Extended_lyn{存档槽位编号}" 开头
				// 注意：StartsWith 的参数构成为 path + 目录分隔符 + "Extended_lyn" + saveSlot.ToString()
				// 例如 path 为 "/.../Extended_lyn"，那么前缀就是 "/.../Extended_lyn/Extended_lyn1"（假设 saveSlot=1）
				if (files[i].StartsWith(path + Path.DirectorySeparatorChar.ToString() + "Extended_lyn" + saveSlot.ToString()))
				{
					// 删除文件
					File.Delete(files[i]);
				}
			}
		}
	}


	private void Save()
	{
		//存档字符串
		// 读取存档字符串：
		// "player_name<svB>玩家A<svA>level<svB>5<svA>coins<svB>100<svA>my_simple_data<svB>123<svA>"

		// 分割成：
		// ["player_name<svB>玩家A", "level<svB>5", "coins<svB>100", "my_simple_data<svB>123"]

		// 再分割每个部分：
		// "my_simple_data<svB>123" → ["my_simple_data", "123"]

		// 发现键是"my_simple_data"，值就是"123"

		//ID.-1.266<oB>0<oA>FlareBomb<oA>SL_S10.20.24.0<oA>-1<oA>-1，ID.-1.266<oB>0<oA>FlareBomb<oA>SL_S10.20.24.0<oA>-1<oA>-1，ID.-1.266<oB>0<oA>FlareBomb<oA>SL_S10.20.24.0<oA>-1<oA>-1

		//StomachStorage_ESS_SAVEFIELD<svB>Player0<svD>ID.-1.4206<oB>0<oA>OverseerCarcass<oA>HI_S05.16.16.0<oA>0.4470588<oA>0.9019608<oA>0.7686275<oA>0<oA>0,ID.-1.4206<oB>0<oA>OverseerCarcass<oA>HI_S05.16.16.0<oA>0.4470588<oA>0.9019608<oA>0.7686275<oA>0<oA>0,ID.-1.7342<oB>0<oA>DataPearl<oA>HI_S05.16.16.0<oA>131<oA>1<oA>Misc,ID.-1.7341<oB>0<oA>DataPearl<oA>HI_S05.16.16.0<oA>131<oA>0<oA>HI,ID.-1.3843<oB>0<oA>ScavengerBomb<oA>HI_S05.16.16.0,ID.-1.3840<oB>0<oA>ScavengerBomb<oA>HI_S05.16.16.0,ID.-1.3841<oB>0<oA>ScavengerBomb<oA>HI_S05.16.16.0<svC><svA><svA><svA>

		//层级 分隔符  作用
		//-------------------------------------------
		//顶级 <svA>   分隔主项
		//     <svB>   主项内的键值分隔
		//二级 <mwA>   分隔子项
		//	   <mwB>   子项内的键值分隔
		//三级 <slosA> 分隔子子项
		//     <slosB> 子子项内的键值分隔
		//四级 <svC>   分隔子子子项
		//	   <svD>   子子子项内的键值分隔

		// 最终保存格式：
		// ESS_savefield_name<svB>Player0<mwB>物品1,物品2<mwA>Player1<mwB>物品3<mwA><svA>



	}

	//保存部分
	/*private static string SaveState_SaveToString(On.SaveState.orig_SaveToString orig, SaveState saveState)
	{
		// 获取原版存档
		string text = orig(saveState);

		// 移除原版存档中的"my_simple_data"字段
		text = State.RemoveField(text, "my_simple_data");

		// 保存"123"
		text += "my_simple_data<svB>123<svA>";

		return text;
	}
	//加载部分
	private static void SaveState_LoadGame(On.SaveState.orig_LoadGame orig, SaveState saveState, string str, RainWorldGame game)
	{
		// 先调用原版方法
		orig(saveState, str, game);

		// 查找保存的"123"数据
		string[] array = Regex.Split(str, "<svA>");
		foreach (var p in array)
		{
			string[] array2 = Regex.Split(p, "<svB>");
			if (array2.Length >= 2 && array2[0] == "my_simple_data")
			{
				// 找到并处理数据
				string savedData = array2[1];
				// 这里可以处理savedData（应该是"123"）
				UnityEngine.Debug.Log(savedData);
			}
		}
	}*/

	public static string GetSavePath(string modName = "CustomStomachStorage_Redlyn")
	{
		// 读取现有内容
		//string existingContent = File.ReadAllText(filePath);

		// 获取当前用户的LocalLow目录
		string localLowPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
		localLowPath = Path.Combine(localLowPath, "..", "LocalLow");
		localLowPath = Path.GetFullPath(localLowPath);  // 规范化路径

		// 构建完整路径
		return Path.Combine(localLowPath, "Videocult", "Rain World", "ModConfigs", $"{modName}_data.txt");
	}
	#endregion

	#region Other

	public static string GenerateRandomText()
	{
		string[] texts = {
			"瞬间即是永恒！",
			};
		return texts[UnityEngine.Random.Range(0, texts.Length)];
	}

	#endregion

	#region Items
	#endregion
	#region Creatures
	#endregion

	#region IL
	// !IsFull
	// 跳进去

	// !IsEmpty
	// 跳进去

	// !IsEmpty
	// if (inHand != -1 && !stomachData.IsFull)return false;
	// 跳进去

	// == null   IsEmpty
	// 跳出去

	// !IsFull
	// 跳出去
	#endregion

	#region 光学迷彩
	public static class Camouflage
	{
		//用来获取特征做判断
		public static readonly PlayerFeature<bool> PlayerCamoflage = PlayerBool("camouflage");
		//用这个字典来保存读取一个放数据的类
		public static ConditionalWeakTable<Player, CmouflageModule> modules = new ConditionalWeakTable<Player, CmouflageModule>();

		//需要执行得内容
		public static void Hook()
		{
			On.Player.ctor += Player_ctor;//初始化迷彩能力

			On.PlayerGraphics.Update += PlayerGraphics_Update;//玩家显示内容的更新
			On.PlayerGraphics.DrawSprites += PlayerGraphics_DrawSprites;//玩家显示的更新


		}


		private static void Player_ctor(On.Player.orig_ctor orig, Player self, AbstractCreature abstractCreature, World world)
		{
			//执行正常流程
			orig.Invoke(self, abstractCreature, world);
			//在玩家初始化后如果有这个特征就作为键加入到module字典里值是迷彩模型
			if (PlayerCamoflage.TryGet(self, out var flag) && flag)
			{
				modules.Add(self, new CmouflageModule());
			}
		}

		private static void PlayerGraphics_Update(On.PlayerGraphics.orig_Update orig, PlayerGraphics self)
		{
			orig.Invoke(self);
			//因为初始化我们已经把所有有特征的玩家都加入字典了,所以我们现在只要查字典里面有没有这个玩家就可以
			if (modules.TryGetValue(self.player, out var cmouflageModule))
			{
				//如果玩家没死
				if (!self.player.dead)
				{
					//测他有没有动,我这里是测上上个位置和这次更新的位置的距离是否小于0.3如果小于就说明没动
					if (Vector2.Distance(self.player.mainBodyChunk.pos, self.player.mainBodyChunk.lastPos) < 0.3)
					{
						//如果没动就把迷彩现在的颜色渐渐往 迷彩时选择的颜色 靠拢
						cmouflageModule.whiteCamoColor = Color.Lerp(cmouflageModule.whiteCamoColor, cmouflageModule.whitePickUpColor, 0.1f);
					}
					else
					{//不然就把颜色往玩家靠拢
						cmouflageModule.whiteCamoColor = Color.Lerp(cmouflageModule.whiteCamoColor, self.player.ShortCutColor(), 0.1f);
					}
				}
			}
		}
		private static void PlayerGraphics_DrawSprites(On.PlayerGraphics.orig_DrawSprites orig, PlayerGraphics self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
		{
			orig.Invoke(self, sLeaser, rCam, timeStacker, camPos);
			if (modules.TryGetValue(self.player, out var cmouflageModule))
			{
				//如果玩家动的距离很少
				if (Vector2.Distance(self.player.mainBodyChunk.pos, self.player.mainBodyChunk.lastPos) < 0.3)
				{
					//就把玩家迷彩选择的目标色 设为相机在玩家位置获取到的像素颜色
					cmouflageModule.whitePickUpColor = rCam.PixelColorAtCoordinate(self.player.mainBodyChunk.pos);
				}

				//然后给玩家的身体部件都染上迷彩现在颜色
				for (int i = 0; i < 12; i++)
				{
					sLeaser.sprites[i].color = cmouflageModule.whiteCamoColor;
				}
			}
		}


	}


	public class CmouflageModule
	{
		//迷彩的实时颜色
		public Color whiteCamoColor = new Color(0f, 0f, 0f);

		//迷彩的目标颜色
		public Color whitePickUpColor;
		public CmouflageModule() { }
	}
	#endregion

	public string Strings_
	{
		get
		{
			return Strings_;
		}
		set
		{
			Strings_ = value;
		}
	} // 属性
}
#pragma warning restore CS0169