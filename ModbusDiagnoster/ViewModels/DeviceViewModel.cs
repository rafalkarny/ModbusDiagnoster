using Modbus.Device;
using ModbusDiagnoster.Commands;
using ModbusDiagnoster.Model.Communication.ModbusRTU;
using ModbusDiagnoster.Model.Communication.ModbusTCP;
using ModbusDiagnoster.Model.Variables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ModbusDiagnoster.ViewModels
{
    public class DeviceViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand StartPooling { get; set; }
        private ObservableCollection<CoilsVariable> _Coils { get; set; }
        public ObservableCollection<CoilsVariable> Coils
        {
            get { return this._Coils; }
            set
            {
                _Coils = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<DiscreteInputsVariable> _Inputs { get; set; }
        public ObservableCollection<DiscreteInputsVariable> Inputs
        {
            get { return this._Inputs; }
            set
            {
                _Inputs = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<HoldingRegistersVariable> _HoldingRegisters { get; set; }
        public ObservableCollection<HoldingRegistersVariable> HoldingRegisters
        {
            get { return this._HoldingRegisters; }
            set
            {
                _HoldingRegisters = value;
                OnPropertyChanged();
                
            }
        }
        private ObservableCollection<InputRegistersVariable> _InputRegisters { get; set; }
        public ObservableCollection<InputRegistersVariable> InputRegisters
        {
            get { return this._InputRegisters; }
            set
            {
                _InputRegisters = value;
                OnPropertyChanged();
            }
        }
        private ModbusTCP _DeviceTCP { get; set; }
        public ModbusTCP DeviceTCP
        {
            get { return this._DeviceTCP; }
            set
            {
                _DeviceTCP = value;
                OnPropertyChanged();
            }
        }
        private ModbusRTU _DeviceRTU { get; set; }
        public ModbusRTU DeviceRTU
        {
            get { return this._DeviceRTU; }
            set
            {
                _DeviceRTU = value;
                OnPropertyChanged();
            }
        }

        private string _statusMessage;
        public string StatusMessage
        {
            get
            {
                return _statusMessage;
            }
            set
            {
                _statusMessage = value;
                OnPropertyChanged(nameof(StatusMessage));
                OnPropertyChanged(nameof(HasStatusMessage));
                MessageBox.Show(_statusMessage);
            }
        }

        public bool HasStatusMessage => !string.IsNullOrEmpty(StatusMessage);

        private bool _ModbusTCPSelected { get; set; }
        public bool ModbusTCPSelected
        {
            get { return this._ModbusTCPSelected; }
            set
            {
                _ModbusTCPSelected = value;
                OnPropertyChanged();
            }
        }

        private bool _ModbusRTUSelected { get; set; }
        public bool ModbusRTUSelected
        {
            get { return this._ModbusRTUSelected; }
            set
            {
                _ModbusRTUSelected = value;
                OnPropertyChanged();
                //MessageBox.Show("Wybrano modbusa RTU");
            }
        }
        public IEnumerable<string> VarTypes => new[] { "Decimal",
        "Integer",
        "Hexadecimal",
        "Binary",
        "BigEndianFloat",
        "LittleEndianFloat" };


        public DeviceViewModel(string name="Nazwa urządzenia", int id=0)
        {
            DeviceRTU = new ModbusRTU();
            DeviceTCP = new ModbusTCP();
            _Coils = new ObservableCollection<CoilsVariable>();
            _Inputs = new ObservableCollection<DiscreteInputsVariable>();
            _HoldingRegisters = new ObservableCollection<HoldingRegistersVariable>();
            _InputRegisters = new ObservableCollection<InputRegistersVariable>();

            StartPooling = new AsyncRelayCommand(StartModbusPooling, (ex) => StatusMessage = ex.Message);

           // _HoldingRegisters.CollectionChanged += ContentCollectionChanged;
        }


        public async Task StartModbusPooling()
        {

            MessageBox.Show(HoldingRegisters[0].VariableTypeFormat.ToString());

            try
            {
                using (TcpClient client = new TcpClient(DeviceTCP.IPAddr, DeviceTCP.Port))
                {
                    ModbusIpMaster master = ModbusIpMaster.CreateIp(client);

                    //MessageBox.Show("Próba odczytu: "+ HoldingRegisters[0].StartAddress +" " +master.Transport.Retries );
                    // read five input values
                    ushort startAddress = HoldingRegisters[0].StartAddress;
                    ushort numInputs = 1;
                    var result =await  master.ReadHoldingRegistersAsync(DeviceTCP.SlaveId,startAddress, numInputs);

                    //MessageBox.Show(result.ToString());

                    for (int i = 0; i < numInputs; i++)
                    {
                        Console.WriteLine(result[i]);
                        HoldingRegisters[0].Value = (float)result[i];
                    }
                }
            }
            catch(Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            //MessageBox.Show("Wywołano zmianę" + propertyName);
        }

  /*      public void ContentCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (INotifyPropertyChanged item in e.OldItems)
                {
                    item.PropertyChanged -= EntityViewModelPropertyChanged;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (INotifyPropertyChanged item in e.NewItems)
                {
                  item.PropertyChanged += EntityViewModelPropertyChanged;
                }
            }
        }

        public void EntityViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            
            OnPropertyChanged();
            //This will get called when the property of an object inside the collection changes
        }*/

        /* private void _innerStuff_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
         {
             if (e.NewItems != null)
             {
                 foreach (Object item in e.NewItems)
                 {
                     ((INotifyPropertyChanged)item).PropertyChanged += ItemPropertyChanged;
                 }
             }
             if (e.OldItems != null)
             {
                 foreach (Object item in e.OldItems)
                 {
                     ((INotifyPropertyChanged)item).PropertyChanged -= ItemPropertyChanged;
                 }
             }
         }

         private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
         {
             OnPropertyChanged();
             //This will get called when the property of an object inside the collection changes
         }*/

    }
}
