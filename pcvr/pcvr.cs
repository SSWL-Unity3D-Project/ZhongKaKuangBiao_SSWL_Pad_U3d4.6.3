using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;
using System.IO;

public class pcvr : MonoBehaviour {
	static public bool bIsHardWare = false;
	public static bool IsPlayerActivePcvr = true;
	static bool IsTestGame = false;
	public static uint ShaCheCurPcvr;
	static bool IsClickLaBaBt;
	static bool IsClickChangeCameraBt;
	static public uint gOldCoinNum = 0;
	static private uint mOldCoinNum = 0;
	//public int CoinNumCurrent = 0;
	static public bool IsCloseDongGanBtDown = false;
	static public bool bPlayerStartKeyDown = false;
	private bool bSetEnterKeyDown = false;
	static public bool bSetMoveKeyDown = false;
	public static bool IsZhenDongFangXiangPan;
	int SubCoinNum = 0;
	public static bool m_IsOpneForwardQinang = false;
	public static bool m_IsOpneBehindQinang = false;
	public static bool m_IsOpneLeftQinang = false;
	public static bool m_IsOpneRightQinang = false;
	public static bool m_IsOpneQinang1 = false;
	public static bool m_IsOpneQinang2 = false;
	public static bool m_IsOpneQinang3 = false;
	public static bool m_IsOpneQinang4 = false;
	public static uint SteerValMax = 0;
	public static uint SteerValCen = 0;
	public static uint SteerValMin = 0;
	public static uint SteerValCur;
	public static float mGetSteer = 0f;
	public static uint BikeShaCheCur;
	public static uint mBikePowerMin = 999999;
	public static uint mBikePowerMax = 0;
	public static float mGetPower = 0f;
	static uint BikePowerLen = 0;
	public static uint BikePowerCur;
	public static uint BikePowerOld;
	bool bIsJiaoYanBikeValue = false;
	static bool IsInitYouMenJiaoZhun = false;
	bool IsJiaoZhunFireBt;
	bool IsFanZhuangYouMen;
	static bool IsInitFangXiangJiaoZhun;
	bool IsFanZhuangFangXiang;
	int FangXiangJiaoZhunCount;
	public static uint CoinCurPcvr;
	public static uint BikePowerCurPcvr;
	public static StartLightState StartBtLight = StartLightState.Mie;
	public static StartLightState DongGanBtLight = StartLightState.Mie;
	bool IsCleanHidCoin;
	static uint BikeDirLenA;
	static uint BikeDirLenB;
	static uint BikeDirLenC;
	static uint BikeDirLenD;
	public static bool IsActiveShaCheEvent;
	public static float shacheValue = 0.0f;
	static bool IsInitShaCheJiaoZhun;
	static bool IsFanZhuangShaChe;
	public static uint mBikeShaCheMin = 999999;
	public static uint mBikeShaCheMax = 0;
	static uint BikeShaCheLen = 1;
	bool IsPlayFangXiangPanZhenDong;
	public static bool isDanqitishi = false;

	//some record information
	public static int coinCurNumPCVR = 0;		 //player coin num
	public static int startCoinNumPCVR = 0;		//start-game coin num
	public static int gameModePCVR = 0;			//game mode:	1-operate; 0-free
	public static int networkPCVR = 0;			//network mode:	1-on; 0-off
	public static int gametimePCVR = 0;			//game time
	public static int recordPCVR = 0;			//player record
	public static int dongganPCVR = 1;			//dong gan:		1-open;0-close
	public static int languagePCVR = 1;			//Language: 1-CH	0-EN
	public static bool IsCloseQiNang = false;
	public static int linghuoduPCVR = 1;			//0-9
	private bool isQinangDoudong = false;
	//prefes de int or string and read d zhuan huan  null "" o fanhuizhi <=0 dei qu fen liangge zaiyiqi de 

	//some information about link
	public static bool isLinkMode = false;
	public static bool isServerPC = false;
	public static NetworkPlayer[] netPlayerObjArr;
	public static int UIState = 0;	//1-movie; 2-link; 3-game; 4-setpanel
	public static int selectLevelFinalLink = -1;

	public static int firstPlayerIndex = -1;
	public static int totalPlayerNum = -1;
	public static Transform selfServerObj = null;
	public static serverbject selfServerScrObj = null;
	public static aFirstHere aFirstScriObj = null;
	public static donghua donghuaScriObj = null;
	public static UITruck UITruckScrObj = null;
	public static smallMapUI smallMapUIScrObj = null;
	public static soundControl2D sound2DScrObj = null;
	public static soundControlEND2D sound2DENDScrObj = null;
	public static GameObject smallMapObj = null;
	public static Transform linkguiTrans = null;
	public static int[] numArr;
	public static serverCameraScript serCameraSObj = null;
	public static XKCarCameraCtrl XKCarCameraSObj = null;
	public static Transform XKCarCameraTranform = null;
	public static RadialBlur RadialBlurSObj = null;
	public static car carSObj = null;
	public static truck playerTruckSObj = null;
	public static float playerThrottle = 0;
	public static float playerSteer = 0;
	public static bool playerBrake = false;
	public static bool isChehen = false;
	public static GameObject xuanguanBeijingClicPlayObj;
	public static GameObject soundENDPlayObj;
	public static CameraShake cameraShakeSObj = null;
	public static danqiTishi danqitishiSObj = null;
	public static HeatDistort heatDistortSObj = null;
	public AudioSource audioBeijingUI;
	public static scoreControl scoreCtrlSobj = null;

	public static pcvr Instance = null;

	public static float[] distanceFour;
	public static int[] indexFour;		//these will record after the four players in game: p1 p2 p3 p4 AI1 AI2 AI3 AI4 0-7
	public static int[] indexOnly;		//the mingci(0 1 2 3)------(2 3 0 1)
	public static int[] indexOnlyFinal;		//the mingci(0 1 2 3)------(2 3 0 1)
	public static int finishedNumber = 0;
	public static int finishedPlayerNumber = 0;
	public static int selfIndex = -1;	//0-p1, 1-p2, 2-p3, 4-p4-----------will change according the game
	public static int[] jiaohuiPointArr;
	public static int[] lahuiPointArr;
	public static truck[] truckObjScriptArr;		//only in the function "showPlayerInfor()" use
	public static gameServerObjSript[] gameServerObjSriptArr;
	public static Transform[] truckObjTransArr;		//only in the function "showPlayerInfor()" use
	public static int mingciFirstPlayer = 0;
	public static int uiRunState = -1;
	public static float total321Time = 0;
	public static float totalTime = 0;
	public static float totalTimeServer = 0;
	public static float totalTimeAddDaoju = 0;
	public static bool isPassgamelevel = false;
	public static float[] useTimeSelf;
	public static float[] useTimeSelfTemp;

	//add - 20170623
	public byte liFankuiQiangdu = 0x00;
	public static byte zhendongFangxiang = 0x00;
	public static byte zhendongQiangdu = 0x00;
	public static byte zhendongShijian = 0x00;
	public static int zhendongDengjiFXP = 4;

	private float lifankuiqiangduTemp = 0.0f;
	private float lifankuiqiangduAdd = 0.0f;
	public byte liFankuiQiangduUse = 0x01;
	//*********************************
	//													   shatu        tudi        suishidi     shuidi      NPC       zudang    xiaodaoju
	private int[,] zhendongQiangduTimeArr1 = new int[7, 3]{{4, 2, 10}, {2, 2, 20}, {2, 2, 5}, {5, 5, 20}, {2, 4, 30}, {2, 4, 30}, {2, 2, 2}};
	private int[,] zhendongQiangduTimeArr2 = new int[7, 3]{{6, 4, 10}, {4, 4, 20}, {4, 4, 5}, {7, 7, 20}, {4, 6, 30}, {4, 6, 30}, {4, 4, 4}};
	private int[,] zhendongQiangduTimeArr3 = new int[7, 3]{{8, 4, 10}, {6, 6, 20}, {6, 6, 5}, {9, 9, 20}, {6, 8, 30}, {6, 8, 30}, {6, 6, 6}};
	private int[,] zhendongQiangduTimeArr4 = new int[7, 3]{{10, 6, 10}, {8, 8, 20}, {8, 8, 5}, {11, 11, 20}, {8, 10, 30}, {8, 10, 30}, {8, 8, 8}};
	private int[,] zhendongQiangduTimeArr5 = new int[7, 3]{{12, 8, 10}, {10, 10, 20}, {10, 10, 5}, {12, 12, 20}, {10, 12, 30}, {10, 12, 30}, {10, 10, 10}};
	private int[,] zhendongQiangduTimeArr6 = new int[7, 3]{{14, 10, 10}, {12, 12, 20}, {12, 12, 5}, {14, 14, 20}, {12, 14, 30}, {12, 14, 30}, {12, 12, 12}};
	private int[,] zhendongQiangduTimeArr7 = new int[7, 3]{{16, 12, 10}, {14, 14, 20}, {14, 14, 5}, {16, 16, 20}, {14, 16, 30}, {14, 16, 30}, {14, 14, 14}};
	private int[,] zhendongQiangduTimeArr8 = new int[7, 3]{{18, 14, 10}, {16, 16, 20}, {16, 16, 5}, {18, 18, 20}, {16, 18, 30}, {16, 18, 30}, {16, 16, 16}};

	private int[,] zhendongQiangduTimeArr = new int[7, 3]{{0, 0, 0}, {0, 0, 0}, {0, 0, 0}, {0, 0, 0}, {0, 0, 0}, {0, 0, 0}, {0, 0, 0}};

	public static GameObject[] chanshengdeObj;
	public static GameObject[] chanshengdeObjNet;
	public static int chanshengdeObjNum = 0;
	public static int chanshengdeObjNetNum = 0;

