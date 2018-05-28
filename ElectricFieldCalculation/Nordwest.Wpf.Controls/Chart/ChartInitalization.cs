
using System.Windows;

namespace Nordwest.Wpf.Controls.Chart
{
    public static class ChartInitalization
    {
        public static void LinLin(ChartControl chart, bool panTool = true, bool zoomTool = true) {
            chart.VerticalAxis = new LinAxis { IsReversed = true };
            chart.HorizontalAxis = new LinAxis();
            chart.LeftAxis.MarkProvider = new LinearMarkProvider(10);
            chart.BottomAxis.MarkProvider = new LinearMarkProvider(10);
            chart.Canvas.LeftMarkProvider = chart.LeftAxis.MarkProvider;
            chart.Canvas.BottomMarkProvider = chart.BottomAxis.MarkProvider;
            chart.TopAxis.Visibility = Visibility.Hidden;
            chart.RightAxis.Visibility = Visibility.Hidden;
            chart.ToolCollection.Add(new PanTool());
            chart.ToolCollection.Add(new ZoomTool());
        }
    }


}
