//// ***********************************************************************
//// 程序集            : Ksy.Client.CommonHelper
//// 作者              : Lyon
//// 创建日期          : 05-03-2018
////
//// 最后编辑者        : Lyon
//// 最后编辑时间      : 06-05-2018
//// ***********************************************************************
//// <版权所有 文件="ConfigManagerHelper.cs" 组织="Lyon">
////     Copyright © Lyon
//// </版权所有>
//// ***********************************************************************
//using System;
//using System.Configuration;
//using System.IO;
//using System.Linq;

//namespace Ji.CommonHelper.Helpers
//{
//    /// <summary>
//    /// 配置管理器帮助类
//    /// </summary>
//    public class ConfigManagerHelper
//    {
//        #region Private 字段

//        /// <summary>
//        /// 应用程序配置
//        /// </summary>
//        private static Configuration config;

//        #endregion Private 字段

//        #region Public 构造函数

//        /// <summary>
//        /// 初始化静态成员 <see cref="ConfigManagerHelper"/> class.
//        /// </summary>
//        static ConfigManagerHelper()
//        {
//            ConfigManagerHelper.config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
//        }

//        #endregion Public 构造函数

//        #region Private 构造函数

//        /// <summary>
//        /// 防止默认 <see cref="ConfigManagerHelper"/> class 的实例被创建.
//        /// </summary>
//        private ConfigManagerHelper()
//        {
//        }

//        #endregion Private 构造函数

//        #region Public 属性

//        /// <summary>
//        /// 获取配置文件路径.
//        /// </summary>
//        /// <value>配置文件路径.</value>
//        public static string ConfigPath { get; private set; }

//        #endregion Public 属性

//        #region Public 方法

//        /// <summary>
//        /// 修改配置文件路径.
//        /// </summary>
//        /// <param name="path">路径.</param>
//        public static void ChangeConfigPath(string path = null)
//        {
//            if (!string.IsNullOrEmpty(path) && File.Exists(path))
//            {
//                ConfigManagerHelper.ConfigPath = path;
//                ConfigManagerHelper.config = ConfigurationManager.OpenExeConfiguration(path);
//            }
//            else
//                ConfigManagerHelper.config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
//        }

//        /// <summary>
//        /// 是否存在键
//        /// </summary>
//        /// <param name="key">The key.</param>
//        /// <returns><c>true</c> if [contains] [the specified key]; otherwise, <c>false</c>.</returns>
//        public static bool Contains(string key)
//        {
//            return ConfigManagerHelper.config.AppSettings.Settings.AllKeys.Contains(key);
//        }

//        /// <summary>
//        /// 获取对应键的值,如果不存在则创建
//        /// </summary>
//        /// <typeparam name="T">值类型</typeparam>
//        /// <param name="key">键</param>
//        /// <param name="defaultValue">The default value.</param>
//        /// <returns>System.String.</returns>
//        public static T GetValue<T>(string key, T defaultValue = default(T))
//        {
//            T result;
//            if (ConfigManagerHelper.Contains(key))
//                result = ConfigManagerHelper.config.AppSettings.Settings[key].Value.ConvertType<T>();
//            else
//            {
//                ConfigManagerHelper.IsExistAndCreateKey(key, defaultValue);
//                result = defaultValue;
//            }
//            return result;
//        }

//        /// <summary>
//        /// 是否存在指定的键,不存在则创建
//        /// </summary>
//        /// <typeparam name="T">值类型</typeparam>
//        /// <param name="key">键.</param>
//        /// <param name="defaultValue">默认值.</param>
//        /// <returns><c>true</c> if [is exist and create key] [the specified key]; otherwise, <c>false</c>.</returns>
//        public static bool IsExistAndCreateKey<T>(string key, T defaultValue)
//        {
//            bool isExist = ConfigManagerHelper.Contains(key);
//            if (!isExist)
//                ConfigManagerHelper.SetValue(key, defaultValue);
//            return isExist;
//        }

//        /// <summary>
//        /// 设置对应键的值
//        /// </summary>
//        /// <typeparam name="T">值类型</typeparam>
//        /// <param name="key">键.</param>
//        /// <param name="value">值.</param>
//        public static void SetValue<T>(string key, T value)
//        {
//            if (ConfigManagerHelper.Contains(key))
//            {
//                KeyValueConfigurationElement keyValueConfigurationElement = ConfigManagerHelper.config.AppSettings.Settings[key];
//                keyValueConfigurationElement.Value = value.ConvertType<string>();
//            }
//            else
//                ConfigManagerHelper.config.AppSettings.Settings.Add(key, value.ConvertType<string>());
//            ConfigManagerHelper.config.Save(ConfigurationSaveMode.Full);
//            ConfigurationManager.RefreshSection("appSettings");
//        }

//        #endregion Public 方法
//    }
//}