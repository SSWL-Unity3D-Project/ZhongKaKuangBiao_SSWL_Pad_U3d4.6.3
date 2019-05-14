using UnityEngine;

public class SSGameUIRoot : SSGameMono
{
    /// <summary>
    /// 当前UI中心锚点.
    /// </summary>
    internal Transform m_UICenterTr;
    static SSGameUIRoot _Instance;
    public static SSGameUIRoot GetInstance()
    {
        if (_Instance == null)
        {
            GameObject obj = new GameObject("_SSGameUIRoot");
            _Instance = obj.AddComponent<SSGameUIRoot>();
        }
        return _Instance;
    }

    private void Start()
    {
        InputEventCtrl.GetInstance().ClickTVYaoKongExitBtEvent += ClickTVYaoKongExitBtEvent;
    }

    private void ClickTVYaoKongExitBtEvent(ButtonState val)
    {
        if (val == ButtonState.DOWN)
        {
            return;
        }

        if (m_UICenterTr != null)
        {
            CreatExitGameUI(m_UICenterTr);
        }
    }

    bool IsRemoveSelf = false;
    internal void RemoveSelf()
    {
        if (IsRemoveSelf == false)
        {
            IsRemoveSelf = true;
            InputEventCtrl.GetInstance().ClickTVYaoKongExitBtEvent -= ClickTVYaoKongExitBtEvent;
            RemoveExitGameUI();
            Destroy(gameObject);
        }
    }

    #region 退出游戏UI界面
    internal bool IsHaveExitGameUI = false;
    internal SSExitGameUI m_SSExitGameUI = null;
    /// <summary>
    /// 创建退出游戏UI界面.
    /// </summary>
    internal void CreatExitGameUI(Transform uiCenterTr)
    {
        if (m_SSExitGameUI == null)
        {
            string prefabPath = "Prefab/GUI/ExitGameUI/ExitGameUI";
            if (Application.loadedLevel != 0)
            {
                prefabPath = "Prefab/GUI/ExitGameUI/ExitGameUI_Game";
            }
            GameObject gmDataPrefab = (GameObject)Resources.Load(prefabPath);
            if (gmDataPrefab != null)
            {
                SSDebug.Log("CreatExitGameUI...");
                GameObject obj = (GameObject)Instantiate(gmDataPrefab, uiCenterTr);
                m_SSExitGameUI = obj.GetComponent<SSExitGameUI>();
                m_SSExitGameUI.Init();
                IsHaveExitGameUI = true;
            }
            else
            {
                SSDebug.LogWarning("CreatExitGameUI -> gmDataPrefab was null, prefabPath == " + prefabPath);
            }
        }
    }
    
    /// <summary>
    /// 删除退出游戏UI界面.
    /// </summary>
    internal void RemoveExitGameUI()
    {
        if (m_SSExitGameUI != null)
        {
            m_SSExitGameUI.RemoveSelf();
            m_SSExitGameUI = null;
            Resources.UnloadUnusedAssets();
        }
    }
    #endregion
}
