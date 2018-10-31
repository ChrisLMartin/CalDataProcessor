using System;
using System.Windows;
using System.IO;
using System.Data;

namespace CalDataProcessor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string[] Filenames { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnSelectFiles_Click(object sender, RoutedEventArgs e)
        {
            // Open multiselect file dialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Text documents (.txt)|*.txt",
                InitialDirectory = @"\\ICAL8000\Users\Public\Documents\Calmetrix\CalCommander 2\Export",
                Multiselect = true
            };

            Nullable<bool> result = dlg.ShowDialog();

            // If files selected, add filename only to list box
            if (result == true)
            {
                Filenames = dlg.FileNames;

                foreach (string filename in Filenames)
                {
                    string file = Path.GetFileNameWithoutExtension(filename);
                    ListCalFiles.Items.Add(file);
                }
            }

            

        }
        /// <summary>
        /// Creates CalRecord for each selected file, and displays parameters and values for last processed
        /// file in a new window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnProcessFiles_Click(object sender, RoutedEventArgs e)
        {
            CalRecordRepository repository = new CalRecordRepository(Filenames);

            //DataTable parameters = new DataTable();
            //DataTable values = new DataTable();

            //foreach (string filename in Filenames)
            //{
            //    CalRecord record = new CalRecord(filename);
            //    parameters = record.ParameterTable;
            //    values = record.ValuesTable;
            //}
            //DisplayParametersWindow parametersWindow = new DisplayParametersWindow(parameters, values);
            //parametersWindow.Show();
        }
    }
}