	//about single select level 20170908
	public static int selectLevelSIndex = 0;
	public static int selectTruckSIndex = 0;
	public static bool isPressStartButton = false;
	public static bool isCanPressStartButtonEndGame = false;
	public static bool isAfterJifen = false;

	//about english version
	public static UIAtlas[] UIENAtlasObjSpeciaPCVR;//load-beijing-select-chatu
	public static UIAtlas UIEN1AtlasObjPCVR;//en1

	//about score show
	private float[,] shijianQuyu = new float[8, 6]{
		{135.00f, 140.00f, 150.00f, 170.00f, 200.00f, 230.00f},
		{135.00f, 140.00f, 150.00f, 170.00f, 200.00f, 230.00f},
		{160.00f, 170.00f, 180.00f, 200.00f, 220.00f, 230.00f},
		{160.00f, 170.00f, 180.00f, 200.00f, 220.00f, 230.00f},
		{130.00f, 140.00f, 145.00f, 160.00f, 190.00f, 230.00f},
		{130.00f, 140.00f, 145.00f, 160.00f, 190.00f, 230.00f},
		{150.00f, 160.00f, 170.00f, 180.00f, 190.00f, 230.00f},
		{115.00f, 120.00f, 130.00f, 170.00f, 200.00f, 230.00f}
	};

	public static float[] curShijianquyu = new float[6]{0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f};

	public static float[] pingjiJiangli = new float[7]{1.0f, 1.0f, 1.0f, 0.8f, 0.65f, 0.5f, 0.4f};
	public static int curPingjiDengji = -1;
	public static float shangciShengyuJiangli = 0.0f;
	public static int[] guakaDengji = new int[8]{-1, -1, -1, -1, -1, -1, -1, -1};
	public static bool isChuangguan = false;
	public static bool canFree = false;
	
	public static Shader nowShaderPcvr1 = null;
	public static Shader nowShaderPcvr2 = null;

	public static int port = 9717;				//use this port
	public static string passWord = "1614";		//the password
	public static string passWordNew = "1408";	//start game will change password to this
	public static string ipString = "192.168.1.112";
	public static int connections = 4;

	static public pcvr GetInstance()
	{
		if (Instance == null)
		{
			GameObject obj = new GameObject("_PCVR");
			DontDestroyOnLoad(obj);
			Instance = obj.AddComponent<pcvr>();
			ScreenLog.init();

			if (bIsHardWare)
			{
				MyCOMDevice.GetInstance();
			}
			
			netPlayerObjArr = new NetworkPlayer[connections];
			indexFour = new int[4]{0, 1, 6, 7};				//will change according to the game
			distanceFour = new float[4]{0, 0, 0, 0};
			jiaohuiPointArr = new int[4]{0, 0, 0, 0};
			lahuiPointArr = new int[4]{0, 0, 0, 0};
			indexOnly = new int[]{0, 1, 2, 3};
			indexOnlyFinal = new int[]{-1, -1, -1, -1};
			numArr = new int[4]{0, 0, 0, 0};
			truckObjScriptArr = new truck[connections];
			gameServerObjSriptArr = new gameServerObjSript[connections];
			truckObjTransArr = new Transform[connections];
			useTimeSelf = new float[4]{-1.0f, -1.0f, -1.0f, -1.0f};
			useTimeSelfTemp = new float[4]{-1.0f, -1.0f, -1.0f, -1.0f};
			buffer4 = new int[8]{0, 0, 0, 0, 0, 0, 0, 0};
			buffer25 = new int[8]{0, 0, 0, 0, 0, 0, 0, 0};
			buffer26 = new int[8]{0, 0, 0, 0, 0, 0, 0, 0};
			bufferTime25 = new float[8]{0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f};
			bufferTime26 = new float[8]{0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f};
			bufferLiangTime25 = new float[8]{0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f};
			bufferLiangTime26 = new float[8]{0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f};

			chanshengdeObj = new GameObject[200];
			chanshengdeObjNet = new GameObject[100];

			UIENAtlasObjSpeciaPCVR = new UIAtlas[4];
		}
		return Instance;
	}

	public void initInforAgain(bool isInitL)
	{
		for (int i = 0; i< 4; i++)
		{
			distanceFour[i] = 0;
			jiaohuiPointArr[i] = 0;
			lahuiPointArr[i] = 0;
			indexOnlyFinal[i] = -1;
			useTimeSelf[i] = -1.0f;
			useTimeSelfTemp[i] = -1.0f;
		}

		if (isInitL)
		{
			for (int i = 0; i< 4; i++)
			{
				numArr[i] = 0;
			}

			for (int i = 0; i< 8; i++)
			{
				buffer4[i] = 0;
				buffer25[i] = 0;
				buffer26[i] = 0;
				bufferTime25[i] = 0.0f;
				bufferTime26[i] = 0.0f;
				bufferLiangTime25[i] = 0.0f;
				bufferLiangTime26[i] = 0.0f;
			}
		}

		indexFour [0] = 0;
		indexFour [1] = 1;
		indexFour [2] = 6;
		indexFour [3] = 7;
		
		indexOnly [0] = 0;
		indexOnly [1] = 1;
		indexOnly [2] = 2;
		indexOnly [3] = 3;

		isQinangDoudong = false;
		m_IsOpneForwardQinang = false;
		m_IsOpneBehindQinang = false;
		m_IsOpneLeftQinang = false;
		m_IsOpneRightQinang = false;
		ControlQinang (10);

		IsZhenDongFangXiangPan = false;
	}

	public void initEndGamele()
	{
		isQinangDoudong = false;
		m_IsOpneForwardQinang = false;
		m_IsOpneBehindQinang = false;
		m_IsOpneLeftQinang = false;
		m_IsOpneRightQinang = false;
		ControlQinang (10);

		ControlShanguangdeng (101, false);

		IsZhenDongFangXiangPan = false;
	}

	//void Start()
	void Awake()
	{
		Debug.Log ("pcvr start function here");
	}

	void AwakeAfterReadInor()
	{
        //get the gameset information
		//if (pcvr.bIsHardWare)
        //Screen.SetResolution(1360, 768, true);
        //getGamesetInfor ();

		if (UITruckScrObj)
		{
			if (Network.isServer)
			{
				changeTimeR();
			}
			else if (!Network.isClient)
			{
				UITruckScrObj.setGameTime(2, gametimePCVR);
			}
		}
		else
		{
			if (Network.isServer)
			{
				Invoke("changeTimeR",0.2f);
			}
			else if (!Network.isClient)
			{
				Invoke("setTimeTTT",0.2f);
			}
		}

		InitJiaoYanMiMa();

		//FangXiangInfo
		/*SteerValMin = (uint)PlayerPrefs.GetInt("mBikeDirMin");
		SteerValCen = (uint)PlayerPrefs.GetInt("mBikeDirCen");
		SteerValMax = (uint)PlayerPrefs.GetInt("mBikeDirMax");
		CheckBikeDirLen();*/
		
		//YouMenInfo
		//mBikePowerMin = (uint)PlayerPrefs.GetInt("mBikePowerMin");
		//mBikePowerMax = (uint)PlayerPrefs.GetInt("mBikePowerMax");
		BikePowerLen = mBikePowerMax < mBikePowerMin ? (mBikePowerMin - mBikePowerMax + 1) : (mBikePowerMax - mBikePowerMin + 1);
		BikePowerLen = Math.Max(1, BikePowerLen);

		//mBikeShaCheMin = (uint)PlayerPrefs.GetInt("mBikeShaCheMin");
		//mBikeShaCheMax = (uint)PlayerPrefs.GetInt("mBikeShaCheMax");
		BikeShaCheLen = mBikeShaCheMax < mBikeShaCheMin ? (mBikeShaCheMin - mBikeShaCheMax + 1) : (mBikeShaCheMax - mBikeShaCheMin + 1);
		BikeShaCheLen = Math.Max(1, BikeShaCheLen);

		/*liFankuiQiangdu = (byte)PlayerPrefs.GetInt("liFankuiQiangdu");
		zhendongQiangdu = (byte)(Convert.ToInt32(PlayerPrefs.GetInt("zhendongQiangdu")));
		zhendongShijian = (byte)PlayerPrefs.GetInt("zhendongShijian");*/

		getLifankuiFXP ();
	}

	void setTimeTTT()
	{
		if (UITruckScrObj)
		{
			UITruckScrObj.setGameTime(1,gametimePCVR);
		}
	}

	void changeTimeR()
	{		
		if (Network.isServer && pcvr.selfServerScrObj)
		{
			pcvr.selfServerScrObj.setServerTime(gametimePCVR);
			
			return;
		}
	}

