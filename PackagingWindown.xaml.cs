using GPMI_Laser_Marking.Models;
using GPMI_Laser_Marking.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace GPMI_Laser_Marking
{
    /// <summary>
    /// Interaction logic for PackagingWindown.xaml
    /// </summary>
    ///
    public sealed partial class Pakagingvalue : ObservableOject
    {
        private string _qR_CodeID;

        public string QR_CodeID
        {
            get { return _qR_CodeID; }
            set
            {
                _qR_CodeID = value;
                OnPropertyChanged("QR_CodeID");
            }
        }

        private int _count;

        public int TotalCount
        {
            get { return _count; }
            set
            {
                _count = value;
                OnPropertyChanged("TotalCount");
            }
        }

        private int _totalqty;

        public int Totalqty
        {
            get { return _totalqty; }
            set
            {
                _totalqty = value;
                OnPropertyChanged("Totalqty");
            }
        }

        private int _onepackagingcount;

        public int CurrentPcs
        {
            get { return _onepackagingcount; }
            set
            {
                _onepackagingcount = value;
                OnPropertyChanged("CurrentPcs");
            }
        }

        private int _pcsinbox;

        public int Pcsinbox
        {
            get { return _pcsinbox; }
            set
            {
                _pcsinbox = value;
                OnPropertyChanged("Pcsinbox");
            }
        }

        private int _currentbox;

        public int Currentbox
        {
            get { return _currentbox; }
            set
            {
                _currentbox = value;
                OnPropertyChanged("Currentbox");
            }
        }
    }

    public partial class PackagingWindown : Window
    {
        private SaToPrint print = new SaToPrint();
        private Pakagingvalue PA = new Pakagingvalue();
        public ObservableCollection<OutputMarking> Pakaging_List = new ObservableCollection<OutputMarking>();

        public PackagingWindown(string ID, string fullname)
        {
            InitializeComponent();
            ID_tbx.Text = ID;
            PA.PropertyChanged += PA_PropertyChanged;
            Output_Grid.ItemsSource = Pakaging_List;

            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                printes.Items.Add(printer);
            }

            printes.SelectedValue = Properties.Settings.Default.printter;
            if (File.Exists(Properties.Settings.Default.printImage))
            {
                image_printer.Source = new BitmapImage(new Uri(Properties.Settings.Default.printImage));
            }
            else
            {
                //string path = "";
                //path = System.AppContext.BaseDirectory;
                //image_printer.Source = new BitmapImage(new Uri("path\\temas-logo.png"));
                //Properties.Settings.Default.printImage = "temas-logo.PNG";
            }
        }

        private void PA_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CurrentPcs")
            {
                CurrentPcs.Text = PA.CurrentPcs.ToString();
                if (PA.CurrentPcs >= PA.Pcsinbox)
                {
                    Part_ID_txt.Text = "FULL";
                    Part_ID_txt.IsEnabled = false;
                    Print_Finish_btn.Opacity = 1;
                }
                else
                {
                    Print_Finish_btn.Opacity = 0.3;
                    Part_ID_txt.Text = "";
                    Part_ID_txt.IsEnabled = true;
                }
            }
            if (e.PropertyName == "Pcsinbox")
            {
                Pcs_in_tbl.Text = PA.Pcsinbox.ToString();
            }
            if (e.PropertyName == "Currentbox")
            {
                Current_Box_txt.Text = PA.Currentbox.ToString();
            }
            if (e.PropertyName == "TotalCount")
            {
                TotalCount_tbl.Text = PA.TotalCount.ToString();
            }
            if (e.PropertyName == "QR_CodeID")
            {
                QR_CODE_ID_txt.Text = $"GPMI-{PartNumber_tbx.Text}-{OrderNo}-{PA.Pcsinbox}-{PA.Currentbox}";
            }
        }

        private void CheckOK_txt_GotFocus(object sender, RoutedEventArgs e)
        {
            Part_ID_txt.Text = "";
        }

        private void Add_PartID_txt_Keydown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (ReadOrderNo_txt.Text == string.Empty)
                {
                    System.Windows.Forms.MessageBox.Show("Please Enter Part Number !");
                }
                else
                {
                    try
                    {
                        AddPart();
                    }
                    catch (SqlException ex)
                    {
                        System.Windows.Forms.MessageBox.Show("Add Fail ! \r\n" + ex.Message);
                    }
                    finally
                    {
                        Part_ID_txt.Text = "";
                        Part_ID_txt.Focus();
                    }
                }
            }
        }

        private void AddPart()
        {
            using (var db = new InputContext())
            {

                var pa =
                (from outMarking in db.outputMarkings
                 where outMarking.Part_ID == Part_ID_txt.Text
                  && (outMarking.FQA_Status == "OK" || outMarking.FQA_Status == "Rework")
                 select outMarking).FirstOrDefault();

                if (pa != null)
                {
                    if (!string.IsNullOrEmpty(pa.QR_CodeID))
                    {
                        System.Windows.Forms.MessageBox.Show($"Part Already Packaged \r\nSản phẩm Đã đóng gói rồi");
                        return;
                    }

                    if (pa.Part_Number != PartNumber_tbx.Text)
                    {
                        System.Windows.Forms.MessageBox.Show($"Wrong! PartNumber for {ReadOrderNo_txt.Text} is {PartNumber_tbx.Text} \r\nCheck lại xem Part Number Đã đúng cho Order No chưa!\r\nPart Number của Order No hiện tại là {PartNumber_tbx.Text} nhưng Part Number đang quét là {pa.Part_Number} ", "Wrong PartID", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                        return;
                    }

                    pa.Date_Time_Pakaging = DateTime.Now;
                    pa.Box_No = PA.Currentbox;
                    pa.Pcs_in_Box = PA.Pcsinbox;
                    pa.QR_CodeID = QR_CODE_ID_txt.Text;
                    pa.Pakaging_Account = ID_tbx.Text;
                    var orderInput = db.orderInputs.FirstOrDefault(p => p.Order_No == OrderNo);
                    pa.Order_No = orderInput.Order_No;
                    pa.Version = orderInput.Version;
                    pa.Vendor_Code = orderInput.Vendor_Code;
                    pa.Order_Date = orderInput.Order_Date;
                    pa.Order_Quantity = orderInput.Order_Quantity;
                    if (PA.TotalCount < pa.Order_Quantity)
                    {
                        Pakaging_List.Add(pa);
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Part Packed quantity is over Order quantity\r\nSố lượng Sản phẩm đóng gói đã vượt qua số lượng đặt hàng");
                        return;
                    }

                    // box hien tai
                    PA.TotalCount++;
                    PA.CurrentPcs++;

                    var newpackaManager = db.packagingManagers.Where(p => p.QRCODE_ID == PA.QR_CodeID).FirstOrDefault();
                    if (newpackaManager == null)
                    {
                        var paka = new PackagingManager() { CurrentPCS = 1, QRCODE_ID = PA.QR_CodeID, Order_No = OrderNo, pcs_in_box = PA.Pcsinbox, paStatus = "Packaging", Box_no = PA.Currentbox };
                        db.packagingManagers.Add(paka);
                    }
                    else
                    {
                        newpackaManager.CurrentPCS = PA.CurrentPcs;
                    }
                    db.SaveChanges();
                    db.Dispose();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show($"Part don't have in Order No -{OrderNo}-, Or FQA Check  \r\nSản phẩm không có trong thuộc Order No : -{OrderNo}- hoặc Chưa check FQA");
                }

                if (PA.CurrentPcs >= PA.Pcsinbox)
                {
                    // print................
                    if (!print.PrintLabel(GetValuePrint(), Properties.Settings.Default.printter))
                    {
                        System.Windows.Forms.MessageBox.Show("Printer Not true " + Properties.Settings.Default.printter);
                    }
                    Part_ID_txt.IsEnabled = false;
                    Print_Finish_btn.Opacity = 1;
                }
            }
        }

        private string OrderNo = string.Empty;

        private void Read_OrderNo_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Return) return;
            try
            {
                if (Properties.Settings.Default.printter == String.Empty)
                {
                    System.Windows.Forms.MessageBox.Show("Please Set Printer !");
                    ResetValue();
                }
                LoadOrderBarcode();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Connect File to Server\r\n{ex}");
            }
        }

        private void LoadOrderBarcode()
        {
            using (var db = new InputContext())
            {
                var pa = db.orderInputs.FirstOrDefault(p => p.Order_No == ReadOrderNo_txt.Text );
                if (pa != null)
                {
                    PartNumber_tbx.Text = pa.Part_Number;
                    PartName_tbx.Text = pa.Part_Name;
                    Quantity_tbx.Text = pa.Order_Quantity.ToString();
                    PA.Totalqty = pa.Order_Quantity;
                    OrderNo = pa.Order_No;
                    Revision_tbx.Text = pa.Version;
                    PA.Pcsinbox = pa.Pcs_in_Box;
                    if (PA.Totalqty % PA.Pcsinbox == 0)
                    {
                        AllBox_tbx.Text = $"{PA.Totalqty / PA.Pcsinbox}";
                    }
                    else
                    {
                        AllBox_tbx.Text = $"{PA.Totalqty / PA.Pcsinbox + 1}";
                    }

                    // box hien tai
                    var lastpack = db.packagingManagers.Where(p => p.Order_No == ReadOrderNo_txt.Text).OrderByDescending(x => x.Box_no).FirstOrDefault();
                    if (lastpack == null)
                    {
                        PA.CurrentPcs = 0;
                        PA.Currentbox = 1;
                        PA.QR_CodeID = $"GPMI-{PartNumber_tbx.Text}-{OrderNo}-{PA.Pcsinbox}-{PA.Currentbox}";
                    }
                    else
                    {
                        PA.Currentbox = lastpack.Box_no;
                        UpdateQRCodeID($"GPMI-{PartNumber_tbx.Text}-{OrderNo}-{PA.Pcsinbox}-{PA.Currentbox}");
                    }
                    PA.TotalCount = db.outputMarkings.Count(t => t.Order_No == OrderNo 
                    && ( t.FQA_Status=="OK" || t.FQA_Status == "Rework")
                    && (t.QR_CodeID !=null && t.QR_CodeID != string.Empty)
                    );
                    ReadOrderNo_txt.IsEnabled = false;
                    PartGird.IsEnabled = true;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Barcode Not True!");
                    ResetValue();
                }
            }
        }

        private void ResetValue()
        {
            Part_ID_txt.Text = "";
            PartNumber_tbx.Text = "";
            PartName_tbx.Text = "";
            ReadOrderNo_txt.Text = "";
            Revision_tbx.Text = "";
            Quantity_tbx.Text = "";
            Pcs_in_tbl.Text = "";
            QR_CODE_ID_txt.Text = "";
        }

        private void Print_Finish_btn_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new InputContext())
            {
                var oldpackaManager = db.packagingManagers.Where(p => p.QRCODE_ID == PA.QR_CodeID).FirstOrDefault();
                if (oldpackaManager != null)
                {
                    oldpackaManager.CurrentPCS = PA.CurrentPcs;
                    oldpackaManager.paStatus = "Finish";
                    db.SaveChanges();
                }
                if (PA.TotalCount >= PA.Totalqty)
                {
                    System.Windows.Forms.MessageBox.Show("Part Packed quantity is equal Order quantity\r\nSố lượng Sản phẩm đóng gói đã đạt số lượng đặt hàng");
                    return;
                }
                PA.Currentbox++;
                PA.CurrentPcs = 0;
                UpdateQRCodeID($"GPMI-{PartNumber_tbx.Text}-{OrderNo}-{PA.Pcsinbox}-{PA.Currentbox}");
            }
            Output_Grid.ItemsSource = Pakaging_List;
            Part_ID_txt.Focus();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.MainWindow.Show();
        }

        private void Print_btn_Click(object sender, RoutedEventArgs e)
        {
            //Print.......
            print.PrintLabel(GetValuePrint(), printes.SelectedValue.ToString());
        }

        private void PrintView_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Properties.Settings.Default.printter == String.Empty)
                {
                    System.Windows.Forms.MessageBox.Show("Please Select Printer !");
                }
                else
                {
                    PrintPreview p = new PrintPreview(GetValuePrint());
                    p.Show();
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private Dictionary<string, string> GetValuePrint()
        {
            Dictionary<string, string> MyDic4 = new Dictionary<string, string>();
            MyDic4.Add("partnumber", PartNumber_tbx.Text);
            MyDic4.Add("partname", PartName_tbx.Text);
            MyDic4.Add("orderno", ReadOrderNo_txt.Text);
            MyDic4.Add("boxno", Current_Box_txt.Text.PadLeft(2, '0') + "/" + AllBox_tbx.Text.PadLeft(2, '0'));
            MyDic4.Add("quantity", CurrentPcs.Text.PadLeft(2, '0') + " Pcs");
            MyDic4.Add("revision", Revision_tbx.Text);
            MyDic4.Add("inspector", ID_tbx.Text);
            MyDic4.Add("varPath", Properties.Settings.Default.printImage);
            MyDic4.Add("qrcodeid", QR_CODE_ID_txt.Text);
            MyDic4.Add("qrcodefix", QR_CODE_ID_txt.Text);
            return MyDic4;
        }

        private void printes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Properties.Settings.Default.printter = printes.SelectedValue.ToString();
            Properties.Settings.Default.Save();
        }

        private void changeimagebtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)

                image_printer.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            Properties.Settings.Default.printImage = openFileDialog.FileName;
            Properties.Settings.Default.Save();
        }

        private void ChangeCode_btn_Click(object sender, RoutedEventArgs e)
        {
            ReadOrderNo_txt.IsEnabled = true;
            PartGird.IsEnabled = false;
            ResetValue();
            ReadOrderNo_txt.Focus();
        }

        private void Output_Grid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void BoxManager_Button_Click(object sender, RoutedEventArgs e)
        {
            BoxManager boxManager = new BoxManager(OrderNo);
            boxManager.ShowDialog();
            if (boxManager.ChooseQRCodeID != null)
            {
                UpdateQRCodeID(boxManager.ChooseQRCodeID);
            }
            else
            {
                UpdateQRCodeID(QR_CODE_ID_txt.Text);
            }
        }

        private void UpdateQRCodeID(string QRCodeID)
        {
            List<OutputMarking> palist;
            bool update = false;
            using (var db = new InputContext())
            {
                palist = db.outputMarkings.Where(p => p.QR_CodeID == QRCodeID).ToList();
                var newpackaManager = db.packagingManagers
                    .Where(p => p.QRCODE_ID == QRCodeID).FirstOrDefault();
                if (newpackaManager != null)
                {
                    update = true;
                    PA.Currentbox = newpackaManager.Box_no;
                    PA.CurrentPcs = newpackaManager.CurrentPCS;
                    PA.TotalCount = db.outputMarkings.Count(t => t.Order_No == OrderNo
                    && (t.FQA_Status == "OK" || t.FQA_Status == "Rework")
                    && (t.QR_CodeID != null && t.QR_CodeID != string.Empty)
                    );
                }
                else
                {
                    Pakaging_List.Clear();
                }
            }
            if (QRCodeID != string.Empty)
            {
                PA.QR_CodeID = QRCodeID;
            }
            if (update)
            {
                Pakaging_List = new ObservableCollection<OutputMarking>(palist);
                Output_Grid.ItemsSource = Pakaging_List;
            }
        }
    }
}