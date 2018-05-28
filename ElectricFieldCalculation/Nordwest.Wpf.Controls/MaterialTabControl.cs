using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Nordwest.Wpf.Controls
{
    public class MaterialTabControl : TabControl
    {
        private FrameworkElement _marker;
        private Canvas _markerCanvas;
        private ContentPresenter _contentPresenter;

        static MaterialTabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MaterialTabControl), new FrameworkPropertyMetadata(typeof(MaterialTabControl)));
            TabStripPlacementProperty.OverrideMetadata(typeof(MaterialTabControl), new FrameworkPropertyMetadata(Dock.Top, OnTabStripPlacementPropertyChanged));
        }

        public MaterialTabControl()
        {
            BindingOperations.SetBinding(this, SelectedContentOverrideProperty,
                new Binding() {Source = this, Path = new PropertyPath(SelectedContentProperty)});
        }

        private static void OnSelectedContentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tc = (MaterialTabControl)d;
            var newValue = e.NewValue;
            tc.AnimateTransition(newValue);
        }

        // When TabControl TabStripPlacement is changing we need to invalidate its TabItem TabStripPlacement
        private static void OnTabStripPlacementPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tc = (MaterialTabControl)d;

            tc.AnimateMarkerToItem(tc.SelectedItem);

            var tabItemCollection = tc.Items;
            for (var i = 0; i < tabItemCollection.Count; i++)
            {
                var ti = tc.ItemContainerGenerator.ContainerFromIndex(i) as TabItem;
                if (ti != null)
                    ti.CoerceValue(TabItem.TabStripPlacementProperty);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _marker = (FrameworkElement)GetTemplateChild(@"Marker");
            _markerCanvas = (Canvas)GetTemplateChild(@"MarkerCanvas");
            _contentPresenter = (ContentPresenter)GetTemplateChild(@"PART_SelectedContentHost");

            AnimateTransition(SelectedContentOverride);
            AnimateMarkerToItem(SelectedItem);
            _markerCanvas.SizeChanged += (sender, args) => AnimateMarkerToItem(SelectedItem);
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            if(e.AddedItems == null || e.AddedItems.Count <= 0)
                return;

            var item = e.AddedItems[0];
            AnimateMarkerToItem(item);
        }

        private void AnimateMarkerToItem(object item)
        {
            if (_markerCanvas == null || _marker == null)
                return;

            _marker.Visibility = item == null ? Visibility.Collapsed : Visibility.Visible;

            var container = ItemContainerGenerator.ContainerFromItem(item) as FrameworkElement;
            if (container == null)
                return;
            
            var canvasLeftTop = container.TranslatePoint(new Point(0, 0), _markerCanvas);
            var canvasRightBottom = container.TranslatePoint(new Point(container.ActualWidth, container.ActualHeight), _markerCanvas);

            double position;
            double size;
            PropertyPath positionProperty;
            PropertyPath sizeProperty;
            if (TabStripPlacement == Dock.Top || TabStripPlacement == Dock.Bottom)
            {
                position = Math.Min(canvasLeftTop.X, canvasRightBottom.X);
                size = Math.Max(canvasLeftTop.X, canvasRightBottom.X) - position;
                positionProperty = new PropertyPath(Canvas.LeftProperty);
                sizeProperty = new PropertyPath(FrameworkElement.WidthProperty);
            }
            else
            {
                position = Math.Min(canvasLeftTop.Y, canvasRightBottom.Y);
                size = Math.Max(canvasLeftTop.Y, canvasRightBottom.Y) - position;
                positionProperty = new PropertyPath(Canvas.TopProperty);
                sizeProperty = new PropertyPath(FrameworkElement.HeightProperty);
            }

            var positionAnimation = new DoubleAnimation();
            positionAnimation.To = position;
            positionAnimation.EasingFunction = new ExponentialEase() {EasingMode = EasingMode.EaseInOut, Exponent = 3};
            positionAnimation.Duration = TimeSpan.FromSeconds(0.2);
            Storyboard.SetTarget(positionAnimation, _marker);
            Storyboard.SetTargetProperty(positionAnimation, positionProperty);

            var sizeAnimation = new DoubleAnimation();
            sizeAnimation.To = size;
            sizeAnimation.EasingFunction = new ExponentialEase() {EasingMode = EasingMode.EaseInOut, Exponent = 3};
            sizeAnimation.Duration = TimeSpan.FromSeconds(0.2);
            Storyboard.SetTarget(sizeAnimation, _marker);
            Storyboard.SetTargetProperty(sizeAnimation, sizeProperty);

            var storyboard = new Storyboard();
            storyboard.Children.Add(positionAnimation);
            storyboard.Children.Add(sizeAnimation);
            storyboard.Begin();
        }

        private void AnimateTransition(object newContent)
        {
            if (_contentPresenter == null)
                return;

            var fadeOutAnimation = new DoubleAnimation();
            fadeOutAnimation.To = 0;
            fadeOutAnimation.BeginTime = TimeSpan.Zero;
            fadeOutAnimation.EasingFunction = new ExponentialEase() { EasingMode = EasingMode.EaseInOut, Exponent = 2 };
            fadeOutAnimation.Duration = TimeSpan.FromSeconds(0.1);
            
            Storyboard.SetTarget(fadeOutAnimation, _contentPresenter);
            Storyboard.SetTargetProperty(fadeOutAnimation, new PropertyPath(ContentPresenter.OpacityProperty));
            
            var fadeInAnimation = new DoubleAnimation();
            fadeInAnimation.BeginTime = TimeSpan.FromSeconds(0.1);
            fadeInAnimation.To = 1;
            fadeInAnimation.EasingFunction = new ExponentialEase() { EasingMode = EasingMode.EaseInOut, Exponent = 2 };
            fadeInAnimation.Duration = TimeSpan.FromSeconds(0.1);
            Storyboard.SetTarget(fadeInAnimation, _contentPresenter);
            Storyboard.SetTargetProperty(fadeInAnimation, new PropertyPath(ContentPresenter.OpacityProperty));

            EventHandler fadeOutAnimationOnCompleted = null;
            fadeOutAnimationOnCompleted = (sender, args) =>
            {
                _contentPresenter.Content = newContent;
                fadeOutAnimation.Completed -= fadeOutAnimationOnCompleted;
            };
            fadeOutAnimation.Completed += fadeOutAnimationOnCompleted;

            var storyboard = new Storyboard();
            storyboard.Children.Add(fadeOutAnimation);
            storyboard.Children.Add(fadeInAnimation);
            storyboard.Begin();
        }

        public static readonly DependencyProperty SelectedContentOverrideProperty = DependencyProperty.Register(
            "SelectedContentOverride", typeof (object), typeof (MaterialTabControl), new PropertyMetadata(default(object), OnSelectedContentPropertyChanged));

        public object SelectedContentOverride
        {
            get { return (object)GetValue(SelectedContentOverrideProperty); }
            set { SetValue(SelectedContentOverrideProperty, value); }
        }
    }

    public static class MaterialColors
    {
        public static readonly DependencyProperty PrimaryColorProperty = DependencyProperty.RegisterAttached(
            "PrimaryColor", typeof (Color), typeof (MaterialColors), new PropertyMetadata(default(Color)));

        public static void SetPrimaryColor(DependencyObject element, Color value)
        {
            element.SetValue(PrimaryColorProperty, value);
        }

        public static Color GetPrimaryColor(DependencyObject element)
        {
            return (Color)element.GetValue(PrimaryColorProperty);
        }
    }
}