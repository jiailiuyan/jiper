/* 迹I柳燕
 *
 * FileName:   NullValidata.cs
 * Version:    1.0
 * Date:       2017/12/18 16:24:47
 * Author:     迹
 *
 *========================================
 *
 * @namespace  Ji.CommonHelper.WPF.Validatas
 * @class      NullValidata
 * @extends
 *
 *========================================
 * 
 */

using System.ComponentModel.DataAnnotations;

namespace Ji.CommonHelper.WPF.Validatas
{
    /// <summary>  </summary>
    public class NullValidata : ValidationAttribute
    {

        public NullValidata(string errorMessage) : base(errorMessage)
        {

        }

        public override bool IsValid(object value)
        {
            return value != null;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return base.IsValid(value, validationContext);
        }
    }
}
