using System;
using System.IO;
using System.Collections.Generic;
using System.Numerics;
using System.Globalization;
using SynteticData.Data;
using ElectricFieldCalculation.Core.IO;
using System.Text;

namespace ElectricFieldCalculation.IO {
    public class FtfTensorCurveImporter : ITensorImporter {

        public TensorCurve Import(string fileName) {

            var f = new List<double>();
            var t = new List<Complex[,]>();

            int counter = 0;

            using (var sr = new StreamReader(fileName)) {
                try {
                    bool reading = false;
                    while (!sr.EndOfStream) {
                        var line = sr.ReadLine().Trim();
                        counter++;

                        var text = line.StartsWith(@"*") || line.StartsWith(@"'");
                        if (text && reading)
                            break;
                        if (text)
                            continue;

                        reading = true;

                        var n1 = ReadNumbers(line, 5);
                        

                        var line2 = sr.ReadLine().Trim();
                        counter++;
                        var n2 = ReadNumbers(line2, 4);


                        var line3 = sr.ReadLine().Trim();
                        counter++;
                        var n3 = ReadNumbers(line3, 8);

                        var zxx = Complex.FromPolarCoordinates(n1[1], DegToRad(n1[2]));
                        var zxy = Complex.FromPolarCoordinates(n1[3], DegToRad(n1[4]));
                        var zyx = Complex.FromPolarCoordinates(n2[0], DegToRad(n2[1]));
                        var zyy = Complex.FromPolarCoordinates(n2[2], DegToRad(n2[3]));

                        f.Add(1.0 / n1[0]);

                        var tt = new Complex[2, 2];
                        tt[0, 0] = zxx;
                        tt[0, 1] = zxy;
                        tt[1, 0] = zyx;
                        tt[1, 1] = zyy;

                        t.Add(tt);
                    }

                }
                catch (Exception e) {
                    throw new FormatException(e.Message + " Строка " + counter, e.InnerException);
                }
            }

            return new TensorCurve(fileName, f.ToArray(), t.ToArray());
        }

        private double DegToRad(double deg) {
            return deg / 180.0 * Math.PI;
        }

        private double[] ReadNumbers(string line, int n) {
            line = line.Replace('/', ' ');
            var s1 = line.Split(new[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
            double[] n1;
            try {
                n1 = Array.ConvertAll(s1, x => double.Parse(x, CultureInfo.InvariantCulture));
            }
            catch (FormatException e) {
                throw new FormatException(@"Не удалось распознать число.");
            }

            if (n1.Length != n)
                throw new FormatException(@"Неожиданное количество чисел в строке.");

            return n1;
        }
    }
}
