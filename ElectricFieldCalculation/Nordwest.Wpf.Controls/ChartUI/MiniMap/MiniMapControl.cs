using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Nordwest.Wpf.Controls.Chart;

namespace Nordwest.Wpf.Controls.MiniMap
{
    [TemplatePart(Name = @"Content", Type = typeof(MiniMapContent))]
    [TemplatePart(Name = @"Resizer", Type = typeof(ResizableControl))]
    public class MiniMapControl : Control
    {
        private MiniMapContent _content;

        public static readonly DependencyProperty MapCornerProperty =
            DependencyProperty.Register("MapCorner", typeof(MapCorner), typeof(MiniMapControl), new PropertyMetadata(default(MapCorner), new PropertyChangedCallback(OnCornerChanged)));

        private static void OnCornerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var t = (MiniMapControl)d;
            var content = t.GetTemplateChild(@"Content") as MiniMapContent;
            var resizer = t.GetTemplateChild(@"Resizer") as ResizableControl;
            if (content == null || resizer == null) return;

            var corner = (MapCorner)e.NewValue;
            switch (corner)
            {
                case MapCorner.TopLeft:
                    resizer.SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Left);
                    resizer.SetValue(VerticalAlignmentProperty, VerticalAlignment.Top);
                    resizer.GripSytle = ResizableControlGripStyle.Bottom | ResizableControlGripStyle.Right |
                                        ResizableControlGripStyle.BottomRight;
                    break;
                case MapCorner.TopRight:
                    resizer.SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Right);
                    resizer.SetValue(VerticalAlignmentProperty, VerticalAlignment.Top);
                    resizer.GripSytle = ResizableControlGripStyle.Bottom | ResizableControlGripStyle.Left |
                                        ResizableControlGripStyle.BottomLeft;
                    break;
                case MapCorner.BottomLeft:
                    resizer.SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Left);
                    resizer.SetValue(VerticalAlignmentProperty, VerticalAlignment.Bottom);
                    resizer.GripSytle = ResizableControlGripStyle.Top | ResizableControlGripStyle.Right |
                                        ResizableControlGripStyle.TopRight;
                    break;
                case MapCorner.BottomRight:
                    resizer.SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Right);
                    resizer.SetValue(VerticalAlignmentProperty, VerticalAlignment.Bottom);
                    resizer.GripSytle = ResizableControlGripStyle.Top | ResizableControlGripStyle.Left |
                                        ResizableControlGripStyle.TopLeft;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        public MapCorner MapCorner
        {
            get { return (MapCorner)GetValue(MapCornerProperty); }
            set { SetValue(MapCornerProperty, value); }
        }

        public List<IDrawToMiniMap> MiniMaps
        {
            get
            {
                return _content != null ? _content.MiniMaps : null;
            }
        }

        static MiniMapControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MiniMapControl), new FrameworkPropertyMetadata(typeof(MiniMapControl)));
        }

        
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var resizer = GetTemplateChild(@"Resizer") as ResizableControl;
            _content = GetTemplateChild(@"Content") as MiniMapContent;
            _content.ViewportChanged += (s, e) => OnViewportChanged();
            var bh = new Binding(@"ActualHeight") { Source = this, Mode = BindingMode.OneWay };
            var bw = new Binding(@"ActualWidth") { Source = this, Mode = BindingMode.OneWay };
            if (resizer != null)
            {
                resizer.SetBinding(MaxHeightProperty, bh);
                resizer.SetBinding(MaxWidthProperty, bw);
            }
            OnCornerChanged(this, new DependencyPropertyChangedEventArgs(MapCornerProperty, null, MapCorner.BottomRight));
        }

        public void Redraw()
        {
            if (Visibility == Visibility.Visible && _content!=null) _content.Redraw();
        }

        public ViewportAxis HorizontalAxis { get { return _content.HorizontalAxis; } set { _content.HorizontalAxis = value; } }
        public ViewportAxis VerticalAxis { get { return _content.VerticalAxis; } set { _content.VerticalAxis = value; } }
        public void UpdateBounds()
        {
            _content.UpdateBounds();
        }
        public Rect Viewport { get { return _content.ViewportRect; } set { _content.ViewportRect = value; } }

        public event EventHandler ViewportChanged;

        public void OnViewportChanged()
        {
            EventHandler handler = ViewportChanged;
            if (handler != null) handler(this,EventArgs.Empty);
        }
    }

    public enum MapCorner
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }
}