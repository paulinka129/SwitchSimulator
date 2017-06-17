namespace SwitchSimulatorCore
{
    public interface IDevice
    {
        string MACAddress { get; set; }
        string Name { get; set; }
        bool Connected { get; set; }
        Port Port { get; set; }
        bool IsFree();
        void Connect(Port port);
        void Disconnect();
    }
}
