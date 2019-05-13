using UnityEngine;
using System.Collections;
using System;

public class donghua : MonoBehaviour {
    /// <summary>
    /// UI中心锚点.
    /// </summary>
    public Transform m_UICenterTr;
	//this judge the game what show first
	//hide the game gui, the link gui,
	//if into game, this script will be hidden
	public MovieTexture movieTexture = null;
	public GameObject movieObj = null;

	public GameObject zuoyiObj = null;
	public GameObject frameObj = null;
	
	public static GameObject serverCtrO = null;
	public GameObject ServerCtrlObjPrefab = null;

	public GameObject coinPanel = null;		//child
	public GameObject linkGuiObj = null;	//link
	public GameObject sSelectGuiObj = null;	//single
	private bool isTestSingleSelect = true;
	
	private bool isReadying = false;
	
	public GameObject soundControlPrefab;
	public GameObject soundControlENDPrefab;

	public Transform donghuaCharuObj = null;

	private float detectTime = 0f;
	private bool hasDetect = false;

	private bool isStopWait = false;
	private float waitTime = 0.0f;
	public bool isEndDonghua = false;

	private float jiangeCurTime = 0.5f;
	private float jiangeCurShortTime = 0.2f;
	private float liangTime = 0.05f;
	private float liangStaticTime = 0.05f;
	private int shanState = -1;
	private int xunhuanState = -1;
	private int xunhuanCishu = 0;
	private bool isMieT = false;

	private float beginTimeT = 0.0f;
	private bool hasReadGameSetInfo = false;

	public UIAtlas[] UIENAtlasObjSpecia;//load-beijing-chatu-select
	public UIAtlas UIEN1AtlasObj;//en1
	public UISprite[] spriteENObjSpecia;//load-beijing-chatu-select
	public UISprite[] spriteENObjSelect;
	public UISprite[] spriteENObj;//other
	
	public Shader nowShader = null;
	public Shader nowShader2 = null;
	
	void Update()
	{
		//if (Time.frameCount % 100 == 0)
		//Debug.Log (movieTexture.duration + " " + audio.clip.length + " " + audio.time + " " +(movieTexture.duration - audio.time));
		if (!hasReadGameSetInfo)
		{
			if (beginTimeT < 0.5f)
			{
				beginTimeT += Time.deltaTime;
			}
			else
			{
				hasReadGameSetInfo = true;
				pcvr.GetInstance().getGamesetInfor();
				
				doAfterReadGamesetInfo ();
			}
			return;
		}

		if (isShowShijiao)
		{
			shijiaoUpdate ();
		}

		if (movieTexture != null && donghuaCharuObj && !isEndDonghua)
		{//Debug.Log (movieTexture.duration + " " + audio.clip.length + " " + audio.time + " " +(movieTexture.duration - audio.time));
			if ((audio.time >= 68.2f && audio.time < 76f) || (audio.time >= 146.48f && audio.time < 154.28f))
			{
				//show the uiSprite
				donghuaCharuObj.localPosition = new Vector3(0, 0, 658);
			}
			else
			{
				donghuaCharuObj.localPosition = new Vector3(3000, 0, 658);
			}
		}

		if (!Network.isServer && pcvr.UIState == 1 && pcvr.coinCurNumPCVR >= pcvr.startCoinNumPCVR && pcvr.StartBtLight != StartLightState.Shan)
		{
			pcvr.StartBtLight = StartLightState.Shan;
		}
	}
	
