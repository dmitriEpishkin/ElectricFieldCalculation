
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Windows;
using ElectricFieldCalculation.Core;
using ElectricFieldCalculation.Core.Data;
using SynteticData.Data;
using SynteticData.FFT;
using SynteticData.TimeSeries;
using SynteticData.Import;
using ElectricFieldCalculation.Core.IO;

namespace SynteticData {
    public class SynteticDataGenerator {

        private readonly ITimeSeriesImporter _tsImporter;
        private readonly ITensorImporter _tensorImporter;
        private readonly ITimeSeriesExporter _tsExporter;

        public SynteticDataGenerator(ITimeSeriesImporter tsImporter, ITensorImporter tensorImporter, ITimeSeriesExporter tsExporter) {
            _tsImporter = tsImporter;
            _tensorImporter = tensorImporter;
            _tsExporter = tsExporter;
        }
        
        public SiteData Generate(string magneticFieldPath, string curvePath, string outputPath, int window, int step, bool acFilter) {

            Console.WriteLine(@"Загрузка импеданса из файла");
            var tensorCurve = _tensorImporter.Import(curvePath);

            Console.WriteLine(@"Загрузка временных рядов Hx, Hy из файла");
            var ts = _tsImporter.Import(magneticFieldPath);
            var baseTs = Cut(new[] { ts.First(t => t.Key.Component == FieldComponent.Hx).Value, ts.First(t => t.Key.Component == FieldComponent.Hy).Value }, window, step);
            var sampleRate = baseTs[0].SampleRate;

            Console.WriteLine(@"Интерполяция импедансов на рабочую сетку частот");
            tensorCurve.Initalize(window, sampleRate);

            Console.WriteLine(@"Расчёт электрического поля");
            var res = Generate(baseTs, tensorCurve, window, step, acFilter, p => {});

            Console.WriteLine(@"Сохранение рассчитанных временных рядов в файл");
            _tsExporter.Export(outputPath,
                new Dictionary<ChannelInfo, TimeSeriesDouble> {
                    {new ChannelInfo("DATA", FieldComponent.Ex), res[0]},
                    {new ChannelInfo("DATA", FieldComponent.Ey), res[1]},
                    {new ChannelInfo("DATA", FieldComponent.Hx), baseTs[0]},
                    {new ChannelInfo("DATA", FieldComponent.Hy), baseTs[1]}
                });
            
            return new SiteData { Ex = res[0], Ey = res[1], Hx = baseTs[0], Hy = baseTs[1] };
        }

        public TimeSeriesDouble[] Generate(SiteData data, int window, int step, bool acFilter, Action<double> progress) {

            progress(0);

            var sampleRate = data.Hx.SampleRate;
            var tensorCurve = data.Z;
            var baseTs = new[] {data.Hx, data.Hy};

            while (window > data.Hx.Data.Count / 2)
                window /= 2;

            tensorCurve.Initalize(window, sampleRate);

            progress(10);

            return Generate(baseTs, tensorCurve, window, step, acFilter, p => progress(10 + p * 0.9));
        }

        private TimeSeriesDouble[][] Generate(TimeSeriesDouble[][] baseTs, TensorCurve tensorCurve, int window, int step, bool acFilter) {
            var res = new TimeSeriesDouble[tensorCurve.OutputsNumber][];
            res[0] = new TimeSeriesDouble[baseTs[0].Length];
            res[1] = new TimeSeriesDouble[baseTs[0].Length];

            for (int i = 0; i < baseTs[0].Length; i++) {
                var o = Generate(new[] {baseTs[0][i], baseTs[1][i]}, tensorCurve, window, step, acFilter, p => { });
                res[0][i] = o[0];
                res[1][i] = o[1];
            }
            return res;
        }

        private TimeSeriesDouble[] Generate(TimeSeriesDouble[] baseTs, TensorCurve tensorCurve, int window, int step, bool acFilter, Action<double> progress) {
            
            if (tensorCurve.InputsNumber != baseTs.Length)
                throw new ArgumentException();
            
            var sampleRate = baseTs[0].SampleRate;
            
            var sp = new IEnumerable<Complex[]>[baseTs.Length];
            for (int i = 0; i < baseTs.Length; i++)  
                sp[i] = FftTapping.Forward(baseTs[i].Data, window, step, progress);

            var spZip = sp[0].Zip(sp[1], (x, y) => new[] {x, y});

            var d1 = new double[baseTs[0].Data.Count];
            var m1 = new double[baseTs[0].Data.Count];
            var d2 = new double[baseTs[1].Data.Count];
            var m2 = new double[baseTs[1].Data.Count];
            
            int counter = 0;
            foreach (var s in spZip) {
                var cSp = tensorCurve.Apply(s, window, sampleRate);
                FftTapping.BackOneWindow(d1, m1, cSp[0], window, step*counter);
                FftTapping.BackOneWindow(d2, m2, cSp[1], window, step * counter);
                counter++;
            }
            FftTapping.NormalizeAfterBackLoop(d1, m1);
            FftTapping.NormalizeAfterBackLoop(d2, m2);

            // удаление пограничных значений
            if (d1.Length > 40) {
                var avg01 = Statistic.Average(d1, 20, 30);
                var avg02 = Statistic.Average(d2, 20, 30);
                var avgN1 = Statistic.Average(d1, d1.Length - 30, d1.Length - 20);
                var avgN2 = Statistic.Average(d2, d2.Length - 30, d2.Length - 20);

                for (int i = 0; i < 20; i++) {
                    d1[i] = avg01;
                    d2[i] = avg02;
                    d1[d1.Length - i - 1] = avgN1;
                    d2[d2.Length - i - 1] = avgN2;
                }

            }

            if (acFilter) {
                RemoveAc(d1, window);
                RemoveAc(d2, window);
            }
            
            RemoveDc(d1);
            RemoveDc(d2);
                
            var res = new TimeSeriesDouble[tensorCurve.OutputsNumber];

            res[0] = new TimeSeriesDouble(sampleRate, baseTs[0].StartTime, d1);
            res[1] = new TimeSeriesDouble(sampleRate, baseTs[0].StartTime, d2);

            progress(100.0);

            return res;
        }

        private TimeSeriesDouble[] Cut(TimeSeriesDouble[] ts, int window, int step) {
            var res = new TimeSeriesDouble[ts.Length];
            for (int i = 0; i < ts.Length; i++) {
                var length = ts[i].Data.Count;
                var realLength = length - (length - window) % step;
                var newAr = new double[realLength];
                for (int e = 0; e < realLength; e++)
                    newAr[e] = ts[i].Data[e];

                res[i] = new TimeSeriesDouble(ts[i].SampleRate, ts[i].StartTime, newAr);

            }
            return res;
        }

        private void RemoveDc(double[] data) {
            var avg = data.Average();
            for (int i = 0; i < data.Length; i++)
                data[i] -= avg;
        }

        private void RemoveAc(double[] data, int window) {

            var win2 = window / 2;

            var dataCopy = new double[data.Length];
            data.CopyTo(dataCopy, 0);

            for (int i = 0; i < data.Length; i++) {
                data[i] -= Statistic.Average(dataCopy, i - win2, i + win2);
            }

        }


    }
}
