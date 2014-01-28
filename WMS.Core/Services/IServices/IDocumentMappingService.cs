using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WMS.Entities;

namespace WMS.Core.Services.IServices
{
    public interface IDocumentMappingService : IService<DocumentMapping, long>
    {
        //get data by workflow ID
        List<DocumentMapping> GetFilteredDataByWorkflow(Workflow workflow);
        void DeleteDocumentMapping(Workflow workflow);
    }
}
