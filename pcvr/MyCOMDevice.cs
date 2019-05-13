using UnityEngine;
using System.Collections;
using System.Threading;
using System;
using System.IO.Ports;
using System.Text;

public class MyCOMDevice : MonoBehaviour
{
    public class ComThreadClass
    {
        public static SerialPort _SerialPort;
        public static int BufLenRead = 35;
        public static int BufLenReadEnd = 4;
        public static int BufLenWrite = 30;
        public static byte[] ReadByteMsg = new byte[BufLenRead];
        public static byte[] WriteByteMsg = new byte[BufLenWrite];
        static string RxStringData;
        //"ABCD" ---> 0x41 0x42 0x43 0x44.
        public static byte[] NewLineArray = new byte[4] { 0x41, 0x42, 0x43, 0x44 };
        public static int ReadTimeout = 0x07d0;
        public static int WriteTimeout = 0x07d0;
        public static bool IsReadMsgComTimeOut;
        public static bool IsReadComMsg;
        /// <summary>
        /// set IsLoadingLevel is true when loading level, otherwise set IsLoadingLevel is false.
        /// </summary>
        public static bool IsLoadingLevel;
        public static bool IsGameOnQuit = false;
        public static int mBaudRate = 38400;
        public static int mDataBits = 8;
#if UNITY_STANDALONE_LINUX
        public static string ComPortName = "/dev/ttyS0";
#else
        // mo ren shi Windows ,Linux hui bao cuo.
        public static string ComPortName = "COM1";
#endif

        public static void OpenComPort()
        {
            if (_SerialPort != null)
            {
                return;
            }

            _SerialPort = new SerialPort(ComPortName, mBaudRate, Parity.None, mDataBits, StopBits.One);
            //ascii to string.
            System.Text.ASCIIEncoding ascii = new System.Text.ASCIIEncoding();
            _SerialPort.NewLine = ascii.GetString(NewLineArray);
            //Debug.Log("Com read newLine is "+_SerialPort.NewLine+"...");
            _SerialPort.Encoding = Encoding.GetEncoding("iso-8859-1");
            _SerialPort.ReadTimeout = ReadTimeout;
            _SerialPort.WriteTimeout = WriteTimeout;
            if (_SerialPort != null)
            {
                try
                {
                    if (_SerialPort.IsOpen)
                    {
                        _SerialPort.Close();
                        Debug.Log("Closing port, because it was already open!");
                    }
                    else
                    {
                        _SerialPort.Open();
                        if (_SerialPort.IsOpen)
                        {
                            Debug.Log(ComPortName + " open sucess");
                        }
                    }
                }
                catch (Exception exception)
                {
                    Debug.Log("error:COM already opened by other PRG... " + exception);
                }
            }
            else
            {
                Debug.Log("Port == null");
            }
        }

        public void Run()
        {
            do
            {
                COMTxData();
                COMRxData();
                Thread.Sleep(10);
            } while (ComThread != null);
            Debug.Log("Close run thead...");
        }

        public void COMTxData()
        {
            try
            {
                _SerialPort.Write(WriteByteMsg, 0, WriteByteMsg.Length);
            }
            catch (Exception exception)
            {
                Debug.Log("Tx error:COM -> " + exception);
            }
        }

        void COMRxData()
        {
            try
            {
                RxStringData = _SerialPort.ReadLine();
                ReadByteMsg = _SerialPort.Encoding.GetBytes(RxStringData);

                ReadMsgTimeOutVal = 0f;
                IsReadComMsg = true;
                IsFindDeviceDt = true;
            }
            catch (Exception exception)
            {
                Debug.Log("Rx error:COM -> " + exception);
            }
        }

        public static void CloseComPort()
        {
            if (_SerialPort == null || !_SerialPort.IsOpen)
            {
                return;
            }
            _SerialPort.Close();
            _SerialPort = null;
        }
    }

    static ComThreadClass _ComThreadClass;
    static Thread ComThread;
    public static bool IsFindDeviceDt;
    public static float ReadMsgTimeOutVal;
    static float TimeLastVal;
    const float TimeUnitDelta = 1f;
    public static uint CountRestartCom;
    static MyCOMDevice _Instance;
    public static MyCOMDevice GetInstance()
    {
        if (_Instance == null)
        {
            GameObject obj = new GameObject("_MyCOMDevice");
            DontDestroyOnLoad(obj);
            _Instance = obj.AddComponent<MyCOMDevice>();
        }
        return _Instance;
    }

    // Use this for initialization
    void Start()
    {
        Debug.Log("Try open " + ComThreadClass.ComPortName + "...");
        //TestInitMsg();
        StartCoroutine(OpenComThread());
    }

    IEnumerator OpenComThread()
    {
        ReadMsgTimeOutVal = 0f;
        ComThreadClass.IsReadMsgComTimeOut = false;
        if (_ComThreadClass == null)
        {
            _ComThreadClass = new ComThreadClass();
        }
        else
        {
            ComThreadClass.CloseComPort();
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(0.5f);
        ComThreadClass.OpenComPort();

        yield return new WaitForSeconds(0.5f);
        if (ComThread == null)
        {
            ComThread = new Thread(new ThreadStart(_ComThreadClass.Run));
            ComThread.Start();
        }
    }
    //void OnGUI()
    //{
    //    if (IsFindDeviceDt)
    //    {
    //        return;
    //    }
    //    string bufMsg = ComThreadClass.ComPortName + " -> SendMsg:";
    //    for (int i = 0; i < ComThreadClass.WriteByteMsg.Length; i++)
    //    {
    //        bufMsg += (" " + ComThreadClass.WriteByteMsg[i].ToString("X2"));
    //    }
    //    GUI.Label(new Rect(10f, 30f, Screen.width, 30f), bufMsg);

    //    string bufGetMsg = "GetMsg:";
    //    for (int i = 0; i < ComThreadClass.ReadByteMsg.Length; i++)
    //    {
    //        bufGetMsg += (" " + ComThreadClass.ReadByteMsg[i].ToString("X2"));
    //    }
    //    GUI.Label(new Rect(10f, 60f, Screen.width, 30f), bufGetMsg);
    //}

    void CloseComThread()
    {
        ComThread.Abort();
        ComThread = null;
    }

    void OnApplicationQuit()
    {
        Debug.Log("ComDevice::OnApplicationQuit...");
        ComThreadClass.IsGameOnQuit = true;
        ComThreadClass.CloseComPort();
        CloseComThread();
    }
}