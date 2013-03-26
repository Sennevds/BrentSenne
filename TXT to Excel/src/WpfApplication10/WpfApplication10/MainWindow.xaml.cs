﻿using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using Microsoft.Office.Interop.Excel;
using MessageBox = System.Windows.MessageBox;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace WpfApplication10
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private int _countingtitle; //zodat die datum enzo er maar 1 keer opstaat

        public MainWindow()
        {
            InitializeComponent();
        }

        private void rFolder_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.ShowDialog();
            if (folder.SelectedPath != string.Empty)
            {
                string[] files = Directory.GetFiles(folder.SelectedPath,"*.txt");
                foreach (var file in files)
                {
                    debug.Items.Add(file); //dit werk wel maar ni voor de volgende stap
                    // ik heb effe zitte sukkelen me nen databinder naar de listbox maar da                                                                                                      was zo veel gezever da
                    //ik da heb opgegeven
                }
            }
        }

        private void Debug_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            debug2.Items.Clear(); //anders blijft er meer en meer bijkomen bij een verandering
            _countingtitle = 0;

            for (int i = 0; i < debug.SelectedItems.Count; i++)
            {
                using (StreamReader reader = new StreamReader(Convert.ToString(debug.SelectedItems[i])))
                    //vanaf hier gewoon simpelweg elke selecteditem in die 2delistbox zetten (debug2)
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (_countingtitle == 0 || line.Contains("Date") == false)
                        {
                            debug2.Items.Add(line);
                            _countingtitle = 1; //zo komt er dus maar 1 keer die datum in
                        }
                    }
                }
            }
        }

        private void sAll_Click(object sender, RoutedEventArgs e)
        {
            if(debug.HasItems)
            debug.SelectAll();
            else
            {
                MessageBox.Show("There are no file's selected", "Error");
            }
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e) //exit button van menu
        {
            MessageBoxResult dialogResult = MessageBox.Show(@"Do you want to quit?", @"Sure", MessageBoxButton.YesNo);

            //vraag ineen message box

            if (dialogResult == MessageBoxResult.Yes)
            {
                Close(); //afsluiten van het programma
            }


        }

        private void About(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "This software is coded by Brent Proost and Senne van de Sompele \nIf you want to use this software please contact us",
                "About");
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            string s = " ";
            string t = " ";
            if (debug.SelectedItems!=null&& debug2.HasItems)
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = "csv files (*.csv)|*.csv";
                dlg.Title = "Export in CSV format";
                dlg.CheckPathExists = true;
                //If InitialDirectory is not specified, the default path is My Documents
                //dlg.InitialDirectory = Application.StartupPath; 
                dlg.ShowDialog();
                if (dlg.FileName != "")
                {
                    StreamWriter myOutputStream = new StreamWriter(dlg.FileName);
                    for (int i = 0; i < debug2.Items.Count; i++)
                    {
                        s = Convert.ToString(debug2.Items[i]);
                        t = s.Replace('\t', ';');
                        myOutputStream.WriteLine(t);
                    }
                    myOutputStream.Close();
                }
            }
            else if (!debug.HasItems)
            {
                MessageBox.Show("There are no file's", "Error");
            }
            else if (!debug2.HasItems && debug.HasItems)
            {
                MessageBox.Show("There are no file's selected", "Error");
            }
            else
            {
                MessageBox.Show("Unknown Error", "Error"); 
            }

        }

        private void clAll_Click(object sender, RoutedEventArgs e)
        {
            if(debug.HasItems)
            debug.Items.Clear();
            else
            {
                MessageBox.Show("There are no files to remove", "Error");
            }
        }
    }
}
