using UnityEngine;
using System.Collections;

public class scoreControl : MonoBehaviour {

	public GameObject[] ScoreValueObj;
	public GameObject[] ScoreBackObj;		//0-biaoti 1-4 score back
	public TweenPosition[] ScoreBackTPSobj;			//0-biaoti, 1-4 score back
	public TweenPosition ScorePaerntTPSobj = null;	//the root
	public TweenPosition LishijiluTPSobj = null;

	public GameObject lishiJiluObj = null;
	public GameObject chuangjiluObj = null;

	public Transform[] pointsBackArr;	//biaoti, back1-4, lishijilu, score-parent pos
	public Transform[] pointsEndBackArr;	//biaoti, back1-4, lishijilu

	private int selfIndex = -1;
	private bool isChuangjilu = false;
	private bool isEndLe = false;
	private bool isWancheng = false;
	private float useTotalTime = 0;

	public int pressButtonState = -1;		//1 - chengjibiao; 2 - jinducao; 3 - xuanze moshi; 4- jixuyouxi

	//finished - time end
	public GameObject finishedObj = null;
	public GameObject timeendObj = null;
	public TweenPosition finishedTPSobj = null;
	public TweenPosition timeendTPSobj = null;

	//pingjia
	public GameObject pingjiaParentObj = null;
	public GameObject pingjiaResultObj = null;
	public UISprite pingjiaSSobj = null;
	public GameObject jinducaoParentObj = null;
	public scoreJindutiao jinducaoSObj = null;
	public GameObject pingfenChenggongObj = null;
	public GameObject pingfenXitongParent = null;
	public TweenPosition pingfenxitongTWPos = null;

	//dao ji shi
	public GameObject daojishiObj = null;
	public UISprite daojishiSprite = null;

	//moshi daojishi
	public GameObject daojishiObjMoshi = null;
	
	float downTimeIntNum = 15.0f;
	float downTimeIntNumStatic = 15.0f;

	public GameObject moshixuanzeObj = null;
	public GameObject jixuchuagnguanObj = null;
	public GameObject chongxuanMoshiObj = null;
	public GameObject insertCoinTishiLeft = null;
	//public GameObject insertCoinTishiRight = null;
	private bool moveLeft = true;
	public int moveStateHere = 0;
	private bool isToubiJieduan = false;

	//insert coin time down
	public UISprite daojishiClient = null;
	private float insertTimeCur = 0.0f;
	private float insertTimeCurStatic = 15.0f;
	private float insertTimeIntCur = 0.0f;

	public GameObject gameOverObj = null;
	public GameObject startButtonObj = null;
	
	// Use this for initialization
	void Start () {//Debug.Log ("scorestartttttttttttttttttttttttt");
		selfIndex = -1;
		isEndLe = false;
		isChuangjilu = false;
		isWancheng = false;
		useTotalTime = 0;
		pressButtonState = -1;
		moveLeft = false;
		moveStateHere = 0;
		isToubiJieduan = false;

		pcvr.scoreCtrlSobj = GetComponent<scoreControl>();

		//score value
		for (int i=0; i <ScoreValueObj.Length; i++)
		{
			ScoreValueObj[i].SetActive(false);
		}

		//biaoti and score back
		for (int i=0; i < ScoreBackTPSobj.Length; i++)
		{
			ScoreBackTPSobj[i].enabled = false;
		}

		for (int i=0; i <ScoreBackObj.Length; i++)
		{
			ScoreBackObj[i].SetActive(false);
			ScoreBackObj[i].transform.localPosition = pointsBackArr[i].localPosition;
		}

		//score parent
		ScorePaerntTPSobj.enabled = false;

		LishijiluTPSobj.enabled = false;

		lishiJiluObj.SetActive (false);
		chuangjiluObj.SetActive (false);

		lishiJiluObj.transform.localPosition = pointsBackArr [5].localPosition;

		finishedObj.SetActive(false);
		timeendObj.SetActive(false);

		pingjiaParentObj.SetActive (false);
		pingjiaResultObj.SetActive (false);
		jinducaoParentObj.SetActive (false);
		pingfenChenggongObj.SetActive (false);

		daojishiObj.SetActive (false);
		daojishiObjMoshi.SetActive (false);
		
		jixuchuagnguanObj.SetActive (true);
		chongxuanMoshiObj.SetActive (false);
		
		insertCoinTishiLeft.SetActive (false);
		//insertCoinTishiRight.SetActive (false);
		
		selectTimeObj1.SetActive (true);
		selectTimeObj2.SetActive (true);
		selectTimeObj3.SetActive (false);

		pingfenxitongTWPos.enabled = false;

		gameOverObj.SetActive (false);
	}

