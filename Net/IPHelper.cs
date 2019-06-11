using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jiper.Net
{
    class IPHelper
    {
        /// <summary>
        /// 计算广播地址算法
        /// <remarks>        
        /// 计算方法 
        ///首先 计算网络地址 = IP地址  逻辑与(&)  子网掩码 
        ///先把IP，子网掩码转为2进制，然后进行逻辑与运算，得出网络地址 
        ///<example>
        ///例： 
        ///IP 192.168.1.3  子网掩码 255.255.0.0 
        ///IP转二进制 11000000.10100100.00000001.00000011 
        ///子网掩码    11111111.11111111.00000000.00000000 
        ///与运算后    11000000.10100100.00000000.00000000 
        ///192.168.0.0这就是网络地址，其中子网掩码全1对应为网络号,全0对应的是主机号,
        ///即192.168.0.0对应的网络号为192.168,主机号为0.0.
        ///将网络地址主机部分全取反后得到的地址便是广播地址: 
        ///广播地址    11000000.10100100.11111111.11111111 
        ///换成10进制则为192.168.0.0 
        ///</example>
        /// </remarks>
        /// </summary>
        private void CalcBroadcast(string LocalIPAddress, string SubnetMaskAddress)
        {
            string[] IPAddresses = LocalIPAddress.Split(new char[] { '.' });
            string[] subnetMaskAddresses = SubnetMaskAddress.Split(new char[] { '.' });

            byte[] IPParts = new byte[4];//IP
            byte[] maskParts = new byte[4];//子网掩码
            byte[] netParts = new byte[4];//网络地址

            for (int i = 0; i < 4; i++)
            {
                IPParts[i] = byte.Parse(IPAddresses[i]);
                maskParts[i] = byte.Parse(subnetMaskAddresses[i]);
                byte ip = IPParts[i];
                byte mask = maskParts[i];
                netParts[i] = ((byte)(ip & mask));//与运算后是网络地址
            }

            //网络号
            ulong netId = 0;
            for (int i = 0; i < 4; i++)
            {
                netId += netParts[i];
                if (i < 3)
                    netId <<= 8;
            }

            //
            ulong IPMask = 0;
            for (int i = 0; i < 4; i++)
            {
                IPMask += maskParts[i];
                if (i < 3)
                    IPMask <<= 8;
                else
                    IPMask = ~IPMask;
            }

            //算广播地址
            ulong broadcastId = IPMask | netId;
            byte[] bIPParts = new byte[4];
            for (int i = 3; i >= 0; i--)
            {
                bIPParts[i] = ((byte)(broadcastId & 255));
                if (i > 0)
                    broadcastId >>= 8;
            }
            string s = string.Format("{0}.{1}.{2}.{3}", bIPParts[0], bIPParts[1], bIPParts[2], bIPParts[3]);

        }
    }
}
