using System.Windows;

namespace Nordwest.Wpf.Controls
{
    public class NameCondition : ConditionBase
    {
        public DependencyObject Object { get; set; }
        public string ObjectName { get; set; }
        public DependencyProperty Property { get; set; }

        public override bool CheckCondition(DependencyObject host)
        {
            return Equals(GetConditionTarget(this, host).GetValue(this.Property), this.Value);
        }

        private static DependencyObject GetConditionTarget(NameCondition condition, DependencyObject host)
        {
            if (condition.Object != null)
                return condition.Object;

            if (string.IsNullOrWhiteSpace(condition.ObjectName))
                return host;

            condition.Object = Animator.TryFindName(host, condition.ObjectName);
            return condition.Object;
        }
    }
}