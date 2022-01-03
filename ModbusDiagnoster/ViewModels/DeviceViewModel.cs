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
using System.Windows.Media;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using ModbusDiagnoster.FileOperations;
using System.IO.Ports;
using System.Threading;
using Modbus.Data;
using Modbus.Utility;
using NModbus.Serial;

namespace ModbusDiagnoster.ViewModels
{
    public class DeviceViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand StartPooling { get; set; }
        public ICommand StopPooling { get; set; }
        public ICommand ClearLogs { get; set; }
        public ICommand ClearMon { get; set; }
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
        public ICommand ConnectToDevice { get; set; }
        public ICommand SaveAll { get; set; }
        public ICommand SaveAsCSV { get; set; }

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
        private ObservableCollection<string> _MonitorMessages { get; set; }
        public ObservableCollection<string> MonitorMessages
        {
            get { return this._MonitorMessages; }
            set
            {
                _MonitorMessages = value;
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
                _DeviceTCP = value;
                //ConnectTCP();
                ExceptionMessages.Insert(0, DateTime.Now.ToString() + "Properties changed, disconnecting...");
                DisconnectTCP();
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
                ExceptionMessages.Insert(0, DateTime.Now.ToString() + "Properties changed, disconnecting...");
                // DisconnectSerial();
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
        private TcpClient tcpClient { get; set; }
        private SerialPort serialPortAdapter { get; set; }

        private Brush _TimerStatus;
        public Brush TimerStatus
        {
            get
            {
                return _TimerStatus;
            }

            set
            {
                _TimerStatus = value;
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
                //MessageBox.Show(_statusMessage);
            }
        }
        private int _SamplePeriod { get; set; }
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
        public System.Timers.Timer timer { get; set; }
        public bool timerStop { get; set; }
        private string _Name { get; set; }
        private string _DeviceDirectory { get; set; }
        public string[] AvaibleSerialPorts { get; set; }
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

        public int[] AvaibleBaudRates { get; set; }


        public DeviceViewModel(string name = "Nazwa urządzenia", string dirPath = "", int id = 0)
        {
            _DeviceRTU = new ModbusRTU();
            _DeviceTCP = new ModbusTCP();
            _Coils = new ObservableCollection<CoilsVariable>();
            _Inputs = new ObservableCollection<DiscreteInputsVariable>();
            _HoldingRegisters = new ObservableCollection<HoldingRegistersVariable>();
            _InputRegisters = new ObservableCollection<InputRegistersVariable>();
            _ExceptionMessages = new ObservableCollection<string>();
            _MonitorMessages = new ObservableCollection<string>();
            _Interfaces = new ObservableCollection<ICaptureDevice>();
            _Packets = new ObservableCollection<MyPacket>();
            _TimerStatus = new SolidColorBrush(Color.FromArgb(255, (byte)52, (byte)73, (byte)94)); //rgba(52, 73, 94,1.0)
            ModbusTCPSelected = true;
            _Name = name;
            _DeviceDirectory = dirPath;

            //Filling comboboxes with avaible ports etc..
            AvaibleSerialPorts = SerialPort.GetPortNames();
            AvaibleBaudRates = new int[] { 1200, 2400, 4800, 9600, 19200, 38400, 57600, 115200 };


            //Loading stored device data 
            if (Directory.Exists(dirPath))
            {
                LoadData();
            }



            StartPooling = new AsyncRelayCommand(StartModbusPooling, (ex) => StatusMessage = ex.Message);
            ConnectToDevice = new RelayCommand(Connect);
            StopPooling = new RelayCommand(StopPoolingMethod);
            ClearLogs = new RelayCommand(OnClearLogs);
            ClearMon = new RelayCommand(OnClearMon);
            StartNetCap = new RelayCommand(OnStartNetCapture);
            StopNetCap = new RelayCommand(OnStopNetCapture);
            ClearPackets = new RelayCommand(OnClearPackets);
            AddHoldingVar = new RelayCommand(OnAddHoldingVar);
            AddInputVar = new RelayCommand(OnAddInputVar);
            AddDiscreteVar = new RelayCommand(OnAddInputDiscreteVar);
            AddCoilVar = new RelayCommand(OnAddCoilVar);
            AddMultipleHoldingVar = new RelayCommand(OnAddMultipleHoldingVars);
            AddMultipleInputVar = new RelayCommand(OnAddMultipleInputRegVars);
            AddMultipleDiscreteVar = new RelayCommand(OnAddMultipleInputDiscreteVars);
            AddMultipleCoilVar = new RelayCommand(OnAddMultipleCoilVars);
            DeleteMultipleHoldingVar = new RelayCommand(OnDeleteMultipleHoldingVar);
            SaveAll = new RelayCommand(OnSaveData);
            SaveAsCSV = new RelayCommand(OnSaveAsCSV);

            timer = new System.Timers.Timer();
            // timer.Elapsed += new ElapsedEventHandler(GetVariableValues);
            timer.Elapsed += new ElapsedEventHandler(GetGroupedVariableValues);
            timer.Interval = 1000;
            timer.Enabled = true;
            timer.Stop();
            timerStop = false;



            // _HoldingRegisters.CollectionChanged += ContentCollectionChanged;
        }


        private void Connect(object obj)
        {
            if (ModbusTCPSelected)
            {
                ConnectTCP();   //Prepare tcp session
            }
            if (ModbusRTUSelected)
            {
                ConnectSerial();    //Prepare adapter
            }
        }
        private void ConnectTCP()
        {
            try
            {
                //_DeviceTCP.TCPclient = new TcpClient("127.0.0.1", 502);
                if (tcpClient != null)
                {
                    if (tcpClient.Connected)
                    {
                        tcpClient.Close();
                        tcpClient.Dispose();
                        tcpClient = null;
                        ExceptionMessages.Insert(0, DateTime.Now.ToString() + "Disconnected device");
                    }
                    else
                    {
                        tcpClient = new TcpClient(_DeviceTCP.IPAddr, _DeviceTCP.Port);

                        if (tcpClient.Connected)
                        {
                            ExceptionMessages.Insert(0, DateTime.Now.ToString() + "Successfully connected to device");
                        }
                    }


                }
                else
                {
                    tcpClient = new TcpClient(_DeviceTCP.IPAddr, _DeviceTCP.Port);
                    if (tcpClient.Connected)
                    {
                        ExceptionMessages.Insert(0, DateTime.Now.ToString() + "Successfully connected to device");
                    }
                }



            }
            catch (Exception exc)
            {
                ExceptionMessages.Insert(0, DateTime.Now.ToString() + "Can't connect to Server or Slave, \n Destination port is closed or wrong," +
                    " \n check if device is working and check firewall rules\n: Error desc: \n" + exc.Message);
            }


        }

        private void ConnectSerial()
        {
            try
            {
                if (serialPortAdapter != null)
                {
                    DisconnectSerial();
                }
                else
                {
                    if (DeviceRTU.Port != "")
                    {
                        using (SerialPort port = new SerialPort(DeviceRTU.Port))
                        {
                            // configure serial port
                            port.BaudRate = DeviceRTU.Baudrate;
                            port.DataBits = DeviceRTU.DataBits;
                            port.Parity = DeviceRTU.PortParity;
                            port.StopBits = DeviceRTU.PortStopBits;
                            port.RtsEnable = DeviceRTU.RTSon;
                            port.DtrEnable = DeviceRTU.DTRon;
                            //port.Open();


                            ExceptionMessages.Insert(0, DateTime.Now.ToString() + "COM port is assigned now, creating adapter...");
                            serialPortAdapter = port;

                            //serialPortAdapter.Close();
                            //serialPortAdapter.Dispose();




                            serialPortAdapter.Open();
                            //serialPortAdapter = new SerialPortAdapter(port);
                        }


                    }
                }



            }
            catch (Exception exc)
            {
                ExceptionMessages.Insert(0, DateTime.Now.ToString() + "Cant prepare COM port, please check serial COM port properties" + exc.Message);
            }


        }

        private bool DisconnectTCP()
        {
            try
            {
                timerStop = true;
                //_DeviceTCP.TCPclient = new TcpClient("127.0.0.1", 502);
                if (tcpClient != null)
                {
                    tcpClient.Close();
                    tcpClient.Dispose();

                }

            }
            catch (Exception exc)
            {
                ExceptionMessages.Insert(0, DateTime.Now.ToString() + "Disconnection error" + exc.Message);
            }

            return false;
        }

        private bool DisconnectSerial()
        {
            try
            {
                timerStop = true;
                if (serialPortAdapter != null)
                {
                    serialPortAdapter.Close();
                    serialPortAdapter.Dispose();
                    serialPortAdapter = null;
                    ExceptionMessages.Insert(0, DateTime.Now.ToString() + "COM port is disconnected now.");
                }
            }
            catch (Exception exc)
            {
                ExceptionMessages.Insert(0, DateTime.Now.ToString() + "Disconnection error" + exc.Message);
            }
            return false;
        }

        private void OnTimerStart()
        {
            TimerStatus = new SolidColorBrush(Color.FromArgb(255, (byte)39, (byte)174, (byte)96)); //rgba(39, 174, 96,1.0)
        }
        private void OnTimerStop()
        {
            TimerStatus = new SolidColorBrush(Color.FromArgb(255, (byte)192, (byte)57, (byte)43)); //rgba(192, 57, 43,1.0)
        }
        private void OnTimerWaiting()
        {
            TimerStatus = new SolidColorBrush(Color.FromArgb(255, (byte)241, (byte)196, (byte)15)); //rgba(241, 196, 15,1.0)
        }

        public async Task StartModbusPooling()  //This method only runs timer start
        {
            try
            {
                // if (tcpClient != null || serialPortAdapter!=null)
                // {
                //ModbusIpMaster master = ModbusIpMaster.CreateIp(DeviceTCP.TCPclient);

                timer.Start();
                DispatchService.Invoke(() =>
                {

                    //ExceptionMessages.Add(DateTime.Now.ToString() + " Result was null ");
                    ExceptionMessages.Insert(0, DateTime.Now.ToString() + " Starting pooling");
                    OnTimerStart();
                });
                /* }
                 else
                 {
                     DispatchService.Invoke(() =>
                     {
                         //ExceptionMessages.Add(DateTime.Now.ToString() + " Result was null ");
                         ExceptionMessages.Insert(0, DateTime.Now.ToString() + "Start... but Communication problem, tcp session is not enstablished");
                         //tcpClient = new TcpClient(_DeviceTCP.IPAddr, _DeviceTCP.Port);
                     });
                 }
 */

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

        //  Method which makes DDoS on Device,
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

        public async void GetGroupedVariableValues(object source, ElapsedEventArgs e)   //This method is call on Timer.Tick()
        {
            try
            {
                timer.Stop();
                DispatchService.Invoke(() =>
                {
                    OnTimerWaiting();
                });

                /* using (TcpClient client = DeviceTCP.TCPclient)
                 {*/
                if (tcpClient != null || serialPortAdapter != null)
                {

                    if (ModbusTCPSelected)
                    {
                        if (tcpClient != null)
                        {
                            if (tcpClient.Connected)
                            {
                                /*                            DispatchService.Invoke(() =>
                                                        {
                                                            ExceptionMessages.Insert(0, DateTime.Now.ToString() + "ModbusTCP selected");
                                                        });*/

                                ModbusIpMaster master = ModbusIpMaster.CreateIp(tcpClient);

                                //MAIN Requesting 
                                await GetHoldingRegisters(master, null);
                                await GetInputRegisters(master, null);
                                await GetDiscreteInputs(master,null);
                                await GetCoils(master,null);
                            }
                            else
                            {
                                DispatchService.Invoke(() =>
                                {
                                    ExceptionMessages.Insert(0, DateTime.Now.ToString() + "Client is disconnected, try to connect first.");
                                });
                            }
                        }

                    }
                    else
                    {
                        if (serialPortAdapter != null)
                        {
                            IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(serialPortAdapter);
                            //serialPortAdapter.Open();
                            await GetHoldingRegisters(null, master);
                            await GetInputRegisters(null, master);
                            await GetDiscreteInputs(null,master);
                            await GetCoils(null,master);
                        }


                    }

                }
                else
                {
                    DispatchService.Invoke(() =>
                    {
                        ExceptionMessages.Insert(0, DateTime.Now.ToString() + "Client is disconnected, try to connect first");
                    });
                }



                if (!timerStop)
                {
                    timer.Start();
                    DispatchService.Invoke(() =>
                    {
                        OnTimerStart();
                    });

                }
                else
                {
                    DispatchService.Invoke(() =>
                    {
                        OnTimerStop();
                    });
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

        private async Task GetHoldingRegisters(ModbusIpMaster master, IModbusSerialMaster serialMaster)
        {

            try
            {

                //ModbusIpMaster master = ModbusIpMaster.CreateIp(tcpClient);

               // List<List<HoldingRegistersVariable>> groupedHR = GroupVariables.GroupHoldingRegisters(HoldingRegisters);
                List<List<HoldingRegistersVariable>> groupedHR = GroupVariables.GroupHoldingRegisters(HoldingRegisters);

                foreach (List<HoldingRegistersVariable> group in groupedHR)
                {
                    if (group.Count > 0)
                    {
                        //For 2 words variables
                        if (group[0].VariableTypeFormat == "BigEndianFloat" || group[0].VariableTypeFormat == "LittleEndianFloat")
                        {

                            ushort numOfRegs = (UInt16)(group.Count * 2);

                            if (tcpClient != null || serialPortAdapter != null)
                            {
                                ushort[] result;

                                //result = await master.ReadHoldingRegistersAsync(DeviceTCP.SlaveId, group[0].StartAddress, numOfRegs);

                                if (ModbusTCPSelected)
                                {
                                    AddMonMessage("Sending request TCP: Slave ID: " + DeviceTCP.SlaveId.ToString() + " Reg start addr:" + group[0].StartAddress.ToString() + " Number of registers: " + numOfRegs.ToString());
                                    var res = await master.ReadHoldingRegistersAsync(DeviceTCP.SlaveId, group[0].StartAddress, numOfRegs);
                                    result = res;
                                }
                                else if (ModbusRTUSelected)
                                {
                                    AddMonMessage("Sending request RTU: Slave ID: " + DeviceTCP.SlaveId.ToString() + " Reg start addr:" + group[0].StartAddress.ToString() + " Number of registers: " + numOfRegs.ToString());

                                    if (serialPortAdapter.IsOpen)
                                    {
                                        var res = await serialMaster.ReadHoldingRegistersAsync(DeviceTCP.SlaveId, group[0].StartAddress, numOfRegs);
                                        result = res;
                                    }
                                    else
                                    {
                                        serialPortAdapter.Open();
                                        var res = await serialMaster.ReadHoldingRegistersAsync(DeviceTCP.SlaveId, group[0].StartAddress, numOfRegs);
                                        result = res;
                                    }
                                }
                                else
                                {
                                    result = new ushort[0];
                                }

                                if (result.Length > 0)
                                {
                                    AddMonMessage("Response received: Len: " + result.Length.ToString());
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
                                                ExceptionMessages.Insert(0, DateTime.Now.ToString() + "Response was to short" + Convert.ToString(result));
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
                                /*}
                                else
                                {
                                    DispatchService.Invoke(() =>
                                    {
                                        //ExceptionMessages.Add(DateTime.Now.ToString() + " " + result[0].ToString());
                                        ExceptionMessages.Insert(0, DateTime.Now.ToString() + "Client is disconnected");
                                    });
                                }*/

                            }
                            else
                            {
                                DispatchService.Invoke(() =>
                                {
                                    //ExceptionMessages.Add(DateTime.Now.ToString() + " " + result[0].ToString());
                                    ExceptionMessages.Insert(0, DateTime.Now.ToString() + "Client is disconnected");
                                });
                            }
                        }
                        /// FOR 1 word variables
                        else
                        {
                            ushort numOfRegs = (UInt16)(group.Count);

                            if (tcpClient != null || serialPortAdapter != null)
                            {


                                //var result = await master.ReadHoldingRegistersAsync(DeviceTCP.SlaveId, group[0].StartAddress, numOfRegs);

                                ushort[] result;

                                if (ModbusTCPSelected)
                                {
                                    AddMonMessage("Sending request TCP: Slave ID: " + DeviceTCP.SlaveId.ToString() + " Reg start addr:" + group[0].StartAddress.ToString() + " Number of registers: " + numOfRegs.ToString());
                                    var res = await master.ReadHoldingRegistersAsync(DeviceTCP.SlaveId, group[0].StartAddress, numOfRegs);
                                    result = res;
                                }
                                else if (ModbusRTUSelected)
                                {
                                    AddMonMessage("Sending request RTU: Slave ID: " + DeviceTCP.SlaveId.ToString() + " Reg start addr:" + group[0].StartAddress.ToString() + " Number of registers: " + numOfRegs.ToString());
                                    if (serialPortAdapter.IsOpen)
                                    {
                                        var res = await serialMaster.ReadHoldingRegistersAsync(DeviceTCP.SlaveId, group[0].StartAddress, numOfRegs);
                                        result = res;
                                    }
                                    else
                                    {
                                        serialPortAdapter.Open();
                                        var res = await serialMaster.ReadHoldingRegistersAsync(DeviceTCP.SlaveId, group[0].StartAddress, numOfRegs);
                                        result = res;
                                    }
                                }
                                else
                                {
                                    result = new ushort[0];
                                }

                                if (result.Length > 0)
                                {
                                    AddMonMessage("Response received: Len: " + result.Length.ToString());
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

                                        //TODO:: Add feature to user can enable advanced diagnostics
                                        //ExceptionMessages.Insert(0, DateTime.Now.ToString() + " " + result[0].ToString());
                                    });
                                }


                            }
                            else
                            {
                                DispatchService.Invoke(() =>
                                {
                                    //ExceptionMessages.Add(DateTime.Now.ToString() + " " + result[0].ToString());
                                    ExceptionMessages.Insert(0, DateTime.Now.ToString() + "Client is disconnected");
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

        private async Task GetInputRegisters(ModbusIpMaster master, IModbusSerialMaster serialMaster)
        {

            try
            {

                //ModbusIpMaster master = ModbusIpMaster.CreateIp(tcpClient);

                List<List<InputRegistersVariable>> groupedHR = GroupVariables.GroupInputRegisters(InputRegisters);

                foreach (List<InputRegistersVariable> group in groupedHR)
                {
                    if (group.Count > 0)
                    {
                        //For 2 words variables
                        if (group[0].VariableTypeFormat == "BigEndianFloat" || group[0].VariableTypeFormat == "LittleEndianFloat")
                        {

                            ushort numOfRegs = (UInt16)(group.Count * 2);

                            if (tcpClient != null || serialPortAdapter != null)
                            {
                                ushort[] result;

                                //result = await master.ReadHoldingRegistersAsync(DeviceTCP.SlaveId, group[0].StartAddress, numOfRegs);

                                if (ModbusTCPSelected)
                                {
                                    AddMonMessage("Sending request TCP: Slave ID: " + DeviceTCP.SlaveId.ToString() + " Reg start addr:" + group[0].StartAddress.ToString() + " Number of registers: " + numOfRegs.ToString());
                                    var res = await master.ReadInputRegistersAsync(DeviceTCP.SlaveId, group[0].StartAddress, numOfRegs);
                                    result = res;
                                }
                                else if (ModbusRTUSelected)
                                {
                                    AddMonMessage("Sending request RTU: Slave ID: " + DeviceTCP.SlaveId.ToString() + " Reg start addr:" + group[0].StartAddress.ToString() + " Number of registers: " + numOfRegs.ToString());

                                    if (serialPortAdapter.IsOpen)
                                    {
                                        var res = await serialMaster.ReadInputRegistersAsync(DeviceTCP.SlaveId, group[0].StartAddress, numOfRegs);
                                        result = res;
                                    }
                                    else
                                    {
                                        serialPortAdapter.Open();
                                        var res = await serialMaster.ReadInputRegistersAsync(DeviceTCP.SlaveId, group[0].StartAddress, numOfRegs);
                                        result = res;
                                    }
                                }
                                else
                                {
                                    result = new ushort[0];
                                }

                                if (result.Length > 0)
                                {
                                    AddMonMessage("Response received: Len: " + result.Length.ToString());
                                    int currResultIndex = 0;
                                    foreach (InputRegistersVariable ir in group)
                                    {
                                        int irIndex = InputRegisters.IndexOf(ir);
                                        if (result.Length > currResultIndex)   //checking if result is long enough
                                        {
                                            if (ir.VariableTypeFormat == "BigEndianFloat")
                                            {
                                                InputRegisters[irIndex].Value = VariableType.convertToFloatBE(result[currResultIndex], result[currResultIndex + 1]);
                                                InputRegisters[irIndex].ConvertedValue = getCalculatedValue(InputRegisters[irIndex].ConversionFunction, InputRegisters[irIndex].Value);
                                            }
                                            else
                                            {
                                                InputRegisters[irIndex].Value = VariableType.convertToFloatLE(result[currResultIndex], result[currResultIndex + 1]);
                                                InputRegisters[irIndex].ConvertedValue = getCalculatedValue(InputRegisters[irIndex].ConversionFunction, InputRegisters[irIndex].Value);
                                            }
                                        }
                                        else
                                        {
                                            DispatchService.Invoke(() =>
                                            {
                                                //ExceptionMessages.Add(DateTime.Now.ToString() + " " + result[0].ToString());
                                                ExceptionMessages.Insert(0, DateTime.Now.ToString() + "Response was to short" + Convert.ToString(result));
                                            });
                                        }

                                        InputRegisters[irIndex].Timestamp = DateTime.Now.ToString();

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
                                /*}
                                else
                                {
                                    DispatchService.Invoke(() =>
                                    {
                                        //ExceptionMessages.Add(DateTime.Now.ToString() + " " + result[0].ToString());
                                        ExceptionMessages.Insert(0, DateTime.Now.ToString() + "Client is disconnected");
                                    });
                                }*/

                            }
                            else
                            {
                                DispatchService.Invoke(() =>
                                {
                                    //ExceptionMessages.Add(DateTime.Now.ToString() + " " + result[0].ToString());
                                    ExceptionMessages.Insert(0, DateTime.Now.ToString() + "Client is disconnected");
                                });
                            }
                        }
                        /// FOR 1 word variables
                        else
                        {
                            ushort numOfRegs = (UInt16)(group.Count);

                            if (tcpClient != null || serialPortAdapter != null)
                            {

                                ushort[] result;

                                if (ModbusTCPSelected)
                                {
                                    AddMonMessage("Sending request TCP: Slave ID: " + DeviceTCP.SlaveId.ToString() + " Reg start addr:" + group[0].StartAddress.ToString() + " Number of registers: " + numOfRegs.ToString());
                                    var res = await master.ReadInputRegistersAsync(DeviceTCP.SlaveId, group[0].StartAddress, numOfRegs);
                                    result = res;
                                }
                                else if (ModbusRTUSelected)
                                {
                                    AddMonMessage("Sending request RTU: Slave ID: " + DeviceTCP.SlaveId.ToString() + " Reg start addr:" + group[0].StartAddress.ToString() + " Number of registers: " + numOfRegs.ToString());
                                    if (serialPortAdapter.IsOpen)
                                    {
                                        var res = await serialMaster.ReadInputRegistersAsync(DeviceTCP.SlaveId, group[0].StartAddress, numOfRegs);
                                        result = res;
                                    }
                                    else
                                    {
                                        serialPortAdapter.Open();
                                        var res = await serialMaster.ReadInputRegistersAsync(DeviceTCP.SlaveId, group[0].StartAddress, numOfRegs);
                                        result = res;
                                    }
                                }
                                else
                                {
                                    result = new ushort[0];
                                }

                                if (result.Length > 0)
                                {
                                    AddMonMessage("Response received: Len: " + result.Length.ToString());
                                    int currResultIndex = 0;
                                    foreach (InputRegistersVariable ir in group)
                                    {
                                        int irIndex = InputRegisters.IndexOf(ir);
                                        if (result.Length > currResultIndex)   //checking if result is long enough
                                        {
                                            switch (InputRegisters[irIndex].VariableTypeFormat)
                                            {
                                                case "Decimal":
                                                    InputRegisters[irIndex].Value = VariableType.convertToDec(result[currResultIndex]);
                                                    InputRegisters[irIndex].ConvertedValue = getCalculatedValue(InputRegisters[irIndex].ConversionFunction, InputRegisters[irIndex].Value);
                                                    break;
                                                case "Integer":
                                                    InputRegisters[irIndex].Value = VariableType.convertToInt16(result[currResultIndex]);
                                                    InputRegisters[irIndex].ConvertedValue = getCalculatedValue(InputRegisters[irIndex].ConversionFunction, InputRegisters[irIndex].Value);
                                                    break;
                                                case "Hexadecimal":
                                                    InputRegisters[irIndex].Value = VariableType.convertToHex(result[currResultIndex]);
                                                    break;
                                                case "Binary":
                                                    InputRegisters[irIndex].Value = VariableType.convertToBin(result[currResultIndex]);
                                                    break;
                                                default:
                                                    InputRegisters[irIndex].Value = result[0].ToString();
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

                                        InputRegisters[irIndex].Timestamp = DateTime.Now.ToString();
                                        currResultIndex += 1;
                                    }

                                    DispatchService.Invoke(() =>
                                    {
                                        //ExceptionMessages.Add(DateTime.Now.ToString() + " " + result[0].ToString());

                                        //TODO:: Add feature to user can enable advanced diagnostics
                                        //ExceptionMessages.Insert(0, DateTime.Now.ToString() + " " + result[0].ToString());
                                    });
                                }


                            }
                            else
                            {
                                DispatchService.Invoke(() =>
                                {
                                    //ExceptionMessages.Add(DateTime.Now.ToString() + " " + result[0].ToString());
                                    ExceptionMessages.Insert(0, DateTime.Now.ToString() + "Client is disconnected");
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

        private async Task GetDiscreteInputs(ModbusIpMaster master, IModbusMaster serialMaster)
        {

            try
            {
                List<List<DiscreteInputsVariable>> groupedDI = GroupVariables.GroupDiscreteInputs(Inputs);

                foreach (List<DiscreteInputsVariable> group in groupedDI)
                {
                    if (group.Count > 0)
                    {
                        ushort numOfRegs = (UInt16)(group.Count);

                        if (tcpClient != null || serialPortAdapter != null)
                        {
                            //SENDING REQUEST

                            bool[] result;

                            if (ModbusTCPSelected)
                            {
                                AddMonMessage("Sending request TCP: Slave ID: " + DeviceTCP.SlaveId.ToString() + " Reg start addr:" + group[0].StartAddress.ToString() + " Number of registers: " + numOfRegs.ToString());
                                var res = await master.ReadInputsAsync(DeviceTCP.SlaveId, group[0].StartAddress, numOfRegs);
                                result = res;
                            }
                            else if (ModbusRTUSelected)
                            {
                                AddMonMessage("Sending request RTU: Slave ID: " + DeviceTCP.SlaveId.ToString() + " Reg start addr:" + group[0].StartAddress.ToString() + " Number of registers: " + numOfRegs.ToString());
                                if (serialPortAdapter.IsOpen)
                                {
                                    var res = await serialMaster.ReadInputsAsync(DeviceTCP.SlaveId, group[0].StartAddress, numOfRegs);
                                    result = res;
                                }
                                else
                                {
                                    serialPortAdapter.Open();
                                    var res = await serialMaster.ReadInputsAsync(DeviceTCP.SlaveId, group[0].StartAddress, numOfRegs);
                                    result = res;
                                }
                            }
                            else
                            {
                                result = new bool[0];
                            }

                            //IF Device returned value:

                            if (result.Length > 0)
                            {
                                AddMonMessage("Response received: Len: " + result.Length.ToString());
                                int currResultIndex = 0;
                                foreach (DiscreteInputsVariable di in group)
                                {
                                    int irIndex = Inputs.IndexOf(di);
                                    if (result.Length > currResultIndex)   //checking if result is long enough
                                    {
                                        switch (result[currResultIndex])
                                        {
                                            case true:
                                                Inputs[irIndex].Value = "1";
                                                break;
                                            case false:
                                                Inputs[irIndex].Value = "0";
                                                break;
                                            default:
                                                Inputs[irIndex].Value = "-1";
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

                                    Inputs[irIndex].Timestamp = DateTime.Now.ToString();
                                    currResultIndex += 1;
                                }

                                DispatchService.Invoke(() =>
                                {
                                    //ExceptionMessages.Add(DateTime.Now.ToString() + " " + result[0].ToString());

                                    //TODO:: Add feature to user can enable advanced diagnostics
                                    //ExceptionMessages.Insert(0, DateTime.Now.ToString() + " " + result[0].ToString());
                                });
                            }
                            else
                            {
                                AddMonMessage("Result was to short: " + result.Length.ToString());
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
                    ExceptionMessages.Insert(0, DateTime.Now.ToString() + ": Inputs error : " + exc.Message);
                });


            }

        }
        private async Task GetCoils(ModbusIpMaster master,IModbusMaster serialMaster)
        {

            try
            {
                List<List<CoilsVariable>> groupedCoils = GroupVariables.GroupCoils(Coils);

                foreach (List<CoilsVariable> group in groupedCoils)
                {
                    if (group.Count > 0)
                    {
                        ushort numOfRegs = (UInt16)(group.Count);

                        if (tcpClient != null || serialPortAdapter != null)
                        {
                            //SENDING REQUEST

                            bool[] result;

                            if (ModbusTCPSelected)
                            {
                                AddMonMessage("Sending request TCP: Slave ID: " + DeviceTCP.SlaveId.ToString() + " Reg start addr:" + group[0].StartAddress.ToString() + " Number of registers: " + numOfRegs.ToString());
                                var res = await master.ReadCoilsAsync(DeviceTCP.SlaveId, group[0].StartAddress, numOfRegs);
                                result = res;
                            }
                            else if (ModbusRTUSelected)
                            {
                                AddMonMessage("Sending request RTU: Slave ID: " + DeviceTCP.SlaveId.ToString() + " Reg start addr:" + group[0].StartAddress.ToString() + " Number of registers: " + numOfRegs.ToString());
                                if (serialPortAdapter.IsOpen)
                                {
                                    var res = await serialMaster.ReadCoilsAsync(DeviceTCP.SlaveId, group[0].StartAddress, numOfRegs);
                                    result = res;
                                }
                                else
                                {
                                    serialPortAdapter.Open();
                                    var res = await serialMaster.ReadCoilsAsync(DeviceTCP.SlaveId, group[0].StartAddress, numOfRegs);
                                    result = res;
                                }
                            }
                            else
                            {
                                result = new bool[0];
                            }

                            //IF Device returned value:

                            if (result.Length > 0)
                            {
                                AddMonMessage("Coils Response received: Len: " + result.Length.ToString());
                                int currResultIndex = 0;
                                foreach (CoilsVariable coil in group)
                                {
                                    int coilIndex = Coils.IndexOf(coil);
                                    if (result.Length > currResultIndex)   //checking if result is long enough
                                    {
                                        switch (result[currResultIndex])
                                        {
                                            case true:
                                                Coils[coilIndex].Value = "1";
                                                break;
                                            case false:
                                                Coils[coilIndex].Value = "0";
                                                break;
                                            default:
                                                Coils[coilIndex].Value = "-1";
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

                                    Coils[coilIndex].Timestamp = DateTime.Now.ToString();
                                    currResultIndex += 1;
                                }

                                DispatchService.Invoke(() =>
                                {
                                    //ExceptionMessages.Add(DateTime.Now.ToString() + " " + result[0].ToString());

                                    //TODO:: Add feature to user can enable advanced diagnostics
                                    //ExceptionMessages.Insert(0, DateTime.Now.ToString() + " " + result[0].ToString());
                                });
                            }
                            else
                            {
                                AddMonMessage("Result was to short: " + result.Length.ToString());
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
                    ExceptionMessages.Insert(0, DateTime.Now.ToString() + ": Coils error : " + exc.Message);
                });


            }

        }

        private void StopPoolingMethod(object obj)
        {
            timerStop = true;
            //timer.Stop();

            DispatchService.Invoke(() =>
            {

                //ExceptionMessages.Add(DateTime.Now.ToString() + " Result was null ");
                ExceptionMessages.Insert(0, DateTime.Now.ToString() + " Stopping pooling");
                OnTimerStop();
            });
            //master.Dispose();
        }

        private void OnClearLogs(object obj)
        {
            ExceptionMessages.Clear();
        }
        private void OnClearMon(object obj)
        {
            MonitorMessages.Clear();
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

        private void OnAddInputVar(object obj)
        {
            InputRegisters.Add(new InputRegistersVariable());
        }
        private void OnAddMultipleInputRegVars(object obj)
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
                    InputRegisters.Add(new InputRegistersVariable(name, currentReg, viewModel.VarType));
                    currentReg += regStep;
                }

            }
            else
            {

            }
        }

        private void OnAddInputDiscreteVar(object obj)
        {
            Inputs.Add(new DiscreteInputsVariable());
        }

        private void OnAddMultipleInputDiscreteVars(object obj)
        {
            AddMultipleDiscreteCoilsViewModel viewModel = new AddMultipleDiscreteCoilsViewModel();
            var dialog = new AddMultipleDiscreteCoils(viewModel);


            if (dialog.ShowDialog() == true)
            {

                ushort regStep = 1;

                int currentStep = viewModel.StartNumber;
                ushort currentReg = viewModel.StartRegNumber;


                for (int i = 0; i < viewModel.Count; i++)
                {
                    string name = viewModel.Prefix + currentStep.ToString() + viewModel.Suffix;
                    currentStep += viewModel.Step;
                    Inputs.Add(new DiscreteInputsVariable(name, currentReg));
                    currentReg += regStep;
                }

            }
            else
            {

            }
        }

        private void OnAddCoilVar(object obj)
        {
            Coils.Add(new CoilsVariable());
        }
        private void OnAddMultipleCoilVars(object obj)
        {
            AddMultipleDiscreteCoilsViewModel viewModel = new AddMultipleDiscreteCoilsViewModel();
            var dialog = new AddMultipleDiscreteCoils(viewModel);


            if (dialog.ShowDialog() == true)
            {

                ushort regStep = 1;

                int currentStep = viewModel.StartNumber;
                ushort currentReg = viewModel.StartRegNumber;


                for (int i = 0; i < viewModel.Count; i++)
                {
                    string name = viewModel.Prefix + currentStep.ToString() + viewModel.Suffix;
                    currentStep += viewModel.Step;
                    Coils.Add(new CoilsVariable(name, currentReg));
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

        private void AddMonMessage(string message = "")
        {
            DispatchService.Invoke(() =>
            {
                MonitorMessages.Insert(0, DateTime.Now.ToString() + ": " + message);
            });
        }

        private void OnSaveData(object obj)
        {
            if (Directory.Exists(_DeviceDirectory))
            {
                try
                {
                    SaveVariables.SaveCoils(Coils, _DeviceDirectory);
                    SaveVariables.SaveDI(Inputs, _DeviceDirectory);
                    SaveVariables.SaveIR(InputRegisters, _DeviceDirectory);
                    SaveVariables.SaveHR(HoldingRegisters, _DeviceDirectory);
                    SaveVariables.SaveTCPparams(DeviceTCP, _DeviceDirectory);
                    SaveVariables.SaveRTUparams(DeviceRTU, _DeviceDirectory);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }


            }

        }

        private void LoadData()
        {
            try
            {
                HoldingRegisters = LoadVariables.LoadHR(_DeviceDirectory);
                InputRegisters = LoadVariables.LoadIR(_DeviceDirectory);
                Inputs = LoadVariables.LoadDI(_DeviceDirectory);
                Coils = LoadVariables.LoadCoils(_DeviceDirectory);
                DeviceTCP = LoadVariables.LoadMbTCP(_DeviceDirectory);
                DeviceRTU = LoadVariables.LoadMbRTU(_DeviceDirectory);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            //MessageBox.Show("Wywołano zmianę" + propertyName);
        }

        private void OnSaveAsCSV(object obj)
        {
            ExportVariablesViewModel exportVariablesViewModel=new ExportVariablesViewModel(_Coils,_Inputs,_HoldingRegisters,_InputRegisters);
            ExportWindow exportWindow=new ExportWindow(exportVariablesViewModel);
            exportWindow.Show();

            //ExportAs.SaveAsCSV(HoldingRegisters);
        }

    }
}
