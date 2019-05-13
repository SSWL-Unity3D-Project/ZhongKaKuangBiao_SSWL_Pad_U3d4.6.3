using UnityEngine;
using System.Collections;

public class zuoyiUI : MonoBehaviour {
	
	public float jiangeTime = 3.0f;
	private float totalTime = 0;
	private int state = 0;
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
			totalTime = 0;
			
			if (state == 0)
			{
				state = 1;
				spriteObj.spriteName = "zuoyi2";
			}
			else if (state == 1)
			{
				state = 2;
				spriteObj.spriteName = "zuoyi3";
			}
			else
			{
				state = 0;
				spriteObj.spriteName = "zuoyi1";
			}
		}
	}
}
