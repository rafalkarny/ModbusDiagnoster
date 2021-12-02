using ModbusDiagnoster.Model.Variables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ModbusDiagnoster.ViewModels
{

    public class AddVariableViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<CoilsVariable> _Coils { get; set; }
        public ObservableCollection<CoilsVariable> Coils
        {
            get { return this._Coils; }
            set
            {
                _Coils = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<DiscreteInputsVariable> _Inputs { get; set; }
        public ObservableCollection<DiscreteInputsVariable> Inputs
        {
            get { return this._Inputs; }
            set
            {
                _Inputs = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<HoldingRegistersVariable> _HoldingRegisters { get; set; }
        public ObservableCollection<HoldingRegistersVariable> HoldingRegisters
        {
            get { return this._HoldingRegisters; }
            set
            {
                _HoldingRegisters = value;
                OnPropertyChanged();

            }
        }
        private ObservableCollection<InputRegistersVariable> _InputRegisters { get; set; }
        public ObservableCollection<InputRegistersVariable> InputRegisters
        {
            get { return this._InputRegisters; }
            set
            {
                _InputRegisters = value;
                OnPropertyChanged();
            }
        }
        private string _Prefix { get; set; }
        public string Prefix
        {
            get { return this._Prefix; }
            set
            {
                _Prefix = value;
                OnPropertyChanged();
            }
        }
        private string _Suffix { get; set; }
        public string Suffix
        {
            get { return this._Suffix; }
            set
            {
                _Suffix = value;
                OnPropertyChanged();
            }
        }
        private int _StartNumber { get; set; }
        public int StartNumber
        {
            get { return this._StartNumber; }
            set
            {
                _StartNumber = value;
                OnPropertyChanged();
            }
        }
        private int _Step { get; set; }
        public int Step
        {
            get { return this._Step; }
            set
            {
                _Step = value;
                OnPropertyChanged();
            }
        }
        private string _Name { get; set; }
        public string Name
        {
            get { return this._Name; }
            set
            {
                _Name = value;
                OnPropertyChanged();
            }
        }
        private string _RegType { get; set; }
        public string RegType
        {
            get { return this._RegType; }
            set
            {
                _RegType = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<string> VarTypes => new[] {
        "Decimal",
        "Integer",
        "Hexadecimal",
        "Binary",
        "BigEndianFloat",
        "LittleEndianFloat" };
        public IEnumerable<string> RegTypes => new[] {
        "Coils",
        "Discrete Input",
        "Holding Registers",
        "Input Registers"
        };
        public AddVariableViewModel()
        {
            _Coils = new ObservableCollection<CoilsVariable>();
            _Inputs = new ObservableCollection<DiscreteInputsVariable>();
            _HoldingRegisters = new ObservableCollection<HoldingRegistersVariable>();
            _InputRegisters = new ObservableCollection<InputRegistersVariable>();
            _Prefix = "";
            _Suffix = "";
            _StartNumber = 1;
            _Step = 1;
            _Name = "";

        }



        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            //MessageBox.Show("Wywołano zmianę" + propertyName);
        }
    }
}
