using System.Collections.Generic;
using ElectricFieldCalculation.Core.Data;
using System.Globalization;
using System.IO;
using ElectricFieldCalculation.Core;

namespace ElectricFieldCalculation.IO {
    public static class PowerSpectraExporter {
        public static void Export(string fileName, Dictionary<ChannelInfo, PowerSpectra> repository) {
            var ci = CultureInfo.InvariantCulture;
            using (var sw = new StreamWriter(fileName)) {
                foreach (var sp in repository) {
                    sw.WriteLine(sp.Key.Name + " " + sp.Key.Component + ";");
                    sw.WriteLine(@"T, s;Mag2");
                    for (int i = 0; i < sp.Value.F.Length; i++) {
                        sw.WriteLine(sp.Value.F[i] + ";" + sp.Value.Mag2[i]);
                    }
                    sw.WriteLine();
                }
            }
        }
    }
}
