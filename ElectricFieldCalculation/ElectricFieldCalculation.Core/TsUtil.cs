
using System;
using SynteticData.TimeSeries;

namespace ElectricFieldCalculation.Core
{
    public static class TsUtil {
        public static TimeSeriesDouble GetDerivate(TimeSeriesDouble ts) {
            var d = new double[ts.Data.Count-1];
            for (int i = 0; i < d.Length; i++)
                d[i] = (ts.Data[i + 1] - ts.Data[i]) * ts.SampleRate;
            return new TimeSeriesDouble(ts.SampleRate, ts.StartTime, d);
        }

        public static Tuple<TimeSeriesDouble, TimeSeriesDouble> Rotate(TimeSeriesDouble x, TimeSeriesDouble y, double radAngle) {
            var xNew = new double[x.Data.Count];
            var yNew = new double[y.Data.Count];

            var cos = Math.Cos(radAngle);
            var sin = Math.Sin(radAngle);

            for (int i = 0; i < x.Data.Count; i++) {
                xNew[i] = x.Data[i] * cos + y.Data[i] * sin;
                yNew[i] = -x.Data[i] * sin + y.Data[i] * cos;
            }

            return new Tuple<TimeSeriesDouble, TimeSeriesDouble>(new TimeSeriesDouble(x.SampleRate, x.StartTime, xNew), new TimeSeriesDouble(y.SampleRate, y.StartTime, yNew));
        }
    }
}
