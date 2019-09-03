using Microsoft.Owin;
using Owin;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

[assembly: OwinStartup(typeof(DeepDive2019.eWeb.API.Startup))]

namespace DeepDive2019.eWeb.API
{
    public class Startup
    {
        // This is the method that OwinStartup will call
        // Largely copied from Application_Startup that would normally be in Global.asax
        public void Configuration(IAppBuilder app)
        {

            GlobalConfiguration.Configure(Register);

            // Moved all these methods from thier own classes into this one for readability
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            RegisterBundles(BundleTable.Bundles);
        }

        // Copied from WebApiConfig.Register that would normally be in App_Start\WebApiconfig.cs
        public static void Register(HttpConfiguration config)
        {
            // Enable CORS
            config.EnableCors();

            // Web API attribute routes
            config.MapHttpAttributeRoutes();

            // Our API route was moved to the next method for readability

            // Make it easier to get the desired format when testing in browser
            config.Formatters.JsonFormatter.AddQueryStringMapping("format", "json", "application/json");
            config.Formatters.XmlFormatter.AddQueryStringMapping("format", "xml", "application/xml");
        }

        // Copied from RouteConfig.RegisterRoutes that would normally be in App_Start\RouteConfig.cs
        // Also moved the MapHttpRoute() call from WebApiConfig.Register so our routes are all in one place
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Web API routes
            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            ).RouteHandler = new SessionStateRouteHandler();

            // MVC routes
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }

        // The following two method aren't used in this example project, but are functional if you want to set it up for your project

        public static void RegisterBundles(BundleCollection bundles)
        {
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
        }
    }
}
