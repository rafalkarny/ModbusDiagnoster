using ModbusDiagnoster.Model.Variables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusDiagnoster.Model.Communication
{

    public enum ModbusType
    {
        Unselected=0,
        RTU=1,
        TCP=2,
        RTUoverTCP=3
    }

    public class Device
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public ModbusType Type { get; set; }
       public byte SlaveId { get; set; }

       
    }
}
