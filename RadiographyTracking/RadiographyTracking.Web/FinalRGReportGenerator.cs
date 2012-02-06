// ----------------------------------------------------------------------
// <copyright file="SampleDocumentWithTableGenerator.cs" author="Atul Verma">
//     Copyright (c) Atul Verma. This utility along with samples demonstrate how to use the Open Xml 2.0 SDK and VS 2010 for document generation. They are unsupported, but you can use them as-is.
// </copyright>
// ------------------------------------------------------------------------

namespace RadiographyTracking.Web
{
    using System.Collections.Generic;
    using DocumentFormat.OpenXml.Wordprocessing;
    using WordDocumentGenerator.Library;
    using RadiographyTracking.Web.Models;
    using System;
    using System.Drawing;
    using System.IO;
    using DocumentFormat.OpenXml.Packaging;
    using System.Linq;
    using DocumentFormat.OpenXml.Drawing;
    using DocumentFormat.OpenXml;

    /// <summary>
    /// Sample refreshable document generator for Test_Template - 2.docx(has table) template
    /// </summary>
    [CLSCompliant(false)]
    public class FinalRGReportGenerator : DocumentGenerator
    {
        #region contentcontroltags
        protected const string CustomerName = "CustomerName";
        protected const string CustomerAddress = "CustomerAddress";
        protected const string CustomerPhone = "CustomerPhone";
        protected const string CustomerEmail = "CustomerEmail";
        protected const string Description = "Description";
        protected const string Specification = "Specification";
        protected const string DrawingNo = "DrawingNo";
        protected const string RTNo = "RTNo";
        protected const string HeatNo = "HeatNo";
        protected const string Coverage = "Coverage";
        protected const string ProcedureRef = "ProcedureRef";
        protected const string ReportNo = "ReportNo";
        protected const string Date = "Date";
        protected const string Film = "Film";
        protected const string LeadScreen = "LeadScreen";
        protected const string Source = "Source";
        protected const string DateOfTest = "DateOfTest";
        protected const string Evaluation = "Evaluation";
        protected const string Acceptance = "Acceptance";
        protected const string Logo = "Logo";
        protected const string CustomerLogo = "CustomerLogo";
        protected const string RGReportRow = "RGReportRow";
        protected const string SlNo = "SlNo";
        protected const string Location = "Location";
        protected const string Thickness = "Thickness";
        protected const string SFD = "SFD";
        protected const string Designation = "Designation";
        protected const string Sensitivity = "Sensitivity";
        protected const string Density = "Density";
        protected const string FilmSize = "FilmSize";
        protected const string Observation = "Observation";
        protected const string Remarks = "Remarks";

        protected const string Result = "Result";
        protected const string TotalArea = "TotalArea";
        protected const string Isotope = "Isotope";
        protected const string Area = "Area";

        protected const string IsotopeCollection = "IsotopeCollection";
        protected const string AreaCollection = "AreaCollection";

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SampleDocumentWithTableGenerator"/> class.
        /// </summary>
        /// <param name="generationInfo">The generation info.</param>
        public FinalRGReportGenerator(DocumentGenerationInfo generationInfo)
            : base(generationInfo)
        {
        }

        #endregion

        #region Overridden methods

