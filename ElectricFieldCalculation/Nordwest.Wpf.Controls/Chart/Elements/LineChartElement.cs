using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Nordwest.Wpf.Controls.Chart
{
    public class LineChartElement : BasePointsChartElement
    {
        public static Action<WriteableBitmap, int, int, int, int, int> GetDashLineDrawAction(int dashSize, int pauseSize) {
            return new Action<WriteableBitmap, int, int, int, int, int>(
                (wb, x1, y1, x2, y2, iColor) => {

                    double curX = x1;
                    double curY = y1;

                    double a;
                    if (x2 == x1)
                        a = Math.PI / 2;
                    else
                        a = Math.Atan(Math.Abs((double)(y2 - y1) / (x2 - x1)));

                    double dx = dashSize * Math.Cos(a) * Math.Sign(x2 - x1);
                    double dy = dashSize * Math.Sin(a) * Math.Sign(y2 - y1);

                    double dxStep = (dashSize + pauseSize) * Math.Cos(a) * Math.Sign(x2 - x1);
                    double dyStep = (dashSize + pauseSize) * Math.Sin(a) * Math.Sign(y2 - y1);

                    while ((x2 > x1 && curX < x2) || (x2 < x1 && curX > x2) || (y2 > y1 && curY < y2) || (y2 < y1 && curY > y2)) {

                        var eX = curX + dx;
                        if ((x2 > x1 && eX > x2) || (x2 < x1 && eX < x2))
                            eX = x2;

                        var eY = curY + dy;
                        if ((y2 > y1 && eY > y2) || (y2 < y1 && eY < y2))
                            eY = y2;

                        wb.DrawLineAa((int)curX, (int)curY, (int)eX, (int)eY, iColor);
                        curX += dxStep;
                        curY += dyStep;
                    }
                });
        }
        
        private Action<WriteableBitmap, int, int, int, int, int> _drawAction = (wb, x1, y1, x2, y2, iColor) => {
            wb.DrawLineAa(x1, y1, x2, y2, iColor);
        };

        public LineChartElement(Action<WriteableBitmap, int, int, int, int, int> drawMarkMethod) {
            _drawAction = drawMarkMethod;
        }
        public LineChartElement(Action<WriteableBitmap, int, int, int, int, int> drawMarkMethod, Func<Rect, Rect> getBoundsRect) : base(getBoundsRect) {
            _drawAction = drawMarkMethod;
        }
        public LineChartElement(Func<Rect,Rect> getModelBounds):base(getModelBounds){}
        public LineChartElement() {}

        public LineChartElement(int stroke) {
            _drawAction = (wb, x1, y1, x2, y2, iColor) => wb.DrawLineAa(x1, y1, x2, y2, iColor, stroke);
        }

        public LineChartElement(int stroke, Func<Rect, Rect> getModelBounds) : base(getModelBounds) {
            _drawAction = (wb, x1, y1, x2, y2, iColor) => wb.DrawLineAa(x1, y1, x2, y2, iColor, stroke);
        }

        public override void Draw(WriteableBitmap writeableBitmap, Rect dirtyModelRect)
        {
            if (writeableBitmap.PixelHeight < 2 || writeableBitmap.PixelWidth < 2) return;

            var p = new List<int>();
            
            for (int i = 1; i < Points.Count; i++)
            {
                if (double.IsNaN(Points[i].X) || double.IsNaN(Points[i].Y) || double.IsNaN(Points[i - 1].X) || double.IsNaN(Points[i - 1].Y)) continue;
                var points = dirtyModelRect.CohenSutherlandLineClip(Points[i - 1], Points[i]);
                if (points == null) continue;

                var x1 = writeableBitmap.CheckX(GetClientX(points[0].X));
                var y1 = writeableBitmap.CheckY(GetClientY(points[0].Y));
                var x2 = writeableBitmap.CheckX(GetClientX(points[1].X));
                var y2 = writeableBitmap.CheckY(GetClientY(points[1].Y));
                
                _drawAction(writeableBitmap, x1, y1, x2, y2, IntColor);
                
            }
        }
    }

    public class PointsChartElement : BasePointsChartElement
    {
        public static Action<WriteableBitmap, int, int, int> GetEllipseDrawAction(int size) {
            if (size <= 0)
                throw new ArgumentOutOfRangeException(nameof(size), @"size <= 0");
            return (wb, x, y, iCol) => wb.FillEllipse(x - size, y - size, x + size, y + size, iCol);
        }

        private readonly Action<WriteableBitmap, int, int, int> _drawAction;
        public PointsChartElement(Action<WriteableBitmap, int, int, int> drawMarkMethod)
            : this(drawMarkMethod, r => r) { }
        public PointsChartElement()
            : this((wb, x, y, iCol) => wb.FillEllipse(x - 2, y - 2, x + 2, y + 2, iCol), r => r) { }
        public PointsChartElement(Func<Rect, Rect> getBoundsRect)
            : this((wb, x, y, iCol) => wb.FillEllipse(x - 2, y - 2, x + 2, y + 2, iCol), getBoundsRect) { }
        public PointsChartElement(Action<WriteableBitmap, int, int, int> drawMarkMethod, Func<Rect, Rect> getBoundsRect)
            : base(getBoundsRect)
        {
            _drawAction = drawMarkMethod;
        }

        public override void Draw(WriteableBitmap writeableBitmap, Rect dirtyModelRect)
        {
            if (writeableBitmap.PixelHeight < 2 || writeableBitmap.PixelWidth < 2) return;

            foreach (var point in Points.Where(dirtyModelRect.Contains))
                DrawMark(writeableBitmap, point);
        }

        private void DrawMark(WriteableBitmap wb, Point point)
        {
            var x = wb.CheckX(GetClientX(point.X));
            var y = wb.CheckY(GetClientY(point.Y));
            _drawAction.Invoke(wb, x, y, IntColor);
        }
    }

}