	void getLifankuiFXP()
	{
		if (zhendongDengjiFXP <= 1)
		{
			for (int i=0; i<7; i++)
			{
				for (int j=0; j<3; j++)
				{
					zhendongQiangduTimeArr[i, j] = zhendongQiangduTimeArr1[i, j];
				}
			}
		}
		else if (zhendongDengjiFXP == 2)
		{
			for (int i=0; i<7; i++)
			{
				for (int j=0; j<3; j++)
				{
					zhendongQiangduTimeArr[i, j] = zhendongQiangduTimeArr2[i, j];
				}
			}
		}
		else if (zhendongDengjiFXP == 3)
		{
			for (int i=0; i<7; i++)
			{
				for (int j=0; j<3; j++)
				{
					zhendongQiangduTimeArr[i, j] = zhendongQiangduTimeArr3[i, j];
				}
			}
		}
		else if (zhendongDengjiFXP == 4)
		{
			for (int i=0; i<7; i++)
			{
				for (int j=0; j<3; j++)
				{
					zhendongQiangduTimeArr[i, j] = zhendongQiangduTimeArr4[i, j];
				}
			}
		}
		else if (zhendongDengjiFXP == 5)
		{
			for (int i=0; i<7; i++)
			{
				for (int j=0; j<3; j++)
				{
					zhendongQiangduTimeArr[i, j] = zhendongQiangduTimeArr5[i, j];
				}
			}
		}
		else if (zhendongDengjiFXP == 6)
		{
			for (int i=0; i<7; i++)
			{
				for (int j=0; j<3; j++)
				{
					zhendongQiangduTimeArr[i, j] = zhendongQiangduTimeArr6[i, j];
				}
			}
		}
		else if (zhendongDengjiFXP == 7)
		{
			for (int i=0; i<7; i++)
			{
				for (int j=0; j<3; j++)
				{
					zhendongQiangduTimeArr[i, j] = zhendongQiangduTimeArr7[i, j];
				}
			}
		}
		else if (zhendongDengjiFXP == 8)
		{
			for (int i=0; i<7; i++)
			{
				for (int j=0; j<3; j++)
				{
					zhendongQiangduTimeArr[i, j] = zhendongQiangduTimeArr8[i, j];
				}
			}
		}

		if (zhendongDengjiFXP == 0)
		{
			//liFankuiQiangdu = 0x00;
			liFankuiQiangdu = (byte)zhendongQiangduTimeArr [0, 0];
			zhendongQiangdu = 0x00;
			zhendongShijian = 0x00;
		}
		else
		{
			liFankuiQiangdu = (byte)zhendongQiangduTimeArr [0, 0];
			zhendongQiangdu = 0x00;
			zhendongShijian = 0x00;
		}

		lifankuiqiangduAdd = liFankuiQiangdu / 4.0f;
	}

	public void setShijianquyu(int levelIndex)
	{
		if (levelIndex < 0)
		{
			return;
		}

		for (int i=0; i<6; i++)
		{
			curShijianquyu[i] = shijianQuyu[levelIndex, i];
		}
	}

	public void getGamesetInfor()
	{
		//ReadGameInfo.GetInstance ().ReadInsertCoinNum ();
		ReadGameInfo.GetInstance ().ReadStarCoinNumSet ();
		ReadGameInfo.GetInstance ().ReadGameStarMode ();
		ReadGameInfo.GetInstance ().ReadGameNetwork ();
		ReadGameInfo.GetInstance ().ReadGameTimeSet ();
		ReadGameInfo.GetInstance ().ReadGameRecord ();
		ReadGameInfo.GetInstance ().ReadFangxiangpanDengji ();
		ReadGameInfo.GetInstance ().ReadGameCHEN ();
		ReadGameInfo.GetInstance ().ReadGameLinghuodu ();

		ReadGameInfo.GetInstance ().ReadBikePowerMin ();
		ReadGameInfo.GetInstance ().ReadBikePowerMax ();
		ReadGameInfo.GetInstance ().ReadBikeShaCheMin ();
		ReadGameInfo.GetInstance ().ReadBikeShaCheMax ();

		Debug.Log ("coinCurNumPCVR  " + coinCurNumPCVR);
		Debug.Log ("startCoinNumPCVR  " + startCoinNumPCVR);
		Debug.Log ("gameModePCVR  " + gameModePCVR);
		Debug.Log ("networkPCVR  " + networkPCVR);
		Debug.Log ("gametimePCVR  " + gametimePCVR);
		Debug.Log ("recordPCVR  " + recordPCVR);
		Debug.Log ("zhendongDengjiFXP  " + zhendongDengjiFXP);

		AwakeAfterReadInor ();
	}

	//void Update()
	void FixedUpdate()
	{
		if (bIsHardWare && Screen.showCursor && Time.frameCount % 15 == 0)
		{
			Screen.showCursor = false;
		}

		if (IsOpenFangXiangPanPower && lifankuiqiangduTemp < liFankuiQiangdu)
		{
			lifankuiqiangduTemp += (Time.deltaTime * lifankuiqiangduAdd);

			liFankuiQiangduUse = (byte)Convert.ToInt32(lifankuiqiangduTemp);
			
			if (liFankuiQiangduUse <= 0x01)
			{
				liFankuiQiangduUse = 0x01;
			}
		}
		else
		{
			liFankuiQiangduUse = liFankuiQiangdu;
		
			if (liFankuiQiangduUse <= 0x01)
			{
				liFankuiQiangduUse = 0x01;
			}
		}

		//Debug.Log(StartBtLight + " "+ m_IsOpneForwardQinang + " "+ m_IsOpneBehindQinang + " "+ m_IsOpneLeftQinang + " "+ m_IsOpneRightQinang + " "+ buffer);
		if (IsTestGame  &&  Input.GetKeyUp(KeyCode.O)) {
			IsHandleDirByKey = !IsHandleDirByKey;
		}

		GetPcvrPowerVal();
		GetPcvrSteerVal();
		GetPcvrShaCheVal();
		if (!bIsHardWare) {
			return;
		}

		if (XkGameCtrl.IsLoadingLevel) {
			return;
		}

		SendMessage();
		GetMessage();
	}
	
	static byte ReadHead_1 = 0x01;
	static byte ReadHead_2 = 0x55;
	static byte WriteHead_1 = 0x02;
	static byte WriteHead_2 = 0x55;
	static byte WriteEnd_1 = 0x0d;
	static byte WriteEnd_2 = 0x0a;
	public bool IsOpenFangXiangPanPower = true;
	public static StartLightState ShaCheBtLight = StartLightState.Mie;
	public void OpenFangXiangPanPower()
	{//Debug.Log ("init OpenFangXiangPanPowerOpenFangXiangPanPowerOpenFangXiangPanPower   ");
		IsOpenFangXiangPanPower = true;
		//liFankuiQiangdu = 0x01;
		lifankuiqiangduTemp = 0.0f;
		liFankuiQiangduUse = 0x01;

		getLifankuiFXP ();
	}
	
	public void CloseFangXiangPanPower()
	{//Debug.Log ("init CloseFangXiangPanPowerCloseFangXiangPanPower   ");
		IsOpenFangXiangPanPower = false;
		liFankuiQiangdu = 0x00;
		lifankuiqiangduTemp = 0.0f;
		liFankuiQiangduUse = 0x01;
	}
	private float doudongJiange = 0.08f;
	public void OpenQinangDouDong(bool isNormal)
	{
		if (isQinangDoudong) {
			return;
		}

		if (isNormal)
		{
			doudongJiange = 0.08f;
		}
		else
		{
			doudongJiange = 0.03f;
		}
		
		isQinangDoudong = true;
		StartCoroutine(PlayQinangDouDong());
	}

	IEnumerator PlayQinangDouDong()
	{
		int count = 8;
		do {
			if (count == 8 || count == 4)
			{
				ControlQinangDoudong(1);
				ControlQinangDoudong(7);
				ControlQinangDoudong(8);
				ControlQinangDoudong(9);
			}
			else if (count == 7 || count == 3)
			{
				ControlQinangDoudong(6);
				ControlQinangDoudong(7);
				ControlQinangDoudong(3);
				ControlQinangDoudong(9);
			}
			else if (count == 6 || count == 2)
			{
				ControlQinangDoudong(6);
				ControlQinangDoudong(7);
				ControlQinangDoudong(8);
				ControlQinangDoudong(4);
			}
			else if (count == 5 || count == 1)
			{
				ControlQinangDoudong(6);
				ControlQinangDoudong(2);
				ControlQinangDoudong(8);
				ControlQinangDoudong(9);
			}
			count--;
			yield return new WaitForSeconds(doudongJiange);
		} while (count > 0);

		ControlQinangDoudong(10);
		isQinangDoudong = false;
	}

	void ControlQinangDoudong(int index)
	{
		ControlQinangHere (index);
	}

	public void ControlQinang(int index)
	{
		if (dongganPCVR == 0 || IsCloseQiNang || isQinangDoudong)
		{
			return;
		}

		ControlQinangHere (index);
	}

	void ControlQinangHere(int index)
	{
		if (IsCloseDongGanBtDown)
		{
			index = 10;
		}

		if (index == 1)
		{//forward - 1
			buffer4[0] = 1;
		}
		else if (index == 2)
		{//right - 2
			buffer4[1] = 1;
		}
		else if (index == 3)
		{//behind - 3
			buffer4[2] = 1;
		}
		else if (index == 4)
		{//left - 4
			buffer4[3] = 1;
		}
		else if (index == 6)
		{//forward - 0
			buffer4[0] = 0;
		}
		else if (index == 7)
		{//right - 2
			buffer4[1] = 0;
		}
		else if (index == 8)
		{//behind - 3
			buffer4[2] = 0;
		}
		else if (index == 9)
		{//left - 4
			buffer4[3] = 0;
		}
		else if (index == 10)
		{//all
			buffer4[0] = 0;
			buffer4[1] = 0;
			buffer4[2] = 0;
			buffer4[3] = 0;
		}
	}

	public static int []buffer4;

