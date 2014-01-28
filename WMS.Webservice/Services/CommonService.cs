using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WMS.Webservice.IServices;
using WMS.Webservice.UDS.CommonWebservice;

namespace WMS.Webservice.Services
{
    public class CommonService: ICommonService
    {
        private readonly UDS.CommonWebservice.CommonWS commonWS;
        private Systems[] systemList;
        public CommonService()
        {
            this.commonWS= new UDS.CommonWebservice.CommonWS();
        }
        public Systems[] GetAllSystems()
        {
            if(systemList==null)
            {  return this.commonWS.GetAllActiveSystem(); }
            return systemList;

        }
    }
}
