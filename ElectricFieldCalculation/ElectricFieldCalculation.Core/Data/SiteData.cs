
using SynteticData.TimeSeries;
using System.ComponentModel;

namespace SynteticData.Data {
    public class SiteData : INotifyPropertyChanged {

        private string _name;

        private TimeSeriesDouble _gic;

        private TimeSeriesDouble _ex;
        private TimeSeriesDouble _ey;
        private TimeSeriesDouble _hx;
        private TimeSeriesDouble _hy;
        private TimeSeriesDouble _hz;

        private TimeSeriesDouble _dHx;
        private TimeSeriesDouble _dHy;

        private TensorCurve _z;

        public string Name {
            get { return _name; }
            set {
                if (_name != value) {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public TimeSeriesDouble Gic {
            get { return _gic; }
            set {
                if (_gic != value)
                {
                    _gic = value;
                    OnPropertyChanged(nameof(Gic));
                }
            }
        }

        public TimeSeriesDouble Ex {
            get { return _ex; }
            set {
                if (_ex != value) {
                    _ex = value;
                    OnPropertyChanged(nameof(Ex));
                }
            }
        }

        public TimeSeriesDouble Ey {
            get { return _ey; }
            set {
                if (_ey != value) {
                    _ey = value;
                    OnPropertyChanged(nameof(Ey));
                }
            }
        }

        public TimeSeriesDouble Hx {
            get { return _hx; }
            set {
                if (_hx != value) {
                    _hx = value;
                    OnPropertyChanged(nameof(Hx));
                }
            }
        }

        public TimeSeriesDouble Hy {
            get { return _hy; }
            set {
                if (_hy != value) {
                    _hy = value;
                    OnPropertyChanged(nameof(Hy));
                }
            }
        }

        public TimeSeriesDouble Hz {
            get { return _hz; }
            set {
                if (_hz != value)
                {
                    _hz = value;
                    OnPropertyChanged(nameof(Hz));
                }
            }
        }

        public TimeSeriesDouble Dhx {
            get { return _dHx; }
            set {
                if (_dHx != value)
                {
                    _dHx = value;
                    OnPropertyChanged(nameof(Dhx));
                }
            }
        }

        public TimeSeriesDouble Dhy {
            get { return _dHy; }
            set {
                if (_dHy != value)
                {
                    _dHy = value;
                    OnPropertyChanged(nameof(Dhy));
                }
            }
        }

        public TensorCurve Z {
            get { return _z; }
            set {
                if (_z != value) {
                    _z = value;
                    OnPropertyChanged(nameof(Z));
                }
            }
        }

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
