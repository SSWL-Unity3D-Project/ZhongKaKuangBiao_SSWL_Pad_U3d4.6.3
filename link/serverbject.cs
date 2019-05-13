using UnityEngine;
using System.Collections;

public class serverbject : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//transform.localScale = new Vector3 (int.Parse(Network.player + "") * 3, 1, 1);
		pcvr.selfServerObj = transform;
		pcvr.selfServerScrObj = GetComponent<serverbject>();
		DontDestroyOnLoad(gameObject);
	}

	[RPC]
	public void SetFirstPlayerRPC(int index)
	{
		pcvr.firstPlayerIndex = index;
	}
	
	[RPC]
	public void SetPlayerNumberRPC(int num, string passwordT)
	{
		pcvr.totalPlayerNum = num;

		if (!Network.isServer)
		Network.incomingPassword = passwordT;
	}

	//send message to server --- can start game
	public void startGame(int levelIndex)
	{
		if (Network.isClient && networkView)
		{
			networkView.RPC("ServerReceiveStartGameRPC", RPCMode.All, levelIndex);
		}
	}

	[RPC]
	public void ServerReceiveStartGameRPC(int levelIndex)
	{
		Network.incomingPassword = pcvr.passWordNew;

		if (Network.isServer)
		{
			int randN = Random.Range (0, 4);
			
			for(int i=0; i<4; i++)
			{
				pcvr.numArr[i] = (randN + i) % 4;
			}

			networkView.RPC("ClientReceiveStartGameRPC", RPCMode.All, Network.connections.Length, pcvr.netPlayerObjArr, pcvr.numArr, levelIndex);

			//the server begin game too
			//pcvr.donghuaScriObj.linkModeBegin(levelIndex);	//will after select truck

			if (pcvr.selfServerScrObj)
			{
				pcvr.selfServerScrObj.setServerTime(pcvr.gametimePCVR);
			}
		}
	}

	[RPC]
	public void ClientReceiveStartGameRPC(int totalNum, NetworkPlayer[] thePlayer, int[] prefabNumIndex, int levelIndex)
	{
		for (int i=0; i < totalNum; i++)
		{
			if (Network.isClient && thePlayer[i] == Network.player)
			{
				for(int a=0; a<4; a++)
				{
					pcvr.numArr[a] = prefabNumIndex[a];
				}

				pcvr.selfIndex = i;
				Debug.Log ("ClientReceiveStaratGameRPC   " + transform + " " + Network.isServer  + " "+ i);
				//startgame here
				//pcvr.linkguiTrans.gameObject.SetActive(false);
				//pcvr.donghuaScriObj.linkModeBegin(levelIndex);	//will after select truck
				pcvr.linkguiTrans.GetComponent<linkGui>().intoSelectTruckGui();
			}
		}
	}

	public void ServerReceiveSelectTruck(int levelIndex)
	{
		networkView.RPC("ServerReceiveSelectTruckRPC", RPCMode.All, levelIndex);
	}
	
	[RPC]
	public void ServerReceiveSelectTruckRPC(int levelIndex)
	{
		if (Network.isServer)
		{
			networkView.RPC("ServerReceiveSelectTruckRClientPC", RPCMode.All, Network.connections.Length, pcvr.netPlayerObjArr, levelIndex);
			pcvr.donghuaScriObj.linkModeBegin(levelIndex);
		}
	}
	
	[RPC]
	public void ServerReceiveSelectTruckRClientPC(int totalNum, NetworkPlayer[] thePlayer, int levelIndex)
	{Debug.Log ("ServerReceiveSelectTruckRClientPCServerReceiveSelectTruckRClientPCServerReceiveSelectTruckRClientPC   " + levelIndex);
		for (int i=0; i < totalNum; i++)
		{
			if (Network.isClient && thePlayer[i] == Network.player)
			{
				Debug.Log ("ServerReceiveSelectTruckRClientPC   " + transform + " " + Network.isServer  + " "+ i);
				//startgame here
				//pcvr.linkguiTrans.gameObject.SetActive(false);
				pcvr.donghuaScriObj.linkModeBegin(levelIndex);
			}
		}
	}
	
	[RPC]
	public void SendGameStateTime(int state, float time321, float gameTimeT, int[] indexOnlyT, int mingciFirstPlayerT)
	{//Debug.Log (transform + " SendGameStateTime  " + Network.isServer + " " + Network.isClient + " " + state + " " +pcvr.uiRunState + " " + time321 + " " + pcvr.total321Time + " "+ gameTimeT + " " + pcvr.totalTime);
		if (pcvr.uiRunState != 10)
		pcvr.uiRunState = state;

		if (state == 1 && Network.isClient)
		{
			pcvr.total321Time = time321;
		}
		else if (state == 2 && Network.isClient)
		{
			pcvr.totalTime = gameTimeT;
		}
		else if (state == 3)
		{
			if (Network.isServer)
			{
				pcvr.UITruckScrObj.LinkModeServerGameEnd();
				return;
			}
			else if (Network.isClient)
			{
				//show score information here
				pcvr.UITruckScrObj.hideClientDaojishi();
				pcvr.GetInstance().initEndGamele();
				/*if (pcvr.uiRunState < 10 && pcvr.finishedNumber > 0)
				{
					pcvr.selfServerScrObj.networkView.RPC("setUseTimeRPC", RPCMode.All, pcvr.selfIndex, 0);
				}*/

				Invoke("showScorew", 1.0f);
				return;
			}
		}

		if (Network.isClient)
		{
			pcvr.mingciFirstPlayer = mingciFirstPlayerT;

			for (int i=0; i<4; i++)
			{
				pcvr.indexOnly[i] = indexOnlyT[i];
			}
		}
	}
	
	void showScorew()
	{
		for (int i = 0; i < 4; i++)
		{Debug.Log("showScore1wServer  " + i + " "+ pcvr.useTimeSelf[i] + " "+ pcvr.finishedNumber);
			if (pcvr.useTimeSelf[i] < 0 && pcvr.finishedNumber > 0)
			{
				pcvr.useTimeSelf[i] = 0;
			}

			pcvr.useTimeSelfTemp[i] = pcvr.totalTimeServer;
		}
		
		pcvr.UITruckScrObj.showPlayerFinalScore(true);
	}

	//spawn npc
	public void spawnReceive(string name)
	{
		if (networkView)
		{
			networkView.RPC("spawnNPCServer", RPCMode.All, name);
		}
		else
		{
			Debug.Log("<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<FEtwe");
		}
	}
	
	[RPC]
	void spawnNPCServer(string name)
	{
		if (Network.isServer)
		{
			pcvr.aFirstScriObj.spawnNPCle(name);
		}
	}
	
	//delete npc
	public void deleteReceive(string name)
	{
		if (networkView)
		{
			networkView.RPC("deleteNPCServer", RPCMode.All, name);
		}
		else
		{
			Debug.Log("<<<<<<<<<<<<<<<<<<<<<<<s<<<<<<<<<<<<FEtwe");
		}
	}
	
	[RPC]
	void deleteNPCServer(string name)
	{
		if (Network.isServer)
		{
			pcvr.aFirstScriObj.delNPCle(name);
		}
	}

	//follow NPC
	public void FollowServerH(string name, int pointIndex, int playerIndex)
	{
		if (networkView)
		{
			networkView.RPC("FollowServerHRPC", RPCMode.All, name, pointIndex, playerIndex);
		}
		else
		{
			Debug.Log("<<<<<<<<<<<asf<<<<<<<<<<<<<<<<<<<<<<<<FEtwe");
		}
	}
	
	[RPC]
	void FollowServerHRPC(string name, int pointIndex, int playerIndex)
	{
		if (Network.isServer)
		{
			pcvr.serCameraSObj.setFollowPlayer(name, pointIndex, playerIndex);
		}
	}
	
	//follow NPC
	public void PointServerH(string TriName, string name, bool look, int playerIndex)
	{
		if (networkView)
		{
			networkView.RPC("PointServerHRPC", RPCMode.All, TriName, name, look, playerIndex);
		}
		else
		{
			Debug.Log("<<<<<<<<<<<asf<<<<<<<<<<<<<<<<<<<<<<<<FEtwe");
		}
	}
	
	[RPC]
	void PointServerHRPC(string TriName, string name, bool look, int playerIndex)
	{
		if (Network.isServer)
		{
			pcvr.serCameraSObj.setInPointObj (TriName, name, look, playerIndex, pcvr.aFirstScriObj.pointObjParent);
		}
	}
	
	[RPC]
	public void setUseTimeRPC(int playerIndex, float useTime)
	{
		pcvr.useTimeSelf[playerIndex] = useTime;
	}

	public void setServerTime(float gametimeT)
	{
		networkView.RPC("setServerTimeRPC", RPCMode.All, gametimeT);
	}
	
	[RPC]
	public void setServerTimeRPC(float gameTimeT)
	{
		if (Network.isClient)
		{
			if (pcvr.UITruckScrObj)
			{
				pcvr.UITruckScrObj.setGameTime(3, gameTimeT);
			}
		}
	}
}
