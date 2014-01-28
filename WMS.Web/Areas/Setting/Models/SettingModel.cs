using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMS.Web.Areas.Setting.Models
{
    public class SettingModel
    {
        public string Application { get; set; }
        public string SecuredConnection { get; set; }
        public IList<ExternalModel> ExternalWebServiceList { get; set; }

        public SettingModel()
        {
            this.ExternalWebServiceList = new List<ExternalModel>();
        }
    }
}