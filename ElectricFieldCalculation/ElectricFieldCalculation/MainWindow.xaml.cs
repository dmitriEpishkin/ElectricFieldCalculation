
using Nordwest.Wpf.Controls.Chart;
using SynteticData.TimeSeries;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SynteticData.Data;
using System.Collections.Generic;

namespace ElectricFieldCalculation {
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        private ViewportAxis _timeAxis = new LinAxis();
        private ViewportAxis _fAxis = new Log10Axis();

        private ViewModel _model;

        private Color[] _mainColors = { Colors.Black, Colors.DarkRed, Colors.ForestGreen, Colors.Blue, Colors.Orange, Colors.Gray };
        
        private ChartControl[] _tsCharts;
        private ChartControl[] _zCharts;
        private ChartControl[] _phCharts;

        private List<ChartControl> _separateTsCharts = new List<ChartControl>();

        public MainWindow() {
            InitializeComponent();

            _tsCharts = new[] { GicChart, ExChart, EyChart, HxChart, HyChart, dHxChart, dHyChart };
            _zCharts = new[] { MainImpedanceChart, AddImpedanceChart };
            _phCharts = new[] { MainPhaseChart, AddPhaseChart };

            Initalize();

            _model = new ViewModel();
            DataContext = _model;
            _model.ChartsPanel = TsChartsGrid;

            _model.PropertyChanged += _model_PropertyChanged;
            _model.DataRepository.PropertyChanged += DataRepository_PropertyChanged;
            _model.DataRepository.SelectedSites.CollectionChanged += SelectedSites_CollectionChanged;

            DataRepository_PropertyChanged(null, new PropertyChangedEventArgs(""));
        }

