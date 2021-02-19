using RTSoftTestApp.Extensions;
using RTSoftTestApp.Model.Json;
using System;
using System.Collections.Generic;
using System.Xml;

namespace RTSoftTestApp
{
    public class CustomXmlReader
    {
        #region Xml consts

        public const string GuidPrefix = "#_";

        public const string Root = "rdf:RDF";

        /// <summary>
        /// Тег с идентификатором объекта. Guid
        /// </summary>
        public const string About = "rdf:about";

        /// <summary>
        /// Тег с именем объекта
        /// </summary>
        public const string ObjectName = "cim:IdentifiedObject.name";

        /// <summary>
        /// Подстанция
        /// </summary>
        public const string Substation = "cim:Substation";

        /// <summary>
        /// Распределительное устройство
        /// </summary>
        public const string Voltagelevel = "cim:VoltageLevel";
        /// <summary>
        /// Тег, указывающий к какой подстанции относится распределительное устройство
        /// </summary>
        public const string VoltageLevelSubstation = "cim:VoltageLevel.Substation";

        /// <summary>
        /// Генератор
        /// </summary>
        public const string SynchronousMachine = "cim:SynchronousMachine";
        /// <summary>
        /// Атрибут, указывает на распределительное устройство. Guid
        /// </summary>
        public const string ResourceId = "rdf:resource";
        /// <summary>
        /// Тег, содержащий атрибут со ссылкой на распределительное устройство
        /// </summary>
        public const string EquipmentContainer = "cim:Equipment.EquipmentContainer";

        #endregion

        public Substation[] ReadXml(string filename)
        {
            if (String.IsNullOrWhiteSpace(filename))
                throw new ArgumentNullException(nameof(filename));

            var substations = new Substations();
            using (var reader = new XmlTextReader(filename))
            {
                while (reader.Read())
                {
                    if (reader.NodeType != XmlNodeType.Element)
                        continue;

                    switch (reader.Name)
                    {
                        case CustomXmlReader.Substation:
                            //var (guid, name) = readGuidAndName(reader);
                            var guid = readObjectGuid(reader);
                            if (!guid.HasValue)
                                throw new NullReferenceException(nameof(guid));

                            var (name, _) = readObjectNameAndParentGuid(reader, String.Empty);

                            substations.AddSubstation(guid.Value, name);
                            break;
                        case CustomXmlReader.Voltagelevel:
                            //(guid, name) = readGuidAndName(reader);
                            guid = readObjectGuid(reader);
                            if (!guid.HasValue)
                                throw new NullReferenceException(nameof(guid));

                            //var substationGuid = readParentObjectGuid(reader, CustomXmlReader.VoltageLevelSubstation);
                            var (voltagelevelName, substationGuid) = readObjectNameAndParentGuid(reader, CustomXmlReader.VoltageLevelSubstation);
                            if (!substationGuid.HasValue)
                                throw new NullReferenceException(nameof(substationGuid));

                            substations.AddVoltageLevel(substationGuid.Value, guid.Value, voltagelevelName);

                            break;
                        case CustomXmlReader.SynchronousMachine:

                            #region Parse SynchronousMachine

                            //(guid, name) = readGuidAndName(reader);
                            guid = readObjectGuid(reader);
                            if (!guid.HasValue)
                                throw new NullReferenceException(nameof(guid));

                            //var voltageLevelGuid = readParentObjectGuid(reader, CustomXmlReader.EquipmentContainer);
                            var (synchronousMachineName, voltageLevelGuid) = readObjectNameAndParentGuid(reader, CustomXmlReader.EquipmentContainer);
                            if (!voltageLevelGuid.HasValue)
                                throw new NullReferenceException(nameof(voltageLevelGuid));

                            substations.AddSynchronousMachine(voltageLevelGuid.Value, guid.Value, synchronousMachineName);

                            #endregion

                            break;
                    }
                }
            }

            return substations.GetSubstations();
        }

        #region Private Methods

        private Guid? readObjectGuid(XmlReader reader)
        {
            reader.MoveToAttribute(CustomXmlReader.About);
            return XmlCustomExtention.ParseGuid(reader.Value, GuidPrefix);
        }

        private (string, Guid?) readObjectNameAndParentGuid(XmlReader reader, string parentObjectTagName)
        {
            string name = null;
            Guid? guid = null;

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {

                    if (reader.Name == CustomXmlReader.ObjectName)
                    {
                        name = reader.ReadString();
                    }
                    else if (reader.Name == parentObjectTagName)
                    {
                        reader.MoveToAttribute(CustomXmlReader.ResourceId);
                        guid = XmlCustomExtention.ParseGuid(reader.Value, GuidPrefix);
                    }
                }

                if (!String.IsNullOrEmpty(name) && (String.IsNullOrEmpty(parentObjectTagName) || guid.HasValue))
                    break;
            }

            return (name, guid);
        }

        #endregion
    }
}
