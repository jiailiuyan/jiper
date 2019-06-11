using System;
using System.Net;
using System.Net.Sockets;

namespace Ji.NetHelper
{
    public static class UDPHelper
    {
        /// <summary> udpClient.Send(new byte[] { 1, 0, 0, 0, 8 }, 5); </summary>
        /// <param name="port"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static UDPSend StartUDP(string ip = "127.0.0.1")
        {
            var udpClient = new UDPSend();
            udpClient.Start(ip);
            return udpClient;
        }

        public static void SendPoint(this UDPSend udp, double x, double y, int? port = null)
        {
            var data = ConverterPoint(x, y);
            udp.Send(data, port);
        }

        public static byte[] PointData = new byte[8];

        public static byte[] ConverterPoint(double x, double y)
        {
            PointData[0] = (byte)(x / 1000 % 10);
            PointData[1] = (byte)(x / 100 % 10);
            PointData[2] = (byte)(x / 10 % 10);
            PointData[3] = (byte)(x % 10);

            PointData[4] = (byte)(y / 1000 % 10);
            PointData[5] = (byte)(y / 100 % 10);
            PointData[6] = (byte)(y / 10 % 10);
            PointData[7] = (byte)(y % 10);
            return PointData;
        }
    }

    public class UDPSend : IDisposable
    {
        private Socket socket;

        private EndPoint remotePoint;
        public EndPoint RemotePoint { get { return remotePoint; } }

        private IPEndPoint endPoint;

        public void Start(string localIP = "127.0.0.1")
        {
            this.remotePoint = new IPEndPoint(IPAddress.Parse(localIP), 0);
            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            this.SetEndPoint(5555);
        }

        public void SetEndPoint(int port, string ip = "127.0.0.1")
        {
            this.endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        }

        public bool Send(byte[] data, int? port = null)
        {
            if (port != null)
            {
                this.endPoint.Port = port.Value;
            }
            try
            {
                this.socket.SendTo(data, data.Length, SocketFlags.None, (EndPoint)endPoint);
                return true;
            }
            catch { }
            return false;
        }

        public void Close()
        {
            if (socket != null)
            {
                socket.Close();
            }
        }

        #region IDisposable 成员

        private bool isdispose = false;

        ~UDPSend()
        {
            if (!isdispose)
            {
                isdispose = true;
                Close();
            }
        }

        public void Dispose()
        {
            if (!isdispose)
            {
                isdispose = true;
                Close();
            }
        }

        #endregion IDisposable 成员
    }
}