using SATO.MLComponent;
using SATO.MLPreviewComponent;
using SATO.MLV5;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace GPMI_Laser_Marking
{
    public class SaToPrint
    {
        MLComponent MLComponent;
        public SaToPrint()
        {
            MLComponent = new MLComponent();
          
            //PrintLabel("SATO CT4-LX 305dpi (Copy 1)");
            
        }

        public bool PrintLabel(Dictionary<string, string> printvalue, string printerDriver)
        {
            try
            {
                int Result = 0;
                String filePath = Environment.CurrentDirectory + "\\Layout.mllayx";
                //filePath = "C:\\Users\\Public\\Documents\\SATO\\MLV5\\Layout.mllayx";
                MLComponent.LayoutFile = filePath;
                if (!File.Exists(printvalue["varPath"]))
                {
                    printvalue["varPath"] = Environment.CurrentDirectory + "\\temas - logo.PNG";
                    MessageBox.Show(printvalue["varPath"]);
                }
                
                Result = MLComponent.SetPrnDataField("varPath", printvalue["varPath"]);
                Result = MLComponent.SetPrnDataField("partnumber", printvalue["partnumber"]);
                Result = MLComponent.SetPrnDataField("partname", printvalue["partname"]);
                Result = MLComponent.SetPrnDataField("orderno", printvalue["orderno"]);
                Result = MLComponent.SetPrnDataField("revision", printvalue["revision"]);
                Result = MLComponent.SetPrnDataField("boxno", printvalue["boxno"]);
                Result = MLComponent.SetPrnDataField("quantity", printvalue["quantity"]);
                Result = MLComponent.SetPrnDataField("inspector", printvalue["inspector"]);
                Result = MLComponent.SetPrnDataField("qrcodeid", printvalue["qrcodeid"]);
                Result = MLComponent.SetPrnDataField("qrcodefix", printvalue["qrcodefix"]);
                Result = MLComponent.SetPrnDataField("pdate", DateTime.Now.ToShortDateString());
                MLComponent.SetPrnDataField("Print quantity", "1");
                MLComponent.Setting = "DRV:" + printerDriver;
                //   MLComponent.Protocol = 0; // Protocols.Status3;
                //     MLComponent.Timeout = 3;
                //            System.Threading.Thread.Sleep(500);

                Result = MLComponent.OpenPort(1);
                
                //Application.DoEvents();
                if (Result != 0)
                {
                    //txtBarcode.SelectAll();
                    //txtBarcode.Focus();
                    return false;
                }


                Result = MLComponent.Output();
                //string test = "";
                // MLComponent.GetStatus(ref test);

                //   Application.DoEvents();
                if (Result != 0)
                {

                    //txtBarcode.SelectAll();
                    //txtBarcode.Focus();

                    return false;
                }


                //Cho phep cut
                // MLComponent.Cut();
                Result = MLComponent.ClosePort();
                if (Result != 0)
                {

                    //txtBarcode.SelectAll();
                    //txtBarcode.Focus();
                    return false;
                }

            }
            catch (MLComponentException EX)
            {
                System.Windows.Forms.MessageBox.Show(EX.Message);
            }
            return true;
        }

    }
}
