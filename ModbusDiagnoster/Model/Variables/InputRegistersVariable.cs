using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ModbusDiagnoster.Model.Variables
{
    public class InputRegistersVariable : INotifyPropertyChanged
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
        private string _VariableTypeFormat { get; set; }
        public string VariableTypeFormat
        {
            get
            {
                return _VariableTypeFormat;
            }
            set
            {
                _VariableTypeFormat = value;
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

        private string _ConvertedValue { get; set; }
        public string ConvertedValue
        {
            get
            {
                return _ConvertedValue;
            }
            set
            {
                _ConvertedValue = value;
                OnPropertyChanged();
            }
        }
        private float _LowRange { get; set; }
        public float LowRange
        {
            get
            {
                return _LowRange;
            }
            set
            {
                _LowRange = value;
                OnPropertyChanged();
            }
        }
        private float _HighRange { get; set; }
        public float HighRange
        {
            get
            {
                return _HighRange;
            }
            set
            {
                _HighRange = value;
                OnPropertyChanged();
            }
        }
        private string _Unit { get; set; }
        public string Unit
        {
            get
            {
                return _Unit;
            }
            set
            {
                _Unit = value;
                OnPropertyChanged();
            }
        }
        private float _LowDisplayRange { get; set; }
        public float LowDisplayRange
        {
            get
            {
                return _LowDisplayRange;
            }
            set
            {
                _LowDisplayRange = value;
                OnPropertyChanged();
            }
        }
        private float _HighDisplayRange { get; set; }
        public float HighDisplayRange
        {
            get
            {
                return _HighDisplayRange;
            }
            set
            {
                _HighDisplayRange = value;
                OnPropertyChanged();
            }
        }
        private string _ConversionFunction { get; set; }
        public string ConversionFunction
        {
            get
            {
                return _ConversionFunction;
            }
            set
            {
                _ConversionFunction = value;
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


        public InputRegistersVariable()
        {
            Checked = false;
            Disabled = false;
            Name = "Variable name";
            Type = ModbusFuncType.InputRegisters;
            StartAddress = 0;
            VariableTypeFormat = "Decimal";
            Description = "Description";
            Unit = "Unit";
            // SamplePeriod = 1;
            LowRange = 0;
            LowDisplayRange = 0;
            HighRange = 0;
            HighDisplayRange = 0;
            Value = "-";
            ConvertedValue = "-";
            //Format = "%8.8f";
            ConversionFunction = "Var";
            Timestamp = "0:00";
            Note = "";

        }

        public InputRegistersVariable(string name = "Name", ushort startAddr = 0, string variableTypeFormat = "Decimal")
        {
            Checked = false;
            Disabled = false;
            Name = name;
            Type = ModbusFuncType.InputRegisters;
            StartAddress = startAddr;
            VariableTypeFormat = variableTypeFormat;
            Description = "Description";
            Unit = "Unit";
            // SamplePeriod = 1;
            LowRange = 0;
            LowDisplayRange = 0;
            HighRange = 0;
            HighDisplayRange = 0;
            Value = "-";
            ConvertedValue = "-";
            //Format = "%8.8f";
            ConversionFunction = "Var";
            Timestamp = "0:00";
            Note = "";

        }


        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

    }
}
