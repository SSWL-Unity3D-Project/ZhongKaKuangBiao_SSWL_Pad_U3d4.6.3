using UnityEngine;
using System;
using System.Collections;

public class ReadGameInfo : MonoBehaviour 
{
	static private ReadGameInfo Instance = null;
	private HandleJson handleJsonObj;
	public static string fileName = "KuaitingConfig.xml";
	public string m_pStarCoinNum = "";
	public string m_pGameMode = "";
	public string m_pGameNetwork = "";
	public string m_pGameVolum = "";
	public string m_pVolumNum = "";
	public string m_pGameTime = "";
	public string m_pInsertCoinNum = "";
	public string m_pGameRecord = "";
	public string m_pFangxiangpanDengji = "";
	public string m_pBikePowerMin = "";
	public string m_pBikePowerMax = "";
	public string m_pBikeShaCheMin = "";
	public string m_pBikeShaCheMax = "";
	public string m_pGameCHEN = "";
	public string m_pLinghuodu = "";

	public string m_pGameDateTime = "";
	public string[] m_pGameRecordArr;
	private static int totalLevelNumTest = 10;

	static public ReadGameInfo GetInstance()
	{
		if(Instance == null)
		{
			GameObject obj = new GameObject("_ReadGameInfo");
			DontDestroyOnLoad(obj);
			Instance = obj.AddComponent<ReadGameInfo>();
			Instance.InitGameInfo();
		}
		return Instance;
	}

	//this will be the first function about the gameset information reset or not
	void InitGameInfo()
	{
		m_pGameRecordArr = new string[totalLevelNumTest];

		for (int i=0; i<totalLevelNumTest; i++)
		{
			m_pGameRecordArr[i] = "";
		}

		handleJsonObj = HandleJson.GetInstance ();
		
		m_pStarCoinNum = handleJsonObj.ReadFromFileXml (fileName, "START_COIN");
		//m_pStarCoinNum = PlayerPrefs.GetString("START_COIN");
		if(m_pStarCoinNum == "" || m_pStarCoinNum == null)
		{
			m_pStarCoinNum = "1";
			WriteStarCoinNumSet("1");
		}

        m_pGameMode = "FREE"; //强制设置为免费模式.
        //m_pGameMode = handleJsonObj.ReadFromFileXml (fileName, "GAME_MODE");
		//m_pGameMode = PlayerPrefs.GetString("GAME_MODE");
		//if(m_pGameMode == "" || m_pGameMode == null)
		//{
		//	m_pGameMode = "oper";
		//	WriteGameStarMode("oper");
		//}
		
		m_pGameNetwork = handleJsonObj.ReadFromFileXml (fileName, "GAME_network");
		//m_pGameNetwork = PlayerPrefs.GetString("GAME_network");
		if(m_pGameNetwork == "" || m_pGameNetwork == null)
		{
			m_pGameNetwork = "on";
			WriteGameNetwork("on");
		}
		
		m_pGameVolum = handleJsonObj.ReadFromFileXml (fileName, "GAME_volum");
		//m_pGameVolum = PlayerPrefs.GetString("GAME_volum");
		if(m_pGameVolum == "" || m_pGameVolum == null)
		{Debug.Log("rrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrssssssss");
			m_pGameVolum = "on";
			WriteGameVolum("on");
		}
		
		m_pVolumNum = handleJsonObj.ReadFromFileXml (fileName, "Volum_num");
		//m_pVolumNum = PlayerPrefs.GetString("Volum_num");
		if(m_pVolumNum == "" || m_pVolumNum == null)
		{Debug.Log("rrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrr");
			m_pVolumNum = "7";
			WriteVolumNumSet("7");
		}
		m_pGameTime = handleJsonObj.ReadFromFileXml (fileName, "START_TIME");
		//m_pGameTime = PlayerPrefs.GetString("START_TIME");
		if(m_pGameTime == "" || m_pGameTime == null)
		{
			m_pGameTime = "180";
			WriteGameTimeSet("180");
		}

		m_pInsertCoinNum = "0";
		
		m_pGameRecord = handleJsonObj.ReadFromFileXml (fileName, "GAME_RECORD");
		//m_pGameRecord = PlayerPrefs.GetString("GAME_RECORD");
		if(m_pGameRecord == "" || m_pGameRecord == null)
		{
			m_pGameRecord = "0";
			WriteGameRecord("0");
		}
		
		m_pFangxiangpanDengji = handleJsonObj.ReadFromFileXml (fileName, "FangxiangpanDengji");
		//m_pFangxiangpanDengji = PlayerPrefs.GetString("FangxiangpanDengji");
		if(m_pFangxiangpanDengji == "" || m_pFangxiangpanDengji == null)
		{
			m_pFangxiangpanDengji = "0";
			WriteFangxiangpanDengji("0");
		}
		
		m_pBikePowerMin = handleJsonObj.ReadFromFileXml (fileName, "mBikePowerMin");
		//m_pBikePowerMin = PlayerPrefs.GetString("mBikePowerMin");
		if(m_pBikePowerMin == "" || m_pBikePowerMin == null)
		{
			m_pBikePowerMin = "0";
			WriteBikePowerMin("0");
		}
		
		m_pBikePowerMax = handleJsonObj.ReadFromFileXml (fileName, "mBikePowerMax");
		//m_pBikePowerMax = PlayerPrefs.GetString("mBikePowerMax");
		if(m_pBikePowerMax == "" || m_pBikePowerMax == null)
		{
			m_pBikePowerMax = "0";
			WriteBikePowerMax("0");
		}
		
		m_pBikeShaCheMin = handleJsonObj.ReadFromFileXml (fileName, "mBikeShaCheMin");
		//m_pBikeShaCheMin = PlayerPrefs.GetString("mBikeShaCheMin");
		if(m_pBikeShaCheMin == "" || m_pBikeShaCheMin == null)
		{
			m_pBikeShaCheMin = "0";
			WriteBikeShaCheMin("0");
		}
		
		m_pBikeShaCheMax = handleJsonObj.ReadFromFileXml (fileName, "mBikeShaCheMax");
		//m_pBikeShaCheMax = PlayerPrefs.GetString("mBikeShaCheMax");
		if(m_pBikeShaCheMax == "" || m_pBikeShaCheMax == null)
		{
			m_pBikeShaCheMax = "0";
			WriteBikeShaCheMax("0");
		}
		
		m_pGameCHEN = handleJsonObj.ReadFromFileXml (fileName, "GAME_CHEN");Debug.Log ("m_pGameCHENm_pGameCHEN "+m_pGameCHEN);
		//m_pGameCHEN = PlayerPrefs.GetString("GAME_CHEN");
		if(m_pGameCHEN == "" || m_pGameCHEN == null)
		{
			m_pGameCHEN = "1";
			WriteGameCHEN("1");
		}

		m_pLinghuodu = handleJsonObj.ReadFromFileXml (fileName, "Game_Linghuodu");
		//m_pLinghuodu = PlayerPrefs.GetString("Game_Linghuodu");
		if(m_pLinghuodu == "" || m_pLinghuodu == null)
		{
			m_pLinghuodu = "6";
			WriteLinghuodu("6");
		}

		readGameDateTime ();
	}

