using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows;

namespace ElectricFieldCalculation
{
    public class CalculationInfoModel : INotifyPropertyChanged
    {
        private Func<Tuple<bool, string>> _validate;
        private Action<Action<double>> _calculate;

        private bool _isCalculating = false;
        private bool _canProcess;
        private double _progress;
        private string _message;

        public CalculationInfoModel(Func<Tuple<bool, string>> validate, Action<Action<double>> calculate)
        {
            _validate = validate;
            _calculate = calculate;

            RunCommand = new DelegateCommand(Calculate, CanCalculate);
        }
        
        public void RaiseValidation() {
            Validate();
        }

        private void Calculate()
        {
            IsCalculating = true;

            Task.Factory.StartNew(() => {

                _calculate(p => Progress = p);

                Application.Current.Dispatcher.Invoke(new Action(() => {                    
                    IsCalculating = false;
                }));
            });
        }

        private bool CanCalculate() {
            return CanProcess;
        }
        
        private void Validate()
        {
            var res = _validate();

            CanProcess = res.Item1;
            Message = res.Item2;

            RunCommand.RaiseCanExecuteChanged();
        }

        public bool IsCalculating {
            get { return _isCalculating; }
            set {
                if (_isCalculating != value)
                {
                    _isCalculating = value;
                    OnPropertyChanged(nameof(IsCalculating));
                    Validate();
                }
            }
        }

        public bool CanProcess {
            get { return _canProcess; }
            set {
                if (_canProcess != value)
                {
                    _canProcess = value;
                    OnPropertyChanged(nameof(CanProcess));
                }
            }
        }

        public double Progress {
            get { return _progress; }
            set {
                if (_progress != value)
                {
                    _progress = value;
                    OnPropertyChanged(nameof(Progress));
                }
            }
        }

        public string Message {
            get { return _message; }
            set {
                if (_message != value)
                {
                    _message = value;
                    OnPropertyChanged(nameof(Message));
                }
            }
        }
        
        public DelegateCommand RunCommand { get; set; }
        
        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
