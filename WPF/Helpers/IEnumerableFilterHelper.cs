using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Ji.CommonHelper.WPF.Helpers
{

    public interface IFilter
    {
        void FilterChange(string filtertext);
    }

    internal class IEnumerableFilter<T> : IFilter
        where T : class
    {

        Func<T, bool> FilterFunc = null;

        protected string FilterText { get; set; }

        IEnumerable Datas;

        internal IEnumerableFilter(Func<T, bool> func)
        {
            this.FilterFunc = func;
        }

        internal void InitFilter(IEnumerable data)
        {
            if (data != null)
            {
                this.Datas = data;
                var view = (CollectionView)CollectionViewSource.GetDefaultView(this.Datas);
                view.Filter = Filter;
            }
        }

        private bool Filter(object item)
        {
            if (string.IsNullOrEmpty(FilterText))
            {
                return true;
            }
            else
            {
                if (FilterFunc != null)
                {
                    return FilterFunc(item as T);
                }
                return true;
            }
        }

        public void FilterChange(string filtertext)
        {
            if (this.Datas != null)
            {
                FilterText = (filtertext + "").ToLower();
                CollectionViewSource.GetDefaultView(this.Datas).Refresh();
            }
        }
    }

    public static class IEnumerableFilterHelper
    {
        public static IFilter CreatFilter<T>(this IEnumerable data, Func<T, bool> func)
              where T : class
        {
            var filter = new IEnumerableFilter<T>(func);
            filter.InitFilter(data);
            return filter;
        }

    }
}
