///* 迹I柳燕
// *
// * FileName:   PluginLoader.cs
// * Version:    1.0
// * Date:       2016/11/23 11:18:49
// * Author:     迹
// *
// *========================================
// *
// * @namespace  Ji.Base
// * @class      PluginLoader
// * @extends
// *
// *========================================
// *
// */

//using System;
//using System.Collections.Generic;
//using System.ComponentModel.Composition;
//using System.ComponentModel.Composition.Hosting;
//using System.Linq;
//using System.Reflection;
//using System.Text;

//namespace Ji.Base
//{
//    public class PluginLoader<T> : MEFManager
//    {
//        public PluginLoader(string searchpattern = "*.dll")
//        {
//            SearchPattern = searchpattern;
//        }

//        public static IEnumerable<T> GetExports(string searchpattern)
//        {
//            var pls = new PluginLoader<T>(searchpattern).GetExports<T>();
//            return pls;
//        }

//        public static IEnumerable<T> GetExports(Assembly assembly)
//        {
//            var pls = new PluginLoader<T>().GetExports<T>(assembly);
//            return pls;
//        }
//    }

//    public static class PluginLoaderHelper
//    {
//        public static IEnumerable<T> GetExports<T>(this Assembly assembly)
//        {
//            var pls = new PluginLoader<T>().GetExports<T>(assembly);
//            return pls;
//        }
//    }
//}