// /*
// * Souce	Path	Global.asax.cs
// * create	Date	2017-2-7 16:36:12		
// * created	By	Ares
// */
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace QuakeViewer
{
    public class Global : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