	void SendMessage()
	{
		//if (!MyCOMDevice.IsFindDeviceDt) {
		//	return;
		//}

		byte []buffer = new byte[MyCOMDevice.ComThreadClass.BufLenWrite];
		for (int i = 6; i < (MyCOMDevice.ComThreadClass.BufLenWrite - 2); i++) {
			//buffer[i] = (byte)UnityEngine.Random.Range(0x00, 0xff);
			buffer[i] = 0x00;
		}
		buffer[0] = WriteHead_1;
		buffer[1] = WriteHead_2;
		buffer[MyCOMDevice.ComThreadClass.BufLenWrite - 2] = WriteEnd_1;
		buffer[MyCOMDevice.ComThreadClass.BufLenWrite - 1] = WriteEnd_2;
		
		switch (StartBtLight) {
		case StartLightState.Liang:
			buffer4[6] = 1;
			break;
			
		case StartLightState.Shan:
			buffer4[6] = 1;
			break;
			
		case StartLightState.Mie:
			buffer4[6] = 0;
			break;
		}
		
		switch (ShaCheBtLight) {
		case StartLightState.Liang:
			buffer[7] = 0xaa;
			break;
			
		case StartLightState.Shan:
			buffer[7] = 0x55;
			break;
			
		case StartLightState.Mie:
			buffer[7] = 0x00;
			break;
		}

		buffer[4] = (byte)(buffer4[0] + (buffer4[1] << 1) + (buffer4[2] << 2) + (buffer4[3] << 3) + (buffer4[4] << 4) + (buffer4[5] << 5) + (buffer4[6] << 6) + (buffer4[7] << 7));
			/*
0	气囊1：充气1、放气0	（快艇气囊1）		0x01	0xFE
1	气囊2：充气1、放气0	（快艇气囊2）		0x02	0xFD
2	气囊3：充气1、放气0	（快艇气囊3）		0x04	0xFB
3	气囊4：充气1、放气0	（快艇气囊4）		0x08	0xF7

1    2

4    3
			 */

		if (IsZhenDongFangXiangPan) {
			zhendongFangxiang = 0xaa;
			buffer[6] = 0x55;
		}
		else {zhendongFangxiang = 0x00;
			if (IsOpenFangXiangPanPower) {
				buffer[6] = 0xaa;
			}
			else {
				buffer[6] = 0x00;
			}
		}

		if (IsCleanHidCoin) {
			buffer[2] = 0xaa;
			buffer[3] = 0x01;
			if (CoinCurPcvr == 0) {
				IsCleanHidCoin = false;
			}
		}
		else {
			buffer[2] = 0x00;
			buffer[3] = 0x00;
		}

		if (IsJiaoYanHid) {
			for (int i = 0; i < 4; i++) {
				buffer[i + 10] = JiaoYanMiMa[i];
			}

			for (int i = 0; i < 4; i++) {
				buffer[i + 14] = JiaoYanDt[i];
			}
		}
		else {
			RandomJiaoYanMiMaVal();
			for (int i = 0; i < 4; i++) {
				buffer[i + 10] = JiaoYanMiMaRand[i];
			}

			//0x41 0x42 0x43 0x44
			for (int i = 15; i < 18; i++) {
				buffer[i] = (byte)UnityEngine.Random.Range(0x00, 0x40);
			}
			buffer[14] = 0x00;
			
			for (int i = 15; i < 18; i++) {
				buffer[14] ^= buffer[i];
			}
		}

		buffer[5] = 0x00;
		for (int i = 2; i < 12; i++) {
			buffer[5] ^= buffer[i];
		}

		buffer[19] = 0x00;
		for (int i = 2; i < (MyCOMDevice.ComThreadClass.BufLenWrite - 2); i++) {
			if (i == 19) {
				continue;
			}
			buffer[19] ^= buffer[i];
		}

		buffer[21] = liFankuiQiangduUse;	//fan kui qiangdu

		buffer[22] = zhendongFangxiang;	//zhendong fangxiang

		buffer[23] = zhendongQiangdu;	//zhenodng qiangdu

		buffer [24] = zhendongShijian;	//zhenodng shijian

		buffer[25] = (byte)(buffer25[0] + (buffer25[1] << 1) + (buffer25[2] << 2) + (buffer25[3] << 3) + (buffer25[4] << 4) + (buffer25[5] << 5) + (buffer25[6] << 6) + (buffer25[7] << 7));
		buffer[26] = (byte)(buffer26[0] + (buffer26[1] << 1) + (buffer26[2] << 2) + (buffer26[3] << 3) + (buffer26[4] << 4) + (buffer26[5] << 5) + (buffer26[6] << 6) + (buffer26[7] << 7));
		
		MyCOMDevice.ComThreadClass.WriteByteMsg = buffer;
	}

	static void RandomJiaoYanDt()
	{	
		for (int i = 1; i < 4; i++) {
			JiaoYanDt[i] = (byte)UnityEngine.Random.Range(0x00, 0x7b);
		}
		JiaoYanDt[0] = 0x00;
		for (int i = 1; i < 4; i++) {
			JiaoYanDt[0] ^= JiaoYanDt[i];
		}
	}

	public void StartJiaoYanIO()
	{
		if (IsJiaoYanHid) {
			return;
		}

		if (true) {
			if (JiaoYanSucceedCount >= JiaoYanFailedMax) {
				return;
			}
			
			if (JiaoYanState == JIAOYANENUM.FAILED && JiaoYanFailedCount >= JiaoYanFailedMax) {
				return;
			}
		}
		RandomJiaoYanDt();
		IsJiaoYanHid = true;
		CancelInvoke("CloseJiaoYanIO");
		Invoke("CloseJiaoYanIO", 5f);
	}

	void CloseJiaoYanIO()
	{
		if (!IsJiaoYanHid) {
			return;
		}
		IsJiaoYanHid = false;
		OnEndJiaoYanIO(JIAOYANENUM.FAILED);
	}

	void OnEndJiaoYanIO(JIAOYANENUM val)
	{
		IsJiaoYanHid = false;
		if (IsInvoking("CloseJiaoYanIO")) {
			CancelInvoke("CloseJiaoYanIO");
		}

		switch (val) {
		case JIAOYANENUM.FAILED:
			JiaoYanFailedCount++;
			break;

		case JIAOYANENUM.SUCCEED:
			JiaoYanSucceedCount++;
			JiaoYanFailedCount = 0;
			break;
		}
		JiaoYanState = val;

		if (JiaoYanFailedCount >= JiaoYanFailedMax || IsJiOuJiaoYanFailed) {
			//JiaoYanFailed
			if (IsJiOuJiaoYanFailed) {
				//JiOuJiaoYanFailed
				//Debug.Log("JOJYSB...");
			}
			else {
				//JiaMiXinPianJiaoYanFailed
				//Debug.Log("JMXPJYSB...");
				IsJiaMiJiaoYanFailed = true;
			}
		}
	}
	public static bool IsJiaMiJiaoYanFailed;
	
	enum JIAOYANENUM
	{
		NULL,
		SUCCEED,
		FAILED,
	}
	static JIAOYANENUM JiaoYanState = JIAOYANENUM.NULL;
	static byte JiaoYanFailedMax = 0x03;
	static byte JiaoYanSucceedCount;
	static byte JiaoYanFailedCount;
	static byte[] JiaoYanDt = new byte[4];
	static byte[] JiaoYanMiMa = new byte[4];
	static byte[] JiaoYanMiMaRand = new byte[4];
	
	//#define First_pin			 	0xe5
	//#define Second_pin		 	0x5d
	//#define Third_pin		 		0x8c
	void InitJiaoYanMiMa()
	{
		JiaoYanMiMa[1] = 0xe5; //0xff;
		JiaoYanMiMa[2] = 0x5d; //0xff;
		JiaoYanMiMa[3] = 0x8c; //0xff;
		JiaoYanMiMa[0] = 0x00;
		for (int i = 1; i < 4; i++) {
			JiaoYanMiMa[0] ^= JiaoYanMiMa[i];
		}
	}

	void RandomJiaoYanMiMaVal()
	{
		for (int i = 0; i < 4; i++) {
			JiaoYanMiMaRand[i] = (byte)UnityEngine.Random.Range(0x00, (JiaoYanMiMa[i] - 1));
		}

		byte TmpVal = 0x00;
		for (int i = 1; i < 4; i++) {
			TmpVal ^= JiaoYanMiMaRand[i];
		}

		if (TmpVal == JiaoYanMiMaRand[0]) {
			JiaoYanMiMaRand[0] = JiaoYanMiMaRand[0] == 0x00 ?
				(byte)UnityEngine.Random.Range(0x01, 0xff) : (byte)(JiaoYanMiMaRand[0] + UnityEngine.Random.Range(0x01, 0xff));
		}
	}

	public static bool IsJiaoYanHid;
	private bool isShortDoudong = false;
	public void OpenFangXiangPanZhenDong(int index, bool isChixu)
	{
		if (zhendongDengjiFXP == 0)
		{
			zhendongQiangdu = 0x00;
			zhendongShijian = 0x00;
		}
		else
		{
			liFankuiQiangdu = (byte)zhendongQiangduTimeArr [index, 0];
			zhendongQiangdu = (byte)zhendongQiangduTimeArr [index, 1];
			zhendongShijian = (byte)zhendongQiangduTimeArr [index, 2];

			liFankuiQiangduUse = liFankuiQiangdu;

			if (liFankuiQiangduUse <= 0x01)
			{
				liFankuiQiangduUse = 0x01;
			}
		}

		if (IsPlayFangXiangPanZhenDong) {
			return;
		}

		IsPlayFangXiangPanZhenDong = true;
		IsZhenDongFangXiangPan = true;
		//StartCoroutine(PlayFangXiangPanZhenDong());

		if (!isChixu)
		{
			isShortDoudong = !isChixu;

			CancelInvoke ("CloseFangXiangPanZhenDongShort");
			Invoke ("CloseFangXiangPanZhenDongShort", 0.3f);
		}
	}
	
	void CloseFangXiangPanZhenDongShort()
	{
		IsPlayFangXiangPanZhenDong = false;
		IsZhenDongFangXiangPan = false;
		isShortDoudong = false;
	}

	public void CloseFangXiangPanZhenDong()
	{
		if (isShortDoudong)
		{
			return;
		}

		IsPlayFangXiangPanZhenDong = false;
		IsZhenDongFangXiangPan = false;
	}

/*public static bool IsSlowLoopCom = false;
	IEnumerator PlayFangXiangPanZhenDong()
	{
		int count = UnityEngine.Random.Range(1, 4);
		count = 8; //test
		do {
			IsZhenDongFangXiangPan = !IsZhenDongFangXiangPan;
			count--;
			yield return new WaitForSeconds(0.05f);
		} while (count > -1);
		IsZhenDongFangXiangPan = false;
		IsPlayFangXiangPanZhenDong = false;
	}*/
	
