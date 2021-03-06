using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Nordwest.Wpf.Controls
{
    public abstract class IntervalControlBase : Control
    {

        #region Events
        public static readonly RoutedEvent IntervalChangedEvent = EventManager.RegisterRoutedEvent("IntervalChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<Interval>), typeof(IntervalControlBase));
        [Category("Behavior")]
        public event RoutedPropertyChangedEventHandler<Interval> IntervalChanged { add { AddHandler(IntervalChangedEvent, value); } remove { RemoveHandler(IntervalChangedEvent, value); } }

        public static readonly RoutedEvent MinimumChangedEvent = EventManager.RegisterRoutedEvent("MinimumChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<double>), typeof(IntervalControlBase));
        [Category("Behavior")]
        public event RoutedPropertyChangedEventHandler<double> MinimumChanged { add { AddHandler(IntervalChangedEvent, value); } remove { RemoveHandler(IntervalChangedEvent, value); } }

        public static readonly RoutedEvent MaximumChangedEvent = EventManager.RegisterRoutedEvent("MaximumChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<double>), typeof(IntervalControlBase));
        [Category("Behavior")]
        public event RoutedPropertyChangedEventHandler<double> MaximumChanged { add { AddHandler(IntervalChangedEvent, value); } remove { RemoveHandler(IntervalChangedEvent, value); } }
        #endregion Events

        public static readonly DependencyProperty MinimumProperty =
                  DependencyProperty.Register("Minimum", typeof(double), typeof(IntervalControlBase),
                          new FrameworkPropertyMetadata(0.0d, FrameworkPropertyMetadataOptions.AffectsRender, OnMinimumChanged), IsValidDoubleValue);
        [Bindable(true), Category("Behavior")]
        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (IntervalControlBase)d;
            //ctrl.CoerceValue(MaximumProperty);
            ctrl.CoerceValue(IntervalProperty);
            ctrl.OnIntervalChanged(ctrl.Interval, ctrl.Interval);
            ctrl.OnMinimumChanged((double)e.OldValue, (double)e.NewValue);
        }
        protected virtual void OnMinimumChanged(double oldMinimum, double newMinimum)
        {
            RaiseEvent(new RoutedPropertyChangedEventArgs<double>(oldMinimum, newMinimum) { RoutedEvent = MinimumChangedEvent });
        }

        public static readonly DependencyProperty MaximumProperty =
                DependencyProperty.Register("Maximum", typeof(double), typeof(IntervalControlBase),
                        new FrameworkPropertyMetadata(10.0d, FrameworkPropertyMetadataOptions.AffectsRender , OnMaximumChanged, CoerceMaximum), IsValidDoubleValue);
        private static object CoerceMaximum(DependencyObject d, object value)
        {
            var ctrl = (IntervalControlBase)d;
            double min = ctrl.Minimum;
            if ((double)value < min)
            {
                return min;
            }
            return value;
        }

        [Bindable(true), Category("Behavior")]
        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (IntervalControlBase)d;
            ctrl.CoerceValue(IntervalProperty);
            ctrl.OnIntervalChanged(ctrl.Interval, ctrl.Interval);
            ctrl.OnMaximumChanged((double)e.OldValue, (double)e.NewValue);
        }
        protected virtual void OnMaximumChanged(double oldMaximum, double newMaximum)
        {
            RaiseEvent(new RoutedPropertyChangedEventArgs<double>(oldMaximum, newMaximum){RoutedEvent = MaximumChangedEvent});
        }

        public static readonly DependencyProperty IntervalProperty =
                DependencyProperty.Register("Interval", typeof(Interval), typeof(IntervalControlBase),
                new FrameworkPropertyMetadata(new Interval(0.0d, 10.0d), FrameworkPropertyMetadataOptions.AffectsRender, OnIntervalChanged, ConstrainToRange), IsValidIntervalValue);

        // made this internal because Slider wants to leverage it 
        internal static object ConstrainToRange(DependencyObject d, object value)
        {
            var ctrl = (IntervalControlBase)d;
            var min = ctrl.Minimum;
            var max = ctrl.Maximum;
            var v = (Interval)value;
            if (v.Start < min)
            {
                if (min + v.Length < max)
                    return new Interval(min, min + v.Length);
                return new Interval(min, max);
            }

            if (v.End > max)
            {
                if (max - v.Length > min) return new Interval(max - v.Length, max);
                return new Interval(min, max);
            }

            return value;
        }
        
        [Bindable(true), Category("Behavior")]
        public Interval Interval
        {
            get { return (Interval)GetValue(IntervalProperty); }
            set { SetValue(IntervalProperty, value); }
        }
        private static void OnIntervalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (IntervalControlBase)d;

            ctrl.OnIntervalChanged((Interval)e.OldValue, (Interval)e.NewValue);
        }
        protected virtual void OnIntervalChanged(Interval oldValue, Interval newValue)
        {
            RaiseEvent(new RoutedPropertyChangedEventArgs<Interval>(oldValue, newValue){RoutedEvent = IntervalChangedEvent});
        }

        private static bool IsValidIntervalValue(object value)
        {
            var i = (Interval) value;
            return IsValidDoubleValue(i.Start) && IsValidDoubleValue(i.End);
        }
        private static bool IsValidDoubleValue(object value)
        {
            var d = (double)value;

            return !(Double.IsNaN(d) || double.IsInfinity(d));
        }
        private static bool IsValidChange(object value)
        {
            var d = (double)value;

            return IsValidDoubleValue(value) && d >= 0.0;
        }

        //public static readonly DependencyProperty ChangeProperty
        //    = DependencyProperty.Register("Change", typeof(double), typeof(IntervalControlBase),
        //                                  new FrameworkPropertyMetadata(1.0d),IsValidChange);
        //[Bindable(true), Category("Behavior")]
        //public double Change
        //{
        //    get
        //    {
        //        return (double)GetValue(ChangeProperty);
        //    }
        //    set
        //    {
        //        SetValue(ChangeProperty, value);
        //    }
        //}

        #region Method Overrides

        
        public override string ToString()
        {
            string typeText = this.GetType().ToString();
            double min = double.NaN;
            double max = double.NaN;
            Interval val = new Interval();
            bool valuesDefined = false;

            // Accessing RangeBase properties may be thread sensitive
            if (CheckAccess())
            {
                min = Minimum;
                max = Maximum;
                val = Interval;
                valuesDefined = true;
            }
            else
            {
                //Not on dispatcher, try posting to the dispatcher with 20ms timeout 
                Dispatcher.Invoke(DispatcherPriority.Send, new TimeSpan(0, 0, 0, 0, 20), new DispatcherOperationCallback(delegate
                                                                                                                             {
                                                                                                                                 min = Minimum;
                                                                                                                                 max = Maximum;
                                                                                                                                 val = Interval;
                                                                                                                                 valuesDefined = true;
                                                                                                                                 return null;
                                                                                                                             }), null);
            }

            // If min, max, value are defined
            if (valuesDefined)
            {
                return string.Format(@"{0}(min={1}, max={2}, interval={3})", typeText, min, max, val);
            }

            // Not able to access the dispatcher
            return typeText;
        }

        #endregion 
    }
}