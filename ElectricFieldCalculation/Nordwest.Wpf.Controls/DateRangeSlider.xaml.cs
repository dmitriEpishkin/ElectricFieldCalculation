using System;
using System.Windows;
using System.Windows.Controls;

namespace Nordwest.Wpf.Controls {

    /// <summary>
    /// http://wpfdaterangeslider.codeplex.com/
    /// Interaction logic for RangeSlider.xaml
    /// </summary>
    public partial class DateRangeSlider : UserControl {
        private long oneSecond = 10000000L;
        public DateRangeSlider() {
            InitializeComponent();

            this.Loaded += Slider_Loaded;
        }

        protected void Slider_Loaded(object sender, RoutedEventArgs e) {
            LowerSlider.ValueChanged += LowerSlider_ValueChanged;
            UpperSlider.ValueChanged += UpperSlider_ValueChanged;
        }

        private void LowerSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if (IsUpperSliderEnabled) {
                UpperSlider.Value = Math.Max(UpperSlider.Value, LowerSlider.Value + oneSecond);

                var _upperValue = new DateTime((long)UpperSlider.Value);
                var _lowerValue = new DateTime((long)LowerSlider.Value);

                if (UpperValue > _lowerValue) {
                    LowerValue = _lowerValue;
                    UpperValue = _upperValue;
                }
                else {
                    UpperValue = _upperValue;
                    LowerValue = _lowerValue;
                }
            }
            else {
                var _lowerValue = new DateTime((long)e.NewValue);

                if (_lowerValue > UpperValue) {
                    LowerSlider.Value = e.OldValue;
                    LowerValue = new DateTime((long)LowerSlider.Value);
                }
                else {
                    LowerValue = _lowerValue;
                }
            }
        }

        private void UpperSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            LowerSlider.Value = Math.Max(Math.Min(UpperSlider.Value - oneSecond, LowerSlider.Value), Minimum.Ticks);

            var _upperValue = new DateTime((long)UpperSlider.Value);
            var _lowerValue = new DateTime((long)LowerSlider.Value);

