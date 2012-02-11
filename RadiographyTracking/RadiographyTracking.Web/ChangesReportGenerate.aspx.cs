using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RadiographyTracking.Web.Models;
using WordDocumentGenerator.Library;
using System.IO;
using System.Data.Entity;
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
            var reportTemplateName = "ChangesReportTemplate.docx";
            var foundryName = Request.Params["FOUNDRY_NAME"];
            var fromDate = Request.Params["FROM_DATE"];
            var toDate = Request.Params["TO_DATE"];

            if (!String.IsNullOrEmpty(fromDate))
            {
                fromDate = DateTime.Parse(fromDate).ToString("dd-MM-yyyy");
            }

            if(!String.IsNullOrEmpty(toDate))
            {
                toDate = DateTime.Parse(toDate).ToString("dd-MM-yyyy");
            }

            if (string.IsNullOrEmpty(reportTemplateName))
                return;

            DocumentGenerationInfo generationInfo = GetDocumentGenerationInfo("ChangesReportGenerator", "1.0", GetDataContext(),
                                        reportTemplateName, false);

            ChangesReportGenerator sampleDocumentGenerator 
                = new ChangesReportGenerator(generationInfo, 
                                                String.IsNullOrEmpty(foundryName) ? "All" : foundryName, 
                                                fromDate, 
                                                toDate);

            byte[] result = result = sampleDocumentGenerator.GenerateDocument();
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

        /// <summary>
        /// Gets the data context for the report to be generated
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Change> GetDataContext()
        {
            var foundryName = Request.Params["FOUNDRY_NAME"];
            var fromDateString = Request.Params["FROM_DATE"];
            var toDateString = Request.Params["TO_DATE"];

            var fromDate = fromDateString == null ? null : (DateTime?)DateTime.Parse(fromDateString);
            var toDate = toDateString == null ? null : (DateTime?)DateTime.Parse(toDateString);

            RadiographyService service = new RadiographyService();
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
            DocumentGenerationInfo generationInfo = new DocumentGenerationInfo();
            generationInfo.Metadata = new DocumentMetadata() { DocumentType = docType, DocumentVersion = docVersion };
            generationInfo.DataContext = dataContext;
            generationInfo.TemplateData = File.ReadAllBytes(Server.MapPath("~/ReportTemplates/" + fileName));
            generationInfo.IsDataBoundControls = useDataBoundControls;
            return generationInfo;
        }


        /// <summary>
        /// Writes the output to file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="fileContents">The file contents.</param>
        private string WriteOutputToFile(string fileName, byte[] fileContents)
        {
            ConsoleColor consoleColor = Console.ForegroundColor;
            if (fileContents != null)
            {
                String path = Server.MapPath("~/ReportTemplates/" + fileName);
                File.WriteAllBytes(path, fileContents);
                return path;
            }
            return "";
        }
    }
}