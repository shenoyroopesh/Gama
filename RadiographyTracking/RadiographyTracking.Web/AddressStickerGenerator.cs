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
    public class AddressStickerGenerator : DocumentGenerator
    {
        #region contentcontroltags
        protected const string AddressLabelRow = "AddressLabelRow";
        protected const string AddressStickerCol1 = "AddressStickerCol1";
        protected const string CustomerName1 = "CustomerName1";
        protected const string City1 = "City1";
        protected const string EndCustomerName1 = "EndCustomerName1";
        protected const string Description1 = "Description1";
        protected const string RTNo1 = "RTNo1";
        protected const string Location1 = "Location1";
        protected const string HeatNo1 = "HeatNo1";
        protected const string Designation1 = "Designation1";
        protected const string Thickness1 = "Thickness1";
        protected const string Density1 = "Density1";
        protected const string Energy1 = "Energy1";
        protected const string Sensitivity1 = "Sensitivity1";
        protected const string Technique1 = "Technique1";
        protected const string DateOfTest1 = "DateOfTest1";
        protected const string Specification1 = "Specification1";


        protected const string AddressStickerCol2 = "AddressStickerCol2";
        protected const string CustomerName2 = "CustomerName2";
        protected const string City2 = "City2";
        protected const string EndCustomerName2 = "EndCustomerName2";
        protected const string Description2 = "Description2";
        protected const string RTNo2 = "RTNo2";
        protected const string Location2 = "Location2";
        protected const string HeatNo2 = "HeatNo2";
        protected const string Designation2 = "Designation2";
        protected const string Thickness2 = "Thickness2";
        protected const string Density2 = "Density2";
        protected const string Energy2 = "Energy2";
        protected const string Sensitivity2 = "Sensitivity2";
        protected const string Technique2 = "Technique2";
        protected const string DateOfTest2 = "DateOfTest2";
        protected const string Specification2 = "Specification2";
        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SampleDocumentWithTableGenerator"/> class.
        /// </summary>
        /// <param name="generationInfo">The generation info.</param>
        public AddressStickerGenerator(DocumentGenerationInfo generationInfo)
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
                    {AddressLabelRow, PlaceHolderType.Recursive},
                    {AddressStickerCol1, PlaceHolderType.NonRecursive},
                    {CustomerName1, PlaceHolderType.NonRecursive},
                    {City1, PlaceHolderType.NonRecursive},
                    {EndCustomerName1, PlaceHolderType.NonRecursive},
                    {Description1, PlaceHolderType.NonRecursive},
                    {RTNo1, PlaceHolderType.NonRecursive},
                    {Location1, PlaceHolderType.NonRecursive},
                    {HeatNo1, PlaceHolderType.NonRecursive},
                    {Designation1, PlaceHolderType.NonRecursive},
                    {Thickness1, PlaceHolderType.NonRecursive},
                    {Density1, PlaceHolderType.NonRecursive},
                    {Energy1, PlaceHolderType.NonRecursive},
                    {Sensitivity1, PlaceHolderType.NonRecursive},
                    {Technique1, PlaceHolderType.NonRecursive},
                    {DateOfTest1, PlaceHolderType.NonRecursive},
                    {Specification1, PlaceHolderType.NonRecursive},
                    {AddressStickerCol2, PlaceHolderType.NonRecursive},
                    {CustomerName2, PlaceHolderType.NonRecursive},
                    {City2, PlaceHolderType.NonRecursive},
                    {EndCustomerName2, PlaceHolderType.NonRecursive},
                    {Description2, PlaceHolderType.NonRecursive},
                    {RTNo2, PlaceHolderType.NonRecursive},
                    {Location2, PlaceHolderType.NonRecursive},
                    {HeatNo2, PlaceHolderType.NonRecursive},
                    {Designation2, PlaceHolderType.NonRecursive},
                    {Thickness2, PlaceHolderType.NonRecursive},
                    {Density2, PlaceHolderType.NonRecursive},
                    {Energy2, PlaceHolderType.NonRecursive},
                    {Sensitivity2, PlaceHolderType.NonRecursive},
                    {Technique2, PlaceHolderType.NonRecursive},
                    {DateOfTest2, PlaceHolderType.NonRecursive},
                    {Specification2, PlaceHolderType.NonRecursive}
                };

            // Handle Rowlevel place holders

            //within each row, these are non-recursive
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


            var reportRow = openXmlElementDataContext.DataContext as AddressStickerRow;

            if (reportRow != null)
            {
                if (reportRow.AddressLabelCol1 != null)
                {
                    switch (tagPlaceHolderValue)
                    {
                        case AddressStickerCol1:
                            foreach (var v in openXmlElementDataContext.Element.Elements())
                            {
                                this.SetContentInPlaceholders(new OpenXmlElementDataContext() 
                                    { Element = v, DataContext = openXmlElementDataContext.DataContext }, document);
                            }
                            break;

                        case CustomerName1:
                            content = reportRow.AddressLabelCol1.RGReport.FixedPattern.Customer.CustomerName;
                            break;

                        case City1:
                            content = reportRow.AddressLabelCol1.RGReport.FixedPattern.Customer.Address;
                            break;
                        case EndCustomerName1:
                            content = reportRow.AddressLabelCol1.RGReport.EndCustomerName;
                            break;

                        case Description1:
                            content = reportRow.AddressLabelCol1.RGReport.FixedPattern.Description;
                            break;

                        case RTNo1:
                            content = reportRow.AddressLabelCol1.RGReport.RTNo;
                            break;

                        case Location1:
                            content = reportRow.AddressLabelCol1.LocationAndSegment;
                            break;
                        case HeatNo1:
                            content = reportRow.AddressLabelCol1.RGReport.HeatNo;
                            break;

                        case Designation1:
                            content = reportRow.AddressLabelCol1.Designation;
                            break;

                        case Thickness1:
                            content = reportRow.AddressLabelCol1.Thickness.ToString();
                            break;

                        case Density1:
                            content = reportRow.AddressLabelCol1.Density;
                            break;

                        case Energy1:
                            content = reportRow.AddressLabelCol1.Energy.Name;
                            break;

                        case Sensitivity1:
                            content = reportRow.AddressLabelCol1.Sensitivity;
                            break;
                        case Technique1:
                            content = reportRow.AddressLabelCol1.Technique;
                            break;
                        case DateOfTest1:
                            content = reportRow.AddressLabelCol1.RGReport.DateOfTest.ToString("dd-MM-yyyy");
                            break;
                        case Specification1:
                            content = reportRow.AddressLabelCol1.RGReport.Specifications;
                            break;
                    }
                }
                else
                {
                    if (placeholderTag == AddressStickerCol1)
                        content = " ";

                }

                if (reportRow.AddressLabelCol2 != null)
                {
                    switch (placeholderTag)
                    {
                        case AddressStickerCol2:
                            foreach (var v in openXmlElementDataContext.Element.Elements())
                            {
                                this.SetContentInPlaceholders(new OpenXmlElementDataContext() { Element = v, DataContext = openXmlElementDataContext.DataContext }, document);
                            }
                            break;

                        case CustomerName2:
                            content = reportRow.AddressLabelCol2.RGReport.FixedPattern.Customer.CustomerName;
                            break;

                        case City2:
                            content = reportRow.AddressLabelCol2.RGReport.FixedPattern.Customer.Address;
                            break;
                        case EndCustomerName2:
                            content = reportRow.AddressLabelCol2.RGReport.EndCustomerName;
                            break;

                        case Description2:
                            content = reportRow.AddressLabelCol2.RGReport.FixedPattern.Description;
                            break;

                        case RTNo2:
                            content = reportRow.AddressLabelCol2.RGReport.RTNo;
                            break;

                        case Location2:
                            content = reportRow.AddressLabelCol2.LocationAndSegment;
                            break;
                        case HeatNo2:
                            content = reportRow.AddressLabelCol2.RGReport.HeatNo;
                            break;

                        case Designation2:
                            content = reportRow.AddressLabelCol2.Designation;
                            break;

                        case Thickness2:
                            content = reportRow.AddressLabelCol2.Thickness.ToString();
                            break;

                        case Density2:
                            content = reportRow.AddressLabelCol2.Density;
                            break;

                        case Energy2:
                            content = reportRow.AddressLabelCol2.Energy.Name;
                            break;

                        case Sensitivity2:
                            content = reportRow.AddressLabelCol2.Sensitivity;
                            break;
                        case Technique2:
                            content = reportRow.AddressLabelCol2.Technique;
                            break;
                        case DateOfTest2:
                            content = reportRow.AddressLabelCol2.RGReport.DateOfTest.ToString("dd-MM-yyyy");
                            break;
                        case Specification2:
                            content = reportRow.AddressLabelCol2.RGReport.Specifications;
                            break;

                    }
                }
                else
                {
                    if (placeholderTag == AddressStickerCol2)
                        content = " ";
                }
            }

            // Set the content for the content control
            if (content != "")
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
                case AddressLabelRow:
                    foreach (AddressStickerRow row in openXmlElementDataContext.DataContext as IEnumerable<AddressStickerRow>)
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