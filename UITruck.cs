using UnityEngine;
using System.Collections;

public class UITruck : MonoBehaviour {

	public float maxSpeed = 200;
	public float gameTime = 120;			//will set this value out of game
	public float gameTimeLink = 120;
	public static float gameTimeTotal = 0;	//will change(can add game time here)
	private float gameTimeTotalStatic = 0;
	public GameObject truckObj = null;
	public GameObject fanxiangObj = null;
	public static bool isFanxiang = false;
	public GameObject frameObj = null;

	//game time
	public UISprite[] gameTimeArr;
	//private float totalTime = 0;
	private float curTime = 0;
	private int fen = 0;
	private int miao = 0;
	private int haomiao = 0;	//10
	
	//3-2-1 go
	//public static int uiRunState = -1;		//-1 before starting game, 1----3 2 1 time, 2----during the game, 3----10s game time down, 10---game over
	//private float total321Time = 0;
	
	//light
	public GameObject lightObj = null;		//light object|||
	public UISprite lightSprite = null;		//uiLight1-3

	//time down
	public GameObject daojishiObj = null;	//countinue and timedown object||||
	public UISprite daojishi = null;		//0-9
	//private static float totalDownTimeS = 10.0f;
	private float totalDownTime = 0;
	private int downTimeIntNum = 0;

	public GameObject daojishiObjClient = null;	//client timedown object||||
	public UISprite daojishiClient = null;		//0-9
	private float totalDownTimeClient = 0;
	private int downTimeIntNumClient = 0;
	private static float totalDownTimeChange = 15.0f;
	private int downTimeWillEndTime = 0;
	private float totalDownTimeWillEndTime = 0;

	//speed
	public UISprite[] speedNumArr;			//0-9
	public static float uiSpeed = 0;
	private int speed1 = 0;
	private int speed2 = 0;
	private int speed3 = 0;
	private float angle = 0;
	private float perA = 0;

	//coin information
	public GameObject coinParentObject = null;
	public GameObject freeObject = null;
	public GameObject insertCoinObj = null;		//insert coin
	public GameObject startButtonObj = null;	//please press start button
	public UISprite coinNumSprite1 = null;
	public UISprite coinNumSprite2 = null;
	public UISprite coinNumSprite3 = null;
	public UISprite coinNumSprite4 = null;

	//over
	public GameObject gameOverObj = null;	//game over
	public GameObject taotaiObj = null;		//lost

	//ming ci
	public GameObject[] backSmallArr;	//the gameobect
	public GameObject[] backLargeArr;
	private string[] nameArrYellow;		//will change the sprite use these names
	private string[] nameArrBlack;
	public UISprite[] nameSpriteArr;	//these sprites will change

	public float[] distanceFourTemp;
	private int[] jiaohuiPointArrTemp;
	private int[] lahuiPointArrTemp;
	private float tempdis = 0;
	private int tempIndex = 0;
	private int tempjiaohui = 0;
	private int templahui = 0;
	//private int firstPlayerMingciTemp = 0;
	public GameObject wanchengObj = null;

	//score
	public UIAtlas ScoreAtlas;
	public UIAtlas ScoreSelfAtlas;
	public UISprite[] mingciSpriteArr;
	public UISprite huangGuanSprite;
	public UISprite[] maohaoSprite;
	public GameObject scoreObject = null;
	public UISprite[] backSpriteObject;		//score4--not self, score5--self
	public UISprite[] playerSpriteObj;
	public GameObject[] timeOtherObject;	//time1 time2 taotai weiwancheng---------if taotai or weiwancheng, the time will not be show
											//the four players information will record in this array
	private UISprite[,] pTimespriteObjArr;
	public UISprite[] p1TimespriteObjArr;
	public UISprite[] p2TimespriteObjArr;
	public UISprite[] p3TimespriteObjArr;
	public UISprite[] p4TimespriteObjArr;
	private string[] nameArray;		//will change the sprite use these names
	//private int[] playerStateArr;			//1-complete,2-lost,3-not complete

	private int totalPlayerAINum = 0;		//only record the number of AI and Player
											//if the number is 4, then will refresh the player information

	private float addGameTime1 = 10;
	private float addGameTime2 = 15;
	public GameObject addTimeObj = null;
	public UISprite addTimeUISobj = null;
	public TweenScale addTimeTScale = null;
	public TweenPosition addTimeTPos = null;

	public GameObject loadObj = null;

	private bool chuliguo = false;
	private int index321 = -1;

	//game end will hide objects
	public GameObject mingciObjEnd = null;
	public GameObject youmenObjEnd = null;
	public GameObject timeObjEnd = null;
	public GameObject mapObjEnd = null;

	private bool isLinkmodeRecord = false;

	public void setAIPlayerHere(truck truckScriptObjT)
	{
		totalPlayerAINum ++;

		if (totalPlayerAINum == 4)
		{
			Invoke("Start1", 2.0f);
		}
	}

	public void setLoadObjVisible(bool flag, int index)
	{//Debug.Log ("setLoadObj1Visible  " + flag + " "+ index);
		if (loadObj && flag)
		{//show
			loadObj.transform.localPosition = new Vector3(0, 0, 664);
		}
		else if (loadObj && !flag)
		{//hide
			loadObj.transform.localPosition = new Vector3(0, 0, 0);
		}
	}
	
	// Use this for initialization
	void Awake () {
		gameTime = pcvr.gametimePCVR * 1.0f;
		gameTimeLink = pcvr.gametimePCVR * 1.0f;

		pcvr.isPassgamelevel = false;
		chuliguo = false;
		index321 = -1;
		pcvr.GetInstance().audioBeijingUI = null;

		if (Network.isServer || Network.isClient)
		{
			gameTimeTotalStatic = gameTimeLink;
			isLinkmodeRecord = true;
		}
		else
		{
			gameTimeTotalStatic = gameTime;
			isLinkmodeRecord = false;
		}

		wanchengObj.SetActive (false);
		isFanxiang = false;

		downTimeIntNumClient = 0;
		downTimeWillEndTime = 0;

		if (pcvr.sound2DScrObj)
		{
			pcvr.sound2DScrObj.playAudioBeijing (false);
		}

		if (zuihouyiquanObj)
		{
			zuihouyiquanObj.SetActive(false);
		}
		
		if (tishiObj)
		{
			tishiObj.SetActive(false);
		}

		if (frameObj)
		{
			frameObj.SetActive(false);
		}

        if (daojishiObjClient != null)
        {
            SSGameUIRoot.GetInstance().m_UICenterTr = daojishiObjClient.transform.parent;
        }
    }

	public void setGameTime(int index, float gametimePCVRT)
	{
		gameTime = gametimePCVRT * 1.0f;
		gameTimeLink = gametimePCVRT * 1.0f;
		
		//Debug.Log ("setGameTimesetGameTimesetGameTime  " + Network.isServer + " " + Network.isClient + " "+ pcvr.gametimePCVR + " "+ index);
		//gameTime = 12.0f;
	}

	// Use this for initialization
	void Start1 () {
		initFirst ();
	}

