using CsvHelper;
using GPMI_Laser_Marking.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GPMI_Laser_Marking
{
    /// <summary>
    /// Interaction logic for Traceability.xaml
    /// </summary>
    public partial class Traceability : Window
    {
        public Traceability()
        {
            InitializeComponent();
            fromDate = DateTime.Now.AddMonths(-1);
            toDate = DateTime.Now.AddDays(1);
            fromdate_dp.SelectedDate = fromDate.Value;
            todate_dp.SelectedDate = toDate.Value;
    }

        private readonly int valueinpage = 100;
        private IEnumerable<dynamic> list;

        private void Search_btn_Click(object sender, RoutedEventArgs e)
        {
             
            switch (fqa_cbx.SelectedIndex)
            {
                case 0:
                    fqacheck = "ALL";

                    break;
                case 1:
                    fqacheck = "OK";

                    break;
                case 2:
                    fqacheck = "NG";
                    break;
                case 3:
                    fqacheck = "UnFQA";
                    break;
                default:
                    break;
            }

            fromDate = (fromdate_dp.SelectedDate).Value;
            toDate = (todate_dp.SelectedDate).Value.AddDays(1);
            using (var dbContext = new InputContext())
            {
                if (Ship_rbtn.IsChecked == true)
                {
                    list = ShippingCheck(dbContext);
                    gird.ItemsSource = list.Take(valueinpage).ToList();
                    totalpage = list.Count() / valueinpage;
                    totalpage++;
                    Allpage_lbl.Content = totalpage.ToString();
                    curentpage_lbl.Content = "1";
                    Couterpcs_tbl.Text = $"{list.Count()} Pcs";
                }
                if (Packaging_rbtn.IsChecked == true)
                {
                    list = PakagingCheck(dbContext);
                    gird.ItemsSource = list.Take(valueinpage).ToList();
                    totalpage = list.Count() / valueinpage;
                    totalpage++;
                    Allpage_lbl.Content = totalpage.ToString();
                    curentpage_lbl.Content = "1";
                    Couterpcs_tbl.Text = $"{list.Count()} Pcs";
                }
                if (Part_ID_rbtn.IsChecked == true)
                {
                    list = Part_ID_Check(dbContext);
                    gird.ItemsSource = list.Take(valueinpage).ToList();
                }
                if (Part_Number_rbtn.IsChecked == true)
                {
                    list = Part_NumberCheck(dbContext);
                    gird.ItemsSource = list.Take(valueinpage).ToList();
                    totalpage = list.Count() / valueinpage;
                    totalpage++;
                    Allpage_lbl.Content = totalpage.ToString();
                    curentpage_lbl.Content = "1";
                    Couterpcs_tbl.Text = $"{list.Count()} Pcs";
                }

                if (OrderNo_rbtn.IsChecked == true)
                {
                    
                   list = OrderNo(dbContext);
                    gird.ItemsSource = list.Take(valueinpage).ToList();
                    totalpage = list.Count() / valueinpage;
                    totalpage++;
                    Allpage_lbl.Content = totalpage.ToString();
                    curentpage_lbl.Content = "1";
                    Couterpcs_tbl.Text = $"{list.Count()} Pcs";
                }

                if (Con_rbtn.IsChecked == true)
                {

                    list = ContainerID(dbContext);
                    gird.ItemsSource = list.Take(valueinpage).ToList();
                    totalpage = list.Count() / valueinpage;
                    totalpage++;
                    Allpage_lbl.Content = totalpage.ToString();
                    curentpage_lbl.Content = "1";
                    Couterpcs_tbl.Text = $"{list.Count()} Pcs";
                }
                list = list.ToList();
                dbContext.Dispose();
            }
        }

        private IQueryable<dynamic> OrderNo(InputContext dbContext)
        {
            var r =
    (from c in dbContext.outputMarkings
     where c.Order_No == Search_value_txt.Text &&
                 c.Date_Time_Pakaging >= fromDate &&
                 c.Date_Time_Pakaging <= toDate
     select new
     {
         c.Order_No,
         c.Part_ID,
         c.Part_Number,
         c.Part_Name,
         c.Station,
         c.CNCMachine,
         c.TestAirValue,
         c.Version,
         c.Order_Date,
         c.Box_No,
         c.Pcs_in_Box,
         c.Date_Time_Marking,
         c.Marking_Account,
         c.FQA_Status,
         c.FQA_Account,
         c.Date_Time_FQA,
         c.QR_CodeID,
         c.Date_Time_Pakaging,
         c.Pakaging_Account,
         c.Shipping_Code,
         c.Date_Time_Shipping,
         c.Shipping_Code_Account
            }); return r;
        }
        string fqacheck = string.Empty;


        private IQueryable<dynamic> Part_NumberCheck(InputContext dbContext)
        {
            if (fqacheck=="OK")
            {
                var d = 
                (from c in dbContext.outputMarkings
                 where c.Part_Number == Search_value_txt.Text &&
                 c.Date_Time_Marking >= fromDate &&
                 c.Date_Time_Marking <= toDate &&
                 (c.FQA_Status == "OK" || c.FQA_Status == "Rework")
                 select new
                 {
                     c.Order_No,
                     c.Part_ID,
                     c.Part_Number,
                     c.Part_Name,
                     c.Station,
                     c.CNCMachine,
                     c.TestAirValue,
                     c.Version,
                     c.Order_Date,
                     c.Box_No,
                     c.Pcs_in_Box,
                     c.Date_Time_Marking,
                     c.Marking_Account,
                     c.FQA_Status,
                     c.FQA_Account,
                     c.Date_Time_FQA,
                     c.QR_CodeID,
                     c.Date_Time_Pakaging,
                     c.Pakaging_Account,
                     c.Shipping_Code,
                     c.Date_Time_Shipping,
                     c.Shipping_Code_Account
                 }); return d;
            }
            if (fqacheck == "NG")
            {             var d =
                (from c in dbContext.outputMarkings
                 where c.Part_Number == Search_value_txt.Text &&
                 c.Date_Time_Marking >= fromDate &&
                 c.Date_Time_Marking <= toDate &&
                 (c.FQA_Status == fqacheck)
                 select new
                 {
                     c.Order_No,
                     c.Part_ID,
                     c.Part_Number,
                     c.Part_Name,
                     c.Station,
                     c.CNCMachine,
                     c.TestAirValue,
                     c.Version,
                     c.Order_Date,
                     c.Box_No,
                     c.Pcs_in_Box,
                     c.Date_Time_Marking,
                     c.Marking_Account,
                     c.FQA_Status,
                     c.FQA_Account,
                     c.Date_Time_FQA,
                     c.QR_CodeID,
                     c.Date_Time_Pakaging,
                     c.Pakaging_Account,
                     c.Shipping_Code,
                     c.Date_Time_Shipping,
                     c.Shipping_Code_Account
                 }); return d;
            }
            if (fqacheck == "UnFQA")
            {        var d =
                (from c in dbContext.outputMarkings
                 where c.Part_Number == Search_value_txt.Text &&
                 c.Date_Time_Marking >= fromDate &&
                 c.Date_Time_Marking <= toDate &&
                 (c.FQA_Status == "")
                 select new
                 {
                     c.Order_No,
                     c.Part_ID,
                     c.Part_Number,
                     c.Part_Name,
                     c.Station,
                     c.CNCMachine,
                     c.TestAirValue,
                     c.Version,
                     c.Order_Date,
                     c.Box_No,
                     c.Pcs_in_Box,
                     c.Date_Time_Marking,
                     c.Marking_Account,
                     c.FQA_Status,
                     c.FQA_Account,
                     c.Date_Time_FQA,
                     c.QR_CodeID,
                     c.Date_Time_Pakaging,
                     c.Pakaging_Account,
                     c.Shipping_Code,
                     c.Date_Time_Shipping,
                     c.Shipping_Code_Account
                 }); return d;
            }

            var r =
                (from c in dbContext.outputMarkings
                 where c.Part_Number == Search_value_txt.Text &&
                 c.Date_Time_Marking >= fromDate &&
                 c.Date_Time_Marking <= toDate 
                 
                 select new
                 {
                     c.Order_No,
                     c.Part_ID,
                     c.Part_Number,
                     c.Part_Name,
                     c.Station,
                     c.CNCMachine,
                     c.TestAirValue,
                     c.Version,
                     c.Order_Date,
                     c.Box_No,
                     c.Pcs_in_Box,
                     c.Date_Time_Marking,
                     c.Marking_Account,
                     c.FQA_Status,
                     c.FQA_Account,
                     c.Date_Time_FQA,
                     c.QR_CodeID,
                     c.Date_Time_Pakaging,
                     c.Pakaging_Account,
                     c.Shipping_Code,
                     c.Date_Time_Shipping,
                     c.Shipping_Code_Account
                 }); return r;
        }

        private IQueryable<dynamic> Part_ID_Check(InputContext dbContext)
        {
            var r = (from c in dbContext.outputMarkings
                     where c.Part_ID.Equals(Search_value_txt.Text)
                     select new
                     {
                         c.Order_No,
                         c.Part_ID,
                         c.Part_Number,
                         c.Part_Name,
                         c.Station,
                         c.CNCMachine,
                         c.TestAirValue,
                         c.Version,
                         c.Order_Date,
                         c.Box_No,
                         c.Pcs_in_Box,
                         c.Date_Time_Marking,
                         c.Marking_Account,
                         c.FQA_Status,
                         c.FQA_Account,
                         c.Date_Time_FQA,
                         c.QR_CodeID,
                         c.Date_Time_Pakaging,
                         c.Pakaging_Account,
                         c.Shipping_Code,
                         c.Date_Time_Shipping,
                         c.Shipping_Code_Account
                     }); return r;
        }

        private IQueryable<dynamic> PakagingCheck(InputContext dbContext)
        {

            var r = (from c in dbContext.outputMarkings
                     where c.QR_CodeID.Equals(Search_value_txt.Text)
                     && c.Date_Time_Pakaging >= fromDate &&
                 c.Date_Time_Pakaging <= toDate
                     select new
                     {
                         c.Order_No,
                         c.Part_ID,
                         c.Part_Number,
                         c.Part_Name,
                         c.Station,
                         c.CNCMachine,
                         c.TestAirValue,
                         c.Version,
                         c.Order_Date,
                         c.Box_No,
                         c.Pcs_in_Box,
                         c.Date_Time_Marking,
                         c.Marking_Account,
                         c.FQA_Status,
                         c.FQA_Account,
                         c.Date_Time_FQA,
                         c.QR_CodeID,
                         c.Date_Time_Pakaging,
                         c.Pakaging_Account,
                         c.Shipping_Code,
                         c.Date_Time_Shipping,
                         c.Shipping_Code_Account
                     }); return r;
        }

        private DateTime? fromDate = DateTime.Today;
        private DateTime? toDate = DateTime.Today;

        private IQueryable<dynamic> ShippingCheck(InputContext dbContext)
        {
            var r =
                 (from c in dbContext.outputMarkings
                  where c.Shipping_Code.Equals(Search_value_txt.Text) &&
                  c.Date_Time_Shipping.Value >= fromDate &&
                  c.Date_Time_Shipping.Value <= toDate
                  select new
                  {
                      c.Order_No,
                      c.Part_ID,
                      c.Part_Number,
                      c.Part_Name,
                      c.Station,
                      c.CNCMachine,
                      c.TestAirValue,
                      c.Version,
                      c.Order_Date,
                      c.Box_No,
                      c.Pcs_in_Box,
                      c.Date_Time_Marking.Value,
                      c.Marking_Account,
                      c.FQA_Status,
                      c.FQA_Account,
                      c.Date_Time_FQA,
                      c.QR_CodeID,
                      c.Date_Time_Pakaging,
                      c.Pakaging_Account,
                      c.Shipping_Code,
                      c.Date_Time_Shipping,
                      c.Shipping_Code_Account
                  }); return r;
        }
        private IQueryable<dynamic> ContainerID(InputContext dbContext)
        {
            var r =
                 (from c in dbContext.outputMarkings
                  where c.Shipping_Code.Equals(Search_value_txt.Text) &&
                  c.Date_Time_Shipping.Value >= fromDate &&
                  c.Date_Time_Shipping.Value <= toDate
                  select new
                  {
                      c.Shipping_Code,
                      c.Date_Time_Shipping,
                      c.Shipping_Code_Account,
                      c.Order_No,
                      c.Part_Number,
                      c.Part_Name,
                      c.Box_No,
                      c.Pcs_in_Box,
                      c.QR_CodeID,
                      c.Date_Time_Pakaging,
                      c.Pakaging_Account,
                      
                      
                  }); return r.Distinct();
        }

        private void Gird_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            try
            {
                int columnIndex = gird.CurrentColumn.DisplayIndex;
                TextBlock targetCell = (TextBlock)gird.SelectedCells[columnIndex].Column.GetCellContent(gird.SelectedItem);
                Search_value_txt.Text = targetCell.Text;
                gird.UnselectAllCells();
            }
            catch (Exception)
            {
            }
        }

        private void Export_btn_Click(object sender, RoutedEventArgs e)
        {
            var records = GetList();

            if (records.Count > 0)
            {
                string filename;
                // Create OpenFileDialog
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog
                {
                    // Set filter for file extension and default file extension
                    //DefaultExt = ".png",
                    Filter = "CSV Files (*.csv)|*.csv"
                };

                // Display OpenFileDialog by calling ShowDialog method
                Nullable<bool> result = dlg.ShowDialog();

                // Get the selected file name and display in a TextBox
                if (result == true)
                {
                    // Open document
                    filename = dlg.FileName;
                    using (var writer = new StreamWriter(filename))
                    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    {
                        csv.WriteRecords(records);
                    }
                }
            }
        }

        private List<dynamic> GetList()
        {
            var dbContext = new InputContext();
            List<dynamic> list = new List<dynamic>();
            if (Ship_rbtn.IsChecked == true)
            {
                list = ShippingCheck(dbContext).ToList();
            }
            if (Packaging_rbtn.IsChecked == true)
            {
                list = PakagingCheck(dbContext).ToList();
            }
            if (Part_ID_rbtn.IsChecked == true)
            {
                list = Part_ID_Check(dbContext).ToList();
            }
            if (Part_Number_rbtn.IsChecked == true)
            {
                list = Part_NumberCheck(dbContext).ToList();
            }
            if (OrderNo_rbtn.IsChecked == true)
            {
                list = OrderNo(dbContext).ToList();
            }
            if (Con_rbtn.IsChecked == true)
            {
                list = ContainerID(dbContext).ToList();
            }
            dbContext.Dispose();
            return list;
        }

        private int pagecout = 0;
        private int totalpage = 0;

        private void Next_btn_Click(object sender, RoutedEventArgs e)
        {
            if (pagecout < totalpage - 1)
            {
                pagecout++;
            }
            curentpage_lbl.Content = (pagecout + 1).ToString();
            gird.ItemsSource = list.Skip(valueinpage * pagecout).Take(valueinpage).ToList();
        }

        private void Back_btn_Click(object sender, RoutedEventArgs e)
        {
            if (pagecout > 0)
            {
                pagecout--;
            }
            curentpage_lbl.Content = (pagecout + 1).ToString();
            gird.ItemsSource = list.Skip(valueinpage * pagecout).Take(valueinpage).ToList();
        }

        private void Search_value_txt_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Search_btn_Click(sender, e);
            }
        }

        private void gird_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new InputContext())
            {
                for (int i = 1; i <= 120; i++)
                {
                    var outputMarking = dbContext.outputMarkings.FirstOrDefault(o => o.Part_ID == i.ToString());

                    if (outputMarking == null)
                    {
                        outputMarking = new OutputMarking
                        {
                            Part_ID = i.ToString(),
                            QR_CodeID = "QRTest",
                            Date_Time_Marking = DateTime.Now,
                            Part_Number = "PN1"
                        };

                        dbContext.outputMarkings.Add(outputMarking);
                    }
                    else
                    {
                        outputMarking.QR_CodeID = "QRTest";
                        outputMarking.Date_Time_Marking = DateTime.Now;
                        outputMarking.Part_Number = "PN1";
                    }
                }
                dbContext.SaveChanges();
            }
        }

    }
}