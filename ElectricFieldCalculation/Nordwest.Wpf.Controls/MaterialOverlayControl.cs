using System.Windows;
using System.Windows.Controls;

namespace Nordwest.Wpf.Controls
{
    /// <summary>
    /// как попап, только не пропадает
    /// </summary>
    public class MaterialOverlayControl : ContentControl
    {
        static MaterialOverlayControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MaterialOverlayControl), new FrameworkPropertyMetadata(typeof(MaterialOverlayControl)));
        }

        public static readonly DependencyProperty IsOpenedProperty = DependencyProperty.Register(
            "IsOpened", typeof (bool), typeof (MaterialOverlayControl), new PropertyMetadata(default(bool)));

        public bool IsOpened
        {
            get { return (bool)GetValue(IsOpenedProperty); }
            set { SetValue(IsOpenedProperty, value); }
        }
    }
}