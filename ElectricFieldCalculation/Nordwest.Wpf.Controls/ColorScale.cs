
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace Nordwest.Wpf.Controls {
    // Цветовая шкала
    // сопоставляет любому действительному цвет
    public class ColorScale {

        private List<float> _vals;
        private List<byte> _r;
        private List<byte> _g;
        private List<byte> _b;
        private List<byte> _a;

        public static Dictionary<string, ColorScale> GetDefaultColorScales(float min, float max) {
            var _colorScales = new Dictionary<string, ColorScale>();

            _colorScales.Add(@"Rainbow", Rainbow(min, max));
            _colorScales.Add(@"Rainbow2", Rainbow2(min, max));

            return _colorScales;
        }

        public ColorScale(SortedDictionary<float, Color> anchors) {
            _vals = anchors.Keys.ToList();
            var c = anchors.Values.ToList();
            _r = c.ConvertAll(x => x.R);
            _g = c.ConvertAll(x => x.G);
            _b = c.ConvertAll(x => x.B);
            _a = c.ConvertAll(x => x.A);
        }

        public static ColorScale Default {
            get {
                SortedDictionary<float, Color> c = new SortedDictionary<float, Color>();
                c.Add(0, Colors.White);
                c.Add(1, Colors.Black);
                return new ColorScale(c);
            }
        }

        public static ColorScale BlackWithe(float min, float max) {
            SortedDictionary<float, Color> colors = new SortedDictionary<float, Color>();
            colors.Add(min, Colors.White);
            colors.Add(max, Colors.Black);
            return new ColorScale(colors);
        }

        public static ColorScale Rainbow(float min, float max) {
            SortedDictionary<float, Color> colors = new SortedDictionary<float, Color>();
            colors.Add(min, Colors.Purple);
            colors.Add(min + (max - min) / 6.0f, Colors.DarkBlue);
            colors.Add(min + (max - min) * 2.0f / 6.0f, Colors.Blue);
            colors.Add(min + (max - min) * 3.0f / 6.0f, Colors.Green);
            colors.Add(min + (max - min) * 4.0f / 6.0f, Colors.Yellow);
            colors.Add(min + (max - min) * 5.0f / 6.0f, Colors.Orange);
            colors.Add(max, Colors.Red);

            return new ColorScale(colors);
        }

        public static ColorScale Rainbow2(float min, float max) {
            SortedDictionary<float, Color> colors = new SortedDictionary<float, Color>();
            colors.Add(min, Colors.Black);
            colors.Add(min + (max - min) / 10.0f, Colors.Brown);
            colors.Add(min + (max - min) * 2.0f / 10.0f, Colors.Purple);
            colors.Add(min + (max - min) * 3.0f / 10.0f, Colors.DarkBlue);
            colors.Add(min + (max - min) * 4.0f / 10.0f, Colors.Blue);
            colors.Add(min + (max - min) * 5.0f / 10.0f, Colors.Green);
            colors.Add(min + (max - min) * 6.0f / 10.0f, Colors.Yellow);
            colors.Add(min + (max - min) * 7.0f / 10.0f, Colors.Orange);
            colors.Add(min + (max - min) * 8.0f / 10.0f, Colors.Red);
            colors.Add(min + (max - min) * 9.0f / 10.0f, Colors.Purple);
            colors.Add(max, Colors.AliceBlue);

            return new ColorScale(colors);
        }

        public byte Red(float val) {
            return GetByte(val, _r);
        }
        public byte Green(float val) {
            return GetByte(val, _g);
        }
        public byte Blue(float val) {
            return GetByte(val, _b);
        }
        public byte Alpha(float val) {
            return GetByte(val, _a);
        }
        public Color GetColor(float val) {
            return Color.FromArgb(GetByte(val, _a), GetByte(val, _r), GetByte(val, _g), GetByte(val, _b));
        }

        private byte GetByte(float val, List<byte> b) {
            if (_vals.Count == 0 || _vals[_vals.Count - 1] < val)
                return 255;

            if (_vals[0] > val)
                return 0;

            int i = 1;
            while (_vals.Count > i && _vals[i] < val)
                i++;

            if (_vals.Count == i) {
                if (val == _vals[_vals.Count - 1])
                    return b[_vals.Count - 1];
                return 0;
            }

            float d = _vals[i] - _vals[i - 1];
            float p = val - _vals[i - 1];

            return (byte)(((d - p) * b[i - 1] + p * b[i]) / d);
        }

    }
}
