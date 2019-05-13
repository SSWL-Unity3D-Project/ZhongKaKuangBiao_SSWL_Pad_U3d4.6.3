using UnityEngine;
using System.Collections;
using System;

public class gameServerObjSript : MonoBehaviour {
	//private GameObject serverCameraObj = null;

	//this script is only for the playerself
	public Transform[] PointArr;			//the six point on the player
	public Transform biaoji;
	private int pointNum = -1;
	
	private Vector3 vector3Value1 = Vector3.zero;
	private Vector3 vector3Value2 = Vector3.zero;
	private NetworkPlayer playerN;
	private bool hadInit = false;

	public bool shamoGuanka = false;
	public Transform[] Wheels;
	private int wheelNum = 0;
	public Transform[] WheelsC;
	
	//smoke
	public ParticleSystem[] yan;
	private int yanNumber = 0;
	
	//dust
	public ParticleSystem[] chenTu;
	private int chenTuNumber = 0;
	
	//water
	public ParticleSystem[] waterTruck;
	private int waterTruckuNumber = 0;
	
	//smoke
	public ParticleSystem[] yanXieqi;
	private int yanNumberXieqi = 0;

	//penhuo
	public ParticleSystem penhuoLizi;

	//pu tong penhuo
	public Transform penhuoliziPutongObj = null;

	string currentLayerCache = "";
	bool meichuli = false;
	bool jieshule = false;

	float pengleStaticTime = 0.35f;
	float pengleCurTime = 0.0f;

	[RPC]
	public void changeGNORELayerRPC()
	{
		ChangeLayersRecursively(gameObject.transform, "IGNORE");
		Invoke ("ChangeLayersAgain", 3);
	}

	void ChangeLayersAgain()
	{
		ChangeLayersRecursively(transform.gameObject.transform, currentLayerCache);
	}

	void ChangeLayersRecursively(Transform trans, String LayerName)
	{
		trans.gameObject.layer = LayerMask.NameToLayer(LayerName);
		
		for (int i = 0; i < trans.childCount; i++)
		{
			trans.GetChild(i).gameObject.layer = LayerMask.NameToLayer(LayerName);
			ChangeLayersRecursively(trans.GetChild(i), LayerName);
		}
	}

