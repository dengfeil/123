namespace TrafficSurveyTool
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.uiTabControl1 = new Sunny.UI.UITabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.rbDataLog = new System.Windows.Forms.RichTextBox();
            this.uiLabel23 = new Sunny.UI.UILabel();
            this.numericUpDownRate = new Sunny.UI.UITextBox();
            this.uiLabel22 = new Sunny.UI.UILabel();
            this.txtCalc = new Sunny.UI.UITextBox();
            this.uiLabel7 = new Sunny.UI.UILabel();
            this.uiTrackBarRotate = new Sunny.UI.UITrackBar();
            this.uiLabel15 = new Sunny.UI.UILabel();
            this.txtEnd = new Sunny.UI.UITextBox();
            this.txtStart = new Sunny.UI.UITextBox();
            this.uiTextBox1 = new Sunny.UI.UITextBox();
            this.tbStartDistance = new Sunny.UI.UITextBox();
            this.txtLength = new Sunny.UI.UITextBox();
            this.checkBoxHexDisplay = new Sunny.UI.UICheckBox();
            this.btnStop = new Sunny.UI.UIButton();
            this.btnStart = new Sunny.UI.UIButton();
            this.btnRefresh = new Sunny.UI.UIButton();
            this.btnConnect = new Sunny.UI.UIButton();
            this.uiLabel1 = new Sunny.UI.UILabel();
            this.uiLabel3 = new Sunny.UI.UILabel();
            this.uiLabel2 = new Sunny.UI.UILabel();
            this.uiLabel6 = new Sunny.UI.UILabel();
            this.cbType = new Sunny.UI.UIComboBox();
            this.cbSerialPortName = new Sunny.UI.UIComboBox();
            this.chartUDPDisp = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.uiContextMenuStrip1 = new Sunny.UI.UIContextMenuStrip();
            this.删除选中行ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.数据保存CSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.数据保存ExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.uiTabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartUDPDisp)).BeginInit();
            this.uiContextMenuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // uiTabControl1
            // 
            this.uiTabControl1.Controls.Add(this.tabPage2);
            this.uiTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiTabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.uiTabControl1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiTabControl1.ItemSize = new System.Drawing.Size(150, 40);
            this.uiTabControl1.Location = new System.Drawing.Point(0, 0);
            this.uiTabControl1.MenuStyle = Sunny.UI.UIMenuStyle.Custom;
            this.uiTabControl1.Name = "uiTabControl1";
            this.uiTabControl1.SelectedIndex = 0;
            this.uiTabControl1.Size = new System.Drawing.Size(1341, 633);
            this.uiTabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.uiTabControl1.TabBackColor = System.Drawing.Color.SlateGray;
            this.uiTabControl1.TabIndex = 1;
            this.uiTabControl1.TabSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.tabPage2.Controls.Add(this.splitContainer2);
            this.tabPage2.Location = new System.Drawing.Point(0, 40);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(1341, 593);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "串口点云图";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.splitContainer2.Panel1.Controls.Add(this.rbDataLog);
            this.splitContainer2.Panel1.Controls.Add(this.uiLabel23);
            this.splitContainer2.Panel1.Controls.Add(this.numericUpDownRate);
            this.splitContainer2.Panel1.Controls.Add(this.uiLabel22);
            this.splitContainer2.Panel1.Controls.Add(this.txtCalc);
            this.splitContainer2.Panel1.Controls.Add(this.uiLabel7);
            this.splitContainer2.Panel1.Controls.Add(this.uiTrackBarRotate);
            this.splitContainer2.Panel1.Controls.Add(this.uiLabel15);
            this.splitContainer2.Panel1.Controls.Add(this.txtEnd);
            this.splitContainer2.Panel1.Controls.Add(this.txtStart);
            this.splitContainer2.Panel1.Controls.Add(this.uiTextBox1);
            this.splitContainer2.Panel1.Controls.Add(this.tbStartDistance);
            this.splitContainer2.Panel1.Controls.Add(this.txtLength);
            this.splitContainer2.Panel1.Controls.Add(this.checkBoxHexDisplay);
            this.splitContainer2.Panel1.Controls.Add(this.btnStop);
            this.splitContainer2.Panel1.Controls.Add(this.btnStart);
            this.splitContainer2.Panel1.Controls.Add(this.btnRefresh);
            this.splitContainer2.Panel1.Controls.Add(this.btnConnect);
            this.splitContainer2.Panel1.Controls.Add(this.uiLabel1);
            this.splitContainer2.Panel1.Controls.Add(this.uiLabel3);
            this.splitContainer2.Panel1.Controls.Add(this.uiLabel2);
            this.splitContainer2.Panel1.Controls.Add(this.uiLabel6);
            this.splitContainer2.Panel1.Controls.Add(this.cbType);
            this.splitContainer2.Panel1.Controls.Add(this.cbSerialPortName);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.chartUDPDisp);
            this.splitContainer2.Size = new System.Drawing.Size(1341, 593);
            this.splitContainer2.SplitterDistance = 235;
            this.splitContainer2.TabIndex = 0;
            // 
            // rbDataLog
            // 
            this.rbDataLog.Location = new System.Drawing.Point(149, 552);
            this.rbDataLog.Name = "rbDataLog";
            this.rbDataLog.Size = new System.Drawing.Size(74, 44);
            this.rbDataLog.TabIndex = 10;
            this.rbDataLog.Text = "";
            this.rbDataLog.Visible = false;
            // 
            // uiLabel23
            // 
            this.uiLabel23.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiLabel23.Location = new System.Drawing.Point(159, 414);
            this.uiLabel23.Name = "uiLabel23";
            this.uiLabel23.Size = new System.Drawing.Size(165, 28);
            this.uiLabel23.TabIndex = 8;
            this.uiLabel23.Text = "判断测量开始点";
            this.uiLabel23.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numericUpDownRate
            // 
            this.numericUpDownRate.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.numericUpDownRate.FillColor = System.Drawing.Color.White;
            this.numericUpDownRate.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.numericUpDownRate.Location = new System.Drawing.Point(28, 552);
            this.numericUpDownRate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numericUpDownRate.Maximum = 2147483647D;
            this.numericUpDownRate.Minimum = -2147483648D;
            this.numericUpDownRate.Name = "numericUpDownRate";
            this.numericUpDownRate.Padding = new System.Windows.Forms.Padding(5);
            this.numericUpDownRate.RectColor = System.Drawing.Color.Empty;
            this.numericUpDownRate.Size = new System.Drawing.Size(83, 34);
            this.numericUpDownRate.Style = Sunny.UI.UIStyle.Custom;
            this.numericUpDownRate.TabIndex = 9;
            this.numericUpDownRate.Text = "0";
            this.numericUpDownRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericUpDownRate_KeyPress);
            // 
            // uiLabel22
            // 
            this.uiLabel22.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiLabel22.Location = new System.Drawing.Point(88, 418);
            this.uiLabel22.Name = "uiLabel22";
            this.uiLabel22.Size = new System.Drawing.Size(65, 23);
            this.uiLabel22.TabIndex = 8;
            this.uiLabel22.Text = "终点";
            this.uiLabel22.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCalc
            // 
            this.txtCalc.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtCalc.DoubleValue = 200D;
            this.txtCalc.FillColor = System.Drawing.Color.White;
            this.txtCalc.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.txtCalc.IntValue = 200;
            this.txtCalc.Location = new System.Drawing.Point(164, 446);
            this.txtCalc.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtCalc.Maximum = 2147483647D;
            this.txtCalc.Minimum = -2147483648D;
            this.txtCalc.Name = "txtCalc";
            this.txtCalc.Padding = new System.Windows.Forms.Padding(5);
            this.txtCalc.Size = new System.Drawing.Size(63, 34);
            this.txtCalc.TabIndex = 7;
            this.txtCalc.Text = "200";
            // 
            // uiLabel7
            // 
            this.uiLabel7.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiLabel7.Location = new System.Drawing.Point(23, 485);
            this.uiLabel7.Name = "uiLabel7";
            this.uiLabel7.Size = new System.Drawing.Size(200, 27);
            this.uiLabel7.TabIndex = 6;
            this.uiLabel7.Text = "旋转角度";
            this.uiLabel7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiTrackBarRotate
            // 
            this.uiTrackBarRotate.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiTrackBarRotate.Location = new System.Drawing.Point(18, 515);
            this.uiTrackBarRotate.Maximum = 3600;
            this.uiTrackBarRotate.Name = "uiTrackBarRotate";
            this.uiTrackBarRotate.Size = new System.Drawing.Size(247, 29);
            this.uiTrackBarRotate.TabIndex = 4;
            this.uiTrackBarRotate.Text = "uiTrackBar1";
            this.uiTrackBarRotate.ValueChanged += new System.EventHandler(this.uiTrackBarRate_ValueChanged);
            // 
            // uiLabel15
            // 
            this.uiLabel15.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiLabel15.Location = new System.Drawing.Point(9, 418);
            this.uiLabel15.Name = "uiLabel15";
            this.uiLabel15.Size = new System.Drawing.Size(73, 23);
            this.uiLabel15.TabIndex = 8;
            this.uiLabel15.Text = "起点";
            this.uiLabel15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtEnd
            // 
            this.txtEnd.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtEnd.DoubleValue = 270D;
            this.txtEnd.FillColor = System.Drawing.Color.White;
            this.txtEnd.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.txtEnd.IntValue = 270;
            this.txtEnd.Location = new System.Drawing.Point(86, 446);
            this.txtEnd.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtEnd.Maximum = 2147483647D;
            this.txtEnd.Minimum = -2147483648D;
            this.txtEnd.Name = "txtEnd";
            this.txtEnd.Padding = new System.Windows.Forms.Padding(5);
            this.txtEnd.Size = new System.Drawing.Size(63, 34);
            this.txtEnd.TabIndex = 7;
            this.txtEnd.Text = "270";
            // 
            // txtStart
            // 
            this.txtStart.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtStart.DoubleValue = 15D;
            this.txtStart.FillColor = System.Drawing.Color.White;
            this.txtStart.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.txtStart.IntValue = 15;
            this.txtStart.Location = new System.Drawing.Point(16, 446);
            this.txtStart.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtStart.Maximum = 2147483647D;
            this.txtStart.Minimum = -2147483648D;
            this.txtStart.Name = "txtStart";
            this.txtStart.Padding = new System.Windows.Forms.Padding(5);
            this.txtStart.Size = new System.Drawing.Size(61, 34);
            this.txtStart.TabIndex = 7;
            this.txtStart.Text = "15";
            // 
            // uiTextBox1
            // 
            this.uiTextBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.uiTextBox1.DoubleValue = 50D;
            this.uiTextBox1.FillColor = System.Drawing.Color.White;
            this.uiTextBox1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiTextBox1.IntValue = 50;
            this.uiTextBox1.Location = new System.Drawing.Point(187, 266);
            this.uiTextBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiTextBox1.Maximum = 2147483647D;
            this.uiTextBox1.Minimum = -2147483648D;
            this.uiTextBox1.Name = "uiTextBox1";
            this.uiTextBox1.Padding = new System.Windows.Forms.Padding(5);
            this.uiTextBox1.Size = new System.Drawing.Size(63, 34);
            this.uiTextBox1.TabIndex = 7;
            this.uiTextBox1.Text = "50";
            // 
            // tbStartDistance
            // 
            this.tbStartDistance.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbStartDistance.DoubleValue = 6400D;
            this.tbStartDistance.FillColor = System.Drawing.Color.White;
            this.tbStartDistance.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.tbStartDistance.IntValue = 6400;
            this.tbStartDistance.Location = new System.Drawing.Point(117, 361);
            this.tbStartDistance.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbStartDistance.Maximum = 2147483647D;
            this.tbStartDistance.Minimum = -2147483648D;
            this.tbStartDistance.Name = "tbStartDistance";
            this.tbStartDistance.Padding = new System.Windows.Forms.Padding(5);
            this.tbStartDistance.Size = new System.Drawing.Size(74, 34);
            this.tbStartDistance.TabIndex = 7;
            this.tbStartDistance.Text = "6400";
            // 
            // txtLength
            // 
            this.txtLength.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtLength.FillColor = System.Drawing.Color.White;
            this.txtLength.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.txtLength.Location = new System.Drawing.Point(117, 320);
            this.txtLength.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtLength.Maximum = 2147483647D;
            this.txtLength.Minimum = -2147483648D;
            this.txtLength.Name = "txtLength";
            this.txtLength.Padding = new System.Windows.Forms.Padding(5);
            this.txtLength.Size = new System.Drawing.Size(74, 34);
            this.txtLength.TabIndex = 7;
            this.txtLength.Text = "0";
            // 
            // checkBoxHexDisplay
            // 
            this.checkBoxHexDisplay.Checked = true;
            this.checkBoxHexDisplay.Cursor = System.Windows.Forms.Cursors.Hand;
            this.checkBoxHexDisplay.Enabled = false;
            this.checkBoxHexDisplay.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.checkBoxHexDisplay.Location = new System.Drawing.Point(141, 104);
            this.checkBoxHexDisplay.Name = "checkBoxHexDisplay";
            this.checkBoxHexDisplay.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.checkBoxHexDisplay.Size = new System.Drawing.Size(74, 29);
            this.checkBoxHexDisplay.TabIndex = 3;
            this.checkBoxHexDisplay.Text = "Hex";
            // 
            // btnStop
            // 
            this.btnStop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStop.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnStop.Location = new System.Drawing.Point(123, 212);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(100, 35);
            this.btnStop.TabIndex = 2;
            this.btnStop.Text = "关闭";
            this.btnStop.Click += new System.EventHandler(this.Stop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStart.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnStart.Location = new System.Drawing.Point(14, 212);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(100, 35);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "开启";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnRefresh.Location = new System.Drawing.Point(123, 152);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 35);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "刷新串口";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConnect.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnConnect.Location = new System.Drawing.Point(14, 152);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(100, 35);
            this.btnConnect.TabIndex = 2;
            this.btnConnect.Text = "连接串口";
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // uiLabel1
            // 
            this.uiLabel1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiLabel1.Location = new System.Drawing.Point(22, 4);
            this.uiLabel1.Name = "uiLabel1";
            this.uiLabel1.Size = new System.Drawing.Size(100, 23);
            this.uiLabel1.TabIndex = 1;
            this.uiLabel1.Text = "雷达型号";
            this.uiLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel3
            // 
            this.uiLabel3.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiLabel3.Location = new System.Drawing.Point(12, 359);
            this.uiLabel3.Name = "uiLabel3";
            this.uiLabel3.Size = new System.Drawing.Size(123, 36);
            this.uiLabel3.TabIndex = 1;
            this.uiLabel3.Text = "原点距离：";
            this.uiLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel2
            // 
            this.uiLabel2.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiLabel2.Location = new System.Drawing.Point(12, 318);
            this.uiLabel2.Name = "uiLabel2";
            this.uiLabel2.Size = new System.Drawing.Size(123, 36);
            this.uiLabel2.TabIndex = 1;
            this.uiLabel2.Text = "测量尺寸：";
            this.uiLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel6
            // 
            this.uiLabel6.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiLabel6.Location = new System.Drawing.Point(12, 264);
            this.uiLabel6.Name = "uiLabel6";
            this.uiLabel6.Size = new System.Drawing.Size(179, 36);
            this.uiLabel6.TabIndex = 1;
            this.uiLabel6.Text = "接收信息间隔(ms)";
            this.uiLabel6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbType
            // 
            this.cbType.FillColor = System.Drawing.Color.White;
            this.cbType.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.cbType.Items.AddRange(new object[] {
            "U921",
            "U951"});
            this.cbType.Location = new System.Drawing.Point(18, 32);
            this.cbType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbType.MinimumSize = new System.Drawing.Size(63, 0);
            this.cbType.Name = "cbType";
            this.cbType.Padding = new System.Windows.Forms.Padding(0, 0, 30, 0);
            this.cbType.RectColor = System.Drawing.Color.Blue;
            this.cbType.Size = new System.Drawing.Size(117, 34);
            this.cbType.Style = Sunny.UI.UIStyle.Custom;
            this.cbType.TabIndex = 0;
            this.cbType.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cbType.SelectedIndexChanged += new System.EventHandler(this.cbType_SelectedIndexChanged);
            // 
            // cbSerialPortName
            // 
            this.cbSerialPortName.FillColor = System.Drawing.Color.White;
            this.cbSerialPortName.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.cbSerialPortName.Location = new System.Drawing.Point(17, 99);
            this.cbSerialPortName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbSerialPortName.MinimumSize = new System.Drawing.Size(63, 0);
            this.cbSerialPortName.Name = "cbSerialPortName";
            this.cbSerialPortName.Padding = new System.Windows.Forms.Padding(0, 0, 30, 0);
            this.cbSerialPortName.RectColor = System.Drawing.Color.Blue;
            this.cbSerialPortName.Size = new System.Drawing.Size(117, 34);
            this.cbSerialPortName.Style = Sunny.UI.UIStyle.Custom;
            this.cbSerialPortName.TabIndex = 0;
            this.cbSerialPortName.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chartUDPDisp
            // 
            this.chartUDPDisp.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.chartUDPDisp.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.TopBottom;
            chartArea1.BackColor = System.Drawing.Color.Lavender;
            chartArea1.Name = "ChartArea1";
            this.chartUDPDisp.ChartAreas.Add(chartArea1);
            this.chartUDPDisp.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chartUDPDisp.Legends.Add(legend1);
            this.chartUDPDisp.Location = new System.Drawing.Point(0, 0);
            this.chartUDPDisp.Name = "chartUDPDisp";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartUDPDisp.Series.Add(series1);
            this.chartUDPDisp.Size = new System.Drawing.Size(1102, 593);
            this.chartUDPDisp.TabIndex = 1;
            this.chartUDPDisp.Text = "chart2";
            this.chartUDPDisp.DoubleClick += new System.EventHandler(this.chartTCPDisp_DoubleClick);
            this.chartUDPDisp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chartTCPDisp_MouseDown);
            this.chartUDPDisp.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chartTCPDisp_MouseMove);
            this.chartUDPDisp.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.chart_MouseWheelX);
            // 
            // uiContextMenuStrip1
            // 
            this.uiContextMenuStrip1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiContextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.uiContextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.删除选中行ToolStripMenuItem,
            this.数据保存CSVToolStripMenuItem,
            this.数据保存ExcelToolStripMenuItem});
            this.uiContextMenuStrip1.Name = "uiContextMenuStrip1";
            this.uiContextMenuStrip1.Size = new System.Drawing.Size(212, 100);
            // 
            // 删除选中行ToolStripMenuItem
            // 
            this.删除选中行ToolStripMenuItem.Name = "删除选中行ToolStripMenuItem";
            this.删除选中行ToolStripMenuItem.Size = new System.Drawing.Size(211, 32);
            this.删除选中行ToolStripMenuItem.Text = "删除选中行";
            // 
            // 数据保存CSVToolStripMenuItem
            // 
            this.数据保存CSVToolStripMenuItem.Name = "数据保存CSVToolStripMenuItem";
            this.数据保存CSVToolStripMenuItem.Size = new System.Drawing.Size(211, 32);
            this.数据保存CSVToolStripMenuItem.Text = "数据保存CSV";
            // 
            // 数据保存ExcelToolStripMenuItem
            // 
            this.数据保存ExcelToolStripMenuItem.Name = "数据保存ExcelToolStripMenuItem";
            this.数据保存ExcelToolStripMenuItem.Size = new System.Drawing.Size(211, 32);
            this.数据保存ExcelToolStripMenuItem.Text = "数据保存Excel";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 633);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1341, 26);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(167, 20);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1341, 659);
            this.Controls.Add(this.uiTabControl1);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainWindow";
            this.Text = "MainWindow";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainWindow_FormClosed);
            this.uiTabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartUDPDisp)).EndInit();
            this.uiContextMenuStrip1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Sunny.UI.UITabControl uiTabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private Sunny.UI.UIButton btnRefresh;
        private Sunny.UI.UIButton btnConnect;
        private Sunny.UI.UILabel uiLabel6;
        private Sunny.UI.UIComboBox cbSerialPortName;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartUDPDisp;
        private Sunny.UI.UICheckBox checkBoxHexDisplay;
        private Sunny.UI.UIButton btnStart;
        private Sunny.UI.UIButton btnStop;
        private Sunny.UI.UILabel uiLabel7;
        private Sunny.UI.UITrackBar uiTrackBarRotate;
        private Sunny.UI.UITextBox numericUpDownRate;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolTip toolTip1;
        private Sunny.UI.UIContextMenuStrip uiContextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 删除选中行ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 数据保存CSVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 数据保存ExcelToolStripMenuItem;
        private Sunny.UI.UITextBox txtLength;
        private Sunny.UI.UILabel uiLabel22;
        private Sunny.UI.UILabel uiLabel15;
        private Sunny.UI.UITextBox txtEnd;
        private Sunny.UI.UITextBox txtStart;
        private Sunny.UI.UILabel uiLabel23;
        private Sunny.UI.UITextBox txtCalc;
        private Sunny.UI.UITextBox uiTextBox1;
        private Sunny.UI.UILabel uiLabel1;
        private Sunny.UI.UIComboBox cbType;
        private System.Windows.Forms.RichTextBox rbDataLog;
        private Sunny.UI.UILabel uiLabel2;
        private Sunny.UI.UITextBox tbStartDistance;
        private Sunny.UI.UILabel uiLabel3;
    }
}