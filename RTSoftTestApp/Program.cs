using Newtonsoft.Json;
using RTSoftTestApp.Model.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

            //XmlDocument document = new XmlDocument();
            //document.Load(@"Example.xml");

            //XmlNamespaceManager nsmgr = new XmlNamespaceManager(document.NameTable);
            //nsmgr.AddNamespace("cim", document.GetNamespaceOfPrefix("cim"));

            //var list = document.GetElementsByTagName(CustomXmlReader.Substation);
            //foreach (XmlNode node in document.DocumentElement)
            //{
            //    switch (node.Name)
            //    {
            //        case CustomXmlReader.Substation:
            //            var guid = node.ParseGuidFromXmlAttribute(CustomXmlReader.About, CustomXmlReader.GuidPrefix);
            //            var nameNode = node.SelectSingleNode("cim:IdentifiedObject.name", nsmgr);//($"@{CustomXmlReader.ObjectName}")?.Value;
            //            var name = nameNode?.InnerText;
            //            break;
            //        case CustomXmlReader.Voltagelevel:
            //            guid = node.ParseGuidFromXmlAttribute(CustomXmlReader.About, CustomXmlReader.GuidPrefix);
            //            name = node.SelectSingleNode(CustomXmlReader.ObjectName).InnerText;
            //            var substationNode = node.SelectSingleNode(CustomXmlReader.VoltageLevelSubstation);
            //            var substationGuid = substationNode.ParseGuidFromXmlAttribute(CustomXmlReader.VoltageLevelSubstation, CustomXmlReader.GuidPrefix);
            //            break;
            //        case CustomXmlReader.SynchronousMachine:
            //            guid = node.ParseGuidFromXmlAttribute(CustomXmlReader.About, CustomXmlReader.GuidPrefix);
            //            name = node.SelectSingleNode(CustomXmlReader.ObjectName).InnerText;
            //            var voltageLevelNode = node.SelectSingleNode(CustomXmlReader.EquipmentContainer);
            //            var voltageLevelGuid = voltageLevelNode.ParseGuidFromXmlAttribute(CustomXmlReader.ResourceId, CustomXmlReader.GuidPrefix);
            //            break;
            //    }
            //}
        }
    }
}
