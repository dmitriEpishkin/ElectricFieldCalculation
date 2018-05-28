using SynteticData.Import;
using SynteticData.TimeSeries;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using ElectricFieldCalculation.Core.Data;

namespace ElectricFieldCalculation.IO {
    public class RealDataImporter : ITimeSeriesImporter {
        public Dictionary<ChannelInfo, TimeSeriesDouble> Import(string fileName) {

            var sampleRate = 1.0;

            var res = new Dictionary<ChannelInfo, List<double>>();
            
            var counter = 0;
            try {
                using (var sr = new StreamReader(fileName)) {
                    var cmps = sr.ReadLine().Split(new[] {" ", "\t" }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 3; i < cmps.Length; i++) {
                        var c = cmps[i].ToUpperInvariant();
                        if (c != "EX" && c != "EY" && c != "HX" && c != "HY" && c != "X" && c != "Y" && c != "Z")
                            continue;
                        if (c == "X")
                            c = "HX";
                        if (c == "Y")
                            c = "HY";
                        if (c == "Z")
                            c = "HZ";

                        res.Add(new ChannelInfo("DATA", ToCmp(c)), new List<double>());
                    }

                    var keys = res.Keys.ToArray();

                    sr.ReadLine();
                    var l1 = sr.ReadLine();
                    counter += 3;
                    var n1 = ReadNumbers(l1);
                    AddNumbersToDictionary(n1, keys, res);

                    var l2 = sr.ReadLine();
                    counter++;
                    var n2 = ReadNumbers(l2);
                    AddNumbersToDictionary(n2, keys, res);

                    sampleRate = 1.0 / (3600 * (n2[0] - n1[0]) + 60 * (n2[1] - n1[1]) + (n2[2] - n1[2]));

                    while (!sr.EndOfStream) {
                        var l = sr.ReadLine();
                        counter++;
                        var n = ReadNumbers(l);
                        AddNumbersToDictionary(n, keys, res);
                    }

                }
            }
            catch (Exception e) {
                throw new FormatException(e.Message + @" Строка " + counter, e.InnerException);
            }
            
            var data = new Dictionary<ChannelInfo, TimeSeriesDouble>();
            foreach (var d in res) {
                if (d.Key.Component == FieldComponent.Hx || d.Key.Component == FieldComponent.Hy) {
                    // залатать дыры
                    while (d.Value[0] > 99999)
                        d.Value.RemoveAt(0);

                    int startBad = 0;
                    for (int i = 1; i < d.Value.Count; i++) {
                        if (startBad == 0 && d.Value[i] > 99999)
                            startBad = i;
                        if (startBad > 0) {
                            if (i == d.Value.Count - 1) {
                                d.Value.RemoveRange(startBad, d.Value.Count - startBad);
                                break;
                            }
                            if (d.Value[i] <= 99999) {
                                Repair(d.Value, startBad, i);
                                startBad = 0;
                            }
                        }

                    }
                }
                data.Add(d.Key, new TimeSeriesDouble((float) sampleRate, new DateTime(), d.Value.ToArray()));
            }
            return data;
        }

        private FieldComponent ToCmp(string str) {

            str = str.Trim().ToUpperInvariant();

            if (str == "EX")
                return FieldComponent.Ex;
            if(str =="EY")
                return FieldComponent.Ey;
            if (str == "HX")
                return FieldComponent.Hx;
            if (str == "HY")
                return FieldComponent.Hy;
            if (str == "HZ")
                return FieldComponent.Hz;

            throw new ArgumentOutOfRangeException("неизвестная компонента поля");
        }

        private void Repair(List<double> d, int startBad, int o) {

            var x1 = startBad - 1;
            var xn = o;
            var y1 = d[x1];
            var yn = d[xn];
            
            for (int i = startBad; i < o; i++) {
                var k = (i - x1) / (double)(xn - x1);
                d[i] = (1 - k) * y1 + k * yn;
            }

        }

        private double[] ReadNumbers(string line) {
            line.Replace('/', ' ');
            var s1 = line.Split(new[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
            double[] n1;
            try {
                n1 = Array.ConvertAll(s1, x => double.Parse(x, CultureInfo.InvariantCulture));
            }
            catch (FormatException e) {
                throw new FormatException(@"Не удалось распознать число.");
            }
            
            return n1;
        }

        private void AddNumbersToDictionary(double[] numbers, ChannelInfo[] keys, Dictionary<ChannelInfo, List<double>> dic) {

            for (int i = 3; i < numbers.Length; i++)
                dic[keys[i - 3]].Add(numbers[i]);
        }

    }
}
