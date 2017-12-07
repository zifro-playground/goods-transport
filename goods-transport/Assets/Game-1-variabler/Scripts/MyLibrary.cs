using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyLibrary{

	public static Bounds CalculateBoundsInChildren(GameObject obj)
	{
		Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();

		if (renderers.Length == 0)
			throw new System.Exception("Could not find any renderers in children of gameobject \"" + obj.name + "\".");

		Bounds bounds = new Bounds(obj.transform.position, Vector3.zero);
		foreach (Renderer renderer in renderers)
		{
			bounds.Encapsulate(renderer.bounds);
		}
		Vector3 localCenter = bounds.center - obj.transform.position;
		bounds.center = localCenter;

		return bounds;
	}

	/// <summary>
	/// Scale a linear range between 0.0-1.0 to an exponential scale using the equation returnValue = A + B * Math.Exp(C * inputValue)
	/// </summary>
	/// <param name="min">The value returned for input value of 0</param>
	/// <param name="mid">The value returned for input value of 0.5</param>
	/// <param name="max">The value returned for input value of 1.0</param>
	/// <param name="value">The value to scale</param>
	/// <returns></returns>
	public static float LinearToExponential(float min, float mid, float max, float value)
	{
		if (value < 0 || value > 1) throw new ArgumentOutOfRangeException("Input value must be between 0 and 1.0");
		if (mid <= 0 || mid >= max) throw new ArgumentOutOfRangeException("MidValue must be greater than 0 and less than MaxValue");

		float A = (min * max - Mathf.Pow(mid, 2)) / (min - 2 * mid + max);
		float B = -A;
		float C = 2 * Mathf.Log((max - mid) / (mid - min));

		return A + B * Mathf.Exp(C * value);
	}
}
