// ***********************************************************************
// 程序集         : Ksy.Client.CommonHelper
// 作者           : 刘晓青
// 创建日期       : 01-09-2018
//
// 最后编辑人员   : 刘晓青
// 最后编辑日期   : 01-09-2018
// ***********************************************************************
// <copyright file="ModelBase.cs" company="">
// </copyright>
// <summary></summary>
// ***********************************************************************
using GalaSoft.MvvmLight;

namespace Ji.CommonHelper.Model
{
    /// <summary>
    /// Class ModelBase.
    /// </summary>
    /// <typeparam name="T1">The type of the t1.</typeparam>
    public class ModelBase<T1> : ObservableObject
           where T1 : class
    {
        #region Public 构造函数

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBase{T1}"/> class.
        /// </summary>
        public ModelBase()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBase{T1}"/> class.
        /// </summary>
        /// <param name="t1">The t1.</param>
        public ModelBase(T1 t1)
        {
            this.T1_Entity = t1;
        }

        #endregion Public 构造函数

        #region Public 属性

        /// <summary>
        /// Gets or sets the T1_ entity.
        /// </summary>
        /// <value>The T1_ entity.</value>
        public T1 T1_Entity { get; set; }

        #endregion Public 属性

        #region Public 方法
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns>ModelBase&lt;T1&gt;.</returns>
        public ModelBase<T1> GetEntitys()
        {
            return this;
        }

        #endregion Public 方法
    }

    /// <summary>
    /// Class ModelBase.
    /// </summary>
    /// <typeparam name="T1">The type of the t1.</typeparam>
    /// <typeparam name="T2">The type of the t2.</typeparam>
    /// <seealso cref="Ji.CommonHelper.Model.ModelBase{T1}" />
    public class ModelBase<T1, T2> : ModelBase<T1>
           where T1 : class
           where T2 : class
    {
        #region Public 构造函数

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBase{T1, T2}"/> class.
        /// </summary>
        public ModelBase()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBase{T1, T2}"/> class.
        /// </summary>
        /// <param name="t1">The t1.</param>
        /// <param name="t2">The t2.</param>
        public ModelBase(T1 t1, T2 t2) :
            base(t1)
        {
            this.T2_Entity = t2;
        }

        #endregion Public 构造函数

        #region Public 属性

        /// <summary>
        /// Gets or sets the T2_ entity.
        /// </summary>
        /// <value>The T2_ entity.</value>
        public T2 T2_Entity { get; set; }

        #endregion Public 属性

        #region Public 方法

        /// <summary>
        /// 获取实体.
        /// </summary>
        /// <returns>ModelBase&lt;T1, T2&gt;.</returns>
        public new ModelBase<T1, T2> GetEntitys()
        {
            return this;
        }

        #endregion Public 方法
    }

    /// <summary>
    /// Class ModelBase.
    /// </summary>
    /// <typeparam name="T1">The type of the t1.</typeparam>
    /// <typeparam name="T2">The type of the t2.</typeparam>
    /// <typeparam name="T3">The type of the t3.</typeparam>
    /// <seealso cref="Ji.CommonHelper.Model.ModelBase{T1, T2}" />
    public class ModelBase<T1, T2, T3> : ModelBase<T1, T2>
           where T1 : class
           where T2 : class
           where T3 : class
    {
        #region Public 构造函数

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBase{T1, T2, T3}"/> class.
        /// </summary>
        public ModelBase()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBase{T1, T2, T3}"/> class.
        /// </summary>
        /// <param name="t1">The t1.</param>
        /// <param name="t2">The t2.</param>
        /// <param name="t3">The t3.</param>
        public ModelBase(T1 t1, T2 t2, T3 t3) :
            base(t1, t2)
        {
            this.T3_Entity = t3;
        }

        #endregion Public 构造函数

        #region Public 属性

        /// <summary>
        /// Gets or sets the T3_ entity.
        /// </summary>
        /// <value>The T3_ entity.</value>
        public T3 T3_Entity { get; set; }

        #endregion Public 属性

        #region Public 方法

        /// <summary>
        /// 获取实体.
        /// </summary>
        /// <returns>ModelBase&lt;T1, T2, T3&gt;.</returns>
        public new ModelBase<T1, T2, T3> GetEntitys()
        {
            return this;
        }

        #endregion Public 方法
    }

