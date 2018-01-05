using UnityEngine;

public class Road : MonoBehaviour
{
	public static Road Instance;

	public Transform LeftEndPoint;
	public Transform MiddelEndPoint;
	public Transform RightEndPoint;

	public Transform ShortEndPoint;

	private void Start()
	{
		Instance = this;
	}
}