	void Update () {
		
		if (Network.isClient)
		{
			return;
		}

		if (isToubiJieduan)
		{
			insertTimeCur -= Time.deltaTime;

			//Debug.Log("zhegerrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrr " + insertTimeIntCur + " " + Mathf.FloorToInt(insertTimeCur));
			if (insertTimeIntCur != Mathf.FloorToInt(insertTimeCur) && pcvr.sound2DScrObj && insertTimeIntCur > 0)
			{
				pcvr.sound2DScrObj.playAudioDaojishi10s();
			}
			
			insertTimeIntCur = Mathf.FloorToInt(insertTimeCur);
			
			daojishiClient.spriteName = insertTimeIntCur.ToString();

			if (pcvr.coinCurNumPCVR >= pcvr.startCoinNumPCVR)
			{
				selectMoshi(false, false);
				isToubiJieduan = false;
			}
			else if (insertTimeCur < 0)
			{
				selectMoshi(true, true);
				isToubiJieduan = false;
			}
			return;
		}
		
		if (pressButtonState != 3)
		{
			return;
		}

		if (!moshixuanzeObj.activeSelf)
		{
			return;
		}
		
		if (downTimeIntNum > 0)
		{
			downTimeIntNum -= Time.deltaTime;
		}
		else
		{
			selectMoshi(true, false);
		}
		
		showTimeDownSelect(downTimeIntNum);
		
		if (selectpanJiangeTime > 0)
		{
			selectpanJiangeTime -= Time.deltaTime;
		}
		else
		{
			selectpanJiangeTime = selectpanJiangeSTime;
			
			changeSelectFangxiangS(++ selectFangxiangIndex);
		}
		
		if (spaceTime <= 0)
		{
			//select truck
			if ((moveStateHere == 0 || moveStateHere == 2) && ((pcvr.bIsHardWare && pcvr.mGetSteer < -0.5f) || (!pcvr.bIsHardWare && Input.GetAxis("Horizontal") < -0.5f)))
			{//left
				//do some thing
				moveToLeft();
				moveStateHere = 1;
			}
			else if ((moveStateHere == 0 || moveStateHere == 1) && ((pcvr.bIsHardWare && pcvr.mGetSteer > 0.5f) || (!pcvr.bIsHardWare && Input.GetAxis("Horizontal") > 0.5f)))
			{//right
				//do some thing
				moveToRight();
				moveStateHere = 2;
			}
			else if ((pcvr.bIsHardWare && Mathf.Abs(pcvr.mGetSteer) <= 0.5f) || (!pcvr.bIsHardWare && Mathf.Abs(Input.GetAxis("Horizontal")) <= 0.5f))
			{
				moveStateHere = 0;
			}
		}
		else
		{
			if ((pcvr.bIsHardWare && Mathf.Abs(pcvr.mGetSteer) <= 0.5f) || (!pcvr.bIsHardWare && Mathf.Abs(Input.GetAxis("Horizontal")) <= 0.5f))
			{
				moveStateHere = 0;
			}
			spaceTime -= Time.deltaTime;
		}
	}
	
