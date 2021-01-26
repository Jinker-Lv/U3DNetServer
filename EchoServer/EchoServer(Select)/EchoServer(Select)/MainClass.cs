using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace EchoServer_Select_
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
        static void Main(string[] args)
        {
            //Socket
            listenfd = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //Bind
            IPAddress ipAdr = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipEp = new IPEndPoint(ipAdr, 8888);
            listenfd.Bind(ipEp);
            //Listen
            listenfd.Listen(0);
            Console.WriteLine("[服务器]启动成功");
            //checkRead
            List<Socket> checkRead = new List<Socket>();
            //主循环
            while (true)
            {
                //填充checkRead列表
                checkRead.Clear();
                checkRead.Add(listenfd);
                foreach (ClientState s in clients.Values)
                {
                    checkRead.Add(s.socket);
                }
                //Select
                Socket.Select(checkRead, null, null, 1000);
                //检查可读对象
                foreach (Socket s in checkRead)
                {
                    if (s == listenfd)
                    {
                        ReadListenfd(s);
                    }
                    else
                    {
                        ReadClientfd(s);
                    }
                }
                System.Threading.Thread.Sleep(1);
            }
        }
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
            catch (SocketException ex)
            {
                clientfd.Close();
                clients.Remove(clientfd);
                Console.WriteLine($"Receive SocketException: {ex}");
                return false;
            }
            //关闭客户端
            if (count == 0)
            {
                clientfd.Close();
                clients.Remove(clientfd);
                Console.WriteLine("Socket Close");
                return false;
            }
            //广播
            string recvStr = System.Text.Encoding.Default.GetString(state.readBuff, 0, count);
            Console.WriteLine($"Receive {recvStr}");
            string sendStr = recvStr;
            byte[] sendByte = System.Text.Encoding.Default.GetBytes(sendStr);
            foreach (ClientState cs in clients.Values)
            {
                cs.socket.Send(sendByte);
            }
            return true;
        }
    }
}
