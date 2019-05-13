using UnityEngine;
using System.Collections;

public class spawnNpcScript : MonoBehaviour {

	public bool fanxiang = false;
	public GameObject NPCPrefab = null;
	public Transform pathNodeObj = null;
	public float speed = 0.0f;
	private GameObject NPCObj = null;
	private float delayTime = 0.0f;
	private NPCControl scriptObj = null;
	private NPCControlSimple scriptSimpleObj = null;
	//private GameObject myCamera = null;

	public void readyToSspawn(float timeT)
	{
		if (!NPCPrefab)
		{
			return;
		}

		/*if (Network.isServer)
		{
			myCamera = GameObject.Find ("serverMainCamera");
		}
		else
		{
			myCamera = GameObject.Find ("_GameCamera");
		}*/

		delayTime = timeT;
		CancelInvoke ("spawnDetect");
		InvokeRepeating ("spawnDetect", 0.018f, 0.06f);
	}

	void spawnDetect()
	{
		if (delayTime <= 0.0f)
		{
			CancelInvoke ("spawnDetect");
			spawnOneHere();
			return;
		}
		
		delayTime -= 0.06f;
	}
	
	void NPCDead(GameObject deleteObj)
	{
		if (scriptObj)
		{
			scriptObj.NPCOver ();
		}

		if (Network.isServer && deleteObj && gameObject.activeSelf)
		{
			Network.Destroy(deleteObj);
		}
		else if (deleteObj)
		{
			DestroyObject(deleteObj);
		}

		NPCObj = null;
	}

	void spawnOneHere()
	{
		if (Network.isServer)//the first one here
		{//link mode - server
			NPCObj = Network.Instantiate(NPCPrefab, transform.position, transform.rotation, 0) as GameObject;

			pcvr.chanshengdeObjNet[pcvr.chanshengdeObjNetNum] = NPCObj;
			pcvr.chanshengdeObjNetNum ++;
		}
		else if (!Network.isClient)
		{//single mode
			NPCObj = Instantiate(NPCPrefab, transform.position, transform.rotation) as GameObject;

			if (NPCObj && NPCObj.networkView)
			{
				Destroy (NPCObj.GetComponent<NetworkView>());
			}

			pcvr.chanshengdeObj[pcvr.chanshengdeObjNum] = NPCObj;
			pcvr.chanshengdeObjNum ++;
		}

		if (!NPCObj)
		{
			return;
		}
		
		//find the NPC scritp and init the information of the NPC
		scriptObj = NPCObj.GetComponent<NPCControl>();

		if (scriptObj)
		{
			//Debug.Log(NPCObj.name + " has no NPCControl");
			scriptObj.initNPCInfor (pathNodeObj, speed, fanxiang);
			return;
		}

		scriptSimpleObj = NPCObj.GetComponent<NPCControlSimple>();
		
		if (scriptSimpleObj)
		{
			//Debug.Log(NPCObj.name + " has no NPCControlSimple");
			scriptSimpleObj.initNPCInfor (pathNodeObj, speed, fanxiang);
			return;
		}
	}

	public void deleteAAA()
	{
		if (NPCObj)
		{
			if (Network.isServer)
			{
				Invoke ("deleteBBB", 1.5f);
			}
			else
			{
				NPCDead(NPCObj);
			}
		}
	}

	void deleteBBB()
	{
		NPCDead(NPCObj);
	}
}
