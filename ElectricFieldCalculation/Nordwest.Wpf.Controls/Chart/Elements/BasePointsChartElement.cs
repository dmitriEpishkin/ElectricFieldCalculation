using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Nordwest.Wpf.Controls.Chart
{
    public abstract class BasePointsChartElement : ChartElement
    {
        private readonly List<Point> _points = new List<Point>();
        protected int _intColor = 0;
        private readonly Func<Rect, Rect> _getBoundsRect;

        protected BasePointsChartElement(Func<Rect, Rect> getBoundsRect)
        {
            _getBoundsRect = getBoundsRect;
        }

        protected BasePointsChartElement():this(r=>r){}

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

        protected int IntColor { get { return _intColor; } }

        public List<Point> Points
        {
            get { return _points; }
        }

        public override Rect GetModelBounds()
        {
            if (Points.Count == 0)
                return Rect.Empty;

            var px = Points.Where(p => !double.IsNaN(p.X));
            var py = Points.Where(p => !double.IsNaN(p.Y));

            var x = px.Any() ? px.Min(p => p.X) : 1;
            var y = py.Any() ? py.Min(p => p.Y) : 1;
            var w = (px.Any() ? px.Max(p => p.X) : 10) - x;
            var h = (py.Any() ? py.Max(p => p.Y) : 10) - y;

            return _getBoundsRect.Invoke(new Rect(x, y, w, h));
        }

        public override ChartElementHitTestResult HitTest(Point modelPoint)
        {
            return new ChartElementHitTestResult(false);//hittest not supported
        }
    }
}