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
using Color = UnityEngine.Color;
using ObjType = AbstractPhysicalObject.AbstractObjectType;
using Random = UnityEngine.Random;

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
internal class ZnameText//Scrap 废案
{
	#region Items
	#endregion
	#region Creatures
	#endregion



	/*
殇街暮雪: 09-13 17:39:43
游戏内的改动同步到world.txt内

闪卡: 09-13 17:43:36
还是比较难想象的,这游戏有些东西在modify里面有些东西在那个啥临时文件夹里面,感觉很难懂

选择部分: 09-13 17:43:56
@闪卡 那个确实只有做地边的才会有大概的了解

选择部分: 09-13 17:44:02
本质上就是一个拥有特殊语法的

选择部分: 09-13 17:44:18
创建对原版文件进行修改的东西

Ronko: 09-13 19:54:01
老师们，我想问一下我进用iterator creator做迭代器的mod里，没有迭是怎么回事啊
*/

	/*
	按 Java 版 1.21，共 42 种附魔；基岩版没有 横扫之刃，所以是 41 种。括号内为最大等级，已标注宝藏/诅咒。
盔甲类
	保护 IV
	火焰保护 IV
	摔落保护 IV
	爆炸保护 IV
	弹射物保护 IV
	呼吸 III
	水下速掘 I
	荆棘 III
	深海探索者 III
	冰霜行者 II（宝藏）
	灵魂疾行 III（宝藏）
	迅捷潜行 III（宝藏）
诅咒类
	绑定诅咒 I（宝藏/诅咒）
	消失诅咒 I（宝藏/诅咒）
近战武器
	锋利 V
	亡灵杀手 V
	节肢杀手 V
	击退 II
	火焰附加 II
	抢夺 III
	横扫之刃 III（仅 Java 版）
工具/通用
	效率 V
	精准采集 I
	时运 III
	耐久 III
	经验修补 I（宝藏）
弓
	力量 V
	冲击 II
	火矢 I
	无限 I
弩
	多重射击 I
	快速装填 III
	穿透 IV
三叉戟
	忠诚 III
	激流 III
	引雷 I
	穿刺 V
钓鱼竿
	海之眷顾 III
	诱饵 III
重锤（1.21 新增）
	密度 V
	破甲 IV
	风爆 III（宝藏）
合计：42 种。基岩版去掉 横扫之刃 就是全部。
	*/

}
#pragma warning restore CS0169