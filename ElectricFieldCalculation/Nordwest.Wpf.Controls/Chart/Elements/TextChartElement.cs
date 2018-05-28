
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Nordwest.Wpf.Controls.Chart.Elements {
    public class TextChartElement : ChartElement {

        private string _text;
        private TextRenderer _textRenderer;    
        private double _mx;
        private double _my;
        private ContentAlignment _alignment;
        private Orientation _orientation;

        private readonly Func<Rect, Rect> _getBoundsRect;

        public TextChartElement(string text, TextRenderer textRenderer, double mx, double my, ContentAlignment alignment, Orientation orientation) : this(text, textRenderer, mx, my, alignment, orientation, r => r ) { }
        public TextChartElement(string text, TextRenderer textRenderer, double mx, double my, ContentAlignment alignment, Orientation orientation, Func<Rect, Rect> getBoundsRect) {
            _text = text;
            _textRenderer = textRenderer;
            _mx = mx;
            _my = my;
            _alignment = alignment;
            _orientation = orientation;
            _getBoundsRect = getBoundsRect;
        }

        public override void Draw(WriteableBitmap writeableBitmap, Rect dirtyModelRect) {
                        
            var x = writeableBitmap.CheckX(GetClientX(_mx));
            var y = writeableBitmap.CheckY(GetClientY(_my));

            var size = _textRenderer.MeasureString(_text, _orientation);

            if (_alignment == ContentAlignment.BottomCenter || _alignment == ContentAlignment.BottomLeft || _alignment == ContentAlignment.BottomRight)
                y = y - size.Item2;
            if (_alignment == ContentAlignment.MiddleCenter || _alignment == ContentAlignment.MiddleLeft || _alignment == ContentAlignment.MiddleRight)
                y = y - size.Item2 / 2;

            if (_alignment == ContentAlignment.BottomRight || _alignment == ContentAlignment.MiddleRight || _alignment == ContentAlignment.TopRight)
                x = x - size.Item1;
            if (_alignment == ContentAlignment.BottomCenter || _alignment == ContentAlignment.MiddleCenter || _alignment == ContentAlignment.TopCenter)
                x = x - size.Item1 / 2;

            writeableBitmap.DrawString(_text, x, y, _textRenderer, _orientation);
                
        }

        public override Rect GetModelBounds() {
            return _getBoundsRect.Invoke(new Rect(_mx, _my, 0, 0));
        }

        public override ChartElementHitTestResult HitTest(Point modelPoint) {
            return new ChartElementHitTestResult(false);//hittest not supported
        }
    }
}
