using UnityEngine;
using System.Collections;

public enum truckTriName{
	defaultTruck,
	laHuiDian,
	jiaoHuiDian,
	addSpeed,
	NPCSpawn,
	NPCDelete,
	addTime,
	addTime2,
	openLight,
	Deng,
	stopFollow,
	enterGonglu,
	leaveGonglu,
	NPCSmall,
	serverFollowTri,
	serverPointTri,
	Player,
	zhongdian,
	AINode,
	daojuLi,
	enterWater,
	leaveWater,
	enterSuishilu,
	leaveSuishilu,
	enterShadiLu,
	leaveShadiLu,
	tishi,
	qinangQian,
	qinangHou,
	qinangJieshu,
    zhongJiang,
}

public class triTruckInfor : MonoBehaviour {
	public truckTriName curTruckTriName = truckTriName.defaultTruck;
	private string triggerName = "";
	
	public string getTriggerName()
	{
		switch(curTruckTriName)
		{
        case truckTriName.zhongJiang:
            {
                triggerName = "zhongJiangTri";
                break;
            }
        case truckTriName.laHuiDian:
			triggerName = "laHuiDianTri";
			break;
		case truckTriName.jiaoHuiDian:
			triggerName = "jiaoHuiDianTri";
			break;
		case truckTriName.addSpeed:
			triggerName = "addSpeedTri";
			break;
		case truckTriName.NPCSpawn:
			triggerName = "NPCSpawnTri";
			break;
		case truckTriName.NPCDelete:
			triggerName = "NPCDeleteTri";
			break;
		case truckTriName.addTime:
			triggerName = "addTimeTri";
			break;
		case truckTriName.addTime2:
			triggerName = "addTime2Tri";
			break;
		case truckTriName.openLight:
			triggerName = "openLightTri";
			break;
		case truckTriName.Deng:
			triggerName = "DengTri";
			break;
		case truckTriName.stopFollow:
			triggerName = "stopFollowTri";
			break;
		case truckTriName.enterGonglu:
			triggerName = "enterGongluTri";
			break;
		case truckTriName.leaveGonglu:
			triggerName = "leaveGongluTri";
			break;
		case truckTriName.NPCSmall:
			triggerName = "NPCSmallTri";
			break;
		case truckTriName.serverFollowTri:
			triggerName = "serverFollowTriTri";
			break;
		case truckTriName.serverPointTri:
			triggerName = "serverPointTriTri";
			break;
		case truckTriName.Player:
			triggerName = "PlayerTri";
			break;
		case truckTriName.zhongdian:
			triggerName = "zhongdianTri";
			break;
		case truckTriName.AINode:
			triggerName = "AINodeTri";
			break;
		case truckTriName.daojuLi:
			triggerName = "daojuLiTri";
			break;
		case truckTriName.enterWater:
			triggerName = "enterWaterTri";
			break;
		case truckTriName.leaveWater:
			triggerName = "leaveWaterTri";
			break;
		case truckTriName.enterSuishilu:
			triggerName = "enterSuishiluTri";
			break;
		case truckTriName.leaveSuishilu:
			triggerName = "leaveSuishiluTri";
			break;
		case truckTriName.enterShadiLu:
			triggerName = "enterShadiLuTri";
			break;
		case truckTriName.leaveShadiLu:
			triggerName = "leaveShadiLuTri";
			break;
		case truckTriName.tishi:
			triggerName = "tishiTri";
			break;
		case truckTriName.qinangQian:
			triggerName = "qinangQianTri";
			break;
		case truckTriName.qinangHou:
			triggerName = "qinangHouTri";
			break;
		case truckTriName.qinangJieshu:
			triggerName = "qinangJieshuTri";
			break;
		default:
			triggerName = "defaultTruck";
			break;
		}
		return triggerName;
	}
}
