﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WordDocumentGenerator.Library;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using RadiographyTracking.Web.Models;

namespace RadiographyTracking.Web
{
    [CLSCompliant(false)]
    public class DummyAddressStickerGenerator : DocumentGenerator
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
        protected const string LocationOnly1 = "LocationOnly1";
        protected const string Segment1 = "Segment1";
        protected const string ThicknessRange1 = "ThicknessRange1";
        protected const string FilmSize1 = "Filmsize1";

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
        protected const string LocationOnly2 = "LocationOnly2";
        protected const string Segment2 = "Segment2";
        protected const string ThicknessRange2 = "ThicknessRange2";
        protected const string FilmSize2 = "Filmsize2";

        protected const string AddressStickerCol3 = "AddressStickerCol3";
        protected const string CustomerName3 = "CustomerName3";
        protected const string City3 = "City3";
        protected const string EndCustomerName3 = "EndCustomerName3";
        protected const string Description3 = "Description3";
        protected const string RTNo3 = "RTNo3";
        protected const string Location3 = "Location3";
        protected const string HeatNo3 = "HeatNo3";
        protected const string Designation3 = "Designation3";
        protected const string Thickness3 = "Thickness3";
        protected const string Density3 = "Density3";
        protected const string Energy3 = "Energy3";
        protected const string Sensitivity3 = "Sensitivity3";
        protected const string Technique3 = "Technique3";
        protected const string DateOfTest3 = "DateOfTest3";
        protected const string Specification3 = "Specification3";
        protected const string LocationOnly3 = "LocationOnly3";
        protected const string Segment3 = "Segment3";
        protected const string ThicknessRange3 = "ThicknessRange3";
        protected const string FilmSize3 = "Filmsize3";
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SampleDocumentWithTableGenerator"/> class.
        /// </summary>
        /// <param name="generationInfo">The generation info.</param>
        public DummyAddressStickerGenerator(DocumentGenerationInfo generationInfo)
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
                    {LocationOnly1, PlaceHolderType.NonRecursive},
                    {Segment1, PlaceHolderType.NonRecursive},
                    {ThicknessRange1, PlaceHolderType.NonRecursive},
                    {FilmSize1, PlaceHolderType.NonRecursive},

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
                    {Specification2, PlaceHolderType.NonRecursive},
                    {LocationOnly2, PlaceHolderType.NonRecursive},
                    {Segment2, PlaceHolderType.NonRecursive},
                    {ThicknessRange2, PlaceHolderType.NonRecursive},
                    {FilmSize2, PlaceHolderType.NonRecursive},

                    {AddressStickerCol3, PlaceHolderType.NonRecursive},
                    {CustomerName3, PlaceHolderType.NonRecursive},
                    {City3, PlaceHolderType.NonRecursive},
                    {EndCustomerName3, PlaceHolderType.NonRecursive},
                    {Description3, PlaceHolderType.NonRecursive},
                    {RTNo3, PlaceHolderType.NonRecursive},
                    {Location3, PlaceHolderType.NonRecursive},
                    {HeatNo3, PlaceHolderType.NonRecursive},
                    {Designation3, PlaceHolderType.NonRecursive},
                    {Thickness3, PlaceHolderType.NonRecursive},
                    {Density3, PlaceHolderType.NonRecursive},
                    {Energy3, PlaceHolderType.NonRecursive},
                    {Sensitivity3, PlaceHolderType.NonRecursive},
                    {Technique3, PlaceHolderType.NonRecursive},
                    {DateOfTest3, PlaceHolderType.NonRecursive},
                    {Specification3, PlaceHolderType.NonRecursive},
                    {LocationOnly3, PlaceHolderType.NonRecursive},
                    {Segment3, PlaceHolderType.NonRecursive},
                    {ThicknessRange3, PlaceHolderType.NonRecursive},
                    {FilmSize3, PlaceHolderType.NonRecursive},
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


            var reportRow = openXmlElementDataContext.DataContext as DummyAddressStickerRow;

            if (reportRow != null)
            {
                if (reportRow.AddressLabelCol1 != null)
                {
                    switch (tagPlaceHolderValue)
                    {
                        case AddressStickerCol1:
                            foreach (var v in openXmlElementDataContext.Element.Elements())
                            {
                                this.SetContentInPlaceholders(new OpenXmlElementDataContext() { Element = v, DataContext = openXmlElementDataContext.DataContext }, document);
                            }
                            break;

                        case RTNo1:
                            content = reportRow.RTNo;
                            break;

                        case Location1:
                            content = reportRow.AddressLabelCol1.Location;
                            break;
                        case HeatNo1:
                            content = reportRow.HeatNo;
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
                            content = reportRow.AddressLabelCol1.EnergyName;
                            break;

                        case Sensitivity1:
                            content = reportRow.AddressLabelCol1.Sensitivity;
                            break;

                        case LocationOnly1:
                            content = reportRow.AddressLabelCol1.Location;
                            break;

                        case Segment1:
                            content = reportRow.AddressLabelCol1.Segment;
                            break;

                        case ThicknessRange1:
                            content = reportRow.AddressLabelCol1.ThicknessRange;
                            break;

                        case FilmSize1:
                            content = reportRow.AddressLabelCol1.FilmSizeString;
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

                        case RTNo2:
                            content = reportRow.RTNo;
                            break;

                        case Location2:
                            content = reportRow.AddressLabelCol2.Location;
                            break;
                        case HeatNo2:
                            content = reportRow.HeatNo;
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
                            content = reportRow.AddressLabelCol2.EnergyName;
                            break;

                        case Sensitivity2:
                            content = reportRow.AddressLabelCol2.Sensitivity;
                            break;

                        case LocationOnly2:
                            content = reportRow.AddressLabelCol2.Location;
                            break;

                        case Segment2:
                            content = reportRow.AddressLabelCol2.Segment;
                            break;

                        case ThicknessRange2:
                            content = reportRow.AddressLabelCol2.ThicknessRange;
                            break;

                        case FilmSize2:
                            content = reportRow.AddressLabelCol2.FilmSizeString;
                            break;
                    }
                }
                else
                {
                    if (placeholderTag == AddressStickerCol2)
                        content = " ";
                }

                if (reportRow.AddressLabelCol3 != null)
                {
                    switch (placeholderTag)
                    {
                        case AddressStickerCol3:
                            foreach (var v in openXmlElementDataContext.Element.Elements())
                            {
                                this.SetContentInPlaceholders(new OpenXmlElementDataContext() { Element = v, DataContext = openXmlElementDataContext.DataContext }, document);
                            }
                            break;

                        case RTNo3:
                            content = reportRow.RTNo;
                            break;

                        case Location3:
                            content = reportRow.AddressLabelCol3.Location;
                            break;

                        case Segment3:
                            content = reportRow.AddressLabelCol3.Segment;
                            break;

                        case FilmSize3:
                            content = reportRow.AddressLabelCol3.FilmSizeString;
                            break;
                    }
                }
                else
                {
                    if (placeholderTag == AddressStickerCol3)
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
                    foreach (DummyAddressStickerRow row in openXmlElementDataContext.DataContext as IEnumerable<DummyAddressStickerRow>)
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