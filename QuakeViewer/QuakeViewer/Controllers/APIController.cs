using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuakeViewer.Models;
using QuakeViewer.Service;
using QuakeViewer.Utils;
using Newtonsoft.Json.Linq;

namespace QuakeViewer.Controllers
{
    public class APIController : Controller
    {

        AccountService accountService { get; set; }
        AreaParamService areaParamService { get; set; }
        ChoiceService choiceService { get; set; }

        public APIController()
        {
            accountService = new AccountService();
            areaParamService = new AreaParamService();
            choiceService = new ChoiceService();
        }

        #region userInformation

        [HttpPost]
        public ActionResult Login(string userName, string password)
        {
            Response.ContentType = "application/json";
            JObject result = new JObject();
            JObject obj = new JObject();

            if (string.IsNullOrEmpty(userName))
            {
                obj.Add("success", false);
                obj.Add("msg", "用户名不能为空！");

                result.Add("result", obj);

                return Content(result.ToString());
            }

            if (string.IsNullOrEmpty(password))
            {
                obj.Add("success", false);
                obj.Add("msg", "密码不能为空！");

                result.Add("result", obj);

                return Content(result.ToString());
            }

            var account = accountService.GetAccountByUserName(userName);

            if (null == account)
            {
                obj.Add("success", false);
                obj.Add("msg", "用户不存在，请先注册！");

                result.Add("result", obj);

                return Content(result.ToString());
            }

            if (account.Password.Equals(SecurityHelper.EncryptToSHA1(password)))
            {
                account.LastLoginDate = DateTime.Now;

                accountService.UpdateAccount(account);

                obj.Add("success", true);
                obj.Add("msg", string.Empty);
                obj.Add("token", account.Id);

                result.Add("result", obj);

                return Content(result.ToString());

            }
            obj.Add("success", true);
            obj.Add("msg", "用户名或密码错误!");
            obj.Add("token", account.Id);

            result.Add("result", obj);

            return Content(result.ToString());
        }

        [HttpPost]
        public ActionResult Regist(string userName, string password, string mobile)
        {
            Response.ContentType = "application/json";
            JObject result = new JObject();
            JObject obj = new JObject();

            if (string.IsNullOrEmpty(userName))
            {
                obj.Add("success", false);
                obj.Add("msg", "用户名不能为空！");

                result.Add("result", obj);

                return Content(result.ToString());
            }

            if (string.IsNullOrEmpty(password))
            {
                obj.Add("success", false);
                obj.Add("msg", "密码不能为空！");

                result.Add("result", obj);

                return Content(result.ToString());
            }
            /*
            if (string.IsNullOrEmpty(mobile))
            {
                obj.Add("success", false);
                obj.Add("msg", "手机号码不能为空！");

                result.Add("result", obj);

                return Content(result.ToString());
            }
            */

            var account = accountService.CheckIfAccountNameExists(userName);

            if (null != account)
            {
                obj.Add("success", false);
                obj.Add("msg", "用户名已经被占用，请使用修改新用户名！");

                result.Add("result", obj);

                return Content(result.ToString());

            }
            /*
            account = accountService.CheckIfAccountMobileExists(mobile);

            if (null != account)
            {
                obj.Add("success", false);
                obj.Add("msg", "手机号码已经被占用，请使用新手机号码！");

                result.Add("result", obj);

                return Content(result.ToString());
            }
            */
            account = new Account();

            account.Id = StringHelper.GuidString();
            account.UserName = userName;
            account.Password = password;
            account.Mobile = mobile;
            account.UserType = (int)EnumUserType.Mobile;
            account.status = 1;
            account.Password = SecurityHelper.EncryptToSHA1(account.Password);
            account.CreateDate = DateTime.Now;
            account.AccountType = (int)EnumAccountType.User;

            accountService.CreateAccount(account);

            obj.Add("success", true);
            obj.Add("msg", string.Empty);

            result.Add("result", obj);

            return Content(result.ToString());
        }

        #endregion

        public ActionResult GetAllProvince(string token)
        {
            Response.ContentType = "application/json";
            JObject result = new JObject();
            JObject obj = new JObject();

            if (string.IsNullOrEmpty(token))
            {
                obj.Add("success", false);
                obj.Add("msg", "权限错误，请登录后使用");

                result.Add("result", obj);

                return Content(result.ToString());
            }

            List<AreaParam> provinceList = areaParamService.GetAreaParams();

            JArray provinceArray = JArray.FromObject(provinceList);

            obj.Add("provinces", provinceArray);

            obj.Add("success", true);
            obj.Add("msg", string.Empty);

            result.Add("result", obj);

            return Content(result.ToString());
        }

