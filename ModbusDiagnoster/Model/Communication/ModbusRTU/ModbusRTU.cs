using ModbusDiagnoster.Model.Communication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusDiagnoster.Model.Communication.ModbusRTU
{

    public class ModbusRTU:Device, INotifyPropertyChanged
    {
        public string Port { get; set; }
        public int Baudrate { get; set; }
        public Parity PortParity { get; set; }
        public StopBits PortStopBits { get; set; }
        public int DataBits { get; set; }
        public int SlaveID { get; set; }
        public bool DTRon { get; set; }
        public bool RTSon { get; set; }
        public bool RTSonTX { get; set; }
        public bool DTRoff { get; set; }
        public bool RTSoff { get; set; }

        public ModbusRTU()
        {
            Port = "";
            Baudrate = 9600;
            DataBits = 8;
            PortParity = Parity.None;
            PortStopBits = StopBits.None;
            SlaveID = 0;
            DTRon = false;
            RTSon = false;
            RTSonTX = false;
            DTRoff = true;
            RTSoff = true;


        }


    }
}
