using System;
using System.Collections.Generic;
using System.Linq;
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
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors.Helpers;
using SwitchSimulatorCore;

namespace SwitchSimulator.Controls
{
    /// <summary>
    /// Interaction logic for NetworkControl.xaml
    /// </summary>
    public partial class NetworkControl : Canvas
    {
       

        public List<SwitchControl> SwitchControls { get; set; }
        public List<DeviceControl> DeviceControls { get; set; }
        public NetworkControl()
        {
            InitializeComponent();
            SwitchControls = new List<SwitchControl>();
            DeviceControls = new List<DeviceControl>();
        }

        public void AddDeviceControl(IDevice computer)
        {
            var devicePanel = new DeviceControl(computer);
            devicePanel.Name = $"DevicePanel{computer.Name}";
            
            devicePanel.DragDropControlImage.ControlImage.Source = new BitmapImage(new Uri(
             "pack://application:,,,/SwitchSimulator;component/Resources/computer.png"));
            devicePanel.DragDropControlImage.ControlImage.Height = 80;
            //devicePanel.DragDropControlImage.Children.Add(deviceImage);
            //devicePanel.DragDropControlImage.ControlLabel.Content = computer.Name;
            devicePanel.OnConnect += ConnectToSwitch;
            devicePanel.OnDisconnect += DisconnetFromSwitch;
            devicePanel.OnDelete += DeleteDevice;
            devicePanel.SetLeft(10);
            devicePanel.SetTop(10);
            DeviceControls.Add(devicePanel);
            Children.Add(devicePanel);
        }

        private void DisconnetFromSwitch(object sender, EventArgs args)
        {
            var computerControl = (sender as DragDropControl).Parent as DeviceControl;
            var computer = computerControl.Device;
            var switchDevice = computer.Port.Switch;
            var switchControl = SwitchControls.FirstOrDefault(s => s.SwitchDevice == switchDevice);


            Label label = new Label();
            switchControl?.ConnectorsDict.TryGetValue(computerControl.Connector, out label);
            Children.Remove(label);
            switchControl?.ConnectorsDict.Remove(computerControl.Connector);
            Children.Remove(computerControl.Connector);

            computerControl.Connector = null;
            computerControl.SwitchControl = null;

            switchDevice.RemoveDevice(computer.Port.Number);


        }

        private void DisconnetFromSwitch(DeviceControl computerControl)
        {
            var computer = computerControl.Device;
            var switchDevice = computer.Port.Switch;
            var switchControl = SwitchControls.FirstOrDefault(s => s.SwitchDevice == switchDevice);

            //switchControl?.Connectors.Remove(computerControl.Connector);
            Label label = new Label();
            switchControl?.ConnectorsDict.TryGetValue(computerControl.Connector, out label);
            Children.Remove(label);
            switchControl?.ConnectorsDict.Remove(computerControl.Connector);
            Children.Remove(computerControl.Connector);
            computerControl.Connector = null;
            computerControl.SwitchControl = null;

            switchDevice.RemoveDevice(computer.Port.Number);
        }

        private void DeleteDevice(object sender, EventArgs args)
        {
            var control = (sender as DragDropControl).Parent as UIElement;
            if (control is DeviceControl)
            {
                var computerControl = control as DeviceControl;
                //Children.Remove(computerControl.Connector);
                //computerControl.Connector = null;
                //Children.Remove(computerControl);
                if (computerControl.Device.Connected)
                {
                    DisconnetFromSwitch(computerControl);
                }
                computerControl.SwitchControl = null;
                computerControl.Device = null;
                Children.Remove(computerControl);
            }
            else if (control is SwitchControl)
            {
                var switchControl = control as SwitchControl;
                if (!switchControl.SwitchDevice.IsOff())
                {
                    switchControl.SwitchDevice.RemoveAllDevices();
                }
                switchControl.SwitchDevice = null;

                //foreach (var connector in switchControl.Connectors)
                //{
                //    Children.Remove(connector);
                //}
                //switchControl.Connectors.Clear();

                foreach (var connector in switchControl.ConnectorsDict)
                {
                    Children.Remove(connector.Key);
                    Children.Remove(connector.Value);
                }
                switchControl.ConnectorsDict.Clear();

                Children.Remove(switchControl);
            }
        }

