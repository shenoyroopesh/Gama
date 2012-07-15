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
    public partial class AddressStickerReportGenerate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var reportTemplateName = Request.Params["TEMPLATE_NAME"];
            var ReportNumber = Request.Params["REPORT_NUMBER"];
            var cellNo = Request.Params["CELL_NO"];

            //date is expected in dd/MM/yyyy format from the url


            if (string.IsNullOrEmpty(reportTemplateName))
                return;

            var generationInfo = GetDocumentGenerationInfo("AddressStickerGenerator", "1.0", GetDataContext(),
                                        reportTemplateName, false);

            var addressStickerGenerator 
                = new AddressStickerGenerator(generationInfo);

            byte[] result = addressStickerGenerator.GenerateDocument();
            var filePath = WriteOutputToFile("AddressStickerReport" + DateTime.Now.ToString("SSMIHH") + ".docx", result);

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

            string filename = "AddressStickerReport.docx";
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
        private IEnumerable<AddressStickerRow> GetDataContext()
        {
            var reportTemplateName = Request.Params["TEMPLATE_NAME"];
            var ReportNumber = Request.Params["REPORT_NUMBER"];
            var cellNo = Request.Params["CELL_NO"];
            var service = new RadiographyService();
            return service.GetAddressStickers(ReportNumber, Int32.Parse(cellNo));
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