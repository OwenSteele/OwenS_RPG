﻿#pragma checksum "..\..\Tutorial.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "8A18E679D53AF73F14502BCED9CF79C1C3F96B9C2E5779DD4B4716970EC1E8AE"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Engine.ViewModels;
using GameEngine;
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


namespace WPFUI {
    
    
    /// <summary>
    /// Tutorial
    /// </summary>
    public partial class Tutorial : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 117 "..\..\Tutorial.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TabItem KeyBindingsTab;
        
        #line default
        #line hidden
        
        
        #line 168 "..\..\Tutorial.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock North_Key;
        
        #line default
        #line hidden
        
        
        #line 203 "..\..\Tutorial.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock West_Key;
        
        #line default
        #line hidden
        
        
        #line 237 "..\..\Tutorial.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock South_Key;
        
        #line default
        #line hidden
        
        
        #line 271 "..\..\Tutorial.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock East_Key;
        
        #line default
        #line hidden
        
        
        #line 306 "..\..\Tutorial.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock Attack_Key;
        
        #line default
        #line hidden
        
        
        #line 341 "..\..\Tutorial.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock Heal_Key;
        
        #line default
        #line hidden
        
        
        #line 376 "..\..\Tutorial.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock Trader_Key;
        
        #line default
        #line hidden
        
        
        #line 411 "..\..\Tutorial.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock Map_Key;
        
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
            System.Uri resourceLocater = new System.Uri("/WPFUI;component/tutorial.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\Tutorial.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
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
            
            #line 20 "..\..\Tutorial.xaml"
            ((WPFUI.Tutorial)(target)).Loaded += new System.Windows.RoutedEventHandler(this.OnLoad_Tutorial);
            
            #line default
            #line hidden
            return;
            case 2:
            this.KeyBindingsTab = ((System.Windows.Controls.TabItem)(target));
            return;
            case 3:
            this.North_Key = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.West_Key = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.South_Key = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.East_Key = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 7:
            this.Attack_Key = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 8:
            this.Heal_Key = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 9:
            this.Trader_Key = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 10:
            this.Map_Key = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 11:
            
            #line 786 "..\..\Tutorial.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.OnClick_Close);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

