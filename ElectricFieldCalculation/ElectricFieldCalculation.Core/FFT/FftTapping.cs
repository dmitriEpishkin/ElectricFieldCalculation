
using System;
using System.Collections.Generic;
using System.Numerics;

namespace SynteticData.FFT {
    public static class FftTapping {

        public static IEnumerable<Complex[]> Forward(IList<double> data, int window, int step, Action<double> progress) {

            var total = 0;
            for (int i = 0; i < data.Count - window + 1; i += step)
                total++;

            var count = 0;
            for (int i = 0; i < data.Count - window + 1; i += step) {
                var w = new double[window];
                for (int j = 0; j < window; j++)
                    w[j] = data[i + j];

                var cur = Fft.Forward(w);

                count++;
                progress(100.0 * count / total);

                yield return PackSpectra(cur, window);
            }
        }

        public static void BackOneWindow(double[] data, double[] mask, Complex[] sp, int window, int position) {
            Fill(data, mask, Fft.Back(UnpackSpectra(sp, window)), position);
        }

        public static void NormalizeAfterBackLoop(double[] data, double[] mask) {
            for (int i = 0; i < data.Length; i++)
                data[i] /= mask[i] == 0 ? 1 : mask[i];
        }
        
        private static void Fill(double[] all, double[] mask, double[] w, int position) {
            for (int i = 0; i < w.Length; i++) {
                all[position + i] += w[i];
                mask[position + i] ++;
            }
        }

        private static Complex[] PackSpectra(Complex[] sp, int window) {
            var cut = new Complex[window / 2];
            Array.Copy(sp, 1, cut, 1, window / 2 - 1);
            cut[0] = new Complex(sp[0].Real, sp[window / 2].Real);
            for (int i = 1; i < window/2; i++)
                cut[i] = (sp[i] + Complex.Conjugate(sp[window - i]))/2;

            return cut;
        }

        private static Complex[] UnpackSpectra(Complex[] sp, int window) {
            var res = new Complex[window];

            res[0] = sp[0].Real;

            for (int i = 1; i < window/2; i++) {
                res[i] = sp[i];
                res[window - i] = Complex.Conjugate(sp[i]);
            }

            res[window/2] = sp[0].Imaginary;

            return res;
        }

    }
}
