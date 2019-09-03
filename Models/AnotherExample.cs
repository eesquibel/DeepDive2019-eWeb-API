using DeepDive2019.eWeb.API.Models.Decorators;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepDive2019.eWeb.API.Models
{

    [Serializable, JsonObject]
    [Create, Read, Update, Delete]
    public class AnotherModel
    {
        [JsonProperty, Read]
        public Guid adr_key { get; set; }

        [JsonProperty, Create, Read, Update]
        public string adr_line1 { get; set; }

        [JsonProperty, Create, Read, Update]
        public string adr_line2 { get; set; }

        [JsonProperty, Create, Read, Update]
        public string adr_city { get; set; }

        [JsonProperty, Create, Read, Update]
        public string adr_state { get; set; }

        [JsonProperty, Create, Read, Update]
        public string adr_post_code { get; set; }
    }
}
