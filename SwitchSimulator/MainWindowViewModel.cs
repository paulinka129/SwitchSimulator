using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using SwitchSimulatorCore;

namespace SwitchSimulator
{
    class MainWindowViewModel
    {
        public List<Switch> Switches { get; set; }
        public List<IDevice> Computers { get; set; }
        private static int ComputerNumber;
        private static int SwitchNumber;

        private bool canExecute = true;

        public bool CanExecute
        {
            get
            {
                return this.canExecute;
            }

            set
            {
                if (this.canExecute == value)
                {
                    return;
                }

                this.canExecute = value;
            }
        }
        

        public ICommand NewSwitchCommand { get; set; }

        public MainWindowViewModel()
        {
            ComputerNumber = 1;
            SwitchNumber = 1;
            Switches = new List<Switch>();
            Computers = new List<IDevice>();
            NewSwitchCommand = new RelayCommand(CreateSwitch, param => this.canExecute);
        }

        public void ShowMessage(object obj)
        {
            MessageBox.Show(obj.ToString());
        }

        public void CreateSwitch(object obj)
        {
            Switch switchDevice = new Switch(DataGenerator.GenerateDeviceName(SwitchNumber, typeof(Switch)));
            SwitchNumber++;
            //MainCanvas.AddSwitchControl(switchDevice);
        }
        public void ChangeCanExecute(object obj)
        {
            canExecute = !canExecute;
        }
    }
}
