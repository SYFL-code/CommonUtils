using BepInEx.Logging;
using CommonUtils.Misc;
using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;

namespace CommonUtils.Core;

public static class PropertyHook
{
	private static Hook? IsWallClimberHook;
	public delegate bool OrigIsWallClimber(Lizard self);

	public static void Initialize()
	{
		InstallIsWallClimberHook();
	}

	public static void UnInitializeAll()
	{
		IsWallClimberHook?.Dispose();
		IsWallClimberHook = null;
	}

	private static void InstallIsWallClimberHook()
	{
		if (IsWallClimberHook == null)
		{
			PropertyInfo property = typeof(Lizard).GetProperty("IsWallClimber",
				BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			MethodInfo? methodInfo = property?.GetGetMethod(true);

			if (methodInfo == null)
			{
				Log.LogError("Could not find Lizard.IsWallClimber getter.");
			}
			else
			{
				MethodInfo method = typeof(PropertyHook).GetMethod("IsWallClimberDetour",
					BindingFlags.Static | BindingFlags.NonPublic);

				if (method == null)
				{
					Log.LogError("Could not find IsWallClimberDetour.");
				}
				else
				{
					IsWallClimberHook = new Hook(methodInfo, method);
				}
			}
		}
	}

	private static bool IsWallClimberDetour(OrigIsWallClimber orig, Lizard self)
	{
		//bool flag = self != null && self.abstractCreature != null && self.abstractCreature.creatureTemplate != null
		//&& self.abstractCreature.creatureTemplate.type == TigerLizardCritob.TigerLizardType;
		
		if (chain == null)
		{
			if (_isWallClimberHandlers.Count == 0)
			{
				chain = null;
				return orig(self);
			}

			chain = (self) => _isWallClimberHandlers[0](orig, self);
			for (int i = 1; i < _isWallClimberHandlers.Count; i++)
			{
				int index = i;
				var prev = chain;
				chain = (self) => _isWallClimberHandlers[index](prev.Invoke, self);
			}
		}

		return chain(self);
	}
	private static Func<Lizard, bool>? chain = null;

	public static event Func<OrigIsWallClimber, Lizard, bool> IsWallClimber
	{
		add
		{
			_isWallClimberHandlers.Add(value);
		}
		remove
		{
			_isWallClimberHandlers.Remove(value);
		}
	}
	private static List<Func<OrigIsWallClimber, Lizard, bool>> _isWallClimberHandlers = [];

}