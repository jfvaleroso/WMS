using System.Web.Mvc;

namespace WMS.Web.Areas.Setting
{
    public class SettingAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Setting";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Setting_default",
                "Setting/{controller}/{action}/{id}/{status}",
                defaults: new { controller = "Process", action = "Index", id = "0", status = UrlParameter.Optional }
            );
        }
    }
}