	void Start()
	{
		pointNum = PointArr.Length;
		meichuli = false;
		jieshule = false;
		pengleCurTime = 0.0f;
		currentLayerCache = LayerMask.LayerToName(transform.gameObject.layer);
		ChangeLayersRecursively(gameObject.transform, "IGNORE");

		wheelNum = Wheels.Length;

		if (wheelNum > 0)
		{
			WheelsC = new Transform[wheelNum];

			for (int i=0; i < wheelNum; i++)
			{
				WheelsC[i] = Wheels[i].GetChild(0);
			}
		}

		yanNumber = 0;
		for (int i=0; i < yan.Length; i++)
		{
			if (yan[i])
			{
				yanNumber ++;
			}
			else
			{
				break;
			}
		}
		
		chenTuNumber = 0;
		for (int i=0; i < chenTu.Length; i++)
		{
			if (chenTu[i])
			{
				chenTuNumber ++;
			}
			else
			{
				break;
			}
		}
		
		waterTruckuNumber = 0;
		for (int i=0; i < waterTruck.Length; i++)
		{
			if (waterTruck[i])
			{
				waterTruckuNumber ++;
			}
			else
			{
				break;
			}
		}

		yanNumberXieqi = 0;
		for (int i=0; i < yanXieqi.Length; i++)
		{
			if (yanXieqi[i])
			{
				yanNumberXieqi ++;
			}
			else
			{
				break;
			}
		}

		for (int i=0; i < yanNumber; i++)
		{
			yan[i].Stop();
		}

		for (int i=0; i < chenTuNumber; i++)
		{
			chenTu[i].Stop();
		}
		
		for (int i=0; i < waterTruckuNumber; i++)
		{
			waterTruck[i].Stop();
		}
		
		for (int i=0; i < yanNumberXieqi; i++)
		{
			yanXieqi[i].Stop();
		}

		if (penhuoLizi)
		{
			penhuoLizi.Stop();
		}
	}
	private float smooth = 8.0f;
	void FixedUpdate()
		//void Update ()
	{
		//Debug.Log("fffffffNetwork.isServeraaa  " + Network.isServer + hadInit +  " "  + transform + " " + transform.position + " "+ vector3Value1 + " " +pcvr.uiRunState + " "+ Network.player + " " + playerN);
		if (pcvr.uiRunState >= 2 && !meichuli)
		{
			Invoke ("ChangeLayersAgain", 0.5f);
			meichuli = true;
		}

		if (Network.player == playerN || !hadInit)
		{
			return;
		}

		if (pcvr.uiRunState >= 2)
		{
			/*if (!rigidbody.useGravity)
			{
				rigidbody.useGravity = true;
			}*/

			if (rigidbody.isKinematic && !jieshule)
			{
				rigidbody.isKinematic = false;
			}

			//transform.position = vector3Value1;

			/*if (pengleCurTime > 0)
			{
				pengleCurTime -= Time.deltaTime;
			}
			else
			{
				transform.position = Vector3.Lerp(transform.position, vector3Value1, smooth);
				transform.position = new Vector3(transform.position.x - 0.5f, vector3Value1.y, transform.position.z);
			}*/
			
			//transform.position = vector3Value1;
			if (dealValueCur > dealValue)
			{
				dealValueCur -= dealValueChange;

				if (dealValueCur < dealValue)
				{
					dealValueCur = dealValue;
				}
			}
			else if (dealValueCur < dealValue)
			{
				dealValueCur += dealValueChange;
				
				if (dealValueCur > dealValue)
				{
					dealValueCur = dealValue;
				}
			}
			changeBeofre = transform.position.y;
			transform.position = Vector3.Lerp(transform.position, vector3Value1, dealValueCur * Time.deltaTime);
			//transform.position = new Vector3(transform.position.x, vector3Value1.y, transform.position.z);
			
			if((addState == 1 && transform.position.y > changeBeofre) || (addState == 2 && transform.position.y < changeBeofre))
			{
				transform.position = new Vector3(transform.position.x, changeBeofre, transform.position.z);
			}
			else if (addState == 0 || (addState == 1 && transform.position.y > vector3Value1.y) || (addState == 2 && transform.position.y < vector3Value1.y))
			{
				transform.position = new Vector3(transform.position.x, vector3Value1.y, transform.position.z);
			}
			
			//transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, vector3Value2, 15.0f * Time.deltaTime);
			transform.localEulerAngles = vector3Value2;

			//if (transform.name.CompareTo("kache01(Clone)") == 0 || transform.name.CompareTo("TruckAI1(xin)(Clone)") == 0 )
			//	Debug.Log (Time.time + " " + dealValueChange+ " " +dealValueCur + " "+  transform + " "+ pcvr.uiRunState + " " + addState + " === "+ transform.position.x  + " "+ transform.position.y  + " "+ transform.position.z + " ===== "+ vector3Value1.x + " "+ vector3Value1.y + " "+ vector3Value1.z);
		}
		else
		{
			if (rigidbody.useGravity)
			{
				rigidbody.useGravity = false;
			}

			transform.position = vector3Value1;
		}
	}

	[RPC]
	public void setGameCtrServerObj(NetworkPlayer player, int index, Vector3 v1, Vector3 v2)
	{
		//serverCameraObj.transform.parent = transform;
		if (Network.isServer)
		{
			//record all the players here
			//createServer.linkObjArr[index] = gameObject;
			Debug.Log(index + " setGameaCtrServerObjsetGameCatrServerObj " + gameObject);
			transform.localPosition = v1;
			transform.localEulerAngles = v2;
		}
		else if (Network.player == player)
		{
			//give the player index
			//createServer.playerIndexFinal = index;
			Debug.Log(index + " setGameCatrServerObjsetGameCatrServerObjsssss " + gameObject);
		}
		else
		{
			transform.localPosition = v1;
			transform.localEulerAngles = v2;
		}
	}

	//get one of the player points
	public Transform getPointTransform(int index)
	{
		if (pointNum <= 0)
		{
			index = 0;
		}
		else
		{
			index = UnityEngine.Random.Range (0, pointNum);
		}

		return PointArr[index];
	}
	
