using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ModbusDiagnoster.Model.Variables
{
    public class CoilsVariable : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _Checked { get; set; }
        public bool Checked
        {
            get
            {
                return _Checked;
            }
            set
            {
                _Checked = value;
                OnPropertyChanged();
            }
        }

        private bool _Disabled { get; set; }
        public bool Disabled
        {
            get
            {
                return _Disabled;
            }
            set
            {
                _Disabled = value;
                OnPropertyChanged();
            }
        }
        private string _Name { get; set; }
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
                OnPropertyChanged();
            }
        }
        private string _Description { get; set; }
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                _Description = value;
                OnPropertyChanged();
            }
        }
        private ModbusFuncType _Type { get; set; }
        public ModbusFuncType Type
        {
            get
            {
                return _Type;
            }
            set
            {
                _Type = value;
                OnPropertyChanged();
            }
        }
        private ushort _StartAddress { get; set; }
        public ushort StartAddress
        {
            get
            {
                return _StartAddress;
            }
            set
            {
                _StartAddress = value;
                OnPropertyChanged();
            }
        }

        private string _Value { get; set; }
        public string Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
                OnPropertyChanged();
            }
        }

        private string _Timestamp { get; set; }
        public string Timestamp
        {
            get
            {
                return _Timestamp;
            }
            set
            {
                _Timestamp = value;
                OnPropertyChanged();
            }
        }
        private string _Note { get; set; }
        public string Note
        {
            get
            {
                return _Note;
            }
            set
            {
                _Note = value;
                OnPropertyChanged();
            }
        }


        public CoilsVariable()
        {
            Checked = false;
            Disabled = false;
            Name = "Variable name";
            Type = ModbusFuncType.InputRegisters;
            StartAddress = 0;
            Description = "Description";
            Value = "-"; 
            //Format = "%8.8f";
            Timestamp = "0:00";
            Note = "";
            

        }

        public CoilsVariable(string name = "Name", ushort startAddr = 0)
        {
            Checked = false;
            Disabled = false;
            Name = name;
            Type = ModbusFuncType.InputRegisters;
            StartAddress = startAddr;
            Description = "Description";
            Value = "-";
            //Format = "%8.8f";
            Timestamp = "0:00";
            Note = "";

        }



        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
    }
}
