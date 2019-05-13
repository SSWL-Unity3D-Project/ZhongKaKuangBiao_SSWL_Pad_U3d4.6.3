using UnityEngine;
using System.Collections;
using System;

public class aFirstHere : MonoBehaviour {
	//this judge the game what show first
	//hide the game gui, the link gui,
	//if into game, this script will be hidden
	public bool isPaoLiangquan = false;
	public int paoJiquan = 1;
	
	public GameObject soundControlPrefab;
	public GameObject soundControlENDPrefab;

	public Transform cameraTransform = null;
	public UITruck truckUI = null;		//script obj
	public smallMapUI mapUI = null;

	public static GameObject serverCtrO = null;
	public GameObject ServerCtrlObjPrefab = null;
	
	public GameObject gameGuiObj = null;	//the parent
	public GameObject uipanel = null;		//child
	public GameObject coinPanel = null;		//child
	public GameObject smallMapGuiObj = null;//map

	public Transform[] playerPoint;
	public Transform[] cameraPoint;
	public GameObject[] playerPrefabs;
	public GameObject[] AIPrefabs;
	public Transform[] playerPathObj;

	//daoju
	public GameObject naozhongParentObj = null;
	public GameObject danqiParentObj = null;
	public GameObject danqiParentObj222 = null;
	public GameObject danqiPrefabs = null;
	private int danqiNum = 0;

	public Transform pointObjParent = null;	//ding dian parent	-	point object
	public Transform triggerObjParent = null;	//ding dian parent - trigger object
	public FastBloom FastBloomT = null;
	
