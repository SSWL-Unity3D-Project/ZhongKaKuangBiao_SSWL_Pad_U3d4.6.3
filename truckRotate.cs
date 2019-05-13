using UnityEngine;
using System.Collections;

public class truckRotate : MonoBehaviour {
	public float rotateSpeed = 70.0f;
	public float moveSpeed = 30;
	public Transform targetObj = null;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update1 () {
		transform.Rotate(0, rotateSpeed * Time.deltaTime, 0, Space.Self);

		if (targetObj)
		transform.position = Vector3.MoveTowards (transform.position, targetObj.position, moveSpeed * Time.deltaTime);
	}

	//first set the position
	public void setPositionFrom(Transform nowTranPos)
	{
		transform.position = nowTranPos.position;
	}

	//then can move to the targetPosition
	public void setTargetObj(Transform targetT)
	{
		targetObj = targetT;
	}
	
	//first set the position
	public void setPositionNow(Transform nowTranPos)
	{
		transform.position = nowTranPos.position;
	}


	public void resetPosition()
	{
		transform.position = Vector3.zero;
	}
}
