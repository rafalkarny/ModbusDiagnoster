﻿using ModbusDiagnoster.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ModbusDiagnoster
{
    /// <summary>
    /// Logika interakcji dla klasy AddVariableWindow.xaml
    /// </summary>
    public partial class AddVariableWindow : Window
    {
        public AddVariableWindow()
        {
            InitializeComponent();
            DataContext = new AddVariableViewModel();
        }
    }
}
