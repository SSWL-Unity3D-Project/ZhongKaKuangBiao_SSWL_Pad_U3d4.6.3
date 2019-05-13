using UnityEngine;
using System;
using System.Collections;

public class NPCControl : MonoBehaviour {

	public bool isMotuo = false;
	private bool fanxiang = false;
	private bool isZhengXiang = true;
	public Transform[] wheelArr;
	public Transform specialPoint = null;
	private int wheelNum = 0;
	public float rotateSpeed = 120.0f;
	public float lidiDistance = 0.5f;
	
	public Light aaaLight = null;
	public Light bbbLight = null;
	public float totalTime1 = 0.5f;
	public float totalTime2 = 0.5f;
	private int lightState1 = -1;
	private int lightState2 = -1;
	private float lightTimeCur1 = 0;
	private float lightTimeCur2 = 0;

	//smoke
	public GameObject[] yan;
	private int yanNumber = 0;

	//dust
	public GameObject[] chenTu;
	private int chenTuNumber = 0;

	private Transform curPath = null;
	private bool isMoving = false;
	public float speed = 7.5f;
	//private float normalSpeed = 7.5f;
	private float lastSpeed = 0;
	//private float subSpeedDistance = 15;
	//private float addSpeedValue = 5.2f;
	private bool isAddSpeed = false;
	private float NPCRotateSpeed = 0.12f;
	private Transform nextPoint = null;
	private Transform NPCTransform = null;
	private float curDistance = 0.0f;
	private int curPathpointTotalNum = 0;
	private int curPathpointNum = 0;

	public float forceValueFly = 52000f;	//will leave away----speeding
	private float forceValue = 0;		//value
	private float forceTimeLong = 1.8f;
	private float forceTimeTotal = 0;
	private float forceTimeCurrent = 0;
	private Vector3 forceVector = Vector3.zero;
	private float topSpeed = 2;
	private float changeTime = 0.25f;
	public int hitState = 0;	//10-will not moving, and only be hitted or be deleted
								//9-be hitted, only add the force
								//0-can move

	//path
	private Transform[] pathpointArr;
	private string[] animationNameStr;

	//terrain
	private  LayerMask mask;
	private  LayerMask maskTerr;
	private RaycastHit hit;
	
	private int numLayerCar = -1;
	private int numLayerTerrain = -1;

	//public Transform[] hitPoints;	//forward back left right-----pointArr
	private Vector3[] hitPointVector;
	public Vector3 testDirection = Vector3.zero;
	private Vector3 testDirectionR = Vector3.zero;
	private float angle = 0.0f;
	private float fallDistance = 0;

	private bool isGongluDuan = true;
	private bool isInGonglu = true;
	
	private Vector3 vector3Value1 = Vector3.zero;
	private Vector3 vector3Value2 = Vector3.zero;

	public AudioSource mingdi;
	public AudioSource jingdi;
	public float mingdiJuliTotal = 20.0f;
	private float mingdiJuliCur = 0;
	private bool mingdiCan = false;
	public float mingdiAngleTotal = 120.0f;
	private float mingdiAngleCur = 0;

	private Animator animatorNPC;
	private bool isHitle = false;
	private string dongzuoName = "";

