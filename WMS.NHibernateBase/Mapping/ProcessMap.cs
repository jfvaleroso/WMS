using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WMS.Entities;
using FluentNHibernate.Mapping;

namespace WMS.NHibernateBase.Mapping
{
    public class ProcessMap: ClassMap<Process>
    {
        public ProcessMap()
        {
            Table("Process");
            Id(x => x.Id);
            Map(x => x.SystemCode);
            Map(x => x.Description);
            Map(x => x.Code);
            Map(x => x.Name);
            Map(x => x.Active);
            Map(x => x.CreatedBy);
            Map(x => x.DateCreated);
            Map(x => x.ModifiedBy);
            Map(x => x.DateModified);
            //one Process to Many SubProcess
            HasMany(x => x.SubProcesses)
                  .Inverse()
                  .Cascade.All();
            //One Process to Many Workflows
            HasMany(x => x.Workflows)
                 .Cascade.All();
        }
    }
}