	// Use this for initialization
	void Start () {//Time.timeScale = 0.5f;
		if (pcvr.bIsHardWare)
			Screen.showCursor = false;
		
		isShowShijiao = false;
		spriteObj.gameObject.SetActive(false);
		setLoadObjVisible (true, 1023);

		beginTimeT = 0.0f;
		hasReadGameSetInfo = false;

		pcvr.nowShaderPcvr1 = nowShader;
		pcvr.nowShaderPcvr2 = nowShader2;

		QualitySettings.currentLevel = QualityLevel.Fantastic;
		Debug.Log ("app level " + QualitySettings.GetQualityLevel());

		if (zuoyiObj)
		{
			zuoyiObj.SetActive(true);
		}
		
		if (frameObj)
		{
			frameObj.SetActive(false);
		}

		pcvr.GetInstance().initInforAgain(true);
		detectTime = Time.time;
		hasDetect = false;
		pcvr.StartBtLight = StartLightState.Mie;
		pcvr.ShaCheBtLight = StartLightState.Mie;
		donghuaCharuObj.localPosition = new Vector3(3000, 0, 658);

		isStopWait = false;
		waitTime = 0.0f;
		isEndDonghua = false;
		isShowShijiao = false;

		//pcvr.CloseFangXiangPanPower();
		createSoundControlObj ();
		pcvr.donghuaScriObj = GetComponent<donghua>();

		//need chang here ----------------- fffffffffffffffffffffffffff
		pcvr.firstPlayerIndex = -1;
		pcvr.totalPlayerNum = -1;
		pcvr.selfIndex = -1;
		pcvr.mingciFirstPlayer = 0;
		pcvr.uiRunState = -1;
		pcvr.total321Time = 0;
		pcvr.totalTime = 0;
		pcvr.totalTimeServer = 0;
		pcvr.totalTimeAddDaoju = 0;
		pcvr.isPassgamelevel = false;
		pcvr.finishedNumber = 0;
		pcvr.finishedPlayerNumber = 0;
		
		isReadying = true;
		pcvr.UIState = 0;

		if (sSelectGuiObj)
		{
			sSelectGuiObj.SetActive (false);
		}
		XkGameCtrl.IsLoadingLevel = false;
		if (movieTexture != null)
		{Debug.Log("play movieeeeeeeeeeeeeeeeeeeeeeeeeeeeeeefffffffffe");
			movieObj.SetActive(true);
			
			if (!Network.isServer && GetComponent<AudioSource>())
			{
				audio.clip = movieTexture.audioClip;
			}
			
			movieTexture.loop = true;
			movieTexture.Play();
            //audio.Play ();
        }

        SSGameUIRoot.GetInstance().m_UICenterTr = m_UICenterTr;
    }

	void doAfterReadGamesetInfo()
	{
		ReadGameInfo.GetInstance ().ReadGameNetwork();
		
		for (int i=0; i<UIENAtlasObjSpecia.Length; i++)
		{
			pcvr.UIENAtlasObjSpeciaPCVR [i] = UIENAtlasObjSpecia[i];
		}
		
		pcvr.UIEN1AtlasObjPCVR = UIEN1AtlasObj;
		
		if (serverCtrO)
		{
			Network.Destroy(serverCtrO);
		}
		
		pcvr.selfServerObj = null;
		pcvr.selfServerScrObj = null;
		
		Network.Disconnect ();
		
		pcvr.UIState = 1;
		
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
		
		coinFree();
		startButtonShow (true);
		setLoadObjVisible (false, 20);
		
		//first hide all the gui
		//coinPanel.SetActive (false);
		linkGuiObj.SetActive (false);
		
		if (pcvr.languagePCVR == 0)
		{
			changeENAtlas();
		}
		
		//only for testtttttttttttttttttttttttttttt
		detectPlayMovie ();

		int volumNum = Convert.ToInt32(ReadGameInfo.GetInstance ().ReadGameVolumNum ());
		Debug.Log ("volumNumttblll ==   " + volumNum);
		AudioListener.volume = volumNum * 0.1f;
	}
	
	public void singleModeBegin(int indexT, int levelIndex)
	{Debug.Log ("singleMo1deBeginsingleM1odeBegin  === " + indexT + " "+ levelIndex);
		if (indexT == 1 && isTestSingleSelect)
		{
			if (sSelectGuiObj)
			{
				sSelectGuiObj.SetActive(true);
				return;
			}
			else
			{
				levelIndex = 2;
			}
		}

		isEndDonghua = true;
		setLoadObjVisible (true, 3);
		movieObj.SetActive(false);
		
		doDonghuaEnd(true);
		
		pcvr.UIState = 0;
		
		//pcvr.numArr = new int[4];
		int randN = UnityEngine.Random.Range (0, 4);
		
		for(int i=0; i<4; i++)
		{
			pcvr.numArr[i] = (randN + i) % 4;
		}

		XkGameCtrl.IsLoadingLevel = true;
		Resources.UnloadUnusedAssets();

		//Application.LoadLevel(levelIndex);	//if single mode, can select level? - the first game level
		loadScenceHere (levelIndex);
	}
	
