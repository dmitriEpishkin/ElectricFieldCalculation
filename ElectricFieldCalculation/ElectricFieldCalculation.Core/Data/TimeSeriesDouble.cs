using System;
using System.Collections.Generic;

namespace SynteticData.TimeSeries {
    public class TimeSeriesDouble {

        private readonly float _sampleRate;
        private readonly DateTime _startTime;
        private readonly IList<double> _data;

        public TimeSeriesDouble(float sampleRate, DateTime startTime, IList<double> data) {
            _sampleRate = sampleRate;
            _startTime = startTime;
            _data = data;
        }

        public float SampleRate {
            get { return _sampleRate; }
        }

        public DateTime StartTime {
            get { return _startTime; }
        }

        public DateTime EndTime {
            get { return _startTime.AddSeconds(Data.Count/SampleRate); }
        }

        public IList<double> Data {
            get { return _data; }
        }
    }
}
