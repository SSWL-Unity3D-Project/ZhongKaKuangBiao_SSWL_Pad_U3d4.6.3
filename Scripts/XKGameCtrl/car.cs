using UnityEngine;
using System.Collections;

public class car : MonoBehaviour {
	public bool shamoGuanka = false;
	private float wheelRadius = 0.4f;
	public float suspensionRange = 0.1f;
	public float suspensionDamper = 50f;
	public float suspensionSpringFront = 18500f;
	public float suspensionSpringRear = 9000f;
	
	public Material brakeLights;
	
	public Vector3 dragMultiplier = new Vector3(2, 5, 1);
	public Vector3 dragMultiplierJiasu = new Vector3(2, 5, 1);
	private Vector3 dragMultiplierPutong = new Vector3(2, 5, 1);
	
	public float throttle = 0f; 
	public float steer = 0f;
	private bool handbrake = false;
	
	public Transform centerOfMass;
	
	public Transform[] frontWheels;
	public Transform[] rearWheels;
	
	private Wheel[] wheels;
	
	private WheelFrictionCurve wfc;

	public float topSpeedNow = 0;
	public bool isSpeedingCar = false;
	public bool isGonglu = false;
	public bool panduanGongluTulu = false;
	private float  topSpeedOld = 160f;
	public float  topSpeed = 160f;
	public float  topSpeedJiasu = 160f;
	private float  topSpeedOldTulu = 160f;
	public float  topSpeedTulu = 100f;
	public float  topSpeedJiasuTulu = 120f;
	public int numberOfGears = 5;
	
	public int maximumTurn = 15;
	public int minimumTurn = 10;
	
	public float resetTime = 5.0f;
	private float resetTimer = 0.0f;

	private float engineForceBeiLvOld = 0;
	public float engineForceBeiLv = 5f;
	private float[] engineForceValues;
	private float[] gearSpeeds;
	
	private int currentGear;
	private float currentEnginePower = 0.0f;
	
	private float handbrakeXDragFactor = 0.8f;
	private float initialDragMultiplierX = 10.0f;
	private float handbrakeTime = 0.0f;
	private float handbrakeTimer = 1.0f;
	
	private Skidmarks skidmarksTruck = null;
	private ParticleEmitter skidSmoke = null;
	public float[] skidmarkTime;
	
	//private SoundControllerTruck sound = null;
	
	private bool canSteer;
	private bool canDrive;

	public Light[] shacheLights;
	private int shacheLightNum = 0;
	
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
	// Use this for initialization
	void Start()
	{
		if (!Network.isServer && !Network.isClient)
		{
			CarStart();
		}

		if (penhuoliziPutongObj)
		{
			penhuoliziPutongPosInit = penhuoliziPutongObj.localPosition.z;
		}

		if (pcvr.playerTruckSObj)
		{
			pcvr.playerTruckSObj.setTopspeed(topSpeedNow);
		}
        //InputEventCtrl.GetInstance().ClickTVYaoKongLeftBtEvent += ClickTVYaoKongLeftBtEvent;
        //InputEventCtrl.GetInstance().ClickTVYaoKongRightBtEvent += ClickTVYaoKongRightBtEvent;
        InputEventCtrl.GetInstance().ClickTVYaoKongUpBtEvent += ClickTVYaoKongUpBtEvent;
        InputEventCtrl.GetInstance().ClickTVYaoKongDownBtEvent += ClickTVYaoKongDownBtEvent;
    }

    float m_MaxTVYouMenVal = 0.5f;
    float m_TVYouMenVal = 0f;
    float m_TVYouMenRealVal = 0.5f;
    ButtonState m_UpTVBtState = ButtonState.UP;
    ButtonState m_DownTVBtState = ButtonState.UP;
    private void ClickTVYaoKongUpBtEvent(ButtonState val)
    {
        if (pcvr.uiRunState < 2 || pcvr.uiRunState == 10 || pcvr.uiRunState == 3 || pcvr.isPassgamelevel)
        {
            return;
        }

        if (SSGameUIRoot.GetInstance().m_SSExitGameUI != null)
        {
            ResetPlayerBtInfo();
            return;
        }

        m_UpTVBtState = val;
        switch (val)
        {
            case ButtonState.UP:
                {
                    if (m_DownTVBtState == ButtonState.DOWN)
                    {
                        m_TVYouMenRealVal = -m_MaxTVYouMenVal;
                    }
                    else
                    {
                        m_TVYouMenRealVal = 0f;
                    }
                    break;
                }
            case ButtonState.DOWN:
                {
                    m_TVYouMenRealVal = m_MaxTVYouMenVal;
                    break;
                }
        }
    }

    private void ClickTVYaoKongDownBtEvent(ButtonState val)
    {
        if (pcvr.uiRunState < 2 || pcvr.uiRunState == 10 || pcvr.uiRunState == 3 || pcvr.isPassgamelevel)
        {
            return;
        }

        if (SSGameUIRoot.GetInstance().m_SSExitGameUI != null)
        {
            ResetPlayerBtInfo();
            return;
        }

        m_DownTVBtState = val;
        switch (val)
        {
            case ButtonState.UP:
                {
                    m_TVYouMenRealVal = m_MaxTVYouMenVal; //强制加满油门.
                    break;
                }
            case ButtonState.DOWN:
                {
                    m_TVYouMenRealVal = -m_MaxTVYouMenVal;
                    break;
                }
        }
    }

