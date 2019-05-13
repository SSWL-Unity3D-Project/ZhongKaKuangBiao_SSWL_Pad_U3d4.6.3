using UnityEngine;
using System.Collections;
using System;

public class singleSelectTruck : MonoBehaviour {
		
	public Transform[] truckprefabArr;
	private Transform[] truckTransformArr;
	private truckRotate[] truckRotateSObj;
	public Transform pointTruck = null;
	private Transform[] pointArr;
	public int[] levelTruckGroupIndex;
	public UISprite chemingSprite = null;
	public UISprite[] nengliangSpriteArr;
	public int[] jisuArr;
	public int[] jiasuArr;
	public int[] caokongArr;
	public int[] piaoyiArr;
	public GameObject[] yemianObjArr;

	private bool moveLeft = true;
	private bool jinyouxile = false;

	private int curIndex = 0;
	private int truckGroupIndex = 1;
	public int moveStateHere = 0;

	// Use this for initialization
	void Start () {
		truckRotateSObj = new truckRotate[8];
		truckTransformArr = new Transform[4];
		moveStateHere = 0;

		//pcvr.selectLevelSIndex;	// level index
		//levelTruckGroupIndex[pcvr.selectLevelSIndex]	//truck group index

		truckGroupIndex = levelTruckGroupIndex[pcvr.selectLevelSIndex];

		if (truckGroupIndex <= 0 || truckGroupIndex > 2)
		{
			truckGroupIndex = 1;
		}

		for (int i = 0; i < 4; i++)
		{//Quaternion rot = truckprefabArr[i + (truckGroupIndex - 1) * 4].rotation;
			Quaternion rot = pointTruck.rotation;
			//truckTransformArr[i] = Instantiate(truckprefabArr[i + (truckGroupIndex - 1) * 4], Vector3.zero, Quaternion.identity) as Transform;;
			truckTransformArr[i] = Instantiate(truckprefabArr[i + (truckGroupIndex - 1) * 4], Vector3.zero, rot) as Transform;;
		}
		
		for (int i=0; i<truckTransformArr.Length; i++)
		{
			truckRotateSObj[i] = truckTransformArr[i].GetComponent<truckRotate>();
		}

		moveLeft = true;
		curIndex = 0;
		//truckRotateSObj [curIndex].setPositionFrom (pointArr[1]);
		truckRotateSObj [curIndex].setPositionNow (pointTruck);
		chemingSprite.spriteName = "name" + (curIndex + 1 + (truckGroupIndex - 1) * 4).ToString();
		
		changeYemian(curIndex);
		changeNengliangtiao (curIndex);
		
		selectTotalTime = 180.3f;
		selectpanJiangeTime = selectpanJiangeSTime;
		selectFangxiangIndex = 0;
		
		selectTimeObj1.SetActive (true);
		selectTimeObj2.SetActive (true);
		selectTimeObj3.SetActive (false);
		InputEventCtrl.GetInstance().ClickStartBtOneEvent += ClickStartBtOneEvent;
        InputEventCtrl.GetInstance().ClickTVYaoKongLeftBtEvent += ClickTVYaoKongLeftBtEvent;
        InputEventCtrl.GetInstance().ClickTVYaoKongRightBtEvent += ClickTVYaoKongRightBtEvent;

        if (pcvr.sound2DScrObj)
		{
			pcvr.sound2DScrObj.playAudioXuanguanBeijing(true);
		}
	}

    private void ClickTVYaoKongLeftBtEvent(ButtonState val)
    {
        OnPlayerClickLeftRightBt(val, SelectEnum.Left);
    }

    private void ClickTVYaoKongRightBtEvent(ButtonState val)
    {
        OnPlayerClickLeftRightBt(val, SelectEnum.Right);
    }