	//this will do after gameset again
	public void initFirst ()
	{
		if (pcvr.sound2DScrObj)
		{
			pcvr.sound2DScrObj.playAudioXuanguanBeijing(false);
		}

		InputEventCtrl.GetInstance ();
		pcvr.GetInstance ();
		Time.timeScale = 1;
		Application.targetFrameRate = 80;
		pcvr.uiRunState = 1;			//should first is -1 ffffffffffffffffffffffffffffffff
		pcvr.total321Time = 0;

		pcvr.GetInstance().setShijianquyu (Application.loadedLevel - 2); 

		if (Network.isServer || Network.isClient)
		{
			gameTimeTotal = gameTimeLink;
		}
		else
		{
			gameTimeTotal = gameTime;
		}

		fanxiangObj.SetActive (false);
		totalPlayerAINum = 10;
		setLoadObjVisible (false, 1);
		pcvr.isPassgamelevel = false;

		if (!Network.isServer)
		{
			pcvr.smallMapObj.SetActive (true);
		}

		//first hide some object
		lightObj.SetActive (false);		//light
		daojishiObj.SetActive (false);	//time down
		daojishiObjClient.SetActive (false);	//time down client
		gameOverObj.SetActive (false);
		taotaiObj.SetActive (false);
		scoreObject.SetActive (false);	//the player score end game
		wanchengObj.SetActive (false);

		addTimeObj.SetActive(false);
		addTimeTScale.enabled = false;
		addTimeTPos.enabled = false;

		gameTimeRSObj.SetActive (false);
		chuangjiluObj.SetActive (false);

		if (pcvr.gameModePCVR == 1)
		{
			coinNumSprite3.spriteName = "x" + Mathf.FloorToInt(pcvr.startCoinNumPCVR / 10).ToString();
			coinNumSprite4.spriteName = "x" + Mathf.FloorToInt(pcvr.startCoinNumPCVR % 10).ToString();

			changeCoinNum(false);

			coinParentObject.SetActive(true);
		}
		else
		{
			pcvr.startCoinNumPCVR = 0;
			freeObject.SetActive (true);
		}
		
		gameTimeArr[6].spriteName = "P";
		gameTimeArr[7].spriteName = "P";
		
		//perA = (500 - 260) / 200.0f;	//260 - 200v; 500-0v
		perA = (500 - 310) / uiMaxSpeed;	//310 is the number(160) on the pan, 500 is the number(0) on the pan
											//310 and 500 is the angle the pin will move to

		//InputEventCtrl.GetInstance().ClickInsertcoinBtEvent += ClickInsertcoinBtEvent;
		InputEventCtrl.GetInstance().ClickStartBtOneEvent += ClickStartBtOneEvent;
		//InputEventCtrl.GetInstance().ClickSetEnterBtEvent += ClickSetEnterBtEvent;
		//InputEventCtrl.GetInstance().ClickSetMoveBtEvent += ClickSetMoveBtEvent;
		//InputEventCtrl.GetInstance().ClickCloseDongGanBtEvent += ClickCloseDongGanBtEvent;
		//InputEventCtrl.GetInstance().ClickLaBaBtEvent += ClickLaBaBt;
		InputEventCtrl.GetInstance().ClickChangeCameraBtEvent += ClickChangeCameraBt;

		//detectInsertCoin ();

		nameArrYellow = new string[12]{"DY1", "DY2", "DY3", "DY4", "DY8", "DY7", "DY6", "DY5", "DYD1", "DYD2", "DYD3", "DYD4"};
		nameArrBlack = new string[12]{"P1", "P2", "P3", "P4", "P8", "P7", "P6", "P5", "DP1", "DP2", "DP3", "DP4", };

		distanceFourTemp = new float[4]{0, 0, 0, 0};
		jiaohuiPointArrTemp = new int[4]{0, 0, 0, 0};
		lahuiPointArrTemp = new int[4]{0, 0, 0, 0};

		//score
		nameArray = new string[8]{"P1", "P2", "P3", "P4", "P8", "P7", "P6", "P5"};
		//playerStateArr = new int[]{1, 1, 1, 1};
		pTimespriteObjArr = new UISprite[4, 12];

		for (int j=0; j<12; j++)
		{
			pTimespriteObjArr[0, j] = p1TimespriteObjArr[j];
			pTimespriteObjArr[1, j] = p2TimespriteObjArr[j];
			pTimespriteObjArr[2, j] = p3TimespriteObjArr[j];
			pTimespriteObjArr[3, j] = p4TimespriteObjArr[j];
		}

		paiMing ();
	}

