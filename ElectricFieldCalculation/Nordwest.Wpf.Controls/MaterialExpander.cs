using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Nordwest.Wpf.Controls
{

    [TemplatePart(Name = @"Resizer", Type = typeof(FrameworkElement))]
    public class MaterialExpander : ContentControl
    {
        private FrameworkElement _resizer;
        private FrameworkElement _border;

        private double _startingPosition;
        private double _startingSize;

        static MaterialExpander()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MaterialExpander), new FrameworkPropertyMetadata(typeof(MaterialExpander)));
        }

        public override void OnApplyTemplate()
        {
            _resizer = (FrameworkElement)GetTemplateChild(@"Resizer");
            _border = (FrameworkElement)GetTemplateChild(@"Border");
            _resizer.MouseDown += ResizerOnMouseDown;
            _resizer.MouseMove += ResizerOnMouseMove;
            _resizer.MouseUp += ResizerOnMouseUp;
        }

        private void ResizerOnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_resizer.IsMouseCaptured)
                _resizer.ReleaseMouseCapture();
        }

        private void ResizerOnMouseMove(object sender, MouseEventArgs e)
        {
            if (!_resizer.IsMouseCaptured)
                return;

            double position = Mouse.GetPosition(_border).Y;

            var delta = position - _startingPosition;
            ExpandedSize = CoarseSize(_startingSize + delta);
        }

        private static double CoarseSize(double d)
        {
            return d < 0 ? 0 : d;
        }

        private void ResizerOnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left)
                return;

            _resizer.CaptureMouse();
            _startingPosition = Mouse.GetPosition(_border).Y;
            _startingSize = _border.ActualHeight;
        }

        public static readonly DependencyProperty ResizerBrushProperty = DependencyProperty.Register(
            "ResizerBrush", typeof(Brush), typeof(MaterialExpander), new PropertyMetadata(default(Brush)));

        public Brush ResizerBrush
        {
            get { return (Brush)GetValue(ResizerBrushProperty); }
            set { SetValue(ResizerBrushProperty, value); }
        }

        public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register(
            "IsExpanded", typeof(bool), typeof(MaterialExpander), new PropertyMetadata(default(bool)));

        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        public static readonly DependencyProperty ExpandedSizeProperty = DependencyProperty.Register(
            "ExpandedSize", typeof(double), typeof(MaterialExpander), new PropertyMetadata(default(double)));

        public double ExpandedSize
        {
            get { return (double)GetValue(ExpandedSizeProperty); }
            set { SetValue(ExpandedSizeProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            "Header", typeof(object), typeof(MaterialExpander), new PropertyMetadata(default(object)));

        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(
            "HeaderTemplate", typeof(DataTemplate), typeof(MaterialExpander), new PropertyMetadata(default(DataTemplate)));

        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        public static readonly DependencyProperty ExpandDirectionProperty = DependencyProperty.Register(
            "ExpandDirection", typeof(ExpandDirection), typeof(MaterialExpander), new PropertyMetadata(default(ExpandDirection)));

        public ExpandDirection ExpandDirection
        {
            get { return (ExpandDirection)GetValue(ExpandDirectionProperty); }
            set { SetValue(ExpandDirectionProperty, value); }
        }
    }

    public class MultiplyConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.OfType<double>().Aggregate((d1, d2) => d1 * d2);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    public class DirectionalToggleButton : ToggleButton
    {
        static DirectionalToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DirectionalToggleButton), new FrameworkPropertyMetadata(typeof(DirectionalToggleButton)));
        }

        public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register(
            "Direction", typeof(ExpandDirection), typeof(DirectionalToggleButton), new PropertyMetadata(default(ExpandDirection)));

        public ExpandDirection Direction
        {
            get { return (ExpandDirection)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }
    }
}