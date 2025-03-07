using GPMI_Laser_Marking.Models;
using SuperSimpleTcp;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.IO.Ports;
using System.Linq;
using System.Text;

//using static SATO.MLV5.Common.Error.ErrorItemFactory.Code.Print.HeaderTail;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace GPMI_Laser_Marking
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///
    public partial class MainWindow : Window
    {
        public ObservableCollection<OutputMarking> Outputlist = new ObservableCollection<OutputMarking>();
        private bool _isopen = false;
        private int OK;
        private int NG;
        private string Part_ID;
        private string Shift;
        private readonly int startTime = 8;
        private readonly int stoptTime = 20;

        //string LaserIP = "127.0.0.2:2500";
        //string BarcodeIP = "127.0.0.3:27110";
        //string JIG_BarcodeIP = "127.0.0.4:27110";
        private SerialPort serialPortPLC;

        private SimpleTcpClient BarcodeConnect; // tạo kết nối tcpip với barcode
        private SimpleTcpClient LaserConnect;// tạo kết nối tcpip với Máy khắc
        private SimpleTcpClient JIGBarcodeConnect; // tạo kết nối tcpip với barcode JIG
        private SimpleTcpClientEvents LaserEvents;
        private SimpleTcpClientEvents BarcodeEvents;
        private SimpleTcpClientEvents JIGBarcodeEvents;

        public MainWindow(string ID, string fullname)
        {
            Application.Current.MainWindow.Hide();
            InitializeComponent();
            ID_tbx.Text = ID;
            Name_tbx.Text = fullname;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Output_Grid.ItemsSource = Outputlist;
            PLCConnection();
            InitializeJIGBarcodeConnection();
            InitializeBarcodeConnection();
            InitializeLaserConnection();
            UpdateView();
        }

        private void InitializeJIGBarcodeConnection()
        {
            JIGBarcodeEvents = new SimpleTcpClientEvents();
            JIGBarcodeEvents.DataReceived += Events_DataReceivedJIG; ;
            JIGBarcodeEvents.Connected += Events_ConnectedJIG;
            JIGBarcodeEvents.Disconnected += Events_DisconnectedJIG;
            if (Properties.Settings.Default.JIG_BarcodeIP != string.Empty)
            {
                JIGBarcodeConnect = new SimpleTcpClient(Properties.Settings.Default.JIG_BarcodeIP)
                {
                    Events = JIGBarcodeEvents
                };

                try
                {
                    JIGBarcodeConnect.Connect();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex} JIG Check Bacode Connect Fail!");
                }
            }
            else
            {
                MessageBox.Show("JIG Check Bacode Connect Fail!");
            }
        }

        private void Events_DisconnectedJIG(object sender, ConnectionEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                JIG_Barcode_Connection_Status.Fill = Brushes.Black;
            });
        }

        private void Events_ConnectedJIG(object sender, ConnectionEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                JIG_Barcode_Connection_Status.Fill = Brushes.GreenYellow;
            });
        }

        private void Events_DataReceivedJIG(object sender, DataReceivedEventArgs e)
        {
            if (e.Data.Array != null)
            {
                var reciev = Encoding.UTF8.GetString(e.Data.Array, 0, e.Data.Count).Trim('\r').Trim('\n');
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    Read_PartNumber_txt.Text = reciev;
                    Read_PartNumber_txt_KeyDown(null, null);
                }));

                //Read_PartNumber_txt.Text = e.Data.ToString();
            }
        }

        #region PLC Connect

        private void PLCConnection()
        {
            if (serialPortPLC == null)
            {
                serialPortPLC = new SerialPort();
            }

            if (!serialPortPLC.IsOpen)
            {
                if (Properties.Settings.Default.PLCCOM != string.Empty)
                {
                    serialPortPLC = new SerialPort(Properties.Settings.Default.PLCCOM, 9600);
                    serialPortPLC.ReadTimeout = 1500;
                    serialPortPLC.WriteTimeout = 1500;
                    serialPortPLC.DataReceived += SerialPortPLC_DataReceived;
                    try
                    {
                        serialPortPLC.Open();
                        PLC_Connection_Status.Fill = Brushes.YellowGreen;
                    }
                    catch (Exception ex)
                    {
                        PLC_Connection_Status.Fill = Brushes.Black;
                        MessageBox.Show($"{ex} \r\nPLC Connect Fail!");
                    }
                }
                else
                {
                    MessageBox.Show($"\r\nPLC Connect Fail!");
                    PLC_Connection_Status.Fill = Brushes.Black;
                }
            }
            else
            {
                MessageBox.Show($" {Properties.Settings.Default.PLCCOM}\r\nPLC COM is Open!");
            }
        }

        private void SerialPortPLC_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var aa = StringToByte(serialPortPLC.ReadExisting());
            if (aa[0] == 01)
            {
                JIGBarcodeConnect.Send("g\r");
            }
            //trigger check barcode_check jig
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

        private void Reconnect_PLC_btn_Click(object sender, RoutedEventArgs e)
        {
            PLCConnection();
        }

        #endregion PLC Connect

        #region LaserConnect

        private void InitializeLaserConnection()
        {
            LaserEvents = new SimpleTcpClientEvents();
            LaserEvents.DataReceived += Events_DataReceived_Laser; ;
            LaserEvents.Connected += Events_Connected_Laser;
            LaserEvents.Disconnected += Events_Disconnected_Laser;
            if (Properties.Settings.Default.LaserIP != string.Empty)
            {
                LaserConnect = new SimpleTcpClient(Properties.Settings.Default.LaserIP) { Events = LaserEvents };

                try
                {
                    LaserConnect.Connect();
                }
                catch (Exception)
                {
                    MessageBox.Show("Laser Marking Connect Fail!");
                }
            }
            else
            {
                MessageBox.Show("Laser Marking Connect Fail!");
            }
        }

        private void Events_Disconnected_Laser(object sender, ConnectionEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Laser_Connection_Status.Fill = Brushes.Black;
            });
        }

        private void Events_Connected_Laser(object sender, ConnectionEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Laser_Connection_Status.Fill = Brushes.GreenYellow;
            });
        }

        private string laserDataReceived = string.Empty;

        private void Events_DataReceived_Laser(object sender, DataReceivedEventArgs e)
        {
            if (e.Data.Array != null)
            {
                laserDataReceived = Encoding.UTF8.GetString(e.Data.Array, 0, e.Data.Count);
            }
        }

        #endregion LaserConnect

        #region BarcodeConnect

        private void InitializeBarcodeConnection()
        {
            BarcodeEvents = new SimpleTcpClientEvents();
            BarcodeEvents.DataReceived += Events_DataReceived_Barcode;
            BarcodeEvents.Connected += Events_Connected_Barcode;
            BarcodeEvents.Disconnected += Events_Disconnected_Barcode;
            if (Properties.Settings.Default.BarcodeIP != string.Empty)
            {
                BarcodeConnect = new SimpleTcpClient(Properties.Settings.Default.BarcodeIP)
                {
                    Events = BarcodeEvents
                };
                try
                {
                    BarcodeConnect.Connect();
                }
                catch (Exception)
                {
                    MessageBox.Show("Barcode Connect Fail!");
                }
            }
            else
            {
                MessageBox.Show("Barcode Connect Fail!");
            }
        }

        private void Events_Disconnected_Barcode(object sender, ConnectionEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Barcode_Connection_Status.Fill = Brushes.Black;
            });
        }

        private void Events_Connected_Barcode(object sender, ConnectionEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Barcode_Connection_Status.Fill = Brushes.GreenYellow;
            });
        }

        private string barcodeDataReceived = string.Empty;

        private void Events_DataReceived_Barcode(object sender, DataReceivedEventArgs e)
        {
            if (e.Data.Array != null)
            {
                barcodeDataReceived = Encoding.UTF8.GetString(e.Data.Array, 0, e.Data.Count);
            }
        }

        #endregion BarcodeConnect

        private void UpdateView()
        {
            _isopen = true;
            Task.Run(() =>
            {
                while (_isopen)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        PartID_txt.Text = Part_ID;
                        if (laserDataReceived.Contains("GO M"))
                        {
                            Laserimage_status_img.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            Laserimage_status_img.Visibility = Visibility.Hidden;
                        }
                    });
                    Thread.Sleep(300);
                    if (!_isopen)
                    {
                        break;
                    }
                }
            });
        }

        private void Read_PartNumber_txt_GotFocus(object sender, RoutedEventArgs e)
        {
            Read_PartNumber_txt.Text = "";
        }

        private void ResetError_btn_Click(object sender, RoutedEventArgs e)
        {
            if (LaserConnect.IsConnected)
            {
                LaserConnect.Send("AD \r\n");
            }
        }

        private void Shutdown_Btn_Click(object sender, RoutedEventArgs e)
        {
            //App.Current.Shutdown();
            this.Close();
        }

        private void Miximize_Btn_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private Regex alphanumericRegex = new Regex("^[a-zA-Z0-9]*$");

        private void Read_PartNumber_txt_KeyDown(object sender, KeyEventArgs e)
        {
            bool check = false;
            if (e == null)
            {
                check = true;
            }
            else if (e.Key == Key.Return)
            {
                check = true;
            }
            if (check)
            {
                try
                {
                    using (InputContext db = new InputContext())
                    {
                        var code = Read_PartNumber_txt.Text.Split('*');
                        string PartNumber = code[0];

                        PartNumber = PartNumber.Trim('\r').Trim('\n');
                        var input = db.orderInputs.FirstOrDefault(p => p.Part_Number.Equals(PartNumber));
                        if (input != null)
                        {
                            byte[] myBytes = new byte[] { (byte)4, (byte)13, (byte)10 };
                            serialPortPLC.Write(myBytes, 0, 1);
                            Read_PartNumber_txt.IsEnabled = false;
                            if (DateTime.Now.Hour >= startTime && DateTime.Now.Hour < stoptTime)
                            { Shift = "D"; }
                            else { Shift = "N"; }
                            var sum = db.orderInputs
                                .Where(x => x.Part_Number == input.Part_Number)
                                .Sum(x => (x.Order_Quantity));
                            partNumber = input.Part_Number;
                            PartName_tbx.Text = input.Part_Name;
                            Version_Tbx.Text = input.Version;
                            Vendor_Code_tbx.Text = input.Vendor_Code;
                            OrderDate_tbx.Text = input.Order_Date.ToString("d");
                            Quantity_tbx.Text = sum.ToString();
                            OK = db.outputMarkings.Count(t => t.Part_Number == input.Part_Number);
                            NG = db.nGMarkings.Count(t => t.Part_Number == PartNumber);
                            var FQANG = db.outputMarkings.Count(t => t.FQA_Status == "NG" && t.Part_Number == input.Part_Number);
                            FNG_txt.Text = FQANG.ToString();
                            UpdatePartID();
                            if (OK < sum + FQANG)
                            {
                                LaserMarking();
                            }
                            else
                            {
                                System.Windows.Forms.MessageBox.Show("Laser Marking quantity is enough !", "Error !");
                                ResetPartNumber();
                            }
                        }
                        else
                        {
                            Thread.Sleep(500);
                            var myBytes = new byte[] { (byte)1, (byte)13, (byte)10 };
                            try
                            {
                                serialPortPLC.Write(myBytes, 0, 1);
                            }
                            catch (Exception) { }
                            ResetPartNumber();
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
            }
        }

        private void UpdatePartID()
        {
            OK_txt.Text = OK.ToString();
            NG_txt.Text = NG.ToString();
            Part_ID = partNumber + $"{Version_Tbx.Text:00}" + Vendor_Code_tbx.Text + DateTime.Now.ToString("yyMMdd") + Shift + $"{OK:00000}";
        }

        private void LaserTest_btn_Click(object sender, RoutedEventArgs e)
        {
            if (LaserConnect.IsConnected)
            {
                LaserConnect.Send($"VS 0 \"test\r\n");
                Thread.Sleep(10);
                LaserConnect.Send("LD \"MarkingFile1.t2l\" 1 N\r\n");// chọn file khắc
                Thread.Sleep(10);
                LaserConnect.Send("GO\r\n");
            }
        }

        private void NG_Comfrim_btn_Click(object sender, RoutedEventArgs e)
        {
            CompareNG();
        }

        private async void Read_Barcode_Again_btn_Click(object sender, RoutedEventArgs e)
        {
            NG_Comfrim_btn.Visibility = Visibility.Hidden;
            bool check = false;
            var task = Task.Run(() => { check = WaitBarcodecmd(); });
            await task;
            if (!check) CompareOKAsync();
            NG_Comfrim_btn.Visibility = Visibility.Visible;
        }

        private void Reconnect_Barcode_btn_Click(object sender, RoutedEventArgs e)
        {
            if (BarcodeConnect == null)
            {
                return;
            }
            if (!BarcodeConnect.IsConnected)
            {
                try
                {
                    BarcodeConnect = new SimpleTcpClient(Properties.Settings.Default.BarcodeIP) { Events = BarcodeEvents };
                    BarcodeConnect.Connect();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Connect Fail \r\n!" + ex.Message);
                }
            }
            else
            {
                BarcodeConnect.Disconnect();
            }
        }

        private void Reconnect_Laser_btn_Click(object sender, RoutedEventArgs e)
        {
            if (LaserConnect == null)
            {
                return;
            }
            if (!LaserConnect.IsConnected)
            {
                try
                {
                    LaserConnect = new SimpleTcpClient(Properties.Settings.Default.LaserIP) { Events = LaserEvents };
                    LaserConnect.Connect();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Connect Fail! \r\n" + ex.Message);
                }
            }
        }

        private void Reconnect_JIGBarcode_btn_Click(object sender, RoutedEventArgs e)
        {
            if (BarcodeConnect == null)
            {
                return;
            }
            if (!JIGBarcodeConnect.IsConnected)
            {
                try
                {
                    JIGBarcodeConnect = new SimpleTcpClient(Properties.Settings.Default.JIG_BarcodeIP)
                    {
                        Events = JIGBarcodeEvents
                    };
                    JIGBarcodeConnect.Connect();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Connect Fail \r\n!" + ex.Message);
                }
            }
            else
            {
                JIGBarcodeConnect.Disconnect();
            }
        }

        //process-------------------------------------------------------------------------------
        private async void LaserMarking()
        {
            if (!LaserConnect.IsConnected)
            {
                return;
            }
            bool checkerror = false;
            bool endstep = false;
            await Task.Run(() =>
            {
                var watch = DateTime.UtcNow;
                LaserConnect.Send($"VS 0 \"{this.Part_ID}\"\r\n"); // input value khắc
                if (WaitLasercmd("VS 1", watch, 1000))
                {
                    return; // time out nhận value
                }
                else
                {
                    Thread.Sleep(20);
                    LaserConnect.Send("LD \"MarkingFile1.t2l\" 1 N\r\n");// chọn file khắc
                    if (WaitLasercmd("LD 1", watch, 1000))
                    {
                        return; // time out nhận value
                    }
                    else
                    {
                        Thread.Sleep(20);
                        LaserConnect.Send("GO\r\n"); // bắt đầu
                        if (WaitLasercmd("GO F", watch, 14000))
                        {
                            return; // time out nhận value hoàn tất
                        }
                        else
                        {
                            checkerror = WaitBarcodecmd();
                            endstep = true;
                        }
                    }
                }
                // wait laser finish
                // Read Barcode//
                //wait read
                //compare barcode//
                //if OK ->CompareOK()
                //else NG ->
            });
            if (!endstep)
            {
                MessageBox.Show($"Dont Finish");
                return;
            }
            if (checkerror)
            {
                ReadbarcodeFail();
                byte[] myBytes = new byte[] { (byte)7, (byte)13, (byte)10 };
                try
                {
                    serialPortPLC.Write(myBytes, 0, 1);
                }
                catch (Exception) { }
            }
            else
            {
                CompareOKAsync();
            }
        }

        private void ReadbarcodeFail()
        {
            NG_Comfrim_grid.Visibility = Visibility.Visible;
            Status_Border.Background = Brushes.Red;
            Status_tbx.Text = "NG";
        }

        private bool WaitLasercmd(string cmd, DateTime starttime, int waittime)
        {
            while (!laserDataReceived.Trim().Contains(cmd))
            {
                if ((DateTime.UtcNow - starttime).TotalMilliseconds > waittime)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        workingstatuslbl.Text = $"Send to Laser Machine Timeout - {(DateTime.UtcNow - starttime).TotalMilliseconds} {cmd}";
                    });
                    return true;
                }
            }
            return false;
        }

        private bool WaitBarcodecmd()
        {
            if (BarcodeConnect.IsConnected)
            {
                var starttime = DateTime.UtcNow;
                BarcodeConnect.Send("g\r"); // đọc barcode
                while (!Part_ID.Equals(barcodeDataReceived.Replace("\n", "").Replace("\r", "").Trim()))
                {
                    if ((DateTime.UtcNow - starttime).TotalMilliseconds > 3000)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            workingstatuslbl.Text = "Barcode incorrect :" + barcodeDataReceived;
                        });

                        return true;
                    }
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        workingstatuslbl.Text = "Wait Read Barcode :" + (3000 - (DateTime.UtcNow - starttime).TotalMilliseconds).ToString();
                    });
                    Thread.Sleep(50);
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Barcode is Disconnected");
                return true;
            }

            return false;
        }

        private async void CompareOKAsync()
        {
            var code = Read_PartNumber_txt.Text.Split('*');
            string CNCMachine = string.Empty;
            if (code.Length>1) { CNCMachine = code[1];}
            try
            {
                OutputMarking outputMarking = new OutputMarking()
                {
                    Date_Time_Marking = DateTime.Now,
                    Station = Station_tbx.Text,
                    Part_Number = partNumber,
                    Part_ID = this.Part_ID,
                    Part_Name = PartName_tbx.Text,
                    Marking_Account = App.Account.ID,
                    CNCMachine = CNCMachine
                };

                using (InputContext db = new InputContext())
                {
                    db.outputMarkings.Add(outputMarking);
                    db.SaveChanges();
                }
                OK++;
                Outputlist.Add(outputMarking);
                NG_Comfrim_grid.Visibility = Visibility.Hidden;
                Status_Border.Background = Brushes.Green;
                Status_tbx.Text = "OK";
                UpdatePartID();
                ResetPartNumber();
                await Task.Run(() =>
                {
                    Thread.Sleep(2000);
                });
                Status_Border.Background = Brushes.Transparent;
                Status_tbx.Text = "---";

                // send OK to PLC
                byte[] myBytes = new byte[] { (byte)2, (byte)13, (byte)10 };
                try
                {
                    serialPortPLC.Write(myBytes, 0, 1);
                }
                catch (Exception) { }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ResetPartNumber()
        {
            Read_PartNumber_txt.IsEnabled = true;
            Read_PartNumber_txt.Text = "";
            Read_PartNumber_txt.Focus();
        }

        private string partNumber = string.Empty;

        private void CompareNG()
        {
            try
            {
                NGMarking outputMarking = new NGMarking()
                {
                    Part_ID = this.Part_ID,
                    Marking_Status = "NG",
                    Date_Time_Marking = DateTime.Now,
                    Station = Station_tbx.Text,
                    Part_Number = partNumber,
                    Marking_Account = App.Account.ID
                };

                using (InputContext db = new InputContext())
                {
                    db.nGMarkings.Add(outputMarking);
                    db.SaveChanges();
                }
                NG++;
                Status_Border.Background = Brushes.Transparent;
                Status_tbx.Text = "---";
                NG_Comfrim_grid.Visibility = Visibility.Hidden;
                ResetPartNumber();
                // send NG to P,jmLC
                byte[] myBytes = new byte[] { (byte)7, (byte)13, (byte)10 };
                try
                {
                    serialPortPLC.Write(myBytes, 0, 1);
                }
                catch (Exception) { }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _isopen = false;
            serialPortPLC?.Dispose();
            BarcodeConnect?.Dispose();
            LaserConnect?.Dispose();
            JIGBarcodeConnect?.Dispose();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (BarcodeConnect.IsConnected)
            {
                BarcodeConnect?.Dispose();
            }
            Application.Current.MainWindow.Show();
        }

        private void Errorlist_btn_Click(object sender, RoutedEventArgs e)
        {
            NGListxaml nGListxaml = new NGListxaml();
            nGListxaml.ShowDialog();
        }

        private void ManualTest_btn_Click(object sender, RoutedEventArgs e)
        {
            var watch = DateTime.UtcNow;
            try
            {
                LaserConnect.Send($"VS 0 \"{this.PartID_Manual_txt.Text}\"\r\n"); // input value khắc
                if (WaitLasercmd("VS 1", watch, 10000))
                {
                    return; // time out nhận value
                }
                else
                {
                    Thread.Sleep(20);
                    LaserConnect.Send("LD \"MarkingFile1.t2l\" 1 N\r\n");// chọn file khắc
                    if (WaitLasercmd("LD 1", watch, 10000))
                    {
                        return; // time out nhận value
                    }
                    else
                    {
                        Thread.Sleep(20);
                        LaserConnect.Send("GO\r\n"); // bắt đầu
                        if (WaitLasercmd("GO F", watch, 15000))
                        {
                            return;
                        }
                    }
                }            
            }
            catch (Exception)
            {
            }
        }

        private void Setting_btn_Click(object sender, RoutedEventArgs e)
        {
            var config = new SetupIP();
            config.ShowDialog();
        }

        ~MainWindow()
        {
            _isopen = false;
        }

        private void Output_Grid_LoadingRow(object sender, System.Windows.Controls.DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
    }
}