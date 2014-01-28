using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WMS.Entities;
using FluentNHibernate.Mapping;

namespace WMS.NHibernateBase.Mapping
{
    public class DocumentMappingMap : ClassMap<DocumentMapping>
    {
        public DocumentMappingMap()
	{
            Table("DocumentMapping");
            Id(x => x.Id);
            Map(x => x.Mandatory);
            Map(x => x.Active);
            Map(x => x.CreatedBy);
            Map(x => x.DateCreated);
            Map(x => x.ModifiedBy);
            Map(x => x.DateModified);
            References(x => x.Workflow, "Workflow_Id");
            References(x => x.Document, "Document_Id");

	}

    }
}
