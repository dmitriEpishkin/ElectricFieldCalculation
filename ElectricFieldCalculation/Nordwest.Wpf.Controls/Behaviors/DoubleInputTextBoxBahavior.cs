
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Nordwest.Wpf.Controls.Behaviors {
    public static class DoubleInputTextBoxBahavior {

        public static readonly DependencyProperty On = DependencyProperty.RegisterAttached(
            "On", typeof(bool), typeof(DoubleInputTextBoxBahavior), new PropertyMetadata(false, OnChanged));

        public static bool GetOn(DependencyObject obj) {
            return (bool)obj.GetValue(On);
        }
        public static void SetOn(DependencyObject obj, bool value) {
            obj.SetValue(On, value);
        }

        private static void OnChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            var element = sender as TextBox;
            if (element == null)
                return;

            element.PreviewKeyDown -= OnPreviewKeyDown;
            element.PreviewKeyDown += OnPreviewKeyDown;
        }
        
        private static void OnPreviewKeyDown(object sender, KeyEventArgs e) {
            if (!GetOn((DependencyObject)sender))
                return;

            var textBox = (TextBox) sender;

            var ent = e.Key == Key.Enter;
            var esc = e.Key == Key.Escape;
            var number = (int) e.Key >= 34 && (int) e.Key <= 43;

            var minus = e.Key == Key.OemMinus;

            var delete = e.Key == Key.Delete || e.Key == Key.Back;

            var period = e.Key == Key.OemPeriod;

            var arrow = e.Key == Key.Left || e.Key == Key.Right;

            if (number || delete || arrow) {
                
            }
            else if (minus) {
                if (textBox.CaretIndex != 0 || textBox.Text.Contains(@"-"))
                    e.Handled = true;
            }
            else if (period) {
                if (textBox.Text.Contains(@"."))
                    e.Handled = true;
            }
            else if (ent || esc) {
                var binding = BindingOperations.GetBindingExpression((TextBox) sender, TextBox.TextProperty);
                if (binding == null)
                    return;

                Keyboard.ClearFocus();

                if (ent)
                    binding.UpdateSource();
                if (esc)
                    binding.UpdateTarget();
            }

            else
                e.Handled = true;

        }
    }
}
