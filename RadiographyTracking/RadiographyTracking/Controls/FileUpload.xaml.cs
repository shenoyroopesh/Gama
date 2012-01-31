using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.ServiceModel.DomainServices.Client;
using System.Windows;
using System.Windows.Controls;
using RadiographyTracking.Web.Services;
using RadiographyTracking.Web.Models;

namespace RadiographyTracking.Controls
{
    public partial class FileUpload : UserControl
    {
        private byte[] fileBuffer = null;
        private FileInfo selectedFile = null;

        public UploadedFile File { get; set; }

        public delegate void FileAddedHandler(object sender, EventArgs e);
        public event FileAddedHandler FileAdded;

        // Invoke the Changed event; called whenever list changes
        protected virtual void OnFileAdded(EventArgs e)
        {
            if (FileAdded != null)
                FileAdded(this, e);
        }

        /// <summary>
        /// Register this property for binding
        /// </summary>
        public static readonly DependencyProperty UploadedFileProperty =
            DependencyProperty.Register("File", typeof(UploadedFile), typeof(FileUpload), null);

        public FileUpload()
        {
            InitializeComponent();
        }


        public void UploadFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All files|*.*";
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    using (FileStream strm = openFileDialog.File.OpenRead())
                    {
                        selectedFile = openFileDialog.File;
                        using (BinaryReader rdr = new BinaryReader(strm))
                        {
                            fileBuffer = rdr.ReadBytes((int)strm.Length);
                        }
                    }
                    CreateFileUpload();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void CreateFileUpload()
        {
            File = new UploadedFile()
            {
                FileName = selectedFile.Name,
                FileType = "image",
                FileExtension = selectedFile.Extension,
                FileData = fileBuffer,
                FileSize = (ulong)fileBuffer.Count()
            };
            //this will save when the parent item will save. 

            //raise file uploaded event
            OnFileAdded(new EventArgs());
        }

        private void ResetAll()
        {
            fileBuffer = null;
            selectedFile = null;
            File = null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.UploadFile();
        }
    }
}