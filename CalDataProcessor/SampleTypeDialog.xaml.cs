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

namespace CalDataProcessor
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class SampleTypeDialog : Window
    {
        public string SampleType { get; set; }

        public SampleTypeDialog(string file)
        {
            InitializeComponent();
            SampleName.Items.Add(file);

        }

        private void Select_Click(object sender, RoutedEventArgs e)
        {
            if (OPC.IsChecked == true)
            {
                this.SampleType = "OPC";
                this.DialogResult = true;
                this.Close();
            }
            else if (EFC.IsChecked == true)
            {
                this.SampleType = "EFC";
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Please select an option or Skip.", "No selection made");
            }
        }

        private void Skip_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

 
    }
}
