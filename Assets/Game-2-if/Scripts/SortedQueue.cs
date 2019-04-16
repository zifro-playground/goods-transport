using System.Collections.Generic;
using UnityEngine;

public class SortedQueue : MonoBehaviour
{
	public static readonly List<GameObject> LEFT_QUEUE = new List<GameObject>();
	public static readonly List<GameObject> FORWARD_QUEUE = new List<GameObject>();
	public static readonly List<GameObject> RIGHT_QUEUE = new List<GameObject>();

	public static void ResetQueues()
	{
		LEFT_QUEUE.Clear();
		RIGHT_QUEUE.Clear();
		FORWARD_QUEUE.Clear();
	}
}
