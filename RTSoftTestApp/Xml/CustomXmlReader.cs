using RTSoftTestApp.Extensions;
using RTSoftTestApp.Manager;
using RTSoftTestApp.Model.Json;
using System;
using System.Xml;

namespace RTSoftTestApp.Xml
{
    public class CustomXmlReader
    {
        #region Xml consts

        public const string GuidPrefix = "#_";

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
            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentNullException(nameof(filename));

            var manager = new SubstationManager();
            using (var reader = new XmlTextReader(filename))
            {
                while (reader.Read())
                {
                    if (reader.NodeType != XmlNodeType.Element)
                        continue;

                    switch (reader.Name)
                    {
                        case Substation:
                            var guid = readObjectGuid(reader);
                            if (!guid.HasValue)
                                throw new NullReferenceException(nameof(guid));

                            var (name, _) = readObjectNameAndParentGuid(reader, string.Empty);

                            manager.AddSubstation(guid.Value, name);
                            break;
                        case Voltagelevel:
                            guid = readObjectGuid(reader);
                            if (!guid.HasValue)
                                throw new NullReferenceException(nameof(guid));

                            var (voltagelevelName, substationGuid) = readObjectNameAndParentGuid(reader, VoltageLevelSubstation);
                            if (!substationGuid.HasValue)
                                throw new NullReferenceException(nameof(substationGuid));

                            manager.AddVoltageLevel(substationGuid.Value, guid.Value, voltagelevelName);

                            break;
                        case SynchronousMachine:

                            guid = readObjectGuid(reader);
                            if (!guid.HasValue)
                                throw new NullReferenceException(nameof(guid));

                            var (synchronousMachineName, voltageLevelGuid) = readObjectNameAndParentGuid(reader, EquipmentContainer);
                            if (!voltageLevelGuid.HasValue)
                                throw new NullReferenceException(nameof(voltageLevelGuid));

                            manager.AddSynchronousMachine(voltageLevelGuid.Value, guid.Value, synchronousMachineName);

                            break;
                    }
                }
            }

            return manager.GetSubstations();
        }

        #region Private Methods

        private Guid? readObjectGuid(XmlReader reader)
        {
            reader.MoveToAttribute(About);
            return XmlCustomExtention.ParseGuid(reader.Value, GuidPrefix);
        }

        private (string, Guid?) readObjectNameAndParentGuid(XmlReader reader, string parentObjectTagName)
        {
            string name = null;
            Guid? guid = null;

            while (reader.Read())
            {
                if (reader.NodeType != XmlNodeType.Element)
                    continue;

                switch (reader.Name)
                {
                    case ObjectName:
                        name = reader.ReadString();
                        break;
                    default:
                        if (reader.Name == parentObjectTagName)
                        {
                            reader.MoveToAttribute(ResourceId);
                            guid = XmlCustomExtention.ParseGuid(reader.Value, GuidPrefix);
                        }

                        break;
                }

                if (!string.IsNullOrEmpty(name) && (string.IsNullOrEmpty(parentObjectTagName) || guid.HasValue))
                    break;
            }

            return (name, guid);
        }

        #endregion
    }
}
