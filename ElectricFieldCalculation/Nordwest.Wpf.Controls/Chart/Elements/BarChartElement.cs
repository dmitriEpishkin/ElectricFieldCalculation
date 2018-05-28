using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Nordwest.Wpf.Controls.Chart
{
    public class BarChartElement : ChartElement
    {
        private readonly List<BarPoint> _points = new List<BarPoint>();
        //private double _halfModelWidth = 0.5;
        //public double ModelWidth
        //{
        //    get { return _halfModelWidth * 2.0; }
        //    set { _halfModelWidth = value / 2.0; }
        //}


        public List<BarPoint> Points
        {
            get { return _points; }
        }

        /// <summary>
        /// дополнительные отступы к модели.
        /// </summary>
        public Thickness Padding { get; set; }

        public override void Draw(WriteableBitmap writeableBitmap, Rect dirtyModelRect)
        {
            for (int i = 0; i < Points.Count; i++)
            {
                if (Points[i].X1 >= Viewport.HorizontalAxis.ModelEnd ||
                    Points[i].X2 <= Viewport.HorizontalAxis.ModelStart) continue;
                var x1 = writeableBitmap.CheckX(GetClientX(Points[i].X1));
                var x2 = writeableBitmap.CheckX(GetClientX(Points[i].X2));
                var y1 = writeableBitmap.CheckY(GetClientY(Points[i].Y));
                var y2 = Viewport.VerticalAxis.IsReversed ? writeableBitmap.PixelHeight - 1 : 0;

                writeableBitmap.FillRectangle(x1, y1, x2, y2, Points[i].Color);
                if (Points[i].BorderColor != 0)
                    writeableBitmap.DrawRectangle(x1, y1, x2, y2 + 1, Points[i].BorderColor);
            }
        }

        public override Rect GetModelBounds()
        {
            if (Points.Count == 0) return Rect.Empty;
            var barPoints = Points.Where(p => !(double.IsNaN(p.X1) || double.IsNaN(p.X2) || double.IsNaN(p.Y))).ToList();
            if (!barPoints.Any()) return Rect.Empty;
            var x = barPoints.Min(p => p.X1);
            var y = barPoints.Min(p => p.Y);
            var w = barPoints.Max(p => p.X2) - x;
            var h = barPoints.Max(p => p.Y) - y;

            return new Rect(x - Padding.Left, y - Padding.Top, w + Padding.Right + Padding.Left, h + Padding.Bottom + Padding.Top);
        }

        public override ChartElementHitTestResult HitTest(Point modelPoint)
        {
            for (var i = 0; i < Points.Count; i++)
            {
                if (modelPoint.X < Points[i].X1 || modelPoint.X > Points[i].X2) continue;

                if (Viewport.VerticalAxis.IsReversed)
                {
                    if (modelPoint.Y <= Points[i].Y)
                        return new BarChartHitTestResult(i, true);
                }
                else
                {
                    if (modelPoint.Y >= Points[i].Y)
                        return new BarChartHitTestResult(i, true);
                }
                return new BarChartHitTestResult(-1, false);
            }
            return new BarChartHitTestResult(-1, false);
        }
    }
    public class BarChartHitTestResult : ChartElementHitTestResult
    {
        private readonly int _pointIndex;

        public BarChartHitTestResult(int pointIndex, bool succes)
            : base(succes)
        {
            _pointIndex = pointIndex;
        }

        public int PointIndex
        {
            get { return _pointIndex; }
        }
    }

    public struct BarPoint
    {
        private readonly double _x1;
        private readonly double _x2;
        private readonly double _y;
        private readonly int _color;
        private readonly int _borderColor;

        /// <summary>
        /// Bar data point.
        /// </summary>
        /// <param name="x1">start position</param>
        /// <param name="x2">end position</param>
        /// <param name="y">top position</param>
        /// <param name="color">fill color</param>
        /// <param name="borderColor">border color</param>
        public BarPoint(double x1, double x2, double y, Color color, Color borderColor)
            : this()
        {
            _x1 = x1;
            _y = y;
            _borderColor = Helpers.ConvertColor(borderColor);
            _x2 = x2;
            _color = Helpers.ConvertColor(color);
        }

        public double X2
        {
            get { return _x2; }
        }

        public double X1
        {
            get { return _x1; }
        }

        public double Y
        {
            get { return _y; }
        }

        public int Color
        {
            get { return _color; }
        }
        public int BorderColor
        {
            get { return _borderColor; }
        }
    }
}