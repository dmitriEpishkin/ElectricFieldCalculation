using System.Windows;
using System.Windows.Data;

namespace Nordwest.Wpf.Controls.Tools
{
    public class DependencyPropertyNotifier : DependencyObject
    {
        public DependencyObject Target { get; private set; }

        public static readonly DependencyProperty PropertyProperty = DependencyProperty.Register(
            "Property", typeof(object), typeof(DependencyPropertyNotifier), new PropertyMetadata(default(object), (s, e) => ((DependencyPropertyNotifier)s).RaisePropertyChanged(e)));

        public object Property
        {
            get { return (object)GetValue(PropertyProperty); }
            set { SetValue(PropertyProperty, value); }
        }

        public event DependencyPropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, e);
        }

        public static DependencyPropertyNotifier On(DependencyObject target, DependencyProperty property)
        {
            var n = new DependencyPropertyNotifier();
            BindingOperations.SetBinding(n, PropertyProperty, new Binding()
            {
                Source = target,
                Path = new PropertyPath(property),
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
            n.Target = target;
            return n;
        }
    }
}