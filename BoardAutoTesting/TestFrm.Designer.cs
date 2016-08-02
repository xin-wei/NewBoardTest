namespace BoardAutoTesting
{
    partial class TestFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestFrm));
            this.ribbonControl1 = new DevComponents.DotNetBar.RibbonControl();
            this.ribbonPanel1 = new DevComponents.DotNetBar.RibbonPanel();
            this.ribbonBar3 = new DevComponents.DotNetBar.RibbonBar();
            this.labelItem1 = new DevComponents.DotNetBar.LabelItem();
            this.txtBarcode = new DevComponents.DotNetBar.TextBoxItem();
            this.lblStatus = new DevComponents.DotNetBar.LabelItem();
            this.ribbonBar1 = new DevComponents.DotNetBar.RibbonBar();
            this.btnStartServer = new DevComponents.DotNetBar.ButtonItem();
            this.btnStopServer = new DevComponents.DotNetBar.ButtonItem();
            this.lblUser = new DevComponents.DotNetBar.LabelItem();
            this.btnLogin = new DevComponents.DotNetBar.ButtonItem();
            this.ribbonBar2 = new DevComponents.DotNetBar.RibbonBar();
            this.btnSetting = new DevComponents.DotNetBar.ButtonItem();
            this.btnReset = new DevComponents.DotNetBar.ButtonItem();
            this.ribbonPanel2 = new DevComponents.DotNetBar.RibbonPanel();
            this.ribbonBar4 = new DevComponents.DotNetBar.RibbonBar();
            this.btnPrintTest = new DevComponents.DotNetBar.ButtonItem();
            this.ribbonTabItem1 = new DevComponents.DotNetBar.RibbonTabItem();
            this.Print_Test = new DevComponents.DotNetBar.RibbonTabItem();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.cbxPortName = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.cbxBaudRate = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnConnect = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.txtIp = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel5 = new System.Windows.Forms.ToolStripLabel();
            this.txtPort = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel6 = new System.Windows.Forms.ToolStripLabel();
            this.lblCraft = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel8 = new System.Windows.Forms.ToolStripLabel();
            this.lblLine = new System.Windows.Forms.ToolStripLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel10 = new System.Windows.Forms.ToolStripLabel();
            this.lblWorkOrder = new System.Windows.Forms.ToolStripLabel();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.craft16 = new CraftControl.UserCraft();
            this.craft15 = new CraftControl.UserCraft();
            this.craft14 = new CraftControl.UserCraft();
            this.craft13 = new CraftControl.UserCraft();
            this.craft12 = new CraftControl.UserCraft();
            this.craft11 = new CraftControl.UserCraft();
            this.craft10 = new CraftControl.UserCraft();
            this.craft9 = new CraftControl.UserCraft();
            this.craft8 = new CraftControl.UserCraft();
            this.craft7 = new CraftControl.UserCraft();
            this.craft6 = new CraftControl.UserCraft();
            this.craft5 = new CraftControl.UserCraft();
            this.craft4 = new CraftControl.UserCraft();
            this.craft3 = new CraftControl.UserCraft();
            this.craft2 = new CraftControl.UserCraft();
            this.craft1 = new CraftControl.UserCraft();
            this.sysInfoSource = new System.Windows.Forms.BindingSource(this.components);
            this.ribbonControl1.SuspendLayout();
            this.ribbonPanel1.SuspendLayout();
            this.ribbonPanel2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sysInfoSource)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            // 
            // 
            // 
            this.ribbonControl1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ribbonControl1.CanCustomize = false;
            this.ribbonControl1.CaptionVisible = true;
            this.ribbonControl1.Controls.Add(this.ribbonPanel1);
            this.ribbonControl1.Controls.Add(this.ribbonPanel2);
            this.ribbonControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ribbonControl1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.ribbonTabItem1,
            this.Print_Test});
            this.ribbonControl1.KeyTipsFont = new System.Drawing.Font("Tahoma", 7F);
            this.ribbonControl1.Location = new System.Drawing.Point(5, 1);
            this.ribbonControl1.Margin = new System.Windows.Forms.Padding(4);
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.ribbonControl1.Size = new System.Drawing.Size(1394, 192);
            this.ribbonControl1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ribbonControl1.SystemText.MaximizeRibbonText = "&Maximize the Ribbon";
            this.ribbonControl1.SystemText.MinimizeRibbonText = "Mi&nimize the Ribbon";
            this.ribbonControl1.SystemText.QatAddItemText = "&Add to Quick Access Toolbar";
            this.ribbonControl1.SystemText.QatCustomizeMenuLabel = "<b>Customize Quick Access Toolbar</b>";
            this.ribbonControl1.SystemText.QatCustomizeText = "&Customize Quick Access Toolbar...";
            this.ribbonControl1.SystemText.QatDialogAddButton = "&Add >>";
            this.ribbonControl1.SystemText.QatDialogCancelButton = "Cancel";
            this.ribbonControl1.SystemText.QatDialogCaption = "Customize Quick Access Toolbar";
            this.ribbonControl1.SystemText.QatDialogCategoriesLabel = "&Choose commands from:";
            this.ribbonControl1.SystemText.QatDialogOkButton = "OK";
            this.ribbonControl1.SystemText.QatDialogPlacementCheckbox = "&Place Quick Access Toolbar below the Ribbon";
            this.ribbonControl1.SystemText.QatDialogRemoveButton = "&Remove";
            this.ribbonControl1.SystemText.QatPlaceAboveRibbonText = "&Place Quick Access Toolbar above the Ribbon";
            this.ribbonControl1.SystemText.QatPlaceBelowRibbonText = "&Place Quick Access Toolbar below the Ribbon";
            this.ribbonControl1.SystemText.QatRemoveItemText = "&Remove from Quick Access Toolbar";
            this.ribbonControl1.TabGroupHeight = 14;
            this.ribbonControl1.TabIndex = 18;
            // 
            // ribbonPanel1
            // 
            this.ribbonPanel1.AutoSize = true;
            this.ribbonPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ribbonPanel1.Controls.Add(this.ribbonBar3);
            this.ribbonPanel1.Controls.Add(this.ribbonBar1);
            this.ribbonPanel1.Controls.Add(this.ribbonBar2);
            this.ribbonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ribbonPanel1.Location = new System.Drawing.Point(0, 59);
            this.ribbonPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.ribbonPanel1.Name = "ribbonPanel1";
            this.ribbonPanel1.Padding = new System.Windows.Forms.Padding(4, 0, 4, 4);
            this.ribbonPanel1.Size = new System.Drawing.Size(1394, 129);
            // 
            // 
            // 
            this.ribbonPanel1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.ribbonPanel1.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.ribbonPanel1.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ribbonPanel1.TabIndex = 1;
            // 
            // ribbonBar3
            // 
            this.ribbonBar3.AutoOverflowEnabled = true;
            // 
            // 
            // 
            this.ribbonBar3.BackgroundMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.ribbonBar3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ribbonBar3.ContainerControlProcessDialogKey = true;
            this.ribbonBar3.Dock = System.Windows.Forms.DockStyle.Left;
            this.ribbonBar3.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.labelItem1,
            this.txtBarcode,
            this.lblStatus});
            this.ribbonBar3.Location = new System.Drawing.Point(691, 0);
            this.ribbonBar3.Margin = new System.Windows.Forms.Padding(4);
            this.ribbonBar3.Name = "ribbonBar3";
            this.ribbonBar3.Size = new System.Drawing.Size(811, 125);
            this.ribbonBar3.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ribbonBar3.TabIndex = 3;
            this.ribbonBar3.Text = "上托盘";
            // 
            // 
            // 
            this.ribbonBar3.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.ribbonBar3.TitleStyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // labelItem1
            // 
            this.labelItem1.Name = "labelItem1";
            this.labelItem1.Text = "ESN/MAC：";
            // 
            // txtBarcode
            // 
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.TextBoxWidth = 150;
            this.txtBarcode.WatermarkColor = System.Drawing.SystemColors.GrayText;
            this.txtBarcode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBarcode_KeyDown);
            // 
            // lblStatus
            // 
            this.lblStatus.BackColor = System.Drawing.Color.Red;
            this.lblStatus.Font = new System.Drawing.Font("黑体", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Text = "FAIL";
            this.lblStatus.TextAlignment = System.Drawing.StringAlignment.Center;
            this.lblStatus.Width = 360;
            // 
            // ribbonBar1
            // 
            this.ribbonBar1.AutoOverflowEnabled = true;
            // 
            // 
            // 
            this.ribbonBar1.BackgroundMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.ribbonBar1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ribbonBar1.ContainerControlProcessDialogKey = true;
            this.ribbonBar1.Dock = System.Windows.Forms.DockStyle.Left;
            this.ribbonBar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btnStartServer,
            this.btnStopServer,
            this.lblUser,
            this.btnLogin});
            this.ribbonBar1.Location = new System.Drawing.Point(192, 0);
            this.ribbonBar1.Margin = new System.Windows.Forms.Padding(4);
            this.ribbonBar1.Name = "ribbonBar1";
            this.ribbonBar1.Size = new System.Drawing.Size(499, 125);
            this.ribbonBar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ribbonBar1.TabIndex = 2;
            // 
            // 
            // 
            this.ribbonBar1.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.ribbonBar1.TitleStyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // btnStartServer
            // 
            this.btnStartServer.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.btnStartServer.Icon = ((System.Drawing.Icon)(resources.GetObject("btnStartServer.Icon")));
            this.btnStartServer.ImagePosition = DevComponents.DotNetBar.eImagePosition.Bottom;
            this.btnStartServer.Name = "btnStartServer";
            this.btnStartServer.SubItemsExpandWidth = 14;
            this.btnStartServer.Text = "系统启动";
            this.btnStartServer.Click += new System.EventHandler(this.btnStartServer_Click);
            // 
            // btnStopServer
            // 
            this.btnStopServer.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.btnStopServer.Icon = ((System.Drawing.Icon)(resources.GetObject("btnStopServer.Icon")));
            this.btnStopServer.ImagePosition = DevComponents.DotNetBar.eImagePosition.Bottom;
            this.btnStopServer.Name = "btnStopServer";
            this.btnStopServer.SubItemsExpandWidth = 14;
            this.btnStopServer.Text = "系统停止";
            this.btnStopServer.Click += new System.EventHandler(this.btnStopServer_Click);
            // 
            // lblUser
            // 
            this.lblUser.Name = "lblUser";
            this.lblUser.Text = "操作员：FX004206-陈汝豪";
            // 
            // btnLogin
            // 
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.SubItemsExpandWidth = 14;
            this.btnLogin.Text = "用户登录";
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // ribbonBar2
            // 
            this.ribbonBar2.AutoOverflowEnabled = true;
            // 
            // 
            // 
            this.ribbonBar2.BackgroundMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.ribbonBar2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ribbonBar2.ContainerControlProcessDialogKey = true;
            this.ribbonBar2.Dock = System.Windows.Forms.DockStyle.Left;
            this.ribbonBar2.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btnSetting,
            this.btnReset});
            this.ribbonBar2.Location = new System.Drawing.Point(4, 0);
            this.ribbonBar2.Margin = new System.Windows.Forms.Padding(4);
            this.ribbonBar2.Name = "ribbonBar2";
            this.ribbonBar2.Size = new System.Drawing.Size(188, 125);
            this.ribbonBar2.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ribbonBar2.TabIndex = 1;
            // 
            // 
            // 
            this.ribbonBar2.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.ribbonBar2.TitleStyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // btnSetting
            // 
            this.btnSetting.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.btnSetting.Icon = ((System.Drawing.Icon)(resources.GetObject("btnSetting.Icon")));
            this.btnSetting.ImagePosition = DevComponents.DotNetBar.eImagePosition.Bottom;
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.SubItemsExpandWidth = 14;
            this.btnSetting.Text = "程序设置";
            this.btnSetting.Click += new System.EventHandler(this.btnSetting_Click);
            // 
            // btnReset
            // 
            this.btnReset.Name = "btnReset";
            this.btnReset.SubItemsExpandWidth = 14;
            this.btnReset.Text = "状态重置";
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // ribbonPanel2
            // 
            this.ribbonPanel2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ribbonPanel2.Controls.Add(this.ribbonBar4);
            this.ribbonPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ribbonPanel2.Location = new System.Drawing.Point(0, 0);
            this.ribbonPanel2.Margin = new System.Windows.Forms.Padding(4);
            this.ribbonPanel2.Name = "ribbonPanel2";
            this.ribbonPanel2.Padding = new System.Windows.Forms.Padding(4, 0, 4, 4);
            this.ribbonPanel2.Size = new System.Drawing.Size(1394, 188);
            // 
            // 
            // 
            this.ribbonPanel2.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.ribbonPanel2.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.ribbonPanel2.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ribbonPanel2.TabIndex = 2;
            this.ribbonPanel2.Visible = false;
            // 
            // ribbonBar4
            // 
            this.ribbonBar4.AutoOverflowEnabled = true;
            // 
            // 
            // 
            this.ribbonBar4.BackgroundMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.ribbonBar4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ribbonBar4.ContainerControlProcessDialogKey = true;
            this.ribbonBar4.Dock = System.Windows.Forms.DockStyle.Left;
            this.ribbonBar4.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btnPrintTest});
            this.ribbonBar4.Location = new System.Drawing.Point(4, 0);
            this.ribbonBar4.Margin = new System.Windows.Forms.Padding(4);
            this.ribbonBar4.Name = "ribbonBar4";
            this.ribbonBar4.Size = new System.Drawing.Size(108, 184);
            this.ribbonBar4.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ribbonBar4.TabIndex = 0;
            // 
            // 
            // 
            this.ribbonBar4.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.ribbonBar4.TitleStyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // btnPrintTest
            // 
            this.btnPrintTest.Name = "btnPrintTest";
            this.btnPrintTest.SubItemsExpandWidth = 14;
            this.btnPrintTest.Text = "打印测试页";
            this.btnPrintTest.Click += new System.EventHandler(this.btnPrintTest_Click);
            // 
            // ribbonTabItem1
            // 
            this.ribbonTabItem1.Checked = true;
            this.ribbonTabItem1.Name = "ribbonTabItem1";
            this.ribbonTabItem1.Panel = this.ribbonPanel1;
            this.ribbonTabItem1.Text = "菜单栏";
            // 
            // Print_Test
            // 
            this.Print_Test.Name = "Print_Test";
            this.Print_Test.Panel = this.ribbonPanel2;
            this.Print_Test.Text = "打印机测试";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(159, 25);
            this.toolStripLabel1.Text = "提供方：工程技术中心";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 28);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(69, 25);
            this.toolStripLabel2.Text = "串口号：";
            // 
            // cbxPortName
            // 
            this.cbxPortName.Name = "cbxPortName";
            this.cbxPortName.Size = new System.Drawing.Size(121, 28);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 28);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(69, 25);
            this.toolStripLabel3.Text = "波特率：";
            // 
            // cbxBaudRate
            // 
            this.cbxBaudRate.Name = "cbxBaudRate";
            this.cbxBaudRate.Size = new System.Drawing.Size(121, 28);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 28);
            // 
            // btnConnect
            // 
            this.btnConnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnConnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(43, 25);
            this.btnConnect.Text = "连接";
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 28);
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.Name = "toolStripLabel4";
            this.toolStripLabel4.Size = new System.Drawing.Size(37, 25);
            this.toolStripLabel4.Text = "IP：";
            // 
            // txtIp
            // 
            this.txtIp.Name = "txtIp";
            this.txtIp.Size = new System.Drawing.Size(130, 28);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 28);
            // 
            // toolStripLabel5
            // 
            this.toolStripLabel5.Name = "toolStripLabel5";
            this.toolStripLabel5.Size = new System.Drawing.Size(69, 25);
            this.toolStripLabel5.Text = "端口号：";
            // 
            // txtPort
            // 
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(60, 28);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 28);
            // 
            // toolStripLabel6
            // 
            this.toolStripLabel6.Name = "toolStripLabel6";
            this.toolStripLabel6.Size = new System.Drawing.Size(54, 25);
            this.toolStripLabel6.Text = "机型：";
            // 
            // lblCraft
            // 
            this.lblCraft.Name = "lblCraft";
            this.lblCraft.Size = new System.Drawing.Size(16, 25);
            this.lblCraft.Text = "_";
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 28);
            // 
            // toolStripLabel8
            // 
            this.toolStripLabel8.Name = "toolStripLabel8";
            this.toolStripLabel8.Size = new System.Drawing.Size(54, 25);
            this.toolStripLabel8.Text = "线体：";
            // 
            // lblLine
            // 
            this.lblLine.Name = "lblLine";
            this.lblLine.Size = new System.Drawing.Size(16, 25);
            this.lblLine.Text = "_";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.toolStripSeparator1,
            this.toolStripLabel2,
            this.cbxPortName,
            this.toolStripSeparator2,
            this.toolStripLabel3,
            this.cbxBaudRate,
            this.toolStripSeparator3,
            this.btnConnect,
            this.toolStripSeparator4,
            this.toolStripLabel4,
            this.txtIp,
            this.toolStripSeparator5,
            this.toolStripLabel5,
            this.txtPort,
            this.toolStripSeparator6,
            this.toolStripLabel6,
            this.lblCraft,
            this.toolStripSeparator7,
            this.toolStripLabel8,
            this.lblLine,
            this.toolStripSeparator8,
            this.toolStripLabel10,
            this.lblWorkOrder});
            this.toolStrip1.Location = new System.Drawing.Point(5, 823);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1394, 28);
            this.toolStrip1.TabIndex = 19;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 28);
            // 
            // toolStripLabel10
            // 
            this.toolStripLabel10.Name = "toolStripLabel10";
            this.toolStripLabel10.Size = new System.Drawing.Size(54, 25);
            this.toolStripLabel10.Text = "工单：";
            // 
            // lblWorkOrder
            // 
            this.lblWorkOrder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.lblWorkOrder.Name = "lblWorkOrder";
            this.lblWorkOrder.Size = new System.Drawing.Size(16, 25);
            this.lblWorkOrder.Text = "_";
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.craft16);
            this.groupPanel1.Controls.Add(this.craft15);
            this.groupPanel1.Controls.Add(this.craft14);
            this.groupPanel1.Controls.Add(this.craft13);
            this.groupPanel1.Controls.Add(this.craft12);
            this.groupPanel1.Controls.Add(this.craft11);
            this.groupPanel1.Controls.Add(this.craft10);
            this.groupPanel1.Controls.Add(this.craft9);
            this.groupPanel1.Controls.Add(this.craft8);
            this.groupPanel1.Controls.Add(this.craft7);
            this.groupPanel1.Controls.Add(this.craft6);
            this.groupPanel1.Controls.Add(this.craft5);
            this.groupPanel1.Controls.Add(this.craft4);
            this.groupPanel1.Controls.Add(this.craft3);
            this.groupPanel1.Controls.Add(this.craft2);
            this.groupPanel1.Controls.Add(this.craft1);
            this.groupPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupPanel1.Location = new System.Drawing.Point(5, 193);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(1394, 630);
            // 
            // 
            // 
            this.groupPanel1.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel1.Style.BackColorGradientAngle = 90;
            this.groupPanel1.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel1.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderBottomWidth = 1;
            this.groupPanel1.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel1.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderLeftWidth = 1;
            this.groupPanel1.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderRightWidth = 1;
            this.groupPanel1.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderTopWidth = 1;
            this.groupPanel1.Style.CornerDiameter = 4;
            this.groupPanel1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel1.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel1.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel1.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.groupPanel1.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.groupPanel1.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.groupPanel1.TabIndex = 20;
            this.groupPanel1.Text = "生产看板";
            // 
            // craft16
            // 
            this.craft16.BackColor = System.Drawing.Color.Transparent;
            this.craft16.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.craft16.Location = new System.Drawing.Point(939, 404);
            this.craft16.Name = "craft16";
            this.craft16.Size = new System.Drawing.Size(196, 196);
            this.craft16.TabIndex = 0;
            // 
            // craft15
            // 
            this.craft15.BackColor = System.Drawing.Color.Transparent;
            this.craft15.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.craft15.Location = new System.Drawing.Point(711, 404);
            this.craft15.Name = "craft15";
            this.craft15.Size = new System.Drawing.Size(196, 196);
            this.craft15.TabIndex = 0;
            // 
            // craft14
            // 
            this.craft14.BackColor = System.Drawing.Color.Transparent;
            this.craft14.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.craft14.Location = new System.Drawing.Point(483, 404);
            this.craft14.Name = "craft14";
            this.craft14.Size = new System.Drawing.Size(196, 196);
            this.craft14.TabIndex = 0;
            // 
            // craft13
            // 
            this.craft13.BackColor = System.Drawing.Color.Transparent;
            this.craft13.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.craft13.Location = new System.Drawing.Point(255, 404);
            this.craft13.Name = "craft13";
            this.craft13.Size = new System.Drawing.Size(196, 196);
            this.craft13.TabIndex = 0;
            // 
            // craft12
            // 
            this.craft12.BackColor = System.Drawing.Color.Transparent;
            this.craft12.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.craft12.Location = new System.Drawing.Point(1167, 202);
            this.craft12.Name = "craft12";
            this.craft12.Size = new System.Drawing.Size(196, 196);
            this.craft12.TabIndex = 0;
            // 
            // craft11
            // 
            this.craft11.BackColor = System.Drawing.Color.Transparent;
            this.craft11.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.craft11.Location = new System.Drawing.Point(939, 202);
            this.craft11.Name = "craft11";
            this.craft11.Size = new System.Drawing.Size(196, 196);
            this.craft11.TabIndex = 0;
            // 
            // craft10
            // 
            this.craft10.BackColor = System.Drawing.Color.Transparent;
            this.craft10.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.craft10.Location = new System.Drawing.Point(711, 202);
            this.craft10.Name = "craft10";
            this.craft10.Size = new System.Drawing.Size(196, 196);
            this.craft10.TabIndex = 0;
            // 
            // craft9
            // 
            this.craft9.BackColor = System.Drawing.Color.Transparent;
            this.craft9.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.craft9.Location = new System.Drawing.Point(483, 202);
            this.craft9.Name = "craft9";
            this.craft9.Size = new System.Drawing.Size(196, 196);
            this.craft9.TabIndex = 0;
            // 
            // craft8
            // 
            this.craft8.BackColor = System.Drawing.Color.Transparent;
            this.craft8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.craft8.Location = new System.Drawing.Point(255, 202);
            this.craft8.Name = "craft8";
            this.craft8.Size = new System.Drawing.Size(196, 196);
            this.craft8.TabIndex = 0;
            // 
            // craft7
            // 
            this.craft7.BackColor = System.Drawing.Color.Transparent;
            this.craft7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.craft7.Location = new System.Drawing.Point(27, 202);
            this.craft7.Name = "craft7";
            this.craft7.Size = new System.Drawing.Size(196, 196);
            this.craft7.TabIndex = 0;
            // 
            // craft6
            // 
            this.craft6.BackColor = System.Drawing.Color.Transparent;
            this.craft6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.craft6.Location = new System.Drawing.Point(1167, 0);
            this.craft6.Name = "craft6";
            this.craft6.Size = new System.Drawing.Size(196, 196);
            this.craft6.TabIndex = 0;
            // 
            // craft5
            // 
            this.craft5.BackColor = System.Drawing.Color.Transparent;
            this.craft5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.craft5.Location = new System.Drawing.Point(939, 0);
            this.craft5.Name = "craft5";
            this.craft5.Size = new System.Drawing.Size(196, 196);
            this.craft5.TabIndex = 0;
            // 
            // craft4
            // 
            this.craft4.BackColor = System.Drawing.Color.Transparent;
            this.craft4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.craft4.Location = new System.Drawing.Point(711, 0);
            this.craft4.Name = "craft4";
            this.craft4.Size = new System.Drawing.Size(196, 196);
            this.craft4.TabIndex = 0;
            // 
            // craft3
            // 
            this.craft3.BackColor = System.Drawing.Color.Transparent;
            this.craft3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.craft3.Location = new System.Drawing.Point(483, 0);
            this.craft3.Name = "craft3";
            this.craft3.Size = new System.Drawing.Size(196, 196);
            this.craft3.TabIndex = 0;
            // 
            // craft2
            // 
            this.craft2.BackColor = System.Drawing.Color.Transparent;
            this.craft2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.craft2.Location = new System.Drawing.Point(255, 0);
            this.craft2.Name = "craft2";
            this.craft2.Size = new System.Drawing.Size(196, 196);
            this.craft2.TabIndex = 0;
            // 
            // craft1
            // 
            this.craft1.BackColor = System.Drawing.Color.Transparent;
            this.craft1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.craft1.Location = new System.Drawing.Point(27, 0);
            this.craft1.Name = "craft1";
            this.craft1.Size = new System.Drawing.Size(196, 196);
            this.craft1.TabIndex = 0;
            // 
            // TestFrm
            // 
            this.ClientSize = new System.Drawing.Size(1404, 853);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.ribbonControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "TestFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "板测自动化软件";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TestFrm_FormClosing);
            this.Load += new System.EventHandler(this.TestFrm_Load);
            this.ribbonControl1.ResumeLayout(false);
            this.ribbonControl1.PerformLayout();
            this.ribbonPanel1.ResumeLayout(false);
            this.ribbonPanel2.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sysInfoSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.RibbonControl ribbonControl1;
        private DevComponents.DotNetBar.RibbonPanel ribbonPanel1;
        private DevComponents.DotNetBar.RibbonBar ribbonBar3;
        private DevComponents.DotNetBar.LabelItem labelItem1;
        private DevComponents.DotNetBar.TextBoxItem txtBarcode;
        private DevComponents.DotNetBar.LabelItem lblStatus;
        private DevComponents.DotNetBar.RibbonBar ribbonBar1;
        private DevComponents.DotNetBar.ButtonItem btnStartServer;
        private DevComponents.DotNetBar.ButtonItem btnStopServer;
        private DevComponents.DotNetBar.LabelItem lblUser;
        private DevComponents.DotNetBar.ButtonItem btnLogin;
        private DevComponents.DotNetBar.RibbonBar ribbonBar2;
        private DevComponents.DotNetBar.ButtonItem btnSetting;
        private DevComponents.DotNetBar.ButtonItem btnReset;
        private DevComponents.DotNetBar.RibbonPanel ribbonPanel2;
        private DevComponents.DotNetBar.RibbonBar ribbonBar4;
        private DevComponents.DotNetBar.ButtonItem btnPrintTest;
        private DevComponents.DotNetBar.RibbonTabItem ribbonTabItem1;
        private DevComponents.DotNetBar.RibbonTabItem Print_Test;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripComboBox cbxPortName;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripComboBox cbxBaudRate;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btnConnect;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripLabel toolStripLabel4;
        private System.Windows.Forms.ToolStripTextBox txtIp;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripLabel toolStripLabel5;
        private System.Windows.Forms.ToolStripTextBox txtPort;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripLabel toolStripLabel6;
        private System.Windows.Forms.ToolStripLabel lblCraft;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripLabel toolStripLabel8;
        private System.Windows.Forms.ToolStripLabel lblLine;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripLabel toolStripLabel10;
        private System.Windows.Forms.ToolStripLabel lblWorkOrder;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private System.Windows.Forms.BindingSource sysInfoSource;
        private CraftControl.UserCraft craft16;
        private CraftControl.UserCraft craft15;
        private CraftControl.UserCraft craft14;
        private CraftControl.UserCraft craft13;
        private CraftControl.UserCraft craft12;
        private CraftControl.UserCraft craft11;
        private CraftControl.UserCraft craft10;
        private CraftControl.UserCraft craft9;
        private CraftControl.UserCraft craft8;
        private CraftControl.UserCraft craft7;
        private CraftControl.UserCraft craft6;
        private CraftControl.UserCraft craft5;
        private CraftControl.UserCraft craft4;
        private CraftControl.UserCraft craft3;
        private CraftControl.UserCraft craft2;
        private CraftControl.UserCraft craft1;

    }
}