
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
    }
}