        /// <summary>
        /// Gets the place holder tag to type collection.
        /// </summary>
        /// <returns></returns>
        protected override Dictionary<string, PlaceHolderType> GetPlaceHolderTagToTypeCollection()
        {
            Dictionary<string, PlaceHolderType> placeHolderTagToTypeCollection = new Dictionary<string, PlaceHolderType>();
            // Handle non recursive placeholders
            placeHolderTagToTypeCollection.Add(CustomerName, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(Description, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(Specification, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(DrawingNo, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(RTNo, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(HeatNo, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(Coverage, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(ProcedureRef, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(ReportNo, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(Date, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(Film, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(LeadScreen, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(Source, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(DateOfTest, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(Evaluation, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(Acceptance, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(Logo, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(CustomerLogo, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(CustomerAddress, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(CustomerEmail, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(CustomerPhone, PlaceHolderType.NonRecursive);

            // Handle Rowlevel place holders
            placeHolderTagToTypeCollection.Add(RGReportRow, PlaceHolderType.Recursive);

            //within each row, these are non-recursive
            placeHolderTagToTypeCollection.Add(SlNo, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(Location, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(Thickness, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(SFD, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(Designation, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(Sensitivity, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(Density, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(FilmSize, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(Observation, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(Remarks, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(Result, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(TotalArea, PlaceHolderType.NonRecursive);

            placeHolderTagToTypeCollection.Add(Isotope, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(Area, PlaceHolderType.NonRecursive);

            placeHolderTagToTypeCollection.Add(IsotopeCollection, PlaceHolderType.Recursive);
            placeHolderTagToTypeCollection.Add(AreaCollection, PlaceHolderType.Recursive);


            return placeHolderTagToTypeCollection;
        }

        /// <summary>
        /// Non recursive placeholder found.
        /// </summary>
        /// <param name="placeholderTag">The placeholder tag.</param>
        /// <param name="openXmlElementDataContext">The open XML element data context.</param>
        protected override void NonRecursivePlaceholderFound(string placeholderTag, OpenXmlElementDataContext openXmlElementDataContext, WordprocessingDocument document)
        {
            if (openXmlElementDataContext == null || openXmlElementDataContext.Element == null || openXmlElementDataContext.DataContext == null)
            {
                return;
            }

            string tagPlaceHolderValue = string.Empty;
            string tagGuidPart = string.Empty;
            GetTagValue(openXmlElementDataContext.Element as SdtElement, out tagPlaceHolderValue, out tagGuidPart);

            var row = openXmlElementDataContext.DataContext as FinalRTReport;

            string tagValue = string.Empty;
            string content = string.Empty;

            if (row != null)
            {
                switch (tagPlaceHolderValue)
                {
                    case CustomerName:
                        content = row.FixedPattern.Customer.CustomerName;
                        break;
                    case CustomerAddress:
                        content = row.FixedPattern.Customer.Address;
                        break;
                    case CustomerEmail:
                        content = row.FixedPattern.Customer.Email;
                        break;
                    case CustomerPhone:
                        content = row.FixedPattern.Customer.PhoneNo;
                        break;
                    case ReportNo:
                        content = row.ReportNo;
                        break;
                    case Description:
                        content = row.FixedPattern.Description;
                        break;
                    case Coverage:
                        content = row.Coverage.CoverageName;
                        break;
                    case ProcedureRef:
                        content = row.ProcedureRef;
                        break;
                    case Source:
                        content = row.SourceSize;
                        break;
                    case Specification:
                        content = row.Specifications;
                        break;
                    case DrawingNo:
                        content = row.DrawingNo;
                        break;
                    case LeadScreen:
                        content = row.LeadScreen;
                        break;
                    case HeatNo:
                        content = row.HeatNo;
                        break;
                    case Date:
                        content = row.ReportDate;
                        break;
                    case Film:
                        content = row.Film;
                        break;
                    case RTNo:
                        content = row.RTNo;
                        break;
                    case DateOfTest:
                        content = row.DateOfTest;
                        break;
                    case Evaluation:
                        content = row.EvaluationAsPer;
                        break;
                    case Acceptance:
                        content = row.AcceptanceAsPer;
                        break;
                    case TotalArea:
                        content = row.TotalArea.ToString();
                        break;
                    case Result:
                        content = row.Status.Status;
                        break;
                    case Logo:
                        //get the company logo
                        var logo = row.getCompanyLogo();
                        SetLogo(openXmlElementDataContext.Element as SdtElement, logo, document);
                        break;
                    case CustomerLogo:
                        var customerLogo = row.getCustomerLogo();
                        SetLogo(openXmlElementDataContext.Element as SdtElement, customerLogo, document);
                        break;
                }
            }
            else
            {
                var reportRow = openXmlElementDataContext.DataContext as FinalRTReportRow;

                if (reportRow != null)
                {
                    switch (tagPlaceHolderValue)
                    {
                        case SlNo:
                            content = reportRow.SlNo.ToString();
                            break;
                        case Location:
                            content = reportRow.LocationAndSegment;
                            break;
                        case Thickness:
                            content = reportRow.Thickness.ToString();
                            break;
                        case SFD:
                            content = reportRow.SFD.ToString();
                            break;
                        case Designation:
                            content = reportRow.Designation;
                            break;
                        case Sensitivity:
                            content = reportRow.Sensitivity;
                            break;
                        case Density:
                            content = reportRow.Density;
                            break;
                        case FilmSize:
                            content = reportRow.FilmSizeString;
                            break;
                        case Observation:
                            content = reportRow.Observations;
                            break;
                        case Remarks:
                            content = reportRow.RemarkText;
                            break;
                    }
                }
                else if (openXmlElementDataContext.DataContext is KeyValuePair<String, int>)
                {
                    var keyvalue = (KeyValuePair<String, int>)openXmlElementDataContext.DataContext;
                    switch (tagPlaceHolderValue)
                    {
                        case Isotope:
                            content = keyvalue.Key;
                            break;
                        case Area:
                            content = keyvalue.Value.ToString();
                            break;
                    }
                }
            }

            //just avoid replacing the logo with blank text
            if (tagPlaceHolderValue != Logo && tagPlaceHolderValue != CustomerLogo)
            {
                // Set the content for the content control
                this.SetContentOfContentControl(openXmlElementDataContext.Element as SdtElement, content);
            }
        }

        void SetLogo(SdtElement element, byte[] image, WordprocessingDocument document)
        {
            // Assuming that the first image is the logo placeholder. This is a reasonable assumption.
            var blip = element.Descendants<Blip>().FirstOrDefault();

            string imgId = "LogoPicture";
            
            //assuming only one headerpart will have images
            var imagePart = document.MainDocumentPart.HeaderParts
                                .Where(p=>p.ImageParts.Count() > 0)
                                .First()
                                .AddImagePart(ImagePartType.Png, imgId);
                
            using(MemoryStream imageStream = new MemoryStream(image))
            {
                imagePart.FeedData(imageStream);
            }
            if (blip != null)
            {
                blip.Embed = imgId;
            }
        }


        /// <summary>
        /// Recursive placeholder found.
        /// </summary>
        /// <param name="placeholderTag">The placeholder tag.</param>
        /// <param name="openXmlElementDataContext">The open XML element data context.</param>
        protected override void RecursivePlaceholderFound(string placeholderTag, OpenXmlElementDataContext openXmlElementDataContext, WordprocessingDocument document)
        {
            if (openXmlElementDataContext == null || openXmlElementDataContext.Element == null || openXmlElementDataContext.DataContext == null)
            {
                return;
            }

            string tagPlaceHolderValue = string.Empty;
            string tagGuidPart = string.Empty;
            GetTagValue(openXmlElementDataContext.Element as SdtElement, out tagPlaceHolderValue, out tagGuidPart);

            switch (tagPlaceHolderValue)
            {
                case RGReportRow:
                    foreach (var row in ((openXmlElementDataContext.DataContext) as FinalRTReport).FinalRTReportRows)
                    {
                        SdtElement clonedElement = this.CloneElementAndSetContentInPlaceholders(new OpenXmlElementDataContext() { Element = openXmlElementDataContext.Element, DataContext = row }, document);
                    }
                    openXmlElementDataContext.Element.Remove();
                    break;
                case IsotopeCollection:
                case AreaCollection:
                    foreach (var pair in ((openXmlElementDataContext.DataContext) as FinalRTReport).EnergyAreas)
                    {
                        SdtElement clonedElement = this.CloneElementAndSetContentInPlaceholders(new OpenXmlElementDataContext() { Element = openXmlElementDataContext.Element, DataContext = pair }, document);
                    }
                    openXmlElementDataContext.Element.Remove();
                    break;
            }
        }

        #endregion

        protected override void IgnorePlaceholderFound(string placeholderTag, OpenXmlElementDataContext openXmlElementDataContext, WordprocessingDocument document)
        {
            throw new System.NotImplementedException();
        }

        protected override void ContainerPlaceholderFound(string placeholderTag, OpenXmlElementDataContext openXmlElementDataContext, WordprocessingDocument document)
        {
            throw new System.NotImplementedException();
        }
    }
}