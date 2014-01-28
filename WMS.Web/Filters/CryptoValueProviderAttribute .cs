using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.Web.Helper;



namespace WMS.Web.Filter
{
    public class CryptoValueProviderAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            filterContext.Controller.ValueProvider = new CryptoValueProvider(filterContext.RouteData);
        }
    }
}