    enum SelectEnum
    {
        Left = 0,
        Right = 1,
    }
    void OnPlayerClickLeftRightBt(ButtonState val, SelectEnum type)
    {
        if (SSGameUIRoot.GetInstance().m_SSExitGameUI != null)
        {
            return;
        }

        if (jinyouxile)
        {
            return;
        }

        if (val == ButtonState.DOWN)
        {
            moveStateHere = 0;
            return;
        }

        if (spaceTime > 0)
        {
            return;
        }

        //select truck
        if ((moveStateHere == 0 || moveStateHere == 2) && type == SelectEnum.Left)
        {//left
         //do some thing
            moveToLeft();
            moveStateHere = 1;
        }
        else if ((moveStateHere == 0 || moveStateHere == 1) && type == SelectEnum.Right)
        {//right
         //do some thing
            moveToRight();
            moveStateHere = 2;
        }
        //else if ((pcvr.bIsHardWare && Mathf.Abs(pcvr.mGetSteer) <= 0.5f) || (!pcvr.bIsHardWare && Mathf.Abs(Input.GetAxis("Horizontal")) <= 0.5f))
        //{
        //    moveStateHere = 0;
        //}
    }

    // Update is called once per frame
    void Update () {
		if (jinyouxile)
		{
			return;
		}

		if (truckTransformArr.Length <= 0 || !pointTruck/* || pointArr.Length <= 0*/)
		{
			return;
		}
		
		if (selectTotalTime > 0)
		{
			selectTotalTime -= Time.deltaTime;
		}
		else
		{
			ClickStartBtOneEvent(ButtonState.UP);
		}
		
		showTimeDownSelect(selectTotalTime);
		
		if (selectpanJiangeTime > 0)
		{
			selectpanJiangeTime -= Time.deltaTime;
		}
		else
		{
			selectpanJiangeTime = selectpanJiangeSTime;
			
			changeSelectFangxiangS(++ selectFangxiangIndex);
		}

		if (spaceTime <= 0)
		{
			//select truck
			//if ((moveStateHere == 0 || moveStateHere == 2) && ((pcvr.bIsHardWare && pcvr.mGetSteer < -0.5f) || (!pcvr.bIsHardWare && Input.GetAxis("Horizontal") < -0.5f)))
			//{//left
			//	//do some thing
			//	moveToLeft();
			//	moveStateHere = 1;
			//}
			//else if ((moveStateHere == 0 || moveStateHere == 1) && ((pcvr.bIsHardWare && pcvr.mGetSteer > 0.5f) || (!pcvr.bIsHardWare && Input.GetAxis("Horizontal") > 0.5f)))
			//{//right
			//	//do some thing
			//	moveToRight();
			//	moveStateHere = 2;
			//}
			//else if ((pcvr.bIsHardWare && Mathf.Abs(pcvr.mGetSteer) <= 0.5f) || (!pcvr.bIsHardWare && Mathf.Abs(Input.GetAxis("Horizontal")) <= 0.5f))
			//{
			//	moveStateHere = 0;
			//}
		}
		else
		{
			//if ((pcvr.bIsHardWare && Mathf.Abs(pcvr.mGetSteer) <= 0.5f) || (!pcvr.bIsHardWare && Mathf.Abs(Input.GetAxis("Horizontal")) <= 0.5f))
			//{
			//	moveStateHere = 0;
			//}
			spaceTime -= Time.deltaTime;
		}
	}
	
	void moveToRight()
	{
		moveLeft = true;
		spaceTime = spaceTimeStatic;
		
		playAudioXuanguanLinkGui();
		
		truckRotateSObj [curIndex].resetPosition ();
		
		curIndex ++;
		
		if (curIndex >= 4)
		{
			curIndex = 0;
		}
		
		changeYemian(curIndex);
		changeNengliangtiao (curIndex);

		Invoke ("changeTruck", 0.1f);
	}
	
	void moveToLeft()
	{
		moveLeft = false;
		spaceTime = spaceTimeStatic;
		
		playAudioXuanguanLinkGui();
		
		truckRotateSObj [curIndex].resetPosition ();
		
		curIndex --;
		
		if (curIndex < 0)
		{
			curIndex = 3;
		}

		changeYemian(curIndex);
		changeNengliangtiao (curIndex);

		Invoke ("changeTruck", 0.1f);
	}

