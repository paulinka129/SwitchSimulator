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
using System.Windows.Shapes;
using DevExpress.Mvvm.Native;
using SwitchSimulatorCore;

namespace SwitchSimulator
{
    /// <summary>
    /// Interaction logic for SelectSwitchView.xaml
    /// </summary>
    public partial class SelectSwitchView : Window
    {
        public List<Switch> Switches { get; set; }
        public Switch SelectedSwitch { get; set; }

        public SelectSwitchView(List<Switch> switches)
        {
            Switches = switches;

            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            SwitchListBox.ItemsSource = Switches;
        }

        private void SwitchListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedSwitch = e.AddedItems[0] as Switch;
            DialogResult = true;
            Close();
        }

        //private void PopulateSwitchListView()
        //{
        //    var gridView = new GridView();
        //    SwitchListView.View = gridView;
        //    gridView.Columns.Add(new GridViewColumn
        //    {
        //        Header = "nazwa switcha",
        //        DisplayMemberBinding = new Binding("Name"),
        //        Width = 150,
        //    });
        //    gridView.Columns.Add(new GridViewColumn
        //    {
        //        Header = "liczba wolnych portów",
        //        DisplayMemberBinding = new Binding("PortCount"),
        //        Width = 150,
        //    });

        //    SwitchListView.Items.Add(Switches);
        //}
        
    }
}
