using Modbus.Device;
using ModbusDiagnoster.Commands;
using ModbusDiagnoster.Model.Communication.ModbusRTU;
using ModbusDiagnoster.Model.Communication.ModbusTCP;
using ModbusDiagnoster.Model.Converters;
using ModbusDiagnoster.Model.Sniffers;
using ModbusDiagnoster.Model.Variables;
using PacketDotNet;
using SharpPcap;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using MaterialDesignThemes.Wpf;
using ModbusDiagnoster.Model.Communication;

namespace ModbusDiagnoster.ViewModels
{
    public class DeviceViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand StartPooling { get; set; }
        public ICommand StopPooling { get; set; }
        public ICommand ClearLogs { get; set; }
        public ICommand AddCoilVar { get; set; }
        public ICommand AddDiscreteVar { get; set; }
        public ICommand AddHoldingVar { get; set; }
        public ICommand AddInputVar { get; set; }
        public ICommand AddMultipleCoilVar { get; set; }
        public ICommand AddMultipleDiscreteVar { get; set; }
        public ICommand AddMultipleHoldingVar { get; set; }
        public ICommand AddMultipleInputVar { get; set; }
        public ICommand DeleteMultipleHoldingVar { get; set; }
        public ICommand StartNetCap { get; set; }
        public ICommand StopNetCap { get; set; }
        public ICommand ClearPackets { get; set; }


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
        private ObservableCollection<ICaptureDevice> _Interfaces { get; set; }
        public ObservableCollection<ICaptureDevice> Interfaces
        {
            get { return this._Interfaces; }
            set
            {
                _Interfaces = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<MyPacket> _Packets { get; set; }
        public ObservableCollection<MyPacket> Packets
        {
            get { return this._Packets; }
            set
            {
                _Packets = value;
                OnPropertyChanged();
            }
        }
        private ModbusTCP _DeviceTCP { get; set; }
        public ModbusTCP DeviceTCP
        {
            get { return this._DeviceTCP; }
            set
            {
                // _DeviceTCP = value;
                try
                {
                    ExceptionMessages.Insert(0, DateTime.Now.ToString() + " Wrong data in TCP Client parameters " +
                                value.IPAddr + " " + value.Port.ToString() + " " + value.SlaveId.ToString());

                    if (_DeviceTCP.TCPclient != null)
                    {
                        byte[] sampleaddr = { 127, 0, 0, 1 };
                        IPAddress addr = new IPAddress(sampleaddr);
                        if (IPAddress.TryParse(value.IPAddr, out addr) && value.Port > 0 && value.Port < 65535 && value.SlaveId > 0 && value.SlaveId < 65535)
                        {
                            _DeviceTCP = value;

                            _DeviceTCP.TCPclient.Close();
                            _DeviceTCP.TCPclient.Dispose();
                            _DeviceTCP.TCPclient = new TcpClient(_DeviceTCP.IPAddr, _DeviceTCP.Port);

                            ExceptionMessages.Insert(0, DateTime.Now.ToString() + " Changed TCP Client parameters ");
                        }
                        else
                        {
                            ExceptionMessages.Insert(0, DateTime.Now.ToString() + " Wrong data in TCP Client parameters " +
                                value.IPAddr + " " + value.Port.ToString() + " " + value.SlaveId.ToString());

                        }
                    }
                }
                catch (Exception exc)
                {
                    ExceptionMessages.Insert(0, DateTime.Now.ToString() + exc.Message);
                }
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

        private ObservableCollection<CoilsVariable> _SelectedCoils { get; set; }
        public ObservableCollection<CoilsVariable> SelectedCoils
        {
            get { return this._SelectedCoils; }
            set
            {
                _SelectedCoils = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<DiscreteInputsVariable> _SelectedDiscrets { get; set; }
        public ObservableCollection<DiscreteInputsVariable> SelectedDiscrets
        {
            get { return this._SelectedDiscrets; }
            set
            {
                _SelectedDiscrets = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<HoldingRegistersVariable> _SelectedHR { get; set; }
        public ObservableCollection<HoldingRegistersVariable> SelectedHR
        {
            get { return this._SelectedHR; }
            set
            {
                _SelectedHR = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<InputRegistersVariable> _SelectedIR { get; set; }
        public ObservableCollection<InputRegistersVariable> SelectedIR
        {
            get { return this._SelectedIR; }
            set
            {
                _SelectedIR = value;
                OnPropertyChanged();
            }
        }

        // private ModbusIpMaster master;

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
        private ICaptureDevice _SelectedInterface { get; set; }
        public ICaptureDevice SelectedInterface
        {
            get { return this._SelectedInterface; }
            set
            {
                _SelectedInterface = value;
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
        public bool timerStop { get; set; }


        public DeviceViewModel(string name = "Nazwa urządzenia", int id = 0)
        {
            _DeviceRTU = new ModbusRTU();
            _DeviceTCP = new ModbusTCP();
            _Coils = new ObservableCollection<CoilsVariable>();
            _Inputs = new ObservableCollection<DiscreteInputsVariable>();
            _HoldingRegisters = new ObservableCollection<HoldingRegistersVariable>();
            _InputRegisters = new ObservableCollection<InputRegistersVariable>();
            _ExceptionMessages = new ObservableCollection<string>();
            _Interfaces = new ObservableCollection<ICaptureDevice>();
            _Packets = new ObservableCollection<MyPacket>();

            ModbusTCPSelected = true;
            LoadDevices();  //Sniffer interfaces

            /// TODO when saving and loading will be avaible!!!
            try
            {
                _DeviceTCP.TCPclient = new TcpClient("127.0.0.1", 502);
            }
            catch (Exception exc)
            {
                ExceptionMessages.Insert(0, DateTime.Now.ToString() + exc.Message);
            }

            StartPooling = new AsyncRelayCommand(StartModbusPooling, (ex) => StatusMessage = ex.Message);
            StopPooling = new RelayCommand(StopPoolingMethod);
            ClearLogs = new RelayCommand(ClearLogsMethod);
            StartNetCap = new RelayCommand(OnStartNetCapture);
            StopNetCap = new RelayCommand(OnStopNetCapture);
            ClearPackets = new RelayCommand(OnClearPackets);
            AddHoldingVar = new RelayCommand(OnAddHoldingVar);
            AddMultipleHoldingVar = new RelayCommand(OnAddMultipleHoldingVars);
            DeleteMultipleHoldingVar = new RelayCommand(OnDeleteMultipleHoldingVar);

            timer = new Timer();
            // timer.Elapsed += new ElapsedEventHandler(GetVariableValues);
            timer.Elapsed += new ElapsedEventHandler(GetGroupedVariableValues);
            timer.Interval = 1000;
            timer.Enabled = true;
            timer.Stop();
            timerStop = false;
            // _HoldingRegisters.CollectionChanged += ContentCollectionChanged;
        }



        public async Task StartModbusPooling()
        {
            try
            {
                // ModbusIpMaster master = ModbusIpMaster.CreateIp(DeviceTCP.TCPclient);
                timer.Start();
                DispatchService.Invoke(() =>
                {
                    //ExceptionMessages.Add(DateTime.Now.ToString() + " Result was null ");
                    ExceptionMessages.Insert(0, DateTime.Now.ToString() + " Starting pooling");
                });
            }
            catch (Exception exc)
            {
                DispatchService.Invoke(() =>
                {
                    //ExceptionMessages.Add(DateTime.Now.ToString() + " Result was null ");
                    ExceptionMessages.Insert(0, DateTime.Now.ToString() + exc.Message);
                });
            }


        }


        public async void GetVariableValues(object source, ElapsedEventArgs e)
        {
            try
            {
                timer.Stop();
                /* using (TcpClient client = DeviceTCP.TCPclient)
                 {*/
                ModbusIpMaster master = ModbusIpMaster.CreateIp(DeviceTCP.TCPclient);



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

                                //Result printing in logs
                                // ExceptionMessages.Insert(0, DateTime.Now.ToString() + " " + result[0].ToString());
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

                timer.Start();
                // }
            }
            catch (Exception exc)
            {
                //MessageBox.Show(exc.Message);
                DispatchService.Invoke(() =>
                {
                    ExceptionMessages.Insert(0, DateTime.Now.ToString() + ": Requesting stopped : " + exc.Message);
                });


            }
        }

        public async void GetGroupedVariableValues(object source, ElapsedEventArgs e)
        {
            try
            {
                timer.Stop();
                /* using (TcpClient client = DeviceTCP.TCPclient)
                 {*/

                await GetHoldingRegisters();

                if(!timerStop)
                {
                    timer.Start();
                    
                }
                else
                {
                    timerStop = false;
                }
               
                // }
            }
            catch (Exception exc)
            {
                //MessageBox.Show(exc.Message);
                DispatchService.Invoke(() =>
                {
                    ExceptionMessages.Insert(0, DateTime.Now.ToString() + ": Requesting stopped : " + exc.Message);
                });


            }
        }

        private async Task GetHoldingRegisters()
        {

            try
            {
                ModbusIpMaster master = ModbusIpMaster.CreateIp(DeviceTCP.TCPclient);

                List<List<HoldingRegistersVariable>> groupedHR = GroupVariables.GroupHoldingRegisters(HoldingRegisters);

                foreach (List<HoldingRegistersVariable> group in groupedHR)
                {
                    if (group.Count > 0)
                    {
                        //For 2 words variables
                        if (group[0].VariableTypeFormat == "BigEndianFloat" || group[0].VariableTypeFormat == "LittleEndianFloat")
                        {

                            ushort numOfRegs = (UInt16)(group.Count * 2);

                            var result = await master.ReadHoldingRegistersAsync(DeviceTCP.SlaveId, group[0].StartAddress, numOfRegs);
                            if (result.Length > 0)
                            {
                                int currResultIndex = 0;
                                foreach (HoldingRegistersVariable hr in group)
                                {
                                    int hrIndex = HoldingRegisters.IndexOf(hr);
                                    if (result.Length > currResultIndex)   //checking if result is long enough
                                    {
                                        if (hr.VariableTypeFormat == "BigEndianFloat")
                                        {
                                            HoldingRegisters[hrIndex].Value = VariableType.convertToFloatBE(result[currResultIndex], result[currResultIndex + 1]);
                                            HoldingRegisters[hrIndex].ConvertedValue = getCalculatedValue(HoldingRegisters[hrIndex].ConversionFunction, HoldingRegisters[hrIndex].Value);
                                        }
                                        else
                                        {
                                            HoldingRegisters[hrIndex].Value = VariableType.convertToFloatLE(result[currResultIndex], result[currResultIndex + 1]);
                                            HoldingRegisters[hrIndex].ConvertedValue = getCalculatedValue(HoldingRegisters[hrIndex].ConversionFunction, HoldingRegisters[hrIndex].Value);
                                        }
                                    }
                                    else
                                    {
                                        DispatchService.Invoke(() =>
                                        {
                                            //ExceptionMessages.Add(DateTime.Now.ToString() + " " + result[0].ToString());
                                            ExceptionMessages.Insert(0, DateTime.Now.ToString() + "Response was to short" + result[0].ToString());
                                        });
                                    }

                                    HoldingRegisters[hrIndex].Timestamp = DateTime.Now.ToString();

                                    currResultIndex += 2;
                                }

                                /*DispatchService.Invoke(() =>
                                {
                                    //ExceptionMessages.Add(DateTime.Now.ToString() + " " + result[0].ToString());
                                    ExceptionMessages.Insert(0, DateTime.Now.ToString() + " " + result[0].ToString());
                                });*/

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
                        /// FOR 1 word variables
                        else
                        {
                            ushort numOfRegs = (UInt16)(group.Count);

                            var result = await master.ReadHoldingRegistersAsync(DeviceTCP.SlaveId, group[0].StartAddress, numOfRegs);
                            if (result.Length > 0)
                            {
                                int currResultIndex = 0;
                                foreach (HoldingRegistersVariable hr in group)
                                {
                                    int hrIndex = HoldingRegisters.IndexOf(hr);
                                    if (result.Length > currResultIndex)   //checking if result is long enough
                                    {
                                        switch (HoldingRegisters[hrIndex].VariableTypeFormat)
                                        {
                                            case "Decimal":
                                                HoldingRegisters[hrIndex].Value = VariableType.convertToDec(result[currResultIndex]);
                                                HoldingRegisters[hrIndex].ConvertedValue = getCalculatedValue(HoldingRegisters[hrIndex].ConversionFunction, HoldingRegisters[hrIndex].Value);
                                                break;
                                            case "Integer":
                                                HoldingRegisters[hrIndex].Value = VariableType.convertToInt16(result[currResultIndex]);
                                                HoldingRegisters[hrIndex].ConvertedValue = getCalculatedValue(HoldingRegisters[hrIndex].ConversionFunction, HoldingRegisters[hrIndex].Value);
                                                break;
                                            case "Hexadecimal":
                                                HoldingRegisters[hrIndex].Value = VariableType.convertToHex(result[currResultIndex]);
                                                break;
                                            case "Binary":
                                                HoldingRegisters[hrIndex].Value = VariableType.convertToBin(result[currResultIndex]);
                                                break;
                                            default:
                                                HoldingRegisters[hrIndex].Value = result[0].ToString();
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        DispatchService.Invoke(() =>
                                        {
                                            //ExceptionMessages.Add(DateTime.Now.ToString() + " " + result[0].ToString());
                                            ExceptionMessages.Insert(0, DateTime.Now.ToString() + "Response was to short" + result[0].ToString());
                                        });
                                    }

                                    HoldingRegisters[hrIndex].Timestamp = DateTime.Now.ToString();
                                    currResultIndex += 1;
                                }

                                DispatchService.Invoke(() =>
                                {
                                    //ExceptionMessages.Add(DateTime.Now.ToString() + " " + result[0].ToString());
                                    ExceptionMessages.Insert(0, DateTime.Now.ToString() + " " + result[0].ToString());
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
                    ExceptionMessages.Insert(0, DateTime.Now.ToString() + ": Holding registers error : " + exc.Message);
                });


            }

        }


        private void StopPoolingMethod(object obj)
        {
            timerStop = true;
            timer.Stop();

            DispatchService.Invoke(() =>
            {
                //ExceptionMessages.Add(DateTime.Now.ToString() + " Result was null ");
                ExceptionMessages.Insert(0, DateTime.Now.ToString() + " Stopping pooling");
            });
            //master.Dispose();
        }

        private void ClearLogsMethod(object obj)
        {
            ExceptionMessages.Clear();
        }

        public string getCalculatedValue(string expression, string variableValue)
        {
            try
            {
                StringBuilder builder = new StringBuilder(expression);
                builder.Replace("Var", variableValue);
                builder.Replace(",", ".");

                DataTable dt = new DataTable();
                var v = dt.Compute(builder.ToString(), "");
                return v.ToString();
            }
            catch (Exception exc)
            {
                DispatchService.Invoke(() =>
                {
                //ExceptionMessages.Add(DateTime.Now.ToString() + " " + result[0].ToString());
                ExceptionMessages.Insert(0, DateTime.Now.ToString() + "Variable Conversion Error " + exc.Message);
                });

                return "!!" + variableValue;

            }

        }

        private void LoadDevices()
        {
          /*  CaptureDeviceList devices = CaptureDeviceList.Instance;

            foreach (ICaptureDevice dev in devices)
            {
                Interfaces.Add(dev);
            }
*/
        }


        private void Device_OnPacketArrival(object s, PacketCapture e)
        {
            var time = e.Header.Timeval.Date;
            var len = e.Data.Length;

            RawCapture rawPacket = e.GetPacket();

            var packet = PacketDotNet.Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);

            //var arpPacket=packet.Extract<PacketDotNet.ArpPacket>();

            var tcpPacket = packet.Extract<PacketDotNet.TcpPacket>();

            if (tcpPacket != null)
            {
                var ipPacket = (PacketDotNet.IPPacket)tcpPacket.ParentPacket;
                System.Net.IPAddress srcIp = ipPacket.SourceAddress;
                System.Net.IPAddress dstIp = ipPacket.DestinationAddress;
                // int srcPort = tcpPacket.SourcePort;
                // int dstPort = tcpPacket.DestinationPort;

                MyPacket newPacket = new MyPacket(time, srcIp, dstIp, tcpPacket);

                DispatchService.Invoke(() =>
                {
                    Packets.Insert(0, newPacket);
                });
            }

        }

        private void OnStartNetCapture(object obj)
        {
            try
            {
                if (SelectedInterface != null)
                {
                    SelectedInterface.Open();
                    SelectedInterface.OnPacketArrival += Device_OnPacketArrival;
                    SelectedInterface.StartCapture();
                }
                else
                {
                    DispatchService.Invoke(() =>
                    {
                        ExceptionMessages.Add(DateTime.Now.ToString() + ": " + "First, select any capture interface");
                    });
                }

            }
            catch (Exception exc)
            {
                DispatchService.Invoke(() =>
                {
                    ExceptionMessages.Add(DateTime.Now.ToString() + ": " + exc.Message);
                });
            }
        }
        private void OnStopNetCapture(object obj)
        {
            try
            {
                if (SelectedInterface != null)
                {
                    if (SelectedInterface.Started)
                    {
                        SelectedInterface.StopCapture();
                        SelectedInterface.Close();
                    }
                }
            }
            catch (Exception exc)
            {
                DispatchService.Invoke(() =>
                {
                    ExceptionMessages.Add(DateTime.Now.ToString() + ": " + exc.Message);
                });
            }
        }

        private void OnClearPackets(object obj)
        {
            if (_Packets != null)
            {
                Packets.Clear();
            }
        }

        private void OnAddHoldingVar(object obj)
        {
            HoldingRegisters.Add(new HoldingRegistersVariable());
        }

        private void OnAddMultipleHoldingVars(object obj)
        {
            AddMultipleHRDialogViewModel viewModel = new AddMultipleHRDialogViewModel();
            var dialog = new AddMultipleHRwindow(viewModel);


            if (dialog.ShowDialog() == true)
            {

                ushort regStep = 0;
                if (viewModel.VarType == "LittleEndianFloat" || viewModel.VarType == "BigEndianFloat")
                {
                    regStep = 2;
                }
                else
                {
                    regStep = 1;
                }

                int currentStep = viewModel.StartNumber;
                ushort currentReg = viewModel.StartRegNumber;


                for (int i = 0; i < viewModel.Count; i++)
                {
                    string name = viewModel.Prefix + currentStep.ToString() + viewModel.Suffix;
                    currentStep += viewModel.Step;
                    HoldingRegisters.Add(new HoldingRegistersVariable(name, currentReg, viewModel.VarType));
                    currentReg += regStep;
                }

            }
            else
            {

            }
        }
        private void OnDeleteMultipleHoldingVar(object obj)
        {

        }


        private void ClosingDialogHrEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            MessageBox.Show(eventArgs.ToString());
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
