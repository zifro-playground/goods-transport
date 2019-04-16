using UnityEngine;
using UnityEngine.Serialization;

public class Road : MonoBehaviour
{
	public static Road instance;

	[FormerlySerializedAs("LeftEndPoint")]
	public Transform leftEndPoint;
	[FormerlySerializedAs("MiddelEndPoint")]
	public Transform middelEndPoint;
	[FormerlySerializedAs("RightEndPoint")]
	public Transform rightEndPoint;

	[FormerlySerializedAs("ShortEndPoint")]
	public Transform shortEndPoint;

	private void Start()
	{
		instance = this;
	}
}
