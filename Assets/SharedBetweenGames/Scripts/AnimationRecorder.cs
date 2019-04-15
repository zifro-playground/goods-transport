using UnityEngine;
using UnityEngine.AI;

public class AnimationRecorder : MonoBehaviour
{
	public Transform target;
	public NavMeshAgent agent;

	void Start ()
	{
		agent.SetDestination(target.position);
	}

	void Update ()
	{
		if (Vector3.Distance(transform.position, target.position) < 1.5f)
		{
			print("Framme");
			agent.enabled = false;
		}
	}
}
