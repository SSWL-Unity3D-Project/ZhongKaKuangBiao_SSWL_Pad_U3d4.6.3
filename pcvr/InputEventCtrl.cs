using UnityEngine;
using System.Collections;

public class InputEventCtrl : MonoBehaviour {

	public static bool IsClickFireBtDown;
	public static uint SteerValCur;
	public static uint TaBanValCur;
	static private InputEventCtrl Instance = null;
	static public InputEventCtrl GetInstance()
	{
		if(Instance == null)
		{
			GameObject obj = new GameObject("_InputEventCtrl");
			Instance = obj.AddComponent<InputEventCtrl>();
			pcvr.GetInstance();
		}
		return Instance;
	}
    
    #region TV Button Event
    /// <summary>
    /// 电视遥控器按键消息.
    /// </summary>
    public delegate void EventHandelTV(ButtonState val);
    public event EventHandelTV ClickTVYaoKongExitBtEvent;
    /// <summary>
    /// 电视遥控器退出按键响应.
    /// </summary>
    public void ClickTVYaoKongExitBt(ButtonState val)
    {
        if (ClickTVYaoKongExitBtEvent != null)
        {
            ClickTVYaoKongExitBtEvent(val);
        }
    }

    public event EventHandelTV ClickTVYaoKongEnterBtEvent;
    /// <summary>
    /// 电视遥控器确定按键响应.
    /// </summary>
    public void ClickTVYaoKongEnterBt(ButtonState val)
    {
        if (ClickTVYaoKongEnterBtEvent != null)
        {
            ClickTVYaoKongEnterBtEvent(val);
        }
    }

    public event EventHandelTV ClickTVYaoKongLeftBtEvent;
    /// <summary>
    /// 电视遥控器方向左按键响应.
    /// </summary>
    public void ClickTVYaoKongLeftBt(ButtonState val)
    {
        if (ClickTVYaoKongLeftBtEvent != null)
        {
            ClickTVYaoKongLeftBtEvent(val);
        }
    }

    public event EventHandelTV ClickTVYaoKongRightBtEvent;
    /// <summary>
    /// 电视遥控器方向右按键响应.
    /// </summary>
    public void ClickTVYaoKongRightBt(ButtonState val)
    {
        if (ClickTVYaoKongRightBtEvent != null)
        {
            ClickTVYaoKongRightBtEvent(val);
        }
    }

    public event EventHandelTV ClickTVYaoKongUpBtEvent;
    /// <summary>
    /// 电视遥控器方向上按键响应.
    /// </summary>
    public void ClickTVYaoKongUpBt(ButtonState val)
    {
        if (ClickTVYaoKongUpBtEvent != null)
        {
            ClickTVYaoKongUpBtEvent(val);
        }
    }

    public event EventHandelTV ClickTVYaoKongDownBtEvent;
    /// <summary>
    /// 电视遥控器方向下按键响应.
    /// </summary>
    public void ClickTVYaoKongDownBt(ButtonState val)
    {
        if (ClickTVYaoKongDownBtEvent != null)
        {
            ClickTVYaoKongDownBtEvent(val);
        }
    }

    class KeyCodeTV
    {
        /// <summary>
        /// 遥控器确定键的键值.
        /// </summary>
        public static KeyCode PadEnter01 = (KeyCode)10;
        public static KeyCode PadEnter02 = (KeyCode)66;
    }
    #endregion

    #region Click Button Envent
    public delegate void EventHandel(ButtonState val);
	public event EventHandel ClickInsertcoinBtEvent;
	public void ClickInsertcoinBt(ButtonState val)
	{
		if(ClickInsertcoinBtEvent != null)
		{
			ClickInsertcoinBtEvent( val );
		}
	}

	public event EventHandel ClickStartBtOneEvent;
	public void ClickStartBtOne(ButtonState val)
	{
		if(ClickStartBtOneEvent != null)
		{
			ClickStartBtOneEvent( val );
			pcvr.StartBtLight = StartLightState.Mie;
		}
	}

