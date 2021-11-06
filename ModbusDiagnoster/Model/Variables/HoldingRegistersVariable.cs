using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusDiagnoster.Model.Variables
{
    public class HoldingRegistersVariable:Variable
    {
        public float Value { get; set; }
        public float LowRange { get; set; }
        public float HighRange { get; set; }
        public string Unit { get; set; }
        public float LowDisplayRange { get; set; }
        public float HighDisplayRange { get; set; }

        public HoldingRegistersVariable()
        {
            Enabled = true;
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

            

            
        }

    }
}
