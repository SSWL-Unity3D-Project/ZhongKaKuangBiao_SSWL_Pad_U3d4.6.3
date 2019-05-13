using UnityEngine;
using System.Collections;

public class deleteTriggerScript : MonoBehaviour {

	public GameObject[] deleteSpawnPointArr;
	private spawnNpcScript[] deleteSpawnNPCSobj;
	private int deleteNum = 0;
	private int deleteTimes = 0;

	// Use this for initialization
	void Start () {

		deleteNum = 0;
		deleteTimes = 0;
		
		for (int i=0; i<deleteSpawnPointArr.Length; i++)
		{
			if (deleteSpawnPointArr[i])
			{
				deleteNum ++;
			}
		}

		if (deleteNum > 0)
		{
			deleteSpawnNPCSobj = new spawnNpcScript[deleteNum];
			
			for (int i=0; i < deleteNum; i++)
			{
				if (deleteSpawnPointArr[i])
				{
					deleteSpawnNPCSobj[i] = deleteSpawnPointArr[i].GetComponent<spawnNpcScript>();
				}
			}
		}
	}

	public bool BeginDelete()
	{
		if (Network.isServer)
		{
			deleteTimes ++;
			//Debug.Log("deleteTimes    " + deleteTimes + " " + pcvr.totalPlayerNum);
			if (deleteTimes < pcvr.totalPlayerNum)
			{
				return false;
			}
		}

		for (int i=0; i < deleteNum; i++)
		{
			deleteSpawnNPCSobj[i].deleteAAA();
		}

		return true;
	}
}
