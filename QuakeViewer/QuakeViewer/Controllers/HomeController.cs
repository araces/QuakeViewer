using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuakeViewer.Models;
using System.Web.Mvc;
using QuakeViewer.DAL;
using System.Web.Mvc.Ajax;

namespace QuakeViewer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var mvcName = typeof(Controller).Assembly.GetName();

            LoginModel model = new LoginModel();

            var context = new BaseContext();

            var sample = context.Choices.Count();

            ViewData["Version"] = mvcName.Version.Major + "." + mvcName.Version.Minor + "." + sample;

            return View(model);
        }
    }
}
