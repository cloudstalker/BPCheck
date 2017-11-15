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
    /// Interaction logic for CutWindow.xaml
    /// </summary>
    public partial class CutWindow : Window
    {
        /// <summary>
        /// The subwindow to cut a strand at a specific place
        /// </summary>
        public CutWindow()
        {
            InitializeComponent();
            OkButton.Focus();
        }

        private void OK_click(object sender, RoutedEventArgs e)
        {
            PassDataToMainWindow();
        }

        private void LeftPick_Click(object sender, RoutedEventArgs e)
        {
            RightPick.IsChecked = false;
            BetweenPick.IsChecked = false;
            CutNum2.IsEnabled = false;
        }

        private void RightPick_Click(object sender, RoutedEventArgs e)
        {
            LeftPick.IsChecked = false;
            BetweenPick.IsChecked = false;
            CutNum2.IsEnabled = false;
        }

        private void InsidePick_Click(object sender, RoutedEventArgs e)
        {
            LeftPick.IsChecked = false;
            RightPick.IsChecked = false;
            CutNum2.IsEnabled = true;
        }

        private void Cancel_click(object sender, RoutedEventArgs e) => this.Close();

        private void OnEnterKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                PassDataToMainWindow();
            }
        }

        private void PassDataToMainWindow()
        {
            var tempWindow = Application.Current.MainWindow as MainWindow;
            try
            {
                if (tempWindow.StrandList.SelectedItem != null)
                {
                    if (CutNum.Text != "")
                    {
                        if ((bool)BetweenPick.IsChecked)
                        {
                            int ind = tempWindow.StrandList.SelectedIndex;  // getting the selected index
                            int cutNum = Convert.ToInt32(CutNum.Text);
                            int cutNum2 = Convert.ToInt32(CutNum2.Text);
                            List<int>[] tempList = BPCheck.Manipulate.PickAndCut(tempWindow.DnaList[ind].Sequence, cutNum, cutNum2);
                            tempWindow.DnaList.Add(new SsDna(tempList[0]) { Name = tempWindow.DnaList[ind].Name + "_0" });
                            tempWindow.DnaList.Add(new SsDna(tempList[1]) { Name = tempWindow.DnaList[ind].Name + "_1" });
                            tempWindow.DnaList.Add(new SsDna(tempList[2]) { Name = tempWindow.DnaList[ind].Name + "_2" });
                        }
                        else
                        {
                            if (LeftPick.IsChecked != null)
                            {
                                int ind = tempWindow.StrandList.SelectedIndex;  // getting the selected index
                                int cutNum = Convert.ToInt32(CutNum.Text);
                                if ((bool)LeftPick.IsChecked)
                                {
                                    List<int>[] tempList = BPCheck.Manipulate.PickAndCut(tempWindow.DnaList[ind].Sequence, cutNum);
                                    tempWindow.DnaList.Add(new SsDna(tempList[0]) { Name = tempWindow.DnaList[ind].Name + "_0" });
                                    tempWindow.DnaList.Add(new SsDna(tempList[1]) { Name = tempWindow.DnaList[ind].Name + "_1" });
                                }

                                if ((bool)RightPick.IsChecked)
                                {
                                    List<int>[] tempList = BPCheck.Manipulate.PickAndCut(tempWindow.DnaList[ind].Sequence, cutNum, "r");
                                    tempWindow.DnaList.Add(new SsDna(tempList[0]) { Name = tempWindow.DnaList[ind].Name + "_0" });
                                    tempWindow.DnaList.Add(new SsDna(tempList[1]) { Name = tempWindow.DnaList[ind].Name + "_1" });
                                }
                            }
                        }
                    }
                    else
                    {
                        tempWindow.Console.Text += "\n" + "Input the number of strand you want cut";
                    }

                }
                else
                {
                    tempWindow.Console.Text += "\n" + "Select a DNA strand.";
                }
                this.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
    }
}
