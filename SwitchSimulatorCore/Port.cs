namespace SwitchSimulatorCore
{
    public class Port
    {
        public int Number { get; set; }
        public IDevice Device { get; set; }
        public PortStatus Status { get; set; }
        public Switch Switch { get; set; }

        public Port(int number, Switch sw)
        {
            Number = number;
            Status = PortStatus.Free;
            Switch = sw;
        }

        public bool PlugIn(IDevice device)
        {
            if (!this.IsPortAvailable() || !device.IsFree()) return false;
            device.Connect(this);
            Device = device;
            Status = PortStatus.Occupied;
            return true;
        }

        public void Unplug()
        {
            Device.Disconnect();
            Device = null;
            Status = PortStatus.Free;
        }

        public bool IsPortAvailable()
        {
            return this.Status == PortStatus.Free;
        }

        public override string ToString()
        {
            return $"port {Number}";
        }
    }

    public enum PortStatus
    {
        Free,
        Occupied
    }
}
