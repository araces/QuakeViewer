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
            provinceSelectItems.Add(new SelectListItem() { Text = "---选择省---", Value = "" });
            foreach (var q in provinceList)
            {
                SelectListItem item = new SelectListItem();
                item.Text = q.Name;
                item.Value = q.Id;

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

        public ActionResult queryData(string dataType, string startTime, string endTime, int? questionIndex)
        {
            Response.ContentType = "application/json";
            JObject result = new JObject();
            JObject obj = new JObject();

            if (!questionIndex.HasValue)
            {
                obj.Add("success", false);
                obj.Add("msg", "请选择统计项！");
                result.Add("result", obj);
                return Content(result.ToString());
            }

            DateTime? startTimeDate = null;
            DateTime? endTimeDate = null;
            if (string.IsNullOrEmpty(startTime))
            {
                startTimeDate = DateTime.Now.AddDays(-30);
                endTimeDate = DateTime.Now;
            }
            else
            {
                try
                {
                    startTimeDate = DateTime.Parse(startTime);
                    endTimeDate = DateTime.Parse(endTime);
                }
                catch (Exception ex)
                {
                    obj.Add("success", false);
                    obj.Add("msg", "日期格式错误，请重新输入");
                    result.Add("result", obj);
                    return Content(result.ToString());
                }
            }


            int? dataTypeInt = null;

            if (!string.IsNullOrEmpty(dataType))
            {
                dataTypeInt = int.Parse(dataType);
            }

            var choices = choiceService.GetChoiceByTimeAndType(dataTypeInt, startTimeDate.Value, endTimeDate.Value);


            if (questionIndex.Value == 1)
            {
                var result1 = (from r in choices
                               group r by r.SecondChoice
                  into key
                               select new
                               {
                                   key = key.Key + "层",
                                   defaultKey = key.Key,
                                   count = choices.Count(p => p.SecondChoice == key.Key)
                               }).OrderBy(p => p.defaultKey);

                JObject data = new JObject();
                JArray labels = new JArray();
                JArray subData = new JArray();
                foreach (var resultObj in result1)
                {
                    labels.Add(resultObj.key);
                    subData.Add(resultObj.count);
                }

                data.Add("labels", labels);
                JArray datasets = new JArray();
                JObject objOne = new JObject();

                objOne.Add("fillColor", "rgba(220,220,220,0.5)");
                objOne.Add("strokeColor", "rgba(220,220,220,1)");
                objOne.Add("pointColor", "rgba(220,220,220,1)");
                objOne.Add("pointStrokeColor", "#fff");
                objOne.Add("data", subData);

                datasets.Add(objOne);
                data.Add("datasets", datasets);
                data.Add("type", questionIndex.Value);
                return Content(data.ToString());
            }
            if (questionIndex.Value == 2)
            {
                var result1 = (from r in choices
                               group r by r.ThirdChoice
                  into key
                               select new
                               {
                                   key = key.Key,
                                   defaultKey = key.Key,
                                   count = choices.Count(p => p.ThirdChoice == key.Key)
                               }).OrderBy(p => p.defaultKey);

                JObject data = new JObject();
                JArray labels = new JArray();
                JArray subData = new JArray();
                foreach (var resultObj in result1)
                {
                    if (resultObj.key == 1)
                    {
                        labels.Add("钢结构");
                    }
                    else if (resultObj.key == 2)
                    {
                        labels.Add("钢筋混凝土");
                    }
                    else if (resultObj.key == 3)
                    {
                        labels.Add("砖砌");
                    }
                    else
                    {
                        labels.Add("土石");
                    }
                    subData.Add(resultObj.count);
                }

                data.Add("labels", labels);
                JArray datasets = new JArray();
                JObject objOne = new JObject();

                objOne.Add("fillColor", "rgba(220,220,220,0.5)");
                objOne.Add("strokeColor", "rgba(220,220,220,1)");
                objOne.Add("data", subData);

                datasets.Add(objOne);
                data.Add("datasets", datasets);
                data.Add("type", questionIndex.Value);
                return Content(data.ToString());
            }
            if (questionIndex.Value == 3)
            {
                var result1 = (from r in choices
                               group r by r.ForthChoice
                  into key
                               select new
                               {
                                   key = key.Key,
                                   defaultKey = key.Key,
                                   count = choices.Count(p => p.ForthChoice == key.Key)
                               }).OrderBy(p => p.defaultKey);

                JObject data = new JObject();
                JArray labels = new JArray();
                JArray subData = new JArray();
                foreach (var resultObj in result1)
                {
                    if (resultObj.key == 1)
                    {
                        labels.Add("专业设计");
                    }
                    else
                    {
                        labels.Add("非专业设计");
                    }

                    subData.Add(resultObj.count);
                }

                data.Add("labels", labels);
                JArray datasets = new JArray();
                JObject objOne = new JObject();

                objOne.Add("fillColor", "rgba(220,220,220,0.5)");
                objOne.Add("strokeColor", "rgba(220,220,220,1)");
                objOne.Add("data", subData);

                datasets.Add(objOne);
                data.Add("datasets", datasets);
                data.Add("type", questionIndex.Value);
                return Content(data.ToString());
            }

            if (questionIndex.Value == 4)
            {
                var result1 = (from r in choices
                               group r by r.FifthChoice
                  into key
                               select new
                               {
                                   key = key.Key,
                                   defaultKey = key.Key,
                                   count = choices.Count(p => p.FifthChoice == key.Key)
                               }).OrderBy(p => p.defaultKey);

                JObject data = new JObject();
                JArray labels = new JArray();
                JArray subData = new JArray();
                foreach (var resultObj in result1)
                {
                    if (resultObj.key == 1)
                    {
                        labels.Add("施工质量差");
                    }
                    else if (resultObj.key == 2)
                    {
                        labels.Add("施工质量一般");
                    }
                    else
                    {
                        labels.Add("施工质量好");
                    }

                    subData.Add(resultObj.count);
                }

                data.Add("labels", labels);
                JArray datasets = new JArray();
                JObject objOne = new JObject();

                objOne.Add("fillColor", "rgba(220,220,220,0.5)");
                objOne.Add("strokeColor", "rgba(220,220,220,1)");
                objOne.Add("data", subData);

                datasets.Add(objOne);
                data.Add("datasets", datasets);
                data.Add("type", questionIndex.Value);
                return Content(data.ToString());
            }

            if (questionIndex.Value == 5)
            {
                var result1 = (from r in choices
                               group r by r.Sixth
                  into key
                               select new
                               {
                                   key = key.Key,
                                   count = choices.Count(p => p.Sixth == key.Key)
                               }).OrderBy(p => p.key);

                JObject data = new JObject();
                JArray labels = new JArray();
                JArray subData = new JArray();
                foreach (var resultObj in result1)
                {
                    if (resultObj.key == 1)
                    {
                        labels.Add("1980年前");
                    }
                    else if (resultObj.key == 2)
                    {
                        labels.Add("1980-1990年");
                    }
                    else if (resultObj.key == 3)
                    {
                        labels.Add("1990-2000年");
                    }
                    else
                    {
                        labels.Add("2000年后");
                    }

                    subData.Add(resultObj.count);
                }

                data.Add("labels", labels);
                JArray datasets = new JArray();
                JObject objOne = new JObject();

                objOne.Add("fillColor", "rgba(220,220,220,0.5)");
                objOne.Add("strokeColor", "rgba(220,220,220,1)");
                objOne.Add("data", subData);

                datasets.Add(objOne);
                data.Add("datasets", datasets);
                data.Add("type", questionIndex.Value);
                return Content(data.ToString());
            }

            return Content("");
        }
    }
}


