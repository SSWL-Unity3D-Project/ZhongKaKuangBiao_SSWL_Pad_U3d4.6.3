using UnityEngine;
using System.Collections;
using System;

public class SetPanel : MonoBehaviour
{
	//zhujiemian
	public GameObject m_ZhujiemianObject;
	public Transform m_ZhujiemianXingXing;
	private int m_IndexZhujiemian = 0;
	public UILabel m_CoinForStar;
	public UILabel m_Lingmindu;
	public UILabel m_GameTimeLable;
	public UITexture m_GameModeDuigou1;
	public UITexture m_GameModeDuigou2;
	public UITexture m_NetworkDuigou1;
	public UITexture m_NetworkDuigou2;
	public UITexture m_VolumeDuigou1;
	public UITexture m_VolumeDuigou2;
	public UILabel m_Volume;
	public UILabel InsertCoinNumLabel;
	public UILabel BtInfoLabel;
	public UILabel YouMenInfoLabel;
	public UILabel FangXiangInfoLabel;
	public UITexture JiaoZhunTexture;
	public Texture[] JiaoZhunTextureArray;
	bool IsInitJiaoZhunPcvr;
	int JiaoZhunCount;
	GameObject JiaoZhunObj;
	public static bool IsOpenSetPanel = false;
	public UITexture m_CHENDuigou1;
	public UITexture m_CHENDuigou2;
	
	public UILabel m_FangxiangpanLabel;

	void Start () 
	{
		pcvr.UIState = 4;
		IsOpenSetPanel = true;
		pcvr.IsCloseQiNang = false;
		pcvr.GetInstance().getGamesetInfor();
		CloseAllQiNang();
		pcvr.GetInstance().CloseFangXiangPanPower();
		//pcvr.IsSlowLoopCom = false;
		UpdateInsertCoin();

		BtInfoLabel.text = "";
		m_ZhujiemianXingXing.localPosition = new Vector3(-510.0f,212.0f,0.0f);
		string GameVolum = ReadGameInfo.GetInstance ().ReadGameVolum ();

		m_CoinForStar.text = pcvr.startCoinNumPCVR.ToString();
		m_GameTimeLable.text = pcvr.gametimePCVR.ToString ();
		m_FangxiangpanLabel.text = pcvr.zhendongDengjiFXP.ToString ();
		m_Volume.text = ReadGameInfo.GetInstance().ReadGameVolumNum ();
		m_Lingmindu.text = pcvr.linghuoduPCVR.ToString ();

		if(pcvr.gameModePCVR == 1)
		{
			m_GameModeDuigou1.enabled = true;
			m_GameModeDuigou2.enabled = false;
		}
		else
		{
			m_GameModeDuigou1.enabled = false;
			m_GameModeDuigou2.enabled = true;
		}

		if(pcvr.networkPCVR == 1)
		{
			m_NetworkDuigou1.enabled = true;
			m_NetworkDuigou2.enabled = false;
		}
		else
		{
			m_NetworkDuigou1.enabled = false;
			m_NetworkDuigou2.enabled = true;
		}

		if(GameVolum == "on" || GameVolum == "" || GameVolum == null)
		{
			m_VolumeDuigou1.enabled = false;
			m_VolumeDuigou2.enabled = true;
		}
		else
		{
			m_VolumeDuigou1.enabled = true;
			m_VolumeDuigou2.enabled = false;
		}

		if (JiaoZhunTexture != null) {
			JiaoZhunObj = JiaoZhunTexture.gameObject;
			JiaoZhunObj.SetActive(false);
		}
		
		if(pcvr.languagePCVR == 1)
		{
			m_CHENDuigou1.enabled = true;
			m_CHENDuigou2.enabled = false;
		}
		else
		{
			m_CHENDuigou1.enabled = false;
			m_CHENDuigou2.enabled = true;
		}
		
		InputEventCtrl.GetInstance().ClickSetEnterBtEvent += ClickSetEnterBtEvent;
		InputEventCtrl.GetInstance().ClickSetMoveBtEvent += ClickSetMoveBtEvent;
		InputEventCtrl.GetInstance().ClickStartBtOneEvent += ClickStartBtOneEvent;
		InputEventCtrl.GetInstance().ClickShaCheBtEvent += ClickShaCheBtEvent;
		InputEventCtrl.GetInstance().ClickLaBaBtEvent += ClickLaBaBtEvent;
		InputEventCtrl.GetInstance().ClickCloseDongGanBtEvent += ClickCloseDongGanBtEvent;
		InputEventCtrl.GetInstance().ClickInsertcoinBtEvent += ClickInsertcoinBtEvent;
		XkGameCtrl.IsLoadingLevel = false;
	}