	public event EventHandel ClickCloseDongGanBtEvent;
	public void ClickCloseDongGanBt(ButtonState val)
	{
		if(ClickCloseDongGanBtEvent != null)
		{
			ClickCloseDongGanBtEvent( val );
		}
	}
	
	public event EventHandel ClickPlayerYouMenBtEvent;
	public void ClickPlayerYouMenBt(ButtonState val)
	{
		if(ClickPlayerYouMenBtEvent != null)
		{
			ClickPlayerYouMenBtEvent( val );
		}
	}

	public event EventHandel ClickSetEnterBtEvent;
	public void ClickSetEnterBt(ButtonState val)
	{
		if(ClickSetEnterBtEvent != null)
		{
			ClickSetEnterBtEvent( val );
		}
	}

	public event EventHandel ClickSetMoveBtEvent;
	public void ClickSetMoveBt(ButtonState val)
	{
		if(ClickSetMoveBtEvent != null)
		{
			ClickSetMoveBtEvent( val );
		}
	}
	
	public event EventHandel ClickShaCheBtEvent;
	public void ClickShaCheBt(ButtonState val)
	{//Debug.Log("shacheeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee sss down pcvr");
		if(ClickShaCheBtEvent != null)
		{//Debug.Log("shacheeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeff down pcvr");
			ClickShaCheBtEvent( val );
		}
	}
	
	public event EventHandel ClickLaBaBtEvent;
	public void ClickLaBaBt(ButtonState val)
	{
		if(ClickLaBaBtEvent != null)
		{
			ClickLaBaBtEvent( val );
		}
	}
	
	public event EventHandel ClickChangeCameraBtEvent;
	public void ClickChangeCameraBt(ButtonState val)
	{
		if(ClickChangeCameraBtEvent != null)
		{
			ClickChangeCameraBtEvent( val );
		}
	}
	#endregion

