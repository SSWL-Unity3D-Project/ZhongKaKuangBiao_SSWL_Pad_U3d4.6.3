#define USE_UNITY_PING //使用ping功能.
using UnityEngine;
using System.Collections;

public class SSUnityPing : MonoBehaviour
{
    private static SSUnityPing instance = null;
    private string s_ip = "";
    private int s_timeout = 2;
    private Coroutine currentCoroutine = null;
    /// <summary>
    /// 创建游戏ping功能组件.
    /// </summary>
    public static void CreatePing(string ip)
    {
#if USE_UNITY_PING //使用ping功能.
        if (instance == null)
        {
            if (string.IsNullOrEmpty(ip))
            {
                return;
            }

            GameObject go = new GameObject("_UnityPing");
            DontDestroyOnLoad(go);
            instance = go.AddComponent<SSUnityPing>();
            instance.s_ip = ip;
        }
#endif
    }

    public static void RemovePing()
    {
        if (instance != null)
        {
            instance.StopCoroutine(instance.currentCoroutine);
            Destroy(instance.gameObject);
            instance = null;
        }
    }

    /// 
    /// 超时时间（单位秒）
    /// 
    public int Timeout
    {
        set
        {
            if (value > 0)
            {
                s_timeout = value;
            }
        }
        get { return s_timeout; }
    }

    private void Start()
    {
        switch (Application.internetReachability)
        {
            case NetworkReachability.ReachableViaCarrierDataNetwork: // 3G/4G
            case NetworkReachability.ReachableViaLocalAreaNetwork: // WIFI
                {
                    if (currentCoroutine != null)
                    {
                        StopCoroutine(currentCoroutine);
                    }
                    currentCoroutine = StartCoroutine(PingConnect());
                }
                break;
                //case NetworkReachability.NotReachable: // 网络不可用
                //default:
                //    {
                //        if (s_callback != null)
                //        {
                //            s_callback(-1);
                //            Destroy(this.gameObject);
                //        }
                //    }
                //    break;
        }
    }

    private void OnDestroy()
    {
        s_ip = "";
        s_timeout = 20;
    }

    IEnumerator PingConnect()
    {
        // Ping網站 
        Ping ping = null;

        while (true)
        {
            yield return new WaitForSeconds(2.0f);

            if (ping != null)
            {
                ping.DestroyPing();
            }

            ping = new Ping(s_ip);

            int addTime = 0;
            int requestCount = s_timeout * 10; // 0.1秒 请求 1 次，所以请求次数是 n秒 x 10


            // 等待请求返回
            while (!ping.isDone)
            {
                yield return new WaitForSeconds(0.1f);

                // 链接失败
                if (addTime > requestCount)
                {
                    addTime = 0;
                    //SSDebug.LogWarning("ping.time =========== " + ping.time.ToString());
                    Timeout = ping.time;
                    yield break;
                }
                addTime++;
            }

            // 链接成功
            if (ping.isDone)
            {
                //SSDebug.Log("ping.time =========== " + ping.time.ToString());
                Timeout = ping.time;
                yield return null;
            }
        }
    }

#if USE_UNITY_PING //使用ping功能.
    private void OnGUI()
    {
        string info = "ping time: " + Timeout;
        float px = 15f;
        float py = Screen.height - 40f;
        GUI.Box(new Rect(px, py, 100f, 25f), "");
        GUI.Label(new Rect(px, py, 100f, 25f), info);
    }
#endif
}
