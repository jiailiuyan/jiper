/* 迹I柳燕
 *
 * FileName:   Locator.cs
 * Version:    1.0
 * Date:       2018/11/23 14:16:33
 * Author:     迹
 *
 *========================================
 *
 * @namespace  System
 * @class      Locator
 * @extends
 *
 *========================================
 *
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using CommonServiceLocator;
using Ji.Base;
using Unity;
using Unity.Lifetime;
using Unity.Resolution;
using Unity.ServiceLocation;

namespace System
{
    /// <summary>
    /// 依赖倒置容器
    /// </summary>
    public class Locator
    {
        private static IUnityContainer Container = null;

        public static void Init(IUnityContainer container = null)
        {
            Container = container ?? new UnityContainer();

            // 为支持某些无法向下引用的依赖倒置
            var locator = new UnityServiceLocator(Container);
            ServiceLocator.SetLocatorProvider(() => locator);

            var llis = PluginLoader<ILocatorInit>.GetExports("*.Locator.*.dll");
            if (llis != null && llis.Count() > 0)
            {
                llis.ForEach(i => i.Register(Container));

                llis.ForEach(i => (i as ILocatorInit)?.Init());
            }
        }

        /// <summary>
        /// 注册指定实例到容器
        /// </summary>
        public static bool RegisterSingleton<TParent, TChild>(TChild child) where TChild : TParent
        {
            var key = typeof(TParent).FullName;
            //Container.RegisterInstance<TParent>(key, child, new ContainerControlledLifetimeManager());
            return RegisterSingleton<TParent, TChild>(child, key);
        }

        public static bool RegisterSingleton<TChild>(TChild child)
        {
            var key = typeof(TChild).FullName;
            return RegisterSingleton<TChild, TChild>(child, key);
        }


        /// <summary>
        /// 注册指定实例到容器
        /// </summary>
        public static bool RegisterSingleton<TParent, TChild>(TChild child, string key) where TChild : TParent
        {
            //var key = typeof(TParent).FullName;
            Container.RegisterInstance<TParent>(key, child, new ContainerControlledLifetimeManager());
            return true;
        }

        /// <summary>
        /// 创建单一实例
        /// </summary>
        public static bool RegisterToSingleton<TParent, TChild>(string key = null) where TChild : TParent
        {
            return Register<TParent, TChild>(false, key);
        }

        public static bool RegisterElementToSingleton<TChild>(string key = null) where TChild : FrameworkElement
        {
            return Register<FrameworkElement, TChild>(false, key);
        }

        /// <summary>
        /// 注册每次 重新实例的类型
        /// </summary>
        public static bool RegisterToCreatNew<TParent, TChild>(string key = null) where TChild : TParent
        {
            return Register<TParent, TChild>(true, key);
        }

        public static bool IsRegister<TParent, TChild>(string key = null)
        {
            var tptype = typeof(TParent);
            if (string.IsNullOrWhiteSpace(key))
            {
                key = typeof(TParent).FullName;
            }

            if (Container.IsRegistered(tptype, key))
            {
                return true;
            }
            return false;
        }

        private static bool Register<TParent, TChild>(bool isnew, string key = null) where TChild : TParent
        {
            var tptype = typeof(TParent);
            if (string.IsNullOrWhiteSpace(key))
            {
                key = typeof(TParent).FullName;
            }

            if (Container.IsRegistered(tptype, key))
            {
                return false;
            }

            if (isnew)
            {
                Container.RegisterType<TParent, TChild>(key, new PerResolveLifetimeManager());
            }
            else
            {
                Container.RegisterType<TParent, TChild>(key, new ContainerControlledLifetimeManager());
            }

            return true;
        }

        /// <summary>
        /// 获取注册类型，未注册类型返回 null，已注册类型无法返回时 如可创建直接创建
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetInstance<T>(string key = null)
        {
            var ttype = typeof(T);
            if (string.IsNullOrWhiteSpace(key))
            {
                key = ttype.FullName;
            }

            if (Container.IsRegistered(ttype, key))
            {
                return ServiceLocator.Current.GetInstance<T>(key);
            }
            return default(T);
        }

        public static T GetElement<T>(string key = null) where T : FrameworkElement
        {
            var ttype = typeof(T);
            if (string.IsNullOrWhiteSpace(key))
            {
                key = ttype.FullName;
            }

            if (Container.IsRegistered(typeof(FrameworkElement), key))
            {
                return ServiceLocator.Current.GetInstance<FrameworkElement>(key) as T;
            }
            return default(T);

        }

        public static IEnumerable<object> GetAllInstances(Type serviceType)
        {
            var exports = ServiceLocator.Current.GetAllInstances(serviceType);

            return exports;
        }


        public static IEnumerable<T> ResolveAll<T>(Type serviceType)
        {
            try
            {
                var exports = Container.ResolveAll(serviceType).Cast<T>().ToList();
                return exports;
            }
            catch (Exception ex)
            {
                LogX.Error(ex);
            }

            return new List<T>();
        }
    }

}