

namespace System.Web.Mvc
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Web;
    using QuakeViewer.Models;
    using QuakeViewer.Service;

    public class NSSessionAccountModelBuilder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            string userId = string.Empty;
            var httpContext = System.Web.HttpContext.Current;

            if (httpContext.Session[AccountService.NS_Session_Account_Key] == null)
            {
                AccountService service = new AccountService();

                var session = service.GetAccountByUserName(httpContext.User.Identity.Name);

                return session;
            }

            return httpContext.Session[AccountService.NS_Session_Account_Key];
        }
    }
}