	void Update () 
	{
		if (pcvr.bIsHardWare) {

			YouMenInfoLabel.text = pcvr.BikePowerCur.ToString();
			FangXiangInfoLabel.text = pcvr.SteerValCur.ToString();

			if (!IsInitJiaoZhunPcvr) {
				if (pcvr.mGetPower > 0.3f) {				
					YouMenInfoLabel.text += ", Throttle Response";
				}

				float offsetSteer = 0.05f;
				if (pcvr.mGetSteer < -offsetSteer) {
					FangXiangInfoLabel.text += ", Turn Left";
				}
				else if (pcvr.mGetSteer > offsetSteer) {
					FangXiangInfoLabel.text += ", Turn Right";
				}
				else {
					FangXiangInfoLabel.text += ", Turn Middle";
				}
			}
		}
		else {
			if (Input.GetKeyDown(KeyCode.T)) {
				OnClickInsertBt();
			}

			int val = (int)(pcvr.mGetSteer * 100);
			FangXiangInfoLabel.text = val.ToString();
			if (!IsInitJiaoZhunPcvr) {
				if (val < 0) {
					FangXiangInfoLabel.text += ", Turn Left";
				}
				else if (val > 0) {
					FangXiangInfoLabel.text += ", Turn Right";
				}
				else {
					FangXiangInfoLabel.text += ", Turn Middle";
				}
			}

			val = (int)(pcvr.mGetPower * 100);
			YouMenInfoLabel.text = val.ToString();
			if (!IsInitJiaoZhunPcvr) {
				if (val > 0) {				
					YouMenInfoLabel.text += ", Throttle Response";
				}
			}
		}
	}

	void OnClickInsertBt()
	{
		if (!pcvr.bIsHardWare)
		pcvr.coinCurNumPCVR ++;
		//ReadGameInfo.GetInstance().WriteInsertCoinNum(pcvr.coinCurNumPCVR.ToString());
		UpdateInsertCoin();
	}
	
	void UpdateInsertCoin()
	{
		InsertCoinNumLabel.text = pcvr.coinCurNumPCVR.ToString();
	}

	void ClickSetMoveBtEvent(ButtonState val)
	{
		if (val == ButtonState.DOWN) {
			return;
		}
		OnClickMoveBtInZhujiemian();
	}

	void ClickSetEnterBtEvent(ButtonState val)
	{
		if (val == ButtonState.DOWN) {
			return;
		}
		OnClickSelectBtInZhujiemian();
	}

	void ClickStartBtOneEvent(ButtonState val)
	{Debug.Log ("startbuttonnnnnnnnnnnnnnnnnnnnnnnn  setpanelllll " + val);
		if (val == ButtonState.DOWN) {
			BtInfoLabel.text = "StartBtDown";
		}
		else {
			BtInfoLabel.text = "StartBtUp";
			if (IsInitJiaoZhunPcvr) {
				if (JiaoZhunCount > 3) {
					ResetJiaoZhunPcvr();
				}
				else {
					UpdataJiaoZhunTexture();
				}
			}
		}
	}

	void ClickCloseDongGanBtEvent(ButtonState val)
	{
		if (val == ButtonState.DOWN) {
			BtInfoLabel.text = "DongGanBtDown";
		}
		else {
			BtInfoLabel.text = "DongGanBtUp";
		}
	}
	
	//keyboard  -  T  -  insert coin
	void ClickInsertcoinBtEvent(ButtonState val)
	{		
		if (val == ButtonState.DOWN) {
			//pcvr.coinCurNumPCVR ++;
			UpdateInsertCoin();
			
			/*if (pcvr.sound2DScrObj)
			{
				pcvr.sound2DScrObj.playAudioInsertCoin();
			}*/
		}
	}

	void ResetJiaoZhunPcvr()
	{
		if (!IsInitJiaoZhunPcvr) {
			return;
		}
		m_ZhujiemianXingXing.gameObject.SetActive(true);
		IsInitJiaoZhunPcvr = false;
		JiaoZhunObj.SetActive(false);
	}

