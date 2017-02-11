using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuakeViewer.Models;
using System.Web.Mvc;
using QuakeViewer.Service;
using QuakeViewer.Utils;




namespace QuakeViewer.Controllers
{
    public class QuakeController : Controller
    {
        AccountService accountService { get; set; }

        public ActionResult Index()
        {
            return View();
            accountService = new AccountService();
        }


        public ActionResult Questions()
        {
            return View();
        }

        public ActionResult QuakeResult()
        {
            return View();
        }
    }
}

