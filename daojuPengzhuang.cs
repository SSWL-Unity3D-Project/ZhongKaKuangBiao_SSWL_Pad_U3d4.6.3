using UnityEngine;
using System.Collections;

public class daojuPengzhuang : MonoBehaviour {

	//public AudioSource audioPengzhuang = null;
	//public GameObject liziXiaoguo = null;

	private float addForceValue = 1000.0f;
	public bool isPengle = false;
	private bool isNPCObj = false;
	private Shader changeShaderObj = null;
	private Vector3 liziPoint = Vector3.zero;
	private bool isChange =false;

	// Use this for initialization
	void Start () {
		changeShaderObj = Shader.Find ("Transparent/Diffuse");

		if (pcvr.aFirstScriObj.audioPengzhuang)
		{
			pcvr.aFirstScriObj.audioPengzhuang.playOnAwake = false;
		}

		SearchRender();

		//changeShder ();

		/*
		//test whether has rigidbody
		 bool hasRigidbody = false;

		foreach(Transform child in transform)
		{
			if (child.GetComponent<Rigidbody>())
			{
				hasRigidbody = true;
				Debug.Log("has " + child.gameObject.name);
				break;
			}
		}*/
	}
	
	// Update is called once per frame
	void Update () {

		if (isPengle)
		{
			if (!isNPCObj && alphavalue <= 0)
			{
				Destroy(gameObject);
			}

			alphavalue -= Time.deltaTime * 0.35f;

			if (alphavalue <= 1.0f && !isChange)
			{
				isChange = true;
				changeShder();
			}

			if (alphavalue < 0) alphavalue = 0;

			_SetupMaterialAlpha(alphavalue);
		}
	
	}
	float alphavalue = 2.0f;
	public void daojuPengshangle(Vector3 forceVec, bool isNPC, Vector3 chetouPointT)
	{//should judge the object is NPC or normal objects
		if (isPengle)
		{
			return;
		}

		alphavalue = 2.0f;
		isNPCObj = isNPC;
		liziPoint = chetouPointT;

		//changeShder ();
		playSoundAndParticle ();
		
		isPengle = true;

		//rigidbody.AddForce(Vector3.Normalize(forceVec) * addForceValue, ForceMode.Acceleration);
		//rigidbody.AddForce(Vector3.up * 1300.0f, ForceMode.Acceleration);
		//Debug.Log ("addforceeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee");
	}

	private Renderer[] m_renderer;
	
	public void SearchRender()
	{
		Renderer[] renderers = this.gameObject.GetComponentsInChildren<Renderer>();
		if(renderers.Length > 0)
		{
			m_renderer = renderers;
		}
	}
	
	public void _SetupMaterialAlpha(float fAlpha)
	{
		if(m_renderer.Length > 0)
		{
			for(int i=0 ; i<m_renderer.Length ; i++)
			{
				Renderer render = m_renderer [ i ];
				
				if( render.materials.Length > 0)
				{
					Material mat = render.materials[0];
					
					if(mat.HasProperty("_Color"))
					{
						Color c = mat.color;
						c.a = fAlpha;
						mat.color = c;
					}
				}
			}
		}
	}

	void changeShder()
	{
		if(m_renderer.Length > 0)
		{
			for(int i=0 ; i<m_renderer.Length ; i++)
			{
				Renderer render = m_renderer [ i ];
				
				if( render.materials.Length > 0)
				{
					Material mat = render.materials[0];
					mat.shader = changeShaderObj;
				}
			}
		}
	}

	void playSoundAndParticle()
	{
		if (pcvr.aFirstScriObj.audioPengzhuang)
		{
			//peng
			pcvr.aFirstScriObj.audioPengzhuang.Stop();
			pcvr.aFirstScriObj.audioPengzhuang.loop = false;
			pcvr.aFirstScriObj.audioPengzhuang.Play();
		}

		GameObject Tobject = null;

		if (pcvr.aFirstScriObj.liziXiaoguo)
		{
			Tobject = (GameObject)Instantiate(pcvr.aFirstScriObj.liziXiaoguo, liziPoint, Quaternion.identity);

			Destroy(Tobject, 2.0f);
		}
	}
}
