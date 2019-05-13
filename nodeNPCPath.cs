using UnityEngine;
using System.Collections;
public enum dongzuoTriName{
	run,
	dead,
	left,
	right,
	jiasu,
}

public class nodeNPCPath : MonoBehaviour {
	public dongzuoTriName curDongzuoName = dongzuoTriName.run;
	private string dongzuoName = "";

	public string getNodeActionName()
	{
		switch(curDongzuoName)
		{
		case dongzuoTriName.run:
			dongzuoName = "runTri";
			break;
		case dongzuoTriName.dead:
			dongzuoName = "deadTri";
			break;
		case dongzuoTriName.left:
			dongzuoName = "leftTri";
			break;
		case dongzuoTriName.right:
			dongzuoName = "rightTri";
			break;
		case dongzuoTriName.jiasu:
			dongzuoName = "jiasuTri";
			break;
		default:
			dongzuoName = "";
			break;
		}
		return dongzuoName;
	}
}
