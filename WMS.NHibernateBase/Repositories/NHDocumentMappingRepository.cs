using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WMS.Core.Repositories;
using WMS.Entities;

namespace WMS.NHibernateBase.Repositories
{
    public class NHDocumentMappingRepository : NHRepositoryBase<DocumentMapping, long>, IDocumentMappingRepository
    {
    }
}
