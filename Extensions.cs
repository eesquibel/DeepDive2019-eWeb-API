using Avectra.netForum.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace DeepDive2019.eWeb.API
{
    public static class Extensions
    {
        public static Guid? GetCustomerKey(this ApiController apiController)
        {
            var Session = HttpContext.Current?.Session;

            if (Session == null)
            {
                return null;
            }

            string customerKey = (string)Session["CustomerKey"];

            if (Guid.TryParse(customerKey, out Guid result))
            {
                return result;
            } else
            {
                return null;
            }
        }

        public static FacadeClass GetIndividual(this ApiController apiController)
        {
            var Session = HttpContext.Current?.Session;

            if (Session == null)
            {
                return null;
            }

            return (FacadeClass)Session["Individual"];
        }
    }
}