	void createSoundControlObj()
	{Debug.Log ("only for teste   createSoundControlObj " + Application.levelCount + " " + Application.loadedLevel + " "+ Application.loadedLevelName);
		if (pcvr.xuanguanBeijingClicPlayObj)
		{
			Destroy(pcvr.xuanguanBeijingClicPlayObj);
		}

		if (soundControlPrefab)
		{
			pcvr.xuanguanBeijingClicPlayObj = Instantiate(soundControlPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		}

		//END
		if (pcvr.soundENDPlayObj)
		{
			Destroy(pcvr.soundENDPlayObj);
		}
		
		if (soundControlENDPrefab)
		{
			pcvr.soundENDPlayObj = Instantiate(soundControlENDPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		}
	}

	void openLi()
	{
		if (!Network.isServer)
		{
			pcvr.GetInstance().OpenFangXiangPanPower();
		}
	}

	void Awake()
	{
		truckUI.setLoadObjVisible (true, 33);
	}

	// Use this for initialization
	void Start () {//Debug.Log ("aFirstHere   Starttttttt");
		if (pcvr.bIsHardWare)
			Screen.showCursor = false;
		Time.timeScale = 1;
		//pcvr.isDanqitishi = false;

		if (isPaoLiangquan && paoJiquan < 2)
		{
			paoJiquan = 2;
			Debug.Log("paoJiquanpaoJiquanpaoJiquanpaoJiquanpaoJiquan =   " + paoJiquan);
		}

		if (FastBloomT)
		{
			FastBloomT.threshhold = FastBloomT.maxThreshhold;
			FastBloomT.changeState = -1;
			FastBloomT.enabled = true;
		}

		if (!Network.isServer)
		{
			pcvr.GetInstance().OpenFangXiangPanPower();
		}

		Invoke("openLi", 0.8f);

		if (Network.isServer && pointObjParent)
		{
			int numN = pointObjParent.childCount;

			for (int i = 0; i < numN; i++)
			{
				pointObjParent.GetChild(i).renderer.enabled = false;
			}
		}

		if (!Network.isServer && !Network.isClient && !pcvr.xuanguanBeijingClicPlayObj)
		{
			createSoundControlObj ();
		}

		truckUI.setLoadObjVisible (true, 3);

		pcvr.GetInstance();
		pcvr.GetInstance().getGamesetInfor();
		pcvr.aFirstScriObj = GetComponent<aFirstHere>();
		pcvr.UITruckScrObj = truckUI;
		pcvr.smallMapUIScrObj = mapUI;
		ReadGameInfo.GetInstance ().ReadGameNetwork();

		//change here ------------------------------ fffffffffffffffffff
		pcvr.mingciFirstPlayer = 0;
		pcvr.total321Time = 0;
		pcvr.totalTime = 0;
		pcvr.totalTimeServer = 0;
		pcvr.totalTimeAddDaoju = 0;
		pcvr.isPassgamelevel = false;
		pcvr.finishedNumber = 0;
		pcvr.finishedPlayerNumber = 0;
		
		pcvr.GetInstance().initInforAgain(false);

		truckUI.coinFree();

		//first hide all the gui
		gameGuiObj.SetActive (false);
		uipanel.SetActive (false);
		coinPanel.SetActive (false);
		pcvr.smallMapObj = smallMapGuiObj;
		smallMapGuiObj.SetActive (false);

		if (Network.isServer)
		{
			naozhongParentObj.SetActive (false);
			spawnParent.gameObject.SetActive(false);
			pcvr.XKCarCameraSObj.enabled = false;
			pcvr.serCameraSObj.enabled = true;
			coinPanel.SetActive (false);
			truckUI.hideAll ();
		}
		else if (Network.isClient)
		{
			pcvr.XKCarCameraSObj.enabled = true;
			pcvr.serCameraSObj.enabled = false;
			coinPanel.SetActive (true);

			naozhongParentObj.SetActive (false);
			spawnParent.gameObject.SetActive(false);
			danqiParentObj.SetActive (false);

			if (danqiParentObj222)
			danqiParentObj222.SetActive (false);
		}
		else
		{
			pcvr.XKCarCameraSObj.enabled = true;
			pcvr.serCameraSObj.enabled = false;

			coinPanel.SetActive (true);

			naozhongParentObj.SetActive (true);
			spawnParent.gameObject.SetActive(true);
			
			if (danqiParentObj222)
				danqiParentObj222.SetActive (false);
		}
		
		createDanqi(true);
		
		//*********************************** begin
		//spawn or delete npc
		spawnNum = spawnParent.childCount;
		spawnArr = new Transform[spawnNum];
		
		for (int i=0; i<spawnNum; i++)
		{
			spawnArr[i] = spawnParent.GetChild(i);
		}
		//*********************************** end

		//show the GUI and play movie
		gameGuiObj.SetActive (true);

		if (Network.isServer)
		{
			linkModeBeginPlay(-1);
		}
		else if (Network.isClient)
		{Debug.Log(pcvr.selfIndex);
			//linkModeBeginPlay(pcvr.numArr[pcvr.selfIndex]);	//change
			linkModeBeginPlay(pcvr.selectTruckSIndex);
		}
		else
		{
			singleModeBeginPlay();
		}

		pcvr.UIState = 3;
		
		int volumNum = Convert.ToInt32(ReadGameInfo.GetInstance ().ReadGameVolumNum ());
		Debug.Log ("volumNumttblll ==   " + volumNum);
		AudioListener.volume = volumNum * 0.1f;
		
		if (pcvr.languagePCVR == 0)
		{
			changeENAtlas();
		}

		XkGameCtrl.IsLoadingLevel = false;
	}

	void singleModeBeginPlay()
	{
		//will spawn player and into game
		uipanel.SetActive(true);

		//int randN = Random.Range (0, 4);
		
		for(int i=0; i<4; i++)
		{
			pcvr.numArr[i] = (pcvr.selectTruckSIndex + i) % 4;
		}
		
		//create four players(AI)
		createPlayer ();
	}
	
	void linkModeBeginPlay(int prefabNumIndexT)
	{
		//will spawn player and into game
		if (Network.isClient)
		{
			uipanel.SetActive(true);
		}
		
		//create four players(AI)
		createPlayerLinkMode (prefabNumIndexT);//should spawn server - model - player - AI
	}
	
	//the client or server disconnected from server
	void OnDisconnectedFromServer(NetworkDisconnection info)
	{
		if (Network.isServer)
		{//if is server, will return to donghua
			Debug.Log("Local server connection disconnected " + Application.loadedLevel);
		}
	}

	//only for single mode
	void createPlayer()
	{//judge how many players into game
		for (int i=0; i<4; i++)
		{
			GameObject prefab = null;

			if (i == 0)
			{
				prefab = playerPrefabs[pcvr.numArr[i]];
			}
			else
			{
				prefab = AIPrefabs[pcvr.numArr[i]];
			}

			GameObject playerObj = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
			playerObj.transform.position = playerPoint[i].position;
			playerObj.transform.rotation = playerPoint[i].rotation;
			//playerObj.transform.localScale = new Vector3(1, 1, 1);
			
			Destroy (playerObj.GetComponent<NetworkView>());
			Destroy (playerObj.GetComponent<gameServerObjSript>());
			
			if (i == 0)
			{//the Player
				playerObj.GetComponent<truck>().SetAfterSpawn(false, null, -1, playerPoint[i], i);
				pcvr.truckObjScriptArr[i] = playerObj.GetComponent<truck>();
				pcvr.truckObjTransArr[i] = playerObj.transform;
				pcvr.indexFour[i] = 0;
			}
			else
			{//the AI
				playerObj.GetComponent<truck>().SetAfterSpawn(true, playerPathObj[i - 1], i - 1, playerPoint[i], i);
				pcvr.truckObjScriptArr[i] = playerObj.GetComponent<truck>();
				pcvr.truckObjTransArr[i] = playerObj.transform;
				pcvr.indexFour[i] = i + 4;

				//only for test.........
				//playerObj.SetActive(false);
			}
			
			truckUI.setAIPlayerHere(playerObj.GetComponent<truck>());
			mapUI.SetAIPlayerHere(playerObj.transform, i);

			//pcvr.chanshengdeObj[pcvr.chanshengdeObjNum] = playerObj;
			//pcvr.chanshengdeObjNum ++;
		}

		cameraTransform.position = cameraPoint [0].position;
		cameraTransform.rotation = cameraPoint [0].rotation;
		cameraTransform.localScale = new Vector3(1, 1, 1);
	}
	private GameObject[] danqiArr;
	public void createDanqi(bool isServer)
	{//Debug.LogError("createeeeeeeeeeeeeeeeeeeeeeeeeeeeeee  danqi");
		danqiParentObj.SetActive (true);
		danqiNum = danqiParentObj.transform.childCount;
		danqiArr = new GameObject[danqiNum];

		//delete the networkview here
		for (int i=0; i < danqiNum; i++)
		{
			GameObject danqiObj = Instantiate(danqiPrefabs, danqiParentObj.transform.GetChild(i).position, danqiParentObj.transform.GetChild(i).rotation) as GameObject;
			danqiObj.transform.localScale = new Vector3(danqiParentObj.transform.GetChild(i).localScale.x, danqiParentObj.transform.GetChild(i).localScale.y, danqiParentObj.transform.GetChild(i).localScale.z);
			//danqiObj.transform.parent = danqiParentObj.transform.GetChild(i);
			Destroy (danqiObj.GetComponent<NetworkView>());
			danqiArr[i] = danqiObj;
		}

		danqiParentObj.SetActive (false);
	}

	public void createDanqi222(bool isServer)
	{//Debug.LogError("createeeeeeeeeeeeeeeeeeeeeeeeeeeeeee222  danqi");
		if (!danqiParentObj222)
		{
			createDanqi(isServer);
			return;
		}

		danqiParentObj222.SetActive (true);
		danqiNum = danqiParentObj222.transform.childCount;
		danqiArr = new GameObject[danqiNum];
		
		//delete the networkview here
		for (int i=0; i < danqiNum; i++)
		{
			GameObject danqiObj = Instantiate(danqiPrefabs, danqiParentObj222.transform.GetChild(i).position, danqiParentObj222.transform.GetChild(i).rotation) as GameObject;
			danqiObj.transform.localScale = new Vector3(danqiParentObj222.transform.GetChild(i).localScale.x, danqiParentObj222.transform.GetChild(i).localScale.y, danqiParentObj222.transform.GetChild(i).localScale.z);
			//danqiObj.transform.parent = danqiParentObj222.transform.GetChild(i);
			Destroy (danqiObj.GetComponent<NetworkView>());
			danqiArr[i] = danqiObj;
		}
		
		danqiParentObj222.SetActive (false);
	}


	/*void createDanqi1(bool isServer)
	{danqiParentObj.SetActive (true);
		danqiNum = danqiParentObj.transform.childCount;

		if (!isServer)
		{
			//delete the networkview here
			for (int i=0; i < danqiNum; i++)
			{
				GameObject danqiObj = Instantiate(danqiPrefabs, danqiParentObj.transform.GetChild(i).position, danqiParentObj.transform.GetChild(i).rotation) as GameObject;
				//danqiObj.transform.parent = danqiParentObj.transform.GetChild(i);
				Destroy (danqiObj.GetComponent<NetworkView>());
			}
		}
		else
		{
			for (int i=0; i < danqiNum; i++)
			{
				GameObject danqiObj = Network.Instantiate(danqiPrefabs, danqiParentObj.transform.GetChild(i).position, danqiParentObj.transform.GetChild(i).rotation, 0) as GameObject;
				//danqiObj.transform.parent = danqiParentObj.transform.GetChild(i);
			}
		}danqiParentObj.SetActive (false);
	}*/
	
	void createPlayerLinkMode(int prefabNumIndexT)
	{//judge how many players into game and is server or client
		int lengthT = Network.connections.Length;

		//should network.instantiate
		if (Network.isServer)
		{//for server - only the AI
			for (int j = lengthT; j < 4; j++)
			{
				GameObject playerObj = Network.Instantiate(AIPrefabs[pcvr.numArr[j]], Vector3.zero, Quaternion.identity, 0) as GameObject;
				playerObj.transform.position = playerPoint[j].position;
				playerObj.transform.rotation = playerPoint[j].rotation;
				//playerObj.transform.localScale = new Vector3(1, 1, 1);

				playerObj.GetComponent<truck>().SetAfterSpawn(true, playerPathObj[j - 1], j - 1, playerPoint[j], j);
				
				if (j == 1)
				{
					AIObject1 = playerObj;
					AIPlayerNet1 = Network.player;
					//AIDetect1();
					Invoke("AIDetect1", 0.2f);
				}
				else if (j == 2)
				{
					AIObject2 = playerObj;
					AIPlayerNet3 = Network.player;
					//AIDetect2();
					Invoke("AIDetect2", 0.2f);
				}
				else if (j == 3)
				{
					AIObject3 = playerObj;
					AIPlayerNet2 = Network.player;
					//AIDetect3();
					Invoke("AIDetect3", 0.2f);
				}

				pcvr.indexFour[j] = j + 4;
				
				//record the objects here
				//pcvr.chanshengdeObjNet[pcvr.chanshengdeObjNetNum] = playerObject;
				//pcvr.chanshengdeObjNetNum ++;
			}
		}
		else if (Network.isClient)
		{//for client - the player
			Debug.Log("xuancheeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeziffff  " + prefabNumIndexT);
			playerObject = Network.Instantiate(playerPrefabs[prefabNumIndexT], Vector3.zero, Quaternion.identity, 0) as GameObject;
			playerObject.transform.position = playerPoint[pcvr.selfIndex].position;
			playerObject.transform.rotation = playerPoint[pcvr.selfIndex].rotation;
			//playerObject.transform.localScale = new Vector3(1, 1, 1);
			playerNet = Network.player;

			playerObject.GetComponent<truck>().SetAfterSpawn(false, null, -1, playerPoint[pcvr.selfIndex], pcvr.selfIndex);
			//pcvr.truckObjScriptArr[pcvr.selfIndex] = playerObject.GetComponent<truck>();
			pcvr.indexFour[pcvr.selfIndex] = pcvr.selfIndex;
			//playerDetect();
			Invoke("playerDetect", 0.2f);
			Debug.Log("chanshengle player  trans  " + playerObject.transform.position + " "+ pcvr.selfIndex);

			//record the objects here
			//pcvr.chanshengdeObjNet[pcvr.chanshengdeObjNetNum] = playerObject;
			//pcvr.chanshengdeObjNetNum ++;
		}
	}

	private GameObject playerObject = null;
	private NetworkPlayer playerNet;

	void playerDetect()
	{
		if (Network.isClient && playerObject && playerObject.networkView)
		{
			playerObject.networkView.RPC("RecordPlayerAIRPC", RPCMode.All, playerNet, pcvr.selfIndex, -1, playerObject.transform.position, new Vector3(playerObject.transform.eulerAngles.x,playerObject.transform.eulerAngles.y,playerObject.transform.eulerAngles.z));
		}
		else
		{
			Invoke("playerDetect", 0.2f);
			Debug.Log("<<<<<<<<<<<<<<<<<<PLAYERDe");
		}
	}

	private GameObject AIObject1 = null;
	private GameObject AIObject2 = null;
	private GameObject AIObject3 = null;
	private NetworkPlayer AIPlayerNet1;
	private NetworkPlayer AIPlayerNet2;
	private NetworkPlayer AIPlayerNet3;

	void AIDetect1()
	{
		if (Network.isServer && AIObject1 && AIObject1.networkView)
		{
			AIObject1.networkView.RPC("RecordPlayerAIRPC", RPCMode.All, AIPlayerNet1, -1, 1, AIObject1.transform.position, new Vector3(AIObject1.transform.eulerAngles.x,AIObject1.transform.eulerAngles.y,AIObject1.transform.eulerAngles.z));
		}
		else
		{
			Invoke("AIDetect1", 0.2f);
			Debug.Log("<<<<<<<<<<<<<<<<<<npcs1De");
		}
	}

	void AIDetect2()
	{
		if (Network.isServer && AIObject2 && AIObject2.networkView)
		{
			AIObject2.networkView.RPC("RecordPlayerAIRPC", RPCMode.All, AIPlayerNet2, -1, 2, AIObject2.transform.position, new Vector3(AIObject2.transform.eulerAngles.x,AIObject2.transform.eulerAngles.y,AIObject2.transform.eulerAngles.z));
		}
		else
		{
			Invoke("AIDetect2", 0.2f);
			Debug.Log("<<<<<<<<<<<<<<<<<<npcs2De");
		}
	}

	void AIDetect3()
	{
		if (Network.isServer && AIObject3 && AIObject3.networkView)
		{
			AIObject3.networkView.RPC("RecordPlayerAIRPC", RPCMode.All, AIPlayerNet3, -1, 3, AIObject3.transform.position, new Vector3(AIObject3.transform.eulerAngles.x,AIObject3.transform.eulerAngles.y,AIObject3.transform.eulerAngles.z));
		}
		else
		{
			Invoke("AIDetect3", 0.2f);
			Debug.Log("<<<<<<<<<<<<<<<<<<npcs3De");
		}
	}

	//spawn or delete NPC(jiang you)
	public Transform spawnParent;
	private int spawnNum = 0;
	private Transform[] spawnArr;
	private Transform spawnTemp = null;
	private Transform delTemp = null;
	
	public AudioSource audioPengzhuang = null;
	public GameObject liziXiaoguo = null;

	public void spawnNPCle(string name)
	{
		spawnTemp = null;
		
		for (int i=0; i < spawnNum; i++)
		{
			if (spawnArr[i] && spawnArr[i].name.CompareTo(name) == 0 )
			{
				spawnTemp = spawnArr[i];
				break;
			}
		}
		
		if (spawnTemp)
		{
			spawnTemp.collider.enabled = false;
			spawnTemp.GetComponent<spawnTriggerScript>().BeginSpawn();
			Destroy(spawnTemp.gameObject);
		}
	}
	
	public void delNPCle(string name)
	{
		delTemp = null;
		
		for (int i=0; i < spawnNum; i++)
		{
			if (spawnArr[i] && spawnArr[i].name.CompareTo(name) == 0 )
			{
				delTemp = spawnArr[i];
				break;
			}
		}
		
		if (delTemp)
		{
			if(delTemp.GetComponent<deleteTriggerScript>().BeginDelete())
			{
				delTemp.collider.enabled = false;
				delTemp.GetComponent<deleteTriggerScript>().BeginDelete();
				Destroy(delTemp.gameObject);
			}
		}
	}

	public void resetSomeTriInfor(bool isClient)
	{
		int number = 0;

		if (!isClient)
		{
			number = spawnParent.childCount;
			
			for (int i=0; i<number; i++)
			{
				spawnParent.GetChild(i).collider.enabled = true;
			}
		}
		else if (isClient)
		{
			number = triggerObjParent.childCount;
			
			for (int i=0; i<number; i++)
			{
				triggerObjParent.GetChild(i).collider.enabled = true;
			}
		}
	}

	public void deleteDanqi()
	{
		for (int i=0; i < danqiArr.Length; i++)
		{
			Destroy (danqiArr[i]);
		}
	}

	public void openFastBloom()
	{
		if (FastBloomT)
		{
			FastBloomT.changeState = 0;
		}
	}
	
	public UISprite[] spriteENObj;//other

	//change atlas for english version
	void changeENAtlas()
	{
		if (pcvr.UIENAtlasObjSpeciaPCVR.Length >= 4 && pcvr.UIENAtlasObjSpeciaPCVR[0] && spriteENObj[0])
		{
			//the first one(0) is the load sprite
			spriteENObj[0].atlas = pcvr.UIENAtlasObjSpeciaPCVR[0];
		}
		
		if (pcvr.UIEN1AtlasObjPCVR)
		{
			for (int i=1; i < spriteENObj.Length; i++)
			{
				if (spriteENObj[i])
				{
					spriteENObj[i].atlas = pcvr.UIEN1AtlasObjPCVR;
				}
			}
		}
	}

	public void addXiaoguo()
	{
		gameObject.AddComponent("GrayscaleEffect");
		gameObject.AddComponent("Blur");

		if (pcvr.nowShaderPcvr1)
		{
			gameObject.GetComponent<GrayscaleEffect> ().shader = pcvr.nowShaderPcvr1;
		}

		if (pcvr.nowShaderPcvr2)
		{
			gameObject.GetComponent<Blur> ().blurShader = pcvr.nowShaderPcvr2;
		}
	}

	public void OverXiaoguo()
	{
		if (gameObject.GetComponent<GrayscaleEffect> ())
		{
			gameObject.GetComponent<GrayscaleEffect> ().rampOffset = -1;
		}
	}
}
