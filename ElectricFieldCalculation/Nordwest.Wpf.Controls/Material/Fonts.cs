using System;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

namespace Nordwest.Wpf.Controls.Material
{
    //пока Roboto не впиливаю
    public static class Fonts
    {
        public static readonly FontFamily Roboto;
        static Fonts()
        {
            //var assemblyName = Assembly.GetExecutingAssembly().FullName;
            //Roboto = new FontFamily(new Uri(string.Format(@"pack://application:,,,/{0};Fonts", assemblyName), UriKind.Absolute), "./#Roboto");
            Roboto = new FontFamily(@"Segoe UI");
        }

        public static readonly FontProperties Display4 = new FontProperties(Roboto, FontStyles.Normal, FontWeights.Light, FontStretches.Normal, 112);
        public static readonly FontProperties Display3 = new FontProperties(Roboto, FontStyles.Normal, FontWeights.Regular, FontStretches.Normal, 56);
        public static readonly FontProperties Display2 = new FontProperties(Roboto, FontStyles.Normal, FontWeights.Regular, FontStretches.Normal, 45);
        public static readonly FontProperties Display1 = new FontProperties(Roboto, FontStyles.Normal, FontWeights.Regular, FontStretches.Normal, 34);
        public static readonly FontProperties Headline = new FontProperties(Roboto, FontStyles.Normal, FontWeights.Regular, FontStretches.Normal, 24);
        public static readonly FontProperties Title = new FontProperties(Roboto, FontStyles.Normal, FontWeights.Medium, FontStretches.Normal, 20);
        public static readonly FontProperties Subhead = new FontProperties(Roboto, FontStyles.Normal, FontWeights.Regular, FontStretches.Normal, 15);
        public static readonly FontProperties Body2 = new FontProperties(Roboto, FontStyles.Normal, FontWeights.Medium, FontStretches.Normal, 13);
        public static readonly FontProperties Body1 = new FontProperties(Roboto, FontStyles.Normal, FontWeights.Regular, FontStretches.Normal, 13);
        public static readonly FontProperties Caption = new FontProperties(Roboto, FontStyles.Normal, FontWeights.Regular, FontStretches.Normal, 12);
        public static readonly FontProperties BUTTON = new FontProperties(Roboto, FontStyles.Normal, FontWeights.Medium, FontStretches.Normal, 14);
    }

    public struct FontProperties
    {
        public FontFamily Family { get; set; }
        public FontStyle Style { get; set; }
        public FontWeight Weight { get; set; }
        public FontStretch Stretch { get; set; }
        public double Size { get; set; }

        public FontProperties(FontFamily family, FontStyle style, FontWeight weight, FontStretch stretch, double size)
            : this()
        {
            Family = family;
            Style = style;
            Weight = weight;
            Stretch = stretch;
            Size = size;
        }
    }
}