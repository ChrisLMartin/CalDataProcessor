using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for DisplayParametersWindow.xaml
    /// </summary>
    public partial class DisplayParametersWindow : Window
    {
        public DisplayParametersWindow()
        {
            InitializeComponent();
        }

        public DisplayParametersWindow(DataTable parameters, DataTable values)
        {
            InitializeComponent();
            parameterDataGrid.DataContext = parameters.DefaultView;
            valueDatagrid.DataContext = values.DefaultView;
        }

    }
}
