using System.Collections.ObjectModel;
using System.IO;
using ModbusDiagnoster.Model.Variables;
using System.Text.Json;
using ModbusDiagnoster.Model.Communication.ModbusTCP;
using ModbusDiagnoster.Model.Communication.ModbusRTU;

namespace ModbusDiagnoster.FileOperations
{
    public static class SaveVariables
    {
        public static bool SaveHR(ObservableCollection<HoldingRegistersVariable> collection,string deviceDirectory)
        {
            if(Directory.Exists(deviceDirectory))
            {
                string fileName = deviceDirectory + @"\HoldingRegisters.json";
                string jsonString = JsonSerializer.Serialize(collection);
                File.WriteAllText(fileName, jsonString);

                return true;

            }
            return false;
        }

        public static bool SaveIR(ObservableCollection<InputRegistersVariable> collection,string deviceDirectory)
        {
            if (Directory.Exists(deviceDirectory))
            {
                string fileName = deviceDirectory + @"\InputRegisters.json";
                string jsonString = JsonSerializer.Serialize(collection);
                File.WriteAllText(fileName, jsonString);
                return true;

            }
            return false;
        }
        public static bool SaveDI(ObservableCollection<DiscreteInputsVariable> collection, string deviceDirectory)
        {
            if (Directory.Exists(deviceDirectory))
            {
                string fileName = deviceDirectory + @"\DiscreteInputs.json";
                string jsonString = JsonSerializer.Serialize(collection);
                File.WriteAllText(fileName, jsonString);
                return true;

            }
            return false;
        }
        public static bool SaveCoils(ObservableCollection<CoilsVariable> collection, string deviceDirectory)
        {
            if (Directory.Exists(deviceDirectory))
            {
                string fileName = deviceDirectory + @"\Coils.json";
                string jsonString = JsonSerializer.Serialize(collection);
                File.WriteAllText(fileName, jsonString);
                return true;

            }
            return false;
        }
        public static bool SaveTCPparams( ModbusTCP device, string deviceDirectory)
        {
            if (Directory.Exists(deviceDirectory))
            {
                string fileName = deviceDirectory + @"\MbTCP.json";
                string jsonString = JsonSerializer.Serialize(device);
                File.WriteAllText(fileName, jsonString);
                return true;

            }
            return false;
        }
        public static bool SaveRTUparams(ModbusRTU device, string deviceDirectory)
        {
            if (Directory.Exists(deviceDirectory))
            {
                string fileName = deviceDirectory + @"\MbRTU.json";
                string jsonString = JsonSerializer.Serialize(device);
                File.WriteAllText(fileName, jsonString);
                return true;

            }
            return false;
        }

    }
}
