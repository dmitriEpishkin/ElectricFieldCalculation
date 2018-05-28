using System;

namespace SynteticData.Interpolation {
    public class CubicSpline {
        /// <summary>
        /// Тип граничных условий
        /// </summary>
        public enum BounadaryConditionType {
            /// <summary>
            /// Первая производная
            /// </summary>
            FirstDerivative,
            /// <summary>
            /// Вторая производная
            /// </summary>
            SecondDerivative
        }

        #region Private Fields
        private bool isReady = false;
        private double[,] koef = null;

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Подготовка интерполяционного сплайна
        /// </summary>
        public void PrepareInterpolationFunction(double[] x, double[] y, BounadaryConditionType bct, double boundaryLeft, double boundaryRight) {
            if (!IsValuesSuccessive(x)) {
                throw new ArgumentException("x must be sorted");
            }

            if (x.Length != y.Length) {
                throw new ArgumentException("x.Length != y.Length");
            }

            koef = new double[5, x.Length];

            FirstStep(x, y); // сортировка !!
            SecondStep(x, y, bct, boundaryLeft, boundaryRight);

            isReady = true;
        }

        /// <summary>
        /// Сама интерполяция
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public double Interpolate(double x) {
            return Interpol(x);
        }

        /// <summary>
        /// Можно ли интерполировать
        /// </summary>
        public bool IsReady {
            get { return isReady; }
        }

        #endregion Public Methods

        #region Private Methods

        private bool IsValuesSuccessive(double[] val) {
            for (int i = 1; i < val.Length; i++) {
                if (val[i] < val[i - 1]) {
                    return false;
                }
            }
            return true;
        }

        private double Interpol(double x) {
            if (koef == null)
                throw new InvalidOperationException();

            int n = koef.GetLength(1);
            int l = n;
            int first = 0;

            while (l > 0) {
                int half = l / 2;
                int middle = first + half;

                if (koef[4, middle] < x) {
                    first = middle + 1;
                    l = l - half - 1;
                }
                else {
                    l = half;
                }
            }

            int i = first - 1;

            if (i < 0) {
                i = 0;
            }

            return koef[0, i] + (x - koef[4, i]) * (koef[1, i] + (x - koef[4, i]) * (koef[2, i] + koef[3, i] * (x - koef[4, i])));

        }

        // это же просто сортировка
        private void FirstStep(double[] x, double[] y) {
            int n = x.Length - 1;
            int g = (n + 1) / 2;

            do {
                int i = g;

                do {
                    int j = i - g;
                    bool c = true;

                    do {
                        if (x[j] <= x[j + g]) {
                            c = false;
                        }
                        else {
                            double tmp = x[j];
                            x[j] = x[j + g];
                            x[j + g] = tmp;

                            tmp = y[j];
                            y[j] = y[j + g];
                            y[j + g] = tmp;
                        }

                        j--;
                    }

                    while (j >= 0 & c);
                    i++;
                }
                while (i <= n);
                g = g / 2;
            }
            while (g > 0);

        }

        private void SecondStep(double[] x, double[] y, BounadaryConditionType bct, double boundaryLeft, double boundaryRight) {
            int n = x.Length;

            double b1, b2, b3, b4;

            if (bct == BounadaryConditionType.FirstDerivative) {
                b1 = 1;
                b2 = 6.0 / (x[1] - x[0]) * ((y[1] - y[0]) / (x[1] - x[0]) - boundaryLeft);
                b3 = 1;
                b4 = 6.0 / (x[n - 1] - x[n - 2]) * (boundaryRight - (y[n - 1] - y[n - 2]) / (x[n - 1] - x[n - 2]));

            }
            else {
                b1 = 0;
                b2 = 2 * boundaryLeft;
                b3 = 0;
                b4 = 2 * boundaryRight;
            }

            double dxj, dyj;

            int nxm1 = n - 1;
            if (n >= 2) {
                if (n > 2) {
                    dxj = x[1] - x[0];
                    dyj = y[1] - y[0];
                    int j = 2;

                    while (j < nxm1) {
                        double dxjp1 = x[j] - x[j - 1];
                        double dyjp1 = y[j] - y[j - 1];
                        double dxp = dxj + dxjp1;
                        koef[1, j - 1] = dxjp1 / dxp; // лямда (кроме первого и последнего)
                        koef[2, j - 1] = 1 - koef[1, j - 1]; // мю
                        koef[3, j - 1] = 6 * (dyjp1 / dxjp1 - dyj / dxj) / dxp; // d
                        dxj = dxjp1;
                        dyj = dyjp1;
                        j++;
                    }

                }
                koef[1, 0] = -b1 / 2;
                koef[2, 0] = b2 / 2;

                if (n != 2) {
                    int j = 2;
                    while (j <= nxm1) {
                        double pj = koef[2, j - 1] * koef[1, j - 2] + 2;
                        koef[1, j - 1] = -koef[1, j - 1] / pj;
                        koef[2, j - 1] = (koef[3, j - 1] - koef[2, j - 1] * koef[2, j - 2]) / pj;
                        j++;
                    }
                }

                double yppb = (b4 - b3 * koef[2, nxm1 - 1]) / (b3 * koef[1, nxm1 - 1] + 2);
                int i = 1;
                while (i <= nxm1) {
                    int j = n - i;
                    double yppa = koef[1, j - 1] * yppb + koef[2, j - 1];
                    double dx = x[j] - x[j - 1];
                    koef[3, j - 1] = ((yppb - yppa) / dx) / 6;
                    koef[2, j - 1] = yppa / 2;
                    koef[1, j - 1] = (y[j] - y[j - 1]) / dx - (koef[2, j - 1] + koef[3, j - 1] * dx) * dx;
                    yppb = yppa;
                    i++;
                }

                for (int j = 1; j <= n; j++) {
                    koef[0, j - 1] = y[j - 1];
                    koef[4, j - 1] = x[j - 1];
                }
            }
        }

        #endregion Private Methods

    }
}
