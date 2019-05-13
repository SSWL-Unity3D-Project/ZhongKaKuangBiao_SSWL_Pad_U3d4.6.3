using UnityEngine;
using System.Collections;

public class soundControlEND2D : MonoBehaviour {

	public AudioSource beijing1Zhongdian;
	public AudioSource beijing2Jifen;
	public AudioSource[] yinxiaoPingfen;
	public AudioSource yinxiaoJinducaoGuocheng;
	public AudioSource yinxiaoJinducao;
	public AudioSource chenggong;
	public AudioSource gameover;
	
	void Awake ()
	{
		pcvr.sound2DENDScrObj = GetComponent<soundControlEND2D>();
		
		playAudioCloseOnawake ();
	}
	
	void playAudioCloseOnawake()
	{
		if (beijing1Zhongdian)
		{
			beijing1Zhongdian.playOnAwake = false;
		}

		if (beijing2Jifen)
		{
			beijing2Jifen.playOnAwake = false;
		}

		for (int i=0; i < yinxiaoPingfen.Length; i++)
		{
			if (yinxiaoPingfen[i])
			{
				yinxiaoPingfen[i].playOnAwake = false;
			}
		}
		
		if (yinxiaoJinducaoGuocheng)
		{
			yinxiaoJinducaoGuocheng.playOnAwake = false;
		}

		if (yinxiaoJinducao)
		{
			yinxiaoJinducao.playOnAwake = false;
		}

		if (chenggong)
		{
			chenggong.playOnAwake = false;
		}

		if (gameover)
		{
			gameover.playOnAwake = false;
		}
	}
	
	public void playAudioZhongdian(bool isPlay)
	{
		if (beijing1Zhongdian && isPlay && !beijing1Zhongdian.isPlaying)
		{
			beijing1Zhongdian.loop = false;
			beijing1Zhongdian.Play();
		}
		else if (beijing1Zhongdian && !isPlay && beijing1Zhongdian.isPlaying)
		{
			beijing1Zhongdian.Stop();
		}
	}
	
	public void playAudioJifen(bool isPlay)
	{
		if (beijing2Jifen && isPlay && !beijing2Jifen.isPlaying)
		{
			beijing2Jifen.loop = true;
			beijing2Jifen.Play();
		}
		else if (beijing2Jifen && !isPlay && beijing2Jifen.isPlaying)
		{
			beijing2Jifen.Stop();
		}
	}
	
	public void playAudioPingfen(bool isPlay, int index)
	{
		if (yinxiaoPingfen.Length == 0 || yinxiaoPingfen.Length < index)
		{
			return;
		}

		AudioSource yinxiaoPingfenTemp = yinxiaoPingfen[index];

		if (yinxiaoPingfenTemp && isPlay && !yinxiaoPingfenTemp.isPlaying)
		{
			yinxiaoPingfenTemp.loop = false;
			yinxiaoPingfenTemp.Play();
		}
		else if (yinxiaoPingfenTemp && !isPlay && yinxiaoPingfenTemp.isPlaying)
		{
			yinxiaoPingfenTemp.Stop();
		}
	}
	
	public void playAudioJinducaoGuocheng(bool isPlay)
	{
		if (yinxiaoJinducaoGuocheng && isPlay && !yinxiaoJinducaoGuocheng.isPlaying)
		{
			yinxiaoJinducaoGuocheng.loop = true;
			yinxiaoJinducaoGuocheng.Play();
		}
		else if (yinxiaoJinducaoGuocheng && !isPlay && yinxiaoJinducaoGuocheng.isPlaying)
		{
			yinxiaoJinducaoGuocheng.Stop();
		}
	}
	
	public void playAudioJinducao(bool isPlay)
	{
		if (yinxiaoJinducao && isPlay && !yinxiaoJinducao.isPlaying)
		{
			yinxiaoJinducao.loop = false;
			yinxiaoJinducao.Play();
		}
		else if (yinxiaoJinducao && !isPlay && yinxiaoJinducao.isPlaying)
		{
			yinxiaoJinducao.Stop();
		}
	}
	
	public void playAudioChenggong(bool isPlay)
	{
		if (chenggong && isPlay && !chenggong.isPlaying)
		{
			chenggong.loop = false;
			chenggong.Play();
		}
		else if (chenggong && !isPlay && chenggong.isPlaying)
		{
			chenggong.Stop();
		}
	}
	
	public void playAudioGameover(bool isPlay)
	{
		if (gameover && isPlay && !gameover.isPlaying)
		{
			gameover.loop = false;
			gameover.Play();
		}
		else if (gameover && !isPlay && gameover.isPlaying)
		{
			gameover.Stop();
		}
	}
}