	static int []buffer25;
	static int []buffer26;
	static float []bufferTime25;
	static float []bufferTime26;
	static float []bufferLiangTime25;
	static float []bufferLiangTime26;
	private float shanTimeT = 0.1f;
	private float guanTimeT = 0.15f;
	public void ControlShanguangdeng(int index, bool isJudege)
	{//Debug.Log ("ControlShanguangdengControlShanguangdengControlShanguangdeng  " + index);
		//0 - 5
		for (int i = 0; i< 8; i++)
		{
			buffer25[i] = 0;
			buffer26[i] = 0;
		}

		if (index >= 100)
		{
			return;
		}

		if (bufferTime25[0] <= 0)
		{
			for (int i = 0; i< 8; i++)
			{
				bufferTime25[i] = Time.time;
				bufferTime26[i] = Time.time;
				bufferLiangTime25[i] = Time.time;
				bufferLiangTime26[i] = Time.time;
			}
		}

		if (index == 1)
		{//
			buffer25[0] = 1;
			bufferTime25[0] = Time.time;

			if (bufferLiangTime25[0] <= 0)
			{
				bufferLiangTime25[0] = Time.time;
			}

			if (Time.time - bufferLiangTime25[0] > shanTimeT)
			{
				buffer25[0] = 0;

				if (Time.time - bufferLiangTime25[0] > guanTimeT)
				{
					bufferLiangTime25[0] = 0;
				}
			}
		}
		else if (index == 2)
		{//
			buffer25[1] = 1;
			bufferTime25[1] = Time.time;
			
			if (bufferLiangTime25[1] <= 0)
			{
				bufferLiangTime25[1] = Time.time;
			}
			
			if (Time.time - bufferLiangTime25[1] > shanTimeT)
			{
				buffer25[1] = 0;
				
				if (Time.time - bufferLiangTime25[1] > guanTimeT)
				{
					bufferLiangTime25[1] = 0;
				}
			}
		}
		else if (index == 3)
		{//
			buffer25[2] = 1;
			bufferTime25[2] = Time.time;
			
			if (bufferLiangTime25[2] <= 0)
			{
				bufferLiangTime25[2] = Time.time;
			}
			
			if (Time.time - bufferLiangTime25[2] > shanTimeT)
			{
				buffer25[2] = 0;
				
				if (Time.time - bufferLiangTime25[2] > guanTimeT)
				{
					bufferLiangTime25[2] = 0;
				}
			}
		}
		else if (index == 4)
		{//
			buffer25[3] = 1;
			bufferTime25[3] = Time.time;
			
			if (bufferLiangTime25[3] <= 0)
			{
				bufferLiangTime25[3] = Time.time;
			}
			
			if (Time.time - bufferLiangTime25[3] > shanTimeT)
			{
				buffer25[3] = 0;
				
				if (Time.time - bufferLiangTime25[3] > guanTimeT)
				{
					bufferLiangTime25[3] = 0;
				}
			}
		}
		else if (index == 5)
		{//
			buffer25[6] = 1;
			buffer26[0] = 1;
			bufferTime25[6] = Time.time;
			bufferTime26[0] = Time.time;
			
			if (bufferLiangTime25[6] <= 0)
			{
				bufferLiangTime25[6] = Time.time;
			}
			
			if (Time.time - bufferLiangTime25[6] > shanTimeT)
			{
				buffer25[6] = 0;
				buffer26[0] = 0;
				
				if (Time.time - bufferLiangTime25[6] > guanTimeT)
				{
					bufferLiangTime25[6] = 0;
				}
			}
		}
		else if (index == 6)
		{//
			buffer25[7] = 1;
			buffer26[1] = 1;
			bufferTime25[7] = Time.time;
			bufferTime26[1] = Time.time;
			
			if (bufferLiangTime25[7] <= 0)
			{
				bufferLiangTime25[7] = Time.time;
			}
			
			if (Time.time - bufferLiangTime25[7] > shanTimeT)
			{
				buffer25[7] = 0;
				buffer26[1] = 0;
				
				if (Time.time - bufferLiangTime25[7] > guanTimeT)
				{
					bufferLiangTime25[7] = 0;
				}
			}
		}
		else if (index == 7)
		{//
			buffer25[4] = 1;
			buffer26[2] = 1;
			bufferTime25[4] = Time.time;
			bufferTime26[2] = Time.time;
			
			if (bufferLiangTime25[4] <= 0)
			{
				bufferLiangTime25[4] = Time.time;
			}
			
			if (Time.time - bufferLiangTime25[4] > shanTimeT)
			{
				buffer25[4] = 0;
				buffer26[2] = 0;
				
				if (Time.time - bufferLiangTime25[4] > guanTimeT)
				{
					bufferLiangTime25[4] = 0;
				}
			}
		}

		if (!isJudege)
		{
			return;
		}

		for (int i = 0; i< 4; i++)
		{
			if (Time.time - bufferTime25[i] >= 3.0f && i != 5)
			{
				buffer25[i] = 1;
				//bufferTime25[i] = Time.time;
				
				if (bufferLiangTime25[i] <= 0)
				{
					bufferLiangTime25[i] = Time.time;
				}
				
				if (Time.time - bufferLiangTime25[i] > shanTimeT)
				{
					buffer25[i] = 0;
					bufferTime25[i] = Time.time;
					
					if (Time.time - bufferLiangTime25[i] > guanTimeT)
					{
						bufferLiangTime25[i] = 0;
					}
				}
			}
		}

		if (Time.time - bufferTime25[7] >= 2.3f)
		{
			buffer25[7] = 1;
			buffer26[1] = 1;
			//bufferTime25[7] = Time.time;
			//bufferTime26[1] = Time.time;
			
			if (bufferLiangTime25[7] <= 0)
			{
				bufferLiangTime25[7] = Time.time;
			}
			
			if (Time.time - bufferLiangTime25[7] > shanTimeT)
			{
				buffer25[7] = 0;
				buffer26[1] = 0;
				bufferTime25[7] = Time.time;
				bufferTime26[1] = Time.time;
				
				if (Time.time - bufferLiangTime25[7] > guanTimeT)
				{
					bufferLiangTime25[7] = 0;
				}
			}
		}
		
		if (Time.time - bufferTime25[6] >= 2.5f)
		{
			buffer25[6] = 1;
			buffer26[0] = 1;
			//bufferTime25[6] = Time.time;
			//bufferTime26[0] = Time.time;
			
			if (bufferLiangTime25[6] <= 0)
			{
				bufferLiangTime25[6] = Time.time;
			}
			
			if (Time.time - bufferLiangTime25[6] > shanTimeT)
			{
				buffer25[6] = 0;
				buffer26[0] = 0;
				bufferTime25[6] = Time.time;
				bufferTime26[0] = Time.time;
				
				if (Time.time - bufferLiangTime25[6] > guanTimeT)
				{
					bufferLiangTime25[6] = 0;
				}
			}
		}
		
		if (Time.time - bufferTime25[4] >= 2.8f)
		{
			buffer25[4] = 1;
			buffer26[2] = 1;
			//bufferTime25[4] = Time.time;
			//bufferTime26[2] = Time.time;
			
			if (bufferLiangTime25[4] <= 0)
			{
				bufferLiangTime25[4] = Time.time;
			}
			
			if (Time.time - bufferLiangTime25[4] > shanTimeT)
			{
				buffer25[4] = 0;
				buffer26[2] = 0;
				bufferTime25[4] = Time.time;
				bufferTime26[2] = Time.time;
				
				if (Time.time - bufferLiangTime25[4] > guanTimeT)
				{
					bufferLiangTime25[4] = 0;
				}
			}
		}
	}

	public void ControlShanguangdengDH(int index, bool isJudege, int stateT)
	{//Debug.Log ("ControlShangua1ngdengDHControlShan1guangdengDHControlSha1  " + index + " " + stateT + " " + Time.time);
		//0 - 5
		if (index != 22)
		{
			for (int i = 0; i< 8; i++)
			{
				buffer25[i] = 0;
				buffer26[i] = 0;
			}
		}
		else
		{
			for (int i = 0; i< 8; i++)
			{
				if (i != 4)
				buffer25[i] = 0;
				if (i != 2)
				buffer26[i] = 0;
			}
		}

		if (index <= 0)
		{
			return;
		}
		
		if (index < 5)
		{//
			buffer25[index - 1] = 1;
		}
		else if (index < 9)
		{//
			buffer25[8 - index] = 1;
		}
		else if (index < 11)
		{//
			buffer25[0] = 1;
			buffer25[1] = 1;
			buffer25[2] = 1;
			buffer25[3] = 1;
		}
		else if (index == 11)
		{//
			buffer25[0] = 1;
			buffer25[3] = 1;
		}
		else if (index == 12)
		{//
			buffer26[1] = 1;
			buffer25[7] = 1;
		}
		else if (index == 13)
		{//
			buffer26[0] = 1;
			buffer25[6] = 1;
		}
		else if (index == 14)
		{//
			buffer26[2] = 1;
			buffer25[4] = 1;
		}
		else if (index == 15 || index == 16)
		{//
			buffer25[0] = 1;
			buffer25[2] = 1;
			buffer25[6] = 1;
			buffer26[1] = 1;
		}
		else if (index == 17)
		{//
			buffer25[1] = 1;
			buffer25[3] = 1;
			buffer25[7] = 1;
			buffer26[0] = 1;
		}
		else if (index == 18)
		{//
			buffer26[2] = 1;
			buffer25[4] = 1;
		}
		else if (index == 19)
		{//
			buffer26[0] = 1;
			buffer25[6] = 1;
		}
		else if (index == 20)
		{//
			buffer26[1] = 1;
			buffer25[7] = 1;
		}
		else if (index == 21)
		{//
			buffer25[0] = 1;
			buffer25[1] = 1;
			buffer25[2] = 1;
			buffer25[3] = 1;
		}
		else if (index == 22)
		{//
			switch(stateT)
			{
			case 0:
			case 8:
				buffer25[0] = 1;
				break;
			case 1:
			case 9:
				buffer25[1] = 1;
				break;
			case 2:
			case 10:
				buffer25[2] = 1;
				break;
			case 3:
			case 11:
				buffer25[3] = 1;
				break;
			case 4:
			case 12:
				buffer25[7] = 1;	//
				break;
			case 5:
			case 13:
				buffer25[6] = 1;
				break;
			case 6:
			case 14:
				buffer26[0] = 1;
				break;
			case 7:
			case 15:
				buffer26[1] = 1;
				break;
			}
		}
		else if (index < 27)
		{//
			for (int i = 0; i< 8; i++)
			{
				buffer25[i] = 1;
				buffer26[i] = 1;
			}
		}
		else if (index == 27)
		{//
			for (int i = 0; i< 8; i++)
			{
				buffer25[i] = 1;
				buffer26[i] = 1;
			}
		}
	}

