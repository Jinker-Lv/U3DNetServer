using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoServer_Select_
{
    class EventHandler
    {
        public static void OnDisconnect(ClientState c) => Console.WriteLine("OnDisconnect");
    }
}