	[RPC]
	public void RecordPlayerAIRPC(NetworkPlayer netPlayerT, int playerIndex, int AIIndex, Vector3 pos, Vector3 rot)
	{
		Debug.Log ("RecordPlayerAIRPC     " + Network.isServer + " " + Network.isClient + " " + playerIndex + " "+ AIIndex + " "+ transform + " "+ pos);	
		if (AIIndex > 0)
		{
			pcvr.truckObjScriptArr[AIIndex] = GetComponent<truck>();
			pcvr.gameServerObjSriptArr[AIIndex] = GetComponent<gameServerObjSript>();
			pcvr.truckObjTransArr[AIIndex] = transform;

			pcvr.smallMapUIScrObj.SetAIPlayerHere (transform, AIIndex);
			pcvr.indexFour[AIIndex] = AIIndex + 4;

			if (biaoji)
			{
				biaoji.GetComponent<truckFlag>().setSelfIndex(AIIndex);
			}
		}
		
		if (playerIndex >= 0)
		{
			pcvr.truckObjScriptArr[playerIndex] = GetComponent<truck>();
			pcvr.gameServerObjSriptArr[playerIndex] = GetComponent<gameServerObjSript>();
			pcvr.truckObjTransArr[playerIndex] = transform;

			pcvr.smallMapUIScrObj.SetAIPlayerHere (transform, playerIndex);
			
			pcvr.indexFour[playerIndex] = playerIndex;
			
			if (biaoji)
			{
				biaoji.GetComponent<truckFlag>().setSelfIndex(playerIndex);
			}
		}

		playerN = netPlayerT;
		pcvr.UITruckScrObj.setAIPlayerHere (GetComponent<truck>());
		
		transform.position = pos;
		vector3Value1 = pos;
		transform.eulerAngles = rot;
		hadInit = true;

		if (GetComponent<truck>() && Network.isClient && GetComponent<truck>().isAutoMove)
		{
			GetComponent<truck>().isPlayerTruck = false;
		}

		//if (GetComponent<truck>() && ((Network.isServer && AIIndex < pcvr.totalPlayerNum) || (Network.isClient && GetComponent<truck>().isAutoMove)))
		if (GetComponent<truck>() 
		    && ((Network.isServer && GetComponent<truck>().isPlayerTruck) 
		    || (Network.isClient && !GetComponent<truck>().isPlayerTruck)))
		{//Debug.Log("deleteeeeeeeeeeeeeeeeeeeee  " + Network.isServer);
			transform.rigidbody.useGravity = false;
			transform.rigidbody.isKinematic = true;
			GetComponent<truck>().onlyAddWheelCollider();

			Destroy (GetComponent<truck>());

			Destroy (GetComponent<car>());
			Destroy (GetComponent<XKCarMoveCtrlU3d>());
			Destroy (GetComponent<XKCarWheelCtrl>());
			return;
		}

		if (GetComponent<truck>())
		{
			GetComponent<truck>().hadInit = true;
		}
		
		if (GetComponent<car>())
		{
			GetComponent<car>().CarStart();
		}
	}

	[RPC]
	void wheelsRotationRPC(Quaternion[] wheelQ, Quaternion[] wheelQC, NetworkPlayer playerT, bool isPengleT)
	{
		if (Network.player == playerT)
		{
			return;
		}

		if (isPengleT)
		{
			pengleCurTime = pengleStaticTime;
		}

		if (wheelNum < 2)
		{
			return;
		}
		//Debug.Log (frontWheels[0] + " " + frontWheels[1] + " "+ wheelQ1 + " "+ wheelQ2);

		if (shamoGuanka)
		{
			for (int i=0; i<wheelNum; i++)
			{
				Wheels[i].rotation = wheelQ[i];
				WheelsC[i].rotation = wheelQC[i];
			}
		}
		else
		{
			for (int i=0; i<wheelNum; i++)
			{
				Wheels[i].rotation = wheelQ[i];
			}
		}
	}

