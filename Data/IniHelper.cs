﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ji.DataHelper
{
    /// <summary> 读写INI文件的类。 </summary>
    public class IniHelper
    {
        // 读写INI文件相关。
        [DllImport("kernel32.dll", EntryPoint = "WritePrivateProfileString", CharSet = CharSet.Ansi)]
        public static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileString", CharSet = CharSet.Ansi)]
        public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileSectionNames", CharSet = CharSet.Ansi)]
        public static extern int GetPrivateProfileSectionNames(IntPtr lpszReturnBuffer, int nSize, string filePath);

        [DllImport("KERNEL32.DLL ", EntryPoint = "GetPrivateProfileSection", CharSet = CharSet.Ansi)]
        public static extern int GetPrivateProfileSection(string lpAppName, byte[] lpReturnedString, int nSize, string filePath);

        /// <summary> 向INI写入数据。 </summary>
        /// <PARAM name="Section"> 节点名。 </PARAM>
        /// <PARAM name="Key">     键名。 </PARAM>
        /// <PARAM name="Value">   值名。 </PARAM>
        public static void Write(string Section, string Key, string Value, string path)
        {
            var r = WritePrivateProfileString(Section, Key, Value, path);
        }

        /// <summary> 读取INI数据。 </summary>
        /// <PARAM name="Section"> 节点名。 </PARAM>
        /// <PARAM name="Key">     键名。 </PARAM>
        /// <PARAM name="Path">    值名。 </PARAM>
        /// <returns> 相应的值。 </returns>
        public static string Read(string Section, string Key, string path)
        {
            var temp = new StringBuilder(255);
            var i = GetPrivateProfileString(Section, Key, "", temp, 255, path);
            return temp.ToString();
        }

        /// <summary> 读取一个ini里面所有的节 </summary>
        /// <param name="sections"></param>
        /// <param name="path">    </param>
        /// <returns></returns>
        public static int GetAllSectionNames(out string[] sections, string path)
        {
            var MAX_BUFFER = 32767;
            var pReturnedString = Marshal.AllocCoTaskMem(MAX_BUFFER);
            var bytesReturned = GetPrivateProfileSectionNames(pReturnedString, MAX_BUFFER, path);
            if (bytesReturned == 0)
            {
                sections = null;
                return -1;
            }
            var local = Marshal.PtrToStringAnsi(pReturnedString, (int)bytesReturned).ToString();
            Marshal.FreeCoTaskMem(pReturnedString);
            //use of Substring below removes terminating null for split
            sections = local.Substring(0, local.Length - 1).Split('\0');
            return 0;
        }

        /// <summary> 得到某个节点下面所有的key和value组合 </summary>
        /// <param name="section"></param>
        /// <param name="keys">   </param>
        /// <param name="values"> </param>
        /// <param name="path">   </param>
        /// <returns></returns>
        public static int GetAllKeyValues(string section, out string[] keys, out string[] values, string path)
        {
            var b = new byte[65535];

            var enc = FileHelper.GetEncodingType(new System.IO.FileInfo(path));
            GetPrivateProfileSection(section, b, b.Length, path);
            var s = enc.GetString(b);
            var tmp = s.Split((char)0);
            var result = new ArrayList();
            foreach (var r in tmp)
            {
                if (r != string.Empty)
                    result.Add(r);
            }
            keys = new string[result.Count];
            values = new string[result.Count];
            for (var i = 0; i < result.Count; i++)
            {
                var item = result[i].ToString().Split(new char[] { '=' });
                if (item.Length == 2)
                {
                    keys[i] = item[0].Trim();
                    values[i] = item[1].Trim();
                }
                else if (item.Length == 1)
                {
                    keys[i] = item[0].Trim();
                    values[i] = "";
                }
                else if (item.Length == 0)
                {
                    keys[i] = "";
                    values[i] = "";
                }
            }

            return 0;
        }
    }
}