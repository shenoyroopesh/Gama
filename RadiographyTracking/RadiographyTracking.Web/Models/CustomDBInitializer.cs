using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Security;

namespace RadiographyTracking.Web.Models
{
    /// <summary>
    /// Custom DB initializer which will seed basic roles and add an admin user for the application
    /// </summary>
    public class CustomDBInitializer : DropCreateDatabaseIfModelChanges<RadiographyContext>   
    {
        protected override void Seed(RadiographyContext context)        
        {            
            base.Seed(context);
            Membership.CreateUser("admin", "admin123");
            Roles.CreateRole("Admin");
            Roles.CreateRole("Clerk");
            Roles.CreateRole("Foundry Supervisor");
            Roles.CreateRole("Corrector");
            Roles.CreateRole("Managing Director");
            Roles.AddUsersToRole(new[] {"admin"}, "Admin");

            //Necessary to have one and only one company
            context.Companies.Add(new Company()
            {
                Name = "Company Name",
                Address = "Sample Address",
                ShortName = "Company"
            });

            //seed data for lookups that won't have any screens

            context.Directions.Add(new Direction()
            {
                Value = "RECEIVED_FROM_HO"
            });

            context.Directions.Add(new Direction()
            {
                Value = "SENT_TO_HO"
            });
            
            context.Remarks.Add(new Remark { Value = "REPAIR" });
            context.Remarks.Add(new Remark { Value = "ACCEPTABLE" });
            context.Remarks.Add(new Remark { Value = "RETAKE" });
            context.Remarks.Add(new Remark { Value = "RESHOOT" });

            context.RGStatuses.Add(new RGStatus() { Status = "CASTING UNDER REPAIR" });
            context.RGStatuses.Add(new RGStatus() { Status = "COMPLETE" });

            context.Shifts.Add(new Shift() { Value = "DAY" });
            context.Shifts.Add(new Shift() { Value = "NIGHT" });

            context.RGReportRowTypes.Add(new RGReportRowType() { Value = "FRESH" });
            context.RGReportRowTypes.Add(new RGReportRowType() { Value = "REPAIR" });
            context.RGReportRowTypes.Add(new RGReportRowType() { Value = "RETAKE" });
            context.RGReportRowTypes.Add(new RGReportRowType() { Value = "RESHOOT" });

            context.SaveChanges();
        }
    }
}