using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WMS.Entities;

namespace WMS.NHibernateBase.Mapping
{
    public class NotificationMappingMap : ClassMap<NotificationMapping>
    {
        public NotificationMappingMap()
        {
            Table("NotificationMapping");
            Id(x => x.Id);
            Map(x => x.EmailContent);
            Map(x => x.SMSContent);
            Map(x => x.Active);
            Map(x => x.CreatedBy);
            Map(x => x.DateCreated);
            Map(x => x.ModifiedBy);
            Map(x => x.DateModified);
            References(x => x.Workflow, "Workflow_Id");
            References(x => x.Status, "Status_Id");
         
        }
    }
}
