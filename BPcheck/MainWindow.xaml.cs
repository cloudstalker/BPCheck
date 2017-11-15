using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Diagnostics;

namespace BPcheck
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
#region Constructors
        /// <summary>
        /// Program's main window
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            // global variable initialization
             
            DnaList = new ObservableCollection<SsDna>();
            StrandList.ItemsSource = DnaList;
            DnaList.Add(new SsDna(new List<int>() { 1, 2, -1, -2 }) { Name = "default_test" });
        }
#endregion

#region Fields
        /// <summary>
        /// The list of DNA that is getting manipulated or stored
        /// </summary>
        public ObservableCollection<SsDna> DnaList;
#endregion


        private void Open_Click(object sender, RoutedEventArgs e)
        {
            // Wipe previous list
            DnaList.Clear();

            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".txt",
                Filter = "FSATA txt file (*.txt)|*.txt|All files (*.*)|*.*"
            };
            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string fileToRead = dlg.FileName;
                try
                {
                    int counter = -1;
                    string tempName = "0";
                    // Read the fsata file
                    using(StreamReader streamReader = new StreamReader(fileToRead))
                    {
                        string line;
                        string tempLine = "";
                        while (true)
                        {
                            line = streamReader.ReadLine();
                            if(line == null)
                            {
                                if (counter != -1)
                                {
                                    DnaList.Add(new SsDna(BPCheck.SeqParse.Parse(FormatSeq.DelSpaceAndCap(tempLine))));
                                    DnaList[counter].Name = tempName;
                                }
                                break;
                            }
                            if (line != "")
                            {
                                if (line[0] == '>')
                                {
                                    if (counter != -1)
                                    {
                                        DnaList.Add(new SsDna(BPCheck.SeqParse.Parse(FormatSeq.DelSpaceAndCap(tempLine))));
                                        DnaList[counter].Name = tempName;
                                    }
                                    tempLine = "";
                                    counter += 1;
                                    line = line.Remove(0, 1);
                                    tempName = line.Trim();

                                }
                                else
                                {
                                    tempLine += line;
                                }
                            }
      
                        }
                    }
                    //for(int i = 0; i < DnaList.Count; i++)
                    //{
                    //    Strands.Items.Add(BPCheck.SeqParse.UnParse(DnaList[i].Sequence));
                    //}
                    Console.Text += "\n" + "File opened.";
                }
                catch(Exception err)
                {
                    Console.Text = err.Message;
                }
            }

            
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            DnaList.Clear();
            //Strands.Items.Clear();
            Console.Text += "\n" + "List cleared.";
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try { DnaList.RemoveAt(StrandList.SelectedIndex); }
            catch(Exception err)
            {
                MessageBox.Show("Select a strand in the list!");
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // Saving the file, read from listbox
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog
            {
                DefaultExt = ".txt",
                Filter = "FSATA txt file (*.txt)|*.txt|All files (*.*)|*.*"
            };
            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();

            if(result == true)
            {
                string fileToSave = dlg.FileName;
                try
                {
                    using(StreamWriter streamWriter = new StreamWriter(fileToSave))
                    {
                        for(int i = 0; i < DnaList.Count; i++)
                        {
                            streamWriter.WriteLine(">" + DnaList[i].Name);
                            streamWriter.WriteLine(BPCheck.SeqParse.UnParse(DnaList[i].Sequence));
                        }
                    }
                    Console.Text += "\n" + "List saved.";
                }
                catch(Exception err)
                {

                }
            }
        }

        private void Console_TextChanged(object sender, TextChangedEventArgs e)
        {
            Console.ScrollToEnd();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddSeqWindow addWin = new AddSeqWindow();
            addWin.Show();
        }

        private void CalcComp_Click(object sender, RoutedEventArgs e)
        {
            SsDna[] ssDna_2 = new SsDna[2];
            var temp = DnaList.Where(s => s.IsChecked).ToList<SsDna>();
            if (temp.Count == 2)
            {
                ssDna_2[0] = temp[0];
                ssDna_2[1] = temp[1];
            }
            else
            {
                Console.Text += "\n" + "Select exatly 2 strands!";
            }
            DsDna tempDsDna = new DsDna(ssDna_2);
            if (tempDsDna.IsSameLength)
            {
                if (tempDsDna.IsComplementary)
                {
                    Console.Text += "\n" + "Two strands are complementary";
                }
                else
                {
                    Console.Text += "\n" + "Fraction of complementary pairs: " + tempDsDna.ComplemPercent.ToString();
                    if(tempDsDna.ComplemPercent<0.8f || tempDsDna.MisMatch.Count > 5)
                    {
                        Console.Text += "\n" + "Mismatches too many, won't display.";
                    }
                    else
                    {
                        Console.Text += "\n" + "Mismatches at:\n";
                        for(int i = 0; i < tempDsDna.MisMatch.Count; i++)
                        {
                            Console.Text += tempDsDna.MisMatch[i] + " ";
                        }
                    }
                    

                    // Later implement a repairing code starting from here ** (using List<int> Mismatch)
                }
            }
            else
            {
                Console.Text += "\n" + "Two strands should have same length";
            }

        }

        private void GenRC_Click(object sender, RoutedEventArgs e)
        {
            if (StrandList.SelectedItem != null)
            {
                int ind = StrandList.SelectedIndex;

                var tempDna = BPCheck.Manipulate.ReverseComp(DnaList[ind].Sequence);
                DnaList.Add(new SsDna(tempDna) { Name = DnaList[ind].Name + "*" });
            }
            else
            {
                Console.Text += "\n" + "Select a strand from list.";
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void WhatsNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("Readme.txt");
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message + " Please reinstall application");
            }
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(this, "Author: HXP", "About");
        }

        private void Duplicate_Click(object sender, RoutedEventArgs e)
        {
            int ind = StrandList.SelectedIndex;
            DnaList.Add(new SsDna(DnaList[ind].Sequence) { Name = DnaList[ind].Name + "2" });
        }

        private void Concat_Click(object sender, RoutedEventArgs e)
        {
            var tempList = DnaList.Where(s => s.IsChecked).ToList<SsDna>();
            string tempName = "";
            List<int> tempSeq = new List<int>();
            for(int i = 0; i < tempList.Count; i++)
            {
                tempName += "+" + tempList[i].Name;
                tempSeq.AddRange(tempList[i].Sequence);
            }
            DnaList.Add(new SsDna(tempSeq) { Name = tempName });
        }

        private void Cut_Click(object sender, RoutedEventArgs e)
        {
            CutWindow cutWindow = new CutWindow();
            cutWindow.Show();
        }

        private void SelectSave_Click(object sender, RoutedEventArgs e)
        {
            // Saving the file, read from listbox
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog
            {
                DefaultExt = ".txt",
                Filter = "FSATA txt file (*.txt)|*.txt|All files (*.*)|*.*"
            };
            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string fileToSave = dlg.FileName;
                try
                {
                    using (StreamWriter streamWriter = new StreamWriter(fileToSave))
                    {
                        var tempList = DnaList.Where(s => s.IsChecked).ToList();
                        for (int i = 0; i < tempList.Count; i++)
                        {
                            streamWriter.WriteLine(">" + tempList[i].Name);
                            streamWriter.WriteLine(BPCheck.SeqParse.UnParse(tempList[i].Sequence));
                        }
                    }
                    Console.Text += "\n" + "List saved.";
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }
            }
        }

        private void SMAI_Click(object sender, RoutedEventArgs e)
        {
            var temp = DnaList[StrandList.SelectedIndex];
            List<int> result = temp.Search(new List<int> {-2, -2, -2, 2, 2, 2 });
            if (result != null)
            {
                Console.Text += "\n" + "Site occurs at (from 0) ";
                for (int i = 0; i < result.Count; i++)
                {
                    Console.Text += result[i] + ", ";
                }
            }
            else
            {
                Console.Text += "\n" + "No occurences found";
            }
        }
    }
}
