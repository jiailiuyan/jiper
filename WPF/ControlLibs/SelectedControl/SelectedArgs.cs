using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace SelectedControl
{

    public enum SelctedType
    {
        PointSelected,
        RectSelected
    }

    public class SelectedArgs : EventArgs
    {
        public ObservableCollection<ISelectedItem> SelectedItems { get; set; }

        public SelctedType Type { get; set; }

        public SelectedArgs(ObservableCollection<ISelectedItem> items, SelctedType type)
        {
            this.SelectedItems = items;
            this.Type = type;
        }

    }
}
