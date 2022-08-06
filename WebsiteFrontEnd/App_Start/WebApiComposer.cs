using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Umbraco.Core.Composing;

namespace MessenceComFrontEnd.App_Start
{
    public class WebApiComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            GlobalConfiguration.Configuration.MapHttpAttributeRoutes();
        }
    }
}