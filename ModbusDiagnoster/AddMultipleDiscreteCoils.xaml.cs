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
    /// Logika interakcji dla klasy AddMultipleDiscreteCoils.xaml
    /// </summary>
    public partial class AddMultipleDiscreteCoils : Window
    {
        public AddMultipleDiscreteCoils()
        {
            InitializeComponent();
            DataContext =new AddMultipleDiscreteCoilsViewModel();
        }

        public AddMultipleDiscreteCoils(AddMultipleDiscreteCoilsViewModel viewmodel)
        {
            InitializeComponent();
            DataContext = viewmodel;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }


        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void okBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
