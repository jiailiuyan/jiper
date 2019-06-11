/* 迹I柳燕
 *
 * FileName:   LengthValidataAttribute.cs
 * Version:    1.0
 * Date:       2017/11/30 迹 10:33:42
 * Author:     迹
 *
 *========================================
 *
 * @namespace  Ji.CommonHelper.WPF.Validatas
 * @class      LengthValidataAttribute
 * @extends
 *
 *========================================
 *
 */

using System.ComponentModel.DataAnnotations;
using Ji.DataHelper;

namespace Ji.CommonHelper.WPF.Validatas
{
    /// <summary></summary>
    public class LengthValidataAttribute : ValidationAttribute
    {
        #region Fields

        private int _MinLength = 1;

        private string _Tip = null;

        #endregion Fields

        #region Properties

        public int MaxLength { get; set; }

        public int MinLength
        {
            get { return _MinLength; }
            set { _MinLength = value; }
        }

        #endregion Properties

        #region Constructors

        public LengthValidataAttribute(string tip, int maxl)
        {
            this.Tip = tip;
            this.MaxLength = maxl;
        }

        #endregion Constructors

        public string Tip
        {
            get { return _Tip; }
            set { _Tip = value; }
        }

        #region Methods

        public override string FormatErrorMessage(string name)
        {
            return Tip ?? base.FormatErrorMessage(name);
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }

            if (value is string)
            {
                var l = StringHelper.ToString(value).Trim().Length;
                return l >= this.MinLength && l <= this.MaxLength;
            }

            return true;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return base.IsValid(value, validationContext);
        }

        #endregion Methods
    }
}