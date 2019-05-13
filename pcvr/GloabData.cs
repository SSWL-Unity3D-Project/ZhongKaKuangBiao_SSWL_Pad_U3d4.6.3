using UnityEngine;
using System.Collections;

public class GlobalData
{//can delete ffffffffffffffffffffffff lxy
	public static int CoinCur;
	private static  GlobalData Instance;
	public static GlobalData GetInstance()
	{
		if (Instance == null) {
			Instance = new GlobalData();
		}
		return Instance;
	}
}
