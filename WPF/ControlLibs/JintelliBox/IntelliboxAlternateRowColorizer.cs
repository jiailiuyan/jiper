using System.Windows.Media;

namespace JintelliBox
{
    /// <summary>
    ///  Provides a simple way to color the even rows of a ListBox with one color and the odd rows
    ///  with another.
    /// </summary>
    public sealed class IntelliboxAlternateRowColorizer : IntelliboxRowColorizer
    {
        /// <summary> The <see cref="Brush" /> to use on even numbered rows (0,2,4,6,...) </summary>
        public Brush EvenRowBrush
        {
            get;
            set;
        }

        /// <summary> The <see cref="Brush" /> to use on odd numbered rows (1,3,5,7,...) </summary>
        public Brush OddRowBrush
        {
            get;
            set;
        }

        /// <summary>
        ///  Returns the brushes that this object wants to use to colorize the background of each row
        ///  in a ListBox control.
        /// </summary>
        /// <returns>
        ///  An array that contains the values of the <see cref="EvenRowBrush" /> and
        ///  <see cref="OddRowBrush" /> at the time of the call.
        /// </returns>
        protected override Brush[] GetBrushes()
        {
            return new[] { EvenRowBrush, OddRowBrush };
        }
    }
}