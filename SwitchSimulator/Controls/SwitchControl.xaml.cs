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
using DevExpress.Xpf.Core;
using SwitchSimulator.EventArg;
using SwitchSimulatorCore;

namespace SwitchSimulator.Controls
{
    /// <summary>
    /// Interaction logic for SwitchControl.xaml
    /// </summary>
    public partial class SwitchControl : StackPanel
    {
        public event OnConnect OnConnect
        {
            add { DragDropControlImage.OnConnect += value; }
            remove { DragDropControlImage.OnConnect -= value; }
        }

        public event OnDelete OnDelete
        {
            add { DragDropControlImage.OnDelete += value; }
            remove { DragDropControlImage.OnDelete -= value; }
        }

        public event OnDisconnect OnDisconnect
        {
            add { DragDropControlImage.OnDisconnect += value; }
            remove { DragDropControlImage.OnDisconnect -= value; }
        }

        public List<Line> Connectors { get; set; }
        public Dictionary<Line, Label> ConnectorsDict { get; set; }
        public Switch SwitchDevice { get; set; }

        public SwitchControl(Switch switchDevice)
        {
            SwitchDevice = switchDevice;
            //Connectors = new List<Line>();
            ConnectorsDict = new Dictionary<Line, Label>();
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            DragDropControlImage.OnMoved += DragDropControlImageOnOnMoved;
        }

        private void DragDropControlImageOnOnMoved(object sender, MovedEventArgs args)
        {
            foreach (var connector in ConnectorsDict)
            {
                connector.Key.X1 = args.X + DragDropControlImage.Width / 2;
                connector.Key.Y1 = args.Y + DragDropControlImage.Height / 2;

                //connector.Value.SetLeft(connector.Key.X1 + (connector.Key.X2 - connector.Key.X1) / 5);
                //connector.Value.SetTop(connector.Key.Y1 + (connector.Key.Y2 - connector.Key.Y1) / 5);
            }
            MoveLabels();
            //foreach (var connector in Connectors)
            //{
            //    connector.X1 = args.X + DragDropControlImage.Width / 2;
            //    connector.Y1 = args.Y + DragDropControlImage.Height / 2;
            //}
        }

        public void MoveLabels()
        {
            foreach (var connector in ConnectorsDict)
            {
                connector.Value.SetLeft(connector.Key.X1 + (connector.Key.X2 - connector.Key.X1) / 3);
                connector.Value.SetTop(connector.Key.Y1 + (connector.Key.Y2 - connector.Key.Y1) / 3);
            }
        }
    }
}
