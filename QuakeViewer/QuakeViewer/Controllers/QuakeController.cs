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
        AreaParamService areaParamService { get; set; }
        ChoiceService choiceService { get; set; }

        public QuakeController()
        {
            accountService = new AccountService();
            areaParamService = new AreaParamService();
            choiceService = new ChoiceService();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Questions(Account session)
        {
            if (null == session)
            {
                return RedirectToAction("Login", "Home");
            }

            List<AreaParam> provinceList = areaParamService.GetProvince();
            var provinceSelectItems = new List<SelectListItem>();

            foreach (var q in provinceList)
            {
                SelectListItem item = new SelectListItem();
                item.Text = q.Id;
                item.Value = q.Name;

                provinceSelectItems.Add(item);
            }

            ViewData["Province"] = provinceSelectItems;

            var choice = choiceService.GetChoiceByUserId(session.Id, (int)EnumUserType.Web);

            if (choice == null)
            {

                return View(new QuestionModel());
            }

            return RedirectToAction("QuakeResult");
        }

        [HttpPost]
        public ActionResult Questions(Account session, QuestionModel model)
        {

            if (null == session)
            {
                return RedirectToAction("Login", "Home");
            }

            Choice choice = new Choice();
            choice.Id = StringHelper.GuidString();
            choice.UserId = session.Id;
            choice.UserName = session.UserName;

            var areaParam = areaParamService.GetAreaParamById(model.Region);

            choice.FirstChoice = model.Region;
            choice.SecondChoice = model.BuildLevel;
            choice.ThirdChoice = model.StructLevel;
            choice.ForthChoice = model.Designed;
            choice.FifthChoice = model.Jobstatus;
            choice.Sixth = model.YearLevel;
            choice.CreateDate = DateTime.Now;
            choice.FromType = (int)EnumUserType.Web;


            int minor = -1;
            int major = -1;

            QuakeViewerCalculate.Calculate(areaParam.GroupNo.Value,
                                           areaParam.SiteType.Value,
                                           areaParam.IntensityDegree.Value,
                                           choice.SecondChoice.Value,
                                           choice.ThirdChoice.Value,
                                           choice.ForthChoice.Value,
                                           choice.FifthChoice.Value,
                                           choice.Sixth.Value,
                                           out minor,
                                           out major);
            choice.MinorResult = minor;
            choice.MajorResult = major;

            choiceService.SaveChoice(choice);

            return RedirectToAction("QuakeResult");
        }

        [HttpGet]
        public ActionResult QuakeResult(Account session)
        {
            if (null == session)
            {
                return RedirectToAction("Login", "Home");
            }

            var choice = choiceService.GetChoiceByUserId(session.Id, (int)EnumUserType.Web);



            return View();
        }
    }
}

