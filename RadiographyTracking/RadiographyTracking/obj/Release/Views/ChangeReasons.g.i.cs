﻿#pragma checksum "C:\Documents and Settings\eqa40593\Gama\RadiographyTracking\RadiographyTracking\Views\ChangeReasons.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "D16B6B4C7B8D78FCFDAD23AC8C7E855F"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.296
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;
using Vagsons.Controls;


namespace RadiographyTracking.Views {
    
    
    public partial class ChangeReasons : System.Windows.Controls.ChildWindow {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Vagsons.Controls.CustomGrid ChangesGrid;
        
        internal System.Windows.Controls.Button CancelButton;
        
        internal System.Windows.Controls.Button OKButton;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/RadiographyTracking;component/Views/ChangeReasons.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ChangesGrid = ((Vagsons.Controls.CustomGrid)(this.FindName("ChangesGrid")));
            this.CancelButton = ((System.Windows.Controls.Button)(this.FindName("CancelButton")));
            this.OKButton = ((System.Windows.Controls.Button)(this.FindName("OKButton")));
        }
    }
}

