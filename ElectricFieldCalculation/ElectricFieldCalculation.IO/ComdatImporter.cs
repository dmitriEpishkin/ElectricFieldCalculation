using SynteticData.Import;
using System;
using System.Collections.Generic;
using SynteticData.TimeSeries;
using System.IO;
using System.Globalization;
using ElectricFieldCalculation.Core.Data;

namespace ElectricFieldCalculation.IO {
    public class ComdatImporter : ITimeSeriesImporter {

        private bool _infoIsSet;

        private int _sampleNumber;
        private double _sampleInterval;

        private Dictionary<string, TimeSeriesDouble> _data;

        private string _curCmp;
        private double _curAmpFactor;
        private double[] _curData;
        private int _curInd;

        public void ConvertToCsv(string fileName, string newFileName) {
            ReadAll(fileName);

            var res = new Dictionary<ChannelInfo, TimeSeriesDouble> {
                {new ChannelInfo("DATA", FieldComponent.Ex), _data[@"'#EX'"]},
                { new ChannelInfo("DATA", FieldComponent.Ey), _data[@"'#EY'"]},
                { new ChannelInfo("DATA", FieldComponent.Hx), _data[@"'#HX'"]},
                { new ChannelInfo("DATA", FieldComponent.Hy), _data[@"'#HY'"]}
            };

            _data = new Dictionary<string, TimeSeriesDouble>();

            new SimpleTsExporter().Export(newFileName, res);
        }

        public Dictionary<ChannelInfo, TimeSeriesDouble> Import(string fileName) {

            ReadAll(fileName);

            var res = new Dictionary<ChannelInfo, TimeSeriesDouble>();
            foreach (var r in _data)
                res.Add(new ChannelInfo("DATA", ToCmp(r.Key.Trim('\'', '#').ToUpperInvariant())), r.Value);

            return res;
        }

        private void ReadAll(string fileName) {
            _data = new Dictionary<string, TimeSeriesDouble>();

            using (var sr = new StreamReader(fileName)) {
                while (!sr.EndOfStream) {
                    var line = sr.ReadLine();
                    ProcessLine(line);
                }
                AddToDictionary();
            }
        }

        private void ProcessLine(string line) {
            if (line.StartsWith(@"*"))
                return;
            if (!_infoIsSet && line.Contains(@"'#DT'")) {
                var split = line.Split(new [] { " ", "  " }, StringSplitOptions.RemoveEmptyEntries);
                _sampleNumber = int.Parse(split[2], CultureInfo.InvariantCulture);
                _sampleInterval = double.Parse(split[4], CultureInfo.InvariantCulture);
                _infoIsSet = true;
            }
            else if (_infoIsSet && line.Contains(@"'#FLD'")) {
                AddToDictionary();
                var split = line.Split(new[] { " ", "  " }, StringSplitOptions.RemoveEmptyEntries);
                _curCmp = split[1];
                _curInd = 0;
                _curAmpFactor = double.Parse(split[2], CultureInfo.InvariantCulture);
                _curData = new double[_sampleNumber];
            }
            else if (_curCmp != null && !string.IsNullOrWhiteSpace(line)) {
                var split = line.Split(new[] { " ", "  ", "/" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < split.Length; i++) {
                    _curData[_curInd++] = double.Parse(split[i], CultureInfo.InvariantCulture) * _curAmpFactor;
                }
            }

        }

        private void AddToDictionary() {
            if (_curCmp == null)
                return;
            if (_curInd != _sampleNumber)
                throw new Exception("прочитаны не все значения ряда");

            _data.Add(_curCmp, new TimeSeriesDouble((float)(1.0 / _sampleInterval), new DateTime(), _curData));
        }
        
        private FieldComponent ToCmp(string str)
        {
            str = str.Trim().ToUpperInvariant();

            if (str == "EX")
                return FieldComponent.Ex;
            if (str == "EY")
                return FieldComponent.Ey;
            if (str == "HX")
                return FieldComponent.Hx;
            if (str == "HY")
                return FieldComponent.Hy;
            if (str == "HZ")
                return FieldComponent.Hz;

            throw new ArgumentOutOfRangeException("неизвестная компонента поля");
        }

    }
}
