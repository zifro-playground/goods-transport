using System.Collections.Generic;
using UnityEngine;

public class SortedQueue : MonoBehaviour
{
	public static List<GameObject> LeftQueue = new List<GameObject>();
	public static List<GameObject> ForwardQueue = new List<GameObject>();
	public static List<GameObject> RightQueue = new List<GameObject>();

	public static void ResetQueues()
	{
		LeftQueue.Clear();
		RightQueue.Clear();
		ForwardQueue.Clear();
	}
}
