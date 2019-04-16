using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Animations;

#endif

public class HierarchyRecorder : MonoBehaviour
{
	// The clip the recording is going to be saved to.
	public AnimationClip clip;

	// Checkbox to start/stop the recording.
	public bool record;

#if UNITY_EDITOR
	// The main feature: the actual recorder.
	GameObjectRecorder m_Recorder;

	void Start()
	{
		// Create the GameObjectRecorder.
		m_Recorder = new GameObjectRecorder(gameObject);

		// Set it up to record the transforms recursively.
		m_Recorder.BindComponentsOfType<Transform>(gameObject, true);
	}

	// The recording needs to be done in LateUpdate in order
	// to be done once everything has been updated
	// (animations, physics, scripts, etc.).
	void LateUpdate()
	{
		if (clip == null)
		{
			return;
		}

		if (record)
		{
			// As long as "record" is on: take a snapshot.
			m_Recorder.TakeSnapshot(Time.deltaTime);
		}
		else if (m_Recorder.isRecording)
		{
			// "record" is off, but we were recording:
			// save to clip and clear recording.
			m_Recorder.SaveToClip(clip);
			m_Recorder.ResetRecording();
		}
	}
#endif
}
