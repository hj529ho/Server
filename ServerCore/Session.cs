using System.Net.Sockets;
using System.Text;

namespace ServerCore
{
    public class Session
    {
        private Socket _socket;
        private int _disconnected = 0;
        SocketAsyncEventArgs _sendArgs = new SocketAsyncEventArgs();
        public void Start(Socket socket)
        {
            _socket = socket;

            SocketAsyncEventArgs recvArgs = new SocketAsyncEventArgs();
            recvArgs.Completed += OnRecvCompleted;
            // recvArgs.UserToken <= 연동하고 싶은 데이터가 있을때 쓰는것
            recvArgs.SetBuffer(new byte[1024],0,1024);
            
            _sendArgs.Completed += OnSendCompleted;
            
            RegisterRecv();
        }
        public void Send(byte[] sendBuff)
        {
            // _socket.Send(sendBuff);
          
            
            _sendArgs.SetBuffer(sendBuff,0,sendBuff.Length);
            RegisterSend(_sendArgs);
        }

        public void Disconnect()
        {
            if (Interlocked.Exchange(ref _disconnected, 1) == 1)
                return;
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
        }


        #region 네트워크 통신

        void RegisterSend(SocketAsyncEventArgs args)
        {
            bool pending = _socket.SendAsync(args);
            if (pending == false)
                OnSendCompleted(null, args);
        }

        void OnSendCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
            {
                try
                {
                  
                }
                catch (Exception e)
                {
                    Console.WriteLine($"OnSendCompleted Failed {e}");
                }
            }
            else
            {
                Disconnect();
            }
        }

        void RegisterRecv()
        {
            _sendArgs.AcceptSocket = null;
           bool pending = _socket.ReceiveAsync(_sendArgs);
           if (pending == false)
           {
               OnRecvCompleted(null,_sendArgs);
           }
        }
        void OnRecvCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
            {
                try
                {
                    //TODO
                    string recvData = Encoding.UTF8.GetString(args.Buffer,args.Offset,args.BytesTransferred);
                    Console.WriteLine($"[From client] {recvData}");
                    RegisterRecv(args);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"OnRecvCompleted Failed {e}");
                }
            }
            else
            {
                Disconnect();
            }
        }
        #endregion

    }
}

