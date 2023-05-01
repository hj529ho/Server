using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;


namespace ServerCore
{
    class Program
    {
        private static Listener _listener = new Listener();

        static void OnAcceptHanlder(Socket clientSocket)
        {
            try
            {
                Session session = new Session();
                session.Start(clientSocket);
                
                byte[] sendBuff = Encoding.UTF8.GetBytes("welcome to GGDOK Server");
                session.Send(sendBuff);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static void Main(string[] args)
        {
            string host = Dns.GetHostName();
            IPHostEntry ipHost =Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);
            _listener.Init(endPoint, OnAcceptHanlder);
            
            while (true)
            {
                ;
            }
        }
    }
}