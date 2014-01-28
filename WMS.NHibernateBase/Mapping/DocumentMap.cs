using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using WMS.Entities;

namespace WMS.NHibernateBase.Mapping
{
    public class DocumentMap : ClassMap<Document>
    {
        public DocumentMap()
        {
            Table("Document");
            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.Code);
            Map(x => x.Description);
            Map(x => x.Active);
            Map(x => x.CreatedBy);
            Map(x => x.DateCreated);
            Map(x => x.ModifiedBy);
            Map(x => x.DateModified);
            HasMany(x => x.DocumentMappings)
             .Inverse()
             .Cascade.All();
        }
    }
}
