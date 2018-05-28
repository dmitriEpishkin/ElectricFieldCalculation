using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Animation;

namespace Nordwest.Wpf.Controls
{
    //такой вот набросок-черновик, служит для анимации, так же как VisualStateManager или триггеры, 
    //но в отличии от них позваляет пересоздавать анимации с новыми параметрами
    //пока не придумал к нему xaml-api и непонятно надо ли вообще
    public class AnimationStateManager<T> where T : AnimationTimeline, new()
    {
        private readonly DependencyObject _target;
        private readonly string _propertyPath;
        private readonly TimeSpan _defaultDuration;
        private readonly List<State> _states = new List<State>();

        public AnimationStateManager(DependencyObject target, string propertyPath) : this(target, propertyPath, TimeSpan.FromSeconds(0.2)) {}

        public AnimationStateManager(DependencyObject target, string propertyPath, TimeSpan defaultDuration)
        {
            _target = target;
            _propertyPath = propertyPath;
            _defaultDuration = defaultDuration;
        }

        public void AddState(string key, Func<bool> condition, object targetValue)
        {
            AddState(key, condition, targetValue, _defaultDuration);
        }

        public void AddState(string key, Func<bool> condition, object targetValue, TimeSpan duration)
        {
            if(string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");
            if (condition == null)
                throw new ArgumentNullException("condition");
            if (targetValue == null)
                throw new ArgumentNullException("targetValue");

            var sb = CreateColorStoryboard(_target, _propertyPath, targetValue, duration);
            var state = _states.SingleOrDefault(s => s.Key == key);
            if (!state.IsEmpty)
            {
                state.Condition = condition;
                state.Storyboard = sb;
            }
            else
                _states.Add(new State(key, condition, sb));
        }

        public void SetTargetValue(string key, object targetValue)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");
            if (targetValue == null)
                throw new ArgumentNullException("targetValue");

            var state = _states.Single(s => s.Key == key);
            state.Storyboard = CreateColorStoryboard(_target, _propertyPath, targetValue, state.Storyboard.Duration);
        }

        private static Storyboard CreateColorStoryboard(DependencyObject target, string path, object toValue, Duration duration)
        {
            var animation = new T();
            SetAnimationToProperty(animation, toValue);
            animation.Duration = duration;
            Storyboard.SetTarget(animation, target);
            Storyboard.SetTargetProperty(animation, new PropertyPath(path));

            var sb = new Storyboard();
            sb.FillBehavior = FillBehavior.HoldEnd;
            sb.Children.Add(animation);
            sb.Freeze();

            return sb;
        }

        private static void SetAnimationToProperty(T animation, object value)
        {
            if (!animation.TargetPropertyType.IsInstanceOfType(value))
                throw new ArgumentException();

            var toProperty = animation.GetType().GetProperty("To");
            if (toProperty == null)
                throw new InvalidOperationException();

            toProperty.SetValue(animation, value, null);
        }

        public void Update()
        {
            var passed = _states.FirstOrDefault(s => s.Condition());
            if(!passed.IsEmpty)
                passed.Storyboard.Begin();
        }

        private struct State
        {
            public readonly string Key;
            public Func<bool> Condition;
            public Storyboard Storyboard;

            public static readonly State Empty = new State();

            public State(string key, Func<bool> condition, Storyboard storyboard)
            {
                Condition = condition;
                Storyboard = storyboard;
                Key = key;
            }

            public bool IsEmpty { get { return this.Key == Empty.Key; } }
        }
    }
}