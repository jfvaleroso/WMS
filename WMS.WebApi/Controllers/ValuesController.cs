using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WMS.Core.Services.IServices;


namespace WMS.WebApi.Controllers
{
    public class ValuesController : ApiController
    {

            #region Constructor
        private readonly IStatusService statusService;
        public ValuesController(IStatusService statusService)
        {
            this.statusService = statusService;
        }
        #endregion

        // GET api/values
 

    


    }
}