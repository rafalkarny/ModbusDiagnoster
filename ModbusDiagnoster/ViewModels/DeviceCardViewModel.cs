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
        private string _DeviceDirectory { get; set; }

        public DeviceCardViewModel()
        {
            this.ID = -1;
            this.DeviceName = "none";
            OpenDeviceWindowCommand=new RelayCommand(OpenDeviceWindow);
            _DeviceDirectory = "";
        }
        public DeviceCardViewModel(string name,string deviceDirectory,int id)
        {
            this.ID = id;
            this.DeviceName = name;
            OpenDeviceWindowCommand = new RelayCommand(OpenDeviceWindow);
            DeleteThisDevice = new RelayCommand(DeleteThis);
            _DeviceDirectory = deviceDirectory;
        }


        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            
        }

        private void OpenDeviceWindow(object obj)
        {
            deviceWindow = new DeviceWindow(this.DeviceName,_DeviceDirectory,this.ID);
            //DeviceViewModel deviceWindow = new DeviceViewModel(this.DeviceName);    
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


    }
}
