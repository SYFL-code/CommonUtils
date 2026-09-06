using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace CommonUtils.Core;

public static class ModuleManager
{
	private static class Storage<TTarget, TData>
		where TTarget : class
		where TData : class
	{
		public static readonly ConditionalWeakTable<TTarget, TData> Table = new();
	}

	public static TData Get<TTarget, TData>(TTarget target)
		where TTarget : class
		where TData : class, new()
	{
		return Storage<TTarget, TData>.Table.GetOrCreateValue(target);
	}
	public static TData Get<TTarget, TData>(TTarget target, Func<TTarget, TData> factory)
		where TTarget : class
		where TData : class
	{
		var callback = new ConditionalWeakTable<TTarget, TData>.CreateValueCallback(factory);
		return Storage<TTarget, TData>.Table.GetValue(target, callback);
	}

}