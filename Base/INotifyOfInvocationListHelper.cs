/* 迹I柳燕
 *
 * FileName:   INotifyOfInvocationListHelper.cs
 * Version:    1.0
 * Date:       2014.03.18
 * Author:     Ji
 *
 *========================================
 * @namespace  JilyHelper
 * @class      INotifyOfInvocationListHelper
 * @extends
 *
 *             WPF 扩展
 *             对于实现 INotify 接口的委托链 增 删 委托的处理
 *
 *========================================
 */

using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Ji.DataHelper;

namespace Ji.BaseData
{
    public static class INotifyOfInvocationListHelper
    {
        /// <summary> 增加委托到 _invocationList 委托列表中 ，执行重复检查 </summary>
        /// <typeparam name="T"> 具有 _invocationList 链的类型 </typeparam>
        /// <param name="data">                       具有线程通知的数据，此项为原委托链的数据类型 </param>
        /// <param name="handler">                    增加到委托链的委托 </param>
        /// <param name="isINotifyCollectionChanged"> 当前类型是否为 INotifyCollectionChanged </param>
        /// <returns> 执行增加是否成功 </returns>
        private static bool JudgeAddEventHandler<T>(this object data, Delegate handler, bool isINotifyCollectionChanged)
        {
            bool canAdd_invocation = false;

            if (data == null)
            {
                return canAdd_invocation;
            }
            try
            {
                var collectionchangedValue = handler;
                var flag = BindingFlags.Instance | BindingFlags.NonPublic;

                string changed = string.Empty;
                FieldInfo judgechanged = null;
                if (isINotifyCollectionChanged)
                {
                    changed = "CollectionChanged";
                    judgechanged = typeof(T).FindField(changed, flag);
                }
                else
                {
                    changed = "PropertyChanged";
                    judgechanged = typeof(T).FindField(changed, flag);
                }

                if (judgechanged == null)
                {
                    return canAdd_invocation;
                }

                var collectionvalue = judgechanged.GetValue(data);
                if (collectionvalue != null)
                {
                    var collectionlist = typeof(MulticastDelegate).FindField("_invocationList", flag).GetValue(collectionvalue) as object[];
                    if (collectionlist != null)
                    {
                        #region Judge Distinct

                        //In This Way To Remove The Same Handler
                        //At Fact, I Want To Remove The Handler When I Add If ( collectionlist == null)

                        var collectionlistdistinct = collectionlist.Distinct().ToList();
                        int collectioncount = collectionlist.Count() - 1;
                        for (int i = collectioncount; i >= 0; i--)
                        {
                            var item = collectionlist[i];
                            if (item != null)
                            {
                                if (collectionlistdistinct.Contains(item))
                                {
                                    collectionlistdistinct.Remove(item);
                                }
                                else
                                {
                                    Delegate collectionchangedValueTemp = null;
                                    if (isINotifyCollectionChanged)
                                    {
                                        collectionchangedValueTemp = item as NotifyCollectionChangedEventHandler;
                                    }
                                    else
                                    {
                                        collectionchangedValueTemp = item as PropertyChangedEventHandler;
                                    }

                                    if (collectionchangedValueTemp != null)
                                    {
                                        var method = typeof(T).GetEvent(changed);
                                        if (method != null)
                                        {
                                            method.RemoveEventHandler(data, collectionchangedValueTemp);
                                        }
                                    }
                                }
                            }
                        }

                        #endregion Judge Distinct

                        if (collectionlist != null && !collectionlist.Contains(collectionchangedValue))
                        {
                            canAdd_invocation = true;
                        }
                    }

                    //In The Second To Add Handle , Can Not Find The Data Of collectionlist
                    else
                    {
                        var method = typeof(T).GetEvent(changed);
                        method.RemoveEventHandler(data, collectionchangedValue);
                        method.AddEventHandler(data, collectionchangedValue);
                    }
                }
                else
                {
                    canAdd_invocation = true;
                }

                if (canAdd_invocation)
                {
                    var method = typeof(T).GetEvent(changed);
                    method.AddEventHandler(data, collectionchangedValue);
                }
            }
            catch
            {
                throw new Exception("Add Event Handler Unsuccess ...");
            }

            return canAdd_invocation;
        }