	public void linkModeBegin(int levelIndex)
	{//change hereeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee --------      fffffffff
		isEndDonghua = true;
		setLoadObjVisible (true, 4);
		
		doDonghuaEnd(true);

		movieObj.SetActive(false);
		
		pcvr.UIState = 0;
		
		XkGameCtrl.IsLoadingLevel = true;
		Resources.UnloadUnusedAssets();
		
		//Application.LoadLevel(levelIndex);	//into game - use the send-index
		loadScenceHere (levelIndex);
	}
	
	//keyboard  -  T  -  insert coin
	void ClickInsertcoinBtEvent(ButtonState val)
	{//Debug.Log ("pcvr.UIStatepcvr.UIStatepcvr.UIStatepcvr.UIState  " + pcvr.UIState);
		if ((pcvr.UIState != 1 && pcvr.UIState != 2) || Network.isServer)
		{
			return;
		}
		
		if (val == ButtonState.DOWN) {
			//coin
			if (!pcvr.bIsHardWare)
			pcvr.coinCurNumPCVR ++;
			changeCoinNum(true);
			startButtonShow (true);

			if (pcvr.sound2DScrObj)
			{
				pcvr.sound2DScrObj.playAudioInsertCoin();
			}
		}
	}
	
	//keyboard - K - start game - only for donghua
	void ClickStartBtOneEvent(ButtonState val)
	{//Debug.Log ("ClickStartBtOneEventClickStartBtOneEventClickStartBtOneEventClickStartBtOneEvent " + pcvr.UIState);
		if (pcvr.UIState != 1 || Network.isServer || isReadying)
		{
			return;
		}
		
		//here will press start button to gameGUI or linkGUI
		if (val == ButtonState.DOWN && (pcvr.networkPCVR == 1 && !pcvr.isChuangguan) && (pcvr.coinCurNumPCVR >= pcvr.startCoinNumPCVR || pcvr.canFree))
		{//link mode
			
			doDonghuaEnd(true);
			//Debug.Log("changegggggggggggggggggggggggggggggggggggggggggggggggggggggggggg2222");
			
			isEndDonghua = true;
			movieObj.SetActive(false);
			startButtonShow (false);

			if (!pcvr.canFree)
			{
				if (!pcvr.bIsHardWare)
					pcvr.coinCurNumPCVR -= pcvr.startCoinNumPCVR;
				else
					pcvr.GetInstance().SubPlayerCoin(pcvr.startCoinNumPCVR);
			}

			changeCoinNum(false);
			pcvr.UIState = 2;
			pcvr.StartBtLight = StartLightState.Mie;

			linkGuiObj.SetActive(true);
			
			//if (pcvr.sound2DScrObj)
			//{
			//	pcvr.sound2DScrObj.playAudioStart();
			//}
		}
		else if (val == ButtonState.DOWN && (pcvr.networkPCVR == 0 || pcvr.isChuangguan) && (pcvr.coinCurNumPCVR >= pcvr.startCoinNumPCVR || pcvr.canFree))
		{//single mode
			//if not link mode, will into game here
			doDonghuaEnd(true);
			pcvr.UIState = 2;
			//Debug.Log("changegggggggggggggggggggggggggggggggggggggggggggggggggggggggggg");
			startButtonShow (false);
			movieObj.SetActive(false);

			if (!pcvr.canFree)
			{
				if (!pcvr.bIsHardWare)
					pcvr.coinCurNumPCVR -= pcvr.startCoinNumPCVR;
				else
					pcvr.GetInstance().SubPlayerCoin(pcvr.startCoinNumPCVR);
			}
			changeCoinNum(true);

			singleModeBegin(1, 2);//change hereeeeeeeeeeeeeeeeeeeeee

			//if (pcvr.sound2DScrObj)
			//{
			//	pcvr.sound2DScrObj.playAudioStart();
			//}
		}
	}

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
	
	void ClickSetEnterBtEvent(ButtonState val)
	{
		if (val == ButtonState.DOWN) {
			return;
		}

		resetJiangliInfor();

		doDonghuaEnd (true);
		XkGameCtrl.IsLoadingLevel = true;
		Resources.UnloadUnusedAssets();
		//GC.Collect();
		Application.LoadLevel(1);	//into the game-set Panel
	}
	
