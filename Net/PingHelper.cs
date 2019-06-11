using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using global::System.Net;

namespace Jiper.Net
{
    public class IcmpPacket
    {
        public Byte Type;    // type of message
        public Byte SubCode;    // type of sub code
        public UInt16 CheckSum;   // ones complement checksum of struct
        public UInt16 Identifier;      // identifier
        public UInt16 SequenceNumber;     // sequence number
        public Byte[] Data;
    } // class IcmpPacket
    class Ping
    {
        const int SOCKET_ERROR = -1;
        const int ICMP_ECHO = 8;
        public static void Main()
        {

            string[] argv = { "www.baidu.com", "10" };
            if (argv.Length == 0)
            {
                Console.WriteLine("Usage:Ping <hostname> -t");
                Console.WriteLine("<hostname> The name of the Host who you want to ping");
                Console.WriteLine("-t Optional Switch to Ping the host continuously");
            }
            else if (argv.Length == 1)
            {
                PingHost(argv[0]);
            }
            else if (argv.Length == 2)
            {
                if (argv[1] == "-t")
                {
                    while (true)
                    {
                        PingHost(argv[0]);
                    }
                }
                else
                {
                    PingHost(argv[0]);
                }
            }
            else
            {
                Console.WriteLine("Error in Arguments");
            }
            Console.Read();
        }
        public static void PingHost(string host)
        {
            IPHostEntry serverHE, fromHE;
            int nBytes = 0;
            int dwStart = 0, dwStop = 0;
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Icmp);
            try
            {
                serverHE = Dns.GetHostByName(host);
            }
            catch (Exception)
            {
                Console.WriteLine("Host not found"); // fail
                return;
            }
            IPEndPoint ipepServer = new IPEndPoint(serverHE.AddressList[0], 0);
            EndPoint epServer = (ipepServer);
            fromHE = Dns.GetHostByName(Dns.GetHostName());
            IPEndPoint ipEndPointFrom = new IPEndPoint(fromHE.AddressList[0], 0);
            EndPoint EndPointFrom = (ipEndPointFrom);
            int PacketSize = 0;
            IcmpPacket packet = new IcmpPacket();
            packet.Type = ICMP_ECHO; //8
            packet.SubCode = 0;
            packet.CheckSum = UInt16.Parse("0");
            packet.Identifier = UInt16.Parse("45");
            packet.SequenceNumber = UInt16.Parse("0");
            int PingData = 32; // sizeof(IcmpPacket) - 8;
            packet.Data = new Byte[PingData];
            for (int i = 0; i < PingData; i++)
                packet.Data[i] = (byte)'#';
            PacketSize = PingData + 8;
            Byte[] icmp_pkt_buffer = new Byte[PacketSize];
            Int32 Index = 0;
            Index = Serialize(packet, icmp_pkt_buffer, PacketSize, PingData);
            if (Index == -1)
            {
                Console.WriteLine("Error in Making Packet");
                return;
            }
            Double double_length = Convert.ToDouble(Index);
            Double dtemp = Math.Ceiling(double_length / 2);
            int cksum_buffer_length = Convert.ToInt32(dtemp);
            UInt16[] cksum_buffer = new UInt16[cksum_buffer_length];
            int icmp_header_buffer_index = 0;
            for (int i = 0; i < cksum_buffer_length; i++)
            {
                cksum_buffer[i] =
                    BitConverter.ToUInt16(icmp_pkt_buffer, icmp_header_buffer_index);
                icmp_header_buffer_index += 2;
            }
            UInt16 u_cksum = checksum(cksum_buffer, cksum_buffer_length);
            packet.CheckSum = u_cksum;
            Byte[] sendbuf = new Byte[PacketSize];
            Index = Serialize(packet, sendbuf, PacketSize, PingData);
            if (Index == -1)
            {
                Console.WriteLine("Error in Making Packet");
                return;
            }
            dwStart = Environment.TickCount; // Start timing
            if ((nBytes = socket.SendTo(sendbuf, PacketSize, 0, epServer)) == SOCKET_ERROR)
            {
                Console.WriteLine("Socket Error cannot Send Packet");
            }
            Byte[] ReceiveBuffer = new Byte[256];
            nBytes = 0;
            bool recd = false;
            int timeout = 0;
            while (!recd)
            {
                nBytes = socket.ReceiveFrom(ReceiveBuffer, 256, 0, ref EndPointFrom);
                if (nBytes == SOCKET_ERROR)
                {
                    Console.WriteLine("Host not Responding");
                    recd = true;
                    break;
                }
                else if (nBytes > 0)
                {
                    dwStop = Environment.TickCount - dwStart; // stop timing
                    Console.WriteLine("Reply from " + epServer.ToString() + " in " + dwStop + "MS,\tBytes Received :" + nBytes);
                    recd = true;
                    break;
                }
                timeout = Environment.TickCount - dwStart;
                if (timeout > 1000)
                {
                    Console.WriteLine("Time Out");
                    recd = true;
                }
            }
            socket.Close();
        }
        public static Int32 Serialize(IcmpPacket packet, Byte[] Buffer, Int32 PacketSize, Int32 PingData)
        {
            Int32 cbReturn = 0;
            int Index = 0;
            Byte[] b_type = new Byte[1];
            b_type[0] = (packet.Type);
            Byte[] b_code = new Byte[1];
            b_code[0] = (packet.SubCode);
            Byte[] b_cksum = BitConverter.GetBytes(packet.CheckSum);
            Byte[] b_id = BitConverter.GetBytes(packet.Identifier);
            Byte[] b_seq = BitConverter.GetBytes(packet.SequenceNumber);
            Array.Copy(b_type, 0, Buffer, Index, b_type.Length);
            Index += b_type.Length;
            Array.Copy(b_code, 0, Buffer, Index, b_code.Length);
            Index += b_code.Length;
            Array.Copy(b_cksum, 0, Buffer, Index, b_cksum.Length);
            Index += b_cksum.Length;
            Array.Copy(b_id, 0, Buffer, Index, b_id.Length);
            Index += b_id.Length;
            Array.Copy(b_seq, 0, Buffer, Index, b_seq.Length);
            Index += b_seq.Length;
            Array.Copy(packet.Data, 0, Buffer, Index, PingData);
            Index += PingData;
            if (Index != PacketSize)
            {
                cbReturn = -1;
                return cbReturn;
            }
            cbReturn = Index;
            return cbReturn;
        }
        public static UInt16 checksum(UInt16[] buffer, int size)
        {
            Int32 cksum = 0;
            int counter;
            counter = 0;
            while (size > 0)
            {
                UInt16 val = buffer[counter];
                cksum += Convert.ToInt32(buffer[counter]);
                counter += 1;
                size -= 1;
            }
            cksum = (cksum >> 16) + (cksum & 0xffff);
            cksum += (cksum >> 16);
            return (UInt16)(~cksum);
        }
    } // class ping
}
