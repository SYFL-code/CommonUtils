using CommonUtils;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CommonUtils.Core
{
	public static class UpdatableManager
	{
		public static List<IUpdatable> updatables = [];

		public static void Apply()
		{
			Plugin.OnUpdate += OnUpdate;
		}

		public static void UnApply()
		{
			Plugin.OnUpdate -= OnUpdate;
			updatables.Clear();
		}

		private static void OnUpdate()
		{
			foreach (IUpdatable u in UpdatableManager.updatables)
			{
				if (u.active)
				{
					u.Update();
				}
			}
		}

		public static void Register(IUpdatable u)
		{
			UpdatableManager.updatables.Add(u);
			updatables.Sort((a, b) => a.Priority.CompareTo(b.Priority));
		}

		public static void Unregister(IUpdatable u)
		{
			UpdatableManager.updatables.Remove(u);
		}

	}

	public interface IUpdatable
	{
		void Update();

		bool active { get; }

		// 优先级
		// 数值越小，优先级越高
		int Priority { get; }
	}

}
