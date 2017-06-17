using System;

namespace SwitchSimulatorCore
{
    public static class DataGenerator
    {
        public static string GenerateMACAddress(int number)
        {
            return $"00:0A:E{number}:{number}E:FD:E{number}";
        }

        public static string GenerateDeviceName(int number, Type type)
        {
            if (type == typeof(Switch))
            {
                return $"Switch0{number}";
            }
            return $"PC0{number}";
        }
    }
}
