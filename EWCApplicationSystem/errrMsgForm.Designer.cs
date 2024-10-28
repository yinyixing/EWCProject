
namespace EWCApplicationSystem
{
    partial class errrMsgForm
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsb_export = new System.Windows.Forms.ToolStripButton();
            this.dgv_errMsg = new System.Windows.Forms.DataGridView();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_errMsg)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.SteelBlue;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsb_export});
            this.toolStrip1.Location = new System.Drawing.Point(0, 222);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(998, 25);
            this.toolStrip1.TabIndex = 17;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsb_export
            // 
            this.tsb_export.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsb_export.ForeColor = System.Drawing.Color.White;
            this.tsb_export.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_export.Name = "tsb_export";
            this.tsb_export.Size = new System.Drawing.Size(50, 22);
            this.tsb_export.Text = "export";
            this.tsb_export.Click += new System.EventHandler(this.tsb_export_Click);
            // 
            // dgv_errMsg
            // 
            this.dgv_errMsg.AllowUserToAddRows = false;
            this.dgv_errMsg.AllowUserToDeleteRows = false;
            this.dgv_errMsg.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv_errMsg.BackgroundColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.dgv_errMsg.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgv_errMsg.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_errMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_errMsg.GridColor = System.Drawing.SystemColors.Control;
            this.dgv_errMsg.Location = new System.Drawing.Point(0, 0);
            this.dgv_errMsg.Name = "dgv_errMsg";
            this.dgv_errMsg.ReadOnly = true;
            this.dgv_errMsg.RowHeadersVisible = false;
            this.dgv_errMsg.RowHeadersWidth = 82;
            this.dgv_errMsg.RowTemplate.Height = 23;
            this.dgv_errMsg.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_errMsg.Size = new System.Drawing.Size(998, 222);
            this.dgv_errMsg.TabIndex = 18;
            // 
            // errrMsgForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(998, 247);
            this.Controls.Add(this.dgv_errMsg);
            this.Controls.Add(this.toolStrip1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "errrMsgForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Error message prompt";
            this.Load += new System.EventHandler(this.errrMsgForm_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_errMsg)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsb_export;
        private System.Windows.Forms.DataGridView dgv_errMsg;
    }
}