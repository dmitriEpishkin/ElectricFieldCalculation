using System;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Windows.Input;
using Nordwest.Wpf.Controls.Chart;

namespace Nordwest.Wpf.Controls
{
    [TemplatePart(Name = @"PaletteChart", Type = typeof(ChartControl))]
    public class PaletteControl:Control
    {
        private ChartControl _chart;
        private PaletteChartElement _chartElement;
        private Palette _palette;
        private PaletteTool _paletteTool;

        static PaletteControl()
         {
             DefaultStyleKeyProperty.OverrideMetadata(typeof(PaletteControl), new FrameworkPropertyMetadata(typeof(PaletteControl)));
         }

        
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _chart = GetTemplateChild(@"PaletteChart") as ChartControl;
            if (_chart != null)
            {
                _chart.LeftAxis.Visibility = Visibility.Hidden;
                _chart.TopAxis.Visibility = Visibility.Hidden;
                _chart.BottomAxis.Visibility = Visibility.Hidden;
                _chart.RightAxis.MarkProvider = new IndexedMarkProvider();
                _chart.VerticalAxis.IsReversed = true;
                _chart.LeftAxisSize = 10;
                _chart.TopAxisSize = 10;
                _chart.BottomAxisSize = 10;
                _chart.RightAxisSize = 60;
                if (_palette != null)
                {
                    _chartElement = new PaletteChartElement(_palette);
                    _chart.ChartElements.Add(_chartElement);
                }
                _chart.ToolCollection.Add(new PanTool());
                _chart.ToolCollection.Add(new ZoomTool());
                _paletteTool = new PaletteTool() { PaletteChartElement = _chartElement };
                _chart.ToolCollection.Add(_paletteTool);
            }
        }

        public Palette Palette
        {
            get { return _palette; }
            set
            {
                if (_palette == value) return;
                if (_palette != null) _palette.PaletteChanged -= _palette_PaletteChanged;
                _palette = value;
                if (_palette != null)
                {
                    _chart.ChartElements.Clear();
                    _chartElement = new PaletteChartElement(_palette);
                    _chart.ChartElements.Add(_chartElement);
                    _palette.PaletteChanged += _palette_PaletteChanged;
                    _chart.ViewportModelRect = _chartElement.GetModelBounds();
                    _paletteTool.PaletteChartElement = _chartElement;
                    _chartElement.Redraw();
                    _palette_PaletteChanged(_palette, EventArgs.Empty);
                }
            }
        }

        
        void _palette_PaletteChanged(object sender, System.EventArgs e)
        {
            var provider = (_chart.RightAxis.MarkProvider as IndexedMarkProvider);
            if (provider != null)
            {
                provider.Indexes.Clear();
                provider.Indexes.AddRange(_palette.PaletteElements.Select((pe, i) => new Tuple<double, string>(i, pe.Value.ToString())));
            }
            _chartElement.Redraw();
        }
    }

    public class PaletteTool:Tool
    {
        private PaletteChartElement _paletteChartElement;

        public PaletteChartElement PaletteChartElement
        {
            get { return _paletteChartElement; }
            set { _paletteChartElement = value; }
        }

        
        void chartCanvas_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left || _paletteChartElement == null) return;
            var point = e.MouseDevice.GetPosition((IInputElement)sender);
            var modelPoint = new Point(ChartCanvas.Viewport.HorizontalAxis.ClientToModel(point.X),
                                       ChartCanvas.Viewport.VerticalAxis.ClientToModel(point.Y));
            var hitResult = _paletteChartElement.HitTest(modelPoint);
            if (hitResult.Success)
            {
                var newIndex = ((PaletteElementHitTestResult)hitResult).Index;
                _paletteChartElement.SelectedIndex = newIndex;
            }
            else
            {
                _paletteChartElement.SelectedIndex = -1;
            }
        }

        protected override void AddHandlers(ChartCanvas chartCanvas)
        {
            chartCanvas.MouseDown += chartCanvas_MouseDown;
        }

        protected override void RemoveHandlers(ChartCanvas chartCanvas)
        {
            chartCanvas.MouseDown -= chartCanvas_MouseDown;
        }
    }
}