	void moveToLeft()
	{
		moveLeft = true;
		spaceTime = spaceTimeStatic;
		jixuchuagnguanObj.SetActive (false);
		chongxuanMoshiObj.SetActive (true);

		playaudioMoveLeftRight ();
		
		//insertCoinTishiLeft.SetActive (false);
		//insertCoinTishiRight.SetActive (false);
	}
	
	void moveToRight()
	{
		moveLeft = false;
		spaceTime = spaceTimeStatic;
		jixuchuagnguanObj.SetActive (true);
		chongxuanMoshiObj.SetActive (false);

		playaudioMoveLeftRight ();
		
		//insertCoinTishiLeft.SetActive (false);
		//insertCoinTishiRight.SetActive (false);
	}

	void playaudioMoveLeftRight()
	{
		if (pcvr.sound2DScrObj)
		{
			pcvr.sound2DScrObj.playAudioXuanguanYidong();
		}
	}

	public void beginShowScoreGui(int index, bool isChuangjiluT, float useTime, bool isWanchengT)
	{//Debug.Log ("beginShowScoreGuibeginShowScoreGuibeginShowScoreGui " + index);
		selfIndex = index;
		isChuangjilu = isChuangjiluT;
		isWancheng = isWanchengT;
		useTotalTime = useTime;

		if (pcvr.sound2DScrObj)
		{
			pcvr.sound2DScrObj.playAudioBeijing (false);
		}

		//showScoreBack();

		if (isWancheng)
		{
			if (pcvr.sound2DENDScrObj)
			{
				pcvr.sound2DENDScrObj.playAudioZhongdian(true);
			}

			finishedObj.SetActive(true);
		}
		else
		{
			timeendObj.SetActive(true);
		}
	}

	public void showScoreBack()
	{//Debug.Log ("showScoreBackshowScoreBackshowScoreBackshowScoreBack");
		if (pcvr.sound2DENDScrObj)
		{
			pcvr.sound2DENDScrObj.playAudioJifen(true);
		}

		//change the camera effect here ???????????????????????????????????
		pcvr.aFirstScriObj.addXiaoguo ();

		//back will move
		for (int i=0; i < ScoreBackTPSobj.Length; i++)
		{
			ScoreBackTPSobj[i].enabled = true;
		}
		
		for (int i=0; i <ScoreBackObj.Length; i++)
		{
			ScoreBackObj[i].SetActive(true);
		}

		pressButtonState = 1;
	}

	//after the fourth back move to the center, will show the score infromation
	public void backMoveEnd()
	{
		if (isEndLe)
		{
			return;
		}

		isEndLe = true;

		CancelInvoke ("showScoreValue1");
		CancelInvoke ("showScoreValue2");
		CancelInvoke ("showScoreValue3");
		CancelInvoke ("showScoreValue4");
		CancelInvoke ("ScoreGuiFlyout");
		CancelInvoke ("showLishijilu");

		Invoke ("showScoreValue1", 0.1f);
		Invoke ("showScoreValue2", 0.2f);
		Invoke ("showScoreValue3", 0.3f);
		Invoke ("showScoreValue4", 0.4f);
		
		Invoke ("showLishijilu", 0.45f);
		Invoke ("ScoreGuiFlyout", 3.4f);

		//Debug.Log ("fourth back move end " + Time.time);
	}

	//show the score and judge selfindex and whether show the "chuangjilu"
	void showScoreValue1()
	{
		ScoreValueObj[0].SetActive(true);

		if (selfIndex == 0 && isChuangjilu)
		{
			chuangjiluObj.SetActive(true);
		}
	}
	
	void showScoreValue2()
	{
		ScoreValueObj[1].SetActive(true);
		
		if (selfIndex == 1 && isChuangjilu)
		{
			chuangjiluObj.SetActive(true);
		}
	}
	
	void showScoreValue3()
	{
		ScoreValueObj[2].SetActive(true);
		
		if (selfIndex == 2 && isChuangjilu)
		{
			chuangjiluObj.SetActive(true);
		}
	}
	