	public void FactoryReset()
	{
		WriteStarCoinNumSet("1");
		WriteGameStarMode("oper");
		WriteGameNetwork("on");
		WriteGameVolum ("on");
		WriteVolumNumSet("7");
		WriteGameTimeSet("120");
		//WriteInsertCoinNum("0");
		pcvr.coinCurNumPCVR = 0;
		pcvr.gametimePCVR = 300;
		WriteGameRecord("0");
		WriteGameCHEN ("1");
		WriteLinghuodu ("6");
	}
	public void ReadStarCoinNumSet()
	{
		if(m_pStarCoinNum == "" || m_pStarCoinNum == null)
		{
			pcvr.startCoinNumPCVR = 0;
		}
		else
		{
			pcvr.startCoinNumPCVR = Convert.ToInt32(m_pStarCoinNum);
		}
	}
	public string ReadGameStarMode()
	{//number - flag
		if(m_pGameMode == "oper" || m_pGameMode == "" || m_pGameMode == null)
		{
			pcvr.gameModePCVR = 1;
		}
		else
		{
			pcvr.gameModePCVR = 0;
			pcvr.startCoinNumPCVR = 0;
		}

		return m_pGameMode;//fffffffffffffffffffffff
	}
	public void ReadGameNetwork()
	{//number - flag
		if(m_pGameNetwork == "on" || m_pGameNetwork == "" || m_pGameNetwork == null)
		{
			pcvr.networkPCVR = 1;
		}
		else
		{
			pcvr.networkPCVR = 0;
		}
	}
	public string ReadGameVolumNum()
	{
		return m_pVolumNum;
	}
	public string ReadGameVolum()
	{
		return m_pGameVolum;
	}
	public void ReadGameTimeSet()
    {
        pcvr.gametimePCVR = 300;
        //if (m_pGameTime == "" || m_pGameTime == null)
		//{
		//	pcvr.gametimePCVR = 180;
		//}
		//else
		//{
		//	pcvr.gametimePCVR = Convert.ToInt32(m_pGameTime);
		//}
	}
	public void ReadInsertCoinNum()
	{
		if(m_pInsertCoinNum == "" || m_pInsertCoinNum == null)
		{
			pcvr.coinCurNumPCVR = 0;
		}
		else
		{
			pcvr.coinCurNumPCVR = Convert.ToInt32(m_pInsertCoinNum);
		}
	}
	public void ReadGameRecord()
	{
		if(m_pGameRecord == "" || m_pGameRecord == null)
		{
			pcvr.recordPCVR = 0;
		}
		else
		{
			pcvr.recordPCVR = Convert.ToInt32(m_pGameRecord);
		}
	}

