using System;
using System.ComponentModel;
using System.Net.Sockets;

namespace ModbusDiagnoster.Model.Communication.ModbusTCP
{
    public class ModbusTCP:Device, INotifyPropertyChanged
    {
        
        private string _IPAddr { get; set; }
        public string IPAddr
        {
            get
            {
                return _IPAddr;
            }
            set
            {
                _IPAddr = value;
                OnPropertyChanged();
            }
        }
        private int _Port { get; set; }
        public int Port
        {
            get
            {
                return _Port;
            }
            set
            {
                _Port = value;
                OnPropertyChanged();
            }
        }
        private TcpClient _TCPclient { get; set; }  //To REMOVE
        public TcpClient TCPclient
        {
            get
            {
                return _TCPclient;
            }
            set
            {
                _TCPclient = value;
                OnPropertyChanged();
            }
        }   //TO REMOVE

        public ModbusTCP() ////
        {
           
            Id = 1;
            Name = "name";
            IPAddr = "127.0.0.1";
            Port = 502;
            Type = ModbusType.TCP;
            SlaveId = 1;
            try
            {

             //   TCPclient = new TcpClient(IPAddr, Port);
            }
            catch(Exception exc)
            {

            }
           /* Coils = new ObservableCollection<CoilsVariable>();
            Inputs = new ObservableCollection<DiscreteInputsVariable>();
            HoldingRegisters = new ObservableCollection<HoldingRegistersVariable>();
            InputRegisters = new ObservableCollection<InputRegistersVariable>();*/

        }

        public ModbusTCP(int id=1,string name= "Device Name",string ipaddr= "127.0.0.1",int port=502,ModbusType type=ModbusType.TCP)
        {
            Id = id;
            Name = name;
            IPAddr = ipaddr;
            Port = port;
            Type = type;
            SlaveId = 1;
            //TCPclient = new TcpClient(IPAddr, Port);

            /* Coils = new ObservableCollection<CoilsVariable>();
             Inputs = new ObservableCollection<DiscreteInputsVariable>();
             HoldingRegisters = new ObservableCollection<HoldingRegistersVariable>();
             InputRegisters = new ObservableCollection<InputRegistersVariable>();*/

        }

   
        /* public ModbusTCP(int id, string name, string ipaddr, int port, ModbusType type
             , ObservableCollection<CoilsVariable> coils, ObservableCollection<DiscreteInputsVariable> inputs, ObservableCollection<HoldingRegistersVariable> holdingRegisters,
             ObservableCollection<InputRegistersVariable> inputRegisters)
         {
             Id = id;
             Name = name;
             IPAddr = ipaddr;
             Port = port;
             Type = type;
             Coils = coils;
             Inputs = inputs;
             HoldingRegisters = holdingRegisters;
             InputRegisters = inputRegisters;

         }*/
    }
}
