using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Series = System.Windows.Forms.DataVisualization.Charting.Series;

namespace TrafficSurveyTool
{
    public partial class MainWindow : Form
    {
        //Recoed Receive data
        private Queue<double> dataQueueY = new Queue<double>(400);//274
        private Queue<double> dataQueueX = new Queue<double>(400);//274
        string splitStr = "AA 01 80 01 ";//921,951对应  BE A0 12 34 
        int lasetAngle = 96;//921 96度   951 108度
        int laserPointCount = 274;//921 274个点  951 400个点
        public MainWindow()
        {
            InitializeComponent();
            this.Text = "捷崇科技 " + Application.ProductName + " V" + Application.ProductVersion;
            cbType.SelectedIndex = 0;
            //cbMode.SelectedIndex = 0;
            //cbProtocol.SelectedIndex = 1;
            //combSelectLaser.SelectedIndex = 0;
            RefreshPort();
            toolStripStatusLabel1.Text = "The author of the software is Jackey";
            //dataGridView1.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (null != getRecevice)
                btnStop.PerformClick();
            System.Environment.Exit(0);
        }


        #region 日志记录、支持其他线程访问  
        private void updaRichBox(string data)
        {
            rbDataLog.Invoke(new Action<string>((str) =>
            {
                if (rbDataLog.Text.Length > 1 * 1024 * 1024)//1M 就clear richbox
                    rbDataLog.Text = "";

                rbDataLog.AppendText(DateTime.Now + "   " + str);
            }), data);
        }

        public delegate void LogAppendDelegate(Color color, string text);
        public void LogAppendMethod(Color color, string text)
        {
            //if (!rbDataLog.ReadOnly)
            //    rbDataLog.ReadOnly = true;
            //this.rbDataLog.Select(this.rbDataLog.Text.Length, 0);
            //this.rbDataLog.Focus();
            //rbDataLog.SelectionColor = color;
            //rbDataLog.AppendText(text + "\n");
        }
        public void LogError(string text)
        {
            LogAppendDelegate la = new LogAppendDelegate(LogAppendMethod);
            rbDataLog.Invoke(la, Color.Red, DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] ") + text);
        }
        public void LogWarning(string text)
        {
            LogAppendDelegate la = new LogAppendDelegate(LogAppendMethod);
            rbDataLog.Invoke(la, Color.Gold, DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] ") + text);
        }
        public void LogMessage(string text)
        {
            LogAppendDelegate la = new LogAppendDelegate(LogAppendMethod);
            rbDataLog.Invoke(la, Color.Black, DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] ") + text);
        }

        #endregion

        #region 串口通讯UDP
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshPort();
        }