	public void ReadFangxiangpanDengji()
	{
		if(m_pFangxiangpanDengji == "" || m_pFangxiangpanDengji == null)
		{
			pcvr.zhendongDengjiFXP = 0;
		}
		else
		{
			pcvr.zhendongDengjiFXP = Convert.ToInt32(m_pFangxiangpanDengji);
		}
	}
	
	public void ReadBikePowerMin()
	{
		if(m_pBikePowerMin == "" || m_pBikePowerMin == null)
		{
			pcvr.mBikePowerMin = 0;
		}
		else
		{
			pcvr.mBikePowerMin = (uint)Convert.ToInt32(m_pBikePowerMin);
		}
	}
	
	public void ReadBikePowerMax()
	{
		if(m_pBikePowerMax == "" || m_pBikePowerMax == null)
		{
			pcvr.mBikePowerMax = 0;
		}
		else
		{
			pcvr.mBikePowerMax = (uint)Convert.ToInt32(m_pBikePowerMax);
		}
	}
	
	public void ReadBikeShaCheMin()
	{
		if(m_pBikeShaCheMin == "" || m_pBikeShaCheMin == null)
		{
			pcvr.mBikeShaCheMin = 0;
		}
		else
		{
			pcvr.mBikeShaCheMin = (uint)Convert.ToInt32(m_pBikeShaCheMin);
		}
	}
	
	public void ReadBikeShaCheMax()
	{
		if(m_pBikeShaCheMax == "" || m_pBikeShaCheMax == null)
		{
			pcvr.mBikeShaCheMax = 0;
		}
		else
		{
			pcvr.mBikeShaCheMax = (uint)Convert.ToInt32(m_pBikeShaCheMax);
		}
	}

	public void ReadGameCHEN()
	{//number - flag
		if(m_pGameCHEN == "1" || m_pGameCHEN == "" || m_pGameCHEN == null)
		{
			pcvr.languagePCVR = 1;
		}
		else
		{
			pcvr.languagePCVR = 0;
		}
	}
	public void ReadGameLinghuodu()
	{
		if(m_pLinghuodu == "" || m_pLinghuodu == null)
		{
			pcvr.linghuoduPCVR = 6;
		}
		else
		{
			pcvr.linghuoduPCVR = Convert.ToInt32(m_pLinghuodu);
		}
	}

	public void WriteStarCoinNumSet(string value)
	{
		handleJsonObj.WriteToFileXml(fileName,"START_COIN",value);
		//PlayerPrefs.SetString("START_COIN", value);
		m_pStarCoinNum = value;
	}
	public void WriteGameStarMode(string value)
	{
		//handleJsonObj.WriteToFileXml(fileName,"GAME_MODE",value);
		//PlayerPrefs.SetString("GAME_MODE", value);
		//m_pGameMode = value;

		/*if (value.CompareTo("FREE") == 0)
		{
			PlayerPrefs.SetString("START_COIN", "0");
			m_pStarCoinNum = "0";
		}*/
	}
	public void WriteGameNetwork(string value)
	{
		handleJsonObj.WriteToFileXml(fileName,"GAME_network",value);
		//PlayerPrefs.SetString("GAME_network", value);
		m_pGameNetwork = value;
	}
	public void WriteGameVolum(string value)
	{
		handleJsonObj.WriteToFileXml(fileName,"GAME_volum",value);
		//PlayerPrefs.SetString("GAME_volum", value);
		m_pGameVolum = value;
	}
	public void WriteVolumNumSet(string value)
	{
		handleJsonObj.WriteToFileXml(fileName,"Volum_num",value);
		//PlayerPrefs.SetString("Volum_num", value);
		m_pVolumNum = value;
	}
	public void WriteGameTimeSet(string value)
	{
		handleJsonObj.WriteToFileXml(fileName,"START_TIME",value);
		//PlayerPrefs.SetString("START_TIME", value);
		m_pGameTime = value;
	}
	/*public void WriteInsertCoinNum(string value)
	{
		m_pInsertCoinNum = value;
		if(m_pInsertCoinNum == "" || m_pInsertCoinNum == null)
		{
			pcvr.coinCurNumPCVR = 0;
		}
		else
		{
			pcvr.coinCurNumPCVR = Convert.ToInt32(m_pInsertCoinNum);
		}
	}*/
	public void WriteGameRecord(string value)
	{
		handleJsonObj.WriteToFileXml(fileName,"GAME_RECORD",value);
		//PlayerPrefs.SetString("GAME_RECORD", value);
		m_pGameRecord = value;
	}
	public void WriteLinghuodu(string value)
	{
		handleJsonObj.WriteToFileXml(fileName,"Game_Linghuodu",value);
		//PlayerPrefs.SetString("Game_Linghuodu", value);
		m_pLinghuodu = value;
	}

