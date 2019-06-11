using System.Windows.Controls;

namespace JintelliBox
{
    /// <summary>
    ///  Represents a column that displays data from an <see cref="IIntelliboxResultsProvider" />
    ///  result set.
    /// </summary>
    public class IntelliboxColumn : GridViewColumn
    {
        /// <summary>
        ///  Associates this column with a property on a result set data row. The data row property
        ///  name that matches this string will be hidden or positioned based on the
        ///  <seealso cref="Hide" /> and <seealso cref="Position" /> values.
        /// </summary>
        public string ForProperty
        {
            get;
            set;
        }

        /// <summary>
        ///  When True, this column will not be shown in the result set. This property is useful if
        ///  you only want to hide a few columns of the result set; otherwise you're probably better
        ///  off just listing all the columns explicitly.
        /// </summary>
        public bool Hide
        {
            get;
            set;
        }

        /// <summary>
        ///  When set, controls the left-to-right position of this column in the result set. Lower
        ///  numbers sort farther to the left. NULL values sort to the right.
        /// </summary>
        public int? Position
        {
            get;
            set;
        }
    }
}