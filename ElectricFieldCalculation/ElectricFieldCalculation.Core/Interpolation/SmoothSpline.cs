
using System;

namespace SynteticData.Interpolation {
    public class SmoothSpline {

        #region Constants

        //    const int NTgmax = 10;
        //   const int NTimax = 10;
        const double Fi = 0.618;

        #endregion

        #region Private Fields
        /*
      ╔═════════════════════════════════════════════════════╗
      ║           S M O O T H    S P L I N E                ║
      ║      ( Построение сглаживающего сплайна )           ║
      ╟─────────────────────────────────────────────────────╢
      ║                 Входные данные:                     ║
      ║     key : 1 - задается параметр сглаживания р,      ║
      ║                   ( 0 <= p <= 1 ),                  ║
      ║           2 - р находится из условия ( sfp < s ),   ║
      ║               где s - дисперсия исходных данных,    ║
      ║               а  sfp - невязка сглаженной и исход-  ║
      ║               ной кривых,                           ║
      ║     X,Y : массивы абсцисс и ординат исходной кри-   ║
      ║           вой ( абсциссы предполагаются возраста-   ║
      ║           ющими ),                                  ║
      ║      dY : относительная  оценка  достоверности ис-  ║
      ║           ходных данных,                            ║
      ║  Npoint : число точек исходной кривой,              ║
      ║     S   : дисперсия исходных данных ( при key = 1   ║
      ║           не задается )                             ║
      ╟─────────────────────────────────────────────────────╢
      ║                Выходные данные:                     ║
      ║       A1   : массив ординат сглаженной  кривой  в   ║
      ║              точках X[i] ,                          ║
      ║   A2,A3,A4 : массивы первых трех производных сгла-  ║
      ║              женной кривой в точках X[i],           ║
      ║       Р    : найденный оптимальный  параметр сгла-  ║
      ║              живания ( при key = 2 ),               ║
      ║     sfp    : невязка исходной и сглаженной кривой   ║
      ╚═════════════════════════════════════════════════════╝      


      ╔═════════════════════════════════════════════════════╗
      ║    S P L I N E   -   I N T E R P O L A T I O N      ║
      ╟─────────────────────────────────────────────────────╢
      ║ После нахождения с помощью процедуры SmSpl коэффи-  ║
      ║ циентов cглаживающего или интерполяционного ( key = ║
      ║ 1, p = 1 ) сплайна  можно  проинтерполировать  его  ║
      ║ значения в заданных точках ( процедура Int )        ║
      ║               Входные данные:                       ║
      ║     X        : абсциссы исходной кривой,            ║
      ║  A1,A2,A3,A4 : массивы ординат и первых трех про-   ║
      ║                изводных сплайна,                    ║
      ║    Npoint    : число точек исходной кривой,         ║
      ║  Xi,Nipoint  : абсциссы  и число точек, в которых   ║
      ║                требуется получить значения сплайна  ║
      ╟─────────────────────────────────────────────────────╢
      ║             Выходные данные:                        ║
      ║     Yi       : массив  ординат  сплайна  в  точках  ║
      ║                    Xi[i]                            ║
      ╚═════════════════════════════════════════════════════╝    
        */
        private int npoint;

        private double[] xg;// = new double[SmoothSpline.NTgmax];
        private double[] yg; //= new double[SmoothSpline.NTgmax];
        private double[] dyg;// = new double[SmoothSpline.NTgmax];

        private double[] a1;// = new double[SmoothSpline.NTgmax];
        private double[] a2;// = new double[SmoothSpline.NTgmax];
        private double[] a3;// = new double[SmoothSpline.NTgmax];
        private double[] a4;// = new double[SmoothSpline.NTgmax];

        private string Borders = "zzz";

        private bool pIsExplicit = true;
        private double p = 0.5;
        private double s = 0.5;
        private bool isReady = false;

        #endregion

        #region Public Methods

        /// <summary>
        /// Подготовить интерполяционный сплайн
        /// </summary>
        /// <param name="x">список абсцисс</param>
        /// <param name="y">список ординат</param>
        /// <param name="dy">невязка(?или что то еще?) ординат (относительная  оценка  достоверности исходных данных</param>
        public void PrepareInterpolationFunction(double[] x, double[] y, double[] dy) {
            if (!IsValuesSuccessive(x)) {
                throw new ArgumentException();
            }

            xg = x;
            yg = y;
            npoint = x.Length;
            dyg = dy;

            a1 = new double[x.Length];
            a2 = new double[x.Length];
            a3 = new double[x.Length];
            a4 = new double[x.Length];

            //   private void Smooth(int key, double s, double p, double sfp)

            double sfp = 10;

            SmSpl(pIsExplicit ? 1 : 2, s, ref p, ref sfp);

            isReady = true;
        }

