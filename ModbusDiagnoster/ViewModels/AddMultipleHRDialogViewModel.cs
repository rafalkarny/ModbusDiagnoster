using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ModbusDiagnoster.ViewModels
{
    public class AddMultipleHRDialogViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _Count { get; set; }
        public int Count
        {
            get { return this._Count; }
            set
            {
                _Count = value;
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
        private string _VarType { get; set; }

        public string VarType
        {
            get
            {
                return _VarType;
            }
            set
            {
                _VarType = value;
                OnPropertyChanged();
            }
        }
        private ushort _StartRegNumber { get; set; }
        public ushort StartRegNumber
        {
            get { return this._StartRegNumber; }
            set
            {
                _StartRegNumber = value;
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

        public AddMultipleHRDialogViewModel()
        {
            _Count = 2;
            _Prefix = "";
            _Suffix = "";
            _StartNumber = 1;
            _Step = 1;
            _VarType = "Decimal";
            _StartRegNumber = 0;
            
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            //MessageBox.Show("Wywołano zmianę" + propertyName);
        }

    }
}
