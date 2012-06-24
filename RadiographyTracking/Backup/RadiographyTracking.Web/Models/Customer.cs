using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using RadiographyTracking.Web.Resources;

namespace RadiographyTracking.Web.Models
{
    public class Customer
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required(ErrorMessage = "Name of the Customer cannot be empty")]
        public String CustomerName { get; set; }

        [Required(ErrorMessage = "Short Name of the Customer cannot be empty")]
        public String ShortName { get; set; }

        [Required(ErrorMessage = "Address for the Customer cannot be empty")]
        public String Address { get; set; }

        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                           ErrorMessageResourceName = "ValidationErrorInvalidEmail", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        public String Email { get; set; }

        public String WebSite { get; set; }

        public String PhoneNo { get; set; }

        public int FoundryID { get; set; }
        /// <summary>
        /// Every Customer belongs to a particular foundry, this particular note will tell which foundry
        /// </summary>
        public Foundry Foundry { get; set; }
        public ICollection<FixedPattern> FixedPatterns;

        public int? LogoID { get; set; }

        public UploadedFile Logo { get; set; }
    }
}
