using UnityEngine;
using System;
using System.Collections;

public class serverCameraScript : MonoBehaviour {

	private Transform lookatObj = null;	//ding dian
	public bool isLookat = false;
	private float dingdianMiaozhunSpeed = 40.0f;
	private static float timeTotal = 1.5f;
	private float timeNow = 0.0f;
	private int curPlayerIndex = -1;
	private int inPlayerIndex = -1;
	public Transform paretnObj = null;			//dingdian
	public Transform FollowParetnObj = null;	//player point
	private Transform lastInParent = null;
	
	float yAngle = 0f;
	float xAngle = 0f;
	private float yVelocity = 0.0f;
	private float xVelocity = 0.0f;
	public float smooth = 0.15f;

	// Use this for initialization
	void Awake () {
		timeNow = 0.0f;
		lookatObj = null;
		isLookat = false;
		curPlayerIndex = -1;
		inPlayerIndex = -1;

		pcvr.serCameraSObj = GetComponent<serverCameraScript>();
	}
	Vector3 forwardVal = Vector3.zero;
	void FixedUpdate1()
	{
		if (!FollowParetnObj)
		{
			return;
		}

		// Damp angle from current y-angle towards target y-angle
		yAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y,
		                               FollowParetnObj.eulerAngles.y, ref yVelocity, smooth);
		
		xAngle = Mathf.SmoothDampAngle(transform.eulerAngles.x,
		                               FollowParetnObj.eulerAngles.x, ref xVelocity, smooth);
	}
	void FixedUpdate()
	//void Update()
	{
		if (lookatObj && isLookat)
		{
			forwardVal = lookatObj.position - transform.position;
			
			if (forwardVal != Vector3.zero)
			{
				Quaternion rotTmp = Quaternion.LookRotation(forwardVal);
				parentRotationNL = Quaternion.Lerp(parentRotationNL, rotTmp, Time.deltaTime * dingdianMiaozhunSpeed);
			}
		}
		else if (FollowParetnObj)
		{
			parentRotationN = Quaternion.Lerp(parentRotationN, FollowParetnObj.rotation, Time.deltaTime * Rotationtime2T);
			parentPosN = Vector3.Lerp(parentPosN, FollowParetnObj.position, Time.deltaTime * Postime1T);
		}
	}
	
	private Vector3 parentPosNL;
	private Quaternion parentRotationNL;
	void LateUpdate()
	{
		if (lookatObj && isLookat)
		{
			transform.rotation = parentRotationNL;
		}
		else if (paretnObj && !isLookat)
		{
			transform.parent = paretnObj;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
		}
		else if (FollowParetnObj)
		{
			//transform.eulerAngles = new Vector3(xAngle, yAngle, 0);
			//transform.rotation = Quaternion.Lerp(transform.rotation, parentRotation, Time.deltaTime * Rotationtime2T);
			//transform.position = Vector3.Lerp(transform.position, parentPos, Time.deltaTime * Postime1T);

			//transform.rotation = parentRotationN;
			//transform.position = parentPosN;

			transform.parent = FollowParetnObj;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
		}
	}

	public float Postime1T = 80.0f;
	public float Rotationtime2T = 80.0f;
	private Vector3 parentPos;
	private Quaternion parentRotation;
	private Vector3 parentPosN;
	private Quaternion parentRotationN;

	public void setFollowPlayer(string TriName, int pointIndex, int playerIndex)
	{//Debug.Log("setFollowPlayersetFollowPlayersetFollowPlayer  " + Time.time + " " + timeNow + " " + timeTotal + " " + playerIndex);
		if (!pcvr.aFirstScriObj.triggerObjParent)
		{
			return;
		}
		
		if (Time.time - timeNow < timeTotal)
		{
			return;
		}

		Transform tempTri = null;
		Transform ParentObjTri = pcvr.aFirstScriObj.triggerObjParent;
		int lenTri = ParentObjTri.childCount;
		
		for (int i = 0; i < lenTri; i++)
		{
			if (ParentObjTri.GetChild(i) && ParentObjTri.GetChild(i).name.CompareTo(TriName) == 0 )
			{
				tempTri = ParentObjTri.GetChild(i);
				break;
			}
		}

		if (!tempTri)
		{
			return;
		}
		
		//ffffffffffffffffffffffffffff change
		FollowParetnObj = pcvr.gameServerObjSriptArr [playerIndex].getPointTransform (pointIndex);
		parentPosN = FollowParetnObj.position;
		parentRotationN = FollowParetnObj.rotation;
		//transform.localPosition = Vector3.zero;
		//transform.localRotation = Quaternion.identity;
		//transform.parent = FollowParetnObj;
		transform.parent = null;
		transform.position = FollowParetnObj.position;
		transform.rotation = FollowParetnObj.rotation;
		curPlayerIndex = playerIndex;
		isLookat = false;
		lookatObj = null;
		paretnObj = null;
		timeNow = Time.time;
	}

	public void setInPointObj(string TriName, string Pointname, bool look, int playerIndex, Transform ParentObj)
	{//Debug.Log("setInPointObjsetInPointObj  " + TriName + " "+ Pointname + " "+ Time.time + " " + timeNow + " " + timeTotal + " "  +look + " " + playerIndex);
		if (!pcvr.aFirstScriObj.triggerObjParent)
		{
			return;
		}
		
		if (Time.time - timeNow < timeTotal)
		{
			return;
		}
		
		Transform tempTri = null;
		Transform ParentObjTri = pcvr.aFirstScriObj.triggerObjParent;
		int lenTri = ParentObjTri.childCount;
		
		for (int i = 0; i < lenTri; i++)
		{
			if (ParentObjTri.GetChild(i) && ParentObjTri.GetChild(i).name.CompareTo(TriName) == 0 )
			{
				tempTri = ParentObjTri.GetChild(i);
				break;
			}
		}
		
		if (!tempTri)
		{
			return;
		}

		lastInParent = tempTri;

		Transform temp = null;
		int len = ParentObj.childCount;
		
		for (int i=0; i<len; i++)
		{
			if (ParentObj.GetChild(i) && ParentObj.GetChild(i).name.CompareTo(Pointname) == 0 )
			{
				temp = ParentObj.GetChild(i);
				break;
			}
		}

		FollowParetnObj = null;
		transform.parent = temp;
		paretnObj = temp;
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		isLookat = look;
		lookatObj = pcvr.truckObjTransArr [playerIndex];
		curPlayerIndex = playerIndex;
		inPlayerIndex = playerIndex;
		timeNow = Time.time;
		transform.LookAt(lookatObj.position);
	}
}
