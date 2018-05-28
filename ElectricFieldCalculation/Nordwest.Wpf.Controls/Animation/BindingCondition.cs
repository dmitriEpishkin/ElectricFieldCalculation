using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Nordwest.Wpf.Controls
{
    public class BindingCondition : ConditionBase
    {
        public Binding Binding { get; set; }

        public static readonly DependencyProperty ActualValueProperty = DependencyProperty.Register(
            "ActualValue", typeof(object), typeof(BindingCondition), new PropertyMetadata(default(object), (o, args) => ((BindingCondition)o).OnConditionChanged()));

        public object ActualValue
        {
            get { return (object)GetValue(ActualValueProperty); }
            set { SetValue(ActualValueProperty, value); }
        }

        public event EventHandler ConditionChanged;

        public override bool CheckCondition(DependencyObject host)
        {
            if (!BindingOperations.IsDataBound(this, ActualValueProperty))
            {
                if (Binding == null)
                    return false;

                var b = new Binding
                {
                    Converter = Binding.Converter,
                    ConverterCulture = Binding.ConverterCulture,
                    ConverterParameter = Binding.ConverterParameter,
                    Source = Binding.Source,
                    Path = Binding.Path,
                    Mode = BindingMode.OneWay,
                    UpdateSourceTrigger = Binding.UpdateSourceTrigger
                };

                var rs = Binding.RelativeSource;
                if (rs != null)
                {
                    if (rs.Mode == RelativeSourceMode.TemplatedParent)
                        b.Source = ((FrameworkElement)host).TemplatedParent;
                    else if (rs.Mode == RelativeSourceMode.FindAncestor)
                        b.Source = FindAncestor(host, rs.AncestorType, 0, rs.AncestorLevel);
                    else if (rs.Mode == RelativeSourceMode.Self)
                        b.Source = host;
                    else
                        throw new NotSupportedException();
                }
                BindingOperations.SetBinding(this, ActualValueProperty, b);
            }
            return Equals(ActualValue, Value);
        }

        private static DependencyObject FindAncestor(DependencyObject c, Type tt, int count, int maxCount)
        {
            while (true)
            {
                var p = VisualTreeHelper.GetParent(c);
                if (p == null || ReferenceEquals(p, c))
                    return null;

                if (tt.IsInstanceOfType(p))
                {
                    count++;
                    if (count >= maxCount)
                        return p;
                }
                c = p;
            }
        }

        protected void OnConditionChanged()
        {
            var handler = ConditionChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }
}