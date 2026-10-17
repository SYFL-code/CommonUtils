using BepInEx;
using System;

namespace CommonUtils
{
	internal static class PluginOwnership
	{
		private static readonly OwnerGate<BaseUnityPlugin> gate = new();

		internal static bool TryClaim(BaseUnityPlugin plugin)
		{
			return gate.TryClaim(plugin);
		}

		internal static bool IsOwner(BaseUnityPlugin plugin)
		{
			return gate.IsOwner(plugin);
		}

		internal static bool TryRelease(BaseUnityPlugin plugin, bool cleanupComplete)
		{
			return gate.TryRelease(plugin, cleanupComplete);
		}
	}
}
