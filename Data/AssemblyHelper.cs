using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ji.DataHelper
{
    public static class AssemblyHelper
    {
        /// <summary> 获取指定程序域里所有的自定义类型 T </summary>
        /// <typeparam name="T"> 将要查询的类型 T </typeparam>
        /// <param name="currentAssembly"> 指定查询的程序集 </param>
        /// <param name="bindingAttr"> 查询的指定参数 </param>
        /// <returns> 查询到的指定程序域所含有的所有自定义类型 T </returns>
        public static IList<T> FindAllStaticTypesInAssemblyDomain<T>(this Assembly currentAssembly, BindingFlags bindingAttr = BindingFlags.Static | BindingFlags.Public) where T : class
        {
            List<T> retDatas = new List<T>();
            var referencedAssemblies = currentAssembly.GetReferencedAssemblies().ToList();

            //此处还是异步加载吧，具体有没有减少时间下回会测试一下
            referencedAssemblies.Add(currentAssembly.GetName());

            //获取当前 程序集所自定义的 T 直接增加一次
            //var datas = currentAssembly.FindStaticFieldValueInAssembly<T>(bindingAttr);
            //if (datas != null)
            //{
            //    retDatas.AddRange(datas);
            //}

            IList<T> datas = new List<T>();
            //获取当前程序集所引用的所有程序集包含的所有自定义 T
            // Ji 在此不能使用 Parallel 因为有时数组长度不够会抛出以下异常
            // 源数组长度不足。请检查 srcIndex 和长度以及数组的下限
            //Parallel.ForEach(referencedAssemblies, referencedAssembly =>
            //   {
            //       var assembly = Assembly.Load(referencedAssembly);
            //       datas = assembly.FindStaticFieldValueInAssembly<T>(bindingAttr);
            //       if (datas != null)
            //       {
            //           retDatas.AddRange(datas);
            //       }
            //   });

            referencedAssemblies.ForEach(referencedAssembly =>
               {
                   var assembly = Assembly.Load(referencedAssembly);
                   datas = assembly.FindStaticFieldValueInAssembly<T>(bindingAttr);
                   if (datas != null)
                   {
                       retDatas.AddRange(datas);
                   }
               });

            return retDatas;
        }
    }
}