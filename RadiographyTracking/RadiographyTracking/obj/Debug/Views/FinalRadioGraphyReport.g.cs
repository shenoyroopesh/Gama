﻿#pragma checksum "D:\ClientWork\Gama\Sources\RadiographyTracking\RadiographyTracking\Views\FinalRadioGraphyReport.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "F87E45214BC8F68FADC019CE990FF77E"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.530
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using BindableDataGrid;
using RadiographyTracking.Controls;
using RadiographyTracking.Views;
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
    
    
    public partial class FinalRadioGraphyReport : RadiographyTracking.Views.BaseCRUDView {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.DomainDataSource ReportSource;
        
        internal System.Windows.Controls.DomainDataSource FixedPatternsSource;
        
        internal System.Windows.Controls.DomainDataSource CustomersSource;
        
        internal System.Windows.Controls.DomainDataSource RGStatusesSource;
        
        internal System.Windows.Controls.DomainDataSource RGRowTypesSource;
        
        internal System.Windows.Controls.Label headerLabel;
        
        internal RadiographyTracking.Controls.BusyIndicator busyIndicator;
        
        internal System.Windows.Controls.TextBox txtRTNo;
        
        internal System.Windows.Controls.TextBlock lblFPNo;
        
        internal System.Windows.Controls.TextBlock lblCustomer;
        
        internal System.Windows.Controls.TextBlock lblCoverage;
        
        internal System.Windows.Controls.TextBlock lblDescription;
        
        internal System.Windows.Controls.TextBlock txtLeadScreen;
        
        internal System.Windows.Controls.TextBlock txtSourceSize;
        
        internal System.Windows.Controls.TextBlock lblRGReportID;
        
        internal System.Windows.Controls.TextBlock lblFixedPatternID;
        
        internal System.Windows.Controls.TextBlock lblRTNo;
        
        internal System.Windows.Controls.TextBlock txtReportNo;
        
        internal System.Windows.Controls.TextBlock txtHeatNo;
        
        internal System.Windows.Controls.TextBlock txtProcedureRef;
        
        internal System.Windows.Controls.TextBlock txtSpecifications;
        
        internal System.Windows.Controls.TextBlock ReportDatePicker;
        
        internal System.Windows.Controls.TextBlock txtFilm;
        
        internal System.Windows.Controls.TextBlock TestDatePicker;
        
        internal System.Windows.Controls.TextBlock cmbShift;
        
        internal System.Windows.Controls.TextBlock txtEvaluation;
        
        internal System.Windows.Controls.TextBlock txtAcceptance;
        
        internal System.Windows.Controls.TextBlock txtDrawingNo;
        
        internal System.Windows.Controls.CheckBox chkOnlyRepairs;
        
        internal System.Windows.Controls.TextBlock lblReportTemplate;
        
        internal System.Windows.Controls.ComboBox cmbSelectTemplate;
        
        internal Vagsons.Controls.CustomGrid ReportDataGrid;
        
        internal System.Windows.Controls.TextBlock lblStatus;
        
        internal System.Windows.Controls.TextBlock lblTotalArea;
        
        internal BindableDataGrid.BindableDataGrid energyAreas;
        
        internal System.Windows.Controls.StackPanel ButtonsPanel;
        
        internal System.Windows.Controls.Button btnPrint;
        
        internal System.Windows.Controls.Button btnCancel;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/RadiographyTracking;component/Views/FinalRadioGraphyReport.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ReportSource = ((System.Windows.Controls.DomainDataSource)(this.FindName("ReportSource")));
            this.FixedPatternsSource = ((System.Windows.Controls.DomainDataSource)(this.FindName("FixedPatternsSource")));
            this.CustomersSource = ((System.Windows.Controls.DomainDataSource)(this.FindName("CustomersSource")));
            this.RGStatusesSource = ((System.Windows.Controls.DomainDataSource)(this.FindName("RGStatusesSource")));
            this.RGRowTypesSource = ((System.Windows.Controls.DomainDataSource)(this.FindName("RGRowTypesSource")));
            this.headerLabel = ((System.Windows.Controls.Label)(this.FindName("headerLabel")));
            this.busyIndicator = ((RadiographyTracking.Controls.BusyIndicator)(this.FindName("busyIndicator")));
            this.txtRTNo = ((System.Windows.Controls.TextBox)(this.FindName("txtRTNo")));
            this.lblFPNo = ((System.Windows.Controls.TextBlock)(this.FindName("lblFPNo")));
            this.lblCustomer = ((System.Windows.Controls.TextBlock)(this.FindName("lblCustomer")));
            this.lblCoverage = ((System.Windows.Controls.TextBlock)(this.FindName("lblCoverage")));
            this.lblDescription = ((System.Windows.Controls.TextBlock)(this.FindName("lblDescription")));
            this.txtLeadScreen = ((System.Windows.Controls.TextBlock)(this.FindName("txtLeadScreen")));
            this.txtSourceSize = ((System.Windows.Controls.TextBlock)(this.FindName("txtSourceSize")));
            this.lblRGReportID = ((System.Windows.Controls.TextBlock)(this.FindName("lblRGReportID")));
            this.lblFixedPatternID = ((System.Windows.Controls.TextBlock)(this.FindName("lblFixedPatternID")));
            this.lblRTNo = ((System.Windows.Controls.TextBlock)(this.FindName("lblRTNo")));
            this.txtReportNo = ((System.Windows.Controls.TextBlock)(this.FindName("txtReportNo")));
            this.txtHeatNo = ((System.Windows.Controls.TextBlock)(this.FindName("txtHeatNo")));
            this.txtProcedureRef = ((System.Windows.Controls.TextBlock)(this.FindName("txtProcedureRef")));
            this.txtSpecifications = ((System.Windows.Controls.TextBlock)(this.FindName("txtSpecifications")));
            this.ReportDatePicker = ((System.Windows.Controls.TextBlock)(this.FindName("ReportDatePicker")));
            this.txtFilm = ((System.Windows.Controls.TextBlock)(this.FindName("txtFilm")));
            this.TestDatePicker = ((System.Windows.Controls.TextBlock)(this.FindName("TestDatePicker")));
            this.cmbShift = ((System.Windows.Controls.TextBlock)(this.FindName("cmbShift")));
            this.txtEvaluation = ((System.Windows.Controls.TextBlock)(this.FindName("txtEvaluation")));
            this.txtAcceptance = ((System.Windows.Controls.TextBlock)(this.FindName("txtAcceptance")));
            this.txtDrawingNo = ((System.Windows.Controls.TextBlock)(this.FindName("txtDrawingNo")));
            this.chkOnlyRepairs = ((System.Windows.Controls.CheckBox)(this.FindName("chkOnlyRepairs")));
            this.lblReportTemplate = ((System.Windows.Controls.TextBlock)(this.FindName("lblReportTemplate")));
            this.cmbSelectTemplate = ((System.Windows.Controls.ComboBox)(this.FindName("cmbSelectTemplate")));
            this.ReportDataGrid = ((Vagsons.Controls.CustomGrid)(this.FindName("ReportDataGrid")));
            this.lblStatus = ((System.Windows.Controls.TextBlock)(this.FindName("lblStatus")));
            this.lblTotalArea = ((System.Windows.Controls.TextBlock)(this.FindName("lblTotalArea")));
            this.energyAreas = ((BindableDataGrid.BindableDataGrid)(this.FindName("energyAreas")));
            this.ButtonsPanel = ((System.Windows.Controls.StackPanel)(this.FindName("ButtonsPanel")));
            this.btnPrint = ((System.Windows.Controls.Button)(this.FindName("btnPrint")));
            this.btnCancel = ((System.Windows.Controls.Button)(this.FindName("btnCancel")));
        }
    }
}

