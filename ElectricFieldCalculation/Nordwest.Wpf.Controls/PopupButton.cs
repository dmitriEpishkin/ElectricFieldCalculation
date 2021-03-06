using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Nordwest.Wpf.Controls.Converters;

namespace Nordwest.Wpf.Controls
{
    [TemplatePart(Name = @"Button", Type = typeof(ToggleButton))]
    [TemplatePart(Name = @"Popup", Type = typeof(Popup))]
    public class PopupButton : HeaderedContentControl
    {
        private ToggleButton _button;
        private Popup _popup;

        static PopupButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PopupButton), new FrameworkPropertyMetadata(typeof(PopupButton)));
        }

        
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _button = (ToggleButton)GetTemplateChild(@"Button");
            _popup = (Popup)GetTemplateChild(@"Popup");

            if (_button != null && (_button.Style == null && Parent is ToolBar))
                _button.Style = (Style)FindResource(ToolBar.ToggleButtonStyleKey);
        }

        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(
            "IsChecked", typeof (bool), typeof (PopupButton), new PropertyMetadata(default(bool)));

        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        public static readonly DependencyProperty PopupPlacementModeProperty = DependencyProperty.Register(
            "PopupPlacementMode", typeof (PlacementMode), typeof (PopupButton), new PropertyMetadata(default(PlacementMode)));

        public PlacementMode PopupPlacementMode
        {
            get { return (PlacementMode) GetValue(PopupPlacementModeProperty); }
            set { SetValue(PopupPlacementModeProperty, value); }
        }

        public static readonly DependencyProperty PopupAnimationProperty = DependencyProperty.Register(
            "PopupAnimation", typeof (PopupAnimation), typeof (PopupButton), new PropertyMetadata(default(PopupAnimation)));

        public PopupAnimation PopupAnimation
        {
            get { return (PopupAnimation) GetValue(PopupAnimationProperty); }
            set { SetValue(PopupAnimationProperty, value); }
        }

        public static readonly DependencyProperty ButtonStyleProperty = DependencyProperty.Register(
            "ButtonStyle", typeof (Style), typeof (PopupButton), new PropertyMetadata(default(Style)));

        public Style ButtonStyle
        {
            get { return (Style) GetValue(ButtonStyleProperty); }
            set { SetValue(ButtonStyleProperty, value); }
        }

        public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.Register(
            "VerticalOffset", typeof (double), typeof (PopupButton), new PropertyMetadata(default(double)));

        public double VerticalOffset
        {
            get { return (double) GetValue(VerticalOffsetProperty); }
            set { SetValue(VerticalOffsetProperty, value); }
        }

        public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.Register(
            "HorizontalOffset", typeof (double), typeof (PopupButton), new PropertyMetadata(default(double)));

        public double HorizontalOffset
        {
            get { return (double) GetValue(HorizontalOffsetProperty); }
            set { SetValue(HorizontalOffsetProperty, value); }
        }

        public static readonly DependencyProperty PopupPlacementCallbackProperty = DependencyProperty.Register(
            "PopupPlacementCallback", typeof (CustomPopupPlacementCallback), typeof (PopupButton), new PropertyMetadata(default(CustomPopupPlacementCallback)));

        public CustomPopupPlacementCallback PopupPlacementCallback
        {
            get { return (CustomPopupPlacementCallback) GetValue(PopupPlacementCallbackProperty); }
            set { SetValue(PopupPlacementCallbackProperty, value); }
        }

        public static readonly DelegateConverter<bool, bool> InvertBooleanConverter =
            new DelegateConverter<bool, bool>(b => !b, b => !b);
    }
}