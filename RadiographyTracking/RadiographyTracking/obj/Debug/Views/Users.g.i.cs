﻿#pragma checksum "C:\Users\YATHISH\Gama\RadiographyTracking\RadiographyTracking\Views\Users.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "E6D8ABADC581B369D228E8594866268E"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using RadiographyTracking.Controls;
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
    
    
    public partial class Users : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Label headerLabel;
        
        internal RadiographyTracking.Controls.BusyIndicator busyIndicator;
        
        internal Vagsons.Controls.CustomGrid UsersDataGrid;
        
        internal System.Windows.Controls.Button btnAdd;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/RadiographyTracking;component/Views/Users.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.headerLabel = ((System.Windows.Controls.Label)(this.FindName("headerLabel")));
            this.busyIndicator = ((RadiographyTracking.Controls.BusyIndicator)(this.FindName("busyIndicator")));
            this.UsersDataGrid = ((Vagsons.Controls.CustomGrid)(this.FindName("UsersDataGrid")));
            this.btnAdd = ((System.Windows.Controls.Button)(this.FindName("btnAdd")));
        }
    }
}

