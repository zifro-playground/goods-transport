using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.Animations;

public class HierarchyRecorder : MonoBehaviour
{
	// The clip the recording is going to be saved to.
	public AnimationClip clip;

	// Checkbox to start/stop the recording.
	public bool record = false;

	// The main feature: the actual recorder.
	private GameObjectRecorder m_Recorder;

	void Start()
	{
		// Create the GameObjectRecorder.
		m_Recorder = new GameObjectRecorder();
		m_Recorder.root = gameObject;

		// Set it up to record the transforms recursively.
		m_Recorder.BindComponent<Transform>(gameObject, true);
	}

	// The recording needs to be done in LateUpdate in order
	// to be done once everything has been updated
	// (animations, physics, scripts, etc.).
	void LateUpdate()
	{
		if (clip == null)
			return;

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
}