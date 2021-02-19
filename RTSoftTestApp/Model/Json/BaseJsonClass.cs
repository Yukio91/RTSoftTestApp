using Newtonsoft.Json;
using System;

namespace RTSoftTestApp.Model.Json
{
    [JsonConverter(typeof(CustomConverter))]
    [JsonObject]
    public class BaseJsonClass
    {
        [JsonIgnore]
        public Guid Guid { get; set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }
    }
}
