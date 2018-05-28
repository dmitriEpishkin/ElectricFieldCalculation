using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Nordwest.Wpf.Controls.Chart.Elements {
    /// <summary>
    /// Набор цветных отрезков: число отрезков N отображается по одной оси
    /// по другой оси отображаются отрезки, ограниченные значениями xMin и xMax.
    /// Каждой точке отрезка сопоставляется числовое значение, которому в свою очередь сопоставляется цвет
    /// 
    /// Использую для отображения оценок качества в мульти-ремоуте
    /// </summary>
    public class ColoredLinesChartElement: ChartElement {

        private readonly ColorScale _colorScale;

        /// <summary>
        /// Набор полос с номерами от 1 до N. 
        /// Заданы словарями в которых key = x, value = значение в точке x
        /// </summary>
        private readonly List<SortedDictionary<float, float>> _lines; 

        private readonly Rect _modelRect;

        public ColoredLinesChartElement(List<SortedDictionary<float, float>> lines, ColorScale colorScale) {
            _lines = lines;
            _colorScale = colorScale;
            
            var x0 = lines.Min(x => x.First().Key);
            const int y0 = 0;

            var xN = lines.Max(x => x.Last().Key);
            var yN = lines.Count + 2;
            
            _modelRect = new Rect(x0, y0, xN - x0, yN - y0);
        }
        
        /// <summary>
        /// Не оптимизированно для случая интерактивного масштабирования!!
        /// </summary>
        public override void Draw(WriteableBitmap writeableBitmap, Rect dirtyModelRect) {

            dirtyModelRect.Intersect(_modelRect);
            var client = Viewport.ModelToClient(dirtyModelRect);
            var x1 = writeableBitmap.CheckX(client.Left);
            var x2 = writeableBitmap.CheckX(client.Right);
            var y1 = writeableBitmap.CheckY(client.Top);
            var y2 = writeableBitmap.CheckY(client.Bottom);
            int wd = x2 - x1;
            int hg = y2 - y1;
            if (wd <= 0 || hg <= 0) return;

            // нарисовать все полоски
            for (int i = 0; i < _lines.Count; i++) {
                var line = _lines[i];
                var lineX = (line.Last().Key - line.First().Key);
                var lineModelBounds = Viewport.ModelToClient(new Rect(line.First().Key, i + 1 - 0.3, lineX, 0.6));

                DrawLine(writeableBitmap, 
                    (int)lineModelBounds.Top, (int)lineModelBounds.Height, 
                    (int)lineModelBounds.Left, (int)lineModelBounds.Right, 
                    line);
            }

        }

        /// <summary>
        /// Нарисовать один отрезок
        /// </summary>
        /// <param name="writeableBitmap">Битмат для рисования</param>
        /// <param name="y0">Высота, на которой расположен отрезок</param>
        /// <param name="thickness">Толщина отрезка</param>
        /// <param name="start">Начало отрезка в пикселях</param>
        /// <param name="end">Конец отрезка в пикселях</param>
        /// <param name="line">Данные для отрисовки</param>
        private void DrawLine(WriteableBitmap writeableBitmap, int y0, int thickness, int start, int end, SortedDictionary<float, float> line) {

            var data = PrepareDataToDraw(start, end, line);

            Contract.Assert(data.Length == end - start);

            var px = CreateByteArray(data, thickness);

            var rect = new Int32Rect(start, y0, data.Length, thickness);
            int stride = rect.Width * writeableBitmap.Format.BitsPerPixel / 8;
            writeableBitmap.WritePixels(rect, px, stride, 0);

        }

        private float[] PrepareDataToDraw(int start, int end, SortedDictionary<float, float> line) {
            var x0 = line.First().Key;
            var realWidth = line.Last().Key - x0;

            var normX = new float[line.Count];
            var normY = new float[line.Count];

            int counter = 0;
            foreach (var k in line) {
                normX[counter] = (float)GetClientX(k.Key); // - x0) / realWidth * clentWidth;
                normY[counter] = k.Value;
                counter++;
            }

            var data = new float[end - start];

            for (int i = start; i < end; i++) {

                var x2 = Array.FindIndex(normX, x => x - i >= 0);
                var x1 = x2 - 1;

                if (Math.Abs(normX[x2] - i) < 0.0001) // разрешение в 10 тыс. точек - с запасом
                    data[i - start] = normY[x2];
                else
                    data[i - start] = (normY[x1] * (normX[x2] - i) + normY[x2] * (i - normX[x1])) / (normX[x2] - normX[x1]);
            }
            return data;
        }

        private byte[] CreateByteArray(float[] data, int thickness) {

            var px = new byte[4*thickness*data.Length];

            for (int i = 0; i < data.Length; i++) {

                var b = _colorScale.Blue(data[i]);
                var g = _colorScale.Green(data[i]);
                var r = _colorScale.Red(data[i]);
                var a = _colorScale.Alpha(data[i]);

                for (int j = 0; j < thickness; j++) {
                    px[4*(i + data.Length*j)] = b;
                    px[4 * (i + data.Length * j) + 1] = g;
                    px[4 * (i + data.Length * j) + 2] = r;
                    px[4 * (i + data.Length * j) + 3] = a;
                }
            }
            return px;
        }

        public override Rect GetModelBounds() {
            return _modelRect;
        }

        public override ChartElementHitTestResult HitTest(Point modelPoint) {
            throw new NotImplementedException();
        }
    }

}
