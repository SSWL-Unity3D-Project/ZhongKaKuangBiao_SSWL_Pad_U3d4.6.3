using UnityEngine;
using System.Collections;

public class soundControl2D : MonoBehaviour {

	//beijing
	private AudioSource audioBeijing;
	public AudioSource[] audioBeijingSceneArr;

	//xuan guan
	public AudioSource audioXuanguanBeijing;
	public AudioSource audioXuanguanYidong;
	public AudioSource audioXuanguanQueren;

	public AudioSource audioInsertCoin;
	public AudioSource audioStartButton;

	public AudioSource audio321Sound3;
	public AudioSource audio321Sound2;
	public AudioSource audio321Sound1;

	public AudioSource audioDaojishi10s;
	public AudioSource audioGameover;
	public AudioSource audioXubiKaishi;
	public AudioSource audioZhongdian;
	public AudioSource audioZuihouyiquan;
	
	public AudioSource audioChidaoju;		//add-speed or add-time
	public AudioSource audioXieya;			//not throttling
	public AudioSource audioZhuangZhangai;	//hit someth
	public AudioSource audioZhuangChe;		//hit orther NPC or AI
	public AudioSource audioKacheXingshi;	//Moving
	public AudioSource audioKacheJisu;		//Speeding
	public AudioSource audioKacheDaisu;		//Stopping
	public AudioSource audioKacheMingdi;	//la ba
	public AudioSource audioDiansha;		//point brake
	public AudioSource audioChangsha;		//press brake
	public AudioSource audioDianhuo;		//ready to move
	public AudioSource audioChiNaozhong;		//naozhong
	public AudioSource audioChiDanqi;		//danqi
	public AudioSource waterAudioSObj = null;
	public AudioSource audioMingdiTingzhi;	//la ba tingzhi
	public AudioSource audioLubiaotishi;	//lu biao

	void Awake ()
	{
		pcvr.sound2DScrObj = GetComponent<soundControl2D>();

		playAudioCloseOnawake ();
	}

	void playAudioCloseOnawake()
	{
		for (int i=0; i <audioBeijingSceneArr.Length; i++)
		{
			audioBeijingSceneArr[i].playOnAwake =false;
		}

		if (audioXuanguanBeijing)
		{
			audioXuanguanBeijing.playOnAwake = false;
		}

		if (audioXuanguanYidong)
		{
			audioXuanguanYidong.playOnAwake = false;
		}

		if (audioXuanguanQueren)
		{
			audioXuanguanQueren.playOnAwake = false;
		}

		if (audioInsertCoin)
		{
			audioInsertCoin.playOnAwake = false;
		}

		if (audioStartButton)
		{
			audioStartButton.playOnAwake = false;
		}

		if (audio321Sound3)
		{
			audio321Sound3.playOnAwake = false;
		}

		if (audio321Sound2)
		{
			audio321Sound2.playOnAwake = false;
		}

		if (audio321Sound1)
		{
			audio321Sound1.playOnAwake = false;
		}
		
		if (audioDaojishi10s)
		{
			audioDaojishi10s.playOnAwake = false;
		}
		
		if (audioGameover)
		{
			audioGameover.playOnAwake = false;
		}
		
		if (audioXubiKaishi)
		{
			audioXubiKaishi.playOnAwake = false;
		}
		
		if (audioZhongdian)
		{
			audioZhongdian.playOnAwake = false;
		}
		
		if (audioZuihouyiquan)
		{
			audioZuihouyiquan.playOnAwake = false;
		}
		if (audioChidaoju)
		{
			audioChidaoju.playOnAwake = false;
		}
		
		if (audioXieya)
		{
			audioXieya.playOnAwake = false;
		}
		
		if (audioZhuangZhangai)
		{
			audioZhuangZhangai.playOnAwake = false;
		}
		
		if (audioZhuangChe)
		{
			audioZhuangChe.playOnAwake = false;
		}
		
		if (audioKacheXingshi)
		{
			audioKacheXingshi.playOnAwake = false;
		}
		
		if (audioKacheJisu)
		{
			audioKacheJisu.playOnAwake = false;
		}
		
		if (audioKacheDaisu)
		{
			audioKacheDaisu.playOnAwake = false;
		}
		
		if (audioKacheMingdi)
		{
			audioKacheMingdi.playOnAwake = false;
		}
		
		if (audioDiansha)
		{
			audioDiansha.playOnAwake = false;
		}
		
		if (audioChangsha)
		{
			audioChangsha.playOnAwake = false;
		}
		
		if (audioDianhuo)
		{
			audioDianhuo.playOnAwake = false;
		}
		
		if (audioChiNaozhong)
		{
			audioChiNaozhong.playOnAwake = false;
		}
		
		if (audioChiDanqi)
		{
			audioChiDanqi.playOnAwake = false;
		}
		
		if (waterAudioSObj)
		{
			waterAudioSObj.playOnAwake = false;
		}
		
		if (audioMingdiTingzhi)
		{
			audioMingdiTingzhi.playOnAwake = false;
		}
		
		if (audioLubiaotishi)
		{
			audioLubiaotishi.playOnAwake = false;
		}
	}
	int beijingIndex = 0;
	public void setBeijing(int levelIndex)
	{
		if (levelIndex < 2)
		{
			audioBeijing = null;
			return;
		}

		beijingIndex = Random.Range (0, 8);
		audioBeijing = audioBeijingSceneArr[beijingIndex];
		//Debug.Log ("beingjjjj   " + beijingIndex);

		//audioBeijing = audioBeijingSceneArr[levelIndex - 2];

		pcvr.GetInstance().audioBeijingUI = audioBeijing;
	}