	public void ControlShanguangdengDHSpecial(int valueT)
	{//Debug.Log ("ControlShanguangdengDHSpecialControlShanguangdengDHSpecial  " + valueT);
		buffer25[4] = valueT;
		buffer26[2] = valueT;
	}

	byte JiOuJiaoYanCount;
	byte JiOuJiaoYanMax = 5;
	public static bool IsJiOuJiaoYanFailed;
	byte EndRead_1 = 0x41;
	byte EndRead_2 = 0x42;
	static float TimeJiOuVal;
	public void GetMessage()
	{
		if (!MyCOMDevice.ComThreadClass.IsReadComMsg) {
			return;
		}

		if (MyCOMDevice.ComThreadClass.IsReadMsgComTimeOut) {
			return;
		}

		if (MyCOMDevice.ComThreadClass.ReadByteMsg.Length < (MyCOMDevice.ComThreadClass.BufLenRead - MyCOMDevice.ComThreadClass.BufLenReadEnd)) {
			Debug.Log("ReadBufLen was wrong! len is "+MyCOMDevice.ComThreadClass.ReadByteMsg.Length);
			return;
		}

		if (IsJiOuJiaoYanFailed) {
			if (Time.time - TimeJiOuVal < 5f) {
				return;
			}
			IsJiOuJiaoYanFailed = false;
			JiOuJiaoYanCount = 0;
			//return;
		}

		if ((MyCOMDevice.ComThreadClass.ReadByteMsg[22]&0x01) == 0x01) {
			JiOuJiaoYanCount++;
			if (JiOuJiaoYanCount >= JiOuJiaoYanMax && !IsJiOuJiaoYanFailed) {
				IsJiOuJiaoYanFailed = true;
				TimeJiOuVal = Time.time;
				//JiOuJiaoYanFailed
			}
		}
		//IsJiOuJiaoYanFailed = true; //test

		byte tmpVal = 0x00;
		string testA = "";
		for (int i = 2; i < (MyCOMDevice.ComThreadClass.BufLenRead - 4 - 6); i++) {
			if (i == 8 || i == 21) {
				continue;
			}
			testA += MyCOMDevice.ComThreadClass.ReadByteMsg[i].ToString("X2") + " ";
			tmpVal ^= MyCOMDevice.ComThreadClass.ReadByteMsg[i];
		}
//		tmpVal ^= EndRead_1;
//		tmpVal ^= EndRead_2;
		testA += EndRead_1 + " ";
		testA += EndRead_2 + " ";

		if (tmpVal != MyCOMDevice.ComThreadClass.ReadByteMsg[21]) {
			//if (MyCOMDevice.ComThreadClass.IsStopComTX) {
			//	return;
			//}
			//MyCOMDevice.ComThreadClass.IsStopComTX = true;
			return;
		}

		if (IsJiaoYanHid) {
			tmpVal = 0x00;

			for (int i = 11; i < 14; i++) {
				tmpVal ^= MyCOMDevice.ComThreadClass.ReadByteMsg[i];
			}

			if (tmpVal == MyCOMDevice.ComThreadClass.ReadByteMsg[10]) {
				bool isJiaoYanDtSucceed = false;
				tmpVal = 0x00;
				for (int i = 15; i < 18; i++) {
					tmpVal ^= MyCOMDevice.ComThreadClass.ReadByteMsg[i];
				}
				
				//校验2...
				if ( tmpVal == MyCOMDevice.ComThreadClass.ReadByteMsg[14]
				    && (JiaoYanDt[1]&0xef) == MyCOMDevice.ComThreadClass.ReadByteMsg[15]
				    && (JiaoYanDt[2]&0xfe) == MyCOMDevice.ComThreadClass.ReadByteMsg[16]
				    && (JiaoYanDt[3]|0x28) == MyCOMDevice.ComThreadClass.ReadByteMsg[17] ) {
					isJiaoYanDtSucceed = true;
				}

				if (isJiaoYanDtSucceed) {
					//JiaMiJiaoYanSucceed
					OnEndJiaoYanIO(JIAOYANENUM.SUCCEED);
				}
			}
		}

		int len = MyCOMDevice.ComThreadClass.ReadByteMsg.Length;
		uint[] readMsg = new uint[len];
		for (int i = 0; i < len; i++) {
			readMsg[i] = MyCOMDevice.ComThreadClass.ReadByteMsg[i];
		}
		KeyProcess( readMsg );
	}

	void CheckBikeDirLen()
	{
		BikeDirLenA = SteerValMin - SteerValCen + 1;
		BikeDirLenB = SteerValCen - SteerValMax + 1;
		BikeDirLenC = SteerValMax - SteerValCen + 1;
		BikeDirLenD = SteerValCen - SteerValMin + 1;
	}

	static bool IsHandleDirByKey = true;
	public static void GetPcvrSteerVal()
	{
		if (!IsHandleDirByKey) {
			if (!bIsHardWare) {
				mGetSteer = Input.GetAxis("Horizontal");
				return;
			}
		}
		else {
			if (!bIsHardWare || IsTestGame) {
				mGetSteer = Input.GetAxis("Horizontal");
				return;
			}
		}

		if (!MyCOMDevice.IsFindDeviceDt) {
			return;
		}

		if (IsInitFangXiangJiaoZhun) {
			return;
		}

		uint bikeDir = SteerValCur;
		uint bikeDirLen = SteerValMax - SteerValMin + 1;
		if (SteerValMax < SteerValMin) {
			bikeDirLen = bikeDir > SteerValCen ? BikeDirLenA : BikeDirLenB;
			bikeDir = Math.Min(bikeDir, SteerValMin);
			bikeDir = Math.Max(bikeDir, SteerValMax);
		}
		else {
			bikeDirLen = bikeDir > SteerValCen ? BikeDirLenC : BikeDirLenD;
			bikeDir = Math.Max(bikeDir, SteerValMin);
			bikeDir = Math.Min(bikeDir, SteerValMax);
		}
		bikeDirLen = Math.Max(1, bikeDirLen);
		
		uint bikeDirCur = SteerValMax - bikeDir;
		float bikeDirPer = (float)bikeDirCur / bikeDirLen;
		if (SteerValMax > SteerValMin) {
			//ZhengJie FangXiangDianWeiQi
			if (bikeDir > SteerValCen) {
				bikeDirCur = bikeDir - SteerValCen;
				bikeDirPer = (float)bikeDirCur / bikeDirLen;
			}
			else {
				bikeDirCur = SteerValCen - bikeDir;
				bikeDirPer = - (float)bikeDirCur / bikeDirLen;
			}
		}
		else {
			//FanJie DianWeiQi
			if(bikeDir > SteerValCen) {
				bikeDirCur = bikeDir - SteerValCen;
				bikeDirPer = - (float)bikeDirCur / bikeDirLen;
			}
			else {
				bikeDirCur = SteerValCen - bikeDir;
				bikeDirPer = (float)bikeDirCur / bikeDirLen;
			}
		}
		mGetSteer = bikeDirPer;

		if (mGetSteer >= 0.95f)
		{
			mGetSteer = 0.95f;
		}
		else if (mGetSteer <= -0.95f)
		{
			mGetSteer = -0.95f;
		}
		//Debug.Log (mGetSteer + " "+ SteerValCur + " " +  Time.time);
	}

	public static void GetPcvrPowerVal()
	{
		if (!bIsHardWare) {
			mGetPower = Input.GetAxis("Vertical");
			return;
		}

		if (!MyCOMDevice.IsFindDeviceDt) {
			return;
		}

		if (IsInitYouMenJiaoZhun) {
			return;
		}

		uint bikePowerCurValTmp = 0;
		if (mBikePowerMin > mBikePowerMax) {
			bikePowerCurValTmp = Math.Min(BikePowerCur, mBikePowerMin);
			bikePowerCurValTmp = Math.Max(bikePowerCurValTmp, mBikePowerMax);
		}
		else {
			bikePowerCurValTmp = Math.Max(BikePowerCur, mBikePowerMin);
			bikePowerCurValTmp = Math.Min(bikePowerCurValTmp, mBikePowerMax);
		}
		
		uint bikePowerDis = mBikePowerMin > mBikePowerMax ? (mBikePowerMin - bikePowerCurValTmp) : (bikePowerCurValTmp - mBikePowerMin);
		float valThrottleTmp = (float)bikePowerDis / BikePowerLen;
		valThrottleTmp = valThrottleTmp <= 0.3f ? 0f : valThrottleTmp;
		valThrottleTmp = valThrottleTmp > 1f ? 1f : valThrottleTmp;
		mGetPower = valThrottleTmp;
		
		if (IsTestGame) {
			mGetPower = 1f; //test
		}
	}

