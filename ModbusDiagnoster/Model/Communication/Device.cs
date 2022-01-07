using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ModbusDiagnoster.Model.Communication
{

    public enum ModbusType
    {
        Unselected=0,
        RTU=1,
        TCP=2,
        RTUoverTCP=3
    }

    public class Device:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string _Name { get; set; }
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
        public int _Id { get; set; }
        public int Id
        {
            get
            {
                return _Id;
            }
            set
            {
                _Id = value;
                OnPropertyChanged();
            }
        }
        public ModbusType _Type { get; set; }
        public ModbusType Type
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
        public byte _SlaveId { get; set; }
        public byte SlaveId
        {
            get
            {
                return _SlaveId;
            }
            set
            {
                _SlaveId = value;
                OnPropertyChanged();
            }
        }


        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
    }

  
}
