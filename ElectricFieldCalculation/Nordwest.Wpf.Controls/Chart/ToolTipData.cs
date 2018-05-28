using System.ComponentModel;

namespace Nordwest.Wpf.Controls.Chart {
    public class ToolTipData : INotifyPropertyChanged {
        private readonly string _name;

        private bool _isEnabled;
        private string _text;

        public ToolTipData(string name, string text, bool isEnabled) {
            _name = name;
            _text = text;
            IsEnabled = isEnabled;
        }

        public bool IsEnabled {
            get { return _isEnabled; }
            set {
                if (_isEnabled != value) {
                    _isEnabled = value;
                    RaisePropertyChanged(nameof(IsEnabled));
                }
            }
        }

        public string Text {
            get { return _text; }
            set {
                if (_text != value) {
                    _text = value;
                    RaisePropertyChanged(nameof(Text));
                }
            }
        }

        public string Name => _name;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) {
            PropertyChanged?.Invoke(this, e);
        }
        protected void RaisePropertyChanged(string propertyName) {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
    }
}