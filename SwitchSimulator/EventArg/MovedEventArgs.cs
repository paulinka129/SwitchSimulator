using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitchSimulator.EventArg
{
    public class MovedEventArgs : EventArgs
    {
        public double X { get; set; }
        public double Y { get; set; }

        public MovedEventArgs(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
