using UnityEngine;
using System.Collections;

public class startbutton : MonoBehaviour {
	public float jiangeTime = 0.5f;
	private float totalTime = 0;
	private bool isFirst = false;
	private UISprite spriteObj = null;
	// Use this for initialization
	void Start () {
		spriteObj = GetComponent<UISprite>();
	}
	
	// Update is called once per frame
	void Update () {

		totalTime += Time.deltaTime;

		if (totalTime >= jiangeTime)
		{
			isFirst = !isFirst;
			totalTime = 0;
	
			if (isFirst)
			{
				spriteObj.spriteName = "uiStart2";
			}
			else
			{
				spriteObj.spriteName = "uiStart1";
			}
		}
	}
}
