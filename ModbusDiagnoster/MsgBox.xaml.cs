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
    /// Logika interakcji dla klasy MsgBox.xaml
    /// </summary>
    public partial class MsgBox : Window
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public bool isAnswerNeeded { get; set; }
        public MsgBox()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        public MsgBox(string question)
        {
            InitializeComponent();
            this.DataContext = this;
            isAnswerNeeded = false;
            this.Question = question;
        }

        public MsgBox(string question,bool isAnswer)
        {
            InitializeComponent();
            this.DataContext = this;
            this.Question = question;
            isAnswerNeeded = isAnswer;
            if(!isAnswer)
            {
                answerTxtBox.Visibility = Visibility.Hidden;
            }
            else
            {
                answerTxtBox.Focus();
            }
        }

        private void yesBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void noBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            
            this.Close();
        }
    }
}
