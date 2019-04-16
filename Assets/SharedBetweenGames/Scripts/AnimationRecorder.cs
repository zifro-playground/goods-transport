using UnityEngine;
using UnityEngine.AI;

public class AnimationRecorder : MonoBehaviour
{
	public NavMeshAgent agent;
	public Transform target;

	void Start()
	{
		agent.SetDestination(target.position);
	}

	void Update()
	{
		if (Vector3.Distance(transform.position, target.position) < 1.5f)
		{
			print("Framme");
			agent.enabled = false;
		}
	}
}