	// Update is called once per frame
	void Update () {
		if (pcvr.isCanPressStartButtonEndGame && (pcvr.isPassgamelevel || (pcvr.uiRunState == 10 && pcvr.isAfterJifen)))
		{//10s game time down
			totalDownTime -= Time.deltaTime;
			
			if (downTimeIntNum != Mathf.FloorToInt(totalDownTime) && pcvr.sound2DScrObj && downTimeIntNum > 0)
			{
				pcvr.sound2DScrObj.playAudioDaojishi10s();
			}
			
			downTimeIntNum = Mathf.FloorToInt(totalDownTime);
			
			if (!daojishiObj.activeSelf)
			{
				daojishiObj.SetActive(true);
				
				if (pcvr.gameModePCVR != 1)
				{
					insertCoinObj.SetActive(false);
					freeObject.SetActive(true);
				}
			}
			
			if (pcvr.coinCurNumPCVR < pcvr.startCoinNumPCVR)
			{
				if (!insertCoinObj.activeSelf)
				insertCoinObj.SetActive(true);
			}
			else if (!startButtonObj.activeSelf)
			{
				startButtonObj.SetActive(true);
			}
			
			daojishi.spriteName = downTimeIntNum.ToString();
			
			if (totalDownTime <= 0)
			{
				//game over here -- the game over trigger point too
				notPressStartButtonEnd();
				return;
			}
			return;
		}

		//Debug.Log(transform + " "+ pcvr.uiRunState + " "+ pcvr.finishedNumber + " "+ Network.isClient + " "+ pcvr.totalTime + " "+ gameTimeTotal + " "+ totalDownTimeClient);
		if (pcvr.finishedNumber > 0 
		    && pcvr.finishedPlayerNumber < pcvr.totalPlayerNum
		    && pcvr.finishedNumber < 4
		    && Network.isClient
		    && !scoreObject.activeSelf)
		{//10s game time down---??????????????????????????????????????
			totalDownTimeClient = gameTimeTotal - pcvr.totalTime;
			Debug.Log("zhegerrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrr " + downTimeIntNumClient + " " + Mathf.FloorToInt(totalDownTimeClient));
			if (downTimeIntNumClient != Mathf.FloorToInt(totalDownTimeClient) && pcvr.sound2DScrObj && downTimeIntNumClient > 0)
			{
				pcvr.sound2DScrObj.playAudioDaojishi10s();
			}
			
			downTimeIntNumClient = Mathf.FloorToInt(totalDownTimeClient);
			
			daojishiClient.spriteName = downTimeIntNumClient.ToString();
			
			if (!daojishiObjClient.activeSelf && downTimeIntNumClient > 0)
			{
				daojishiObjClient.SetActive(true);
			}

			if (timeObjEnd.activeSelf)
			{
				timeObjEnd.SetActive(true);
			}

			if (Time.frameCount % 2 == 0)
			{
				changeGametime();
			}

			if (!wanchengObj.activeSelf)
			{
				wanchengObj.SetActive (true);
			}
		}
		else if (Network.isClient && daojishiObjClient.activeSelf)
		{
			daojishiObjClient.SetActive(false);
		}

		if (totalPlayerAINum < 10 || pcvr.uiRunState == 10 || pcvr.isPassgamelevel)
		{//uiRunState 10 - game over
			return;
		}

		if (!Network.isClient)
		{
			if (pcvr.uiRunState == 1)
			{//3-2-1-go
				pcvr.total321Time += Time.deltaTime;
				
				if (pcvr.total321Time >= 3)
				{
					pcvr.uiRunState = 2;
				}
			}
			else if (pcvr.uiRunState == 2)
			{//during the game
				pcvr.totalTime += Time.deltaTime;
				pcvr.totalTimeServer += Time.deltaTime;

				totalDownTimeWillEndTime = gameTimeTotal - pcvr.totalTime;
				if (totalDownTimeWillEndTime < totalDownTimeChange)
				{
					if (downTimeWillEndTime != Mathf.FloorToInt(totalDownTimeWillEndTime) && pcvr.sound2DScrObj && downTimeWillEndTime >= 0)
					{
						pcvr.sound2DScrObj.playAudioDaojishi10s();
					}
					
					downTimeWillEndTime = Mathf.FloorToInt(totalDownTimeWillEndTime);
					
					daojishiClient.spriteName = downTimeWillEndTime.ToString();
					
					if (!daojishiObjClient.activeSelf && downTimeWillEndTime > 0)
					{
						daojishiObjClient.SetActive(true);
					}
				}

				if (Network.isServer && gameTimeTotal - pcvr.totalTime <= 0)
				{
					pcvr.uiRunState = 3;

					if (fanxiangObj.activeSelf)
					{
						fanxiangObj.SetActive(false);
					}
				}
			}
		}
		else if (pcvr.uiRunState == 2)
		{
			pcvr.totalTimeServer += Time.deltaTime;
		}

		if (Network.isServer)
		{
			paiMing ();

			//send the state and the time---- 321time or gametime
			pcvr.selfServerScrObj.networkView.RPC("SendGameStateTime", RPCMode.All, pcvr.uiRunState, pcvr.total321Time, pcvr.totalTime, pcvr.indexOnly, pcvr.mingciFirstPlayer);

			return;
		}

		changeSpeedShow();

		if (!Network.isClient)
		{
			paiMing ();
		}

		showPlayerInfor ();

		if (!isFanxiang && fanxiangObj.activeSelf)
		{
			fanxiangObj.SetActive(false);
		}
		else if (isFanxiang && !fanxiangObj.activeSelf && pcvr.uiRunState != 3)
		{
			fanxiangObj.SetActive(true);
		}

		if (pcvr.uiRunState < 0)
		{//before init
			return;
		}

		if (pcvr.uiRunState == 1)
		{//3-2-1-go
			if (pcvr.total321Time < 1)
			{
				lightSprite.spriteName = "uiLight1";
				lightObj.SetActive (true);

				if (pcvr.sound2DScrObj && index321 != 3)
				{
					pcvr.sound2DScrObj.playAudio321(3);
				}
				index321 = 3;
			}
			else if (pcvr.total321Time < 2)
			{
				lightSprite.spriteName = "uiLight2";

				pcvr.playerTruckSObj.playAudioDianhuo(true);
				
				if (pcvr.sound2DScrObj && index321 != 2)
				{
					pcvr.sound2DScrObj.playAudio321(2);
				}
				index321 = 2;
			}
			else if (pcvr.total321Time < 3)
			{
				lightSprite.spriteName = "uiLight3";
				
				if (pcvr.sound2DScrObj && index321 != 1)
				{
					pcvr.sound2DScrObj.playAudio321(1);
				}
				index321 = 1;
			}
			return;
		}
		else if (pcvr.uiRunState == 2)
		{//during the game
			if (lightObj.activeSelf)
			{
				lightObj.SetActive (false);
			}

			if (!chuliguo)
			{
				chuliguo = true;
				if (pcvr.sound2DScrObj)
				{
					pcvr.sound2DScrObj.setBeijing(Application.loadedLevel);			//will change the index here
					pcvr.sound2DScrObj.playAudioBeijing (true);
				}
				pcvr.playerTruckSObj.playAudioDianhuo (false);
				pcvr.playerTruckSObj.playAudioMoveStop (1);
			}

			if (Time.frameCount % 2 == 0)
			{
				changeGametime();

				if (Time.frameCount % 600 == 0)
				{
					System.GC.Collect ();
				}
			}
		}
		else if (pcvr.uiRunState == 3 && !Network.isClient)
		{//10s game time down
			Debug.Log("totalDownTimetotalDownTimetotalDownTime  " + totalDownTime);
			if (/*totalDownTime <= 0*/true)
			{
				//game over here -- the game over trigger point too
				pcvr.uiRunState = 10;
				pcvr.playerTruckSObj.speedEndHere();
				pcvr.isCanPressStartButtonEndGame = false;
				gameOverHere();
				return;
			}
		}
	}

	void gameOverHere()
	{
		daojishiObj.SetActive(false);
		insertCoinObj.SetActive(false);
		startButtonObj.SetActive(false);

		stopSoundGameEnd ();
		pcvr.GetInstance().initEndGamele();

		pcvr.isPassgamelevel = false;
		daojishiObj.SetActive(false);
		gameOverObj.SetActive(true);
		showPlayerFinalScore(false);	//will call only after the game end

		if (pcvr.sound2DScrObj)
		{
			pcvr.sound2DScrObj.playAudioGameover();
		}
		
		if (daojishiObjClient)
		{
			daojishiObjClient.SetActive(false);
		}
	}
	
	public void passLevelHere()
	{//the single mode, when the player pass the finished point
		if (daojishiObj.activeSelf)
		{
			daojishiObj.SetActive(false);
		}

		stopSoundGameEnd ();

		/*if (pcvr.currentLevelNum == 1)
		{
			pcvr.isPassgamelevel = true;
		}
		else if (pcvr.currentLevelNum == 2)
		{
			pcvr.isPassgamelevel = false;
		}*/
		pcvr.isPassgamelevel = true;

        //judge the AI information here
        pcvr.useTimeSelf[pcvr.selfIndex] = pcvr.totalTime;

		Invoke("hideSomeObjects", 0.15f);
		Invoke("showPlayerFinalScoreSingle", 0.30f);
	}

	void showPlayerFinalScoreSingle()
	{
		showPlayerFinalScore (false);
	}
	
	public void passLevelHereLink(int indexPAI)
	{
		if (daojishiObj.activeSelf)
		{
			daojishiObj.SetActive(false);
		}
		
		stopSoundGameEnd ();
		
		//judge the AI information here
		pcvr.selfServerScrObj.networkView.RPC("setUseTimeRPC", RPCMode.All, indexPAI, /*pcvr.totalTime*/pcvr.totalTimeServer);
	}

	public void stopSoundGameEnd()
	{
		/*if (pcvr.sound2DScrObj)
		{
			pcvr.sound2DScrObj.playAudioBeijing (false);
		}*/

		if (pcvr.playerTruckSObj)
		{
			pcvr.playerTruckSObj.playAudioMoveStop (5);
			pcvr.playerTruckSObj.playAudioBrake(2, false);
			pcvr.playerTruckSObj.playAudioMingdi(false);
		}
	}

