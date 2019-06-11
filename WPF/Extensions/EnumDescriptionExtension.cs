/* 迹I柳燕
 *
 * FileName:   EnumDescriptionExtension.cs
 * Version:    1.0
 * Date:       2017/8/9 17:50:39
 * Author:     迹
 *
 *========================================
 *
 * @namespace  Ji.CommonHelper.WPF.Extensions
 * @class      EnumDescriptionExtension
 * @extends
 *
 *========================================
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;

namespace Ji.CommonHelper.WPF.Extensions
{
    /// <summary> {local:EnumValues local:EmployeeType} </summary>

    [MarkupExtensionReturnType(typeof(object[]))]
    public class EnumDescriptionExtension : MarkupExtension
    {
        public EnumDescriptionExtension()
        {
        }

        public EnumDescriptionExtension(Type enumType)
        {
            this.EnumType = enumType;
        }

        [ConstructorArgument("enumType")]
        public Type EnumType { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (this.EnumType != null)
            {
                return Enum.GetValues(this.EnumType);
            }
            return string.Empty;
        }
    }
}