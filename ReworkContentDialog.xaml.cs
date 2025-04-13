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

namespace GPMI_Laser_Marking
{
    /// <summary>
    /// Interaction logic for ReworkContentDialog.xaml
    /// </summary>
    public partial class ReworkContentDialog : Window
    {
        public string ReworkContent { get; private set; }
        public ReworkContentDialog()
        {
            InitializeComponent();
        }
        private void OK_Click(object sender, RoutedEventArgs e)
        {
            ReworkContent = ReworkContentTextBox.Text.Trim();
            DialogResult = true;
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            ReworkContentTextBox.Clear();
        }

        private void ReworkContentTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
