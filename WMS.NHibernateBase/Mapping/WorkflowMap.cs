using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WMS.Entities;
using FluentNHibernate.Mapping;

namespace WMS.NHibernateBase.Mapping
{
   public class WorkflowMap : ClassMap<Workflow>
    {
       public WorkflowMap ()
	    {
            Table("Workflow");
            Id(x => x.Id);
            Map(x => x.Code);
            Map(x => x.Name);
            Map(x => x.Description);
            Map(x => x.Active);
            Map(x => x.CreatedBy);
            Map(x => x.DateCreated);
            Map(x => x.ModifiedBy);
            Map(x => x.DateModified);
            Map(x => x.Requestor);
            References(x => x.Process, "Process_Id");
            References(x => x.SubProcess, "SubProcess_Id");
            References(x => x.Classification,"Classification_Id");
            //one subprocess to many classification
            HasMany(x => x.DocumentMappings)
                .Inverse()
               .Cascade.All();
            HasMany(x => x.NotificationMappings)
               .Inverse()
              .Cascade.All();
            HasMany(x => x.WorkflowMappings)
               .Inverse()
              .Cascade.All();

	     }
   }
    
}
