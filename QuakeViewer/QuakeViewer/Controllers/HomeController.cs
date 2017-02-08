using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuakeViewer.DAL;
using QuakeViewer.Models;
using System.Web.Mvc.Ajax;

namespace QuakeViewer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var mvcName = typeof(Controller).Assembly.GetName();


            var context = new BaseContext();

            var sample = context.Dict.FirstOrDefault(p => p.Keys == 1);

            ViewData["Version"] = mvcName.Version.Major + "." + mvcName.Version.Minor + "." + sample.Values;

            return View();
        }
    }
}
