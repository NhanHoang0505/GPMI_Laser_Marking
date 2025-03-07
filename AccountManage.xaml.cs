using GPMI_Laser_Marking.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
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
    /// Interaction logic for AccountManage.xaml
    /// </summary>
    /// 

    public partial class AccountManage : Window
    {
        public AccountManage()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (var db = new InputContext())
            {
                var a = from d in db.accounts select d;
                Output_Grid.ItemsSource = a.ToList();
                db.SaveChanges();
                db.Dispose();
            }
        }

        private void Output_Grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var acc = (Account)this.Output_Grid.SelectedItem;
  
            if (acc != null)
            {
                this.ID_txt.Text = acc.ID;
                this.FullName_txt.Text = acc.FullName;
                this.Password.Text = acc.Password;
                this.FQA_checkbox.IsChecked = acc.FQA;
                this.Marking_checkbox.IsChecked = acc.Marking;
                this.Shippingg_checkbox.IsChecked = acc.Shipping;
                this.Manager_Account_checkbox.IsChecked = acc.Manager;
                this.Pakaging_checkbox.IsChecked = acc.Pakaging;
                this.Import_checkbox.IsChecked = acc.Import;
            }
        }
        private void UpdateData()
        {
            var db = new InputContext();
            var a = from d in db.accounts select d;
            Output_Grid.ItemsSource = a.ToList();
            db.Dispose();
            this.ID_txt.Text = String.Empty;
            this.FullName_txt.Text = String.Empty;
            this.Password.Text = String.Empty;
            this.FQA_checkbox.IsChecked = false;
            this.Marking_checkbox.IsChecked = false;
            this.Shippingg_checkbox.IsChecked = false;
            this.Manager_Account_checkbox.IsChecked = false;
            this.Pakaging_checkbox.IsChecked = false;
            this.Import_checkbox.IsChecked = false;
        }
        private void Update_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new InputContext())
                {
                    var r = (from d in db.accounts
                             where d.ID == ID_txt.Text
                             select d);
                    foreach (var item in r)
                    {
                        item.ID = this.ID_txt.Text;
                        item.FullName = this.FullName_txt.Text;
                        item.FQA = (bool)this.FQA_checkbox.IsChecked;
                        item.Manager = (bool)this.Manager_Account_checkbox.IsChecked;
                        item.Marking = (bool)this.Marking_checkbox.IsChecked;
                        item.Pakaging = (bool)this.Pakaging_checkbox.IsChecked;
                        item.Shipping = (bool)this.Shippingg_checkbox.IsChecked;
                        item.Password = this.Password.Text;
                        item.Import = (bool)this.Import_checkbox.IsChecked;
                    }
                    db.SaveChanges();
                    db.Dispose();
                    UpdateData();
                }
            }
            catch (SqlException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void Add_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var acc = new Account();
                acc.ID = this.ID_txt.Text;
                acc.FullName = this.FullName_txt.Text;
                acc.FQA = (bool)this.FQA_checkbox.IsChecked;
                acc.Manager = (bool)this.Manager_Account_checkbox.IsChecked;
                acc.Marking = (bool)this.Marking_checkbox.IsChecked;
                acc.Pakaging = (bool)this.Pakaging_checkbox.IsChecked;
                acc.Shipping = (bool)this.Shippingg_checkbox.IsChecked;
                acc.Password = this.Password.Text;
                acc.Import = (bool)this.Import_checkbox.IsChecked;

                using (var db = new InputContext())
                {
                    if (!db.accounts.Any(x => x.ID == acc.ID))
                    {
                        db.accounts.Add(acc);
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Account is already created");
                    }
                    db.SaveChanges();
                    db.Dispose();
                    UpdateData();
                }
            }
            catch (SqlException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void Delete_btn_Click(object sender, RoutedEventArgs e)
        {
            var acc = new Account() { ID = this.ID_txt.Text };
            var db = new InputContext();
            db.accounts.Attach(acc);
            db.accounts.Remove(acc);
            db.SaveChanges();
            db.Dispose();
            UpdateData();
        }


    }
}
