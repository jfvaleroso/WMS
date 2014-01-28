using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using WMS.Entities;

namespace WMS.NHibernateBase.Mapping
{
    public class SubProcessMap : ClassMap<SubProcess>
    {
        public SubProcessMap()
        {
            Table("SubProcess");
            Id(x => x.Id);
            Map(x => x.Code);
            Map(x => x.Name);
            Map(x => x.Description);
            Map(x => x.Active);
            Map(x => x.CreatedBy);
            Map(x => x.DateCreated);
            Map(x => x.ModifiedBy);
            Map(x => x.DateModified);
            References(x => x.Process, "Process_Id");
            //one subprocess to many workflow
            HasMany(x => x.Workflows)
               .Cascade.All();
            //one subprocess to many classification
            HasMany(x => x.Classifications)
                .Inverse()
               .Cascade.All();
        }
    }
}
