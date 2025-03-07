using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GPMI_Laser_Marking
{
    public partial class SetupIP : Form
    {
        public SetupIP()
        {
            InitializeComponent();
            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {
                PLC_Combobox.Items.Add(s);
            }
            if (PLC_Combobox.SelectedItem == null)
            {
                PLC_Combobox.SelectedItem = "COM4";
            }
            
        }
        public bool Check { get;private set; } =false;
        private void SetupIP_Load(object sender, EventArgs e)
        {
            try
            {
                LSIP.Text = Properties.Settings.Default.LaserIP.Split(':')[0];
                if (Properties.Settings.Default.LaserIP == "")
                {
                    LSPort.Text = "2500";
                }
                else
                {
                    LSPort.Text = Properties.Settings.Default.LaserIP.Split(':')[1];
                }
                
                BIP.Text = Properties.Settings.Default.BarcodeIP.Split(':')[0];
                if (Properties.Settings.Default.BarcodeIP == "")
                {
                    PPort.Text = "27110";
                }
                else
                {
                    PPort.Text = Properties.Settings.Default.BarcodeIP.Split(':')[1];
                }
                


                BarcodeIP_Beforetxt.Text = Properties.Settings.Default.JIG_BarcodeIP.Split(':')[0];
                if (Properties.Settings.Default.JIG_BarcodeIP == "")
                {
                    BarcodePort_Befor_txt.Text = "27110";
                }
                else
                {
                    BarcodePort_Befor_txt.Text = Properties.Settings.Default.JIG_BarcodeIP.Split(':')[1];
                }


                if (PLC_Combobox.Text=="")
                {
                    PLC_Combobox.SelectedItem = Properties.Settings.Default.PLCCOM;
                }
            }
            catch (Exception ex )
            {

                MessageBox.Show($"{ex} \r\nCheck IP");
            }

        }

        private void Savebtn_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.BarcodeIP = BIP.Text + ":" + PPort.Text;
            if (PPort.Text == string.Empty || BIP.Text == string.Empty)
            {
                Properties.Settings.Default.BarcodeIP = "127.0.0.3:27110";
            }

            Properties.Settings.Default.LaserIP = LSIP.Text + ":" + LSPort.Text;
            if (LSIP.Text == string.Empty || LSPort.Text == string.Empty)
            {
                Properties.Settings.Default.LaserIP = "127.0.0.2:2500";
            }

            
            
            if (PLC_Combobox.SelectedItem!= null)
            {
                Properties.Settings.Default.PLCCOM = PLC_Combobox.SelectedItem.ToString();
            }

            Properties.Settings.Default.JIG_BarcodeIP = BarcodeIP_Beforetxt.Text + ":" + BarcodePort_Befor_txt.Text;

            if (BarcodeIP_Beforetxt.Text == string.Empty || BarcodePort_Befor_txt.Text == string.Empty)
            {
                Properties.Settings.Default.JIG_BarcodeIP = "127.0.0.4:27110";
            }
            
            
            
            Properties.Settings.Default.Save();
            Check = true;
            this.Close();
        }
    }
}
