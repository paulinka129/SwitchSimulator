using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DevExpress.Data.Helpers;
using DevExpress.Xpf.Core.Commands;
using SwitchSimulatorCore;
using DevExpress.Xpf.Grid;
using SwitchSimulator.Controls;
using SwitchSimulatorCore.Annotations;

namespace SwitchSimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowView : Window, INotifyPropertyChanged
    {
        public static int ComputerNumber;
        public static int SwitchNumber;
        public Switch SelectedSwitch;
        private List<Switch> switches;

        public List<Switch> Switches
        {
            get { return switches; }
            set
            {
                if (value != null)
                {
                    switches = value;
                    OnPropertyChanged(nameof(Switches));
                }
            }
        }

        public List<IDevice> Computers { get; set; }

        public MainWindowView()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            ComputerNumber = 1;
            SwitchNumber = 1;
            Switches = new List<Switch>();
            Computers = new List<IDevice>();
            SwitchesComboBox.ItemsSource = Switches;
            
            DeviceControl devCtr = new DeviceControl(null);
            //Task.Factory.StartNew(() =>
            //{
            //    Dispatcher.InvokeAsync(() => devCtr.Visibility = Visibility.Hidden);
            //    Dispatcher.InvokeAsync(() => LeftPanel.Children.Add(devCtr));
            //    Dispatcher.InvokeAsync(() => LeftPanel.Children.Remove(devCtr));
            //});
        }

        public void NewSwitch_Click(object sender, RoutedEventArgs e)
        {
            Switch switchDevice = new Switch(DataGenerator.GenerateDeviceName(SwitchNumber, typeof(Switch)));
            SwitchNumber++;
            MainCanvas.AddSwitchControl(switchDevice);
            Switches.Add(switchDevice);
            SwitchesComboBox.Items.Refresh();
        }

        private void NewComputer_Click(object sender, RoutedEventArgs e)
        {
            IDevice device = new Computer(DataGenerator.GenerateMACAddress(ComputerNumber), DataGenerator.GenerateDeviceName(ComputerNumber, typeof(IDevice)));
            ComputerNumber++;
            MainCanvas.AddDeviceControl(device);
            Computers.Add(device);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SwitchesComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedSwitch = ((ComboBox) sender).SelectedItem as Switch;
            //SwitchTableGrid.Items.Refresh();
            //SelectedSwitch?.SwitchTable.Add(SelectedSwitch.Ports[0], SelectedSwitch.Ports[0].Device.MACAddress);
            SwitchTableGrid.ItemsSource = SelectedSwitch?.SwitchTable;
        }
    }
}
