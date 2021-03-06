using System.ComponentModel;
using System.Windows;

namespace Nordwest.Wpf.Controls.Chart {
    public class Viewport : INotifyPropertyChanged {
        private ViewportAxis _verticalAxis;
        private ViewportAxis _horizontalAxis;

        public Viewport(ViewportAxis horizontalAxis, ViewportAxis verticalAxis) {
            VerticalAxis = verticalAxis;
            HorizontalAxis = horizontalAxis;
        }

        public ViewportAxis HorizontalAxis {
            get { return _horizontalAxis; }
            set {
                if (_horizontalAxis == value)
                    return;

                if (_horizontalAxis != null)
                    _horizontalAxis.PropertyChanged -= OnHorizontalAxisPropertyChanged;

                _horizontalAxis = value;

                if (_horizontalAxis != null)
                    _horizontalAxis.PropertyChanged += OnHorizontalAxisPropertyChanged;

                RaisePropertyChanged(nameof(HorizontalAxis));
            }
        }

        public ViewportAxis VerticalAxis {
            get { return _verticalAxis; }
            set {
                if (_verticalAxis == value)
                    return;

                if (_verticalAxis != null)
                    _verticalAxis.PropertyChanged -= OnVerticalAxisPropertyChanged;

                _verticalAxis = value;

                if (_verticalAxis != null)
                    _verticalAxis.PropertyChanged += OnVerticalAxisPropertyChanged;

                RaisePropertyChanged(nameof(VerticalAxis));
            }
        }

        public Rect ModelRectangle {
            get {
                return new Rect(
                    _horizontalAxis.ModelStart,
                    _verticalAxis.ModelStart,
                    _horizontalAxis.Model.Delta(),
                    _verticalAxis.Model.Delta()
                );
            }
            set {
                if (!IsFinite(value.X) || !IsFinite(value.Y) || !IsFinite(value.Width) || !IsFinite(value.Height))
                    return;

                _horizontalAxis.Model = new ViewRange(value.X, value.X + value.Width);
                _verticalAxis.Model = new ViewRange(value.Y, value.Y + value.Height);

                RaisePropertyChanged(nameof(ModelRectangle));
            }
        }

        private void OnHorizontalAxisPropertyChanged(object sender, PropertyChangedEventArgs e) {
            RaisePropertyChanged($@"{nameof(HorizontalAxis)}.{e.PropertyName}");
        }
        private void OnVerticalAxisPropertyChanged(object sender, PropertyChangedEventArgs e) {
            RaisePropertyChanged($@"{nameof(VerticalAxis)}.{e.PropertyName}");
        }

        public Rect ModelToClient(Rect model) {
            var x1 = _horizontalAxis.ModelToClient(model.Left);
            var x2 = _horizontalAxis.ModelToClient(model.Right);
            var y1 = _verticalAxis.ModelToClient(model.Top);
            var y2 = _verticalAxis.ModelToClient(model.Bottom);

            return Helpers.RectFromLTRB(x1, y1, x2, y2);
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