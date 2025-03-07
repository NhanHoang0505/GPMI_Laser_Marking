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
using GPMI_Laser_Marking.Models;

namespace GPMI_Laser_Marking
{
    /// <summary>
    /// Interaction logic for LoginEnter.xaml
    /// </summary>
    public partial class LoginEnter : Window
    {
        public bool LoginOK { get; set; } = false;
        public LoginEnter()
        {
            InitializeComponent();
            ID_txt.Text = Properties.Settings.Default.Account;
            LoginAccountAdmin = "";
        }
        public string LoginAccountAdmin { get; private set; } 
        private void Login_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var db = new InputContext();
                var r = (from d in db.accounts
                         where d.ID == ID_txt.Text
                         select d).ToList();
                if (r.Count() == 0 || r == null)
                {
                    System.Windows.Forms.MessageBox.Show("Account does not exist !");
                    return;
                }
                else
                {
                    foreach (var item in r)
                    {
                        if (item.Password == PW_pbx.Password)
                        {
                            Properties.Settings.Default.Account = item.ID;
                            App.Account.ID = item.ID;
                            App.Account.FullName = item.FullName;
                            App.Account.FQA = item.FQA;
                            App.Account.Marking = item.Marking;
                            App.Account.Shipping = item.Shipping;
                            App.Account.Pakaging = item.Pakaging;
                            App.Account.Manager = item.Manager;
                            App.Account.Import = item.Import;
                            Properties.Settings.Default.Save();
                            db.Dispose();
                            LoginOK = true;

                            this.Close();
                            if (item.Manager)
                            {
                                LoginAccountAdmin = item.ID;
                            }     
                            return;
                        }
                    }

                }
                LoginOK = false;

                db.Dispose();
                System.Windows.Forms.MessageBox.Show("Login fail");
            }
            catch (Exception ex)
            {

                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }

        private void PW_pbx_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Login_btn_Click(sender, e);
            }
        }
    }
}