	void InitJiaoZhunPcvr()
	{
		if (IsInitJiaoZhunPcvr) {
			return;
		}
		pcvr.GetInstance().InitFangXiangJiaoZhun();
		m_ZhujiemianXingXing.gameObject.SetActive(false);
		IsInitJiaoZhunPcvr = true;

		JiaoZhunCount = 3;
		JiaoZhunTexture.mainTexture = JiaoZhunTextureArray[3];
		JiaoZhunObj.SetActive(true);
	}

	void UpdataJiaoZhunTexture()
	{
		JiaoZhunCount ++;
		JiaoZhunTexture.mainTexture = JiaoZhunTextureArray[JiaoZhunCount];
	}

	void OnClickMoveBtInZhujiemian()
	{
		if (IsInitJiaoZhunPcvr) {
			return;
		}

		m_IndexZhujiemian ++;
		if(m_IndexZhujiemian > 25)
		{
			m_IndexZhujiemian = 0;
		}
		switch (m_IndexZhujiemian)
		{
		case 0:
		{
			m_ZhujiemianXingXing.localPosition = new Vector3(-510.0f,212.0f,0.0f);
			break;
		}
		case 1:
		{
			m_ZhujiemianXingXing.localPosition = new Vector3(-640.0f,139.0f,0.0f);
			break;
		}
		case 2:
		{
			m_ZhujiemianXingXing.localPosition = new Vector3(6.7f,139.0f,0.0f);
			break;
		}
		case 3:
		{
			m_ZhujiemianXingXing.localPosition = new Vector3(-510.0f,66.0f,0.0f);
			break;
		}
		case 4:
		{
			m_ZhujiemianXingXing.localPosition = new Vector3(-510.0f,-4.5f,0.0f);
			break;
		}
		case 5:
		{
			m_ZhujiemianXingXing.localPosition = new Vector3(-275.9f,-4.5f,0.0f);
			break;
		}
		case 6:
		{
			m_ZhujiemianXingXing.localPosition = new Vector3(-111.4f,-4.5f,0.0f);
			break;
		}
		case 7:
		{
			m_ZhujiemianXingXing.localPosition = new Vector3(-610.0f,-78.0f,0.0f);
			break;
		}
		case 8:
		{
			m_ZhujiemianXingXing.localPosition = new Vector3(-610.0f,-108.0f,0.0f);
			break;
		}
		case 9:
		{
			m_ZhujiemianXingXing.localPosition = new Vector3(-610.0f,-138.0f,0.0f);
			break;
		}
		case 10:
		{
			m_ZhujiemianXingXing.localPosition = new Vector3(-610.0f,-168.0f,0.0f);
			break;
		}
		case 11:
		{
			m_ZhujiemianXingXing.localPosition = new Vector3(-610.0f,-198.0f,0.0f);
			break;
		}
		case 12:
		{
			m_ZhujiemianXingXing.localPosition = new Vector3(-610.0f,-228.0f,0.0f);
			break;
		}
		case 13:
		{
			m_ZhujiemianXingXing.localPosition = new Vector3(-610.0f,-258.0f,0.0f);
			break;
		}
		case 14:
		{
			m_ZhujiemianXingXing.localPosition = new Vector3(-610.0f,-288.0f,0.0f);
			break;
		}
		case 15:
		{
			m_ZhujiemianXingXing.localPosition = new Vector3(56.0f,-81.0f,0.0f);
			break;
		}
		case 16:
		{
			m_ZhujiemianXingXing.localPosition = new Vector3(372.40f,-81.0f,0.0f);
			break;
		}
		case 17:
		{//volume
			m_ZhujiemianXingXing.localPosition = new Vector3(52.3f,-158.4f,0.0f);
			break;
		}
		case 18:
		{//resetvolume
			m_ZhujiemianXingXing.localPosition = new Vector3(542.6f,-157.1f,0.0f);
			break;
		}
		case 19:
		{//game time
			m_ZhujiemianXingXing.localPosition = new Vector3(542.6f,-188.6f,0.0f);
			break;
		}
		case 20:
		{//net work
			m_ZhujiemianXingXing.localPosition = new Vector3(425.7f,-231.4f,0.0f);
			break;
		}
		case 21:
		{//net work
			m_ZhujiemianXingXing.localPosition = new Vector3(591.6f,-231.4f,0.0f);
			break;
		}
		case 22:
		{//CH
			m_ZhujiemianXingXing.localPosition = new Vector3(284.5f,-278.9f,0.0f);
			break;
		}
		case 23:
		{//EN
			m_ZhujiemianXingXing.localPosition = new Vector3(586.3f,-278.9f,0.0f);
			break;
		}
		case 24:
		{//fang xiang pan
			m_ZhujiemianXingXing.localPosition = new Vector3(149.0f,-358.0f,0.0f);
			break;
		}
		case 25:
		{//exit
			m_ZhujiemianXingXing.localPosition = new Vector3(-510.0f,-358.0f,0.0f);
			break;
		}
		}
	}
	void OnClickSelectBtInZhujiemian()
	{
		switch(m_IndexZhujiemian)
		{
		case 0:
		{
			int CoinNum = Convert.ToInt32(m_CoinForStar.text);
			CoinNum ++;
			if(CoinNum >9)
			{
				CoinNum = 1;
			}
			m_CoinForStar.text = CoinNum.ToString();
			ReadGameInfo.GetInstance().WriteStarCoinNumSet(CoinNum.ToString());
			break;
		}
		case 1:
		{
			m_GameModeDuigou1.enabled = true;
			m_GameModeDuigou2.enabled = false;
			ReadGameInfo.GetInstance().WriteGameStarMode("oper");

			ReadGameInfo.GetInstance().ReadStarCoinNumSet();

			if (pcvr.startCoinNumPCVR > 0)
			{
				m_CoinForStar.text = pcvr.startCoinNumPCVR.ToString();
			}
			else
			{
				pcvr.startCoinNumPCVR = 1;
				m_CoinForStar.text = pcvr.startCoinNumPCVR.ToString();
				ReadGameInfo.GetInstance().WriteStarCoinNumSet(pcvr.startCoinNumPCVR.ToString());
			}
			break;
		}
		case 2:
		{
			m_GameModeDuigou1.enabled = false;
			m_GameModeDuigou2.enabled = true;
			ReadGameInfo.GetInstance().WriteGameStarMode("FREE");
			
			ReadGameInfo.GetInstance().WriteStarCoinNumSet("0");
			pcvr.startCoinNumPCVR = 0;
			m_CoinForStar.text = pcvr.startCoinNumPCVR.ToString();
			break;
		}
		case 3:
		{
			ResetFactory();
			break;
		}
		case 4:
		{
			pcvr.StartBtLight = StartLightState.Liang;
			break;
		}
		case 5:
		{
			pcvr.StartBtLight = StartLightState.Shan;
			break;
		}
		case 6:
		{
			pcvr.StartBtLight = StartLightState.Mie;
			break;
		}
		case 7:
		{
			pcvr.m_IsOpneForwardQinang = true;
			pcvr.GetInstance ().ControlQinang (1);
			break;
		}
		case 8:
		{
			CloseAllQiNang();
			break;
		}
		case 9:
		{
			pcvr.m_IsOpneRightQinang = true;
			pcvr.GetInstance ().ControlQinang (2);
			break;
		}
		case 10:
		{
			CloseAllQiNang();
			break;
		}
		case 11:
		{
			pcvr.m_IsOpneBehindQinang = true;
			pcvr.GetInstance ().ControlQinang (3);
			break;
		}
		case 12:
		{
			CloseAllQiNang();
			break;
		}
		case 13:
		{
			pcvr.m_IsOpneLeftQinang = true;
			pcvr.GetInstance ().ControlQinang (4);
			break;
		}
		case 14:
		{
			CloseAllQiNang();
			break;
		}
		case 15:
		{
			InitJiaoZhunPcvr();
			break;
		}
		case 16:
		{
			int lingmindu = Convert.ToInt32(m_Lingmindu.text);
			lingmindu ++;
			if(lingmindu >9)
			{
				lingmindu = 0;
			}
			m_Lingmindu.text = lingmindu.ToString();
			ReadGameInfo.GetInstance().WriteLinghuodu(lingmindu.ToString());
			break;
		}
		case 17:
		{
			//audio - 0-10
			if (m_VolumeDuigou1.enabled)
			{
				int volumNum = Convert.ToInt32(m_Volume.text);
				volumNum++;
				if(volumNum > 10)
				{
					volumNum = 0;
				}
				m_Volume.text = volumNum.ToString();
				ReadGameInfo.GetInstance().WriteVolumNumSet(volumNum.ToString());
			}
			else
			{
				m_VolumeDuigou1.enabled = true;
				m_VolumeDuigou2.enabled = false;
				ReadGameInfo.GetInstance().WriteGameVolum("off");
			}

			break;
		}
		case 18:
		{
			//audio - normal
			m_VolumeDuigou1.enabled = false;
			m_VolumeDuigou2.enabled = true;
			ReadGameInfo.GetInstance().WriteGameVolum("on");
			ReadGameInfo.GetInstance().WriteVolumNumSet("7");
			m_Volume.text = "7";
			break;
		}
		case 19:
		{//game time
			int gameTime = Convert.ToInt32(m_GameTimeLable.text);
			gameTime += 30;
			if(gameTime > 300)
			{
				gameTime = 180;
			}
			m_GameTimeLable.text = gameTime.ToString();
			ReadGameInfo.GetInstance().WriteGameTimeSet(gameTime.ToString());
			break;
		}
		case 20:
		{
			m_NetworkDuigou1.enabled = true;
			m_NetworkDuigou2.enabled = false;
			ReadGameInfo.GetInstance().WriteGameNetwork("on");
			break;
		}
		case 21:
		{
			m_NetworkDuigou1.enabled = false;
			m_NetworkDuigou2.enabled = true;
			ReadGameInfo.GetInstance().WriteGameNetwork("off");
			break;
		}
		case 22:
		{
			m_CHENDuigou1.enabled = true;
			m_CHENDuigou2.enabled = false;
			ReadGameInfo.GetInstance().WriteGameCHEN("1");
			break;
		}
		case 23:
		{
			m_CHENDuigou1.enabled = false;
			m_CHENDuigou2.enabled = true;
			ReadGameInfo.GetInstance().WriteGameCHEN("0");
			break;
		}
		case 24:
		{
			int fxpDengji = Convert.ToInt32(m_FangxiangpanLabel.text);
			fxpDengji ++;
			if(fxpDengji > 8)
			{
				fxpDengji = 0;
			}
			m_FangxiangpanLabel.text = fxpDengji.ToString();
			ReadGameInfo.GetInstance().WriteFangxiangpanDengji(fxpDengji.ToString());
			break;
		}
		case 25:
		{
			CloseAllQiNang();
			pcvr.StartBtLight = StartLightState.Mie;
			XkGameCtrl.IsLoadingLevel = true;
			IsOpenSetPanel = false;
			pcvr.GetInstance().getGamesetInfor();
			Resources.UnloadUnusedAssets();
			pcvr.UIState = 0;
			GC.Collect();
			Application.LoadLevel(0);
			break;
		}
		}
	}

