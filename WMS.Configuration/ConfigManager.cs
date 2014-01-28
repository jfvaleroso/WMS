using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web.Configuration;


namespace WMS.Configurations
{
    public static class ConfigManager
    {
        public static Config _config;
        public static Config WMS
        {
            get
            {
                if (_config == null)
                {
                    _config = Config.Section;
                }
                return _config;
            }
            set
            {
                _config = value;
            }
        }
        public static System.Configuration.Configuration GetConfig()
        {
            return WebConfigurationManager.OpenWebConfiguration("~/config");
        }
        public static Config GetSection(System.Configuration.Configuration config)
        {
            return (config.GetSection("wms") as Config);
        }
    }
}
