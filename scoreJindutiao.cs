using UnityEngine;
using System.Collections;

public class scoreJindutiao : MonoBehaviour {
	private float delayTime = 1.0f;
	private float delayTimeStatic = 0.5f;
	private bool isBeginMove1 = false;
	private bool isBeginMove2 = false;
	private float moveSpeed = 0.5f;
	private float fillValue1 = 0.0f;
	private float fillValue2 = 0.0f;

	private UISprite UISobj  = null;
	// Use this for initialization
	void Start () {

		UISobj = GetComponent<UISprite>();
	}
	
	// Update is called once per frame
	void Update () {

		if (isBeginMove1)
		{
			if (delayTime > 0)
			{
				delayTime -= Time.deltaTime;
			}
			else
			{
				if (UISobj.fillAmount < fillValue1)
				UISobj.fillAmount += Time.deltaTime * moveSpeed;
				else
				{
					isBeginMove1 = false;
					pcvr.scoreCtrlSobj.finishedPartOne();
				}
			}
		}
		else if (isBeginMove2)
		{
			if (delayTime > 0)
			{
				delayTime -= Time.deltaTime;
			}
			else
			{
				if (UISobj.fillAmount < fillValue2)
					UISobj.fillAmount += Time.deltaTime * moveSpeed;
				else
				{
					isBeginMove2 = false;
					pcvr.scoreCtrlSobj.finishedJindutiao();
				}
			}
		}
	
	}

	public void initFillAmount(float value)
	{Debug.Log ("iiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiamount  " + value);
		UISobj = GetComponent<UISprite>();
		UISobj.fillAmount = value;
	}

	public void beginMove1(float value)
	{
		isBeginMove1 = true;
		fillValue1 = value;
		delayTime = delayTimeStatic;
	}
	
	public void beginMove2(float value)
	{
		isBeginMove2 = true;
		fillValue2 = value;
		delayTime = delayTimeStatic;
		UISobj.fillAmount = 0;
	}

	public void changeMoveSpeed(float speed)
	{
		if (speed <= 0)
		{
			speed = 1.0f;
		}

		moveSpeed = speed;
	}
}
