﻿using RadiographyTracking.Web.Utility;

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
    public class RGReportGenerator : DocumentGenerator
    {
        #region contentcontroltags
        protected const string FPNo = "FPNo";
        protected const string CustomerName = "CustomerName";
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
        protected const string LeadScreenBack = "LeadScreenBack";
        protected const string Source = "Source";
        protected const string Strength = "Strength";
        protected const string SourceSize = "SourceSize";
        protected const string DateOfTest = "DateOfTest";
        protected const string Evaluation = "Evaluation";
        protected const string Acceptance = "Acceptance";
        protected const string Logo = "Logo";
        protected const string ReportTypeNo = "ReportTypeNo";
        protected const string Viewing = "Viewing";
        protected const string EndCustomer = "EndCustomer";
        protected const string HeaderTechnique = "HeaderTechnique";

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

        protected const string Finding = "Finding";
        protected const string Classification = "Classification";

        protected const string Remarks = "Remarks";
        protected const string Technique = "Technique";

        protected const string Result = "Result";
        protected const string TotalArea = "TotalArea";
        protected const string TotalFilmCount = "TotalFilmCount";
        protected const string ExposedTotalArea = "ExposedTotalArea";
        protected const string RetakeTotalArea = "RetakeTotalArea";
        protected const string Isotope = "Isotope";
        protected const string Area = "Area";
        protected const string IsotopeCollection = "IsotopeCollection";
        protected const string ExposedIsotopeCollection = "ExposedIsotopeCollection";
        protected const string RetakeIsotopeCollection = "RetakeIsotopeCollection";
        protected const string AreaCollection = "AreaCollection";
        protected const string ExposedAreaCollection = "ExposedAreaCollection";
        protected const string RetakeAreaCollection = "RetakeAreaCollection";

        protected const string InchCms = "InchCms";
        protected const string InchsCms = "InchsCms";

        #endregion

        public bool IsFilmSizeInCms { get; set; }

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SampleDocumentWithTableGenerator"/> class.
        /// </summary>
        /// <param name="generationInfo">The generation info.</param>
        public RGReportGenerator(DocumentGenerationInfo generationInfo)
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
                    {LeadScreenBack, PlaceHolderType.NonRecursive},
                    {Source, PlaceHolderType.NonRecursive},
                    {SourceSize, PlaceHolderType.NonRecursive},
                    {Strength, PlaceHolderType.NonRecursive},
                    {DateOfTest, PlaceHolderType.NonRecursive},
                    {Evaluation, PlaceHolderType.NonRecursive},
                    {Acceptance, PlaceHolderType.NonRecursive},
                    {Logo, PlaceHolderType.NonRecursive},
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
                    {Finding, PlaceHolderType.NonRecursive},
                    {Classification, PlaceHolderType.NonRecursive},
                    {Viewing, PlaceHolderType.NonRecursive},
                    {Remarks, PlaceHolderType.NonRecursive},
                    {Technique, PlaceHolderType.NonRecursive},
                    {Result, PlaceHolderType.NonRecursive},
                    {TotalArea, PlaceHolderType.NonRecursive},
                    {TotalFilmCount, PlaceHolderType.NonRecursive},
                    {ExposedTotalArea, PlaceHolderType.NonRecursive},
                    {RetakeTotalArea, PlaceHolderType.NonRecursive},
                    {Isotope, PlaceHolderType.NonRecursive},
                    {Area, PlaceHolderType.NonRecursive},
                    {IsotopeCollection, PlaceHolderType.Recursive},
                    {AreaCollection, PlaceHolderType.Recursive},
                    {ExposedIsotopeCollection, PlaceHolderType.Recursive},
                    {ExposedAreaCollection, PlaceHolderType.Recursive},
                    {RetakeIsotopeCollection, PlaceHolderType.Recursive},
                    {EndCustomer, PlaceHolderType.NonRecursive},
                    {HeaderTechnique, PlaceHolderType.NonRecursive},
                    {RetakeAreaCollection, PlaceHolderType.Recursive},

                    {InchCms, PlaceHolderType.NonRecursive},
                    {InchsCms, PlaceHolderType.NonRecursive}
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
            if (openXmlElementDataContext == null || openXmlElementDataContext.Element == null ||
                openXmlElementDataContext.DataContext == null)
                return;

            string tagPlaceHolderValue;
            string tagGuidPart;
            GetTagValue(openXmlElementDataContext.Element as SdtElement, out tagPlaceHolderValue, out tagGuidPart);
            var content = string.Empty;

            if (openXmlElementDataContext.DataContext is RGReport)
            {
                var row = openXmlElementDataContext.DataContext as RGReport;

                switch (tagPlaceHolderValue)
                {
                    case FPNo:
                        content = row.FixedPattern.FPNo;
                        break;
                    case CustomerName:
                        content = row.FixedPattern.Customer.CustomerName;
                        break;
                    case ReportNo:
                        content = row.ReportNo;
                        break;
                    case Coverage:
                        content = row.Coverage.CoverageName;
                        break;
                    case ProcedureRef:
                        content = row.ProcedureRef;
                        break;
                    case Source:
                        content = row.Source;
                        break;
                    case SourceSize:
                        content = row.SourceSize;
                        break;
                    case Strength:
                        content = row.Strength;
                        break;
                    case Description:
                        content = row.FixedPattern.Description;
                        break;
                    case Specification:
                        content = row.Specifications;
                        break;
                    case DrawingNo:
                        content = row.DrawingNo;
                        break;
                    case Viewing:
                        content = row.Viewing;
                        break;
                    case LeadScreen:
                        content = row.LeadScreen;
                        break;
                    case LeadScreenBack:
                        content = row.LeadScreenBack;
                        break;
                    case HeatNo:
                        content = row.HeatNo;
                        break;
                    case Date:
                        content = row.ReportDate.ToString("dd/MM/yyyy");
                        break;
                    case Film:
                        content = row.Film;
                        break;
                    case RTNo:
                        content = row.RTNo;
                        break;
                    case DateOfTest:
                        content = row.DateOfTest.ToString("dd/MM/yyyy");
                        break;
                    case Evaluation:
                        content = row.EvaluationAsPer;
                        break;
                    case Acceptance:
                        content = row.AcceptanceAsPer;
                        break;
                    case TotalArea:
                        if (!IsFilmSizeInCms)
                            content = row.TotalArea;
                        else
                            content = row.TotalAreaInCms;
                        break;
                    case TotalFilmCount:
                        content = row.TotalFilmCount;
                        break;
                    case ExposedTotalArea:
                       if (!IsFilmSizeInCms)
                            content = row.ExposedTotalArea;
                        else
                            content = row.ExposedTotalAreaInCms;
                        break;
                    case RetakeTotalArea:
                        if (!IsFilmSizeInCms)
                            content = row.RetakeTotalArea;
                        else
                            content = row.RetakeTotalAreaInCms;
                        break;
                    case Result:
                        content = row.Status.Status;
                        break;
                    case Logo:
                        //get the company logo
                        var logo = row.getCompanyLogo();
                        SetLogo(openXmlElementDataContext.Element as SdtElement, logo, document);
                        break;
                    case ReportTypeNo:
                        content = row.ReportTypeAndNo;
                        break;
                    case EndCustomer:
                        content = row.EndCustomerName;
                        break;
                    case HeaderTechnique:
                        content = GetTechniqueWithComma(row.RGReportRows);
                        break;

                    case InchCms:
                        if (!IsFilmSizeInCms)
                            content = "Inch";
                        else
                            content = "Cm";
                        break;

                    case InchsCms:
                        if (!IsFilmSizeInCms)
                            content = "Inches";
                        else
                            content = "Cms";
                        break;
                }
            }
            else if (openXmlElementDataContext.DataContext is RGReportRow)
            {
                var reportRow = openXmlElementDataContext.DataContext as RGReportRow;

                switch (tagPlaceHolderValue)
                {
                    case SlNo:
                        content = reportRow.SlNo.ToString();
                        break;
                    case Location:
                        content = reportRow.Location;
                        break;
                    case Segment:
                        content = reportRow.Segment;
                        break;
                    case LocationSegment:
                        content = reportRow.LocationAndSegment;
                        break;
                    case Thickness:
                        content = reportRow.ThicknessRange;
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
                        if (!IsFilmSizeInCms)
                            content = reportRow.FilmSizeWithCount;
                        else
                            content = reportRow.FilmSizeWithCountInCms;
                        break;
                    case Observation:
                        content = reportRow.Observations;
                        break;
                    case Finding:
                        content = reportRow.Findings;
                        break;
                    case Classification:
                        content = reportRow.Classifications;
                        break;
                    case Remarks:
                        content = reportRow.RemarkText;
                        break;
                    case Technique:
                        content = reportRow.Technique;
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

            if (tagPlaceHolderValue != Logo)
            {
                // Set the content for the content control
                this.SetContentOfContentControl(openXmlElementDataContext.Element as SdtElement, content);
            }
        }

        void SetLogo(SdtElement element, byte[] image, WordprocessingDocument document)
        {
            try
            {
                var blip = element.Descendants<Blip>().FirstOrDefault();

                var imgId = "LogoPicture";

                var existingImage = document.MainDocumentPart.GetPartById(blip.Embed);

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
                    foreach (var row in ((RGReport)(openXmlElementDataContext.DataContext)).RGReportRows)
                    {
                        CloneElementAndSetContentInPlaceholders(new OpenXmlElementDataContext() { Element = openXmlElementDataContext.Element, DataContext = row }, document);
                    }
                    openXmlElementDataContext.Element.Remove();
                    break;
                case IsotopeCollection:
                case AreaCollection:
                    if (!IsFilmSizeInCms)
                    {
                        foreach (var pair in ((RGReport)(openXmlElementDataContext.DataContext)).EnergyAreas)
                        {
                            CloneElementAndSetContentInPlaceholders(new OpenXmlElementDataContext() { Element = openXmlElementDataContext.Element, DataContext = pair }, document);
                        }
                    }
                    else
                    {
                        foreach (var pair in ((RGReport)(openXmlElementDataContext.DataContext)).EnergyAreasInCms)
                        {
                            CloneElementAndSetContentInPlaceholders(new OpenXmlElementDataContext() { Element = openXmlElementDataContext.Element, DataContext = pair }, document);
                        }
                    }
                    openXmlElementDataContext.Element.Remove();
                    break;
                case ExposedIsotopeCollection:
                case ExposedAreaCollection:
                    if (!IsFilmSizeInCms)
                    {
                        foreach (var pair in ((RGReport)(openXmlElementDataContext.DataContext)).ExposedEnergyAreas)
                        {
                            CloneElementAndSetContentInPlaceholders(new OpenXmlElementDataContext() { Element = openXmlElementDataContext.Element, DataContext = pair }, document);
                        }
                    }
                    else
                    {
                        foreach (var pair in ((RGReport)(openXmlElementDataContext.DataContext)).ExposedEnergyAreasInCms)
                        {
                            CloneElementAndSetContentInPlaceholders(new OpenXmlElementDataContext() { Element = openXmlElementDataContext.Element, DataContext = pair }, document);
                        }
                    }
                    openXmlElementDataContext.Element.Remove();
                    break;
                case RetakeIsotopeCollection:
                case RetakeAreaCollection:
                    if (!IsFilmSizeInCms)
                    {
                        foreach (var pair in ((RGReport)(openXmlElementDataContext.DataContext)).RetakeEnergyAreas)
                        { 
                            CloneElementAndSetContentInPlaceholders(new OpenXmlElementDataContext() { Element = openXmlElementDataContext.Element, DataContext = pair }, document);
                        }
                    }
                    else
                    {
                        foreach (var pair in ((RGReport)(openXmlElementDataContext.DataContext)).RetakeEnergyAreasInCms)
                        {
                            CloneElementAndSetContentInPlaceholders(new OpenXmlElementDataContext() { Element = openXmlElementDataContext.Element, DataContext = pair }, document);
                        }
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

        protected string GetTechniqueWithComma(ICollection<RGReportRow> FinalRTReportRows)
        {
            return string.Join(",", FinalRTReportRows.Select(p => p.Technique).Distinct()); ;
        }
    }
}