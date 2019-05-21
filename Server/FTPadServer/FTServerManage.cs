//#define OUT_PRINT_NET_MSG //输出网络消息.
#define SHOW_PLAYER_PAD_INFO //显示玩家手柄操作信息.
#define USE_FT_SERVER_PAD //使用纷腾手柄服务器.
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using GClientLib;
using System.Collections;
using System.Net.NetworkInformation;

namespace Server.FTPadServer
{
    /// <summary>
    /// 纷腾服务器管理组件.
    /// </summary>
    public class FTServerManage : MonoBehaviour
    {
        public Image img;
        internal Sprite m_ErWeiMaSprite;
        /// <summary>
        /// 纷腾服务器消息缓存容器.
        /// </summary>
        List<string> MsgList;
        SocketLib dll_MainLib;
        private string nSessionGuid;
        public string SessionID
        {
            get { return nSessionGuid; }
            set { nSessionGuid = value; }
        }

        private string sGlobalNum = "";
        public string GlobalNum
        {
            get { return sGlobalNum; }
            set { sGlobalNum = value; }
        }

#if USE_FT_SERVER_PAD //使用纷腾手柄服务器.
        /// <summary>
        /// 是否使用纷腾手柄服务器.
        /// </summary>
        static bool IsUseFTServerPad = true;
#else
        /// <summary>
        /// 是否使用纷腾手柄服务器.
        /// </summary>
        static bool IsUseFTServerPad = false;
#endif
        static FTServerManage _Instance;
        /// <summary>
        /// 创建纷腾服务器管理组件.
        /// </summary>
        public static void CreateFTServerManage()
        {
            //return; //test
            if (IsUseFTServerPad == false)
            {
                return;
            }

            SSDebug.Log("CreateFTServerManage----------------------------------");
            if (_Instance == null)
            {
                GameObject obj = new GameObject("_FTServerManage");
                _Instance = obj.AddComponent<FTServerManage>();
            }

            if (_Instance != null)
            {
                _Instance.Init();
            }
        }

        /// <summary>
        /// 获取纷腾服务器管理组件.
        /// </summary>
        public static FTServerManage GetInstance()
        {
            if (IsUseFTServerPad == false)
            {
                return null;
            }

            if (_Instance == null)
            {
                CreateFTServerManage();
            }
            return _Instance;
        }

        bool IsInit = false;
        /// <summary>
        /// 初始化.
        /// </summary>
        void Init()
        {
            if (m_FTServerInterface == null)
            {
                CreatFTServerInterface();
            }

            if (IsInit == true)
            {
                return;
            }
            IsInit = true;
            InitInfo();
            StartCoroutine(DelayLinkServer());
        }

        /// <summary>
        /// 延迟连接服务器.
        /// </summary>
        IEnumerator DelayLinkServer()
        {
            yield return new WaitForSeconds(5f);
            MainStart();
            yield return new WaitForSeconds(3f);
            RegLocal();
        }

        /// <summary>
        /// 在服务器进行连接.
        /// </summary>
        public void MainStart()
        {
            dll_MainLib = new SocketLib();
            dll_MainLib._GCSysMessage += A_ShowMessage;
            dll_MainLib._GCSocketClosed += A_GCSocketClosed;
            dll_MainLib._GCSocketConnected += A_GCSocketConnected;
            dll_MainLib._GCSocketError += A_GCSocketError;
            dll_MainLib._GCSocketReceived += A_GCSocketReceived;
            //纷腾服务器IP.
            string ftServerIp = "123.59.41.81";
            //纷腾服务器端口.
            int port = 10011;
            dll_MainLib.GC_StartUp(ftServerIp, port);
            //创建游戏ping功能组件.
            SSUnityPing.CreatePing(ftServerIp);
        }