	//play the game back music
	public void playAudioBeijing(bool isPlay)
	{
		if (audioBeijing && isPlay)
		{
			audioBeijing.Stop();
			audioBeijing.loop = true;
			audioBeijing.Play();
		}
		else if (audioBeijing && !isPlay)
		{
			audioBeijing.Stop();
		}
	}

	//xuanguan - beijing
	public void playAudioXuanguanBeijing(bool isPlay)
	{
		if (isPlay && audioXuanguanBeijing && !audioXuanguanBeijing.isPlaying)
		{Debug.Log ("playAudioXuanguanBeijingplayAudioXuanguanBeijing " + isPlay);
			audioXuanguanBeijing.Stop();
			audioXuanguanBeijing.loop = true;
			audioXuanguanBeijing.Play();
		}
		else if (!isPlay && audioXuanguanBeijing && audioXuanguanBeijing.isPlaying)
		{
			audioXuanguanBeijing.Stop();
		}
	}

	//xuanguan - yidong
	public void playAudioXuanguanYidong()
	{
		if (audioXuanguanYidong)
		{
			audioXuanguanYidong.loop = false;
			audioXuanguanYidong.Stop();
			audioXuanguanYidong.Play();
		}
	}

	//xuanguan - queren
	public void playAudioXuanguanQueren()
	{
		if (audioXuanguanQueren)
		{
			audioXuanguanQueren.Stop();
			audioXuanguanQueren.loop = false;
			audioXuanguanQueren.Play();
		}
	}

	//insert coin
	public void playAudioInsertCoin()
	{
		if (audioInsertCoin)
		{
			audioInsertCoin.loop = false;
			audioInsertCoin.Stop();
			audioInsertCoin.Play();
		}
	}

	//press start button
	public void playAudioStart()
	{
		if (audioStartButton)
		{
			audioStartButton.loop = false;
			audioStartButton.Stop();
			audioStartButton.Play();
		}
	}

	//dao ji shi 3-2-1
	public void playAudio321(int index)
	{
		if (index == 3 && audio321Sound3 && !audio321Sound3.isPlaying)
		{
			audio321Sound3.loop = false;
			audio321Sound3.Play();
		}
		else if (index == 2 && audio321Sound2 && !audio321Sound2.isPlaying)
		{
			audio321Sound2.loop = false;
			audio321Sound2.Play();
		}
		else if (index == 1 && audio321Sound1 && !audio321Sound1.isPlaying)
		{
			audio321Sound1.loop = false;
			audio321Sound1.Play();
		}
	}

	//10s daojishi
	public void playAudioDaojishi10s()
	{
		if (audioDaojishi10s && !audioDaojishi10s.isPlaying)
		{
			audioDaojishi10s.loop = false;
			audioDaojishi10s.Play();
		}
	}

	//game over
	public void playAudioGameover()
	{
		if (audioGameover && !audioGameover.isPlaying)
		{
			audioGameover.loop = false;
			audioGameover.Play();
		}
	}

	//start
	public void playAudioXubikaishi()
	{
		if (audioXubiKaishi && !audioXubiKaishi.isPlaying)
		{
			audioXubiKaishi.loop = false;
			audioXubiKaishi.Play();
		}
	}

