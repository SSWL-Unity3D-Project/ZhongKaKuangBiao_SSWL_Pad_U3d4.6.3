using UnityEngine;
using System.Collections;

public class OpenLight : MonoBehaviour {
	public Light LightD;
	public float LightCloseTime = 0;
	public GameObject LightPar;
	public float LightParCloseTime = 0;

	// Use this for initialization
	void Start () {
		if (LightD)
		{
			LightD.enabled = false;
		}

		if (LightPar)
		{
			LightPar.SetActive (false);
		}
	}

	public void OpenLightAndPar()
	{
		if (LightD)
		{
			LightD.enabled = true;
			Invoke ("CloseLight", LightCloseTime);
		}

		if (LightPar)
		{
			LightPar.SetActive (true);
			Invoke ("CloseLightPar", LightParCloseTime);
		}
	}

	void CloseLight()
	{
		LightD.enabled = false;
		LightD.gameObject.SetActive(false);
	}

	void CloseLightPar()
	{
		LightPar.SetActive (false);
	}
}
