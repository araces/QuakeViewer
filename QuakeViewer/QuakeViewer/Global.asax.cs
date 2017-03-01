// /*
// * Souce	Path	Global.asax.cs
// * create	Date	2017-2-7 16:36:12		
// * created	By	Ares
// */
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using QuakeViewer.Models;
using System.Web.Mvc;
using System.Web.Routing;
using QuakeViewer.Utils;

namespace QuakeViewer
{
    public class Global : HttpApplication
    {
        protected void Application_Start()
        {
            ModelBinders.Binders.Add(typeof(Account), new NSSessionAccountModelBuilder());
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception unhandledException = Server.GetLastError();
            LogHelper.Error(unhandledException);
        }
    }
}
