using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Nordwest.Wpf.Controls
{
    [TemplatePart(Name = @"LessButton", Type = typeof(Button))]
    [TemplatePart(Name = @"MoreButton", Type = typeof(Button))]
    [TemplatePart(Name = @"ZoomInButton", Type = typeof(Button))]
    [TemplatePart(Name = @"ZoomOutButton", Type = typeof(Button))]
    [TemplatePart(Name = @"MinGrip", Type = typeof(FrameworkElement))]
    [TemplatePart(Name = @"MaxGrip", Type = typeof(FrameworkElement))]
    [TemplatePart(Name = @"BodyGrip", Type = typeof(FrameworkElement))]
    [TemplatePart(Name = @"Slider", Type = typeof(Canvas))]
    [TemplatePart(Name = @"Track", Type = typeof(Grid))]
    public class VerticalScroll : IntervalControlBase
    {
        private Button _lessButton;
        private Button _moreButton;
        private Button _zoomInButton;
        private Button _zoomOutButton;
        private FrameworkElement _minGrip;
        private FrameworkElement _maxGrip;
        private FrameworkElement _bodyGrip;
        private Canvas _track;
        private Grid _slider;

        static VerticalScroll()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VerticalScroll), new FrameworkPropertyMetadata(typeof(VerticalScroll)));
        }

        
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _lessButton = GetTemplateChild(@"LessButton") as Button;
            _moreButton = GetTemplateChild(@"MoreButton") as Button;
            _minGrip = GetTemplateChild(@"MinGrip") as FrameworkElement;
            _maxGrip = GetTemplateChild(@"MaxGrip") as FrameworkElement;
            _bodyGrip = GetTemplateChild(@"BodyGrip") as FrameworkElement;
            _track = GetTemplateChild(@"Track") as Canvas;
            _slider = GetTemplateChild(@"Slider") as Grid;
            _zoomInButton = GetTemplateChild(@"ZoomInButton") as Button;
            _zoomOutButton = GetTemplateChild(@"ZoomOutButton") as Button;

            if (_lessButton != null)
                _lessButton.Click +=
                    OnLessButtonOnMouseDown;
            if (_moreButton != null)
                _moreButton.Click +=
                    OnButtonOnMouseDown;

            if (_minGrip != null)
            {
                _minGrip.PreviewMouseDown += Grip_MouseDown;
                _minGrip.PreviewMouseMove += _minGrip_MouseMove;
                _minGrip.PreviewMouseUp += Grip_MouseUp;
            }

            if (_maxGrip != null)
            {
                _maxGrip.PreviewMouseDown += Grip_MouseDown;
                _maxGrip.PreviewMouseMove += _maxGrip_MouseMove;
                _maxGrip.PreviewMouseUp += Grip_MouseUp;
            }

            if (_bodyGrip != null)
            {
                _bodyGrip.PreviewMouseDown += Grip_MouseDown;
                _bodyGrip.PreviewMouseMove += _bodyGrip_MouseMove;
                _bodyGrip.PreviewMouseUp += Grip_MouseUp;
            }

            if (_zoomInButton != null)
            {
                _zoomInButton.Click += new RoutedEventHandler(_zoomInButton_Click);
            }
            if (_zoomOutButton != null)
            {
                _zoomOutButton.Click += new RoutedEventHandler(_zoomOutButton_Click);
            }

            _slider.SetBinding(WidthProperty, new Binding(@"ActualWidth") { Source = _track, Mode = BindingMode.OneWay });

            //выставляем слайдер
            var cstart = GetClient(Interval.Start);
            var cend = GetClient(Interval.End);

            _slider.Height = cend - cstart;
            _slider.SetValue(Canvas.TopProperty, cstart);
        }

        void _zoomOutButton_Click(object sender, RoutedEventArgs e)
        {
            Interval = Interval.Inflate(Interval.Length * 0.1, Interval.Length * 0.1);
        }

        void _zoomInButton_Click(object sender, RoutedEventArgs e)
        {
            Interval = Interval.Inflate(-Interval.Length * 0.1, -Interval.Length * 0.1);
        }

        private void OnButtonOnMouseDown(object s, RoutedEventArgs routedEventArgs)
        {
            Interval = new Interval(Interval.Start + Interval.Length/10.0, Interval.End + Interval.Length/10.0);
        }

        private void OnLessButtonOnMouseDown(object s, RoutedEventArgs routedEventArgs)
        {
            Interval = new Interval(Interval.Start - Interval.Length/10.0, Interval.End - Interval.Length/10.0);
        }

        private double _capturedPos = double.NaN;
        private FrameworkElement _capturedGrip;
        void Grip_MouseUp(object sender, MouseButtonEventArgs e)
        {
            e.MouseDevice.Capture((IInputElement)sender, CaptureMode.None);
            _capturedPos = double.NaN;
            _capturedGrip = null;
        }
        void Grip_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.MouseDevice.Capture((IInputElement)sender);
            _capturedPos = e.GetPosition(_track).Y;
            _capturedGrip = (FrameworkElement) sender;
        }

        
        void _minGrip_MouseMove(object sender, MouseEventArgs e)
        {
            if (double.IsNaN(_capturedPos) || _capturedGrip != sender) return;
            var newPos = e.GetPosition(_track).Y;

            var oldModel = GetModel(_capturedPos);
            var newModel = GetModel(newPos);
            var delta = oldModel - newModel;

            var newStart = Interval.Start - delta;
            if (newStart < Minimum) newStart = Minimum;
            if (newStart > Interval.End) newStart = Interval.End;

            Interval = new Interval(newStart, Interval.End);
            _capturedPos = newPos;
        }
        
        void _maxGrip_MouseMove(object sender, MouseEventArgs e)
        {
            if (double.IsNaN(_capturedPos) || _capturedGrip != sender) return;
            var newPos = e.GetPosition(_track).Y;
            var oldModel = GetModel(_capturedPos);
            var newModel = GetModel(newPos);
            var delta = oldModel - newModel;

            var newEnd = Interval.End - delta;
            if (newEnd > Maximum) newEnd = Maximum;
            if (newEnd < Interval.Start) newEnd = Interval.Start;

            Interval = new Interval(Interval.Start, newEnd);
            _capturedPos = newPos;

        }
        
        void _bodyGrip_MouseMove(object sender, MouseEventArgs e)
        {
            if (double.IsNaN(_capturedPos) || _capturedGrip != sender) return;
            var newPos = e.GetPosition(_track).Y;

            var oldModel = GetModel(_capturedPos);
            var newModel = GetModel(newPos);
            var delta = oldModel - newModel;
            Interval = new Interval(Interval.Start - delta, Interval.End - delta);
            _capturedPos = newPos;
        }

        private double GetModel(double client)
        {
            return client / (_track.ActualHeight - _slider.RowDefinitions[0].ActualHeight - _slider.RowDefinitions[2].ActualHeight) * (Maximum - Minimum) + Minimum;
        }
        private double GetClient(double model)
        {
            return (model - Minimum) / (Maximum - Minimum) * (_track.ActualHeight - _slider.RowDefinitions[0].ActualHeight - _slider.RowDefinitions[2].ActualHeight);
        }

        
        protected override void OnIntervalChanged(Interval oldValue, Interval newValue)
        {
            var cstart = GetClient(newValue.Start);
            var cend = GetClient(newValue.End);

            _slider.Height = cend - cstart + _slider.RowDefinitions[0].ActualHeight + _slider.RowDefinitions[2].ActualHeight;
            _slider.SetValue(Canvas.TopProperty, cstart);

            base.OnIntervalChanged(oldValue, newValue);
        }
    }

}