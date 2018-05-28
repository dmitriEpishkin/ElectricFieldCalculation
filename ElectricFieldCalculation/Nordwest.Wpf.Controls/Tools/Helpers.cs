using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Nordwest.Wpf.Controls
{
    public static class Helpers
    {

        #region WritableBitmap Extensions
        public static int CheckX(this WriteableBitmap bmp, double x)
        {
            if (double.IsPositiveInfinity(x)) return bmp.PixelWidth - 1;
            if (double.IsNegativeInfinity(x)) return 0;
            if (x < 0) x = 0;
            if (x > bmp.PixelWidth - 1) x = bmp.PixelWidth - 1;
            return (int)Math.Round(x, MidpointRounding.AwayFromZero);
        }
        public static int CheckY(this WriteableBitmap bmp, double y)
        {
            if (double.IsPositiveInfinity(y)) return bmp.PixelHeight - 1;
            if (double.IsNegativeInfinity(y)) return 0;
            if (y < 0) y = 0;
            if (y > bmp.PixelHeight - 1) y = bmp.PixelHeight - 1;
            return (int)Math.Round(y, MidpointRounding.AwayFromZero);
        }
        public static int ConvertColor(Color color)
        {
            var a = color.A + 1;
            var col = (color.A << 24)
                     | ((byte)((color.R * a) >> 8) << 16)
                     | ((byte)((color.G * a) >> 8) << 8)
                     | ((byte)((color.B * a) >> 8));
            return col;
        }
        
        public static void Offset(this WriteableBitmap bmp, int x, int y)
        {
            var source = BitmapFactory.New(bmp.PixelWidth, bmp.PixelHeight);
            source.Blit(new Rect(0, 0, bmp.PixelWidth, bmp.PixelHeight), bmp, new Rect(0, 0, bmp.PixelWidth, bmp.PixelHeight));
            var destRect = new Rect(x > 0 ? x : 0, y > 0 ? y : 0, bmp.PixelWidth - x, bmp.PixelHeight - y);
            var sourceRect = new Rect(x < 0 ? -x : 0, y < 0 ? -y : 0, bmp.PixelWidth - x, bmp.PixelHeight - y);
            bmp.Blit(destRect, source, sourceRect);
        }
        #endregion

        public static double GetBrightness(this Color c)
        {
            return Math.Sqrt(
               c.R * c.R * .241 +
               c.G * c.G * .691 +
               c.B * c.B * .068) / 256.0;
        }

        public static Color GetContrastColor(this Color c)
        {
            return c.GetBrightness() < 0.3
                                    ? Colors.White
                                    : Colors.Black;
        }

        public static Rect RectFromLTRB(double left, double top, double right, double bottom)
        {
            double x;
            double w;
            if (left < right)
            {
                x = left;
                w = right - left;
            }
            else
            {
                x = right;
                w = left - right;
            }
            double y;
            double h;
            if (top < bottom)
            {
                y = top;
                h = bottom - top;
            }
            else
            {
                y = bottom;
                h = top - bottom;
            }
            return new Rect(x, y, w, h);
        }

        public static bool HasNaNValues(this Rect rect)
        {
            return double.IsNaN(rect.X) || double.IsInfinity(rect.X) ||
                   double.IsNaN(rect.Y) || double.IsInfinity(rect.Y) ||
                   double.IsNaN(rect.Width) || double.IsInfinity(rect.Width) ||
                   double.IsNaN(rect.Height) || double.IsInfinity(rect.Height);
        }

        public static T FindLogicalAncestor<T>(this DependencyObject dependencyObject) where T : class {
            do {
                var buf = dependencyObject;
                dependencyObject = LogicalTreeHelper.GetParent(dependencyObject);
                if (dependencyObject == null)
                    dependencyObject = VisualTreeHelper.GetParent(buf);
            } while (dependencyObject != null && !(dependencyObject is T));
            return dependencyObject as T;
        }
    }
}