        /// <summary>
        /// Сама интерполяция
        /// </summary>
        public double Interpolate(double x) {
            double dx;

            if (x < xg[0]) {
                if (Borders == "const") {
                    return a1[0];
                }
                dx = xg[0] - x;
                return a1[1] - a2[1] * dx - a3[1] / 2 * Math.Pow(dx, 2) - a4[1] / 6 * Math.Pow(dx, 2) * dx;
            }
            if (x >= xg[npoint - 1]) {
                return a1[npoint - 1];
            }
            int index = 0;
            dx = 0;
            for (int i = 0; i < xg.Length - 1; i++) {
                if (x >= xg[i] && x < xg[i + 1]) {
                    dx = x - xg[i];
                    index = i;
                    break;
                }
            }
            return a1[index] + a2[index] * dx + a3[index] / 2 * Math.Pow(dx, 2) + a4[index] / 6 * Math.Pow(dx, 2) * dx;
        }

        /// <summary>
        /// Можно ли интерполировать
        /// </summary>
        public bool IsReady {
            get { return isReady; }
        }

        #endregion

        #region Public Properties
        /// <summary>
        /// Параметр сглаживания. Задается явно если PIsExplicit == true
        /// Иначе находиться из условия 
        /// (невязка исходной и сглаженной прямой меньше S)
        /// <![CDATA[0 <= p <= 1]]>
        /// </summary>
        public double P {
            get { return p; }
            set {
                if (value < 0 || value > 1) {
                    throw new ArgumentOutOfRangeException();
                }
                p = value;
            }
        }

        /// <summary>
        /// Дисперсия исходных данных (задается, если PIsExplicit == false)
        /// </summary>
        public double S {
            get { return s; }
            set { s = value; }
        }

        /// <summary>
        /// Задаем ли P напрямую, или оно рассчитывается исходя из S
        /// </summary>
        public bool PIsExplicit {
            get { return pIsExplicit; }
            set { pIsExplicit = value; }
        }

        #endregion

        #region Private Methods

        private bool IsValuesSuccessive(double[] val) {
            for (int i = 1; i < val.Length; i++) {
                if (val[i] < val[i - 1]) {
                    return false;
                }
            }
            return true;
        }

        private void SmSpl(int key, double s, ref double p, ref double sfp) {
            double[,] v = new double[xg.Length, 7];
            double six1mp, sixp;

            Setupq(v, a4);

            #region Find P if key == 2

            if (key != 1) {
                if (s == 0) {
                    p = 1;
                    Chol1D(p, v, a4, a3, a1);
                    sfp = 0;
                }
                else {
                    p = 0;
                    sfp = Ff(p, v, a4, a3, a1);
                    if (sfp > s) { p = MinP(0, 1, s, v, a4, a3, a1); }
                }
            }

            #endregion



            sfp = Ff(p, v, a4, a3, a1);


            six1mp = 6 * (1 - p);

            //for i := 1 to npoint do
            for (int i = 0; i < npoint; i++) {
                a1[i] = yg[i] - six1mp * System.Math.Pow(dyg[i], 2) * a1[i];
            }



            sixp = 6 * p;
            //for i := 1 to npoint do a3[i] := a3[i]*sixp;
            for (int i = 1; i <= npoint; i++) {
                a3[i - 1] = a3[i - 1] * sixp;
            }

            // for i := 1 to npoint-1 do begin
            for (int i = 1; i <= npoint - 1; i++) {
                a4[i - 1] = (a3[i] - a3[i - 1]) / v[i - 1, 3];
                a2[i - 1] = (a1[i] - a1[i - 1]) / v[i - 1, 3] - (a3[i - 1] + a4[i - 1] / 3 * v[i - 1, 3]) / 2 * v[i - 1, 3];
            }
        }

        private void Setupq(double[,] v, double[] qty) {
            int npm1 = npoint - 1;
            double diff, prev;

            v[0, 3] = xg[1] - xg[0];

            for (int i = 2; i <= npm1; i++) {
                v[i - 1, 3] = xg[i] - xg[i - 1];
                v[i - 1, 0] = dyg[i - 2] / v[i - 2, 3];
                v[i - 1, 1] = -dyg[i - 1] / v[i - 1, 3] - dyg[i - 1] / v[i - 2, 3];
                v[i - 1, 2] = dyg[i] / v[i - 1, 3];
            }
            v[npoint - 1, 0] = 0;

            for (int i = 1; i < npm1; i++) {
                v[i, 4] = Math.Pow(v[i, 0], 2) + Math.Pow(v[i, 1], 2) + Math.Pow(v[i, 2], 2);
            }

            if (npm1 >= 3) {
                for (int i = 3; i <= npm1; i++) {
                    v[i - 2, 5] = v[i - 2, 1] * v[i - 1, 0] + v[i - 2, 2] * v[i - 1, 1];
                }
            }

            v[npm1 - 1, 5] = 0;

            if (npm1 >= 4) {
                for (int i = 4; i <= npm1; i++) {
                    v[i - 1, 6] = v[i - 3, 2] * v[i - 1, 0];
                }
            }

            v[npm1 - 2, 6] = 0;
            v[npm1 - 1, 6] = 0;

            prev = (yg[1] - yg[0]) / v[0, 3];

            for (int i = 2; i <= npm1; i++) {
                diff = (yg[i] - yg[i - 1]) / v[i - 1, 3];
                qty[i - 1] = diff - prev;
                prev = diff;
            }
        }