	void changeGametime()
	{
		curTime = gameTimeTotal - pcvr.totalTime;

		fen = Mathf.FloorToInt(curTime / 60);
		miao = Mathf.FloorToInt(curTime -fen * 60);
		haomiao = Mathf.FloorToInt((curTime - Mathf.FloorToInt (curTime)) * 100);
		
		if (fen <= 0)
		{
			fen = 0;
		}
		
		if (miao <= 0)
		{
			miao = 0;
		}
		
		if (haomiao <= 0)
		{
			haomiao = 0;
		}

		if (fen <= 0 && miao <= 0 && haomiao <= 20)
		{
			haomiao = 0;
		}
		
		if (curTime > totalDownTimeChange)
		{
			gameTimeArr[0].spriteName = Mathf.FloorToInt(fen / 10).ToString();
			gameTimeArr[1].spriteName = (fen % 10).ToString();
			
			gameTimeArr[2].spriteName = Mathf.FloorToInt(miao / 10).ToString();
			gameTimeArr[3].spriteName = (miao % 10).ToString();
			
			gameTimeArr[4].spriteName = Mathf.FloorToInt(haomiao / 10).ToString();
			gameTimeArr[5].spriteName = (haomiao % 10).ToString();

			gameTimeArr[6].spriteName = "P";
			gameTimeArr[7].spriteName = "P";
		}
		else if (curTime > 0)
		{
			gameTimeArr[0].spriteName = "H" + Mathf.FloorToInt(fen / 10).ToString();
			gameTimeArr[1].spriteName = "H" + (fen % 10).ToString();
			
			gameTimeArr[2].spriteName = "H" + Mathf.FloorToInt(miao / 10).ToString();
			gameTimeArr[3].spriteName = "H" + (miao % 10).ToString();
			
			gameTimeArr[4].spriteName = "H" + Mathf.FloorToInt(haomiao / 10).ToString();
			gameTimeArr[5].spriteName = "H" + (haomiao % 10).ToString();
			
			gameTimeArr[6].spriteName = "HP";
			gameTimeArr[7].spriteName = "HP";
		}
		else
		{
			fen = 0;
			miao = 0;
			haomiao = 0;
			
			gameTimeArr[0].spriteName = "H" + Mathf.FloorToInt(fen / 10).ToString();
			gameTimeArr[1].spriteName = "H" + (fen % 10).ToString();
			
			gameTimeArr[2].spriteName = "H" + Mathf.FloorToInt(miao / 10).ToString();
			gameTimeArr[3].spriteName = "H" + (miao % 10).ToString();
			
			gameTimeArr[4].spriteName = "H" + Mathf.FloorToInt(haomiao / 10).ToString();
			gameTimeArr[5].spriteName = "H" + (haomiao % 10).ToString();

			if (!Network.isServer && !Network.isClient)
			{Debug.Log("jieeeeeeeeeeeeeeeeeeeeeeeeeeeeshule");
				pcvr.uiRunState = 3;
				//pcvr.isCanPressStartButtonEndGame = true;

				if (fanxiangObj.activeSelf)
				{
					fanxiangObj.SetActive(false);
				}

				pcvr.playerTruckSObj.stopChentuTimeend();
			}

			totalDownTime = totalDownTimeChange;
		}
	}
	float uiSpeedT = 0;
	float uiMaxSpeed = 160.0f;
	void changeSpeedShow()
	{
		if (uiSpeed < 0)
		{
			uiSpeed = 0;
		}

		if (Mathf.Abs(uiSpeed - maxSpeed) < 3.0f)
		{
			uiSpeedT = uiMaxSpeed;
		}
		else
		{
			uiSpeedT = (uiMaxSpeed * uiSpeed) / maxSpeed;
		}
		
		speed1 = Mathf.FloorToInt (uiSpeedT / 100);
		speed2 = Mathf.FloorToInt (uiSpeedT / 10) % 10;
		speed3 = Mathf.FloorToInt (uiSpeedT) % 10;

		speedNumArr[0].spriteName = speed1.ToString();
		speedNumArr[1].spriteName = speed2.ToString();
		speedNumArr[2].spriteName = speed3.ToString();

		//perA = (500 - 310) / uiMaxSpeed;
		//around
		angle = 500 - perA * uiSpeedT;

		if (angle <= 257)
		{
			angle = 260;
		}

		speedNumArr [3].transform.eulerAngles = new Vector3 ( 0, 0, angle);
	}

	public void coinFree()
	{Debug.Log ("pcvr.gameModePCVR  " +pcvr.gameModePCVR);
		if (pcvr.gameModePCVR == 1)
		{
			coinNumSprite3.spriteName = "x" + Mathf.FloorToInt(pcvr.startCoinNumPCVR / 10).ToString();
			coinNumSprite4.spriteName = "x" + Mathf.FloorToInt(pcvr.startCoinNumPCVR % 10).ToString();

			changeCoinNum(true);
			coinParentObject.SetActive(true);
		}
		else
		{//free
			insertCoinObj.SetActive(false);
			coinParentObject.SetActive(false);
			freeObject.SetActive (true);
			pcvr.startCoinNumPCVR = 0;
		}
	}

	public void hideAll()
	{
		startButtonObj.SetActive (false);
		insertCoinObj.SetActive(false);
		coinParentObject.SetActive(false);
		freeObject.SetActive (false);
	}

	public void changeCoinNum(bool flag)
	{
		coinNumSprite1.spriteName = Mathf.FloorToInt(pcvr.coinCurNumPCVR / 10).ToString();
		coinNumSprite2.spriteName = Mathf.FloorToInt(pcvr.coinCurNumPCVR % 10).ToString();

		insertCoinObj.SetActive(false);
	}

	//keyboard  -  T  -  insert coin
	void ClickInsertcoinBtEvent(ButtonState val)
	{
		if (Network.isServer)
		{
			return;
		}

		if (val == ButtonState.DOWN) {
			//coin
			if (!pcvr.bIsHardWare)
			pcvr.coinCurNumPCVR ++;
			changeCoinNum(false);
			
			if (pcvr.sound2DScrObj)
			{
				pcvr.sound2DScrObj.playAudioInsertCoin();
			}
		}
		else {
			//
		}
	}

	//keyboard - K - start game ----- only for time over, single model
	void ClickStartBtOneEventTTTTTTTT(ButtonState val)
	{//Debug.Log ("startbuttonnnnnnnnnnnnnnnnnnnnnnnn  uitruckkkkk " + val);
		if (Network.isServer)
		{
			return;
		}

		/*if (pcvr.uiRunState == 2 && pcvr.danqitishiSObj && val == ButtonState.DOWN)
		{
			pcvr.danqitishiSObj.useOneDanqi();
		}*/

		if (Network.isClient)
		{
			return;
		}

		if (pcvr.uiRunState != 3 ||  pcvr.coinCurNumPCVR < pcvr.startCoinNumPCVR)
		{//will change here, only time is over and coin number is enough can press start
			return;
		}

		if (val == ButtonState.DOWN)
		{
			//should change whether can press start................................
			if (daojishiObj.activeSelf)
			{
				daojishiObj.SetActive(false);
			}

			gameTimeTotal += gameTime;
			gameTimeTotalStatic += gameTime;
			pcvr.uiRunState = 2;
			
			if (pcvr.sound2DScrObj)
			{
				pcvr.sound2DScrObj.playAudioXubikaishi();
			}
		}
	}

	void ClickStartBtOneEvent(ButtonState val)
	{//Debug.Log ("startbuttonnnnnnnnnnnnnnnnnnnnnnnn  uitruckkkkk " + val);
		if (Network.isServer)
		{
			return;
		}
		
		if (Network.isClient)
		{
			return;
		}

		if (pcvr.scoreCtrlSobj.pressButtonState == 1)
		{//chengjibiao
			if (val == ButtonState.DOWN)
			{
				pcvr.scoreCtrlSobj.skipMoveBack();
			}
			return;
		}
		else if (pcvr.scoreCtrlSobj.pressButtonState == 2)
		{//jindutiao
			if (val == ButtonState.DOWN)
			{
				pcvr.scoreCtrlSobj.jiasuJindutiao();
			}
			return;
		}
		else if (pcvr.scoreCtrlSobj.pressButtonState == 3)
		{//moshi xuanze
			if (val == ButtonState.DOWN)
			{
				pcvr.scoreCtrlSobj.selectMoshi(false, false);
			}
			return;
		}
		
		/*if (!pcvr.isCanPressStartButtonEndGame || pcvr.coinCurNumPCVR < pcvr.startCoinNumPCVR)
		{//will change here, only time is over and coin number is enough can press start
			return;
		}*/
	}

	public void pressStartButtonLe(bool isPressButton)
	{
		//should change whether can press start................................
		if (daojishiObj.activeSelf)
		{
			daojishiObj.SetActive(false);
		}
		
		if (pcvr.sound2DScrObj)
		{
			pcvr.sound2DScrObj.playAudioXubikaishi();
		}
		
		//will into dong hua or xuanguan
		pressStartButtonEnd (isPressButton);
	}
	
