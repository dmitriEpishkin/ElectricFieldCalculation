using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Nordwest.Wpf.Controls.Converters
{
    public class DelegateConverter<TValue, TParam, TResult>:IValueConverter 
    {
        private readonly Func<TValue, TParam, TResult> _func;
        private readonly Func<TResult, TParam, TValue> _backFunc;

        public DelegateConverter(Func<TValue, TParam, TResult> func)
        {
            _func = func;
        }

        public DelegateConverter(Func<TValue, TParam, TResult> forwardFunc, Func<TResult, TParam, TValue> backwordFunc)
            : this(forwardFunc)
        {
            _backFunc = backwordFunc;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is TValue) || !(parameter is TParam))
                return DependencyProperty.UnsetValue;
            var v1 = (TValue)value;
            var v2 = (TParam)parameter;
            return _func(v1, v2);
        }

        
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (_backFunc == null)
                return DependencyProperty.UnsetValue;

            if (!(value is TResult) || !(parameter is TParam))
                return DependencyProperty.UnsetValue;

            var v1 = (TResult)value;
            var v2 = (TParam)parameter;
            return _backFunc(v1, v2);
        }
    }

    public class DelegateConverter<TValue, TResult> : IValueConverter 
    {
        private readonly Func<TValue, TResult> _func;
        private readonly Func<TResult, TValue> _backFunc;

        public DelegateConverter(Func<TValue, TResult> func)
        {
            _func = func;
        }

        public DelegateConverter(Func<TValue,  TResult> forwardFunc, Func<TResult,  TValue> backwordFunc)
            : this(forwardFunc)
        {
            _backFunc = backwordFunc;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is TValue))
                return DependencyProperty.UnsetValue;
            var v1 = (TValue)value;
            return _func(v1);
        }

        
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (_backFunc == null)
                return DependencyProperty.UnsetValue;

            if (!(value is TResult))
                return DependencyProperty.UnsetValue;

            var v1 = (TResult)value;
            return _backFunc(v1);
        }
    }
}