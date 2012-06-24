﻿#pragma checksum "D:\ClientWork\Gama\Sources\RadiographyTracking\RadiographyTracking\Views\EnterRadioGraphyReport.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "5E7A28867F55EC30052ACF79AFF16765"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17626
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
    
    
    public partial class EnterRadioGraphyReport : RadiographyTracking.Views.BaseCRUDView {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.DomainDataSource RGReportSource;
        
        internal System.Windows.Controls.DomainDataSource EditRGReportsSource;
        
        internal System.Windows.Controls.DomainDataSource FixedPatternsSource;
        
        internal System.Windows.Controls.DomainDataSource CustomersSource;
        
        internal System.Windows.Controls.DomainDataSource RGStatusesSource;
        
        internal System.Windows.Controls.DomainDataSource RGRowTypesSource;
        
        internal System.Windows.Controls.Label headerLabel;
        
        internal RadiographyTracking.Controls.BusyIndicator busyIndicator;
        
        internal System.Windows.Controls.TextBox txtFPNo;
        
        internal System.Windows.Controls.ComboBox cmbCoverage;
        
        internal System.Windows.Controls.TextBox txtRTNo;
        
        internal System.Windows.Controls.Button btnFetch;
        
        internal System.Windows.Controls.TextBlock lblFPNo;
        
        internal System.Windows.Controls.TextBlock lblCustomer;
        
        internal System.Windows.Controls.TextBlock lblCoverage;
        
        internal System.Windows.Controls.TextBlock lblDescription;
        
        internal System.Windows.Controls.TextBox txtLeadScreen;
        
        internal System.Windows.Controls.TextBox txtSourceSize;
        
        internal System.Windows.Controls.TextBlock lblRGReportID;
        
        internal System.Windows.Controls.TextBlock lblFixedPatternID;
        
        internal System.Windows.Controls.TextBlock lblRTNo;
        
        internal System.Windows.Controls.TextBox txtReportNo;
        
        internal System.Windows.Controls.TextBox txtHeatNo;
        
        internal System.Windows.Controls.TextBox txtProcedureRef;
        
        internal System.Windows.Controls.TextBox txtSpecifications;
        
        internal System.Windows.Controls.DatePicker ReportDatePicker;
        
        internal System.Windows.Controls.TextBox txtFilm;
        
        internal System.Windows.Controls.DatePicker TestDatePicker;
        
        internal System.Windows.Controls.ComboBox cmbShift;
        
        internal System.Windows.Controls.TextBox txtEvaluation;
        
        internal System.Windows.Controls.TextBox txtAcceptance;
        
        internal System.Windows.Controls.TextBox txtDrawingNo;
        
        internal System.Windows.Controls.TextBlock lblStatus;
        
        internal Vagsons.Controls.CustomGrid RGReportDataGrid;
        
        internal Vagsons.Controls.CustomGrid RGReportDataGridClerk;
        
        internal System.Windows.Controls.Button btnAdd;
        
        internal System.Windows.Controls.TextBlock txtTotalArea;
        
        internal BindableDataGrid.BindableDataGrid energyAreas;
        
        internal System.Windows.Controls.StackPanel ButtonsPanel;
        
        internal System.Windows.Controls.Button btnSave;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/RadiographyTracking;component/Views/EnterRadioGraphyReport.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.RGReportSource = ((System.Windows.Controls.DomainDataSource)(this.FindName("RGReportSource")));
            this.EditRGReportsSource = ((System.Windows.Controls.DomainDataSource)(this.FindName("EditRGReportsSource")));
            this.FixedPatternsSource = ((System.Windows.Controls.DomainDataSource)(this.FindName("FixedPatternsSource")));
            this.CustomersSource = ((System.Windows.Controls.DomainDataSource)(this.FindName("CustomersSource")));
            this.RGStatusesSource = ((System.Windows.Controls.DomainDataSource)(this.FindName("RGStatusesSource")));
            this.RGRowTypesSource = ((System.Windows.Controls.DomainDataSource)(this.FindName("RGRowTypesSource")));
            this.headerLabel = ((System.Windows.Controls.Label)(this.FindName("headerLabel")));
            this.busyIndicator = ((RadiographyTracking.Controls.BusyIndicator)(this.FindName("busyIndicator")));
            this.txtFPNo = ((System.Windows.Controls.TextBox)(this.FindName("txtFPNo")));
            this.cmbCoverage = ((System.Windows.Controls.ComboBox)(this.FindName("cmbCoverage")));
            this.txtRTNo = ((System.Windows.Controls.TextBox)(this.FindName("txtRTNo")));
            this.btnFetch = ((System.Windows.Controls.Button)(this.FindName("btnFetch")));
            this.lblFPNo = ((System.Windows.Controls.TextBlock)(this.FindName("lblFPNo")));
            this.lblCustomer = ((System.Windows.Controls.TextBlock)(this.FindName("lblCustomer")));
            this.lblCoverage = ((System.Windows.Controls.TextBlock)(this.FindName("lblCoverage")));
            this.lblDescription = ((System.Windows.Controls.TextBlock)(this.FindName("lblDescription")));
            this.txtLeadScreen = ((System.Windows.Controls.TextBox)(this.FindName("txtLeadScreen")));
            this.txtSourceSize = ((System.Windows.Controls.TextBox)(this.FindName("txtSourceSize")));
            this.lblRGReportID = ((System.Windows.Controls.TextBlock)(this.FindName("lblRGReportID")));
            this.lblFixedPatternID = ((System.Windows.Controls.TextBlock)(this.FindName("lblFixedPatternID")));
            this.lblRTNo = ((System.Windows.Controls.TextBlock)(this.FindName("lblRTNo")));
            this.txtReportNo = ((System.Windows.Controls.TextBox)(this.FindName("txtReportNo")));
            this.txtHeatNo = ((System.Windows.Controls.TextBox)(this.FindName("txtHeatNo")));
            this.txtProcedureRef = ((System.Windows.Controls.TextBox)(this.FindName("txtProcedureRef")));
            this.txtSpecifications = ((System.Windows.Controls.TextBox)(this.FindName("txtSpecifications")));
            this.ReportDatePicker = ((System.Windows.Controls.DatePicker)(this.FindName("ReportDatePicker")));
            this.txtFilm = ((System.Windows.Controls.TextBox)(this.FindName("txtFilm")));
            this.TestDatePicker = ((System.Windows.Controls.DatePicker)(this.FindName("TestDatePicker")));
            this.cmbShift = ((System.Windows.Controls.ComboBox)(this.FindName("cmbShift")));
            this.txtEvaluation = ((System.Windows.Controls.TextBox)(this.FindName("txtEvaluation")));
            this.txtAcceptance = ((System.Windows.Controls.TextBox)(this.FindName("txtAcceptance")));
            this.txtDrawingNo = ((System.Windows.Controls.TextBox)(this.FindName("txtDrawingNo")));
            this.lblStatus = ((System.Windows.Controls.TextBlock)(this.FindName("lblStatus")));
            this.RGReportDataGrid = ((Vagsons.Controls.CustomGrid)(this.FindName("RGReportDataGrid")));
            this.RGReportDataGridClerk = ((Vagsons.Controls.CustomGrid)(this.FindName("RGReportDataGridClerk")));
            this.btnAdd = ((System.Windows.Controls.Button)(this.FindName("btnAdd")));
            this.txtTotalArea = ((System.Windows.Controls.TextBlock)(this.FindName("txtTotalArea")));
            this.energyAreas = ((BindableDataGrid.BindableDataGrid)(this.FindName("energyAreas")));
            this.ButtonsPanel = ((System.Windows.Controls.StackPanel)(this.FindName("ButtonsPanel")));
            this.btnSave = ((System.Windows.Controls.Button)(this.FindName("btnSave")));
            this.btnCancel = ((System.Windows.Controls.Button)(this.FindName("btnCancel")));
        }
    }
}

