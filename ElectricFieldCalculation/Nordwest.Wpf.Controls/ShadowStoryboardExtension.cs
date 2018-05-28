using System;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Xaml;

namespace Nordwest.Wpf.Controls
{
    [MarkupExtensionReturnType(typeof(Storyboard))]
    public class ShadowStoryboardExtension : MarkupExtension
    {
        private TimeSpan _duration = TimeSpan.FromSeconds(0.2);

        public ShadowStoryboardExtension()
        {
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (SourceShadow == null && TargetShadow == null)
                throw new InvalidOperationException(@"TargetShadow and/or SourceShadow properties must be setted.");
            if (TargetProperty == null)
                throw new InvalidOperationException(@"TargetProperty property must be setted.");

            var sb = new Storyboard();
            var blurAnimation = new DoubleAnimation()
            {
                From = SourceShadow != null ? SourceShadow.BlurRadius : (double?)null,
                To = TargetShadow != null ? TargetShadow.BlurRadius : (double?)null,
                Duration = new Duration(_duration),
                EasingFunction = new ExponentialEase { EasingMode = EasingMode.EaseInOut },
                BeginTime = BeginTime,
            };
            var depthAnimation = new DoubleAnimation()
            {
                From = SourceShadow != null ? SourceShadow.ShadowDepth : (double?)null,
                To = TargetShadow != null ? TargetShadow.ShadowDepth : (double?)null,
                Duration = new Duration(_duration),
                EasingFunction = new ExponentialEase { EasingMode = EasingMode.EaseInOut },
                BeginTime = BeginTime
            };
            var colorAnimation = new System.Windows.Media.Animation.ColorAnimation()
            {
                From = SourceShadow != null ? SourceShadow.Color : (Color?)null,
                To = TargetShadow != null ? TargetShadow.Color : (Color?)null,
                Duration = new Duration(_duration),
                EasingFunction = new ExponentialEase { EasingMode = EasingMode.EaseInOut },
                BeginTime = BeginTime
            };

            if (TargetName != null)
            {
                Storyboard.SetTargetName(blurAnimation, TargetName);
                Storyboard.SetTargetName(depthAnimation, TargetName);
                Storyboard.SetTargetName(colorAnimation, TargetName);
            }

            Storyboard.SetTargetProperty(blurAnimation, new PropertyPath(TargetProperty.Path + @"." + DropShadowEffect.BlurRadiusProperty.Name));
            Storyboard.SetTargetProperty(depthAnimation, new PropertyPath(TargetProperty.Path + @"." + DropShadowEffect.ShadowDepthProperty.Name));
            Storyboard.SetTargetProperty(colorAnimation, new PropertyPath(TargetProperty.Path + @"." + DropShadowEffect.ColorProperty.Name));

            sb.Children.Add(blurAnimation);
            sb.Children.Add(depthAnimation);
            sb.Children.Add(colorAnimation);
            return sb;
        }

        public DropShadowEffect SourceShadow { get; set; }
        public DropShadowEffect TargetShadow { get; set; }

        public string TargetName { get; set; }
        public PropertyPath TargetProperty { get; set; }

        public TimeSpan Duration
        {
            get { return _duration; }
            set { _duration = value; }
        }

        public TimeSpan BeginTime { get; set; }
    }

    public class ShadowExtension : MarkupExtension
    {
        private static readonly ResourceDictionary _myResourceDictionary;

        static ShadowExtension()
        {
            _myResourceDictionary = new ResourceDictionary
            {
                Source = new Uri("/Nordwest.Wpf.Controls;component/Themes/MaterialExpander.xaml", UriKind.RelativeOrAbsolute)
            };
        }

        public int Level { get; set; }

        public ShadowExtension() { }

        public ShadowExtension(int level)
        {
            Level = level;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var shadow = new DropShadowEffect();
            if (Level < 0 || Level > 5)
                throw new InvalidOperationException();
            if (Level != 0)
            {
                var sh = (DropShadowEffect)_myResourceDictionary[@"Shadow" + Level];
                shadow.BlurRadius = sh.BlurRadius;
                shadow.Color = sh.Color;
                shadow.Direction = sh.Direction;
                shadow.Opacity = sh.Opacity;
                shadow.ShadowDepth = sh.ShadowDepth;
                shadow.RenderingBias = sh.RenderingBias;
            }
            return shadow;
        }
    }
}