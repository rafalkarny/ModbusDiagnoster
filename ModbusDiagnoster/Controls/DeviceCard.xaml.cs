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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ModbusDiagnoster.Controls
{
    /// <summary>
    /// Logika interakcji dla klasy DeviceCard.xaml
    /// </summary>
    public partial class DeviceCard : UserControl
    {
        public event EventHandler DeleteButtonClick;
        
        public DeviceCard()
        {
            InitializeComponent();
            //DataContext = new DeviceCardViewModel();
        }

     

        public DeviceCard(string name,int id)
        {
            InitializeComponent();
            DataContext = new DeviceCardViewModel(name,id);
            


        }

        private void deleteDevice_Click(object sender, RoutedEventArgs e)
        {
            if(this.DeleteButtonClick!=null)
            {
                this.DeleteButtonClick(sender, e);
            }
            
        }
    }
}