	void changeTruck()
	{
		truckRotateSObj [curIndex].setPositionNow (pointTruck);
		
		pcvr.selectTruckSIndex = curIndex;
		chemingSprite.spriteName = "name" + (curIndex + 1 + (truckGroupIndex - 1) * 4).ToString();
	}
	
	void changeYemian(int index)
	{
		/*if (index == 1)
		{
			index = 3;
		}
		else if (index == 3)
		{
			index = 1;
		}*/

		for(int i=0; i<4; i++)
		{
			if (index == i)
			{
				yemianObjArr[i].SetActive(true);
			}
			else
			{
				yemianObjArr[i].SetActive(false);
			}
		}
	}

	void changeNengliangtiao(int index)
	{
		index = index + (truckGroupIndex - 1) * 4;

		nengliangSpriteArr [0].spriteName = "SXZhiValue" + jisuArr[index].ToString();
		nengliangSpriteArr [1].spriteName = "SXZhiValue" + jiasuArr[index].ToString();
		nengliangSpriteArr [2].spriteName = "SXZhiValue" + caokongArr[index].ToString();
		nengliangSpriteArr [3].spriteName = "SXZhiValue" + piaoyiArr[index].ToString();
	}

	void moveToLeftOLD()
	{
		moveLeft = true;
		spaceTime = spaceTimeStatic;
		
		playAudioXuanguanLinkGui();

		truckRotateSObj [curIndex].setTargetObj (pointArr[0]);

		curIndex ++;

		if (curIndex >= 4)
		{
			curIndex = 0;
		}

		truckRotateSObj [curIndex].setPositionFrom (pointArr[2]);
		truckRotateSObj [curIndex].setTargetObj (pointArr[1]);

		pcvr.selectTruckSIndex = curIndex;
		chemingSprite.spriteName = "name" + (curIndex + 1 + (truckGroupIndex - 1) * 4).ToString();
	}

	void moveToRightOLD()
	{
		moveLeft = false;
		spaceTime = spaceTimeStatic;
		
		playAudioXuanguanLinkGui();
		
		truckRotateSObj [curIndex].setTargetObj (pointArr[2]);

		curIndex --;
		
		if (curIndex < 0)
		{
			curIndex = 3;
		}
		
		truckRotateSObj [curIndex].setPositionFrom (pointArr [0]);
		truckRotateSObj [curIndex].setTargetObj (pointArr[1]);
		
		pcvr.selectTruckSIndex = curIndex;
		chemingSprite.spriteName = "name" + (curIndex + 1 + (truckGroupIndex - 1) * 4).ToString();
	}
	
	//keyboard - K - start game - only for link gui
	void ClickStartBtOneEvent(ButtonState val)
	{//Debug.Log ("startbuttonnnnnnaaaaaannnnnnnnnnnnnnnnnn  linkguiiii " + val + " " +pcvr.firstPlayerIndex + " "+ Network.isClient + " "+ Network.player + " "+ jinyouxile);
		if (jinyouxile)
		{
			return;
		}
		
		if (val == ButtonState.DOWN)
		{
			return;
		}
        
        if (SSGameUIRoot.GetInstance().IsHaveExitGameUI == true)
        {
            if (selectTotalTime <= 0f)
            {
                //选择卡车倒计时结束.
            }
            else
            {
                if (SSGameUIRoot.GetInstance().m_SSExitGameUI == null)
                {
                    SSGameUIRoot.GetInstance().IsHaveExitGameUI = false;
                }
                return;
            }
        }

        if (SSGameUIRoot.GetInstance().m_SSExitGameUI != null)
        {
            if (selectTotalTime <= 0f)
            {
                //选择卡车倒计时结束.
                SSGameUIRoot.GetInstance().RemoveSelf();
            }
            else
            {
                return;
            }
        }

		jinyouxile = true;
        InputEventCtrl.GetInstance().ClickTVYaoKongLeftBtEvent -= ClickTVYaoKongLeftBtEvent;
        InputEventCtrl.GetInstance().ClickTVYaoKongRightBtEvent -= ClickTVYaoKongRightBtEvent;
        InputEventCtrl.GetInstance().ClickStartBtOneEvent -= ClickStartBtOneEvent;

        if (Network.isClient)
		{
			if (pcvr.firstPlayerIndex == int.Parse(Network.player + ""))
			{
				for (int i = 0; i < 4; i++)
				{
					Destroy(truckTransformArr[i].gameObject);
				}
				
				playAudioStartLinkGui();
				
				//send message to other players to begin game
				pcvr.selfServerScrObj.ServerReceiveSelectTruck(pcvr.selectLevelFinalLink);
				
				gameObject.SetActive (false);

			}
		}
		else
		{
			for (int i = 0; i < 4; i++)
			{
				Destroy(truckTransformArr[i].gameObject);
			}
			
			playAudioStartLinkGui();

			pcvr.donghuaScriObj.singleModeBegin(2, (pcvr.selectLevelSIndex + 2));
			
			gameObject.SetActive (false);
		}
	}
	
