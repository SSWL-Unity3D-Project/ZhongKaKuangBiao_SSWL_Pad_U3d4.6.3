using UnityEngine;

public class SSGameGlobalData : MonoBehaviour
{
    static SSGameGlobalData _Instance;
    public static SSGameGlobalData GetInstance()
    {
        return _Instance;
    }
    
    // Use this for initialization
    void Start()
    {
        _Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [System.Serializable]
    public class AiTruckData
    {
        /// <summary>
        /// Ai卡车起步时间.
        /// Ai起步阶段使用比较低的速度.
        /// </summary>
        public float qiBuTime = 8f;
    }
    /// <summary>
    /// Ai卡车的公用配置数据信息.
    /// </summary>
    public AiTruckData m_AiTruckData;
}
