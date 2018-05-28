using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Nordwest.Wpf.Controls.Converters;

namespace Nordwest.Wpf.Controls
{
    public class MaterialFlatIconToggleButton : ToggleButton
    {
        public static readonly TimeSpan AnimationDuration = TimeSpan.FromSeconds(0.2);

        private Path _path;
        private AnimationStateManager<System.Windows.Media.Animation.ColorAnimation> _iconColorManager;

        static MaterialFlatIconToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MaterialFlatIconToggleButton), new FrameworkPropertyMetadata(typeof(MaterialFlatIconToggleButton)));
        }

        public MaterialFlatIconToggleButton()
        {
            IsEnabledChanged += (sender, args) => Update();
            Checked += (sender, args) => Update();
            Unchecked += (sender, args) => Update();
        }

        private void Update()
        {
            if (_iconColorManager != null)
                _iconColorManager.Update();
        }

        public override void OnApplyTemplate()
        {
            InvalidateProperty(IsIconProperty);
            _path = (Path)GetTemplateChild(@"Path");
            if (_path != null)
            {
                _iconColorManager = new AnimationStateManager<System.Windows.Media.Animation.ColorAnimation>(_path, @"Fill.Color");
                _iconColorManager.AddState(@"Disabled", () => !IsEnabled, DisabledColor);
                _iconColorManager.AddState(@"Normal", () => IsEnabled && !IsChecked.GetValueOrDefault(), NormalColor);
                _iconColorManager.AddState(@"Checked", () => IsEnabled && IsChecked.GetValueOrDefault(), CheckedColor);
                _iconColorManager.Update();
            }
        }

        private static void ApplyColor(DependencyObject o, DependencyPropertyChangedEventArgs args, string key)
        {
            var control = (MaterialFlatIconToggleButton)o;
            var color = (Color)args.NewValue;

            if (control._iconColorManager != null)
                control._iconColorManager.SetTargetValue(key, color);
            control.Update();
        }

        public static readonly DependencyProperty CheckedColorProperty = DependencyProperty.Register(
            "CheckedColor", typeof (Color), typeof (MaterialFlatIconToggleButton), new PropertyMetadata(default(Color), (o, args) => ApplyColor(o, args, @"Checked")));

        public Color CheckedColor
        {
            get { return (Color)GetValue(CheckedColorProperty); }
            set { SetValue(CheckedColorProperty, value); }
        }

        public static readonly DependencyProperty NormalColorProperty = DependencyProperty.Register(
            "NormalColor", typeof(Color), typeof(MaterialFlatIconToggleButton), new PropertyMetadata(default(Color), (o, args) => ApplyColor(o, args, @"Normal")));

        public Color NormalColor
        {
            get { return (Color)GetValue(NormalColorProperty); }
            set { SetValue(NormalColorProperty, value); }
        }

        public static readonly DependencyProperty DisabledColorProperty = DependencyProperty.Register(
            "DisabledColor", typeof(Color), typeof(MaterialFlatIconToggleButton), new PropertyMetadata(default(Color), (o, args) => ApplyColor(o, args, @"Disabled")));

        public Color DisabledColor
        {
            get { return (Color)GetValue(DisabledColorProperty); }
            set { SetValue(DisabledColorProperty, value); }
        }

        public static readonly DependencyProperty IsIconProperty = DependencyProperty.Register(
            "IsIcon", typeof (bool), typeof (MaterialFlatIconToggleButton), new PropertyMetadata(default(bool)));

        public bool IsIcon
        {
            get { return (bool)GetValue(IsIconProperty); }
            set { SetValue(IsIconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            "Icon", typeof(Geometry), typeof(MaterialFlatIconToggleButton), new PropertyMetadata(default(Geometry)));

        public Geometry Icon
        {
            get { return (Geometry)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconCanvasWidthProperty = DependencyProperty.Register(
            "IconCanvasWidth", typeof (double), typeof (MaterialFlatIconToggleButton), new PropertyMetadata(default(double)));

        public double IconCanvasWidth
        {
            get { return (double)GetValue(IconCanvasWidthProperty); }
            set { SetValue(IconCanvasWidthProperty, value); }
        }

        public static readonly DependencyProperty IconCanvasHeightProperty = DependencyProperty.Register(
            "IconCanvasHeight", typeof (double), typeof (MaterialFlatIconToggleButton), new PropertyMetadata(default(double)));

        public double IconCanvasHeight
        {
            get { return (double)GetValue(IconCanvasHeightProperty); }
            set { SetValue(IconCanvasHeightProperty, value); }
        }

        public static readonly DependencyProperty IconWidthProperty = DependencyProperty.Register(
            "IconWidth", typeof (double), typeof (MaterialFlatIconToggleButton), new PropertyMetadata(default(double)));

        public double IconWidth
        {
            get { return (double)GetValue(IconWidthProperty); }
            set { SetValue(IconWidthProperty, value); }
        }

        public static readonly DependencyProperty IconHeightProperty = DependencyProperty.Register(
            "IconHeight", typeof (double), typeof (MaterialFlatIconToggleButton), new PropertyMetadata(default(double)));

        public double IconHeight
        {
            get { return (double)GetValue(IconHeightProperty); }
            set { SetValue(IconHeightProperty, value); }
        }

        public static readonly DelegateConverter<bool, bool, Visibility> InvertableBooleanToVisibilityConverter = 
            new DelegateConverter<bool, bool, Visibility>((isVisible, invert) => (invert ? !isVisible : isVisible ) ? Visibility.Visible : Visibility.Collapsed);

    }
}