using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Nordwest.Wpf.Controls.Chart;

namespace Nordwest.Wpf.Controls.MiniMap
{
    [TemplatePart(Name = @"Map", Type = typeof(Image))]
    public class MiniMapContent : UserControl
    {
        private readonly BackgroundWorker _renderer = new BackgroundWorker();
        private bool _isBusy;
        private bool _needRedraw;
        private readonly List<IDrawToMiniMap> _miniMaps = new List<IDrawToMiniMap>();

        private Rect _boundsRect = new Rect(1, 1, 10, 10);
        private Rect _viewportRect = new Rect(1, 1, 10, 10);
        private Point _cachedScale;
        private Canvas _canvas;
        private Rectangle _viewportRectangle;

        private ViewportAxis _horizontalAxis;
        public ViewportAxis HorizontalAxis
        {
            get { return _horizontalAxis; }
            set { _horizontalAxis = value; CacheTransform(); }
        }

        private ViewportAxis _verticalAxis;
        public ViewportAxis VerticalAxis
        {
            get { return _verticalAxis; }
            set
            {
                _verticalAxis = value;
                CacheTransform();
            }
        }

        public Rect BoundsRect
        {
            get { return _boundsRect; }
            set
            {
                if (_boundsRect == value) return;
                _boundsRect = value;
                CacheTransform();
                //UpdateViewport();
                OnBoundsChanged();
            }
        }
        public Rect ViewportRect
        {
            get { return _viewportRect; }
            set
            {
                if (_viewportRect == value) return;
                _viewportRect = value;
                _viewportRect.Intersect(_boundsRect);
                UpdateViewport();
                OnViewportChanged();
            }
        }

        public MiniMapContent()
        {
            Background = new SolidColorBrush(Colors.White);
            _renderer.DoWork += _renderer_DoWork;
            _renderer.RunWorkerCompleted += _renderer_RunWorkerCompleted;
        }
        static MiniMapContent()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MiniMapContent), new FrameworkPropertyMetadata(typeof(MiniMapContent)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _canvas = GetTemplateChild(@"Map") as Canvas;
            _viewportRectangle = GetTemplateChild(@"ViewportRectangle") as Rectangle;
            _viewportRectangle.MouseDown += ViewportRectsngle_MouseDown;
            _viewportRectangle.MouseUp += ViewportRectsngle_MouseUp;
            _viewportRectangle.MouseMove += ViewportRectsngle_MouseMove;
        }

        public List<IDrawToMiniMap> MiniMaps
        {
            get { return _miniMaps; }
        }

