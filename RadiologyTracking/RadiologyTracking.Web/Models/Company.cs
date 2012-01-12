using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using RadiologyTracking.Web.Resources;

namespace RadiologyTracking.Web.Models
{
    public class Company
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Order=1)]
        public int ID { get; set; }

        [Display(Order=2, Description="This is the name that will be used in Final Printable Reports")]
        [Required(ErrorMessage ="Name of the company cannot be empty")]
        public String Name { get; set; }

        [Display(Order=3, Description="Short Name is used everywhere except final reports")]
        [Required(ErrorMessage = "Short Name of the company cannot be empty")]
        public String ShortName { get; set; }

        [Display(Order=4)]
        [Required(ErrorMessage="Address for the Company cannot be empty")]
        public String Address { get; set; }

        [Display(Order=5)]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                           ErrorMessageResourceName = "ValidationErrorInvalidEmail", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        public String Email { get; set; }

        [Display(Order=6)]
        public String WebSite { get; set; }

        [Display(Order=7)]
        public String PhoneNo { get; set; }
    }
}