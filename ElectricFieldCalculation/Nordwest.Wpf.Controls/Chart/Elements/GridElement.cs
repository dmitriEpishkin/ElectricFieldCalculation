
using System;
using System.Windows;
using System.Windows.Media.Imaging;
using Nordwest.Wpf.Controls.Properties;

namespace Nordwest.Wpf.Controls.Chart {
    public class GridElement : ChartElement {

        private readonly float[,] _data;
        private readonly float[] _x;
        private readonly float[] _y;
        private readonly ColorScale _colorScale;
        private Rect _modelRect;

        // кешируемая часть
        private WriteableBitmap _bmp; // все данные
        private WriteableBitmap _curBmp; // только выбранный фрагмент
        private WriteableBitmap _curResizeBmp; // выбранный фрагмент с учетом масштаба
        private Rect _chartRect; // текущая отображаемая область

        private int[] xx, yy; // 

        public GridElement(float[] x, float[] y, float[,] data, ColorScale colorScale) {
            _data = data;
            _x = x;
            _y = y;
            _modelRect = new Rect(x[0], y[0], x[x.Length - 1] - x[0], y[y.Length - 1] - y[0]);
            _colorScale = colorScale;
        }
        
        public override void Draw(WriteableBitmap writeableBitmap, Rect dirtyModelRect) {

            if (_bmp == null) CreatePrint();

            bool draw = false;

            if (_chartRect != dirtyModelRect) {
                _chartRect = dirtyModelRect;
                CreateCurrentPrint(dirtyModelRect);
                draw = true;
            }
            dirtyModelRect.Intersect(_modelRect);
            var client = Viewport.ModelToClient(dirtyModelRect);
            var x1 = writeableBitmap.CheckX(client.Left);
            var x2 = writeableBitmap.CheckX(client.Right);
            var y1 = writeableBitmap.CheckY(client.Top);
            var y2 = writeableBitmap.CheckY(client.Bottom);
            int wd = x2 - x1;
            int hg = y2 - y1;
            if (wd <= 0 || hg <= 0) return;

            if (_curResizeBmp == null ||
                wd != _curResizeBmp.PixelWidth || hg != _curResizeBmp.PixelHeight)
                draw = true;
            if (draw)
                _curResizeBmp = _curBmp.Resize(wd, hg, WriteableBitmapExtensions.Interpolation.NearestNeighbor);
            writeableBitmap.Blit(new Rect(x1+1, y1+1, wd, hg), _curResizeBmp, new Rect(0, 0, wd, hg));
        }

        private void CreatePrint() {
            _bmp = BitmapFactory.New(_x.Length, _y.Length);

            var px = new byte[_x.Length*_y.Length*4];

            InitializeBorder();

            for (var i = 0; i < _y.Length; i++) {
                for (var j = 0; j < _x.Length; j++) {
                    var z = _data[xx[j], yy[i]];
                    
                    var j2 = Viewport.HorizontalAxis.IsReversed ? _x.Length - 1 - j : j;
                    var i2 = Viewport.VerticalAxis.IsReversed ? _y.Length - 1 - i : i;
                    
                    // превратить в цвет
                    px[4 * (_x.Length * (i2) + (j2))] = _colorScale.Blue(z);
                    px[4 * (_x.Length * (i2) + (j2)) + 1] = _colorScale.Green(z);
                    px[4 * (_x.Length * (i2) + (j2)) + 2] = _colorScale.Red(z);
                    px[4 * (_x.Length * (i2) + (j2)) + 3] = _colorScale.Alpha(z);
                }
            }
            var rect = new Int32Rect(0, 0, _x.Length, _y.Length);
            int stride = rect.Width * _bmp.Format.BitsPerPixel / 8;
            _bmp.WritePixels(rect, px, stride, 0);
        }

