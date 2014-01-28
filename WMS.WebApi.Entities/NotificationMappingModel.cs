using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.WebApi.Entities
{
    public class NotificationMappingModel
    {
        public string SMSContent { get; set; }
        public string EmailContent { get; set; }
        public string StatusCode { get; set; }
    }
}