	public void pressStartButtonLeOver(bool isPressButton)
	{
		//should change whether can press start................................
		if (daojishiObj.activeSelf)
		{
			daojishiObj.SetActive(false);
		}
		
		//will into dong hua or xuanguan
		pressStartButtonEnd (isPressButton);
	}

	//
	void ClickSetMoveBtEvent(ButtonState val)
	{
		if (val == ButtonState.DOWN || !frameObj) {
			return;
		}

		if (frameObj.activeSelf)
		{
			frameObj.SetActive(false);
		}
		else{
			frameObj.SetActive(true);
		}
	}
	
	void resetJiangliInfor()
	{//Debug.Log ("resetJi1xuchuagguanInforres1etJixuchuagguanInforresetJixu1chuagguanInfor");
		pcvr.shangciShengyuJiangli = 0.0f;
		pcvr.isChuangguan = false;
		pcvr.canFree = false;
		
		for (int i=0; i < pcvr.guakaDengji.Length; i++)
		{
			pcvr.guakaDengji[i] = -1;
		}
		
		pcvr.curPingjiDengji = -1;
	}

	void ClickSetEnterBtEvent(ButtonState val)
	{
		if (val == ButtonState.DOWN) {
			return;
		}

		resetJiangliInfor ();

		XkGameCtrl.IsLoadingLevel = true;
		Resources.UnloadUnusedAssets();
		//GC.Collect();
		
		if (pcvr.sound2DScrObj)
		{
			pcvr.sound2DScrObj.playAudioBeijing (false);
		}

		pcvr.GetInstance().CloseFangXiangPanPower();
		Application.LoadLevel(1);	//into the game-set panel
	}
	
	void ClickCloseDongGanBtEvent(ButtonState val)
	{
		if (val == ButtonState.DOWN) {
			return;
		}
		
		/*if (DongGanCtrl.GetInstance() == null) {
			return;
		}
		IsCloseDongGan = !IsCloseDongGan;
		HandleDongGanUI();*/
	}

	public void ClickLaBaBt(ButtonState val)
	{
		if (val == ButtonState.DOWN)
		{
			pcvr.playerTruckSObj.playAudioMingdi(true);
		}
		else if (val == ButtonState.UP)
		{
			pcvr.playerTruckSObj.playAudioMingdi(false);
		}
	}
	
	public void ClickChangeCameraBt(ButtonState val)
    {
        if (pcvr.uiRunState < 2 || pcvr.uiRunState == 10 || pcvr.uiRunState == 3 || pcvr.isPassgamelevel)
        {
            return;
        }

        if (SSGameUIRoot.GetInstance().m_SSExitGameUI != null)
        {
            return;
        }

        if (val == ButtonState.DOWN)
		{
			pcvr.XKCarCameraSObj.setToFirstThirdPerson();
		}
	}
	
	/*void HandleDongGanUI()
	{
		if (DongGanCtrl.GetInstance() == null) {
			return;
		}
		
		if (!IsCloseDongGan) {
			DongGanCtrl.GetInstance().ShowDongGanOpen();
		}
		else {
			DongGanCtrl.GetInstance().ShowDongGanClose();
		}
	}*/


	//whether to show "please insert coin"
	//the game mode is operate
	//befor starting game or after the game time is ending
	void detectInsertCoin()
	{
		if (pcvr.gameModePCVR != 1)
		{
			pcvr.startCoinNumPCVR = 0;
			insertCoinObj.SetActive(false);
			return;
		}

		if (pcvr.coinCurNumPCVR < pcvr.startCoinNumPCVR)
		{
			insertCoinObj.SetActive(true);
		}
	}

	private int tempFind = 0;
	//show every player information ---- the left-top position
	void paiMing()
	{
		tempdis = 0;
		tempjiaohui = 0;
		templahui = 0;
		tempIndex = 0;

		for (int i=0; i<4; i++)
		{
			distanceFourTemp[i] = pcvr.distanceFour[i];
			jiaohuiPointArrTemp[i] = pcvr.jiaohuiPointArr[i];
			lahuiPointArrTemp[i] = pcvr.lahuiPointArr[i];
			pcvr.indexOnly[i] = i;
		}

		//from "high" to "low"
		for (int a=0; a<4; a++)
		{
			for (int b=0; b< 3 - a; b++)
			{
				if ((jiaohuiPointArrTemp[b] < jiaohuiPointArrTemp[b + 1])
				    || (jiaohuiPointArrTemp[b] == jiaohuiPointArrTemp[b + 1] && lahuiPointArrTemp[b] < lahuiPointArrTemp[b + 1 ])
				    || (jiaohuiPointArrTemp[b] == jiaohuiPointArrTemp[b + 1] && lahuiPointArrTemp[b] == lahuiPointArrTemp[b + 1 ] && distanceFourTemp[b] < distanceFourTemp[b + 1]))
				{
					tempdis = distanceFourTemp[b];
					distanceFourTemp[b] = distanceFourTemp[b + 1];
					distanceFourTemp[b + 1] = tempdis;

					tempjiaohui = jiaohuiPointArrTemp[b];
					jiaohuiPointArrTemp[b] = jiaohuiPointArrTemp[b + 1];
					jiaohuiPointArrTemp[b + 1] = tempjiaohui;
					
					templahui = lahuiPointArrTemp[b];
					lahuiPointArrTemp[b] = lahuiPointArrTemp[b + 1];
					lahuiPointArrTemp[b + 1] = templahui;

					tempIndex = pcvr.indexOnly[b];
					pcvr.indexOnly[b] = pcvr.indexOnly[b + 1];
					pcvr.indexOnly[b + 1] = tempIndex;
				}
			}
		}
		//Debug.Log (pcvr.indexOnly [0] + " " + pcvr.indexOnly [1] + " " + pcvr.indexOnly [2] + " " + pcvr.indexOnly [3]);
		//example: indexfour[4]----0 1 6 7(P1 P2 AI3 AI4)
		//		:indexOnly[4]-----according to the distance give the mingci
		//		:
		//the position not active
		for (int i=0; i<4; i++) 
		{
			if (pcvr.indexOnly[i] == 0)
			{
				pcvr.mingciFirstPlayer = i;
			}

			if (pcvr.truckObjScriptArr[pcvr.indexOnly[i]])
			pcvr.truckObjScriptArr[pcvr.indexOnly[i]].mingciSelf = i;
		}
		
		//pcvr.mingciFirstPlayer = firstPlayerMingciTemp;

		//showPlayerInfor ();
	}

