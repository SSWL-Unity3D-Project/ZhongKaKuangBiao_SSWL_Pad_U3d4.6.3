using UnityEngine;
using System.Collections;

public class followLinkScript : MonoBehaviour {
	public int followIndex = 0;

	// Use this for initialization
	void Start () {
		if (followIndex > 5 || followIndex < 0)
		{
			followIndex = 0;
		}
	}

	public int getIndex()
	{
		return followIndex;
	}
}