	void CloseAllQiNang()
	{
		pcvr.m_IsOpneForwardQinang = false;
		pcvr.m_IsOpneBehindQinang = false;
		pcvr.m_IsOpneLeftQinang = false;
		pcvr.m_IsOpneRightQinang = false;
		pcvr.GetInstance ().ControlQinang (10);
	}

	void ResetFactory()
	{
		ReadGameInfo.GetInstance().FactoryReset();
		m_CoinForStar.text = "1";
		m_Volume.text = "7";
		m_GameModeDuigou1.enabled = true;
		m_GameModeDuigou2.enabled = false;
		m_NetworkDuigou1.enabled = true;
		m_NetworkDuigou2.enabled = false;
		m_VolumeDuigou1.enabled = false;
		m_VolumeDuigou2.enabled = true;
		m_CHENDuigou1.enabled = true;
		m_CHENDuigou2.enabled = false;

		if (pcvr.bIsHardWare) {
			pcvr.GetInstance().SubPlayerCoin(pcvr.coinCurNumPCVR);
		}
		pcvr.coinCurNumPCVR = 0;
		UpdateInsertCoin();
	}

	void ClickShaCheBtEvent(ButtonState val)
	{
		if (val == ButtonState.DOWN) {
			BtInfoLabel.text = "BrakeBtDown";
		}
		else {
			BtInfoLabel.text = "BrakeBtUp";
		}
	}

	void ClickLaBaBtEvent(ButtonState val)
	{
		if (val == ButtonState.DOWN) {
			BtInfoLabel.text = "SpeakerBtDown";
		}
		else {
			BtInfoLabel.text = "SpeakerBtUp";
		}
	}
}
