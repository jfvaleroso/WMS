using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Globalization;
using System.Web.Helpers;
using WMS.Core.Helper.Common;

namespace WMS.Web.Helper
{
    public class CryptoValueProvider :  IValueProvider
    {
        RouteData routeData = null;
        object data;

        public CryptoValueProvider(RouteData routeData)
        {
            this.routeData = routeData;
        }

        public bool ContainsPrefix(string prefix)
        {
            if (this.routeData.Values["id"] == null)
            {return false;}
            data =Base.Decrypt(this.routeData.Values["id"].ToString());
            return true;
        }

        public ValueProviderResult GetValue(string key)
        {
            ValueProviderResult result;
            result = new ValueProviderResult(data,
                "Id", CultureInfo.CurrentCulture);
            return result;
        }
    }
}