        /// <summary>
        /// 在服务器上注册当前机器信息.
        /// </summary>
        public void RegLocal()
        {
            if (dll_MainLib == null)
            {
                SSDebug.LogWarning("RegLocal -> dll_MainLib was null");
                return;
            }

            string defaultPcMac = "000000000000";
            string boxNum = defaultPcMac;
#if UNITY_STANDALONE_WIN
            try
            {
                bool isFindLocalAreaConnection = false;
                NetworkInterface[] nis = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface ni in nis)
                {
                    if (ni.Name == "本地连接" || ni.Name == "Local Area Connection")
                    {
                        isFindLocalAreaConnection = true;
                        boxNum = ni.GetPhysicalAddress().ToString();
                        break;
                    }
                }

                if (isFindLocalAreaConnection == false)
                {
                    SSDebug.LogWarning("RegLocal -> not find local area connection!");
                }
            }
            catch (Exception ex)
            {
                SSDebug.LogWarning("RegLocal -> Mac get error! ex == " + ex);
            }
#endif

            string systemInfo = boxNum;
            dll_MainLib.GC_SendCommand("REG", systemInfo);
            SSDebug.Log("RegLocal -> systemInfo == " + systemInfo);
        }

        /// <summary>
        /// 服务器返回的消息.
        /// </summary>
        private void A_GCSocketReceived(string sArguement)
        {
            sArguement = sArguement.Replace("\r\n", "");
            A_ShowMessage(sArguement);

            string s = sArguement;
            string[] sagr;
            sagr = s.Split(',');
            if (sagr.Length >= 1)
            {
                switch (sagr[0])
                {
                    case "REGOK":
                        {
                            //注册成功.
                            if (sagr.Length >= 1)
                            {
                                SessionID = sagr[1];
                            }
                            if (sagr.Length >= 2)
                            {
                                GlobalNum = sagr[2];
                            }
                            //获取二维码数据.
                            dll_MainLib.GC_SendCommand("Q2CODE", GlobalNum);
                            break;
                        }
                    case "TEST":
                        {
                            //测试.
                            if (sagr.Length >= 4)
                            {
                                dll_MainLib.GC_SendCommand("TEST", sagr[1] + " " + GlobalNum + " " + sagr[3]);
                            }
                            else
                            {
                                dll_MainLib.GC_SendCommand("TEST", sagr[1] + " " + GlobalNum + " " + sagr[sagr.Length - 1]);
                            }
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// 网络连接出错.
        /// </summary>
        private void A_GCSocketError(string sArg)
        {
            SSDebug.LogWarning("A_GCSocketError -> Dll网络连接出错::arg == " + sArg);
            A_ShowMessage("Dll网络连接出错");
            A_ShowMessage(sArg);
        }

        /// <summary>
        /// 网络连接成功.
        /// </summary>
        private void A_GCSocketConnected()
        {
            A_ShowMessage("Dll网络连接成功");
        }

        /// <summary>
        /// 网络连接关闭.
        /// </summary>
        private void A_GCSocketClosed()
        {
            SSDebug.LogWarning("A_GCSocketError -> Dll连接关闭");
            A_ShowMessage("Dll连接关闭");
            m_ServerNetState = ServerNetState.GCSocketClosed;
        }

        /// <summary>
        /// 发送心跳消息到服务器.
        /// </summary>
        void SendXinTiaoMsg()
        {
            if (dll_MainLib == null)
            {
                return;
            }

            if (m_ServerNetState != ServerNetState.CONNECTED_SERVER)
            {
                return;
            }
            //发送心跳消息..
            dll_MainLib.GC_SendCommand("XinTiaoMsg", "123");
            //SSDebug.Log("SendXinTiaoMsg................");
        }

        public enum ServerNetState
        {
            /// <summary>
            /// 没有连接.
            /// </summary>
            NOT_LINK = 0,
            /// <summary>
            /// 连接到网络服务器.
            /// </summary>
            CONNECTED_SERVER = 1,
            /// <summary>
            /// 电脑网络故障.
            /// </summary>
            GCSocketClosed = 2,
        }
        /// <summary>
        /// 服务器网络状态.
        /// </summary>
        ServerNetState m_ServerNetState = ServerNetState.NOT_LINK;

        /// <summary>
        /// 最后一次检测服务器状态的时间.
        /// </summary>
        float m_LastCheckFTServerTime = 0f;
        /// <summary>
        /// 检测纷腾服务器连接状态.
        /// </summary>
        void CheckFTServerLinkState()
        {
            if (dll_MainLib == null)
            {
                return;
            }

            if (Time.frameCount % 5 == 0)
            {
                if (m_ServerNetState == ServerNetState.GCSocketClosed)
                {
                    /*if (SSUIRoot.GetInstance().m_GameUIManage != null && SSUIRoot.GetInstance().m_GameUIManage.m_WangLuoGuZhangUI == null)
                    {
                        //创建网络故障UI界面.
                        SSUIRoot.GetInstance().m_GameUIManage.CreatWangLuoGuZhangUI(SSGameUICtrl.WangLuoGuZhang.Null);
                    }*/
                }
            }

            if (Time.time - m_LastCheckFTServerTime < 10f)
            {
                return;
            }
            m_LastCheckFTServerTime = Time.time;
            
            bool isRelinkServer = false;
            switch (m_ServerNetState)
            {
                case ServerNetState.GCSocketClosed:
                    {
                        //当前机器网络故障,需要重新连接服务器.
                        isRelinkServer = true;
                        break;
                    }
            }

            if (isRelinkServer == true)
            {
                if (NetWorkState() == false)
                {
                    //当前网卡处于禁用状态,不允许重新连接网络服务器.
                    SSDebug.LogWarning("CheckFTServerLinkState disable..................");
                    return;
                }
                //当前机器网络故障,需要重新连接服务器.
                StartCoroutine(DelayLinkServer());
            }
        }

        /// <summary>
        /// 网卡状态
        /// </summary>
        public bool NetWorkState()
        {
            string defaultPcMac = "000000000000";
            string boxNum = defaultPcMac;
#if UNITY_STANDALONE_WIN
            try
            {
                NetworkInterface[] nis = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface ni in nis)
                {
                    if (ni.Name == "本地连接" || ni.Name == "Local Area Connection")
                    {
                        boxNum = ni.GetPhysicalAddress().ToString();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                SSDebug.LogWarning("NetWorkState -> Mac get error! ex == " + ex);
            }
#endif
            return boxNum == defaultPcMac ? false : true;
        }
        
        //private void OnApplicationQuit()
        //{
        //    if (dll_MainLib != null)
        //    {
        //        dll_MainLib.GC_CloseSocket();
        //    }
        //}

        private void A_ShowMessage(string msg)
        {
            if (msg == null || msg.Length < 1)
            {
                return;
            }
            
            string s = msg;
            string[] args = s.Split(',');
            if (args.Length < 1)
            {
                return;
            }

            switch (args[0])
            {
                case "DATA":
                    {
                        //DATA,374b1b26-ea3c-4669-aaca-7e42dc799c0e,move,-32,109
                        //DATA,374b1b26-ea3c-4669-aaca-7e42dc799c0e,button,0
                        if (args.Length > 2)
                        {
                            string key = args[2];
                            if (key == "move")
                            {
                                string huiHuaId = args[1];
                                PlayerData playerDt = FindGamePlayerData(huiHuaId);
                                if (playerDt != null)
                                {
                                    playerDt.directionMsg = msg;
                                }
                            }
                            else if (key == "button")
                            {
                                string huiHuaId = args[1];
                                PlayerData playerDt = FindGamePlayerData(huiHuaId);
                                if (playerDt != null)
                                {
                                    playerDt.buttonMsg = msg;
                                }
                            }
                        }
                        break;
                    }
                default:
                    {
                        if (MsgList != null)
                        {
                            MsgList.Add(msg);
                        }
                        break;
                    }
            }
        }

        void ShowMsg(string s)
        {
#if OUT_PRINT_NET_MSG //输出网络消息.
            SSDebug.Log("ShowMsg -> msg == " + s);
#endif
            OnReceivedMsgFromFTServer(s);
        }

        /// <summary>
        /// 当收到纷腾服务器的回传消息.
        /// </summary>
        void OnReceivedMsgFromFTServer(string args)
        {
            if (args == null || args.Length < 1)
            {
                return;
            }
            //SSDebug.Log("OnReceivedMsgFromFTServer -> msg == " + args);

            string[] sagr = args.Split(',');
            if (sagr.Length >= 1)
            {
                switch (sagr[0])
                {
                    case "LOGIN":
                        {
                            //玩家登录手柄消息.
                            OnReceivedPlayerLoginMsg(sagr);
                            break;
                        }
                    case "DATA":
                        {
                            //玩家手柄操作消息.
                            OnReceivedPlayerPadMsg(sagr);
                            break;
                        }
                    case "WEBSESSION_CLOSE":
                        {
                            //玩家退出手柄消息.
                            OnReceivedPlayerExit(sagr);
                            break;
                        }
                    case "Q2CODE":
                        {
                            //收到游戏二维码数据消息.
                            OnReceivedGameErWeiMaMsg(sagr);
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// 玩家数据信息.
        /// </summary>
        [Serializable]
        public class PlayerData
        {
            /// <summary>
            /// 当前登录玩家的会话id.
            /// </summary>
            public string huiHuaId = "";
            /// <summary>
            /// 玩家Id.
            /// </summary>
            public int userId = 0;
            /// <summary>
            /// 玩家昵称.
            /// </summary>
            internal string name = "";
            /// <summary>
            /// 玩家性别.
            /// </summary>
            internal string sex = "";
            /// <summary>
            /// 玩家头像url.
            /// </summary>
            internal string headUrl = "";
            /// <summary>
            /// 方向信息.
            /// </summary>
            internal string directionMsg = "";
            /// <summary>
            /// 方向信息.
            /// </summary>
            internal string directionInfo = "0, 0";
            /// <summary>
            /// 按键信息.
            /// </summary>
            internal string buttonMsg = "";
            /// <summary>
            /// 按键点击次数.
            /// </summary>
            internal int buttonCount = 0;
            /// <summary>
            /// 子弹数量.
            /// </summary>
            internal int ammoCount = 0;
            /// <summary>
            /// 是否开始进行游戏.
            /// </summary>
            internal bool isPlayGame = false;
            /// <summary>
            /// 玩家索引信息.
            /// </summary>
            internal PlayerEnum indexPlayer = PlayerEnum.Null;
            /// <summary>
            /// 获取玩家操作手柄的状态信息.
            /// </summary>
            internal string GetPlayerActionInfo()
            {
                return "userId " + userId + ", btClickCount " + buttonCount + ", ammoCount " + ammoCount + ", direction " + directionInfo;
            }
        }
        /// <summary>
        /// 登录游戏的玩家数据信息列表.
        /// </summary>
        public List<PlayerData> m_LoginPlayerDt = new List<PlayerData>();

        /// <summary>
        /// 查找玩家游戏数据.
        /// </summary>
        PlayerData FindGamePlayerData(string huiHuaId)
        {
            PlayerData playerDt = m_LoginPlayerDt.Find((dt) =>
            {
                return dt.huiHuaId.Equals(huiHuaId);
            });
            return playerDt;
        }

        /// <summary>
        /// 查找玩家游戏数据.
        /// </summary>
        PlayerData FindGamePlayerData(PlayerEnum indexPlayer)
        {
            PlayerData playerDt = m_LoginPlayerDt.Find((dt) =>
            {
                return dt.indexPlayer.Equals(indexPlayer);
            });
            return playerDt;
        }

        /// <summary>
        /// 查找玩家游戏数据.
        /// </summary>
        PlayerData FindGamePlayerData(int userId)
        {
            PlayerData playerDt = m_LoginPlayerDt.Find((dt) =>
            {
                return dt.userId.Equals(userId);
            });
            return playerDt;
        }
        
        /// <summary>
        /// 添加玩家微信数据信息.
        /// </summary>
        void AddGamePlayerData(PlayerData playerDt)
        {
            if (playerDt != null && m_LoginPlayerDt != null)
            {
                if (FindGamePlayerData(playerDt.userId) == null)
                {
                    m_LoginPlayerDt.Add(playerDt);
                }
            }
        }

        /// <summary>
        /// 删除玩家微信数据信息.
        /// </summary>
        internal void RemoveGamePlayerData(int userId)
        {
            PlayerData playerDt = FindGamePlayerData(userId);
            if (playerDt != null)
            {
                m_LoginPlayerDt.Remove(playerDt);
            }
        }

        /// <summary>
        /// 当玩家开始进行游戏.
        /// </summary>
        internal void OnPlayerStartGame(int userId, PlayerEnum indexPlayer)
        {
            PlayerData playerDt = FindGamePlayerData(userId);
            if (playerDt != null)
            {
                playerDt.isPlayGame = true;
                playerDt.indexPlayer = indexPlayer;
            }
        }

        /// <summary>
        /// 当玩家发射子弹.
        /// </summary>
        internal void OnPlayerFireAmmo(PlayerEnum indexPlayer)
        {
            PlayerData playerDt = FindGamePlayerData(indexPlayer);
            if (playerDt != null)
            {
                if (playerDt.ammoCount >= 999)
                {
                    playerDt.ammoCount = 0;
                }
                playerDt.ammoCount++;
            }
        }

        int userIdTest = 0; //测试用户id信息.
        /// <summary>
        /// 收到玩家手柄登录消息.
        /// </summary>
        void OnReceivedPlayerLoginMsg(string[] args)
        {
            //会话id信息是当玩家每次登录后产生的.
            //LOGIN,会话id信息,机器特征码,玩家id,玩家昵称
            //LOGIN,374b1b26-ea3c-4669-aaca-7e42dc799c0e,43142003142014402211616555881165971,id,name
            //玩家登录消息.
            if (args.Length >= 3)
            {
                int userId = 0;
                string name = "";
                string sex = "";
                string headUrl = "";
                userIdTest++;
                userId = userIdTest;
                name = "test";
                sex = "1";

                if (FindGamePlayerData(userId) == null)
                {
                    //添加玩家数据.
                    PlayerData playerDt = new PlayerData();
                    playerDt.huiHuaId = args[1];
                    playerDt.userId = userId;
                    playerDt.name = name;
                    playerDt.sex = sex;
                    playerDt.headUrl = headUrl;
                    AddGamePlayerData(playerDt);
                    OnPlayerStartGame(userId, PlayerEnum.PlayerOne); //test
                }

                /*WebSocketSimpet.PlayerWeiXinData playerWeiXinDt = new WebSocketSimpet.PlayerWeiXinData();
                playerWeiXinDt.sex = sex;
                playerWeiXinDt.headUrl = headUrl;
                playerWeiXinDt.userName = name;
                playerWeiXinDt.userId = userId;
                if (pcvr.GetInstance().m_HongDDGamePadInterface != null)
                {
                    pcvr.GetInstance().m_HongDDGamePadInterface.OnPlayerLoginFromFTServer(playerWeiXinDt);

                    //测试,暂时当收到登录消息后直接发送开始按键消息.
                    StartCoroutine(TestDelaySendClickStartBtMsg(userId));
                }*/
            }
        }

        IEnumerator TestDelaySendClickStartBtMsg(int userId)
        {
            yield return new WaitForSeconds(2f);
            //if (pcvr.GetInstance().m_HongDDGamePadInterface != null)
            //{
                //测试,暂时当收到登录消息后直接发送开始按键消息.
                //开始按键消息.
                //string startBtDown = Assets.XKGame.Script.HongDDGamePad.HongDDGamePad.PlayerShouBingFireBt.startGameBtDown.ToString();
                //pcvr.GetInstance().m_HongDDGamePadInterface.OnReceiveActionOperationMsgFTServer(startBtDown, userId);
            //}
        }

        /// <summary>
        /// 收到玩家手柄操作消息.
        /// </summary>
        void OnReceivedPlayerPadMsg(string[] args)
        {
            //DATA,374b1b26-ea3c-4669-aaca-7e42dc799c0e,move,-32,109
            //DATA,374b1b26-ea3c-4669-aaca-7e42dc799c0e,button,0
            if (args.Length < 3)
            {
                return;
            }

            int userId = 0; //玩家id信息.
            //userId = 123;
            string huiHuaId = args[1];
            PlayerData playerDt = FindGamePlayerData(huiHuaId);
            if (playerDt != null)
            {
                userId = playerDt.userId;
            }
            else
            {
                return;
            }

            string key = args[2];
            switch (key)
            {
                case "move":
                    {
                        //手柄方向数据消息.
                        //采用向量方式将收到的手柄坐标信息转换为方向信息.
                        //    -1
                        //-1      1
                        //     1
                        //DATA,374b1b26-ea3c-4669-aaca-7e42dc799c0e,move,-32,109
                        if (args.Length >= 5)
                        {
                            float px = Assets.XKGame.Script.Comm.MathConverter.StringToFloat(args[3]);
                            float py = Assets.XKGame.Script.Comm.MathConverter.StringToFloat(args[4]);
                            if (px == py && px == 0f)
                            {
                                //玩家手指离开方向.
                                /*string angle = Assets.XKGame.Script.HongDDGamePad.HongDDGamePad.PlayerShouBingDir.up.ToString();
                                if (pcvr.GetInstance().m_HongDDGamePadInterface != null)
                                {
                                    pcvr.GetInstance().m_HongDDGamePadInterface.OnReceiveDirectionAngleMsgFTServer(angle, userId);
                                }*/
                                if (InputEventCtrl.GetInstance() != null)
                                {
                                    InputEventCtrl.GetInstance().OnReceiveDirectionAngleMsgFTServer(0f);
                                }
                            }
                            else
                            {
                                Vector2 vP0 = new Vector2(-1f, 0f);
                                Vector2 vP1 = new Vector2(px, py);
                                vP1 = vP1.normalized;
                                float cosVal = Vector2.Dot(vP0, vP1);
                                float sign = py > 0f ? -1f : 1f; //方向向下时角度为负数,方向向上时角度为正数.
                                float angle = sign * Mathf.Acos(cosVal) * Mathf.Rad2Deg;
                                //SSDebug.Log("angle =================== " + angle + ", userId == " + userId);
                                /*if (pcvr.GetInstance().m_HongDDGamePadInterface != null)
                                {
                                    pcvr.GetInstance().m_HongDDGamePadInterface.OnReceiveDirectionAngleMsgFTServer(angle.ToString(), userId);
                                }*/
                                if (InputEventCtrl.GetInstance() != null)
                                {
                                    InputEventCtrl.GetInstance().OnReceiveDirectionAngleMsgFTServer(px);
                                }
                            }
                        }
                        break;
                    }
                case "button":
                    {
                        //手柄按键消息.
                        //DATA,374b1b26-ea3c-4669-aaca-7e42dc799c0e,button,0
                        /*if (pcvr.GetInstance().m_HongDDGamePadInterface != null)
                        {
                            //发射按键消息.
                            string fireBtDown = Assets.XKGame.Script.HongDDGamePad.HongDDGamePad.PlayerShouBingFireBt.fireBDown.ToString();
                            pcvr.GetInstance().m_HongDDGamePadInterface.OnReceiveActionOperationMsgFTServer(fireBtDown, userId);
                        }*/
                        string btKey = "0";
                        if (args.Length >= 4)
                        {
                            btKey = args[3];
                        }
                        ButtonState btState = btKey == "0" ? ButtonState.UP : ButtonState.DOWN;
                        
                        if (Application.loadedLevel == 0)
                        {
                            //循环动画关卡.
                            if (InputEventCtrl.GetInstance() != null)
                            {
                                InputEventCtrl.GetInstance().ClickStartBtOne(btState);
                            }
                        }
                        else
                        {
                            //游戏关卡.
                            if (InputEventCtrl.GetInstance() != null)
                            {
                                InputEventCtrl.GetInstance().ClickShaCheBt(btState);
                            }
                        }
                        break;
                    }
            }
        }
        
        /// <summary>
        /// 收到玩家退出手柄消息.
        /// </summary>
        void OnReceivedPlayerExit(string[] args)
        {
            //WEBSESSION_CLOSE,374b1b26-ea3c-4669-aaca-7e42dc799c0e,ClientClosing
            if (args.Length < 3)
            {
                return;
            }

            string huiHuaId = args[1];
            PlayerData playerDt = FindGamePlayerData(huiHuaId);
            if (playerDt != null)
            {
                //删除玩家数据信息.
                RemoveGamePlayerData(playerDt.userId);
            }
        }

        /// <summary>
        /// 收到游戏二维码数据消息.
        /// </summary>
        void OnReceivedGameErWeiMaMsg(string[] args)
        {
            //二维码信息返回.
            try
            {
                if (args.Length > 1)
                {
                    byte[] buffer = dll_MainLib.GC_GetQRCodeBitmap(args[1], 100, 100);
                    OnReceivedErWeiMaData(buffer);
                    m_ServerNetState = ServerNetState.CONNECTED_SERVER;

                    /*if (SSUIRoot.GetInstance().m_GameUIManage != null && SSUIRoot.GetInstance().m_GameUIManage.m_WangLuoGuZhangUI != null)
                    {
                        //删除网络故障UI界面.
                        SSUIRoot.GetInstance().m_GameUIManage.RemoveWangLuoGuZhangUI();
                    }*/
                }
                else
                {
                    SSDebug.LogWarning("OnReceivedGameErWeiMaMsg -> args was wrong!");
                }
            }
            catch (Exception e)
            {

                SSDebug.LogError(e.Message);
            }
        }

        void Awake()
        {
            if (IsUseFTServerPad == true)
            {
                if (_Instance == null)
                {
                    _Instance = this;
                    Init();
                    DontDestroyOnLoad(gameObject);
                }
            }
        }

        void InitInfo()
        {
            SSDebug.Log("FTServerManage::InitInfo.......................");
            if (MsgList == null)
            {
                MsgList = new List<string>();
                StartCoroutine(LoopCheckMsg());
            }
        }

        /// <summary>
        /// 循环询问消息池.
        /// </summary>
        IEnumerator LoopCheckMsg()
        {
            do
            {
                ShowText();
                yield return new WaitForSeconds(0.008f);
            }
            while (true);
        }

        /// <summary>
        /// 循环询问消息池.
        /// </summary>
        //void FixedUpdate()
        //{
        //    ShowText();
        //}

        void Update()
        {
            //检测服务器连接状态.
            CheckFTServerLinkState();
            //检测发送心跳消息.
            CheckSendXinTiaoMsg();
        }

        class XinTiaoMsgData
        {
            float timeLast = 0f;
            /// <summary>
            /// 获取是否可以发送心跳消息.
            /// </summary>
            internal bool GetIsCanSendXinTiaoMsg()
            {
                bool isSend = false;
                if (Time.time - timeLast >= 30f)
                {
                    timeLast = Time.time;
                    isSend = true;
                }
                return isSend;
            }
        }
        /// <summary>
        /// 心跳消息数据.
        /// </summary>
        XinTiaoMsgData m_XinTiaoMsgDt = new XinTiaoMsgData();
        /// <summary>
        /// 检测发送心跳消息.
        /// </summary>
        void CheckSendXinTiaoMsg()
        {
            if (m_XinTiaoMsgDt != null)
            {
                if (m_XinTiaoMsgDt.GetIsCanSendXinTiaoMsg() == true)
                {
                    //发送心跳消息.
                    SendXinTiaoMsg();
                }
            }
        }

        /// <summary>
        /// 测试玩家输入的数据信息.
        /// </summary>
        void CheckPlayerInputInfo()
        {
            if (m_LoginPlayerDt != null && m_LoginPlayerDt.Count > 0)
            {
                for (int i = 0; i < m_LoginPlayerDt.Count; i++)
                {
                    if (m_LoginPlayerDt[i].isPlayGame == true)
                    {
                        string info = m_LoginPlayerDt[i].GetPlayerActionInfo();
                        GUI.Box(new Rect(15f, 30f + (i * 25f), 500f, 25f), "");
                        GUI.Label(new Rect(15f, 30f + (i * 25f), 500f, 25f), info);
                    }
                }
            }
        }

#if SHOW_PLAYER_PAD_INFO //显示玩家手柄操作信息.
        private void OnGUI()
        {
            CheckPlayerInputInfo();
        }
#endif

        /// <summary>
        /// 收到二位码数据.
        /// </summary>
        void OnReceivedErWeiMaData(byte[] buffer)
        {
            if (buffer != null && buffer.Length > 1)
            {
                //将二维码像素列表信息转换为图片.
                Texture2D tx = new Texture2D(100, 100, TextureFormat.ARGB32, false);
                tx.LoadImage(buffer);
                OnErWeiMaLoad(tx);
                buffer = null;

                if (img != null)
                {
                    //二维码测试.
                    img.sprite = Sprite.Create(tx, new Rect(0, 0, tx.width, tx.height), new Vector2(0, 0));
                    m_ErWeiMaSprite = img.sprite;
                }
            }
        }
        
        public void ShowText()
        {
            if (MsgList.Count > 0)
            {
                string s = MsgList[0];
                ShowMsg(s);
                MsgList.RemoveAt(0);
            }

            List<PlayerData> listPlayerDt = m_LoginPlayerDt;
            if (listPlayerDt.Count > 0)
            {
                for (int i = 0; i < listPlayerDt.Count; i++)
                {
                    if (listPlayerDt[i].directionMsg != "")
                    {
                        //方向信息.
                        ShowMsg(listPlayerDt[i].directionMsg);
                        //DATA,374b1b26-ea3c-4669-aaca-7e42dc799c0e,move,-32,109
                        string[] sagr = listPlayerDt[i].directionMsg.Split(',');
                        if (sagr.Length >= 5)
                        {
                            string px = sagr[3].Length >= 6 ? sagr[3].Substring(0, 6) : sagr[3];
                            string py = sagr[4].Length >= 6 ? sagr[4].Substring(0, 6) : sagr[4];
                            listPlayerDt[i].directionInfo = px + ", " + py;
                        }
                        listPlayerDt[i].directionMsg = "";
                    }

                    if (listPlayerDt[i].buttonMsg != "")
                    {
                        //按键信息.
                        ShowMsg(listPlayerDt[i].buttonMsg);
                        if (listPlayerDt.Count > i && listPlayerDt[i] != null)
                        {
                            if (listPlayerDt[i].buttonCount >= 999)
                            {
                                listPlayerDt[i].buttonCount = 0;
                            }
                            listPlayerDt[i].buttonCount++;
                            listPlayerDt[i].buttonMsg = "";
                        }
                    }
                }
            }
        }

#region 服务器消息管理.
        /// <summary>
        /// 纷腾服务器消息接口.
        /// </summary>
        internal FTServerInterface m_FTServerInterface;
        /// <summary>
        /// 创建纷腾服务器消息接口组件.
        /// </summary>
        void CreatFTServerInterface()
        {
            SSDebug.Log("CreatFTServerInterface+++++++++++++++++++++++++++++++++++");
            if (m_FTServerInterface == null)
            {
                GameObject obj = new GameObject("_FTServerInterface");
                m_FTServerInterface = obj.AddComponent<FTServerInterface>();
            }
        }

        /// <summary>
        /// 当二维码加载之后.
        /// </summary>
        void OnErWeiMaLoad(Texture2D val)
        {
            if (m_FTServerInterface != null)
            {
                SSDebug.Log("OnErWeiMaLoad -> loaded erWeiMa..........................");
                m_FTServerInterface.OnErWeiMaLoad(val);
            }
            else
            {
                SSDebug.LogWarning("OnErWeiMaLoad -> m_FTServerInterface was null");
            }
        }
#endregion
    }
}

enum PlayerEnum
{
	Null = 0,
	PlayerOne = 1,
}