	float disCurT = 0.0f;
	float shouldMove = 15.0f;
	float dealValue = 0.0f;
	float dealValueCur = 0.0f;
	float dealValueChange = 0;
	int addState = 0;	//1-jian	2-jia
	float changeBeofre = 0;
	[RPC]
	void RefeshTransform(Vector3 vec1, Vector3 vec2, NetworkPlayer playerT, float huoliziPosition)
	{
		playerN = playerT;
		
		if (Network.player == playerT)
		{
			return;
		}
		
		//disCurT = Mathf.Abs (vec1.z - transform.position.z);
		disCurT = Mathf.Abs (Vector3.Distance(new Vector3(vec1.x, 0, vec1.z), new Vector3(transform.position.x, 0, transform.position.z)));
		dealValue = disCurT * 1.20f;
		
		if (dealValueCur <= 0)
		{
			dealValueCur = dealValue;
		}
		//if (transform.name.CompareTo("TruckAI1(xin)(Clone)") == 0 )
		//	Debug.Log (disCurT + " "+ dealValue + " "+ dealValueCur + " "+ Time.time + " "+ transform.position + " "+ vec1);
		dealValueChange = Mathf.Abs (dealValueCur - dealValue) / 3.0f;
		
		if (!shamoGuanka)
		{
			//vector3Value1 = vec1;
			vector3Value1 = new Vector3(vec1.x, vec1.y - 0.12f, vec1.z);
		}
		else
		{
			vector3Value1 = new Vector3(vec1.x, vec1.y + 0.06f, vec1.z);
		}

		if (transform.position.y - vector3Value1.y > 0.01f)
		{
			addState = 1;
		}
		else if (vector3Value1.y - transform.position.y > 0.01f)
		{
			addState = 2;
		}
		else
		{
			addState = 0;
		}
		
		if (pcvr.uiRunState >= 2)
		{
			//transform.localEulerAngles = vec2;
			vector3Value2 = vec2;
			
			if (penhuoliziPutongObj)
			{
				penhuoliziPutongObj.localPosition = new Vector3(penhuoliziPutongObj.localPosition.x, penhuoliziPutongObj.localPosition.y, huoliziPosition);
			}
		}
	}
	[RPC]
	void RefeshTransform1(Vector3 vec1, Vector3 vec2, NetworkPlayer playerT, float huoliziPosition)
	{
		playerN = playerT;

		if (Network.player == playerT)
		{
			return;
		}

		//disCurT = Mathf.Abs (vec1.z - transform.position.z);
		disCurT = Mathf.Abs (Vector3.Distance(new Vector3(vec1.x, 0, vec1.z), new Vector3(transform.position.x, 0, transform.position.z)));
		if (disCurT > 0)
		{
			if (shouldMove > disCurT)
				dealValue = shouldMove / disCurT;
			else
				dealValue = disCurT /shouldMove * 1.5f;
		}
		else
			dealValue = 5.5f;

		if (dealValue < 5.5f)
			dealValue = 5.5f;
		else if (dealValue > 15)
			dealValue = 15;

		if (dealValueCur <= 0)
		{
			dealValueCur = dealValue;
		}
		//if (/*transform.name.CompareTo("kache01(Clone)") == 0 || */transform.name.CompareTo("TruckAI1(xin)(Clone)") == 0 )
		//Debug.Log (disCurT + " "+ dealValue + " "+ dealValueCur + " "+ Time.time + " "+ transform.position + " "+ vec1);
		dealValueChange = Mathf.Abs (dealValueCur - dealValue) / 3.0f;

		if (!shamoGuanka)
		{
			//vector3Value1 = vec1;
			vector3Value1 = new Vector3(vec1.x, vec1.y - 0.07f, vec1.z);
		}
		else
		{
			vector3Value1 = new Vector3(vec1.x, vec1.y + 0.06f, vec1.z);
		}

		if (pcvr.uiRunState >= 2)
		{
			transform.localEulerAngles = vec2;

			if (penhuoliziPutongObj)
			{
				penhuoliziPutongObj.localPosition = new Vector3(penhuoliziPutongObj.localPosition.x, penhuoliziPutongObj.localPosition.y, huoliziPosition);
			}
		}
	}
	
	[RPC]
	public void SendMoveInforRPC(int playerIndex, float curMoveDistanceT, int jiaohuiIndexT, int lahuiIndexT)
	{
		if (Network.isServer)
		{
			pcvr.distanceFour[playerIndex] = curMoveDistanceT;
			pcvr.jiaohuiPointArr[playerIndex] = jiaohuiIndexT;
			pcvr.lahuiPointArr[playerIndex] = lahuiIndexT;
		}
	}
	
	//about yan and chentu
	[RPC]
	public void controlYanRPC(bool isActive, int index)
	{
		if (isActive)
		{
			for (int i=0; i < yanNumber; i++)
			{
				yan[i].Play();
			}
		}
		else
		{
			for (int i=0; i < yanNumber; i++)
			{
				yan[i].Stop();
			}
		}
	}

	[RPC]
	public void controlChentuRPC(bool isActive, int index)
	{
		if (isActive)
		{
			for (int i=0; i < chenTuNumber; i++)
			{
				chenTu[i].Play();
			}
		}
		else
		{
			for (int i=0; i < chenTuNumber; i++)
			{
				chenTu[i].Stop();
			}
		}
	}
	
