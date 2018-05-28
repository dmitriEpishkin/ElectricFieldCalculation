using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Nordwest.Wpf.Controls.Chart
{
    public abstract class ChartElement
    {
        private Viewport _viewport;

        public Viewport Viewport
        {
            get { return _viewport; }
            set { _viewport = value; }
        }

        public abstract void Draw(WriteableBitmap writeableBitmap, Rect dirtyModelRect);
        public abstract Rect GetModelBounds();

        public event EventHandler<ChartElementNeedRedrawEventArgs> NeedRedraw;

        protected void OnNeedRedraw(Rect dirtyModelRect)
        {
            EventHandler<ChartElementNeedRedrawEventArgs> handler = NeedRedraw;
            if (handler != null) handler(this, new ChartElementNeedRedrawEventArgs(dirtyModelRect));
        }

        public void Redraw()
        {
            OnNeedRedraw(GetModelBounds());
        }
        public void Redraw(Rect dirtyRect)
        {
            OnNeedRedraw(dirtyRect);
        }
        

        protected double GetClientX(double model) { return Viewport.HorizontalAxis.ModelToClient(model); }
        protected double GetClientY(double model) { return Viewport.VerticalAxis.ModelToClient(model); }

        public abstract ChartElementHitTestResult HitTest(Point modelPoint);
    }

    public class ChartElementHitTestResult
    {
        private readonly bool _success;

        public ChartElementHitTestResult(bool success)
        {
            _success = success;
        }

        public bool Success
        {
            get { return _success; }
        }
    }
    
}