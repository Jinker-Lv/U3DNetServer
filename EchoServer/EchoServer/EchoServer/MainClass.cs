using System;
using System.Net.Sockets;
using System.Net;
using System.Collections.Generic;

namespace EchoServer
{
    class ClientState
    {
        public Socket socket;
        public byte[] readBuff = new byte[1024];
    }
    class MainClass
    {
        //监听Socket
        static Socket listenfd;
        //客户端Socket及状态信息
        static Dictionary<Socket, ClientState> clients = new Dictionary<Socket, ClientState>();

        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //Socket
            listenfd = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //Bind
            IPAddress ipAdr = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipEp = new IPEndPoint(ipAdr, 8888);
            listenfd.Bind(ipEp);
            //Listen
            listenfd.Listen(0);
            Console.WriteLine("[服务器]启动成功");
            //Accept
            //listenfd.BeginAccept(AcceptCallback, listenfd);
            //等待
            //Console.ReadLine();
            #region

            while (true)
            {
                //检查listenfd
                if (listenfd.Poll(0, SelectMode.SelectRead))
                {
                    ReadListenfd(listenfd);
                }
                //检查clientfd
                foreach(ClientState s in clients.Values)
                {
                    Socket clientfd = s.socket;
                    if (clientfd.Poll(0, SelectMode.SelectRead))
                    {
                        if (!ReadClientfd(clientfd))
                        {
                            break;
                        }
                    }
                }
                //防止CPU占用过高
                System.Threading.Thread.Sleep(1);

                /*
                //Accept
                Socket connfd = listenfd.Accept();
                Console.WriteLine("[服务器]Accept");
                //Receive
                byte[] readBuff = new byte[1024];
                int count = connfd.Receive(readBuff);
                string readStr = System.Text.Encoding.Default.GetString(readBuff, 0, count);
                Console.WriteLine($"[服务器接收]{readStr}");
                //send
                string sendStr = System.DateTime.Now.ToString();
                byte[] sendBytes = System.Text.Encoding.Default.GetBytes(sendStr);
                connfd.Send(sendBytes); 
                */
            }

            #endregion
        }
        #region
        /*
        public static void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                Console.WriteLine("[服务器]Accept");
                Socket listenfd = (Socket)ar.AsyncState;
                Socket clientfd = listenfd.EndAccept(ar);
                //client列表
                ClientState state = new ClientState();
                state.socket = clientfd;
                clients.Add(clientfd, state);
                //接受数据BeginReceive
                clientfd.BeginReceive(state.readBuff, 0, 1024, 0, ReceiveCallback, state);
                //继续Accept
                listenfd.BeginAccept(AcceptCallback, listenfd);
            }
            catch(SocketException ex)
            {
                Console.WriteLine($"Socket Accept fail :{ex}");
            }
        }
        public static void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                ClientState state = (ClientState)ar.AsyncState;
                Socket clientfd = state.socket;
                int count = clientfd.EndReceive(ar);
                //客户端关闭
                if (count == 0)
                {
                    clientfd.Close();
                    clients.Remove(clientfd);
                    Console.WriteLine("Socket Close");
                    return;
                }
                string recvStr = System.Text.Encoding.Default.GetString(state.readBuff, 0, count);
                string sendStr = clientfd.RemoteEndPoint.ToString() + ":" + recvStr;
                byte[] sendBytes = System.Text.Encoding.Default.GetBytes($"echo {sendStr}");
                foreach(ClientState s in clients.Values)
                {
                    s.socket.Send(sendBytes);
                }
                //减少代码量，不使用异步
                clientfd.BeginReceive(state.readBuff, 0, 1024, 0, ReceiveCallback, state);
            }
            catch(SocketException ex)
            {
                Console.WriteLine($"Socket Receive fail: {ex}");
            }
        }
        */
        #endregion
        public static void ReadListenfd(Socket listenfd)
        {
            Console.WriteLine("Accept");
            Socket clientfd = listenfd.Accept();
            ClientState state = new ClientState();
            state.socket = clientfd;
            clients.Add(clientfd, state);
        }
        //读取Clientfd
        public static bool ReadClientfd(Socket clientfd)
        {
            ClientState state = clients[clientfd];
            //接受
            int count = 0;
            try
            {
                count = clientfd.Receive(state.readBuff);
            }
            catch(SocketException ex)
            {
                clientfd.Close();
                clients.Remove(clientfd);
                Console.WriteLine($"Receive SocketException: {ex}");
                return false;
            }
            //关闭客户端
            if(count == 0)
            {
                clientfd.Close();
                clients.Remove(clientfd);
                Console.WriteLine("Socket Close");
                return false;
            }
            //广播
            string recvStr = System.Text.Encoding.Default.GetString(state.readBuff, 0, count);
            Console.WriteLine($"Receive {recvStr}");
            string sendStr = clientfd.RemoteEndPoint.ToString() + ":" + recvStr;
            byte[] sendByte = System.Text.Encoding.Default.GetBytes(sendStr);
            foreach(ClientState cs in clients.Values)
            {
                cs.socket.Send(sendByte);
            }
            return true;
        }
    }
}
