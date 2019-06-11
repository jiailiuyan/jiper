using System;
using System.Collections.Generic;
using System.Management;

namespace Ji.NetHelper
{
    public static class NetInfoHelper
    {
        /// <summary> 获取网卡信息 </summary>
        /// <returns></returns>
        public static List<NetInfoStruct> GetNetInfo()
        {
            var netList = new List<NetInfoStruct>();
            var query = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = 'TRUE'");
            var queryCollection = query.Get();
            foreach (ManagementObject mo in queryCollection)
            {
                NetInfoStruct net = new NetInfoStruct();
                net.IP = mo["IPAddress"] as string[];
                net.Subnet = mo["IPSubnet"] as string[];
                net.Gateway = mo["DefaultIPGateway"] as string[];
                net.DNS = mo["DNSServerSearchOrder"] as string[];
                net.Description = mo["Description"] as string;
                net.Mac = mo["MACAddress"] as string;
                net.Name = GetName(mo["Description"] as string);
                netList.Add(net);
            }
            return netList;
        }

        /// <summary> 获取网卡名称 </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        private static string GetName(string description)
        {
            return description.Contains("Virtual") ? "虚拟连接" : (description.Contains("Wireless") ? "无线连接" : "本地连接");
        }

        /// <summary> 计算广播地址 </summary>
        /// <param name="LocalIPAddress">IP</param>
        /// <param name="SubnetMaskAddress">子网掩码</param>
        /// <returns></returns>
        public static string GetBroadcast(string LocalIPAddress, string SubnetMaskAddress)
        {
            string[] IPAddresses = LocalIPAddress.Split(new char[] { '.' });
            string[] subnetMaskAddresses = SubnetMaskAddress.Split(new char[] { '.' });

            //IP
            byte[] IPParts = new byte[4];
            //子网掩码
            byte[] maskParts = new byte[4];
            //网络地址
            byte[] netParts = new byte[4];

            for (int i = 0; i < 4; i++)
            {
                IPParts[i] = byte.Parse(IPAddresses[i]);
                maskParts[i] = byte.Parse(subnetMaskAddresses[i]);
                byte ip = IPParts[i];
                byte mask = maskParts[i];
                //与运算后是网络地址
                netParts[i] = ((byte)(ip & mask));
            }

            //网络号
            ulong netId = 0;
            for (int i = 0; i < 4; i++)
            {
                netId += netParts[i];
                if (i < 3)
                {
                    netId <<= 8;
                }
            }

            ulong IPMask = 0;
            for (int i = 0; i < 4; i++)
            {
                IPMask += maskParts[i];
                IPMask = i < 3 ? IPMask << 8 : ~IPMask;
            }

            //算广播地址
            ulong broadcastId = IPMask | netId;
            byte[] bIPParts = new byte[4];
            for (int i = 3; i >= 0; i--)
            {
                bIPParts[i] = ((byte)(broadcastId & 255));
                if (i > 0)
                {
                    broadcastId >>= 8;
                }
            }

            return string.Format("{0}.{1}.{2}.{3}", bIPParts[0], bIPParts[1], bIPParts[2], bIPParts[3]);
        }

        public static Response GetIPDetails()
        {
            string url = "http://www.freegeoip.net/xml/";
            using (var stream = HttpHelper.GetStreamResponse(url))
            {
                return DataHelper.XmlData.ReadFromStream<Response>(stream);
            }
        }

        /// <summary> 是否能 Ping 通指定的主机 </summary>
        /// <param name="ip">ip 地址或主机名或域名</param>
        public static System.Net.NetworkInformation.PingReply Ping(this string ip)
        {
            var p = new System.Net.NetworkInformation.Ping();
            var options = new System.Net.NetworkInformation.PingOptions();
            options.DontFragment = true;
            byte[] buffer = System.Text.Encoding.ASCII.GetBytes("Ji");
            try
            {
                return p.Send(ip, 300, buffer, options);
            }
            catch { }
            return default(System.Net.NetworkInformation.PingReply);
        }

        public static string GetIp()
        {
            string ip = string.Empty;
            try
            {
                ip = System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName())[0].ToString();

                //http://jira.op.ksyun.com/browse/YLYUN-5996?filter=12785
                //在 System.Net.IPAddress.get_Address()
                //在 Yuanbo.ChssClient.InPatient.Model.YiZhuModelBaseVM.SetLock(Nullable`1 sdlx, Nullable`1 mxmodel) 位置 D:\Work\GitProject\HaaS\Yuanbo.ChssClient.InPatient\Model\YiZhuModelBaseVM.cs:行号 279
                //在 Yuanbo.ChssClient.InPatient.ViewModel.HS.LinShiYiZhuVM.Query() 位置 D:\Work\GitProject\HaaS\Yuanbo.ChssClient.InPatient\ViewModel\HS\LinShiYiZhuVM.cs:行号 336
                //在 Yuanbo.ChssClient.InPatient.ViewModel.HS.LinShiYiZhuVM.Load() 位置 D:\Work\GitProject\HaaS\Yuanbo.ChssClient.InPatient\ViewModel\HS\LinShiYiZhuVM.cs:行号 321

                //ip = new System.Net.IPAddress(System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList[0].Address).ToString();
            }
            catch (Exception ex)
            {
                LogX.Error(ex);
            }
            return ip;
        }
    }

    public class Response
    {
        public string IP { get; set; } //106.39.104.218public string /IP{get;set;} //
        public string CountryCode { get; set; } //CNpublic string /CountryCode{get;set;} //
        public string CountryName { get; set; } //Chinapublic string /CountryName{get;set;} //
        public string RegionCode { get; set; } //11public string /RegionCode{get;set;} //
        public string RegionName { get; set; } //Beijing Shipublic string /RegionName{get;set;} //
        public string City { get; set; } //Beijingpublic string /City{get;set;} //
        public string ZipCode { get; set; } //public string /ZipCode{get;set;} //
        public string TimeZone { get; set; } //Asia/Shanghaipublic string /TimeZone{get;set;} //
        public string Latitude { get; set; } //39.929public string /Latitude{get;set;} //
        public string Longitude { get; set; } //116.388public string /Longitude{get;set;} //
        public string MetroCode { get; set; } //0public string /MetroCode{get;set;} //
    }

    public struct NetInfoStruct
    {
        public string[] IP;
        public string[] Subnet;
        public string[] Gateway;
        public string[] DNS;
        public string Mac;
        public string Name;
        public string Description;

        public NetInfoStruct(string[] ip, string[] subnet, string[] getway, string[] dns, string mac, string name, string description)
        {
            this.IP = ip;
            this.Subnet = subnet;
            this.Gateway = getway;
            this.DNS = dns;
            this.Mac = mac;
            this.Name = name;
            this.Description = description;
        }
    }
}