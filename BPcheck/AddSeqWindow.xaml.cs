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

namespace BPcheck
{
    /// <summary>
    /// Interaction logic for AddSeqWindow.xaml
    /// </summary>
    public partial class AddSeqWindow : Window
    {
        public AddSeqWindow()
        {
            InitializeComponent();
            OkButton.Focus();
        }

        private void OK_click(object sender, RoutedEventArgs e)
        {
            PassDataToMainWindow();
        }

        private void Cancel_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OnEnterKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                PassDataToMainWindow();
            }
        }

        private void PassDataToMainWindow()
        {
            var tempWindow = Application.Current.MainWindow as MainWindow; // Getting an instance
            if (SeqName.Text != "" && SeqString.Text != "")
            {
                var tempSeq = BPCheck.SeqParse.Parse(FormatSeq.DelSpaceAndCap(SeqString.Text));
                tempWindow.DnaList.Add(new SsDna(tempSeq) { Name = SeqName.Text });
            }
            else
            {
                tempWindow.Console.Text += "\n" + "Fields cannot be left blank!";
            }
            this.Close();
        }
    }
}
