using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Nordwest.Wpf.Controls.Chart.Elements {
    public class LinePointsChartElement : BasePointsChartElement {
        
        private readonly Action<WriteableBitmap, int, int, int> _drawAction;

        public LinePointsChartElement(Func<Rect, Rect> getModelBounds, Action<WriteableBitmap, int, int, int> drawAction) : base(getModelBounds) {
            _drawAction = drawAction; 
        }

        public LinePointsChartElement() {
            _drawAction = (wb, x, y, iCol) => wb.FillEllipse(x - 2, y - 2, x + 2, y + 2, iCol);
        }
        
        public override void Draw(WriteableBitmap writeableBitmap, Rect dirtyModelRect) {
            if (writeableBitmap.PixelHeight < 2 || writeableBitmap.PixelWidth < 2) return;

            for (int i = 1; i < Points.Count; i++) {
                if (double.IsNaN(Points[i].X) || double.IsNaN(Points[i].Y) || double.IsNaN(Points[i - 1].X) || double.IsNaN(Points[i - 1].Y)) continue;
                var points = dirtyModelRect.CohenSutherlandLineClip(Points[i - 1], Points[i]);
                if (points == null) continue;
                
                var x1 = writeableBitmap.CheckX(GetClientX(points[0].X));
                var y1 = writeableBitmap.CheckY(GetClientY(points[0].Y));
                var x2 = writeableBitmap.CheckX(GetClientX(points[1].X));
                var y2 = writeableBitmap.CheckY(GetClientY(points[1].Y));
                writeableBitmap.DrawLineAa(x1, y1, x2, y2, _intColor);
            }

            foreach (var point in Points.Where(dirtyModelRect.Contains))
                DrawMark(writeableBitmap, point);
        }

        private void DrawMark(WriteableBitmap wb, Point point) {
            var x = wb.CheckX(GetClientX(point.X));
            var y = wb.CheckY(GetClientY(point.Y));
            _drawAction.Invoke(wb, x, y, IntColor);
        }
    }
}