	void showPlayerInfor()
	{
		for (int i=0; i<4; i++) 
		{
			tempFind = pcvr.indexFour[pcvr.indexOnly[i]];
			//Debug.Log("showPlayerInfor "+ i + " "+ tempFind + " "+ pcvr.indexOnly[i]);
			//self or not
			if (i == 0 && pcvr.selfIndex == tempFind)
			{//is self and is the first
				backSmallArr[i].SetActive(false);
				nameSpriteArr[i + 4].spriteName = nameArrYellow[tempFind + 8];	//0-7 is small, 8-11 is larger(1p 2p 3p 4p)
				backLargeArr[i].SetActive(true);
			}
			else if (i == 0)
			{//not self but is the first
				backLargeArr[i].SetActive(false);
				nameSpriteArr[i].spriteName = nameArrYellow[tempFind];
				backSmallArr[i].SetActive(true);
			}
			else if (i > 0 && pcvr.selfIndex == tempFind)
			{//self but not the first
				backSmallArr[i].SetActive(false);
				nameSpriteArr[i + 4].spriteName = nameArrBlack[tempFind + 8];
				backLargeArr[i].SetActive(true);
			}
			else
			{
				backLargeArr[i].SetActive(false);
				nameSpriteArr[i].spriteName = nameArrBlack[tempFind];
				backSmallArr[i].SetActive(true);
			}

			//pcvr.mingciFirstPlayer = firstPlayerMingciTemp;

			//pcvr.truckObjScriptArr[pcvr.indexOnly[i]].mingciSelf = i;
		}
	}
	private bool isLinkMode = false;
	public void showPlayerFinalScore(bool isLinkModeT)
	{
		float selfTotalTime = 0.0f;

		hideSomeObjects ();

		gameOverObj.SetActive (false);
		//scoreObject.SetActive (true);
		Debug.Log ("finalllllllll  " + pcvr.indexOnlyFinal[0] + " "+ pcvr.indexOnlyFinal[1] + " "+ pcvr.indexOnlyFinal[2] + " "+ pcvr.indexOnlyFinal[3]);

		int[] indexOnlyT = new int[]{0, 1, 2, 3};

		for (int j = 0; j < 4; j++)
		{
			for (int a = 0; a < 4; a++)
			{
				if (pcvr.indexOnlyFinal[j] == indexOnlyT[a])
				{
					indexOnlyT[a] = -2;
					break;
				}
			}
		}

		for (int j = 0; j < 4; j++)
		{
			if (indexOnlyT[j] >= 0)
			{
				for (int a = 0; a < 4; a++)
				{
					if (pcvr.indexOnlyFinal[a] < 0)
					{
						pcvr.indexOnlyFinal[a] = indexOnlyT[j];
						indexOnlyT[j] = -2;
						break;
					}
				}
			}
		}
		Debug.Log ("finalllllllllxgh  " + pcvr.indexOnlyFinal[0] + " "+ pcvr.indexOnlyFinal[1] + " "+ pcvr.indexOnlyFinal[2] + " "+ pcvr.indexOnlyFinal[3]);

		//init first
		//huang guan
		huangGuanSprite.atlas = ScoreAtlas;

		for (int h=0; h<4; h++)
		{
			//ming ci
			mingciSpriteArr[h].atlas = ScoreAtlas;

			//player-name infor
			playerSpriteObj[h].atlas = ScoreAtlas;

			//left time
			pTimespriteObjArr[h, 0].atlas = ScoreAtlas;
			pTimespriteObjArr[h, 1].atlas = ScoreAtlas;
			
			pTimespriteObjArr[h, 2].atlas = ScoreAtlas;
			pTimespriteObjArr[h, 3].atlas = ScoreAtlas;
			
			pTimespriteObjArr[h, 4].atlas = ScoreAtlas;
			pTimespriteObjArr[h, 5].atlas = ScoreAtlas;

			//total time
			pTimespriteObjArr[h, 6].atlas = ScoreAtlas;
			pTimespriteObjArr[h, 7].atlas = ScoreAtlas;
			
			pTimespriteObjArr[h, 8].atlas = ScoreAtlas;
			pTimespriteObjArr[h, 9].atlas = ScoreAtlas;
			
			pTimespriteObjArr[h, 10].atlas = ScoreAtlas;
			pTimespriteObjArr[h, 11].atlas = ScoreAtlas;

			//mao hao
			for (int j = 0; j < 4; j++)
			{
				maohaoSprite[h * 4 + j].atlas = ScoreAtlas;
			}
		}

		int selfLastIndex = 0;
		bool selfWanchengle = false;

		for (int i=0; i<4; i++)
		{tempFind = pcvr.indexFour[pcvr.indexOnlyFinal[i]];
			//the back sprite
			if (pcvr.selfIndex == tempFind)
			{selfLastIndex = i;
				//is self
				//player - name
				playerSpriteObj[i].atlas = ScoreSelfAtlas;

				//mingci
				mingciSpriteArr[i].atlas = ScoreSelfAtlas;

				//huang guan
				if (i == 0)
				{
					huangGuanSprite.atlas = ScoreSelfAtlas;
				}
				
				//left time
				pTimespriteObjArr[i, 0].atlas = ScoreSelfAtlas;
				pTimespriteObjArr[i, 1].atlas = ScoreSelfAtlas;
				
				pTimespriteObjArr[i, 2].atlas = ScoreSelfAtlas;
				pTimespriteObjArr[i, 3].atlas = ScoreSelfAtlas;
				
				pTimespriteObjArr[i, 4].atlas = ScoreSelfAtlas;
				pTimespriteObjArr[i, 5].atlas = ScoreSelfAtlas;
				
				//total time
				pTimespriteObjArr[i, 6].atlas = ScoreSelfAtlas;
				pTimespriteObjArr[i, 7].atlas = ScoreSelfAtlas;
				
				pTimespriteObjArr[i, 8].atlas = ScoreSelfAtlas;
				pTimespriteObjArr[i, 9].atlas = ScoreSelfAtlas;
				
				pTimespriteObjArr[i, 10].atlas = ScoreSelfAtlas;
				pTimespriteObjArr[i, 11].atlas = ScoreSelfAtlas;
				
				//mao hao
				for (int j = 0; j < 4; j++)
				{
					maohaoSprite[i * 4 + j].atlas = ScoreSelfAtlas;
				}

				backSpriteObject[i].spriteName = "score5";

				float totalTimeaY = pcvr.useTimeSelf[pcvr.indexOnlyFinal[i]];

				if (totalTimeaY > 0)
				{
					selfTotalTime = totalTimeaY;

				}
				else
				{
					selfTotalTime = pcvr.useTimeSelfTemp[pcvr.indexOnlyFinal[i]];
				}
			}
			else
			{
				//is not self
				backSpriteObject[i].spriteName = "score4";
			}

			//the player name here
			playerSpriteObj[i].spriteName = nameArray[tempFind];

			//show or hide the time/taotai/weiwancheng
			float totalTimeY = pcvr.useTimeSelf[pcvr.indexOnlyFinal[i]];

			//if (playerStateArr[pcvr.indexOnly[i]] == 1)
			if (totalTimeY > 0)
			{//complete
				if (pcvr.selfIndex == tempFind)
				selfWanchengle = true;
				timeOtherObject[0 + i * 4].SetActive(true);
				timeOtherObject[1 + i * 4].SetActive(true);
				timeOtherObject[2 + i * 4].SetActive(false);
				timeOtherObject[3 + i * 4].SetActive(false);
			}
			//else if (playerStateArr[pcvr.indexOnly[i]] == 2)
			else if (totalTimeY < 0)
			{//lost
				timeOtherObject[0 + i * 4].SetActive(false);
				timeOtherObject[1 + i * 4].SetActive(false);
				timeOtherObject[2 + i * 4].SetActive(true);
				timeOtherObject[3 + i * 4].SetActive(false);
			}
			//else if (playerStateArr[pcvr.indexOnly[i]] == 3)
			else if (totalTimeY == 0)
			{//not complete
				timeOtherObject[0 + i * 4].SetActive(false);
				timeOtherObject[1 + i * 4].SetActive(false);
				timeOtherObject[2 + i * 4].SetActive(false);
				timeOtherObject[3 + i * 4].SetActive(true);
			}

			if (totalTimeY > 0)
			{
				//change the time here -----------fffffffffffffffffffffffffffffff  shold use the left time and another time  lxy
				//time1	----	left time
				float totalTimeL = gameTimeTotalStatic + pcvr.totalTimeAddDaoju - totalTimeY;
				int fenL = 0;
				int miaoL = 0;
				int haomiaoL = 0;
				
				fenL = Mathf.FloorToInt(totalTimeL / 60);
				miaoL = Mathf.FloorToInt(totalTimeL -fenL * 60);
				haomiaoL = Mathf.FloorToInt((totalTimeL - Mathf.FloorToInt (totalTimeL)) * 100) + 1;

				if (fenL <= 0)
				{
					fenL = 0;
				}

				if (miaoL <= 0)
				{
					miaoL = 0;
				}

				if (haomiaoL <= 0)
				{
					haomiaoL = 0;
				}

				pTimespriteObjArr[i, 0].spriteName = Mathf.FloorToInt(fenL / 10).ToString();
				pTimespriteObjArr[i, 1].spriteName = (fenL % 10).ToString();
				
				pTimespriteObjArr[i, 2].spriteName = Mathf.FloorToInt(miaoL / 10).ToString();
				pTimespriteObjArr[i, 3].spriteName = (miaoL % 10).ToString();
				
				pTimespriteObjArr[i, 4].spriteName = Mathf.FloorToInt(haomiaoL / 10).ToString();
				pTimespriteObjArr[i, 5].spriteName = (haomiaoL % 10).ToString();

				//time2	----	total time
				int fenY = 0;
				int miaoY = 0;
				int haomiaoY = 0;

				fenY = Mathf.FloorToInt(totalTimeY / 60);
				miaoY = Mathf.FloorToInt(totalTimeY -fenY * 60);
				haomiaoY = Mathf.FloorToInt((totalTimeY - Mathf.FloorToInt (totalTimeY)) * 100);
				
				if (fenY <= 0)
				{
					fenY = 0;
				}
				
				if (miaoY <= 0)
				{
					miaoY = 0;
				}
				
				if (haomiaoY <= 0)
				{
					haomiaoY = 0;
				}

				pTimespriteObjArr[i, 6].spriteName = Mathf.FloorToInt(fenY / 10).ToString();
				pTimespriteObjArr[i, 7].spriteName = (fenY % 10).ToString();
				
				pTimespriteObjArr[i, 8].spriteName = Mathf.FloorToInt(miaoY / 10).ToString();
				pTimespriteObjArr[i, 9].spriteName = (miaoY % 10).ToString();
				
				pTimespriteObjArr[i, 10].spriteName = Mathf.FloorToInt(haomiaoY / 10).ToString();
				pTimespriteObjArr[i, 11].spriteName = (haomiaoY % 10).ToString();
			}
		}

		float recordTimeT = 0;
		recordTimeT = (float)ReadGameInfo.GetInstance ().getRecordTime (Application.loadedLevel);

		bool isChuangjilu = false;

		//self score here
		if ( recordTimeT <= 0 || (recordTimeT > 0 && selfTotalTime < recordTimeT))
		{
			//show
			if (selfWanchengle)
				isChuangjilu = true;

			if (isChuangjilu)
			ReadGameInfo.GetInstance ().WriteGameRecordAllLevel (Application.loadedLevel, selfTotalTime.ToString());
			
			string result=selfTotalTime.ToString( "#0.0000");
			//Debug.Log(ReadGameInfo.GetInstance().getRecordTime(Application.loadedLevel) + " "+ selfTotalTime + " "+ result + " "+ isChuangjilu);
		}

		if (!isLinkModeT && !isLinkmodeRecord)
		showRecordAndSelfTime (isChuangjilu, recordTimeT, selfTotalTime, selfLastIndex);

		isLinkMode = isLinkModeT;
		//Debug.Log ("islinkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkk " + isLinkmodeRecord + " "+ isLinkModeT);
		if (!isLinkModeT && !isLinkmodeRecord)
		{//single mode
			//
			//scoreObject.SetActive (true);
			//Invoke ("endLeeeeee", 7.0f);	//will change

			if (pcvr.scoreCtrlSobj)
			{
				daojishiObjClient.SetActive(false);
				daojishiObj.SetActive(false);
				chuangjiluObj.SetActive (false);
				scoreObject.SetActive (true);
				pcvr.scoreCtrlSobj.beginShowScoreGui(selfLastIndex, isChuangjilu, selfTotalTime, selfWanchengle);
			}
		}
		else
		{//link mode
			daojishiObjClient.SetActive(false);
			daojishiObj.SetActive(false);
			scoreObject.SetActive (true);

			pcvr.scoreCtrlSobj.ClientShowScore();
			Invoke ("endLeeeeee", 7.0f);
        }

        if (SSGameUIRoot.GetInstance().m_SSExitGameUI != null)
        {
            SSGameUIRoot.GetInstance().RemoveSelf();
        }
    }

