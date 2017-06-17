using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using DevExpress.Xpf.Core;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SwitchSimulator.Controls;
using SwitchSimulatorCore;

namespace SwitchSimulator.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public IView View { get; set; }
        private ObservableCollection<Switch> switches;
        private ObservableCollection<IDevice> computers;
        private ObservableCollection<SwitchControl> switchControls;
        private ObservableCollection<DeviceControl> computerControls;
        private ObservableDictionary<Port, string> selectedSwitchTable;
        private Switch selectedSwitch;
        private static int ComputerNumber;
        private static int SwitchNumber;
        public NetworkControl nc;
        public RelayCommand<object> NewSwitchCommand { get; set; }
        public RelayCommand<object> NewComputerCommand { get; set; }

        public ObservableDictionary<Port, string> SelectedSwitchTable
        {
            get { return selectedSwitchTable; }
            set
            {
                selectedSwitchTable = value;
                RaisePropertyChanged("SelectedSwitchTable");
            }
        }
        public ObservableCollection<SwitchControl> SwitchControls
        {
            get { return switchControls; }
            set
            {
                switchControls = value;
                RaisePropertyChanged("SwitchControls");
            }
        }

        public ObservableCollection<DeviceControl> ComputerControls
        {
            get { return computerControls; }
            set
            {
                computerControls = value;
                RaisePropertyChanged("ComputerControls");
            }
        }

        public Switch SelectedSwitch
        {
            get { return selectedSwitch; }
            set
            {
                selectedSwitch = value;
                RaisePropertyChanged("SelectedSwitch");
            }
        }
        public ObservableCollection<Switch> Switches
        {
            get { return switches; }
            set
            {
                switches = value;
                RaisePropertyChanged("Switches");
            }
        }

        public ObservableCollection<IDevice> Computers
        {
            get { return computers; }
            set
            {
                computers = value;
                RaisePropertyChanged("Computers");
            }
        }

 
        public MainViewModel(NetworkControl nc)
        {
            this.nc = nc;
            ComputerNumber = 1;
            SwitchNumber = 1;
            Switches = new ObservableCollection<Switch>();
            Computers = new ObservableCollection<IDevice>();
            SwitchControls = new ObservableCollection<SwitchControl>();
            ComputerControls = new ObservableCollection<DeviceControl>();
            SelectedSwitchTable = new ObservableDictionary<Port, string>();
            NewSwitchCommand = new RelayCommand<object>(CreateSwitch);
            NewComputerCommand = new RelayCommand<object>(CreateComputer);
            DeleteDeviceCommand = new RelayCommand<object>(DeleteDevice);
            SelectedSwitchChangedCommand = new RelayCommand<object>(SelectedSwitchChanged);

        }

        private void DeleteDevice(object obj)
        {
            var dragDropControl = obj as DragDropControl;
            if (dragDropControl != null)
            {
                var control = dragDropControl.Parent as UIElement;
                if (control is DeviceControl)
                {
                    var computerControl = control as DeviceControl;
                    //computerControl.Parent
                }
                else if (control is SwitchControl)
                {
                
                }
            }
        }

        public RelayCommand<object> DeleteDeviceCommand { get; set; }

        private void SelectedSwitchChanged(object obj)
        {
            SelectedSwitch = ((ComboBox)obj).SelectedItem as Switch;
            SelectedSwitchTable.Clear();
            foreach (var key in SelectedSwitch.SwitchTable.Keys)
            {
                SelectedSwitchTable.Add(key, SelectedSwitch.SwitchTable[key]);
            }
            //SwitchTableGrid.Items.Refresh();
            //SelectedSwitch?.SwitchTable.Add(SelectedSwitch.Ports[0], SelectedSwitch.Ports[0].Device.MACAddress);
            //SwitchTableGrid.ItemsSource = SelectedSwitch?.SwitchTable;
        }

        public RelayCommand<object> SelectedSwitchChangedCommand { get; set; }

        private void CreateComputer(object obj)
        {
            var networkControl = obj as NetworkControl;
            if (networkControl != null)
            {
                IDevice device = new Computer(DataGenerator.GenerateMACAddress(ComputerNumber), DataGenerator.GenerateDeviceName(ComputerNumber, typeof(IDevice)));
                ComputerNumber++;
                Computers.Add(device); 
                networkControl.AddDeviceControl(device);
            }
        }

        public void CreateSwitch(object obj)
        {
            var networkControl = obj as NetworkControl;
            if (networkControl != null)
            {
                Switch switchDevice = new Switch(DataGenerator.GenerateDeviceName(SwitchNumber, typeof(Switch)));
                SwitchNumber++;
                Switches.Add(switchDevice);
                networkControl.AddSwitchControl(switchDevice);
            }
        }
        
    }
}