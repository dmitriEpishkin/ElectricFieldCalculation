
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using ElectricFieldCalculation.Core.Data;
using SynteticData.Import;
using SynteticData.TimeSeries;

namespace ElectricFieldCalculation.IO
{
    public class GicImporter : ITimeSeriesImporter
    {
        public Dictionary<ChannelInfo, TimeSeriesDouble> Import(string fileName) {

            var res = new Dictionary<ChannelInfo, TimeSeriesDouble>();

            var data = new List<Line>();

            using (var sr = new StreamReader(fileName)) {
                while (!sr.EndOfStream) {
                    var line = sr.ReadLine().Split(new[] {' ', '\t', ';', ','}, StringSplitOptions.RemoveEmptyEntries);
                    if (line.Length == 0)
                        continue;

                    if (line.Length != 7)
                        throw new Exception("неожиданное число столбцов в файле");

                    data.Add(new Line {
                        Time = new DateTime(int.Parse(line[0]), int.Parse(line[1]), int.Parse(line[2]), int.Parse(line[3]), int.Parse(line[4]), int.Parse(line[5])),
                        Val = double.Parse(line[6], CultureInfo.InvariantCulture)
                    });

                }
            }

            // проверка целостности данных
            var d = data[1].Time - data[0].Time;
            for (int i = 2; i < data.Count; i++)
                if (data[i].Time - data[i - 1].Time != d)
                    throw new Exception("частота дискретизации должна быть постоянной");

            // добавление в словарь
            res.Add(new ChannelInfo("DATA", FieldComponent.Gic), new TimeSeriesDouble(1.0f / (float) d.TotalSeconds, data[0].Time, data.ConvertAll(q => q.Val)));

            return res;
        }



        private class Line {
            public DateTime Time { get; set; }
            public double Val { get; set; }
        }

    }
}
