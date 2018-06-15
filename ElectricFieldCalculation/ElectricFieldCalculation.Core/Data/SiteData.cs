
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ElectricFieldCalculation.Core.Data;

namespace SynteticData.Data {

    public class SiteData : INotifyPropertyChanged {

        private string _name;

        private TensorCurve _z;

        private double _angleE;
        private double _angleH;

        public List<ComponentData> GetAllData() {
            return new List<ComponentData>(new [] {Ex, Ey, Hx, Hy, Dhx, Dhy, Gic}).FindAll(t => t.Ts != null);
        }

        public string Name {
            get { return _name; }
            set {
                if (_name != value) {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
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

        public ComponentData Gic { get; } = new ComponentData();
        public ComponentData Ex { get; } = new ComponentData();
        public ComponentData Ey { get; } = new ComponentData();
        public ComponentData Hx { get; } = new ComponentData();
        public ComponentData Hy { get; } = new ComponentData();
        public ComponentData Hz { get; } = new ComponentData();
        public ComponentData Dhx { get; } = new ComponentData();
        public ComponentData Dhy { get; } = new ComponentData();
        
        public double AngleE {
            get { return _angleE; }
            set {
                if (_angleE != value) {
                    _angleE = value;
                    OnPropertyChanged(nameof(AngleE));
                }
            }
        }

        public double AngleH {
            get { return _angleH; }
            set {
                if (_angleH != value) {
                    _angleH = value;
                    OnPropertyChanged(nameof(AngleH));
                }
            }
        }

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
