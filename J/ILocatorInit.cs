/* 迹I柳燕
 *
 * FileName:   ILocatorInit.cs
 * Version:    1.0
 * Date:       2018/11/23 14:47:03
 * Author:     迹
 *
 *========================================
 *
 * @namespace  Ji.CommonHelper.J
 * @interface      ILocatorInit
 * @extends
 *
 *========================================
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Ji.Base;
using Unity;

namespace System
{
    /// <summary>  </summary>
    public interface ILocatorInit
    {
        void Register(IUnityContainer container);

        void Init();
    }

    public interface IExporter
    {
        string SignKey { get; }
    }

    public abstract class LocatorInit<I> : ILocatorInit
        where I : IExporter
    {
        public virtual void Register(IUnityContainer container)
        {
            var ass = GetExportAssemblys();
            ass.Add(GetCurrentAssembly());

            foreach (var asi in ass)
            {
                var ics = asi.GetExports<I>();
                foreach (var item in ics)
                {
                    container.RegisterType(typeof(I), item.GetType(), item.SignKey);
                }
            }
        }

        public virtual List<Assembly> GetExportAssemblys()
        {
            return new List<Assembly> { };
        }

        protected Assembly GetCurrentAssembly()
        {
            return this.GetType().Assembly;
        }

        public abstract void Init();
    }
}
