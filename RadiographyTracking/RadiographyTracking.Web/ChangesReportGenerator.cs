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
    public class ChangesReportGenerator : DocumentGenerator
    {
        #region contentcontroltags
        protected const string FoundryName = "FoundryName";
        protected const string FromDate = "FromDate";
        protected const string ToDate = "ToDate";

        protected const string ChangeReportRow = "ChangeReportRow";
        protected const string When = "When";
        protected const string What = "What";
        protected const string Where = "Where";        
        protected const string FromValue = "FromValue";
        protected const string ToValue = "ToValue";
        protected const string ByWhom = "ByWhom";
        protected const string Why = "Why";        
        #endregion

        private String foundryName;
        private String fromDate;
        private String toDate;

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SampleDocumentWithTableGenerator"/> class.
        /// </summary>
        /// <param name="generationInfo">The generation info.</param>
        public ChangesReportGenerator(DocumentGenerationInfo generationInfo, String foundryName, String fromDate, String toDate)
            : base(generationInfo)
        {
            this.foundryName = foundryName;
            this.fromDate = fromDate;
            this.toDate = toDate;
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
            placeHolderTagToTypeCollection.Add(FoundryName, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(FromDate, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(ToDate, PlaceHolderType.NonRecursive);

            // Handle Rowlevel place holders
            placeHolderTagToTypeCollection.Add(ChangeReportRow, PlaceHolderType.Recursive);

            //within each row, these are non-recursive
            placeHolderTagToTypeCollection.Add(What, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(Where, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(When, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(FromValue, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(ToValue, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(ByWhom, PlaceHolderType.NonRecursive);
            placeHolderTagToTypeCollection.Add(Why, PlaceHolderType.NonRecursive);

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

            string tagValue = string.Empty;
            string content = string.Empty;

            switch (tagPlaceHolderValue)
            {
                case FoundryName:
                    content = this.foundryName;
                    break;
                case FromDate:
                    content = this.fromDate;
                    break;
                case ToDate:
                    content = this.toDate;
                    break;
            }

            var reportRow = openXmlElementDataContext.DataContext as Change;

            if (reportRow != null)
            {
                switch (tagPlaceHolderValue)
                {
                    case What:
                        content = reportRow.What;
                        break;
                    case Where:
                        content = reportRow.Where;
                        break;
                    case When:
                        content = reportRow.When.ToString("dd-MM-yyyy");
                        break;
                    case FromValue:
                        content = reportRow.FromValue;
                        break;
                    case ToValue:
                        content = reportRow.ToValue;
                        break;
                    case ByWhom:
                        content = reportRow.ByWhom;
                        break;
                    case Why:
                        content = reportRow.Why;
                        break;
                }
            } 
            
            // Set the content for the content control
            this.SetContentOfContentControl(openXmlElementDataContext.Element as SdtElement, content);
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
                case ChangeReportRow:
                    foreach (Change row in openXmlElementDataContext.DataContext as IEnumerable<Change>)
                    {
                        SdtElement clonedElement = this.CloneElementAndSetContentInPlaceholders(new OpenXmlElementDataContext() { Element = openXmlElementDataContext.Element, DataContext = row }, document);
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