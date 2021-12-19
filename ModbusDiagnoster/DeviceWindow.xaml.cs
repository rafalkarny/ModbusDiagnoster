using ModbusDiagnoster.ViewModels;
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

namespace ModbusDiagnoster
{
    /// <summary>
    /// Logika interakcji dla klasy DeviceWindow.xaml
    /// </summary>
    public partial class DeviceWindow : Window
    {
        public DeviceWindow()
        {
            InitializeComponent();
            DataContext = new DeviceViewModel();
        }
        public DeviceWindow(string name,string dirPath,int id)
        {
            InitializeComponent();
            DataContext = new DeviceViewModel(name,dirPath, id);
        }

        private void propertiesBtn_Click(object sender, RoutedEventArgs e)
        {
            switch(propertiesGrid.Visibility)
            {
                case Visibility.Collapsed:
                    propertiesGrid.Visibility = Visibility.Visible;
                    grdSplitter.Visibility = Visibility.Visible;
                    grdCol1.Width = new GridLength(200);


                    break;
                case Visibility.Visible:
                    propertiesGrid.Visibility = Visibility.Collapsed;
                    grdSplitter.Visibility = Visibility.Collapsed;
                    grdCol1.Width = new GridLength(0);
                    break;

                default:
                    propertiesGrid.Visibility = Visibility.Visible;
                    grdSplitter.Visibility = Visibility.Visible;
                    break;

            }
        }

    }
}