            if (UpperValue > _lowerValue) {
                LowerValue = _lowerValue;
                UpperValue = _upperValue;
            }
            else {
                UpperValue = _upperValue;
                LowerValue = _lowerValue;
            }
        }

        #region Dependency Property - IsUpperSliderEnabled
        public bool IsUpperSliderEnabled {
            get { return (bool)GetValue(IsUpperSliderEnabledProperty); }
            set { SetValue(IsUpperSliderEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsUpperSliderEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsUpperSliderEnabledProperty =
            DependencyProperty.Register("IsUpperSliderEnabled", typeof(bool), typeof(DateRangeSlider), new UIPropertyMetadata(true));
        #endregion

        #region Dependency Property - IsLowerSliderEnabled
        public bool IsLowerSliderEnabled {
            get { return (bool)GetValue(IsLowerSliderEnabledProperty); }
            set { SetValue(IsLowerSliderEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsLowerSliderEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsLowerSliderEnabledProperty =
            DependencyProperty.Register("IsLowerSliderEnabled", typeof(bool), typeof(DateRangeSlider), new UIPropertyMetadata(true));
        #endregion

        #region Dependency Property - Minimum
        public DateTime Minimum {
            get { return (DateTime)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(DateTime), typeof(DateRangeSlider), new UIPropertyMetadata(DateTime.Now.AddDays(-15)));
        #endregion

        #region Dependency Property - Lower Value
        public DateTime LowerValue {
            get { return (DateTime)GetValue(LowerValueProperty); }
            set { SetValue(LowerValueProperty, value); }
        }

        public static readonly DependencyProperty LowerValueProperty =
            DependencyProperty.Register("LowerValue", typeof(DateTime), typeof(DateRangeSlider), new UIPropertyMetadata(DateTime.Now.AddDays(-7)));
        #endregion

        #region Dependency Property - Upper Value
        public DateTime UpperValue {
            get { return (DateTime)GetValue(UpperValueProperty); }
            set { SetValue(UpperValueProperty, value); }
        }

        public static readonly DependencyProperty UpperValueProperty =
            DependencyProperty.Register("UpperValue", typeof(DateTime), typeof(DateRangeSlider), new UIPropertyMetadata(DateTime.Now.AddDays(7)));

        #endregion

        #region Dependency Property - Maximum
        public DateTime Maximum {
            get { return (DateTime)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(DateTime), typeof(DateRangeSlider), new UIPropertyMetadata(DateTime.Now.AddDays(15), new PropertyChangedCallback(OnMaximumChanged)));

        public static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            DateRangeSlider slider = (DateRangeSlider)d;

            if (slider.IsUpperValueLockedToMax) {
                slider.UpperValue = (DateTime)e.NewValue;
            }
        }

        #endregion

        #region Dependency Property - Small Change
        public TimeSpan SmallChange {
            get { return (TimeSpan)GetValue(SmallChangeProperty); }
            set { SetValue(SmallChangeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SmallChange.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SmallChangeProperty =
            DependencyProperty.Register("SmallChange", typeof(TimeSpan), typeof(DateRangeSlider),
                new UIPropertyMetadata(new TimeSpan(0, 0, 0, 1), new PropertyChangedCallback(OnSmallChangePropertyChanged)));

        protected static void OnSmallChangePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            System.Diagnostics.Debug.WriteLine(e.NewValue);
        }
        #endregion

        #region Dependency Property - Large Change

        public TimeSpan LargeChange {
            get { return (TimeSpan)GetValue(LargeChangeProperty); }
            set { SetValue(LargeChangeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LargeChange.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LargeChangeProperty =
            DependencyProperty.Register("LargeChange", typeof(TimeSpan), typeof(DateRangeSlider),
                    new UIPropertyMetadata(new TimeSpan(0, 0, 1, 0), new PropertyChangedCallback(OnLargeChangePropertyChanged)));

        protected static void OnLargeChangePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            System.Diagnostics.Debug.WriteLine(e.NewValue);
        }
        #endregion

        #region Dependency Property - Lock Upper Value to Max
        public bool IsUpperValueLockedToMax {
            get { return (bool)GetValue(IsUpperValueLockedToMaxProperty); }
            set { SetValue(IsUpperValueLockedToMaxProperty, value); }
        }

        public static readonly DependencyProperty IsUpperValueLockedToMaxProperty =
            DependencyProperty.Register("IsUpperValueLockedToMax", typeof(bool), typeof(DateRangeSlider), new UIPropertyMetadata(false, new PropertyChangedCallback(OnIsUpperValueLockedToMaxChanged)));

        public static void OnIsUpperValueLockedToMaxChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            DateRangeSlider slider = (DateRangeSlider)d;

            if ((bool)e.NewValue) {
                slider.UpperSlider.Value = slider.UpperSlider.Maximum;
                slider.IsUpperSliderEnabled = false;
            }
            else {
                slider.IsUpperSliderEnabled = true;
            }
        }
        #endregion

        #region Dependency Property - Lock Upper Value to Max
        public bool IsLowerValueLockedToMin {
            get { return (bool)GetValue(IsLowerValueLockedToMinProperty); }
            set { SetValue(IsLowerValueLockedToMinProperty, value); }
        }

        public static readonly DependencyProperty IsLowerValueLockedToMinProperty =
            DependencyProperty.Register("IsLowerValueLockedToMin", typeof(bool), typeof(DateRangeSlider), new UIPropertyMetadata(false, new PropertyChangedCallback(OnIsLowerValueLockedToMinChanged)));

        public static void OnIsLowerValueLockedToMinChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            DateRangeSlider slider = (DateRangeSlider)d;

            if ((bool)e.NewValue) {
                slider.LowerSlider.Value = slider.LowerSlider.Minimum;
                slider.IsLowerSliderEnabled = false;
            }
            else {
                slider.IsLowerSliderEnabled = true;
            }
        }
        #endregion

        #region Events

        public static readonly RoutedEvent LowerValueChangedEvent = EventManager.RegisterRoutedEvent("LowerValueChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<DateTime>), typeof(DateRangeSlider));
        public event RoutedPropertyChangedEventHandler<DateTime> LowerValueChanged {
            add { AddHandler(LowerValueChangedEvent, value); }
            remove { RemoveHandler(LowerValueChangedEvent, value); }
        }

        public static readonly RoutedEvent UpperValueChangedEvent = EventManager.RegisterRoutedEvent("UpperValueChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<DateTime>), typeof(DateRangeSlider));
        public event RoutedPropertyChangedEventHandler<DateTime> UpperValueChanged {
            add { AddHandler(UpperValueChangedEvent, value); }
            remove { RemoveHandler(UpperValueChangedEvent, value); }
        }

        #endregion //Events

    }
}