	void detectPlayMovie()
	{
		setLoadObjVisible (false, 1025);

		if (pcvr.isPressStartButton)
		{
			pcvr.isPressStartButton = false;
			pcvr.isCanPressStartButtonEndGame = false;
			pcvr.selectLevelSIndex = 0;
			pcvr.selectTruckSIndex = 0;
			pcvr.isAfterJifen = false;
			coinPanel.SetActive (true);

			InputEventCtrl.GetInstance().ClickInsertcoinBtEvent += ClickInsertcoinBtEvent;
			InputEventCtrl.GetInstance().ClickStartBtOneEvent += ClickStartBtOneEvent;
			InputEventCtrl.GetInstance().ClickSetEnterBtEvent += ClickSetEnterBtEvent;
			InputEventCtrl.GetInstance().ClickSetMoveBtEvent += ClickSetMoveBtEvent;
			
			isReadying = false;

			ClickStartBtOneEvent(ButtonState.DOWN);
			
			//Invoke("judgeCreatServer", 1.5f);
			return;
		}

		pcvr.isPressStartButton = false;
		pcvr.isCanPressStartButtonEndGame = false;
		pcvr.selectLevelSIndex = 0;
		pcvr.selectTruckSIndex = 0;
		pcvr.isAfterJifen = false;
		
		//this movie will loop, if end, will play again
		if (movieTexture != null && !movieTexture.isPlaying)
		{Debug.Log("play movieeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee");
			movieObj.SetActive(true);

			if (!Network.isServer && GetComponent<AudioSource>())
			{
				audio.clip = movieTexture.audioClip;
			}

			movieTexture.loop = true;
			movieTexture.Play();
			//audio.Play ();
		}
		
		//Invoke("judgeCreatServer", 1.5f);
        judgeCreatServer();
    }
	
	void judgeCreatServer()
	{
		//pcvr.CloseFangXiangPanPower();
		if (getIsServer())
		{
			resetJiangliInfor();

			//create server
			InvokeRepeating("detectCreateServer", 0.5f, 3.1f);
			InputEventCtrl.GetInstance().ClickSetEnterBtEvent += ClickSetEnterBtEvent;
			InputEventCtrl.GetInstance().ClickSetMoveBtEvent += ClickSetMoveBtEvent;
		}
		else
		{
			coinPanel.SetActive (true);
			InputEventCtrl.GetInstance().ClickInsertcoinBtEvent += ClickInsertcoinBtEvent;
			InputEventCtrl.GetInstance().ClickStartBtOneEvent += ClickStartBtOneEvent;
			InputEventCtrl.GetInstance().ClickSetEnterBtEvent += ClickSetEnterBtEvent;
			InputEventCtrl.GetInstance().ClickSetMoveBtEvent += ClickSetMoveBtEvent;
		}
		
		isReadying = false;
        ClickStartBtOneEvent(ButtonState.DOWN); //跳过循环动画.
    }
	
	void detectCreateServer()
	{
		if (Network.peerType == NetworkPeerType.Disconnected)
		{
			//begin to create
			CreateServerKT();
		}
	}
	
	void CreateServerKT()
	{
		NetworkConnectionError error = Network.InitializeServer (pcvr.connections, pcvr.port, false);
		Debug.Log ("error create  " + error);
	}
	
	public bool getIsServer()
    {
        return false; //强制设定为客户端.
        //if (pcvr.networkPCVR == 1 && pcvr.ipString.CompareTo(Network.player.ipAddress) == 0)
		//{
		//	return true;
		//}		
		//return false;
	}
	
	//after server is created - server
	void OnServerInitialized()
	{
		coinPanel.SetActive (false);
		Network.incomingPassword = pcvr.passWord;
		CancelInvoke ("detectCreateServer");
		Debug.Log("Server initialized and ready");
	}
	
	//player connect here
	//the "Network.connections.Length" will add immediately
	void OnPlayerConnected(NetworkPlayer playerC)
	{
		int length = Network.connections.Length;
		//Debug.Log ("player connected " + length + " " +  playerC + " " + Network.isServer + " " + (length + int.Parse (playerC + "")));
		
		if(!serverCtrO)
		{
			serverCtrO = Network.Instantiate (ServerCtrlObjPrefab, ServerCtrlObjPrefab.transform.position, ServerCtrlObjPrefab.transform.rotation, 0) as GameObject;
		}
		
		if(!serverCtrO)
		{
			return;
		}
		
		pcvr.netPlayerObjArr[length - 1] = playerC;
		
		if (Network.isServer && length == 1 && serverCtrO && serverCtrO.networkView)
		{
			serverCtrO.networkView.RPC("SetFirstPlayerRPC", RPCMode.All, int.Parse(pcvr.netPlayerObjArr[0] + ""));
		}
		
		if (Network.isServer && serverCtrO && serverCtrO.networkView)
		{
			serverCtrO.networkView.RPC("SetPlayerNumberRPC", RPCMode.All, length, Network.incomingPassword);
		}
	}
	
