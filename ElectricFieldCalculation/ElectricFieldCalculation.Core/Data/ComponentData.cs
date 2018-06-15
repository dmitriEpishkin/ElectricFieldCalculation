using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using SynteticData.TimeSeries;

namespace ElectricFieldCalculation.Core.Data {
    public class ComponentData : Model
    {
        private TimeSeriesDouble _ts;
        private PowerSpectra _spectra;

        public TimeSeriesDouble Ts {
            get { return _ts; }
            set {
                if (_ts != value) {
                    _ts = value;
                    RaisePropertyChanged(nameof(Ts));
                }
            }
        }

        public PowerSpectra Spectra
        {
            get { return _spectra; }
            set
            {
                if (_spectra != value)
                {
                    _spectra = value;
                    RaisePropertyChanged(nameof(Spectra));
                }
            }
        }

    }
}
