using System.Net;
using System.Net.Sockets;

namespace ServerCore
{
    public class Listener
    {
        private Socket _listenSocket;
        private Action<Socket> _onAcceptHandler;
        public void Init(IPEndPoint endPoint, Action<Socket> OnAcceptHanlder)
        {
            _listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _onAcceptHandler += OnAcceptHanlder;

            _listenSocket.Bind(endPoint);
            _listenSocket.Listen(10);
            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.Completed += OnAcceptCompleted;
            RegisterAccept(args);
        }
        void RegisterAccept(SocketAsyncEventArgs args)
        {
            args.AcceptSocket = null;
            
            bool pending = _listenSocket.AcceptAsync(args);
            if (pending == false)
            {
                OnAcceptCompleted(null,args);
            }
        }
        void OnAcceptCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success)
            {
                _onAcceptHandler.Invoke(args.AcceptSocket);
            }
            else
            {
                Console.WriteLine(args.SocketError.ToString());
            }
            RegisterAccept(args);
        }
    }
}