    /// <summary>
    /// Class ModelBase.
    /// </summary>
    /// <typeparam name="T1">The type of the t1.</typeparam>
    /// <typeparam name="T2">The type of the t2.</typeparam>
    /// <typeparam name="T3">The type of the t3.</typeparam>
    /// <typeparam name="T4">The type of the t4.</typeparam>
    /// <seealso cref="Ji.CommonHelper.Model.ModelBase{T1, T2, T3}" />
    public class ModelBase<T1, T2, T3, T4> : ModelBase<T1, T2, T3>
           where T1 : class
           where T2 : class
           where T3 : class
           where T4 : class
    {
        #region Public 构造函数

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBase{T1, T2, T3, T4}"/> class.
        /// </summary>
        public ModelBase()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBase{T1, T2, T3, T4}"/> class.
        /// </summary>
        /// <param name="t1">The t1.</param>
        /// <param name="t2">The t2.</param>
        /// <param name="t3">The t3.</param>
        /// <param name="t4">The t4.</param>
        public ModelBase(T1 t1, T2 t2, T3 t3, T4 t4) :
            base(t1, t2, t3)
        {
            this.T4_Entity = t4;
        }

        #endregion Public 构造函数

        #region Public 属性

        /// <summary>
        /// Gets or sets the T4_ entity.
        /// </summary>
        /// <value>The T4_ entity.</value>
        public T4 T4_Entity { get; set; }

        #endregion Public 属性

        #region Public 方法

        /// <summary>
        /// 获取实体.
        /// </summary>
        /// <returns>ModelBase&lt;T1, T2, T3, T4&gt;.</returns>
        public new ModelBase<T1, T2, T3, T4> GetEntitys()
        {
            return this;
        }

        #endregion Public 方法
    }

