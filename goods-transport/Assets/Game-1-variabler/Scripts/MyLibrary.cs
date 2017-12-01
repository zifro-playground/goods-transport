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
}
