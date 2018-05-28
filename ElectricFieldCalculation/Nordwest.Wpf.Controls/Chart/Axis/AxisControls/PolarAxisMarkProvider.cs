using System;
using System.Windows.Media;

namespace Nordwest.Wpf.Controls.Chart.Axis.AxisControls {
    public class PolarAxisMarkProvider {
        public double MinStep { get; set; } = 16;
        public double MinMajorStep { get; set; } = 80;

        public void IterateMarks(ViewportAxis xAxis, ViewportAxis yAxis, DrawingContext dc, Action<DrawingContext, double, string> action) {

        }

        public void IterateRadiusGrid(ViewportAxis xAxis, ViewportAxis yAxis, Action<double, double, double, double> drawGridAction, bool major) {
            if (xAxis == null || yAxis == null)
                return;

            var xInterval = ToViewInterval(xAxis);
            var yInterval = ToViewInterval(yAxis);
            
            var x0 = ToClient(xAxis, 0);
            var y0 = ToClient(yAxis, 0);

            var xmax = Math.Max(Math.Abs(xInterval.Start), Math.Abs(xInterval.End));
            var ymax = Math.Max(Math.Abs(yInterval.Start), Math.Abs(yInterval.End));
            var rmax = Math.Sqrt(xmax * xmax + ymax * ymax);

            double angleStep;

            if (major)
                angleStep = Math.PI * 40 / 180;
            else
                angleStep = Math.PI * 10 / 180;

            for (double angle = 0; angle < 2 * Math.PI; angle += angleStep) {
                var cx = ToClient(xAxis, rmax * Math.Cos(angle));
                var cy = ToClient(yAxis, rmax * Math.Sin(angle));
                drawGridAction.Invoke(x0, y0, cx, cy);
            }
        }

        public void IterateEllipseGrid(ViewportAxis xAxis, ViewportAxis yAxis, Action<double, double, double, double> drawGridAction, bool major) {
            if (xAxis == null || yAxis == null)
                return;

            var xInterval = ToViewInterval(xAxis);
            var yInterval = ToViewInterval(yAxis);

            var minStep = major ? MinMajorStep : MinStep;

            var xmstep = AxisHelper.GetStep(xAxis, minStep);
            var ymstep = AxisHelper.GetStep(yAxis, minStep);

            var x0 = ToClient(xAxis, 0);
            var y0 = ToClient(yAxis, 0);

            var xmax = Math.Max(Math.Abs(xInterval.Start), Math.Abs(xInterval.End));
            var ymax = Math.Max(Math.Abs(yInterval.Start), Math.Abs(yInterval.End));
            var rmax = Math.Sqrt(xmax * xmax + ymax * ymax);

            for (double x = xmstep, y = ymstep; x <= rmax || y <= rmax; x += xmstep, y += ymstep) {
                var cx = Math.Abs(ToClient(xAxis, x) - x0);
                var cy = Math.Abs(ToClient(yAxis, y) - y0);
                drawGridAction.Invoke(x0, y0, cx, cy);
            }
        }
        
        protected static double ToClient(ViewportAxis axis, double value) {
            return axis.ViewToClient(value);
        }
        protected static double ToValue(ViewportAxis axis, double client) {
            return axis.ClientToView(client);
        }

        protected static Interval ToViewInterval(ViewportAxis axis) {
            var v = axis.View;
            var s = v.Start;
            var e = v.End;

            return new Interval(Math.Min(s, e), Math.Max(s, e));
        }
    }
}