	void playAudioStartLinkGui()
	{
		if (pcvr.sound2DScrObj)
		{
			pcvr.sound2DScrObj.playAudioXuanguanQueren();
		}
	}
	
	void playAudioStartInsert()
	{
		if (pcvr.sound2DScrObj)
		{
			pcvr.sound2DScrObj.playAudioStart();
		}
	}
	
	//move left or right
	void playAudioXuanguanLinkGui()
	{
		if (pcvr.sound2DScrObj)
		{
			pcvr.sound2DScrObj.playAudioXuanguanYidong();
		}
	}
	
	private float spaceTime = 0;
	private float spaceTimeStatic = 0.40f;		//the space time of moving to left or right

	//about some down time
	public GameObject selectTimeObj1 = null;
	public GameObject selectTimeObj2 = null;
	public GameObject selectTimeObj3 = null;
	public UISprite selectTimeSObj1 = null;
	public UISprite selectTimeSObj2 = null;
	public UISprite selectTimeSObj3 = null;
	private float selectTotalTime = 30.0f;
	
	private int timeNow = 0;
	
	void showTimeDownSelect(float timeT)
	{
		if (timeT < 0)
		{
			timeT = 0;
		}
		
		timeNow = Mathf.FloorToInt(timeT);

        if (timeNow > 99)
        {
            if (selectTimeSObj1.enabled == true)
            {
                selectTimeSObj1.enabled = false;
                selectTimeSObj2.enabled = false;
            }
        }
        else if (timeNow >= 10)
        {
            if (selectTimeSObj1.enabled == false)
            {
                selectTimeSObj1.enabled = true;
                selectTimeSObj2.enabled = true;
            }
            selectTimeSObj1.spriteName = Mathf.FloorToInt(timeNow / 10).ToString();
			selectTimeSObj2.spriteName = Mathf.FloorToInt(timeNow % 10).ToString();
		}
		else
		{
			selectTimeObj1.SetActive (false);
			selectTimeObj2.SetActive (false);
			selectTimeObj3.SetActive (true);
			selectTimeSObj3.spriteName = timeNow.ToString();
		}
	}
	
	//fangxiangpan
	public UISprite selectFangxiang = null;
	private int selectFangxiangIndex = 0;
	private int fangxiangIndexTemp = 0;
	private string fangxiangNameTemp = "";
	private float selectpanJiangeTime = 0.5f;
	private float selectpanJiangeSTime = 0.2f;
	
	void changeSelectFangxiangS(int index)
	{
		fangxiangIndexTemp = index % 4;
		switch(fangxiangIndexTemp)
		{
		case 0:
			fangxiangNameTemp = "fangxiangpan1";
			break;
		case 1:
			fangxiangNameTemp = "fangxiangpan3";
			break;
		case 2:
			fangxiangNameTemp = "fangxiangpan1";
			break;
		case 3:
			fangxiangNameTemp = "fangxiangpan2";
			break;
		}
		
		selectFangxiang.spriteName = fangxiangNameTemp;
	}
}