        public void AddSwitchControl(Switch switchDevice)
        {
            var switchPanel = new SwitchControl(switchDevice);
            switchPanel.Name = $"SwitchPanel{switchDevice.Name}";
            switchPanel.DragDropControlImage.ControlImage.Source = new BitmapImage(new Uri(
             "pack://application:,,,/SwitchSimulator;component/Resources/switch.png"));

            HideContextMenu(switchPanel);

            switchPanel.OnDelete += DeleteDevice;
            switchPanel.SetLeft(10);
            switchPanel.SetTop(10);
            SwitchControls.Add(switchPanel);
            Children.Add(switchPanel);
        }

        private void HideContextMenu(SwitchControl switchPanel)
        {
            var barItem1 = switchPanel.DragDropControlImage.ContextMenu.Items.FirstOrDefault(x => ((BarItem)x).Name == "PlugInButton") as BarItem;
            if (barItem1 != null)
            {
                barItem1.IsVisible = false;
            }

            var barItem2 = switchPanel.DragDropControlImage.ContextMenu.Items.FirstOrDefault(x => ((BarItem)x).Name == "UnplugButton") as BarItem;
            if (barItem2 != null)
            {
                barItem2.IsVisible = false;
            }
        }
        public void DisableAllDeviceControls()
        {
             
        }

        private void ConnectToSwitch(object sender, EventArgs args)
        {
            var computerControl = (sender as DragDropControl).Parent as DeviceControl;
            var selectSwitchWindow = new SelectSwitchView(SwitchControls.Select(s => s.SwitchDevice).ToList());
            var result = selectSwitchWindow.ShowDialog();
            if (result.HasValue && result.Value)
            {
                var switchControl =
                    SwitchControls.FirstOrDefault(s => s.SwitchDevice == selectSwitchWindow.SelectedSwitch);

                if (switchControl == null || computerControl == null) return;

                if (switchControl.SwitchDevice.AddDevice(computerControl.Device))
                {
                    computerControl.SwitchControl = switchControl;
                    DrawConnector(computerControl, switchControl);
                }
            }
        }

        private void DrawConnector(DeviceControl computerControl, SwitchControl switchControl)
        {
            var coordinatesSwitch = switchControl.DragDropControlImage.RenderTransform as TranslateTransform;
            var coordinatesComputer = computerControl.DragDropControlImage.RenderTransform as TranslateTransform;

            if (coordinatesSwitch == null)
            {
                coordinatesSwitch = new TranslateTransform();
                switchControl.DragDropControlImage.RenderTransform = coordinatesSwitch;
            }

            if (coordinatesComputer == null)
            {
                coordinatesComputer = new TranslateTransform();
                computerControl.DragDropControlImage.RenderTransform = coordinatesComputer;
            }

            Line connector = new Line();
            connector.X1 = coordinatesSwitch.X + switchControl.DragDropControlImage.Width / 2;
            connector.Y1 = coordinatesSwitch.Y + switchControl.DragDropControlImage.Height / 2;
            connector.X2 = coordinatesComputer.X + computerControl.DragDropControlImage.Width / 2;
            connector.Y2 = coordinatesComputer.Y + computerControl.DragDropControlImage.Height / 2;

            connector.StrokeThickness = 2;
            connector.Stroke = Brushes.Black;
            computerControl.Connector = connector;

            Label connectorLabel = new Label();
            Random rnd = new Random();
            connectorLabel.SetLeft(connector.X1 + (connector.X2-connector.X1)/3);
            connectorLabel.SetTop(connector.Y1 + (connector.Y2-connector.Y1)/3);
            connectorLabel.Content = computerControl.Device.Port.ToString();

            switchControl.ConnectorsDict.Add(connector, connectorLabel);
            Children.Add(connector);
            Children.Add(connectorLabel);

            //switchControl.Connectors.Add(connector);
            //Children.Add(connector);

            Canvas.SetZIndex(connector,-100);
        }

        private void RedrawConnectors(List<Line> connectors)
        {
            foreach (var connector in connectors)
            {
                Children.Remove(connector);
            }

            
        }

        //private void RefreshConnectors()
        //{
        //    List<Line> tempLines = Children.OfType<Line>().Select(line => line).ToList();

        //    foreach (var tempLine in tempLines)
        //    {
        //        Children.Remove(tempLine);
        //    }
        //}

    }
}
