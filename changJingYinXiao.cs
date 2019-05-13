using UnityEngine;
using System.Collections;

public class changJingYinXiao : MonoBehaviour {
	private AudioSource myAudioSource;
	// Use this for initialization
	void Start () {
		myAudioSource = (AudioSource)GetComponent(typeof(AudioSource));

		if (myAudioSource)
		{
			myAudioSource.playOnAwake = false;
			Invoke("beginPlay", 1.0f);
		}
	}

	void beginPlay()
	{
		if (pcvr.uiRunState >= 2)
		{
			myAudioSource.loop = true;
			myAudioSource.Play ();
			return;
		}
		else
		{
			Invoke("beginPlay", 1.0f);
		}
	}

	void Update()
	{
		if (myAudioSource 
		    && (pcvr.uiRunState == 10 || pcvr.uiRunState == 3 || pcvr.isPassgamelevel) 
		    && myAudioSource.isPlaying)
		{
			myAudioSource.Stop();
		}
	}
}