    /// <summary>
    /// Class ModelBase.
    /// </summary>
    /// <typeparam name="T1">The type of the t1.</typeparam>
    /// <typeparam name="T2">The type of the t2.</typeparam>
    /// <typeparam name="T3">The type of the t3.</typeparam>
    /// <typeparam name="T4">The type of the t4.</typeparam>
    /// <typeparam name="T5">The type of the t5.</typeparam>
    /// <seealso cref="Ji.CommonHelper.Model.ModelBase{T1, T2, T3, T4}" />
    public class ModelBase<T1, T2, T3, T4, T5> : ModelBase<T1, T2, T3, T4>
           where T1 : class
           where T2 : class
           where T3 : class
           where T4 : class
           where T5 : class
    {
        #region Public 构造函数

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBase{T1, T2, T3, T4, T5}"/> class.
        /// </summary>
        /// <param name="t1">The t1.</param>
        /// <param name="t2">The t2.</param>
        /// <param name="t3">The t3.</param>
        /// <param name="t4">The t4.</param>
        /// <param name="t5">The t5.</param>
        public ModelBase(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5) :
            base(t1, t2, t3, t4)
        {
            this.T5_Entity = t5;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBase{T1, T2, T3, T4, T5}"/> class.
        /// </summary>
        public ModelBase()
        {
        }

        #endregion Public 构造函数

        #region Public 属性

        /// <summary>
        /// Gets or sets the T5_ entity.
        /// </summary>
        /// <value>The T5_ entity.</value>
        public T5 T5_Entity { get; set; }

        #endregion Public 属性

        #region Public 方法

        /// <summary>
        /// 获取实体.
        /// </summary>
        /// <returns>ModelBase&lt;T1, T2, T3, T4, T5&gt;.</returns>
        public new ModelBase<T1, T2, T3, T4, T5> GetEntitys()
        {
            return this;
        }

        #endregion Public 方法
    }

    /// <summary>
    /// Class ModelBase.
    /// </summary>
    /// <typeparam name="T1">The type of the t1.</typeparam>
    /// <typeparam name="T2">The type of the t2.</typeparam>
    /// <typeparam name="T3">The type of the t3.</typeparam>
    /// <typeparam name="T4">The type of the t4.</typeparam>
    /// <typeparam name="T5">The type of the t5.</typeparam>
    /// <typeparam name="T6">The type of the t6.</typeparam>
    /// <seealso cref="Ji.CommonHelper.Model.ModelBase{T1, T2, T3, T4, T5}" />
    public class ModelBase<T1, T2, T3, T4, T5, T6> : ModelBase<T1, T2, T3, T4, T5>
           where T1 : class
           where T2 : class
           where T3 : class
           where T4 : class
           where T5 : class
           where T6 : class
    {
        #region Public 构造函数

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBase{T1, T2, T3, T4, T5, T6}"/> class.
        /// </summary>
        /// <param name="t1">The t1.</param>
        /// <param name="t2">The t2.</param>
        /// <param name="t3">The t3.</param>
        /// <param name="t4">The t4.</param>
        /// <param name="t5">The t5.</param>
        /// <param name="t6">The t6.</param>
        public ModelBase(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6) :
            base(t1, t2, t3, t4, t5)
        {
            this.T6_Entity = t6;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBase{T1, T2, T3, T4, T5, T6}"/> class.
        /// </summary>
        public ModelBase()
        {
        }

        #endregion Public 构造函数

        #region Public 属性

        /// <summary>
        /// Gets or sets the T6_ entity.
        /// </summary>
        /// <value>The T6_ entity.</value>
        public T6 T6_Entity { get; set; }

        #endregion Public 属性

        #region Public 方法

        /// <summary>
        /// 获取实体.
        /// </summary>
        /// <returns>ModelBase&lt;T1, T2, T3, T4, T5, T6&gt;.</returns>
        public new ModelBase<T1, T2, T3, T4, T5, T6> GetEntitys()
        {
            return this;
        }

        #endregion Public 方法
    }

    /// <summary>
    /// Class ModelBase.
    /// </summary>
    /// <typeparam name="T1">The type of the t1.</typeparam>
    /// <typeparam name="T2">The type of the t2.</typeparam>
    /// <typeparam name="T3">The type of the t3.</typeparam>
    /// <typeparam name="T4">The type of the t4.</typeparam>
    /// <typeparam name="T5">The type of the t5.</typeparam>
    /// <typeparam name="T6">The type of the t6.</typeparam>
    /// <typeparam name="T7">The type of the t7.</typeparam>
    /// <seealso cref="Ji.CommonHelper.Model.ModelBase{T1, T2, T3, T4, T5, T6}" />
    public class ModelBase<T1, T2, T3, T4, T5, T6, T7> : ModelBase<T1, T2, T3, T4, T5, T6>
           where T1 : class
           where T2 : class
           where T3 : class
           where T4 : class
           where T5 : class
           where T6 : class
           where T7 : class
    {
        #region Public 构造函数

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBase{T1, T2, T3, T4, T5, T6, T7}"/> class.
        /// </summary>
        public ModelBase()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBase{T1, T2, T3, T4, T5, T6, T7}"/> class.
        /// </summary>
        /// <param name="t1">The t1.</param>
        /// <param name="t2">The t2.</param>
        /// <param name="t3">The t3.</param>
        /// <param name="t4">The t4.</param>
        /// <param name="t5">The t5.</param>
        /// <param name="t6">The t6.</param>
        /// <param name="t7">The t7.</param>
        public ModelBase(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7) :
            base(t1, t2, t3, t4, t5, t6)
        {
            this.T7_Entity = t7;
        }

        #endregion Public 构造函数

        #region Public 属性

        /// <summary>
        /// Gets or sets the T7_ entity.
        /// </summary>
        /// <value>The T7_ entity.</value>
        public T7 T7_Entity { get; set; }

        #endregion Public 属性

        #region Public 方法

        /// <summary>
        /// 获取实体.
        /// </summary>
        /// <returns>ModelBase&lt;T1, T2, T3, T4, T5, T6, T7&gt;.</returns>
        public new ModelBase<T1, T2, T3, T4, T5, T6, T7> GetEntitys()
        {
            return this;
        }

        #endregion Public 方法
    }

    /// <summary>
    /// Class ModelBase.
    /// </summary>
    /// <typeparam name="T1">The type of the t1.</typeparam>
    /// <typeparam name="T2">The type of the t2.</typeparam>
    /// <typeparam name="T3">The type of the t3.</typeparam>
    /// <typeparam name="T4">The type of the t4.</typeparam>
    /// <typeparam name="T5">The type of the t5.</typeparam>
    /// <typeparam name="T6">The type of the t6.</typeparam>
    /// <typeparam name="T7">The type of the t7.</typeparam>
    /// <typeparam name="T8">The type of the t8.</typeparam>
    /// <seealso cref="Ji.CommonHelper.Model.ModelBase{T1, T2, T3, T4, T5, T6, T7}" />
    public class ModelBase<T1, T2, T3, T4, T5, T6, T7, T8> : ModelBase<T1, T2, T3, T4, T5, T6, T7>
           where T1 : class
           where T2 : class
           where T3 : class
           where T4 : class
           where T5 : class
           where T6 : class
           where T7 : class
           where T8 : class
    {
        #region Public 构造函数

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBase{T1, T2, T3, T4, T5, T6, T7, T8}"/> class.
        /// </summary>
        public ModelBase()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBase{T1, T2, T3, T4, T5, T6, T7, T8}"/> class.
        /// </summary>
        /// <param name="t1">The t1.</param>
        /// <param name="t2">The t2.</param>
        /// <param name="t3">The t3.</param>
        /// <param name="t4">The t4.</param>
        /// <param name="t5">The t5.</param>
        /// <param name="t6">The t6.</param>
        /// <param name="t7">The t7.</param>
        /// <param name="t8">The t8.</param>
        public ModelBase(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8) :
            base(t1, t2, t3, t4, t5, t6, t7)
        {
            this.T8_Entity = t8;
        }

        #endregion Public 构造函数

        #region Public 属性

        /// <summary>
        /// Gets or sets the T8_ entity.
        /// </summary>
        /// <value>The T8_ entity.</value>
        public T8 T8_Entity { get; set; }

        #endregion Public 属性

        #region Public 方法

        /// <summary>
        /// 获取实体.
        /// </summary>
        /// <returns>ModelBase&lt;T1, T2, T3, T4, T5, T6, T7, T8&gt;.</returns>
        public new ModelBase<T1, T2, T3, T4, T5, T6, T7, T8> GetEntitys()
        {
            return this;
        }

        #endregion Public 方法
    }

    /// <summary>
    /// Class ModelBase.
    /// </summary>
    /// <typeparam name="T1">The type of the t1.</typeparam>
    /// <typeparam name="T2">The type of the t2.</typeparam>
    /// <typeparam name="T3">The type of the t3.</typeparam>
    /// <typeparam name="T4">The type of the t4.</typeparam>
    /// <typeparam name="T5">The type of the t5.</typeparam>
    /// <typeparam name="T6">The type of the t6.</typeparam>
    /// <typeparam name="T7">The type of the t7.</typeparam>
    /// <typeparam name="T8">The type of the t8.</typeparam>
    /// <typeparam name="T9">The type of the t9.</typeparam>
    /// <seealso cref="Ji.CommonHelper.Model.ModelBase{T1, T2, T3, T4, T5, T6, T7, T8}" />
    public class ModelBase<T1, T2, T3, T4, T5, T6, T7, T8, T9> : ModelBase<T1, T2, T3, T4, T5, T6, T7, T8>
           where T1 : class
           where T2 : class
           where T3 : class
           where T4 : class
           where T5 : class
           where T6 : class
           where T7 : class
           where T8 : class
           where T9 : class
    {
        #region Public 构造函数

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBase{T1, T2, T3, T4, T5, T6, T7, T8, T9}"/> class.
        /// </summary>
        public ModelBase()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBase{T1, T2, T3, T4, T5, T6, T7, T8, T9}"/> class.
        /// </summary>
        /// <param name="t1">The t1.</param>
        /// <param name="t2">The t2.</param>
        /// <param name="t3">The t3.</param>
        /// <param name="t4">The t4.</param>
        /// <param name="t5">The t5.</param>
        /// <param name="t6">The t6.</param>
        /// <param name="t7">The t7.</param>
        /// <param name="t8">The t8.</param>
        /// <param name="t9">The t9.</param>
        public ModelBase(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9) :
            base(t1, t2, t3, t4, t5, t6, t7, t8)
        {
            this.T9_Entity = t9;
        }

        #endregion Public 构造函数

        #region Public 属性

        /// <summary>
        /// Gets or sets the T9_ entity.
        /// </summary>
        /// <value>The T9_ entity.</value>
        public T9 T9_Entity { get; set; }

        #endregion Public 属性

        #region Public 方法

        /// <summary>
        /// 获取实体.
        /// </summary>
        /// <returns>ModelBase&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9&gt;.</returns>
        public new ModelBase<T1, T2, T3, T4, T5, T6, T7, T8, T9> GetEntitys()
        {
            return this;
        }

        #endregion Public 方法
    }

    /// <summary>
    /// Class ModelBase.
    /// </summary>
    /// <typeparam name="T1">The type of the t1.</typeparam>
    /// <typeparam name="T2">The type of the t2.</typeparam>
    /// <typeparam name="T3">The type of the t3.</typeparam>
    /// <typeparam name="T4">The type of the t4.</typeparam>
    /// <typeparam name="T5">The type of the t5.</typeparam>
    /// <typeparam name="T6">The type of the t6.</typeparam>
    /// <typeparam name="T7">The type of the t7.</typeparam>
    /// <typeparam name="T8">The type of the t8.</typeparam>
    /// <typeparam name="T9">The type of the t9.</typeparam>
    /// <typeparam name="T10">The type of the T10.</typeparam>
    /// <seealso cref="Ji.CommonHelper.Model.ModelBase{T1, T2, T3, T4, T5, T6, T7, T8, T9}" />
    public class ModelBase<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : ModelBase<T1, T2, T3, T4, T5, T6, T7, T8, T9>
          where T1 : class
          where T2 : class
          where T3 : class
          where T4 : class
          where T5 : class
          where T6 : class
          where T7 : class
          where T8 : class
          where T9 : class
          where T10 : class
    {
        #region Public 构造函数

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBase{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10}"/> class.
        /// </summary>
        public ModelBase()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBase{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10}"/> class.
        /// </summary>
        /// <param name="t1">The t1.</param>
        /// <param name="t2">The t2.</param>
        /// <param name="t3">The t3.</param>
        /// <param name="t4">The t4.</param>
        /// <param name="t5">The t5.</param>
        /// <param name="t6">The t6.</param>
        /// <param name="t7">The t7.</param>
        /// <param name="t8">The t8.</param>
        /// <param name="t9">The t9.</param>
        /// <param name="t10">The T10.</param>
        public ModelBase(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10) :
            base(t1, t2, t3, t4, t5, t6, t7, t8, t9)
        {
            this.T10_Entity = t10;
        }

        #endregion Public 构造函数

        #region Public 属性

        /// <summary>
        /// Gets or sets the T10_ entity.
        /// </summary>
        /// <value>The T10_ entity.</value>
        public T10 T10_Entity { get; set; }

        #endregion Public 属性

        #region Public 方法

        /// <summary>
        /// 获取实体.
        /// </summary>
        /// <returns>ModelBase&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10&gt;.</returns>
        public new ModelBase<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> GetEntitys()
        {
            return this;
        }

        #endregion Public 方法
    }

    /// <summary>
    /// Class ModelBase.
    /// </summary>
    /// <typeparam name="T1">The type of the t1.</typeparam>
    /// <typeparam name="T2">The type of the t2.</typeparam>
    /// <typeparam name="T3">The type of the t3.</typeparam>
    /// <typeparam name="T4">The type of the t4.</typeparam>
    /// <typeparam name="T5">The type of the t5.</typeparam>
    /// <typeparam name="T6">The type of the t6.</typeparam>
    /// <typeparam name="T7">The type of the t7.</typeparam>
    /// <typeparam name="T8">The type of the t8.</typeparam>
    /// <typeparam name="T9">The type of the t9.</typeparam>
    /// <typeparam name="T10">The type of the T10.</typeparam>
    /// <typeparam name="T11">The type of the T11.</typeparam>
    /// <seealso cref="Ji.CommonHelper.Model.ModelBase{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10}" />
    public class ModelBase<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : ModelBase<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
        where T5 : class
        where T6 : class
        where T7 : class
        where T8 : class
        where T9 : class
        where T10 : class
        where T11 : class
    {
        #region Public 构造函数

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBase{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11}"/> class.
        /// </summary>
        public ModelBase()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBase{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11}"/> class.
        /// </summary>
        /// <param name="t1">The t1.</param>
        /// <param name="t2">The t2.</param>
        /// <param name="t3">The t3.</param>
        /// <param name="t4">The t4.</param>
        /// <param name="t5">The t5.</param>
        /// <param name="t6">The t6.</param>
        /// <param name="t7">The t7.</param>
        /// <param name="t8">The t8.</param>
        /// <param name="t9">The t9.</param>
        /// <param name="t10">The T10.</param>
        /// <param name="t11">The T11.</param>
        public ModelBase(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11) :
            base(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10)
        {
            this.T11_Entity = t11;
        }

        #endregion Public 构造函数

        #region Public 属性

        /// <summary>
        /// Gets or sets the T11_ entity.
        /// </summary>
        /// <value>The T11_ entity.</value>
        public T11 T11_Entity { get; set; }

        #endregion Public 属性

        #region Public 方法

        /// <summary>
        /// 获取实体.
        /// </summary>
        /// <returns>ModelBase&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11&gt;.</returns>
        public new ModelBase<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> GetEntitys()
        {
            return this;
        }

        #endregion Public 方法
    }

    /// <summary>
    /// Class ModelBase.
    /// </summary>
    /// <typeparam name="T1">The type of the t1.</typeparam>
    /// <typeparam name="T2">The type of the t2.</typeparam>
    /// <typeparam name="T3">The type of the t3.</typeparam>
    /// <typeparam name="T4">The type of the t4.</typeparam>
    /// <typeparam name="T5">The type of the t5.</typeparam>
    /// <typeparam name="T6">The type of the t6.</typeparam>
    /// <typeparam name="T7">The type of the t7.</typeparam>
    /// <typeparam name="T8">The type of the t8.</typeparam>
    /// <typeparam name="T9">The type of the t9.</typeparam>
    /// <typeparam name="T10">The type of the T10.</typeparam>
    /// <typeparam name="T11">The type of the T11.</typeparam>
    /// <typeparam name="T12">The type of the T12.</typeparam>
    /// <seealso cref="Ji.CommonHelper.Model.ModelBase{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11}" />
    public class ModelBase<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : ModelBase<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
       where T1 : class
       where T2 : class
       where T3 : class
       where T4 : class
       where T5 : class
       where T6 : class
       where T7 : class
       where T8 : class
       where T9 : class
       where T10 : class
       where T11 : class
       where T12 : class
    {
        #region Public 构造函数

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBase{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12}"/> class.
        /// </summary>
        public ModelBase()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBase{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12}"/> class.
        /// </summary>
        /// <param name="t1">The t1.</param>
        /// <param name="t2">The t2.</param>
        /// <param name="t3">The t3.</param>
        /// <param name="t4">The t4.</param>
        /// <param name="t5">The t5.</param>
        /// <param name="t6">The t6.</param>
        /// <param name="t7">The t7.</param>
        /// <param name="t8">The t8.</param>
        /// <param name="t9">The t9.</param>
        /// <param name="t10">The T10.</param>
        /// <param name="t11">The T11.</param>
        /// <param name="t12">The T12.</param>
        public ModelBase(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12) :
            base(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11)
        {
            this.T12_Entity = t12;
        }

        #endregion Public 构造函数

        #region Public 属性

        /// <summary>
        /// Gets or sets the T12_ entity.
        /// </summary>
        /// <value>The T12_ entity.</value>
        public T12 T12_Entity { get; set; }

        #endregion Public 属性

        #region Public 方法

        /// <summary>
        /// 获取实体.
        /// </summary>
        /// <returns>ModelBase&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12&gt;.</returns>
        public new ModelBase<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> GetEntitys()
        {
            return this;
        }

        #endregion Public 方法
    }

    /// <summary>
    /// Class ABC1.
    /// </summary>
    internal class ABC1
    {
        #region Public 构造函数

        /// <summary>
        /// Initializes a new instance of the <see cref="ABC1"/> class.
        /// </summary>
        public ABC1()
        {
        }

        #endregion Public 构造函数

        #region Public 属性

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int ID { get; set; }

        #endregion Public 属性
    }

    /// <summary>
    /// Class ABC2.
    /// </summary>
    internal class ABC2
    {
        #region Public 构造函数

        /// <summary>
        /// Initializes a new instance of the <see cref="ABC2"/> class.
        /// </summary>
        public ABC2()
        {
        }

        #endregion Public 构造函数
    }

    /// <summary>
    /// Class ABC3.
    /// </summary>
    internal class ABC3
    {
        #region Public 构造函数

        /// <summary>
        /// Initializes a new instance of the <see cref="ABC3"/> class.
        /// </summary>
        public ABC3()
        {
        }

        #endregion Public 构造函数
    }

    /// <summary>
    /// Class BCD.
    /// </summary>
    /// <seealso cref="Ji.CommonHelper.Model.ModelBase{Ji.CommonHelper.Model.ABC1, Ji.CommonHelper.Model.ABC2, Ji.CommonHelper.Model.ABC3}" />
    internal class BCD : ModelBase<ABC1, ABC2, ABC3>
    {
        #region Public 构造函数

        /// <summary>
        /// Initializes a new instance of the <see cref="BCD"/> class.
        /// </summary>
        public BCD()
        {
            this.T1_Entity = new ABC1();
            this.T2_Entity = new ABC2();
            this.T3_Entity = new ABC3();
            var ens = this.GetEntitys();
        }

        #endregion Public 构造函数

        #region Public 属性

        /// <summary>
        /// Gets or sets the ab C1_ identifier.
        /// </summary>
        /// <value>The ab C1_ identifier.</value>
        public int ABC1_ID
        {
            get { return this.T1_Entity.ID; }
            set { this.T1_Entity.ID = value; }
        }

        #endregion Public 属性
    }
}