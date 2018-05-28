using System;
using System.Windows;

namespace Nordwest.Wpf.Controls
{
    public class AnimatorState : Freezable
    {
        private FreezableCollection<ConditionBase> _conditions;
        private FreezableCollection<AnimationBase> _animations;

        public AnimatorState()
        {
            Conditions = new FreezableCollection<ConditionBase>();
            Animations = new FreezableCollection<AnimationBase>();
            Group = string.Empty;
        }

        public FreezableCollection<ConditionBase> Conditions
        {
            get { return _conditions; }
            set
            {
                if (IsFrozen)
                    throw new InvalidOperationException();
                _conditions = value;
            }
        }

        public FreezableCollection<AnimationBase> Animations
        {
            get { return _animations; }
            set
            {
                if (IsFrozen)
                    throw new InvalidOperationException();
                _animations = value;
            }
        }

        public string Group { get; set; }

        protected override Freezable CreateInstanceCore()
        {
            return new AnimatorState();
        }
    }
}