using System.Globalization;
using System.Windows.Controls;

namespace Ji.CommonHelper.WPF.Validatas
{
    public class KeepTrueValidationRule : ValidationRule
    {

        private static KeepTrueValidationRule instance;

        private const string KeepTrueValidationRuleLockStr = "KeepTrueValidationRuleLockStr";

        /// <summary> 获取 KeepTrueValidationRule 单例 </summary>
        public static KeepTrueValidationRule Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (KeepTrueValidationRuleLockStr)
                    {
                        if (instance == null)
                        {
                            instance = new KeepTrueValidationRule();
                        }
                    }
                }
                return instance;
            }
        }


        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return new ValidationResult(true, null);
        }
    }
}
