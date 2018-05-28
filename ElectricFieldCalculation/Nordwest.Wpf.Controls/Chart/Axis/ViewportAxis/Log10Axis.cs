using System;

namespace Nordwest.Wpf.Controls.Chart {
    public class Log10Axis : ViewportAxis {
        public Log10Axis() {
            // избавляемся от дефолтного нуля
            Model = new ViewRange(0.1, 10);
        }

        public override double ViewToHomogenous(double value) => Math.Log10(value);
        public override double HomogenousToView(double value) => Math.Pow(10, value);
    }
}