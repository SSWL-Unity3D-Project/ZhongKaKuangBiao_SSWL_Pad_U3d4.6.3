using UnityEngine;
using System;
using System.Collections;

public class truck : MonoBehaviour {

	public bool isAutoMove = false;		//is AI or Player
	public bool isPlayerTruck = false;
	public float AINodeZuoyouPianyiliang = 8.0f;
	public float AINodeGaoPianyiliang = 6.0f;
	public float zhuandongSudu = 0.05f;
	public float stopFollowTime = 2.0f;
	private bool isStopFollowC = false;
	public Transform pathObj;	//the paths parent object
	public int pathObjIndex;	//the paths parent object
    /// <summary>
    /// 游戏开始5秒之后卡车的最高速度.
    /// </summary>
	public float  topSpeedTotal = 160f; //will not change during the game, the top speed
    /// <summary>
    /// 游戏开始后5秒内卡车的最高速度.
    /// </summary>
    public float  startTopSpeedTotal = 160f; //will not change during the game, the top speed
    /// <summary>
    /// 卡车最高速度(km/h).
    /// </summary>
    float m_TruckTopSpeed = 100f;
	private float daojuLastTime = -1;
	public float daojuTime = 1.0f;
	public GameObject m_DaojuParticle;		//should net work
	public float m_BaozhaForward = 0.0f;
	public float m_BaozhaUp = 0.0f;
	private float tishiLastTime = -1;

	private bool isInitOK = false;		//after the truck object is inited ok, then can control the truck
	private Transform truckTrans = null;
	public bool hadInit = false;
	private Vector3 lastPos = Vector3.zero;
	private float distanceTotalKM = 0.0f;

	//public Transform pathObj;	//the paths parent object  ----------move up, can change during edit the scence
	private Transform[] pathObjArr;	//record all the paths objects
	private int totalPathNum = 0;	//the total paths number
	private int curPathIndex = 0;	//current path index
	private int curPathChildNum = 0;
	public Transform[] curPathChildArr;
	private Transform curPathObj = null;
	public Transform curPathPoint = null;
	public Vector3 curPathPointPos = Vector3.zero;
	//private int runIndex = 0;
	
	private bool isGuoleZhongdian = false;
	private int curPaoquanNumber = 0;

	private float wheelRadius = 0.4f;
	public float suspensionRange = 0.1f;
	public float suspensionDamper = 50f;
	public float suspensionSpringFront = 18500f;
	public float suspensionSpringRear = 9000f;
	
	public Vector3 dragMultiplier = new Vector3(2, 5, 1);

	//showGui - top - nowspeed
	//160		120		106
	//226		169		150
	private float throttle = 0f; 
	public float steer = 0f;
	private bool handbrake = false;
	
	public Transform centerOfMass;
	
	public Transform[] frontWheels;
	public Transform[] rearWheels;
	
	private Wheel[] wheels;
	
	private WheelFrictionCurve wfc;

	//private float  topSpeed = 160f;
	private float  topSpeedSecond = 160f;	//will change the km/h-m/s
	public int numberOfGears = 5;
	
	private int maximumTurn = 22;
	private int minimumTurn = 10;
	private float truckMaxAngle = 50.0f;
	
	public Transform FirstPointCameraPoint = null;
	public Transform resetCameraPoint = null;
	
	private float[] engineForceValues;
	private float[] gearSpeeds;
	
	private int currentGear;
	private float currentEnginePower = 0.0f;
	
	private float handbrakeXDragFactor = 0.5f;
	private float initialDragMultiplierX = 10.0f;
	
	private Skidmarks skidmarks = null;
	private ParticleEmitter skidSmoke = null;
	public float[] skidmarkTime;
	
	//private SoundController sound = null;
	
	private bool canSteer;
	private bool canDrive;

	private bool isFirstHit = false;
	private float firstDistance = 0;
	private float firstTimeH = 0;
	
	public  LayerMask dimianLayer;
	private RaycastHit hit;
	private int numLayerCar = -1;
	private int numLayerTerrain = -1;
	
	//terrain
	//private  LayerMask mask;
	//private RaycastHit hit;
	//public Transform[] hitPoints;	//forward back left right-----pointArr
	private Vector3[] hitPointVector;
	public Vector3 testDirection = Vector3.zero;
	private Vector3 testDirectionR = Vector3.zero;
	private float angleValue = 0.0f;
	public float angleZhuan = 0.0f;
	public float dianChengFF = 0;
	public float dianChengFR = 0;
	private int forwardBackStateQN = 0;	//1-back  2-forward
	private float noQNTime = -1.0f;
	private float noQNStaticTime = 1.0f;
	
	class Wheel
	{
		public WheelCollider collider;
		public Transform wheelGraphic;
		public Transform tireGraphic;
		public bool driveWheel = false;
		public bool steerWheel = false;
		public int lastSkidmark = -1;
		public Vector3 lastEmitPosition = Vector3.zero;
		public float lastEmitTime = Time.time;
		public Vector3 wheelVelo = Vector3.zero;
		public Vector3 groundSpeed = Vector3.zero;
	}
	
	public float jiasuTotalTime = 8.0f;
	public float jiasuCurTime = 0;
	private float changeTime = 0.05f;

	//about qingxie
	private int wheelCountMe = 0;
	private int wheelCountMeInt = 0;

	public float resetAngleTotalQX = 0;

	//mingci
	public float speedAdjust = 1;
	public int mingciSelf = 0;
	public  int mingciFirstPlayerRecord = 0;
	private static float adjustTime = 1.0f;
	private float curAdjustTime = 0;
	private int AIIndex = -1;
	private bool AIFinished = false;
	public float AdjustSpeedShould = 60;

	public bool isAdjusting = false;

	public enum TruckGameState
	{
		notMoving,
		moving,
		addSpeed
	}
	public TruckGameState TGameState = TruckGameState.notMoving;
	private bool hadDeleted = false;
	void Start ()
	{/*pcvr.uiRunState = 2;//ffffffffffffffffffffffffff
		isPlayerTruck = true;
		
		if (isPlayerTruck)
		{
			pcvr.playerTruckSObj = transform.GetComponent<truck>();
		}*/
		Time.timeScale = 1;
		pcvr.StartBtLight = StartLightState.Mie;
		lahuiNumber = 0;
		lastPos = Vector3.zero;
		distanceTotalKM = 0.0f;
		noQNTime = -1.0f;
		forwardBackStateQN = 0;
		curPaoquanNumber = 0;

		//Debug.Log ("truck start   " + transform + " " + Network.isServer + " "+ Network.isClient + " AIIndex = " + AIIndex + " " + isAutoMove + " " +pcvr.totalPlayerNum);

		needJiance = true;
		currentLayerCache = LayerMask.LayerToName(transform.gameObject.layer);

		if (!Network.isServer && !Network.isClient)
		{
			hadInit = true;
			StartAfter1();
		}
		else
		{
			StartAfter();
		}
	}

	void StartAfter()
	{
		if (!hadInit)
		{
			Invoke ("StartAfter", 0.5f);
			return;
		}
		else
		{
			StartAfter1();
		}
	}

	void StartAfter1()
	{
		truckTrans = transform;
		lahuiNumber = 0;

		if (Network.isClient && isAutoMove)
		{
			isPlayerTruck = false;
		}
		
		if (pcvr.sound2DScrObj && isPlayerTruck)
		{
			pcvr.sound2DScrObj.setPitchVar (xingshiSpeedMin, xingshiPitchMin, xingshiSpeedMax, xingshiPitchMax);
		}

		//if ((Network.isServer && AIIndex < pcvr.totalPlayerNum) || (Network.isClient && isAutoMove))
		if ((Network.isServer && isPlayerTruck) || (Network.isClient && !isPlayerTruck))
		{Debug.Log("deleteeeeeeeeeeeeeeeeeeeeestart  " + Network.isServer);
			onlyAddWheelCollider();
			Destroy (GetComponent<truck>());
			
			Destroy (GetComponent<car>());
			Destroy (GetComponent<XKCarMoveCtrlU3d>());
			Destroy (GetComponent<XKCarWheelCtrl>());

			transform.rigidbody.useGravity = false;
			transform.rigidbody.isKinematic = true;
			TGameState = TruckGameState.notMoving;
			hadDeleted = true;
			return;
		}

		if (GetComponent<car>())
		{
			GetComponent<car>().CarStart();
		}

		TGameState = TruckGameState.moving;

		speedAdjust = 1 - UnityEngine.Random.Range (0.1f, 0.2f); 
		
		hitPointVector = new Vector3[4]{Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero};

		wheelRotation = new Quaternion[6];
		wheelRotationChild = new Quaternion[6];
		
		for (int i=0; i<6; i++)
		{
			wheelRotation[i] = Quaternion.identity;
			wheelRotationChild[i] = Quaternion.identity;
		}

		if (isPlayerTruck)
		{
			resetResetInfor ();
			return;
		}

		wheels = new Wheel[frontWheels.Length + rearWheels.Length];
		
		SetupWheelColliders();
		
		SetupCenterOfMass();

        //topSpeed = topSpeedTotal;
        m_TruckTopSpeed = startTopSpeedTotal;
        changeToSecond (m_TruckTopSpeed);
		SetupGears();
		resetResetInfor ();
		
		initialDragMultiplierX = dragMultiplier.x;

		initYiXia (1.0f);
        StartCoroutine(DelayChangeTruckTopSpeed());
	}

    /// <summary>
    /// 延迟一定时间之后改变卡车的最高速度.
    /// </summary>
    IEnumerator DelayChangeTruckTopSpeed()
    {
        yield return new WaitForSeconds(8f);
        m_TruckTopSpeed = topSpeedTotal;
        changeToSecond(m_TruckTopSpeed);
    }

	public void onlyAddWheelCollider()
	{Debug.Log (transform + " " + Network.isServer + " only addddd" );
		for (int i=0; i<frontWheels.Length; i++)
			frontWheels[i].gameObject.AddComponent<WheelCollider>();
		for (int i=0; i<rearWheels.Length; i++)
			rearWheels[i].gameObject.AddComponent<WheelCollider>();
	}

	void initYiXia(float timeT)
	{
		speedAdjust = timeT;
	}

