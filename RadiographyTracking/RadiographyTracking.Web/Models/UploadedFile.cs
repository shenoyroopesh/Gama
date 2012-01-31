using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.ComponentModel.DataAnnotations;

namespace RadiographyTracking.Web.Models
{
    public class UploadedFile
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public String FileName { get; set; }

        public String FileType { get; set; }

        public byte[] FileData { get; set; }

        public String FileExtension { get; set; }

        [CLSCompliant(false)]
        public UInt64 FileSize { get; set; }
    }
}