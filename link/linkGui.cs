using UnityEngine;
using System.Collections;
using System;

public class linkGui : MonoBehaviour {

	public GameObject linkUIObj = null;
	public GameObject selectTruckObj = null;
	public GameObject selectObj = null;
	public GameObject waitGuiObj = null;
	public GameObject playerInforObj = null;

	public GameObject singleTruckObj = null;
	public GameObject singleWordObj = null;
	public GameObject singleLightObj = null;
	public GameObject linkTruckObj = null;
	public GameObject linkWordObj = null;
	public GameObject linkLightObj = null;

	public GameObject startObj = null;
	public UISprite[] playerSpriteArr;	//"p1"-1p, "p2"-2p..."p5"-AI-1, "p6"-AI-2
	public TweenColor[] playerTColorArr;
	public bool moveLeft = true;

	private bool isLinkmode = false;
	private int selectIndex = 0;	//1 - select single mode; 2 - link to server;

	private bool hasShowZijibiaoji = false;
	private int detectNum = 0;

	//select level
	private int totalLevelNum = 8;	//8-6	the total game-level number
	private bool hasSelectLevel = false;
	private int selectLevelIndex = 0;		//should judge, use this to change the sprite
	private int selectLevelNOChange = 0;	//will ++ or --, and not adjust
	private int selectLevelFinal = 0;		//will send this as the game level and start game
	public GameObject selectParentObj = null;
	private bool firstPlayerPressSelect = false;
	private bool jinyouxile = false;
	public int moveStateHere = 0;

	public GameObject[] zijiBiaoji;

	// Use this for initialization
	void Start () {Debug.Log("dao link gui leeeeee");
		pcvr.GetInstance().CloseFangXiangPanPower();
		pcvr.selectLevelFinalLink = -1;
		jinyouxile = false;
		moveStateHere = 0;

		hasShowZijibiaoji = false;
		detectNum = 0;
		
		singleTotalTime = 20.3f;
		waitTotalTime = 30.3f;
		selectTotalTime = 30.3f;

		singlepanJiangeTime = singlepanJiangeSTime;
		selectpanJiangeTime = selectpanJiangeSTime;

		singleFangxiangIndex = 0;
		selectFangxiangIndex = 0;

		singleTimeObj1.SetActive (true);
		singleTimeObj2.SetActive (true);
		singleTimeObj3.SetActive (false);

		waitTimeObj1.SetActive (true);
		waitTimeObj2.SetActive (true);
		waitTimeObj3.SetActive (false);

		selectTimeObj1.SetActive (true);
		selectTimeObj2.SetActive (true);
		selectTimeObj3.SetActive (false);

		for (int i=0; i<playerTColorArr.Length; i++)
		{
			playerTColorArr[i].enabled = false;
		}

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
		
		mCurSelectLTWPosScript1 = mSelectLTWPosScript1;
		mCurSelectLTWPosScript2 = mSelectLTWPosScript2;
		mCurSelectLTWPosScript2A = mSelectLTWPosScript2A;
		mCurSelectLTWPosScript3 = mSelectLTWPosScript3;
		mCurSelectLTWPosScript3A = mSelectLTWPosScript3A;
		
		initFirst ();
		InputEventCtrl.GetInstance().ClickStartBtOneEvent += ClickStartBtOneEvent;

        pcvr.linkguiTrans = transform;

		if (pcvr.sound2DScrObj)
		{
			pcvr.sound2DScrObj.playAudioXuanguanBeijing(true);
		}
		
		int volumNum = Convert.ToInt32(ReadGameInfo.GetInstance ().ReadGameVolumNum ());
		Debug.Log ("volumNumttblll ==   " + volumNum);
		AudioListener.volume = volumNum * 0.1f;
        ClickStartBtOneEvent(ButtonState.DOWN); //跳过游戏模式选择界面.
    }

