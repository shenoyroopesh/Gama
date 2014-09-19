﻿using System;
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
    public partial class FinalRGReportGenerate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var reportTemplateName = Request.Params["Template"];
            var IsFilmSizeInCms = Convert.ToBoolean(Request.Params["FilmSize"]);

            if (string.IsNullOrEmpty(reportTemplateName))
                return;

            DocumentGenerationInfo generationInfo = GetDocumentGenerationInfo("FinalRGReportGenerator", "1.0", GetDataContext(),
                                        reportTemplateName, false);

            FinalRGReportGenerator sampleDocumentGenerator = new FinalRGReportGenerator(generationInfo);
            sampleDocumentGenerator.IsFilmSizeInCms = IsFilmSizeInCms;
            byte[] result = result = sampleDocumentGenerator.GenerateDocument();
            var filePath = WriteOutputToFile("RadiographyReportTemplate_Out" + DateTime.Now.ToString("SSMIHH") + ".docx", result);

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

            string filename = "RTNo" + Request.Params["RTNo"] + ".docx";
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
        private FinalRTReport GetDataContext()
        {
            string rtNo = Request.Params["RTNo"];
            string Filter = Request.Params["Filter"];

            if (String.IsNullOrEmpty(rtNo))
                return null;

            RadiographyService service = new RadiographyService();
            return service.GetFinalRTReport(rtNo, Filter);
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