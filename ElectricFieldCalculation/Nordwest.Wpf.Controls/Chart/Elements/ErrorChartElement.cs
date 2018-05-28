using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Nordwest.Wpf.Controls.Properties;

namespace Nordwest.Wpf.Controls.Chart
{
    public class ErrorChartElement : ChartElement
    {
        private readonly Func<Rect, Rect> _getBoundsRect;
        private readonly List<ErrorData> _points = new List<ErrorData>();
        private int _capSize = 2;
        private int _intColor = 0;

        public ErrorChartElement() : this(rect => rect) {}

        public ErrorChartElement(Func<Rect, Rect> getBoundsRect)
        {
            _getBoundsRect = getBoundsRect;
        }

        public override void Draw(WriteableBitmap writeableBitmap, Rect dirtyModelRect)
        {
            if (writeableBitmap.PixelHeight < 2 || writeableBitmap.PixelWidth < 2)
                return;

            for (int i = 0; i < Points.Count; i++)
            {
                if (double.IsNaN(Points[i].X) || double.IsNaN(Points[i].Y))
                    continue;
                if (Points[i].X < dirtyModelRect.Left || Points[i].X > dirtyModelRect.Right)
                    continue;

                var x1 = writeableBitmap.CheckX(GetClientX(Points[i].X));

                var y = GetClientY(Points[i].Y);
                var y1 = GetClientY(Points[i].YMax);
                var y2 = GetClientY(Points[i].YMin);
                var y11 = writeableBitmap.CheckY(y1);
                var y21 = writeableBitmap.CheckY(y2);
                if (y1 >= 0 && y1 < writeableBitmap.PixelHeight && y1 != y)
                    DrawCap(writeableBitmap, x1, y11);
                if (y2 >= 0 && y2 < writeableBitmap.PixelHeight && y2 != y)
                    DrawCap(writeableBitmap, x1, y21);

                writeableBitmap.DrawLineAa(x1, y11, x1, y21, _intColor);
            }

        }

        private void DrawCap(WriteableBitmap writeableBitmap, int x, int y)
        {
            if (y < 0 || y >= writeableBitmap.PixelHeight)
                return;
            var x1 = writeableBitmap.CheckX(x - _capSize);
            var x2 = writeableBitmap.CheckX(x + _capSize + 1);
            writeableBitmap.DrawLineAa(x1, y, x2, y, _intColor);
        }

        public override ChartElementHitTestResult HitTest(Point modelPoint)
        {
            return new ChartElementHitTestResult(false);//hittest not supported
        }

        public override Rect GetModelBounds()
        {
            if (Points.Count == 0)
                return Rect.Empty;
            var x = Points.Where(p => !double.IsNaN(p.X)).Min(p => p.X);
            var y = Points.Where(p => !double.IsNaN(p.Y)).Min(p => double.IsNaN(p.YMin) ? p.Y : p.YMin);
            var w = Points.Where(p => !double.IsNaN(p.X)).Max(p => p.X) - x;
            var h = Points.Where(p => !double.IsNaN(p.Y)).Max(p => double.IsNaN(p.YMax) ? p.Y : p.YMax) - y;

            return _getBoundsRect(new Rect(x, y, w, h));
        }

        public Color Color
        {
            get
            {
                var intBytes = BitConverter.GetBytes(_intColor);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(intBytes);

                return Color.FromArgb(intBytes[0], intBytes[1], intBytes[2], intBytes[3]);
            }
            set { _intColor = Helpers.ConvertColor(value); }
        }

        public int CapSize
        {
            get { return _capSize; }
            set { _capSize = value; }
        }

        public List<ErrorData> Points
        {
            get { return _points; }
        }
    }

    public struct ErrorData
    {
        private readonly double _x;
        private readonly double _y;
        private readonly double _yMax;
        private readonly double _yMin;

        public ErrorData(double x, double y, double yMax, double yMin)
        {
           // if (y < yMin || y > yMax) 
           //     throw new ArgumentException(Resources.ErrorData_ArgumentException);

            if (y < yMin) y = yMin;
            else if (y > yMax) y = yMax;

            _x = x;
            _y = y;
            _yMax = yMax;
            _yMin = yMin;
        }

        public double YMin
        {
            get { return _yMin; }
        }

        public double YMax
        {
            get { return _yMax; }
        }

        public double Y
        {
            get { return _y; }
        }

        public double X
        {
            get { return _x; }
        }
    }
}