using UnityEngine;
using System.Collections;

public class pointLinkScript : MonoBehaviour {
	public Transform pointObj = null;
	public bool isLookat = false;

	// Use this for initialization
	void Start () {
	}

	public string getPointName()
	{
		return pointObj.name;
	}

	public bool getIslookat()
	{
		return isLookat;
	}
}
