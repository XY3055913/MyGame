using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
/// <summary>
/// Socket 通讯管理
/// </summary>
public class NetWorkSocket : MonoBehaviour
{
    #region 单例
    private static NetWorkSocket instance;
    public static NetWorkSocket Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("NetWorkSocket");

                DontDestroyOnLoad(obj);
                instance = obj.GetOrCreatComponent<NetWorkSocket>();
            }
            return instance;
        }
    }
    #endregion

    //客户端Socket
    private Socket m_Client;

    //缓冲区
    private byte[] buffer = new byte[2048];
    //数据包压缩长度界限
    private const int m_CompressLen = 200;

    #region 发送消息所需变量
    //发送消息队列
    private Queue<byte[]> m_SendQueue = new Queue<byte[]>();

    //检查队列的委托
    private Action m_CheckSendQueue;
    #endregion


    #region 接收消息所需变量
    //接收到的数据放到缓冲区里
    private byte[] m_ReceiveBuffer = new byte[10240];

    //接收数据包的缓冲数据流
    private MMO_MemoryStream m_ReceiveMS = new MMO_MemoryStream();
    //接收消息的队列
    private Queue<byte[]> m_ReceiveQueue = new Queue<byte[]>();

    private int m_ReceiveCount = 0;

    #endregion

    #region 连接到服务器
    /// <summary>
    /// 连接Socket服务器
    /// </summary>
    /// <param name="ip">IP</param>
    /// <param name="port">端口号</param>
    public void Connect(string ip,int port)
    {
        //如果客户端Socket已存在 且处于连接状态，就返回出去
        if (m_Client != null && m_Client.Connected) return;
        //连接方式
        m_Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        //尝试连接
        try
        {
            m_Client.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
            //连接成功监听委托
            m_CheckSendQueue = OnCheckSendQueueCallBack;
            Debug.Log("与服务器连接成功");

            //连接成功后就开始收消息了
            ReceiveMsg();
           
        }
        catch (Exception ex)
        {

            Debug.Log("连接失败=" + ex.ToString()) ;
        }
    }
    #endregion

    //--------------发送消息--------------------

    #region OnCheckSendQueueCallBack检查队列的委托回调
    /// <summary>
    /// 检查队列的委托回调
    /// </summary>
    private void OnCheckSendQueueCallBack()
    {
        lock (m_SendQueue)
        {
            //如果队列中有数据包，则发送数据包
            if (m_SendQueue.Count>0)
            {
                //发送数据包,从队列里取出一个进行发送
                Send(m_SendQueue.Dequeue());
            }

        }
    }
    #endregion

    #region 封装数据包
    /// <summary>
    /// 封装数据包
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private byte[] MakeData(byte[] data)
    {
        byte[] retBuffer = null;

        //1.长度压缩判断
        bool isCompress = data.Length > m_CompressLen ? true : false;
        if (isCompress)
        {
            //2.进行压缩
            data = ZlibHelper.CompressBytes(data);
        }

        //3.异或
        data = SecurityUtil.Xor(data);
        //4.异或因子 Crc 校验码
        ushort crc = Crc16.CalculateCrc16(data);

     
    
        //写入
        using (MMO_MemoryStream ms = new MMO_MemoryStream())
        {
            ms.WriteUShort((ushort)(data.Length + 3));//数据长度
            ms.WriteBool(isCompress);//压缩判断
            ms.WriteUShort(crc);//异或
            ms.Write(data, 0, data.Length);//数据写入
            retBuffer = ms.ToArray();
        }
        return retBuffer;
    }
    #endregion



    #region 发送消息 把消息加入队列
    /// <summary>
    /// 发送消息
    /// </summary>
    public void SendMsg(byte[] buffer) 
    {      
        //得到封装后的数据包
        byte[] sendBuffer = MakeData(buffer);

        lock (m_SendQueue)//锁一下，防止并发
        {
            //把数据包加入队列
            m_SendQueue.Enqueue(sendBuffer);
            //启动检查队列的委托(执行委托)，开启后会监听委托中的回调OnCheckSendQueueCallBack
            m_CheckSendQueue.BeginInvoke(null, null);
        }
    }

    #endregion
    #region Send 真正发送数据包到服务器
    /// <summary>
    /// 真正发送数据包到服务器
    /// </summary>
    /// <param name="buffer"></param>
    private void Send(byte[] buffer) 
    {
        m_Client.BeginSend(buffer,0,buffer.Length,SocketFlags.None,SengCallBack, m_Client);  
    }
    #endregion
    #region SengCallBack发送数据包的回调
    /// <summary>
    /// 发送数据包的回调
    /// </summary>
    /// <param name="ar"></param>
    private void SengCallBack(IAsyncResult ar)
    {
        m_Client.EndSend(ar);
        //继续检查队列
        OnCheckSendQueueCallBack();
        
    }
    #endregion

    //-----------------接收消息-------------------------

    #region ReceiveMsg 接收数据

    /// <summary>
    /// 接收数据
    /// </summary>
    /// <param name="obj"></param>
    private void ReceiveMsg()
    {
        //异步接收数据   
        //参数(缓冲区，偏移，长度，SocketFlags.None，ReceiveCallBack（回调方法），回调参数)   
        m_Client.BeginReceive(m_ReceiveBuffer, 0, m_ReceiveBuffer.Length, SocketFlags.None, ReceiveCallBack, m_Client);
    }
    #endregion

    #region ReceiveCallBack 接收数据的回调

    //接收数据的回调
    private void ReceiveCallBack(IAsyncResult ar)
    {
        try
        {
            //接收了多少(字节)条数据
            int len = m_Client.EndReceive(ar);
            if (len > 0)
            {
                //已接收到数据

                //把接收到的数据 写入缓冲数据流的尾部
                m_ReceiveMS.Position = m_ReceiveMS.Length;
                //把指定长度的字节 写入数据流
                m_ReceiveMS.Write(m_ReceiveBuffer, 0, len);

                //如果缓存数据流的长度>2,说明有至少有两个不完整的包发过来了
                //为什么是2，因为 客户端封装的数据包用的是ushort 长度就是2
                if (m_ReceiveMS.Length > 2)
                {
                    //进行循环，拆包
                    while (true)
                    {
                        //把数据流指针位置放0处
                        m_ReceiveMS.Position = 0;
                        //curMsgLen =包体的长度
                        int curMsgLen = m_ReceiveMS.ReadUShort();
                        //总包的长度
                        int curFullMsgLen = 2 + curMsgLen;
                        //如果数据流的长度>=整体的长度  说明至少收到了一个完整包
                        if (m_ReceiveMS.Length >= curFullMsgLen)
                        {
                            //至少收到一个完整包
                            //定义包体的byte[]数组
                            byte[] buffer = new byte[curMsgLen];

                            //把数据流指针放到2的位置,也就是包体开头的位置
                            m_ReceiveMS.Position = 2;
                            //把包体读到byte[]数组
                            m_ReceiveMS.Read(buffer, 0, curMsgLen);

                         
                            lock (m_ReceiveQueue)
                            {
                                m_ReceiveQueue.Enqueue(buffer);
                            }
                

                            //---------------------处理剩余字节数组------------------------------
                            int remainLen = (int)m_ReceiveMS.Length - curFullMsgLen;
                            if (remainLen > 0)//如果有剩余字节
                            {
                                //把指针放在第一个包的尾部
                                m_ReceiveMS.Position = curFullMsgLen;
                                //定义剩余字节数组
                                byte[] remainBuffer = new byte[remainLen];
                                //把数据读到剩余字节数组
                                m_ReceiveMS.Read(remainBuffer, 0, remainLen);
                                //清空数据流
                                m_ReceiveMS.Position = 0;
                                m_ReceiveMS.SetLength(0);
                                //把剩余的字节数组重新写入数据流
                                m_ReceiveMS.Write(remainBuffer, 0, remainBuffer.Length);
                                remainBuffer = null;
                            }
                            else //没有剩余字节
                            {
                                //清空数据流
                                m_ReceiveMS.Position = 0;
                                m_ReceiveMS.SetLength(0);
                                break;
                            }
                        }
                        else
                        {
                            //还没收到完整包
                            break;
                        }

                    }
                }
                //进行下一次接收数据包
                ReceiveMsg();
            }
            else //len=0
            {
                //服务器断开连接
                Debug.Log(string.Format("服务器{0}断开连接1", m_Client.RemoteEndPoint.ToString()));
            
            }
        }
        catch
        {
            //服务器断开连接
            Debug.Log(string.Format("服务器{0}断开连接2", m_Client.RemoteEndPoint.ToString()));
       
          
        }
    }
    #endregion




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        #region 从队列中获取数据
        while (true)
        {
            if (m_ReceiveCount <= 5)
            {
                m_ReceiveCount++;
                lock (m_ReceiveQueue)
                {
                    if (m_ReceiveQueue.Count > 0)
                    {
                        //buffer 包体(队列中的数据包)
                        byte[] buffer = m_ReceiveQueue.Dequeue();
                        //这是异或之后的数组
                        byte[] bufferNew = new byte[buffer.Length - 3];

                        bool isCompress = false;

                        ushort crc = 0;

                        using (MMO_MemoryStream ms = new MMO_MemoryStream(buffer))
                        {
                            isCompress = ms.ReadBool();
                            crc = ms.ReadUShort();
                            ms.Read(bufferNew,0, bufferNew.Length);
                           
                        }

                        //校验Crc
                        int newCrc = Crc16.CalculateCrc16(bufferNew);
                        Debug.Log("传递Crc=" + crc);
                        Debug.Log("本地newCrc=" + newCrc);
                        if (newCrc== crc)//本地Crc与数据包的进行校验
                        {
                            //异或 得到原始数据
                            bufferNew = SecurityUtil.Xor(bufferNew);

                            if (isCompress)//如果压缩了
                            {
                                //进行解压
                                bufferNew = ZlibHelper.DeCompressBytes(bufferNew);
                            }

                            ushort protoCode = 0;

                            byte[] protoContent = new byte[bufferNew.Length-2];
                            using (MMO_MemoryStream ms = new MMO_MemoryStream(bufferNew))
                            {
                                //读 协议编号
                                protoCode = ms.ReadUShort();
                                //读内容
                                ms.Read(protoContent, 0, protoContent.Length);

                                //派发协议编号和内容
                                EventDispatcher.Instance.Dispath(protoCode, protoContent);
                            }
                        }
                        else
                        {
                            break;
                        }


                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                m_ReceiveCount = 0;
                break;
            }


        }
        #endregion

    }



    //断开连接
    private void OnDestroy()
    {
        if (m_Client != null && m_Client.Connected)
        {
            m_Client.Shutdown(SocketShutdown.Both);
            m_Client.Close();
        }


    }








}
