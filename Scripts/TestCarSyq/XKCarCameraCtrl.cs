using UnityEngine;
using System.Collections;

public class XKCarCameraCtrl : MonoBehaviour
{public Transform aimTrans;
	public Transform target;
	public float angleV = 0;
	public float smooth = 0.05f;
	public float distance = 5.0f;
	public float haight = 5.0f;

	public float angleVPerson1 = 0;
	public float smoothPerson1 = 0.05f;
	public float distancePerson1 = 5.0f;
	public float haightPerson1 = 5.0f;

	private float angleVPerson3 = 0;
	private float smoothPerson3 = 0.05f;
	private float distancePerson3 = 5.0f;
	private float haightPerson3 = 5.0f;

	private bool isThirdPerson = true;
	
	private float yVelocity = 0.0f;
	private float xVelocity = 0.0f;
	Rigidbody RigCar;
	float CarSteerVal;
	public LayerMask lineOfSightMask = 0;

	private float startTime = 0;
	private bool stopFollow = false;

	public float speed = 8.0f;

	//about change field of view
	private bool isSpeeding = false;
	private bool isSpeedingTruck = false;
	private float changeEverySecond = 0;
	private float fieldOfViewSpeed = 105.0f;
	private float changeFieldTime = 0.8f;		//change time(s)
	private float changeCurTime = 0.0f;
	private bool isHuifuzhong = false;
	private float huifuTime = 0.7f;				//hui fu time
	private bool isAdd =false;
	private float changeEverySecondHF = 30.0f;
	private float changeEverySecondT = 0.0f;
	private float firstPartTime = 0.7f;			//di yi duan shi jian
	private float coefPart1 = 0.4f;				//di yi duan xi shu
	private float coefPart2 = 3.0f;				//di er duan xi shu
	public bool isPart2 = false;

	void Start()
	{
		if (target)
			RigCar = target.GetComponent<Rigidbody>();
		
		angleVPerson3 = angleV;
		smoothPerson3 = smooth;
		distancePerson3 = distance;
		haightPerson3 = haight;

		isThirdPerson = true;
		isPart2 = false;
	}
	
	void Awake()
	{
		startTime = Time.time;
		pcvr.XKCarCameraSObj = GetComponent<XKCarCameraCtrl>();
		pcvr.XKCarCameraTranform = transform;
	}

	float carSpeed = 0.0f;
	Vector3 direction = Vector3.zero;
	float targetDistance = 0f;
	Vector3 posFollow = Vector3.zero;
	int dirSt = 0;
	//float dirVal = 0f;
	float yAngle = 0f;
	float xAngle = 0f;

