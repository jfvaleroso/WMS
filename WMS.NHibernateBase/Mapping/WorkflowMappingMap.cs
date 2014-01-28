using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WMS.Entities;
using FluentNHibernate.Mapping;

namespace WMS.NHibernateBase.Mapping
{
    public class WorkflowMappingMap: ClassMap<WorkflowMapping>
    {
        public WorkflowMappingMap()
        {
            Table("WorkflowMapping");
            Id(x => x.Id);
            Map(x => x.LevelId);
            Map(x => x.SLA);
            Map(x => x.Operator);
            Map(x => x.Active);
            Map(x => x.CreatedBy);
            Map(x => x.DateCreated);
            Map(x => x.ModifiedBy);
            Map(x => x.DateModified);
            References(x => x.Workflow, "Workflow_Id");
            Map(x => x.Approver);
            Map(x => x.AlertTo);
            Map(x => x.SMSNotification);
            Map(x => x.EmailNotification);

           
        }
    }
}
