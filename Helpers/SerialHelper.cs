using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMI_Laser_Marking.Helpers
{  
    public class SerialHelper
    {
        public event EventHandler PLC_Triger;
        private SerialPort serialPort;
 
        public SerialHelper()
        {
            serialPort = new SerialPort("COM5", 9600);
            serialPort.Parity = Parity.Even;
            serialPort.DataReceived += SerialPort_DataReceived;
            serialPort.Open();
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                var b = StringToByte(serialPort.ReadExisting());
                PLC_Triger?.Invoke(null,null);
            }
            catch (Exception)
            {

            }
        }

        public string WriteHexstring( string stringValue)
        {
            byte[] byteSend = HexstringTobyte(stringValue);

            string dataSend = string.Empty;
            try
            {
                serialPort.Write(byteSend, 0, byteSend.Length);
                dataSend = BytesToString(byteSend);
            }
            catch (Exception)
            {
                dataSend = "Time out";
            }
            return dataSend;
        }
        private static byte[] HexstringTobyte(string hex)
        {
            byte[] raw;

            hex = hex.Replace(" ", "");
            raw = new byte[hex.Length / 2];
            for (int i = 0; i < raw.Length; i++)
            {
                try
                {
                    raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
                }
                catch (Exception)
                {
                }

            }
            return raw;

        }
        private string BytesToString(byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes))
            {
                using (var streamReader = new StreamReader(stream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }
        private byte[] StringToByte(string dataSend)
        {
            byte[] value = new byte[dataSend.Length];
            for (int i = 0; i < dataSend.Length; i++)
            {
                value[i] = (byte)dataSend[i];
            }
            return value;
        }
    }
}
