using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Nordwest.Wpf.Controls.Chart
{
    public class HorizontalAxisControl : AxisControlBase
    {
        public VerticalAlignment AxisAlignment { get; set; }
        private int _tickSize = 5;

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (Axis != null && Visibility == Visibility.Visible) {

                if (TrimSideLabels) {
                    OccupiedLabelPlaces.Add(new Rect(0, 0, 1, ActualHeight));
                    OccupiedLabelPlaces.Add(new Rect(ActualWidth, 0, 1, ActualHeight));
                }
                else {
                    OccupiedLabelPlaces.Clear();
                }

                var marks = MarkProvider.GetMarks(Axis).Select(m => new Tuple<double, FormattedText>(m.Item1,
                    new FormattedText(m.Item2, CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                        new Typeface(FontFamily, FontStyle, FontWeight, FontStretch), FontSize,
                        Foreground))).ToList();

                if (marks.Count > 0) {
                    var maxWidth = marks.Max(m => m.Item2.Width) + LabelMargin;
                    foreach (var m in marks) {
                        RenderLabel(drawingContext, m.Item1, m.Item2, maxWidth);
                    }
                }

                RenderAxis(drawingContext);
            }
        }

        private void RenderAxis(DrawingContext dc)
        {
            var p = new Pen(Brushes.Black,1);
            var halfPenSize = p.Thickness/2;
            var width = Math.Floor(ActualWidth);
            var height = Math.Floor(ActualHeight);
            var halfHeight = Math.Floor(ActualHeight / 2);
            switch (AxisAlignment)
            {
                case VerticalAlignment.Top:
                    dc.PushGuidelineSet(new GuidelineSet(new double[0], new[] { -halfPenSize })); 
                    dc.DrawLine(p, new Point(0, 0), new Point(width, 0));
                    break;
                case VerticalAlignment.Bottom:
                    dc.PushGuidelineSet(new GuidelineSet(new double[0], new[] { height - halfPenSize })); 
                    dc.DrawLine(p, new Point(0, height), new Point(width, height));
                    break;
                case VerticalAlignment.Center:
                case VerticalAlignment.Stretch:
                    dc.PushGuidelineSet(new GuidelineSet(new double[0], new[] { halfHeight - halfPenSize })); 
                    dc.DrawLine(p, new Point(0, halfHeight), new Point(width, halfHeight));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            dc.Pop();
        }
        
        protected override void RenderLabel(DrawingContext dc, double position, FormattedText ft, double max)
        {
            var p = new Pen(Brushes.Black, 1);
            Rect rect;
            var halfPenSize = p.Thickness / 2;
            var pos = Math.Round(position, MidpointRounding.AwayFromZero);
            dc.PushGuidelineSet(new GuidelineSet(new[] { pos - halfPenSize }, new double[0]));

            bool drawLabel;

            switch (AxisAlignment)
            {
                case VerticalAlignment.Top:
                    rect = GetRectangleByAlignment(new Point(pos, _tickSize),
                                          new Size(max, ft.Height), TextAlignment);
                    drawLabel = !OccupiedLabelPlaces.Exists(r => r.IntersectsWith(rect));
                    if (drawLabel)
                        dc.DrawLine(p, new Point(pos, 0), new Point(pos, _tickSize));
                    else
                        dc.DrawLine(p, new Point(pos, 0), new Point(pos, _tickSize / 2.0));
                    break;
                case VerticalAlignment.Bottom:
                    rect = GetRectangleByAlignment(new Point(pos, ActualHeight - _tickSize),
                                          new Size(max, ft.Height), TextAlignment);
                    drawLabel = !OccupiedLabelPlaces.Exists(r => r.IntersectsWith(rect));
                    if (drawLabel)
                        dc.DrawLine(p, new Point(pos, ActualHeight - _tickSize), new Point(pos, ActualHeight));
                    else
                        dc.DrawLine(p, new Point(pos, ActualHeight - _tickSize / 2.0), new Point(pos, ActualHeight));
                    break;
                case VerticalAlignment.Center:
                case VerticalAlignment.Stretch:
                    rect = GetRectangleByAlignment(new Point(pos, ActualHeight / 2),
                                          new Size(max, ft.Height), TextAlignment);
                    drawLabel = !OccupiedLabelPlaces.Exists(r => r.IntersectsWith(rect));
                    if (drawLabel)
                        dc.DrawLine(p, new Point(pos, ActualHeight / 2 - _tickSize), new Point(pos, ActualHeight / 2 + _tickSize));
                    else
                        dc.DrawLine(p, new Point(pos, ActualHeight / 2 - _tickSize / 2.0), new Point(pos, ActualHeight / 2 + _tickSize / 2.0));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            dc.Pop();
            
            if (drawLabel) {
                var wReal = ft.Width;
                var wSpace = rect.Width;
                double x;
                if (TextAlignment == ContentAlignment.BottomLeft ||
                    TextAlignment == ContentAlignment.MiddleLeft ||
                    TextAlignment == ContentAlignment.TopLeft)
                    x = rect.X;
                else if (TextAlignment == ContentAlignment.BottomCenter ||
                         TextAlignment == ContentAlignment.MiddleCenter ||
                         TextAlignment == ContentAlignment.TopCenter)
                    x = rect.X + (wSpace - wReal) / 2.0;
                else {
                    x = rect.X + (wSpace - wReal);
                }

                dc.DrawText(ft, new Point(x, rect.Location.Y));
                OccupiedLabelPlaces.Add(rect);
            }
        }
    }
}