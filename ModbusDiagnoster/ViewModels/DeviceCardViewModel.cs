using ModbusDiagnoster.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ModbusDiagnoster.ViewModels
{
    public class DeviceCardViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand OpenDeviceWindowCommand { get; set; }
        public ICommand DeleteThisDevice { get; set; }

        public event EventHandler Delete;
        public delegate void DeleteEventHandler();

        private DeviceWindow deviceWindow { get; set; }
        private string _DeviceName { get; set; }
        public  string DeviceName
        {
            get { return this._DeviceName; }
            set
            {
                _DeviceName = value;
                OnPropertyChanged();
            }
        }
        private int _ID { get; set; }

        public int ID
        {
            get { return this._ID; }
            set
            {
                _ID = value;
                OnPropertyChanged();
            }
        }

        public DeviceCardViewModel()
        {
            this.ID = -1;
            this.DeviceName = "none";
            OpenDeviceWindowCommand=new RelayCommand(OpenDeviceWindow);
        }
        public DeviceCardViewModel(string name,int id)
        {
            this.ID = id;
            this.DeviceName = name;
            OpenDeviceWindowCommand = new RelayCommand(OpenDeviceWindow);
            DeleteThisDevice = new RelayCommand(DeleteThis);
        }


        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            //MessageBox.Show("Wywołano zmianę" + propertyName);
        }

        private void OpenDeviceWindow(object obj)
        {

            //MessageBox.Show("Klik!");
            
                deviceWindow = new DeviceWindow();
                deviceWindow.Show();
            
        }

        private void DeleteThis(object obj)
        {
            //MessageBox.Show("Wywołano usunięcie");
            if (Delete != null)
            {
                Delete(this,new EventArgs());
            }
        }

       /* public DeviceCard(string text, int id)
        {
            
            this.deviceName.Text = text;
            this.ID = id;
        }*/

    }
}