	void endLeeeeee()
	{
		if (pcvr.sound2DScrObj)
		{
			pcvr.sound2DScrObj.playAudioBeijing (false);
		}

		if (isLinkMode)
		{
			setLoadObjVisible (true, 300);
			clearAllTheObj ();
			Invoke ("endLeLoadNext", 1.0f);
		}
		else
		{
			scoreObject.SetActive (false);
			gameTimeRSObj.SetActive(false);
			pcvr.isCanPressStartButtonEndGame = true;
			pcvr.isAfterJifen = true;
			//will into donghua or xuanguan
			totalDownTime = totalDownTimeChange;
			return;
		}
	}

	void endLeLoadNext()
	{
		//if link mode, here will change
		if (isLinkMode)
		{
			Application.LoadLevel (0);	//game end
		}
		else
		{
			//no use
			if (pcvr.isPassgamelevel && Application.loadedLevel + 1 < Application.levelCount)
			{//the game level is start from 3, 0 is donghua and 1 is game-set Panel
				//Application.LoadLevel (3);	//will change here
				Application.LoadLevel (Application.loadedLevel + 1);	//game end
			}
			else
			{
				Application.LoadLevel (0);	//game end
			}
		}
	}
	
	//20170908
	void pressStartButtonEnd(bool isPressButtonT)
	{
		pcvr.uiRunState = 10;
		pcvr.isPressStartButton = isPressButtonT;
		pcvr.isCanPressStartButtonEndGame = false;

		hideSomeObjects ();

		setLoadObjVisible (true, 300);
		clearAllTheObj ();
		Invoke ("endLeLoadNextSingle", 1.0f);
	}

	void notPressStartButtonEnd()
	{
		pcvr.uiRunState = 10;
		pcvr.isCanPressStartButtonEndGame = false;
		
		hideSomeObjects ();

		setLoadObjVisible (true, 300);
		clearAllTheObj ();
		Invoke ("endLeLoadNextSingle", 1.0f);
	}

	void endLeLoadNextSingle()
	{
		Application.LoadLevel (0);
	}

	public void AddGameTime(int index)
	{
		if (index == 1)
		{//10s
			gameTimeTotal += addGameTime1;
			pcvr.totalTimeAddDaoju += addGameTime1;
			addTimeUISobj.spriteName = "10s";
			ShowAddtime();

		}
		else if (index == 2)
		{//15s
			gameTimeTotal += addGameTime2;
			pcvr.totalTimeAddDaoju += addGameTime2;
			addTimeUISobj.spriteName = "15s";
			ShowAddtime();
		}
	}
	
	void ShowAddtime()
	{
		addTimeObj.SetActive(true);

		addTimeTScale.ResetToBeginning ();
		EventDelegate.Add (addTimeTScale.onFinished, onFinishedScale);
		addTimeTScale.enabled = true;
		addTimeTScale.PlayForward ();
		
		addTimeTPos.ResetToBeginning ();
		addTimeTPos.enabled = true;
		addTimeTPos.PlayForward ();
	}
	
	void onFinishedScale()
	{
		addTimeTScale.enabled = false;
		addTimeTPos.enabled = false;
		EventDelegate.Remove(addTimeTScale.onFinished, onFinishedScale);

		Invoke ("hideAddtime", 0.5f);
	}

	void hideAddtime()
	{
		addTimeObj.SetActive(false);
	}

	public void LinkModeServerGameEnd()
	{
		pcvr.uiRunState = 10;
		setLoadObjVisible (true, 30);
		//clearAllTheObj ();
		
		if (pcvr.sound2DScrObj)
		{
			pcvr.sound2DScrObj.playAudioBeijing (false);
		}

		Invoke ("endLeeeeeeServer", 2.5f);
	}
	
