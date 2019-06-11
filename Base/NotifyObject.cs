using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Ji.LinqHelper;

namespace Ji.BaseData
{
    [Serializable]
    public abstract class NotifyObject : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        protected virtual void RaisePropertyChanged()
        {
            Action raiseaction = () =>
            {
                var trace = new StackTrace();
                var methodName = trace.GetFrame(2).GetMethod();
                if (methodName.Name.StartsWith("set_"))
                {
                    var propertyName = methodName.Name.Remove(0, 4);
                    if (!string.IsNullOrWhiteSpace(propertyName))
                    {
                        this.RaisePropertyChanged(propertyName);
                    }
                }
            };

            (System.Windows.Application.Current != null && System.Windows.Application.Current.CheckAccess() ? raiseaction : () => System.Windows.Application.Current.Dispatcher.BeginInvoke(raiseaction))();
        }

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected virtual void RaisePropertyChanging(string propertyName)
        {
            if (this.PropertyChanging != null)
            {
                this.PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        /// <param name="propertyNames">The properties that have a new value.</param>
        protected void RaisePropertyChanged(params string[] propertyNames)
        {
            if (propertyNames != null)
            {
                propertyNames.ForEach(name => this.RaisePropertyChanged(name));
            }
        }

        protected void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            var propertyName = ExpressionHelper.ExtractPropertyName(propertyExpression);
            if (!string.IsNullOrWhiteSpace(propertyName))
            {
                this.RaisePropertyChanged(propertyName);
            }
        }

        protected void RaisePropertyChanging<T>(Expression<Func<T>> propertyExpression)
        {
            var propertyName = ExpressionHelper.ExtractPropertyName(propertyExpression);
            if (!string.IsNullOrWhiteSpace(propertyName))
            {
                this.RaisePropertyChanging(propertyName);
            }
        }

        /// <summary> 同步到UI线程进行依赖属性通知 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storage"></param>
        /// <param name="value"></param>
        /// <param name="issameraise"> 值相同时是否触发通知 </param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected virtual bool SetProperty<T>(ref T storage, T value, bool issameraise = false)
        {
            if (object.Equals(storage, value) && issameraise)
            {
                return false;
            }

            storage = value;

            Action raiseaction = () =>
            {
                var trace = new StackTrace();
                var methodName = trace.GetFrame(2).GetMethod();
                if (methodName.Name.StartsWith("set_"))
                {
                    var propertyName = methodName.Name.Remove(0, 4);
                    if (!string.IsNullOrWhiteSpace(propertyName))
                    {
                        this.RaisePropertyChanged(propertyName);
                    }
                }
            };

            (System.Windows.Application.Current != null && System.Windows.Application.Current.CheckAccess() ? raiseaction : () => System.Windows.Application.Current.Dispatcher.BeginInvoke(raiseaction))();

            return true;
        }
    }
}