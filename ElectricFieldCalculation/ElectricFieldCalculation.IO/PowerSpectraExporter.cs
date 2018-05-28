using ElectricFieldCalculation.Core.Data;
using System.Globalization;
using System.IO;

namespace ElectricFieldCalculation.IO {
    public static class PowerSpectraExporter {
        public static void Export(string fileName, PowerSpectra ex, PowerSpectra ey, PowerSpectra hx, PowerSpectra hy) {
            var ci = CultureInfo.InvariantCulture;
            using (var sw = new StreamWriter(fileName)) {
                sw.WriteLine(@"T;Avg(EX);Avg(EY);Avg(HX);Avg(HY);Power(EX);Power(EY);Power(HX);Power(HY)");
                for (int i = 0; i < ex.F.Length; i++) {
                    sw.WriteLine("{0};{1};{2};{3};{4};{5};{6};{7};{8}", (1.0/ex.F[i]).ToString(ci),
                        ex.Mag[i].ToString(ci), ey.Mag[i].ToString(ci), hx.Mag[i].ToString(ci), hy.Mag[i].ToString(ci),
                        ex.Mag2[i].ToString(ci), ey.Mag2[i].ToString(ci), hx.Mag2[i].ToString(ci), hy.Mag2[i].ToString(ci));
                }
            }
        }
    }
}
