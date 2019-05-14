using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Radial Blur (Color Accumulation)")]
[RequireComponent(typeof(Camera))]

public class RadialBlur : ImageEffectBase {
	
	public float SampleDist = 1f;
	public float SampleStrength = 0.0f;
	public float SampleStrengthJiasu = 3.0f;
	public float sampleSpeedMin = 14.0f;
	public float SampleStrengthZhengchang = 0.0f;
	private RenderTexture accumTexture;
	private bool isSpeeding = false;
	public static float uiSpeed = 0;

	override protected void Start()
	{
		pcvr.RadialBlurSObj = GetComponent<RadialBlur>();
		SampleStrength = SampleStrengthZhengchang;
		if(!SystemInfo.supportsRenderTextures)
		{
			enabled = false;
			return;
		}
		base.Start();
	}
	
	override protected void OnDisable()
	{
		base.OnDisable();
		DestroyImmediate(accumTexture);
	}
	
	// Called by camera to apply image effect
	void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
		if (isSpeeding)
		{
			if (Mathf.Abs(uiSpeed) < sampleSpeedMin)
			{
				SampleStrength = SampleStrengthZhengchang;
			}
			else
			{
				SampleStrength = SampleStrengthJiasu;
			}
		}

		// Create the accumulation texture
		if (accumTexture == null || accumTexture.width != source.width || accumTexture.height != source.height)
		{
			DestroyImmediate(accumTexture);
			accumTexture = new RenderTexture(source.width, source.height, 0);
			accumTexture.hideFlags = HideFlags.HideAndDontSave;
			Graphics.Blit( source, accumTexture );
		}
		
		// Setup the texture and floating point values in the shader
		material.SetTexture("_MainTex", accumTexture);
		material.SetFloat("_fSampleDist", SampleDist);
		material.SetFloat("_fSampleStrength", SampleStrength);
		
		// Render the image using the motion blur shader
		Graphics.Blit (source, accumTexture, material);
		Graphics.Blit (accumTexture, destination);
	}

	public void setJiasu()
	{
		if (isSpeeding)
		{
			return;
		}

		isSpeeding = true;
		SampleStrengthZhengchang = SampleStrength;
		SampleStrength = SampleStrengthJiasu;
	}
	
	public void setZhengchang()
	{
		isSpeeding = false;
		SampleStrength = SampleStrengthZhengchang;
	}

	public void setStrengthValue(float valueT)
	{
		SampleStrength = valueT;
	}
}
