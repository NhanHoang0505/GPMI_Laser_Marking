using GPMI_Laser_Marking.Models;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace GPMI_Laser_Marking
{
    /// <summary>
    /// Interaction logic for FQAWindown.xaml
    /// </summary>
    ///
    public sealed partial class Computer : ObservableOject
    {
        private int _ok;

        public int OK
        {
            get { return _ok; }
            set
            {
                _ok = value;
                OnPropertyChanged("OK");
            }
        }

        private int _ng;

        public int NG
        {
            get { return _ng; }
            set
            {
                _ng = value;
                OnPropertyChanged("NG");
            }
        }
    }

    public partial class FQAWindown : Window
    {
        public ObservableCollection<OutputMarking> fqalist = new ObservableCollection<OutputMarking>();
        private Computer MyComputer = new Computer();

        public FQAWindown(string ID, string fullname)
        {
            InitializeComponent();
            ID_tbx.Text = ID;
            Name_tbx.Text = fullname;
            this.Output_Grid.ItemsSource = fqalist;
            MyComputer.PropertyChanged += MyComputer_PropertyChanged;
        }

        private void MyComputer_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "OK":
                    OK_txt.Text = MyComputer.OK.ToString();
                    break;

                case "NG":
                    NG_txt.Text = MyComputer.NG.ToString();
                    break;

                default:
                    break;
            }
        }

        private void ReadPartID_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                try
                {
                    using (var db = new InputContext())
                    {
                        var outputMarking = db.outputMarkings.FirstOrDefault(p => p.Part_ID == ReadPartID_txt.Text);
                        if (outputMarking != null)
                        {
                            OrderNo_tbl.Text = outputMarking.Order_No;
                            PartNumber_tbx.Text = outputMarking.Part_Number;
                            PartName_tbx.Text = outputMarking.Part_Name;
                            Version_tbx.Text = outputMarking.Version;
                            VenderCode_tbx.Text = outputMarking.Vendor_Code;
                            Quantity_tbx.Text = $"{ outputMarking.Order_Quantity}";
                            if (outputMarking.Order_Date!= null)
                            {
                                OrderDate_tbx.Text = outputMarking.Order_Date.Value.ToShortDateString();
                            }
                            else
                            {
                                OrderDate_tbx.Text = "";
                            }                                         
                            MyComputer.OK = db.outputMarkings.Count(t => t.Order_No == outputMarking.Order_No && (t.FQA_Status == "OK" || t.FQA_Status == "Rework"));
                            MyComputer.NG = db.outputMarkings.Count(t => t.Order_No == outputMarking.Order_No && t.FQA_Status == "NG");
                            NG_btn.IsEnabled = true;
                            OK_btn.IsEnabled = true;
                            Rewwork_btn.IsEnabled = true;
                            switch (outputMarking.FQA_Status)
                            {
                                case "OK":
                                    StatusPart_tbl.Text = $"This Part Already Checked - OK";
                                    OK_btn.IsEnabled = false;
                                    Rewwork_btn.IsEnabled = false;
                                    break;

                                case "Rework":
                                    StatusPart_tbl.Text = $"This Part Already Checked - Rework";
                                    OK_btn.IsEnabled = false;
                                    Rewwork_btn.IsEnabled = false;
                                    break;

                                case "NG":
                                    StatusPart_tbl.Text = $"This Part Already Checked - NG";
                                    OK_btn.IsEnabled = false;
                                    NG_btn.IsEnabled = false;
                                    break;

                                default:
                                    StatusPart_tbl.Text = "";
                                    Rewwork_btn.IsEnabled = false;
                                    break;
                            }
                            Status_grid.IsEnabled = true;
                            ReadPartID_txt.IsEnabled = false;
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("Barcode Not True!");
                        }
                    }
                }
                catch (Exception)
                {
                    System.Windows.Forms.MessageBox.Show("Server disconnected!");
                }
            }
        }

        private void ResetValue()
        {
            ReadPartID_txt.Text = "";
            PartNumber_tbx.Text = "";
            PartName_tbx.Text = "";
            VenderCode_tbx.Text = "";
            Version_tbx.Text = "";
            OrderDate_tbx.Text = "";
            OrderNo_tbl.Text = "";
            Quantity_tbx.Text = "";
            Status_grid.IsEnabled = false;
            ReadPartID_txt.Focus();
            OK_txt.Text = "";
            NG_txt.Text = "";
        }

        private void OK_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new InputContext())
                {
                    var count = db.outputMarkings.Count(t => t.Order_No == OrderNo_tbl.Text && (t.FQA_Status == "OK" || t.FQA_Status == "Rework"));
                    var ok = db.outputMarkings.SingleOrDefault(p => p.Part_ID == ReadPartID_txt.Text);
                    if (ok != null)
                    {
                        if (string.IsNullOrEmpty( ok.FQA_Status))
                        {
                            ok.Date_Time_FQA = DateTime.Now;
                            ok.FQA_Status = "OK";
                            ok.FQA_Account = ID_tbx.Text;
                            db.SaveChanges();
                            MyComputer.OK++;
                            fqalist.Add(ok);
                        }
                        else
                        {
                            StatusPart_tbl.Text = $"This Part Already Checked - {ok.FQA_Status}";
                        }
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("This Part Not True");
                    }
                }
            }
            catch (SqlException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            ResetRead();
        }

        private void Return_btn_Click(object sender, RoutedEventArgs e)
        {
            ResetRead();
        }

        private void NG_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new InputContext())
                {
                    var ok = db.outputMarkings.FirstOrDefault(p => p.Part_ID == ReadPartID_txt.Text);
                    if (ok != null)
                    {
                        if (ok.FQA_Status == null)
                        {
                            ok.Date_Time_FQA = DateTime.Now;
                            ok.FQA_Status = "NG";
                            ok.FQA_Account = ID_tbx.Text;
                            db.SaveChanges();
                            MyComputer.NG++;
                            fqalist.Add(ok);
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show($"This Part Already Checked - {ok.FQA_Status}\r\n Login for Change Part to NG");
                            LoginEnter loginEnter = new LoginEnter();
                            loginEnter.ShowDialog();
                            if (loginEnter.LoginAccountAdmin != string.Empty)
                            {
                                ok.Date_Time_FQA = DateTime.Now;
                                ok.FQA_Status = "NG";
                                ok.FQA_Account = loginEnter.LoginAccountAdmin;
                                db.SaveChanges();
                                MyComputer.NG++;
                                MyComputer.OK--;
                                fqalist.Add(ok);
                            }
                        }
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("This Part Not True");
                    }
                }
            }
            catch (SqlException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            ResetRead();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.MainWindow.Show();
        }

        private void Rewwork_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new InputContext())
                {
                    //var count = db.outputMarkings.Count(t => t.Order_No == OrderNo_tbl.Text && (t.FQA_Status == "OK" || t.FQA_Status == "Rework"));
                    //if (count >= int.Parse(Quantity_tbx.Text))
                    //{
                    //    System.Windows.Forms.MessageBox.Show("Quantity is enough ! ");
                    //    return;
                    //}
                    var ok = db.outputMarkings.FirstOrDefault(p => p.Part_ID == ReadPartID_txt.Text);
                    if (ok != null)
                    {
                        if (ok.FQA_Status == "NG")
                        {
                            ok.Date_Time_FQA = DateTime.Now;
                            ok.FQA_Status = "Rework";
                            ok.FQA_Account = ID_tbx.Text;
                            db.SaveChanges();
                            MyComputer.OK++;
                            MyComputer.NG--;
                            fqalist.Add(ok);
                        }
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("This Part Not True");
                    }
                }
            }
            catch (SqlException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            ResetRead();
        }

        private void ResetRead()
        {
            StatusPart_tbl.Text = "";
            ReadPartID_txt.Text = string.Empty;
            Status_grid.IsEnabled = false;
            ReadPartID_txt.IsEnabled = true;
            ReadPartID_txt.Focus();
        }

        private void Traceability_btn_Click(object sender, RoutedEventArgs e)
        {
            new Traceability().Show();
        }

        private OrderInput orderInput = new OrderInput();

        private void Read_InputOrder_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Return) return;
            try
            {
                using (var db = new InputContext())
                {
                    var orderno = db.orderInputs.FirstOrDefault(o => o.Order_No == OrderNo_tbl.Text);
                    if (orderno != null)
                    {
                        orderInput = orderno;
                        PartNumber_tbx.Text = orderno.Part_Number;
                        PartName_tbx.Text = orderno.Part_Name;
                        Version_tbx.Text = orderno.Version;
                        VenderCode_tbx.Text = orderno.Vendor_Code;
                        OrderDate_tbx.Text = orderno.Order_Date.ToShortDateString();
                        Quantity_tbx.Text = orderno.Order_Quantity.ToString();
                        MyComputer.OK = db.outputMarkings.Count(t=>t.FQA_Status == "OK" || t.FQA_Status == "Rework");
                        MyComputer.NG = db.outputMarkings.Count(t => t.FQA_Status == "NG");
                        OrderNo_tbl.IsEnabled = false;
                        ReadPartID_txt.IsEnabled = true;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Order No Not True !");
                        OrderNo_tbl.Text = string.Empty;
                    }
                }
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("Server disconnected!");
            }
        }

        private void Output_Grid_LoadingRow(object sender, System.Windows.Controls.DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
    }
}