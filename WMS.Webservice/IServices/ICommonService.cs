using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WMS.Webservice.UDS.CommonWebservice;

namespace WMS.Webservice.IServices
{
     public interface ICommonService
    {

         Systems[] GetAllSystems();
    }
}
