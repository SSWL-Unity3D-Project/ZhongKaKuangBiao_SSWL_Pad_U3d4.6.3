using UnityEngine;
using System.Collections;

public class danqiTishi : MonoBehaviour {
	public GameObject tishiObj = null;
	public GameObject[] bianhuaObj;
	public TweenScale[] bianhuaTSObj;
	public GameObject[] danqiPingObj;
	public UISprite[] danqiPingUISObj;

	public int danqiNum = 0;
	public int danqiMaxNum = 0;
	private float useTime = 0.0f;

	private bool isSpeeding = false;
	private int changingIndex = -1;
	
	void Awake()
	{gameObject.SetActive (false);
		pcvr.danqitishiSObj = GetComponent<danqiTishi>();
	}

	// Use this for initialization
	void Start () {
		if (tishiObj)
		{
			tishiObj.SetActive(false);
		}

		danqiMaxNum = bianhuaObj.Length;
		useTime = 0;

		for (int i=0; i<bianhuaObj.Length; i++)
		{
			bianhuaObj[i].SetActive(false);
			bianhuaTSObj[i].enabled = false;
			danqiPingObj[i].SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update1 () {
		if (isSpeeding)
		{
			danqiPingUISObj[changingIndex].fillAmount = pcvr.playerTruckSObj.jiasuCurTime / pcvr.playerTruckSObj.jiasuTotalTime;
		}
	}

	public void getOneDanqi()
	{return;
		/*if (danqiNum >= danqiMaxNum || danqiMaxNum <= 0 || !pcvr.danqitishiSObj)
		{
			return;
		}

		if (!pcvr.isDanqitishi)
		{
			pcvr.isDanqitishi = true;
			tishiObj.SetActive(true);

			Invoke("hideTishi", 5.0f);
		}

		danqiNum ++;
		showAddDanqi ();*/
	}

	void hideTishi()
	{
		tishiObj.SetActive(false);
	}
	
	void showAddDanqi()
	{
		bool shouldSub = false;

		if (isSpeeding)
		{
			if (danqiNum >= danqiMaxNum)
			{
				pcvr.playerTruckSObj.speedingHere ();
				shouldSub = true;
			}
			else
			{
				changingIndex --;
				danqiPingObj[changingIndex].SetActive(true);
			}
		}

		int index = danqiMaxNum - danqiNum;

		bianhuaObj[index].SetActive(true);
		danqiPingUISObj[index].fillAmount = 1.0f;
		danqiPingObj[index].SetActive(true);
		
		bianhuaTSObj[index].ResetToBeginning ();
		if (index == 0)
			EventDelegate.Add (bianhuaTSObj[index].onFinished, onFinishedScale0);
		else if (index == 1)
			EventDelegate.Add (bianhuaTSObj[index].onFinished, onFinishedScale1);
		else if (index == 2)
			EventDelegate.Add (bianhuaTSObj[index].onFinished, onFinishedScale2);
		bianhuaTSObj[index].enabled = true;
		bianhuaTSObj[index].PlayForward ();

		//Debug.Log ("showwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwww   " + index);

		if (shouldSub)
		{
			danqiNum --;
		}
	}
	
	void onFinishedScale0()
	{
		int index = 0;
		
		bianhuaObj[index].SetActive(false);
		bianhuaTSObj[index].enabled = false;
		EventDelegate.Remove(bianhuaTSObj[index].onFinished, onFinishedScale0);
	}
	
	void onFinishedScale1()
	{
		int index = 1;
		
		bianhuaObj[index].SetActive(false);
		bianhuaTSObj[index].enabled = false;
		EventDelegate.Remove(bianhuaTSObj[index].onFinished, onFinishedScale1);
	}
	
	void onFinishedScale2()
	{
		int index = 2;
		
		bianhuaObj[index].SetActive(false);
		bianhuaTSObj[index].enabled = false;
		EventDelegate.Remove(bianhuaTSObj[index].onFinished, onFinishedScale2);
	}

	public void useOneDanqi()
	{return;
		/*if (danqiNum <= 0 || danqiMaxNum <= 0 || !pcvr.danqitishiSObj || (Time.time - useTime < 2.0f && useTime > 0))
		{
			return;
		}

		useTime = Time.time;
		
		if (tishiObj.activeSelf)
		{
			tishiObj.SetActive(false);
		}

		int index = danqiMaxNum - danqiNum;
		//danqiPingObj[index].SetActive(false);

		changingIndex = index;

		if (isSpeeding)
		{
			danqiPingObj[index - 1].SetActive(false);
		}
		
		//Debug.Log ("useeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee   " + index);
		danqiNum --;

		isSpeeding = true;
		pcvr.playerTruckSObj.speedingHere ();*/
	}

	public void SpeedingEnd()
	{return;
		/*isSpeeding = false;
		danqiPingObj [changingIndex].SetActive (false);*/
	}
}
