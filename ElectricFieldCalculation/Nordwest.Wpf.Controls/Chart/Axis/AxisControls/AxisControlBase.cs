using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Nordwest.Wpf.Controls.Chart
{
    public abstract class AxisControlBase : Control
    {

        private AxisMarkProvider _markProvider = new LinearMarkProvider(50);
        public Orientation Orientation { get; set; }
        public ContentAlignment TextAlignment { get; set; }
        
        // логика расположения подписей
        public bool TrimSideLabels { get; set; }
        public bool EqualSpacing { get; set; } = true;
        public int LabelMargin { get; set; } = 3;

        public AxisMarkProvider MarkProvider
        {
            get { return _markProvider; }
            set {
                if (_markProvider != value) {
                    _markProvider = value;
                    OnMarkProviderChanged();
                }
            }
        }

        public static Rect GetRectangleByAlignment(Point point, Size size, ContentAlignment alignment)
        {
            double dx = 0, dy = 0;

            switch (alignment) {
                case ContentAlignment.TopLeft:
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.BottomLeft:
                    dx = -size.Width;
                    break;
                case ContentAlignment.TopCenter:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.BottomCenter:
                    dx = -size.Width / 2;
                    break;
            }

            switch (alignment) {
                case ContentAlignment.TopLeft:
                case ContentAlignment.TopCenter:
                case ContentAlignment.TopRight:
                    dy = -size.Height;
                    break;
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.MiddleRight:
                    dy = -size.Height / 2;
                    break;
            }

            point.Offset(dx, dy);

            return new Rect(point, size);
        }

        protected abstract void RenderLabel(DrawingContext dc, double position, FormattedText text, double max);

        public List<Rect> OccupiedLabelPlaces { get; set; } = new List<Rect>();

        public static readonly DependencyProperty AxisProperty = DependencyProperty.Register("Axis", typeof(ViewportAxis), typeof(AxisControlBase), new FrameworkPropertyMetadata(new LinAxis { Model = new ViewRange(1, 10) }, OnAxisChanged), a => ((ViewportAxis)a) != null);

        [Bindable(true)]
        public ViewportAxis Axis
        {
            get { return (ViewportAxis)GetValue(AxisProperty); }
            set { SetValue(AxisProperty, value); }
        }

        private static void OnAxisChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            ((AxisControlBase)sender).OnAxisChanged((ViewportAxis)e.OldValue, (ViewportAxis)e.NewValue);
        }

        protected virtual void OnAxisChanged(ViewportAxis oldValue, ViewportAxis newValue) {
            RaiseEvent(new RoutedPropertyChangedEventArgs<ViewportAxis>(oldValue, newValue) { RoutedEvent = AxisChangedEvent });
        }

        public static readonly RoutedEvent AxisChangedEvent = EventManager.RegisterRoutedEvent("AxisChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<ViewportAxis>), typeof(AxisControlBase));

        [Category("Behavior")]
        public event RoutedPropertyChangedEventHandler<ViewportAxis> AxisChanged
        {
            add { AddHandler(AxisChangedEvent, value); }
            remove { RemoveHandler(AxisChangedEvent, value); }
        }

        private void OnMarkProviderChanged() {
            MarkProviderChanged?.Invoke(this, null);
        }

        public event EventHandler MarkProviderChanged;
    }

    public enum ContentAlignment {
        TopLeft = 1,
        TopCenter = 2,
        TopRight = 4,
        MiddleLeft = 16,
        MiddleCenter = 32,
        MiddleRight = 64,
        BottomLeft = 256,
        BottomCenter = 512,
        BottomRight = 1024,
    }
}