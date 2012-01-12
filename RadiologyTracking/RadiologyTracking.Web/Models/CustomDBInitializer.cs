using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Security;

namespace RadiologyTracking.Web.Models
{
    /// <summary>
    /// Custom DB initializer which will seed basic roles and add an admin user for the application
    /// </summary>
    public class CustomDBInitializer : DropCreateDatabaseIfModelChanges<RadiologyContext>   
    {
        protected override void Seed(RadiologyContext context)        
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

            context.SaveChanges();
        }
    }
}