        private void CreateCurrentPrint(Rect dirtyModelRect) {
            var rect = Rect.Intersect(dirtyModelRect, _modelRect);
            if (rect.IsEmpty)
                return;
            var model = new Rect(_modelRect.Location, _modelRect.Size);
            if (Viewport.HorizontalAxis is Log10Axis) {
                var x = Math.Log10(rect.Left);
                rect = new Rect(x, rect.Top, Math.Log10(rect.Right) - x, rect.Height);
                x = Math.Log10(model.Left);
                model = new Rect(x, model.Top, Math.Log10(model.Right) - x, model.Height);
            }
            if (Viewport.VerticalAxis is Log10Axis) {
                var y = Math.Log10(rect.Top);
                rect = new Rect(rect.Left, y, rect.Width, Math.Log10(rect.Bottom) - y);
                y = Math.Log10(model.Top);
                model = new Rect(model.Left, y, model.Width, Math.Log10(model.Bottom) - y);
            }

            var x1 = (int)((rect.Left - model.Left)/model.Width*_bmp.PixelWidth);
            var x2 = (int)((rect.Right - model.Left)/model.Width*_bmp.PixelWidth);
            var y1 = (int) ((rect.Top - model.Top)/model.Height*_bmp.PixelHeight);
            var y2 = (int) ((rect.Bottom - model.Top)/model.Height*_bmp.PixelHeight);
            if (x1 < 0) x1 = 0;
            if (x2 > _bmp.PixelWidth) x2 = _bmp.PixelWidth;
            if (x2 <= x1) x2 = x1 + 1; 
            if (y1 < 0) y1 = 0;
            if (y2 > _bmp.PixelHeight) y2 = _bmp.PixelHeight;
            if (y2 <= y1) y2 = y1 + 1;
            var x0 = Viewport.HorizontalAxis.IsReversed ? _bmp.PixelWidth - x2 : x1;
            var y0 = Viewport.VerticalAxis.IsReversed ? _bmp.PixelHeight - y2 : y1;
            _curBmp = _bmp.Crop(x0, y0, x2 - x1, y2 - y1);
        }

        private double minX, maxX, minY, maxY, stepX, stepY;
        
        private void InitializeBorder() {

            if (Viewport.HorizontalAxis is Log10Axis) {
                minX = Math.Log10(_x[0]);
                maxX = Math.Log10(_x[_x.Length - 1]);
            }
            else if (Viewport.HorizontalAxis is LinAxis) {
                minX = _x[0];
                maxX = _x[_x.Length - 1];
            }
            else throw new NotSupportedException(Resources.GridElement_GetValue_AxisTypeNotSupported_Exception);

            if (Viewport.VerticalAxis is Log10Axis) {
                minY = Math.Log10(_y[0]);
                maxY = Math.Log10(_y[_y.Length - 1]);
            }
            else if (Viewport.VerticalAxis is LinAxis) {
                minY = _y[0];
                maxY = _y[_y.Length - 1];
            }
            else throw new NotSupportedException(Resources.GridElement_GetValue_AxisTypeNotSupported_Exception);

            stepX = (maxX - minX) / (_x.Length - 1);
            stepY = (maxY - minY) / (_y.Length - 1);

            int x2 = 0;
            xx = new int[_x.Length];
            for (int i = 0; i < _x.Length; i++) {
                var valX = minX + stepX * (i + 0.5);
                var realX = Viewport.HorizontalAxis is Log10Axis ? Math.Pow(10, valX) : valX;
                while (x2 < _x.Length - 1 && _x[x2] < realX) x2++;
                xx[i] = x2;
            }

            int y2 = 0;
            yy = new int[_y.Length];
            for (int i = 0; i < _y.Length; i++) {
                var valY = minY + stepY * (i + 0.5);
                var realY = Viewport.VerticalAxis is Log10Axis ? Math.Pow(10, valY) : valY;
                while (y2 < _y.Length - 1 && _y[y2] < realY) y2++;
                yy[i] = y2;
            }

        }

        public override Rect GetModelBounds() {
            return _modelRect;
        }

        public override ChartElementHitTestResult HitTest(Point modelPoint) {
            return new ChartElementHitTestResult(false);//hittest not supported
        }
    }
}
