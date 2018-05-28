using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Nordwest.Wpf.Controls.Chart.Elements {
    public class PoligonChartElement : BasePointsChartElement {

        public PoligonChartElement() : base() { }

        public PoligonChartElement(Func<Rect,Rect> applyMargin) : base(applyMargin) { }

        public override void Draw(WriteableBitmap writeableBitmap, Rect dirtyModelRect) {

            if (writeableBitmap.PixelHeight < 2 || writeableBitmap.PixelWidth < 2) return;

            var p = new List<int>();
            
            for (int i = 1; i < Points.Count; i++) {
                if (double.IsNaN(Points[i].X) || double.IsNaN(Points[i].Y) || double.IsNaN(Points[i - 1].X) || double.IsNaN(Points[i - 1].Y)) continue;
                //var points = dirtyModelRect.CohenSutherlandLineClip(Points[i - 1], Points[i]);
                //if (points == null) continue;

                p.Add(writeableBitmap.CheckX(GetClientX(Points[i-1].X)));
                p.Add(writeableBitmap.CheckY(GetClientY(Points[i-1].Y)));
                p.Add(writeableBitmap.CheckX(GetClientX(Points[i].X)));
                p.Add(writeableBitmap.CheckY(GetClientY(Points[i].Y)));
            }

            var pAr = p.ToArray();

            if (pAr.Length > 7) {
                writeableBitmap.FillPolygon(pAr, Color.FromArgb(120, Color.R, Color.G, Color.B));
                writeableBitmap.DrawPolyline(pAr, Color);
            }

        }
    }
}
