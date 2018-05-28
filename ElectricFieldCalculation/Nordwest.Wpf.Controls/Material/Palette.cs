using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Nordwest.Wpf.Controls.Material
{
    public class Palette
    {
        public static readonly Color PrimaryColorItemsLight = Color.FromArgb(222, 0, 0, 0);
        public static readonly Color SecondaryColorItemsLight = Color.FromArgb(137, 0, 0, 0);
        public static readonly Color HintAndDisabledColorItemsLight = Color.FromArgb(65, 0, 0, 0);
        public static readonly Color DividersColorItemsLight = Color.FromArgb(31, 0, 0, 0);

        public static readonly Color PrimaryColorItemsDark = System.Windows.Media.Colors.White;
        public static readonly Color SecondaryColorItemsDark = Color.FromArgb(179, 255, 255, 255);
        public static readonly Color HintAndDisabledColorItemsDark = Color.FromArgb(76, 255, 255, 255);
        public static readonly Color DividersColorItemsDark = Color.FromArgb(31, 255, 255, 255);

        public static readonly Color FlatButtonNormalColorLight = System.Windows.Media.Colors.Transparent;
        public static readonly Color FlatButtonHoverColorLight = Color.FromArgb(50, 153, 153, 153);
        public static readonly Color FlatButtonPressedColorLight = Color.FromArgb(102, 153, 153, 153);
        public static readonly Color FlatButtonDisabledColorLight = System.Windows.Media.Colors.Transparent;

        public static readonly Color FlatButtonNormalColorDark = System.Windows.Media.Colors.Transparent;
        public static readonly Color FlatButtonHoverColorDark = Color.FromArgb(39, 204, 204, 204);
        public static readonly Color FlatButtonPressedColorDark = Color.FromArgb(63, 204, 204, 204);
        public static readonly Color FlatButtonDisabledColorDark = System.Windows.Media.Colors.Transparent;

        private Dictionary<string, Color> _colors = new Dictionary<string, Color>()
        {
            {"50",  Color.FromRgb(236,239,241)},
            {"100", Color.FromRgb(207,216,220)},
            {"200", Color.FromRgb(176,190,197)},
            {"300", Color.FromRgb(144,164,174)},
            {"400", Color.FromRgb(120,144,156)},
            {"500", Color.FromRgb(96,125,139)},
            {"600", Color.FromRgb(84,110,122)},
            {"700", Color.FromRgb(69,90,100)},
            {"800", Color.FromRgb(55,71,79)},
            {"900", Color.FromRgb(38,50,56)},

            {"A100", Color.FromRgb(255,229,127)},
            {"A200", Color.FromRgb(255,215,64)},
            {"A400", Color.FromRgb(255,196,0)},
            {"A700", Color.FromRgb(255,171,0)},
            //{"A100", Color.FromRgb(255,209,128)},
            //{"A200", Color.FromRgb(255,171,64)},
            //{"A400", Color.FromRgb(255,145,0)},
            //{"A700", Color.FromRgb(255,109,0)},
        };

        private Dictionary<string, bool> _isLight = new Dictionary<string, bool>()
        {
            {"50",  true},
            {"100", true},
            {"200", true},
            {"300", true},
            {"400", false},
            {"500", false},
            {"600", false},
            {"700", false},
            {"800", false},
            {"900", false},

            {"A100", true},
            {"A200", true},
            {"A400", true},
            {"A700", true},
        };

        public Dictionary<string, Color> Colors
        {
            get { return _colors; }
        }

        public Dictionary<string, bool> IsLight
        {
            get { return _isLight; }
        }

        public static readonly DependencyProperty PaletteProperty = DependencyProperty.RegisterAttached(
            "Palette", typeof (Palette), typeof (Palette), new FrameworkPropertyMetadata(default(Palette), FrameworkPropertyMetadataOptions.Inherits));

        public static void SetPalette(DependencyObject element, Palette value)
        {
            element.SetValue(PaletteProperty, value);
        }

        public static Palette GetPalette(DependencyObject element)
        {
            return (Palette)element.GetValue(PaletteProperty);
        }
    }
}