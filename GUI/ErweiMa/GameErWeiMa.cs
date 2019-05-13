using UnityEngine;
using Server.FTPadServer;
using UnityEngine.UI;

public class GameErWeiMa : MonoBehaviour
{
    /// <summary>
    /// 游戏二维码.
    /// </summary>
    public Image ErWeiMaImg;
    // Use this for initialization
    void Start ()
    {
        SetGameErWeiMaData();
    }

    void SetGameErWeiMaData()
    {
        if (ErWeiMaImg == null)
        {
            return;
        }

        FTServerManage ftServer = FTServerManage.GetInstance();
        if (ftServer != null)
        {
            if (ftServer.m_ErWeiMaSprite != null)
            {
                ErWeiMaImg.sprite = ftServer.m_ErWeiMaSprite;
            }
            ftServer.img = ErWeiMaImg;
        }
    }
}
