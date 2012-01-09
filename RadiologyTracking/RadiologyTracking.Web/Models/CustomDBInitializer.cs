using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Security;

namespace RadiologyTracking.Web.Models
{
    public class CustomDBInitializer : DropCreateDatabaseIfModelChanges<RadiologyContext>   
    {
        protected override void Seed(RadiologyContext context)        
        {            
            base.Seed(context);            
            //Add a User and Role Sample!            
            Membership.CreateUser("admin", "admin123");            
            Roles.CreateRole("Admin");            
            Roles.AddUsersToRole(new[] {"admin"}, "Admin");        
        }    
    }

}