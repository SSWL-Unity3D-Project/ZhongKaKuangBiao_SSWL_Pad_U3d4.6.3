using UnityEngine;
using System.Collections;

public class truckFlag : MonoBehaviour {
	public bool isPlayer = false;
	public Material[] biaoJiMatPlayer;
	public Material[] biaoJiMatAI;

	private float distance = 0.0f;
	public float distanceMin = 60.0f;
	public float distanceMax = 300.0f;
	public float scaleMaxBeishu = 2.0f;
	private float scaleMinX = 1.0f;
	private float scaleMinZ = 1.0f;
	private float scaleBeishu = 1.0f;

	private int selfIndex = -1;

	// Use this for initialization
	void Start () {
		//single mode
		if (!Network.isServer && !Network.isClient)
		{
			if (isPlayer)
			{//player
				gameObject.SetActive(false);
			}
			else
			{//AI
				selfIndex = transform.root.GetComponent<truck>().getAIIndex();

				changeMaterialAI(selfIndex);
			}
		}

		scaleMinX = transform.localScale.x;
		scaleMinZ = transform.localScale.z;
	}

	void StartLink () {
		Debug.Log (transform.root.gameObject + " StartLink   " + isPlayer + " "+ pcvr.selfIndex + " "+ selfIndex);
		if (Network.isClient)
		{
			if (isPlayer)
			{
				changeMaterialPlayer(selfIndex);

				if (pcvr.selfIndex == selfIndex)
				{
					gameObject.SetActive(false);
				}
			}
			else
			{
				changeMaterialAI(selfIndex);
			}
		}
		else if (Network.isServer)
		{
			//show all the biaoji
			if (isPlayer)
			{
				changeMaterialPlayer(selfIndex);
			}
			else
			{
				changeMaterialAI(selfIndex);
			}

		}
	}

	public void setSelfIndex(int index)
	{
		selfIndex = index;

		StartLink ();
	}

	void changeMaterialPlayer(int index)
	{
		if (index < 0)
		{
			return;
		}
		
		if (transform.GetChild(0) && biaoJiMatPlayer.Length > 3 && biaoJiMatPlayer[index])
		{
			transform.GetChild(0).GetComponent<MeshRenderer>().material = biaoJiMatPlayer[index];
		}
	}

	void changeMaterialAI(int index)
	{
		if (index < 0)
		{
			return;
		}

		if (transform.GetChild(0) && biaoJiMatAI.Length > 3 && biaoJiMatAI[3 - index])
		{
			transform.GetChild(0).GetComponent<MeshRenderer>().material = biaoJiMatAI[3 - index];
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (pcvr.XKCarCameraTranform && selfIndex >= 0)
		{
			transform.LookAt (pcvr.XKCarCameraTranform);
			distance = Vector3.Distance(transform.position, pcvr.XKCarCameraTranform.position);

			if (distance <= distanceMin)
			{
				scaleBeishu = 1.0f;
			}
			else if (distance >= distanceMax)
			{
				scaleBeishu = scaleMaxBeishu;
			}
			else
			{
				scaleBeishu = 1 + ((distance - distanceMin) / (distanceMax - distanceMin)) * (scaleMaxBeishu- 1.0f);
			}

			transform.localScale = new Vector3(scaleMinX * scaleBeishu, scaleMinX * scaleBeishu, scaleMinZ);
		}
	}
}
