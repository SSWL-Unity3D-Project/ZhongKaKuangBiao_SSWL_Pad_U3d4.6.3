using UnityEngine;
using System.Collections;

public class XKCarMoveCtrlU3d : MonoBehaviour
{
	Rigidbody CarRig;
	float CarSpeedCur;
	// Use this for initialization
	void Start()
	{
		CarRig = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update()
	{
		CarSpeedCur = CarRig.velocity.magnitude;
	}

	void OnGUI1()
	{
		if (Network.isServer || Network.isClient)
		{
			return;
		}

		float carSpeed = CarSpeedCur * 3.6f;
		string strA = "speed "+carSpeed.ToString("f2")+" km/h";
		GUI.Label(new Rect(10f, 155f, 500f, 25f), strA);
	}
}