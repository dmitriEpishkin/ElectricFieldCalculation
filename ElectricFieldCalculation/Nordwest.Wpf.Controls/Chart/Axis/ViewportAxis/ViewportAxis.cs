using System;
using System.ComponentModel;

namespace Nordwest.Wpf.Controls.Chart {
    public abstract class ViewportAxis : INotifyPropertyChanged {
        private double _cachedModelScreenScale;

        private ViewRange _model;
        private ViewRange _view;
        private ViewRange _client;

        // MODEL<=>VIEW <-> CLIENT

        private bool _updating;
        private bool _isReversed;

        public ViewportAxis() {
            PropertyChanged += (sender, e) => {

                // model-world sync
                if (!_updating) {
                    _updating = true;
                    switch (e.PropertyName) {
                        case nameof(Model):
                            _view = new ViewRange(ModelToView(_model.Start), ModelToView(_model.End));
                            break;
                        case nameof(View):
                            _model = new ViewRange(ViewToModel(_view.Start), ViewToModel(_view.End));
                            break;
                    }
                    _updating = false;

                    UpdateCache();
                }
            };
        }

        // MODEL

        public ViewRange Model {
            get { return _model; }
            set {
                if (!IsFinite(value.Start) || !IsFinite(value.End))
                    return;

                if (_model.Start != value.Start || _model.End != value.End) {
                    _model = value;
                    RaisePropertyChanged(nameof(Model));
                }
            }
        }
        public double ModelStart {
            get { return _model.Start; }
            set { Model = new ViewRange(value, _model.End); }
        }
        public double ModelEnd {
            get { return _model.End; }
            set { Model = new ViewRange(_model.Start, value); }
        }

        // VIEW

        public ViewRange View {
            get { return _view; }
            set {
                if (!IsFinite(value.Start) || !IsFinite(value.End))
                    return;

                if (_view.Start != value.Start || _view.End != value.End) {
                    _view = value;
                    RaisePropertyChanged(nameof(View));
                }
            }
        }
        public double ViewStart {
            get { return _view.Start; }
            set { View = new ViewRange(value, _view.End); }
        }
        public double ViewEnd {
            get { return _view.End; }
            set { View = new ViewRange(_view.Start, value); }
        }

        // CLIENT

        public ViewRange Client {
            get { return _client; }
            set {
                if (!IsFinite(value.Start) || !IsFinite(value.End))
                    return;

                if (_client.Start != value.Start || _client.End != value.End) {
                    _client = value;
                    RaisePropertyChanged(nameof(Client));
                }
            }
        }
        public double ClientStart {
            get { return _client.Start; }
            set { Client = new ViewRange(value, _client.End); }
        }
        public double ClientEnd {
            get { return _client.End; }
            set { Client = new ViewRange(_client.Start, value); }
        }

        public bool IsReversed {
            get { return _isReversed; }
            set {
                if (_isReversed != value) {
                    _isReversed = value;
                    RaisePropertyChanged(nameof(Client));
                }
            }
        }
        
        // TRANSFORM

        public virtual double ModelToView(double value) => value;
        public virtual double ViewToModel(double value) => value;

        public double ViewToClient(double value) {
            value = _cachedModelScreenScale * (ViewToHomogenous(value) - ViewToHomogenous(_view.Start));
            value = ClientToClient(value);
            return value;
        }
        public double ClientToView(double value) {
            value = ClientToClient(value);
            value = HomogenousToView(value / _cachedModelScreenScale + ViewToHomogenous(_view.Start));
            return value;
        }

        public abstract double ViewToHomogenous(double value);
        public abstract double HomogenousToView(double value);

        public double ModelToClient(double value) {
            value = ModelToView(value);
            value = ViewToClient(value);
            return value;
        }
        public double ClientToModel(double value) {
            value = ClientToView(value);
            value = ViewToModel(value);
            return value;
        }

        private double ClientToClient(double value) {
            if (_isReversed)
                value = _client.Delta() - value;
            return value;
        }

        private void UpdateCache() {
            _cachedModelScreenScale = (_client.End - _client.Start) / (ViewToHomogenous(_view.End) - ViewToHomogenous(_view.Start));
        }

        private static bool IsFinite(double value) {
            return !double.IsNaN(value) && !double.IsInfinity(value);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) {
            PropertyChanged?.Invoke(this, e);
        }
        protected void RaisePropertyChanged(string propertyName) {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
    }
}