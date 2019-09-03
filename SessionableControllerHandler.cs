using System.Web.Http.WebHost;
using System.Web.Routing;
using System.Web.SessionState;

namespace DeepDive2019.eWeb.API
{
    /// <summary>
    /// A HttpControllerHandler that implements IRequiresSessionState to enable sessions data on an APIController
    /// </summary>
    public class SessionableControllerHandler : HttpControllerHandler, IRequiresSessionState
    {
        public SessionableControllerHandler(RouteData routeData) : base(routeData)
        {
        }
    }
}
