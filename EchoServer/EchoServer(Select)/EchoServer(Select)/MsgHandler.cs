using System;
using System.Collections.Generic;

namespace EchoServer_Select_
{
    class MsgHandler
    {
        public static void MsgEnter(ClientState c, string msgArgs) {
            //解析参数
            //string[] split = msgArgs.Split(',');
            //string desc = split[0];
            //float x = float.Parse(split[1]);
            //float y = float.Parse(split[2]);
            //float z = float.Parse(split[3]);
            //float eulY = float.Parse(split[4]);
            ////赋值
            //c.hp = 100;
            //c.y = y;
            //c.x = x;
            //c.z = z;
            //c.eulY = eulY;
            //string sendStr = "Enter|" + msgArgs;
            ////广播
            //Console.WriteLine("赋值成功了");
            //foreach(ClientState cs in MainClass.clients.Values)
            //{
            //    //TODO:
            //}
        }
        public static void MsgList(ClientState c, string msgArgs) => Console.WriteLine($"MsgList {msgArgs}");
    }
}
