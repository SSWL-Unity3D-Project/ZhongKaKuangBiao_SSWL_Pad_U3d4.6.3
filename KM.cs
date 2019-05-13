using UnityEngine;
using System.Collections;

public class KM : MonoBehaviour {

	public UISprite numSprite1 = null;
	public UISprite numSprite2 = null;
	public UISprite numSprite3 = null;
	public UISprite numSprite4 = null;

	private int number1 = 0;
	private int number2 = 0;
	private int number3 = 0;
	private int number4 = 0;

	public static float truckDistanceKM = 0;	// truck speed

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		truckDistanceKM = truckDistanceKM / 1000.0f;

		if (truckDistanceKM <= 0)
		{
			truckDistanceKM = 0;
		}
		else if (truckDistanceKM >= 1000)
		{
			truckDistanceKM = truckDistanceKM - Mathf.FloorToInt((truckDistanceKM / 1000.0f)) * 1000;
		}
		number1 = Mathf.FloorToInt(truckDistanceKM / 100);
		number2 = Mathf.FloorToInt(truckDistanceKM / 10) % 10;
		number3 = Mathf.FloorToInt(truckDistanceKM) % 10;
		number4 = Mathf.FloorToInt(truckDistanceKM * 10.0f) % 10;

		numSprite1.spriteName = number1.ToString ();
		numSprite2.spriteName = number2.ToString ();
		numSprite3.spriteName = number3.ToString ();
		numSprite4.spriteName = number4.ToString ();
	}
}
