/* 迹I柳燕
 *
 * FileName:   UnsafeSetting.cs
 * Version:    1.0
 * Date:       2017/8/31 15:11:05
 * Author:     迹
 *
 *========================================
 *
 * @namespace  Ji.CommonHelper.Settings
 * @class      UnsafeSetting
 * @extends
 *
 *========================================
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Reflection;
using System.Text;

namespace Ji.CommonHelper.Settings
{
    /// <summary></summary>
    public class UnsafeSetting
    {
        public static bool SetUnsafeMode(bool enable)
        {
            Assembly assembly = Assembly.GetAssembly(typeof(SettingsSection));
            if (assembly != null)
            {
                Type type = assembly.GetType("System.Net.Configuration.SettingsSectionInternal");
                if (type != null)
                {
                    object obj = type.InvokeMember("Section", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.GetProperty, null, null, new object[0]);
                    if (obj != null)
                    {
                        FieldInfo field = type.GetField("useUnsafeHeaderParsing", BindingFlags.Instance | BindingFlags.NonPublic);
                        if (field != null)
                        {
                            field.SetValue(obj, enable);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}