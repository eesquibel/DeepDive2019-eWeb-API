using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace DeepDive2019.eWeb.API
{
    /// <summary>
    /// Throwable error exception that uses the Error model class for outputing a JSON-friendly error message.
    /// </summary>
    public class ApiResponseException : HttpResponseException
    {
        /// <summary>
        /// Internal constructor, do not use.
        /// </summary>
        /// <param name="status">The status code for the HTTP response.</param>
        protected ApiResponseException(HttpStatusCode status) : base(new HttpResponseMessage(status))
        {
        }

        /// <summary>
        /// Initializes a new instance of the ApiResponseException class with a specified error message.
        /// </summary>
        /// <param name="status">The status code for the HTTP response.</param>
        /// <param name="message">The human-readable message that describes the error.</param>
        public ApiResponseException(HttpStatusCode status, string message) : this(status)
        {
            Response.Content = new Error(message);
        }

        /// <summary>
        /// Initializes a new instance of the ApiResponseException class with a specified error message and code.
        /// </summary>
        /// <param name="status">The status code for the HTTP response.</param>
        /// <param name="message">The human-readable message that describes the error.</param>
        /// <param name="code">The machine-readable code that uniquely identifies this error condition.</param>
        public ApiResponseException(HttpStatusCode status, string message, object code) : this(status)
        {
            Response.Content = new Error(message, code);
        }

        /// <summary>
        /// Initializes a new instance of the ApiResponseException class with a specified Exception.
        /// </summary>
        /// <param name="status">The status code for the HTTP response.</param>
        /// <param name="e">The exception that is the cause of the current exception.</param>
        public ApiResponseException(HttpStatusCode status, Exception e) : this(status, e.Message)
        {
            new Exception("sdf", e);
        }

        /// <summary>
        /// Initializes a new instance of the ApiResponseException class with a specified Exception and code.
        /// </summary>
        /// <param name="status">The status code for the HTTP response.</param>
        /// <param name="e">The exception that is the cause of the current exception.</param>
        /// <param name="code">The machine-readable code that uniquely identifies this error condition.</param>
        public ApiResponseException(HttpStatusCode status, Exception e, object code) : this(status, e.Message, code)
        {
        }
    }
}