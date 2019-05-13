using UnityEngine;
using System.Collections;
using System;

public class singleSelectGui : MonoBehaviour {

	public GameObject linkUIObj = null;
	public GameObject selectTruckObj = null;
	public bool moveLeft = true;

	//select level
	private int totalLevelNum = 7;	//8-6	the total game-level number
	private int selectLevelIndex = 0;		//should judge, use this to change the sprite
	private int selectLevelNOChange = 0;	//will ++ or --, and not adjust
	private int selectLevelFinal = 0;		//will send this as the game level and start game
	private bool jinyouxile = false;
	public int moveStateHere = 0;
	private bool isShijiandao = false;
	
	public GameObject tishiParentObj = null;
	public GameObject leftYesObj = null;
	public GameObject rightNoObj = null;
	private bool isTishi = false;
	private bool isYes = false;

	// Use this for initialization
	void Start()
    {
        if (XKGameVersionCtrl.IsInit == false)
        {
            GameObject obj = new GameObject("_GameVersion");
            XKGameVersionCtrl gameVersionCom = obj.AddComponent<XKGameVersionCtrl>();
            if (gameVersionCom != null)
            {
                gameVersionCom.Init();
            }
        }

        if (Screen.width != 1280 || Screen.height != 720 || Screen.fullScreen != false)
        {
            Screen.SetResolution(1280, 720, false);
        }
        Debug.Log("dao link gui leeeeee");
		pcvr.GetInstance().CloseFangXiangPanPower();
		jinyouxile = false;
		moveStateHere = 0;
		isShijiandao = false;

		selectTotalTime = 180.3f;

		selectpanJiangeTime = selectpanJiangeSTime;

		selectFangxiangIndex = 0;

		//pcvr.guakaDengji [0] = 1;


		selectTimeObj1.SetActive (true);
		selectTimeObj2.SetActive (true);
		selectTimeObj3.SetActive (false);

		tishiParentObj.SetActive (false);
		leftYesObj.SetActive (false);
		rightNoObj.SetActive (false);

		mCurSelectScriptObj1 = mSelectScriptObj1;
		mCurSelectScriptObj2 = mSelectScriptObj2;
		mCurSelectScriptObj2A = mSelectScriptObj2A;
		mCurSelectScriptObj3 = mSelectScriptObj3;
		mCurSelectScriptObj3A = mSelectScriptObj3A;
		
		mCurSelectLTWScaleScript1 = mSelectLTWScaleScript1;
		mCurSelectLTWScaleScript2 = mSelectLTWScaleScript2;
		mCurSelectLTWScaleScript2A = mSelectLTWScaleScript2A;
		mCurSelectLTWScaleScript3 = mSelectLTWScaleScript3;
		mCurSelectLTWScaleScript3A = mSelectLTWScaleScript3A;

		changeDengjiName (mCurSelectScriptObj1, 0);
		changeDengjiName (mCurSelectScriptObj2, 1);
		changeDengjiName (mCurSelectScriptObj2A, 1);
		changeDengjiName (mCurSelectScriptObj3, 7);
		changeDengjiName (mCurSelectScriptObj3A, 7);
		
		mCurSelectLTWPosScript1 = mSelectLTWPosScript1;
		mCurSelectLTWPosScript2 = mSelectLTWPosScript2;
		mCurSelectLTWPosScript2A = mSelectLTWPosScript2A;
		mCurSelectLTWPosScript3 = mSelectLTWPosScript3;
		mCurSelectLTWPosScript3A = mSelectLTWPosScript3A;
		
		initFirst ();
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

        if (val == ButtonState.DOWN)
        {
            moveStateHere = 0;
            return;
        }

        if (spaceTime > 0)
        {
            return;
        }

        if (selectTruckObj.activeInHierarchy == true)
        {
            return;
        }

        //select level
        if ((moveStateHere == 0 || moveStateHere == 2) && type == SelectEnum.Left)
        {//left
            moveLeft = true;
            spaceTime = spaceTimeStatic;
            moveStateHere = 1;

            if (isTishi)
            {
                playAudioXuanguanLinkGui();

                leftYesObj.SetActive(true);
                rightNoObj.SetActive(false);

                isYes = true;
                return;
            }

            //here will judge the level number
            selectLevelIndex--; selectLevelNOChange--;
            if (selectLevelIndex < 0)
            {
                selectLevelIndex = totalLevelNum - 1;
            }

            if (selectLevelNOChange >= 0)
            {
                selectLevelFinal = totalLevelNum - selectLevelNOChange % totalLevelNum;

                if (selectLevelFinal == totalLevelNum)
                {
                    selectLevelFinal = 0;
                }
            }
            else
            {
                selectLevelFinal = Mathf.Abs(selectLevelNOChange) % totalLevelNum;

                if (selectLevelFinal == totalLevelNum)
                {
                    selectLevelFinal = 0;
                }
            }

            //Debug.Log("lefttttttttttMovessssssssssssssssssssssssXuanzeeeeeeeeeeeeeeee  " + selectLevelIndex.ToString() + " "+ selectLevelNOChange + " " + (selectLevelFinal + 1));

            playAudioXuanguanLinkGui();

            moveToLeftLe();
        }
        else if ((moveStateHere == 0 || moveStateHere == 1) && type == SelectEnum.Right)
        {//right
            moveLeft = false;
            spaceTime = spaceTimeStatic;
            moveStateHere = 2;

            if (isTishi)
            {
                playAudioXuanguanLinkGui();

                leftYesObj.SetActive(false);
                rightNoObj.SetActive(true);

                isYes = false;
                return;
            }

            selectLevelIndex++; selectLevelNOChange++;
            if (selectLevelIndex > totalLevelNum - 1)
            {
                selectLevelIndex = 0;
            }

            if (selectLevelNOChange >= 0)
            {
                selectLevelFinal = totalLevelNum - selectLevelNOChange % totalLevelNum;

                if (selectLevelFinal == totalLevelNum)
                {
                    selectLevelFinal = 0;
                }
            }
            else
            {
                selectLevelFinal = Mathf.Abs(selectLevelNOChange) % totalLevelNum;

                if (selectLevelFinal == totalLevelNum)
                {
                    selectLevelFinal = 0;
                }
            }
            //Debug.Log("rigggggggggggggMovesssssssssssssssssssssssXuanzeeeeeeeeeeeeeeee  " + selectLevelIndex.ToString() +" "+ selectLevelNOChange + " " + (selectLevelFinal + 1));

            playAudioXuanguanLinkGui();

            moveToRightLe();
        }
        //else if ((pcvr.bIsHardWare && Mathf.Abs(pcvr.mGetSteer) <= 0.5f) || (!pcvr.bIsHardWare && Mathf.Abs(Input.GetAxis("Horizontal")) <= 0.5f))
        //{
        //    moveStateHere = 0;
        //}
    }

