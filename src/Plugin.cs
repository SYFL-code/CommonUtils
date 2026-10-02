global using CommonUtils.Core;
global using System;
global using UnityEngine;
global using Color = UnityEngine.Color;
global using Random = UnityEngine.Random;
using BepInEx;
using CommonUtils.Debug;
using SlugBase.Features;
using static SlugBase.Features.FeatureTypes;


namespace CommonUtils
{
	//[BepInPlugin(Plugin.GUID, Plugin.NAME, Plugin.VERSION)]
	class Plugin// : BaseUnityPlugin
	{
		#region 信息
		public static string GUID = "common-utils.redlyn";
		public static string NAME = "Common Utils";
		public static string VERSION = "0.1.0";

		public static string Name = "CommonUtils";

		public static string version = "01";
		public static string buildTime = "1990-01-01 00:00:00";
		#endregion

		#region Release & DEBUG
#if DEBUG
		public static bool DebugMode { get; } = true;
		public static bool ForceLog { get; } = true;
#else
	public const bool DebugMode = false;
	public const bool ForceLog = false;
#endif
		#endregion

		public static Plugin plugin = new Plugin(); //

		//public bool isEnabled;
		public bool inited;

		#region Unity

		public void Awake()// Awake → OnEnable → Start
		{
			Log.LogDebug($"{Name} Mod Awake");
		}
		public void Start()
		{
			Log.LogDebug($"{Name} Mod Start");
		}
		internal static event Action? OnUpdate;
		public void Update()
		{
			OnUpdate?.Invoke();
		}

		#endregion

		public void OnEnable()
		{
			//Log.LogDebug($"{Name} Mod OnEnable! isEnabled: {isEnabled}");

			//if (this.isEnabled)
			//	return;
			//this.isEnabled = true;

			CommonUtils.Core.GlobalVar.Hook();

			UpdatableManager.Apply();
			UpdatableManager.Register(Debugger._debugger.Instance);

			// Put your custom hooks here!-在此放置你自己的钩子
			//On.RainWorld.OnModsInit += On_RainWorld_OnModsInit;
			//On.RainWorld.OnModsEnabled += On_RainWorld_OnModsEnabled;
			//On.RainWorld.OnModsDisabled += On_RainWorld_OnModsDisabled;

			//HookManager.Initialize();
		}

		public void OnDisable()
		{
			//Log.LogDebug($"{Name} Mod OnDisable! isEnabled: {isEnabled}");

			//if (!this.isEnabled)
			//	return;
			//this.isEnabled = false;

			UpdatableManager.UnApply();

			// Remove your custom hooks here!-在此取消你的钩子
			//On.RainWorld.OnModsInit -= On_RainWorld_OnModsInit;
			//On.RainWorld.OnModsEnabled -= On_RainWorld_OnModsEnabled;
			//On.RainWorld.OnModsDisabled -= On_RainWorld_OnModsDisabled;

			//HookManager.UninitializeAll();
		}

		private void On_RainWorld_OnModsInit(On.RainWorld.orig_OnModsInit orig, RainWorld rainWorld)
		{
			orig?.Invoke(rainWorld);

			Log.LogInfo($"{Name} Mod OnModsInit! inited: {inited}");

			try
			{
				// Load any resources, such as sprites or sounds-加载任何资源 包括图像素材和音效

				// Put your custom hooks here!-在此放置你自己的钩子

			}
			catch (Exception ex)
			{
				Log.LogException(ex);
			}
		}

		private void On_RainWorld_OnModsEnabled(On.RainWorld.orig_OnModsEnabled orig, RainWorld rainWorld, ModManager.Mod[] newlyEnabledMods)
		{
			orig?.Invoke(rainWorld, newlyEnabledMods);

			Log.LogInfo($"{Name} Mod OnModsEnabled! inited: {inited}, newlyEnabledMods: {newlyEnabledMods}");

			if (this.inited)
				return;
			this.inited = true;


		}

		private void On_RainWorld_OnModsDisabled(On.RainWorld.orig_OnModsDisabled orig, RainWorld rainWorld, ModManager.Mod[] newlyDisabledMods)
		{
			orig?.Invoke(rainWorld, newlyDisabledMods);

			Log.LogInfo($"{Name} Mod OnModsDisabled! inited: {inited}, newlyDisabledMods: {newlyDisabledMods}");

			if (!this.inited)
				return;
			this.inited = false;

			try
			{
				// Remove your custom hooks here!-在此取消你的钩子


				foreach (var mod in newlyDisabledMods)
				{
					if (mod.id == GUID)
					{
						break;
					}
				}
			}
			catch (Exception ex)
			{
				Log.LogException(ex);
			}
		}



	}
}