        #region Draw minimap
        public void Redraw()
        {
            if (_isBusy) _needRedraw = true;
            else if (_miniMaps.Count != 0)
            {
                _isBusy = true;
                // var bounds = _miniMaps.Select(m => m.GetModelBounds()).Aggregate(Rect.Union);

                _renderer.RunWorkerAsync(new RendererArgs((int)ActualWidth, (int)ActualHeight, _boundsRect));
            }
        }

        
        void _renderer_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var image = e.Result as WriteableBitmap;
            //var map = GetTemplateChild("Map") as Canvas;
            if (_canvas != null) _canvas.Background = new ImageBrush(image);
            _isBusy = false;
            if (_needRedraw)
            {
                _needRedraw = false;

                Redraw();
            }
        }

        
        void _renderer_DoWork(object sender, DoWorkEventArgs e)
        {
            var args = (RendererArgs)e.Argument;
            if (args.ModelRect.Width <= double.Epsilon || args.ModelRect.Height <= double.Epsilon) return;
            var w = Math.Max(2, args.ClientWidth);
            var h = Math.Max(2, args.ClientHeight);
            var image = BitmapFactory.New(w, h);
            try
            {
                using (image.GetBitmapContext())
                {
                    foreach (var miniMap in _miniMaps)
                        miniMap.DrawToMiniMap(image, args.ModelRect);
                }
            }
            catch (Exception ex)
            {
                image = BitmapFactory.New(w, h);
                Debug.WriteLine(ex.GetType() + @" in NWChart.ChartUI.MiniMapContent._renderer_DoWork(object sender, DoWorkEventArgs e).");
            }
            image.Freeze();
            e.Result = image;

        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            Redraw();
            UpdateBounds();
            if (HorizontalAxis != null && VerticalAxis != null)
            {
                CacheTransform();
                UpdateViewport();
            }
        }

        struct RendererArgs
        {
            private readonly int _clientWidth;
            private readonly int _clientHeight;
            private readonly Rect _modelRect;

            public RendererArgs(int clientWidth, int clientHeight, Rect modelRect)
                : this()
            {
                _clientWidth = clientWidth;
                _clientHeight = clientHeight;
                _modelRect = modelRect;
            }

            public Rect ModelRect
            {
                get { return _modelRect; }
            }

            public int ClientHeight
            {
                get { return _clientHeight; }
            }

            public int ClientWidth
            {
                get { return _clientWidth; }
            }
        }
        #endregion

        public void UpdateBounds()
        {
            if (_miniMaps.Count > 0)
            {
                var rect = _miniMaps.Select(m => m.GetModelBounds()).Aggregate(Rect.Union);
                if (rect.HasNaNValues() || rect.Width == 0 || rect.Height == 0)
                    rect = new Rect(1, 1, 10, 10);
                BoundsRect = rect;
            }
        }
        private void UpdateViewport()
        {
            var cx1 = GetClientX(_viewportRect.Left);
            var cx2 = GetClientX(_viewportRect.Right);
            var cy1 = GetClientY(_viewportRect.Top);
            var cy2 = GetClientY(_viewportRect.Bottom);
            _viewportRectangle.SetValue(Canvas.LeftProperty, cx1);
            _viewportRectangle.SetValue(Canvas.TopProperty, cy1);
            _viewportRectangle.Width = cx2 - cx1;
            _viewportRectangle.Height = cy2 - cy1;
        }
        private double GetClientX(double model)
        {
            var client = (_cachedScale.X * (HorizontalAxis.ViewToHomogenous(model) - HorizontalAxis.ViewToHomogenous(_boundsRect.Left)));
            if (HorizontalAxis.IsReversed) return _canvas.ActualWidth - client;
            return client + 0;
        }
        private double GetClientY(double model)
        {
            var client = (_cachedScale.Y * (VerticalAxis.ViewToHomogenous(model) - VerticalAxis.ViewToHomogenous(_boundsRect.Top)));
            if (VerticalAxis.IsReversed) return _canvas.ActualHeight - client;
            return client + 0;
        }
        public virtual double GetModelX(double client)
        {
            if (HorizontalAxis.IsReversed) client = _canvas.ActualWidth - client;
            else client = client - 0;
            return HorizontalAxis.HomogenousToView((client) / _cachedScale.X + HorizontalAxis.ViewToHomogenous(_boundsRect.Left));
        }
        public virtual double GetModelY(double client)
        {
            if (VerticalAxis.IsReversed) client = _canvas.ActualHeight - client;
            else client = client - 0;
            return VerticalAxis.HomogenousToView((client) / _cachedScale.Y + VerticalAxis.ViewToHomogenous(_boundsRect.Top));
        }
        private void CacheTransform()
        {
            if (HorizontalAxis==null || VerticalAxis == null) return;
            var scaleX = _canvas.ActualWidth / (HorizontalAxis.ViewToHomogenous(_boundsRect.Right) - HorizontalAxis.ViewToHomogenous(_boundsRect.Left));
            var scaleY = _canvas.ActualHeight / (VerticalAxis.ViewToHomogenous(_boundsRect.Bottom) - HorizontalAxis.ViewToHomogenous(_boundsRect.Top));
            _cachedScale = new Point(scaleX, scaleY);
        }

        public event EventHandler ViewportChanged;
        public event EventHandler BoundsChanged;

        public void OnBoundsChanged()
        {
            EventHandler handler = BoundsChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }
        public void OnViewportChanged()
        {
            EventHandler handler = ViewportChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        private Point _lastPosition;
        private Area _capturedArea = Area.None;
        private Area ViewportHitTest(Point position, double thickness)
        {
            if (position.X <= thickness)
            {
                if (position.Y <= thickness) return Area.TopLeft;
                if (position.Y >= _viewportRectangle.ActualHeight - thickness) return Area.BottomLeft;
                return Area.MiddleLeft;
            }
            if (position.X >= _viewportRectangle.ActualWidth - thickness)
            {
                if (position.Y <= thickness) return Area.TopRight;
                if (position.Y >= _viewportRectangle.ActualHeight - thickness) return Area.BottomRight;
                return Area.MiddleRight;
            }

            if (position.Y <= thickness) return Area.TopCenter;
            if (position.Y >= _viewportRectangle.ActualHeight - thickness) return Area.BottomCenter;
            return Area.MiddleCenter;
        }
        private void ViewportRectsngle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;
            var pos = e.MouseDevice.GetPosition((IInputElement)sender);
            _capturedArea = ViewportHitTest(pos, _viewportRectangle.StrokeThickness);
            _lastPosition = e.MouseDevice.GetPosition(_canvas);
            e.MouseDevice.Capture((IInputElement)sender);
        }
        private void ViewportRectsngle_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _capturedArea = Area.None;
            e.MouseDevice.Capture((IInputElement)sender, CaptureMode.None);
        }
        private void ViewportRectsngle_MouseMove(object sender, MouseEventArgs e)
        {
            if (_capturedArea == Area.None)
            {
                var hoverArea = ViewportHitTest(e.MouseDevice.GetPosition((IInputElement)sender), 3);
                var newCursor = _viewportRectangle.Cursor;
                switch (hoverArea)
                {
                    case Area.MiddleCenter:
                        newCursor = Cursors.Hand;
                        break;
                    case Area.None:
                        break;
                    case Area.MiddleLeft:
                    case Area.MiddleRight:
                        newCursor = Cursors.SizeWE;
                        break;
                    case Area.TopCenter:
                    case Area.BottomCenter:
                        newCursor = Cursors.SizeNS;
                        break;
                    case Area.TopLeft:
                    case Area.BottomRight:
                        newCursor = Cursors.SizeNWSE;
                        break;
                    case Area.TopRight:
                    case Area.BottomLeft:
                        newCursor = Cursors.SizeNESW;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                if (_viewportRectangle.Cursor != newCursor) _viewportRectangle.Cursor = newCursor;
            }
            else
            {
                var pos = e.MouseDevice.GetPosition(_canvas);
                switch (_capturedArea)
                {
                    case Area.MiddleCenter:
                        var delta = pos - _lastPosition;
                        var left = (double)_viewportRectangle.GetValue(Canvas.LeftProperty);
                        var top = (double)_viewportRectangle.GetValue(Canvas.TopProperty);
                        var x = GetModelX(left + delta.X);
                        var y = GetModelY(top + delta.Y);
                        var w = GetModelX(left + _viewportRectangle.ActualWidth + delta.X) - x;
                        var h = GetModelY(top + _viewportRectangle.ActualHeight + delta.Y) - y;
                        //if (x < BoundsRect.Left) x = BoundsRect.Left;
                        //if (x > BoundsRect.Right - w) x = BoundsRect.Right - w;
                        //if (y < BoundsRect.Top) y = BoundsRect.Top;
                        //if (y > BoundsRect.Bottom - h) y = BoundsRect.Bottom - h;
                        ViewportRect = new Rect(x, y, w, h);
                        _lastPosition = pos;
                        break;
                    case Area.None:
                        break;
                    case Area.MiddleLeft:
                        var newLeft = GetModelX(pos.X);
                        if (ViewportRect.X + ViewportRect.Width - newLeft > 0)
                            ViewportRect = new Rect(newLeft, ViewportRect.Y, ViewportRect.X + ViewportRect.Width - newLeft, ViewportRect.Height);
                        break;
                    case Area.MiddleRight:
                        var newRight = GetModelX(pos.X);
                        if (newRight - ViewportRect.X > 0)
                            ViewportRect = new Rect(ViewportRect.X, ViewportRect.Y, newRight - ViewportRect.X, ViewportRect.Height);
                        break;
                    case Area.TopCenter:
                        var newTop = GetModelY(pos.Y);
                        if (ViewportRect.Y + ViewportRect.Height - newTop > 0)
                            ViewportRect = new Rect(ViewportRect.X, newTop, ViewportRect.Width, ViewportRect.Y + ViewportRect.Height - newTop);
                        break;
                    case Area.BottomCenter:
                        var newBottom = GetModelY(pos.Y);
                        if (newBottom - ViewportRect.Y > 0)
                            ViewportRect = new Rect(ViewportRect.X, ViewportRect.Y, ViewportRect.Width, newBottom - ViewportRect.Y);
                        break;
                    case Area.TopLeft:
                        var newTopLeft = new Point(GetModelX(pos.X), GetModelY(pos.Y));
                        if (ViewportRect.X + ViewportRect.Width - newTopLeft.X > 0 && ViewportRect.Y + ViewportRect.Height - newTopLeft.Y > 0)
                            ViewportRect = new Rect(newTopLeft.X, newTopLeft.Y, ViewportRect.X + ViewportRect.Width - newTopLeft.X, ViewportRect.Y + ViewportRect.Height - newTopLeft.Y);
                        break;
                    case Area.TopRight:
                        var newTopRight = new Point(GetModelX(pos.X), GetModelY(pos.Y));
                        if (newTopRight.X - ViewportRect.X > 0 && ViewportRect.Y + ViewportRect.Height - newTopRight.Y > 0)
                            ViewportRect = new Rect(ViewportRect.X, newTopRight.Y, newTopRight.X - ViewportRect.X, ViewportRect.Y + ViewportRect.Height - newTopRight.Y);
                        break;
                    case Area.BottomLeft:
                        var newBottomLeft = new Point(GetModelX(pos.X), GetModelY(pos.Y));
                        if (ViewportRect.X + ViewportRect.Width - newBottomLeft.X > 0 && newBottomLeft.Y - ViewportRect.Y > 0)
                            ViewportRect = new Rect(newBottomLeft.X, ViewportRect.Y, ViewportRect.X + ViewportRect.Width - newBottomLeft.X, newBottomLeft.Y - ViewportRect.Y);
                        break;
                    case Area.BottomRight:
                        var newBottomRight = new Point(GetModelX(pos.X), GetModelY(pos.Y));
                        if (newBottomRight.X - ViewportRect.X > 0 && newBottomRight.Y - ViewportRect.Y > 0)
                            ViewportRect = new Rect(ViewportRect.X, ViewportRect.Y, newBottomRight.X - ViewportRect.X, newBottomRight.Y - ViewportRect.Y);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        private enum Area
        {
            None,
            MiddleCenter,
            MiddleLeft,
            MiddleRight,
            TopCenter,
            BottomCenter,
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight
        }
    }

    public interface IDrawToMiniMap
    {
        void DrawToMiniMap(WriteableBitmap writeableBitmap, Rect modelRect);
        Rect GetModelBounds();
    }
}