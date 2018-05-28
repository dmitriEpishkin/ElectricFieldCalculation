using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Nordwest.Wpf.Controls.Converters {
    [ValueConversion(typeof(TimeSpan), typeof(double))]
    public class TimeSpanToDoubleConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            TimeSpan givenValue = (TimeSpan)value;
            return givenValue.Ticks;
        }

        public object ConvertBack(object value, Type targetType,
           object parameter, System.Globalization.CultureInfo culture) {
            return new TimeSpan(((long)value));
        }
    }
}
