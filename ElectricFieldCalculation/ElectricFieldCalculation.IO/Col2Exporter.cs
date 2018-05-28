
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using ElectricFieldCalculation.Core.Data;
using ElectricFieldCalculation.Core.IO;
using SynteticData.TimeSeries;

namespace ElectricFieldCalculation.IO
{
    public class Col2Exporter : ITimeSeriesExporter
    {
        public void Export(string fileName, Dictionary<ChannelInfo, TimeSeriesDouble> data) {

            var count = data.First().Value.Data.Count;

            using (var sw = new StreamWriter(fileName)) {

                WriteHeader(sw, data);

                for (int i = 0; i < count; i++) 
                    WriteLine(sw, data, i);

            }
        }

        private void WriteHeader(StreamWriter sw, Dictionary<ChannelInfo, TimeSeriesDouble> data) {

            StringBuilder str = new StringBuilder(@"YYYY MM DD HH MM SS   ");
            
            foreach (var k in data.Keys) {
                str.Append($"  {k.Name} ");
                str.Append($"{CmpToStr(k.Component)} ");
            }
            
            sw.WriteLine(str.ToString());
            sw.WriteLine(new string('-', str.Length));
        }

        private string CmpToStr(FieldComponent cmp) {
            switch (cmp) {
                case FieldComponent.Unckown:
                    throw new ArgumentOutOfRangeException();
                case FieldComponent.Gic:
                    throw new ArgumentOutOfRangeException();
                case FieldComponent.Ex:
                    return "EX";
                case FieldComponent.Ey:
                    return "EY";
                case FieldComponent.Hx:
                    return "X";
                case FieldComponent.Hy:
                    return "Y";
                case FieldComponent.Hz:
                    return "Z";
                case FieldComponent.Dx:
                    throw new ArgumentOutOfRangeException();
                case FieldComponent.Dy:
                    throw new ArgumentOutOfRangeException();
                default:
                    throw new ArgumentOutOfRangeException(nameof(cmp), cmp, null);
            }
        }

        private void WriteLine(StreamWriter sw, Dictionary<ChannelInfo, TimeSeriesDouble> data, int i) {

            var ts = data.First().Value;

            var time = ts.StartTime + TimeSpan.FromSeconds(i / ts.SampleRate);

            StringBuilder str = new StringBuilder($"{time.Year:0000} {time.Month:00} {time.Day:00} {time.Hour:00} {time.Minute:00} {time.Second:00}   ");
            foreach (var k in data.Values) {
                str.Append($"  {k.Data[i].ToString(CultureInfo.InvariantCulture)} ");
            }

            sw.WriteLine(str.ToString());
        }

    }
}
