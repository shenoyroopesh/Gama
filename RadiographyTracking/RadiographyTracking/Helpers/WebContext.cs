using System;
using System.Collections.Generic;
using System.Linq;
using RadiographyTracking.Web.Models;
using System.ServiceModel.DomainServices.Client.ApplicationServices;
using System.ComponentModel;
using System.ServiceModel.DomainServices.Client;
using RadiographyTracking.Web.Services;

namespace RadiographyTracking.Web.Services
{
    public sealed partial class RadiographyContext : DomainContext
    {
        partial void OnCreated()
        {
            if (!DesignerProperties.IsInDesignTool)
            {
                ((WebDomainClient<RadiographyContext.IRadiographyServiceContract>)this.DomainClient)
                  .ChannelFactory.Endpoint.Binding.SendTimeout = new TimeSpan(0, 30, 0);
            }
        }
    }
}