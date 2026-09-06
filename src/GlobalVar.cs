using CommonUtils;
using CommonUtils.Debug;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;
using UnityEngine;


namespace CommonUtils.Core
{
	public static class GlobalVar
	{
		//玩家变量
		//public static Dictionary<string, PlayerVar> playerVars = new();

		//全局系统变量
		public static RainWorldGame? game = null;

		#region 玩家变量
		public static void Hook()
		{
			HookManager.Register("On.Player.ctor += Player_ctor (GlobalVar)", new HookManager.HookData
			{
				Priority = HookManager.Top,
				InitializeHooks = () => On.Player.ctor += Player_ctor,
				UnInitializeHooks = () => On.Player.ctor -= Player_ctor,
			});
		}

		private static void Player_ctor(On.Player.orig_ctor orig, Player player, AbstractCreature abstractCreature, World world)
		{
			orig.Invoke(player, abstractCreature, world);

			//赋值给全局变量供其他函数使用
			GlobalVar.game = world.game;
		}

		#endregion


	}
}
