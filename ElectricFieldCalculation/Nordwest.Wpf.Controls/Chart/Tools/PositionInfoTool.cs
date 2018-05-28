using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nordwest.Wpf.Controls.Chart.Tools {
    public class PositionInfoTool : Tool {
        private ToolTipData _posData;
        private Func<double, double, string> _getString;
        public PositionInfoTool(Func<double, double, string> getString) {
            _posData = new ToolTipData(@"Position", "", true);
            _getString = getString;
        }
        
        protected override void AddHandlers(ChartCanvas chartCanvas) {
            chartCanvas.AddToolTip(_posData);
            chartCanvas.MouseMove += chartCanvas_MouseMove;
            chartCanvas.MouseLeave += chartCanvas_MouseLeave;
        }

        private void chartCanvas_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e) {
            var c = (ChartCanvas) sender;
            c.HideToolTip();
        }

        void chartCanvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e) {
            var c = (ChartCanvas) sender;
            var mousePosition = e.GetPosition(c);
            var x = c.Viewport.HorizontalAxis.ClientToModel(mousePosition.X);
            var y = c.Viewport.VerticalAxis.ClientToModel(mousePosition.Y);
            _posData.Text = _getString(x, y);
        }

        protected override void RemoveHandlers(ChartCanvas chartCanvas) {
            chartCanvas.RemoveToolTip(_posData);
            chartCanvas.MouseMove -= chartCanvas_MouseMove;
        }
    }
}
