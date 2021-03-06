using System;
using System.Windows;
using System.Windows.Input;

namespace Nordwest.Wpf.Controls.Chart
{
    public class PanTool : Tool
    {
        private Point _lastMousePos;
        private bool _mouseDown;
        private readonly Func<Rect, Rect> _getBoundsMethod;

        private bool _zoomOnBoundary = true;
        private bool _x = true;
        private bool _y = true;

        public PanTool(bool y, bool x, bool zoomOnBoundary) : this(y, x) {
            _zoomOnBoundary = zoomOnBoundary;
        }

        public PanTool(bool y, bool x) : this(r => r) {
            _x = x;
            _y = y;
        }
        public PanTool(Func<Rect, Rect> getCustomBoundsMethod)
        {
            _getBoundsMethod = getCustomBoundsMethod;
        }
        public PanTool() : this(r => r) { }

        #region Overrides of Tool

        protected override void AddHandlers(ChartCanvas chartCanvas)
        {
            chartCanvas.MouseDown += chartCanvas_MouseDown;
            chartCanvas.MouseUp += chartCanvas_MouseUp;
            chartCanvas.MouseMove += chartCanvas_MouseMove;
        }

        
        void chartCanvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!_mouseDown) return;
            var mousePos = e.MouseDevice.GetPosition((IInputElement)sender);

            var delta = _lastMousePos - mousePos;
            Pan(delta);

            OnPanUsed(new PanToolEventArgs(mousePos, _lastMousePos));

            _lastMousePos = mousePos;
        }

        void chartCanvas_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.RightButton != MouseButtonState.Released) return;
            _mouseDown = false;
            e.MouseDevice.Capture((IInputElement)sender, CaptureMode.None);
        }

        void chartCanvas_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.RightButton != MouseButtonState.Pressed) return;
            var mousePos = e.MouseDevice.GetPosition((IInputElement)sender);

            _lastMousePos = mousePos;
            _mouseDown = true;
            e.MouseDevice.Capture((IInputElement)sender);
        }

        protected override void RemoveHandlers(ChartCanvas chartCanvas)
        {
            chartCanvas.MouseDown -= chartCanvas_MouseDown;
            chartCanvas.MouseUp -= chartCanvas_MouseUp;
            chartCanvas.MouseMove -= chartCanvas_MouseMove;
        }

        #endregion

        
        public void Pan(Vector delta)
        {
            
            //var cx1 = delta.X;
            //var cy1 = delta.Y;
            var cx1 = ChartCanvas.Viewport.HorizontalAxis.ClientStart + delta.X;
            var cy1 = ChartCanvas.Viewport.VerticalAxis.ClientStart + delta.Y;
            var cx2 = ChartCanvas.Viewport.HorizontalAxis.ClientEnd + delta.X;
            var cy2 = ChartCanvas.Viewport.VerticalAxis.ClientEnd + delta.Y;

            var x1 = ChartCanvas.Viewport.HorizontalAxis.ClientToModel(cx1);
            var y1 = ChartCanvas.Viewport.VerticalAxis.ClientToModel(cy1);
            var x2 = ChartCanvas.Viewport.HorizontalAxis.ClientToModel(cx2);
            var y2 = ChartCanvas.Viewport.VerticalAxis.ClientToModel(cy2);
            var modelRectangle = new Rect(Math.Min(x1, x2), Math.Min(y1, y2), Math.Abs(x2 - x1), Math.Abs(y2 - y1));

            if (!_y) modelRectangle = new Rect(modelRectangle.X, ChartCanvas.Viewport.ModelRectangle.Y, modelRectangle.Width,
                                          ChartCanvas.Viewport.ModelRectangle.Height);
            if (!_x) modelRectangle = new Rect(ChartCanvas.Viewport.ModelRectangle.X, modelRectangle.Y,
                                          ChartCanvas.Viewport.ModelRectangle.Width, modelRectangle.Height);

            var bounds = _getBoundsMethod.Invoke(ChartCanvas.GetModelBounds());

            if (_zoomOnBoundary)
                modelRectangle.Intersect(bounds);

            if (modelRectangle == Rect.Empty)
                return;

            ChartCanvas.Viewport.ModelRectangle = modelRectangle;
        }

        public event EventHandler<PanToolEventArgs> PanUsed;

        public void OnPanUsed(PanToolEventArgs e)
        {
            EventHandler<PanToolEventArgs> handler = PanUsed;
            if (handler != null) handler(this, e);
        }
    }

    public class PanToolEventArgs : EventArgs
    {
        private readonly Point _newMousePosition;
        private readonly Point _oldMousePosition;

        public PanToolEventArgs(Point newMousePosition, Point oldMousePosition)
        {
            _newMousePosition = newMousePosition;
            _oldMousePosition = oldMousePosition;
        }

        public Point OldMousePosition
        {
            get { return _oldMousePosition; }
        }

        public Point NewMousePosition
        {
            get { return _newMousePosition; }
        }
    }
}