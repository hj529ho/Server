using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace DummyClient
{
    class Program
    {
        static void Main(string[] args)
        {
            string host = Dns.GetHostName();
            IPHostEntry ipHost =Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);
            //listensocket

            Socket socket = new Socket(endPoint.AddressFamily,SocketType.Stream,ProtocolType.Tcp);
            
            socket.Connect(endPoint);
            
            Console.WriteLine($"Connected to {socket.RemoteEndPoint}");

            while (true)
            {
                for (int i = 0; i < 5; i++)
                {
                    string aa = $"GGDOK Enter{i}";
                    byte[] sendBUff = Encoding.UTF8.GetBytes(aa);
                    int sendBytes = socket.Send(sendBUff);
                    
                }
                byte[] recvBuff = new byte[1024];
                int recvBytes = socket.Receive(recvBuff);
                string data= Encoding.UTF8.GetString(recvBuff, 0, recvBytes);
            
                Console.WriteLine($"From Server :{data}");
            }
            
        }
    }
}