	void endLeeeeeeServer()
	{clearAllTheObj ();
		Application.LoadLevel (0);	//game end - server
	}

	public void LinkOnePassLevel()
	{//will change the time here
		/*int timeT = 0;

		if (gameTimeTotal - pcvr.totalTime > totalDownTimeChange)
		{
			timeT = Mathf.FloorToInt(gameTimeTotal - totalDownTimeChange);
		}
		else
		{
			timeT = Mathf.FloorToInt(pcvr.totalTime);
		}

		pcvr.totalTime = timeT * 1.0f;*/

		pcvr.totalTime = gameTimeTotal - totalDownTimeChange;
	}

	public void hideClientDaojishi()
	{
		if (daojishiObjClient && daojishiObjClient.activeSelf)
		{
			daojishiObjClient.SetActive(false);
		}
	}

	void hideSomeObjects()
	{
		if (fanxiangObj.activeSelf)
		{
			fanxiangObj.SetActive(false);
		}

		if (mingciObjEnd && mingciObjEnd.activeSelf)
		{
			mingciObjEnd.SetActive(false);
		}

		if (youmenObjEnd && youmenObjEnd.activeSelf)
		{
			youmenObjEnd.SetActive(false);
		}

		if (timeObjEnd && timeObjEnd.activeSelf)
		{
			timeObjEnd.SetActive(false);
		}

		if (mapObjEnd && mapObjEnd.activeSelf)
		{
			mapObjEnd.SetActive(false);
		}

		pcvr.GetInstance ().CloseFangXiangPanPower ();
	}

	public GameObject gameTimeRSObj = null;
	public GameObject chuangjiluObj = null;
	public Transform[] chuangjiluPosArr;
	
	public UISprite[] gameTimeArrR;
	private float curTimeR = 0;
	private int fenR = 0;
	private int miaoR = 0;
	private int haomiaoR = 0;
	
	public UISprite[] gameTimeArrS;
	private float curTimeS = 0;
	private int fenS = 0;
	private int miaoS = 0;
	private int haomiaoS = 0;

	void showRecordAndSelfTime(bool ischuangjiluT, float recordTime, float selfTotalTime, int selfIndex)
	{
		if (recordTime < 0)
		{
			recordTime = 0;
		}

		if (selfTotalTime < 0)
		{
			selfTotalTime = 0;
		}

		gameTimeRSObj.SetActive (true);
		chuangjiluObj.SetActive (false);
		chuangjiluObj.transform.position = chuangjiluPosArr[selfIndex].position;
		chuangjiluObj.SetActive (ischuangjiluT);

		//record time
		curTimeR = recordTime;
		
		fenR = Mathf.FloorToInt(curTimeR / 60);
		miaoR = Mathf.FloorToInt(curTimeR -fenR * 60);
		haomiaoR = Mathf.FloorToInt((curTimeR - Mathf.FloorToInt (curTimeR)) * 100);
		Debug.Log ("aaaaaaaaaaaaaaaaaa  f " + fenR + " m " +miaoR + " h " +haomiaoR + " " +curTimeR + " " + selfIndex);
		gameTimeArrR[0].spriteName = Mathf.FloorToInt(fenR / 10).ToString();
		gameTimeArrR[1].spriteName = (fenR % 10).ToString();
		
		gameTimeArrR[2].spriteName = Mathf.FloorToInt(miaoR / 10).ToString();
		gameTimeArrR[3].spriteName = (miaoR % 10).ToString();
		
		gameTimeArrR[4].spriteName = Mathf.FloorToInt(haomiaoR / 10).ToString();
		gameTimeArrR[5].spriteName = (haomiaoR % 10).ToString();
		
		gameTimeArrR[6].spriteName = "P";
		gameTimeArrR[7].spriteName = "P";
		return;
		/*
		//self time
		curTimeS = selfTotalTime;
		
		fenS = Mathf.FloorToInt(curTimeS / 60);
		miaoS = Mathf.FloorToInt(curTimeS -fenS * 60);
		haomiaoS = Mathf.FloorToInt((curTimeS - Mathf.FloorToInt (curTimeS)) * 100);

		gameTimeArrS[0].spriteName = "H" +Mathf.FloorToInt(fenS / 10).ToString();
		gameTimeArrS[1].spriteName = "H" +(fenS % 10).ToString();
		
		gameTimeArrS[2].spriteName = "H" +Mathf.FloorToInt(miaoS / 10).ToString();
		gameTimeArrS[3].spriteName = "H" +(miaoS % 10).ToString();
		
		gameTimeArrS[4].spriteName = "H" +Mathf.FloorToInt(haomiaoS / 10).ToString();
		gameTimeArrS[5].spriteName = "H" +(haomiaoS % 10).ToString();
		
		gameTimeArrS[6].spriteName = "HP";
		gameTimeArrS[7].spriteName = "HP";*/
	}
	
	public GameObject zuihouyiquanObj = null;

	public void showZuihouyiquan()
	{
		if (zuihouyiquanObj)
		{
			zuihouyiquanObj.SetActive(true);
		}
	}

	//en one circle and go on
	public void clearAllNPCObj()
	{
		for (int i = 0; i < pcvr.chanshengdeObjNum; i++)
		{
			//Debug.Log("chanshengle    " + i + " " + pcvr.chanshengdeObj[i]);
			Destroy(pcvr.chanshengdeObj[i]);
			pcvr.chanshengdeObj[i] = null;
		}
		
		pcvr.chanshengdeObjNum = 0;
		
		for (int i = 0; i < pcvr.chanshengdeObjNetNum; i++)
		{
			//Debug.Log("chanshengle Netttt   " + i + " " + pcvr.chanshengdeObjNet[i]);
			Network.Destroy(pcvr.chanshengdeObjNet[i]);
			Destroy(pcvr.chanshengdeObjNet[i]);
			pcvr.chanshengdeObjNet[i] = null;
		}
		
		pcvr.chanshengdeObjNetNum = 0;
		pcvr.aFirstScriObj.deleteDanqi ();

		//will create danqi Later
		Invoke ("createDanqiLater", 0.5f);
	}

	void createDanqiLater()
	{
		pcvr.aFirstScriObj.createDanqi222 (false);
	}

	void clearAllTheObj()
	{
		if (pcvr.XKCarCameraSObj)
		{
			pcvr.XKCarCameraSObj.removeParent ();
		}

		for (int i = 0; i < pcvr.chanshengdeObjNum; i++)
		{
			//Debug.Log("chanshengle    " + i + " " + pcvr.chanshengdeObj[i]);
			Destroy(pcvr.chanshengdeObj[i]);
			pcvr.chanshengdeObj[i] = null;
		}

		pcvr.chanshengdeObjNum = 0;

		for (int i = 0; i < pcvr.chanshengdeObjNetNum; i++)
		{
			//Debug.Log("chanshengle Netttt   " + i + " " + pcvr.chanshengdeObjNet[i]);
			Network.Destroy(pcvr.chanshengdeObjNet[i]);
			Destroy(pcvr.chanshengdeObjNet[i]);
			pcvr.chanshengdeObjNet[i] = null;
		}
		
		pcvr.chanshengdeObjNetNum = 0;
		pcvr.aFirstScriObj.deleteDanqi ();

	}
	
	public GameObject tishiObj = null;
	public UISprite tishiSpriteObj = null;

	public void showTishiObj(int indexShow, float timeShow)
	{
		if (tishiObj)
		{
			tishiObj.SetActive(true);
			tishiSpriteObj.spriteName = indexShow.ToString();

			CancelInvoke("HideTishiObj");
			Invoke ("HideTishiObj", timeShow);
		}
	}

	void HideTishiObj()
	{
		if (tishiObj)
		{
			tishiObj.SetActive(false);
		}
	}
}
