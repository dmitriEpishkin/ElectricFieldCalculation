
using SynteticData.TimeSeries;
using System.Collections.Generic;
using ElectricFieldCalculation.Core.Data;

namespace SynteticData.Import {
    public interface ITimeSeriesImporter {
        Dictionary<ChannelInfo, TimeSeriesDouble> Import(string fileName);
    }
}