	void showScoreValue4()
	{
		pressButtonState = 0;

		ScoreValueObj[3].SetActive(true);
		
		if (selfIndex == 3 && isChuangjilu)
		{
			chuangjiluObj.SetActive(true);
		}
	}

	//show lishijilu
	void showLishijilu()
	{//Debug.Log ("showwwwwwwwwwwwwwwwwwlishijilu  " + Time.time);
		LishijiluTPSobj.enabled = true;
		lishiJiluObj.SetActive (true);
	}
	
	void ScoreGuiFlyout()
	{
		//show score end here
		lishiJiluObj.transform.parent = transform;
		chuangjiluObj.transform.parent = transform;

		ScorePaerntTPSobj.enabled = true;
		//Debug.Log ("Score1GuiFlyout back move end " + Time.time);
	}
	
	public void ClientShowScore()
	{
		//stop back move and score show slowly, and show all the score immediately
		//the score back will stop move and set the end position
		for (int i=0; i < ScoreBackTPSobj.Length; i++)
		{
			ScoreBackTPSobj[i].enabled = false;
		}
		
		for (int i=0; i <ScoreBackObj.Length; i++)
		{
			ScoreBackObj[i].SetActive(true);
			ScoreBackObj[i].transform.localPosition = pointsEndBackArr[i].localPosition;
		}
		
		//the score value will show here
		//cancle invoe here
		for (int i=0; i <ScoreValueObj.Length; i++)
		{
			ScoreValueObj[i].SetActive(true);
		}
		
		//lishi jilu
		LishijiluTPSobj.enabled = false;
		lishiJiluObj.transform.localPosition = pointsEndBackArr [5].localPosition;
		lishiJiluObj.SetActive (true);

		pcvr.scoreCtrlSobj.enabled = false;
	}

	public void skipMoveBack()
	{
		if (pressButtonState != 1)
		{
			return;
		}

		pressButtonState = -1;

		//cancel all the event
		CancelInvoke ("showScoreValue1");
		CancelInvoke ("showScoreValue2");
		CancelInvoke ("showScoreValue3");
		CancelInvoke ("showScoreValue4");
		CancelInvoke ("ScoreGuiFlyout");
		CancelInvoke ("showLishijilu");

		//stop back move and score show slowly, and show all the score immediately
		//the score back will stop move and set the end position
		for (int i=0; i < ScoreBackTPSobj.Length; i++)
		{
			ScoreBackTPSobj[i].enabled = false;
		}
		
		for (int i=0; i <ScoreBackObj.Length; i++)
		{
			ScoreBackObj[i].transform.localPosition = pointsEndBackArr[i].localPosition;
		}

		//the score value will show here
		//cancle invoe here
		for (int i=0; i <ScoreValueObj.Length; i++)
		{
			ScoreValueObj[i].SetActive(true);
		}
		
		if (isChuangjilu)
		{
			chuangjiluObj.SetActive(true);
		}

		//lishi jilu
		LishijiluTPSobj.enabled = false;
		lishiJiluObj.transform.localPosition = pointsEndBackArr [5].localPosition;
		lishiJiluObj.SetActive (true);

		//the score parent will fly out after 3 seconds
		Invoke ("ScoreGuiFlyout", 3.0f);
	}
	float jiangli = 0;
	public void judgeDengji()
	{//Debug.Log ("judegedengjiiiiiiiiiiiiiiiiiii   " );
		//useTotalTime
		pcvr.curPingjiDengji = pcvr.curShijianquyu.Length;
		jiangli = 0;
		pcvr.canFree = false;

		for (int i = 0; i < pcvr.curShijianquyu.Length; i++)
		{//Debug.Log("useTotalTime  " + i + " "+ useTotalTime + " " + pcvr.curShijianquyu[i]);
			if (useTotalTime > 0 && useTotalTime <= pcvr.curShijianquyu[i])
			{
				pcvr.curPingjiDengji = i;
				break;
			}
		}

		float curLevelJiangli = pcvr.pingjiJiangli[pcvr.curPingjiDengji];
		Debug.Log ("curlevel   " + (pcvr.guakaDengji[Application.loadedLevel - 2]) + " "+ (pcvr.curPingjiDengji));

		if (pcvr.guakaDengji[Application.loadedLevel - 2] >= 0 && pcvr.guakaDengji[Application.loadedLevel - 2] <= 2)
		{
			curLevelJiangli = 0;
		}

		if (pcvr.guakaDengji[Application.loadedLevel - 2] < 0 || pcvr.guakaDengji[Application.loadedLevel - 2] > pcvr.curPingjiDengji)
		{
			pcvr.guakaDengji[Application.loadedLevel - 2] = pcvr.curPingjiDengji;
		}

		Debug.Log ("curlevelaaa   " + (pcvr.guakaDengji[Application.loadedLevel - 2]) + " "+ (pcvr.curPingjiDengji));
		jiangli = curLevelJiangli + pcvr.shangciShengyuJiangli;
		Debug.Log ("jianglijianglijiangli  " + jiangli + " " + curLevelJiangli + " "+ pcvr.pingjiJiangli[pcvr.curPingjiDengji] + " "+ pcvr.shangciShengyuJiangli);
		if (jiangli >= 2.0f)
		{
			jiangli = 1.8f;
		}

		if (jiangli == 1.0f)
		{
			jiangli = 1.01f;
		}

		if (jiangli >= 1.0f)
		{
			pcvr.canFree = true;
		}

		pingjiaParentObj.SetActive (true);
	}

