using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ji.DataHelper
{
    public static class ByteHelper
    {
        /// <summary> 转换为十六进制 </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string ConvertToHexString(this byte[] values)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte value in values)
            {
                sb.AppendFormat("{0:X2}", value);
            }
            return sb.ToString();
        }

        public static List<byte> WriteString(this string value)
        {
            var datas = new List<byte>();
            if (!string.IsNullOrWhiteSpace(value))
            {
                datas.AddRange(WriteShort((short)value.Length));

                foreach (char item in value)
                {
                    datas.Add(BitConverter.GetBytes(item)[0]);
                }
            }

            return datas;
        }

        public static List<byte> WriteByte(this byte value)
        {
            return new List<byte>() { value };
        }

        public static List<byte> WriteBool(this bool value)
        {
            return new List<byte>() { value ? (byte)1 : (byte)0 };
        }

        public static List<byte> WriteShort(this short value)
        {
            return new List<byte>(BitConverter.GetBytes(value).Reverse());
        }

        public static List<byte> WriteLong(this long value)
        {
            return new List<byte>(BitConverter.GetBytes(value).Reverse());
        }

        public static List<byte> WriteInt(this int value)
        {
            return new List<byte>(BitConverter.GetBytes(value).Reverse());
        }

        public static bool ReadBool(this List<byte> value, ref int startindex)
        {
            var b = (int)value[startindex] == 0 ? false : true;
            startindex += 1;
            return b;
        }

        public static int ReadByte(this List<byte> value, ref int startindex)
        {
            var b = (int)value[startindex];
            startindex += 1;
            return b;
        }

        public static string ReadString(this List<byte> value, ref int startindex)
        {
            var length = value.ReadShort(ref startindex);
            if (length > 0)
            {
                var bytes = value.Skip(startindex).Take(length).ToArray();

                startindex += length;
                return Encoding.UTF8.GetString(bytes, 0, length);
            }

            return string.Empty;
        }

        public static short ReadShort(this List<byte> value, ref int startindex)
        {
            var datas = value.Skip(startindex).Take(2).Reverse().ToArray();
            startindex += 2;
            return BitConverter.ToInt16(datas, 0);
        }

        public static int ReadInt(this List<byte> value, ref int startindex)
        {
            var datas = value.Skip(startindex).Take(4).Reverse().ToArray();
            startindex += 4;
            return BitConverter.ToInt32(datas, 0);
        }

        public static long ReadLong(this List<byte> value, ref int startindex)
        {
            var datas = value.Skip(startindex).Take(8).Reverse().ToArray();
            startindex += 8;
            return BitConverter.ToInt64(datas, 0);
        }
    }
}