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
using SwitchSimulator.EventArg;
using SwitchSimulatorCore;

namespace SwitchSimulator.Controls
{
    /// <summary>
    /// Interaction logic for DeviceControl.xaml
    /// </summary>
    public partial class DeviceControl : StackPanel
    {
        public event OnConnect OnConnect
        {
            add { DragDropControlImage.OnConnect += value; }
            remove { DragDropControlImage.OnConnect -= value; }
        }

        public event OnDisconnect OnDisconnect
        {
            add { DragDropControlImage.OnDisconnect += value; }
            remove { DragDropControlImage.OnDisconnect -= value; }
        }

        public event OnDelete OnDelete
        {
            add { DragDropControlImage.OnDelete += value; }
            remove { DragDropControlImage.OnDelete -= value; }
        }

        public Line Connector { get; set; }
        
        public IDevice Device { get; set; }

        public SwitchControl SwitchControl { get; set; }

        public DeviceControl(IDevice device)
        {
            Device = device;
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            DragDropControlImage.OnMoved += DragDropControlImageOnOnMoved; 
        }

        private void DragDropControlImageOnOnMoved(object sender, MovedEventArgs args)
        {
            if (Connector != null)
            {
                Connector.X2 = args.X + DragDropControlImage.Width / 2;
                Connector.Y2 = args.Y + DragDropControlImage.Height / 2;

                SwitchControl?.MoveLabels();
            }
        }
    }
}