	//finished point
	public void playAudioZhongdian()
	{
		if (audioZhongdian && !audioZhongdian.isPlaying)
		{
			audioZhongdian.loop = false;
			audioZhongdian.Play();
		}
	}

	//zuihouyiquan
	public void playAudioZuihouyiquan(bool play)
	{
		if (play && audioZuihouyiquan && !audioZuihouyiquan.isPlaying)
		{
			audioZuihouyiquan.loop = false;
			audioZuihouyiquan.Play();
		}
		else if (!play && audioZuihouyiquan && audioZuihouyiquan.isPlaying)
		{
			audioZuihouyiquan.Stop();
		}
	}

	//daoju and hit other AI or NPC
	public void playAudioHit(int audioIndex)
	{
		if (audioIndex == 1 && audioChidaoju)
		{
			//daoju
			audioChidaoju.Stop();
			audioChidaoju.loop = false;
			audioChidaoju.Play();
		}
		else if (audioIndex == 10 && audioChiNaozhong)
		{
			//daoju
			audioChiNaozhong.Stop();
			audioChiNaozhong.loop = false;
			audioChiNaozhong.Play();
		}
		else if (audioIndex == 11 && audioChiDanqi)
		{
			//daoju
			audioChiDanqi.Stop();
			audioChiDanqi.loop = false;
			audioChiDanqi.Play();
		}
		/*else if (audioIndex == 2 && audioZhuangZhangai && !audioZhuangZhangai.isPlaying)
		{
			//zhangaiwu
			audioZhuangZhangai.loop = false;
			audioZhuangZhangai.Play();
		}*/
		else if (audioIndex == 3 && audioZhuangChe && !audioZhuangChe.isPlaying)
		{
			//zhuangche
			audioZhuangChe.loop = false;
			audioZhuangChe.Play();
		}
	}
	
	//1-not moving; 2-moving; 3-speeding; 4-stop all audio
	public int playAudioMoveStop(int audioIndex)
	{
		int audioMoveStopState = 0;

		if (audioIndex == 1 && audioKacheDaisu)
		{
			//stop - daisu
			if (!audioKacheDaisu.isPlaying)
			{
				audioKacheDaisu.loop = true;
				audioKacheDaisu.Play();
			}
			
			if (audioKacheXingshi && audioKacheXingshi.isPlaying)
			{
				audioKacheXingshi.Stop();
			}
			
			if (audioKacheJisu && audioKacheJisu.isPlaying)
			{
				audioKacheJisu.Stop();
			}
			
			audioMoveStopState = 1;
		}
		else if (audioIndex == 2 && audioKacheXingshi)
		{
			//moving
			if (!audioKacheXingshi.isPlaying)
			{
				audioKacheXingshi.loop = true;
				audioKacheXingshi.Play();
			}
			
			if (audioKacheJisu && audioKacheJisu.isPlaying)
			{
				audioKacheJisu.Stop();
			}
			
			if (audioKacheDaisu && audioKacheDaisu.isPlaying)
			{
				audioKacheDaisu.Stop();
			}
			
			audioMoveStopState = 2;
		}
		else if (audioIndex == 3 && audioKacheJisu)
		{
			//speeding
			if (!audioKacheJisu.isPlaying)
			{
				audioKacheJisu.loop = true;
				audioKacheJisu.Play();
			}
			
			if (audioKacheXingshi && audioKacheXingshi.isPlaying)
			{
				audioKacheXingshi.Stop();
			}
			
			if (audioKacheDaisu && audioKacheDaisu.isPlaying)
			{
				audioKacheDaisu.Stop();
			}
			
			audioMoveStopState = 3;
		}
		else
		{//after game over or pass game level, and the truck stop moving
			if (audioKacheDaisu && audioKacheDaisu.isPlaying)
			{
				audioKacheDaisu.Stop();
			}
			
			if (audioKacheXingshi && audioKacheXingshi.isPlaying)
			{
				audioKacheXingshi.Stop();
			}
			
			if (audioKacheJisu && audioKacheJisu.isPlaying)
			{
				audioKacheJisu.Stop();
			}
			
			audioMoveStopState = 1;
		}

		return audioMoveStopState;
	}
	
