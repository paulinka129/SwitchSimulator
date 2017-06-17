using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using SwitchSimulatorCore.Annotations;

namespace SwitchSimulatorCore
{
    public class Switch : INotifyPropertyChanged
    {
        public int PortCount { get; }
        public Port[] Ports { get; set; }

        private Dictionary<Port, string> switchTable;
        public Dictionary<Port, string> SwitchTable
        {
            get { return switchTable; }
            set
            {
                if (switchTable != value)
                {
                    switchTable = value;
                    OnPropertyChanged(nameof(SwitchTable));
                }
            }
        }

        public string Name { get; set; }

        public Switch()
        {
            PortCount = 8;
            Ports = new Port[PortCount];
            for (var i = 0; i < PortCount; i++)
            {
                var port = new Port(i, this);
                Ports[i] = port;
            }

            SwitchTable = new Dictionary<Port, string>();
        }

        public Switch(string name) : this()
        {
            Name = name;
        }

        public Switch(int portCount, Port[] ports, Dictionary<Port, string> switchTable, string name)
        {
            PortCount = portCount;
            Ports = ports;
            SwitchTable = switchTable;
            Name = name;
        }
        
        public bool AddDevice(IDevice device)
        {
            var firstAvailablePort = Ports.FirstOrDefault(p => p.IsPortAvailable());
            if (firstAvailablePort == null) return false;
            if (firstAvailablePort.PlugIn(device))
            {
                AddMACToSwitchTable(device);
                return true;
            }
            return false;
        }

        public void RemoveDevice(IDevice device)
        {
            RemoveFromSwitchTable(device);
            Ports[device.Port.Number].Unplug();
        }

        public void RemoveAllDevices()
        {
            Ports?.Where(p => !p.IsPortAvailable()).ToList().ForEach(r => Ports[r.Number].Unplug());
        }

        public int FreePortCount()
        {
            return Ports.Count(x => x.IsPortAvailable());
        }

        public bool IsOff()
        {
            return FreePortCount() == PortCount;
        }

        public void AddMACToSwitchTable(IDevice device)
        {
            SwitchTable.Add(device.Port, device.MACAddress);
        }

        public void RemoveFromSwitchTable(IDevice device)
        {
            SwitchTable.Remove(device.Port);
        }

        public void ClearSwitchTable()
        {
            SwitchTable.Clear();
        }
        public override string ToString()
        {
            return $"{Name}, liczba wolnych portów: {FreePortCount()}";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