	[RPC]
	public void controlWaterTruckRPC(bool isActive, int index)
	{
		if (isActive)
		{
			for (int i=0; i < waterTruckuNumber; i++)
			{
				waterTruck[i].Play();
			}
		}
		else
		{
			for (int i=0; i < waterTruckuNumber; i++)
			{
				waterTruck[i].Stop();
			}
		}
	}
	
	//about yanXieqi
	[RPC]
	public void controlYanXieqiRPC(bool isActive, int index)
	{
		if (isActive)
		{
			for (int i=0; i < yanNumberXieqi; i++)
			{
				yanXieqi[i].Play();
			}
		}
		else
		{
			for (int i=0; i < yanNumberXieqi; i++)
			{
				yanXieqi[i].Stop();
			}
		}
	}
	
	//about penhuo
	[RPC]
	public void controlPenhuoLiziRPC(bool isActive)
	{
		if (isActive && penhuoLizi)
		{
			penhuoLizi.Play();
		}
		else if (penhuoLizi)
		{
			penhuoLizi.Stop();
		}
	}

	float pushF = 50.0f;
	Vector3 forceVec = Vector3.zero;
	float hitPointTime = 0;
	bool isHit = false;

	[RPC]
	public void hitByPlayerPlayerRPC( Vector3 forceVecT, float speed, NetworkPlayer playerT)
	{//Debug.Log ("hitnpccccccccplayer  " + transform + " " + Network.isClient + " " + Network.isServer + " "+ Network.player + " "+ playerT);
		if (Network.player == playerT)
		{
			hitResult(forceVecT, speed);
		}
	}

	public void hitByPlayerPlayer( Vector3 forceVecT, float speed)
	{
		if (Network.isClient)
		{
			transform.networkView.RPC("hitByPlayerPlayerRPC", RPCMode.AllBuffered, forceVecT, speed, Network.player);
			return;
		}

		hitResult(forceVecT, speed);
	}

	void hitResult(Vector3 forceVecT, float speed)
	{
		forceVec = forceVecT;
		hitPointTime = Time.time;
		
		if (isHit)
		{
			return;
		}
		
		isHit = true;
		/*if (speed > 100.0f)
		{
			pushF = 70.0f;
		}
		else
		{
			pushF = 60.0f;
		}*/
		
		CancelInvoke ("pushForward");
		InvokeRepeating ("pushForward", 0, 0.03f);
	}
	
	void pushForward ()
	{
		if (Time.time - hitPointTime > 0.5f)
		{
			isHit = false;
			CancelInvoke ("pushForward");
			return;
		}
		//rigidbody.AddForce(Vector3.Normalize(forceVec) * pushF, ForceMode.Force);
		rigidbody.AddForce(Vector3.Normalize(forceVec) * pushF, ForceMode.Acceleration);
	}
	
	[RPC]
	public void AIEndGameLinkRPC(int playerIndex, bool isPlayer, NetworkPlayer playerT)
	{
		pcvr.indexOnlyFinal[pcvr.finishedNumber] = playerIndex;
		pcvr.finishedNumber ++;

		if (Network.player != playerT)
		{
			jieshule = true;
			rigidbody.isKinematic = true;
		}

		if (Network.isServer && pcvr.finishedNumber == 1)
		{
			pcvr.UITruckScrObj.LinkOnePassLevel();
		}

		if (isPlayer)
		{
			pcvr.finishedPlayerNumber ++;
		}

		if (pcvr.finishedPlayerNumber >= pcvr.totalPlayerNum || pcvr.finishedNumber >= 4)
		{
			transform.networkView.RPC("gameEndHereLink", RPCMode.AllBuffered);
		}
	}

	[RPC]
	void gameEndHereLink()
	{
		if (pcvr.isPassgamelevel)
		{
			return;
		}

		pcvr.isPassgamelevel = true;
		pcvr.UITruckScrObj.hideClientDaojishi ();

		if (Network.isServer)
		{
			pcvr.UITruckScrObj.LinkModeServerGameEnd();
			return;
		}
		else if (Network.isClient)
		{
			Invoke("showScorew", 1.0f);
		}
	}

	void showScorew()
	{
		for (int i = 0; i < 4; i++)
		{Debug.Log("showScor1ewGameserver  " + i + " "+ pcvr.useTimeSelf[i] + " "+ pcvr.finishedNumber);
			if (pcvr.useTimeSelf[i] < 0 && pcvr.finishedNumber > 0)
			{
				pcvr.useTimeSelf[i] = 0;
			}
		}

		pcvr.UITruckScrObj.showPlayerFinalScore(true);
	}
}
