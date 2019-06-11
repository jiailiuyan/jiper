using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Ji.DataHelper
{
    public static class RIPEHelper
    {
        /// <summary>
        /// RIPEMD160哈希算法
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public static string GetRIPEMD160String(this string para)
        {
            try
            {
                var md5 = RIPEMD160Managed.Create();
                var byteData = System.Text.Encoding.UTF8.GetBytes(para);
                byteData = md5.ComputeHash(byteData);
                var OutString = BitConverter.ToString(byteData).Replace("-", "").ToUpper();
                return OutString;
            }
            catch
            {
                return para;
            }
        }
    }

    public static class MD5Helper
    {
        #region Methods

        public static string GetCustomMD5(this string str)
        {
            try
            {
                var mdhash = (new MD5(str)).GetStringHash();
                return mdhash.ToLower();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                //Debugger.Break();
            }
            return string.Empty;
        }

        public static string GetCustomMD5(this byte[] datas)
        {
            try
            {
                var mdhash = (new MD5(datas)).GetStringHash();
                return mdhash.ToLower();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return string.Empty;
        }

        public static string GetBase64Hash(this byte[] datas)
        {
            try
            {
                var mdhash = (new MD5(datas)).GetBase64Hash();
                return mdhash;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return string.Empty;
        }

        public static string GetCustomMD5(this FileInfo info)
        {
            if (info != null && info.Exists)
            {
                try
                {
                    var datas = File.ReadAllBytes(info.FullName);
                    var mdhash = (new MD5(datas)).GetStringHash();
                    return mdhash.ToLower();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    //Debugger.Break();
                }
            }
            return string.Empty;
        }

        #endregion Methods

        #region Classes

        public class MD5
        {
            #region Fields

            public byte[] message;

            private readonly uint[] T = new uint[65]
            {
                0,
                0xd76aa478, 0xe8c7b756, 0x242070db, 0xc1bdceee,
                0xf57c0faf, 0x4787c62a, 0xa8304613, 0xfd469501,
                0x698098d8, 0x8b44f7af, 0xffff5bb1, 0x895cd7be,
                0x6b901122, 0xfd987193, 0xa679438e, 0x49b40821,
                0xf61e2562, 0xc040b340, 0x265e5a51, 0xe9b6c7aa,
                0xd62f105d, 0x02441453, 0xd8a1e681, 0xe7d3fbc8,
                0x21e1cde6, 0xc33707d6, 0xf4d50d87, 0x455a14ed,
                0xa9e3e905, 0xfcefa3f8, 0x676f02d9, 0x8d2a4c8a,
                0xfffa3942, 0x8771f681, 0x6d9d6122, 0xfde5380c,
                0xa4beea44, 0x4bdecfa9, 0xf6bb4b60, 0xbebfbc70,
                0x289b7ec6, 0xeaa127fa, 0xd4ef3085, 0x04881d05,
                0xd9d4d039, 0xe6db99e5, 0x1fa27cf8, 0xc4ac5665,
                0xf4292244, 0x432aff97, 0xab9423a7, 0xfc93a039,
                0x655b59c3, 0x8f0ccc92, 0xffeff47d, 0x85845dd1,
                0x6fa87e4f, 0xfe2ce6e0, 0xa3014314, 0x4e0811a1,
                0xf7537e82, 0xbd3af235, 0x2ad7d2bb, 0xeb86d391
            };

            private uint[] X = new uint[0];

            #endregion Fields

            #region Constructors

            public MD5(byte[] message)
            {
                this.message = message;
            }

            public MD5(string message)
            {
                this.message = StringToBytes(message);
            }

            #endregion Constructors

            #region Properties

            private int b => message.Length;

            #endregion Properties

            #region Methods

            public uint[] GetHash()
            {
                var extMessage = new List<byte>();
                extMessage.AddRange(message);
                extMessage.Add(128);
                while ((extMessage.Count * 8) % 512 != 448)
                {
                    extMessage.Add(0);
                }
                extMessage.AddRange(BitConverter.GetBytes((ulong)b * 8));

                var M = new uint[extMessage.Count / 4];
                {
                    var tmp = extMessage.ToArray();
                    for (var i = 0; i < extMessage.Count / 4; ++i)
                    {
                        M[i] = BitConverter.ToUInt32(tmp, i * 4);
                    }
                }

                uint A = 0x67452301, B = 0xEFCDAB89, C = 0x98BADCFE, D = 0x10325476;

                for (var i = 0; i <= M.Length / 16 - 1; ++i)
                {
                    X = new uint[16];
                    for (var j = 0; j <= 15; ++j)
                    {
                        X[j] = M[i * 16 + j];
                    }

                    uint AA = A, BB = B, CC = C, DD = D;

                    Round1(ref A, B, C, D, 0, 7, 1); Round1(ref D, A, B, C, 1, 12, 2); Round1(ref C, D, A, B, 2, 17, 3); Round1(ref B, C, D, A, 3, 22, 4); Round1(ref A, B, C, D, 4, 7, 5); Round1(ref D, A, B, C, 5, 12, 6); Round1(ref C, D, A, B, 6, 17, 7); Round1(ref B, C, D, A, 7, 22, 8); Round1(ref A, B, C, D, 8, 7, 9); Round1(ref D, A, B, C, 9, 12, 10); Round1(ref C, D, A, B, 10, 17, 11); Round1(ref B, C, D, A, 11, 22, 12); Round1(ref A, B, C, D, 12, 7, 13); Round1(ref D, A, B, C, 13, 12, 14); Round1(ref C, D, A, B, 14, 17, 15); Round1(ref B, C, D, A, 15, 22, 16);
                    Round2(ref A, B, C, D, 1, 5, 17); Round2(ref D, A, B, C, 6, 9, 18); Round2(ref C, D, A, B, 11, 14, 19); Round2(ref B, C, D, A, 0, 20, 20); Round2(ref A, B, C, D, 5, 5, 21); Round2(ref D, A, B, C, 10, 9, 22); Round2(ref C, D, A, B, 15, 14, 23); Round2(ref B, C, D, A, 4, 20, 24); Round2(ref A, B, C, D, 9, 5, 25); Round2(ref D, A, B, C, 14, 9, 26); Round2(ref C, D, A, B, 3, 14, 27); Round2(ref B, C, D, A, 8, 20, 28); Round2(ref A, B, C, D, 13, 5, 29); Round2(ref D, A, B, C, 2, 9, 30); Round2(ref C, D, A, B, 7, 14, 31); Round2(ref B, C, D, A, 12, 20, 32);
                    Round3(ref A, B, C, D, 5, 4, 33); Round3(ref D, A, B, C, 8, 11, 34); Round3(ref C, D, A, B, 11, 16, 35); Round3(ref B, C, D, A, 14, 23, 36); Round3(ref A, B, C, D, 1, 4, 37); Round3(ref D, A, B, C, 4, 11, 38); Round3(ref C, D, A, B, 7, 16, 39); Round3(ref B, C, D, A, 10, 23, 40); Round3(ref A, B, C, D, 13, 4, 41); Round3(ref D, A, B, C, 0, 11, 42); Round3(ref C, D, A, B, 3, 16, 43); Round3(ref B, C, D, A, 6, 23, 44); Round3(ref A, B, C, D, 9, 4, 45); Round3(ref D, A, B, C, 12, 11, 46); Round3(ref C, D, A, B, 15, 16, 47); Round3(ref B, C, D, A, 2, 23, 48);
                    Round4(ref A, B, C, D, 0, 6, 49); Round4(ref D, A, B, C, 7, 10, 50); Round4(ref C, D, A, B, 14, 15, 51); Round4(ref B, C, D, A, 5, 21, 52); Round4(ref A, B, C, D, 12, 6, 53); Round4(ref D, A, B, C, 3, 10, 54); Round4(ref C, D, A, B, 10, 15, 55); Round4(ref B, C, D, A, 1, 21, 56); Round4(ref A, B, C, D, 8, 6, 57); Round4(ref D, A, B, C, 15, 10, 58); Round4(ref C, D, A, B, 6, 15, 59); Round4(ref B, C, D, A, 13, 21, 60); Round4(ref A, B, C, D, 4, 6, 61); Round4(ref D, A, B, C, 11, 10, 62); Round4(ref C, D, A, B, 2, 15, 63); Round4(ref B, C, D, A, 9, 21, 64);

                    A += AA;
                    B += BB;
                    C += CC;
                    D += DD;
                }

                A = BitConverter.ToUInt32(BitConverter.GetBytes(A).Reverse().ToArray(), 0);
                B = BitConverter.ToUInt32(BitConverter.GetBytes(B).Reverse().ToArray(), 0);
                C = BitConverter.ToUInt32(BitConverter.GetBytes(C).Reverse().ToArray(), 0);
                D = BitConverter.ToUInt32(BitConverter.GetBytes(D).Reverse().ToArray(), 0);

                return new uint[] { A, B, C, D };
            }

            public string GetStringHash()
            {
                return GetStringHash(GetHash());
            }

            public string GetBase64Hash()
            {
                return GetBase64Hash(GetHash());
            }

            public string GetBase64Hash(uint[] datas)
            {
                var mds = new List<byte>();
                foreach (var item in datas)
                {
                    mds.Add((byte)(item >> 24));
                    mds.Add((byte)(item >> 16));
                    mds.Add((byte)(item >> 8));
                    mds.Add((byte)(item));
                }
                return Convert.ToBase64String(mds.ToArray());
            }

            public string GetStringHash(uint[] datas)
            {
                var sb = new StringBuilder();
                for (var i = 0; i < datas.Length; ++i)
                {
                    var s = string.Format("{0:X}", datas[i]);
                    var span = 8 - s.Length;
                    while (span > 0)
                    {
                        span--;
                        sb.Append("0");
                    }
                    sb.Append(s);
                }
                return sb.ToString();
            }

            private uint F(uint x, uint y, uint z) => (x & y) | (~x & z);

            private uint G(uint x, uint y, uint z) => (x & z) | (~z & y);

            private uint H(uint x, uint y, uint z) => x ^ y ^ z;

            private uint I(uint x, uint y, uint z) => y ^ (~z | x);

            private uint ROTL(uint x, int c) => (x << c) | (x >> (32 - c));

            private void Round1(ref uint a, uint b, uint c, uint d, int k, int s, int i) => a = b + (ROTL(a + F(b, c, d) + X[k] + T[i], s));

            private void Round2(ref uint a, uint b, uint c, uint d, int k, int s, int i) => a = b + (ROTL(a + G(b, c, d) + X[k] + T[i], s));

            private void Round3(ref uint a, uint b, uint c, uint d, int k, int s, int i) => a = b + (ROTL(a + H(b, c, d) + X[k] + T[i], s));

            private void Round4(ref uint a, uint b, uint c, uint d, int k, int s, int i) => a = b + (ROTL(a + I(b, c, d) + X[k] + T[i], s));

            /// <summary> 修改字符串默认UTF8编码 </summary>

            private byte[] StringToBytes(string str)
            {
                return Encoding.UTF8.GetBytes(str);
            }

            #endregion Methods
        }

        #endregion Classes
    }
}