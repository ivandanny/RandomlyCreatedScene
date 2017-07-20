using UnityEngine;

public static class HexMetrics {
	
	public const float outerRadius = 10f;
	
	public const float innerRadius = outerRadius * 0.866025404f;

	public const float smallRadius = outerRadius * 1.0f;
	public const float smallerRadius = innerRadius * 1.0f;

	public static Vector3[] corners = {
		new Vector3(0f, 0f, smallRadius),
		new Vector3(smallerRadius, 0f, 0.5f * smallRadius),
		new Vector3(smallerRadius, 0f, -0.5f * smallRadius),
		new Vector3(0f, 0f, -smallRadius),
		new Vector3(-smallerRadius, 0f, -0.5f * smallRadius),
		new Vector3(-smallerRadius, 0f, 0.5f * smallRadius),
		new Vector3(0f, 0f, smallRadius)
	};
}