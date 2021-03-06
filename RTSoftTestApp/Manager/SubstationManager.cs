﻿using RTSoftTestApp.Model.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RTSoftTestApp.Manager
{
    public class SubstationManager
    {
        private Dictionary<Guid, Substation> _dictSubstations = new Dictionary<Guid, Substation>();
        private Dictionary<Guid, Guid> _dictVLsSubstations = new Dictionary<Guid, Guid>();
        private List<Tuple<Guid, Guid, string>> _voltageLevelBufferedList = new List<Tuple<Guid, Guid, string>>();
        private List<Tuple<Guid, Guid, string>> _synchronousMachineBufferedList = new List<Tuple<Guid, Guid, string>>();

        public IEnumerable<Substation> GetSubstations()
        {
            foreach (var (substationGuid, voltageLevelGuid, name) in _voltageLevelBufferedList)
            {
                if (!_dictSubstations.TryGetValue(substationGuid, out var substation))
                    throw new Exception($"Key {substationGuid} not contained in dictionary {nameof(_dictSubstations)}!");

                substation.VoltageLevels.Add(
                new VoltageLevel()
                {
                    Guid = voltageLevelGuid,
                    Name = name
                });

                _dictVLsSubstations.Add(voltageLevelGuid, substationGuid);
            }
            _voltageLevelBufferedList.Clear();

            foreach (var (voltageLevelGuid, synchronousMachineGuid, name) in _synchronousMachineBufferedList)
            {
                if (!_dictVLsSubstations.TryGetValue(voltageLevelGuid, out var substationGuid))
                    throw new Exception($"Key {voltageLevelGuid} not contained in dictionary {nameof(_dictVLsSubstations)}!");

                if (!_dictSubstations.TryGetValue(substationGuid, out var substation))
                    throw new Exception($"Key {substationGuid} not contained in dictionary {nameof(_dictSubstations)}!");

                var voltageLevel = substation.VoltageLevels.FirstOrDefault(vl => vl.Guid.Equals(voltageLevelGuid));
                voltageLevel.SynchronousMachines.Add(new SynchronousMachine()
                {
                    Guid = synchronousMachineGuid,
                    Name = name
                });
            }
            _synchronousMachineBufferedList.Clear();

            return _dictSubstations.Values;
        }

        public void AddSubstation(Guid substationGuid, string name)
        {
            _dictSubstations.Add(substationGuid, new Substation()
            {
                Guid = substationGuid,
                Name = name
            });
        }

        public void AddVoltageLevel(Guid substationGuid, Guid voltageLevelGuid, string name)
        {
            if (_dictSubstations.TryGetValue(substationGuid, out var substation))
            {
                substation.VoltageLevels.Add(
                new VoltageLevel()
                {
                    Guid = voltageLevelGuid,
                    Name = name
                });

                _dictVLsSubstations.Add(voltageLevelGuid, substationGuid);
            }
            else
            {
                _voltageLevelBufferedList.Add(new Tuple<Guid, Guid, string>(substationGuid, voltageLevelGuid, name));
            }
        }

        public void AddSynchronousMachine(Guid voltageLevelGuid, Guid guid, string name)
        {
            if (!_dictVLsSubstations.TryGetValue(voltageLevelGuid, out var substationGuid))
            {
                _synchronousMachineBufferedList.Add(new Tuple<Guid, Guid, string>(voltageLevelGuid, guid, name));
                return;
            }

            if (!_dictSubstations.TryGetValue(substationGuid, out var substation))
                throw new Exception($"Key {substationGuid} not contained in dictionary {nameof(_dictSubstations)}!");

            var voltageLevel = substation.VoltageLevels.FirstOrDefault(vl => vl.Guid.Equals(voltageLevelGuid));
            voltageLevel.SynchronousMachines.Add(new SynchronousMachine()
            {
                Guid = guid,
                Name = name
            });
        }
    }
}
