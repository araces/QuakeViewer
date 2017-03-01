using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuakeViewer.Models;
using System.Web.Mvc;
using QuakeViewer.Service;
using QuakeViewer.Utils;
using Newtonsoft.Json.Linq;



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
                item.Text = q.Name;
                item.Value = q.Id;

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



            QuakeViewerCalculate quakeViewerCalculate = new QuakeViewerCalculate();

            quakeViewerCalculate.InputData(areaParam.GroupNo.Value,
                                           areaParam.SiteType.Value,
                                           areaParam.IntensityDegree.Value,
                                           choice.SecondChoice.Value,
                                           choice.ThirdChoice.Value,
                                           choice.ForthChoice.Value == 1,
                                           choice.FifthChoice.Value,
                                           choice.Sixth.Value);
            quakeViewerCalculate.ResponseMinor();
            quakeViewerCalculate.ResponseMajor();

            choice.MinorResult = quakeViewerCalculate.DamageDgreeMinor;
            choice.MajorResult = quakeViewerCalculate.DamageDgreeMajor;

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

            if (null == choice)
            {
                choice = new Choice();
                choice.CreateDate = DateTime.Now;
                choice.UserId = choice.UserId;
                choice.UserName = choice.UserName;
                choice.MajorResult = -1;
                choice.ThirdChoice = -1;
                choice.ForthChoice = -1;
                choice.FifthChoice = -1;
            }

            ResultModel model = new ResultModel();
            model.UserName = choice.UserName;
            model.MajorLevel = choice.MajorResult.Value;
            model.Reason1 = choice.ThirdChoice.Value;
            model.Reason2 = choice.ForthChoice.Value;
            model.Reason3 = choice.FifthChoice.Value;

            return View(model);
        }

        public ActionResult GetAreaParamsById(Account session, string parentId)
        {

            Response.ContentType = "application/json";
            JObject result = new JObject();
            JObject obj = new JObject();
   
            if (null == session)
            {
                obj.Add("success", false);
                obj.Add("msg", "权限错误");
                result.Add("result", obj);
                return Content(result.ToString());
            }
    
            if (string.IsNullOrEmpty(parentId))
            {
                obj.Add("success", false);
                obj.Add("msg", "参数错误");
                result.Add("result", obj);
                return Content(result.ToString());
            }


            List<AreaParam> areaParams = areaParamService.GetAreaParamsByParentId(parentId);

            areaParams = areaParams.OrderBy(p => p.ParentId).ToList();

            JArray arrays = JArray.FromObject(areaParams);

            obj.Add("success", true);
            obj.Add("areas", arrays);
            obj.Add("msg", "");
            result.Add("result", obj);
            return Content(result.ToString());

        }
    }
}