	//the "Network.connections.Length" will not sub immediately
	void OnPlayerDisconnected(NetworkPlayer player)
	{
		int lengthT = Network.connections.Length;
		int findIndex = -1;
		
		for (int i = 0; i < lengthT; i++)
		{
			if (pcvr.netPlayerObjArr[i] == player)
			{
				findIndex = i;
				break;
			}
		}
		
		if (lengthT > 1 && findIndex + 1 < lengthT)
		{
			//move
			for (int j = findIndex + 1; j < lengthT; j++)
			{
				pcvr.netPlayerObjArr[j - 1] = pcvr.netPlayerObjArr[j];
			}
		}
		
		if (Network.isServer && lengthT > 1 && serverCtrO && serverCtrO.networkView)
		{
			serverCtrO.networkView.RPC("SetFirstPlayerRPC", RPCMode.All, int.Parse(pcvr.netPlayerObjArr[0] + ""));
		}
		else
		{
			Debug.Log("<<<<<<<<<<<<<<<<<<<<<<<str");
		}
		
		if (Network.isServer && lengthT > 1 && serverCtrO && serverCtrO.networkView)
		{
			serverCtrO.networkView.RPC("SetPlayerNumberRPC", RPCMode.All, lengthT - 1, Network.incomingPassword);
		}
	}

	public GameObject loadObj = null;
	//coin information
	public GameObject coinParentObject = null;
	public GameObject freeObject = null;
	public GameObject insertCoinObj = null;		//insert coin
	public GameObject startButtonObj = null;	//please press start button
	public UISprite coinNumSprite1 = null;
	public UISprite coinNumSprite2 = null;
	public UISprite coinNumSprite3 = null;
	public UISprite coinNumSprite4 = null;
	
	void changeCoinNum(bool flag)
	{
		coinNumSprite1.spriteName = Mathf.FloorToInt(pcvr.coinCurNumPCVR / 10).ToString();
		coinNumSprite2.spriteName = Mathf.FloorToInt(pcvr.coinCurNumPCVR % 10).ToString();
		
		if (flag && pcvr.UIState == 1)
		{
			if (pcvr.coinCurNumPCVR < pcvr.startCoinNumPCVR)
			{
				insertCoinObj.SetActive(true);
			}
			else
			{
				insertCoinObj.SetActive(false);
			}
		}
		else
		{
			insertCoinObj.SetActive(false);
		}
	}
	
	void startButtonShow(bool flag)
	{
		if (flag && pcvr.UIState == 1)
		{
			if (pcvr.coinCurNumPCVR < pcvr.startCoinNumPCVR)
			{
				startButtonObj.SetActive(false);
			}
			else
			{
				startButtonObj.SetActive(true);
			}
		}
		else
		{
			startButtonObj.SetActive (false);
		}
	}
	
