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
    using Models;
    using System;
    using System.IO;
    using DocumentFormat.OpenXml.Packaging;
    using System.Linq;
    using DocumentFormat.OpenXml.Drawing;

    /// <summary>
    /// Sample refreshable document generator for Test_Template - 2.docx(has table) template
    /// </summary>
    [CLSCompliant(false)]
    public class FinalRGReportGenerator : DocumentGenerator
    {
        #region contentcontroltags
        protected const string FPNo = "FPNo";
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
        protected const string ReportTypeNo = "ReportTypeNo";

        protected const string RGReportRow = "RGReportRow";
        protected const string SlNo = "SlNo";
        protected const string Location = "Location";
        protected const string Segment = "Segment";
        protected const string LocationSegment = "LocationSegment";
        protected const string Thickness = "Thickness";
        protected const string SFD = "SFD";
        protected const string Designation = "Designation";
        protected const string Sensitivity = "Sensitivity";
        protected const string Density = "Density";
        protected const string FilmSize = "FilmSize";
        protected const string Observation = "Observation";
        protected const string Remarks = "Remarks";
        protected const string Technique = "Technique";

        protected const string Result = "Result";
        protected const string TotalArea = "TotalArea";
        protected const string Isotope = "Isotope";
        protected const string Area = "Area";
        protected const string ExposedTotalArea = "ExposedTotalArea";
        protected const string RetakeTotalArea = "RetakeTotalArea";
   
        protected const string IsotopeCollection = "IsotopeCollection";
        protected const string ExposedIsotopeCollection = "ExposedIsotopeCollection";
        protected const string RetakeIsotopeCollection = "RetakeIsotopeCollection";
   
        protected const string AreaCollection = "AreaCollection";
        protected const string ExposedAreaCollection = "ExposedAreaCollection";
        protected const string RetakeAreaCollection = "RetakeAreaCollection";
   
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
            var placeHolderTagToTypeCollection = new Dictionary<string, PlaceHolderType>
                {
                    {FPNo, PlaceHolderType.NonRecursive},
                    {CustomerName, PlaceHolderType.NonRecursive},
                    {Description, PlaceHolderType.NonRecursive},
                    {Specification, PlaceHolderType.NonRecursive},
                    {DrawingNo, PlaceHolderType.NonRecursive},
                    {RTNo, PlaceHolderType.NonRecursive},
                    {HeatNo, PlaceHolderType.NonRecursive},
                    {Coverage, PlaceHolderType.NonRecursive},
                    {ProcedureRef, PlaceHolderType.NonRecursive},
                    {ReportNo, PlaceHolderType.NonRecursive},
                    {Date, PlaceHolderType.NonRecursive},
                    {Film, PlaceHolderType.NonRecursive},
                    {LeadScreen, PlaceHolderType.NonRecursive},
                    {Source, PlaceHolderType.NonRecursive},
                    {DateOfTest, PlaceHolderType.NonRecursive},
                    {Evaluation, PlaceHolderType.NonRecursive},
                    {Acceptance, PlaceHolderType.NonRecursive},
                    {Logo, PlaceHolderType.NonRecursive},
                    {CustomerLogo, PlaceHolderType.NonRecursive},
                    {CustomerAddress, PlaceHolderType.NonRecursive},
                    {CustomerEmail, PlaceHolderType.NonRecursive},
                    {CustomerPhone, PlaceHolderType.NonRecursive},
                    {ReportTypeNo, PlaceHolderType.NonRecursive},

                    {RGReportRow, PlaceHolderType.Recursive},
                    {SlNo, PlaceHolderType.NonRecursive},
                    {Location, PlaceHolderType.NonRecursive},
                    {Segment, PlaceHolderType.NonRecursive},
                    {LocationSegment, PlaceHolderType.NonRecursive},
                    {Thickness, PlaceHolderType.NonRecursive},
                    {SFD, PlaceHolderType.NonRecursive},
                    {Designation, PlaceHolderType.NonRecursive},
                    {Sensitivity, PlaceHolderType.NonRecursive},
                    {Density, PlaceHolderType.NonRecursive},
                    {FilmSize, PlaceHolderType.NonRecursive},
                    {Observation, PlaceHolderType.NonRecursive},
                    {Remarks, PlaceHolderType.NonRecursive},
                    {Result, PlaceHolderType.NonRecursive},
                    {Technique, PlaceHolderType.NonRecursive},
                    
                    {TotalArea, PlaceHolderType.NonRecursive},
                    {ExposedTotalArea, PlaceHolderType.NonRecursive},
                    {RetakeTotalArea, PlaceHolderType.NonRecursive},
   
                    {Isotope, PlaceHolderType.NonRecursive},
                    {Area, PlaceHolderType.NonRecursive},
                    {IsotopeCollection, PlaceHolderType.Recursive},
                    {AreaCollection, PlaceHolderType.Recursive},
                    {ExposedIsotopeCollection, PlaceHolderType.Recursive},
                    {ExposedAreaCollection, PlaceHolderType.Recursive},
                    {RetakeIsotopeCollection, PlaceHolderType.Recursive},
                    {RetakeAreaCollection, PlaceHolderType.Recursive}
   
                };
            
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

            string tagPlaceHolderValue;
            string tagGuidPart;
            GetTagValue(openXmlElementDataContext.Element as SdtElement, out tagPlaceHolderValue, out tagGuidPart);

            var content = string.Empty;

            if (openXmlElementDataContext.DataContext is FinalRTReport)
            {
                var row = openXmlElementDataContext.DataContext as FinalRTReport;

                switch (tagPlaceHolderValue)
                {
                    case FPNo:
                        content = row.FixedPattern.FPNo;
                        break;
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
                        content = row.TotalArea;
                        break;
                    case ExposedTotalArea:
                        content = row.ExposedTotalArea;
                        break;
                    case RetakeTotalArea:
                        content = row.RetakeTotalArea;
                        break;

                    case Result:
                        content = row.Status.Status;
                        break;
                    case Logo:
                        //get the company logo
                        var logo = row.GetCompanyLogo();
                        SetLogo(openXmlElementDataContext.Element as SdtElement, logo, document);
                        break;
                    case CustomerLogo:
                        var customerLogo = row.GetCustomerLogo();
                        SetLogo(openXmlElementDataContext.Element as SdtElement, customerLogo, document);
                        break;
                    case ReportTypeNo:
                        content = row.ReportTypeNo;
                        break;
                }
            }
            else if (openXmlElementDataContext.DataContext is FinalRTReportRow)
            {
                var reportRow = openXmlElementDataContext.DataContext as FinalRTReportRow;

                switch (tagPlaceHolderValue)
                {
                    case SlNo:
                        content = reportRow.SlNo.ToString();
                        break;
                    case Technique:
                        content = reportRow.Technique;
                        break;
                    case LocationSegment:
                        content = reportRow.LocationAndSegment;
                        break;
                    case Location:
                        content = reportRow.Location;
                        break;
                    case Segment:
                        content = reportRow.Segment;
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
                        content = reportRow.FilmSizeWithCount;
                        break;
                    case Observation:
                        content = reportRow.Observations;
                        break;
                    case Remarks:
                        content = reportRow.RemarkText;
                        break;
                }
            }
            else if (openXmlElementDataContext.DataContext is KeyValuePair<String, float>)
            {
                var keyvalue = (KeyValuePair<String, float>)openXmlElementDataContext.DataContext;
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

            //just avoid replacing the logo with blank text
            if (tagPlaceHolderValue != Logo && tagPlaceHolderValue != CustomerLogo)
            {
                // Set the content for the content control
                SetContentOfContentControl(openXmlElementDataContext.Element as SdtElement, content);
            }
        }

        void SetLogo(SdtElement element, byte[] image, WordprocessingDocument document)
        {
            try
            {
                // Assuming that the first image is the logo placeholder. This is a reasonable assumption.
                var blip = element.Descendants<Blip>().FirstOrDefault();

                string imgId = "LogoPicture";

                //assuming only one headerpart will have images
                var imagePart = document.MainDocumentPart.HeaderParts.First(p => p.ImageParts.Any())
                                    .AddImagePart(ImagePartType.Png, imgId);

                using (var imageStream = new MemoryStream(image))
                {
                    imagePart.FeedData(imageStream);
                }
                if (blip != null)
                {
                    blip.Embed = imgId;
                }
            }
            catch (Exception e)
            {
                //do nothing - just image will not be replaced
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

            string tagPlaceHolderValue;
            string tagGuidPart;
            GetTagValue(openXmlElementDataContext.Element as SdtElement, out tagPlaceHolderValue, out tagGuidPart);

            switch (tagPlaceHolderValue)
            {
                case RGReportRow:
                    foreach (var row in ((FinalRTReport) (openXmlElementDataContext.DataContext)).FinalRTReportRows)
                    {
                        CloneElementAndSetContentInPlaceholders(new OpenXmlElementDataContext() { Element = openXmlElementDataContext.Element, DataContext = row }, document);
                    }
                    openXmlElementDataContext.Element.Remove();
                    break;
                case IsotopeCollection:
                case AreaCollection:
                    foreach (var pair in ((FinalRTReport) (openXmlElementDataContext.DataContext)).EnergyAreas)
                    {
                        CloneElementAndSetContentInPlaceholders(new OpenXmlElementDataContext() { Element = openXmlElementDataContext.Element, DataContext = pair }, document);
                    }
                    openXmlElementDataContext.Element.Remove();
                    break;
                case ExposedIsotopeCollection:
                case ExposedAreaCollection:
                    foreach (var pair in ((FinalRTReport)(openXmlElementDataContext.DataContext)).ExposedEnergyAreas)
                    {
                        CloneElementAndSetContentInPlaceholders(new OpenXmlElementDataContext() { Element = openXmlElementDataContext.Element, DataContext = pair }, document);
                    }
                    openXmlElementDataContext.Element.Remove();
                    break;
                case RetakeIsotopeCollection:
                case RetakeAreaCollection:
                    foreach (var pair in ((FinalRTReport)(openXmlElementDataContext.DataContext)).RetakeEnergyAreas)
                    {
                        CloneElementAndSetContentInPlaceholders(new OpenXmlElementDataContext() { Element = openXmlElementDataContext.Element, DataContext = pair }, document);
                    }
                    openXmlElementDataContext.Element.Remove();
                    break;
            }
        }

        #endregion

        protected override void IgnorePlaceholderFound(string placeholderTag, OpenXmlElementDataContext openXmlElementDataContext, WordprocessingDocument document)
        {
            throw new NotImplementedException();
        }

        protected override void ContainerPlaceholderFound(string placeholderTag, OpenXmlElementDataContext openXmlElementDataContext, WordprocessingDocument document)
        {
            throw new NotImplementedException();
        }
    }
}