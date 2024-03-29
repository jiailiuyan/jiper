using System.Windows;

namespace Ji.WPFHelper.MvvmHelper
{
    /// <summary>
    ///   xmlns:mvvm="clr-namespace:Ji.WPFHelper.MvvmHelper;assembly=aSee.CommonHelper"
    ///   mvvm:ViewModelLocator.AutoWireViewModel="True"
    /// This class defines the attached property and related change handler that calls the ViewModelLocator in Prism.Mvvm.
    /// </summary>
    public static class ViewModelLocator
    {
        /// <summary>
        /// The AutoWireViewModel attached property.
        /// </summary>
        public static DependencyProperty AutoWireViewModelProperty = DependencyProperty.RegisterAttached("AutoWireViewModel", typeof(bool), typeof(ViewModelLocator), new PropertyMetadata(false, AutoWireViewModelChanged));

        public static bool GetAutoWireViewModel(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutoWireViewModelProperty);
        }

        public static void SetAutoWireViewModel(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoWireViewModelProperty, value);
        }

        private static void AutoWireViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                ViewModelLocationProvider.AutoWireViewModelChanged(d, Bind);
            }
        }

        /// <summary>
        /// Sets the DataContext of a View
        /// </summary>
        /// <param name="view">The View to set the DataContext on</param>
        /// <param name="dataContext">The object to use as the DataContext for the View</param>
        private static void Bind(object view, object viewModel)
        {
            var element = view as FrameworkElement;
            if (element != null)
            {
                element.DataContext = viewModel;
            }
        }
    }
}