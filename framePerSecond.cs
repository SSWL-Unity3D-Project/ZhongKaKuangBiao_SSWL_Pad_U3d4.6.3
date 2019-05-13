using UnityEngine;
using System.Collections;
/*Attach this to a GUIText to make a frames/second indicator.
It calculates frames/second over each updateInterval,
so the display does not keep changing wildly.
 */

public class framePerSecond : MonoBehaviour {
	
	
	private float updateInterval = 0.5f;
	private float accum = 0.0f; // FPS accumulated over the interval
	private int frames = 0; // Frames drawn over the interval
	private float timeleft; // Left time for current interval
	
	private float FPS= 0.0f;
	//public GUIText FPS_Text_Ref;
	public UILabel FPSUILable;

	// Use this for initialization
	void Start () {gameObject.SetActive (false);
		//Application.targetFrameRate = 65;
		timeleft = updateInterval;
		//FPS_Text_Ref = GetComponent(GUIText) as GUIText;
	}
	
	// Update is called once per frame
	void Update () {
		timeleft -= Time.deltaTime;
		accum += Time.timeScale/Time.deltaTime;
		++frames;
		
		// Interval ended - update GUI text and start new interval
		if( timeleft <= 0.0f )
		{
			// display two fractional digits (f2 format)
			//Debug.Log(accum + " "+ frames + " "+ Time.deltaTime);
			FPS = (accum/frames);
			timeleft = updateInterval;
			accum = 0.0f;
			frames = 0;
			//FPS_Text_Ref.text = System.String.Empty+ Mathf.FloorToInt(FPS);
			FPSUILable.text = System.String.Empty+ Mathf.FloorToInt(FPS);
		}
	}
}