	public static void GetPcvrShaCheVal()
	{
		if (!bIsHardWare) {
			return;
		}
		
		if (!MyCOMDevice.IsFindDeviceDt) {
			return;
		}
		
		if (IsInitShaCheJiaoZhun) {
			return;
		}
		
		uint bikeShaCheCurValTmp = 0;
		if (mBikeShaCheMin > mBikeShaCheMax) {
			bikeShaCheCurValTmp = Math.Min(BikeShaCheCur, mBikeShaCheMin);
			bikeShaCheCurValTmp = Math.Max(bikeShaCheCurValTmp, mBikeShaCheMax);
		}
		else {
			bikeShaCheCurValTmp = Math.Max(BikeShaCheCur, mBikeShaCheMin);
			bikeShaCheCurValTmp = Math.Min(bikeShaCheCurValTmp, mBikeShaCheMax);
		}
		
		uint bikeShaCheDis = mBikeShaCheMin > mBikeShaCheMax ? (mBikeShaCheMin - bikeShaCheCurValTmp) : (bikeShaCheCurValTmp - mBikeShaCheMin);
		float valTmp = (float)bikeShaCheDis / BikeShaCheLen;
		valTmp = valTmp <= 0.3f ? 0f : 1f;
		shacheValue = valTmp;
		if (IsTestGame) {
			return; //test
		}

		if (!IsActiveShaCheEvent && valTmp > 0.3f) {
			IsActiveShaCheEvent = true;
			ShaCheBtLight = StartLightState.Liang;
			InputEventCtrl.GetInstance().ClickShaCheBt( ButtonState.DOWN );
		}
		else if (IsActiveShaCheEvent && valTmp < 0.3f){
			IsActiveShaCheEvent = false;
			ShaCheBtLight = StartLightState.Mie;
			InputEventCtrl.GetInstance().ClickShaCheBt( ButtonState.UP );
		}
	}

	public void SubPlayerCoin(int subNum)
	{
		//Debug.Log("SubPlayerCoin ---- num "+subNum);
		if (gOldCoinNum >= subNum) {
			gOldCoinNum = (uint)(gOldCoinNum - subNum);
		}
		else {
			SubCoinNum = (int)(subNum - gOldCoinNum);
			if (mOldCoinNum < SubCoinNum) {
				return;
			}
			//Debug.Log("mOldCoinNum "+mOldCoinNum+", SubCoinNum "+SubCoinNum);
			mOldCoinNum -= (uint)SubCoinNum;
			coinCurNumPCVR = (int)mOldCoinNum;
			gOldCoinNum = 0;
		}
	}
	
	public void InitYouMenJiaoZhun()
	{
		if (IsInitYouMenJiaoZhun) {
			return;
		}
		//ScreenLog.Log("pcvr -> InitYouMenJiaoZhun...");
		mBikePowerMin = 999999;
		mBikePowerMax = 0;
		
		IsJiaoZhunFireBt = false;
		IsInitYouMenJiaoZhun = true;
	}
	
	void ResetYouMenJiaoZhun()
	{
		if (!IsInitYouMenJiaoZhun) {
			return;
		}

		//ScreenLog.Log("pcvr -> ResetYouMenJiaoZhun...");
		IsJiaoZhunFireBt = false;
		IsInitYouMenJiaoZhun = false;
		//bIsJiaoYanBikeValue = false;
		
		uint TmpVal = 0;
		if (IsFanZhuangYouMen) {
			TmpVal = mBikePowerMax;
			mBikePowerMax = mBikePowerMin;
			mBikePowerMin = TmpVal;
			BikePowerLen = mBikePowerMin - mBikePowerMax + 1;
		}
		else {
			BikePowerLen = mBikePowerMax - mBikePowerMin + 1;
		}
		BikePowerLen = Math.Max(1, BikePowerLen);

		//PlayerPrefs.SetInt("mBikePowerMax", (int)mBikePowerMax);
		//PlayerPrefs.SetInt("mBikePowerMin", (int)mBikePowerMin);

		ReadGameInfo.GetInstance ().WriteBikePowerMax (mBikePowerMax.ToString());
		ReadGameInfo.GetInstance ().WriteBikePowerMin (mBikePowerMin.ToString());
	}

	public void InitShaCheJiaoZhun()
	{
		if (IsInitShaCheJiaoZhun) {
			return;
		}
		mBikeShaCheMin = 999999;
		mBikeShaCheMax = 0;
		IsJiaoZhunFireBt = false;
		IsInitShaCheJiaoZhun = true;
	}

	void ResetShaCheJiaoZhun()
	{
		if (!IsInitShaCheJiaoZhun) {
			return;
		}
		IsJiaoZhunFireBt = false;
		IsInitShaCheJiaoZhun = false;
		bIsJiaoYanBikeValue = false;
		
		uint TmpVal = 0;
		if (IsFanZhuangShaChe) {
			TmpVal = mBikeShaCheMax;
			mBikeShaCheMax = mBikeShaCheMin;
			mBikeShaCheMin = TmpVal;
			BikeShaCheLen = mBikeShaCheMin - mBikeShaCheMax + 1;
		}
		else {
			BikeShaCheLen = mBikeShaCheMax - mBikeShaCheMin + 1;
		}
		BikeShaCheLen = Math.Max(1, BikeShaCheLen);

		//PlayerPrefs.SetInt("mBikeShaCheMax", (int)mBikeShaCheMax);
		//PlayerPrefs.SetInt("mBikeShaCheMin", (int)mBikeShaCheMin);
		
		ReadGameInfo.GetInstance ().WriteBikeShaCheMax (mBikeShaCheMax.ToString());
		ReadGameInfo.GetInstance ().WriteBikeShaCheMin (mBikeShaCheMin.ToString());
	}

	public void InitFangXiangJiaoZhun()
	{
		if (IsInitFangXiangJiaoZhun) {
			return;
		}
		//ScreenLog.Log("pcvr -> InitFangXiangJiaoZhun...");
		//FangXiangInfo
		/*SteerValMin = 999999;
		SteerValCen = 1765;
		SteerValMax = 0;*/
		
		IsJiaoZhunFireBt = false;
		FangXiangJiaoZhunCount = 0;
		//IsInitFangXiangJiaoZhun = true;
		bIsJiaoYanBikeValue = true;

		InitYouMenJiaoZhun();
		IsJiaoZhunFireBt = true;
	}
	
/*	void ResetFangXiangJiaoZhun()
	{
		if (!IsInitFangXiangJiaoZhun) {
			return;
		}
		//ScreenLog.Log("pcvr -> ResetFangXiangJiaoZhun...");
		IsJiaoZhunFireBt = false;
		FangXiangJiaoZhunCount = 0;
		IsInitFangXiangJiaoZhun = false;
		
		uint TmpVal = 0;
		if (IsFanZhuangFangXiang) {
			TmpVal = SteerValMax;
			SteerValMax = SteerValMin;
			SteerValMin = TmpVal;
			//ScreenLog.Log("CheTouFangXiangFanZhuan -> SteerValMin " + SteerValMin + ", SteerValMax " +SteerValMax);
		}
		else {
			//ScreenLog.Log("CheTouFangXiangZhengZhuan -> SteerValMin " + SteerValMin + ", SteerValMax " +SteerValMax);
		}
		CheckBikeDirLen();
		//PlayerPrefs.SetInt("mBikeDirMin", (int)SteerValMin);
		//PlayerPrefs.SetInt("mBikeDirCen", (int)SteerValCen);
		//PlayerPrefs.SetInt("mBikeDirMax", (int)SteerValMax);
	}
*/
	void ShaCheJiaoZhun()
	{
		if (!IsInitShaCheJiaoZhun) {
			return;
		}
		
		if (BikeShaCheCur < mBikeShaCheMin) {
			mBikeShaCheMin = BikeShaCheCur;
			//PlayerPrefs.SetInt("mBikeShaCheMin", (int)mBikeShaCheMin);

			ReadGameInfo.GetInstance ().WriteBikeShaCheMin (mBikeShaCheMin.ToString());
		}
		
		if (BikeShaCheCur > mBikeShaCheMax) {
			mBikeShaCheMax = BikeShaCheCur;
			//PlayerPrefs.SetInt("mBikeShaCheMax", (int)mBikeShaCheMax);
			
			ReadGameInfo.GetInstance ().WriteBikeShaCheMax (mBikeShaCheMax.ToString());
		}
		
		if (bPlayerStartKeyDown && !IsJiaoZhunFireBt) {
			IsJiaoZhunFireBt = true;
			uint dVal_0 = BikeShaCheCur - mBikeShaCheMin;
			uint dVal_1 = mBikeShaCheMax - BikeShaCheCur;
			if (dVal_0 > dVal_1) {
				IsFanZhuangShaChe = false;
			}
			else if (dVal_0 < dVal_1) {
				IsFanZhuangShaChe = true;
			}
			ResetShaCheJiaoZhun();
		}
		else if(!bPlayerStartKeyDown && IsJiaoZhunFireBt) {
			IsJiaoZhunFireBt = false;
		}
	}

