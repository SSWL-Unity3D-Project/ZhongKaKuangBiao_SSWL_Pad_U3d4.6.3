using UnityEngine;

public class ZhongJiangTriggerData : MonoBehaviour
{
    /// <summary>
    /// 是否已经进入到该触发器.
    /// </summary>
    internal bool IsEnterTrigger = false;
    /// <summary>
    /// 是否要超越玩家.
    /// </summary>
    internal bool IsAiChaoYuePlayer = false;
    [System.Serializable]
    public class AiMoveSpeedData
    {
        public float[] speedArray = new float[1] { 100f };
        public float GetRandomSpeed()
        {
            int index = Random.Range(0, 100) % speedArray.Length;
            return speedArray[index];
        }
    }
    /// <summary>
    /// Ai卡车用来超越玩家的速度.
    /// </summary>
    public AiMoveSpeedData MaxSpeed;
    /// <summary>
    /// Ai卡车用来让玩家冲在前面的速度.
    /// </summary>
    public AiMoveSpeedData MinSpeed;
    /// <summary>
    /// 当玩家碰上中奖触发器之后,Ai卡车用该方法获取
    /// </summary>
    public float GetAiTruckSpeed()
    {
        if (IsAiChaoYuePlayer == true)
        {
            return MaxSpeed.GetRandomSpeed();
        }
        else
        {
            return MinSpeed.GetRandomSpeed();
        }
    }
}
