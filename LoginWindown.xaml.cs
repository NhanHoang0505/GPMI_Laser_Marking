using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MessageBox = System.Windows.Forms.MessageBox;

namespace GPMI_Laser_Marking
{
    /// <summary>
    /// Interaction logic for LoginWindown.xaml
    /// </summary>
    public partial class LoginWindown : Window
    {
        public LoginWindown()
        {
            var mutex = new System.Threading.Mutex(false, System.Reflection.Assembly.GetEntryAssembly().GetName().Name);
            if (!mutex.WaitOne(0))
            {
                System.Windows.MessageBox.Show(($"Already running. Press any key to continue . . ."));
                Environment.Exit(1);
            }
            WaittingServer sv = new WaittingServer();
            sv.ShowDialog();
            InitializeComponent();
        }


        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Test");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //this.Hide();
            new MainWindow(App.Account.ID, App.Account.FullName).ShowDialog();    
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //this.Hide();
            new FQAWindown(App.Account.ID, App.Account.FullName).Show();
            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //this.Hide();
            new PackagingWindown(App.Account.ID, App.Account.FullName).ShowDialog();
            
        }
        private bool isLogin = false;
        private void Login_btn_Click(object sender, RoutedEventArgs e)
        {
            if (isLogin)
            {
                string message = "Do you want Logout ?";
                string caption = "Logout Comfirm !";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;

                // Displays the MessageBox.
                result = MessageBox.Show(message, caption, buttons,MessageBoxIcon.Question);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    App.Account = new Models.Account();
                    UpdateAccount();
                    isLogin = false;
                    return;
                }
                return;
            }
            var login = new LoginEnter();
            login.ShowDialog();
            isLogin = login.LoginOK;
            if (isLogin)
            {
                UpdateAccount();
            }
        }
        private void UpdateAccount() 
            {
            laser_btn.IsEnabled = App.Account.Marking;
            FQA_btn.IsEnabled = App.Account.FQA;
            Pakaging_btn.IsEnabled = App.Account.Pakaging;
            Shipping_btn.IsEnabled = App.Account.Shipping;
            account_btn.IsEnabled = App.Account.Manager;
            import_btn.IsEnabled = App.Account.Manager;
            update_btn.IsEnabled = App.Account.Manager;
            ID_tbx.Text = App.Account.ID;
            Name_tbx.Text = App.Account.FullName;
        } 

        private void Account_btn_Click(object sender, RoutedEventArgs e)
        {
            //this.Hide();
            new AccountManage().ShowDialog();
        }

        private void Import_btn_Click(object sender, RoutedEventArgs e)
        {
            //this.Hide();
            new Import_Input_Order().ShowDialog();
        }

        private void Update_btn_Click(object sender, RoutedEventArgs e)
        {
            //this.Hide();
            new UpdateInput().ShowDialog();
        }

        private void Shutdown_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void Shipping_btn_Click(object sender, RoutedEventArgs e)
        {
            new ShippingWindow().ShowDialog();
        }

        private void TraceabilityBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {                
                new Traceability().Show();               
            }
            catch (Exception)
            {

            }
            
        }

        private void ServerSettingbtn_Click(object sender, RoutedEventArgs e)
        {     
           SetupIP setupIP = new SetupIP();
            setupIP.ShowDialog();
           
        }
    }
}
