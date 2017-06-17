using System.ComponentModel;
using System.Runtime.CompilerServices;
using SwitchSimulatorCore.Annotations;

namespace SwitchSimulatorCore
{
    public class Computer : IDevice, INotifyPropertyChanged
    {
        public string MACAddress { get; set; }
        public string Name { get; set; }

        private bool connected;
        public bool Connected
        {
            get { return connected; }
            set
            {
                if (connected != value)
                {
                    connected = value;
                    OnPropertyChanged(nameof(Connected));
                }
            }
        }
        public Port Port { get; set; }
        public bool IsFree()
        {
            return !this.Connected;
        }

        public Computer()
        {
            Connected = false;
        }

        public Computer(string macAddress, string name)
        {
            MACAddress = macAddress;
            Name = name;
            Connected = false;
        }

        public void Connect(Port port)
        {
            Port = port;
            Connected = true;
        }

        public void Disconnect()
        {
            Port = null;
            Connected = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