	public void playAudioBrake(int audioIndex, bool isPlay)
	{
		if (audioIndex == 1 && isPlay && audioDiansha && !audioDiansha.isPlaying)
		{
			//diansha
			audioDiansha.loop = false;
			audioDiansha.Play();
		}
		else if (audioIndex == 1 && !isPlay && audioDiansha && audioDiansha.isPlaying)
		{
			//diansha stop
			audioDiansha.Stop();
		}
		else if (audioIndex == 2 && isPlay && audioChangsha && !audioChangsha.isPlaying)
		{
			//changsha
			audioChangsha.loop = true;
			audioChangsha.Play();
		}
		else if (audioIndex == 2 && !isPlay && audioChangsha && audioChangsha.isPlaying)
		{
			//changsha stop
			audioChangsha.Stop();
		}
	}
	
	public void playAudioDianhuo(bool isPlay)
	{
		if (audioDianhuo && isPlay && !audioDianhuo.isPlaying)
		{
			audioDianhuo.loop = false;
			audioDianhuo.Play();
		}
		else if (audioDianhuo && !isPlay && audioDianhuo.isPlaying)
		{
			audioDianhuo.Stop();
		}
	}
	
	public void playAudioMingdi(bool isPlay)
	{
		if (audioKacheMingdi && isPlay && !audioKacheMingdi.isPlaying)
		{
			audioKacheMingdi.Stop();
			audioKacheMingdi.loop = true;
			audioKacheMingdi.Play();
		}
		else if (audioKacheMingdi && !isPlay)
		{
			if (audioMingdiTingzhi)
			{
				audioKacheMingdi.Stop();
				audioMingdiTingzhi.loop = false;
				audioMingdiTingzhi.Play();
			}
			else
				audioKacheMingdi.Stop();
		}
	}
	
	public int playAudioXieya(bool isPlay)
	{
		int xieyaIndex = 0;
		
		if (audioXieya && isPlay && !audioXieya.isPlaying)
		{
			audioXieya.loop = false;
			audioXieya.Play();
			xieyaIndex = 1;
		}
		else if (audioXieya && !isPlay && audioXieya.isPlaying)
		{
			audioXieya.Stop();
			xieyaIndex = 2;
		}

		return xieyaIndex;
	}
	
	public void playAudioWater(bool isPlay)
	{
		if (waterAudioSObj && isPlay && !waterAudioSObj.isPlaying)
		{
			waterAudioSObj.loop = false;
			waterAudioSObj.Play();
		}
		else if (waterAudioSObj && !isPlay && waterAudioSObj.isPlaying)
		{
			waterAudioSObj.Stop();
		}
	}
	
	private float xingshiSpeedMin = 10.0f;
	private float xingshiPitchMin = 0.1f;
	private float xingshiSpeedMax = 120.0f;
	private float xingshiPitchMax = 1.0f;

	public void setPitchVar(float xingshiSpeedMinT, float xingshiPitchMinT, float xingshiSpeedMaxT, float xingshiPitchMaxT)
	{
		xingshiSpeedMin = xingshiSpeedMinT;
		xingshiPitchMin = xingshiPitchMinT;
		xingshiSpeedMax = xingshiSpeedMaxT;
		xingshiPitchMax = xingshiPitchMaxT;
	}

	public void changeSpeedPithc(int audioMoveStopState, float nowSpeed)
	{
		if (audioMoveStopState == 2 && audioKacheXingshi.isPlaying && Mathf.Abs(nowSpeed) < xingshiSpeedMin)
		{
			audioKacheXingshi.pitch = xingshiPitchMin;
		}
		else if (audioMoveStopState == 2 && audioKacheXingshi.isPlaying && Mathf.Abs(nowSpeed) > xingshiSpeedMax)
		{
			audioKacheXingshi.pitch = xingshiPitchMax;
		}
		else if (audioMoveStopState == 2 && audioKacheXingshi.isPlaying)
		{
			audioKacheXingshi.pitch = xingshiPitchMin + (Mathf.Abs(nowSpeed) * (xingshiPitchMax - xingshiPitchMin)) / (xingshiSpeedMax - xingshiSpeedMin);
			
			if (audioKacheXingshi.pitch >= xingshiPitchMax)
			{
				audioKacheXingshi.pitch = xingshiPitchMax;
			}
		}
	}
	
	//zuihouyiquan
	public void playAudioLubiaotishi(bool play)
	{
		if (play && audioLubiaotishi)
		{
			audioLubiaotishi.loop = false;
			audioLubiaotishi.Stop();
			audioLubiaotishi.Play();
		}
		else if (!play && audioLubiaotishi && audioLubiaotishi.isPlaying)
		{
			audioLubiaotishi.Stop();
		}
	}
}