	void coinFree()
	{Debug.Log ("pcvr.gameModePCVR  " +pcvr.gameModePCVR);
		if (pcvr.gameModePCVR == 1)
		{//operate mode
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
	
	void setLoadObjVisible(bool flag, int index)
	{
		if (loadObj && flag)
		{//show
			if (Network.isServer)
			{
				spriteObj.gameObject.SetActive(false);
			}
			else if (index < 1000)
			{
				spriteObj.gameObject.SetActive(true);
				isShowShijiao = true;
			}

			loadObj.transform.localPosition = new Vector3(0, 0, 664);
		}
		else if (loadObj && !flag)
		{//hide
			isShowShijiao = false;
			loadObj.transform.localPosition = new Vector3(0, 0, 0);
		}
	}
	
	void createSoundControlObj()
	{
		if (pcvr.xuanguanBeijingClicPlayObj)
		{
			DestroyObject(pcvr.xuanguanBeijingClicPlayObj);
			pcvr.xuanguanBeijingClicPlayObj = null;
		}

		if (pcvr.xuanguanBeijingClicPlayObj)
		{
			Destroy(pcvr.xuanguanBeijingClicPlayObj);
		}

		if (soundControlPrefab)
		{
			pcvr.xuanguanBeijingClicPlayObj = Instantiate(soundControlPrefab, Vector3.zero, Quaternion.identity) as GameObject;

			DontDestroyOnLoad(pcvr.xuanguanBeijingClicPlayObj);
		}

		//END
		
		if (pcvr.soundENDPlayObj)
		{
			DestroyObject(pcvr.soundENDPlayObj);
			pcvr.soundENDPlayObj = null;
		}
		
		if (pcvr.soundENDPlayObj)
		{
			Destroy(pcvr.soundENDPlayObj);
		}
		
		if (soundControlENDPrefab)
		{
			pcvr.soundENDPlayObj = Instantiate(soundControlENDPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			
			DontDestroyOnLoad(pcvr.soundENDPlayObj);
		}
	}

	void controlDeng()
	{
		if (shanState >= 0)
		{
			if (jiangeCurShortTime > 0 - liangStaticTime)
			{
				jiangeCurShortTime -= Time.deltaTime;

				if (shanState < 27)
				{
					liangTime -= Time.deltaTime;

					if (liangTime <= 0)
					{
						isMieT = true;
					}
				}
			}
			else
			{
				liangTime = liangStaticTime;
				isMieT = false;

				if (xunhuanState < 0 || xunhuanState >= 15)
				shanState ++;

				if (shanState < 5)
				{
					jiangeCurShortTime = 0.1f;
				}
				else if (shanState < 9)
				{
					jiangeCurShortTime = 0.1f;

				}
				else if (shanState < 11)
				{
					if (shanState < 10)
						jiangeCurShortTime = 0.1f;
					else
						jiangeCurShortTime = 0.4f;
				}
				else if (shanState < 15)
				{
					jiangeCurShortTime = 0.2f;
				}
				else if (shanState < 18)
				{
					jiangeCurShortTime = 0.3f;
				}
				else if (shanState < 22)
				{
					jiangeCurShortTime = 0.1f;
				}
				else if (shanState == 22)
				{
					jiangeCurShortTime = 0.1f;
					
					xunhuanCishu ++;
					
					if (xunhuanCishu < 5)
					{
						shanState = 18;
					}
					else
					{
						jiangeCurShortTime = 0.1f;

						if (xunhuanState < 15)
							xunhuanState ++;

						if (xunhuanState == 0)
						{
							dengTimesT = 0;
							CancelInvoke("dengSpecial");
							InvokeRepeating("dengSpecial", 0, 0.05f);
						}
					}
				}
				else if (shanState < 27)
				{
					CancelInvoke("dengSpecial");
					jiangeCurShortTime = 0.1f;
				}
				else if (shanState == 27)
				{
					jiangeCurShortTime = 3.0f;
				}
				else
				{
					jiangeCurTime = 10.0f;	//every part jiange shijian
					shanState = -1;
					xunhuanState = -1;
				}
			}
		}
		else 
		{
			if (jiangeCurTime <= 0)
			{
				if (shanState < 0)
				{
					shanState = 1;
					jiangeCurShortTime = 0.2f;
					liangTime = liangStaticTime;
					isMieT = false;
					xunhuanCishu = 0;
				}
			}
			else if (jiangeCurTime > 0)
			{
				jiangeCurTime -= Time.deltaTime;
			}
		}

		if (isMieT)
		{
			pcvr.GetInstance().ControlShanguangdengDH(0, false, xunhuanState);
		}
		else
		{
			pcvr.GetInstance().ControlShanguangdengDH(shanState, false, xunhuanState);
		}
	}

	void doDonghuaEnd(bool isEndT)
	{
		isEndDonghua = isEndT;
        if (movieTexture != null)
        {
            movieTexture.Stop();
        }
		audio.Stop ();

		if (zuoyiObj && isEndT)
		{
			zuoyiObj.SetActive(false);
		}

		//pcvr.GetInstance().ControlShanguangdengDH(0, false, -1);
		pcvr.GetInstance().ControlShanguangdeng (1001, false);
	}

	int dengTimesT = 0;
	void dengSpecial()
	{
		if (shanState != 22 || dengTimesT >= 200)
		{
			CancelInvoke("dengSpecial");
			return;
		}

		if (dengTimesT % 3 == 0)
		{
			pcvr.GetInstance().ControlShanguangdengDHSpecial(1);
		}
		else
		{
			pcvr.GetInstance().ControlShanguangdengDHSpecial(0);
		}

		dengTimesT ++;
	}
	AsyncOperation async;
	string loadName = "";
	int loadIndex = 0;
	bool isShowShijiao = false;
	
	public float jiangeTime = 0.5f;
	private float totalTime = 0;
	private int stateShijiao = 0;
	
	public UISprite spriteObj = null;

	void loadScenceHere(int index)
	{
		loadIndex = index;
		StartCoroutine(loadScene());
	}

	IEnumerator loadScene()
	{
		async = Application.LoadLevelAsync(loadIndex);
		yield return async;
	}

	void shijiaoUpdate ()
	{
		totalTime += Time.deltaTime;
		
		if (totalTime >= jiangeTime)
		{
			totalTime = 0;
			
			if (stateShijiao == 0)
			{
				stateShijiao = 1;
				spriteObj.spriteName = "shijiao1";
			}
			else if (stateShijiao == 1)
			{
				stateShijiao = 0;
				spriteObj.spriteName = "shijiao2";
			}
		}
	}

	//change atlas for english version
	void changeENAtlas()
	{
		if (pcvr.UIENAtlasObjSpeciaPCVR.Length >= 3)
		{
			for (int i=0; i < spriteENObjSpecia.Length - 4; i++)
			{
				if (spriteENObjSpecia[i] && pcvr.UIENAtlasObjSpeciaPCVR[i])
				{
					spriteENObjSpecia[i].atlas = pcvr.UIENAtlasObjSpeciaPCVR[i];
				}
			}

			for (int i=3; i < spriteENObjSpecia.Length; i++)
			{
				if (spriteENObjSpecia[i] && pcvr.UIENAtlasObjSpeciaPCVR[1])
				{
					spriteENObjSpecia[i].atlas = pcvr.UIENAtlasObjSpeciaPCVR[1];
				}
			}
		}

		if (pcvr.UIENAtlasObjSpeciaPCVR.Length >= 4 && pcvr.UIENAtlasObjSpeciaPCVR[3])
		{
			for (int i=0; i < spriteENObjSelect.Length; i++)
			{
				if (spriteENObjSelect[i])
				{
					spriteENObjSelect[i].atlas = pcvr.UIENAtlasObjSpeciaPCVR[3];
				}
			}
		}

		if (pcvr.UIEN1AtlasObjPCVR)
		{
			for (int i=0; i < spriteENObj.Length; i++)
			{
				if (spriteENObj[i])
				{
					spriteENObj[i].atlas = pcvr.UIEN1AtlasObjPCVR;
				}
			}
		}
	}
	
	public void resetJiangliInfor()
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

	//the old version
	void Update11111()
	{
		if (isShowShijiao)
		{
			shijiaoUpdate ();
		}
		
		if (movieTexture != null && donghuaCharuObj && !isEndDonghua)
		{//Debug.Log (movieTexture.duration + " " + audio.clip.length + " " + audio.time + " " +(movieTexture.duration - audio.time));
			//controlDeng();
			
			if (isStopWait && !movieTexture.isPlaying)
			{
				waitTime += Time.deltaTime;
				
				if (waitTime > 1.0f)
				{
					isStopWait = false;
					movieTexture.Play();
					audio.Play ();
				}
				return;
			}
			
			if (movieTexture.duration - audio.time <= 0.2f && movieTexture.isPlaying)
			{
				doDonghuaEnd(false);
				
				isStopWait = true;
				waitTime = 0.0f;
				return;
			}
			
			if (audio.time >= 72.50f)
			{
				donghuaCharuObj.localPosition = new Vector3(3000, 0, 658);
			}
			else if (audio.time >= 68.2f)
			{
				donghuaCharuObj.localPosition = new Vector3(0, 0, 658);
			}
			else
			{
				donghuaCharuObj.localPosition = new Vector3(3000, 0, 658);
			}
		}
		
		if (!Network.isServer && pcvr.UIState == 1 && pcvr.coinCurNumPCVR >= pcvr.startCoinNumPCVR && pcvr.StartBtLight != StartLightState.Shan)
		{
			pcvr.StartBtLight = StartLightState.Shan;
		}
		
		if (hasDetect)
		{
			return;
		}
		
		/*if (Time.time - detectTime > 10.0f)
		{
			pcvr.CloseFangXiangPanPower();
			hasDetect = true;
		}
		else if (Time.time - detectTime <= 10.0f && Time.frameCount % 30 == 0)
		{
			pcvr.OpenFangXiangPanPower ();
		}*/
	}
}
