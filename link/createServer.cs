using UnityEngine;
using System.Collections;

public class createServer : MonoBehaviour {

	//when run this script
	public static createServer _instance;

	void Awake()
	{
		_instance = this;
	}

	void Start ()
	{

	}

	/*public Camera DHCameraObj = null;
	public static createServer _instance;
	public static bool isServer = false;
	public static int playerNum = 0;
	public static int playerFinishNum = -1;
	public static int playerOnlyNum = -1;		//only into game conrol
	public static int playerIndexFinal = -1;
	public static bool duankaile = false;

	public static bool serverIntoscence = false;

	public GameObject toubiObj = null;
	public GameObject ServerCtrlObjPrefab = null;

	public static string fileName = "IP.xml";
	//private HandleJson handleJsonObj;

	public static GameObject serverCtrO = null;
	public static GameObject serverCtrOC = null;

	public static NetworkPlayer[] linkArr;
	public static GameObject[] linkObjArr;	//the player on the server
	public static NpcController[] NPCObjArr;

	public static int[] numCreateArr;
	public static float firstPlayerDistance = 0.0f;
	string GameNetwork = "";

	// Use this for initialization
	void Start ()
	{

		toubiObj.SetActive (true);
		duankaile = false;
		DHCameraObj.enabled = true;

		initCreate (1);
	}

	void initCreate(int index)
	{
		if (serverCtrO)
		{
			Network.Destroy(serverCtrO);
		}

		if (index == 1)
		{
			Debug.Log("disssahere  1");
			Network.Disconnect ();				//after the game end, should disconnect the network
		}

		isServer = false;
		playerNum = 0;
		playerFinishNum = -1;
		playerOnlyNum = -1;
		playerIndexFinal = -1;
		serverIntoscence = false;
		firstPlayerDistance = 0;

		//handleJsonObj = HandleJson.GetInstance ();
		//ipString = handleJsonObj.ReadFromFileXml (fileName, "IP");

		linkArr = new NetworkPlayer[connections];
		linkObjArr = new GameObject[connections];
		NPCObjArr = new NpcController[4];
		
		numCreateArr = new int[4];
		int randN = Random.Range (0, 4);

		for(int i=0; i<4; i++)
		{
			numCreateArr[i] = (randN + i) % 4;
		}
	}

	void OnDisconnectedFromServer(NetworkDisconnection info)
	{
		if (Network.isServer)
		{
			Debug.Log("Local server connection disconnected " + Application.loadedLevel);

			if (Application.loadedLevel == 0)
			{
				//the cartoon level, will only create again
				createServerAgain();
			}
			else
			{
				disconnectbaS(2);
			}
		}
	}
	
	void disconnectbaS(int index)
	{
		inControl.intoScence = false;
		linkGui.serverCtrObjectLink = null;
		Network.Disconnect ();
		Debug.Log("disssahere  2 " + index);
		Destroy (createServer.serverCtrOC);
		XkGameCtrl.IsLoadingLevel = true;
		Application.LoadLevel(0);
	}*/
}
