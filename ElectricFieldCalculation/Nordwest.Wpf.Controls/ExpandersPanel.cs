using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Linq;

namespace Nordwest.Wpf.Controls
{
    public class ExpandersPanel : ItemsControl
    {
        static ExpandersPanel()
        {
            //FocusableProperty.OverrideMetadata(typeof(ExpandersPanel), new FrameworkPropertyMetadata(false));
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ExpandersPanel), new FrameworkPropertyMetadata(typeof(ExpandersPanel)));
        }

        /// <summary>
        /// Determines if the specified item is (or is eligible to be) its own container.
        /// </summary>
        /// <param name="item">The item to check.</param>
        /// <returns>
        /// true if the item is (or is eligible to be) its own container; otherwise, false.
        /// </returns>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is ExpandersPanelItem;
        }

        /// <summary>
        /// Creates or identifies the element that is used to display the given item.
        /// </summary>
        /// <returns>
        /// The element that is used to display the given item.
        /// </returns>
        
        protected override DependencyObject GetContainerForItemOverride()
        {
            var item = new ExpandersPanelItem();
            item.SetBinding(StyleProperty,
                            new Binding(@"ContainerStyle")
                            {
                                Source = this,
                                Path = new PropertyPath(ContainerStyleProperty),
                                Mode = BindingMode.OneWay
                            });
            return item;
        }

        public static readonly DependencyProperty ContainerStyleProperty =
            DependencyProperty.Register("ContainerStyle", typeof(Style), typeof(ExpandersPanel), new PropertyMetadata(default(Style)));

        public Style ContainerStyle
        {
            get { return (Style)GetValue(ContainerStyleProperty); }
            set { SetValue(ContainerStyleProperty, value); }
        }
    }

    [TemplatePart(Name = ExpanderName, Type = typeof(Expander))]
    public class ExpandersPanelItem : ContentControl
    {
        private const string ExpanderName = "Expander";

        static ExpandersPanelItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ExpandersPanelItem), new FrameworkPropertyMetadata(typeof(ExpandersPanelItem)));
        }

        
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var expander = (Expander)GetTemplateChild(ExpanderName);
            if (expander == null) return;
            expander.SetBinding(Expander.IsExpandedProperty,
                                new Binding
                                {
                                    Source = this,
                                    Path = new PropertyPath(IsExpandedProperty),
                                    Mode = BindingMode.TwoWay
                                });
        }


        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(Object), typeof(ExpandersPanelItem), new PropertyMetadata(default(Object), OnHeaderChanged));
        public Object Header
        {
            get { return (Object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        private static void OnHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var epi = (ExpandersPanelItem)d;
            epi.OnHeaderChanged();
        }
        public void OnHeaderChanged()
        {
            EventHandler handler = HeaderChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }
        public event EventHandler HeaderChanged;

        public static readonly DependencyProperty CollapsedHeaderProperty =
            DependencyProperty.Register("CollapsedHeader", typeof(Object), typeof(ExpandersPanelItem), new PropertyMetadata(default(Object), OnCollapsedHeaderChanged));
        public Object CollapsedHeader
        {
            get { return (Object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        private static void OnCollapsedHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var epi = (ExpandersPanelItem)d;
            epi.OnCollapsedHeaderChanged();
        }
        public void OnCollapsedHeaderChanged()
        {
            EventHandler handler = CollapsedHeaderChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }
        public event EventHandler CollapsedHeaderChanged;

        public static readonly DependencyProperty IsExpandedProperty =
            DependencyProperty.Register("IsExpanded", typeof(bool), typeof(ExpandersPanelItem), new PropertyMetadata(default(bool), OnIsExpandedChanged));
        private static void OnIsExpandedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var epi = (ExpandersPanelItem)d;
            epi.OnIsExpandedChanged();
        }
        public void OnIsExpandedChanged()
        {
            EventHandler handler = IsExpandedChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public event EventHandler IsExpandedChanged;

        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }
    }

    public class FillPanel : Panel
    {
        protected override Size MeasureOverride(Size availableSize)
        {
            //в первый раз даем всем чайлдам столько места сколько можем, т.к. нам нужен их DesiredSize
            foreach (var item in Children.OfType<ExpandersPanelItem>())
                item.Measure(availableSize);

            //во второй раз - точно столько, сколько есть
            var expCount = Children.OfType<ExpandersPanelItem>().Count(c => c.IsExpanded);
            var collapsedSize = Children.OfType<ExpandersPanelItem>().Select(child => !child.IsExpanded ? child.DesiredSize.Height + child.Margin.Top + child.Margin.Bottom : 0).Sum();
            var expandedSize = (availableSize.Height - collapsedSize) / expCount;

            foreach (UIElement child in Children)
            {
                if (!(child is ExpandersPanelItem))
                    continue;
                var c = child as ExpandersPanelItem;
                var width = availableSize.Width - c.Margin.Left - c.Margin.Right;
                double heigth = 0;
                if (c.IsExpanded)
                    heigth = expandedSize - c.Margin.Top - c.Margin.Bottom;
                else
                    heigth = child.DesiredSize.Height;
                if (heigth >= 0 && width >= 0)
                    c.Measure(new Size(width, heigth));
            }

            return base.MeasureOverride(availableSize);
        }

        
        protected override Size ArrangeOverride(Size finalSize)
        {
            var expCount = Children.OfType<ExpandersPanelItem>().Count(c => c.IsExpanded);
            var collapsedSize = Children.OfType<ExpandersPanelItem>().Select(child => !child.IsExpanded ? child.DesiredSize.Height + child.Margin.Top + child.Margin.Bottom : 0).Sum();
            var expandedSize = (finalSize.Height - collapsedSize) / expCount;

            var y = 0.0;
            foreach (UIElement child in Children)
            {
                if (!(child is ExpandersPanelItem))
                    continue;
                var c = child as ExpandersPanelItem;
                var top = y + c.Margin.Top;
                var left = c.Margin.Left;
                var width = finalSize.Width - c.Margin.Left - c.Margin.Right;
                double heigth;
                if (c.IsExpanded)
                {
                    heigth = expandedSize - c.Margin.Top - c.Margin.Bottom;
                    y += expandedSize;
                }
                else
                {
                    heigth = child.DesiredSize.Height;
                    y += child.DesiredSize.Height + c.Margin.Top + c.Margin.Bottom;
                }
                if (heigth >= 0 && width >= 0)
                    c.Arrange(new Rect(left, top, width, heigth));
            }
            // Return final size of the panel
            return base.ArrangeOverride(finalSize);
        }

        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            if (visualAdded != null && visualAdded is ExpandersPanelItem)
                ((ExpandersPanelItem)visualAdded).IsExpandedChanged += ItemIsExpandedChanged;
            if (visualRemoved != null && visualRemoved is ExpandersPanelItem)
                ((ExpandersPanelItem)visualRemoved).IsExpandedChanged -= ItemIsExpandedChanged;
            base.OnVisualChildrenChanged(visualAdded, visualRemoved);
        }

        void ItemIsExpandedChanged(object sender, EventArgs e)
        {
            InvalidateArrange();
        }
    }
}