    void changeDengjiName(UISprite UISpriteObj, int index)
	{//Debug.Log ("cccccccccccccccccccccccccccccccccccccccccccccc   " + index);
		UISpriteObj.transform.GetChild(0).GetComponent<UISprite>().spriteName = "dengji" + pcvr.guakaDengji[index].ToString ();
	}

	void changeDengjiNameWithName(UISprite UISpriteObj, string name)
	{
		UISpriteObj.spriteName = name;
	}

	void initFirst()
	{
		moveLeft = true;
		selectLevelIndex = 0;
		selectLevelNOChange = 0;
		selectLevelFinal = 0;
		moveStateHere = 0;
	}

	// Update is called once per frame
	void Update ()
    {
		if (jinyouxile)
		{
			return;
		}

		if (selectTotalTime > 0)
		{
			selectTotalTime -= Time.deltaTime;
		}
		else
		{
			isShijiandao = true;
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

		//test	here will move up	20170523
		if (spaceTime <= 0)
		{
			//select level
			//if ((moveStateHere == 0 || moveStateHere == 2) && ((pcvr.bIsHardWare && pcvr.mGetSteer < -0.5f) || (!pcvr.bIsHardWare && Input.GetAxis("Horizontal") < -0.5f)))
			//{//left
			//	moveLeft = true;
			//	spaceTime = spaceTimeStatic;
			//	moveStateHere = 1;
				
			//	if (isTishi)
			//	{
			//		playAudioXuanguanLinkGui();

			//		leftYesObj.SetActive(true);
			//		rightNoObj.SetActive(false);

			//		isYes = true;
			//		return;
			//	}
				
			//	//here will judge the level number
			//	selectLevelIndex --; selectLevelNOChange--;
			//	if (selectLevelIndex < 0)
			//	{
			//		selectLevelIndex = totalLevelNum - 1;
			//	}
				
			//	if (selectLevelNOChange >= 0)
			//	{
			//		selectLevelFinal = totalLevelNum - selectLevelNOChange % totalLevelNum;
					
			//		if (selectLevelFinal == totalLevelNum)
			//		{
			//			selectLevelFinal = 0;
			//		}
			//	}
			//	else
			//	{
			//		selectLevelFinal = Mathf.Abs(selectLevelNOChange) % totalLevelNum;
					
			//		if (selectLevelFinal == totalLevelNum)
			//		{
			//			selectLevelFinal = 0;
			//		}
			//	}
				
			//	//Debug.Log("lefttttttttttMovessssssssssssssssssssssssXuanzeeeeeeeeeeeeeeee  " + selectLevelIndex.ToString() + " "+ selectLevelNOChange + " " + (selectLevelFinal + 1));
				
			//	playAudioXuanguanLinkGui();
				
			//	moveToLeftLe();
			//}
			//else if ((moveStateHere == 0 || moveStateHere == 1) && ((pcvr.bIsHardWare && pcvr.mGetSteer > 0.5f) || (!pcvr.bIsHardWare && Input.GetAxis("Horizontal") > 0.5f)))
			//{//right
			//	moveLeft = false;
			//	spaceTime = spaceTimeStatic;
			//	moveStateHere = 2;

			//	if (isTishi)
			//	{
			//		playAudioXuanguanLinkGui();
					
			//		leftYesObj.SetActive(false);
			//		rightNoObj.SetActive(true);

			//		isYes = false;
			//		return;
			//	}
				
			//	selectLevelIndex ++; selectLevelNOChange ++;
			//	if (selectLevelIndex > totalLevelNum - 1)
			//	{
			//		selectLevelIndex = 0;
			//	}
				
			//	if (selectLevelNOChange >= 0)
			//	{
			//		selectLevelFinal = totalLevelNum - selectLevelNOChange % totalLevelNum;
					
			//		if (selectLevelFinal == totalLevelNum)
			//		{
			//			selectLevelFinal = 0;
			//		}
			//	}
			//	else
			//	{
			//		selectLevelFinal = Mathf.Abs(selectLevelNOChange) % totalLevelNum;
					
			//		if (selectLevelFinal == totalLevelNum)
			//		{
			//			selectLevelFinal = 0;
			//		}
			//	}
			//	//Debug.Log("rigggggggggggggMovesssssssssssssssssssssssXuanzeeeeeeeeeeeeeeee  " + selectLevelIndex.ToString() +" "+ selectLevelNOChange + " " + (selectLevelFinal + 1));
				
			//	playAudioXuanguanLinkGui();
				
			//	moveToRightLe();
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
			
			if (!isHalfLe && spaceTime < spaceTimeHalfStatic)
			{
				isHalfLe = true;
				
				if (moveLeft)
				{
					moveToLeftHalfLe();
				}
				else
				{
					moveToRightHalfLe();
				}
			}
		}
	}

	//keyboard - K - start game - only for link gui
	void ClickStartBtOneEvent(ButtonState val)
	{//Debug.Log ("startbuttonnnnnnnnnnnnnnnnnnnnnnnn  linkguiiii select level single " + val);
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
                //选择关卡倒计时结束.
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
                //选择关卡倒计时结束.
            }
            else
            {
                return;
            }
        }

