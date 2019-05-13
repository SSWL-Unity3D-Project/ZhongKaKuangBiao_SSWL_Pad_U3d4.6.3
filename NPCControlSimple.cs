using UnityEngine;
using System;
using System.Collections;

public class NPCControlSimple : MonoBehaviour {
	
	private bool hasAnimator = false;
	public Transform specialPoint = null;
	public float lidiDistance = 0.5f;
	
	private Transform curPath = null;
	private bool isMoving = false;
	private float speed = 7.5f;
	private float NPCRotateSpeed = 0.12f;
	private Transform nextPoint = null;
	private Transform NPCTransform = null;
	private float curDistance = 0.0f;
	private int curPathpointTotalNum = 0;
	private int curPathpointNum = 0;
	
	//path
	private Transform[] pathpointArr;
	private string[] animationNameStr;
	
	//terrain
	private  LayerMask mask;
	private  LayerMask maskTerr;
	private RaycastHit hit;
	
	private int numLayerCar = -1;
	private int numLayerTerrain = -1;
	
	private Animator animatorNPC;
	private string dongzuoName = "";
	
	// Use this for initialization
	void Start () {
		numLayerCar = LayerMask.NameToLayer("car");
		numLayerTerrain = LayerMask.NameToLayer("terrain");
		mask = 1<<( LayerMask.NameToLayer("car"));
		maskTerr = 1<<( LayerMask.NameToLayer("terrain"));
		NPCTransform = transform;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (!isMoving)
		{
			return;
		}
		
		curDistance = Vector3.Distance(new Vector3( NPCTransform.position.x, 0, NPCTransform.position.z), new Vector3( nextPoint.position.x, 0, nextPoint.position.z));

		if (curDistance <= 10.1f && curPathpointNum + 1 < curPathpointTotalNum)
		{
			if (hasAnimator)
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
		}
		else if (curDistance <= 10.1f && curPathpointNum + 1 >= curPathpointTotalNum)
		{
			isMoving = false;
			Destroy(gameObject);
			return;
		}
		
		if (nextPoint)
		{
			NPCTransform.forward = Vector3.Lerp(NPCTransform.forward, nextPoint.position - NPCTransform.position, Time.deltaTime * NPCRotateSpeed);
			rigidbody.velocity = Vector3.Normalize( NPCTransform.forward ) * speed;
			
			if(Physics.Raycast(specialPoint.position, -Vector3.up, out hit, 8.0f, mask.value) || Physics.Raycast(specialPoint.position, -Vector3.up, out hit, 8.0f, maskTerr.value))
			{
				NPCTransform.position = new Vector3(NPCTransform.position.x, hit.point.y + lidiDistance, NPCTransform.position.z);
			}
		}
	}
	
	public void initNPCInfor(Transform pathObj, float speedT, bool fanxiangT)
	{
		if (GetComponent<Animator>())
		{
			hasAnimator = true;
			animatorNPC = GetComponent<Animator>();
			
			animatorNPC.enabled = true;
		}
		
		curPathpointTotalNum = -1;
		curPathpointNum = 0;
		NPCTransform = transform;
		curPath = pathObj;

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
				
				if (hasAnimator)
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
		}
		else
		{
			isMoving = false;
			return;
		}
		
		nextPoint = pathpointArr[curPathpointNum];
		transform.LookAt (nextPoint.position);
		isMoving = true;
		
		if (hasAnimator)
		{
			doAnimationTri ("runTri");
		}
	}

	private int findIndex = 0;

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
		
		animatorNPC.SetTrigger(name);
		
		if (name.CompareTo("deadTri") == 0)
		{
			Invoke("hideAnimator", 3);
		}
		
		dongzuoName = name;
	}
	
	void hideAnimator()
	{
		animatorNPC.enabled = false;
	}
}

