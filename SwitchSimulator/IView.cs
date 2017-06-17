using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitchSimulator
{
    public interface IView
    {
        void AddSwitchToCanvas();
        void AddComputerToCanvas();
        void RemoveSwitchFromCanvas();
        void RemoveComputerFromCanvas();
    }
}