        public ActionResult CheckIfHasResult(string token)
        {
            Response.ContentType = "application/json";
            JObject result = new JObject();
            JObject obj = new JObject();

            if (string.IsNullOrEmpty(token))
            {
                obj.Add("success", false);
                obj.Add("msg", "权限错误，请登录后使用");

                result.Add("result", obj);

                return Content(result.ToString());
            }

            //var account = accountService.GetAccountById(token);



            var choice = choiceService.GetChoiceByUserId(token, (int)EnumUserType.Mobile);

            if (choice != null)
            {
                ResultModel existedModel = new ResultModel();
                existedModel.UserName = choice.UserName;
                existedModel.MajorLevel = choice.MajorResult.Value;
                existedModel.Reason1 = choice.ThirdChoice.Value;
                existedModel.Reason2 = choice.ForthChoice.Value;
                existedModel.Reason3 = choice.FifthChoice.Value;

                obj.Add("resultModel", JObject.FromObject(existedModel));
                obj.Add("success", true);
                result.Add("result", obj.ToString());

                return Content(result.ToString());
            }

            obj.Add("msg", string.Empty);
            obj.Add("success", false);
            result.Add("result", obj.ToString());
            return Content(result.ToString());


        }

        [HttpPost]
        public ActionResult QuestionsResult(string token, string regionId,
                                            int storyNum,
                                            int struType,
                                            int isDesigned,
                                            int contructionQuality,
                                            int builtYearGroup)
        {
            Response.ContentType = "application/json";
            JObject result = new JObject();
            JObject obj = new JObject();

            if (string.IsNullOrEmpty(token))
            {
                obj.Add("success", false);
                obj.Add("msg", "权限错误，请登录后使用");

                result.Add("result", obj);

                return Content(result.ToString());
            }

            Choice choice = new Choice();

            choice.Id = StringHelper.GuidString();

            var account = accountService.GetAccountById(token);

            choice.UserId = account.Id;
            choice.UserName = account.UserName;

            var areaParam = areaParamService.GetAreaParamById(regionId);

            choice.FirstChoice = regionId;
            choice.SecondChoice = storyNum;
            choice.ThirdChoice = struType;
            choice.ForthChoice = isDesigned;
            choice.FifthChoice = contructionQuality;
            choice.Sixth = builtYearGroup;
            choice.CreateDate = DateTime.Now;
            choice.FromType = (int)EnumUserType.Mobile;



            QuakeViewerCalculate quakeViewerCalculate = new QuakeViewerCalculate();

            /* quakeViewerCalculate.InputData(areaParam.GroupNo.Value,
                                            areaParam.SiteType.Value,
                                            areaParam.IntensityDegree.Value,
                                            choice.SecondChoice.Value,
                                            choice.ThirdChoice.Value,
                                            choice.ForthChoice.Value,
                                            choice.FifthChoice.Value,
                                            choice.Sixth.Value == 1); */

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

            ResultModel model = new ResultModel();
            model.UserName = choice.UserName;
            model.MajorLevel = choice.MajorResult.Value;
            model.MinorLevel = choice.MinorResult.Value;
            model.Reason1 = choice.ThirdChoice.Value;
            model.Reason2 = choice.ForthChoice.Value;
            model.Reason3 = choice.FifthChoice.Value;

            obj.Add("resultModel", JObject.FromObject(model));
            obj.Add("success", true);
            result.Add("result", obj);

            return Content(result.ToString());
        }

        public ActionResult UpdateAreaParams()
        {

            var areas = areaParamService.GetAreaParams();
            Dictionary<string, AreaParam> dicts = areas.ToDictionary(p => p.Id, p => p);

            foreach (var q in areas)
            {
                if (q.ParentId.Equals("0"))
                {
                    q.Description = q.Name;
                }
                else
                {
                    string description = null;
                    GetFullString(dicts, q.Id, ref description);
                    q.Description = description;
                    areaParamService.UpdateParams(q);
                }
            }



            return Content("OK");
        }

        public void GetFullString(Dictionary<string, AreaParam> dicts, string id, ref string description)
        {

            AreaParam areaParam = dicts[id];
            if (string.IsNullOrEmpty(description))
            {
                description = areaParam.Name;
            }
            else
            {
                description = $"{areaParam.Name}.{description}";
            }

            string parentId = areaParam.ParentId;
            if (parentId.Equals("0"))
            {
                return;
            }
            else
            {
                GetFullString(dicts, parentId, ref description);
            }

        }


    }
}
