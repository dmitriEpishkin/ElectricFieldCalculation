﻿#pragma checksum "..\..\..\Chart\ChartControl.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "2001F6C4AC588777A3836EC4AAB8DF2C81726592"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using Nordwest.Wpf.Controls.Chart;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Nordwest.Wpf.Controls.Chart {
    
    
    /// <summary>
    /// ChartControl
    /// </summary>
    public partial class ChartControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 8 "..\..\..\Chart\ChartControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private System.Windows.Controls.Grid grid;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\Chart\ChartControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private Nordwest.Wpf.Controls.Chart.ChartCanvas chartCanvas;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\Chart\ChartControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private Nordwest.Wpf.Controls.Chart.VerticalAxisControl leftAxis;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\Chart\ChartControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private Nordwest.Wpf.Controls.Chart.VerticalAxisControl rightAxis;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\Chart\ChartControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private Nordwest.Wpf.Controls.Chart.HorizontalAxisControl topAxis;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\Chart\ChartControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private Nordwest.Wpf.Controls.Chart.HorizontalAxisControl bottomAxis;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Nordwest.Wpf.Controls;component/chart/chartcontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Chart\ChartControl.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 7 "..\..\..\Chart\ChartControl.xaml"
            ((Nordwest.Wpf.Controls.Chart.ChartControl)(target)).Loaded += new System.Windows.RoutedEventHandler(this.OnLoaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.grid = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.chartCanvas = ((Nordwest.Wpf.Controls.Chart.ChartCanvas)(target));
            return;
            case 4:
            this.leftAxis = ((Nordwest.Wpf.Controls.Chart.VerticalAxisControl)(target));
            return;
            case 5:
            this.rightAxis = ((Nordwest.Wpf.Controls.Chart.VerticalAxisControl)(target));
            return;
            case 6:
            this.topAxis = ((Nordwest.Wpf.Controls.Chart.HorizontalAxisControl)(target));
            return;
            case 7:
            this.bottomAxis = ((Nordwest.Wpf.Controls.Chart.HorizontalAxisControl)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