        private static SerialPort sp = new SerialPort();
        private void initPortPara()
        {
            //sp.PortName = SelectedComPort;
            sp.BaudRate = 460800;
            sp.DataBits = 8;
            sp.Parity = Parity.None;
            sp.StopBits = StopBits.One;
        }
        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (sp.IsOpen != true)
                {
                    sp.PortName = cbSerialPortName.SelectedItem.ToString();
                    initPortPara();
                    if (OpenDevice(sp.PortName, sp.BaudRate, sp.DataBits, sp.Parity, sp.StopBits))
                    {
                        //ConnectTest按钮
                        toolStripStatusLabel1.Text = "Connect " + cbSerialPortName.Text + " Success";

                        btnConnect.FillColor = Color.YellowGreen;
                        btnConnect.Invoke(new Action<string>((str) => { btnConnect.Text = str; }), "连接成功");
                        //recevie setting
                        //bAccpet = true;
                        //getRecevice = new Thread(new ThreadStart(testDelegate));
                        //getRecevice.IsBackground = true;
                        //getRecevice.Start();
                    }
                    else
                    {
                        toolStripStatusLabel1.Text = "Connect " + cbSerialPortName.Text + " Failed";
                        btnConnect.Invoke(new Action<string>((str) => { btnConnect.FillColor = Color.Red; btnConnect.Text = str; }), "连接失败");
                    }
                }
                else
                {
                    if (CloseDevice())
                    {
                        btnConnect.FillColor = Color.Gold;
                        btnConnect.Invoke(new Action<string>((str) => { btnConnect.Text = str; }), "断开连接");
                        toolStripStatusLabel1.Text = "DisConnect " + cbSerialPortName.Text;
                        //bAccpet = false;
                        //checkedListBox1.Items.Clear();
                    }
                    else
                    {
                        btnConnect.Invoke(new Action<string>((str) => { btnConnect.Text = str; }), "断开连接失败");
                        toolStripStatusLabel1.Text = "DisConnect " + cbSerialPortName.Text + "Fail";
                        btnConnect.FillColor = Color.Red;
                    }
                    //SetButtonEnable(false);
                    //timerStart.Stop();
                    //getRecevice.Abort();
                }
            }
            catch (Exception ex)
            {
                updaRichBox("[Error]Connect/DisConnect Fail! " + ex.Message);
            }
            finally { /*btnReceiveData.PerformClick();*/ }
        }
        private void RefreshPort()
        {
            //updaRichBox("Refresh Port...");
            cbSerialPortName.Items.Clear();

            string[] ports = SerialPort.GetPortNames();


            foreach (string port in ports)
            {
                cbSerialPortName.Items.Add(port);
                //updaRichBox("Port Name: " + port);
            }

            if (ports.Length == 0)
            {
                //btConnect.Enabled = false;
                cbSerialPortName.Text = "";
                //updaRichBox("Can't Find Port!");
            }
            else
            {
                //btConnect.Enabled = true;
                cbSerialPortName.SelectedIndex = 0;
            }
        }

        public bool OpenDevice(string portName = "COM1", int baudRate = 115200, int databits = 8, Parity parity = Parity.None, StopBits stopBits = StopBits.One)
        {
            //Ensure port isn't already opened:
            if (sp.IsOpen)
                sp.Close();
            //Assign desired settings to the serial port:
            sp.PortName = portName;
            sp.BaudRate = baudRate;
            sp.DataBits = databits;
            sp.Parity = parity;
            sp.StopBits = stopBits;
            //These timeouts are default and cannot be editted through the class at this point:
            sp.ReadTimeout = -1;
            sp.WriteTimeout = 10000;

            try
            {
                sp.Open();
            }
            catch (Exception ex)
            {
                updaRichBox("[Error]连接串口失败！" + ex.Message);
                return false;
            }

            return true;
        }

        public bool CloseDevice()
        {
            if (sp.IsOpen)
            {
                try
                {
                    sp.Close();
                }
                catch (Exception ex)
                {
                    updaRichBox("[Error]断开串口失败! " + ex.Message);
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        private Thread getRecevice;
        bool bAccpet = false;

        delegate void DelegateAcceptData();
        delegate void reaction();
        void AcceptData()
        {
            if (rbDataLog.InvokeRequired)
            {
                try
                {
                    DelegateAcceptData ddd = new DelegateAcceptData(AcceptData);
                    this.Invoke(ddd, new object[] { });
                }
                catch { }
            }
            else
            {
                try
                {
                    string strRecieve = sp.ReadExisting();
                    rbDataLog.AppendText(strRecieve);
                    rbDataLog.ScrollToCaret();//将控件内容滚动到当前插入符号位置。
                    //SaveLog();
                }
                catch (Exception ex) { updaRichBox("[Error]串口数据接收字符串失败！ " + ex.Message); }
            }
        }

        private void sp_DataReceived()
        {
            try

            {
                System.Threading.Thread.Sleep(int.Parse(uiTextBox1.Text));//延时100ms等待接收完数据

                // this.Invoke就是跨线程访问ui的方法

                //this.Invoke((EventHandler)(delegate
                {
                    if (!checkBoxHexDisplay.Checked)
                    {
                        //rbDataLog.AppendText(sp.ReadLine());
                        //updaRichBox(sp.ReadLine());
                        AcceptData();
                    }
                    else
                    {
                        Byte[] ReceivedData = new Byte[sp.BytesToRead];
                        sp.Read(ReceivedData, 0, ReceivedData.Length);
                        string RecvDataText = null;
                        for (int i = 0; i < ReceivedData.Length - 1; i++)
                        {
                            RecvDataText += (ReceivedData[i].ToString("X2") + " ");//"0x" +
                        }
                        //rbDataLog.AppendText(RecvDataText);
                        //updaRichBox(RecvDataText);
                        if (RecvDataText != null && RecvDataText.Contains(splitStr))//FC FD FE FF
                        {
                            dataQueueX.Clear();
                            dataQueueY.Clear();
                            //RecvDataText.Split("FC FD FE FF");
                            //dataQueueY.Enqueue();
                            //RecvDataText = "FC FD FE FF 3B 02 5B C3 0C 5B 47 00 00 00 64 01 A8 00 13 00 00 00 00 00 00 00 00 00 03 61 08 54 08 54 08 64 08 74 08 7B 08 88 08 94 08 A1 08 AF 08 BE 08 C8 08 DB 08 F8 08 FC 08 14 09 2D 09 30 09 47 09 58 09 6D 09 84 09 94 09 BC 09 C8 09 DC 09 CB 09 D9 09 E7 09 18 0A 8B 0A 8B 0B 1A 0C 3F 0C 53 0C 66 0C 71 0C 68 0C 60 0C 7C 0C 7C 0C 7B 0C 71 0C 5E 0C 5D 0C 1C 0C 1A 0C 1D 0C 0F 0C 0B 0C F9 0B E5 0B E5 0B DB 0B D1 0B BE 0B B4 0B AC 0B A1 0B 97 0B 8E 0B 85 0B 71 0B 68 0B 5D 0B 54 0B 4A 0B 37 0B 37 0B 24 0B 1A 0B 10 0B 07 0B FD 0A FD 0A F4 0A E1 0A D7 0A CC 0A C4 0A C3 0A BA 0A B0 0A B0 0A A7 0A 9D 0A 9D 0A 92 0A 89 0A 80 0A 76 0A 76 0A 6C 0A 63 0A 63 0A 63 0A 59 0A 4F 0A 46 0A 46 0A 46 0A 3C 0A 33 0A 29 0A 28 0A 28 0A 1F 0A 16 0A 15 0A 14 0A 0C 0A 0A 0A 61 08 C5 07 9C 07 85 07 83 07 83 07 7A 07 83 07 7A 07 7B 07 70 07 7A 07 71 07 70 07 70 07 71 07 67 07 67 07 5D 07 66 07 5D 07 5C 07 5D 07 5C 07 5D 07 5C 07 5C 07 5C 07 5C 07 5C 07 5C 07 53 07 5B 07 5B 07 53 07 5D 07 61 07 81 07 EB 07 83 09 8D 09 8C 09 8C 09 8C 09 8C 09 8C 09 8C 09 8C 09 8C 09 96 09 8C 09 8C 09 8C 09 8C 09 8D 09 96 09 96 09 96 09 95 09 A0 09 9F 09 A0 09 A0 09 A0 09 A8 09 A8 09 A8 09 A8 09 A9 09 A9 09 B3 09 B3 09 BC 09 46 03 32 03 34 03 36 03 2D 03 3E 03 D8 09 CE 09 D6 09 D3 09 DF 09 DA 09 B9 02 A8 02 A5 02 B9 02 B0 02 B7 02 BF 02 CB 02 CB 02 C2 02 B8 02 C5 02 D1 02 D4 02 E6 02 DF 02 F4 02 F3 02 F8 02 F3 02 F1 02 FF 02 F8 02 FB 02 05 03 0B 03 02 03 07 03 F1 02 D4 02 C9 02 B9 02 B5 02 A7 02 A5 02 A7 02 AA 02 B0 02 AA 02 B8 02 B3 02 BA 02 C1 02 BD 02 C2 02 C1 02 C1 02 CA 02 C6 02 B2 02 8A 02 62 02 55 02 53 02 52 02 54 02 4D 02 4C 02 40 02 3D 02 4B 02 33 02 36 02 3A 02 2A 02 33 02 2C 02 19 02 1E 02 2A 02 1D 02 1D 02 1B 02 19 02 0D 02 0A 02 11 02 67 94 FC FD FE FF ";
                            //RecvDataText = "BE A0 12 34 02 38 03 02 00 00 00 5B C3 2E F4 42 00 14 56 79 01 05 17 19 09 19 08 19 13 19 C3 28 FF 18 02 19 F1 18 F7 18 01 19 F9 18 F6 18 F2 18 F1 18 E6 18 F5 18 E9 18 E5 18 DD 18 E2 18 D8 18 E2 18 D8 18 D8 18 D7 18 E1 18 D4 18 D8 18 D9 18 E7 18 D6 18 DF 18 DA 18 D6 18 DE 18 D7 18 D2 18 D4 18 D5 18 DC 18 D6 18 E6 18 DB 18 E7 18 D6 18 DE 18 ED 18 EA 18 E3 18 EB 18 E6 18 F3 18 E3 18 F0 18 ED 18 F2 18 EE 18 F9 18 01 19 F7 18 08 19 01 19 03 19 14 19 00 19 12 19 1F 19 19 19 23 19 37 19 67 19 3A 19 32 19 53 19 42 19 49 19 5C 19 49 19 BE 28 6C 19 6B 19 77 19 83 19 84 19 CE 19 C1 28 C1 28 C3 28 C3 28 CF 28 C1 28 C1 28 E8 19 B8 28 C3 28 C1 28 BE 28 C1 28 C1 28 C3 28 BE 28 D5 28 C3 28 C1 28 C3 28 C1 28 C1 28 C3 28 BE 28 01 1B C3 28 88 1B C3 28 C6 28 BE 28 D6 1A C1 28 BE 28 C3 28 BB 28 C3 28 C6 28 BE 28 BB 28 5B 1B 5F 1B 6B 1B 8A 1B 84 1B 8F 1B C1 28 BE 28 C1 28 E1 1B FB 1B 03 1C 13 1C 2C 1C 27 1C 43 1C C1 28 6E 1C D8 1C BE 28 C6 28 C1 28 C9 28 C1 28 BE 28 C3 28 C1 28 C1 28 C3 28 C1 28 C1 28 C6 28 C3 28 C1 28 B2 28 C1 28 C6 28 C1 28 C1 28 C1 28 C1 28 BE 28 DE 28 C6 28 C1 28 C1 28 A4 28 C3 28 C3 28 C1 28 C3 28 C1 28 C1 28 BB 28 C1 28 3C 21 53 21 C1 28 C1 28 C1 28 C1 28 C1 28 C1 28 07 28 2A 28 61 28 6A 28 C6 28 CC 28 C1 28 D2 28 C1 28 C1 28 C3 28 C1 28 C1 28 C1 28 B6 28 D5 28 C6 28 B5 28 94 28 83 28 A0 28 C1 28 BE 28 47 2A 54 2A 80 2A 7E 2A 59 2A 4D 2A 2E 2A 3A 2A 45 2A 77 2A C6 28 C1 28 C1 28 BE 28 AF 28 D2 28 C1 28 A9 28 BE 28 C1 28 C1 28 C1 28 C1 28 C1 28 C6 28 C1 28 C6 28 BE 28 C9 28 A6 28 BE 28 C6 28 C3 28 C1 28 C3 28 C1 28 C1 28 C3 28 C1 28 C6 28 C6 28 CF 28 D8 28 C3 28 C1 28 C1 28 C1 28 C1 28 C6 28 C1 28 C1 28 CC 28 C1 28 D5 28 BE 28 B2 27 AB 27 91 27 78 27 97 27 C1 28 C1 28 C1 28 B2 28 C1 28 C1 28 7D 27 75 27 78 27 8B 27 6E 27 56 27 49 27 25 27 3E 27 A4 28 C1 28 86 26 65 26 8F 26 C1 28 C1 28 37 27 E1 26 C1 26 B6 26 C1 28 BB 28 C6 28 BB 28 90 26 50 26 30 26 48 26 C3 28 C1 28 C3 28 C6 28 DF 26 CC 26 BC 26 BC 26 A8 26 C0 26 9A 26 CC 26 B0 26 C1 28 BE 28 CE 26 90 26 75 26 75 26 85 26 C1 28 C1 28 C1 28 C6 28 C1 28 C9 27 D3 27 E1 27 BC 27 B5 28 C3 28 C3 28 C3 28 C1 28 C9 28 BB 28 AF 28 C1 28 CC 28 C1 28 C3 28 C1 28 BE 28 C1 28 BE 28 C3 28 BE 28 BE 28 BE 28 C3 28 C1 28 C1 28 04 0D 15 0D 1F 0D 18 0D EA 0C D8 0C D0 0C AB 0C 70 0C 8D 0C 2A 0D BF 0C BE 0D 16 0E 09 0E F6 0D FB 0D 05 0E FC 0D FE 0D 17 0E 32 0E A4 28 C1 28 C1 28 C1 28 C1 28 C1 28 31 2A BB 28 BE 28 B2 28 B5 28 BE 28 AF 28 BB 28 1E 04 F4 03 CC 03 A5 03 8F 03 8E 03 8B 03 89 03 D1 D8BE A0 12 34";

                            //string juequString = MidStrEx(RecvDataText, splitStr, splitStr);//"BE A0 12 34 "
                            //string[] dataString = MidStrEx(RecvDataText, splitStr, splitStr).Split(' ');//截取激光雷达数据

                            string bbb = RecvDataText.Substring(0, RecvDataText.IndexOf(splitStr));//"FC FD FE FF"
                            string ss = CutByteString(RecvDataText, bbb.Length, 824 * 3 - 1);
                            string[] dataString = null;// = MidStrEx(ss + " " + splitStr , splitStr, splitStr).Split(' ');//截取激光雷达数据

                            if (cbType.SelectedIndex == 1)
                            {
                                ss = CutByteString(RecvDataText, bbb.Length, 824 * 3 - 1);
                                //dataString = MidStrEx(ss + " " + splitStr, splitStr, splitStr).Split(' ');//截取激光雷达数据
                                dataString = ss.Split(' ');
                                if (dataString.Count() < 820)//951雷达
                                    return;
                            }
                            else if (cbType.SelectedIndex == 0)//921雷达
                            {
                                ss = CutByteString(RecvDataText, bbb.Length, 579 * 3 - 1);
                                //dataString = MidStrEx(ss + " " + splitStr, splitStr, splitStr).Split(' ');//截取激光雷达数据
                                dataString = ss.Split(' ');
                                if (cbType.SelectedIndex == 0 && dataString.Count() < 579)
                                    return;
                            }

                            int startPoint = 0;
                            //雷达型号不同，前面数据长度、内容不同
                            if (cbType.SelectedIndex == 1)
                            {
                                startPoint = 22;
                            }
                            else if (cbType.SelectedIndex == 0)
                            {
                                startPoint = 29;
                            }
                            int byteCount = startPoint + laserPointCount * 2;
                                //for (int i = 25; i < 573; i += 2)//573
                            for (int i = startPoint; i < byteCount; i += 2)//573
                            {
                                int low = System.Int32.Parse(dataString[i], System.Globalization.NumberStyles.HexNumber);//s为string类型，以“41”为例，输出为65
                                int high = System.Int32.Parse(dataString[i + 1], System.Globalization.NumberStyles.HexNumber);//s为string类型，以“41”为例，输出为65
                                                                                                                                //System.Convert.ToString(dataString[i], 10);
                                int a = low + high * 256;
                                //深度仿射，有角度偏移，96/274，每个点相差角度，*cos(96 / 2- i * 96 / 274)
                                //double angle = (96 / 2 - (i - 25) / 2 * 96 / 273); // / 180 * Math.PI
                                double angle = (lasetAngle / 2 - (i - startPoint) / 2 * lasetAngle / (laserPointCount - 1)); // / 180 * Math.PI
                                double radian = (angle / 180 * Math.PI);

                                double yDeep = (int)(a * Math.Cos(radian));
                                double xWidth = (int)(a * Math.Sin(radian));
                                if (yDeep > 60000)
                                    yDeep = 60000;
                                //if (i == startPoint)//开始点位
                                //{
                                //    firstDistance = xWidth;
                                //    updaRichBox("start: " + dataString[startPoint] + " " + dataString[startPoint+1] + "   realDeep: " + yDeep + "\r\n");
                                //}
                                //if(i==571)//结束点位
                                //    updaRichBox("end: " + dataString[571] + " " + dataString[572] + "   realDeep: " + yDeep + "\r\n");
                                //if (i == startPoint + laserPointCount * 2)//结束点位
                                //    updaRichBox("end: " + dataString[startPoint + laserPointCount * 2] + " " + dataString[startPoint + laserPointCount * 2 + 1] + "   realDeep: " + yDeep + "\r\n");

                                //yDeep = Math.Sqrt(yDeep * yDeep + xWidth * xWidth) * Math.Sin(uiTrackBarRotate.Value);
                                dataQueueY.Enqueue(yDeep);
                                dataQueueX.Enqueue(xWidth);

                                //updaRichBox((i-25)/2 + " " + angle.ToString("#0.00 ") + radian.ToString("#0.00 ") + b + "\r\n");
                            }
                            //updaRichBox(splitStr + juequString + "\r\n");
                        }
                    }
                    sp.DiscardInBuffer();//丢弃接收缓冲区数据
                }//));
            }
            catch (Exception ex) { updaRichBox("[Error]串口数据接收深度信息失败！ " + ex.Message); }
        }
        private System.Windows.Forms.DataVisualization.Charting.Chart ChartTest;

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (bAccpet != false)
                {
                    MessageBox.Show("正在接收数据，重新开始请先关闭！！！");
                    return;
                }
                ChartTest = chartUDPDisp;

                InitChart(ref this.ChartTest);

                StartTestChart();
            }
            catch (Exception ex) { Console.WriteLine("[Error] 开始显示实时点云功能错误 " + ex.Message); }
        }

        private void StartTestChart()
        {
            dataQueueY.Clear();
            dataQueueX.Clear();
            //ChartRunTimer = new System.Windows.Forms.Timer();
            //ChartRunTimer.Interval = 1000;
            //ChartRunTimer.Tick += ChartRunTimer_Tick;
            //ChartRunTimer.Start();
            
            getRecevice = new Thread(new ThreadStart(DispPointData));
            getRecevice.IsBackground = true;
            bAccpet = true;
            getRecevice.Start();

            //UDPCommand.Enabled = true;
            //UDPSendCommand.Enabled = true;

        }
        private void Stop_Click(object sender, EventArgs e)
        {
            //ChartRunTimer.Stop();
           
            try
            {
                SendCommand("AA 01 80 03 00 00 00 00 00 00 00 00 00 00 FF");
                bAccpet = false;
                //UDPCommand.Enabled = false;
                //UDPSendCommand.Enabled = false;
                //Thread.Sleep(50);
                ////停止主监听线程
                //if (null != getRecevice)
                //{
                //    if (getRecevice.IsAlive)
                //    {
                //        if (!getRecevice.Join(500))
                //        {
                //            //关闭线程
                //            getRecevice.Abort();
                //        }
                //    }
                //    getRecevice = null;
                //}
            }
            catch { }

            //ZoomChartX(ref chartUDPDisp); //增加的话会使得缩放错乱
            //ZoomChartY(ref chartUDPDisp);//增加的话会使得缩放错乱
        }

        public void SendCommand(string command)
        {
            try
            {
                List<byte> data = new List<byte>();
                if (sp.IsOpen)
                {
                    //string stopCommand = "AA 01 80 03 00 00 00 00 00 00 00 00 00 00 FF";

                    byte[] newBuffer = HexStrTobyte(command);

                    //字符串形式命令
                    //sp.Encoding = System.Text.Encoding.GetEncoding("GB2312");
                    //string parsed = command.Replace("\\\\r", "\r").Replace("\\\\n", "\n");
                    //parsed += "\r\n";
                    //data.AddRange(System.Text.Encoding.ASCII.GetBytes(parsed));
                    //sp.Write(newBuffer);//发送数据
                    //byte数组形式命令
                    sp.Write(newBuffer, 0, newBuffer.Length);

                    toolStripStatusLabel1.Text = "停止发送命令： " + command;
                    LogMessage("停止发送命令： " + command);
                }
                else
                {
                    MessageBox.Show("[ERROR]请先打开串口！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("[ERROR]发送命令失败" + ex.Message);
            }
        }

        #endregion
        int lastLength = 0;
        private void DispPointData()
        {
            try
            {
                while (bAccpet)
                {
                    //System.Threading.Thread.Sleep(50);//延时100ms等待接收完数据

                    //接收数据
                    sp_DataReceived();
                    this.ChartTest.Invoke(new Action(() =>
                    {
                        if (dataQueueY.Count == laserPointCount && dataQueueX.Count == laserPointCount)//274
                        {
                            this.ChartTest.Series[0].Points.Clear();
                            this.ChartTest.Series[1].Points.Clear();
                            this.ChartTest.Series[2].Points.Clear();
                            double angle = double.Parse(numericUpDownRate.Text) - (90 - lasetAngle / 2);//顺时针,减去42度是 90-96/2=42.移动至X轴平行,90-108/2=36
                            double dx = 0;
                            double dy = 0;//围绕某个点旋转
                            List<int> length = new List<int> { };
                            List<int> hight = new List<int> { };
                            for (int i = 0; i < dataQueueY.Count; i++)//dataQueueY.Count=274
                            {
                               
                                //double CurrX = dataQueueX.ElementAt(i);
                                //double CurrY = dataQueueY.ElementAt(i);
                                double xx = (dataQueueX.ElementAt(i) - dx) * Math.Cos(angle * Math.PI / 180) - (dataQueueY.ElementAt(i) - dy) * Math.Sin(angle * Math.PI / 180) + dx;
                                double yy = (dataQueueY.ElementAt(i) - dy) * Math.Cos(angle * Math.PI / 180) + (dataQueueX.ElementAt(i) - dx) * Math.Sin(angle * Math.PI / 180) + dy;
                                this.ChartTest.Series[0].Points.AddXY((int)xx, (int)yy);

                                length.Add((int)xx);
                                hight.Add((int)yy);
                                //this.ChartTest.Series[0].Points.AddXY(dataQueueX.ElementAt(i), dataQueueY.ElementAt(i));
                                //this.ChartTest.Series[0].Points.AddXY((i + 1), dataQueueY.ElementAt(i));
                                //this.ChartTest.ChartAreas[0].AxisX.Maximum = dataQueueY.Count;
                            }

                            if (cbType.SelectedIndex == 1)
                            {
                                //int startCalcLength = -6000;
                                //int endCalcLength = -6000;
                                //for (int i = int.Parse(txtStart.Text); i < int.Parse(txtEnd.Text); i++)//hight.Count
                                //{
                                //    if (hight[i] > startCalcLength)
                                //        startCalcLength = hight[i];
                                //}
                                //if ((startCalcLength + 6400) > int.Parse(txtLength.Text)) //length[endCalcLength] - length[startCalcLength]
                                //    this.txtLength.Text = (startCalcLength + 6400).ToString();




                                List<int> dataList = new List<int> { };
                                //if (hight[int.Parse(txtCalc.Text)] > -6000)
                                {
                                    int maxCalcLength = hight[int.Parse(txtCalc.Text)];
                                    for (int i = int.Parse(txtStart.Text); i < int.Parse(txtEnd.Text); i++)//hight.Count
                                    {

                                        if (hight[i] > -6000)
                                        {
                                            dataList.Add(length[i]);
                                        }
                                    }
                                    //if (dataList.Count < 30)
                                    //    return;
                                    dataList.Sort();//从小到大排序
                                    if (dataList.Count > 30)//  && dataList[dataList.Count - 1]- dataList[0] > int.Parse(this.txtLength.Text)
                                    {
                                        lastLength = dataList[dataList.Count - 1] - dataList[0];
                                        this.txtLength.Text = (dataList[dataList.Count - 1] - dataList[0]).ToString();
                                        updaRichBox("宽度：" + lastLength + "\r\n");
                                    }
                                    //updaRichBox("最小X轴：" + dataList[0] + "   最大X轴：" + dataList[dataList.Count - 1] + "  测量长度：" + (dataList[dataList.Count - 1] - dataList[0]).ToString());
                                    //表格显示
                                    //if (dataList.Count < 30)
                                    //{
                                    //    object[] aa = new object[6];
                                    //    aa[0] = DateTime.Now;
                                    //    aa[1] = null;
                                    //    aa[2] = null;
                                    //    aa[3] = lastLength;
                                    //    aa[4] = null;
                                    //    aa[5] = null;
                                    //    this.dataGridView1.Rows.Insert(0, aa);
                                    //    //Thread.Sleep(100);
                                    //    lastLength = 0;
                                    //    this.txtLength.Text = "0";
                                    //}
                                }
                            }
                            else if (cbType.SelectedIndex == 0)
                            {
                                List<int> dataList = new List<int> { };
                                //if (hight[int.Parse(txtCalc.Text)] > -int.Parse(tbStartDistance.Text))
                                {
                                    //高度、长度算法，U951测量高度，U931测量长度
                                    int maxCalcLength = hight[int.Parse(txtCalc.Text)];
                                    for (int i = int.Parse(txtStart.Text); i < int.Parse(txtEnd.Text); i++)//hight.Count
                                    {

                                        if (hight[i] > -int.Parse(tbStartDistance.Text) + 300)
                                        {
                                            dataList.Add(length[i]);
                                        }
                                    }

                                    dataList.Sort();//从小到大排序
                                    //if (dataList[dataList.Count - 1] - dataList[0] > int.Parse(this.txtLength.Text))
                                    if (dataList.Count > 10) 
                                    {
                                        lastLength = dataList[dataList.Count - 1] - dataList[0];
                                        this.txtLength.Text = (dataList[dataList.Count - 1] - dataList[0]).ToString();
                                        updaRichBox("长度：" + lastLength + "\r\n");
                                    }
                                    //updaRichBox("最小X轴：" + dataList[0] + "   最大X轴：" + dataList[dataList.Count - 1] + "  测量长度：" + (dataList[dataList.Count - 1] - dataList[0]).ToString());
                                    //表格显示
                                    if (dataList.Count < 30)
                                    {
                                        object[] aa = new object[6];
                                        aa[0] = DateTime.Now;
                                        aa[1] = null;
                                        aa[2] = null;
                                        aa[3] = lastLength;
                                        aa[4] = null;
                                        aa[5] = null;
                                        //this.dataGridView1.Rows.Insert(0, aa);
                                        //Thread.Sleep(100);
                                        lastLength = 0;
                                        this.txtLength.Text = "0";
                                    }



                                    #region 计算车辆长度算法
                                    ////高度、长度算法，U951测量高度，U931测量长度
                                    //int startCalcLength = 0;
                                    //int endCalcLength = 0;
                                    //for (int i = int.Parse(txtCalc.Text); i < hight.Count; i++)//hight.Count
                                    //{
                                    //    if (hight[i] > startCalcLength)
                                    //    {
                                    //        startCalcLength = i;

                                    //    }
                                    //    if (hight[i] > (-6200 + 500))
                                    //    {
                                    //        endCalcLength = i;
                                    //    }
                                    //}
                                    //for (int i = int.Parse(txtEnd.Text); i > int.Parse(txtStart.Text); i--)
                                    //{
                                    //    if (hight[i] > (-6200 + 500))
                                    //    {
                                    //        startCalcLength = i;
                                    //    }
                                    //}
                                    //if (length[endCalcLength] - length[startCalcLength] > int.Parse(txtLength.Text))
                                    //    this.txtLength.Text = (length[endCalcLength] - length[startCalcLength]).ToString();
                                    #endregion
                                }
                            }

                            this.ChartTest.Series[1].Points.AddXY((dataQueueX.ElementAt(0) - dx) * Math.Cos(angle * Math.PI / 180) - (dataQueueY.ElementAt(0) - dy) * Math.Sin(angle * Math.PI / 180) + dx,
                                            (dataQueueY.ElementAt(0) - dy) * Math.Cos(angle * Math.PI / 180) + (dataQueueX.ElementAt(0) - dx) * Math.Sin(angle * Math.PI / 180) + dy);
                            this.ChartTest.Series[1].Points.AddXY(0,0);
                            //this.ChartTest.Series[2].Points.AddXY(dataQueueX.ElementAt(dataQueueX.Count - 1), dataQueueY.ElementAt(dataQueueY.Count - 1));
                            this.ChartTest.Series[2].Points.AddXY((dataQueueX.ElementAt(dataQueueX.Count - 1) - dx) * Math.Cos(angle * Math.PI / 180) - (dataQueueY.ElementAt(dataQueueX.Count - 1) - dy) * Math.Sin(angle * Math.PI / 180) + dx,
                                            (dataQueueY.ElementAt(dataQueueX.Count - 1) - dy) * Math.Cos(angle * Math.PI / 180) + (dataQueueX.ElementAt(dataQueueX.Count - 1) - dx) * Math.Sin(angle * Math.PI / 180) + dy);
                            this.ChartTest.Series[2].Points.AddXY(0, 0);
                           
                            //updaRichBox("StartPoint: x = " + dataQueueX.ElementAt(0) + " y = " + dataQueueY.ElementAt(0) + "   EndPoint: x = " + dataQueueY.ElementAt(laserPointCount-1) + " y = " + dataQueueY.ElementAt(laserPointCount-1) + "\r\n");//273
                        }
                    }));
                }
            }
            catch (Exception ex) { Console.WriteLine("[Error] 更新点云数据出错: " + ex.Message); }
        }
        private void InitChart(ref Chart chart1)
        {
            //定义图表区域
            chart1.ChartAreas.Clear();
            ChartArea chartArea1 = new ChartArea("C1");
            chart1.ChartAreas.Add(chartArea1);

            //定义存储和显示点的容器
            chart1.Series.Clear();
            chart1.ChartAreas[0].BackColor = Color.Lavender;
            Series series1 = new Series("深度");
            Series series2 = new Series("起点");
            Series series3 = new Series("终点");
            series1.ChartArea = "C1";
            series2.ChartArea = "C1";
            series3.ChartArea = "C1";
            chart1.Series.Add(series1);
            chart1.Series.Add(series2);
            chart1.Series.Add(series3);
            //设置图表显示样式
            chart1.ChartAreas[0].AxisY.Minimum = -60000;
            chart1.ChartAreas[0].AxisY.Maximum = 60000;
            chart1.ChartAreas[0].AxisX.Minimum = -60000;
            chart1.ChartAreas[0].AxisX.Maximum = 60000;
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "F0";
            //chart1.ChartAreas[0].AxisY.Interval = 5000;
            //chart1.ChartAreas[0].AxisX.Interval = 5000;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.Silver;//网格线
            chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.Silver;
            //chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.Transparent;//透明网格线
            //this.chart1.ChartAreas[0].AxisX.Enabled = AxisEnabled.False;//会不显示网格线
            //chart1.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
            //设置标题
            chart1.Titles.Clear();
            chart1.Titles.Add("S01");
            chart1.Titles[0].Text = "激光雷达实时距离显示";
            chart1.Titles[0].ForeColor = Color.RoyalBlue;//title  字体颜色
            chart1.Titles[0].Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);//title  字体格式
            //XY轴标签
            chart1.ChartAreas[0].AxisX.Title = "宽度 单位/mm";
            chart1.ChartAreas[0].AxisY.Title = "深度 单位/mm";
            //设置图表显示样式
            chart1.Series[0].Color = Color.Blue;
            chart1.Series[1].Color = Color.Green;
            chart1.Series[2].Color = Color.Red;
            chart1.Series[0].ChartType = SeriesChartType.Line;
            chart1.Series[1].ChartType = SeriesChartType.Line;
            chart1.Series[2].ChartType = SeriesChartType.Line;

            chart1.Series[0].BorderWidth = 1;
            chart1.Series[1].BorderWidth = 1;
            chart1.Series[2].BorderWidth = 1;

            chart1.Series[0].MarkerSize = 5;
            chart1.Series[1].MarkerSize = 5;
            chart1.Series[2].MarkerSize = 5;

            chart1.Series[0].MarkerStyle = MarkerStyle.Circle;
            chart1.Series[1].MarkerStyle = MarkerStyle.Circle;
            chart1.Series[2].MarkerStyle = MarkerStyle.Circle;

            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            chart1.Series[2].Points.Clear();

            chart1.Series[0].ToolTip = "X：#VALX\nY：#VALY\n编号: #INDEX";
            series1.XValueType = ChartValueType.Int32;
            series1.YValueType = ChartValueType.Int32;
            series2.XValueType = ChartValueType.Int32;
            series2.YValueType = ChartValueType.Int32;
            series3.XValueType = ChartValueType.Int32;
            series3.YValueType = ChartValueType.Int32;
        }
        private void ZoomChartX(ref Chart chart1)
        {
            // Zoom into the X axis
            chart1.ChartAreas[0].AxisX.ScaleView.Zoom(chart1.ChartAreas[0].AxisX.Minimum, chart1.ChartAreas[0].AxisX.Maximum);

            // Enable range selection and zooming end user interface
            //chart1.ChartAreas[0].CursorX.IsUserEnabled = true;
            //chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            //chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;

            //将滚动内嵌到坐标轴中
            chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;

            // 设置滚动条的大小
            chart1.ChartAreas[0].AxisX.ScrollBar.Size = 6;

            // 设置滚动条的按钮的风格，下面代码是将所有滚动条上的按钮都显示出来
            chart1.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.All;//ScrollBarButtonStyle.All;

            // 设置自动放大与缩小的最小量
            chart1.ChartAreas[0].AxisX.ScaleView.SmallScrollSize = double.NaN;
            chart1.ChartAreas[0].AxisX.ScaleView.SmallScrollMinSize = 2;
        }
        private void ZoomChartY(ref Chart chart1)
        {
            // Zoom into the X axis
            chart1.ChartAreas[0].AxisY.ScaleView.Zoom(chart1.ChartAreas[0].AxisY.Minimum, chart1.ChartAreas[0].AxisY.Maximum);

            // Enable range selection and zooming end user interface
            //chart1.ChartAreas[0].CursorY.IsUserEnabled = true;
            //chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            //chart1.ChartAreas[0].AxisY.ScaleView.Zoomable = true;

            //将滚动内嵌到坐标轴中
            chart1.ChartAreas[0].AxisY.ScrollBar.IsPositionedInside = true;

            // 设置滚动条的大小
            chart1.ChartAreas[0].AxisY.ScrollBar.Size = 6;

            // 设置滚动条的按钮的风格，下面代码是将所有滚动条上的按钮都显示出来
            chart1.ChartAreas[0].AxisY.ScrollBar.ButtonStyle = ScrollBarButtonStyles.All;//ScrollBarButtonStyle.All;

            // 设置自动放大与缩小的最小量
            chart1.ChartAreas[0].AxisY.ScaleView.SmallScrollSize = double.NaN;
            chart1.ChartAreas[0].AxisY.ScaleView.SmallScrollMinSize = 2;
        }
        /// <summary>
        /// 字符串截取，提取startstr和endstr之间的字符串，若出现多次，只提取第一次匹配的
        /// </summary>
        /// <param name="sourse">需要匹配的字符串</param>
        /// <param name="startstr">开始匹配的字符串</param>
        /// <param name="endstr">结束匹配的字符串</param>
        /// <returns></returns>
        public string MidStrEx(string sourse, string startstr, string endstr)
        {
            string result = string.Empty;
            int startindex, endindex;
            try
            {
                startindex = sourse.IndexOf(startstr);
                if (startindex == -1)
                    return result;
                string tmpstr = sourse.Substring(startindex + startstr.Length);
                endindex = tmpstr.IndexOf(endstr);
                if (endindex == -1)
                    return result;
                result = tmpstr.Remove(endindex);
            }
            catch (Exception ex)
            {
               updaRichBox("[Error]MidStrEx Err 截取字符串失败！" + ex.Message);
            }
            return result;
        }

        void chart_MouseWheelX(object sender, MouseEventArgs e)
        {
            try
            {
                Chart chart = (Chart)sender;
                double zoomfactor = 1.5;   //设置缩放比例
                bool isZoom = false;//是否同步缩放标志，X轴缩放置true，Y轴跟着缩放
                #region X轴缩放
                double xstartpoint = chart.ChartAreas[0].AxisX.ScaleView.ViewMinimum;      //获取当前x轴最小坐标
                double xendpoint = chart.ChartAreas[0].AxisX.ScaleView.ViewMaximum;      //获取当前x轴最大坐标
                double xmouseponit = chart.ChartAreas[0].AxisX.PixelPositionToValue(e.X);    //获取鼠标在chart中x坐标
                double xratio = (xendpoint - xmouseponit) / (xmouseponit - xstartpoint);      //计算当前鼠标基于坐标两侧的比值，后续放大缩小时保持比例不变
                double Xsize = chart.ChartAreas[0].AxisX.Maximum - chart.ChartAreas[0].AxisX.Minimum;
                if (e.Delta > 0)    //滚轮上滑放大
                {
                    if (Xsize > 50)     //缩放视图不小于5
                    {
                        if ((xmouseponit >= chart.ChartAreas[0].AxisX.ScaleView.ViewMinimum) && (xmouseponit <= chart.ChartAreas[0].AxisX.ScaleView.ViewMaximum)) //判断鼠标位置不在x轴两侧边沿
                        {
                            double xspmovepoints = Math.Round((xmouseponit - xstartpoint) * (zoomfactor - 1) / zoomfactor, 1);    //计算x轴起点需要右移距离,保留一位小数
                            double xepmovepoints = Math.Round(xendpoint - xmouseponit - xratio * (xmouseponit - xstartpoint - xspmovepoints), 1);    //计算x轴末端左移距离，保留一位小数
                            double viewsizechange = xspmovepoints + xepmovepoints;         //计算x轴缩放视图缩小变化尺寸
                                                                                           //chart.ChartAreas[0].AxisX.ScaleView.Size -= (int)viewsizechange;        //设置x轴缩放视图大小
                                                                                           //chart.ChartAreas[0].AxisX.ScaleView.Position += (int)xspmovepoints;        //设置x轴缩放视图起点，右移保持鼠标中心点
                                                                                           //chart.ChartAreas[0].AxisX.Minimum += (int)xmouseponit;
                                                                                           //chart.ChartAreas[0].AxisX.Maximum += (int)xmouseponit;
                            chart.ChartAreas[0].AxisX.Minimum /= zoomfactor;
                            chart.ChartAreas[0].AxisX.Maximum /= zoomfactor;
                        }

                    }
                }
                else     //滚轮下滑缩小
                {

                    //if (Xsize < chart.ChartAreas[0].AxisX.Maximum)
                    {
                        double xspmovepoints = Math.Round((zoomfactor - 1) * (xmouseponit - xstartpoint), 1);   //计算x轴起点需要左移距离
                        double xepmovepoints = Math.Round((zoomfactor - 1) * (xendpoint - xmouseponit), 1);    //计算x轴末端右移距离


                        if (chart.ChartAreas[0].AxisX.ScaleView.Size + xspmovepoints + xepmovepoints < chart.ChartAreas[0].AxisX.Maximum)  //判断缩放视图尺寸是否超过曲线尺寸
                        {
                            if ((xstartpoint - xspmovepoints <= chart.ChartAreas[0].AxisX.Minimum) || (xepmovepoints + xendpoint >= chart.ChartAreas[0].AxisX.Maximum))  //判断缩放值是否达到曲线边界
                            {
                                if (xstartpoint - xspmovepoints <= chart.ChartAreas[0].AxisX.Minimum)    //缩放视图起点小于等于0
                                {
                                    xspmovepoints = xstartpoint;
                                    chart.ChartAreas[0].AxisX.ScaleView.Position = chart.ChartAreas[0].AxisX.Minimum;    //缩放视图起点设为0
                                }
                                else
                                {
                                    //chart.ChartAreas[0].AxisX.ScaleView.Position -= (int)xspmovepoints;  //缩放视图起点大于0，按比例缩放
                                    chart.ChartAreas[0].AxisX.Minimum -= (int)xmouseponit;
                                    chart.ChartAreas[0].AxisX.Maximum -= (int)xmouseponit;
                                }
                                if (xepmovepoints + xendpoint >= chart.ChartAreas[0].AxisX.Maximum)  //缩放视图终点大于曲线最大值
                                {
                                    chart.ChartAreas[0].AxisX.ScaleView.Size = chart.ChartAreas[0].AxisX.Maximum - chart.ChartAreas[0].AxisX.ScaleView.Position;  //设置缩放视图尺寸=曲线最大值-视图起点值
                                    chart.ChartAreas[0].AxisX.Maximum -= (int)xmouseponit;
                                    chart.ChartAreas[0].AxisX.Minimum -= (int)xmouseponit;
                                }
                                else
                                {
                                    //double viewsizechange = (int)(xspmovepoints + xepmovepoints);         //计算x轴缩放视图缩小变化尺寸
                                    //chart.ChartAreas[0].AxisX.ScaleView.Size += (int)viewsizechange;   //按比例缩放视图大小
                                    chart.ChartAreas[0].AxisY.Minimum *= zoomfactor;
                                    chart.ChartAreas[0].AxisY.Maximum *= zoomfactor;
                                }
                            }
                            else
                            {
                                //double viewsizechange = xspmovepoints + xepmovepoints;         //计算x轴缩放视图缩小变化尺寸
                                //chart.ChartAreas[0].AxisX.ScaleView.Size += (int)viewsizechange;   //按比例缩放视图大小
                                //chart.ChartAreas[0].AxisX.ScaleView.Position -= (int)xspmovepoints;   //按比例缩放视图大小
                                //uiTrackBarRate.Value -= (int)xspmovepoints;
                                //chart.ChartAreas[0].AxisX.Minimum -= (int)xmouseponit;
                                //chart.ChartAreas[0].AxisX.Maximum -= (int)xmouseponit;
                                chart.ChartAreas[0].AxisX.Minimum *= zoomfactor;
                                chart.ChartAreas[0].AxisX.Maximum *= zoomfactor;

                            }
                        }
                        else
                        {
                            //chart.ChartAreas[0].AxisX.ScaleView.Size = chart.ChartAreas[0].AxisX.Maximum - chart.ChartAreas[0].AxisX.Minimum;
                            //chart.ChartAreas[0].AxisX.ScaleView.Position = chart.ChartAreas[0].AxisX.Minimum;
                            //chart.ChartAreas[0].AxisX.Minimum -= (int)xmouseponit;
                            //chart.ChartAreas[0].AxisX.Maximum -= (int)xmouseponit;
                            chart.ChartAreas[0].AxisX.Minimum *= zoomfactor;
                            chart.ChartAreas[0].AxisX.Maximum *= zoomfactor;
                            isZoom = true;
                        }
                    }
                }
                #endregion

                #region Y轴缩放
                //double zoomfactor = 2;   //设置缩放比例
                double ystartpoint = chart.ChartAreas[0].AxisY.ScaleView.ViewMinimum;      //获取当前x轴最小坐标
                double yendpoint = chart.ChartAreas[0].AxisY.ScaleView.ViewMaximum;      //获取当前x轴最大坐标
                double ymouseponit = chart.ChartAreas[0].AxisY.PixelPositionToValue(e.Y);    //获取鼠标在chart中x坐标
                double yratio = (yendpoint - ymouseponit) / (ymouseponit - ystartpoint);      //计算当前鼠标基于坐标两侧的比值，后续放大缩小时保持比例不变
                double Ysize = chart.ChartAreas[0].AxisY.Maximum - chart.ChartAreas[0].AxisY.Minimum;
                if (e.Delta > 0)    //滚轮上滑放大
                {
                    if (Ysize > 50)     //缩放视图不小于5
                    {
                        if ((ymouseponit >= chart.ChartAreas[0].AxisY.ScaleView.ViewMinimum) && (ymouseponit <= chart.ChartAreas[0].AxisY.ScaleView.ViewMaximum)) //判断鼠标位置不在x轴两侧边沿
                        {
                            double yspmovepoints = Math.Round((ymouseponit - ystartpoint) * (zoomfactor - 1) / zoomfactor, 1);    //计算x轴起点需要右移距离,保留一位小数
                            double yepmovepoints = Math.Round(yendpoint - ymouseponit - yratio * (ymouseponit - ystartpoint - yspmovepoints), 1);    //计算x轴末端左移距离，保留一位小数
                            double viewsizechange = yspmovepoints + yepmovepoints;         //计算x轴缩放视图缩小变化尺寸
                                                                                           //chart.ChartAreas[0].AxisY.ScaleView.Size -= (int)viewsizechange;        //设置x轴缩放视图大小
                                                                                           //chart.ChartAreas[0].AxisY.ScaleView.Position += (int)yspmovepoints;        //设置x轴缩放视图起点，右移保持鼠标中心点
                                                                                           //chart.ChartAreas[0].AxisY.Minimum += (int)ymouseponit;
                                                                                           //chart.ChartAreas[0].AxisY.Maximum += (int)ymouseponit;
                            chart.ChartAreas[0].AxisY.Minimum /= zoomfactor;
                            chart.ChartAreas[0].AxisY.Maximum /= zoomfactor;
                        }
                    }
                }
                else     //滚轮下滑缩小
                {
                    //if (Ysize < chart.ChartAreas[0].AxisY.Maximum)
                    {
                        double yspmovepoints = Math.Round((zoomfactor - 1) * (ymouseponit - ystartpoint), 1);   //计算x轴起点需要左移距离
                        double yepmovepoints = Math.Round((zoomfactor - 1) * (yendpoint - ymouseponit), 1);    //计算x轴末端右移距离
                                                                                                               //Ysize +
                        if (yspmovepoints + yepmovepoints < chart.ChartAreas[0].AxisY.Maximum && !isZoom)  //判断缩放视图尺寸是否超过曲线尺寸
                        {
                            if ((ystartpoint - yspmovepoints <= chart.ChartAreas[0].AxisY.Minimum) || (yepmovepoints + yendpoint >= chart.ChartAreas[0].AxisY.Maximum))  //判断缩放值是否达到曲线边界
                            {
                                if (ystartpoint - yspmovepoints <= chart.ChartAreas[0].AxisY.Minimum)    //缩放视图起点小于等于0
                                {
                                    yspmovepoints = ystartpoint;
                                    chart.ChartAreas[0].AxisY.ScaleView.Position = chart.ChartAreas[0].AxisY.Minimum;    //缩放视图起点设为0
                                }
                                else
                                {
                                    //chart.ChartAreas[0].AxisY.ScaleView.Position -= (int)xspmovepoints;  //缩放视图起点大于0，按比例缩放
                                    //chart.ChartAreas[0].AxisY.Minimum -= (int)ymouseponit;
                                    //chart.ChartAreas[0].AxisY.Maximum -= (int)ymouseponit;
                                }
                                if (yepmovepoints + yendpoint >= chart.ChartAreas[0].AxisY.Maximum)  //缩放视图终点大于曲线最大值
                                {
                                    //chart.ChartAreas[0].AxisY.ScaleView.Size = chart.ChartAreas[0].AxisY.Maximum - chart.ChartAreas[0].AxisY.ScaleView.Position;  //设置缩放视图尺寸=曲线最大值-视图起点值
                                    //chart.ChartAreas[0].AxisY.Maximum -= (int)ymouseponit; ;
                                    //chart.ChartAreas[0].AxisY.Minimum -= (int)ymouseponit;
                                }
                                else
                                {
                                    //double viewsizechange = xspmovepoints + xepmovepoints;         //计算x轴缩放视图缩小变化尺寸
                                    //chart.ChartAreas[0].AxisY.ScaleView.Size += (int)viewsizechange;   //按比例缩放视图大小
                                    chart.ChartAreas[0].AxisY.Minimum *= zoomfactor;
                                    chart.ChartAreas[0].AxisY.Maximum *= zoomfactor;
                                }
                            }
                            else
                            {
                                //double viewsizechange = xspmovepoints + xepmovepoints;         //计算x轴缩放视图缩小变化尺寸
                                //chart.ChartAreas[0].AxisY.ScaleView.Size += viewsizechange;   //按比例缩放视图大小
                                //chart.ChartAreas[0].AxisY.ScaleView.Position -= (int)xspmovepoints;   //按比例缩放视图大小
                                //chart.ChartAreas[0].AxisY.Maximum -= (int)ymouseponit; ;
                                //chart.ChartAreas[0].AxisY.Minimum -= (int)ymouseponit;
                                chart.ChartAreas[0].AxisY.Minimum *= zoomfactor;
                                chart.ChartAreas[0].AxisY.Maximum *= zoomfactor;
                            }
                        }
                        else
                        {
                            //chart.ChartAreas[0].AxisY.ScaleView.Size = chart.ChartAreas[0].AxisY.Maximum - chart.ChartAreas[0].AxisY.Minimum;
                            //chart.ChartAreas[0].AxisY.ScaleView.Position = chart.ChartAreas[0].AxisY.Minimum;
                            //chart.ChartAreas[0].AxisY.Minimum -= (int)ymouseponit;
                            //chart.ChartAreas[0].AxisY.Maximum -= (int)ymouseponit;
                            chart.ChartAreas[0].AxisY.Minimum *= zoomfactor;
                            chart.ChartAreas[0].AxisY.Maximum *= zoomfactor;
                        }
                    }
                }
                #endregion

                if (chart.ChartAreas[0].AxisX.Maximum > 60000)
                    chart.ChartAreas[0].AxisX.Maximum = 60000;
                if (chart.ChartAreas[0].AxisX.Minimum < -60000)
                    chart.ChartAreas[0].AxisX.Minimum = -60000;

                if (chart.ChartAreas[0].AxisY.Maximum > 60000)
                    chart.ChartAreas[0].AxisY.Maximum = 60000;
                if (chart.ChartAreas[0].AxisY.Minimum < -60000)
                    chart.ChartAreas[0].AxisY.Minimum = -60000;
            }
            catch (Exception ex) { LogError("放缩异常：" + ex.Message); }
        }
        //chart图放缩，功能还未实现
        private void chartTCPDisp_DoubleClick(object sender, EventArgs e)
        {
            //双击还原
            Chart chart = (Chart)sender;
            chart.ChartAreas[0].AxisX.Maximum = 60000;
            chart.ChartAreas[0].AxisX.Minimum = -60000;
            chart.ChartAreas[0].AxisY.Maximum = 60000;
            chart.ChartAreas[0].AxisY.Minimum = -60000;
            uiTrackBarRotate.Value = 0;
        }

        double downPtX; //记录鼠标按下的位置
        double downPtY;//记录鼠标按下的位置

        private void chartTCPDisp_MouseDown(object sender, MouseEventArgs e)
        {
            //查找鼠标点击的位置，判断是否超出chart图界限
            Chart chart = (Chart)sender;
           
            //if (chart.ChartAreas[0].AxisY.ScaleView.Size > 5)     //缩放视图不小于5
            {
                double xmouseponit = chart.ChartAreas[0].AxisX.PixelPositionToValue(e.X);    //获取鼠标在chart中x坐标
                double ymouseponit = chart.ChartAreas[0].AxisY.PixelPositionToValue(e.Y);    //获取鼠标在chart中y坐标
                if ((xmouseponit >= chart.ChartAreas[0].AxisX.ScaleView.ViewMinimum) && 
                    (xmouseponit <= chart.ChartAreas[0].AxisX.ScaleView.ViewMaximum) &&
                    (ymouseponit >= chart.ChartAreas[0].AxisY.ScaleView.ViewMinimum) && 
                    (ymouseponit <= chart.ChartAreas[0].AxisY.ScaleView.ViewMaximum)) //判断鼠标位置不在x,y轴两侧边沿
                {
                    downPtX = (int)xmouseponit;//(float)ev.X;
                    downPtY = (int)ymouseponit;//(float)ev.Y;
                    //double xspmovepoints = Math.Round((xmouseponit - xstartpoint) * (zoomfactor - 1) / zoomfactor, 1);    //计算x轴起点需要右移距离,保留一位小数
                    //double xepmovepoints = Math.Round(xendpoint - xmouseponit - xratio * (xmouseponit - xstartpoint - xspmovepoints), 1);    //计算x轴末端左移距离，保留一位小数
                    //double viewsizechange = xspmovepoints + xepmovepoints;         //计算x轴缩放视图缩小变化尺寸
                    //chart.ChartAreas[0].AxisX.ScaleView.Size -= viewsizechange;        //设置x轴缩放视图大小
                    //chart.ChartAreas[0].AxisX.ScaleView.Position += xspmovepoints;        //设置x轴缩放视图起点，右移保持鼠标中心点
                }
            }

        }

        //鼠标移动
        private void chartTCPDisp_MouseMove(object sender, MouseEventArgs e)
        {
            Chart chart = (Chart)sender;
            if (e.Button == MouseButtons.Left)
            {
                try
                {
                    //if (chart.ChartAreas[0].AxisY.ScaleView.Size > 5)     //缩放视图不小于5
                    {
                        double xmouseponit = chart.ChartAreas[0].AxisX.PixelPositionToValue(e.X);    //获取鼠标在chart中x坐标
                        double ymouseponit = chart.ChartAreas[0].AxisY.PixelPositionToValue(e.Y);    //获取鼠标在chart中y坐标
                        if ((xmouseponit >= chart.ChartAreas[0].AxisX.ScaleView.ViewMinimum) &&
                            (xmouseponit <= chart.ChartAreas[0].AxisX.ScaleView.ViewMaximum) &&
                            (ymouseponit >= chart.ChartAreas[0].AxisY.ScaleView.ViewMinimum) &&
                            (ymouseponit <= chart.ChartAreas[0].AxisY.ScaleView.ViewMaximum)) //判断鼠标位置不在x,y轴两侧边沿
                        {
                            chart.ChartAreas[0].AxisX.ScaleView.Position -= (int)(xmouseponit - downPtX);
                            chart.ChartAreas[0].AxisY.Minimum -= (int)(ymouseponit - downPtY);
                            chart.ChartAreas[0].AxisY.Maximum -= (int)(ymouseponit - downPtY);
                            chart.ChartAreas[0].AxisX.Minimum -= (int)(xmouseponit - downPtX);
                            chart.ChartAreas[0].AxisX.Maximum -= (int)(xmouseponit - downPtX);
                        }
                    }
                }
                catch (Exception ex) { updaRichBox("[Error] 鼠标移动错误！ " + ex.Message); }

            }
            else if (e.Button == MouseButtons.Right)
            {
                try
                {
                    double xmouseponit = chart.ChartAreas[0].AxisX.PixelPositionToValue(e.X);    //获取鼠标在chart中x坐标
                    double ymouseponit = chart.ChartAreas[0].AxisY.PixelPositionToValue(e.Y);    //获取鼠标在chart中y坐标
                    if ((xmouseponit >= chart.ChartAreas[0].AxisX.ScaleView.ViewMinimum) &&
                        (xmouseponit <= chart.ChartAreas[0].AxisX.ScaleView.ViewMaximum) &&
                        (ymouseponit >= chart.ChartAreas[0].AxisY.ScaleView.ViewMinimum) &&
                        (ymouseponit <= chart.ChartAreas[0].AxisY.ScaleView.ViewMaximum)) //判断鼠标位置不在x,y轴两侧边沿
                    {
                        //计算起始点和终点角度
                        double angleEnd = Math.Atan2((ymouseponit - 0), (xmouseponit - 0)) * 180 / Math.PI;
                        double angleStart = Math.Atan2((downPtY - 0), (downPtX - 0)) * 180 / Math.PI;
                        //超过180度需要考虑角度转换
                        if (angleEnd - angleStart < -30)
                            angleEnd+=360;
                        else if(angleEnd - angleStart > 30)
                            angleEnd -= 360;

                        uiTrackBarRotate.Value += (int)((angleEnd - angleStart) * 10);
                        downPtY = ymouseponit;//移动点角度赋予起始点，避免在累加过程中累加数值越来越大
                        downPtX = xmouseponit;//移动点角度赋予起始点，避免在累加过程中累加数值越来越大

                    }
                }
                catch (Exception ex) { updaRichBox("[Error] 图像旋转错误！ " + ex.Message); }
            }

        }

        /// <summary>
        ///文本框与滚动条数据绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiTrackBarRate_ValueChanged(object sender, EventArgs e)
        {
            numericUpDownRate.Text = ((double)(uiTrackBarRotate.Value) / 10.0).ToString("F1");//获取旋转角度
            try
            {
                if (bAccpet == false && dataQueueY.Count == laserPointCount && dataQueueX.Count == laserPointCount)
                //if (bAccpet == false && dataQueueY.Count == 274 && dataQueueX.Count == 274)//getRecevice == null &&
                {
                    this.ChartTest.Series[0].Points.Clear();
                    this.ChartTest.Series[1].Points.Clear();
                    this.ChartTest.Series[2].Points.Clear();
                    //double angle = double.Parse(numericUpDownRate.Text) - 42;//顺时针,减去42度是 90-96/2=42.移动至X轴平行
                    double angle = double.Parse(numericUpDownRate.Text) - (90 - lasetAngle / 2);
                    double dx = 0;
                    double dy = 0;//围绕某个点旋转
                    for (int i = 0; i < dataQueueY.Count; i++)//dataQueueY.Count=274
                    {
                        double xx = (dataQueueX.ElementAt(i) - dx) * Math.Cos(angle * Math.PI / 180) - (dataQueueY.ElementAt(i) - dy) * Math.Sin(angle * Math.PI / 180) + dx;
                        double yy = (dataQueueY.ElementAt(i) - dy) * Math.Cos(angle * Math.PI / 180) + (dataQueueX.ElementAt(i) - dx) * Math.Sin(angle * Math.PI / 180) + dy;
                        this.ChartTest.Series[0].Points.AddXY((int)xx, (int)yy);
                        //this.ChartTest.Series[0].Points.AddXY(dataQueueX.ElementAt(i), dataQueueY.ElementAt(i));
                    }

                    this.ChartTest.Series[1].Points.AddXY((dataQueueX.ElementAt(0) - dx) * Math.Cos(angle * Math.PI / 180) - (dataQueueY.ElementAt(0) - dy) * Math.Sin(angle * Math.PI / 180) + dx,
                                    (dataQueueY.ElementAt(0) - dy) * Math.Cos(angle * Math.PI / 180) + (dataQueueX.ElementAt(0) - dx) * Math.Sin(angle * Math.PI / 180) + dy);
                    this.ChartTest.Series[1].Points.AddXY(0, 0);
                    //this.ChartTest.Series[2].Points.AddXY(dataQueueX.ElementAt(dataQueueX.Count - 1), dataQueueY.ElementAt(dataQueueY.Count - 1));
                    this.ChartTest.Series[2].Points.AddXY((dataQueueX.ElementAt(dataQueueX.Count - 1) - dx) * Math.Cos(angle * Math.PI / 180) - (dataQueueY.ElementAt(dataQueueX.Count - 1) - dy) * Math.Sin(angle * Math.PI / 180) + dx,
                                    (dataQueueY.ElementAt(dataQueueX.Count - 1) - dy) * Math.Cos(angle * Math.PI / 180) + (dataQueueX.ElementAt(dataQueueX.Count - 1) - dx) * Math.Sin(angle * Math.PI / 180) + dy);
                    this.ChartTest.Series[2].Points.AddXY(0, 0);

                    updaRichBox("StartPoint: x = " + dataQueueX.ElementAt(0) + " y = " + dataQueueY.ElementAt(0) + "   EndPoint: x = " + dataQueueY.ElementAt(dataQueueY.Count - 1) + " y = " + dataQueueY.ElementAt(dataQueueY.Count - 1) + "\r\n");
                }
            }
            catch { }
        }
        /// <summary>
        /// 文本框按下enter按钮，将数据传给滚动条
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numericUpDownRate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                uiTrackBarRotate.Value = (int)(double.Parse(numericUpDownRate.Text) * 10);
            }
        }

        /// <summary>
        /// 16进制string类型数据转byte数据
        /// </summary>
        /// <param name="hexString">输入16进制string</param>
        /// <returns>byte数组</returns>
        private static byte[] HexStrTobyte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2).Trim(), 16);
            return returnBytes;
        }
        public static string CutByteString(string str, int startIndex, int len)

        {
            string result = string.Empty;// 最终返回的结果
            if (string.IsNullOrEmpty(str)) { return result; }
            int byteLen = System.Text.Encoding.Default.GetByteCount(str);// 单字节字符长度
            int charLen = str.Length;// 把字符平等对待时的字符串长度
            if (startIndex == 0)
            { return CutByteString(str, len); }
            else if (startIndex >= byteLen)
            { return result; }
            else //startIndex < byteLen
            {
                int AllLen = startIndex + len;
                int byteCountStart = 0;// 记录读取进度
                int byteCountEnd = 0;// 记录读取进度
                int startpos = 0;// 记录截取位置                
                int endpos = 0;// 记录截取位置
                for (int i = 0; i < charLen; i++)
                {
                    if (Convert.ToInt32(str.ToCharArray()[i]) > 255)// 按中文字符计算加2
                    { byteCountStart += 2; }
                    else// 按英文字符计算加1
                    { byteCountStart += 1; }
                    if (byteCountStart > startIndex)// 超出时只记下上一个有效位置
                    {
                        startpos = i;
                        AllLen = startIndex + len - 1;
                        break;
                    }
                    else if (byteCountStart == startIndex)// 记下当前位置
                    {
                        startpos = i + 1;
                        break;
                    }
                }
                if (startIndex + len <= byteLen)//截取字符在总长以内
                {
                    for (int i = 0; i < charLen; i++)
                    {
                        if (Convert.ToInt32(str.ToCharArray()[i]) > 255)// 按中文字符计算加2
                        { byteCountEnd += 2; }
                        else// 按英文字符计算加1
                        { byteCountEnd += 1; }
                        if (byteCountEnd > AllLen)// 超出时只记下上一个有效位置
                        {
                            endpos = i;
                            break;
                        }
                        else if (byteCountEnd == AllLen)// 记下当前位置
                        {
                            endpos = i + 1;
                            break;
                        }
                    }
                    endpos = endpos - startpos;
                }
                else if (startIndex + len > byteLen)//截取字符超出总长
                {
                    endpos = charLen - startpos;
                }
                if (endpos >= 0)
                { result = str.Substring(startpos, endpos); }
            }
            return result;
        }
        public static string CutByteString(string str, int len)
        {
            string result = string.Empty;// 最终返回的结果
            if (string.IsNullOrEmpty(str)) { return result; }
            int byteLen = System.Text.Encoding.Default.GetByteCount(str);// 单字节字符长度
            int charLen = str.Length;// 把字符平等对待时的字符串长度
            int byteCount = 0;// 记录读取进度
            int pos = 0;// 记录截取位置
            if (byteLen > len)
            {
                for (int i = 0; i < charLen; i++)
                {
                    if (Convert.ToInt32(str.ToCharArray()[i]) > 255)// 按中文字符计算加2
                    { byteCount += 2; }
                    else// 按英文字符计算加1
                    { byteCount += 1; }
                    if (byteCount > len)// 超出时只记下上一个有效位置
                    {
                        pos = i;
                        break;
                    }
                    else if (byteCount == len)// 记下当前位置
                    {
                        pos = i + 1;
                        break;
                    }
                }
                if (pos >= 0)
                { result = str.Substring(0, pos); }
            }
            else
            { result = str; }
            return result;
        }
        private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbType.SelectedIndex == 0)//921
            {
                splitStr = "FC FD FE FF ";//FC FD FE FF || AA 01 80 01 
                lasetAngle = 96;
                laserPointCount = 274;
                uiTextBox1.Text = "20";
                txtStart.Text = "15";
                txtEnd.Text = "270";
                txtCalc.Text = "200";
            }
            else if (cbType.SelectedIndex == 1)
            {
                splitStr = "BE A0 12 34 ";//951
                lasetAngle = 108;
                laserPointCount = 400;
                uiTextBox1.Text = "50";
                txtStart.Text = "210";
                txtEnd.Text = "360";
                txtCalc.Text = "300";
            }
        }
    }
}
