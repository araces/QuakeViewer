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

        public APIController()
        {
            accountService = new AccountService();
        }

        [HttpPost]
        public ActionResult Login(string userName, string password)
        {

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

                result.Add("result", obj);

                return Content(result.ToString());

            }
            obj.Add("success", true);
            obj.Add("msg", "用户名或密码错误!");

            result.Add("result", obj);

            return Content(result.ToString());
        }

        [HttpPost]
        public ActionResult Regist(string userName, string password, string mobile)
        {
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

            if (string.IsNullOrEmpty(mobile))
            {
                obj.Add("success", false);
                obj.Add("msg", "手机号码不能为空！");

                result.Add("result", obj);

                return Content(result.ToString());
            }

            var account = accountService.CheckIfAccountNameExists(userName);

            if (null != account)
            {
                obj.Add("success", false);
                obj.Add("msg", "用户名已经被占用，请使用修改新用户名！");

                result.Add("result", obj);

                return Content(result.ToString());

            }

            account = accountService.CheckIfAccountMobileExists(mobile);

            if (null != account)
            {
                obj.Add("success", false);
                obj.Add("msg", "手机号码已经被占用，请使用新手机号码！");

                result.Add("result", obj);

                return Content(result.ToString());
            }

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
    }
}
