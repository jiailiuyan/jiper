/* 迹I柳燕
 *
 * FileName:   JiptionAttribute.cs
 * Version:    1.0
 * Date:       2016/11/14 11:24:47
 * Author:     迹
 *
 *========================================
 *
 * @namespace  Ji.CommonHelper.Attributes
 * @class      JiptionAttribute
 * @extends
 *
 *========================================
 *
 */

using System;

namespace Ji.CommonHelper.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class JiptionAttribute : Attribute
    {
        #region Fields

        /// <summary> 扩展 .Net 2.0 中的 Description 等同 3.5之上的 DescriptionAttribute </summary>
        public static readonly JiptionAttribute Default = new JiptionAttribute();

        private string description;

        #endregion Fields

        #region Constructors

        /// <summary> 扩展 .Net 2.0 中的 Description 等同 3.5之上的 DescriptionAttribute </summary>
        public JiptionAttribute() : this(string.Empty)
        {
        }

        /// <summary> 扩展 .Net 2.0 中的 Description 等同 3.5之上的 DescriptionAttribute </summary>
        public JiptionAttribute(string description)
        {
            this.description = description;
        }

        #endregion Constructors

        #region Properties

        /// <summary> 扩展 .Net 2.0 中的 Description 等同 3.5之上的 DescriptionAttribute </summary>
        public virtual string Jiption
        {
            get
            {
                return this.DescriptionValue;
            }
        }

        /// <summary> 扩展 .Net 2.0 中的 Description 等同 3.5之上的 DescriptionAttribute </summary>
        protected string DescriptionValue
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary> 扩展 .Net 2.0 中的 Description 等同 3.5之上的 DescriptionAttribute </summary>
        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            JiptionAttribute descriptionAttribute = obj as JiptionAttribute;
            return descriptionAttribute != null && descriptionAttribute.Jiption == this.Jiption;
        }

        /// <summary> 扩展 .Net 2.0 中的 Description 等同 3.5之上的 DescriptionAttribute </summary>
        public override int GetHashCode()
        {
            return this.Jiption.GetHashCode();
        }

        /// <summary> 扩展 .Net 2.0 中的 Description 等同 3.5之上的 DescriptionAttribute </summary>
        public override bool IsDefaultAttribute()
        {
            return this.Equals(JiptionAttribute.Default);
        }

        #endregion Methods
    }
}