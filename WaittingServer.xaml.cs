using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GPMI_Laser_Marking
{
    /// <summary>
    /// Interaction logic for WaittingServer.xaml
    /// </summary>
    public partial class WaittingServer : Window
    {    
        public WaittingServer()
        {
            InitializeComponent();           
            if (Properties.Settings.Default.Connection == String.Empty)
            {
               var result= System.Windows.Forms.MessageBox.Show("No for Local test, Yes for connect to Server GMPI","Choose Sever",System.Windows.Forms.MessageBoxButtons.YesNo,System.Windows.Forms.MessageBoxIcon.Question);
                if (result==System.Windows.Forms.DialogResult.Yes)
                {
                    Properties.Settings.Default.Connection = "InputConnectionString";

                }
                else
                {
                    Properties.Settings.Default.Connection = "InputConnectionString";
                }
                
            }
            
            Task.Run(() =>
            {
                try
                {
                    using (var db = new InputContext())
                    {
                        db.accounts.FirstOrDefault(n => n.Manager);
                    }

                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Connect to SQL Server Fail !\r\n\r\n" + ex.Message);

                }

                this.Dispatcher.Invoke(new Action(() => {
                    this.Close();
                    close = true;
                }));
            });

        }

        bool close = false;
        private void text_Loaded(object sender, RoutedEventArgs e)
        {
           var task = Task.Run(() => {

                for (int i = 0; i < 30; i++)
                {
                    Thread.Sleep(200);
                    this.Dispatcher.Invoke(new Action(() => {
                        text.Text = "Connect to Server".PadLeft(16 + i, '.').PadRight(25,'.');
                        if (i > 8)
                        {
                            i = 0;
                        }
                    }));
                   if (close)
                   {
                       break;
                   }
               }
            });
        }
    }

}
