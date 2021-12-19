using MaterialDesignThemes.Wpf;
using ModbusDiagnoster.Commands;
using ModbusDiagnoster.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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

        private string MainWorkingDirectory { get; set; }



            public MainViewModel()
        {
            _DevicesList = new ObservableCollection<DeviceCardViewModel>();
            AddDeviceCommand = new RelayCommand(AddDevice);
            try
            {
                MainWorkingDirectory=Directory.GetCurrentDirectory();
                LoadDevices();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            

           /* for(int i=0;i<2;i++)
            {
                DevicesList.Add(new DeviceCard("To jest "+ i,i));
            }*/
        }

        private void AddDevice(object obj)
        {
           

            MsgBox msg = new MsgBox("New device name: ", true);

            bool? result = msg.ShowDialog();

            if (result == true && msg.Answer!=null)
            {
                AddDeviceDirectory(msg.Answer);
                string devicesDir = MainWorkingDirectory + @"\Devices\" + msg.Answer;
                DeviceCardViewModel newDev = new DeviceCardViewModel(msg.Answer,devicesDir,GetFreeId());
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
                DeviceCardViewModel senderModel = (DeviceCardViewModel)sender;
                DeleteDeviceDirectory(MainWorkingDirectory+@"\Devices\"+ senderModel.DeviceName);
                DevicesList.Remove(senderModel);
                
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

        private void LoadDevices()
        {

            //string mainDirectory=Directory.GetCurrentDirectory();
            //MessageBox.Show(mainDirectory);

            string devicesDir =MainWorkingDirectory+ @"\Devices\";
            if(Directory.Exists(devicesDir))
            {
                string[] dirs = Directory.GetDirectories(devicesDir);
                foreach (string dir in dirs)
                {
                    string name = dir.Replace((devicesDir),"");
                    DeviceCardViewModel newDev = new DeviceCardViewModel(name,dir, GetFreeId());
                    newDev.Delete += DeleteDevice;
                    DevicesList.Add(newDev);
                }
                
            }
            else
            {
                Directory.CreateDirectory(devicesDir);
                
            }
        }
        private void AddDeviceDirectory(string deviceName)
        {
            if(Directory.Exists(MainWorkingDirectory+ @"\Devices\"))
            {
                Directory.CreateDirectory(MainWorkingDirectory+@"\Devices\" + deviceName);
            }
            else
            {
                MessageBox.Show("Directory not exist");
            }
            
        }
        private void DeleteDeviceDirectory(string deviceDirectory)
        {
            if(Directory.Exists(deviceDirectory))
            {
                //string dirToDel = MainWorkingDirectory + @"\Devices\" + deviceName;
                string[] files = Directory.GetFiles(deviceDirectory);
                string[] dirs = Directory.GetDirectories(deviceDirectory);

                foreach (string file in files)
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                }

                foreach (string dir in dirs)
                {
                    DeleteDeviceDirectory(dir);
                }

                Directory.Delete(deviceDirectory);
            }
            
        }

    }
}