        if ((pcvr.guakaDengji[selectLevelFinal] >= 0 && pcvr.guakaDengji[selectLevelFinal] <= 2) && !isTishi && !isShijiandao)
		{
			tishiParentObj.SetActive(true);
			leftYesObj.SetActive(true);
			rightNoObj.SetActive (false);
			isTishi = true;
			isYes = true;
			return;
		}

		if (isTishi && !isYes && !isShijiandao)
		{
			tishiParentObj.SetActive(false);

			isTishi = false;
			return;
		}

        InputEventCtrl.GetInstance().ClickTVYaoKongLeftBtEvent -= ClickTVYaoKongLeftBtEvent;
        InputEventCtrl.GetInstance().ClickTVYaoKongRightBtEvent -= ClickTVYaoKongRightBtEvent;
        InputEventCtrl.GetInstance().ClickStartBtOneEvent -= ClickStartBtOneEvent;
        linkUIObj.SetActive(false);
		
		//pcvr.donghuaScriObj.singleModeBegin(2, (selectLevelFinal + 2));
		
		playAudioStartLinkGui();

		//into slect truck GUI
		pcvr.selectLevelSIndex = selectLevelFinal;
		selectTruckObj.SetActive (true);
		gameObject.SetActive (false);
	}
	
	void playAudioStartLinkGui()
	{
		if (pcvr.sound2DScrObj)
		{
			//pcvr.sound2DScrObj.playAudioXuanguanQueren();
			pcvr.sound2DScrObj.playAudioStart();
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

	//		3		2
	//
	//			1
	public UISprite mSelectScriptObj1 = null;	//the sprite object
	public UISprite mSelectScriptObj2 = null;
	public UISprite mSelectScriptObj2A = null;
	public UISprite mSelectScriptObj3 = null;
	public UISprite mSelectScriptObj3A = null;

	public UISprite mCurSelectScriptObj1 = null;
	public UISprite mCurSelectScriptObj2 = null;
	public UISprite mCurSelectScriptObj2A = null;
	public UISprite mCurSelectScriptObj3 = null;
	public UISprite mCurSelectScriptObj3A = null;

	public bool isFenge = false;

	public TweenScale mSelectLTWScaleScript1 = null;	//scale 1: 
	public TweenScale mSelectLTWScaleScript2 = null;	//scale 2:
	public TweenScale mSelectLTWScaleScript2A = null;	//scale 2A:
	public TweenScale mSelectLTWScaleScript3 = null;	//scale 3:
	public TweenScale mSelectLTWScaleScript3A = null;	//scale 3A:
	
	public TweenPosition mSelectLTWPosScript1 = null;	//pos 1:
	public TweenPosition mSelectLTWPosScript2 = null;	//pos 2:
	public TweenPosition mSelectLTWPosScript2A = null;	//pos 2A:
	public TweenPosition mSelectLTWPosScript3 = null;	//pos 3:
	public TweenPosition mSelectLTWPosScript3A = null;	//pos 3A:
	
	private TweenScale mCurSelectLTWScaleScript1 = null;
	private TweenScale mCurSelectLTWScaleScript2 = null;
	private TweenScale mCurSelectLTWScaleScript2A = null;
	private TweenScale mCurSelectLTWScaleScript3 = null;
	private TweenScale mCurSelectLTWScaleScript3A = null;
	
	private TweenPosition mCurSelectLTWPosScript1 = null;
	private TweenPosition mCurSelectLTWPosScript2 = null;
	private TweenPosition mCurSelectLTWPosScript2A = null;
	private TweenPosition mCurSelectLTWPosScript3 = null;
	private TweenPosition mCurSelectLTWPosScript3A = null;

	private float spaceTime = 0;
	private float spaceTimeStatic = 0.55f;		//the space time of moving to left or right
	private float spaceTimeHalfStatic = 0.4f;	//less than this time, will change the sprite information
	private bool isHalfLe = false;				//judge whether change

	public Vector3 spriteScale1 = Vector3.zero;	//the scale value 1
	public Vector3 spriteScale2 = Vector3.zero;	//the scale value 0.5
	public Vector3 spriteScale3 = Vector3.zero;	//the scale value 0.5
	public Vector3 spriteScale4 = Vector3.zero;	//the scale value 0

	public Vector3 spritePos1 = Vector3.zero;	//the position
	public Vector3 spritePos2 = Vector3.zero;
	public Vector3 spritePos3 = Vector3.zero;
	public Vector3 spritePos4 = Vector3.zero;

	void beginMoveLe(bool isLeft)
	{//Debug.Log ("tttbeginMoveLebeginMoveLebeginMoveLe  " + isLeft);
		mCurSelectLTWScaleScript1.ResetToBeginning ();

		if (isLeft)
			EventDelegate.Add (mCurSelectLTWScaleScript1.onFinished, onFinishedScaleSelectLLeft);
		else
			EventDelegate.Add (mCurSelectLTWScaleScript1.onFinished, onFinishedScaleSelectLRight);
			
		mCurSelectLTWScaleScript1.enabled = true;
		mCurSelectLTWScaleScript1.PlayForward ();

		mCurSelectLTWScaleScript2.ResetToBeginning ();
		mCurSelectLTWScaleScript2.enabled = true;
		mCurSelectLTWScaleScript2.PlayForward ();
		
		mCurSelectLTWScaleScript3.ResetToBeginning ();
		mCurSelectLTWScaleScript3.enabled = true;
		mCurSelectLTWScaleScript3.PlayForward ();
		
		mCurSelectLTWScaleScript2A.ResetToBeginning ();
		mCurSelectLTWScaleScript2A.enabled = true;
		mCurSelectLTWScaleScript2A.PlayForward ();
		
		mCurSelectLTWScaleScript3A.ResetToBeginning ();
		mCurSelectLTWScaleScript3A.enabled = true;
		mCurSelectLTWScaleScript3A.PlayForward ();
		
		mCurSelectLTWPosScript1.ResetToBeginning ();
		mCurSelectLTWPosScript1.enabled = true;
		mCurSelectLTWPosScript1.PlayForward ();
		
		mCurSelectLTWPosScript2.ResetToBeginning ();
		mCurSelectLTWPosScript2.enabled = true;
		mCurSelectLTWPosScript2.PlayForward ();
		
		mCurSelectLTWPosScript3.ResetToBeginning ();
		mCurSelectLTWPosScript3.enabled = true;
		mCurSelectLTWPosScript3.PlayForward ();
		
		mCurSelectLTWPosScript2A.ResetToBeginning ();
		mCurSelectLTWPosScript2A.enabled = true;
		mCurSelectLTWPosScript2A.PlayForward ();
		
		mCurSelectLTWPosScript3A.ResetToBeginning ();
		mCurSelectLTWPosScript3A.enabled = true;
		mCurSelectLTWPosScript3A.PlayForward ();
	}

	void onFinishedScaleSelectLLeft()
	{
		onFinishedScaleSelectL (true);
	}

	void onFinishedScaleSelectLRight()
	{
		onFinishedScaleSelectL (false);
	}
	
	void onFinishedScaleSelectL(bool isLeft)
	{//Debug.Log ("tttonFinishedScaleSelectLonFinishedScaleSelectLonFinishedScaleSelectL  " + isLeft + " "+ moveLeft);

		if ((!isLeft && moveLeft) || (isLeft && !moveLeft))
		{
			return;
		}
		//after end scale changing
		mCurSelectLTWScaleScript1.enabled = false;
		mCurSelectLTWScaleScript2.enabled = false;
		mCurSelectLTWScaleScript3.enabled = false;
		mCurSelectLTWScaleScript2A.enabled = false;
		mCurSelectLTWScaleScript3A.enabled = false;

		mCurSelectLTWPosScript1.enabled = false;
		mCurSelectLTWPosScript2.enabled = false;
		mCurSelectLTWPosScript2A.enabled = false;
		mCurSelectLTWPosScript3.enabled = false;
		mCurSelectLTWPosScript3A.enabled = false;

		if (isLeft)
			EventDelegate.Remove(mCurSelectLTWScaleScript1.onFinished, onFinishedScaleSelectLLeft);
		else
			EventDelegate.Remove(mCurSelectLTWScaleScript1.onFinished, onFinishedScaleSelectLRight);

		moveEndLe (isLeft);
	}

	void moveToRightHalfLe()
	{//Debug.Log ("moveToRightHalfLemoveToRightHalfLemoveToRightHalfLe " + selectLevelIndex);
		//1		1-0.5	liang-an	7-5
		mCurSelectScriptObj1.depth = 5;

		//2		no change
		mCurSelectScriptObj2.depth = 5;

		//3		0.5-1	an-liang	5-7
		mCurSelectScriptObj3.depth = 7;
		
		mCurSelectScriptObj1.transform.GetChild (0).GetComponent<UISprite> ().depth = 6;
		mCurSelectScriptObj2.transform.GetChild (0).GetComponent<UISprite> ().depth = 6;
		mCurSelectScriptObj3.transform.GetChild (0).GetComponent<UISprite> ().depth = 8;

		//change the sprite
		int selectIndexTemp = (totalLevelNum + 1) - selectLevelIndex;	//the obj1

		if (selectIndexTemp >= (totalLevelNum + 1))
		{
			selectIndexTemp = 0;
		}
	}
	
	void moveToLeftHalfLe()
	{//Debug.Log ("moveToLeftHalfLemoveToLeftHalfLe " + selectLevelIndex);
		//1		1-0.5	liang-an	7-5
		mCurSelectScriptObj1.depth = 5;
		
		//2		0.5-1	an-liang	5-7
		mCurSelectScriptObj2.depth = 7;
		
		//3		no change
		mCurSelectScriptObj3.depth = 5;
		
		mCurSelectScriptObj1.transform.GetChild (0).GetComponent<UISprite> ().depth = 6;
		mCurSelectScriptObj2.transform.GetChild (0).GetComponent<UISprite> ().depth = 8;
		mCurSelectScriptObj3.transform.GetChild (0).GetComponent<UISprite> ().depth = 6;
	}

	void moveEndLe(bool isleft)
	{//Debug.Log ("moveEndLemoveEndLemoveEndLe " + selectLevelIndex);

		UISprite selectSobj1 = mCurSelectScriptObj1;
		UISprite selectSobj2 = mCurSelectScriptObj2;
		UISprite selectSobj3 = mCurSelectScriptObj3;

		TweenScale selectTSObj1 = mCurSelectLTWScaleScript1;
		TweenScale selectTSObj2 = mCurSelectLTWScaleScript2;
		//TweenScale selectTSObj2A = mCurSelectLTWScaleScript2A;
		TweenScale selectTSObj3 = mCurSelectLTWScaleScript3;
		//TweenScale selectTSObj3A = mCurSelectLTWScaleScript3A;
		
		TweenPosition selectTPObj1 = mCurSelectLTWPosScript1;
		TweenPosition selectTPObj2 = mCurSelectLTWPosScript2;
		TweenPosition selectTPObj3 = mCurSelectLTWPosScript3;

		if (isleft)
		{
			mCurSelectScriptObj1 = selectSobj2;
			mCurSelectScriptObj2 = selectSobj3;
			mCurSelectScriptObj3 = selectSobj1;

			mCurSelectLTWScaleScript1 = selectTSObj2;
			mCurSelectLTWScaleScript2 = selectTSObj3;
			mCurSelectLTWScaleScript3 = selectTSObj1;

			mCurSelectLTWPosScript1 = selectTPObj2;
			mCurSelectLTWPosScript2 = selectTPObj3;
			mCurSelectLTWPosScript3 = selectTPObj1;
		}
		else
		{
			mCurSelectScriptObj1 = selectSobj3;
			mCurSelectScriptObj2 = selectSobj1;
			mCurSelectScriptObj3 = selectSobj2;
			
			mCurSelectLTWScaleScript1 = selectTSObj3;
			mCurSelectLTWScaleScript2 = selectTSObj1;
			mCurSelectLTWScaleScript3 = selectTSObj2;
			
			mCurSelectLTWPosScript1 = selectTPObj3;
			mCurSelectLTWPosScript2 = selectTPObj1;
			mCurSelectLTWPosScript3 = selectTPObj2;
		}

		if (isleft)
		{
			changeDengjiNameWithName(mCurSelectScriptObj2.transform.GetChild(0).GetComponent<UISprite>(), mCurSelectScriptObj2A.transform.GetChild(0).GetComponent<UISprite>().spriteName);
			mCurSelectScriptObj2.spriteName = mCurSelectScriptObj2A.spriteName;
		}
		else
		{
			changeDengjiNameWithName(mCurSelectScriptObj3.transform.GetChild(0).GetComponent<UISprite>(), mCurSelectScriptObj3A.transform.GetChild(0).GetComponent<UISprite>().spriteName);
			mCurSelectScriptObj3.spriteName = mCurSelectScriptObj3A.spriteName;
		}

		mCurSelectScriptObj2.depth = 5;
		mCurSelectScriptObj3.depth = 5;
		mCurSelectScriptObj1.depth = 7;

		mCurSelectScriptObj2.transform.GetChild (0).GetComponent<UISprite> ().depth = 6;
		mCurSelectScriptObj3.transform.GetChild (0).GetComponent<UISprite> ().depth = 6;
		mCurSelectScriptObj1.transform.GetChild (0).GetComponent<UISprite> ().depth = 8;
		
		if (isleft)
		{
			mCurSelectScriptObj2.transform.localScale = spriteScale2;
			mCurSelectScriptObj2.transform.localPosition = spritePos2;
		}
		else
		{
			mCurSelectScriptObj3.transform.localScale = spriteScale3;
			mCurSelectScriptObj3.transform.localPosition = spritePos3;
		}

		mCurSelectScriptObj1.enabled = true;
		mCurSelectScriptObj2.enabled = true;
		mCurSelectScriptObj3.enabled = true;

		mCurSelectScriptObj2A.enabled = false;
		mCurSelectScriptObj3A.enabled = false;
	}

	void moveToLeftLe()
	{Debug.Log ("moveToLeftLemoveToLeftLemoveToLeftLemoveToLeftLemoveToLeftLe " + selectLevelFinal);
		//here will move the sprite, and change the scale
		isHalfLe = false;

		mCurSelectScriptObj3A.enabled = true;
		mCurSelectScriptObj3.enabled = false;
		
		//change the sprite
		int selectIndexTemp = totalLevelNum - selectLevelIndex;	//the obj1
		
		//the obj3
		if (selectIndexTemp < totalLevelNum - 1)
		{
			selectIndexTemp = selectIndexTemp + 2;
		}
		else if (selectIndexTemp == totalLevelNum - 1)
		{
			selectIndexTemp = 1;
		}
		else if (selectIndexTemp == totalLevelNum)
		{
			selectIndexTemp = 2;
		}

        //Debug.Log("moveToLeftLe::selectIndexTemp ==================== " + selectIndexTemp);
        if (selectIndexTemp >= 6)
        {
            //丰裕口关卡去掉.
            mCurSelectScriptObj2A.spriteName = "guaka" + (selectIndexTemp + 1).ToString();
        }
        else
        {
            mCurSelectScriptObj2A.spriteName = "guaka" + selectIndexTemp.ToString();
        }
		changeDengjiName (mCurSelectScriptObj2A, selectIndexTemp - 1);
        
		mCurSelectLTWScaleScript1.from = spriteScale1;
		mCurSelectLTWScaleScript1.to = spriteScale3;
		mCurSelectLTWPosScript1.from = spritePos1;
		mCurSelectLTWPosScript1.to = spritePos3;
		
		mCurSelectLTWScaleScript2.from = spriteScale2;
		mCurSelectLTWScaleScript2.to = spriteScale1;
		mCurSelectLTWPosScript2.from = spritePos2;
		mCurSelectLTWPosScript2.to = spritePos1;
		
		mCurSelectLTWScaleScript2A.from = spriteScale4;
		mCurSelectLTWScaleScript2A.to = spriteScale2;
		mCurSelectLTWPosScript2A.from = spritePos4;
		mCurSelectLTWPosScript2A.to = spritePos2;
		
		mCurSelectLTWScaleScript3A.from = spriteScale3;
		mCurSelectLTWScaleScript3A.to = spriteScale4;
		mCurSelectLTWPosScript3A.from = spritePos3;
		mCurSelectLTWPosScript3A.to = spritePos4;
		
		beginMoveLe (true);
	}

	void moveToRightLe()
	{Debug.Log ("moveToLeftLemoveToLeftLemoveToLeftLemoveToLeftLemoveToLeftLerrrrrrrrrrrrr " + selectLevelFinal);
		isHalfLe = false;

		mCurSelectScriptObj2A.enabled = true;
		mCurSelectScriptObj2.enabled = false;
		
		//change the sprite
		int selectIndexTemp = (totalLevelNum + 1) - selectLevelIndex;	//the obj1
		
		if (selectIndexTemp >= (totalLevelNum + 1))
		{
			selectIndexTemp = 0;
		}
		
		//the obj2
		if (selectIndexTemp > 1)
		{
			selectIndexTemp = selectIndexTemp - 1;
		}
		else
		{
			selectIndexTemp = totalLevelNum;
		}

        //Debug.Log("moveToRightLe::selectIndexTemp ==================== " + selectIndexTemp);
        if (selectIndexTemp >= 6)
        {
            //丰裕口关卡去掉.
            mCurSelectScriptObj3A.spriteName = "guaka" + (selectIndexTemp + 1).ToString();
        }
        else
        {
            mCurSelectScriptObj3A.spriteName = "guaka" + selectIndexTemp.ToString();
        }
		changeDengjiName (mCurSelectScriptObj3A, selectIndexTemp - 1);

		mCurSelectLTWScaleScript1.from = spriteScale1;
		mCurSelectLTWScaleScript1.to = spriteScale2;
		mCurSelectLTWPosScript1.from = spritePos1;
		mCurSelectLTWPosScript1.to = spritePos2;
		
		mCurSelectLTWScaleScript2A.from = spriteScale2;
		mCurSelectLTWScaleScript2A.to = spriteScale4;
		mCurSelectLTWPosScript2A.from = spritePos2;
		mCurSelectLTWPosScript2A.to = spritePos4;

		mCurSelectLTWScaleScript3.from = spriteScale3;
		mCurSelectLTWScaleScript3.to = spriteScale1;
		mCurSelectLTWPosScript3.from = spritePos3;
		mCurSelectLTWPosScript3.to = spritePos1;
		
		mCurSelectLTWScaleScript3A.from = spriteScale4;
		mCurSelectLTWScaleScript3A.to = spriteScale3;
		mCurSelectLTWPosScript3A.from = spritePos4;
		mCurSelectLTWPosScript3A.to = spritePos3;

		beginMoveLe (false);
	}

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
            selectTimeSObj1.enabled = false;
            selectTimeSObj2.enabled = false;
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
