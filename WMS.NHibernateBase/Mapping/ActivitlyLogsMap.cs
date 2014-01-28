using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using WMS.Entities;

namespace WMS.NHibernateBase.Mapping
{
    public class ActivitlyLogsMap : ClassMap<ActivityLogs>
    {
        public ActivitlyLogsMap()
        {
            Table("ActivityLogs");
            Id(x => x.Id);
            Map(x => x.Type);
            Map(x => x.Description);
            Map(x => x.Timestamp);
            Map(x => x.ExecutedBy);
        }
    }
}
