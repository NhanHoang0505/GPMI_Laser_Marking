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
    /// Interaction logic for UpdateInput.xaml
    /// </summary>
    public partial class UpdateInput : Window
    {
        public UpdateInput()
        {
            InitializeComponent();
        }

        private void Part_Number_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                var db = new InputContext();
                var c = from d in db.orderInputs where d.Part_Number == Part_Number.Text select d;
                foreach (var item in c)
                {
                    Part_Name.Text = item.Part_Name;
                    Order_No.Text = item.Order_No;
                    Order_date.Text = item.Order_Date.ToString("d");
                    Order_Quantity.Text = item.Order_Quantity.ToString();
                    Version.Text = item.Version;
                    Vendor_Code.Text = item.Vendor_Code;
                    Pcs_Box.Text = item.Pcs_in_Box.ToString();
                }
                db.Dispose();

            }
        }

        private void Save_btn_Click(object sender, RoutedEventArgs e)
        {
            var db = new InputContext();
            var c = from d in db.orderInputs where d.Part_Number == Part_Number.Text select d;
            foreach (var item in c)
            {
                item.Order_Quantity = Convert.ToInt32(Order_Quantity.Text);
                item.Input_Account = App.Account.ID;
            }
            db.SaveChanges();
             var f = from d in db.orderInputs where d.Part_Number == Part_Number.Text select d.Order_Quantity;
            foreach (var item in f)
            {
                if (item == Convert.ToInt32(Order_Quantity.Text))
                {
                    System.Windows.Forms.MessageBox.Show("Update Finish");
                }; 
            }
            db.Dispose();
        }
    }
}
