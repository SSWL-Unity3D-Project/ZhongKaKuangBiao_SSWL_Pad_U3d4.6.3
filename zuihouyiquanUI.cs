using UnityEngine;
using System.Collections;

public class zuihouyiquanUI : MonoBehaviour {
	
	public TweenPosition zuihouyiquanPos1 = null;
	public TweenPosition zuihouyiquanPos2 = null;
	public TweenAlpha zuihouyiquanAlpha = null;

	// Use this for initialization
	void Start () {gameObject.SetActive (false);
		zuihouyiquanPos1.enabled = false;
		zuihouyiquanPos2.enabled = false;
		zuihouyiquanAlpha.enabled = false;

		showZuihouyiquan ();
	}
	
	public void showZuihouyiquan()
	{
		zuihouyiquanPos1.ResetToBeginning ();
		EventDelegate.Add (zuihouyiquanPos1.onFinished, onFinishedPos1);
		zuihouyiquanPos1.enabled = true;
		zuihouyiquanPos1.PlayForward ();
	}
	
	void onFinishedPos1()
	{
		zuihouyiquanPos1.enabled = false;
		EventDelegate.Remove(zuihouyiquanPos1.onFinished, onFinishedPos1);
		
		zuihouyiquanAlpha.enabled = true;
		
		Invoke ("hideZuihouyiquanAlpha", 2.0f);
	}
	
	void hideZuihouyiquanAlpha()
	{
		zuihouyiquanAlpha.enabled = false;
		
		zuihouyiquanPos2.ResetToBeginning ();
		EventDelegate.Add (zuihouyiquanPos2.onFinished, onFinishedPos2);
		zuihouyiquanPos2.enabled = true;
		zuihouyiquanPos2.PlayForward ();
	}
	
	void onFinishedPos2()
	{
		zuihouyiquanPos2.enabled = false;
		EventDelegate.Remove(zuihouyiquanPos2.onFinished, onFinishedPos2);
		
		if (pcvr.sound2DScrObj)
		{
			pcvr.sound2DScrObj.playAudioZuihouyiquan (false);
		}
		
		gameObject.SetActive(false);
	}
}