        /// <summary> 清空具有 _invocationList 的委托列表 </summary>
        /// <typeparam name="T"> 具有 _invocationList 链的类型 </typeparam>
        /// <param name="data">                       具有线程通知的数据 </param>
        /// <param name="remove_targetdata">          删除指定类型中的所有此类型注册委托 </param>
        /// <param name="isINotifyCollectionChanged"> 当前类型是否为 INotifyCollectionChanged </param>
        /// <returns> 执行删除是否成功 </returns>
        private static bool JudgeRemoveEventHandler<T>(this object data, object remove_targetdata, bool isINotifyCollectionChanged, bool forceremove = false)
        {
            if (data == null)
            {
                return false;
            }

            bool canRemove_invocation = false;

            var flag = BindingFlags.Instance | BindingFlags.NonPublic;

            string changed = string.Empty;
            FieldInfo judgechanged = null;
            if (isINotifyCollectionChanged)
            {
                changed = "CollectionChanged";
                judgechanged = typeof(T).FindField(changed, flag);
            }
            else
            {
                changed = "PropertyChanged";
                judgechanged = typeof(T).FindField(changed, flag);
            }

            if (judgechanged == null)
            {
                return canRemove_invocation;
            }

            var collectionvalue = judgechanged.GetValue(data);
            if (collectionvalue != null)
            {
                var collectionlist = typeof(MulticastDelegate).FindField("_invocationList", flag).GetValue(collectionvalue) as object[];
                if (collectionlist != null)
                {
                    int collectioncount = collectionlist.Count() - 1;
                    for (int i = collectioncount; i >= 0; i--)
                    {
                        var item = collectionlist[i];
                        if (item != null)
                        {
                            var target = typeof(Delegate).FindField("_target", flag).GetValue(item);
                            bool canRemove = target == null || target.Equals(remove_targetdata);
                            if (forceremove || canRemove)
                            {
                                Delegate collectionchangedValueTemp = null;
                                if (isINotifyCollectionChanged)
                                {
                                    collectionchangedValueTemp = item as NotifyCollectionChangedEventHandler;
                                }
                                else
                                {
                                    collectionchangedValueTemp = item as PropertyChangedEventHandler;
                                }

                                if (collectionchangedValueTemp == null)
                                {
                                    return canRemove_invocation;
                                }

                                var method = typeof(T).GetEvent(changed);
                                method.RemoveEventHandler(data, collectionchangedValueTemp);
                                canRemove_invocation = true;
                            }
                        }
                    }
                }
            }

            return canRemove_invocation;
        }

        /// <summary> 对 INotifyPropertyChanged 的委托链增加委托函数 </summary>
        /// <typeparam name="T"> 具有线程通知的传入类型 </typeparam>
        /// <param name="property"> 实现线程通知的类型 </param>
        /// <param name="handler">  将要增加的委托 </param>
        /// <returns> 是否增加成功 </returns>
        public static bool AddPropertyEventHandler<T>(this INotifyPropertyChanged property, PropertyChangedEventHandler handler)
        {
            return JudgeAddEventHandler<T>(property, handler, false);
        }

        /// <summary> 对 INotifyCollectionChanged 的委托链增加委托函数 </summary>
        /// <typeparam name="T"> 具有线程通知的传入类型 </typeparam>
        /// <param name="collection"> 实现线程通知的类型 </param>
        /// <param name="handler">    将要增加的委托 </param>
        /// <returns> 是否增加成功 </returns>
        public static bool AddCollectionEventHandler<T>(this INotifyCollectionChanged collection, NotifyCollectionChangedEventHandler handler)
        {
            return JudgeAddEventHandler<T>(collection, handler, true);
        }

        /// <summary> 对 INotifyPropertyChanged 的委托链删除指定Target的委托函数 </summary>
        /// <typeparam name="T"> 具有线程通知的传入类型 </typeparam>
        /// <param name="property">          将要增加的委托 </param>
        /// <param name="remove_targetdata"></param>
        /// <returns> 是否删除成功 </returns>
        public static bool RemovePropertyEventHandler<T>(this INotifyPropertyChanged property, object remove_targetdata)
        {
            return JudgeRemoveEventHandler<T>(property, remove_targetdata, false);
        }

        /// <summary> 对 INotifyCollectionChanged 的委托链删除指定Target的委托函数 </summary>
        /// <typeparam name="T"> 具有线程通知的传入类型 </typeparam>
        /// <param name="collection">        实现线程通知的类型 </param>
        /// <param name="remove_targetdata"> 将要增加的委托 </param>
        /// <returns> 是否删除成功 </returns>
        public static bool RemoveCollectionEventHandler<T>(this INotifyCollectionChanged collection, object remove_targetdata)
        {
            return JudgeRemoveEventHandler<T>(collection, remove_targetdata, true);
        }

        public static bool RemoveAllCollectionEventHandler<T>(this INotifyCollectionChanged collection)
        {
            return JudgeRemoveEventHandler<T>(collection, null, true);
        }
    }
}