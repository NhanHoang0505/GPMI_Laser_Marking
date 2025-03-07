using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace GPMI_Laser_Marking
{
    /// <summary>
    /// Interaction logic for ShippingWindow.xaml
    /// </summary>
    public partial class ShippingWindow : Window
    {
        public ObservableCollection<Models.OutputMarking> Shipping_List = new ObservableCollection<Models.OutputMarking>();
        public ShippingWindow()
        {
            InitializeComponent();
            Output_Grid.ItemsSource = Shipping_List;
            ID_tbx.Text = App.Account.ID;
            Name_tbx.Text = App.Account.FullName;
        }




        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.MainWindow.Show();
        }

        private void ShippingID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                ChangeFinish_btn_Click(sender, e);
            }
        }

        private void QRCode_ID_txt_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Return)
            {
                using (var db = new InputContext())                    
                {
                    string code = QRCode_ID_txt.Text.Trim('\r').Trim('\n');
                    var paka = db.outputMarkings.Where(p => p.QR_CodeID == code);
                    if (paka.FirstOrDefault() != null)
                    {
                        PartNumber_tbx.Text = paka.First().Part_Number;
                        PartName_tbx.Text = paka.First().Part_Number;
                        OrderNo_tbx.Text = paka.First().Order_No.ToString();
                        Quantity_tbx.Text = paka.First().Order_Quantity.ToString();
                        Pcs_tbx.Text = paka.First().Pcs_in_Box.ToString();
                        //if (paka.FirstOrDefault().Shipping_Code == null)
                        //{
                            foreach (var value in paka)
                             {
                                value.Date_Time_Shipping = DateTime.Now;
                                value.QR_CodeID = QRCode_ID_txt.Text;
                                value.Shipping_Code_Account = App.Account.ID;
                                value.Shipping_Code = ShippingID_txt.Text;
                            }
                            db.SaveChanges();
                            Shipping_List.Add(paka.First());
                            var a = db.outputMarkings.Where(p => p.Shipping_Code == ShippingID_txt.Text).Select(p => p.QR_CodeID).Distinct().Count();
                            Count_txt.Text = a.ToString();
                        //}
                        //else
                        //{
                        //    System.Windows.Forms.MessageBox.Show($" Quantity of {QRCode_ID_txt.Text} already shipped !", "Error");

                        //}


                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("QR Code ID don't exist !");
                    }
                    QRCode_ID_txt.Text = "";
                    QRCode_ID_txt.Focus();
                }
            }
        }

        private void ChangeFinish_btn_Click(object sender, RoutedEventArgs e)
        {
            if (ShippingID_txt.Text.Trim() != String.Empty)
            {
                QRCode_ID_txt.IsEnabled = true;

                var db = new InputContext();
                var a = db.outputMarkings.Where(p => p.Shipping_Code == ShippingID_txt.Text).Select(p => p.QR_CodeID).Distinct().Count();
                Count_txt.Text = a.ToString();
                ChangeShipping_btn.IsEnabled = true;
                ShippingID_txt.IsEnabled = false;
                ChangeFinish_btn.IsEnabled = false;
            }
            else
            {
                QRCode_ID_txt.IsEnabled = false;
            }

        }

        private void ChangeShipping_btn_Click(object sender, RoutedEventArgs e)
        {
            ShippingID_txt.IsEnabled = true;
            ChangeFinish_btn.IsEnabled = true;
            ChangeShipping_btn.IsEnabled = false;
        }
    }
}
