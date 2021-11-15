﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusDiagnoster.Model.Variables
{
    public enum ModbusFuncType
    {
        Unselected=0,
        Coils=1,
        DiscreteInputs=2,
        HoldingRegisters=3,
        InputRegisters=4
       /* CS,
        CSB,
        CSDW,
        CSW,
        IS,
        HR,
        IR,
        HRL,
        HRF,
        HRLM,
        HRFM,
        IRL,
        IRF,
        IRLM,
        IRFM,
        HRD,
        HRI,
        HRDM,
        HRIM*/

    }
    public class Variable:INotifyPropertyChanged
    {
        public bool Disabled { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ModbusFuncType Type {get;set;}
        public ushort StartAddress { get; set; }
        public string Format { get; set; }
        public int SamplePeriod { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;


    }
}