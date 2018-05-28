using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Nordwest.Wpf.Controls.Chart
{
    public class VerticalAxisControl : AxisControlBase
    {

        public HorizontalAlignment AxisAlignment { get; set; }
        private int _tickSize = 5;
        
        protected override void OnRender(DrawingContext drawingContext)
        {
            if (Axis != null && Visibility == Visibility.Visible) {
                
                if (TrimSideLabels) {
                    OccupiedLabelPlaces.Add(new Rect(0, 0, ActualWidth, 1));
                    OccupiedLabelPlaces.Add(new Rect(0, ActualHeight, ActualWidth, 1));
                }
                else {
                    OccupiedLabelPlaces.Clear();
                }

                var marks = MarkProvider.GetMarks(Axis);

                if (Axis is Log10Axis)
                    foreach (var m in marks) {
                        RenderLogLabel(drawingContext, m.Item1, m.Item2);
                    }
                else {
                    var marks2 = marks.Select(m => new Tuple<double, FormattedText>(m.Item1,
                        new FormattedText(m.Item2, CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                            new Typeface(FontFamily, FontStyle, FontWeight, FontStretch), FontSize,
                            Foreground))).ToList();

                    if (marks2.Count > 0) {
                        var max = marks2.Max(m => m.Item2.Height) + LabelMargin;
                        foreach (var m in marks2) {
                            RenderLabel(drawingContext, m.Item1, m.Item2, max);
                        }
                    }
                }

                RenderAxis(drawingContext);
            }
            base.OnRender(drawingContext);
        }

        private void RenderAxis(DrawingContext dc)
        {
            var pen = new Pen(Brushes.Black, 1);
            var width = Math.Floor(ActualWidth) - 0.5;
            var height = Math.Floor(ActualHeight) - 0.5;
            var halfWidth = Math.Floor(ActualWidth / 2) + 0.5;

            switch (AxisAlignment)
            {
                case HorizontalAlignment.Left:
                    dc.DrawLine(pen, new Point(0.5, 0.5), new Point(0.5, height + 1));
                    break;
                case HorizontalAlignment.Right:
                    dc.DrawLine(pen, new Point(width, 0.5), new Point(width, height + 1));
                    break;
                case HorizontalAlignment.Center:
                case HorizontalAlignment.Stretch:
                    dc.DrawLine(pen, new Point(halfWidth, 0.5), new Point(halfWidth, height + 1));
                    break;
            }
        }
        
        protected override void RenderLabel(DrawingContext dc, double position, FormattedText ft, double max)
        {
            var p = new Pen(Brushes.Black, 1);
            Rect rect;
            var halfPenSize = p.Thickness / 2;
            var pos = Math.Round(position, MidpointRounding.AwayFromZero);
            dc.PushGuidelineSet(new GuidelineSet(new double[0], new[] { pos - halfPenSize }));

            bool drawLabel;

            switch (AxisAlignment)
            {
                case HorizontalAlignment.Left:
                    rect = GetRectangleByAlignment(new Point(_tickSize + 2, pos),
                                          new Size(ft.Width, max), TextAlignment);
                    drawLabel = !OccupiedLabelPlaces.Exists(r => r.IntersectsWith(rect));
                    if (drawLabel)
                        dc.DrawLine(p, new Point(0, pos), new Point(_tickSize, pos));
                    else
                        dc.DrawLine(p, new Point(0, pos), new Point(_tickSize / 2.0, pos));
                    break;
                case HorizontalAlignment.Right:
                    rect = GetRectangleByAlignment(new Point(ActualWidth - (_tickSize + 2), pos),
                                          new Size(ft.Width, max), TextAlignment);
                    drawLabel = !OccupiedLabelPlaces.Exists(r => r.IntersectsWith(rect));
                    if (drawLabel)
                        dc.DrawLine(p, new Point(ActualWidth - _tickSize, pos), new Point(ActualWidth, pos));
                    else
                        dc.DrawLine(p, new Point(ActualWidth - _tickSize / 2.0, pos), new Point(ActualWidth, pos));
                    break;
                case HorizontalAlignment.Center:
                case HorizontalAlignment.Stretch:
                    rect = GetRectangleByAlignment(new Point(ActualWidth/2, position),
                                          new Size(ft.Width, max), TextAlignment);
                    drawLabel = !OccupiedLabelPlaces.Exists(r => r.IntersectsWith(rect));
                    if (drawLabel)
                        dc.DrawLine(p, new Point(ActualWidth / 2 - _tickSize, pos), new Point(ActualWidth / 2 + _tickSize, pos));
                    else
                        dc.DrawLine(p, new Point(ActualWidth / 2 - _tickSize / 2.0, pos), new Point(ActualWidth / 2 + _tickSize / 2.0, pos));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            dc.Pop();

            if (drawLabel)
            {
                var hReal = ft.Height;
                var hSpace = rect.Height;
                double y;
                if (TextAlignment == ContentAlignment.BottomLeft ||
                    TextAlignment == ContentAlignment.BottomCenter ||
                    TextAlignment == ContentAlignment.BottomRight)
                    y = rect.Y + (hReal - hSpace);
                else if (TextAlignment == ContentAlignment.MiddleLeft ||
                         TextAlignment == ContentAlignment.MiddleCenter ||
                         TextAlignment == ContentAlignment.MiddleRight)
                    y = rect.Y + (hSpace - hReal) / 2.0;
                else
                {
                    y = rect.Y;
                }

                dc.DrawText(ft, new Point(rect.Location.X, y));
                OccupiedLabelPlaces.Add(rect);
            }
        }

        protected void RenderLogLabel(DrawingContext dc, double position, string text) {
            
            double val = Math.Log10(double.Parse(text));

            var ft = new FormattedText(val.ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                                       new Typeface(FontFamily, FontStyle, FontWeight, FontStretch), FontSize*0.75,
                                       Foreground);
            var fpow = new FormattedText(@"10", CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                                       new Typeface(FontFamily, FontStyle, FontWeight, FontStretch), FontSize,
                                       Foreground);

            var wd = ft.Width + fpow.Width - 1;
            var hg = ft.Height + fpow.Height - 7;

            Rect rect;

            var p = new Pen(Brushes.Black, 1);
            var halfPenSize = p.Thickness / 2;
            var pos = Math.Round(position, MidpointRounding.AwayFromZero);
            dc.PushGuidelineSet(new GuidelineSet(new double[0], new[] { pos - halfPenSize }));
            switch (AxisAlignment) {
                case HorizontalAlignment.Left:
                    rect = GetRectangleByAlignment(new Point(_tickSize + 2, pos),
                                          new Size(wd, hg), TextAlignment);
                    dc.DrawLine(p, new Point(0, pos), new Point(_tickSize, pos));
                    break;
                case HorizontalAlignment.Right:
                    rect = GetRectangleByAlignment(new Point(ActualWidth - (_tickSize + 2), pos),
                                          new Size(wd, hg), TextAlignment);
                    dc.DrawLine(p, new Point(ActualWidth - _tickSize, pos), new Point(ActualWidth, pos));
                    break;
                case HorizontalAlignment.Center:
                case HorizontalAlignment.Stretch:
                    rect = GetRectangleByAlignment(new Point(ActualWidth / 2, pos),
                                          new Size(wd, hg), TextAlignment);
                    dc.DrawLine(p, new Point(ActualWidth / 2 - _tickSize, pos), new Point(ActualWidth / 2 + _tickSize, pos));

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            dc.Pop();

            if (!OccupiedLabelPlaces.Exists(r => r.IntersectsWith(rect)))
            {
                Point ftL = rect.TopRight;
                ftL.X -= ft.Width;
                Point fpowL = rect.BottomLeft;
                fpowL.Y -= fpow.Height;
                
                dc.DrawText(ft, ftL);
                dc.DrawText(fpow, fpowL);
                OccupiedLabelPlaces.Add(rect);
            }

        }

    }
}