	public void showPingjiaResult()
	{//Debug.Log ("pingjiaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa " + pcvr.curPingjiDengji);
		pingjiaSSobj.spriteName = "dengji" + pcvr.curPingjiDengji.ToString ();
		Invoke ("showpingjiaresuletDelay", 1.0f);
	}
	
	void showpingjiaresuletDelay()
	{
		pingjiaResultObj.SetActive (true);
		
		if (pcvr.sound2DENDScrObj)
		{
			pcvr.sound2DENDScrObj.playAudioPingfen(true, pcvr.curPingjiDengji);
		}
	}

	public void showJinducaoDibu()
	{
		//judge whether pass all the levels(8)
		bool isPassAllLevels = true;
		
		for (int i=0; i < pcvr.guakaDengji.Length; i++)
		{
			if (pcvr.guakaDengji[i] < 0)
			{
				isPassAllLevels = false;
				break;
			}
		}
		isPassAllLevels = false;
		if (isPassAllLevels)
		{
			//pass all the level
			//reset all the informaiton
			//then into xunhuandonghua
			Invoke ("endGameIntoDonghua", 2.0f);
			return;
		}
		else
		{
			Invoke ("showJinducaoDibuDelay", 1.0f);
		}
	}

	void endGameIntoDonghua()
	{//pass all the level
		resetJixuchuangguanInfor();
		pcvr.UITruckScrObj.pressStartButtonLe (false);
	}
	
	public void showJinducaoDibuDelay()
	{
		jinducaoParentObj.SetActive (true);
		jinducaoSObj.initFillAmount (pcvr.shangciShengyuJiangli);

		Invoke ("beginchangeJindutiao1", 0.5f);
	}

	void beginchangeJindutiao1()
	{
		pressButtonState = 2;

		if (pcvr.sound2DENDScrObj)
		{
			pcvr.sound2DENDScrObj.playAudioJinducaoGuocheng(true);
		}

		if (jiangli > 1.0f)
		{
			jinducaoSObj.beginMove1 (1.0f);
		}
		else
		{
			jinducaoSObj.beginMove1 (jiangli);
		}
	}

	//jin du tiao move end
	public void finishedPartOne()
	{
		//do first
		if (pcvr.sound2DENDScrObj)
		{
			pcvr.sound2DENDScrObj.playAudioJinducaoGuocheng(false);
		}

		if (jiangli >= 1.0f)
		{
			//chenggong
			pingfenChenggongObj.SetActive(true);

			if (pcvr.sound2DENDScrObj)
			{
				pcvr.sound2DENDScrObj.playAudioChenggong(true);
			}
		}
		else
		{
			pressButtonState = -1;
			finishedJindutiao();
		}
	}

