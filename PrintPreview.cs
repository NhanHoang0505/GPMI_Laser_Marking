using SATO.MLComponent;
using SATO.MLPreviewComponent;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace GPMI_Laser_Marking
{
    public partial class PrintPreview : Form
    {
        private MLPreviewComponentWIN mlPreviewComponentWIN2;
        Dictionary<string, string> printvalue;
        public PrintPreview(Dictionary<string, string> dic)
        {
            InitializeComponent();
            printvalue =dic;

        }
        public void PreivewData()
        {
                int Result = 0;
                String filePath = Environment.CurrentDirectory + "\\Layout.mllayx";
                //filePath = "C:\\Users\\Public\\Documents\\SATO\\MLV5\\Layout.mllayx";
                mlPreviewComponentWIN2.LayoutFile = filePath;
                mlPreviewComponentWIN2.PrnDataType = SATO.MLPreviewComponent.PrnDataTypes.Prn;
                mlPreviewComponentWIN2.Setting = "DSP:";
            mlPreviewComponentWIN2.PrinterCaption = "SATO CL4NX 305dpi";
            //printvalue["varPath"] =  "temas-logo.png";
            if (!File.Exists(printvalue["varPath"]))
                {
                    printvalue["varPath"] = "temas-logo.png";
                MessageBox.Show(printvalue["varPath"]);
                }
            

                Result = mlPreviewComponentWIN2.SetPrnDataField("varPath", printvalue["varPath"]);
                Result = mlPreviewComponentWIN2.SetPrnDataField("partnumber", printvalue["partnumber"]);
                Result = mlPreviewComponentWIN2.SetPrnDataField("partname", printvalue["partname"].Replace(" ", "_"));
                Result = mlPreviewComponentWIN2.SetPrnDataField("orderno", printvalue["orderno"].Replace(" ", "_"));
                Result = mlPreviewComponentWIN2.SetPrnDataField("revision", printvalue["revision"]);
                Result = mlPreviewComponentWIN2.SetPrnDataField("boxno", printvalue["boxno"].Replace(" ", "_"));
                Result = mlPreviewComponentWIN2.SetPrnDataField("quantity", printvalue["quantity"].Replace(" ", "_"));
                Result = mlPreviewComponentWIN2.SetPrnDataField("inspector", printvalue["inspector"].Replace(" ", "_"));
                Result = mlPreviewComponentWIN2.SetPrnDataField("qrcodeid", printvalue["qrcodeid"].Replace(" ", "_"));
                Result = mlPreviewComponentWIN2.SetPrnDataField("qrcodefix", printvalue["qrcodefix"].Replace(" ", "_"));
                Result = mlPreviewComponentWIN2.SetPrnDataField("pdate", DateTime.Now.ToShortDateString());
                mlPreviewComponentWIN2.SetPrnDataField("Print quantity", "1");
                if (Result != 0)
                {
                    MessageBox.Show("Value print error");
                
                }
                Result = mlPreviewComponentWIN2.Output();
                if (Result != 0)
                {
                    MessageBox.Show("Value print error");
                }
        }
        public void AddPreviewControl()
        {
            this.mlPreviewComponentWIN2 = new SATO.MLPreviewComponent.MLPreviewComponentWIN();

            // 
            // mlPreviewComponentWIN2
            // 
            this.mlPreviewComponentWIN2.Alignment = SATO.MLPreviewComponent.AlignmentSettings.mlprvAlignCenterCenter;
            this.mlPreviewComponentWIN2.HeaderTailSetting = false;
            this.mlPreviewComponentWIN2.LayoutFile = "Default.mllayx";
            this.mlPreviewComponentWIN2.LayoutNameCaption = "";

            //Chỉnh location ở đây
            this.mlPreviewComponentWIN2.Location = new System.Drawing.Point(0, 0);
            this.mlPreviewComponentWIN2.MountBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.mlPreviewComponentWIN2.MountColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.mlPreviewComponentWIN2.Name = "mlPreviewComponentWIN2";
            this.mlPreviewComponentWIN2.Page = 1;
            this.mlPreviewComponentWIN2.PrintAreaBorder = true;
            this.mlPreviewComponentWIN2.PrinterCaption = "";
            this.mlPreviewComponentWIN2.PrnData = "";
            this.mlPreviewComponentWIN2.PrnDataType = SATO.MLPreviewComponent.PrnDataTypes.Tsv;
            this.mlPreviewComponentWIN2.Rotation = SATO.MLPreviewComponent.RotationSettings.mlprvRotation90;
            this.mlPreviewComponentWIN2.Setting = "DSP:";

            //Chỉnh size ở đây
            this.mlPreviewComponentWIN2.Size = new System.Drawing.Size(this.panel1.Width, this.panel1.Height);
            this.mlPreviewComponentWIN2.Stretch = true;
            this.mlPreviewComponentWIN2.TabIndex = 1;
            this.mlPreviewComponentWIN2.TaxRate = "";
            this.mlPreviewComponentWIN2.TotalQtyCaption = 0;
            this.mlPreviewComponentWIN2.Zoom = 100D;
            //Mình dùng theo phương pháp này có OK KHônG???
            this.panel1.Controls.Add(this.mlPreviewComponentWIN2);
     
             }

        private void PrintPreview_Load(object sender, EventArgs e)
        {
            AddPreviewControl();
            this.SuspendLayout();
            valuepanel.Hide();
            PreivewData();
            this.mlPreviewComponentWIN2.Size = new System.Drawing.Size(this.panel1.Width, this.panel1.Height);
            this.SuspendLayout();
            this.Refresh();
        }

        

        private void okbtn_Click(object sender, EventArgs e)
        {
            var db = new InputContext();
            var test = printvalue["partnumber"];
            var value = (db.outputMarkings.
                 Where(qrcode=> (qrcode.QR_CodeID == qrcodetxt.Text && qrcode.Part_Number == test))).FirstOrDefault();
            if (value != null )
            {
                printvalue["qrcodeid"] = value.QR_CodeID;
                printvalue["qrcodefix"] = value.QR_CodeID;
                printvalue["boxno"] = value.Box_No.ToString();
                printvalue["quantity"] = value.Pcs_in_Box.ToString();
                PreivewData();
            }
            else
            {
                MessageBox.Show(qrcodetxt.Text +" is incorect !" );
            }
            
            valuepanel.Hide();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void editvaluebtn_Click(object sender, EventArgs e)
        {
            valuepanel.Show();
        }

        private void cancel_btn_Click(object sender, EventArgs e)
        {
            valuepanel.Hide();
        }

        private void close_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private SaToPrint print = new SaToPrint();
        private void print_btn_Click(object sender, EventArgs e)
        {
            print.PrintLabel(printvalue, Properties.Settings.Default.printter);
        }
    }
}
