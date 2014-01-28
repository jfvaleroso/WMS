using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.Configurations;
using WMS.Helper.Transaction;
using WMS.Web.Areas.Setting.Models;

namespace WMS.Web.Areas.Setting.Controllers
{
    [Authorize(Roles="Super Admin")]
    public class ExternalController : Controller
    {


        Configuration configuration;
        Config config;
        public ExternalController()
        {
            this.configuration = ConfigManager.GetConfig();
            this.config = ConfigManager.GetSection(this.configuration);
        }


        public ActionResult Index()
        {
            //get secured connection
            string securedConnection = ConfigurationManager.ConnectionStrings["WorkflowConn"].ToString();
            SettingModel model = new SettingModel();
            model.Application = this.config.Application;
            model.SecuredConnection = securedConnection;
            model.ExternalWebServiceList = GetAllExternalWebService(this.config);
            return View(model);
        }
        public ActionResult Modify(string id)
        {
            ExternalModel model = new ExternalModel();
            model = GetExternalWebServiceByName(this.config, id);
            return View(model);

        }
        [HttpPost]
        public ActionResult Modify(ExternalModel model)
        {

            if (ModelState.IsValid)
            {
                SaveChangesToExternalWebService(model);
                return Json(new {  message = MessageCode.modified, code = StatusCode.modified, content = model.SystemCode });
            }
            return RedirectToAction("Index");


        }

        #region External Webservice Helper
        private List<ExternalModel> GetAllExternalWebService(Config config)
        {
            List<ExternalModel> list = new List<ExternalModel>();
            foreach (ExternalWebServiceElement element in config.ExternalWebService)
            {
                ExternalModel externalWebService = new ExternalModel();
                externalWebService.Description = element.Description;
                externalWebService.Name = element.Name;
                externalWebService.SystemCode = element.SystemCode;
                externalWebService.URL = element.URL;
                list.Add(externalWebService);
            }
            return list;
        }  
        private ExternalModel GetExternalWebServiceByName(Config config, string systemCode)
        {
            ExternalModel external = new ExternalModel();
            if (!string.IsNullOrEmpty(systemCode))
            {
                ExternalWebServiceElement element = config.ExternalWebService.OfType<ExternalWebServiceElement>().Where<ExternalWebServiceElement>(x => x.SystemCode.Equals(systemCode)).FirstOrDefault<ExternalWebServiceElement>();
                if (element != null)
                {
                    external.Description = element.Description;
                    external.Name = element.Name;
                    external.SystemCode = element.SystemCode;
                    external.URL = element.URL;
                }
            }
            return external;
        }
        private void SaveChangesToExternalWebService(ExternalModel model)
        {

            if (model != null)
            {

                ExternalWebServiceElement element = this.config.ExternalWebService.OfType<ExternalWebServiceElement>().Where<ExternalWebServiceElement>(x => x.SystemCode.Equals(model.SystemCode)).First<ExternalWebServiceElement>();
                element.SystemCode = model.SystemCode;
                element.URL = model.URL;
                element.Name = model.Name;
                element.Description = model.Description;
                this.configuration.Save();
            }

        }
        #endregion

    }
}
