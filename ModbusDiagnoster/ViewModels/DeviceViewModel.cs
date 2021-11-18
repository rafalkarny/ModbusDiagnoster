using Modbus.Device;
using ModbusDiagnoster.Commands;
using ModbusDiagnoster.Model.Communication.ModbusRTU;
using ModbusDiagnoster.Model.Communication.ModbusTCP;
using ModbusDiagnoster.Model.Converters;
using ModbusDiagnoster.Model.Variables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace ModbusDiagnoster.ViewModels
{
    public class DeviceViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand StartPooling { get; set; }
        public ICommand StopPooling { get; set; }
        public ICommand ClearLogs { get; set; }
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
        private ObservableCollection<string> _ExceptionMessages { get; set; }
        public ObservableCollection<string> ExceptionMessages
        {
            get { return this._ExceptionMessages; }
            set
            {
                _ExceptionMessages = value;
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
        public int _SamplePeriod { get; set; }
        public int SamplePeriod
        {
            get { return this._SamplePeriod; }
            set
            {
                _SamplePeriod = value;
                OnPropertyChanged();
                
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
        public IEnumerable<string> VarTypes => new[] { 
        "Decimal",
        "Integer",
        "Hexadecimal",
        "Binary",
        "BigEndianFloat",
        "LittleEndianFloat" };


        public Timer timer { get; set; }

        public DeviceViewModel(string name="Nazwa urządzenia", int id=0)
        {
            DeviceRTU = new ModbusRTU();
            DeviceTCP = new ModbusTCP();
            _Coils = new ObservableCollection<CoilsVariable>();
            _Inputs = new ObservableCollection<DiscreteInputsVariable>();
            _HoldingRegisters = new ObservableCollection<HoldingRegistersVariable>();
            _InputRegisters = new ObservableCollection<InputRegistersVariable>();
            _ExceptionMessages = new ObservableCollection<string>();

            ModbusTCPSelected = true;

            StartPooling = new AsyncRelayCommand(StartModbusPooling, (ex) => StatusMessage = ex.Message);
            StopPooling = new RelayCommand(StopPoolingMethod);
            ClearLogs = new RelayCommand(ClearLogsMethod);

            timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler(GetVariableValues);
            timer.Interval = 1000;
            timer.Enabled = true;
            timer.Stop();

           // _HoldingRegisters.CollectionChanged += ContentCollectionChanged;
        }


        public async Task StartModbusPooling()
        {
            timer.Start();
      /*      //MessageBox.Show(HoldingRegisters[0].VariableTypeFormat.ToString());

            try
            {
                *//* using (TcpClient client = new TcpClient(DeviceTCP.IPAddr, DeviceTCP.Port))
                 {
                     ModbusIpMaster master = ModbusIpMaster.CreateIp(client);

                     //MessageBox.Show("Próba odczytu: "+ HoldingRegisters[0].StartAddress +" " +master.Transport.Retries );
                     // read five input values
                     ushort startAddress = HoldingRegisters[0].StartAddress;
                     ushort numInputs = 2;
                     var result =await  master.ReadHoldingRegistersAsync(DeviceTCP.SlaveId,startAddress, numInputs);

                     //MessageBox.Show(result.ToString());

                     for (int i = 0; i < numInputs; i+=2)
                     {
                         Console.WriteLine(result[i]);
                         //HoldingRegisters[0].Value = (float)result[i];
                         Console.WriteLine(result[i + 1]);
                         string tmp = VariableType.convertToFloatLE(result[i], result[i + 1]);
                         Console.WriteLine(tmp);
                         HoldingRegisters[0].Value = tmp;
                     }
                 }*//*
                using (TcpClient client = new TcpClient(DeviceTCP.IPAddr, DeviceTCP.Port))
                {
                    ModbusIpMaster master = ModbusIpMaster.CreateIp(client);

                    

                    foreach(HoldingRegistersVariable variable in HoldingRegisters)
                    {
                        if(variable.VariableTypeFormat== "BigEndianFloat" || variable.VariableTypeFormat== "LittleEndianFloat")
                        {
                            var result = await master.ReadHoldingRegistersAsync(DeviceTCP.SlaveId, variable.StartAddress, 2);
                            if (result.Length > 0)
                            {
                                if(variable.VariableTypeFormat == "BigEndianFloat")
                                {
                                    variable.Value = VariableType.convertToFloatBE(result[0], result[1]);
                                    variable.ConvertedValue = getCalculatedValue(variable.ConversionFunction, variable.Value);
                                }
                                else
                                {
                                    variable.Value = VariableType.convertToFloatLE(result[0], result[1]);
                                    variable.ConvertedValue = getCalculatedValue(variable.ConversionFunction, variable.Value);
                                }
                            }
                        }
                        else
                        {
                            var result = await master.ReadHoldingRegistersAsync(DeviceTCP.SlaveId, variable.StartAddress, 1);
                            if(result.Length>0)
                            {
                                switch (variable.VariableTypeFormat)
                                {
                                    case "Decimal":
                                        variable.Value = VariableType.convertToDec(result[0]);
                                        variable.ConvertedValue = getCalculatedValue(variable.ConversionFunction, variable.Value);
                                        break;
                                    case "Integer":
                                        variable.Value = VariableType.convertToInt16(result[0]);
                                        variable.ConvertedValue = getCalculatedValue(variable.ConversionFunction, variable.Value);
                                        break;
                                    case "Hexadecimal":
                                        variable.Value = VariableType.convertToHex(result[0]);
                                        break;
                                    case "Binary":
                                        variable.Value = VariableType.convertToBin(result[0]);
                                        break;
                                    default:
                                        variable.Value = result[0].ToString();
                                        break;
                                }
                            }
                        }
                        
                    }
                  
                }
            }
            catch(Exception exc)
            {
                MessageBox.Show(exc.Message);
            }*/
        }


        public async void GetVariableValues(object source, ElapsedEventArgs e)
        {
            try
            {

                using (TcpClient client = new TcpClient(DeviceTCP.IPAddr, DeviceTCP.Port))
                {
                    ModbusIpMaster master = ModbusIpMaster.CreateIp(client);



                    foreach (HoldingRegistersVariable variable in HoldingRegisters)
                    {
                        if (variable.VariableTypeFormat == "BigEndianFloat" || variable.VariableTypeFormat == "LittleEndianFloat")
                        {
                            var result = await master.ReadHoldingRegistersAsync(DeviceTCP.SlaveId, variable.StartAddress, 2);
                            if (result.Length > 0)
                            {

                                DispatchService.Invoke(() =>
                                {
                                    //ExceptionMessages.Add(DateTime.Now.ToString() + " " + result[0].ToString());
                                    ExceptionMessages.Insert(0, DateTime.Now.ToString() + " " + result[0].ToString());
                                });

                                if (variable.VariableTypeFormat == "BigEndianFloat")
                                {
                                    variable.Value = VariableType.convertToFloatBE(result[0], result[1]);
                                    variable.ConvertedValue = getCalculatedValue(variable.ConversionFunction, variable.Value);
                                }
                                else
                                {
                                    variable.Value = VariableType.convertToFloatLE(result[0], result[1]);
                                    variable.ConvertedValue = getCalculatedValue(variable.ConversionFunction, variable.Value);
                                }
                            }
                            else
                            {
                                DispatchService.Invoke(() =>
                                {
                                    //ExceptionMessages.Add(DateTime.Now.ToString() + " Result was null ");
                                    ExceptionMessages.Insert(0, DateTime.Now.ToString() + " Result was null ");
                                });
                            }
                        }
                        else
                        {
                            var result = await master.ReadHoldingRegistersAsync(DeviceTCP.SlaveId, variable.StartAddress, 1);
                            
                            if (result.Length > 0)
                            {
                                DispatchService.Invoke(() =>
                                {
                                    //ExceptionMessages.Add(DateTime.Now.ToString() + " " + result[0].ToString());
                                    ExceptionMessages.Insert(0, DateTime.Now.ToString() + " " + result[0].ToString());
                                });
                                

                                switch (variable.VariableTypeFormat)
                                {
                                    case "Decimal":
                                        variable.Value = VariableType.convertToDec(result[0]);
                                        variable.ConvertedValue = getCalculatedValue(variable.ConversionFunction, variable.Value);
                                        break;
                                    case "Integer":
                                        variable.Value = VariableType.convertToInt16(result[0]);
                                        variable.ConvertedValue = getCalculatedValue(variable.ConversionFunction, variable.Value);
                                        break;
                                    case "Hexadecimal":
                                        variable.Value = VariableType.convertToHex(result[0]);
                                        break;
                                    case "Binary":
                                        variable.Value = VariableType.convertToBin(result[0]);
                                        break;
                                    default:
                                        variable.Value = result[0].ToString();
                                        break;
                                }
                            }
                            else
                            {
                                DispatchService.Invoke(() =>
                                {
                                    //ExceptionMessages.Add(DateTime.Now.ToString() + " Result was null ");
                                    ExceptionMessages.Insert(0, DateTime.Now.ToString() + " Result was null ");
                                });
                                
                            }
                        }

                    }

                }
            }
            catch (Exception exc)
            {
                //MessageBox.Show(exc.Message);
                DispatchService.Invoke(() =>
                {
                    ExceptionMessages.Add(DateTime.Now.ToString()+": " + exc.Message);
                });
                

            }
        }

        private void StopPoolingMethod(object obj)
        {
            timer.Stop();
        }
       
        private void ClearLogsMethod(object obj)
        {
            ExceptionMessages.Clear();
        }

        public string getCalculatedValue(string expression,string variableValue)
        {
            StringBuilder builder = new StringBuilder(expression);
            builder.Replace("Var", variableValue);
            builder.Replace(",", ".");

            DataTable dt = new DataTable();
            var v = dt.Compute(builder.ToString(), "");
            return v.ToString();
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
