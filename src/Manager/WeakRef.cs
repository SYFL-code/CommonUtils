using System;

public sealed class WeakRef<T> where T : class
{
	private WeakReference<T?> _reference;

	public WeakRef() : this(null) { }

	public WeakRef(T? target)
	{
		_reference = new WeakReference<T?>(target);
	}

	/// <summary>目标对象；已被回收时返回 null。</summary>
	public T? Value
	{
		get => _reference.TryGetTarget(out var target) ? target : null;
		set => _reference.SetTarget(value);
	}

	/// <summary>目标是否仍然存活。</summary>
	public bool IsAlive => _reference.TryGetTarget(out _);

	/// <summary>尝试获取目标，成功返回 true。</summary>
	public bool TryGet(out T? target) => _reference.TryGetTarget(out target);

	public void Clear() => _reference.SetTarget(null);

	public override string ToString() =>
		IsAlive ? $"WeakRef<{typeof(T).Name}>(alive)" : $"WeakRef<{typeof(T).Name}>(collected)";
}