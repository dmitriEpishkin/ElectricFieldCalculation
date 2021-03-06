using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Nordwest.Wpf.Controls.Chart
{
    public partial class ChartControl : UserControl
    {
        private Point _lastMovePosition;

        private bool _scaling = false;
        private bool _placeLabelsInside;

        private readonly List<Rect> _occupiedLabelPlaces = new List<Rect>();

        public ChartControl()
        {
            InitializeComponent();
            
            chartCanvas.Viewport.PropertyChanged += OnViewportPropertyChanged;
            chartCanvas.IsToolTipFollowsMouse = false;

            LeftAxis.OccupiedLabelPlaces = _occupiedLabelPlaces;
            RightAxis.OccupiedLabelPlaces = _occupiedLabelPlaces;
            BottomAxis.OccupiedLabelPlaces = _occupiedLabelPlaces;
            TopAxis.OccupiedLabelPlaces = _occupiedLabelPlaces;

            LeftAxis.MarkProviderChanged += LeftAxis_MarkProviderChanged;
            RightAxis.MarkProviderChanged += RightAxis_MarkProviderChanged;
            BottomAxis.MarkProviderChanged += BottomAxis_MarkProviderChanged;
            TopAxis.MarkProviderChanged += TopAxis_MarkProviderChanged;
        }

        private void TopAxis_MarkProviderChanged(object sender, EventArgs e) {
            Canvas.TopMarkProvider = TopAxis.MarkProvider;
        }

        private void BottomAxis_MarkProviderChanged(object sender, EventArgs e) {
            Canvas.BottomMarkProvider = BottomAxis.MarkProvider;
        }

        private void RightAxis_MarkProviderChanged(object sender, EventArgs e) {
            Canvas.RightMarkProvider = RightAxis.MarkProvider;
        }

        private void LeftAxis_MarkProviderChanged(object sender, EventArgs e) {
            Canvas.LeftMarkProvider = LeftAxis.MarkProvider;
        }

        private void OnViewportPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            InvalidateAxis();
        }

        
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            bottomAxis.Axis = chartCanvas.Viewport.HorizontalAxis;
            leftAxis.Axis = chartCanvas.Viewport.VerticalAxis;
            rightAxis.Axis = chartCanvas.Viewport.VerticalAxis;
            topAxis.Axis = chartCanvas.Viewport.HorizontalAxis;
            
            chartCanvas.Refresh();

            InvalidateAxis();
        }

        private void InvalidateAxis() {

            _occupiedLabelPlaces.Clear();

            topAxis.InvalidateVisual();
            leftAxis.InvalidateVisual();
            rightAxis.InvalidateVisual();
            bottomAxis.InvalidateVisual();

        }

        public ObservableCollection<ChartElement> ChartElements
        {
            get { return chartCanvas.ChartElements; }
        }

        public ToolCollection ToolCollection { get { return chartCanvas.Tools; } }

        public Rect ViewportModelRect
        {
            get { return chartCanvas.Viewport.ModelRectangle; }
            set { chartCanvas.Viewport.ModelRectangle = value; }
        }

        public void InitalizeHorizontalAxis(ViewportAxis axis, AxisMarkProvider markProvider) {
            HorizontalAxis = axis;
            BottomAxis.MarkProvider = markProvider;
            TopAxis.MarkProvider = markProvider;
        }

        public void InitalizeVerticalAxis(ViewportAxis axis, AxisMarkProvider markProvider) {
            VerticalAxis = axis;
            LeftAxis.MarkProvider = markProvider;
            RightAxis.MarkProvider = markProvider;
        }

        public bool PlaceLabelsInside {
            get { return _placeLabelsInside; }
            set {
                _placeLabelsInside = value;
                if (_placeLabelsInside) {

                    grid.ColumnDefinitions[0].Width = new GridLength(0);
                    grid.ColumnDefinitions[2].Width = new GridLength(0);
                    grid.RowDefinitions[0].Height = new GridLength(0);
                    grid.RowDefinitions[2].Height = new GridLength(0);

                    Grid.SetRow(topAxis, 1);
                    Grid.SetRow(bottomAxis, 1);
                    topAxis.AxisAlignment = VerticalAlignment.Top;
                    topAxis.TextAlignment = ContentAlignment.BottomCenter;

                    bottomAxis.AxisAlignment = VerticalAlignment.Bottom;
                    bottomAxis.TextAlignment = ContentAlignment.TopCenter;

                    Grid.SetColumn(leftAxis, 1);
                    Grid.SetColumn(rightAxis, 1);
                    leftAxis.AxisAlignment = HorizontalAlignment.Left;
                    leftAxis.TextAlignment = ContentAlignment.MiddleRight;
                    
                    rightAxis.AxisAlignment = HorizontalAlignment.Right;
                    rightAxis.TextAlignment = ContentAlignment.MiddleLeft;

                    topAxis.TrimSideLabels = true;
                    bottomAxis.TrimSideLabels = true;
                    leftAxis.TrimSideLabels = true;
                    rightAxis.TrimSideLabels = true;
                }
                else {

                    grid.ColumnDefinitions[0].Width = new GridLength(50);
                    grid.ColumnDefinitions[2].Width = new GridLength(50);
                    grid.RowDefinitions[0].Height = new GridLength(30);
                    grid.RowDefinitions[2].Height = new GridLength(30);

                    Grid.SetRow(topAxis, 0);
                    Grid.SetRow(bottomAxis, 2);
                    topAxis.AxisAlignment = VerticalAlignment.Bottom;
                    topAxis.TextAlignment = ContentAlignment.TopCenter;

                    bottomAxis.AxisAlignment = VerticalAlignment.Top;
                    bottomAxis.TextAlignment = ContentAlignment.BottomCenter;

                    Grid.SetColumn(leftAxis, 0);
                    Grid.SetColumn(rightAxis, 2);
                    leftAxis.AxisAlignment = HorizontalAlignment.Right;
                    leftAxis.TextAlignment = ContentAlignment.MiddleLeft;

                    rightAxis.AxisAlignment = HorizontalAlignment.Left;
                    rightAxis.TextAlignment = ContentAlignment.MiddleRight;

                    topAxis.TrimSideLabels = false;
                    bottomAxis.TrimSideLabels = false;
                    leftAxis.TrimSideLabels = false;
                    rightAxis.TrimSideLabels = false;
                }
            }
        }

        public ViewportAxis VerticalAxis
        {
            get { return chartCanvas.Viewport.VerticalAxis; }
            set
            {
                chartCanvas.Viewport.VerticalAxis = value;
                leftAxis.Axis = value;
                rightAxis.Axis = value;
            }
        }

        public ViewportAxis HorizontalAxis
        {
            get { return chartCanvas.Viewport.HorizontalAxis; }
            set
            {
                chartCanvas.Viewport.HorizontalAxis = value;
                topAxis.Axis = value;
                bottomAxis.Axis = value;
            }
        }

        public ChartCanvas Canvas => chartCanvas;

        public VerticalAxisControl LeftAxis => leftAxis;
        public VerticalAxisControl RightAxis => rightAxis;
        public HorizontalAxisControl TopAxis => topAxis;
        public HorizontalAxisControl BottomAxis => bottomAxis;

        public double TopAxisSize { get { return grid.RowDefinitions[0].Height.Value; } set { grid.RowDefinitions[0].Height = new GridLength(value); } }
        public double BottomAxisSize { get { return grid.RowDefinitions[2].Height.Value; } set { grid.RowDefinitions[2].Height = new GridLength(value); } }
        public double LeftAxisSize { get { return grid.ColumnDefinitions[0].Width.Value; } set { grid.ColumnDefinitions[0].Width = new GridLength(value); } }
        public double RightAxisSize { get { return grid.ColumnDefinitions[2].Width.Value; } set { grid.ColumnDefinitions[2].Width = new GridLength(value); } }

        public bool IsToolTipFollowsMouse
        {
            get { return chartCanvas.IsToolTipFollowsMouse; }
            set { chartCanvas.IsToolTipFollowsMouse = value; }
        }

        public Rect GetBounds()
        {
            return chartCanvas.GetModelBounds();
        }

        public bool Scaling
        {
            get { return _scaling; }
            set
            {
                if (_scaling != value)
                {
                    _scaling = value;

                    if (_scaling)
                    {
                        chartCanvas.MouseMove += Control_OnMouseMove;
                        chartCanvas.MouseWheel += Control_OnMouseWheel;
                        chartCanvas.MouseLeftButtonDown += Control_MouseLeftButtonDown;
                        chartCanvas.MouseLeftButtonUp += Control_MouseLeftButtonUp;
                    }
                    else
                    {
                        chartCanvas.MouseMove -= Control_OnMouseMove;
                        chartCanvas.MouseWheel -= Control_OnMouseWheel;
                        chartCanvas.MouseLeftButtonDown -= Control_MouseLeftButtonDown;
                        chartCanvas.MouseLeftButtonUp -= Control_MouseLeftButtonUp;
                    }
                }
            }
        }

        private void Control_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            chartCanvas.CaptureMouse();
        }
        private void Control_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            chartCanvas.ReleaseMouseCapture();
        }
        private void Control_OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var r = ViewportModelRect;

            if (VerticalAxis is Log10Axis)
            {
                var centerLog = Math.Log10(r.Top) + Math.Log10(r.Height)/2.0;
                r.Height += e.Delta/750.0*r.Height;
                r.Y = Math.Pow(10, centerLog - Math.Log10(r.Height)/2.0);
            }
            else
            {
                var center = r.Top + r.Height / 2.0;
                r.Height += e.Delta / 750.0 * r.Height;
                r.Y = center - r.Height / 2.0;
            }

            ViewportModelRect = r;
            e.Handled = true;
        }
        private void Control_OnMouseMove(object sender, MouseEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var pos = e.GetPosition(this);
                if (_lastMovePosition == new Point())
                    _lastMovePosition = pos;

                var d = pos.Y - _lastMovePosition.Y;
                _lastMovePosition = pos;

                var r = ViewportModelRect;

                var y = VerticalAxis.ClientToModel(VerticalAxis.ModelToClient(r.Y) - d);

                if (VerticalAxis is Log10Axis)
                    r.Height *= y/r.Y;

                r.Y = y;

                if (!double.IsNaN(r.X) && !double.IsNaN(r.Y) && !double.IsNaN(r.Width) && !double.IsNaN(r.Height))
                    ViewportModelRect = r;
            }
            else
            {
                _lastMovePosition = new Point();
            }
        }

    }
}
