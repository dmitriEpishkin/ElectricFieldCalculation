using System.Collections.Generic;
using ElectricFieldCalculation.Core.Data;
using SynteticData.TimeSeries;

namespace ElectricFieldCalculation.Core.IO {
    public interface ITimeSeriesExporter {
        void Export(string fileName, Dictionary<ChannelInfo, TimeSeriesDouble> data);
    }
}
