//
//  Souce	Path	HomeService.cs
//  create	Date	2017-2-11 14:46:12		
//  created	By	Ares.Zhao
//
using System;
using System.Linq;
using QuakeViewer.Utils;
using QuakeViewer.DAL;
using QuakeViewer.Models;

namespace QuakeViewer.Service
{
    public class AccountService
    {
        public static string NS_Session_Account_Key = "NS_Session_Account_Key";

        AccountContext accountContext { get; set; }

        public AccountService() : this(new AccountContext())
        {
        }
        public AccountService(AccountContext _accountContext)
        {
            accountContext = _accountContext;
        }


        public Account GetAccountByUserName(string userName)
        {

            var account = accountContext.Accounts.FirstOrDefault(p => p.UserName == userName);

            return account;

        }

        public Account GetAccountById(string id)
        {
            var account = accountContext.Accounts.FirstOrDefault(p => p.Id == id);
            return account;
        }

        public Account CheckIfAccountNameExists(string userName)
        {

            var account = accountContext.Accounts.FirstOrDefault(p => p.UserName.ToUpper() == userName.ToUpper());

            return account;
        }

        public Account CheckIfAccountMobileExists(string mobile)
        {
            var account = accountContext.Accounts.FirstOrDefault(p => p.Mobile == mobile);

            return account;
        }

        public void UpdateAccount(Account model)
        {
            accountContext.Accounts.Attach(model);
            accountContext.SaveChanges();
        }


        public Account CreateAccount(Account model)
        {

            if (string.IsNullOrEmpty(model.Id))
            {
                model.Id = StringHelper.GuidString();
            }

            model.CreateDate = DateTime.Now;
            model.LastLoginDate = DateTime.Now;
            model.AccountType = (int)EnumAccountType.User;
            model.status = 1;

            accountContext.Accounts.Add(model);
            accountContext.SaveChanges();
            return model;
        }
    }
}