	public void chenggongBofangend()
	{
		if (pcvr.sound2DENDScrObj)
		{
			pcvr.sound2DENDScrObj.playAudioJinducaoGuocheng(true);
		}

		jinducaoSObj.beginMove2 (jiangli - 1.0f);
	}
	
	//jin du tiao move end----part two --- 
	public void finishedJindutiao()
	{
		pressButtonState = -1;
		playAudioJindutiaowan ();
		Invoke ("EnterXuanzemoshi", 3.0f);
	}

	void EnterXuanzemoshi()
	{
		pingfenxitongTWPos.enabled = true;
	}

	public void endPingfenxitongMove()
	{
		daojishiObjMoshi.SetActive (true);

		if (pcvr.networkPCVR == 1)
		{
			downTimeIntNum = downTimeIntNumStatic;
			pressButtonState = 3;
		}
		else
		{
			//downTimeIntNum = downTimeIntNumStatic;
			//pressButtonState = 3;
			//ji xu chuangguan
			if (pcvr.canFree || pcvr.coinCurNumPCVR >= pcvr.startCoinNumPCVR)
			{//will in to select level mode
				pcvr.isChuangguan = true;
				pressButtonState = 0;
				jiesuan(true);
				pcvr.UITruckScrObj.pressStartButtonLe (true);
				isToubiJieduan = false;
			}
			else
			{
				pressButtonState = 0;
				
				gaiToubile();
			}
		}
        
        //直接进入游戏关卡选择界面.
        selectMoshi(true, false);
	}

	//change the speed
	public void jiasuJindutiao()
	{
		jinducaoSObj.changeMoveSpeed (2.0f);
	}

	void playAudioJindutiaowan()
	{
		if (pcvr.sound2DENDScrObj)
		{
			pcvr.sound2DENDScrObj.playAudioJinducaoGuocheng(false);
			pcvr.sound2DENDScrObj.playAudioJinducao(true);
		}
	}
	
	//time end or press state
	public void selectMoshi(bool isTimeend, bool toubiTimeEnd)
	{//Debug.Log ("selec1tMoshiselec1tMoshisel1ectMoshi  linkguiiii " + moveLeft + " "+ pcvr.canFree + " "+ pcvr.coinCurNumPCVR + " "+ pcvr.startCoinNumPCVR + " "+ isTimeend);
		if (isToubiJieduan && pcvr.coinCurNumPCVR < pcvr.startCoinNumPCVR && !isTimeend)
		{
			return;
		}

		if (!moveLeft)
		{
			//ji xu chuangguan
			if (pcvr.canFree || pcvr.coinCurNumPCVR >= pcvr.startCoinNumPCVR)
			{//will in to select level mode
				pcvr.isChuangguan = true;
				pressButtonState = 0;
				jiesuan(true);
				pcvr.UITruckScrObj.pressStartButtonLe (true);
				isToubiJieduan = false;
			}
			else
			{
				pressButtonState = 0;

				if (toubiTimeEnd)
				{
					gameOverLeee();
					isToubiJieduan = false;
				}
				else
				{
					gaiToubile();
				}
			}
		}
		else
		{
			//mo shi xuan ze
			if (pcvr.canFree || pcvr.coinCurNumPCVR >= pcvr.startCoinNumPCVR)
			{//will in to select level mode
				pcvr.isChuangguan = false;
				pressButtonState = 0;
				jiesuan(true);
				pcvr.UITruckScrObj.pressStartButtonLe (true);
				isToubiJieduan = false;
			}
			else
			{
				pressButtonState = 0;
				
				if (toubiTimeEnd)
				{
					gameOverLeee();
					isToubiJieduan = false;
				}
				else
				{
					gaiToubile();
				}
			}
		}
	}

