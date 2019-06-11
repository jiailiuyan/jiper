using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YB_QZJ_Controler.Helpers
{
    public static class Telneter
    {
        public static bool IsConnect(string address, int port)
        {
            try
            {
                using (var client = new PrimS.Telnet.Client("127.0.0.1", 9201, new System.Threading.CancellationToken()))
                {
                    return client.IsConnected;
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool IsConnect(string address)
        {
            if (address?.Contains(":") == true)
            {
                var ds = address.Split(':');

                var a = ds[0];
                var p = ds[1];
                return IsConnect(a, p);
            }
            return false;
        }

        public static bool IsConnect(string address, string port)
        {
            if (!string.IsNullOrWhiteSpace(address) && int.TryParse(port, out var p))
            {
                return IsConnect(address, p);
            }
            return false;
        }
    }

}
