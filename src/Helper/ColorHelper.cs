using CommonUtils;
using UnityEngine;
namespace CommonUtils.Core;

public static class ColorHelper
{
	public static float Lerp(Color a, Color b, Color current)
	{
		// Color 可以隐式转为 Vector4（RGBA 四维空间）
		Vector4 vA = a;
		Vector4 vB = b;
		Vector4 vC = current;

		Vector4 direction = vB - vA;
		float sqrLen = direction.sqrMagnitude;

		// 防除零：如果起点和终点颜色完全相同，说明无法区分，直接视为完全到达（1）
		if (sqrLen < 1e-6f)
			return 1f;

		// 核心公式：投影长度 / 总长度
		// 即 (current - a) · (b - a) / |b - a|²
		float t = Vector4.Dot(vC - vA, direction) / sqrLen;

		// 限制在 0~1 之间（防止浮点数越界）
		return Mathf.Clamp01(t);
	}
}