using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.Entities;
using WMS.Core.Services.IServices;
using WMS.Webservice.IServices;
using WMS.Webservice.UDS.RoleWebservice;
using WMS.Web.Helper;
using WMS.Web.Models;
using WMS.Web.Filter;

namespace WMS.Web.Controllers
{
    public class HomeController : Controller
    {

       
        public HomeController()
        {
           
        }
      
        public ActionResult Index()
        {

            return View();
        }

       
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
