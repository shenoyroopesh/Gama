﻿#pragma checksum "C:\Users\YATHISH\Gama\RadiographyTracking\RadiographyTracking\Views\Company.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "78C302B519AE36224E8E705C1052CE58"
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


namespace RadiographyTracking.Views {
    
    
    public partial class Company : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Label headerLabel;
        
        internal RadiographyTracking.Controls.BusyIndicator busyIndicator;
        
        internal System.Windows.Controls.DataForm companyForm;
        
        internal RadiographyTracking.Controls.FileUpload FileUploader;
        
        internal System.Windows.Controls.Button btnSave;
        
        internal System.Windows.Controls.Button btnCancel;
        
        internal System.Windows.Controls.DomainDataSource companyDomainDataSource;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/RadiographyTracking;component/Views/Company.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.headerLabel = ((System.Windows.Controls.Label)(this.FindName("headerLabel")));
            this.busyIndicator = ((RadiographyTracking.Controls.BusyIndicator)(this.FindName("busyIndicator")));
            this.companyForm = ((System.Windows.Controls.DataForm)(this.FindName("companyForm")));
            this.FileUploader = ((RadiographyTracking.Controls.FileUpload)(this.FindName("FileUploader")));
            this.btnSave = ((System.Windows.Controls.Button)(this.FindName("btnSave")));
            this.btnCancel = ((System.Windows.Controls.Button)(this.FindName("btnCancel")));
            this.companyDomainDataSource = ((System.Windows.Controls.DomainDataSource)(this.FindName("companyDomainDataSource")));
        }
    }
}

