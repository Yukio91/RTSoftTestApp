using Newtonsoft.Json;
using System.Collections.Generic;

namespace RTSoftTestApp.Model.Json
{
    /// <summary>
    /// Распределительное устройство
    /// </summary>
    [JsonObject]
    public class VoltageLevel:BaseJsonClass
    {
        [JsonProperty(PropertyName = "List")]
        public List<SynchronousMachine> SynchronousMachines { get; set; }

        [JsonConstructor]
        public VoltageLevel()
        {
            SynchronousMachines = new List<SynchronousMachine>();
        }
    }
}
