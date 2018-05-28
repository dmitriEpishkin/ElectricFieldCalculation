using System;
using System.Numerics;

namespace SynteticData.FFT {
    public class Fft {
        public static Complex[] Forward(double[] data) {
            int Nx = data.Length;

            Complex[] spectr = new Complex[Nx];
            double[] re = new double[Nx];
            double[] im = new double[Nx];

            int m = 1; int k = 2;
            while (k < Nx) { k *= 2; m++; }
            int NX = k;
            for (int ix = 0; ix < NX; ix++) {
                re[ix] = data[ix];
                im[ix] = 0f;
            }
            int key = 0;
            FftCore(NX, m, key, ref re, ref im);

            double delitel;
            delitel = (double)(NX/2);

            for (int i = 0; i < data.Length; i++) {
                spectr[i] = new Complex(re[i] / delitel, im[i] / delitel);
            }
            return spectr;
        }

        public static double[] Back(Complex[] data) {
            int Nx = data.Length;

            double[] re = new double[Nx];
            double[] im = new double[Nx];

            int m = 1; int k = 2;
            while (k < Nx) { k *= 2; m++; }
            int NX = k;
            for (int ix = 0; ix < NX; ix++) {
                re[ix] = data[ix].Real;
                im[ix] = data[ix].Imaginary;
            }
            int key = 1;
            FftCore(NX, m, key, ref re, ref im);

            return re;
        }

        /*-----------------------------------------------------------------*/
        /*                                                                 */
        /*		pnt - число точек в массиве (pnt = 2**m)                   */
        /*		m - степень числа 2                                        */
        /*		key = 0 - прямое преобразование, key = 1 - обратное        */
        /*		re, im - массивы чисел									   */
        private static void FftCore(int pnt, int m, int key, ref double[] real, ref double[] imag) {
            int i_p, point2, j, i, k, le, le1, ip;
            double t1, t2, u1, u2, u20, u10, f1, f2, arg;

            if (key == 1) for (i = 0; i < pnt; i++) imag[i] = -imag[i];
            point2 = pnt >> 1;
            for (j = 1, i = 1; i < pnt; i++) {
                if (i < j) {
                    t1 = real[j - 1];
                    t2 = imag[j - 1];
                    real[j - 1] = real[i - 1];
                    imag[j - 1] = imag[i - 1];
                    real[i - 1] = t1;
                    imag[i - 1] = t2;
                }
                k = point2;
                while (k < j) {
                    j -= k;
                    k >>= 1;
                }
                j += k;
            }
            for (i_p = 1, le = 2; i_p <= m; le <<= 1, i_p++) {
                le1 = le >> 1;
                u1 = 1;
                u2 = 0;
                arg = Math.PI / (double)le1;
                f1 = Math.Cos(arg);
                f2 = Math.Sin(arg);
                for (j = 1; j <= le1; j++) {
                    for (i = j; i <= pnt; i += le) {
                        ip = (int)(i + le1);
                        t1 = (real[ip - 1] * u1 - imag[ip - 1] * u2);
                        t2 = (real[ip - 1] * u2 + imag[ip - 1] * u1);
                        real[ip - 1] = real[i - 1] - t1;
                        imag[ip - 1] = imag[i - 1] - t2;
                        real[i - 1] += t1;
                        imag[i - 1] += t2;
                    }
                    u10 = u1 * f1 - u2 * f2;
                    u20 = u1 * f2 + u2 * f1;
                    u1 = u10;
                    u2 = u20;
                }
            }
        }
        
        public static float[] GetFrequencies(int nWindow, float sampleRate) {
            float[] f = new float[nWindow/2];
            float df = sampleRate/nWindow;
            for (int i = 0; i < f.Length; i++) {
                f[i] = df*(i + 1);
            }
            return f;
        }

        public static float[] GetFrequencies(int nWindow, float sampleRate, int startF, int endF) {
            float[] f = new float[endF-startF];
            float df = sampleRate / nWindow;
            for (int i = startF; i < endF; i++) {
                f[i-startF] = df * (i + 1);
            }
            return f;
        }

        public static double[] GetBlackmanNuttallWindow(int n) {
            double[] window = new double[n];
            for (int i = 0; i < n; i++) {
                window[i] = 0.3635819 - 0.4891775 * Math.Cos(2.0 * Math.PI * i / (double)n) + 0.1365995 * Math.Cos(4.0 * Math.PI * i / (double)n) - 0.0106411 * Math.Cos(6.0 * Math.PI * i / (double)n);
            }
            return window;
        }

    }
}
