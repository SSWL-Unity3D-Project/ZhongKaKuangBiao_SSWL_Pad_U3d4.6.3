using UnityEngine;
using System.Collections;

public class XKCarWheelCtrl : MonoBehaviour
{
	public Transform[] RearWheel;
	Transform[] RearWheelReal;
	void Start()
	{
		RearWheelReal = new Transform[4];
		for (int i = 0;  i < 4; i++) {
			RearWheelReal[i] = RearWheel[i].GetChild(0);
		}
	}

	// Update is called once per frame
	void Update()
	{
		Vector3 wheelLPos = RearWheel[0].localPosition;
		wheelLPos.x = RearWheel[2].localPosition.x;
		wheelLPos.z = RearWheel[2].localPosition.z;
		RearWheel[2].localPosition = Vector3.Lerp(RearWheel[2].localPosition, wheelLPos, Time.deltaTime * 10f);

		wheelLPos = RearWheel[1].localPosition;
		wheelLPos.x = RearWheel[3].localPosition.x;
		wheelLPos.z = RearWheel[3].localPosition.z;
		RearWheel[3].localPosition = Vector3.Lerp(RearWheel[3].localPosition, wheelLPos, Time.deltaTime * 10f);

		RearWheelReal[2].localEulerAngles = RearWheelReal[0].localEulerAngles;
		RearWheelReal[3].localEulerAngles = RearWheelReal[1].localEulerAngles;
	}
}