using System;
using System.ComponentModel;

namespace Ji.CommonHelper.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class OrderDescriptionAttribute : DescriptionAttribute
    {
        public int Order { get; private set; }

        public bool CanView { get; private set; }

        public OrderDescriptionAttribute(int order, string description, bool canview = true) : base(description)
        {
            this.CanView = canview;
            this.Order = order;
        }
    }
}