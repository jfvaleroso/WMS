using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using WMS.Configurations;


namespace WMS.Webservice.Manager
{
    public class ServiceManager
    {

        #region UDS External Webservice Manager
        public static class RoleManager
        {
            private static UDS.RoleWebservice.RoleWS roleWS;
            public static UDS.RoleWebservice.RoleWS RoleWS
            {
                get
                {
                    if (roleWS == null)
                    {
                        roleWS = new UDS.RoleWebservice.RoleWS();
                        string url = ExternalWebserviceURL("UDS-ROLE");
                        if (!string.IsNullOrEmpty(url))
                            roleWS.Url = url;
                    }
                    return roleWS;
                }
                set
                {
                    roleWS = value;
                }
            }


        }
        public static class CommonManager
        {
            private static UDS.CommonWebservice.CommonWS commonWS;
            public static UDS.CommonWebservice.CommonWS CommonWS
            {
                get
                {
                    if (commonWS == null)
                    {
                        commonWS = new UDS.CommonWebservice.CommonWS();
                        string url = ExternalWebserviceURL("UDS-COMMON");
                        if (!string.IsNullOrEmpty(url))
                            commonWS.Url = url;
                    }
                    return commonWS;
                }
                set
                {
                    commonWS = value;
                }
            }


        }
        #endregion

        public static class ConfigManager
        {
            private static System.Configuration.Configuration configuration;
            private static Config config;
            public static Config Config
            {
                get
                {
                    configuration = WMS.Configurations.ConfigManager.GetConfig(); 
                    config = WMS.Configurations.ConfigManager.GetSection(configuration);
                    return config;
                }
                set
                {
                    config = value;
                }
            }


        }


        #region Helper
        public static string ExternalWebserviceURL(string systemCode)
        {
            string url = "";
            ExternalWebServiceElement service = ConfigManager.Config.ExternalWebService.OfType<ExternalWebServiceElement>().Where<ExternalWebServiceElement>(x => x.SystemCode.Equals(systemCode)).First<ExternalWebServiceElement>();
            if (service != null)
            {
                if (ValidWebservice(service.URL.ToString()))
                    url = service.URL.ToString();
            }
            return url;
        }

        #region ValidateWebservice
        public static bool ValidWebservice(string url)
        {
            HttpWebResponse res = null;
            try
            {
                // Create a request to the passed URI.  
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Credentials = CredentialCache.DefaultNetworkCredentials;

                // Get the response object.  
                res = (HttpWebResponse)req.GetResponse();

                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
        #endregion




    }
}
