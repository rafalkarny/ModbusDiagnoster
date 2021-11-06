using ModbusDiagnoster.Model.Communication;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusDiagnoster.Model.Communication.ModbusRTU
{

    public class ModbusRTU:Device
    {
        public string Port { get; set; }
        public int Baudrate { get; set; }
        public Parity PortParity { get; set; }
        public StopBits PortStopBits { get; set; }
        public int SlaveID { get; set; }


    }
}
