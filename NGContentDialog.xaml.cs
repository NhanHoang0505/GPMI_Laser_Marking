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
    /// Interaction logic for NGContentDialog.xaml
    /// </summary>
    public partial class NGContentDialog : Window
    {
        public NGContentDialog()
        {
            InitializeComponent();
        }
        public string NGContent { get; private set; }
        private void OK_Click(object sender, RoutedEventArgs e)
        {
            NGContent = NGContentTextBox.Text.Trim();
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
            NGContentTextBox.Clear();
        }

        private void NGContentTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
