using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace DeepDive2019.eWeb.API
{
    public class ApiResponseException : HttpResponseException
    {
        public ApiResponseException(HttpStatusCode status) : base(new HttpResponseMessage(status))
        {
        }

        public ApiResponseException(HttpStatusCode status, string message) : this(status)
        {
            Response.Content = new Error(message);
        }

        public ApiResponseException(HttpStatusCode status, string message, object code) : this(status)
        {
            Response.Content = new Error(message, code);
        }

        public ApiResponseException(HttpStatusCode status, Exception e) : this(status, e.Message)
        {
        }

        public ApiResponseException(HttpStatusCode status, Exception e, object code) : this(status, e.Message, code)
        {
        }
    }
}