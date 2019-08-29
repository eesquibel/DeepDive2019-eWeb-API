using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace DeepDive2019.eWeb.API
{
    [Serializable, JsonObject]
    public class Error
    {
        [JsonProperty]
        public bool error => true;

        [JsonProperty]
        public string message { get; set; }

        [JsonProperty]
        public object code { get; set; }

        public Error(string message)
        {
            this.message = message;
        }

        public Error(string message, object code) : this(message)
        {
            this.code = code;
        }

        public Error(Exception e) : this(e.Message)
        {

        }

        public Error(Exception e, object code) : this(e.Message, code)
        {

        }

        public static implicit operator HttpContent(Error error)
        {
            return new StringContent(JsonConvert.SerializeObject(error), System.Text.Encoding.UTF8, "application/json");
        }
    }
}