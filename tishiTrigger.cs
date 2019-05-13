using UnityEngine;
using System.Collections;

public class tishiTrigger : MonoBehaviour {
	public int index = 0;
	public float tishiTime = 4.0f;

	public int getSpriteIndex()
	{
		if (index <= 0)
		{
			return 0;
		}

		return index;
	}

	public float getShowTime()
	{
		if (tishiTime <= 0)
		{
			return 1.0f;
		}

		return tishiTime;
	}
}
