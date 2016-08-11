namespace CraftControl
{
    partial class UserCraft
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnCraft = new DevComponents.DotNetBar.ButtonX();
            this.btnLine = new DevComponents.DotNetBar.ButtonX();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.Panel1.Controls.Add(this.btnCraft);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnLine);
            this.splitContainer1.Size = new System.Drawing.Size(196, 196);
            this.splitContainer1.SplitterDistance = 148;
            this.splitContainer1.TabIndex = 1;
            // 
            // btnCraft
            // 
            this.btnCraft.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCraft.BackColor = System.Drawing.Color.Silver;
            this.btnCraft.ColorTable = DevComponents.DotNetBar.eButtonColor.Blue;
            this.btnCraft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCraft.Font = new System.Drawing.Font("微软雅黑", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCraft.Location = new System.Drawing.Point(0, 0);
            this.btnCraft.Name = "btnCraft";
            this.btnCraft.Size = new System.Drawing.Size(196, 148);
            this.btnCraft.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnCraft.TabIndex = 0;
            this.btnCraft.TextColor = System.Drawing.Color.Black;
            // 
            // btnLine
            // 
            this.btnLine.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnLine.BackColor = System.Drawing.Color.Silver;
            this.btnLine.ColorTable = DevComponents.DotNetBar.eButtonColor.Blue;
            this.btnLine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLine.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLine.Location = new System.Drawing.Point(0, 0);
            this.btnLine.Name = "btnLine";
            this.btnLine.Size = new System.Drawing.Size(196, 44);
            this.btnLine.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnLine.TabIndex = 0;
            this.btnLine.TextColor = System.Drawing.Color.Maroon;
            // 
            // UserCraft
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.splitContainer1);
            this.Name = "UserCraft";
            this.Size = new System.Drawing.Size(196, 196);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private DevComponents.DotNetBar.ButtonX btnCraft;
        private DevComponents.DotNetBar.ButtonX btnLine;

    }
}
