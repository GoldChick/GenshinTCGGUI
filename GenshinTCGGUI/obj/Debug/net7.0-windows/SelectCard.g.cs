﻿#pragma checksum "..\..\..\SelectCard.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "8EF59F3AC20A0B2267E9FBA1CD49941823108100"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using GenshinTCGGUI;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
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


namespace GenshinTCGGUI {
    
    
    /// <summary>
    /// SelectCard
    /// </summary>
    public partial class SelectCard : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 14 "..\..\..\SelectCard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.UniformGrid CharacterCardsSelected;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\SelectCard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.UniformGrid ActionCardsSelected;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\SelectCard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ScrollViewer ActionCardScroll;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\..\SelectCard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel ActionCardToSelectPanel;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\SelectCard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ScrollViewer CharacterCardScroll;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\SelectCard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel CharacterCardToSelectPanel;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/GenshinTCGGUI;component/selectcard.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\SelectCard.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 11 "..\..\..\SelectCard.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ChooseCharacter);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 12 "..\..\..\SelectCard.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ChooseAction);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 13 "..\..\..\SelectCard.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ChooseSave);
            
            #line default
            #line hidden
            return;
            case 4:
            this.CharacterCardsSelected = ((System.Windows.Controls.Primitives.UniformGrid)(target));
            return;
            case 5:
            this.ActionCardsSelected = ((System.Windows.Controls.Primitives.UniformGrid)(target));
            return;
            case 6:
            this.ActionCardScroll = ((System.Windows.Controls.ScrollViewer)(target));
            return;
            case 7:
            this.ActionCardToSelectPanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 8:
            this.CharacterCardScroll = ((System.Windows.Controls.ScrollViewer)(target));
            return;
            case 9:
            this.CharacterCardToSelectPanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

