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
using SwitchSimulator.EventArg;

namespace SwitchSimulator.Controls
{
    public delegate void OnConnect(object sender, EventArgs args);
    public delegate void OnDelete(object sender, EventArgs args);
    public delegate void OnDisconnect(object sender, EventArgs args);
    public delegate void OnMoved(object sender, MovedEventArgs args);

    /// <summary>
    /// Interaction logic for DragDropControl.xaml
    /// </summary>
    public partial class DragDropControl : StackPanel
    {
        protected bool isDragging;
        private Point clickPosition;
        public event OnConnect OnConnect;
        public event OnDelete OnDelete;
        public event OnDisconnect OnDisconnect;
        public event OnMoved OnMoved;
        public Canvas ParentCanvas { get; set; }

        public DragDropControl()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += new MouseButtonEventHandler(Control_MouseLeftButtonDown);
            this.MouseLeftButtonUp += new MouseButtonEventHandler(Control_MouseLeftButtonUp);
            this.MouseMove += new MouseEventHandler(Control_MouseMove);
        }

        private void Control_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isDragging = true;
            var draggableControl = sender as StackPanel;
            clickPosition = e.GetPosition(this);
            draggableControl.CaptureMouse();
        }

        private void Control_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            var draggable = sender as StackPanel;
            draggable.ReleaseMouseCapture();
        }

        public virtual Canvas GetParentCanvas()
        {
            var parent = this.Parent as Panel;
            return parent.Parent as Canvas;
            //return ParentCanvas;
        }

        private void Control_MouseMove(object sender, MouseEventArgs e)
        {
            var draggableControl = sender as StackPanel;

            if (isDragging && draggableControl != null)
            {
                var parent = GetParentCanvas();
                Point currentPosition = e.GetPosition(parent);

                var transform = draggableControl.RenderTransform as TranslateTransform;
                if (transform == null)
                {
                    transform = new TranslateTransform();
                    draggableControl.RenderTransform = transform;
                }

                transform.X = currentPosition.X - clickPosition.X;
                transform.Y = currentPosition.Y - clickPosition.Y;

                if (transform.X < 0)
                {
                    transform.X = 0;
                }
                if (transform.X > parent.Width - this.Width)
                {
                    transform.X = parent.Width - this.Width;
                }

                if (transform.Y < 0)
                {
                    transform.Y = 0;
                }
                if (transform.Y > parent.Height - this.Height)
                {
                    transform.Y = parent.Height - this.Height;
                }

                OnMoved?.Invoke(this, new MovedEventArgs(transform.X, transform.Y));
            }
        }

        private void PlugInButton_OnItemClick(object sender, ItemClickEventArgs e)
        {
            OnConnect?.Invoke(this, EventArgs.Empty);
        }

        private void DeleteDeviceButton_OnItemClick(object sender, ItemClickEventArgs e)
        {
            OnDelete?.Invoke(this, EventArgs.Empty);
        }

        private void UnplugButton_OnItemClick(object sender, ItemClickEventArgs e)
        {
            OnDisconnect?.Invoke(this, EventArgs.Empty);
        }
    }
}
