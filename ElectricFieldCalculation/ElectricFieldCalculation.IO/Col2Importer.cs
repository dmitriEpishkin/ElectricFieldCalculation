
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using ElectricFieldCalculation.Core.Data;
using SynteticData.Import;
using SynteticData.TimeSeries;

namespace ElectricFieldCalculation.IO
{
    public class Col2Importer : ITimeSeriesImporter
    {
        public Dictionary<ChannelInfo, TimeSeriesDouble> Import(string fileName) {
            var res = new Dictionary<ChannelInfo, TimeSeriesDouble>();

            var data = new List<Line>();
            var header = new List<ChannelInfo>();

            var ci = CultureInfo.InvariantCulture;

            using (var sr = new StreamReader(fileName)) {

                // заголовок
                header = PrepareHeader(sr.ReadLine().Split(new [] {' ', '\t' }, StringSplitOptions.RemoveEmptyEntries));

                // пустая строка
                sr.ReadLine();

                while (!sr.EndOfStream) {
                    var line = sr.ReadLine().Split(new[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);

                    if (line.Length != header.Count + 6)
                        throw new Exception("Колличество стобцов данных не согласуется с заголовком");

                    var l = new Line {
                        Time = new DateTime(int.Parse(line[0], ci), int.Parse(line[1], ci), int.Parse(line[2], ci), int.Parse(line[3], ci), int.Parse(line[4], ci), int.Parse(line[5], ci))
                    };

                    for (int i = 6; i < line.Length; i++)
                        l.Data.Add(double.Parse(line[i], ci));

                    data.Add(l);
                }
            }

            CheckData(data);

            var startTime = data[0].Time;
            var sampleRate = 1.0 / (data[1].Time - data[0].Time).TotalSeconds;

            var lists = new Dictionary<ChannelInfo, List<double>>();

            foreach (ChannelInfo t in header)
                lists.Add(t, new List<double>());

            foreach (var d in data) {
                for (int i = 0; i < header.Count; i++)
                    lists[header[i]].Add(d.Data[i]);
            }

            foreach (var l in lists)
                res.Add(l.Key, new TimeSeriesDouble((float)sampleRate, startTime, l.Value));

            return res;
        }

        private List<ChannelInfo> PrepareHeader(string[] header) {

            var list = new List<ChannelInfo>();

            if (header[0] != "YYYY" || header[1] != "MM" || header[2] != "DD" || header[3] != "HH" || header[4] != "MM" || header[5] != "SS")
                throw new Exception("Первые 6 столбцов должны называться как \"YYYY MM DD HH MM SS\"");
            
            var start = 6;
            for (int i = 6; i < header.Length; i++) {
                if (header[i] == "X" || header[i] == "Y" || header[i] == "Z" || header[i] == "EX" || header[i] == "EY" || header[i] == "GIC") {
                    var str = header[start];
                    for (int j = start + 1; j < i; j++)
                        str += " " + header[i];
                    list.Add(ToChannelInfo(str, header[i]));
                    start = i + 1;
                }
            }

            return list;
        }

        private void CheckData(List<Line> lines) {
            var d = lines[1].Time - lines[0].Time;
            for (int i = 2; i < lines.Count; i++)
                if (lines[2].Time - lines[1].Time != d)
                    throw new Exception("Частота дискретизации должна быть постоянной");
        }

        private ChannelInfo ToChannelInfo(string name, string cmp) {
            FieldComponent c;
            if (cmp == "X")
                c = FieldComponent.Hx;
            else if (cmp == "Y")
                c = FieldComponent.Hy;
            else if (cmp == "Z")
                c = FieldComponent.Hz;
            else if (cmp == "EX")
                c = FieldComponent.Ex;
            else if (cmp == "EY")
                c = FieldComponent.Ey;
            else if (cmp == "GIC")
                c = FieldComponent.Gic;
            else throw new Exception("неизвестное обозначение: " + cmp);

            return new ChannelInfo(name, c);
        }

        private class Line {
            public DateTime Time { get; set; }
            public List<double> Data { get; } = new List<double>();
        }
    }
}
