using Newtonsoft.Json;
using RTSoftTestApp.Xml;
using System.IO;

namespace RTSoftTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var customReader = new CustomXmlReader();
            var substations = customReader.ReadXml(@"Example.xml");

            JsonSerializer serializer = new JsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Ignore;
            serializer.Formatting = Formatting.Indented;

            using (var sw = new StreamWriter(@"json.json"))
            {
                using (var writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, substations);
                }
            }
        }
    }
}