    /// <summary>
    ///重置玩家按键信息.
    /// </summary>
    void ResetPlayerBtInfo()
    {
        m_TVSteerVal = 0f;
        m_TVSteerRealVal = 0f;
        m_LeftTVBtState = ButtonState.UP;
        m_RightTVBtState = ButtonState.UP;
    }

    float m_TVSteerVal = 0f;
    float _TVSteerRealVal = 0f;
    float m_TVSteerRealVal
    {
        set { _TVSteerRealVal = value; }
        get
        {
            if (InputEventCtrl.GetInstance() != null)
            {
                return InputEventCtrl.GetInstance().m_PlayerRealDir;
            }
            return _TVSteerRealVal;
        }
    }
    ButtonState m_LeftTVBtState = ButtonState.UP;
    ButtonState m_RightTVBtState = ButtonState.UP;
    private void ClickTVYaoKongLeftBtEvent(ButtonState val)
    {
        if (pcvr.uiRunState < 2 || pcvr.uiRunState == 10 || pcvr.uiRunState == 3 || pcvr.isPassgamelevel)
        {
            return;
        }

        if (SSGameUIRoot.GetInstance().m_SSExitGameUI != null)
        {
            ResetPlayerBtInfo();
            return;
        }

        m_LeftTVBtState = val;
        switch (val)
        {
            case ButtonState.UP:
                {
                    if (m_RightTVBtState == ButtonState.DOWN)
                    {
                        m_TVSteerRealVal = 1f;
                    }
                    else
                    {
                        m_TVSteerRealVal = 0f;
                    }
                    break;
                }
            case ButtonState.DOWN:
                {
                    m_TVSteerRealVal = -1f;
                    break;
                }
        }
    }

    private void ClickTVYaoKongRightBtEvent(ButtonState val)
    {
        if (pcvr.uiRunState < 2 || pcvr.uiRunState == 10 || pcvr.uiRunState == 3 || pcvr.isPassgamelevel)
        {
            return;
        }

        if (SSGameUIRoot.GetInstance().m_SSExitGameUI != null)
        {
            ResetPlayerBtInfo();
            return;
        }

        m_RightTVBtState = val;
        switch (val)
        {
            case ButtonState.UP:
                {
                    if (m_LeftTVBtState == ButtonState.DOWN)
                    {
                        m_TVSteerRealVal = -1f;
                    }
                    else
                    {
                        m_TVSteerRealVal = 0f;
                    }
                    break;
                }
            case ButtonState.DOWN:
                {
                    m_TVSteerRealVal = 1f;
                    break;
                }
        }
    }

    private bool hasInitCar = false;
	public void CarStart () {
		if (hasInitCar)
		{
			return;
		}
		Debug.Log (transform + " init car heressssssssssssssssssssss ");
		hasInitCar = true;

		engineForceBeiLvOld = engineForceBeiLv;
		topSpeedOld = topSpeed;
		topSpeedOldTulu = topSpeedTulu;
		pcvr.carSObj = GetComponent<car>();
		CarTr = transform;
		hitLiziPointNum = CarVerHitTrLizi.Length;

		wheels = new Wheel[frontWheels.Length + rearWheels.Length];
		//sound = transform.GetComponent<SoundControllerTruck>();
		shacheLightNum = shacheLights.Length;

		// Measuring 1 - 60
		//accelerationTimer = Time.time;
		
		SetupWheelColliders();
		
		SetupCenterOfMass();
		
		topSpeed = Convert_Miles_Per_Hour_To_Meters_Per_Second(topSpeed);
		SetupGears();

		if (!Network.isServer)
		SetUpSkidmarks();
		
		initialDragMultiplierX = dragMultiplier.x;

		dragMultiplierPutong = new Vector3 (dragMultiplier.x, dragMultiplier.y, dragMultiplier.z);
	
	}
	Vector3 relativeVelocityUpdate = Vector3.zero;
	void Update()
	{//Debug.Log (Network.isServer + " " +transform + " "+ pcvr.uiRunState + " " + pcvr.totalTime + " "+ transform.position.x + " "+ transform.position.y + " "+ transform.position.z);
		if(pcvr.uiRunState < 2 || pcvr.uiRunState == 10 || pcvr.uiRunState == 3 || pcvr.isPassgamelevel)
		{
			return;
		}

		relativeVelocityUpdate = transform.InverseTransformDirection(rigidbody.velocity);
		
		//GetInput();
		
		Check_If_Car_Is_Flipped();
		
		UpdateWheelGraphics(relativeVelocityUpdate);
		
		UpdateGear(relativeVelocityUpdate);
		
		CheckPlayerInputSteer(steer);

		if (curHitTime > 0)
		{
			curHitTime -= Time.deltaTime;
		}
		else
		{
			CheckCarHorHitInfoPar();
		}
	}
	Vector3 relativeVelocity = Vector3.zero;
	void FixedUpdate()
	{
		if(pcvr.uiRunState < 2 || pcvr.uiRunState == 10 || pcvr.uiRunState == 3 || pcvr.isPassgamelevel)
		{
			return;
		}

		GetInput();

		// The rigidbody velocity is always given in world space, but in order to work in local space of the car model we need to transform it first.
		relativeVelocity = transform.InverseTransformDirection(rigidbody.velocity);
		
		CalculateState();	
		
		UpdateFriction(relativeVelocity);
		
		UpdateDrag(relativeVelocity);
		
		CalculateEnginePower(relativeVelocity);
		
		ApplyThrottle(canDrive, relativeVelocity);
		
		ApplySteering(canSteer, relativeVelocity);

		//if (!shamoGuanka)
			CheckCarHorHitInfo(shamoGuanka);
	}
	private Vector3 relativeVelocityLateUpdate = Vector3.zero;
	private float speedLateUpdate = 0.0f;
	
