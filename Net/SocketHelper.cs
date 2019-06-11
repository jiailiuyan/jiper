using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Ji.NetHelper
{
    public static class SocketHelper
    {
        public static int FindNoUsedPort(int defaultport = 12001)
        {
            int port = defaultport;
            UdpClient u = null;
            bool isfind = false;
            while (!isfind)
            {
                try
                {
                    u = new UdpClient(port);
                    isfind = true;
                }
                catch
                {
                    port++;
                }
                finally
                {
                    if (u != null)
                    {
                        u.Close();
                    }
                }
            }
            return port;
        }

        public static IPEndPoint CreatIPEndPoint(string ip = "127.0.0.1")
        {
            return new IPEndPoint(System.Net.IPAddress.Parse(ip), SocketHelper.FindNoUsedPort());
        }
    }

    public class MessageArgs : EventArgs
    {
        public List<byte> Datas { get; set; }
    }

    public class JSocket
    {
        public Thread tClient = null;
        public IPEndPoint pClient = null;
        public Socket sClient = null;
        public NetworkStream nsClient = null;
        public bool m_bConnectedClient = false;

        public event EventHandler<MessageArgs> RecivedMessage;

        protected virtual void OnRecivedMessage(List<byte> datas)
        {
            if (this.RecivedMessage != null)
            {
                this.RecivedMessage(this, new MessageArgs() { Datas = datas });
            }
        }

        public bool Connect(IPEndPoint server)
        {
            pClient = server;
            sClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sClient.Connect(pClient);
            if (sClient.Connected)
            {
                nsClient = new NetworkStream(sClient);
                tClient = new Thread(new ThreadStart(this.ThreadListenClient));
                tClient.Start();
                m_bConnectedClient = true;
                return true;
            }
            return false;
        }

        public bool Sent(byte[] sentArray)
        {
            int length = sentArray.Length;
            if (sClient.Connected)
            {
                int sentNum = sClient.Send(sentArray, 0, length, SocketFlags.None);
                if (sentNum == length)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public void Close()
        {
            sClient.Dispose();
            // sClient = null;
            m_bConnectedClient = false;
        }

        public void ThreadListenClient()
        {
            string tmp = string.Empty;
            while (m_bConnectedClient)
            {
                try
                {
                    byte[] data = new byte[10240];
                    if (sClient.Connected)
                    {
                        int count = sClient.Receive(data);
                        if (count != 0)
                        {
                            OnRecivedMessage(new List<byte>(data));
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                catch { }
            }
        }
    }
}