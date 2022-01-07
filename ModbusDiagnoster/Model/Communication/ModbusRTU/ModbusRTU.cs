using System.ComponentModel;
using System.IO.Ports;


namespace ModbusDiagnoster.Model.Communication.ModbusRTU
{

    public class ModbusRTU:Device, INotifyPropertyChanged
    {
        public string Port
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
        private string _Port { get; set; }
        public int Baudrate
        {
            get
            {
                return _Baudrate;
            }
            set
            {
                _Baudrate = value;
                OnPropertyChanged();
            }
        }

        private int _Baudrate { get; set; }
        public Parity PortParity
        {
            get
            {
                return _PortParity;
            }
            set
            {
                _PortParity = value;
                OnPropertyChanged();
            }
        }
        private Parity _PortParity { get; set; }
        public StopBits PortStopBits
        {
            get
            {
                return _PortStopBits;
            }
            set
            {
                _PortStopBits = value;
                OnPropertyChanged();
            }
        }
        private StopBits _PortStopBits { get; set; }
        public int DataBits
        {
            get
            {
                return _DataBits;
            }
            set
            {
                _DataBits = value;
                OnPropertyChanged();
            }
        }
        private int _DataBits { get; set; }
        public int SlaveID
        {
            get
            {
                return _SlaveID;
            }
            set
            {
                _SlaveID = value;
                OnPropertyChanged();
            }
        }
        private int _SlaveID { get; set; }
        public bool DTRon
        {
            get
            {
                return _DTRon;
            }
            set
            {
                _DTRon = value;
                OnPropertyChanged();
            }
        }
        private bool _DTRon { get; set; }
        public bool RTSon
        {
            get
            {
                return _RTSon;
            }
            set
            {
                _RTSon = value;
                OnPropertyChanged();
            }
        }
        private bool _RTSon { get; set; }
        public bool RTSonTX
        {
            get
            {
                return _RTSonTX;
            }
            set
            {
                _RTSonTX = value;
                OnPropertyChanged();
            }
        }
        private bool _RTSonTX { get; set; }
        public bool DTRoff
        {
            get
            {
                return _DTRoff;
            }
            set
            {
                _DTRoff = value;
                OnPropertyChanged();
            }
        }
        private bool _DTRoff { get; set; }
        public bool RTSoff
        {
            get
            {
                return _RTSoff;
            }
            set
            {
                _RTSoff = value;
                OnPropertyChanged();
            }
        }
        private bool _RTSoff { get; set; }

        public ModbusRTU()
        {
            _Port = "";
            _Baudrate = 9600;
            _DataBits = 8;
            _PortParity = Parity.None;
            _PortStopBits = StopBits.None;
            _SlaveID = 0;
            _DTRon = false;
            _RTSon = false;
            _RTSonTX = false;
            _DTRoff = true;
            _RTSoff = true;


        }


    }
}
