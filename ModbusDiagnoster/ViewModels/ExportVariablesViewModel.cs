using ModbusDiagnoster.Commands;
using ModbusDiagnoster.Model.Variables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace ModbusDiagnoster.ViewModels
{
    public class ExportVariablesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand SelectDir { get; set; }
        public ICommand ExportCommand { get; set; }


        private string _Filename;
        public string Filename
        {
            get
            {
                return _Filename;
            }
            set
            {
                _Filename = value;
                OnPropertyChanged();
            }
        }

        private string _FilePath;
        public string FilePath
        {
            get
            {
                return _FilePath;
            }
            set
            {
                _FilePath = value;
                OnPropertyChanged();
            }
        }
        private bool _CoilsSelected { get; set; }
        public bool CoilsSelected
        {
            get { return this._CoilsSelected; }
            set
            {
                _CoilsSelected = value;
                OnPropertyChanged();
            }
        }
        private bool _DiSelected { get; set; }
        public bool DiSelected
        {
            get { return this._DiSelected; }
            set
            {
                _DiSelected = value;
                OnPropertyChanged();
            }
        }
        private bool _HrSelected { get; set; }
        public bool HrSelected
        {
            get { return this._HrSelected; }
            set
            {
                _HrSelected = value;
                OnPropertyChanged();
            }
        }
        private bool _IrSelected { get; set; }
        public bool IrSelected
        {
            get { return this._IrSelected; }
            set
            {
                _IrSelected = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<CoilsVariable> _Coils { get; set; }

        private ObservableCollection<DiscreteInputsVariable> _Inputs { get; set; }

        private ObservableCollection<HoldingRegistersVariable> _HoldingRegisters { get; set; }

        private ObservableCollection<InputRegistersVariable> _InputRegisters { get; set; }

        private string _CoilsSaved;
        public string CoilsSaved
        {
            get
            {
                return _CoilsSaved;
            }
            set
            {
                _CoilsSaved = value;
                OnPropertyChanged();
            }
        }
        private string _DiSaved;
        public string DiSaved
        {
            get
            {
                return _DiSaved;
            }
            set
            {
                _DiSaved = value;
                OnPropertyChanged();
            }
        }
        private string _HrSaved;
        public string HrSaved
        {
            get
            {
                return _HrSaved;
            }
            set
            {
                _HrSaved = value;
                OnPropertyChanged();
            }
        }
        private string _IrSaved;
        public string IrSaved
        {
            get
            {
                return _IrSaved;
            }
            set
            {
                _IrSaved = value;
                OnPropertyChanged();
            }
        }
        public ExportVariablesViewModel()
        {
            SelectDir = new RelayCommand(OnSelectDirectory);
            ExportCommand = new RelayCommand(OnExport);

            CoilsSelected = true;
            DiSelected = true;
            HrSelected = true;
            IrSelected = true;
            CoilsSaved = "";
            DiSaved = "";
            HrSaved = "";
            IrSaved = "";

            _Coils = new ObservableCollection<CoilsVariable>();
            _Inputs = new ObservableCollection<DiscreteInputsVariable>();
            _HoldingRegisters = new ObservableCollection<HoldingRegistersVariable>();
            _InputRegisters = new ObservableCollection<InputRegistersVariable>();

        }

        public ExportVariablesViewModel(ObservableCollection<CoilsVariable> coils, ObservableCollection<DiscreteInputsVariable> di, ObservableCollection<HoldingRegistersVariable> hr, ObservableCollection<InputRegistersVariable> ir)
        {
            SelectDir = new RelayCommand(OnSelectDirectory);
            ExportCommand = new RelayCommand(OnExport);

            CoilsSelected = true;
            DiSelected = true;
            HrSelected = true;
            IrSelected = true;
            CoilsSaved = "";
            DiSaved = "";
            HrSaved = "";
            IrSaved = "";

            _Coils = coils;
            _Inputs = di;
            _HoldingRegisters = hr;
            _InputRegisters = ir;

        }

        private void OnSelectDirectory(object obj)
        {
            try
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();

                DialogResult result = dialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
                {
                    FilePath = dialog.SelectedPath;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void OnExport(object obj)
        {

            if (Directory.Exists(FilePath))
            {
                if (CoilsSelected)
                {
                    if (exportToFile(_Coils, "-Coils"))
                    {
                        CoilsSaved = "Ok";
                    }
                }
                if (DiSelected)
                {
                    if (exportToFile(_Inputs, "-Discrete_Inputs"))
                    {
                        DiSaved = "Ok";
                    }
                }
                if (HrSelected)
                {
                    if (exportToFile(_HoldingRegisters, "-Holding_Registers"))
                    {
                        HrSaved = "Ok";
                    }
                }
                if (IrSelected)
                {
                    if (exportToFile(_InputRegisters, "-Input_Registers"))
                    {
                        IrSaved = "Ok";
                    }
                }
            }

            //MessageBox.Show(FilePath + @"\" + Filename + "" + ".csv");

        }

        private bool exportToFile<T>(ObservableCollection<T> collection,string suffix)
        {
            try
            {

                var builder = new StringBuilder();
                var newline = "";

                //headers..
                if (collection != null)
                {
                    if (collection.Count > 0)
                    {

                        foreach (var propertyInfo in collection[0].GetType().GetProperties())
                        {
                            newline += propertyInfo.Name + ";";

                        }
                    }

                }
                builder.AppendLine(newline);
                newline = "";

                //variables

                foreach (var obj in collection)
                {
                    //Console.WriteLine(obj.GetType().ToString());

                    foreach (var propertyInfo in obj.GetType().GetProperties())
                    {

                        newline += propertyInfo.GetValue(obj).ToString() + ";";


                    }
                    builder.AppendLine(newline);
                    newline = "";
                }

                if (collection != null)
                {
                    if (collection.Count > 0)
                    {
                        File.WriteAllText(FilePath + @"\" + Filename +suffix + ".csv", builder.ToString());

                        if(File.Exists(FilePath + @"\" + Filename + suffix + ".csv"))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

        }


        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

    }
}
