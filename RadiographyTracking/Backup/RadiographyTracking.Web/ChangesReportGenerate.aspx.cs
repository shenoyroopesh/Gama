using System;
using System.Collections.Generic;
using System.Globalization;
using RadiographyTracking.Web.Models;
using WordDocumentGenerator.Library;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using System.Threading;
using RadiographyTracking.Web.Services;

namespace RadiographyTracking.Web
{
    public partial class ChangesReportGenerate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            const string reportTemplateName = "ChangesReportTemplate.docx";
            var foundryName = Request.Params["FOUNDRY_NAME"];
            var fromDate = Request.Params["FROM_DATE"];
            var toDate = Request.Params["TO_DATE"];

            //date is expected in dd/MM/yyyy format from the url

            if (!String.IsNullOrEmpty(fromDate))
            {
                fromDate = DateTime.Parse(fromDate, CustomCulture).ToString("dd-MM-yyyy");
            }

            if(!String.IsNullOrEmpty(toDate))
            {
                toDate = DateTime.Parse(toDate, CustomCulture).ToString("dd-MM-yyyy");
            }

            if (string.IsNullOrEmpty(reportTemplateName))
                return;

            var generationInfo = GetDocumentGenerationInfo("ChangesReportGenerator", "1.0", GetDataContext(),
                                        reportTemplateName, false);

            var sampleDocumentGenerator 
                = new ChangesReportGenerator(generationInfo, 
                                                String.IsNullOrEmpty(foundryName) ? "All" : foundryName, 
                                                fromDate, 
                                                toDate);

            byte[] result = sampleDocumentGenerator.GenerateDocument();
            var filePath = WriteOutputToFile("ChangesReportTemplate" + DateTime.Now.ToString("SSMIHH") + ".docx", result);

            using (var wordDocument = WordprocessingDocument.Open(filePath, true))
            {
                wordDocument.ChangeDocumentType(WordprocessingDocumentType.Document);
                var mainDocumentPart = wordDocument.MainDocumentPart;
                var document = mainDocumentPart.Document;
                // Clean up: The user will appreciate a clean document!
                var helper = new OpenXmlHelper(DocumentGenerationInfo.NamespaceUri);
                helper.RemoveContentControlsAndKeepContents(document);
                document.Save();
            }

            //download the file to the user 

            string filename = "ChangesReport" + foundryName + ".docx";
            Response.ContentType = "application/ms-word";
            Response.AddHeader("content-disposition", "attachment; filename="+filename);
            Response.TransmitFile(filePath);
            Response.Flush();

            //clean up the file
            File.Delete(filePath);
            Response.End();
        }

        private CultureInfo _customCulture;

        //date will be present in dd/MM/yyyy in the request parameters - this culture will be used to parse it
        private CultureInfo CustomCulture
        {
            get
            {
                if (_customCulture == null)
                {
                    _customCulture = (CultureInfo) Thread.CurrentThread.CurrentCulture.Clone();
                    _customCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
                }
                return _customCulture;
            }
        }

        /// <summary>
        /// Gets the data context for the report to be generated
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Change> GetDataContext()
        {
            var foundryName = Request.Params["FOUNDRY_NAME"];
            var fromDateString = Request.Params["FROM_DATE"];
            var toDateString = Request.Params["TO_DATE"];

            //date will be present in dd/MM/yyyy
            var fromDate = fromDateString == null ? null : (DateTime?)DateTime.Parse(fromDateString, CustomCulture);
            var toDate = toDateString == null ? null : (DateTime?)DateTime.Parse(toDateString, CustomCulture);

            var service = new RadiographyService();
            if (foundryName == "") foundryName = null;
            return service.GetChanges(foundryName, fromDate, toDate);
        }

        /// <summary>
        /// Gets the document generation info.
        /// </summary>
        /// <param name="docType">Type of the doc.</param>
        /// <param name="docVersion">The doc version.</param>
        /// <param name="dataContext">The data context.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="useDataBoundControls">if set to <c>true</c> [use data bound controls].</param>
        /// <returns></returns>
        private DocumentGenerationInfo GetDocumentGenerationInfo(string docType, string docVersion, object dataContext, string fileName, bool useDataBoundControls)
        {
            var generationInfo = new DocumentGenerationInfo
                                     {
                                         Metadata =
                                             new DocumentMetadata {DocumentType = docType, DocumentVersion = docVersion},
                                         DataContext = dataContext,
                                         TemplateData =
                                             File.ReadAllBytes(Server.MapPath("~/ReportTemplates/" + fileName)),
                                         IsDataBoundControls = useDataBoundControls
                                     };
            return generationInfo;
        }


        /// <summary>
        /// Writes the output to file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="fileContents">The file contents.</param>
        private string WriteOutputToFile(string fileName, byte[] fileContents)
        {
            if (fileContents != null)
            {
                var path = Server.MapPath("~/ReportTemplates/" + fileName);
                File.WriteAllBytes(path, fileContents);
                return path;
            }
            return "";
        }
    }
}