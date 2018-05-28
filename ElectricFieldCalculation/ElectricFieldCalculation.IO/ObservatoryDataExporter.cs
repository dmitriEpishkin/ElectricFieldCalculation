using ElectricFieldCalculation.Core.IO;
using SynteticData.TimeSeries;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using ElectricFieldCalculation.Core.Data;

namespace ElectricFieldCalculation.IO {
    public class ObservatoryDataExporter : ITimeSeriesExporter {
        public void Export(string fileName, Dictionary<ChannelInfo, TimeSeriesDouble> data) {
            if (data.Count != 4)
                throw new ArgumentException();

            var d = data.Select(x => x.Value).ToArray();

            var dt = Math.Round(1.0 / d[0].SampleRate);

            using (var sw = new StreamWriter(fileName)) {
                sw.WriteLine("HH MM SS      EX      EY     HX     HY");
                sw.WriteLine("---------------------------------------");
                var c = d[0].Data.Count;
                var time = new DateTime(2000, 1, 1, 0, 0, 0);
                for (int i = 0; i < c; i++) {
                    var line = " " + time.Hour.ToString(@"00");
                    line += " " + time.Minute.ToString(@"00");
                    line += " " + time.Second.ToString(@"00") + "      ";

                    for (int j = 0; j < 4; j++) {
                        line += d[j].Data[i].ToString(CultureInfo.InvariantCulture) + "  ";
                    }
                    sw.WriteLine(line);

                    time = time.AddSeconds(dt);
                }
            }
        }
    }
}
