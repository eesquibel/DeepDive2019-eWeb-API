using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace DeepDive2019.eWeb.API
{
    /// <summary>
    /// Error model that can be set as the content for an HttpResponseException for returning JSON-friendly error messages
    /// </summary>
    [Serializable, JsonObject]
    public class Error
    {
        /// <summary>
        /// Always flag the response as an error with the error property.
        /// </summary>
        [JsonProperty]
        public bool error => true;

        /// <summary>
        /// The human-readable message that describes the error
        /// </summary>
        [JsonProperty]
        public string message { get; set; }

        /// <summary>
        /// The machine-readable unique code to identify this error condition.
        /// </summary>
        [JsonProperty]
        public object code { get; set; }

        /// <summary>
        /// Initializes a new instance of the Error class with a specific error message.
        /// </summary>
        /// <param name="message">The human-readable message that describes the error</param>
        public Error(string message)
        {
            this.message = message;
        }

        /// <summary>
        /// Initializes a new instance of the Error class with a specific error message and code.
        /// </summary>
        /// <param name="message">The human-readable message that describes the error</param>
        /// <param name="code">The machine-readable code that uniquely identifies this error condition.</param>
        public Error(string message, object code) : this(message)
        {
            this.code = code;
        }

        /// <summary>
        /// Initializes a new instance of the Error class with a specified Exception.
        /// </summary>
        /// <param name="e">The exception that is the cause of the current error.</param>
        public Error(Exception e) : this(e.Message)
        {

        }

        /// <summary>
        /// Initializes a new instance of the Error class with a specified Exception and code.
        /// </summary>
        /// <param name="e">The exception that is the cause of the current error.</param>
        /// <param name="code">The machine-readable code that uniquely identifies this error condition.</param>
        public Error(Exception e, object code) : this(e.Message, code)
        {

        }

        /// <summary>
        /// Used to convert this exception to a JSON response when used as an HttpContent
        /// </summary>
        /// <param name="error"></param>
        public static implicit operator HttpContent(Error error)
        {
            return new StringContent(JsonConvert.SerializeObject(error), System.Text.Encoding.UTF8, "application/json");
        }
    }
}