	// Use this for initialization
	void Start () {
		numLayerCar = LayerMask.NameToLayer("car");
		numLayerTerrain = LayerMask.NameToLayer("terrain");
		mask = 1<<( LayerMask.NameToLayer("car"));
		maskTerr = 1<<( LayerMask.NameToLayer("terrain"));
		NPCTransform = transform;
		wheelNum = wheelArr.Length;
		pointArrLength = pointArr.Length;
		isZhengXiang = true;

		hitState = 0;
		hitPointVector = new Vector3[4]{Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero};
		
		//init some infromation*********************begin
		if (Network.isClient)
		{
			if (aaaLight)
			{
				lightState1 = 1;
			}
			
			if (bbbLight)
			{
				lightState2 = 1;
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
		}
		//********************************end

		if (mingdi)
		{
			mingdi.loop = true;
			mingdi.playOnAwake = false;
		}

		if (jingdi)
		{
			jingdi.loop = true;
			jingdi.playOnAwake = false;
			jingdi.Play();
		}
	}
	void LateUpdate()
	{
		//transform.position = Vector3.Lerp(transform.position, vector3Value1, 0.1f);

		if (Network.isClient)
		{
			if (rigidbody.useGravity)
			{
				//rigidbody.useGravity = false;
			}
			//if (transform.name.CompareTo("CopCar(Clone)") == 0)
			//Debug.Log(transform + " "+ transform.position + " "+ vector3Value1);
			//transform.position = Vector3.Lerp(transform.position, vector3Value1, 0.1f);
			return;
		}
	}
	// Update is called once per frame
	//void Update ()
		void FixedUpdate()
	{//Debug.Log (Network.isClient + " " +transform + " " + pcvr.totalTime + " "+ transform.position);
		if (mingdi 
		    && (pcvr.uiRunState == 10 || pcvr.uiRunState == 3 || pcvr.isPassgamelevel) 
		    && mingdi.isPlaying)
		{
			mingdi.Stop();
		}

		if (jingdi 
		    && (pcvr.uiRunState == 10 || pcvr.uiRunState == 3 || pcvr.isPassgamelevel) 
		    && jingdi.isPlaying)
		{
			jingdi.Stop();
		}

		if (lightState1 > 0)
		{
			lightTimeCur1 += Time.deltaTime;
			if (lightTimeCur1 >= totalTime1)
			{
				if (lightState1 == 1)
				{
					lightState1 = 2;
					aaaLight.enabled = true;
				}
				else if (lightState1 == 2)
				{
					lightState1 = 1;
					aaaLight.enabled = false;
				}
				
				lightTimeCur1 = 0;
			}
		}
		
		if (lightState2 > 0)
		{
			lightTimeCur2 += Time.deltaTime;
			if (lightTimeCur2 >= totalTime2)
			{
				if (lightState2 == 1)
				{
					lightState2 = 2;
					bbbLight.enabled = true;
				}
				else if (lightState2 == 2)
				{
					lightState2 = 1;
					bbbLight.enabled = false;
				}
				
				lightTimeCur2 = 0;
			}
		}

		if (Network.isClient)
		{
			if (rigidbody.useGravity)
			{
				rigidbody.useGravity = false;
			}
			//transform.position = Vector3.Lerp(transform.position, vector3Value1, 0.1f);
			return;
		}

		if (hitState == 10)
		{
			if (rigidbody.isKinematic)
			{
				rigidbody.isKinematic = false;
			}
			return;
		}
		else if (hitState == 9)
		{
			fallDistance = 0;

			//if(Physics.Raycast(transform.position, -Vector3.up, out hit, 8.0f, mask.value))
			if(Physics.Raycast(specialPoint.position, -Vector3.up, out hit, 8.0f, mask.value) || Physics.Raycast(specialPoint.position, -Vector3.up, out hit, 8.0f, maskTerr.value))
			{
				fallDistance = Vector3.Distance(transform.position, hit.point);
			}

			if ((forceTimeCurrent >= forceTimeTotal && fallDistance <= 0.35f) || forceTimeCurrent >= forceTimeTotal * 3)
			{
				rigidbody.isKinematic = true;
				isMoving = false;
				hitState = 10;

				Invoke("hideNPCObj", 2.3f);
			}

			//rigidbody.AddForce(forceVector * 38, ForceMode.Acceleration);
			//rigidbody.AddForce(Vector3.up * 35, ForceMode.Acceleration);

			forceTimeCurrent += Time.deltaTime;
			return;
		}

		if (!isMoving)
		{
			return;
		}

		curDistance = Vector3.Distance(new Vector3( NPCTransform.position.x, 0, NPCTransform.position.z), new Vector3( nextPoint.position.x, 0, nextPoint.position.z));

		/*if (curDistance <= subSpeedDistance)
		{
			if (lastSpeed <= 0)
			{
				lastSpeed = speed;
			}

			speed = 0.90f * lastSpeed * (curDistance / subSpeedDistance);

			if (speed <= 2.5f)
			{
				speed = 2.5f;
			}

			isAddSpeed = false;
		}

		if (isAddSpeed)
		{
			speed += addSpeedValue * Time.deltaTime;
			if (speed >= normalSpeed)
			{
				speed = normalSpeed;
				
				isAddSpeed = false;
			}
		}*/
		//Debug.Log ("distance    " + curDistance + " " + speed);
		//Debug.Log ("curDistance  " + curDistance + " " + nextPoint + " " + curPathpointNum + " "+ curPathpointTotalNum + " " +nextPoint);

		if (mingdi && Time.frameCount % 4 == 0)
		{
			mingdiCan = false;

			for (int i = 0; i < 4; i++)
			{
				mingdiAngleCur = 200.0f;
				mingdiJuliCur = 200.0f;

				if (pcvr.truckObjTransArr[i])
				{
					mingdiJuliCur = Vector3.Distance(new Vector3( NPCTransform.position.x, 0, NPCTransform.position.z), new Vector3( pcvr.truckObjTransArr[i].position.x, 0, pcvr.truckObjTransArr[i].position.z));

					if (mingdiJuliCur <= mingdiJuliTotal)
					{
						mingdiAngleCur = Vector3.Angle(NPCTransform.forward, pcvr.truckObjTransArr[i].position - NPCTransform.position);
						if(Mathf.Abs(mingdiAngleCur) <= mingdiAngleTotal)
						{
							mingdiCan = true;
							break;
						}
					}
				}
			}

			if (mingdiCan && !mingdi.isPlaying)
			{
				mingdi.Play ();
			}
			else if (!mingdiCan && mingdi.isPlaying)
			{
				mingdi.Stop ();
			}
		}

		if (!fanxiang)
		{
			if (curDistance <= 10.1f && curPathpointNum + 1 < curPathpointTotalNum)
			{
				if (isMotuo && animatorNPC)
				{
					findNextAnimation(curPathpointNum);
				}

				//next point
				curPathpointNum ++;
				
				if (curPathpointNum < curPathpointTotalNum)
				{
					nextPoint = pathpointArr[curPathpointNum];
				}
				else
				{
					//find next path
					curPathpointNum = 0;
					nextPoint = pathpointArr[curPathpointNum];
				}

				isAddSpeed = true;
				lastSpeed = 0;
			}
			else if (curDistance <= 10.1f && curPathpointNum + 1 >= curPathpointTotalNum)
			{
				Destroy(gameObject);
				return;
				//find the first point again
				/*if (curPathpointTotalNum <= 1)
				{
					//if only has one point
					isMoving = false;
				}
				else
				{
					curPathpointNum = 0;
					nextPoint = pathpointArr[curPathpointNum];
				}

				isAddSpeed = true;
				lastSpeed = 0;

				return;*/
			}
		}
		else
		{
			if (isZhengXiang)
			{
				if (curDistance <= 10.1f && curPathpointNum + 1 < curPathpointTotalNum)
				{
					if (isMotuo && animatorNPC)
					{
						findNextAnimation(curPathpointNum);
					}

					//next point
					curPathpointNum ++;
					
					if (curPathpointNum < curPathpointTotalNum)
					{
						nextPoint = pathpointArr[curPathpointNum];
					}
					else
					{
						//find next path
						curPathpointNum = 0;
						nextPoint = pathpointArr[curPathpointNum];
					}

					isAddSpeed = true;
					lastSpeed = 0;
				}
				else if (curDistance <= 10.1f && curPathpointNum + 1 >= curPathpointTotalNum)
				{
					Destroy(gameObject);
					return;
					//find the first point again
					/*if (curPathpointTotalNum <= 1)
					{
						//if only has one point
						isMoving = false;
					}
					else
					{
						nextPoint = pathpointArr[curPathpointNum];
						isZhengXiang = false;
					}

					isAddSpeed = true;
					lastSpeed = 0;
					return;*/
				}
			}
			else
			{
				if (curDistance <= 10.1f && curPathpointNum - 1 >= 0)
				{
					if (isMotuo && animatorNPC)
					{
						findNextAnimation(curPathpointNum);
					}

					//next point
					curPathpointNum --;
					
					if (curPathpointNum < curPathpointTotalNum)
					{
						nextPoint = pathpointArr[curPathpointNum];
					}
					else
					{
						//find next path
						curPathpointNum = 0;
						nextPoint = pathpointArr[curPathpointNum];
					}

					isAddSpeed = true;
					lastSpeed = 0;
				}
				else if (curDistance <= 10.1f && curPathpointNum - 1 < 0)
				{
					Destroy(gameObject);
					return;
					//find the first point again
					/*if (curPathpointTotalNum <= 1)
					{
						//if only has one point
						isMoving = false;
					}
					else
					{
						nextPoint = pathpointArr[curPathpointNum];
						isZhengXiang = true;
					}

					isAddSpeed = true;
					lastSpeed = 0;
					return;*/
				}
			}
		}

		if (nextPoint)
		{
			NPCTransform.forward = Vector3.Lerp(NPCTransform.forward, nextPoint.position - NPCTransform.position, Time.deltaTime * NPCRotateSpeed);
			rigidbody.velocity = Vector3.Normalize( NPCTransform.forward ) * speed;

			if(Physics.Raycast(specialPoint.position, -Vector3.up, out hit, 8.0f, mask.value) || Physics.Raycast(specialPoint.position, -Vector3.up, out hit, 8.0f, maskTerr.value))
			{
				NPCTransform.position = new Vector3(NPCTransform.position.x, hit.point.y + lidiDistance, NPCTransform.position.z);
			}
			
			hitPointVector[0] = Vector3.zero;	//forward
			hitPointVector[1] = Vector3.zero;	//back
			
			if(Physics.Raycast(pointArr[0].position, -Vector3.up, out hit, 8.0f, mask.value) || Physics.Raycast(pointArr[0].position, -Vector3.up, out hit, 8.0f, maskTerr.value))
			{
				hitPointVector[0] = hit.point;
			}
			
			if(Physics.Raycast(pointArr[1].position, -Vector3.up, out hit, 8.0f, mask.value) || Physics.Raycast(pointArr[1].position, -Vector3.up, out hit, 8.0f, maskTerr.value))
			{
				hitPointVector[1] = hit.point;
			}
			
			testDirection = Vector3.Normalize (hitPointVector[0] - hitPointVector[1]);
			NPCTransform.forward = Vector3.Lerp(NPCTransform.forward, testDirection, 0.75f);
			
			hitPointVector[2] = Vector3.zero;	//left
			hitPointVector[3] = Vector3.zero;	//right
			
			if(Physics.Raycast(pointArr[2].position, -Vector3.up, out hit, 8.0f, mask.value) || Physics.Raycast(pointArr[2].position, -Vector3.up, out hit, 8.0f, maskTerr.value))
			{
				hitPointVector[2] = hit.point;
			}
			
			if(Physics.Raycast(pointArr[3].position, -Vector3.up, out hit, 8.0f, mask.value) || Physics.Raycast(pointArr[3].position, -Vector3.up, out hit, 8.0f, maskTerr.value))
			{
				hitPointVector[3] = hit.point;
			}
			
			testDirectionR = Vector3.Normalize (hitPointVector[3] - hitPointVector[2]);
			angle = Vector3.Angle (testDirectionR, NPCTransform.right);

			if(hitPointVector[3].y - hitPointVector[2].y > 0.01f && (angle > 1 && angle <45))
			{
				NPCTransform.eulerAngles = new Vector3 (NPCTransform.eulerAngles.x, NPCTransform.eulerAngles.y, angle);
			}
			else if(hitPointVector[3].y - hitPointVector[2].y < -0.01f && (angle > 1 && angle <45))
			{
				NPCTransform.eulerAngles = new Vector3 (NPCTransform.eulerAngles.x, NPCTransform.eulerAngles.y, -angle);
			}
			else
			{
				angle = 0;
				NPCTransform.eulerAngles = new Vector3 (NPCTransform.eulerAngles.x, NPCTransform.eulerAngles.y, -angle);
			}
		}

		for (int i=0; i < wheelNum; i++)
		{
			wheelArr[i].Rotate(Vector3.right * rotateSpeed * Time.deltaTime);
		}
		
		if (Network.isServer)
		{
			vector3Value1 = NPCTransform.position;
			vector3Value2 = new Vector3(NPCTransform.eulerAngles.x,NPCTransform.eulerAngles.y,NPCTransform.eulerAngles.z);
			
			NPCTransform.networkView.RPC("RefeshTransformNPC", RPCMode.AllBuffered, vector3Value1, vector3Value2, Network.player);
			
			return;
		}
	}
	
	public void initNPCInfor(Transform pathObj, float speedT, bool fanxiangT)
	{
		if (isMotuo)
		{
			animatorNPC = GetComponent<Animator>();

			if (animatorNPC)
			{
				animatorNPC.enabled = true;
			}
		}

		curPathpointTotalNum = -1;
		curPathpointNum = 0;
		NPCTransform = transform;
		fanxiang = fanxiangT;
		curPath = pathObj;

		//init some infromation*********************begin
		if (aaaLight)
		{
			lightState1 = 1;
		}

		if (bbbLight)
		{
			lightState2 = 1;
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
		//********************************end

		if (speedT > 7.2f)
		{
			speed = speedT * 0.278f;
			//normalSpeed = speed;
		}
		//speed = speed * 0.3f;	//testttttttttttttttttt
		if (curPath)
		{
			curPathpointTotalNum = curPath.childCount;
		}

		int findIndex = 0;

		if (curPathpointTotalNum > 0)
		{
			pathpointArr = new Transform[curPathpointTotalNum];
			animationNameStr = new string[curPathpointTotalNum];

			for (int i=0; i < curPathpointTotalNum; i++)
			{
				findIndex = Convert.ToInt32(curPath.GetChild(i).name);
				pathpointArr[findIndex] = curPath.GetChild(i);

				if (isMotuo)
				{
					if (curPath.GetChild(i).GetComponent<nodeNPCPath>())
					{
						animationNameStr[findIndex] = curPath.GetChild(i).GetComponent<nodeNPCPath>().getNodeActionName();
					}
					else
					{
						animationNameStr[findIndex] = "";
					}
				}
			}
			
			controlYan(true, 3);

			if (!isGongluDuan || !isInGonglu)
			{
				controlChentu(true, 3);
			}
			else
			{
				controlChentu(false, 31);
			}
		}
		else
		{
			isMoving = false;
			
			controlYan(false, 4);
			
			controlChentu(false, 4);
			return;
		}
		
		nextPoint = pathpointArr[curPathpointNum];
		transform.LookAt (nextPoint.position);
		isMoving = true;

		if (isMotuo && animatorNPC)
		{
			doAnimationTri ("runTri");
		}
	}

	public void hitByPlayer(Vector3 forceVec)
	{
		if (isHitle)
		{
			return;
		}
		
		isHitle = true;

		if (Network.isClient)
		{
			NPCTransform.networkView.RPC("hitByPlayerRPC", RPCMode.AllBuffered, forceVec);
			return;
		}

		hitResult (forceVec);
	}

	[RPC]
	public void hitByPlayerRPC(Vector3 forceVec)
	{
		if (Network.isServer)
		{
			hitResult (forceVec);
		}
	}

	void hitResult(Vector3 forceVec)
	{//Debug.Log ("hitResulthitResulthitResulthitResult " + transform);
		if (mingdi && mingdi.isPlaying)
		{
			mingdi.Stop ();
		}

		if (isMotuo)
		{
			if (animatorNPC)
			{
				rigidbody.isKinematic = true;
				doAnimationTri("deadTri");
			}

			hitState = 10;
			
			controlYan (false, 5);
			controlChentu(false, 5);

			if (Network.isServer)
			{
				NPCTransform.networkView.RPC("changeGNORELayerRPC", RPCMode.AllBuffered);
			}

			ChangeLayersRecursively(gameObject.transform, "IGNORE");

			Invoke("hideNPCObj", 3.0f);
		}

		topSpeed = rigidbody.velocity.magnitude * 0.5f;
		hitState = 9;
		forceVector = forceVec;
		forceTimeCurrent = 0;
		rigidbody.isKinematic = false;
		
		forceValue = forceValueFly;
		forceTimeTotal = forceTimeLong;

		controlYan (false, 5);
		controlChentu(false, 5);
	}
	
	[RPC]
	public void changeGNORELayerRPC()
	{
		ChangeLayersRecursively(gameObject.transform, "IGNORE");
	}

	void ChangeLayersRecursively(Transform trans, String LayerName)
	{return;
		/*trans.gameObject.layer = LayerMask.NameToLayer(LayerName);

		for (int i = 0; i < trans.childCount; i++)
		{
			trans.GetChild(i).gameObject.layer = LayerMask.NameToLayer(LayerName);
			ChangeLayersRecursively(trans.GetChild(i), LayerName);
		}*/
	}
	
	public void NPCOver()
	{
		isMoving = false;
	}
	
	public void OnTriggerEnter(Collider other)
	{
		if (Network.isClient)
		{
			return;
		}

		if (other.transform.root.gameObject.layer == numLayerCar ||other.transform.root.gameObject.layer == numLayerTerrain )
		{//Debug.Log ("O11nTerOnReturn " + other.gameObject + " " + other.gameObject.name + " "+ other.transform.position );
			if (other.transform.root.gameObject.name.CompareTo("ff") != 0)
			{
				isInGonglu = true;

				controlChentu(false, 6);
			}
			return;
		}

		if (other.transform.GetComponent<triTruckInfor>()
			&& other.transform.GetComponent<triTruckInfor>().getTriggerName().CompareTo("enterGongluTri") == 0)
		{
			isGongluDuan = true;
			isInGonglu = true;

			controlChentu(false, 7);
		}
		else if (other.transform.GetComponent<triTruckInfor>()
			&& other.transform.GetComponent<triTruckInfor>().getTriggerName().CompareTo("leaveGongluTri") == 0)
		{
			isGongluDuan = false;
			isInGonglu = false;

			controlChentu(true, 8);
		}
	}
	
	void OnTriggerStay( Collider other )
	{
		if (Network.isClient)
		{
			return;
		}

		if (other.transform.root.gameObject.layer == numLayerCar ||other.transform.root.gameObject.layer == numLayerTerrain )
		{
			if (other.gameObject.name.CompareTo("ff") != 0 && !isInGonglu)
			{
				isInGonglu = true;
				controlChentu(false, 202);
			}
			return;
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if (Network.isClient)
		{
			return;
		}

		if (other.transform.root.gameObject.layer == numLayerCar ||other.transform.root.gameObject.layer == numLayerTerrain )
		{
			//Debug.Log("huiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiii " + other.gameObject.name);

			if (other.transform.root.gameObject.name.CompareTo("ff") != 0)
			{
				isInGonglu = false;

				controlChentu(true, 9);
			}
		}
	}
	
	public Transform[] pointArr;		//the points( may be 4 points) on the AI 1-forward; 2-back; 3-left; 4-right
	private int pointArrLength = 0;		//the total number of the points on the AI
	private int findIndex = 0;
	private float findDistance = 0;

	//hit here
	//1-forward; 2-back; 3-left; 4-right
	public int getHitDirection(Vector3 hitPoint)
	{
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

	void controlYan(bool isActive, int index)
	{
		if (Network.isServer)
		{
			NPCTransform.networkView.RPC("controlYanRPC", RPCMode.AllBuffered, isActive, index);
			return;
		}

		for (int i=0; i < yanNumber; i++)
		{
			yan[i].SetActive(isActive);
		}
	}

	void controlChentu(bool isActive, int index)
	{
		if (!gameObject.activeSelf)
		{
			return;
		}

		if (Network.isServer)
		{
			NPCTransform.networkView.RPC("controlChentuRPC", RPCMode.AllBuffered, isActive, index);
			return;
		}

		for (int i=0; i < chenTuNumber; i++)
		{
			chenTu[i].SetActive(isActive);
		}
	}
	
	//about yan and chentu
	[RPC]
	public void controlYanRPC(bool isActive, int index)
	{
		for (int i=0; i < yanNumber; i++)
		{
			yan[i].SetActive(isActive);
		}
	}
	
	[RPC]
	public void controlChentuRPC(bool isActive, int index)
	{
		for (int i=0; i < chenTuNumber; i++)
		{
			chenTu[i].SetActive(isActive);
		}
	}

	//link
	[RPC]
	void RefeshTransformNPC(Vector3 vec1, Vector3 vec2, NetworkPlayer playerT)
	{
		if (Network.isServer)
		{
			return;
		}
		
		vector3Value1 = vec1;
		transform.localEulerAngles = vec2;
	}
	string findName = "";
	void findNextAnimation(int index)
	{
		findName = "";
		findName = animationNameStr[index];

		if (findName.CompareTo("") != 0)
		{
			doAnimationTri (findName);
		}
	}

	void doAnimationTri(string name)
	{
		if (name.CompareTo(dongzuoName) == 0 && name.CompareTo("runTri") == 0)
		{
			return;
		}

		if (Network.isServer && networkView)
		{
			networkView.RPC("doAnimationNPCRPCTri", RPCMode.AllBuffered, name);
		}
		else
		{
			animatorNPC.SetTrigger(name);

			if (name.CompareTo("deadTri") == 0)
			{
				Invoke("hideAnimator", 3);
			}
		}
		
		dongzuoName = name;
	}

	void hideAnimator()
	{
		animatorNPC.enabled = false;
	}
	
	[RPC]
	void doAnimationNPCRPCTri(string aniName)
	{
		if (GetComponent<Animator>())
		{
			GetComponent<Animator>().SetTrigger(aniName);
		}
	}

	void hideNPCObj()
	{
		if (Network.isServer && networkView && transform && gameObject.activeSelf)
		{
			NPCTransform.networkView.RPC("changeGNORELayerRPC", RPCMode.AllBuffered);
			networkView.RPC("hideNPCObjRPC", RPCMode.AllBuffered);
		}
		else if (transform && gameObject.activeSelf)
		{
			ChangeLayersRecursively(gameObject.transform, "IGNORE");
			transform.root.gameObject.SetActive(false);
		}
	}

	[RPC]
	void hideNPCObjRPC()
	{
		transform.root.gameObject.SetActive(false);
	}
	
	public void addDaojuPengzhuang(Vector3 forceVec, bool isNPC, Vector3 chetouPointT)
	{
		if (Network.isClient)
		{
			NPCTransform.networkView.RPC("hitByPlaaddDaojuPengzhuangRPC", RPCMode.AllBuffered, forceVec, isNPC, chetouPointT);
			return;
		}
		
		addDaojuPengzhuangResult (forceVec, isNPC, chetouPointT);
	}
	
	[RPC]
	public void hitByPlaaddDaojuPengzhuangRPC(Vector3 forceVec, bool isNPC, Vector3 chetouPointT)
	{
		addDaojuPengzhuangResult (forceVec, isNPC, chetouPointT);
	}
	
	void addDaojuPengzhuangResult(Vector3 forceVec, bool isNPC, Vector3 chetouPointT)
	{
		gameObject.AddComponent<daojuPengzhuang>();
		
		transform.GetComponent<daojuPengzhuang>().daojuPengshangle(forceVec, isNPC, chetouPointT);
	}
}