	void Update()
	{
        UpdatePlayerDirectionToZero();
        if (pcvr.bIsHardWare)
        {
            if (Input.GetKeyUp(KeyCode.Space) && pcvr.bPlayerStartKeyDown)
			{pcvr.bPlayerStartKeyDown = false;
				ClickStartBtOne( ButtonState.UP );
			}
			
			if(Input.GetKeyDown(KeyCode.Space) && !pcvr.bPlayerStartKeyDown)
			{pcvr.bPlayerStartKeyDown = true;Debug.Log("aaaaaaaaaaaaaaaaaaafffff");
				ClickStartBtOne( ButtonState.DOWN );
			}
			
			if(Input.GetKeyUp(KeyCode.T))
			{
				ClickInsertcoinBt( ButtonState.UP );
			}
			
			if(Input.GetKeyDown(KeyCode.T))
			{
				pcvr.coinCurNumPCVR ++;
				ClickInsertcoinBt( ButtonState.DOWN );
			}
			
			if(Input.GetKeyUp(KeyCode.B))
			{
				ClickChangeCameraBt( ButtonState.UP );
			}
			
			if(Input.GetKeyDown(KeyCode.B))
			{
				ClickChangeCameraBt( ButtonState.DOWN );
			}
			return;
        }

        //(KeyCode)10 -> acbox虚拟机的遥控器确定键消息.
        if (Input.GetKeyDown(KeyCode.KeypadEnter)
            || Input.GetKeyDown(KeyCode.Return)
            || Input.GetKeyDown(KeyCodeTV.PadEnter01)
            || Input.GetKeyDown(KeyCodeTV.PadEnter02)
            || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            //遥控器的确定键消息.
            ClickTVYaoKongEnterBt(ButtonState.DOWN);
            ClickStartBtOne(ButtonState.DOWN);
            ClickChangeCameraBt(ButtonState.DOWN);
        }

        if (Input.GetKeyUp(KeyCode.KeypadEnter)
            || Input.GetKeyUp(KeyCode.Return)
            || Input.GetKeyUp(KeyCodeTV.PadEnter01)
            || Input.GetKeyUp(KeyCodeTV.PadEnter02)
            || Input.GetKeyUp(KeyCode.JoystickButton0))
        {
            //遥控器的确定键消息.
            ClickTVYaoKongEnterBt(ButtonState.UP);
            ClickStartBtOne(ButtonState.UP);
            ClickChangeCameraBt(ButtonState.UP);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //接收遥控器的返回键/键盘上的Esc按键信息.
            ClickTVYaoKongExitBt(ButtonState.DOWN);
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            //接收遥控器的返回键/键盘上的Esc按键信息.
            ClickTVYaoKongExitBt(ButtonState.UP);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.Keypad4))
        {
            //接收遥控器/键盘上的向左按键信息.
            ClickTVYaoKongLeftBt(ButtonState.DOWN);
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.Keypad4))
        {
            //接收遥控器/键盘上的向左按键信息.
            ClickTVYaoKongLeftBt(ButtonState.UP);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Keypad6))
        {
            //接收遥控器/键盘上的向右按键信息.
            ClickTVYaoKongRightBt(ButtonState.DOWN);
        }

        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.Keypad6))
        {
            //接收遥控器/键盘上的向右按键信息.
            ClickTVYaoKongRightBt(ButtonState.UP);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            //接收遥控器/键盘上的向上按键信息.
            ClickTVYaoKongUpBt(ButtonState.DOWN);
        }

        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.Keypad2))
        {
            //接收遥控器/键盘上的向上按键信息.
            ClickTVYaoKongUpBt(ButtonState.UP);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.Keypad8))
        {
            //接收遥控器/键盘上的向下按键信息.
            ClickTVYaoKongDownBt(ButtonState.DOWN);
        }

        if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.Keypad8))
        {
            //接收遥控器/键盘上的向下按键信息.
            ClickTVYaoKongDownBt(ButtonState.UP);
        }
        //---------------------------------------------------------------------------------//

        //if (Input.GetKeyUp(KeyCode.T))
		//{
		//	ClickInsertcoinBt( ButtonState.UP );
		//}
		
		//if(Input.GetKeyDown(KeyCode.T))
		//{
		//	ClickInsertcoinBt( ButtonState.DOWN );
		//}

		//if(Input.GetKeyUp(KeyCode.E))
		//{
		//	ClickStartBtOne( ButtonState.UP );
		//}

		//if(Input.GetKeyDown(KeyCode.E))
		//{
		//	ClickStartBtOne( ButtonState.DOWN );
		//}

		//if(Input.GetKeyUp(KeyCode.P))
		//{
		//	ClickCloseDongGanBt( ButtonState.UP );
		//}
		
		//if(Input.GetKeyDown(KeyCode.P))
		//{
		//	ClickCloseDongGanBt( ButtonState.DOWN );
		//}

		//if(Input.GetKeyUp(KeyCode.W))
		//{
		//	ClickPlayerYouMenBt( ButtonState.UP );
		//}

		//if(Input.GetKeyDown(KeyCode.W))
		//{
		//	ClickPlayerYouMenBt( ButtonState.DOWN );
		//}

		//setPanel enter button
		//if(Input.GetKeyUp(KeyCode.F4))
		//{
		//	ClickSetEnterBt( ButtonState.UP );
		//}
		
		//if(Input.GetKeyDown(KeyCode.F4))
		//{
		//	ClickSetEnterBt( ButtonState.DOWN );
		//}

		//setPanel move button
		//if(Input.GetKeyUp(KeyCode.F5))
		//{
		//	ClickSetMoveBt( ButtonState.UP );
		//}
		
		//if(Input.GetKeyDown(KeyCode.F5))
		//{
		//	ClickSetMoveBt( ButtonState.DOWN );
		//}

		/*if(Input.GetKeyUp(KeyCode.Space))
		{
			ClickShaCheBt( ButtonState.UP );
		}
		
		if(Input.GetKeyDown(KeyCode.Space))
		{
			ClickShaCheBt( ButtonState.DOWN );
		}*/

		//if(Input.GetKeyUp(KeyCode.G))
		//{
		//	ClickLaBaBt( ButtonState.UP );
		//}

		//if(Input.GetKeyDown(KeyCode.G))
		//{
		//	ClickLaBaBt( ButtonState.DOWN );
		//}
		
		//if(Input.GetKeyUp(KeyCode.B))
		//{
		//	ClickChangeCameraBt( ButtonState.UP );
		//}
		
		//if(Input.GetKeyDown(KeyCode.B))
		//{
		//	ClickChangeCameraBt( ButtonState.DOWN );
		//}
	}

    /// <summary>
    /// 玩家真实方向.
    /// </summary>
    internal float m_PlayerRealDir = 0f;
    ButtonState DirBtLeft = ButtonState.UP;
    ButtonState DirBtRight = ButtonState.UP;
    public class PlayerDirectionData
    {
        internal float maxAngle = 200f;
        /// <summary>
        /// 自动归位最大时间.
        /// </summary>
        internal float maxTime = 0.35f;
        internal float speed = 0f;
        internal float lastTimeDir = 0f;
        internal bool IsDirectionToZero = false;
        public PlayerDirectionData()
        {
            speed = 1f / maxTime;
        }
    }
    internal PlayerDirectionData m_PlayerDirectionData = new PlayerDirectionData();

    void UpdatePlayerDirectionToZero()
    {
        if (m_PlayerDirectionData.IsDirectionToZero == true)
        {
            float dTime = Time.time - m_PlayerDirectionData.lastTimeDir;
            float dirVal = dTime * m_PlayerDirectionData.speed;
            if (m_PlayerRealDir != 0f)
            {
                if (Mathf.Abs(m_PlayerRealDir) >= dirVal)
                {
                    if (m_PlayerRealDir > 0f)
                    {
                        m_PlayerRealDir -= dirVal;
                    }
                    else
                    {
                        m_PlayerRealDir += dirVal;
                    }
                }
                else
                {
                    m_PlayerRealDir = 0f;
                    m_PlayerDirectionData.IsDirectionToZero = false;
                }
            }
            //SSDebug.Log("OnReceiveDirectionAngleMsgFTServer -> m_PlayerRealDir == " + m_PlayerRealDir);
        }
    }

    public void OnReceiveDirectionAngleMsgFTServer(float angle)
    {
        if (angle == 0f)
        {
            //m_PlayerRealDir = 0f;
            m_PlayerDirectionData.IsDirectionToZero = true;
        }
        else
        {
            m_PlayerDirectionData.IsDirectionToZero = false;
            m_PlayerDirectionData.lastTimeDir = Time.time;
            float maxAngle = m_PlayerDirectionData.maxAngle;
            if (Mathf.Abs(angle) > maxAngle)
            {
                angle = Mathf.Sign(angle) * maxAngle;
            }
            m_PlayerRealDir = Mathf.Sign(angle) * (Mathf.Abs(angle) / maxAngle);
        }
        //SSDebug.Log("OnReceiveDirectionAngleMsgFTServer -> m_PlayerRealDir == " + m_PlayerRealDir);

        ButtonState dirBtLeftVal = ButtonState.UP;
        ButtonState dirBtRightVal = ButtonState.UP;
        if (angle < 0f)
        {
            dirBtLeftVal = ButtonState.DOWN;
        }
        else if (angle > 0f)
        {
            dirBtRightVal = ButtonState.DOWN;
        }

        if (DirBtLeft != dirBtLeftVal)
        {
            DirBtLeft = dirBtLeftVal;
            ClickTVYaoKongLeftBt(DirBtLeft);
        }

        if (DirBtRight != dirBtRightVal)
        {
            DirBtRight = dirBtRightVal;
            ClickTVYaoKongRightBt(DirBtRight);
        }
    }
}

public enum ButtonState : int
{
	UP = 1,
	DOWN = -1
}