    void initFirst()
	{
		moveLeft = true;
		selectIndex = 0;
		selectLevelIndex = 0;
		selectLevelNOChange = 0;
		selectLevelFinal = 0;
		hasSelectLevel = false;
		moveStateHere = 0;
		
		//hide some gui
		selectObj.SetActive (true);
		waitGuiObj.SetActive (false);
		playerInforObj.SetActive (false);
		
		singleTruckObj.SetActive (true);
		singleWordObj.SetActive (true);
		singleLightObj.SetActive (true);
		
		linkTruckObj.SetActive (false);
		linkWordObj.SetActive (false);
		linkLightObj.SetActive (false);
		
		if (selectParentObj)
		{
			selectParentObj.SetActive(false);
		}

		playerSpriteArr [0].spriteName = "P1";
		//playerTColorArr[0].enabled = true;
	}

	// Update is called once per frame
	void Update () {
		if (jinyouxile)
		{
			return;
		}

		if (selectIndex == 2 && !hasShowZijibiaoji)
		{
			detectNum ++;

			if (detectNum > 3 && pcvr.totalPlayerNum > 0)
			{
				hasShowZijibiaoji = true;

				if (zijiBiaoji.Length > 3 && zijiBiaoji[pcvr.totalPlayerNum - 1])
				{
					for (int i=0; i<4; i++)
					{
						if (i == pcvr.totalPlayerNum - 1)
							zijiBiaoji[i].SetActive(true);
						else
							zijiBiaoji[i].SetActive(false);
					}
				}
				Debug.Log ("pcvr.totalPlayerNum  pcvr.totalPlayerNum " + pcvr.totalPlayerNum);
			}
		}

		if (pcvr.firstPlayerIndex != int.Parse(Network.player + "")
		    && (selectIndex == 1 || selectIndex == 2))
		{//others
			if (startObj.activeSelf || waitGuiObj.activeSelf)
			{
				startObj.SetActive(false);
				waitGuiObj.SetActive(false);
			}

			if (!playerInforObj.activeSelf)
			{
				playerInforObj.SetActive (true);
			}
		}

		if (pcvr.firstPlayerIndex == int.Parse(Network.player + "")
		    && (selectIndex == 1 || selectIndex == 2))
		{//first player
			if (!startObj.activeSelf)
			{
				startObj.SetActive(true);
			}

			if (!firstPlayerPressSelect && !waitGuiObj.activeSelf)
			{
				waitGuiObj.SetActive(true);
				playerInforObj.SetActive (true);
			}
			else if (firstPlayerPressSelect && (waitGuiObj.activeSelf || playerInforObj.activeSelf))
			{
				waitGuiObj.SetActive(false);
				playerInforObj.SetActive (false);
			}

			if (waitGuiObj.activeSelf)
			{
				if (waitTotalTime > 0)
				{
					waitTotalTime -= Time.deltaTime;
				}
				else
				{
					ClickStartBtOneEvent(ButtonState.DOWN);
				}
				
				showTimeDownWait(waitTotalTime);
			}
		}

		if (firstPlayerPressSelect && selectIndex == 2
		    && pcvr.firstPlayerIndex == int.Parse(Network.player + "")
		    && selectParentObj
		    && !selectParentObj.activeSelf)
		{//the normal value, is move to left
			isLinkmode = true;
			moveLeft = true;
			selectLevelIndex = 0;
			hasSelectLevel = false;
			
			selectParentObj.SetActive(true);
		}

		if (playerInforObj.activeSelf)
		{
			if (pcvr.totalPlayerNum <= 1)
			{
				playerSpriteArr [1].spriteName = "P7";
				playerSpriteArr [2].spriteName = "P6";
				playerSpriteArr [3].spriteName = "P5";
				//playerTColorArr[1].enabled = false;
				//playerTColorArr[2].enabled = false;
				//playerTColorArr[3].enabled = false;
			}
			else if (pcvr.totalPlayerNum == 2)
			{
				playerSpriteArr [1].spriteName = "P2";
				playerSpriteArr [2].spriteName = "P6";
				playerSpriteArr [3].spriteName = "P5";
				//playerTColorArr[1].enabled = true;
				//playerTColorArr[2].enabled = false;
				//playerTColorArr[3].enabled = false;
			}
			else if (pcvr.totalPlayerNum == 3)
			{
				playerSpriteArr [1].spriteName = "P2";
				playerSpriteArr [2].spriteName = "P3";
				playerSpriteArr [3].spriteName = "P5";
				//playerTColorArr[1].enabled = true;
				//playerTColorArr[2].enabled = true;
				//playerTColorArr[3].enabled = false;
			}
			else if (pcvr.totalPlayerNum == 4)
			{
				playerSpriteArr [1].spriteName = "P2";
				playerSpriteArr [2].spriteName = "P3";
				playerSpriteArr [3].spriteName = "P4";
				//playerTColorArr[1].enabled = true;
				//playerTColorArr[2].enabled = true;
				//playerTColorArr[3].enabled = true;
			}
		}

		if (!isLinkmode)
		{
			if (selectIndex != 2)
			{
				if (singleTotalTime > 0)
				{
					singleTotalTime -= Time.deltaTime;
				}
				else
				{
					singleTotalTime = 1.0f;
					ClickStartBtOneEvent(ButtonState.DOWN);
				}

				showTimeDownSingle(singleTotalTime);

				if (singlepanJiangeTime > 0)
				{
					singlepanJiangeTime -= Time.deltaTime;
				}
				else
				{
					singlepanJiangeTime = singlepanJiangeSTime;
					
					changeSingleFangxiangS(++ singleFangxiangIndex);
				}
			}

			//left and right ---- select link or single mode
			if (!moveLeft && ((pcvr.bIsHardWare && pcvr.mGetSteer < -0.5f) || (!pcvr.bIsHardWare && Input.GetAxis("Horizontal") < -0.5f)))
			{//left
				moveLeft = true;
				playAudioXuanguanLinkGui();
				
				singleTruckObj.SetActive (true);
				singleWordObj.SetActive (true);
				singleLightObj.SetActive (true);
				
				linkTruckObj.SetActive (false);
				linkWordObj.SetActive (false);
				linkLightObj.SetActive (false);
			}
			else if (moveLeft && ((pcvr.bIsHardWare && pcvr.mGetSteer > 0.5f) || (!pcvr.bIsHardWare && Input.GetAxis("Horizontal") > 0.5f)))
			{//right
				moveLeft = false;
				playAudioXuanguanLinkGui();
				
				singleTruckObj.SetActive (false);
				singleWordObj.SetActive (false);
				singleLightObj.SetActive (false);
				
				linkTruckObj.SetActive (true);
				linkWordObj.SetActive (true);
				linkLightObj.SetActive (true);
			}
		}
		else if (firstPlayerPressSelect)	//here will change	20170523
		{
			if (selectTotalTime > 0)
			{
				selectTotalTime -= Time.deltaTime;
			}
			else
			{
				ClickStartBtOneEvent(ButtonState.DOWN);
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
			{//Debug.Log("lefffffffffffff " + moveLeft + " "+ moveStateHere);
             //select level
                if ((moveStateHere == 0 || moveStateHere == 2) && ((pcvr.bIsHardWare && pcvr.mGetSteer < -0.5f) || (!pcvr.bIsHardWare && Input.GetAxis("Horizontal") < -0.5f)))
                {//left
                    moveLeft = true;
                    spaceTime = spaceTimeStatic;
                    moveStateHere = 1;

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
                else if ((moveStateHere == 0 || moveStateHere == 1) && ((pcvr.bIsHardWare && pcvr.mGetSteer > 0.5f) || (!pcvr.bIsHardWare && Input.GetAxis("Horizontal") > 0.5f)))
                {//right
                    moveLeft = false;
                    spaceTime = spaceTimeStatic;
                    moveStateHere = 2;

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
                else if ((pcvr.bIsHardWare && Mathf.Abs(pcvr.mGetSteer) <= 0.5f) || (!pcvr.bIsHardWare && Mathf.Abs(Input.GetAxis("Horizontal")) <= 0.5f))
                {
                    moveStateHere = 0;
                }
            }
			else
			{
				if ((pcvr.bIsHardWare && Mathf.Abs(pcvr.mGetSteer) <= 0.5f) || (!pcvr.bIsHardWare && Mathf.Abs(Input.GetAxis("Horizontal")) <= 0.5f))
				{
					moveStateHere = 0;
				}
				
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
	}

	//keyboard - K - start game - only for link gui
	void ClickStartBtOneEvent(ButtonState val)
	{Debug.Log ("ClickStartBtOneEventstartbuttonnnnnnnnnnnnnnnnnnnnnnnn  linkguiiii " + val + " " + jinyouxile + " "+ gameObject.activeSelf);
		if (jinyouxile)
		{
			return;
		}

		if (pcvr.UIState != 2)
		{
			return;
		}

		if (val == ButtonState.UP)
		{
			return;
		}

		if (selectIndex == 2 && pcvr.firstPlayerIndex == int.Parse(Network.player + "") && !firstPlayerPressSelect)
		{
			firstPlayerPressSelect = true;
			waitGuiObj.SetActive(false);
			playerInforObj.SetActive (false);
			return;
		}

		//if is the first player, will into game here
		if (selectIndex == 2 && pcvr.firstPlayerIndex == int.Parse(Network.player + ""))
		{
			if (Network.incomingPassword.CompareTo(pcvr.passWord) != 0)
			{
				return;
			}

			//playAudioStartLinkGui();
			//pcvr.sound2DScrObj.playAudioXuanguanQueren();

			//send message to other players to begin game
			pcvr.selectLevelFinalLink = selectLevelFinal + 2;
			pcvr.selfServerScrObj.startGame(pcvr.selectLevelFinalLink);//changehere??????????????????????????????????
			jinyouxile = true;
		}

		if (selectIndex > 0)
		{
			return;
		}

		if (moveLeft)
		{//the single mode, will into game here
			selectIndex = 1;
			linkUIObj.SetActive(false);

			pcvr.donghuaScriObj.singleModeBegin(1, 2);

			//playAudioStartLinkGui();
		}
		else
		{//the link mode, will wait for into game
			//link
			selectIndex = 3;
			NetworkConnectionError error = Network.Connect(/*pcvr.ipString*/"192.168.1.11", pcvr.port, pcvr.passWord);
			Debug.Log("ready to connect to server, but not sure can link  " + error);
			Invoke("resetSelect", 1.5f);

			//playAudioStartInsert();
		}
	}

	void resetSelect()
	{
		if (selectIndex == 3)
		{
			selectIndex = 0;
		}
	}

	//client
	void OnConnectedToServer()
	{
		pcvr.donghuaScriObj.resetJiangliInfor ();
		CancelInvoke ("resetSelect");
		selectIndex = 2;
		selectObj.SetActive(false);

		Debug.Log("had connected to server " + Time.time + " " + pcvr.totalPlayerNum + " " + Network.player + " "+ pcvr.firstPlayerIndex);
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

		//change the sprite
		int selectIndexTemp = (totalLevelNum + 1) - selectLevelIndex;	//the obj1

		if (selectIndexTemp >= (totalLevelNum + 1))
		{
			selectIndexTemp = 0;
		}
		/*
		//the obj2
		if (selectIndexTemp > 1)
		{
			selectIndexTemp = selectIndexTemp - 1;
		}
		else
		{
			selectIndexTemp = totalLevelNum;
		}

		mCurSelectScriptObj2.spriteName = "guaka" + selectIndexTemp.ToString();

		Debug.Log ("mCurSelectScriptObj2.spriteNamemCurSelectScriptObj2.spriteName  " + mCurSelectScriptObj2.spriteName);*/
	}
	
	void moveToLeftHalfLe()
	{//Debug.Log ("moveToLeftHalfLemoveToLeftHalfLe " + selectLevelIndex);
		//1		1-0.5	liang-an	7-5
		mCurSelectScriptObj1.depth = 5;
		
		//2		0.5-1	an-liang	5-7
		mCurSelectScriptObj2.depth = 7;
		
		//3		no change
		mCurSelectScriptObj3.depth = 5;
		/*
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
		
		mCurSelectScriptObj3.spriteName = "guaka" + selectIndexTemp.ToString();

		Debug.Log ("mCurSelectScriptObj3.mCurSelectScriptObj3.spriteName  " + mCurSelectScriptObj3.spriteName);*/
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
			mCurSelectScriptObj2.spriteName = mCurSelectScriptObj2A.spriteName;
		}
		else
		{
			mCurSelectScriptObj3.spriteName = mCurSelectScriptObj3A.spriteName;
		}

		mCurSelectScriptObj2.depth = 5;
		mCurSelectScriptObj3.depth = 5;
		mCurSelectScriptObj1.depth = 7;
		
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
	{//Debug.Log ("moveToLeftLemoveToLeftLemoveToLeftLemoveToLeftLemoveToLeftLe");
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
		
		mCurSelectScriptObj2A.spriteName = "guaka" + selectIndexTemp.ToString();

		
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
	{//Debug.Log ("moveToLeftLemoveToLeftLemoveToLeftLemoveToLeftLemoveToLeftLerrrrrrrrrrrrr");
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
		
		mCurSelectScriptObj3A.spriteName = "guaka" + selectIndexTemp.ToString();

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
	public GameObject singleTimeObj1 = null;
	public GameObject singleTimeObj2 = null;
	public GameObject singleTimeObj3 = null;
	public UISprite singleTimeSObj1 = null;
	public UISprite singleTimeSObj2 = null;
	public UISprite singleTimeSObj3 = null;
	private float singleTotalTime = 10.0f;

	public GameObject waitTimeObj1 = null;
	public GameObject waitTimeObj2 = null;
	public GameObject waitTimeObj3 = null;
	public UISprite waitTimeSObj1 = null;
	public UISprite waitTimeSObj2 = null;
	public UISprite waitTimeSObj3 = null;
	private float waitTotalTime = 30.0f;
	
	public GameObject selectTimeObj1 = null;
	public GameObject selectTimeObj2 = null;
	public GameObject selectTimeObj3 = null;
	public UISprite selectTimeSObj1 = null;
	public UISprite selectTimeSObj2 = null;
	public UISprite selectTimeSObj3 = null;
	private float selectTotalTime = 30.0f;

	private int timeNow = 0;

	void showTimeDownSingle(float timeT)
	{
		if (timeT < 0)
		{
			timeT = 0;
		}

		timeNow = Mathf.FloorToInt(timeT);

		if (timeNow >= 10)
		{
			singleTimeSObj1.spriteName = Mathf.FloorToInt(timeNow / 10).ToString();
			singleTimeSObj2.spriteName = Mathf.FloorToInt(timeNow % 10).ToString();
		}
		else
		{
			singleTimeObj1.SetActive (false);
			singleTimeObj2.SetActive (false);
			singleTimeObj3.SetActive (true);
			singleTimeSObj3.spriteName = timeNow.ToString();
		}
	}

	void showTimeDownWait(float timeT)
	{
		if (timeT < 0)
		{
			timeT = 0;
		}

		timeNow = Mathf.FloorToInt(timeT);
		
		if (timeNow >= 10)
		{
			waitTimeSObj1.spriteName = Mathf.FloorToInt(timeNow / 10).ToString();
			waitTimeSObj2.spriteName = Mathf.FloorToInt(timeNow % 10).ToString();
		}
		else
		{
			waitTimeObj1.SetActive (false);
			waitTimeObj2.SetActive (false);
			waitTimeObj3.SetActive (true);
			waitTimeSObj3.spriteName = timeNow.ToString();
		}
	}
	
	void showTimeDownSelect(float timeT)
	{
		if (timeT < 0)
		{
			timeT = 0;
		}
		
		timeNow = Mathf.FloorToInt(timeT);
		
		if (timeNow >= 10)
		{
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
	public UISprite singleFangxiang = null;
	public UISprite selectFangxiang = null;
	private int singleFangxiangIndex = 0;
	private int selectFangxiangIndex = 0;
	private int fangxiangIndexTemp = 0;
	private string fangxiangNameTemp = "";
	private float singlepanJiangeTime = 0.5f;
	private float selectpanJiangeTime = 0.5f;
	private float singlepanJiangeSTime = 0.2f;
	private float selectpanJiangeSTime = 0.2f;

	void changeSingleFangxiangS(int index)
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

		singleFangxiang.spriteName = fangxiangNameTemp;
	}
	
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

	public void intoSelectTruckGui()
	{
		selectTruckObj.SetActive (true);
		gameObject.SetActive (false);
	}
}
