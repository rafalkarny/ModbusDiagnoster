using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModbusDiagnoster.Model.Variables;
using System.Text.Json;
using ModbusDiagnoster.Model.Communication.ModbusTCP;
using ModbusDiagnoster.Model.Communication.ModbusRTU;
using System.Collections.ObjectModel;
using System.IO;

namespace ModbusDiagnoster.FileOperations
{
    public static class LoadVariables
    {
        public static ObservableCollection<HoldingRegistersVariable> LoadHR(string deviceDirectory)
        {
            if (Directory.Exists(deviceDirectory))
            {
                string fileName = deviceDirectory+ @"\HoldingRegisters.json";
                string jsonString = File.ReadAllText(fileName);
                ObservableCollection<HoldingRegistersVariable> holdingRegisters;
                holdingRegisters = JsonSerializer.Deserialize<ObservableCollection<HoldingRegistersVariable>>(jsonString);
                return holdingRegisters;

            }
            return new ObservableCollection<HoldingRegistersVariable>();
        }

        public static ObservableCollection<InputRegistersVariable> LoadIR(string deviceDirectory)
        {
            if (Directory.Exists(deviceDirectory))
            {
                string fileName = deviceDirectory + @"\InputRegisters.json";
                string jsonString = File.ReadAllText(fileName);
                ObservableCollection<InputRegistersVariable> inputRegisters;
                inputRegisters = JsonSerializer.Deserialize<ObservableCollection<InputRegistersVariable>>(jsonString);
                return inputRegisters;

            }
            return new ObservableCollection<InputRegistersVariable>();
        }

        public static ObservableCollection<DiscreteInputsVariable> LoadDI(string deviceDirectory)
        {
            if (Directory.Exists(deviceDirectory))
            {
                string fileName = deviceDirectory + @"\DiscreteInputs.json";
                string jsonString = File.ReadAllText(fileName);
                ObservableCollection<DiscreteInputsVariable> diRegisters;
                diRegisters = JsonSerializer.Deserialize<ObservableCollection<DiscreteInputsVariable>>(jsonString);
                return diRegisters;

            }
            return new ObservableCollection<DiscreteInputsVariable>();
        }
        public static ObservableCollection<CoilsVariable> LoadCoils(string deviceDirectory)
        {
            if (Directory.Exists(deviceDirectory))
            {
                string fileName = deviceDirectory + @"\Coils.json";
                string jsonString = File.ReadAllText(fileName);
                ObservableCollection<CoilsVariable> coilsRegisters;
                coilsRegisters = JsonSerializer.Deserialize<ObservableCollection<CoilsVariable>>(jsonString);
                return coilsRegisters;

            }
            return new ObservableCollection<CoilsVariable>();
        }

        public static ModbusTCP LoadMbTCP(string deviceDirectory)
        {
            if (Directory.Exists(deviceDirectory))
            {
                string fileName = deviceDirectory + @"\MbTCP.json";
                string jsonString = File.ReadAllText(fileName);
                ModbusTCP modbusTCP;
                modbusTCP = JsonSerializer.Deserialize<ModbusTCP>(jsonString);
                return modbusTCP;

            }
            return new ModbusTCP();
        }
        public static ModbusRTU LoadMbRTU(string deviceDirectory)
        {
            if (Directory.Exists(deviceDirectory))
            {
                string fileName = deviceDirectory + @"\MbRTU.json";
                string jsonString = File.ReadAllText(fileName);
                ModbusRTU modbusRTU;
                modbusRTU = JsonSerializer.Deserialize<ModbusRTU>(jsonString);
                return modbusRTU;

            }
            return new ModbusRTU();
        }

    }
}
