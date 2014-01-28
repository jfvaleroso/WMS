using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using WMS.Webservice.IServices;
using WMS.Webservice.Manager;
using WMS.Webservice.UDS.RoleWebservice;

namespace WMS.Webservice.Services
{
    public class RoleService : SqlRoleProvider, IRoleService
    {
        private readonly UDS.RoleWebservice.RoleWS roleWS;
        private List<string> roleList= new List<string>();
        private Role[] roles;
         public RoleService()
        {
          this.roleWS= new UDS.RoleWebservice.RoleWS();
          
        }

         public override string[] GetRolesForUser(string username)
         {
             if (!roleList.Any())
             {
                 var roles = this.roleWS.GetRoleByUserNameAndSystemCode(username, ServiceManager.ConfigManager.Config.Application);
                 foreach (var item in roles)
                 { roleList.Add(item.RoleName); }
                 return roleList.ToArray();
             }
             return roleList.ToArray();
         }

         public Role[] GetAllRoles(string systemCode)
         {
             if(roles==null)
             { 
                 roles = this.roleWS.GetAllRoleBySystemCode(systemCode);
                 return roles;
             }
             return roles;
         }
    }
}
