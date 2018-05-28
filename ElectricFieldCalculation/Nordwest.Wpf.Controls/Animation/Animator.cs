using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using Nordwest.Wpf.Controls.Tools;

namespace Nordwest.Wpf.Controls
{

    // атенншн, времени нету приводить это в порядок, так что тапками не кидайтесь ((((
    // ворде работает норм
    [ContentProperty(@"States")]
    public class Animator : Freezable
    {
        private FrameworkElement _host;
        private readonly List<DependencyPropertyNotifier> _notifiers = new List<DependencyPropertyNotifier>();
        private readonly FreezableCollection<AnimatorState> _states;
        private readonly Dictionary<string, AnimatorState> _activeStates = new Dictionary<string, AnimatorState>();

        public Animator()
        {
            _states = new FreezableCollection<AnimatorState>();
        }

        private void OnTargetChanged()
        {
            Initialize();
        }

        private void Initialize()
        {
            SetContext();

            var list = new List<DependencyProperty>();
            foreach (var state in States)
            {
                foreach (var condition in state.Conditions.OfType<NameCondition>())
                {
                    if (list.Contains(condition.Property))
                        continue;
                    list.Add(condition.Property);
                    Subscribe(condition.Property);
                }
                foreach (var condition in state.Conditions.OfType<BindingCondition>())
                {
                    condition.CheckCondition(_host);
                    condition.ConditionChanged += (sender, args) => Animate();
                }
            }

            Animate();
        }

        private void OnPropertyChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            Animate();
        }

        private void Animate()
        {
            var sb = new Storyboard() { FillBehavior = FillBehavior.HoldEnd };
            var groups = States.GroupBy(s => s.Group);

            foreach (var g in groups)
            {
                var state = g.FirstOrDefault(s => s.Conditions.All(c => c.CheckCondition(_host)));
                if (!_activeStates.ContainsKey(g.Key))
                    _activeStates.Add(g.Key, null);

                if (ReferenceEquals(_activeStates[g.Key], state))
                    continue;
                _activeStates[g.Key] = state;
                if (state == null)
                    continue;

                foreach (var a in state.Animations)
                {
                    var tList = PrepareAnimation(a);
                    if (tList != null)
                        foreach (var t in tList)
                            if (t != null)
                                sb.Children.Add(t);
                }
            }

            if (sb.Children.Any())
                sb.Begin(_host, HandoffBehavior.SnapshotAndReplace);
        }

        private IList<AnimationTimeline> PrepareAnimation(AnimationBase animation)
        {
            var target = GetAnimationTarget(animation, _host);
            if (target == null)
                return null;

            var timelines = animation.GetTimeline();

            var timlineList = timelines as IList<AnimationTimeline> ?? timelines.ToList();
            foreach (var timeline in timlineList)
                Storyboard.SetTarget(timeline, target);

            return timlineList;
        }

        private static DependencyObject GetAnimationTarget(AnimationBase animation, DependencyObject host)
        {
            if (animation.Object != null)
                return animation.Object;

            if (string.IsNullOrWhiteSpace(animation.ObjectName))
                return host;

            animation.Object = TryFindName(host, animation.ObjectName);
            return animation.Object;
        }

        public static DependencyObject TryFindName(DependencyObject parent, string name)
        {
            var element = parent as FrameworkElement;
            if (element == null)
                return null;

            var result = element.FindName(name) as DependencyObject;
            if (result != null)
                return result;
            var control = element as Control;
            if (control == null || control.Template == null)
                return null;

            result = control.Template.FindName(name, control) as DependencyObject;
            return result;
        }

        private void Subscribe(DependencyProperty property)
        {
            var notifier = DependencyPropertyNotifier.On(Host, property);
            notifier.PropertyChanged += OnPropertyChanged;
            _notifiers.Add(notifier);
        }

        private void SetContext()
        {
            foreach (var state in _states)
                foreach (var animation in state.Animations)
                    animation.DataContext = Host;
        }

        protected override Freezable CreateInstanceCore()
        {
            return new Animator();
        }

        private FrameworkElement Host
        {
            get { return _host; }
            set
            {
                if (ReferenceEquals(_host, value))
                    return;
                _host = value;
                OnTargetChanged();
            }
        }

        public static readonly DependencyProperty AnimatorProperty = DependencyProperty.RegisterAttached(
            "Animator", typeof(Animator), typeof(Animator), new PropertyMetadata(default(Animator), (o, args) =>
            {
                var animator = (Animator)args.NewValue;
                animator.Host = (FrameworkElement)o;
            }));

        public static void SetAnimator(DependencyObject element, Animator value)
        {
            element.SetValue(AnimatorProperty, value);
        }

        public static Animator GetAnimator(DependencyObject element)
        {
            return (Animator)element.GetValue(AnimatorProperty);
        }

        public FreezableCollection<AnimatorState> States
        {
            get { return _states; }
        }
    }
}