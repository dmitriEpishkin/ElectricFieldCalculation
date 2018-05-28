using System.Windows;

namespace Nordwest.Wpf.Controls
{
    public abstract class ConditionBase : DependencyObject
    {
        public object Value { get; set; }
        public abstract bool CheckCondition(DependencyObject host);
    }
}