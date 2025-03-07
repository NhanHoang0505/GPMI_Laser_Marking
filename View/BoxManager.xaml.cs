using GPMI_Laser_Marking.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
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

namespace GPMI_Laser_Marking.View
{
    /// <summary>
    /// Interaction logic for BoxManager.xaml
    /// </summary>
    /// 


    public partial class BoxManager : Window
    {
        public ObservableCollection<PackagingManager> Items { get; set; }
        public ObservableCollection<OutputMarking> OItems { get; set; }
        private PackagingManager selectedItem;
        public PackagingManager SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                // Perform any additional actions or update other properties as needed
            }
        }
        private OutputMarking selectedOItem;
        public OutputMarking SelectedOItem
        {
            get { return selectedOItem; }
            set
            {
                selectedOItem = value;
                // Perform any additional actions or update other properties as needed
            }
        }
        public BoxManager(string Order)
        {
            Items=new  ObservableCollection<PackagingManager>();
            OItems = new ObservableCollection<OutputMarking>();
            InitializeComponent();
            this.DataContext = this;
            OrderNo_txt.Text = Order;
            using (var db = new InputContext())
            {
                var newpackaManager = db.packagingManagers.Where(p => p.Order_No == Order);
                Items.Clear();
                foreach (var item in newpackaManager)
                {
                    Items.Add(item);
                }
            }
        }

        private void OrderNo_txt_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Enter)
            //{
            //    using (var db = new InputContext())
            //    {
            //        var newpackaManager = db.packagingManagers.Where(p => p.Order_No == OrderNo_txt.Text);
            //        Items.Clear();
            //        foreach (var item in newpackaManager)
            //        {
            //            Items.Add(item);
            //        }
            //    }            
            //}
        }

        private void Packaging_lbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedItem != null)
            {
                PackagingID_txt.Text = SelectedItem.QRCODE_ID;
            }
                UpdatelistPartID();
        }
        private void UpdatelistPartID()
        {
            if (PackagingID_txt.Text != "" || PackagingID_txt != null)
            {
                int count =1;
                using (var db = new InputContext())
                {
                    var Output = db.outputMarkings.Where(p => p.QR_CodeID == PackagingID_txt.Text);
                    OItems.Clear();
                    foreach (var item in Output)
                    {
                        OItems.Add(item);
                    }
                }
            }
        }


        private void PartID_lbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedOItem!=null)
            {
                SeletedPartID_txt.Text = SelectedOItem.Part_ID;
            }
            else
            {
                SeletedPartID_txt.Text = string.Empty;
            }
              
        }

        public string ChooseQRCodeID { get;private set; }
        private void ChooseBox_btn_Click(object sender, RoutedEventArgs e)
        {
            ChooseQRCodeID = PackagingID_txt.Text;
            this.Close();
        }

        private void UnBoxAll_btn_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedItem==null)
            {
                return;
            }
            using (var db = new InputContext())
            {
                var unboxitems = db.outputMarkings.Where(p => p.QR_CodeID == SelectedItem.QRCODE_ID);
                foreach (var item in unboxitems)
                {
                    item.Order_No = string.Empty;
                    item.Pcs_in_Box = 0;
                    item.FQA_Status = string.Empty;
                    item.Box_No = 0;
                    item.QR_CodeID = string.Empty;
                    item.Order_Quantity = 0;
                }
                var unboxQR = db.packagingManagers.Where(p => p.QRCODE_ID == SelectedItem.QRCODE_ID);
                foreach (var item in unboxQR)
                {
                    item.CurrentPCS = 0;
                    item.paStatus = "Edited";
                }
               
                var newpackaManager = db.packagingManagers.Where(p => p.Order_No == SelectedItem.Order_No).ToList();
                Items.Clear();
                foreach (var item in newpackaManager)
                {
                    Items.Add(item);
                }
                db.SaveChanges();
            }

            UpdatelistPartID();
        }

        private void UnBox1Part_Click(object sender, RoutedEventArgs e)
        {
            Unbox();
            
        }

        private void UnBoxNGPart_Click(object sender, RoutedEventArgs e)
        {
            UnboxNG();
        }
        private void UnboxNG()
        {
            if (SelectedOItem != null)
            {
                using (var db = new InputContext())
                {
                    var unboxitems = db.outputMarkings.Where(p => p.Part_ID == SelectedOItem.Part_ID);
                    foreach (var item in unboxitems)
                    {
                        item.Order_No = string.Empty;
                        item.Pcs_in_Box = 0;
                        item.FQA_Status = "NG";
                        item.Box_No = 0;
                        item.QR_CodeID = string.Empty;
                        item.Order_Quantity = 0;
                        System.Windows.Forms.MessageBox.Show(item.Part_ID);
                    }
                    db.SaveChanges();
                    var unboxQR = db.packagingManagers.Where(p => p.QRCODE_ID == PackagingID_txt.Text);
                    foreach (var item in unboxQR)
                    {
                        if (item.CurrentPCS > 0)
                        {
                            item.CurrentPCS--;
                        }
                        item.paStatus = "Edited";
                        item.CurrentPCS = db.outputMarkings.Where(p => p.QR_CodeID == PackagingID_txt.Text).Count();
                    }
                    db.SaveChanges();
                    var newpackaManager = db.packagingManagers.Where(p => p.Order_No == SelectedItem.Order_No).ToList();
                    Items.Clear();
                    foreach (var item in newpackaManager)
                    {
                        Items.Add(item);
                    }

                    db.SaveChanges();
                }
                UpdatelistPartID();
            }
        }
        private void Unbox()
        {
            if (SelectedOItem != null && selectedItem != null)
            {
                using (var db = new InputContext())
                {
                    var unboxitems = db.outputMarkings.Where(p => p.Part_ID == SelectedOItem.Part_ID);
                    foreach (var item in unboxitems)
                    {
                        item.Order_No = string.Empty;
                        item.Pcs_in_Box = 0;                
                        item.Box_No = 0;
                        item.QR_CodeID = string.Empty;
                        item.Order_Quantity = 0;
                        System.Windows.Forms.MessageBox.Show(item.Part_ID);
                    }
                    db.SaveChanges();
                    var unboxQR = db.packagingManagers.Where(p => p.QRCODE_ID == PackagingID_txt.Text).FirstOrDefault();
                    if (unboxQR!= null)
                    {
                        unboxQR.paStatus = "Edited";
                        var a = db.outputMarkings.Where(p => p.QR_CodeID == PackagingID_txt.Text).ToList();
                        unboxQR.CurrentPCS = db.outputMarkings.Where(p => p.QR_CodeID == PackagingID_txt.Text).Count();
                    }
                    db.SaveChanges();

                    var newpackaManager = db.packagingManagers.Where(p => p.Order_No == SelectedItem.Order_No).ToList();
                    Items.Clear();
                    foreach (var item in newpackaManager)
                    {
                        Items.Add(item);
                    }

                    db.SaveChanges();
                }
                UpdatelistPartID();
            }
        }

        private void Replace_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedOItem != null)
            {
                using (var db = new InputContext())
                {
                    var oldpart =
                     (from outMarking in db.outputMarkings
                      where outMarking.Part_ID == SelectedOItem.Part_ID
                      && (outMarking.QR_CodeID == PackagingID_txt.Text)
                      select outMarking).FirstOrDefault();

                    var newpart =
                    (from outMarking in db.outputMarkings
                     where outMarking.Part_ID == Replace_Part_txt.Text
                     && (outMarking.QR_CodeID == string.Empty || outMarking.QR_CodeID==null)
                      && (outMarking.FQA_Status == "OK" || outMarking.FQA_Status == "Rework")
                     select outMarking).FirstOrDefault();

                    bool checkold = false;
                    bool checknew = false;
                    if (oldpart != null)
                    {
                        checkold = true;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show($"Old Part is Not True");
                    }
                    if (newpart != null)
                    {
                        checknew = true;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show($"New Part is Not True");
                    }
                    if (checkold && checknew)
                    {
                        if (newpart.Part_Number != newpart.Part_Number)
                        {
                            checkold = false;
                            MessageBox.Show("The new Part number is different from the old part number\r\nPart Number mới khác Part Number cũ");
                            return;
                        }
                        newpart.QR_CodeID = oldpart.QR_CodeID;
                        newpart.Order_No = oldpart.Order_No;
                        newpart.Pcs_in_Box = oldpart.Pcs_in_Box;
                        newpart.Box_No = oldpart.Box_No;
                        newpart.Order_Quantity = oldpart.Order_Quantity;

                        oldpart.Order_No = string.Empty;
                        oldpart.Pcs_in_Box = 0;
                        oldpart.Box_No = 0;
                        //oldpart.FQA_Status = "NG";
                        oldpart.QR_CodeID = string.Empty;
                        oldpart.Order_Quantity = 0;
                        db.SaveChanges();
                    }
                }
                UpdatelistPartID();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Part ID has not been selected\r\nChưa chọn part ID");
            }
        }

        private void Replace_Enter(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }
            if (SelectedOItem != null)
            {
                using (var db = new InputContext())
                {
                    var oldpart =
                     (from outMarking in db.outputMarkings
                     where outMarking.Part_ID == SelectedOItem.Part_ID
                     && (outMarking.QR_CodeID == PackagingID_txt.Text)
                     select outMarking).FirstOrDefault();

                    var newpart =
                    (from outMarking in db.outputMarkings
                     where outMarking.Part_ID == Replace_Part_txt.Text
                     && (outMarking.QR_CodeID == string.Empty)
                      && (outMarking.FQA_Status == "OK" || outMarking.FQA_Status == "Rework")
                     select outMarking).FirstOrDefault();
                    bool checkold = false;
                    bool checknew = false;
                    if (oldpart!= null)
                    {
                        checkold = true; 
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show($"Old Part is Not True");
                    }
                    if (newpart != null)
                    {
                        checknew = true;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show($"New Part is Not True");
                    }
                    if (checkold&& checknew)
                    {
                        if (newpart.Part_Number != newpart.Part_Number)
                        {
                            checkold = false;
                            MessageBox.Show("The new Part number is different from the old part number\r\nPart Number mới khác Part Number cũ");
                            return;
                        }
                        newpart.QR_CodeID = oldpart.QR_CodeID;
                        newpart.Order_No = oldpart.Order_No;
                        newpart.Pcs_in_Box = oldpart.Pcs_in_Box;
                        newpart.Box_No = oldpart.Box_No;
                        newpart.Order_Quantity = oldpart.Order_Quantity;

                        oldpart.Order_No = string.Empty;
                        oldpart.Pcs_in_Box = 0;
                        oldpart.Box_No = 0;
                        //oldpart.FQA_Status = "NG";
                        oldpart.QR_CodeID = string.Empty;
                        oldpart.Order_Quantity = 0;
                        db.SaveChanges();
                    }
                }
                UpdatelistPartID();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Part ID has not been selected\r\nChưa chọn part ID");
            }

        }

        private void PartID_lbx_LostFocus(object sender, RoutedEventArgs e)
        {
          
        }
    }
}
