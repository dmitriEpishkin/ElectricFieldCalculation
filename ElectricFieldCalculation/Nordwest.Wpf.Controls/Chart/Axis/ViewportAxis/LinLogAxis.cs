using System;

namespace Nordwest.Wpf.Controls.Chart {
    public class LinLogAxis : ViewportAxis {
        private double LongLogEps = 0.001;

        public override double ViewToHomogenous(double value) {
            var x = value / LongLogEps;
            return Math.Log(x + Math.Sqrt(x * x + 1));
        }
        public override double HomogenousToView(double value) {
            var x = 0.5 * (Math.Exp(value) - Math.Exp(-value));
            return x * LongLogEps;
        }
    }
}