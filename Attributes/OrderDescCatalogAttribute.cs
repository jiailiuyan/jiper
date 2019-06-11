using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ji.CommonHelper.Attributes
{
    /// <summary>
    /// 排序、描述、类别特性
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class OrderDescCatalogAttribute : OrderDescriptionAttribute
    {
        public string Catalog { get; private set; }

        public OrderDescCatalogAttribute(int order, string description, string catalog = "") : base(order, description)
        {
            this.Catalog = catalog;
        }
    }
}