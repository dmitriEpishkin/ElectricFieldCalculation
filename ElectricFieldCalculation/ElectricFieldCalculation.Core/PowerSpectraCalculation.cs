using ElectricFieldCalculation.Core.Data;
using SynteticData.FFT;
using SynteticData.TimeSeries;
using System;
using System.Collections.Generic;
using SynteticData.Data;

namespace ElectricFieldCalculation.Core {
    public static class PowerSpectraCalculation {
        
        public static PowerSpectra Run(TimeSeriesDouble data, int window, Action<double> progress) {

            while (3 * window > data.Data.Count)
                window /= 2;

            var startF = 5;
            var endF = window / 2 - 5;

            var f = Fft.GetFrequencies(window, data.SampleRate, startF, endF);

            var mag = Calculate(data.Data, window, window / 4, startF, endF, progress);

            //var c = 1 / Math.Sqrt(data.SampleRate);
            //mag = Array.ConvertAll(mag, x => x * c);

            progress(100.0);

            return new PowerSpectra(f, mag.Item1, mag.Item2);
        }

        private static Tuple<double[],double[]> Calculate(IList<double> data, int window, int step, int startF, int endF, Action<double> progress) {
            var res = new double[endF - startF];
            var res2 = new double[endF - startF];
            int count = 0;

            var n = 1 + (data.Count - window) / step;

            for (int i = 0; i < data.Count - window; i += step) {
                var w = new double[window];
                for (int j = 0; j < window; j++)
                    w[j] = data[i + j];

                var cur = Fft.Forward(w);

                for (int q = startF; q < endF; q++) {
                    var mag = cur[q].Magnitude;
                    res[q - startF] += mag;
                    res2[q - startF] += mag * mag;
                }

                count++;
                progress((double) count / n * 100.0);
            }

            for (int i = 0; i < res.Length; i++) {
                res[i] /= count;
                res2[i] /= count;
            }

            return new Tuple<double[], double[]>(res, res2);
        }

    }
}