	void FixedUpdate()
	{//Debug.Log (target.position.y + " "+ haight + " "+ transform.position.x + " "+ transform.position.y + " "+  transform.position.z);
		if (!target || stopFollow || !RigCar)
		{
			return;
		}

		if (!Network.isServer && !Network.isClient)
		{
			updatePart1();
			updatePart2();
		}
		else if (Network.isClient)
		{
			updatePart1();
			updatePart2();		//test
		}
	}
	void LateUpdate1()
	{
		if (!target || stopFollow || !RigCar)
		{
			return;
		}

		if (Network.isClient)
		{
			updatePart2();
		}
	}
	private float fieldSpeed = 0.0f;
	void updatePart1()
	{
		carSpeed = RigCar.velocity.magnitude * 3.6f;

		if (!isSpeeding)
		{
			fieldSpeed = Mathf.Clamp(carSpeed / 20.0f + 60.0f, 60, 80);
			//camera.fieldOfView = Mathf.Clamp(carSpeed / 20.0f + 60.0f, 60, 80);
			if (!isHuifuzhong)
			{
				camera.fieldOfView = fieldSpeed;
			}
			else if (carSpeed < 60.0f || Mathf.Abs(camera.fieldOfView - fieldSpeed) < 0.2f)
			{
				isHuifuzhong = false;
			}
			else
			{
				if (camera.fieldOfView < fieldSpeed)
				{
					camera.fieldOfView += changeEverySecondHF * Time.deltaTime;
					
					if (camera.fieldOfView >= fieldSpeed)
					{
						camera.fieldOfView = fieldSpeed;
						isHuifuzhong = false;
					}
				}
				else
				{
					camera.fieldOfView -= changeEverySecondHF * Time.deltaTime;
					
					if (camera.fieldOfView <= fieldSpeed)
					{
						camera.fieldOfView = fieldSpeed;
						isHuifuzhong = false;
					}
				}
			}
		}
		else
		{
			//camera.fieldOfView = Mathf.Clamp(carSpeed / 20.0f + 60.0f, 60, 100);
			changeCurTime += Time.deltaTime;
			//paowuxian
			//changeEverySecondT = (changeEverySecond / (changeFieldTime * changeFieldTime)) * (changeCurTime * changeCurTime);

			//two lines for this
			if (changeCurTime < firstPartTime)
			{
				changeEverySecondT = coefPart1 * changeCurTime * changeEverySecond;
			}
			else
			{
				changeEverySecondT = coefPart2 * changeCurTime * changeEverySecond;
				isPart2 = true;
			}

			if (camera.fieldOfView < fieldOfViewSpeed)
			{
				camera.fieldOfView += changeEverySecondT * Time.deltaTime;
			}

			if (camera.fieldOfView > fieldOfViewSpeed)
			{
				camera.fieldOfView = fieldOfViewSpeed;
			}
			//Debug.Log(changeCurTime +" "+  changeEverySecondT + " "+ camera.fieldOfView + " "+ fieldOfViewSpeed);
		}
		//Debug.Log (carSpeed + " "+ isSpeeding + " " + isHuifuzhong + " " + camera.fieldOfView + " "+ Time.time);
		direction = transform.rotation * (-Vector3.forward);
		targetDistance = AdjustLineOfSight(target.position + new Vector3(0, haight, 0), direction);
		posFollow = target.position + new Vector3(0, haight, 0) + direction * targetDistance;
		//posFollow = new Vector3 (posFollow.x, posFollow.y, target.position.z - distance);
		//posFollow = new Vector3 (posFollow.x, (target.position.y + haight), posFollow.z);
		//Debug.Log ("targetDistance " + Vector3.Distance(target.position, transform.position) + " ( " +direction.x + " " + direction.y + " " + direction.z + " ) " + targetDistance + " "+ (direction * targetDistance));
		dirSt = CarSteerVal >= 0f ? 1 : -1;
		
		if (CarSteerVal == 0f) {
			dirSt = 0;
		}
		
		// Damp angle from current y-angle towards target y-angle
		yAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y,
		                               target.eulerAngles.y, ref yVelocity, smooth);
		
