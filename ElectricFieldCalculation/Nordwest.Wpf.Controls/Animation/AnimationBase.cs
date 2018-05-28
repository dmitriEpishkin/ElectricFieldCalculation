using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Animation;

namespace Nordwest.Wpf.Controls
{
    public abstract class AnimationBase : FrameworkElement
    {
        public PropertyPath Path { get; set; }
        public string ObjectName { get; set; }
        public DependencyObject Object { get; set; }
        public abstract IEnumerable<AnimationTimeline> GetTimeline();

        public static readonly DependencyProperty ToProperty = DependencyProperty.Register(
            "To", typeof(object), typeof(AnimationBase), new PropertyMetadata(default(object)));

        public object To
        {
            get { return (object)GetValue(ToProperty); }
            set { SetValue(ToProperty, value); }
        }
    }
}