        private void SelectedSites_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
            DataRepository_PropertyChanged(null, new PropertyChangedEventArgs(""));
        }

        private void _model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == @"Z" && _model.Z != null)
                AddZToCharts(_model.Z);

            if (e.PropertyName == "SeparateCharts")
                DataRepository_PropertyChanged(null, new PropertyChangedEventArgs(""));
        }

        private void DataRepository_PropertyChanged(object sender, PropertyChangedEventArgs e) {

            ClearCharts();

            if (_model.SeparateCharts) {
                TsChartsGrid.Children.Clear();

                Func<CheckBox, bool> check = x => x.IsChecked.HasValue && x.IsChecked.Value;

                Action<CheckBox, string, string, TimeSeriesDouble, int> add = (c, header, legend, data, row) => {
                    if (check(c) && data != null) {
                        var chart = new ChartControl();
                        var g = CreateChartGrid(header, legend, chart);
                        AddToChart("", new StackPanel(), chart, data, Colors.Black);
                        TsChartsGrid.Children.Add(g);
                    } 
                };

                foreach (var d in _model.DataRepository.SelectedSites) {
                    add(Gic, "I (GIC), А", d.Name, d.Gic, 0);
                    add(Ex, "Ex, мВ/км", d.Name, d.Ex, 1);
                    add(Ey, "Ey, мВ/км", d.Name, d.Ey, 2);
                    add(Hx, "Hx, нТл", d.Name, d.Hx, 3);
                    add(Hy, "Hy, нТл", d.Name, d.Hy, 4);
                    add(Dx, "dHx/dt", d.Name, d.Dhx, 5);
                    add(Dy, "dHy/dt", d.Name, d.Dhy, 6);                   
                }

            }

            else {

                TsChartsGrid.Children.Clear();
                
                TsChartsGrid.Children.Add(GicGrid);
                TsChartsGrid.Children.Add(ExGrid);
                TsChartsGrid.Children.Add(EyGrid);
                TsChartsGrid.Children.Add(HxGrid);
                TsChartsGrid.Children.Add(HyGrid);
                TsChartsGrid.Children.Add(dHxGrid);
                TsChartsGrid.Children.Add(dHyGrid);

                GicLegend.Children.Clear();
                ExLegend.Children.Clear();
                EyLegend.Children.Clear();
                HxLegend.Children.Clear();
                HyLegend.Children.Clear();
                dHxLegend.Children.Clear();
                dHyLegend.Children.Clear();

                foreach (var data in _model.DataRepository.SelectedSites) {
                    AddToChart(data.Name, GicLegend, GicChart, data.Gic, GetNextColor(GicChart, _mainColors));
                    AddToChart(data.Name, ExLegend, ExChart, data.Ex, GetNextColor(ExChart, _mainColors));
                    AddToChart(data.Name, EyLegend, EyChart, data.Ey, GetNextColor(EyChart, _mainColors));
                    AddToChart(data.Name, HxLegend, HxChart, data.Hx, GetNextColor(HxChart, _mainColors));
                    AddToChart(data.Name, HyLegend, HyChart, data.Hy, GetNextColor(HyChart, _mainColors));
                    AddToChart(data.Name, dHxLegend, dHxChart, data.Dhx, GetNextColor(dHxChart, _mainColors));
                    AddToChart(data.Name, dHyLegend, dHyChart, data.Dhy, GetNextColor(dHyChart, _mainColors));
                }
                Array.ForEach(_tsCharts, t => ((Grid)t.Parent).Visibility = t.ChartElements.Count > 0 ? Visibility.Visible : Visibility.Collapsed);

                if (Gic.IsChecked != null && !Gic.IsChecked.Value)
                    GicGrid.Visibility = Visibility.Collapsed;
                if (Ex.IsChecked != null && !Ex.IsChecked.Value)
                    ExGrid.Visibility = Visibility.Collapsed;
                if (Ey.IsChecked != null && !Ey.IsChecked.Value)
                    EyGrid.Visibility = Visibility.Collapsed;
                if (Hx.IsChecked != null && !Hx.IsChecked.Value)
                    HxGrid.Visibility = Visibility.Collapsed;
                if (Hy.IsChecked != null && !Hy.IsChecked.Value)
                    HyGrid.Visibility = Visibility.Collapsed;
                if (Dx.IsChecked != null && !Dx.IsChecked.Value)
                    dHxGrid.Visibility = Visibility.Collapsed;
                if (Dy.IsChecked != null && !Dy.IsChecked.Value)
                    dHyGrid.Visibility = Visibility.Collapsed;
            }

        }

        private void Initalize() {

            Array.ForEach(_tsCharts, InitalizeTsChart);
            Array.ForEach(_zCharts, InitalizeSpChart);
            Array.ForEach(_phCharts, InitalizePhaseChart);

        }

        private void InitalizeTsChart(ChartControl chart) {
            
            chart.VerticalAxis = new LinAxis { IsReversed = true };
            chart.HorizontalAxis = _timeAxis;
            chart.LeftAxis.MarkProvider = new LinearMarkProvider(10);
            chart.BottomAxis.MarkProvider = new LinearMarkProvider(10);
            chart.Canvas.LeftMarkProvider = chart.LeftAxis.MarkProvider;
            chart.Canvas.BottomMarkProvider = chart.BottomAxis.MarkProvider;
            chart.TopAxis.Visibility = Visibility.Hidden;
            chart.ToolCollection.Add(new PanTool());
            chart.ToolCollection.Add(new ZoomTool());
            
        }

        private void InitalizeSpChart(ChartControl chart) {
            chart.VerticalAxis = new Log10Axis { IsReversed = true };
            chart.HorizontalAxis = _fAxis;
            chart.LeftAxis.MarkProvider = new Log10MarkProvider();
            chart.BottomAxis.MarkProvider = new Log10MarkProvider();
            chart.Canvas.LeftMarkProvider = chart.LeftAxis.MarkProvider;
            chart.Canvas.BottomMarkProvider = chart.BottomAxis.MarkProvider;
            chart.TopAxis.Visibility = Visibility.Hidden;
            chart.RightAxis.Visibility = Visibility.Hidden;
            chart.ToolCollection.Add(new PanTool());
            chart.ToolCollection.Add(new ZoomTool());
        }

        private void InitalizePhaseChart(ChartControl chart) {
            chart.VerticalAxis = new LinAxis { IsReversed = true };
            chart.HorizontalAxis = _fAxis;
            chart.LeftAxis.MarkProvider = new LinearMarkProvider(10);
            chart.BottomAxis.MarkProvider = new Log10MarkProvider();
            chart.Canvas.LeftMarkProvider = chart.LeftAxis.MarkProvider;
            chart.Canvas.BottomMarkProvider = chart.BottomAxis.MarkProvider;
            chart.TopAxis.Visibility = Visibility.Hidden;
            chart.RightAxis.Visibility = Visibility.Hidden;
            chart.ToolCollection.Add(new PanTool());
            chart.ToolCollection.Add(new ZoomTool());
        }

        private Color GetNextColor(ChartControl chart, Color[] colors) {
            var maxInd = chart.ChartElements.OfType<BasePointsChartElement>().Count(e => colors.Contains(e.Color));
            var ind = maxInd % colors.Length; 
            return colors[ind];
        }

        private void ClearCharts() {
            Array.ForEach(_tsCharts, c => { c.ChartElements.Clear(); c.Canvas.Refresh(); RefreshBorders(c); });
            _separateTsCharts.Clear();
        }

        private void AddZToCharts(TensorCurve z) {

            MainImpedanceChart.ChartElements.Clear();
            MainPhaseChart.ChartElements.Clear();
            AddImpedanceChart.ChartElements.Clear();
            AddPhaseChart.ChartElements.Clear();

            Func<TensorCurve, int, int, bool, Point[]> getPoints = (t, x, y, mag) => {

                var points = new Point[t.SourceFrequencies.Length];
                for (int i = 0; i < points.Length; i++) {
                    var v = t.SourceTensors[i][x, y];
                    points[i] = new Point(1.0 / t.SourceFrequencies[i], mag ? v.Magnitude : (v.Phase) / Math.PI * 180.0);
                }

                return points;
            };

            var xy = new PointsChartElement { Color = GetNextColor(MainImpedanceChart, _mainColors) };
            xy.Points.AddRange(getPoints(z, 0, 1, true));
            MainImpedanceChart.ChartElements.Add(xy);
            
            var yx = new PointsChartElement { Color = GetNextColor(MainImpedanceChart, _mainColors) };
            yx.Points.AddRange(getPoints(z, 1, 0, true));
            MainImpedanceChart.ChartElements.Add(yx);

            RefreshBorders(MainImpedanceChart);
            MainImpedanceChart.Canvas.Refresh();
            
            var xyP = new PointsChartElement { Color = GetNextColor(MainPhaseChart, _mainColors) };
            xyP.Points.AddRange(getPoints(z, 0, 1, false));
            MainPhaseChart.ChartElements.Add(xyP);

            var yxP = new PointsChartElement { Color = GetNextColor(MainPhaseChart, _mainColors) };
            yxP.Points.AddRange(getPoints(z, 1, 0, false));
            MainPhaseChart.ChartElements.Add(yxP);

            RefreshBorders(MainPhaseChart);
            MainPhaseChart.Canvas.Refresh();
            
            var xx = new PointsChartElement { Color = GetNextColor(AddImpedanceChart, _mainColors) };
            xx.Points.AddRange(getPoints(z, 0, 0, true));
            AddImpedanceChart.ChartElements.Add(xx);

            var yy = new PointsChartElement { Color = GetNextColor(AddImpedanceChart, _mainColors) };
            yy.Points.AddRange(getPoints(z, 1, 1, true));
            AddImpedanceChart.ChartElements.Add(yy);

            RefreshBorders(AddImpedanceChart);
            AddImpedanceChart.Canvas.Refresh();
            
            var xxP = new PointsChartElement { Color = GetNextColor(AddPhaseChart, _mainColors) };
            xxP.Points.AddRange(getPoints(z, 0, 0, false));
            AddPhaseChart.ChartElements.Add(xxP);

            var yyP = new PointsChartElement { Color = GetNextColor(AddPhaseChart, _mainColors) };
            yyP.Points.AddRange(getPoints(z, 1, 1, false));
            AddPhaseChart.ChartElements.Add(yyP);

            RefreshBorders(AddPhaseChart);
            AddPhaseChart.Canvas.Refresh();
        }

        private void AddToChart(string name, StackPanel legend, ChartControl chart, TimeSeriesDouble ts, Color color) {

            if (ts != null) {
                var e = new LineChartElement {Color = color};
                for (int i = 0; i < ts.Data.Count; i++) {
                    e.Points.Add(new Point(i / ts.SampleRate / 3600.0, ts.Data[i]));
                }
                chart.ChartElements.Add(e);

                var t = new TextBlock();
                t.Text = name;
                t.FontWeight = FontWeights.Bold;
                t.FontSize = 16;
                t.Margin = new Thickness(0, 0, 6, 0);
                t.Foreground = new SolidColorBrush(color);

                legend.Children.Add(t);
            }

            chart.Canvas.Refresh();
            RefreshBorders(chart);
        }
        
        private void RefreshBorders(ChartControl chart) {
            var s = chart.GetBounds();
            chart.ViewportModelRect = s;
        }

        private void removeAll_Click(object sender, RoutedEventArgs e) {
            ClearCharts();
        }

        private void Gic_OnClick(object sender, RoutedEventArgs e) {
            DataRepository_PropertyChanged(null, null);
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            foreach (var d in e.RemovedItems.Cast<SiteData>())
                _model.DataRepository.SelectedSites.Remove(d);
            foreach (var d in e.AddedItems.Cast<SiteData>())
                _model.DataRepository.SelectedSites.Add(d);
        }

        private Grid CreateChartGrid(string name, string legendText, ChartControl chart) {
            var grid = new Grid();
            grid.Height = 180;
            grid.Margin = new Thickness(0, 0, 0, -10);
                        
            chart.Margin = new Thickness(0, 0, 0, 0);
            InitalizeTsChart(chart);

            var header = new TextBlock();
            header.Text = name;
            header.HorizontalAlignment = HorizontalAlignment.Right;
            header.Margin = new Thickness(50, 5, 50, 5);
            header.FontSize = 16;
            header.FontWeight = FontWeights.Bold;

            var legend = new TextBlock();
            legend.HorizontalAlignment = HorizontalAlignment.Left;
            legend.Text = legendText;
            legend.Margin = new Thickness(50, 5, 50, 5);
            legend.FontSize = 16;
            legend.FontWeight = FontWeights.Bold;

            var x = new TextBlock();
            x.Text = "Время, часы";
            x.FontSize = 14;
            x.HorizontalAlignment = HorizontalAlignment.Right;
            x.VerticalAlignment = VerticalAlignment.Bottom;
            x.Margin = new Thickness(55, 35, 55, 35);

            grid.Children.Add(chart);
            grid.Children.Add(header);
            grid.Children.Add(legend);
            grid.Children.Add(x);

            return grid;
        }
        
    }
}
