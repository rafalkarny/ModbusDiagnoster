using MaterialDesignThemes.Wpf;
using ModbusDiagnoster.Commands;
using ModbusDiagnoster.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ModbusDiagnoster.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        //
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand AddDeviceCommand { get; set; }

        
        //

        private ObservableCollection<DeviceCardViewModel> _DevicesList { get; set; }
        public ObservableCollection<DeviceCardViewModel> DevicesList
        {
            get { return this._DevicesList; }
            set
            {
                
                _DevicesList = value;
                OnPropertyChanged();
                
            }
        }


            public MainViewModel()
        {
            this._DevicesList = new ObservableCollection<DeviceCardViewModel>();
            this.AddDeviceCommand = new RelayCommand(AddDevice);

            

           /* for(int i=0;i<2;i++)
            {
                DevicesList.Add(new DeviceCard("To jest "+ i,i));
            }*/
        }

        private void AddDevice(object obj)
        {

            MsgBox msg = new MsgBox("Podaj nazwę nowego urządzenia", true);

            bool? result = msg.ShowDialog();

            if (result == true && msg.Answer!=null)
            {
                DeviceCardViewModel newDev = new DeviceCardViewModel(msg.Answer,GetFreeId());
                newDev.Delete += DeleteDevice;
                //newDev.DeleteButtonClick += DeleteDevice;
                DevicesList.Add(newDev);

            }

           
            //MessageBox.Show("Dodawanie urządzenia");
            /*foreach (DeviceCardViewModel device in _DevicesList)
            {
                MessageBox.Show(device.DeviceName);
            }*/
        }
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            MessageBox.Show("Wywołano zmianę" + propertyName);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            
        }

        private void DeleteDevice(object sender,EventArgs arg)
        {
            //MessageBox.Show(sender.ToString());

            MsgBox msg = new MsgBox("Czy na pewno chcesz usunąć urządzenie?", false);

            bool? result = msg.ShowDialog();

            if(result==true)
            {
                DevicesList.Remove((DeviceCardViewModel)sender);
            }

            

        }

        private int GetFreeId()
        {
            int freeId = DevicesList.Count;
            bool isReady = false;
           
            if(DevicesList.Count>0)
            {
                while (!isReady)
                {
                    foreach (DeviceCardViewModel device in DevicesList)
                    {
                        if (device.ID == freeId)
                        {
                            freeId += 1;
                            break;
                        }
                        else if (device == DevicesList.Last())
                        {
                            isReady = true;
                        }
                    }
                }
            }
            else
            {
                return 1;
            }
           
           

            return freeId;
        }

    }
}
