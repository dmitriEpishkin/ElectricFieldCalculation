using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using Nordwest.Wpf.Controls.Properties;

namespace Nordwest.Wpf.Controls
{
    public class Palette
    {
        private readonly List<PaletteElement> _palette = new List<PaletteElement>{
            new PaletteElement(0, Color.FromRgb(0, 0, 0)), 
            new PaletteElement(30,Color.FromRgb(100, 100, 100)), 
            new PaletteElement(50,Color.FromRgb(150, 150, 150)), 
            new PaletteElement(70,Color.FromRgb(200, 200, 200)), 
            new PaletteElement(100,Color.FromRgb(255, 255, 255))
        };
        private double[] _valuesCache;

        public Palette()
        {
            _valuesCache = _palette.Select(p => p.Value).ToArray();
        }

        public ReadOnlyCollection<PaletteElement> PaletteElements
        {
            get { return _palette.AsReadOnly(); }
        }

        public Color GetColor(double value)
        {
            return GetPalleteElement(value).Color;
        }
        public Brush GetBrush(double value)
        {
            return GetPalleteElement(value).Brush;
        }
        public int GetIntColor(double value)
        {
            return GetPalleteElement(value).IntColor;
        }
        public PaletteElement GetPalleteElement(double value)
        {
            var res = Array.BinarySearch(_valuesCache, value);
            if (res < 0) res = (~res) - 1;
            return res >= _palette.Count ? new PaletteElement(float.NaN, Colors.Magenta) : _palette[res];
        }

        
        public void SetPalette(IList<double> values, IList<Color> colors)
        {
            if (values.Count != colors.Count)
                throw new ArgumentException(Resources.Palette_SetPalette_NotSameCount_ArgumentException);
            _palette.Clear();
            for (var i = 0; i < values.Count; i++)
                _palette.Add(new PaletteElement(values[i], colors[i]));
            _valuesCache = _palette.Select(p => p.Value).ToArray();

            OnPaletteChanged();
        }

        public event EventHandler PaletteChanged;

        public void OnPaletteChanged()
        {
            EventHandler handler = PaletteChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        private int _selectedIndex = -1;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                if (_selectedIndex == value) return;
                _selectedIndex = value;
                OnSelectedIndexChanged();
            }
        }

        public event EventHandler SelectedIndexChanged;

        public void OnSelectedIndexChanged()
        {
            EventHandler handler = SelectedIndexChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public struct PaletteElement
        {
            private readonly double _value;
            private readonly Color _color;
            private readonly Brush _brush;
            private readonly int _intColor;

            public PaletteElement(double value, Color color)
                : this()
            {
                _value = value;
                _color = color;
                _brush = new SolidColorBrush(color);
                _intColor = Helpers.ConvertColor(color);
            }

            public double Value
            {
                get { return _value; }
            }

            public Color Color
            {
                get { return _color; }
            }

            public Brush Brush
            {
                get { return _brush; }
            }

            public int IntColor
            {
                get { return _intColor; }
            }
        } 
    }
}