	void readGameDateTime()
	{
		m_pGameDateTime = handleJsonObj.ReadFromFileXml (fileName, "GAME_DATETIME");
		//m_pGameDateTime = PlayerPrefs.GetString("GAME_DATETIME");
		
		if (m_pGameDateTime.CompareTo ("") == 0 || m_pGameDateTime.CompareTo (DateTime.Now.ToShortDateString ().ToString ()) != 0) {
			//reset the record information
			
			for (int i=0; i<totalLevelNumTest; i++)
			{
				m_pGameRecordArr[i] = "";
				WriteGameRecordAllLevel(i, "");
			}

			WriteGameDateTime(DateTime.Now.ToShortDateString ().ToString ());
		}

		//get the record inforamtion
		for (int i=0; i<totalLevelNumTest; i++)
		{
			string name = "Level" + i.ToString();
			m_pGameRecordArr[i] = handleJsonObj.ReadFromFileXml (fileName, name);
			//m_pGameRecordArr[i] = PlayerPrefs.GetString(name);
		}
	}

	void WriteGameDateTime(string value)
	{
		handleJsonObj.WriteToFileXml(fileName,"GAME_DATETIME",value);
		//PlayerPrefs.SetString("GAME_DATETIME", value);
	}

	public void WriteGameRecordAllLevel(int levelIndex, string value)
	{
		string levelName = "Level" + levelIndex.ToString();

		m_pGameRecordArr[levelIndex] = value;

		handleJsonObj.WriteToFileXml(fileName,levelName,value);
		//PlayerPrefs.SetString(levelName, value);
	}

	public double getRecordTime(int levelIndex)
	{
		if (m_pGameRecordArr [levelIndex].CompareTo("") == 0)
		{
			return 0;
		}

		float totalTimeT = 0.0f;

		if (float.TryParse (m_pGameRecordArr [levelIndex], out totalTimeT))
		{
			return Convert.ToDouble (m_pGameRecordArr [levelIndex]);
		}
		else
		{
			return 0;
		}
	}

	
	//20170808
	public void WriteFangxiangpanDengji(string value)
	{
		handleJsonObj.WriteToFileXml(fileName,"FangxiangpanDengji",value);
		//PlayerPrefs.SetString("FangxiangpanDengji", value);
		m_pFangxiangpanDengji = value;
	}

	public void WriteBikePowerMin(string value)
	{
		handleJsonObj.WriteToFileXml(fileName,"mBikePowerMin",value);
		//PlayerPrefs.SetString("mBikePowerMin", value);
		m_pBikePowerMin = value;
	}

	public void WriteBikePowerMax(string value)
	{
		handleJsonObj.WriteToFileXml(fileName,"mBikePowerMax",value);
		//PlayerPrefs.SetString("mBikePowerMax", value);
		m_pBikePowerMax = value;
	}

	public void WriteBikeShaCheMin(string value)
	{
		handleJsonObj.WriteToFileXml(fileName,"mBikeShaCheMin",value);
		//PlayerPrefs.SetString("mBikeShaCheMin", value);
		m_pBikeShaCheMin = value;
	}

	public void WriteBikeShaCheMax(string value)
	{
		handleJsonObj.WriteToFileXml(fileName,"mBikeShaCheMax",value);
		//PlayerPrefs.SetString("mBikeShaCheMax", value);
		m_pBikeShaCheMax = value;
	}

	public void WriteGameCHEN(string value)
	{
		handleJsonObj.WriteToFileXml(fileName,"GAME_CHEN",value);
		//PlayerPrefs.SetString("GAME_CHEN", value);
		m_pGameCHEN = value;
	}
}
