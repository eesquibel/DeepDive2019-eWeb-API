using System.Web;
using System.Web.Routing;

namespace DeepDive2019.eWeb.API
{
    /// <summary>
    /// An IRouteHandler that returns an <em>SessionableControllerHandler</em> to enable session state on an API Controller
    /// </summary>
    public class SessionStateRouteHandler : IRouteHandler
    {
        IHttpHandler IRouteHandler.GetHttpHandler(RequestContext requestContext)
        {
            return new SessionableControllerHandler(requestContext.RouteData);
        }
    }
}