		xAngle = Mathf.SmoothDampAngle(transform.eulerAngles.x,
		                               target.eulerAngles.x + angleV, ref xVelocity, smooth);
	}

	void updatePart2()
	{		
		// Look at the target
		/*transform.eulerAngles = new Vector3(xAngle, yAngle, 0);
		transform.position = posFollow;
		return;*/
		if (isThirdPerson)
		{
			transform.eulerAngles = new Vector3(xAngle, yAngle, 0);
			transform.position = posFollow;
		}
		else
		{
			transform.eulerAngles = new Vector3(xAngle, yAngle, 0);
			transform.parent = aimTrans;
			transform.localPosition = Vector3.zero;
			//transform.localRotation = Quaternion.identity;
		}
	}

	public float backPos = -15f;
	RaycastHit hit;

	float AdjustLineOfSight (Vector3 target , Vector3 direction)
	{
		if (distance > 0 && Physics.Raycast (target, direction,out hit, distance, lineOfSightMask.value))
			return hit.distance;
		else
			return distance;
	}

	public void SetCarSteerVal(float val)
	{
		CarSteerVal = val;
	}
	
	public void setZhujue(Transform truck, Transform cameraPointRest, Transform firstPersonPoint)
	{
		target = truck;
		RigCar = target.GetComponent<Rigidbody>();

		if (firstPersonPoint)
			aimTrans = firstPersonPoint;
		else
			aimTrans = truck;
	}
	
	public void setStopFollow(bool flag)
	{
		stopFollow = flag;
	}

	public void setToFirstThirdPerson()
	{
		isThirdPerson = !isThirdPerson;

		if (isThirdPerson)
		{
			angleV = angleVPerson3;
			smooth = smoothPerson3;
			distance = distancePerson3;
			haight = haightPerson3;
			transform.parent = null;

			isSpeeding = isSpeedingTruck;
		}
		else
		{
			angleV = angleVPerson1;
			smooth = smoothPerson1;
			distance = distancePerson1;
			haight = haightPerson1;

			isHuifuzhong = false;
			isSpeeding = false;
		}
	}

	public void removeParent()
	{
		transform.parent = null;
	}

	void FirstPersonJiasu()
	{
		if (isSpeedingTruck)
		{
			isPart2 = true;
		}
	}

	public void setJiasu()
	{
		if (!isThirdPerson)
		{
			isHuifuzhong = false;
			isSpeeding = false;
			isSpeedingTruck = true;
			
			CancelInvoke ("FirstPersonJiasu");
			Invoke ("FirstPersonJiasu", 0.8f);
			return;
		}

		CancelInvoke ("FirstPersonJiasu");
		isSpeeding = true;
		isSpeedingTruck = true;
		changeEverySecond = (fieldOfViewSpeed - camera.fieldOfView) / changeFieldTime;
		//Debug.Log ("changeEverySecondchangeEverySecondchangeEverySecondchangeEverySecond " + changeEverySecond);
	}
	
	public void setZhengchang()
	{
		changeCurTime = 0.0f;
		isPart2 = false;

		if (!isThirdPerson)
		{
			isHuifuzhong = false;
			isSpeeding = false;
			isSpeedingTruck = false;
			return;
		}

		isSpeeding = false;
		isSpeedingTruck = false;
		
		float carSpeedT = RigCar.velocity.magnitude * 3.6f;
		float aaa = Mathf.Clamp(carSpeedT / 20.0f + 60.0f, 60, 80);

		if (carSpeedT < 100.0f || Mathf.Abs(camera.fieldOfView - aaa) < 0.2f)
		{
			isHuifuzhong = false;
		}
		else
		{
			changeEverySecondHF = Mathf.Abs(aaa - camera.fieldOfView) / huifuTime;
			isHuifuzhong = true;
		}

		return;
		/*
		if (camera.fieldOfView < aaa)
		{
			isAdd = true;
			changeEverySecondHF = (aaa - camera.fieldOfView) / huifuTime;
			isHuifuzhong = true;
		}
		else if (camera.fieldOfView > aaa)
		{
			isAdd = false;
			changeEverySecondHF = (camera.fieldOfView - aaa) / huifuTime;
			isHuifuzhong = true;
		}
		else if (camera.fieldOfView > aaa)
		{
			isHuifuzhong = false;
		}*/
	}

	public GUISkin GUISkin;
	public void OnGUI1()
	{
		if (Network.isServer || Network.isClient)
		{
			return;
		}

		GUI.skin = GUISkin;
		
		if (GUI.Button(new Rect(20, 136, 100, 20), "reset scence"))
		{
			Application.LoadLevel(Application.loadedLevel);	//current level again
		}
		
		GUI.Box(new Rect(5, 100, 150, 150), "");
		
		GUI.Label(new Rect(10, 107, 200, 50), (Time.time - startTime).ToString());

		GUI.Label(new Rect(10, 170, 200, 50), pcvr.mBikePowerMax.ToString());
		GUI.Label(new Rect(10, 185, 200, 50), pcvr.mGetSteer.ToString());
		GUI.Label(new Rect(5, 200, 200, 50), pcvr.playerTruckSObj.fangxiangpanDoudongIndex.ToString());
		GUI.Label(new Rect(25, 200, 200, 50), pcvr.GetInstance().liFankuiQiangduUse.ToString());
		GUI.Label(new Rect(70, 200, 200, 50), pcvr.zhendongDengjiFXP.ToString());
		GUI.Label(new Rect(90, 200, 200, 50), pcvr.GetInstance().liFankuiQiangdu.ToString());
		GUI.Label(new Rect(110, 200, 200, 50), pcvr.zhendongQiangdu.ToString());
		GUI.Label(new Rect(130, 200, 200, 50), pcvr.zhendongShijian.ToString());

		if (pcvr.buffer4[0] == 1 && pcvr.buffer4[1] == 1)
			GUI.Label(new Rect(20, 215, 200, 50), "chong     chong");
		else if (pcvr.buffer4[0] == 0 && pcvr.buffer4[1] == 0)
			GUI.Label(new Rect(20, 215, 200, 50), "fang     fang");
		else if (pcvr.buffer4[0] == 0 && pcvr.buffer4[1] == 1)
			GUI.Label(new Rect(20, 215, 200, 50), "fang     chong");
		else if (pcvr.buffer4[0] == 1 && pcvr.buffer4[1] == 0)
			GUI.Label(new Rect(20, 215, 200, 50), "chong     fang");
		
		if (pcvr.buffer4[3] == 1 && pcvr.buffer4[2] == 1)
			GUI.Label(new Rect(20, 230, 200, 50), "chong     chong");
		else if (pcvr.buffer4[3] == 0 && pcvr.buffer4[2] == 0)
			GUI.Label(new Rect(20, 230, 200, 50), "fang     fang");
		else if (pcvr.buffer4[3] == 0 && pcvr.buffer4[2] == 1)
			GUI.Label(new Rect(20, 230, 200, 50), "fang     chong");
		else if (pcvr.buffer4[3] == 1 && pcvr.buffer4[2] == 0)
			GUI.Label(new Rect(20, 230, 200, 50), "chong     fang");
	}
}