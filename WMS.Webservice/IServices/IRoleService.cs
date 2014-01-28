using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WMS.Webservice.UDS.RoleWebservice;

namespace WMS.Webservice.IServices
{
    public interface IRoleService
    {

        Role[] GetAllRoles(string systemCode);
    }
}
