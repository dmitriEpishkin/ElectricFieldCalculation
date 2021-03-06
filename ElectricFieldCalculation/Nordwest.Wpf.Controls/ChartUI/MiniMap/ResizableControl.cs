using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Nordwest.Wpf.Controls.MiniMap
{
    [TemplatePart(Name = @"TopGrip", Type = typeof(UIElement))]
    [TemplatePart(Name = @"BottomGrip", Type = typeof(UIElement))]
    [TemplatePart(Name = @"LeftGrip", Type = typeof(UIElement))]
    [TemplatePart(Name = @"RightGrip", Type = typeof(UIElement))]
    [TemplatePart(Name = @"TopLeftGrip", Type = typeof(UIElement))]
    [TemplatePart(Name = @"TopRightGrip", Type = typeof(UIElement))]
    [TemplatePart(Name = @"BottomLeftGrip", Type = typeof(UIElement))]
    [TemplatePart(Name = @"BottomRightGrip", Type = typeof(UIElement))]
    public class ResizableControl : Control
    {
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(object), typeof(ResizableControl), null);
        public object Content
        {
            get { return GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public static readonly DependencyProperty GripSizeProperty =
            DependencyProperty.Register("GripSize", typeof(double), typeof(ResizableControl), new PropertyMetadata(default(double)));
        public double GripSize
        {
            get { return (double)GetValue(GripSizeProperty); }
            set { SetValue(GripSizeProperty, value); }
        }

        public static readonly DependencyProperty GripStyleProperty =
            DependencyProperty.Register("GripSytle", typeof(ResizableControlGripStyle), typeof(ResizableControl), new PropertyMetadata(default(ResizableControlGripStyle), GripStylePropertyCallBack));
        public ResizableControlGripStyle GripSytle
        {
            get { return (ResizableControlGripStyle)GetValue(GripStyleProperty); }
            set { SetValue(GripStyleProperty, value); }
        }

        static ResizableControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ResizableControl), new FrameworkPropertyMetadata(typeof(ResizableControl)));
        }

        
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var grid = GetTemplateChild(@"grid") as Grid;
            if (grid != null)
            {
                var gsb = new Binding(@"GripSize") { Source = this, Mode = BindingMode.OneWay };
                grid.RowDefinitions[0].SetBinding(RowDefinition.HeightProperty, gsb);
                grid.RowDefinitions[2].SetBinding(RowDefinition.HeightProperty, gsb);
                grid.ColumnDefinitions[0].SetBinding(ColumnDefinition.WidthProperty, gsb);
                grid.ColumnDefinitions[2].SetBinding(ColumnDefinition.WidthProperty, gsb);
            }

            var topGrip = GetTemplateChild(@"TopGrip") as UIElement;
            var bottomGrip = GetTemplateChild(@"BottomGrip") as UIElement;
            var leftGrip = GetTemplateChild(@"LeftGrip") as UIElement;
            var rightGrip = GetTemplateChild(@"RightGrip") as UIElement;
            var topLeftGrip = GetTemplateChild(@"TopLeftGrip") as UIElement;
            var topRightGrip = GetTemplateChild(@"TopRightGrip") as UIElement;
            var bottomLeftGrip = GetTemplateChild(@"BottomLeftGrip") as UIElement;
            var bottomRightGrip = GetTemplateChild(@"BottomRightGrip") as UIElement;

            if (topGrip != null)
            {
                topGrip.MouseDown += Grip_MouseDown;
                topGrip.MouseMove += topGrip_MouseMove;
                topGrip.MouseUp += Grip_MouseUp;
                topGrip.IsEnabled = GripSytle.HasFlag(ResizableControlGripStyle.Top);
            }

            if (bottomGrip != null)
            {
                bottomGrip.MouseDown += Grip_MouseDown;
                bottomGrip.MouseMove += bottomGrip_MouseMove;
                bottomGrip.MouseUp += Grip_MouseUp;
                bottomGrip.IsEnabled = GripSytle.HasFlag(ResizableControlGripStyle.Bottom);
            }

            if (leftGrip != null)
            {
                leftGrip.MouseDown += Grip_MouseDown;
                leftGrip.MouseMove += leftGrip_MouseMove;
                leftGrip.MouseUp += Grip_MouseUp;
                leftGrip.IsEnabled = GripSytle.HasFlag(ResizableControlGripStyle.Left);
            }

            if (rightGrip != null)
            {
                rightGrip.MouseDown += Grip_MouseDown;
                rightGrip.MouseMove += rightGrip_MouseMove;
                rightGrip.MouseUp += Grip_MouseUp;
                rightGrip.IsEnabled = GripSytle.HasFlag(ResizableControlGripStyle.Right);
            }

            if (topLeftGrip != null)
            {
                topLeftGrip.MouseDown += Grip_MouseDown;
                topLeftGrip.MouseMove += topLeftGrip_MouseMove;
                topLeftGrip.MouseUp += Grip_MouseUp;
                topLeftGrip.IsEnabled = GripSytle.HasFlag(ResizableControlGripStyle.TopLeft);
            }

            if (topRightGrip != null)
            {
                topRightGrip.MouseDown += Grip_MouseDown;
                topRightGrip.MouseMove += topRightGrip_MouseMove;
                topRightGrip.MouseUp += Grip_MouseUp;
                topRightGrip.IsEnabled = GripSytle.HasFlag(ResizableControlGripStyle.TopRight);
            }

            if (bottomLeftGrip != null)
            {
                bottomLeftGrip.MouseDown += Grip_MouseDown;
                bottomLeftGrip.MouseMove += bottomLeftGrip_MouseMove;
                bottomLeftGrip.MouseUp += Grip_MouseUp;
                bottomLeftGrip.IsEnabled = GripSytle.HasFlag(ResizableControlGripStyle.BottomLeft);
            }

            if (bottomRightGrip != null)
            {
                bottomRightGrip.MouseDown += Grip_MouseDown;
                bottomRightGrip.MouseMove += bottomRightGrip_MouseMove;
                bottomRightGrip.MouseUp += Grip_MouseUp;
                bottomRightGrip.IsEnabled = GripSytle.HasFlag(ResizableControlGripStyle.BottomRight);
            }
        }

        private Point _resizeCapturedPos;
        private bool _captured;
        
        private void ResizeTop(Point newPos)
        {
            var delta = newPos.Y - _resizeCapturedPos.Y;
            if (Height <= delta)
                Height = MinHeight;
            else if (Height - delta > MaxHeight)
                Height = MaxHeight;
            else
                this.Height -= delta;
        }
        
        private void ResizeBottom(Point newPos)
        {
            var delta = newPos.Y - _resizeCapturedPos.Y;
            if (Height <= -delta)
                Height = MinHeight;
            else if (Height + delta > MaxHeight)
                Height = MaxHeight;
            else
                Height += delta;
        }
        
        private void ResizeLeft(Point newPos)
        {
            var delta = newPos.X - _resizeCapturedPos.X;
            if (Width <= delta)
                Width = MinWidth;
            else if (Width - delta > MaxWidth)
                Width = MaxWidth;
            else
                Width -= delta;
        }
        
        private void ResizeRight(Point newPos)
        {
            var delta = newPos.X - _resizeCapturedPos.X;
            if (Width <= -delta)
                Width = MinWidth;
            else if (Width + delta > MaxWidth)
                Width = MaxWidth;
            else
                Width += delta;
        }

        void Grip_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;
            e.MouseDevice.Capture((IInputElement)sender);
            _resizeCapturedPos = e.MouseDevice.GetPosition((IInputElement)Parent);
            _captured = true;
        }
        void Grip_MouseUp(object sender, MouseButtonEventArgs e)
        {
            e.MouseDevice.Capture((IInputElement)sender, CaptureMode.None);
            _captured = false;
        }

        void topGrip_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_captured || !GripSytle.HasFlag(ResizableControlGripStyle.Top)) return;
            var newPos = e.MouseDevice.GetPosition((IInputElement)Parent);
            ResizeTop(newPos);
            _resizeCapturedPos = newPos;
        }
        void bottomGrip_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_captured || !GripSytle.HasFlag(ResizableControlGripStyle.Bottom)) return;
            var newPos = e.MouseDevice.GetPosition((IInputElement)this.Parent);
            ResizeBottom(newPos);
            _resizeCapturedPos = newPos;
        }
        void leftGrip_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_captured || !GripSytle.HasFlag(ResizableControlGripStyle.Left)) return;
            var newPos = e.MouseDevice.GetPosition((IInputElement)this.Parent);
            ResizeLeft(newPos);
            _resizeCapturedPos = newPos;
        }
        void rightGrip_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_captured || !GripSytle.HasFlag(ResizableControlGripStyle.Right)) return;
            var newPos = e.MouseDevice.GetPosition((IInputElement)this.Parent);
            ResizeRight(newPos);
            _resizeCapturedPos = newPos;
        }
        void topLeftGrip_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_captured || !GripSytle.HasFlag(ResizableControlGripStyle.TopLeft)) return;
            var newPos = e.MouseDevice.GetPosition((IInputElement)this.Parent);
            ResizeTop(newPos);
            ResizeLeft(newPos);
            _resizeCapturedPos = newPos;
        }
        void topRightGrip_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_captured || !GripSytle.HasFlag(ResizableControlGripStyle.TopRight)) return;
            var newPos = e.MouseDevice.GetPosition((IInputElement)this.Parent);
            ResizeTop(newPos);
            ResizeRight(newPos);
            _resizeCapturedPos = newPos;
        }
        void bottomLeftGrip_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_captured || !GripSytle.HasFlag(ResizableControlGripStyle.BottomLeft)) return;
            var newPos = e.MouseDevice.GetPosition((IInputElement)this.Parent);
            ResizeBottom(newPos);
            ResizeLeft(newPos);
            _resizeCapturedPos = newPos;
        }
        void bottomRightGrip_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_captured || !GripSytle.HasFlag(ResizableControlGripStyle.BottomRight)) return;
            var newPos = e.MouseDevice.GetPosition((IInputElement)this.Parent);
            ResizeBottom(newPos);
            ResizeRight(newPos);
            _resizeCapturedPos = newPos;
        }

        private static void GripStylePropertyCallBack(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var t = dependencyObject as ResizableControl;
            if (t == null) return;

            var newValue = (ResizableControlGripStyle)e.NewValue;
            var topGrip = t.GetTemplateChild(@"TopGrip") as UIElement;
            var bottomGrip = t.GetTemplateChild(@"BottomGrip") as UIElement;
            var leftGrip = t.GetTemplateChild(@"LeftGrip") as UIElement;
            var rightGrip = t.GetTemplateChild(@"RightGrip") as UIElement;
            var topLeftGrip = t.GetTemplateChild(@"TopLeftGrip") as UIElement;
            var topRightGrip = t.GetTemplateChild(@"TopRightGrip") as UIElement;
            var bottomLeftGrip = t.GetTemplateChild(@"BottomLeftGrip") as UIElement;
            var bottomRightGrip = t.GetTemplateChild(@"BottomRightGrip") as UIElement;
            if (topGrip != null) topGrip.IsEnabled = newValue.HasFlag(ResizableControlGripStyle.Top);
            if (bottomGrip != null) bottomGrip.IsEnabled = newValue.HasFlag(ResizableControlGripStyle.Bottom);
            if (leftGrip != null) leftGrip.IsEnabled = newValue.HasFlag(ResizableControlGripStyle.Left);
            if (rightGrip != null) rightGrip.IsEnabled = newValue.HasFlag(ResizableControlGripStyle.Right);
            if (topLeftGrip != null) topLeftGrip.IsEnabled = newValue.HasFlag(ResizableControlGripStyle.TopLeft);
            if (topRightGrip != null) topRightGrip.IsEnabled = newValue.HasFlag(ResizableControlGripStyle.TopRight);
            if (bottomLeftGrip != null)
                bottomLeftGrip.IsEnabled = newValue.HasFlag(ResizableControlGripStyle.BottomLeft);
            if (bottomRightGrip != null)
                bottomRightGrip.IsEnabled = newValue.HasFlag(ResizableControlGripStyle.BottomRight);
        }
    }
    [Flags]
    public enum ResizableControlGripStyle
    {
        None = 0,
        Left = 1,
        Top = 2,
        Right = 4,
        Bottom = 8,
        TopLeft = 16,
        TopRight = 32,
        BottomLeft = 64,
        BottomRight = 128
    }
}