using UnityEngine;
using System.Collections;

public class XKAudioCtrl : MonoBehaviour
{
	private float[] SpectrumData;
	private float maxValue = 0.1f;
	private int valueIndex = 0;
	private float jiangeTotalTime = 0.2f;
	private float jiangeTimeCur = 0.0f;
	//public AudioSource AdSource;
	// Use this for initialization
	void Start()
	{
		SpectrumData = new float[64];	//8192
	}
	// Update is called once per frame
	//void FixedUpdate()
	void Update()
	{
		if (jiangeTimeCur <= 0)
		{
			jiangeTimeCur = jiangeTotalTime;
			
			if (pcvr.uiRunState == 2 && pcvr.GetInstance().audioBeijingUI && pcvr.GetInstance().audioBeijingUI.isPlaying)
			{
				pcvr.GetInstance().audioBeijingUI.GetSpectrumData(SpectrumData, 0, FFTWindow.BlackmanHarris);

				//AudioListener.GetSpectrumData(SpectrumData, 0, FFTWindow.BlackmanHarris);
				//DrawAudioLine();
				
				if (maxValue < SpectrumData[0])
				{
					maxValue = SpectrumData[0];
				}
				//Debug.Log("SpectrumData[0]SpectrumData[0]   " + SpectrumData[0]);
				if (SpectrumData[0] >= maxValue * 0.85f)
				{
					valueIndex = 3;
				}
				else if (SpectrumData[0] >= maxValue * 0.65f)
				{
					valueIndex = 7;
				}
				else if (SpectrumData[0] >= maxValue * 0.5f)
				{
					valueIndex = 4;
				}
				else if (SpectrumData[0] >= maxValue * 0.35)
				{
					valueIndex = 5;
				}
				else if (SpectrumData[0] >= maxValue * 0.18f)
				{
					valueIndex = 2;
				}
				else if (SpectrumData[0] >= maxValue * 0.07f)
				{
					valueIndex = 1;
				}
				else if (SpectrumData[0] >= 0.0f)
				{
					valueIndex = 6;
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