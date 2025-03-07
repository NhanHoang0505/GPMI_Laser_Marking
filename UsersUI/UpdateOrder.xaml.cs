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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GPMI_Laser_Marking.UsersUI
{
    /// <summary>
    /// Interaction logic for UpdateOrder.xaml
    /// </summary>
    public partial class UpdateOrder : UserControl
    {
        public UpdateOrder()
        {
            InitializeComponent();
           
        }

        private void UpdateOrder1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("UpdateOrder");
        }
    }
}
