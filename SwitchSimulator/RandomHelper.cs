using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitchSimulator
{
    public static class RandomHelper
    {
        public static int GenerateRandomLength()
        {
            Random rnd = new Random();
            return rnd.Next(50, 100);
        }
    }
}
