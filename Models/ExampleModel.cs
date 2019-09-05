using DeepDive2019.eWeb.API.Models.Decorators;
using Newtonsoft.Json;
using System;

namespace DeepDive2019.eWeb.API.Models
{

    [Serializable, JsonObject]
    [Read, Update]
    public class ExampleModel
    {
        [JsonIgnore]
        public Guid cst_key { get; set; }

        [JsonProperty, Read]
        public long cst_recno { get; set; }

        [JsonProperty, Create, Read]
        public string ind_first_name { get; set; }

        [JsonProperty, Create, Read]
        public string ind_last_name { get; set; }

        [JsonProperty, Create, Read, Update]
        public string ind_badge_name { get; set; }
    }
}
