using GameSeverApp.DBModel;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace GameServerApp
{
    class Program
    {
        //服务器IP
        private static string m_ServerIP = "192.168.50.253";
        //服务器端口
        private static int m_Port = 1011;
        // 服务器监听socket      
        private static Socket m_ServerSocket;

        static void Main(string[] args)
        {
            MailDBModel.Instance.Init();
            //实例化soket
            m_ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //绑定，向操作系统申请一个可用的IP和的端口用来通讯
            m_ServerSocket.Bind(new IPEndPoint(IPAddress.Parse(m_ServerIP), m_Port));
            //监听数量，0默认无限
            m_ServerSocket.Listen(50000);

            Console.WriteLine("服务器监听启动成功{0}", m_ServerSocket.LocalEndPoint.ToString());
            //定义一个线程
            Thread m_Thread = new Thread(ListenClientCallBack);
            //开启线程
            m_Thread.Start();
            Console.ReadLine();



            
        }

        /// <summary>
        /// 监听客户端连接
        /// </summary>
        /// <param name="obj"></param>
        private static void ListenClientCallBack()
        {

            while (true)
            {
                //接收客户端请求
                Socket socket = m_ServerSocket.Accept();


                Console.WriteLine("客户端{0}已经连接", socket.RemoteEndPoint.ToString());

                //一个角色就相当于一个客户端
                Role role = new Role();
                //实例化客户端连接
                ClientSocket clientSocket = new ClientSocket(socket, role);

                //把角色添加高角色管理类账本里
                RoleMgr.Instance.AllRole.Add(role);

            }
        }
    }
}
