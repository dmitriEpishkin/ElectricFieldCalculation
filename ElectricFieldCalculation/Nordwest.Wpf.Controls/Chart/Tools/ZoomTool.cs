using System;
using System.Windows;
using System.Windows.Input;

namespace Nordwest.Wpf.Controls.Chart
{
    public class ZoomTool : Tool {

        private bool _zoomOutBorders;
        private bool _y;
        private bool _x;

        #region Overrides of Tool

        protected override void AddHandlers(ChartCanvas chartCanvas)
        {
            chartCanvas.MouseWheel += chartCanvas_MouseWheel;
            chartCanvas.MouseDoubleClick += chartCanvas_MouseDoubleClick;
        }

        void chartCanvas_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Right) return;
            ZoomAll();
            OnZoomAllUsed();
        }

        void chartCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var mPos = e.GetPosition(ChartCanvas);
            
           // var relPos = new Point(mPos.X / ChartCanvas.ActualWidth, mPos.Y / ChartCanvas.ActualHeight);
            Zoom(mPos, e.Delta);
            OnZoomUsed(new ZoomToolEventArgs(mPos, e.Delta));
        }

        protected override void RemoveHandlers(ChartCanvas chartCanvas)
        {
            chartCanvas.MouseWheel -= chartCanvas_MouseWheel;
            chartCanvas.MouseDoubleClick -= chartCanvas_MouseDoubleClick;
        }

        #endregion

        public ZoomTool() { _x = true; _y = true; }

        public ZoomTool(bool y, bool x) {
            _y = y;
            _x = x;
        }

        public ZoomTool(bool y, bool x, bool zoomOutBordes) : this(y, x) {
            _zoomOutBorders = zoomOutBordes;
        }

        
        public void Zoom(Point mPos, int delta)
        {
            var factor = delta / 1200.0;

            var cx1 = factor * mPos.X;
            var cx2 = ChartCanvas.Viewport.HorizontalAxis.ClientEnd - factor * (ChartCanvas.ActualWidth - mPos.X);
            var cy1 = factor * mPos.Y;
            var cy2 = ChartCanvas.Viewport.VerticalAxis.ClientEnd - factor * (ChartCanvas.ActualHeight - mPos.Y);
            var mx1 = ChartCanvas.Viewport.HorizontalAxis.ClientToModel(cx1);
            var mx2 = ChartCanvas.Viewport.HorizontalAxis.ClientToModel(cx2);
            var my1 = ChartCanvas.Viewport.VerticalAxis.ClientToModel(cy1);
            var my2 = ChartCanvas.Viewport.VerticalAxis.ClientToModel(cy2);
            
            var modelRectangle = (_x && _y)
                ? new Rect(Math.Min(mx1, mx2), Math.Min(my1, my2), Math.Abs(mx2 - mx1), Math.Abs(my2 - my1))
                : (_x
                    ? new Rect(Math.Min(mx1, mx2), ChartCanvas.Viewport.VerticalAxis.ModelStart,
                        Math.Abs(mx2 - mx1), ChartCanvas.Viewport.VerticalAxis.Model.Delta())
                    : new Rect(ChartCanvas.Viewport.HorizontalAxis.ModelStart, Math.Min(my1, my2), 
                        ChartCanvas.Viewport.HorizontalAxis.Model.Delta(), Math.Abs(my2 - my1)));

            if (!_zoomOutBorders) {
                var bounds = ChartCanvas.GetModelBounds();
                modelRectangle.Intersect(bounds);
            }

            if (Keyboard.IsKeyDown(Key.LeftShift))
            {
                modelRectangle = new Rect(modelRectangle.X, ChartCanvas.Viewport.ModelRectangle.Y, modelRectangle.Width,
                                          ChartCanvas.Viewport.ModelRectangle.Height);
            }
            else if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                modelRectangle = new Rect(ChartCanvas.Viewport.ModelRectangle.X, modelRectangle.Y,
                                          ChartCanvas.Viewport.ModelRectangle.Width, modelRectangle.Height);
            }

            if (modelRectangle == Rect.Empty) return;

            ChartCanvas.Viewport.ModelRectangle = modelRectangle;
        }

        public void ZoomAll()
        {
            var bounds = ChartCanvas.GetModelBounds();
            if (bounds == Rect.Empty) return;
            ChartCanvas.Viewport.ModelRectangle = bounds;
        }

        public event EventHandler<ZoomToolEventArgs> ZoomUsed;
        public event EventHandler ZoomAllUsed;

        public void OnZoomAllUsed()
        {
            EventHandler handler = ZoomAllUsed;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public void OnZoomUsed(ZoomToolEventArgs e)
        {
            EventHandler<ZoomToolEventArgs> handler = ZoomUsed;
            if (handler != null) handler(this, e);
        }
    }

    public class ZoomToolEventArgs:EventArgs
    {
        private readonly Point _position;
        private readonly int _delta;
        
        public ZoomToolEventArgs(Point position, int delta)
        {
            _position = position;
            _delta = delta;
        }

        public Point Position
        {
            get { return _position; }
        }

        public int Delta
        {
            get { return _delta; }
        }
    }
    
}