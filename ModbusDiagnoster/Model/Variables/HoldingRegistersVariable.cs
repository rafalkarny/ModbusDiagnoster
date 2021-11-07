using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ModbusDiagnoster.Model.Variables
{
    public class HoldingRegistersVariable : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool Disabled { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ModbusFuncType Type { get; set; }
        public ushort StartAddress { get; set; }
        public string Format { get; set; }
        public int SamplePeriod { get; set; }

        private float _Value { get; set;}
        public float Value 
        { 
            get { 
                return _Value; 
            } 
            set {
                _Value = value;
                OnPropertyChanged(); 
            }
        }

        public float ConvertedValue { get; set; }
        public float LowRange { get; set; }
        public float HighRange { get; set; }
        public string Unit { get; set; }
        public float LowDisplayRange { get; set; }
        public float HighDisplayRange { get; set; }
        public string ConversionFunction { get; set; }
       

        public HoldingRegistersVariable()
        {
            Disabled = false;
            Name = "Nazwa zmiennej";
            Type = ModbusFuncType.HoldingRegisters;
            StartAddress = 0;
            Description = "Opis zmiennej";
            Unit = "Jednostka";
            SamplePeriod = 1;
            LowRange = 0;
            LowDisplayRange = 0;
            HighRange = 0;
            HighDisplayRange = 0;
            Value = -1;
            ConvertedValue = -1;
            Format = "%8.8f";
            ConversionFunction = "Var";


            


        }
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

    }
}
