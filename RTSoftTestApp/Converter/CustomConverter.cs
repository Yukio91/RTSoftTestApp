using Newtonsoft.Json;
using RTSoftTestApp.Model.Json;
using System;

namespace RTSoftTestApp.Converter
{
    public class CustomConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Substation);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var jsonClass = value as Substation;
            writer.WriteStartObject();
            writer.WritePropertyName(jsonClass.Name);

            writer.WriteStartArray();
            foreach (var vl in jsonClass.VoltageLevels)
            {
                writer.WriteStartObject();
                writer.WritePropertyName(vl.Name);

                writer.WriteStartArray();

                foreach (var sm in vl.SynchronousMachines)
                {
                    writer.WriteValue(sm.Name);
                }

                writer.WriteEndArray();

                writer.WriteEndObject();

            }

            writer.WriteEndArray();

            writer.WriteEndObject();
            writer.Flush();
        }
    }
}
