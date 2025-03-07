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
    /// Interaction logic for NGListxaml.xaml
    /// </summary>
    public partial class NGListxaml : Window
    {
        public NGListxaml()
        {
            InitializeComponent();
            var db = new InputContext();
            var r = (from d in db.nGMarkings

                     select new
                     {
                         ID = d.ID.ToString(),
                         Part_ID = d.Part_ID.ToString(),
                         Part_Number = d.Part_Number.ToString(),
                         Status = d.Marking_Status.ToString(),
                         Account = d.Marking_Account.ToString(),
                         Station = d.Station,
                         Time = d.Date_Time_Marking.ToString()
                     }).Take(500).ToList();
            Error_Grid.ItemsSource = r;
        }

    }
}
