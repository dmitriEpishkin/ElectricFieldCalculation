using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nordwest.Wpf.Controls.Chart.Axis.AxisControls {
    public static class AxisHelper {

        public static double GetStep(ViewportAxis axis, double minStep) {
            var mstep = Math.Abs(ToValue(axis, minStep) - ToValue(axis, 0));
            var pow = Math.Floor(Math.Log10(mstep)); //степень шага
            if (mstep <= 2 * Math.Pow(10, pow))
                //если модельный шаг в диапазоне (1; 2] * 10^"степень шага" - округляем до шага 2
                mstep = 2 * Math.Pow(10, pow);
            else if (mstep <= 5 * Math.Pow(10, pow))
                //если в диапазоне от (2, 5] * 10^"степень шага" - округляем с шагом 5
                mstep = 5 * Math.Pow(10, pow);
            else //если (5, 10] * 10^"степень шага" - с шагом 10
                mstep = 10 * Math.Pow(10, pow);
            return mstep;
        }

        private static double ToClient(ViewportAxis axis, double value) {
            return axis.ViewToClient(value);
        }
        private static double ToValue(ViewportAxis axis, double client) {
            return axis.ClientToView(client);
        }
    }
}
