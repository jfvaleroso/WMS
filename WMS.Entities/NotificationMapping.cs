using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;


namespace WMS.Entities
{
    public class NotificationMapping : Entity<long>
    {

        public virtual Workflow Workflow { get; set; }
        public virtual Status Status { get; set; }
        
        public virtual string EmailContent { get; set; }
        public virtual string SMSContent { get; set; }
    }
}
