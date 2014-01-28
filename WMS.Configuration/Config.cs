using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web.Configuration;


namespace WMS.Configurations
{
    public class Config : ConfigurationSection
    {

        public static Config Section
        {
            get
            {
                object section = WebConfigurationManager.GetSection("wms", "~/config");
                if (section != null)
                {
                    return (section as Config);
                }
                return null;
            }
        }  

       

        [ConfigurationProperty("application", DefaultValue = "", IsRequired = false)]
        public string Application
        {
            get
            {
                return (string)base["application"];
            }
            set
            {
                base["application"] = value;
            }
        }

        [ConfigurationProperty("developer", DefaultValue = "", IsRequired = false)]
        public string Developer
        {
            get
            {
                return (string)base["developer"];
            }
            set
            {
                base["developer"] = value;
            }
        }

       

        [ConfigurationProperty("externalWebService")]
        public ExternalWebServiceCollection ExternalWebService
        {
            get
            {
                return (base["externalWebService"] as ExternalWebServiceCollection);
            }
        }


      

    }
}
