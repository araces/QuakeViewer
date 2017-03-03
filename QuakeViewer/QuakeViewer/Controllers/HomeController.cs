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
    public class HomeController : Controller
    {
        AccountService accountService { get; set; }

        public HomeController()
        {
            accountService = new AccountService();
        }

        [HttpGet]
        public ActionResult LoginBackup()
        {
            LoginModel model = new LoginModel();

            return View(model);
        }

        [HttpGet]
        public ActionResult Login()
        {

            LoginModel model = new LoginModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {

            if (string.IsNullOrEmpty(model.UserName))
            {
                ModelState.AddModelError("login_error", "用户名不能为空！");

                return View(model);
            }

            if (string.IsNullOrEmpty(model.Password))
            {

                ModelState.AddModelError("login_error", "密码不能为空！");

                return View(model);
            }

            var account = accountService.GetAccountByUserName(model.UserName);

            if (null == account)
            {
                ModelState.AddModelError("login_error", "用户不存在，请先注册！");
                return View(model);
            }

            if (account.Password.Equals(SecurityHelper.EncryptToSHA1(model.Password)))
            {
                System.Web.Security.FormsAuthentication.SetAuthCookie(model.UserName, true);

                account.LastLoginDate = DateTime.Now;
                accountService.SaveSession(account);
                accountService.UpdateAccount(account);
                return RedirectToAction("Questions", "Quake");
            }

            ModelState.AddModelError("login_error", "用户名或密码错误！");

            return View(model);
        }

        [HttpGet]
        public ActionResult Regist()
        {

            LoginModel model = new LoginModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult Regist(LoginModel model)
        {
            if (string.IsNullOrEmpty(model.RegistUserName))
            {
                ModelState.AddModelError("regist_error", "用户名不能为空！");

                return View(model);
            }

            if (string.IsNullOrEmpty(model.RegistPassword))
            {

                ModelState.AddModelError("regist_error", "密码不能为空！");

                return View(model);
            }

            if (string.IsNullOrEmpty(model.Mobile))
            {

                ModelState.AddModelError("regist_error", "手机号码不能为空！");

                return View(model);
            }

            var account = accountService.CheckIfAccountNameExists(model.RegistUserName);

            if (null != account)
            {
                ModelState.AddModelError("regist_error", "用户名已经被占用，请使用修改新用户名！");

                return View(model);
            }

            account = accountService.CheckIfAccountMobileExists(model.Mobile);

            if (null != account)
            {
                ModelState.AddModelError("regist_error", "手机号码已经被占用，请使用修改新用户名！");

                return View(model);
            }

            account = new Account();

            account.Id = StringHelper.GuidString();
            account.UserName = model.RegistUserName;
            account.Password = model.RegistPassword;
            account.Mobile = model.Mobile;
            account.UserType = (int)EnumUserType.Web;
            account.status = 1;
            account.Password = SecurityHelper.EncryptToSHA1(account.Password);
            account.CreateDate = DateTime.Now;
            account.AccountType = (int)EnumAccountType.User;

            accountService.CreateAccount(account);

            return RedirectToAction("Login");
        }

    }
}
