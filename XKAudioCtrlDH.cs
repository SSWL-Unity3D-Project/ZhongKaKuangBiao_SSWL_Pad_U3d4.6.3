using UnityEngine;
using System.Collections;

public class XKAudioCtrlDH : MonoBehaviour
{
	private float[] SpectrumData;
	private float maxValue = 0.1f;
	private int valueIndex = 0;
	private float jiangeTotalTime = 0.2f;
	private float jiangeTimeCur = 0.2f;
	private AudioSource DHBeijingAudio = null;
	private donghua donghuaSObj = null;

	// Use this for initialization
	void Start()
	{
		SpectrumData = new float[64];	//8192
		jiangeTimeCur = 0.2f;
		DHBeijingAudio = GetComponent<AudioSource> ();
		donghuaSObj = GetComponent<donghua>();
	}

	void Update()
	{
		if (!donghuaSObj || donghuaSObj.isEndDonghua)
		{
			return;
		}

		if (jiangeTimeCur <= 0)
		{
			jiangeTimeCur = jiangeTotalTime;
			
			if (DHBeijingAudio && DHBeijingAudio.isPlaying)
			{
				DHBeijingAudio.GetSpectrumData(SpectrumData, 0, FFTWindow.BlackmanHarris);
				
				if (maxValue < SpectrumData[0])
				{
					maxValue = SpectrumData[0];
				}

				if (SpectrumData[0] >= maxValue * 0.75f)
				{
					valueIndex = 1;
				}
				else if (SpectrumData[0] >= maxValue * 0.55f)
				{
					valueIndex = 4;
				}
				else if (SpectrumData[0] >= maxValue * 0.3f)
				{
					valueIndex = 5;
				}
				else if (SpectrumData[0] >= maxValue * 0.2)
				{
					valueIndex = 3;
				}
				else if (SpectrumData[0] >= maxValue * 0.10f)
				{
					valueIndex = 2;
				}
				else if (SpectrumData[0] >= maxValue * 0.04f)
				{
					valueIndex = 6;
				}
				else if (SpectrumData[0] >= 0.0f)
				{
					valueIndex = 7;
				}
				else
				{
					valueIndex = 0;
				}
			}
			else
			{
				valueIndex = 0;
			}
		}
		else
		{
			jiangeTimeCur -= Time.deltaTime;
		}
		
		pcvr.GetInstance().ControlShanguangdeng(valueIndex, true);
	}
	
	public Vector3 StartPos;
	public Vector3 EndPos;
	void DrawAudioLine()
	{
		Debug.DrawLine(StartPos, EndPos, Color.red);
		float skipVal = 10f / SpectrumData.Length;
		float xVal = 0f;
		float yVal = 0f;
		for (int i = 0; i < SpectrumData.Length; i++) {
			yVal = SpectrumData[i] * 4f;
			xVal = skipVal * i;
			Debug.DrawLine(StartPos + new Vector3(xVal, yVal, 0f), StartPos + new Vector3(xVal, -yVal, 0f), Color.yellow);
		}
	}
}