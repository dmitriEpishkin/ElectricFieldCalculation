using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Windows.Controls.Primitives;

namespace Nordwest.Wpf.Controls
{
    [TemplatePart(Name = @"UpButton", Type = typeof(RepeatButton))]
    [TemplatePart(Name = @"DownButton", Type = typeof(RepeatButton))]
    [TemplatePart(Name = @"TextBox", Type = typeof(TextBox))]
    public class UpDownControl : Control
    {
        private RepeatButton _upBtn;
        private RepeatButton _downBtn;
        private TextBox _textBox;

        static UpDownControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UpDownControl), new FrameworkPropertyMetadata(typeof(UpDownControl)));
        }

        
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _upBtn = (RepeatButton)GetTemplateChild(@"UpButton");
            _downBtn = (RepeatButton)GetTemplateChild(@"DownButton");
            _textBox = (TextBox)GetTemplateChild(@"TextBox");

            if (_upBtn != null) _upBtn.Click += _upBtn_Click;
            if (_downBtn != null) _downBtn.Click += _downBtn_Click;
            if (_textBox != null) _textBox.PreviewTextInput += _textBox_PreviewTextInput;
        }

        
        void _textBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = true;

            decimal newValue;
            if (!decimal.TryParse(_textBox.Text.Insert(_textBox.SelectionStart, e.Text), NumberStyles.Number, NumberFormatInfo.InvariantInfo, out newValue))
                System.Media.SystemSounds.Hand.Play();
            else
                Value = newValue;
        }

        void _downBtn_Click(object sender, RoutedEventArgs e)
        {
            Value -= Step;
        }

        void _upBtn_Click(object sender, RoutedEventArgs e)
        {
            Value += Step;
        }

        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(decimal), typeof(UpDownControl), new PropertyMetadata((decimal)100, OnMaximumChanged, CoerceMaximum));

        public decimal Maximum
        {
            get { return (decimal)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        private static object CoerceMaximum(DependencyObject d, object basevalue)
        {
            var control = (UpDownControl)d;
            var newMaximum = (decimal)basevalue;
            return Math.Max(newMaximum, control.Minimum);

        }

        private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.CoerceValue(ValueProperty);
        }

        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(decimal), typeof(UpDownControl), new PropertyMetadata((decimal)0, OnMinimumChanged, CoerceMinimum));

        public decimal Minimum
        {
            get { return (decimal)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        private static object CoerceMinimum(DependencyObject d, object basevalue)
        {
            //var minimum = (decimal)basevalue;
            //var control = (UpDownControl)d;
            //return minimum;
            return basevalue;
        }

        private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.CoerceValue(MaximumProperty);
            d.CoerceValue(ValueProperty);
        }

        public static readonly DependencyProperty StepProperty =
            DependencyProperty.Register("Step", typeof(decimal), typeof(UpDownControl), new PropertyMetadata((decimal)1));

        public decimal Step
        {
            get { return (decimal)GetValue(StepProperty); }
            set { SetValue(StepProperty, value); }
        }


        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(decimal), typeof(UpDownControl),
              new FrameworkPropertyMetadata((decimal)0, OnValueChanged, CoerceValue));

        public decimal Value
        {
            get { return (decimal)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var control = (UpDownControl)obj;

            var oldValue = (decimal)args.OldValue;
            var newValue = (decimal)args.NewValue;


            var e = new RoutedPropertyChangedEventArgs<decimal>(oldValue, newValue, ValueChangedEvent);

            control.OnValueChanged(e);
        }

        private static object CoerceValue(DependencyObject element, object value)
        {
            var newValue = (decimal)value;
            var control = (UpDownControl)element;

            newValue = Math.Max(control.Minimum, Math.Min(control.Maximum, newValue));
            //newValue = Decimal.Round(newValue, control.DecimalPlaces);

            return newValue;
        }

        protected virtual void OnValueChanged(RoutedPropertyChangedEventArgs<decimal> args)
        {
            RaiseEvent(args);
        }

        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
            "ValueChanged", RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<decimal>), typeof(UpDownControl));

        public event RoutedPropertyChangedEventHandler<decimal> ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }

    }
}