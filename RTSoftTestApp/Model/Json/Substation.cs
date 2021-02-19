using Newtonsoft.Json;
using RTSoftTestApp.Converter;
using System.Collections.Generic;
using System.Text;

namespace RTSoftTestApp.Model.Json
{

    /// <summary>
    /// Подстанция
    /// </summary>    
    [JsonConverter(typeof(CustomConverter))]
    [JsonObject]
    public class Substation: BaseJsonClass
    {
        [JsonProperty(PropertyName = "List")]
        public List<VoltageLevel> VoltageLevels { get; set; }

        [JsonConstructor]
        public Substation()
        {
            VoltageLevels = new List<VoltageLevel>();
        }
    }
}