	void gaiToubile()
	{
		insertCoinTishiLeft.SetActive (true);
		moshixuanzeObj.SetActive(false);
		startButtonObj.SetActive(false);
		isToubiJieduan = true;
		insertTimeCur = insertTimeCurStatic;
		insertTimeIntCur = 0;
	}

	void gameOverLeee()
	{
		jiesuan(false);
		resetJixuchuangguanInfor();

		insertCoinTishiLeft.SetActive (false);
		moshixuanzeObj.SetActive(false);
		startButtonObj.SetActive (false);
		
		if (pcvr.sound2DENDScrObj)
		{
			pcvr.sound2DENDScrObj.playAudioJifen(false);
		}

		if (pcvr.sound2DENDScrObj)
		{
			pcvr.sound2DENDScrObj.playAudioGameover(true);
		}

		pcvr.aFirstScriObj.OverXiaoguo ();
		gameOverObj.SetActive (true);

		Invoke ("gameOverIntoDonghua", 3.0f);
	}

	void gameOverIntoDonghua()
	{
		pcvr.UITruckScrObj.pressStartButtonLeOver (false);
	}
	
	void jiesuan(bool isBaoliu)
	{
		if (!isBaoliu)
		{
			return;
		}
		
		if (jiangli > 1.0f)
		{
			pcvr.shangciShengyuJiangli = jiangli - 1.0f;
		}
		else
			pcvr.shangciShengyuJiangli = jiangli;
	}

	void resetJixuchuangguanInfor()
	{//Debug.Log ("resetJi1xuchuagguanInforres1etJixuchuagguanInforresetJixu1chuagguanInfor");
		pcvr.shangciShengyuJiangli = 0;

		for (int i=0; i < pcvr.guakaDengji.Length; i++)
		{
			pcvr.guakaDengji[i] = -1;
		}

		pcvr.curPingjiDengji = -1;
	}

	private float spaceTime = 0;
	private float spaceTimeStatic = 0.40f;		//the space time of moving to left or right
	
	//about some down time
	public GameObject selectTimeObj1 = null;
	public GameObject selectTimeObj2 = null;
	public GameObject selectTimeObj3 = null;
	public UISprite selectTimeSObj1 = null;
	public UISprite selectTimeSObj2 = null;
	public UISprite selectTimeSObj3 = null;
	private float selectTotalTime = 30.0f;
	
	private int timeNow = 0;

	void showTimeDownSelect(float timeT)
	{
		if (timeT < 0)
		{
			timeT = 0;
		}
		
		timeNow = Mathf.FloorToInt(timeT);
		
		if (timeNow >= 10)
		{
			selectTimeSObj1.spriteName = Mathf.FloorToInt(timeNow / 10).ToString();
			selectTimeSObj2.spriteName = Mathf.FloorToInt(timeNow % 10).ToString();
		}
		else
		{
			selectTimeObj1.SetActive (false);
			selectTimeObj2.SetActive (false);
			selectTimeObj3.SetActive (true);
			selectTimeSObj3.spriteName = timeNow.ToString();
		}
	}
	
	//fangxiangpan
	public UISprite selectFangxiang = null;
	private int selectFangxiangIndex = 0;
	private int fangxiangIndexTemp = 0;
	private string fangxiangNameTemp = "";
	private float selectpanJiangeTime = 0.5f;
	private float selectpanJiangeSTime = 0.2f;
	
	void changeSelectFangxiangS(int index)
	{
		fangxiangIndexTemp = index % 4;
		switch(fangxiangIndexTemp)
		{
		case 0:
			fangxiangNameTemp = "fangxiangpan1";
			break;
		case 1:
			fangxiangNameTemp = "fangxiangpan3";
			break;
		case 2:
			fangxiangNameTemp = "fangxiangpan1";
			break;
		case 3:
			fangxiangNameTemp = "fangxiangpan2";
			break;
		}
		
		selectFangxiang.spriteName = fangxiangNameTemp;
	}
}
