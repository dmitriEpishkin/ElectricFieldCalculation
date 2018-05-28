using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Nordwest.Wpf.Controls.Converters
{
    public class MultiDelegateConverter<TValue1, TValue2, TResult> : IMultiValueConverter
    {

        private readonly Func<TValue1, TValue2, TResult> _func;
        private readonly Func<TResult, Tuple<TValue1, TValue2>> _backFunc;

        public MultiDelegateConverter(Func<TValue1, TValue2, TResult> func)
        {
            _func = func;
        }

        public MultiDelegateConverter(Func<TValue1, TValue2, TResult> forwardFunc, Func<TResult, Tuple<TValue1, TValue2>> backwordFunc)
            : this(forwardFunc)
        {
            _backFunc = backwordFunc;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2)
                return DependencyProperty.UnsetValue;

            if (!(values[0] is TValue1))
                return DependencyProperty.UnsetValue;
            var v1 = (TValue1)values[0];

            if (!(values[1] is TValue2))
                return DependencyProperty.UnsetValue;
            var v2 = (TValue2)values[1];

            return _func(v1, v2);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if (_backFunc == null)
                return targetTypes.Select(t=>DependencyProperty.UnsetValue).ToArray();

            if (!(value is TResult))
                return targetTypes.Select(t => DependencyProperty.UnsetValue).ToArray();

            var v1 = (TResult)value;
            var result = _backFunc(v1);
            return new object[] {result.Item1, result.Item2};
        }
    }

    public class MultiDelegateConverter<TValue1, TValue2, TValue3, TResult> : IMultiValueConverter
    {

        private readonly Func<TValue1, TValue2, TValue3, TResult> _func;
        private readonly Func<TResult, Tuple<TValue1, TValue2, TValue3>> _backFunc;

        public MultiDelegateConverter(Func<TValue1, TValue2, TValue3, TResult> func)
        {
            _func = func;
        }

        public MultiDelegateConverter(Func<TValue1, TValue2, TValue3, TResult> forwardFunc, Func<TResult, Tuple<TValue1, TValue2, TValue3>> backwordFunc)
            : this(forwardFunc)
        {
            _backFunc = backwordFunc;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2)
                return DependencyProperty.UnsetValue;

            if (!(values[0] is TValue1))
                return DependencyProperty.UnsetValue;
            var v1 = (TValue1)values[0];

            if (!(values[1] is TValue2))
                return DependencyProperty.UnsetValue;
            var v2 = (TValue2)values[1];

            if (!(values[2] is TValue3))
                return DependencyProperty.UnsetValue;
            var v3 = (TValue3)values[2];

            return _func(v1, v2, v3);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if (_backFunc == null)
                return targetTypes.Select(t => DependencyProperty.UnsetValue).ToArray();

            if (!(value is TResult))
                return targetTypes.Select(t => DependencyProperty.UnsetValue).ToArray();

            var v1 = (TResult)value;
            var result = _backFunc(v1);
            return new object[] { result.Item1, result.Item2, result.Item3 };
        }
    }
}