	// Use this for initialization
	void Start1 () {//Debug.Log (transform + " sttttttttttttttttttttttttt11" );
		numLayerCar = LayerMask.NameToLayer("car");
		numLayerTerrain = LayerMask.NameToLayer("terrain");
		//mask = 1<<( LayerMask.NameToLayer("car"));

		truckTrans = transform;
		firstDistance = 0;
		firstTimeH = 0;
		pointArrLength = pointArr.Length;
		percSpeed = (120.0f / 105.0f) * (160.0f / 120.0f);

		if (!Network.isServer && !Network.isClient)
		TGameState = TruckGameState.moving;
		audioMoveStopState = 1;
		audioIsXieya = true;

		//init some infromation*********************
		isInGonglu = true;
		isGongluDuan = true;

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
		
		controlYan(false, 1);
		
		if (!isGongluDuan || !isInGonglu)
		{
			controlChentu(false, 1);
		}
		else
		{
			controlChentu(false, 101);
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

		controlYanXieqi (false, 0);

		controlPenhuoLizi (false);

		//water
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
		
		controlWaterTruck (false, 0);
		//**********************
	}
	//private gameServerObjSript myGSEROs = null;
	public void SetAfterSpawn(bool isAutoMoveT, Transform pathObjT, int pathIndexT, Transform spawnPointObj, int AIindexT)
	{//Debug.Log ("SetAfterSpawn   " + transform + " " +isAutoMoveT + " " + Network.isServer + " "+ Network.isClient + " "+ AIindexT);
		//myGSEROs = GetComponent<gameServerObjSript>();
		isAutoMove = isAutoMoveT;
		pathObj = pathObjT;
		pathObjIndex = pathIndexT;
		truckTrans = transform;
		lahuiObj = spawnPointObj;
		lahuiPos = spawnPointObj.position;

		if (isPlayerTruck)
		{
			pcvr.playerTruckSObj = transform.GetComponent<truck>();
		}

		if (isAutoMove)
		{
			AIIndex = AIindexT;
		}
		else
		{
			pcvr.selfIndex = AIindexT;
		}

		if (pcvr.XKCarCameraSObj && isPlayerTruck)
		{
			pcvr.XKCarCameraSObj.setZhujue(truckTrans, resetCameraPoint, FirstPointCameraPoint);
		}

		if (isAutoMove && pathObj)
		{
			totalPathNum = pathObj.childCount;
			
			if (totalPathNum > 0)
			{
				pathObjArr = new Transform[totalPathNum];
				int index = -1;
				
				//record all the paths
				for (int i=0; i < totalPathNum; i++)
				{
					index = Convert.ToInt32(pathObj.GetChild(i).name.Substring(10,2));
					pathObjArr[index] = pathObj.GetChild(i);
				}
			}
			
			if (totalPathNum > 0 && pathObjArr[0].childCount > 1)
			{
				curPathIndex = 0;
				if (!findNextPathInfor(curPathIndex))
				{
					isAutoMove = false;
				}
			}
			else
			{
				isAutoMove = false;
			}
		}
		else
		{
			isAutoMove = false;
		}

		Start1 ();
	}

	bool findNextPathInfor(int pathIndex)
	{
		if (pathIndex >= totalPathNum)
		{
			return false;
		}

		curPathObj = pathObjArr[pathIndex];
		curPathChildNum = curPathObj.childCount;

		if (curPathChildNum <= 0)
		{
			return false;
		}
		
		curPathChildArr = new Transform[curPathChildNum];
		
		int index = -1;
		
		for (int i=0; i<curPathChildNum; i++)
		{
			index = Convert.ToInt32(curPathObj.GetChild(i).name.Substring(5,2));
			curPathChildArr[index] = curPathObj.GetChild(i);
		}

		curPathPoint = curPathChildArr[0];
		findNextPos (curPathPoint);
		return true;
	}

	void findNextPos(Transform curPathPointT)
	{
		if (pathObjIndex == 1)
		{
			curPathPointPos = curPathPointT.position;

			if (Physics.Raycast(curPathPointPos, -Vector3.up, out hit, 50.0f, dimianLayer))
			{
				curPathPointPos.y = hit.point.y + AINodeGaoPianyiliang;
			}
		}
		else if (pathObjIndex == 2)
		{
			curPathPointPos = curPathPointT.position + transform.right * AINodeZuoyouPianyiliang;
			
			if (Physics.Raycast(curPathPointPos, -Vector3.up, out hit, 50.0f, dimianLayer))
			{
				curPathPointPos.y = hit.point.y + AINodeGaoPianyiliang;
			}
		}
		else if (pathObjIndex == 0)
		{
			curPathPointPos = curPathPointT.position + transform.right * (-AINodeZuoyouPianyiliang);
			
			if (Physics.Raycast(curPathPointPos, -Vector3.up, out hit, 50.0f, dimianLayer))
			{
				curPathPointPos.y = hit.point.y + AINodeGaoPianyiliang;
			}
		}
	}

	public float nowSpeed = 0;
	public float nowSpeedV = 0;
	public int isMoving = 0;			//0-not moving, 1-
	private float percSpeed = 1.0f;
	private float topSpeedNow = 0.0f;
	void Update()
	{
		if (hadDeleted || !hadInit || pcvr.uiRunState == 10 || pcvr.uiRunState == 3 || AIFinished || pcvr.isPassgamelevel)
		{//if game over, the truck will slowly down
			return;
		}

		Vector3 relativeVelocity = truckTrans.InverseTransformDirection(rigidbody.velocity);
		
		nowSpeedV = relativeVelocity.z;
		nowSpeed = relativeVelocity.z * 3.6f;

		if (TGameState == TruckGameState.notMoving)
		{
			return;
		}
		else if (TGameState == TruckGameState.addSpeed)
		{
			jiasuCurTime -= Time.deltaTime;
			
			if (jiasuCurTime <= 0 || (jiasuCurTime < (jiasuTotalTime - 1.5f) && nowSpeed <= topSpeedNow * 0.2f))
			{
				TGameState = TruckGameState.moving;
				speedEndHere();
			}
		}

		if (isPlayerTruck)
		{
			UITruck.uiSpeed = rigidbody.velocity.magnitude * 3.6f;
			RadialBlur.uiSpeed = UITruck.uiSpeed;

			if (pcvr.uiRunState >= 2)
			{
				if (lastPos == Vector3.zero)
				{
					lastPos = truckTrans.position;
				}
				else
				{
					distanceTotalKM += Vector3.Distance(truckTrans.position, lastPos);
					lastPos = truckTrans.position;
					KM.truckDistanceKM = distanceTotalKM;
				}
			}
		}

		if (pengle && isAutoMove && needJiance)
		{
			pengleTime -= Time.deltaTime;
			if(Physics.Raycast(truckTrans.position + Vector3.up * 5.0f + (-Vector3.forward) * 1.0f, -Vector3.up, out hit, 100.0f, dimianLayer))
			{
				lidiDistance = Vector3.Distance(new Vector3(0, truckTrans.position.y, 0), new Vector3(0, hit.point.y, 0));

				if (lidiDistance > lidiDisMaxCan * 2)
				{
					lidiTime += 0.2f;
				}
				else if (lidiDistance > lidiDisMaxCan)
				{
					lidiTime += Time.deltaTime;
				}
				else
				{
					lidiTime = 0;
				}
			}
			else
			{
				lidiTime += Time.deltaTime;
			}

			if (lidiTime > 1.6f)
			{
				fanxiangLahui(3);
			}

			if (pengleTime <= 0)
			{
				pengle = false;
				lidiTime = 0.0f;
			}
		}
		else if (isAutoMove && needJiance)
		{
			if(Physics.Raycast(truckTrans.position + Vector3.up * 5.0f + (-Vector3.forward), -Vector3.up, out hit, 100.0f, dimianLayer))
			{
				lidiDistance = Vector3.Distance(new Vector3(0, truckTrans.position.y, 0), new Vector3(0, hit.point.y, 0));
				
				if (lidiDistance > lidiDisMaxCan * 2)
				{
					lidiTime += Time.deltaTime;
				}
				else
				{
					lidiTime = 0;
				}
			}
			else
			{
				lidiTime += Time.deltaTime;
			}
			
			if (lidiTime > 0.85f)
			{
				fanxiangLahui(4);
			}
		}

		if((pcvr.uiRunState >= 2 && pcvr.uiRunState != 10) && !isPlayerTruck)
		{
			GetInput();
		}

		if (!isPlayerTruck)
		{
			UpdateWheelGraphics(relativeVelocity);
			
			UpdateGear(relativeVelocity);
		}

		if (!isFirstHit)
		{
			rigidbody.isKinematic = true;
			isFirstHit = true;
			controlChentu(false, 9);
			isInitOK = true;
			/*firstTimeH += Time.deltaTime;

			if(Physics.Raycast(truckTrans.position + Vector3.up * 5.0f + (-Vector3.forward) * 2.0f, -Vector3.up, out hit, 100.0f, dimianLayer) || firstTimeH > 1.0f)
			{
				firstDistance = Vector3.Distance( new Vector3( 0, truckTrans.position.y, 0 ), new Vector3( 0, hit.point.y, 0 ));

				if (firstDistance <= 0.5f || firstTimeH > 1.0f)
				{
					isFirstHit = true;
					//if (!isPlayerTruck)
					rigidbody.isKinematic = true;
					controlChentu(false, 9);
					isInitOK = true;

					truckTrans.position = new Vector3(truckTrans.position.x, hit.point.y + 0.8f, truckTrans.position.z);
				}
			}*/
			return;
		}
		
		if (isPlayerTruck && pcvr.uiRunState >= 2)
		{
			if(Physics.Raycast(truckTrans.position + Vector3.up * 5.0f + (-Vector3.forward) * 2.0f, -Vector3.up, out hit, 100.0f, dimianLayer))
			{
				luokongTime = 0;
			}
			else
			{
				luokongTime += Time.deltaTime;

				if (luokongTime >= luokongStaticTime && !isStopFollowC)
				{
					luokongTime = 0;
					fanxiangLahui(5);
				}
			}
			
			if (nowSpeed < 1.0f && audioMoveStopState != 1 )
			{
				playAudioBrake(1, false);
				playAudioBrake(2, false);
			}

			if (Mathf.Abs(nowSpeed) > 1.0f && audioMoveStopState == 1)
			{
				playAudioMoveStop (2);
			}
			else if (Mathf.Abs(nowSpeed) < 1.0f && audioMoveStopState != 1)
			{
				playAudioMoveStop (1);
			}

			if (pcvr.playerThrottle == 0 && !audioIsXieya)
			{
				audioIsXieya = true;
				playAudioXieya(true);
			}
			else if (pcvr.playerThrottle != 0 && audioIsXieya)
			{
				audioIsXieya = false;
			}

			if (pcvr.playerBrake)
			{
				audioDianshaTime += Time.deltaTime;
			}
			else if (audioDianshaTime > 0.25f && audioDianshaTime <= 0.5f && !pcvr.isChehen)
			{
				audioDianshaTime = 0;
				playAudioBrake(1, true);
			}
			else
			{
				audioDianshaTime = 0;
			}
			
			if (pcvr.sound2DScrObj)
			{
				pcvr.sound2DScrObj.changeSpeedPithc(audioMoveStopState, nowSpeed);
			}

			if ((!isInGonglu/* || TGameState == TruckGameState.addSpeed*/) && Mathf.Abs(nowSpeed) > 1.0f)
			{
				/*if (isInwater)
				{
					//fangxiangpanDoudongIndex = 3;
				}
				else if (isInSuishiLu)
				{
					//fangxiangpanDoudongIndex = 2;
				}
				else if (isInShadiLu)
				{
					//fangxiangpanDoudongIndex = 0;
				}
				else
				{
					//fangxiangpanDoudongIndex = 1;
				}*/

				pcvr.GetInstance().OpenFangXiangPanZhenDong(fangxiangpanDoudongIndex, true);
			}
			else
			{
				pcvr.GetInstance().CloseFangXiangPanZhenDong();
			}
		}
		
		if (resetTimeTotal < 2.5f && pcvr.uiRunState >= 2)
		{
			resetTimeTotal += Time.deltaTime;
			
			//the forward-direction distance
			if ((isPlayerTruck && pcvr.playerThrottle > 0 && !pcvr.playerBrake)
			    || (!isPlayerTruck && throttle > 0))
			{
				pressTimeTotal += Time.deltaTime;
				resetDistance = Vector3.Distance(resetPosition, truckTrans.position);
				resetDistanceXZ = Vector3.Distance(new Vector3(resetPosition.x, 0, resetPosition.z), new Vector3(truckTrans.position.x, 0, truckTrans.position.z));
			}
			else
			{
				pressTimeTotal = 0;
				resetDistance = 100;
				resetDistanceXZ = 100;
			}
			
			//the Y-direction distance
			resetDistanceY = Mathf.Abs(resetPosition.y - truckTrans.position.y);
		}
		else if (pcvr.uiRunState >= 2)
		{
			//check whether the truck needs to reset
			if ((pressTimeTotal > 1.5f && resetDistance < 2.5f)
			    || (resetDistanceY > 7.65f && resetDistanceXZ < 5.0f)
			    /*|| resetDistanceY > 15.65f*/)
			{//Debug.Log(pressTimeTotal + " " + resetDistance + " " + resetDistanceY);
				resetNewPoint(1);
			}
			
			resetResetInfor();
		}

		if (isActiveChentu && relativeVelocity.z < 17.0f && isPlayerTruck)
		{
			controlChentu(false, 97);	//or water

			if (isInwater)
			{
				controlWaterTruck(false, 970);
			}
		}
		else if (!isActiveChentu && relativeVelocity.z >= 17.0f && isPlayerTruck)
		{
			if (!isGongluDuan || !isInGonglu)
			{
				if (isInwater)
				{
					controlWaterTruck(true, 210);
				}
				else
				{
					controlChentu(true, 200);	//or water
				}
			}
		}
	}

	private Vector3 vector3Value1 = Vector3.zero;
	private Vector3 vector3Value2 = Vector3.zero;
	float distancePath1 = 0;
	private Quaternion[] wheelRotation;
	private Quaternion[] wheelRotationChild;
	bool notttt = false;
	void FixedUpdate()
	{
		if (hadDeleted || !hadInit || !truckTrans || pcvr.uiRunState == 10 || AIFinished || pcvr.isPassgamelevel || pcvr.uiRunState == 3)
		{//change  lxy  server or client
			if (isFirstHit && (AIFinished || pcvr.isPassgamelevel || pcvr.uiRunState == 10 || pcvr.uiRunState == 3) && !rigidbody.isKinematic)
			{
				rigidbody.velocity = Vector3.Lerp (rigidbody.velocity, Vector3.zero, 2.1f * Time.deltaTime);

				//Debug.Log("playerrrrrrrrrr" + transform + " "+ rigidbody.velocity + " "+ rigidbody.velocity.magnitude);
				if (rigidbody.velocity.magnitude <= 0.15f)
				{
					rigidbody.isKinematic = true;
				}
			}
			return;
		}

		if (TGameState == TruckGameState.notMoving)
		{
			return;
		}

		if ( pcvr.uiRunState >= 2 && !isPlayerTruck && throttle > 0 
		     && !rigidbody.isKinematic 
		     && ((truckTrans.eulerAngles.x >= 335 && truckTrans.eulerAngles.x <= 360) || truckTrans.eulerAngles.x < 90))
		{
			if (nowSpeed < m_TruckTopSpeed * speedAdjust)
			rigidbody.velocity = Vector3.Lerp (rigidbody.velocity, (truckTrans.forward * (m_TruckTopSpeed * speedAdjust)), changeTime * Time.deltaTime);

			if (!isActiveChentu && (!isGongluDuan || !isInGonglu))
			{
				controlChentu(true, 20);	//or water
			}
		}

		/*if ( pcvr.uiRunState >= 2 && isPlayerTruck && pcvr.playerThrottle > 0 && !notttt
		     && !rigidbody.isKinematic 
		     && ((truckTrans.eulerAngles.x >= 335 && truckTrans.eulerAngles.x <= 360) || truckTrans.eulerAngles.x < 90))
		{
			if (nowSpeed < topSpeedTotal * 2)
				rigidbody.velocity = Vector3.Lerp (rigidbody.velocity, (truckTrans.forward * (topSpeedTotal * 2)), 1.0f * Time.deltaTime);
			else
				notttt = true;
			
			Debug.Log(transform + " "+ rigidbody.velocity + " "+ topSpeedTotal + " "+ nowSpeed);
		}
		else if (notttt && pcvr.uiRunState >= 2 && isPlayerTruck 
		         && !rigidbody.isKinematic && nowSpeed < 30.0f)
		{
			notttt = false;
		}*/

		if (isAutoMove && pcvr.uiRunState >= 2)
		{
			if (!curPathPoint)
			{
				return;
			}

			if (curAdjustTime >= adjustTime)
			{
				curAdjustTime = 0;
				//adjust
				if (Application.loadedLevel == 8)
				{
					adjustSpeedHereLevel1();
				}
				else
				{
					adjustSpeedHereLevel2();
				}
			}
			else
			{
				curAdjustTime += Time.deltaTime;
			}

			truckTrans.forward = Vector3.Lerp(truckTrans.forward, curPathPointPos - truckTrans.position, zhuandongSudu * Time.deltaTime);
			
			hitPointVector[0] = Vector3.zero;	//forward
			hitPointVector[1] = Vector3.zero;	//back
			
			if(Physics.Raycast(pointArr[0].position, -Vector3.up, out hit, 8.0f, dimianLayer))
			{
				hitPointVector[0] = hit.point;
			}
			
			if(Physics.Raycast(pointArr[1].position, -Vector3.up, out hit, 8.0f, dimianLayer))
			{
				hitPointVector[1] = hit.point;
			}
			
			testDirection = Vector3.Normalize (hitPointVector[0] - hitPointVector[1]);
			truckTrans.forward = Vector3.Lerp(truckTrans.forward, testDirection, 0.75f);
			
			hitPointVector[2] = Vector3.zero;	//left
			hitPointVector[3] = Vector3.zero;	//right
			
			if(Physics.Raycast(pointArr[2].position, -Vector3.up, out hit, 8.0f, dimianLayer))
			{
				hitPointVector[2] = hit.point;
			}
			
			if(Physics.Raycast(pointArr[3].position, -Vector3.up, out hit, 8.0f, dimianLayer))
			{
				hitPointVector[3] = hit.point;
			}
			
			testDirectionR = Vector3.Normalize (hitPointVector[3] - hitPointVector[2]);
			angleValue = Vector3.Angle (testDirectionR, truckTrans.right);
			
			if(hitPointVector[3].y > hitPointVector[2].y && (angleValue > 1 && angleValue <45))
			{
				truckTrans.eulerAngles = new Vector3 (truckTrans.eulerAngles.x, truckTrans.eulerAngles.y, angleValue);
			}
			else if(hitPointVector[3].y < hitPointVector[2].y && (angleValue > 1 && angleValue <45))
			{
				truckTrans.eulerAngles = new Vector3 (truckTrans.eulerAngles.x, truckTrans.eulerAngles.y, -angleValue);
			}
			else if(hitPointVector[3].y == hitPointVector[2].y)
			{
				angleValue = 0;
				truckTrans.eulerAngles = new Vector3 (truckTrans.eulerAngles.x, truckTrans.eulerAngles.y, -angleValue);
			}
		}
		else if (pcvr.uiRunState == 2)
		{
			if (Mathf.Abs(nowSpeed) > 1.0f && pcvr.mGetSteer > 0.3f)
			{
				pcvr.m_IsOpneLeftQinang = true;
				pcvr.m_IsOpneRightQinang = false;
			}
			else if (Mathf.Abs(nowSpeed) > 1.0f && pcvr.mGetSteer < -0.3f)
			{
				pcvr.m_IsOpneLeftQinang = false;
				pcvr.m_IsOpneRightQinang = true;
			}
			else
			{
				pcvr.m_IsOpneLeftQinang = false;
				pcvr.m_IsOpneRightQinang = false;
			}

			if (noQNTime < 0)
			{
				noQNTime = Time.time;
			}

			if (forwardBackStateQN == 1)
			{//back
				noQNTime = Time.time;

				if (pcvr.m_IsOpneLeftQinang)
				{
					pcvr.GetInstance ().ControlQinang (1);
					pcvr.GetInstance ().ControlQinang (4);
					pcvr.GetInstance ().ControlQinang (3);

					pcvr.GetInstance ().ControlQinang (7);
				}
				else if (pcvr.m_IsOpneRightQinang)
				{
					pcvr.GetInstance ().ControlQinang (2);
					pcvr.GetInstance ().ControlQinang (4);
					pcvr.GetInstance ().ControlQinang (3);
					
					pcvr.GetInstance ().ControlQinang (6);
				}
				else
				{
					pcvr.GetInstance ().ControlQinang (4);
					pcvr.GetInstance ().ControlQinang (3);
					
					pcvr.GetInstance ().ControlQinang (6);
					pcvr.GetInstance ().ControlQinang (7);
				}
			}
			else if (forwardBackStateQN == 2)
			{//forward
				noQNTime = Time.time;

				if (pcvr.m_IsOpneLeftQinang)
				{
					pcvr.GetInstance ().ControlQinang (1);
					pcvr.GetInstance ().ControlQinang (2);
					pcvr.GetInstance ().ControlQinang (4);
					
					pcvr.GetInstance ().ControlQinang (8);
				}
				else if (pcvr.m_IsOpneRightQinang)
				{
					pcvr.GetInstance ().ControlQinang (1);
					pcvr.GetInstance ().ControlQinang (2);
					pcvr.GetInstance ().ControlQinang (3);
					
					pcvr.GetInstance ().ControlQinang (9);
				}
				else
				{
					pcvr.GetInstance ().ControlQinang (1);
					pcvr.GetInstance ().ControlQinang (2);
					
					pcvr.GetInstance ().ControlQinang (9);
					pcvr.GetInstance ().ControlQinang (8);
				}
			}
			else if (forwardBackStateQN == 0)
			{//no back forward
				if (TGameState == TruckGameState.addSpeed)
				{
					noQNTime = Time.time;
					
					if (isUseNormal && Time.time - chongqiTime > 1.0f)
					{
						isUseNormal = false;
					}

					if (pcvr.m_IsOpneLeftQinang)
					{
						if (isUseNormal)
						{
							pcvr.GetInstance ().ControlQinang (1);
							pcvr.GetInstance ().ControlQinang (2);
						}

						pcvr.GetInstance ().ControlQinang (4);
						
						pcvr.GetInstance ().ControlQinang (8);
					}
					else if (pcvr.m_IsOpneRightQinang)
					{
						if (isUseNormal)
						{
							pcvr.GetInstance ().ControlQinang (1);
							pcvr.GetInstance ().ControlQinang (2);
						}

						pcvr.GetInstance ().ControlQinang (3);
						
						pcvr.GetInstance ().ControlQinang (9);
					}
					else
					{
						if (isUseNormal)
						{
							pcvr.GetInstance ().ControlQinang (1);
							pcvr.GetInstance ().ControlQinang (2);
						}
						
						pcvr.GetInstance ().ControlQinang (9);
						pcvr.GetInstance ().ControlQinang (8);
					}
				}
				else
				{
					if (pcvr.m_IsOpneLeftQinang)
					{
						noQNTime = Time.time;

						pcvr.GetInstance ().ControlQinang (1);
						pcvr.GetInstance ().ControlQinang (4);
						
						pcvr.GetInstance ().ControlQinang (7);
						pcvr.GetInstance ().ControlQinang (8);
					}
					else if (pcvr.m_IsOpneRightQinang)
					{
						noQNTime = Time.time;

						pcvr.GetInstance ().ControlQinang (2);
						pcvr.GetInstance ().ControlQinang (3);
						
						pcvr.GetInstance ().ControlQinang (6);
						pcvr.GetInstance ().ControlQinang (9);
					}
					else
					{
						if (Time.time - noQNTime > noQNStaticTime && nowSpeed > topSpeedNow * 0.3f)
						{
							pcvr.GetInstance().OpenQinangDouDong(false);
						}
						else
						{
							pcvr.GetInstance ().ControlQinang (6);
							pcvr.GetInstance ().ControlQinang (7);
							pcvr.GetInstance ().ControlQinang (8);
							pcvr.GetInstance ().ControlQinang (9);
						}
					}
				}
			}

			//Debug.Log("aaa " +  transform + " " + hitPointVector[2].y + " " + hitPointVector[3].y + " " +angleValue + " "+ pcvr.m_IsOpneLeftQinang + " "+ pcvr.m_IsOpneRightQinang);
			if (lahuiObj)
			{
				angleZhuan = Vector3.Angle(lahuiObj.forward, truckTrans.forward);
				dianChengFR = Vector3.Dot(lahuiObj.right, truckTrans.forward);

				if(Mathf.Abs(angleZhuan) >= 110.0f)
				{
					fanxiangTime += Time.deltaTime;
					//UITruck.isFanxiang = true;
					if (fanxiangTime >= fanxiangStaticTime * 0.6f)
					{
						UITruck.isFanxiang = true;
					}
					if (fanxiangTime >= fanxiangStaticTime)
					{
						UITruck.isFanxiang = false;
						fanxiangTime = 0;
						fanxiangLahui(6);
					}
				}
				else if(Mathf.Abs(angleZhuan) < 110.0f)
				{
					fanxiangTime = 0;
					UITruck.isFanxiang = false;
				}
			}
			else
			{
				fanxiangTime = 0;
				UITruck.isFanxiang = false;
			}
		}
		else if (!isAutoMove)
		{
			pcvr.GetInstance ().ControlQinang (10);
		}
		// The rigidbody velocity is always given in world space, but in order to work in local space of the car model we need to transform it first.
		Vector3 relativeVelocity = truckTrans.InverseTransformDirection(rigidbody.velocity);

		if (lahuiObj)
		{
			curMoveDistance = Vector3.Distance(lahuiObj.position, truckTrans.position);
		}

		if((pcvr.uiRunState >= 2 && pcvr.uiRunState != 10) && !isPlayerTruck)
		{
			CalculateState();
			UpdateFriction(relativeVelocity);
			UpdateDrag(relativeVelocity);
			CalculateEnginePower(relativeVelocity);
			ApplyThrottle(canDrive, relativeVelocity);
			ApplySteering(canSteer, relativeVelocity);
		}
		
		if (Network.isServer || Network.isClient)
		{
			if (isAutoMove)
			{
				vector3Value1 = new Vector3(truckTrans.position.x, truckTrans.position.y, truckTrans.position.z);
				vector3Value2 = new Vector3(truckTrans.eulerAngles.x, truckTrans.eulerAngles.y, truckTrans.eulerAngles.z);
				
				truckTrans.networkView.RPC("RefeshTransform", RPCMode.AllBuffered, vector3Value1, vector3Value2, Network.player, 0.0f);
			}
			wheelRotation[0] = frontWheels[0].rotation;
			wheelRotation[1] = frontWheels[1].rotation;
			wheelRotationChild[0] = frontWheels[0].GetChild(0).rotation;
			wheelRotationChild[1] = frontWheels[1].GetChild(0).rotation;
			
			for (int i=0; i<4; i++)
			{
				wheelRotation[2 + i] = rearWheels[i].rotation;
				wheelRotationChild[2 + i] = rearWheels[i].GetChild(0).rotation;
			}
			
			transform.networkView.RPC("wheelsRotationRPC", RPCMode.AllBuffered, wheelRotation, wheelRotationChild, Network.player, penglePlayer);
			
			if (penglePlayer)
			{
				penglePlayer = false;
			}
		}
	}

	void LateUpdate()
	{
		if (hadDeleted || !hadInit || !truckTrans || pcvr.uiRunState == 10 || AIFinished || pcvr.isPassgamelevel)
		{
			return;
		}

		if (TGameState == TruckGameState.notMoving)
		{
			return;
		}

		if (isAutoMove && AIIndex >= 0)
		{//AI
			pcvr.distanceFour[AIIndex] = curMoveDistance;
			pcvr.jiaohuiPointArr[AIIndex] = jiaohuiIndex;
			pcvr.lahuiPointArr[AIIndex] = lahuiIndex;
		}
		else if (!isAutoMove)
		{//Player
			if (Network.isClient && networkView)
			{
				networkView.RPC("SendMoveInforRPC", RPCMode.All, pcvr.selfIndex, curMoveDistance, jiaohuiIndex, lahuiIndex);
			}
			else
			{
				pcvr.distanceFour[pcvr.selfIndex] = curMoveDistance;
				pcvr.jiaohuiPointArr[pcvr.selfIndex] = jiaohuiIndex;
				pcvr.lahuiPointArr[pcvr.selfIndex] = lahuiIndex;
			}
		}
	}

	void SetupWheelColliders()
	{
		SetupWheelFrictionCurve();
		
		int wheelCount1 = 0;
		wheelCountMe = wheelCount1;
		
		foreach (Transform t in frontWheels)
		{
			wheels[wheelCount1] = SetupWheel(t, true);
			wheelCount1++;
			wheelCountMe = wheelCount1;
		}
		
		foreach (Transform t in rearWheels)
		{
			wheels[wheelCount1] = SetupWheel(t, false);
			wheelCount1++;
			wheelCountMe = wheelCount1;
		}

		wheelCountMeInt = (int)(wheelCountMe / 2);
	}
	
	void SetupWheelFrictionCurve()
	{
		wfc = new WheelFrictionCurve();
		wfc.extremumSlip = 1;
		wfc.extremumValue = 50;
		wfc.asymptoteSlip = 2;
		wfc.asymptoteValue = 25;
		wfc.stiffness = 1;
	}
	
	Wheel SetupWheel(Transform wheelTransform, bool isFrontWheel)
	{
		GameObject go = new GameObject(wheelTransform.name + " Collider");
		go.transform.position = wheelTransform.position;
		go.transform.parent = truckTrans;
		go.transform.rotation = wheelTransform.rotation;
		
		WheelCollider wc = go.AddComponent(typeof(WheelCollider)) as WheelCollider;
		wc.suspensionDistance = suspensionRange;
		JointSpring js = wc.suspensionSpring;
		
		if (isFrontWheel)
			js.spring = suspensionSpringFront;
		else
			js.spring = suspensionSpringRear;
		
		js.damper = suspensionDamper;
		wc.suspensionSpring = js;
		
		Wheel wheel = new Wheel(); 
		wheel.collider = wc;
		wc.sidewaysFriction = wfc;
		wheel.wheelGraphic = wheelTransform;
		wheel.tireGraphic = wheelTransform.GetComponentsInChildren<Transform>()[1];
		//flag-lxy

		wheelRadius = wheel.tireGraphic.renderer.bounds.size.y / 2;	
		wheel.collider.radius = wheelRadius;
		
		if (isFrontWheel)
		{
			wheel.steerWheel = true;
			
			go = new GameObject(wheelTransform.name + " Steer Column");
			go.transform.position = wheelTransform.position;
			go.transform.rotation = wheelTransform.rotation;
			go.transform.parent = truckTrans;
			wheelTransform.parent = go.transform;
		}
		else
			wheel.driveWheel = true;
		
		return wheel;
	}
	
	void SetupCenterOfMass()
	{
		if(centerOfMass != null)
			rigidbody.centerOfMass = centerOfMass.localPosition;
	}
	
	void SetupGears()
	{
		engineForceValues = new float[numberOfGears];
		gearSpeeds = new float[numberOfGears];
		
		float tempTopSpeed = topSpeedSecond * speedAdjust;
		
		for(int i = 0; i < numberOfGears; i++)
		{
			if(i > 0)
				gearSpeeds[i] = tempTopSpeed / 4 + gearSpeeds[i-1];
			else
				gearSpeeds[i] = tempTopSpeed / 4;
			
			tempTopSpeed -= tempTopSpeed / 4;
		}
		
		float engineFactor = (topSpeedSecond * speedAdjust) / gearSpeeds[gearSpeeds.Length - 1];
		
		for(int i = 0; i < numberOfGears; i++)
		{
			float maxLinearDrag = gearSpeeds[i] * gearSpeeds[i];// * dragMultiplier.z;
			engineForceValues[i] = maxLinearDrag * engineFactor;
		}
	}
	
	void SetUpSkidmarks()
	{
		if(GameObject.FindObjectOfType(typeof(Skidmarks)))
		{
			skidmarks = (Skidmarks)FindObjectOfType(typeof(Skidmarks));
			skidSmoke = skidmarks.GetComponentInChildren<ParticleEmitter>();
		}
		else
			Debug.Log("No skidmarks object found. Skidmarks will not be drawn");
		
		skidmarkTime = new float[wheelCountMe];
		for (int i=0; i<wheelCountMe; i++)
		{
			skidmarkTime[i] = 0f;
		}
		//foreach (float f in skidmarkTime)
		//	f = 0.0;
	}
	
	/**************************************************/
	/* Functions called from Update()                 */
	/**************************************************/
	
	void GetInput()
	{
		if (TGameState == TruckGameState.notMoving)
		{
			return;
		}

		throttle = Input.GetAxis("Vertical");
		steer = Input.GetAxis("Horizontal");

		if (!isInitOK)
		{
			throttle = 0;
			steer = 0;
		}

		if(isInitOK && isAutoMove)
		{
			throttle = 1;
			steer = 0;
		}

		if (throttle > 0 && isMoving == 1)
		{//changellllllllll  lxy
			if (!isActiveYan)
				controlYan(true, 3);
			rigidbody.isKinematic = false;
		}
		else if (throttle > 0 && isMoving != 1)
		{
			if (!isActiveYan)
				controlYan(true, 4);
			isMoving = 1;
			rigidbody.isKinematic = false;
		}

		if (!resetIsRecord)
		{
			resetResetInfor();
			resetIsRecord = true;
		}
	}

	private float fanxiangTime = 0;
	public float fanxiangStaticTime = 3.0f;
	
	private float luokongTime = 0;
	private float luokongStaticTime = 3.0f;

	void resetResetInfor()
	{//Debug.Log ("reset   resetRessetInforresetResestInforresetResestInforresetRessetInfor");
		resetIsRecord = false;
		resetDistance = 0;
		resetDistanceXZ = 0;
		resetDistanceY = 0;
		resetTimeTotal = 0;
		luokongTime = 0;
		resetPosition = truckTrans.position;
	}

	void fanxiangLahui(int index)
	{
		resetNewPoint(index);
		resetResetInfor();
	}
	
	public int wheelCount;
	void UpdateWheelGraphics(Vector3 relativeVelocity)
	{
		wheelCount = -1;
		
		foreach(Wheel w in wheels)
		{
			wheelCount++;
			WheelCollider wheel = w.collider;
			WheelHit wh = new WheelHit();
			
			// First we get the velocity at the point where the wheel meets the ground, if the wheel is touching the ground
			if(wheel.GetGroundHit(out wh))
			{
				//flag-lxy
				w.wheelGraphic.localPosition = wheel.transform.up * (wheelRadius + wheel.transform.InverseTransformPoint(wh.point).y);
				w.wheelVelo = rigidbody.GetPointVelocity(wh.point);
				w.groundSpeed = w.wheelGraphic.InverseTransformDirection(w.wheelVelo);

				// Code to handle skidmark drawing. Not covered in the tutorial
				if(skidmarks)
				{
					if(skidmarkTime[wheelCount] < 0.02f && w.lastSkidmark != -1)
					{
						skidmarkTime[wheelCount] += Time.deltaTime;
					}
					else
					{
						float dt = skidmarkTime[wheelCount] == 0.0f ? Time.deltaTime : skidmarkTime[wheelCount];
						skidmarkTime[wheelCount] = 0.0f;
						
						float handbrakeSkidding = handbrake && w.driveWheel ? w.wheelVelo.magnitude * 0.3f : 0;
						float skidGroundSpeed = Mathf.Abs(w.groundSpeed.x) - 15;
						if(skidGroundSpeed > 0 || handbrakeSkidding > 0)
						{//Debug.Log("skidmarks  " + transform + " " +wheelCount);
							//Vector3 staticVel = truckTrans.TransformDirection(skidSmoke.localVelocity) + skidSmoke.worldVelocity;
							if(w.lastSkidmark != -1)
							{
								float emission = UnityEngine.Random.Range(skidSmoke.minEmission, skidSmoke.maxEmission);
								float lastParticleCount = w.lastEmitTime * emission;
								float currentParticleCount = Time.time * emission;
								int noOfParticles = Mathf.CeilToInt(currentParticleCount) - Mathf.CeilToInt(lastParticleCount);
								int lastParticle = Mathf.CeilToInt(lastParticleCount);
							}
							
							w.lastEmitPosition = wh.point;
							w.lastEmitTime = Time.time;
							
							w.lastSkidmark = skidmarks.AddSkidMark(wh.point + rigidbody.velocity * dt, wh.normal, (skidGroundSpeed * 0.1f + handbrakeSkidding) * Mathf.Clamp01(wh.force / wheel.suspensionSpring.spring), w.lastSkidmark);
							//sound.Skid(true, Mathf.Clamp01(skidGroundSpeed * 0.1));
						}
						else
						{
							w.lastSkidmark = -1;
							//sound.Skid(false, 0);
						}
					}
				}
			}
			else
			{
				// If the wheel is not touching the ground we set the position of the wheel graphics to
				// the wheel's transform position + the range of the suspension.
				w.wheelGraphic.position = wheel.transform.position + (-wheel.transform.up * suspensionRange);
				if(w.steerWheel)
					w.wheelVelo *= 0.9f;
				else
					w.wheelVelo *= 0.9f * (1 - throttle);
				
				if(skidmarks)
				{
					w.lastSkidmark = -1;
					//sound.Skid(false, 0);
				}
			}

			if(w.steerWheel && !rigidbody.isKinematic)
			{//forward
				Vector3 ea = w.wheelGraphic.parent.localEulerAngles;
				//ea.y = steer * maximumTurn;

				if (Mathf.Abs(resetAngleTotalQX) < truckMaxAngle)
				{
					ea.y = (resetAngleTotalQX / truckMaxAngle) * maximumTurn;
				}
				else
				{
					ea.y = 0;
				}
				//ea.y = resetAngleTotalQX;
				w.wheelGraphic.parent.localEulerAngles = ea;
				w.tireGraphic.Rotate(Vector3.left * (w.groundSpeed.z / wheelRadius) * Time.deltaTime * Mathf.Rad2Deg);
				//w.tireGraphic.Rotate(Vector3.left * Time.deltaTime * 50.0f);
				//if (wheelCount == 1)
				//	Debug.Log(w.tireGraphic.eulerAngles + " " +w.wheelGraphic.parent.localEulerAngles + "  "+ maximumTurn + " " + ea.y);
			}
			else if(w.driveWheel && !rigidbody.isKinematic)
			{//back
				// If the wheel is a drive wheel it only gets the rotation that visualizes speed.
				// If we are hand braking we don't rotate it.
				w.tireGraphic.Rotate(Vector3.left * (w.groundSpeed.z / wheelRadius) * Time.deltaTime * Mathf.Rad2Deg);
			}
		}
	}
	
	void UpdateGear(Vector3 relativeVelocity )
	{
		currentGear = 0;
		for(int i = 0; i < numberOfGears - 1; i++)
		{
			if(relativeVelocity.z > gearSpeeds[i])
				currentGear = i + 1;
		}
	}
	
	/**************************************************/
	/* Functions called from FixedUpdate()            */
	/**************************************************/
	
	void UpdateDrag(Vector3 relativeVelocity)
	{
		Vector3 relativeDrag = new Vector3(	-relativeVelocity.x * Mathf.Abs(relativeVelocity.x), 
		                                         -relativeVelocity.y * Mathf.Abs(relativeVelocity.y), 
		                                         -relativeVelocity.z * Mathf.Abs(relativeVelocity.z) );
		
		Vector3 drag = Vector3.Scale(dragMultiplier, relativeDrag);
		
		if(initialDragMultiplierX > dragMultiplier.x) // Handbrake code
		{			
			drag.x /= (relativeVelocity.magnitude / ((topSpeedSecond * speedAdjust) / ( 1 + 2 * handbrakeXDragFactor ) ) );
			drag.z *= (1 + Mathf.Abs(Vector3.Dot(rigidbody.velocity.normalized, truckTrans.forward)));
			drag += rigidbody.velocity * Mathf.Clamp01(rigidbody.velocity.magnitude / (topSpeedSecond * speedAdjust));
		}
		else // No handbrake
		{
			drag.x *= (topSpeedSecond * speedAdjust) / relativeVelocity.magnitude;
		}
		
		if(Mathf.Abs(relativeVelocity.x) < 5 && !handbrake)
			drag.x = -relativeVelocity.x * dragMultiplier.x;
		
		if (!rigidbody.isKinematic)
			rigidbody.AddForce(truckTrans.TransformDirection(drag) * rigidbody.mass * Time.deltaTime);
		//Debug.Log ("drag  " + drag);
	}
	
	void UpdateFriction(Vector3 relativeVelocity )
	{
		float sqrVel = relativeVelocity.x * relativeVelocity.x;
		
		// Add extra sideways friction based on the car's turning velocity to avoid slipping
		wfc.extremumValue = Mathf.Clamp(300 - sqrVel, 0, 300);
		wfc.asymptoteValue = Mathf.Clamp(150 - (sqrVel / 2), 0, 150);
		
		foreach(Wheel w in wheels)
		{
			w.collider.sidewaysFriction = wfc;
			w.collider.forwardFriction = wfc;
		}
	}

	void CalculateEnginePower(Vector3 relativeVelocity)
	{
		if(throttle == 0)
		{
			currentEnginePower -= Time.deltaTime * 200;
		}
		else if( HaveTheSameSign(relativeVelocity.z, throttle) )
		{
			float normPower = (currentEnginePower / engineForceValues[engineForceValues.Length - 1]) * 2;
			currentEnginePower += Time.deltaTime * 200 * EvaluateNormPower(normPower);
		}
		else
		{
			currentEnginePower -= Time.deltaTime * 300;
		}
		
		if(currentGear == 0)
		{
			currentEnginePower = Mathf.Clamp(currentEnginePower, 0, engineForceValues[0]);
		}
		else
		{
			currentEnginePower = Mathf.Clamp(currentEnginePower, engineForceValues[currentGear - 1], engineForceValues[currentGear]);
		}
	}
	
	void CalculateState()
	{
		canDrive = false;
		canSteer = false;
		
		foreach(Wheel w in wheels)
		{
			if(w.collider.isGrounded)
			{
				if(w.steerWheel)
					canSteer = true;
				if(w.driveWheel)
					canDrive = true;
			}
		}
	}
	
	void ApplyThrottle(bool canDrive, Vector3 relativeVelocity)
	{
		if(canDrive)
		{
			float throttleForce = 0;
			float brakeForce = 0;
			
			if (HaveTheSameSign(relativeVelocity.z, throttle))
			{
				if (!handbrake)
				{
					throttleForce = Mathf.Sign(throttle) * currentEnginePower * rigidbody.mass;
				}
			}
			else
			{
				brakeForce = Mathf.Sign(throttle) * engineForceValues[0] * rigidbody.mass;
			}

			if (!rigidbody.isKinematic && (throttleForce + brakeForce) > 0)
				rigidbody.AddForce(truckTrans.forward * Time.deltaTime * (throttleForce + brakeForce));
		}
	}
	public bool pengle = false;
	private bool penglePlayer = false;
	private static float pengleTotalTime = 1.0f;
	private float pengleTime = 0.0f;
	private float lidiTime = 0.0f;
	private float lidiDistance = 0.0f;
	private float lidiDisMaxCan = 2.0f;
	public float ForwardValue = 1;
	public float turnSpeed = 0.5f;
	float turnRadius = 1;
	void ApplySteering(bool canSteera, Vector3 relativeVelocity)
	{
		if(canSteera)
		{
			turnSpeed = 0.4f;
			turnRadius = 0.5f;

			if (pengle || nowSpeed < 5.0f)
			{
				truckTrans.Rotate(Vector3.up * 30.0f * steer * Time.deltaTime);
			}
			else
			{
				truckTrans.RotateAround( truckTrans.position + truckTrans.forward * turnRadius * ForwardValue, 
				                        truckTrans.up, 
				                        turnSpeed * Mathf.Rad2Deg * Time.deltaTime * steer);
			}

			if (truckTrans.eulerAngles.z < 340 && truckTrans.eulerAngles.z > 20)
			{
				if (truckTrans.eulerAngles.z < 180)
					truckTrans.eulerAngles = new Vector3(truckTrans.eulerAngles.x, truckTrans.eulerAngles.y, 20);
				else
					truckTrans.eulerAngles = new Vector3(truckTrans.eulerAngles.x, truckTrans.eulerAngles.y, 340);
			}

			Vector3 debugStartPoint = truckTrans.position + truckTrans.forward * turnRadius * ForwardValue;
			Vector3 debugEndPoint = debugStartPoint + Vector3.up * 5;
			
			Debug.DrawLine(debugStartPoint, debugEndPoint, Color.red);

			if(initialDragMultiplierX > dragMultiplier.x) // Handbrake
			{
				float rotationDirection = Mathf.Sign(steer); // rotationDirection is -1 or 1 by default, depending on steering
				if(steer == 0)
				{
					if(rigidbody.angularVelocity.y < 1) // If we are not steering and we are handbraking and not rotating fast, we apply a random rotationDirection
						rotationDirection = UnityEngine.Random.Range(-1.0f, 1.0f);
					else
						rotationDirection = rigidbody.angularVelocity.y; // If we are rotating fast we are applying that rotation to the car
				}
				// -- Finally we apply this rotation around a point between the cars front wheels.
				truckTrans.RotateAround( truckTrans.TransformPoint( (	frontWheels[0].localPosition + frontWheels[1].localPosition) * 0.5f), 
				                        truckTrans.up, 
				                        rigidbody.velocity.magnitude * Mathf.Clamp01(1 - rigidbody.velocity.magnitude / (topSpeedSecond * speedAdjust)) * rotationDirection * Time.deltaTime * 2);
			}
		}
		else
		{
			truckTrans.Rotate(Vector3.up * 30.0f * steer * Time.deltaTime);
			if (truckTrans.eulerAngles.z < 340 && truckTrans.eulerAngles.z > 20)
			{
				if (truckTrans.eulerAngles.z < 180)
					truckTrans.eulerAngles = new Vector3(truckTrans.eulerAngles.x, truckTrans.eulerAngles.y, 20);
				else
					truckTrans.eulerAngles = new Vector3(truckTrans.eulerAngles.x, truckTrans.eulerAngles.y, 340);
			}
		}
	}
	
	/**************************************************/
	/*               Utility Functions                */
	/**************************************************/
	
	bool HaveTheSameSign(float first, float second)
	{
		if (Mathf.Sign(first) == Mathf.Sign(second))
			return true;
		else
			return false;
	}
	
	float EvaluateSpeedToTurn(float speed)
	{
		if(speed > (topSpeedSecond * speedAdjust) / 2)
			return minimumTurn;
		
		float speedIndex = 1 - (speed / ((topSpeedSecond * speedAdjust) / 2));
		return minimumTurn + speedIndex * (maximumTurn - minimumTurn);
	}
	
	float EvaluateNormPower(float normPower)
	{
		if(normPower < 1)
			return 10 - normPower * 9;
		else
			return 1.9f - normPower * 0.9f;
	}
	
	float GetGearState()
	{
		Vector3 relativeVelocity = truckTrans.InverseTransformDirection(rigidbody.velocity);
		int lowLimit = (int)(currentGear == 0 ? 0 : gearSpeeds[currentGear-1]);
		return (relativeVelocity.z - lowLimit) / (gearSpeeds[currentGear - lowLimit]) * (1f - currentGear * 0.1f) + currentGear * 0.1f;
	}

	//about reset the position  ----  if has no distance with moving, or rotate in one point, or fall down for a long time
	public int jiaohuiIndex = 0;		//all the players(AI) will pass the jiaohuidian
	public int lahuiIndex = 0;		//the lahuidian will be different, but if the jiaohuidian is same, will judge lahuidian
	public int lahuiNumber = 0;		//if two round will be use
	public Transform lahuiObj = null;
	private Vector3 lahuiPos = Vector3.zero;
	public float resetDistance = 0;
	private float resetDistanceXZ = 0;
	public float resetDistanceY = 0;
	private float resetTimeTotal = 0;
	private float pressTimeTotal = 0;
	private Vector3 resetPosition = Vector3.zero;
	public  float curMoveDistance = 0;		//will calcute the distance with the same jiaohuidian and lahuidian
	private bool resetIsRecord = false;
	private Transform jiaohuiObj = null;

	private bool needJiance = true;

	void resetNewPoint(int index)
	{//return;
		lidiTime = 0;
		Debug.Log (index + " "+ transform + " here is reset tttttttttttttttttttttttttttttttttt " + lahuiObj + " "+ Time.time);
		if (!lahuiObj)
		{
			return;
		}

		if (isAutoMove && !needJiance)
		{
			return;
		}

		needJiance = false;

		bool isK = false;
		if (rigidbody.isKinematic)
		{
			isK = true;
			rigidbody.isKinematic = false;
		}

		if (Network.isClient)
		{
			truckTrans.networkView.RPC("changeGNORELayerRPC", RPCMode.AllBuffered);
		}

		ChangeLayersRecursively(gameObject.transform, "IGNORE");
		Invoke ("ChangeLayersAgain", 3);

		float randV = 0;

		if (Application.loadedLevel == 6)
		{
			randV = UnityEngine.Random.Range (-8.0f, 8.0f);
		}

		truckTrans.forward = lahuiObj.forward;
		truckTrans.position = new Vector3(lahuiObj.position.x + randV, lahuiPos.y + 1.0f, lahuiObj.position.z);
		truckTrans.rigidbody.velocity = Vector3.zero;
		currentEnginePower = 0;
		curMoveDistance = 0;
		resetAngleTotalQX = 0;
		
		if (isK && !isPlayerTruck)
		{
			rigidbody.isKinematic = true;
		}

		if (isAutoMove)
		{
			initYiXia(1.35f);
		}
		else if (isPlayerTruck)
		{
			pcvr.carSObj.aaa (0);
		}
	}

	void ChangeLayersAgain()
	{
		ChangeLayersRecursively(truckTrans.gameObject.transform, currentLayerCache);

		needJiance = true;
	}

	string currentLayerCache = "";
	void ChangeLayersRecursively(Transform trans, String LayerName)
	{
		trans.gameObject.layer = LayerMask.NameToLayer(LayerName);

		for (int i = 0; i < trans.childCount; i++)
		{
			trans.GetChild(i).gameObject.layer = LayerMask.NameToLayer(LayerName);
			ChangeLayersRecursively(trans.GetChild(i), LayerName);
		}
	}

	int nodeIndexT = -1;
	bool shouldPanduan = false;
	bool hasRigidbody = false;

	void OnTriggerEnter( Collider other )
	{//Debug.Log ("OnTri1gger1Enter " + transform + " " + other.gameObject + " " + other.gameObject.name + " "+ other.transform.position + " "+ other.transform.root.gameObject.layer + " "+ numLayerCar + " "+ numLayerTerrain + " "+ dimianLayer.value);
		if (TGameState == TruckGameState.notMoving
		    || AIFinished
		    || pcvr.uiRunState == 10
		    || pcvr.isPassgamelevel)
		{
			return;
		}

		if (Network.isServer && (!isAutoMove || isPlayerTruck))
		{
			return;
		}
		else if (Network.isClient && (isAutoMove || !isPlayerTruck))
		{
			return;
		}

		if (other.transform.root.gameObject.layer == numLayerCar || other.transform.root.gameObject.layer == numLayerTerrain
		    || other.gameObject.layer == numLayerCar || other.gameObject.layer == numLayerTerrain )
		{
			return;
		}

		shouldPanduan = false;

		//if is AI path node trigger, will return
		if (other.transform.GetComponent<triTruckInfor>()
		    && other.transform.GetComponent<triTruckInfor>().getTriggerName().CompareTo("AINodeTri") == 0)
		{
			if (isAutoMove && curPathObj == other.transform.parent)
			{
				nodeIndexT = -1;
				nodeIndexT = Convert.ToInt32(other.name.Substring(5,2));
				//Debug.Log("ainodeddddddd  " + transform + " "+ nodeIndexT + " " + curPathChildNum + " "+ curPathIndex);
				if (nodeIndexT >= 0)
				{
					//judge and change  ----  whether need to find next path?
					if (nodeIndexT < curPathChildNum - 1)
					{
						curPathPoint = curPathChildArr[nodeIndexT + 1];
						findNextPos (curPathPoint);
					}
					else
					{
						if (findNextPathInfor(++ curPathIndex))
						{
							curPathPoint = curPathChildArr[0];
							findNextPos (curPathPoint);
						}
					}
				}
			}
			return;
		}

		if (other.transform.GetComponent<triTruckInfor>()
		    && other.transform.GetComponent<triTruckInfor>().getTriggerName().CompareTo("laHuiDianTri") == 0)
		{//only record the point information as the last valid position
			if (lahuiObj == other.transform)
			{
				return;
			}

			lahuiObj = other.transform;		//this only a point and reset the forwad
			lahuiPos = truckTrans.position;
			//other.transform.collider.enabled = false;
			curMoveDistance = 0;
			lahuiIndex ++;
			lahuiNumber ++;

			return;
		}
		else if (other.transform.GetComponent<triTruckInfor>()
		     && other.transform.GetComponent<triTruckInfor>().getTriggerName().CompareTo("jiaoHuiDianTri") == 0)
		{//if has more than one path
			if (jiaohuiObj == other.transform)
			{
				return;
			}

			jiaohuiObj = other.transform;
			jiaohuiIndex ++;
			lahuiIndex = 0;
			
			return;
		}
		else if (other.transform.GetComponent<triTruckInfor>()
		     && other.transform.GetComponent<triTruckInfor>().getTriggerName().CompareTo("addSpeedTri") == 0)
		{//speeding
			if (isAutoMove)
			{
				return;
			}
			
			if (daojuLastTime < 0)
			{
				daojuLastTime = Time.time;
			}
			else if (Time.time - daojuLastTime <= daojuTime)
			{
				return;
			}

			/*if (pcvr.danqitishiSObj.danqiNum >= pcvr.danqitishiSObj.danqiMaxNum)
			{
				return;
			}*/

			other.transform.collider.enabled = false;

			//if (pcvr.danqitishiSObj)
			//	pcvr.danqitishiSObj.getOneDanqi();
			speedingHere();
			
			playAudioHit (11);
			
			daojuLastTime = Time.time;
			if (Network.isClient)
			{
				GameObject Tobject = (GameObject)Network.Instantiate(m_DaojuParticle, other.transform.position + other.transform.forward * m_BaozhaForward + Vector3.up * m_BaozhaUp, other.transform.rotation, 0);

				destoryNetObj(Tobject);
				Destroy(other.gameObject);
			}
			else
			{
				GameObject Tobject = (GameObject)Instantiate(m_DaojuParticle, other.transform.position + other.transform.forward * m_BaozhaForward + Vector3.up * m_BaozhaUp, other.transform.rotation);
				Destroy(Tobject, 2.0f);
				
				Destroy(other.gameObject);
			}

			return;
		}
		else if (other.transform.GetComponent<triTruckInfor>()
			&& other.transform.GetComponent<triTruckInfor>().getTriggerName().CompareTo("NPCSpawnTri") == 0)
		{//spawn NPC
			if (isAutoMove)
			{
				return;
			}

			//isClient or not -- linkMode
			if (!Network.isClient)
			{//single mode
				other.transform.collider.enabled = false;
				other.transform.GetComponent<spawnTriggerScript>().BeginSpawn();
				//Destroy(other.gameObject);
			}
			else
			{//is client, will send message to server to spawn npc
				other.transform.collider.enabled = false;
				//Destroy(other.gameObject);
				
				pcvr.selfServerScrObj.spawnReceive(other.name);
			}
			
			return;
		}
		else if (other.transform.GetComponent<triTruckInfor>()
			&& other.transform.GetComponent<triTruckInfor>().getTriggerName().CompareTo("NPCDeleteTri") == 0)
		{//delete NPC
			if (isAutoMove)
			{
				return;
			}

			//isClient or not -- linkMode
			if (!Network.isClient)
			{//single mode
				other.transform.collider.enabled = false;
				other.transform.GetComponent<deleteTriggerScript>().BeginDelete();
				//Destroy(other.gameObject);
			}
			else
			{//is client
				other.transform.collider.enabled = false;
				//Destroy(other.gameObject);
				
				pcvr.selfServerScrObj.deleteReceive(other.name);
			}
			
			return;
		}
		else if (other.transform.GetComponent<triTruckInfor>()
		    && (other.transform.GetComponent<triTruckInfor>().getTriggerName().CompareTo("addTimeTri") == 0
		    || other.transform.GetComponent<triTruckInfor>().getTriggerName().CompareTo("addTime2Tri") == 0))
		{//add time
			if (isAutoMove || Network.isClient)
			{
				return;
			}

			if (daojuLastTime < 0)
			{
				daojuLastTime = Time.time;
			}
			else if (Time.time - daojuLastTime <= daojuTime)
			{
				return;
			}

			other.transform.collider.enabled = false;

			if(other.transform.GetComponent<triTruckInfor>().getTriggerName().CompareTo("addTimeTri") == 0)
			{
				pcvr.UITruckScrObj.AddGameTime(1);
				playAudioHit (10);
			}
			else if(other.transform.GetComponent<triTruckInfor>().getTriggerName().CompareTo("addTime2Tri") == 0)
			{
				pcvr.UITruckScrObj.AddGameTime(2);
				playAudioHit (10);
			}

			daojuLastTime = Time.time;
			if (Network.isClient)
			{
				GameObject Tobject = (GameObject)Network.Instantiate(m_DaojuParticle, other.transform.position + other.transform.forward * m_BaozhaForward + Vector3.up * m_BaozhaUp, other.transform.rotation, 0);

				destoryNetObj(Tobject);
				Network.Destroy(other.gameObject);
			}
			else
			{
				GameObject Tobject = (GameObject)Instantiate(m_DaojuParticle, other.transform.position + other.transform.forward * m_BaozhaForward + Vector3.up * m_BaozhaUp, other.transform.rotation);
				Destroy(Tobject, 2.0f);
				
				Destroy(other.gameObject);
			}
			
			return;
		}
		else if (other.transform.GetComponent<triTruckInfor>()
			&& other.transform.GetComponent<triTruckInfor>().getTriggerName().CompareTo("openLightTri") == 0)
		{
			if (isAutoMove)
			{
				return;
			}

			if (other.transform.GetComponent<OpenLight>())
			{
				other.transform.GetComponent<OpenLight>().OpenLightAndPar();
			}
			return;
		}
		else if (other.transform.GetComponent<triTruckInfor>()
			&& other.transform.GetComponent<triTruckInfor>().getTriggerName().CompareTo("DengTri") == 0)
		{shouldPanduan = true;
			for(int i = 0; i < other.transform.childCount; i++)
			{
				if (other.transform.GetChild(i).GetComponent<Light>())
				{
					other.transform.GetChild(i).GetComponent<Light>().enabled = false;
				}
			}

			pengle = true;
			pengleTime = pengleTotalTime;
			
			ChangeLayersRecursively(other.transform, "IGNORE");
		}
		else if (other.transform.GetComponent<triTruckInfor>()
		         && other.transform.GetComponent<triTruckInfor>().getTriggerName().CompareTo("tishiTri") == 0
		         && other.transform.GetComponent<tishiTrigger>())
		{//tishi
			if (isAutoMove)
			{
				return;
			}

			if (tishiLastTime < 0)
			{
				tishiLastTime = Time.time;
			}
			else if (Time.time - tishiLastTime <= 0.3f)
			{
				return;
			}

			tishiLastTime = Time.time;
			//other.transform.collider.enabled = false;
			pcvr.UITruckScrObj.showTishiObj(other.transform.GetComponent<tishiTrigger>().getSpriteIndex(), other.transform.GetComponent<tishiTrigger>().getShowTime());
			//Destroy(other.gameObject);

			if (pcvr.sound2DScrObj)
			{
				pcvr.sound2DScrObj.playAudioLubiaotishi(true);
			}
			
			return;
		}
		else if (other.transform.GetComponent<triTruckInfor>()
		         && other.transform.GetComponent<triTruckInfor>().getTriggerName().CompareTo("qinangQianTri") == 0)
		{//qinang qian
			if (isAutoMove)
			{
				return;
			}

			other.transform.collider.enabled = false;
			forwardBackStateQN = 2;
			//Destroy(other.gameObject);
			
			return;
		}
		else if (other.transform.GetComponent<triTruckInfor>()
		         && other.transform.GetComponent<triTruckInfor>().getTriggerName().CompareTo("qinangHouTri") == 0)
		{//qinang hou
			if (isAutoMove)
			{
				return;
			}
			
			other.transform.collider.enabled = false;
			forwardBackStateQN = 1;
			//Destroy(other.gameObject);
			
			return;
		}
		else if (other.transform.GetComponent<triTruckInfor>()
		         && other.transform.GetComponent<triTruckInfor>().getTriggerName().CompareTo("qinangJieshuTri") == 0)
		{//qinang stop
			if (isAutoMove)
			{
				return;
			}
			
			other.transform.collider.enabled = false;
			forwardBackStateQN = 0;

			if (TGameState == TruckGameState.addSpeed)
			{
				CancelInvoke("speedingQNControl");
				InvokeRepeating("speedingQNControl", 0, 0.08f);
			}

			//Destroy(other.gameObject);
			
			return;
		}
		else if (other.transform.GetComponent<triTruckInfor>()
			&& other.transform.GetComponent<triTruckInfor>().getTriggerName().CompareTo("stopFollowTri") == 0)
		{
			if (pcvr.XKCarCameraSObj && isPlayerTruck)
			{
				isStopFollowC = true;
				stopFollowCamera();
				CancelInvoke("followAgain");
				Invoke ("followAgain", stopFollowTime);
			}
			return;
		}
		else if (other.transform.GetComponent<triTruckInfor>()
			&& other.transform.GetComponent<triTruckInfor>().getTriggerName().CompareTo("enterGongluTri") == 0)
		{
			if (isPlayerTruck && pcvr.carSObj && !isInGonglu)
			{
				pcvr.carSObj.changeTopSpeedGongluTulu (true, 1);
			}

			isGongluDuan = true;
			isInGonglu = true;
			controlChentu(false, 3);
			return;
		}
		else if (other.transform.GetComponent<triTruckInfor>()
			&& other.transform.GetComponent<triTruckInfor>().getTriggerName().CompareTo("leaveGongluTri") == 0)
		{
			if (isPlayerTruck && pcvr.carSObj && isInGonglu)
			{
				pcvr.carSObj.changeTopSpeedGongluTulu (false, 2);
			}

			isGongluDuan = false;
			isInGonglu = false;
			controlChentu(true, 4);
			return;
		}
		else if (other.transform.GetComponent<triTruckInfor>()
		         && other.transform.GetComponent<triTruckInfor>().getTriggerName().CompareTo("enterWaterTri") == 0)
		{
			if (isAutoMove)
			{
				return;
			}

			isInwater = true;

			if (pcvr.heatDistortSObj)
			{
				pcvr.heatDistortSObj.InitPlayScreenWater();
			}

			fangxiangpanDoudongIndex = 1;
			controlWaterTruck(true, 3);
			return;
		}
		else if (other.transform.GetComponent<triTruckInfor>()
		         && other.transform.GetComponent<triTruckInfor>().getTriggerName().CompareTo("leaveWaterTri") == 0)
		{
			if (isAutoMove)
			{
				return;
			}

			isInwater = false;
			fangxiangpanDoudongIndex = 3;
			controlWaterTruck(false, 4);
			return;
		}
		else if (other.transform.GetComponent<triTruckInfor>()
		         && other.transform.GetComponent<triTruckInfor>().getTriggerName().CompareTo("enterSuishiluTri") == 0)
		{
			if (isAutoMove)
			{
				return;
			}

			isInSuishiLu = true;
			fangxiangpanDoudongIndex = 2;
			return;
		}
		else if (other.transform.GetComponent<triTruckInfor>()
		         && other.transform.GetComponent<triTruckInfor>().getTriggerName().CompareTo("leaveSuishiluTri") == 0)
		{
			if (isAutoMove)
			{
				return;
			}

			isInSuishiLu = false;
			fangxiangpanDoudongIndex = 1;
			return;
		}
		else if (other.transform.GetComponent<triTruckInfor>()
		         && other.transform.GetComponent<triTruckInfor>().getTriggerName().CompareTo("enterShadiLuTri") == 0)
		{
			if (isAutoMove)
			{
				return;
			}

			isInShadiLu = true;
			fangxiangpanDoudongIndex = 0;
			return;
		}
		else if (other.transform.GetComponent<triTruckInfor>()
		         && other.transform.GetComponent<triTruckInfor>().getTriggerName().CompareTo("leaveShadiLuTri") == 0)
		{
			if (isAutoMove)
			{
				return;
			}

			isInShadiLu = false;
			fangxiangpanDoudongIndex = 1;
			return;
		}
		else if (other.transform.GetComponent<triTruckInfor>()
		          && other.transform.GetComponent<triTruckInfor>().getTriggerName().CompareTo("daojuLiTri") == 0)
		{//daoju
			if (isAutoMove)
			{
				return;
			}
			//playAudioHit (3);
			//other.transform.collider.enabled = false;
			ChangeLayersRecursively(other.transform, "IGNORE");
			
			//forward - small NPC will fly out
			other.transform.root.GetComponent<daojuPengzhuang>().daojuPengshangle(truckTrans.forward, false, chetouPoint.position);
			
			pengle = true;
			pengleTime = pengleTotalTime;
			
			return;
		}
		else if ((other.transform.GetComponent<triTruckInfor>()
			&& other.transform.GetComponent<triTruckInfor>().getTriggerName().CompareTo("NPCSmallTri") == 0)
		    || (other.transform.root.GetComponent<triTruckInfor>()
		    && other.transform.root.GetComponent<triTruckInfor>().getTriggerName().CompareTo("NPCSmallTri") == 0))
		{//small NPC
			if (Time.time - m_HitTimmer < 0.1f && m_HitTimmer > 0)
			{
				return;
			}
			//ChangeLayersRecursively(other.transform, "IGNORE");

			m_HitTimmer = Time.time;

			findOtherIndex = other.transform.root.GetComponent<NPCControl>().getHitDirection(truckTrans.position);
			curDirectionIndex = getHitDirection(other.transform.position);
			//playAudioHit (3);	//20170714 will change here

			//forward - small NPC will fly out
			other.transform.root.GetComponent<NPCControl>().hitByPlayer(truckTrans.forward);

			other.transform.root.GetComponent<NPCControl>().addDaojuPengzhuang(truckTrans.forward, true, chetouPoint.position);

			//other.transform.root.gameObject.AddComponent<daojuPengzhuang>();

			//other.transform.root.GetComponent<daojuPengzhuang>().daojuPengshangle(truckTrans.forward, true, chetouPoint.position);

			if (isPlayerTruck)
			{
				return;
			}

			pengle = true;
			pengleTime = pengleTotalTime;
			
			return;
		}
		else if (other.transform.GetComponent<triTruckInfor>()
			&& other.transform.GetComponent<triTruckInfor>().getTriggerName().CompareTo("serverFollowTriTri") == 0)
		{//Debug.Log("follo***  ");
			//follow player - only for link mode
			if (!Network.isClient)
			{
				return;
			}
			other.transform.collider.enabled = false;

			//pcvr.selfServerScrObj.FollowServerH(other.gameObject.name, other.transform.GetComponent<followLinkScript>().getIndex(), pcvr.selfIndex);
			pcvr.selfServerScrObj.FollowServerH(other.gameObject.name, UnityEngine.Random.Range (0, 5), pcvr.selfIndex);
			return;
		}
		else if (other.transform.GetComponent<triTruckInfor>()
			&& other.transform.GetComponent<triTruckInfor>().getTriggerName().CompareTo("serverPointTriTri") == 0)
		{//Debug.Log("innnnnnnnnnnnnnnnnnnnnnnn  " + Time.time + " "+ gameObject + " " + other.gameObject.name);
			//at point - enter - only for link mode
			//can't hide or delete here, into and leave the trigger as the same trigger box
			if (!Network.isClient)
			{
				return;
			}
			other.transform.collider.enabled = false;

			pcvr.selfServerScrObj.PointServerH(other.gameObject.name, other.transform.GetComponent<pointLinkScript>().getPointName(), other.transform.GetComponent<pointLinkScript>().getIslookat(), pcvr.selfIndex);
			return;
		}
		else if ((other.transform.GetComponent<triTruckInfor>()
			&& other.transform.GetComponent<triTruckInfor>().getTriggerName().CompareTo("PlayerTri") == 0)
			|| (other.transform.root.GetComponent<triTruckInfor>()
		    && other.transform.root.GetComponent<triTruckInfor>().getTriggerName().CompareTo("PlayerTri") == 0))
		{
			//hit other player
			if (isPlayerTruck)
			{
				playAudioHit (3);
				//Debug.Log("getHitDirection(truckTrans.position)    " + getHitDirection(other.transform.position) + " "+ nowSpeed);
				//forward and speed
				/*if (getHitDirection(other.transform.position) == 1 && nowSpeed > 10.0f)
				{
					if (Network.isClient)
					{
						other.transform.root.GetComponent<gameServerObjSript>().hitByPlayerPlayer(truckTrans.forward, nowSpeed);
					}
					else
					{
						other.transform.root.GetComponent<truck>().hitByPlayerPlayer(truckTrans.forward, nowSpeed);
					}
				}*/
			}
			/*else
			{
				rigidbody.isKinematic = true;
			}*/

			pengle = true;
			penglePlayer = true;
			pengleTime = pengleTotalTime;
			return;
		}
		else if (other.transform.GetComponent<triTruckInfor>()
			&& other.transform.GetComponent<triTruckInfor>().getTriggerName().CompareTo("zhongdianTri") == 0
		   	&& lahuiNumber >= 10)
		{
			GameEndOrPassCloseYan ();

			if (Network.isServer)
			{
				//only for AI
				if (isAutoMove)
				{
					if (!pcvr.aFirstScriObj.isPaoLiangquan/* || isGuoleZhongdian*/ || (curPaoquanNumber >= pcvr.aFirstScriObj.paoJiquan - 1))
					{
						if (AIIndex >= 0)
						{//AI
							jiaohuiIndex = (4 - pcvr.finishedNumber) * 100;
							pcvr.jiaohuiPointArr[AIIndex] = jiaohuiIndex;
						}

						AIFinished = true;
						AIEndGameLink(AIIndex, false);
						pcvr.UITruckScrObj.passLevelHereLink(AIIndex);
					}
					else
					{
						curPathIndex = -1;
						isGuoleZhongdian = true;
						curPaoquanNumber ++;
					}
				}
			}
			else if (Network.isClient)
			{
				if (!pcvr.aFirstScriObj.isPaoLiangquan/* || isGuoleZhongdian*/ || (curPaoquanNumber >= pcvr.aFirstScriObj.paoJiquan - 1))
				{
					if (networkView)
					{
						jiaohuiIndex = (4 - pcvr.finishedNumber) * 100;
						networkView.RPC("SendMoveInforRPC", RPCMode.All, pcvr.selfIndex, curMoveDistance, jiaohuiIndex, lahuiIndex);
					}

					//only for player
					pcvr.uiRunState = 10;	//will send message to server
					other.gameObject.SetActive(false);

					speedEndHere();
					AIEndGameLink(pcvr.selfIndex, true);
					pcvr.UITruckScrObj.passLevelHereLink(pcvr.selfIndex);
					
					if (pcvr.sound2DScrObj)
					{
						pcvr.sound2DScrObj.playAudioZhongdian();
					}

					pcvr.GetInstance().initEndGamele();
				}
				else
				{
					isGuoleZhongdian = true;
					curPaoquanNumber ++;
					pcvr.UITruckScrObj.clearAllNPCObj();
					
					/*pcvr.UITruckScrObj.showZuihouyiquan();
					
					if (pcvr.sound2DScrObj)
					{
						pcvr.sound2DScrObj.playAudioZuihouyiquan (true);
					}*/

					pcvr.aFirstScriObj.resetSomeTriInfor(true);
				}
			}
			else
			{//single mode
				if (isAutoMove)
				{
					if (!pcvr.aFirstScriObj.isPaoLiangquan/* || isGuoleZhongdian*/ || (curPaoquanNumber >= pcvr.aFirstScriObj.paoJiquan - 1))
					{
						if (AIIndex >= 0)
						{//AI
							jiaohuiIndex = (4 - pcvr.finishedNumber) * 100;
							pcvr.jiaohuiPointArr[AIIndex] = jiaohuiIndex;
						}

						AIFinished = true;
						AIEndGameZhongdian();

						pcvr.indexOnlyFinal[pcvr.finishedNumber] = AIIndex;
						pcvr.finishedNumber ++;
					}
					else
					{
						curPathIndex = -1;
						isGuoleZhongdian = true;
						curPaoquanNumber ++;
					}
				}
				else
				{
					if (!pcvr.aFirstScriObj.isPaoLiangquan/* || isGuoleZhongdian*/ || (curPaoquanNumber >= pcvr.aFirstScriObj.paoJiquan - 1))
					{
						jiaohuiIndex = (4 - pcvr.finishedNumber) * 100;
						pcvr.jiaohuiPointArr[pcvr.selfIndex] = jiaohuiIndex;


						pcvr.uiRunState = 10;
						other.gameObject.SetActive(false);

						speedEndHere();
						
						pcvr.indexOnlyFinal[pcvr.finishedNumber] = pcvr.selfIndex;
						pcvr.finishedNumber ++;

						pcvr.UITruckScrObj.passLevelHere();
						
						if (pcvr.sound2DScrObj)
						{
							pcvr.sound2DScrObj.playAudioZhongdian();
						}

						pcvr.GetInstance().initEndGamele();
					}
					else
					{
						isGuoleZhongdian = true;
						curPaoquanNumber ++;
						pcvr.UITruckScrObj.clearAllNPCObj();
						Debug.Log("pppppppppppppppppppppppppppppppppppppppppppppppp   " + curPaoquanNumber + " "+ pcvr.aFirstScriObj.paoJiquan);
						/*pcvr.UITruckScrObj.showZuihouyiquan();
						
						if (pcvr.sound2DScrObj)
						{
							pcvr.sound2DScrObj.playAudioZuihouyiquan (true);
						}*/

						pcvr.aFirstScriObj.resetSomeTriInfor(false);
					}
				}
			}
			lahuiNumber = 0;
			return;
		}
		else
		{shouldPanduan = true;
			pengleTime = pengleTotalTime;
			pengle = true;
			//playAudioHit (2);
		}

		if (shouldPanduan)
		{
			hasRigidbody = false;
			if (other.transform.GetComponent<Rigidbody>())
			{
				hasRigidbody = true;
			}
			else
			{
				foreach(Transform child in other.transform)
				{
					if (child.GetComponent<Rigidbody>())
					{
						hasRigidbody = true;
						break;
					}
				}
			}

			if (hasRigidbody)
			{
				ChangeLayersRecursively(other.transform, "IGNORE");

				other.gameObject.AddComponent<daojuPengzhuang>();
				
				//forward - small NPC will fly out
				if (isAutoMove)
					other.transform.GetComponent<daojuPengzhuang>().daojuPengshangle(truckTrans.forward, false, truckTrans.position);
				else
					other.transform.GetComponent<daojuPengzhuang>().daojuPengshangle(truckTrans.forward, false, chetouPoint.position);
			}
		}
	}

	void AIEndGameZhongdian()
	{
		if (isAutoMove && AIIndex >= 0)
		{//AI
			pcvr.useTimeSelf[AIIndex] = pcvr.totalTime;
		}
	}

	void GameEndOrPassCloseYan()
	{
		controlChentu(false, 1010);	//or water
		controlYan(false, 1010);
		
		if (isInwater)
		{
			controlWaterTruck(false, 1010);
		}
	}

	void AIEndGameLink(int indexPlyerAi, bool isPlayer)
	{
		truckTrans.networkView.RPC("AIEndGameLinkRPC", RPCMode.AllBuffered, indexPlyerAi, isPlayer, Network.player);
	}

	private bool isInGonglu = true;
	private bool isGongluDuan = true;
	
	//smoke
	public ParticleSystem[] yan;
	private int yanNumber = 0;
	private bool isActiveYan = false;
	
	//dust
	public ParticleSystem[] chenTu;
	private int chenTuNumber = 0;
	private bool isActiveChentu = false;
	
	//water
	public ParticleSystem[] waterTruck;
	private int waterTruckuNumber = 0;
	public bool isInwater = false;
	
	//xieqi smoke
	public ParticleSystem[] yanXieqi;
	private int yanNumberXieqi = 0;

	//penhuo
	public ParticleSystem penhuoLizi;

	public int fangxiangpanDoudongIndex = 1;
	public bool isInSuishiLu = false;
	public bool isInShadiLu = false;

	void OnTriggerStay( Collider other )
	{//Debug.Log ("OnTrig1gerStayOnTr1iggerStayOnTr1iggerStay " + transform + " " + other.gameObject + " " + other.gameObject.name + " "+ other.transform.position + " "+ other.transform.root.gameObject.layer + " "+ numLayerCar + " "+ numLayerTerrain);
		if (TGameState == TruckGameState.notMoving
		    || AIFinished
		    || pcvr.uiRunState == 10
		    || pcvr.isPassgamelevel)
		{
			return;
		}
		
		if (Network.isServer && (!isAutoMove || isPlayerTruck))
		{
			return;
		}
		else if (Network.isClient && (isAutoMove || !isPlayerTruck))
		{
			return;
		}

		if (other.transform.root.gameObject.layer == numLayerCar || other.transform.root.gameObject.layer == numLayerTerrain
		    || other.gameObject.layer == numLayerCar || other.gameObject.layer == numLayerTerrain )
		{
			//Debug.Log ("O11nTerOnReturn " + other.gameObject + " " + other.gameObject.name + " "+ other.transform.position );
			
			if(chetouPoint && Physics.Raycast(chetouPoint.position, -Vector3.up, out hit, 50.0f, dimianLayer))
			{
				if (hit.transform.gameObject.name.CompareTo("ff") == 0 || hit.transform.root.gameObject.name.CompareTo("ff") == 0)
				{
					if (isInGonglu)
					{
						//tulu
						if (isPlayerTruck && pcvr.carSObj && isInGonglu)
						{
							pcvr.carSObj.changeTopSpeedGongluTulu (false,3);
						}
						isInGonglu = false;
						controlChentu(true, 6);
					}
				}
				else if (!isInGonglu)
				{
					//gonglu
					if (isPlayerTruck && pcvr.carSObj && !isInGonglu)
					{
						pcvr.carSObj.changeTopSpeedGongluTulu (true,4);
					}
					isInGonglu = true;
					controlChentu(false, 2);
				}
			}
			return;
		}
	}

	void changeToSecond(float speed)
	{
		topSpeedSecond = speed * 0.278f;
	}

	public Transform chetouPoint;
	public Transform[] pointArr;		//the points( may be 4 points) on the AI
	private int pointArrLength = 0;		//the total number of the points on the AI
	private int findIndex = 0;
	private int findOtherIndex = 0;	//the other who collide with self ---- direction
	private float findDistance = 0;
	private int curDirectionIndex = 0;

	private float m_HitTimmer = 0.0f;	//the add force time
	//private int IsHitState = 0;			//whether be hitted --- 0-not hit, 1-hit and move away, 2-hit but not move away
	//hit here
	//1-forward; 2-back; 3-left; 4-right
	public int getHitDirection(Vector3 hitPoint)
	{//Debug.Log ("getHitDirection  " + hitPoint + " "+ pointArrLength);
		findIndex = -1;
		
		for (int i = 0; i < pointArrLength; i++)
		{
			if (findIndex < 0)
			{
				findIndex = i + 1;
				findDistance = Vector3.Distance(hitPoint, pointArr[i].transform.position);
			}
			else if (findDistance > Vector3.Distance(hitPoint, pointArr[i].transform.position))
			{
				findIndex = i +1;
				findDistance = Vector3.Distance(hitPoint, pointArr[i].transform.position);
			}
		}
		
		return findIndex;
	}

	//isSpeeing - false - the speed is more than a high value
	//isSpeeing - true - add speed
	public void speedingHere()
	{//Debug.Log ("speedin1gHere   " + TGameState + " "+ isInGonglu);
		if (TGameState == TruckGameState.addSpeed)
		{
			playAudioHit (1);
			jiasuCurTime = jiasuTotalTime;
			return;
		}

		if (jiasuTotalTime < 1.0f)
		{
			jiasuTotalTime = 1.0f;
		}

		jiasuCurTime = jiasuTotalTime;
		TGameState = TruckGameState.addSpeed;
		playAudioHit (1);
		//playAudioMoveStop (3);

		pcvr.carSObj.changeTorqueNew (isInGonglu);

		CancelInvoke("kaishileSpeed");
		InvokeRepeating("kaishileSpeed", 0.15f, 0.03f);
	}

	void kaishileSpeed()
	{//Debug.Log ("kaishiSpeeddddddddddddddddddddddddddddddd  " + Time.time);
		if (TGameState != TruckGameState.addSpeed)
		{
			CancelInvoke("kaishileSpeed");
			return;
		}

		if (!pcvr.XKCarCameraSObj.isPart2)
		{
			return;
		}
		else
		{
			CancelInvoke("kaishileSpeed");
		}
		Debug.Log ("kaishiSpeeddddddddddddddddddddddddddddddd aaaaa " + Time.time);
		if (pcvr.RadialBlurSObj)
		{
			pcvr.RadialBlurSObj.setJiasu();
		}
		
		pcvr.aFirstScriObj.openFastBloom ();
		
		chongqiTime = Time.time;
		isLeftChongqi = false;
		isUseNormal = true;
		CancelInvoke("speedingQNControl");
		InvokeRepeating("speedingQNControl", 0, 0.08f);
		
		pcvr.m_IsOpneForwardQinang = true;

		controlPenhuoLizi (true);
	}

	public void speedEndHere()
	{//Debug.Log ("speedin1gHereaaaaa   " + TGameState + " "+ isInGonglu);
		//pcvr.danqitishiSObj.SpeedingEnd ();
		CancelInvoke("speedingQNControl");

		pcvr.carSObj.changeTorqueHui (isInGonglu);
		pcvr.m_IsOpneForwardQinang = false;

		if (audioMoveStopState == 3)
		{
			playAudioMoveStop (2);
		}

		controlPenhuoLizi (false);
	}

	public void speedingHereAnotherVersion()
	{//Debug.Log ("speedin1gHere   " + TGameState + " "+ isInGonglu);
		
		pcvr.aFirstScriObj.openFastBloom ();
		
		if (TGameState == TruckGameState.addSpeed)
		{
			playAudioHit (1);
			jiasuCurTime = jiasuTotalTime;
			return;
		}
		
		chongqiTime = Time.time;
		isLeftChongqi = false;
		isUseNormal = true;
		CancelInvoke("speedingQNControl");
		InvokeRepeating("speedingQNControl", 0, 0.08f);
		
		if (jiasuTotalTime < 1.0f)
		{
			jiasuTotalTime = 1.0f;
		}
		
		jiasuCurTime = jiasuTotalTime;
		TGameState = TruckGameState.addSpeed;
		playAudioHit (1);
		//playAudioMoveStop (3);
		
		pcvr.carSObj.changeTorqueNew (isInGonglu);
		
		pcvr.m_IsOpneForwardQinang = true;
		
		controlPenhuoLizi (true);
	}

	private float chongqiTime = -1.0f;
	private bool isLeftChongqi = false;
	private bool isUseNormal = true;

	void speedingQNControl()
	{
		isLeftChongqi = !isLeftChongqi;

		if (pcvr.uiRunState != 2 || forwardBackStateQN != 0)
		{
			CancelInvoke("speedingQNControl");
			return;
		}

		if (!isUseNormal)
		{
			if(isLeftChongqi)
			{
				pcvr.GetInstance ().ControlQinang (1);
				pcvr.GetInstance ().ControlQinang (7);
			}
			else
			{
				pcvr.GetInstance ().ControlQinang (6);
				pcvr.GetInstance ().ControlQinang (2);
			}
		}
	}

	public void YanHereBegin()
	{
		if (!isActiveYan)
			controlYan(true, 33);
	}

	public void YanHereEnd()
	{
		if (isActiveYan)
			controlYan(false, 22);
	}

	//about yan1 and chentu
	void controlYan(bool isActive, int index)
	{//Debug.Log ("truck    controlYan   " + index + " "+ isActive);
		isActiveYan = isActive;

		if (Network.isServer || Network.isClient)
		{
			truckTrans.networkView.RPC("controlYanRPC", RPCMode.AllBuffered, isActive, index);
			return;
		}

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
	
	void controlChentu(bool isActive, int index)
	{//Debug.Log (transform + " truck    control1Chentu   " + index + " "+ isActive + " " +chenTuNumber);
		isActiveChentu = isActive;

		if (Network.isServer || Network.isClient)
		{
			truckTrans.networkView.RPC("controlChentuRPC", RPCMode.AllBuffered, isActive, index);
			return;
		}

		if (isActive)
		{
			for (int i=0; i < chenTuNumber; i++)
			{
				chenTu[i].Play ();
			}
		}
		else
		{
			for (int i=0; i < chenTuNumber; i++)
			{
				chenTu[i].Stop ();
			}
		}
	}

	void controlWaterTruck(bool isActive, int index)
	{
		if (index != 970)
		controlChentu (!isActive, index);
		isActiveChentu = isActive;
		if (Network.isServer || Network.isClient)
		{
			truckTrans.networkView.RPC("controlWaterTruckRPC", RPCMode.AllBuffered, isActive, index);
			return;
		}
		
		if (isActive)
		{
			for (int i=0; i < waterTruckuNumber; i++)
			{
				waterTruck[i].Play ();
			}
		}
		else
		{
			for (int i=0; i < waterTruckuNumber; i++)
			{
				waterTruck[i].Stop ();
			}
		}
	}
	
	//about yan2 and chentu
	void controlYanXieqi(bool isActive, int index)
	{//Debug.Log ("truck    controlYanXieqi   " + index + " "+ isActive);
		
		if (Network.isServer || Network.isClient)
		{
			truckTrans.networkView.RPC("controlYanXieqiRPC", RPCMode.AllBuffered, isActive, index);
			return;
		}
		
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

	void controlPenhuoLizi(bool isActive)
	{//Debug.Log ("truck    controlYanXieqi   " + index + " "+ isActive);
		
		if (Network.isClient)
		{
			truckTrans.networkView.RPC("controlPenhuoLiziRPC", RPCMode.AllBuffered, isActive);
			return;
		}
		
		if (isActive && penhuoLizi)
		{
			penhuoLizi.Play();
		}
		else if (penhuoLizi)
		{
			penhuoLizi.Stop();
		}
	}

	//judge the mingci-----whether need to adjust the speed
	//compare with the first-player(maybe is not the first one----fastest)
	//faster than:0-0.55f; 1-0.7f; 2-0.8f;
	//lower than:3-1.15f; 2-1.1f; 1-1.05f;
	void adjustSpeedHereLevel2()
	{
		if (isAdjusting)
		{
			return;
		}

		if (mingciSelf > pcvr.mingciFirstPlayer)
		{
			//lower
			if (mingciSelf == 1)
			{
				speedAdjust = 1.6f;
			}
			else if (mingciSelf == 2)
			{
				speedAdjust = 1.6f;
			}
			else if (mingciSelf == 3)
			{
				speedAdjust = 1.6f;
			}
		}
		else if (mingciSelf < pcvr.mingciFirstPlayer)
		{
			if (nowSpeed <= AdjustSpeedShould)
			{
				return;
			}

			//faster
			if (mingciSelf == 0)
			{
				speedAdjust = 1f;
			}
			else if (mingciSelf == 1)
			{
				speedAdjust = 1f;
			}
			else if (mingciSelf == 2)
			{
				speedAdjust = 1f;
			}
		}
		
		SetupGears();
		initialDragMultiplierX = dragMultiplier.x;
	}

	//cheng shi
	void adjustSpeedHereLevel1()
	{
		if (isAdjusting)
		{
			return;
		}
		
		if (mingciSelf > pcvr.mingciFirstPlayer)
		{
			//lower
			if (mingciSelf == 1)
			{
				speedAdjust = 1.5f;
			}
			else if (mingciSelf == 2)
			{
				speedAdjust = 1.65f;
			}
			else if (mingciSelf == 3)
			{
				speedAdjust = 1.85f;
			}
		}
		else if (mingciSelf < pcvr.mingciFirstPlayer)
		{
			if (nowSpeed <= AdjustSpeedShould)
			{
				return;
			}
			
			//faster
			if (mingciSelf == 0)
			{
				speedAdjust = 0.70f;
			}
			else if (mingciSelf == 1)
			{
				speedAdjust = 0.80f;
			}
			else if (mingciSelf == 2)
			{
				speedAdjust = 0.85f;
			}
		}
		
		SetupGears();
		initialDragMultiplierX = dragMultiplier.x;
	}

	/*void adjustSpeedHere1(float val)
	{
		if (isAdjusting)
		{
			return;
		}

		isAdjusting = true;
		speedAdjust = val;
		
		SetupGears();
		initialDragMultiplierX = dragMultiplier.x;
	}*/

	void stopFollowCamera()
	{
		pcvr.XKCarCameraSObj.setStopFollow(true);
	}

	void followAgain()
	{
		isStopFollowC = false;
		pcvr.XKCarCameraSObj.setStopFollow(false);
		fanxiangLahui (2);
	}

	public int audioMoveStopState = 1;		//1-stop; 2-moving; 3-speeding
	private bool audioIsXieya = false;
	private float xieyaLastTime = 0.0f;
	private float audioDianshaTime = 0;
	public float xingshiSpeedMin = 10.0f;
	public float xingshiPitchMin = 0.1f;
	public float xingshiSpeedMax = 120.0f;
	public float xingshiPitchMax = 1.0f;

	//daoju and hit other AI or NPC
	void playAudioHit(int audioIndex)
	{
		if (!isPlayerTruck)
		{
			return;
		}
		
		if (pcvr.sound2DScrObj)
		{
			pcvr.sound2DScrObj.playAudioHit (audioIndex);
		}
	}

	//1-not moving; 2-moving; 3-speeding; 4-stop all audio
	public void playAudioMoveStop(int audioIndex)
	{
		if (!isPlayerTruck)
		{
			return;
		}
		
		if (pcvr.sound2DScrObj)
		{
			audioMoveStopState = pcvr.sound2DScrObj.playAudioMoveStop (audioIndex);
		}
	}

	public void playAudioBrake(int audioIndex, bool isPlay)
	{
		if (!isPlayerTruck)
		{
			return;
		}
		
		if (pcvr.sound2DScrObj)
		{
			pcvr.sound2DScrObj.playAudioBrake (audioIndex, isPlay);
		}
	}

	public void playAudioDianhuo(bool isPlay)
	{
		if (!isPlayerTruck)
		{
			return;
		}
		
		if (pcvr.sound2DScrObj)
		{
			pcvr.sound2DScrObj.playAudioDianhuo (isPlay);
		}
	}
	
	public void playAudioMingdi(bool isPlay)
	{
		if (!isPlayerTruck)
		{
			return;
		}

		if (pcvr.uiRunState < 2 || !hadInit || !truckTrans || pcvr.uiRunState == 10
		    || pcvr.isPassgamelevel)
		{
			return;
		}
		
		if (pcvr.sound2DScrObj)
		{
			pcvr.sound2DScrObj.playAudioMingdi (isPlay);
		}
	}

	public void playAudioXieya(bool isPlay)
	{
		if (!isPlayerTruck || (xieyaLastTime > 0 && Time.time - xieyaLastTime <= 1.0f))
		{
			return;
		}

		int aaa = 0;

		if (pcvr.sound2DScrObj)
		{
			aaa = pcvr.sound2DScrObj.playAudioXieya (isPlay);
		}
		
		if (aaa == 1)
		{
			controlYanXieqi(true, 0);
			xieyaLastTime = Time.time;
		}
	}

	float pushF = 50.0f;
	Vector3 forceVec = Vector3.zero;
	float hitPointTime = 0;
	bool isHit = false;

	public void hitByPlayerPlayer( Vector3 forceVecT, float speed)
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

	public int getAIIndex()
	{
		return AIIndex;
	}

	public void setTopspeed(float speed)
	{
		topSpeedNow = speed;
	}

	void destoryNetObj(GameObject deletObj)
	{
		Network.Destroy(deletObj);
		Destroy (deletObj, 1.0f);
	}

	public void stopChentuTimeend()
	{
		GameEndOrPassCloseYan ();
	}
}