        private void Chol1D(double p, double[,] v, double[] qty, double[] u, double[] qu) {
            int npm1, npm2;
            double prev, six1mp, twop, ratio;

            npm1 = npoint - 1;
            six1mp = 6 * (1 - p);
            twop = 2 * p;

            for (int i = 2; i <= npm1; i++) {
                v[i - 1, 0] = six1mp * v[i - 1, 4] + twop * (v[i - 2, 3] + v[i - 1, 3]);
                v[i - 1, 1] = six1mp * v[i - 1, 5] + p * v[i - 1, 3];
                v[i - 1, 2] = six1mp * v[i - 1, 6];
            }

            npm2 = npoint - 2;
            if (npm2 < 2) {
                u[0] = 0;
                u[1] = qty[1] / v[1, 0];
                u[2] = 0;
            }
            else {
                for (int j = 2; j <= npm2; j++) {
                    int i = j - 1;
                    ratio = v[i, 1] / v[i, 0];
                    v[i + 1, 0] = v[i + 1, 0] - ratio * v[i, 1];
                    v[i + 1, 1] = v[i + 1, 1] - ratio * v[i, 2];
                    v[i, 1] = ratio;
                    ratio = v[i, 2] / v[i, 0];
                    v[i + 2, 0] = v[i + 2, 0] - ratio * v[i, 2];
                    v[i, 2] = ratio;
                }

                u[0] = 0;
                v[0, 2] = 0;
                u[1] = qty[1];

                for (int i = 1; i < npm2; i++) {
                    u[i + 1] = qty[i + 1] - v[i, 1] * u[i] - v[i - 1, 2] * u[i - 1];
                }

                u[npoint - 1] = 0;
                u[npm1 - 1] = u[npm1 - 1] / v[npm1 - 1, 0];

                for (int j = npm2; j >= 2; j--) {
                    int i = j - 1;
                    u[i] = u[i] / v[i, 0] - u[i + 1] * v[i, 1] - u[i + 2] * v[i, 2];
                }
            }

            prev = 0;
            for (int j = 2; j <= npoint; j++) {
                int i = j - 1;
                qu[i] = (u[i] - u[i - 1]) / v[i - 1, 3];
                qu[i - 1] = qu[i] - prev;
                prev = qu[i];
            }

            qu[npoint - 1] = -qu[npoint - 1];
        }

        private double Ff(double p, double[,] v, double[] a1, double[] a2, double[] a3) {
            Chol1D(p, v, a1, a2, a3);

            double sfp = 0;

            for (int i = 0; i < npoint; i++) {
                sfp = sfp + Math.Pow(a3[i] * dyg[i], 2);
            }

            return sfp * 36 * Math.Pow(1 - p, 2);
        }

        private double MinP(double xs, double xf, double s, double[,] v, double[] a1, double[] a2, double[] a3) {
            double m, l, a, b, fl, fm, fa, fb;
            int k;

            l = xs + (1 - Fi) * (xf - xs);
            m = xs + Fi * (xf - xs);
            k = 1;
            a = xs;
            b = xf;
            fl = Math.Abs(s - Ff(l, v, a1, a2, a3));
            fm = Math.Abs(s - Ff(m, v, a1, a2, a3));
            fa = fl;
            fb = fm;

            while (fl > s * 0.01 || fm > s * 0.01) {
                if (fl < fm) {
                    b = m;
                    m = l;
                    fb = fm;
                    l = a + (1 - Fi) * (b - a);
                    fm = fl;
                    fl = Math.Abs(s - Ff(l, v, a1, a2, a3));
                }
                else {
                    a = l;
                    l = m;
                    fa = fl;
                    m = a + Fi * (b - a);
                    fl = fm;
                    fm = Math.Abs(s - Ff(m, v, a1, a2, a3));
                }

                k++;
            }

            return (a + b) / 2;
        }

        #endregion

    }
}
