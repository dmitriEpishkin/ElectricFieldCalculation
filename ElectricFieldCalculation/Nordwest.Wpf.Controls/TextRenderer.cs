using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Linq;

namespace Nordwest.Wpf.Controls
{
    public class TextRenderer
    {
        private const int _stringCacheSize = 1000;
        private const int _glyphCachSize = 255;

        private readonly ConcurrentDictionary<string, WriteableBitmap> _cache = new ConcurrentDictionary<string, WriteableBitmap>(1, _stringCacheSize);
        ConcurrentDictionary<char, WriteableBitmap> _glyphs = new ConcurrentDictionary<char, WriteableBitmap>(1, _glyphCachSize);
        private readonly bool _useGliphRenderer = false;

        private double _size = 12;
        private Color _color = Colors.Black;
        private Typeface _typeface = new Typeface(@"Verdana");
        private bool _useCache = true;

        public double Size
        {
            get { return _size; }
            set
            {
                if (_size == value) return;
                _size = value;
                ClearCaches();
            }
        }
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }
        public Typeface Typeface
        {
            get { return _typeface; }
            set
            {
                if (_typeface == value) return;
                _typeface = value;
                ClearCaches();
            }
        }
        public bool UseCache
        {
            get { return _useCache; }
            set { _useCache = value; }
        }

        public TextRenderer() : this(false) { }
        public TextRenderer(bool useGliphRenderer)
        {
            _useGliphRenderer = useGliphRenderer;
            if (useGliphRenderer)
            {
                PrepareGlyphs();
            }
        }

        public void ClearCaches()
        {
            _cache.Clear();
            _glyphs.Clear();
        }
        
        private void PrepareGlyphs()
        {
            var g = new Dictionary<char, WriteableBitmap>(_glyphCachSize);
            for (int i = 0; i < 9; i++)
            {
                var key = i.ToString().ToCharArray()[0];
                g.Add(key, Render(i.ToString()));
            }
            g.Add('.', Render(@"."));
            _glyphs = new ConcurrentDictionary<char, WriteableBitmap>(g);
        }
        /// <summary>
        /// Возвращает размер отрендеренной строки. Если useGliphRenderer=true, быстро, если нет, рендерит всю строку(правда кеширует ее)
        /// </summary>
        /// <param name="text"></param>
        /// <param name="orientation"></param>
        /// <returns></returns>
        public Tuple<int, int> MeasureString(string text, Orientation orientation = Orientation.Horizontal)
        {
            if (_useGliphRenderer)
            {
                var chars = text.ToCharArray();
                var width = 0;
                var heigth = 0;
                foreach (var c in chars)
                {
                    var glyph = _glyphs.GetOrAdd(c, s => Render(s.ToString()));
                    if (orientation == Orientation.Horizontal)
                    {
                        width += glyph.PixelWidth;
                        heigth = Math.Max(heigth, glyph.PixelHeight);
                    }
                    else
                    {
                        heigth += glyph.PixelHeight;
                        width = Math.Max(heigth, glyph.PixelWidth);
                    }
                }
                return new Tuple<int, int>(width, heigth);
            }
            else
            {
                var textBmp = GetTextBitmap(text, orientation);
                return new Tuple<int, int>(textBmp.PixelWidth, textBmp.PixelHeight);
            }
        }

        private WriteableBitmap Render(string text, Orientation orientation = Orientation.Horizontal)
        {
            var ftext = new FormattedText(text, CultureInfo.CurrentCulture,
                                                                                 FlowDirection.LeftToRight,
                                                                                 Typeface, Size,
                                                                                 new SolidColorBrush(Color));
            var drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            drawingContext.DrawText(ftext, new Point(2, 2));
            drawingContext.Close();
            Rect bounds;
            if (text == @" ") 
                bounds = new Rect(0, 0, ftext.Width, ftext.Height);
            else 
                bounds = drawingVisual.ContentBounds;
            var rtb = new RenderTargetBitmap((int)bounds.Right + 1,
                                             (int)bounds.Bottom + 1, 96, 96,
                                             PixelFormats.Pbgra32);
            rtb.Render(drawingVisual);

            var bmp = new WriteableBitmap(rtb);
            if (orientation == Orientation.Vertical)
                bmp = bmp.Rotate(270);
            return bmp;
        }

        private WriteableBitmap RenderGlyphs(string text, Orientation orientation)
        {
            var chars = text.ToCharArray();
            var width = 0;
            var height = 0;
            var glyphlist = new List<WriteableBitmap>(text.Length);
            foreach (var c in chars)
            {
                var glyph = _glyphs.GetOrAdd(c, s => Render(s.ToString()));
                width += glyph.PixelWidth;
                height = Math.Max(height, glyph.PixelHeight);
                glyphlist.Add(glyph);
            }

            var bmp = BitmapFactory.New(width, height);

            var next = 0;
            using (bmp.GetBitmapContext())
            {
                foreach (var g in glyphlist)
                {
                    bmp.Blit(new Point(next, 0), g, new Rect(0, 0, g.PixelWidth, g.PixelHeight), Colors.White, WriteableBitmapExtensions.BlendMode.Alpha);
                    next += g.PixelWidth;
                }
            }
            if (orientation == Orientation.Vertical)
                bmp = bmp.Rotate(270);
            return bmp;
        }

        
        public WriteableBitmap GetTextBitmap(string text, Orientation orientation = Orientation.Horizontal)
        {
            if (_cache.Count >= 1000)
            {
                var key = _cache.First().Key;
                WriteableBitmap unused;
                _cache.TryRemove(key, out unused);
            }

            if (_useGliphRenderer)
            {
                return _useCache
                           ? _cache.GetOrAdd(text, s => Render(s, orientation))
                           : RenderGlyphs(text, orientation);
            }
            else
            {
                return _useCache
                           ? _cache.GetOrAdd(text, s => Render(s, orientation))
                           : Render(text, orientation);
            }
        }
    }

    public static class WriteableBitmapTextRender
    {
        public static void DrawString(this WriteableBitmap bmp,
           string text,
           double x,
           double y,
           TextRenderer textRenderer,
           Orientation orientation = Orientation.Horizontal)
        {
            var textBmp = textRenderer.GetTextBitmap(text, orientation);
            bmp.Blit(
               new Point(x, y),
               textBmp,
               new Rect(0, 0, textBmp.PixelWidth, textBmp.PixelHeight),
               Colors.White,
               WriteableBitmapExtensions.BlendMode.Alpha);
        }
    }
}