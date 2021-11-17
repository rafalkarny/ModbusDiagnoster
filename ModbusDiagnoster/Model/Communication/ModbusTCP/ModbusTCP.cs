using ModbusDiagnoster.Model.Communication;
using ModbusDiagnoster.Model.Variables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ModbusDiagnoster.Model.Communication.ModbusTCP
{
    public class ModbusTCP:Device
    {
        public string IPAddr { get; set; }
        public int Port { get; set; }

        public ModbusTCP() ////
        {
            Id = 1;
            Name = "Nazwa urządzenia";
            IPAddr = "127.0.0.1";
            Port = 502;
            Type = ModbusType.TCP;
            SlaveId = 1;
           /* Coils = new ObservableCollection<CoilsVariable>();
            Inputs = new ObservableCollection<DiscreteInputsVariable>();
            HoldingRegisters = new ObservableCollection<HoldingRegistersVariable>();
            InputRegisters = new ObservableCollection<InputRegistersVariable>();*/

        }

        public ModbusTCP(int id=1,string name= "Nazwa urządzenia",string ipaddr= "127.0.0.1",int port=502,ModbusType type=ModbusType.TCP)
        {
            Id = id;
            Name = name;
            IPAddr = ipaddr;
            Port = port;
            Type = type;
            SlaveId = 1;
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
