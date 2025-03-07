using GPMI_Laser_Marking.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
    /// Interaction logic for Import_Input_Order.xaml
    /// </summary>
    public partial class Import_Input_Order : Window
    {
        public ObservableCollection<OrderInput> inputlist = new ObservableCollection<OrderInput>();
        public Import_Input_Order()
        {
            InitializeComponent();
            Output_Grid.ItemsSource = inputlist;
            using (var db = new InputContext())
            {
                try
                {
                    foreach (var inputs in db.orderInputs.Take(20))
                    {
                        inputlist.Add(inputs);
                    };
                }
                catch (Exception)
                {
                    MessageBox.Show("Connection Fail !");
                }
            }
            
            import_panel.IsEnabled = App.Account.Import;
            Order_date.SelectedDate = DateTime.Now;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;
            var values = new string[8];
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    try
                    {
                        var fileStream = openFileDialog.OpenFile();
                        using (StreamReader reader = new StreamReader(fileStream))
                        {
                            fileContent = reader.ReadToEnd();
                        }
                        values = fileContent.Split('\u000A')[1].Trim().Split('\t');
                        Part_Name.Text = values[1];
                        Part_Number.Text = values[0];
                        Order_No.Text = values[2];
                        Order_date.Text = values[3];
                        Order_Quantity.Text = values[4];
                        Pcs_Box.Text = values[5];
                        Vendor_Code.Text = values[6];
                        Version.Text = values[7];
                        Save_btn.IsEnabled = true;
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Please Save and Close File Input \r\n" + filePath);
                    }
                }
            }

            //MessageBox.Show(fileContent, "File Content at path: " + filePath, MessageBoxButtons.OK);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Save_btn.IsEnabled = true;
            Grid_info.IsEnabled = true;
        }
        private string account = string.Empty;
        private void Save_btn_Click(object sender, RoutedEventArgs e)
        {
            DateTime loadedDate = Order_date.DisplayDate;
            if (!int.TryParse(Pcs_Box.Text, out int pcs))
            {
                MessageBox.Show("Check Pcs In Box again !");
                return;
            }
            if (int.TryParse(Order_Quantity.Text, out int qty))
            {
                var input = new Models.OrderInput()
                {
                    Part_Name = Part_Name.Text,
                    Part_Number = Part_Number.Text,
                    Order_No = Order_No.Text,
                    Order_Date = loadedDate,
                    Order_Quantity = qty,
                    Pcs_in_Box = pcs,
                    Vendor_Code = Vendor_Code.Text,
                    Version = Version.Text,
                    Input_Account = App.Account.ID,
                    Note = Note_txt.Text
                    
                };
                using (var db = new InputContext())
                {
                    try
                    {
                        db.orderInputs.Add(input);
                        db.SaveChanges();
                        //inputlist.Add(input);
                        Search_btn_Click(sender, new RoutedEventArgs());
                        MessageBox.Show("Add PartNumer Finish");
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Add PartNumer Fail !");
                    }
                }
                Output_Grid.ScrollIntoView(Output_Grid.Items[Output_Grid.Items.Count - 1]);
            }
            else
            {
                MessageBox.Show("Check Order Quayity again !");
            }
            
        }

        private void Search_btn_Click(object sender, RoutedEventArgs e)
        {
            inputlist.Clear();
            
            var db = new InputContext();
            IQueryable<OrderInput> value;
            if (partnumber_rbtn.IsChecked==true)
            {
                 value =
                db.orderInputs.Where(search => search.Part_Number.Contains(Search_value_txt.Text)).Take(50);
            }
            else
            {
                 value =
                db.orderInputs.Where(search => search.Order_No.Contains(Search_value_txt.Text)).Take(50);
            }

            try
            {
                
                foreach (var inputs in value)
                {
                    inputlist.Add(inputs);
                };
            }
            catch (Exception)
            {
                MessageBox.Show("Connection Fail !");
            }
            finally { db.Dispose(); }

            var cot = Output_Grid.Items.Count - 1;
            if (cot > 0)
            Output_Grid.ScrollIntoView(Output_Grid.Items[cot]); // chuyen den dong cuoi cung
            
        }


        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            DateTime loadedDate = Order_date.DisplayDate;
            if (!int.TryParse(Pcs_Box.Text, out int pcs))
            {
                MessageBox.Show("Check Pcs In Box again !");
                return;
            }
            if (int.TryParse(Order_Quantity.Text, out int qty))
            {
                using (var db = new InputContext())
                {
                    try
                    {
                        var input = db.orderInputs.FirstOrDefault(b => b.Order_No == Order_No.Text);
                        if (input != null)
                        {
                            input.Part_Name = Part_Name.Text;
                            input.Part_Number = Part_Number.Text;
                            input.Order_No = Order_No.Text;
                            input.Order_Date = loadedDate;
                            input.Order_Quantity = qty;
                            input.Pcs_in_Box = pcs;
                            input.Vendor_Code = Vendor_Code.Text;
                            input.Version = Version.Text;
                            input.Input_Account = App.Account.ID;
                            input.Note = Note_txt.Text;
                            db.SaveChanges();
                            MessageBox.Show("Update Part Numer Finish");
                            Search_btn_Click(sender, new RoutedEventArgs());
                        }
                        else
                        {
                            MessageBox.Show(" Part Numer Not Exist !");
                        }                        
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Update PartNumer Fail !");
                    }
                }
            }
            else
            {
                MessageBox.Show("Check Order Quayity again !");
            }
            Output_Grid.Items.Refresh();
        }

        private void Output_Grid_SelectedCellsChanged_1(object sender, SelectedCellsChangedEventArgs e)
        {
            UpdateBtn.IsEnabled = true;
            var list = (OrderInput)Output_Grid.SelectedItem;
            if (list != null)
            {
                Part_Name.Text = list.Part_Name;
                Part_Number.Text = list.Part_Number;
                Order_No.Text = list.Order_No;
                Order_date.SelectedDate = list.Order_Date;
                Order_Quantity.Text = list.Order_Quantity.ToString();
                Pcs_Box.Text = list.Pcs_in_Box.ToString();
                Vendor_Code.Text = list.Vendor_Code;
                Version.Text = list.Version;
                list.Input_Account = App.Account.ID;
                Note_txt.Text = list.Note;
                
            }


        }

        private void Output_Grid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
    }
}