	//pu tong penhuo
	public Transform penhuoliziPutongObj = null;
	private float penhuoliziPutongPosInit = 0.0f;
	private float penhuoliziPutongPos = 0.0f;
	public float penhuoliziPutongPosLength = 0;
	public float penhuoliziPutongSpeedMin = 10;
	public float penhuoliziPutongSpeedMax = 100;
	private float penhuoliziPutongBizhi = 1.0f;

	void LateUpdate()
	{
		if(pcvr.uiRunState < 2 || pcvr.uiRunState == 10 || pcvr.uiRunState == 3 || pcvr.isPassgamelevel)
		{
			if (Network.isClient)
			{
				//vector3Value1 = transform.position;
				vector3Value1 = new Vector3(transform.position.x,transform.position.y,transform.position.z);
				vector3Value2 = new Vector3(transform.eulerAngles.x,transform.eulerAngles.y,transform.eulerAngles.z);
				
				transform.networkView.RPC("RefeshTransform", RPCMode.AllBuffered, vector3Value1, vector3Value2, Network.player, penhuoliziPutongPos);
			}
			return;
		}
		
		if (penhuoliziPutongObj)
		{
			if (penhuoliziPutongSpeedMax <= 0)
			{
				penhuoliziPutongSpeedMax = 10;
			}

			relativeVelocityLateUpdate = transform.InverseTransformDirection(rigidbody.velocity);
			speedLateUpdate = Mathf.Abs(relativeVelocityLateUpdate.z * 3.6f);
			penhuoliziPutongBizhi = speedLateUpdate / penhuoliziPutongSpeedMax;

			if (penhuoliziPutongBizhi > 1)
			{
				penhuoliziPutongBizhi = 1.0f;
			}

			penhuoliziPutongPos = penhuoliziPutongPosInit - penhuoliziPutongPosLength * penhuoliziPutongBizhi;
			penhuoliziPutongObj.localPosition = new Vector3(penhuoliziPutongObj.localPosition.x, penhuoliziPutongObj.localPosition.y, penhuoliziPutongPos);
		}
		
		if (Network.isClient)
		{
			//vector3Value1 = transform.position;
			vector3Value1 = new Vector3(transform.position.x, transform.position.y, transform.position.z);
			vector3Value2 = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);

			transform.networkView.RPC("RefeshTransform", RPCMode.AllBuffered, vector3Value1, vector3Value2, Network.player, penhuoliziPutongPos);
		}
	}
	private Vector3 vector3Value1 = Vector3.zero;
	private Vector3 vector3Value2 = Vector3.zero;

	void SetupWheelColliders()
	{
		SetupWheelFrictionCurve();
		
		int wheelCount1 = 0;
		
		foreach (Transform t in frontWheels)
		{
			wheels[wheelCount1] = SetupWheel(t, true);
			wheelCount1++;
		}
		
		foreach (Transform t in rearWheels)
		{
			wheels[wheelCount1] = SetupWheel(t, false);
			wheelCount1++;
		}
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
		go.transform.parent = transform;
		go.transform.rotation = wheelTransform.rotation;

		if (LayerMask.NameToLayer("lunzi") > 0)
		go.layer = LayerMask.NameToLayer("lunzi");
		
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
		
		//wheelRadius = wheel.tireGraphic.renderer.bounds.size.y / 2 * transform.localScale.x;
		wheelRadius = wheel.tireGraphic.renderer.bounds.size.y / 2;
		wheel.collider.radius = wheelRadius;

		if (shamoGuanka)
			wheel.collider.radius = 0.55f;
		//wheel.collider.transform.localScale = new Vector3 (1,1,1);
		//Debug.Log (wheelRadius + " " + transform + "tttttttttttttttt " + transform.localScale.x);
		if (isFrontWheel)
		{
			wheel.steerWheel = true;
			
			go = new GameObject(wheelTransform.name + " Steer Column");
			go.transform.position = wheelTransform.position;
			go.transform.rotation = wheelTransform.rotation;
			go.transform.parent = transform;
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
		
		float tempTopSpeed = topSpeed;
		
		for(int i = 0; i < numberOfGears; i++)
		{
			if(i > 0)
				gearSpeeds[i] = tempTopSpeed / 4 + gearSpeeds[i-1];
			else
				gearSpeeds[i] = tempTopSpeed / 4;
			
			tempTopSpeed -= tempTopSpeed / 4;
		}
		
		float engineFactor = topSpeed / gearSpeeds[gearSpeeds.Length - 1];
		
		for(int i = 0; i < numberOfGears; i++)
		{
			float maxLinearDrag = gearSpeeds[i] * gearSpeeds[i];// * dragMultiplier.z;
			engineForceValues[i] = engineForceBeiLv * maxLinearDrag * engineFactor;
		}
	}
	public Skidmarks skidmarksPrefab;
	void SetUpSkidmarks()
	{
		if(skidmarksPrefab)
		{
			if (Network.isClient)
			{
				skidmarksTruck = Network.Instantiate(skidmarksPrefab, Vector3.zero, Quaternion.identity, 0) as Skidmarks;
				//pcvr.chanshengdeObjNet[pcvr.chanshengdeObjNetNum] = skidmarksTruck.gameObject;
				//pcvr.chanshengdeObjNetNum ++;
			}
			else if (!Network.isServer)
			{
				skidmarksTruck = Instantiate(skidmarksPrefab, Vector3.zero, Quaternion.identity) as Skidmarks;
				//pcvr.chanshengdeObj[pcvr.chanshengdeObjNum] = skidmarksTruck.gameObject;
				//pcvr.chanshengdeObjNum ++;
			}

			skidSmoke = skidmarksTruck.GetComponentInChildren<ParticleEmitter>();
		}
		else
			Debug.Log("No skidmarks object found. Skidmarks will not be drawn");
		
		skidmarkTime = new float[4];
		for (int i=0; i<4; i++)
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
		if (pcvr.bIsHardWare)
		{
			throttle = pcvr.mGetPower;
			//throttle = Input.GetAxis("Vertical");	//test

			if (pcvr.IsActiveShaCheEvent)
			{
				throttle = 0 - pcvr.shacheValue;
			}

			 steer = pcvr.mGetSteer;
			//steer = Input.GetAxis("Horizontal");	//test
		}
		else
		{

            //throttle = 0.5f; //强制给到最大油门.
            //throttle = Input.GetAxis("Vertical");
            if (m_TVYouMenRealVal >= 0f)
            {
                m_TVYouMenVal = Mathf.Lerp(m_TVYouMenVal, m_TVYouMenRealVal, 0.1f);
            }
            else
            {
                m_TVYouMenVal = -1f;
            }

            throttle = m_TVYouMenVal;
            if (throttle < 0)
			{
				throttle = throttle / 100.0f;
			}
            //卡车方向信息.
            //steer = Input.GetAxis("Horizontal");
            m_TVSteerVal = Mathf.Lerp(m_TVSteerVal, m_TVSteerRealVal, 0.1f);
            steer = m_TVSteerRealVal;
        }

		/*if (pcvr.playerTruckSObj.angleZhuan >= 89.5f)
		{
			if (steer < 0 && pcvr.playerTruckSObj.dianChengFR < 0)
			{
				steer = 0;
			}
			else if (steer > 0 && pcvr.playerTruckSObj.dianChengFR > 0)
			{
				steer = 0;
			}
		}*/

		InputAccel = throttle;
		InputSteer = steer;
		
		if (rigidbody.isKinematic && Mathf.Abs(throttle) > 0)
		{
			rigidbody.isKinematic = false;
		}
		else if (!rigidbody.isKinematic && Mathf.Abs(rigidbody.velocity.magnitude) < 0.2f && Mathf.Abs(throttle) == 0)
		{
			rigidbody.isKinematic = true;
		}

		
		if(throttle < 0.0)
			brakeLights.SetFloat("_Intensity", Mathf.Abs(throttle));
		else
			brakeLights.SetFloat("_Intensity", 0.0f);
		
		//CheckHandbrake();


		pcvr.playerThrottle = throttle;
		pcvr.playerSteer = steer;
		
		if (throttle > 0 && pcvr.playerTruckSObj)
		{
			pcvr.playerTruckSObj.YanHereBegin();
		}
		else if (pcvr.playerTruckSObj)
		{
			pcvr.playerTruckSObj.YanHereEnd();
		}
		
		pcvr.playerBrake = handbrake;
	}
	
	void CheckHandbrake()
	{
		if((pcvr.bIsHardWare && pcvr.IsActiveShaCheEvent) || (!pcvr.bIsHardWare && Input.GetKey("space")))
		{
			if(!handbrake)
			{
				handbrake = true;
				handbrakeTime = Time.time;
				dragMultiplier.x = initialDragMultiplierX * handbrakeXDragFactor;

				if (shacheLightNum > 0)
				{
					openShacheLight();
				}
			}
		}
		else if(handbrake)
		{
			handbrake = false;
			StartCoroutine(StopHandbraking(Mathf.Min(5, Time.time - handbrakeTime)));

			if (shacheLightNum > 0)
			{
				closeShacheLight();
			}
		}
		InputBrake = handbrake == true ? 1f : 0f;
	}
	
	IEnumerator StopHandbraking(float seconds)
	{
		float diff = initialDragMultiplierX - dragMultiplier.x;
		handbrakeTimer = 1;
		
		// Get the x value of the dragMultiplier back to its initial value in the specified time.
		while(dragMultiplier.x < initialDragMultiplierX && !handbrake)
		{
			dragMultiplier.x += diff * (Time.deltaTime / seconds);
			handbrakeTimer -= Time.deltaTime / seconds;
			yield return 1;	//fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff
		}
		
		dragMultiplier.x = initialDragMultiplierX;
		handbrakeTimer = 0;
	}
	
	void Check_If_Car_Is_Flipped()
	{
		if(transform.localEulerAngles.z > 80 && transform.localEulerAngles.z < 280)
			resetTimer += Time.deltaTime;
		else
			resetTimer = 0;
		
		if(resetTimer > resetTime)
			FlipCar();
	}
	
	void FlipCar()
	{
		transform.rotation = Quaternion.LookRotation(transform.forward);
		transform.position += Vector3.up * 0.5f;
		rigidbody.velocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;
		resetTimer = 0;
		currentEnginePower = 0;
	}
	
	public int wheelCount;
	bool isCheHenT = false;
	void UpdateWheelGraphics(Vector3 relativeVelocity)
	{
		wheelCount = -1;
		
		foreach(Wheel w in wheels)
		{
			wheelCount++;
			isCheHenT = false;
			WheelCollider wheel = w.collider;
			WheelHit wh = new WheelHit();
			
			// First we get the velocity at the point where the wheel meets the ground, if the wheel is touching the ground
			if(wheel.GetGroundHit(out wh))
			{
				w.wheelGraphic.localPosition = wheel.transform.up * (wheelRadius + wheel.transform.InverseTransformPoint(wh.point).y);
				w.wheelVelo = rigidbody.GetPointVelocity(wh.point);
				w.groundSpeed = w.wheelGraphic.InverseTransformDirection(w.wheelVelo);
				//Debug.Log(w.groundSpeed + " " + w.wheelVelo + " " + wh.point);
				
				// Code to handle skidmark drawing. Not covered in the tutorial
				if(skidmarksTruck)
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
						{
							Vector3 staticVel = transform.TransformDirection(skidSmoke.localVelocity) + skidSmoke.worldVelocity;
							if(w.lastSkidmark != -1)
							{
								float emission = UnityEngine.Random.Range(skidSmoke.minEmission, skidSmoke.maxEmission);
								float lastParticleCount = w.lastEmitTime * emission;
								float currentParticleCount = Time.time * emission;
								int noOfParticles = Mathf.CeilToInt(currentParticleCount) - Mathf.CeilToInt(lastParticleCount);
								int lastParticle = Mathf.CeilToInt(lastParticleCount);
								
								for(int i = 0; i <= noOfParticles; i++)
								{
									float particleTime = Mathf.InverseLerp(lastParticleCount, currentParticleCount, lastParticle + i);
									skidSmoke.Emit(	Vector3.Lerp(w.lastEmitPosition, wh.point, particleTime) + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f)), staticVel + (w.wheelVelo * 0.05f), Random.Range(skidSmoke.minSize, skidSmoke.maxSize) * Mathf.Clamp(skidGroundSpeed * 0.1f,0.5f,1), Random.Range(skidSmoke.minEnergy, skidSmoke.maxEnergy), Color.white);
								}
							}
							else
							{
								skidSmoke.Emit(	wh.point + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f)), staticVel + (w.wheelVelo * 0.05f), Random.Range(skidSmoke.minSize, skidSmoke.maxSize) * Mathf.Clamp(skidGroundSpeed * 0.1f,0.5f,1f), Random.Range(skidSmoke.minEnergy, skidSmoke.maxEnergy), Color.white);
							}
							
							w.lastEmitPosition = wh.point;
							w.lastEmitTime = Time.time;
							
							w.lastSkidmark = skidmarksTruck.AddSkidMarkTruck(wh.point + rigidbody.velocity * dt, wh.normal, (skidGroundSpeed * 0.1f + handbrakeSkidding) * Mathf.Clamp01(wh.force / wheel.suspensionSpring.spring), w.lastSkidmark);
							//sound.Skid(true, Mathf.Clamp01(skidGroundSpeed * 0.1f));
							isCheHenT = true;
							if (pcvr.playerTruckSObj)
							pcvr.playerTruckSObj.playAudioBrake(2, true);
						}
						else
						{
							w.lastSkidmark = -1;
							//sound.Skid(false, 0);
							if (pcvr.playerTruckSObj)
							pcvr.playerTruckSObj.playAudioBrake(2, false);
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
				
				if(skidmarksTruck)
				{
					w.lastSkidmark = -1;
					//sound.Skid(false, 0);
					if (pcvr.playerTruckSObj)
					pcvr.playerTruckSObj.playAudioBrake(2, false);
				}
			}
			// If the wheel is a steer wheel we apply two rotations:
			// *Rotation around the Steer Column (visualizes the steer direction)
			// *Rotation that visualizes the speed
			if(w.steerWheel)
			{
				Vector3 ea = w.wheelGraphic.parent.localEulerAngles;
				ea.y = steer * maximumTurn;
				w.wheelGraphic.parent.localEulerAngles = ea;
				w.tireGraphic.Rotate(Vector3.right * (w.groundSpeed.z / wheelRadius) * Time.deltaTime * Mathf.Rad2Deg);
			}
			else if(!handbrake && w.driveWheel)
			{
				// If the wheel is a drive wheel it only gets the rotation that visualizes speed.
				// If we are hand braking we don't rotate it.
				w.tireGraphic.Rotate(Vector3.right * (w.groundSpeed.z / wheelRadius) * Time.deltaTime * Mathf.Rad2Deg);
			}
		}

		pcvr.isChehen = isCheHenT;
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
			drag.x /= (relativeVelocity.magnitude / (topSpeed / ( 1 + 2 * handbrakeXDragFactor ) ) );
			drag.z *= (1 + Mathf.Abs(Vector3.Dot(rigidbody.velocity.normalized, transform.forward)));
			drag += rigidbody.velocity * Mathf.Clamp01(rigidbody.velocity.magnitude / topSpeed);
		}
		else // No handbrake
		{
			drag.x *= topSpeed / relativeVelocity.magnitude;
		}
		
		if(Mathf.Abs(relativeVelocity.x) < 5 && !handbrake)
			drag.x = -relativeVelocity.x * dragMultiplier.x;
		
		if (!rigidbody.isKinematic && drag != Vector3.zero)
		rigidbody.AddForce(transform.TransformDirection(drag) * rigidbody.mass * Time.deltaTime);
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
			currentEnginePower -= Time.deltaTime * 400;
		}
		else if( HaveTheSameSign(relativeVelocity.z, throttle) )
		{
			float normPower = (currentEnginePower / engineForceValues[engineForceValues.Length - 1]) * 2;
			currentEnginePower += Time.deltaTime * 200 * EvaluateNormPower(normPower);
		}
		else
		{
			currentEnginePower -= Time.deltaTime * 700;
		}
		
		if(currentGear == 0)
			currentEnginePower = Mathf.Clamp(currentEnginePower, 0, engineForceValues[0]);
		else
			currentEnginePower = Mathf.Clamp(currentEnginePower, engineForceValues[currentGear - 1], engineForceValues[currentGear]);
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
		if (rigidbody.velocity.magnitude >= topSpeed) {
			return;
		}

		if(canDrive && canSteer)
		{
			Physics.gravity = new Vector3(0, -30, 0);
			
			if (isLuokong)
			{
				changeEngineForceBeiLvLuokongStop();
			}
		}
		else
		{
			Physics.gravity = new Vector3(0, -100, 0);

			if (!isLuokong)
			{
				changeEngineForceBeiLvLuokong();
			}
		}

		float throttleForce = 0;
		float brakeForce = 0;

		if (canDrive)
		{
			
			/*if (throttle == 0)
			{
				throttleForce = 0;
				brakeForce = 0;
			}
			else*/
			if (HaveTheSameSign(relativeVelocity.z, throttle))
			{
				if (!handbrake)
					throttleForce = Mathf.Sign(throttle) * currentEnginePower * rigidbody.mass;
			}
			else if(throttle == 0)
				brakeForce = Mathf.Sign(throttle) * engineForceValues[0] * rigidbody.mass;
			else
				brakeForce =/* Mathf.Sign(throttle) **/ engineForceValues[0] * rigidbody.mass * (1f * throttle);
			if (!rigidbody.isKinematic)
				rigidbody.AddForce(transform.forward * Time.deltaTime * (throttleForce + brakeForce));
		}
	}
	
	void ApplySteering(bool canSteer, Vector3 relativeVelocity)
	{
		if(canSteer)
		{
			float turnRadius = 3.0f / Mathf.Sin((90 - (steer * 30)) * Mathf.Deg2Rad);
			float minMaxTurn = EvaluateSpeedToTurn(rigidbody.velocity.magnitude);
			float turnSpeed = Mathf.Clamp(relativeVelocity.z / turnRadius, -minMaxTurn / 10, minMaxTurn / 10);
			
			transform.RotateAround(	transform.position + transform.right * turnRadius * steer, 
			                       transform.up, 
			                       turnSpeed * Mathf.Rad2Deg * Time.deltaTime * steer);
			
			Vector3 debugStartPoint = transform.position + transform.right * turnRadius * steer;
			Vector3 debugEndPoint = debugStartPoint + Vector3.up * 5;
			
			Debug.DrawLine(debugStartPoint, debugEndPoint, Color.red);
			
			if(initialDragMultiplierX > dragMultiplier.x) // Handbrake
			{
				float rotationDirection = Mathf.Sign(steer); // rotationDirection is -1 or 1 by default, depending on steering
				if(steer == 0)
				{
					if(rigidbody.angularVelocity.y < 1) // If we are not steering and we are handbraking and not rotating fast, we apply a random rotationDirection
						rotationDirection = Random.Range(-1.0f, 1.0f);
					else
						rotationDirection = rigidbody.angularVelocity.y; // If we are rotating fast we are applying that rotation to the car
				}
				// -- Finally we apply this rotation around a point between the cars front wheels.
				transform.RotateAround( transform.TransformPoint( (	frontWheels[0].localPosition + frontWheels[1].localPosition) * 0.5f), 
				                       transform.up, 
				                       rigidbody.velocity.magnitude * Mathf.Clamp01(1 - rigidbody.velocity.magnitude / topSpeed) * rotationDirection * Time.deltaTime * 2);
			}
		}
	}
	
	/**************************************************/
	/*               Utility Functions                */
	/**************************************************/
	
	float Convert_Miles_Per_Hour_To_Meters_Per_Second(float value)
	{
		//return value * 0.44704f;
		return value / 3.6f;
	}
	
	float Convert_Meters_Per_Second_To_Miles_Per_Hour(float value)
	{
		return value * 2.23693629f;	
	}
	
	bool HaveTheSameSign(float first, float second)
	{
		if (Mathf.Sign(first) == Mathf.Sign(second))
			return true;
		else
			return false;
	}
	
	float EvaluateSpeedToTurn(float speed)
	{
		if(speed > topSpeed / 2)
			return minimumTurn;
		
		float speedIndex = 1 - (speed / (topSpeed / 2));
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
		Vector3 relativeVelocity = transform.InverseTransformDirection(rigidbody.velocity);
		int lowLimit = (int)(currentGear == 0 ? 0 : gearSpeeds[currentGear-1]);
		return (relativeVelocity.z - lowLimit) / (gearSpeeds[currentGear - lowLimit]) * (1f - currentGear * 0.1f) + currentGear * 0.1f;
	}

	public void aaa(int index)
	{//Debug.Log ("aaaaaaaaaaaaaa  " + index + " " + Time.time);
		SetupGears();
		initialDragMultiplierX = dragMultiplier.x;
	}

	//speeding here
	public void changeTorqueNew(bool isGonglu)
	{
		if (pcvr.XKCarCameraSObj)
		{
			pcvr.XKCarCameraSObj.setJiasu();
		}

		dragMultiplier = new Vector3 (dragMultiplierJiasu.x, dragMultiplierJiasu.y, dragMultiplierJiasu.z);

		engineForceBeiLv = engineForceBeiLvOld * 2.0f;

		if (isGonglu)
		{
			topSpeed = topSpeedJiasu;
		}
		else
		{
			topSpeed = topSpeedJiasuTulu;
		}
		topSpeedNow = topSpeed;
		
		if (pcvr.playerTruckSObj)
		{
			pcvr.playerTruckSObj.setTopspeed(topSpeedNow);
		}

		isSpeedingCar = true;
		topSpeed = Convert_Miles_Per_Hour_To_Meters_Per_Second(topSpeed);

		aaa(1);
	}

	//speeding end
	public void changeTorqueHui(bool isGonglu)
	{
		if (pcvr.RadialBlurSObj)
		{
			pcvr.RadialBlurSObj.setZhengchang();
		}

		if (pcvr.XKCarCameraSObj)
		{
			pcvr.XKCarCameraSObj.setZhengchang();
		}
		
		dragMultiplier = new Vector3 (dragMultiplierPutong.x, dragMultiplierPutong.y, dragMultiplierPutong.z);

		engineForceBeiLv = engineForceBeiLvOld;

		if (isGonglu)
		{
			topSpeed = topSpeedOld;
		}
		else
		{
			topSpeed = topSpeedOldTulu;
		}
		topSpeedNow = topSpeed;
		
		if (pcvr.playerTruckSObj)
		{
			pcvr.playerTruckSObj.setTopspeed(topSpeedNow);
		}

		isSpeedingCar = false;

		topSpeed = Convert_Miles_Per_Hour_To_Meters_Per_Second(topSpeed);
		aaa(3);
	}

	//gonglu - tulu
	public void changeTopSpeedGongluTulu(bool isGongluT, int indexT)
	{//Debug.Log ("changeTopSpeedGongluTuluchangeTopSpeedGongluTulu  " + isGongluT + " "+ isSpeedingCar);
		if (!panduanGongluTulu)
		{
			return;
		}

		if (!isSpeedingCar)
		{
			if (isGongluT)
			{
				topSpeed = topSpeedOld;
			}
			else
			{
				topSpeed = topSpeedOldTulu;
			}
		}
		else
		{
			if (isGongluT)
			{
				topSpeed = topSpeedJiasu;
			}
			else
			{
				topSpeed = topSpeedJiasuTulu;
			}
		}

		topSpeedNow = topSpeed;
		
		if (pcvr.playerTruckSObj)
		{
			pcvr.playerTruckSObj.setTopspeed(topSpeedNow);
		}

		isGonglu = isGongluT;
		topSpeed = Convert_Miles_Per_Hour_To_Meters_Per_Second(topSpeed);
		aaa(2);
	}

	void openShacheLight()
	{
		for (int i = 0; i < shacheLightNum; i++)
		{
			shacheLights[i].enabled = true;
		}
	}

	void closeShacheLight()
	{
		for (int i = 0; i < shacheLightNum; i++)
		{
			shacheLights[i].enabled = false;
		}
	}

	
	float InputAccel;
	float InputSteer;
	float InputBrake;
	Vector3 VecDirSpeed;
	public float DirSpeed = 20f;
	Transform CarTr;

	void CheckPlayerInputSteer(float steer)
	{
		if (steer == 0f) {
			return;
		}
		
		if (rigidbody.velocity.magnitude * 3.6f > 10f) {
			return;
		}
		
		if (InputAccel == 0f && InputBrake == 0f) {
			return;
		}
		
		float dirState = steer > 0f ? 1f : -1f;
		if (InputAccel < 0f) {
			dirState *= -1;
		}
		VecDirSpeed.y = dirState * DirSpeed * Time.deltaTime;
		CarTr.Rotate(VecDirSpeed);
	}
	
	int CountVerHitVal;
	bool IsCarVerHit;
	float TimeCarVerHit;
	public float RotCarHitY = 30f;
	public float PushDisVal = 5f;
	public LayerMask CarHitLayer;
	public Transform[] CarVerHitTr;
	
	void CheckCarHorHitInfo(bool isShamo)
	{
//		if (speed > 40f) {
//			return;
//		}
		
		if (InputAccel <= 0f) {
			IsCarVerHit = false;
			//rigidbody.constraints = RigidbodyConstraints.None;
			return;
		}
		
		if (InputBrake > 0f) {
			IsCarVerHit = false;
			//rigidbody.constraints = RigidbodyConstraints.None;
			return;
		}
		
		if (CarVerHitTr.Length <= 9) {
			return;
		}
		
		Vector3 posA = Vector3.zero;

		RaycastHit hit;
		if (IsCarVerHit) {
			if (Time.realtimeSinceStartup - TimeCarVerHit > 0.2f) {
				IsCarVerHit = false;
			}

			if (isShamo) {
				return;		
			}
			
			float rotSpeedY = 0f;
			Vector3 vecBA = Vector3.zero;
			Vector3 vecABTmp = Vector3.zero;
			int carHitNum = 0;
			if (CountVerHitVal < (int)(CarVerHitTr.Length * 0.5f)) {
				//rot right
				rotSpeedY = Time.deltaTime * RotCarHitY;
				vecBA = CarTr.right;
			}
			else {
				//rot left
				rotSpeedY = -Time.deltaTime * RotCarHitY;
				vecBA = -CarTr.right;
			}
			
			if (carHitNum != 2) {
				CarTr.Rotate(new Vector3(0f, rotSpeedY, 0f));
			}
			else {
				CarTr.forward = Vector3.Lerp(CarTr.forward, vecABTmp.normalized, Time.deltaTime * 3f);
			}
			
			vecBA = vecBA.normalized * Time.deltaTime * PushDisVal;
			CarTr.Translate(vecBA, Space.World);
			return;
		}
		
		if (CountVerHitVal >= CarVerHitTr.Length) {
			CountVerHitVal = 0;
		}
		posA = CarVerHitTr[CountVerHitVal].position;
		
		if (Physics.Raycast(posA, CarVerHitTr[CountVerHitVal].forward, out hit, 1f, CarHitLayer)) {
			//Debug.Log("CountVerHitVal "+CountVerHitVal);
			IsCarVerHit = true;
			TimeCarVerHit = Time.realtimeSinceStartup;
			//rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ;
			return;
		}
		CountVerHitVal++;
		/*if (CountVerHitVal >= CarVerHitTr.Length)
		{
			rigidbody.constraints = RigidbodyConstraints.None;
		}*/
	}

	public GameObject pengzhuangLIzi;
	public LayerMask CarHitLayerLizi;
	public Transform[] CarVerHitTrLizi;
	private int hitLiziPointNum = 0;
	private int curHitIndex = -1;
	private bool hasFindHit = false;
	RaycastHit hitLizi;
	public AudioSource audioZhuangZhangai;	//hit someth
	public float hitJianggeTime = 1.0f;
	private float curHitTime = 0;
	private int fangxiangpanDoudongIndex = 1;

	void  CheckCarHorHitInfoPar()
	{
		if (hitLiziPointNum <= 0 || !pengzhuangLIzi)
		{
			return;
		}

		hasFindHit = false;

		for (int i = 0; i< hitLiziPointNum; i++)
		{
			if (Physics.Raycast(CarVerHitTrLizi[i].position, CarVerHitTrLizi[i].forward, out hitLizi, 1f, CarHitLayerLizi))
			{
				if (curHitIndex != i)
				{
					curHitIndex = i;
					GameObject Tobject = (GameObject)Instantiate(pengzhuangLIzi, hitLizi.point, Quaternion.identity);
					Destroy(Tobject, 2.0f);
					playAudioHit (2);

					//if (i != 4 && i != 9)
					//{
						if ((hitLizi.transform.GetComponent<triTruckInfor>()
						     && hitLizi.transform.GetComponent<triTruckInfor>().getTriggerName().CompareTo("NPCSmallTri") == 0)
						    || (hitLizi.transform.root.GetComponent<triTruckInfor>()
						    && hitLizi.transform.root.GetComponent<triTruckInfor>().getTriggerName().CompareTo("NPCSmallTri") == 0)
						    || (hitLizi.transform.GetComponent<triTruckInfor>()
						 	&& hitLizi.transform.GetComponent<triTruckInfor>().getTriggerName().CompareTo("PlayerTri") == 0)
						    || (hitLizi.transform.root.GetComponent<triTruckInfor>()
						    && hitLizi.transform.root.GetComponent<triTruckInfor>().getTriggerName().CompareTo("PlayerTri") == 0))
						{
							pcvr.playerTruckSObj.fangxiangpanDoudongIndex = 4;
						}
						else if (hitLizi.transform.gameObject.layer == LayerMask.NameToLayer("car"))
						{
							pcvr.playerTruckSObj.fangxiangpanDoudongIndex = 5;
						}
						else if (hitLizi.transform.gameObject.layer == LayerMask.NameToLayer("xiaodaoju"))
						{
							pcvr.playerTruckSObj.fangxiangpanDoudongIndex = 6;
						}
						else
							pcvr.playerTruckSObj.fangxiangpanDoudongIndex = 1;

						pcvr.GetInstance().OpenFangXiangPanZhenDong(pcvr.playerTruckSObj.fangxiangpanDoudongIndex, false);
						pcvr.GetInstance().OpenQinangDouDong(true);
					//}
				}

				hasFindHit = true;
				curHitTime = hitJianggeTime;
				break;
			}
		}

		if (!hasFindHit)
		{
			curHitIndex = -1;

			if (pcvr.playerTruckSObj.isInwater)
			{
				pcvr.playerTruckSObj.fangxiangpanDoudongIndex = 3;
			}
			else if (pcvr.playerTruckSObj.isInSuishiLu)
			{
				pcvr.playerTruckSObj.fangxiangpanDoudongIndex = 2;
			}
			else if (pcvr.playerTruckSObj.isInShadiLu)
			{
				pcvr.playerTruckSObj.fangxiangpanDoudongIndex = 0;
			}
			else
			{
				pcvr.playerTruckSObj.fangxiangpanDoudongIndex = 1;
			}
		}
	}
	
	void playAudioHit(int audioIndex)
	{
		if (audioIndex == 2 && audioZhuangZhangai && !audioZhuangZhangai.isPlaying)
		{
			//zhangaiwu
			audioZhuangZhangai.loop = false;
			audioZhuangZhangai.Play();

			if (pcvr.cameraShakeSObj)
			pcvr.cameraShakeSObj.setCameraShakeImpulseValue();
		}
	}

	private bool isLuokong = false;
	void changeEngineForceBeiLvLuokong()
	{
		isLuokong = true;
		engineForceBeiLv = 1.0f;

		aaa(33);
	}

	void changeEngineForceBeiLvLuokongStop()
	{
		if (isSpeedingCar)
		{
			engineForceBeiLv = engineForceBeiLvOld * 2.0f;
		}
		else
		{
			engineForceBeiLv = engineForceBeiLvOld;
		}
		
		isLuokong = false;
		
		aaa(333);
	}
}
