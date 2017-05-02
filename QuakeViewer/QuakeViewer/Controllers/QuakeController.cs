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
        AreaExtendParamsService areaExtendParamsService { get; set; }
        ChoiceService choiceService { get; set; }

        public QuakeController()
        {
            accountService = new AccountService();
            areaExtendParamsService = new AreaExtendParamsService();
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

            List<KeyValuePair<string, string>> provinceList = areaExtendParamsService.GetProvince();
            var provinceSelectItems = new List<SelectListItem>();
            provinceSelectItems.Add(new SelectListItem() {Text = "---选择省---", Value = ""});
            foreach (var q in provinceList)
            {
                SelectListItem item = new SelectListItem
                {
                    Text = q.Value,
                    Value = q.Key
                };

                provinceSelectItems.Add(item);
            }

            ViewData["Province"] = provinceSelectItems;


            return View(new QuestionModel());
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

            var areaParam = areaExtendParamsService.GetStreetByStreatId(model.Street);

            choice.FirstChoice = model.Street;
            choice.SecondChoice = model.BuildLevel; //200
            choice.ThirdChoice = model.StructLevel; //4
            choice.ForthChoice = model.Designed; //0
            choice.FifthChoice = model.Jobstatus; //1
            choice.Sixth = model.YearLevel; //1
            choice.CreateDate = DateTime.Now;
            choice.FromType = (int) EnumUserType.Web;
            choice.Address = model.Address;


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

            return RedirectToAction("QuakeResult", new {id = choice.Id});
        }

        [HttpGet]
        public ActionResult QuakeResult(Account session, string id)
        {
            if (null == session)
            {
                return RedirectToAction("Login", "Home");
            }
            Choice choice = null;
            if (string.IsNullOrEmpty(id))
            {
                choice = choiceService.GetChoiceByUserId(session.Id, (int) EnumUserType.Web);
            }
            else
            {
                choice = choiceService.GetChoiceById(id);
            }

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
            model.MinorLevel = choice.MinorResult.Value;
            model.Reason1 = choice.ThirdChoice.Value;
            model.Reason2 = choice.ForthChoice.Value;
            model.Reason3 = choice.FifthChoice.Value;

            return View(model);
        }

        public ActionResult QuestionCharts(Account session)
        {
            return View();
        }

        public ActionResult GetAreaParamsById(Account session, string parentId, int type)
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

            JArray arrays = new JArray();
            List<KeyValuePair<string, string>> areaParams = null;
            if (type == 1)
            {
                areaParams = areaExtendParamsService.GetCityByProvinceId(parentId);
            }
            else if (type == 2)
            {
                areaParams = areaExtendParamsService.GetRegionByCityId(parentId);
            }
            else if (type == 3)
            {
                areaParams = areaExtendParamsService.GetStreetByRegionId(parentId);
            }

            foreach (var q in areaParams)
            {
                JObject area = new JObject();
                area.Add("Id", q.Key);
                area.Add("Name", q.Value);
                arrays.Add(area);
            }

            obj.Add("success", true);
            obj.Add("areas", arrays);
            obj.Add("msg", "");
            result.Add("result", obj);
            return Content(result.ToString());
        }

        public ActionResult queryData(string startTime, string endTime)
        {
            Response.ContentType = "application/json";
            JObject result = new JObject();
            JObject obj = new JObject();


            DateTime? startTimeDate = null;
            DateTime? endTimeDate = null;

            if (string.IsNullOrEmpty(startTime))
            {
                startTimeDate = DateTime.Now.AddDays(-30);
            }
            else if (startTime.Length == 8)
            {
                startTimeDate =
                    DateTime.Parse(
                        $"{startTime.Substring(0, 4)}-{startTime.Substring(4, 2)}-{startTime.Substring(6, 2)}");
            }
            else
            {
                obj.Add("success", false);
                obj.Add("msg", "日期格式错误，请重新输入");
                result.Add("result", obj);
                return Content(result.ToString());
            }

            if (string.IsNullOrEmpty(endTime))
            {
                endTimeDate = DateTime.Now;
            }
            else if (endTime.Length == 8)
            {
                endTimeDate =
                    DateTime.Parse($"{endTime.Substring(0, 4)}-{endTime.Substring(4, 2)}-{endTime.Substring(6, 2)}")
                        .AddDays(1);
            }
            else
            {
                obj.Add("success", false);
                obj.Add("msg", "日期格式错误，请重新输入");
                result.Add("result", obj);
                return Content(result.ToString());
            }


            var choices = choiceService.GetChoiceByTime(startTimeDate.Value, endTimeDate.Value);

            var dict = areaExtendParamsService.GetAreaDict();

            JArray array = new JArray();

            foreach (var q in choices)
            {
                DisplayChoice display = DisplayChoice.GetDisplayChoiceFromNormakChoice(q);
                Console.WriteLine(q.Id);
                display.FirstChoice = dict[q.FirstChoice];
                array.Add(JObject.FromObject(display));
            }


            return Content(array.ToString());
        }
    }
}