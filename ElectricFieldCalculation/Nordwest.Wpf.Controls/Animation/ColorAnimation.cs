using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;

namespace Nordwest.Wpf.Controls
{
    public class ColorAnimation : AnimationBase
    {
        public override IEnumerable<AnimationTimeline> GetTimeline()
        {
            if (!(To is Color) || Path == null)
                return null;

            var a = new System.Windows.Media.Animation.ColorAnimation((Color)To, TimeSpan.FromSeconds(0.2));
            Storyboard.SetTargetProperty(a, Path);
            return new[] {a};
        }
    }

    public class ShadowAnimation : AnimationBase
    {
        public override IEnumerable<AnimationTimeline> GetTimeline()
        {
            var target = To as DropShadowEffect;
            if (!(To is DropShadowEffect) || Path == null)
                return null;

            var blurAnimation = new DoubleAnimation()
            {
                To = target.BlurRadius,
                Duration = TimeSpan.FromSeconds(0.2),
                EasingFunction = new ExponentialEase { EasingMode = EasingMode.EaseInOut },
                BeginTime = TimeSpan.Zero,
            };
            var depthAnimation = new DoubleAnimation()
            {
                To = target.ShadowDepth,
                Duration = TimeSpan.FromSeconds(0.2),
                EasingFunction = new ExponentialEase { EasingMode = EasingMode.EaseInOut },
                BeginTime = TimeSpan.Zero
            };
            var colorAnimation = new System.Windows.Media.Animation.ColorAnimation()
            {
                To = target.Color,
                Duration = TimeSpan.FromSeconds(0.2),
                EasingFunction = new ExponentialEase { EasingMode = EasingMode.EaseInOut },
                BeginTime = TimeSpan.Zero
            };
            
            var converter = new PropertyPathConverter();
            var pathStr = converter.ConvertToString(Path);
            Storyboard.SetTargetProperty(blurAnimation, CombinePath(converter, pathStr, DropShadowEffect.BlurRadiusProperty));//new PropertyPath(Path.Path + @"." + DropShadowEffect.BlurRadiusProperty.Name));
            Storyboard.SetTargetProperty(depthAnimation, CombinePath(converter, pathStr, DropShadowEffect.ShadowDepthProperty)); //new PropertyPath(Path.Path + @"." + DropShadowEffect.ShadowDepthProperty.Name));
            Storyboard.SetTargetProperty(colorAnimation, CombinePath(converter, pathStr, DropShadowEffect.ColorProperty)); //new PropertyPath(Path.Path + @"." + DropShadowEffect.ColorProperty.Name));

            return new AnimationTimeline[] { blurAnimation, depthAnimation, colorAnimation };
        }

        private static PropertyPath CombinePath(PropertyPathConverter converter, string pathStr, DependencyProperty blurRadiusProperty)
        {
            var newPath = (PropertyPath)converter.ConvertFromString(pathStr + @".(DropShadowEffect." + blurRadiusProperty.Name+@")");
            return newPath;
        }
    }
}