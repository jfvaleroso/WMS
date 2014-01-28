using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace WMS.Configurations
{
    public class ExternalWebServiceElement : ConfigurationElement
    {
        [ConfigurationProperty("systemCode", IsRequired = false)]
        public string SystemCode
        {
            get
            {
                return (base["systemCode"] as string);
            }
            set
            {
                base["systemCode"] = value;
            }
        }
        
        [ConfigurationProperty("name", IsRequired = false)]
        public string Name
        {
            get
            {
                return (base["name"] as string);
            }
            set
            {
                base["name"] = value;
            }
        }

        [ConfigurationProperty("description", IsRequired = false)]
        public string Description
        {
            get
            {
                return (base["description"] as string);
            }
            set
            {
                base["description"] = value;
            }
        }

        [ConfigurationProperty("url", IsRequired = false)]
        public string URL
        {
            get
            {
                return (base["url"] as string);
            }
            set
            {
                base["url"] = value;
            }
        }

       
    }
}