	void YouMenJiaoZhun()
	{
		if (!IsInitYouMenJiaoZhun) {
			return;
		}

		if (BikePowerCur < mBikePowerMin) {
			mBikePowerMin = BikePowerCur;
			//PlayerPrefs.SetInt("mBikePowerMin", (int)mBikePowerMin);

			ReadGameInfo.GetInstance ().WriteBikePowerMin (mBikePowerMin.ToString());
		}
		
		if (BikePowerCur > mBikePowerMax) {
			mBikePowerMax = BikePowerCur;
			//PlayerPrefs.SetInt("mBikePowerMax", (int)mBikePowerMax);

			ReadGameInfo.GetInstance ().WriteBikePowerMax (mBikePowerMax.ToString());
		}
		
		if (bPlayerStartKeyDown && !IsJiaoZhunFireBt) {
			IsJiaoZhunFireBt = true;
			uint dVal_0 = BikePowerCur - mBikePowerMin;
			uint dVal_1 = mBikePowerMax - BikePowerCur;
			if (dVal_0 > dVal_1) {
				//YouMenZhengZhuang
				IsFanZhuangYouMen = false;
			}
			else if (dVal_0 < dVal_1) {
				//YouMenFanZhuang
				IsFanZhuangYouMen = true;
			}
			ResetYouMenJiaoZhun();
			InitShaCheJiaoZhun();
			IsJiaoZhunFireBt = true;
		}
		else if(!bPlayerStartKeyDown && IsJiaoZhunFireBt) {
			IsJiaoZhunFireBt = false;
		}
	}
	/*
	void FangXiangJiaoZhun()
	{
		if (!IsInitFangXiangJiaoZhun) {
			return;
		}
		
		//Record FangXiangInfo
		if (SteerValCur < SteerValMin) {
			SteerValMin = SteerValCur;
			//PlayerPrefs.SetInt("mBikeDirMin", (int)SteerValMin);
		}
		
		if (SteerValCur > SteerValMax) {
			SteerValMax = SteerValCur;
			//PlayerPrefs.SetInt("mBikeDirMax", (int)SteerValMax);
		}
		
		if (bPlayerStartKeyDown && !IsJiaoZhunFireBt) {
			IsJiaoZhunFireBt = true;
			FangXiangJiaoZhunCount++;
			switch (FangXiangJiaoZhunCount) {
			case 1:
				//CheTouZuoZhuan
				uint dVal_0 = SteerValCur - SteerValMin;
				uint dVal_1 = SteerValMax - SteerValCur;
				if (dVal_0 < dVal_1) {
					IsFanZhuangFangXiang = false;
				}
				else if (dVal_0 > dVal_1) {
					IsFanZhuangFangXiang = true;
				}
				break;
				
			case 2:
				//CheTouZhuanDaoZhongJian
				SteerValCen = SteerValCur;
				break;
				
			case 3:
				//CheTouYouZhuan
				ResetFangXiangJiaoZhun();
				InitYouMenJiaoZhun();
				IsJiaoZhunFireBt = true;
				break;
			}
		}
		else if(!bPlayerStartKeyDown && IsJiaoZhunFireBt) {
			IsJiaoZhunFireBt = false;
		}
	}
*/
	public static uint BikeBeiYongPowerCurPcvr;
	void KeyProcess(uint []buffer)
	{
		if (!MyCOMDevice.IsFindDeviceDt) {
			return;
		}

		if (buffer[0] != ReadHead_1 || buffer[1] != ReadHead_2) {
			return;
		}
		SteerValCur = ((buffer[6]&0x0f) << 8) + buffer[7]; //fangXiang

		bool isTest = false;
		if (!isTest) {
			BikePowerCur = ((buffer[2]&0x0f) << 8) + buffer[3]; //youMen
			BikePowerCurPcvr = BikePowerCur;
			
			BikeShaCheCur = ((buffer[4]&0x0f) << 8) + buffer[5]; //shaChe
			ShaCheCurPcvr = BikeShaCheCur;
		}
		else {
			BikePowerCur = SteerValCur; //test
			BikeShaCheCur = SteerValCur; //test
		}

		/*if (HardWareTest.IsTestHardWare) {
			uint tmpBYYouMen = ((buffer[29]&0x0f) << 8) + buffer[30]; //youMen-----change to AD
			BikeBeiYongPowerCurPcvr = tmpBYYouMen;
		}*/
		//game coinInfo
		CoinCurPcvr = buffer[8];
		if (CoinCurPcvr > 0 && !Network.isServer) {
			if (!IsCleanHidCoin) {
				IsCleanHidCoin = true;
				mOldCoinNum += CoinCurPcvr;
				coinCurNumPCVR = (int)mOldCoinNum;
				InputEventCtrl.GetInstance().ClickInsertcoinBt(ButtonState.DOWN);
			}
		}

		if (bIsJiaoYanBikeValue) {
			//FangXiangJiaoZhun();
			YouMenJiaoZhun();
			ShaCheJiaoZhun();
		}

		if(!Network.isServer)
		{
			if ( !IsCloseDongGanBtDown && 0x02 == (buffer[9]&0x02) ) {
				IsCloseDongGanBtDown = true;
				InputEventCtrl.GetInstance().ClickCloseDongGanBt( ButtonState.DOWN );
			}
			else if ( IsCloseDongGanBtDown && 0x00 == (buffer[9]&0x02) ) {
				IsCloseDongGanBtDown = false;
				InputEventCtrl.GetInstance().ClickCloseDongGanBt( ButtonState.UP );
			}

			if ( !bPlayerStartKeyDown && 0x01 == (buffer[9]&0x01) ) {
				bPlayerStartKeyDown = true;
				InputEventCtrl.GetInstance().ClickStartBtOne( ButtonState.DOWN );
			}
			else if ( bPlayerStartKeyDown && 0x00 == (buffer[9]&0x01) ) {
				bPlayerStartKeyDown = false;
				InputEventCtrl.GetInstance().ClickStartBtOne( ButtonState.UP );
			}
			
			if ( !IsClickLaBaBt && 0x04 == (buffer[9]&0x04) ) {
				IsClickLaBaBt = true;
				InputEventCtrl.GetInstance().ClickLaBaBt( ButtonState.DOWN );
			}
			else if( IsClickLaBaBt && 0x00 == (buffer[9]&0x04) ) {
				IsClickLaBaBt = false;
				InputEventCtrl.GetInstance().ClickLaBaBt( ButtonState.UP );
			}
			
			if ( !IsClickChangeCameraBt && 0x08 == (buffer[9]&0x08) ) {
				IsClickChangeCameraBt = true;
				InputEventCtrl.GetInstance().ClickChangeCameraBt( ButtonState.DOWN );
			}
			else if( IsClickChangeCameraBt && 0x00 == (buffer[9]&0x08) ) {
				IsClickChangeCameraBt = false;
				InputEventCtrl.GetInstance().ClickChangeCameraBt( ButtonState.UP );
			}
		}

		if ( !bSetEnterKeyDown && 0x10 == (buffer[9]&0x10) ) {
			bSetEnterKeyDown = true;
			InputEventCtrl.GetInstance().ClickSetEnterBt( ButtonState.DOWN );
		}
		else if ( bSetEnterKeyDown && 0x00 == (buffer[9]&0x10) ) {
			bSetEnterKeyDown = false;
			InputEventCtrl.GetInstance().ClickSetEnterBt( ButtonState.UP );
		}

		if ( !bSetMoveKeyDown && 0x20 == (buffer[9]&0x20) ) {
			bSetMoveKeyDown = true;
			InputEventCtrl.GetInstance().ClickSetMoveBt( ButtonState.DOWN );
		}
		else if( bSetMoveKeyDown && 0x00 == (buffer[9]&0x20) ) {
			bSetMoveKeyDown = false;
			InputEventCtrl.GetInstance().ClickSetMoveBt( ButtonState.UP );
		}
		//23-30
		SteerValMin = ((buffer[23]&0x0f) << 8) + buffer[24]; //left limit
		SteerValCen = ((buffer[25]&0x0f) << 8) + buffer[26]; //middle
		SteerValMax = ((buffer[27]&0x0f) << 8) + buffer[28]; //right limit

		if (SteerValMin < SteerValMax)
		{
			//SteerValMin = SteerValMin + (uint)((SteerValCen - SteerValMin) * 0.6f);
			//SteerValMax = SteerValMax - (uint)((SteerValMax - SteerValCen) * 0.6f);

			SteerValMin = SteerValMin + (uint)((SteerValCen - SteerValMin) * linghuoduPCVR * 0.1f);
			SteerValMax = SteerValMax - (uint)((SteerValMax - SteerValCen) * linghuoduPCVR * 0.1f);
		}
		else if (SteerValMin > SteerValMax)
		{
			//SteerValMin = SteerValMin - (uint)((SteerValMin - SteerValCen) * 0.6f);
			//SteerValMax = SteerValMax + (uint)((SteerValCen - SteerValMax) * 0.6f);

			SteerValMin = SteerValMin - (uint)((SteerValMin - SteerValCen) * linghuoduPCVR * 0.1f);
			SteerValMax = SteerValMax + (uint)((SteerValCen - SteerValMax) * linghuoduPCVR * 0.1f);
		}

		CheckBikeDirLen();
	}
	
	//public GUISkin GUISkin;
	void OnGUI1()
	{//Time.timeScale = 0.5f;
		
		//GUI.skin = GUISkin;
		
		if (pcvr.buffer25[0] == 1)
			GUI.Label(new Rect(300, 150, 50, 50), "0");
		if (pcvr.buffer25[1] == 1)
			GUI.Label(new Rect(350, 150, 50, 50), "0");
		if (pcvr.buffer25[2] == 1)
			GUI.Label(new Rect(400, 150, 50, 50), "0");
		if (pcvr.buffer25[3] == 1)
			GUI.Label(new Rect(450, 150, 50, 50), "0");


		if (pcvr.buffer26[1] == 1)
			GUI.Label(new Rect(300, 200, 50, 50), "0");
		if (pcvr.buffer25[7] == 1)
			GUI.Label(new Rect(450, 200, 50, 50), "0");
		
		
		if (pcvr.buffer26[0] == 1)
			GUI.Label(new Rect(300, 250, 50, 50), "0");
		if (pcvr.buffer25[6] == 1)
			GUI.Label(new Rect(450, 250, 50, 50), "0");
		
		
		if (pcvr.buffer26[2] == 1)
			GUI.Label(new Rect(300, 300, 50, 50), "0");
		if (pcvr.buffer25[4] == 1)
			GUI.Label(new Rect(450, 300, 50, 50), "0");
	}
}

public enum StartLightState
{
	Liang,
	Shan,
	Mie
}