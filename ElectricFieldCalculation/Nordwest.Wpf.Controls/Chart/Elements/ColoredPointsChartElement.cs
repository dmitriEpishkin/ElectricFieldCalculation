using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Nordwest.Wpf.Controls.Chart.Elements
{
    /// <summary>
    /// Расскрашенный в разные цвета график из точек
    /// </summary>
    public class ColoredPointsChartElement : ChartElement
    {
        private readonly List<int> _intColors = new List<int>();
        private readonly List<Point> _points = new List<Point>();

        private readonly Func<Rect, Rect> _getBoundsRect;

        private readonly Action<WriteableBitmap, int, int, int> _drawAction;

        public ColoredPointsChartElement(Action<WriteableBitmap, int, int, int> drawMarkMethod)
            : this(drawMarkMethod, r => r) { }

        public ColoredPointsChartElement()
            : this((wb, x, y, iCol) => wb.FillEllipse(x - 2, y - 2, x + 2, y + 2, iCol), r => r) { }

        public ColoredPointsChartElement(Func<Rect, Rect> getBoundsRect)
            : this((wb, x, y, iCol) => wb.FillEllipse(x - 2, y - 2, x + 2, y + 2, iCol), getBoundsRect) { }

        public ColoredPointsChartElement(Action<WriteableBitmap, int, int, int> drawMarkMethod, Func<Rect, Rect> getBoundsRect)
        {
            _drawAction = drawMarkMethod;
            _getBoundsRect = getBoundsRect;
        }

        public void AddData(Point point, Color color) {
            _intColors.Add(Helpers.ConvertColor(color));
            _points.Add(point);
        }

        public void Clear()
        {
            _intColors.Clear();
            _points.Clear();
        }

        public override void Draw(WriteableBitmap writeableBitmap, Rect dirtyModelRect)
        {
            if (writeableBitmap.PixelHeight < 2 || writeableBitmap.PixelWidth < 2) return;

            for (int i = 0; i < _points.Count; i++) {
                if (!dirtyModelRect.Contains(_points[i]))
                    continue;

                DrawMark(writeableBitmap, _points[i], _intColors[i]);
            }
        }

        private void DrawMark(WriteableBitmap wb, Point point, int intColor)
        {
            var x = wb.CheckX(GetClientX(point.X));
            var y = wb.CheckY(GetClientY(point.Y));
            _drawAction.Invoke(wb, x, y, intColor);
        }

        public override Rect GetModelBounds()
        {
            if (_points.Count == 0)
                return Rect.Empty;

            var x = _points.Where(p => !double.IsNaN(p.X)).Min(p => p.X);
            var y = _points.Where(p => !double.IsNaN(p.Y)).Min(p => p.Y);
            var w = _points.Where(p => !double.IsNaN(p.X)).Max(p => p.X) - x;
            var h = _points.Where(p => !double.IsNaN(p.Y)).Max(p => p.Y) - y;

            return _getBoundsRect.Invoke(new Rect(x, y, w, h));
        }

        public override ChartElementHitTestResult HitTest(Point modelPoint)
        {
            return new ChartElementHitTestResult(false);//hittest not supported
        }

    }
}
