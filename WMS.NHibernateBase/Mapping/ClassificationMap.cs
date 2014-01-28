using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WMS.Entities;
using FluentNHibernate.Mapping;

namespace WMS.NHibernateBase.Mapping
{
    public class ClassificationMap : ClassMap<Classification>
    {
        public ClassificationMap()
        {
            Table("Classification");
            Id(x => x.Id);
            Map(x => x.Code);
            Map(x => x.Name);
            Map(x => x.Description);
            Map(x => x.Active);
            Map(x => x.CreatedBy);
            Map(x => x.DateCreated);
            Map(x => x.ModifiedBy);
            Map(x => x.DateModified);
            References(x => x.SubProcess, "SubProcess_Id");
            HasMany(x => x.Workflows)
             .Cascade.All();
        }
    }
}
