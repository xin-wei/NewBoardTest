namespace BoardAutoTesting
{
    partial class SettingFrm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingFrm));
            this.btnSave = new DevComponents.DotNetBar.ButtonX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.dgvLineInfo = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.LineInfo_Menu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnDeleteCraft = new System.Windows.Forms.ToolStripMenuItem();
            this.btnNewCraft = new System.Windows.Forms.ToolStripMenuItem();
            this.groupPanel2 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.lineInfoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.Craft_Idx = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Line_Idx = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Route_Name = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.MCU_IP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ATE_IP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.b_Repair = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.b_Last = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Port_Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLineInfo)).BeginInit();
            this.LineInfo_Menu.SuspendLayout();
            this.groupPanel2.SuspendLayout();
            this.panelEx1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lineInfoBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSave.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSave.Location = new System.Drawing.Point(889, 4);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 29);
            this.btnSave.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "保存";
            this.btnSave.Visible = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Location = new System.Drawing.Point(1019, 4);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 29);
            this.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "完成";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // dgvLineInfo
            // 
            this.dgvLineInfo.AllowUserToAddRows = false;
            this.dgvLineInfo.AllowUserToDeleteRows = false;
            this.dgvLineInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLineInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Craft_Idx,
            this.Line_Idx,
            this.Route_Name,
            this.MCU_IP,
            this.ATE_IP,
            this.b_Repair,
            this.b_Last,
            this.Port_Id});
            this.dgvLineInfo.ContextMenuStrip = this.LineInfo_Menu;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvLineInfo.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvLineInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLineInfo.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dgvLineInfo.Location = new System.Drawing.Point(0, 0);
            this.dgvLineInfo.Margin = new System.Windows.Forms.Padding(4);
            this.dgvLineInfo.Name = "dgvLineInfo";
            this.dgvLineInfo.RowHeadersVisible = false;
            this.dgvLineInfo.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dgvLineInfo.RowTemplate.Height = 23;
            this.dgvLineInfo.Size = new System.Drawing.Size(1129, 525);
            this.dgvLineInfo.TabIndex = 2;
            this.dgvLineInfo.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvLineInfo_CellClick);
            this.dgvLineInfo.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvLineInfo_CellMouseDown);
            this.dgvLineInfo.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvLineInfo_CellValueChanged);
            // 
            // LineInfo_Menu
            // 
            this.LineInfo_Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnDeleteCraft,
            this.btnNewCraft});
            this.LineInfo_Menu.Name = "contextMenuStrip1";
            this.LineInfo_Menu.Size = new System.Drawing.Size(109, 52);
            // 
            // btnDeleteCraft
            // 
            this.btnDeleteCraft.Name = "btnDeleteCraft";
            this.btnDeleteCraft.Size = new System.Drawing.Size(108, 24);
            this.btnDeleteCraft.Text = "删除";
            this.btnDeleteCraft.Click += new System.EventHandler(this.btnDeleteCraft_Click);
            // 
            // btnNewCraft
            // 
            this.btnNewCraft.Name = "btnNewCraft";
            this.btnNewCraft.Size = new System.Drawing.Size(108, 24);
            this.btnNewCraft.Text = "添加";
            this.btnNewCraft.Click += new System.EventHandler(this.btnNewCraft_Click);
            // 
            // groupPanel2
            // 
            this.groupPanel2.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel2.Controls.Add(this.dgvLineInfo);
            this.groupPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupPanel2.Location = new System.Drawing.Point(0, 0);
            this.groupPanel2.Margin = new System.Windows.Forms.Padding(4);
            this.groupPanel2.Name = "groupPanel2";
            this.groupPanel2.Size = new System.Drawing.Size(1135, 552);
            // 
            // 
            // 
            this.groupPanel2.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel2.Style.BackColorGradientAngle = 90;
            this.groupPanel2.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel2.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderBottomWidth = 1;
            this.groupPanel2.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel2.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderLeftWidth = 1;
            this.groupPanel2.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderRightWidth = 1;
            this.groupPanel2.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderTopWidth = 1;
            this.groupPanel2.Style.CornerDiameter = 4;
            this.groupPanel2.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel2.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel2.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel2.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.groupPanel2.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.groupPanel2.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.groupPanel2.TabIndex = 3;
            this.groupPanel2.Text = "线体分布";
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.btnSave);
            this.panelEx1.Controls.Add(this.btnCancel);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelEx1.Location = new System.Drawing.Point(0, 552);
            this.panelEx1.Margin = new System.Windows.Forms.Padding(4);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(1135, 38);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 4;
            // 
            // lineInfoBindingSource
            // 
            this.lineInfoBindingSource.DataSource = typeof(BoardAutoTesting.Model.LineInfo);
            // 
            // Craft_Idx
            // 
            this.Craft_Idx.DataPropertyName = "CraftId";
            this.Craft_Idx.HeaderText = "测试机台编号";
            this.Craft_Idx.Name = "Craft_Idx";
            this.Craft_Idx.ReadOnly = true;
            // 
            // Line_Idx
            // 
            this.Line_Idx.DataPropertyName = "LineIdx";
            this.Line_Idx.FillWeight = 130F;
            this.Line_Idx.HeaderText = "线体";
            this.Line_Idx.Name = "Line_Idx";
            this.Line_Idx.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Line_Idx.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // Route_Name
            // 
            this.Route_Name.DataPropertyName = "RouteName";
            this.Route_Name.FillWeight = 180F;
            this.Route_Name.HeaderText = "站位";
            this.Route_Name.Name = "Route_Name";
            this.Route_Name.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Route_Name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Route_Name.Width = 180;
            // 
            // MCU_IP
            // 
            this.MCU_IP.DataPropertyName = "McuIp";
            this.MCU_IP.HeaderText = "单片机IP";
            this.MCU_IP.Name = "MCU_IP";
            // 
            // ATE_IP
            // 
            this.ATE_IP.DataPropertyName = "AteIp";
            this.ATE_IP.HeaderText = "ATE_IP";
            this.ATE_IP.Name = "ATE_IP";
            // 
            // b_Repair
            // 
            this.b_Repair.DataPropertyName = "IsRepair";
            this.b_Repair.HeaderText = "维修中";
            this.b_Repair.Name = "b_Repair";
            this.b_Repair.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // b_Last
            // 
            this.b_Last.DataPropertyName = "IsOut";
            this.b_Last.HeaderText = "最后一站";
            this.b_Last.Name = "b_Last";
            // 
            // Port_Id
            // 
            this.Port_Id.HeaderText = "端口号";
            this.Port_Id.Name = "Port_Id";
            // 
            // SettingFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1135, 590);
            this.Controls.Add(this.groupPanel2);
            this.Controls.Add(this.panelEx1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SettingFrm";
            this.Text = "基础信息设定";
            this.Load += new System.EventHandler(this.SettingFrm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLineInfo)).EndInit();
            this.LineInfo_Menu.ResumeLayout(false);
            this.groupPanel2.ResumeLayout(false);
            this.panelEx1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lineInfoBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX btnSave;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.Controls.DataGridViewX dgvLineInfo;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel2;
        private DevComponents.DotNetBar.PanelEx panelEx1;
        private System.Windows.Forms.ContextMenuStrip LineInfo_Menu;
        private System.Windows.Forms.ToolStripMenuItem btnDeleteCraft;
        private System.Windows.Forms.ToolStripMenuItem btnNewCraft;
        private System.Windows.Forms.BindingSource lineInfoBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn Craft_Idx;
        private System.Windows.Forms.DataGridViewComboBoxColumn Line_Idx;
        private System.Windows.Forms.DataGridViewComboBoxColumn Route_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn MCU_IP;
        private System.Windows.Forms.DataGridViewTextBoxColumn ATE_IP;
        private System.Windows.Forms.DataGridViewCheckBoxColumn b_Repair;
        private System.Windows.Forms.DataGridViewCheckBoxColumn b_Last;
        private System.Windows.Forms.DataGridViewTextBoxColumn Port_Id;
    }
}