using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Nordwest.Wpf.Controls.Chart
{
    public class PaletteChartElement : ChartElement
    {
        private readonly Palette _palette;
        public PaletteChartElement(Palette palette)
        {
            _palette = palette;
            _palette.SelectedIndexChanged += _palette_SelectedIndexChanged;
        }

        public int SelectedIndex
        {
            get { return _palette.SelectedIndex; }
            set { _palette.SelectedIndex = value; }
        }

        public override void Draw(WriteableBitmap writeableBitmap, Rect dirtyModelRect)
        {
            var start = (int)Math.Floor(dirtyModelRect.Y);
            var end = (int)Math.Ceiling(dirtyModelRect.Bottom);
            for (int i = start; i <= end; i++)
            {
                var x = 0;
                var y = writeableBitmap.CheckY(GetClientY(i));
                var x2 = writeableBitmap.PixelWidth;
                var y2 = writeableBitmap.CheckY(GetClientY(i + 1));
                if (y > y2) { var t = y; y = y2; y2 = t; }
                writeableBitmap.FillRectangle(x, y, x2, y2 - 1,
                                              _palette.PaletteElements[i].IntColor);
                if (i == SelectedIndex)
                {
                    var color = _palette.PaletteElements[i].Color.GetBrightness() < 0.3
                                    ? Colors.White
                                    : Colors.Black;
                    writeableBitmap.DrawRectangle(x, y, x2, y2,
                                                  color);
                }
            }
        }

        public override Rect GetModelBounds()
        {
            var h = _palette.PaletteElements.Count - 1;
            return new Rect(0, 0, 1, h);
        }

        public override ChartElementHitTestResult HitTest(Point modelPoint)
        {
            var y = modelPoint.Y;
            if (y >= 0 && y <= _palette.PaletteElements.Count)
            {
                var index = Math.Truncate(y);
                return new PaletteElementHitTestResult(true, (int)index);
            }
            return new ChartElementHitTestResult(false);
        }

        void _palette_SelectedIndexChanged(object sender, EventArgs e)
        {
            Redraw();
        }
    }

    public class PaletteElementHitTestResult : ChartElementHitTestResult
    {
        private readonly int _index;
        public int Index
        {
            get { return _index; }
        }

        public PaletteElementHitTestResult(bool success, int index)
            : base(success)
        {
            _index = index;
        }
    }
}