using System.Collections.Generic;
using ElectricFieldCalculation.Core.IO;
using SynteticData.TimeSeries;
using System.Globalization;
using System.IO;
using System.Linq;
using ElectricFieldCalculation.Core.Data;

namespace ElectricFieldCalculation.IO {
    public class SimpleTsExporter : ITimeSeriesExporter {
        public void Export(string fileName, Dictionary<ChannelInfo, TimeSeriesDouble> data) {
            var d = data.Select(x => x.Value).ToArray();
            using (var sw = new StreamWriter(fileName)) {
                for (int i = 0; i < d[0].Data.Count; i++) {
                    var line = "";
                    for (int j = 0; j < d.Length; j++) {
                        line += d[j].Data[i].ToString(CultureInfo.InvariantCulture) + (j == d.Length - 1 ? "" : ";");
                    }
                    sw.WriteLine(line);
                }
            }
        }
    }
}
