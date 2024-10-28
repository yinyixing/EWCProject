
namespace EWCApplicationSystem
{
    partial class ExportForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ckb_Dindex = new System.Windows.Forms.CheckBox();
            this.ckb_Weight = new System.Windows.Forms.CheckBox();
            this.ckb_Nquan = new System.Windows.Forms.CheckBox();
            this.ckb_Nmatrix = new System.Windows.Forms.CheckBox();
            this.btn_export = new System.Windows.Forms.Button();
            this.btn_close = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pib_choose = new System.Windows.Forms.PictureBox();
            this.txt_folder_path = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pib_choose)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.groupBox1.Controls.Add(this.ckb_Dindex);
            this.groupBox1.Controls.Add(this.ckb_Weight);
            this.groupBox1.Controls.Add(this.ckb_Nquan);
            this.groupBox1.Controls.Add(this.ckb_Nmatrix);
            this.groupBox1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.ForeColor = System.Drawing.Color.MediumBlue;
            this.groupBox1.Location = new System.Drawing.Point(12, 116);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(419, 127);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Export operation";
            // 
            // ckb_Dindex
            // 
            this.ckb_Dindex.AutoSize = true;
            this.ckb_Dindex.Location = new System.Drawing.Point(206, 82);
            this.ckb_Dindex.Name = "ckb_Dindex";
            this.ckb_Dindex.Size = new System.Drawing.Size(175, 25);
            this.ckb_Dindex.TabIndex = 4;
            this.ckb_Dindex.Text = "development index";
            this.ckb_Dindex.UseVisualStyleBackColor = true;
            // 
            // ckb_Weight
            // 
            this.ckb_Weight.AutoSize = true;
            this.ckb_Weight.Location = new System.Drawing.Point(12, 82);
            this.ckb_Weight.Name = "ckb_Weight";
            this.ckb_Weight.Size = new System.Drawing.Size(81, 25);
            this.ckb_Weight.TabIndex = 3;
            this.ckb_Weight.Text = "weight";
            this.ckb_Weight.UseVisualStyleBackColor = true;
            // 
            // ckb_Nquan
            // 
            this.ckb_Nquan.AutoSize = true;
            this.ckb_Nquan.Location = new System.Drawing.Point(206, 39);
            this.ckb_Nquan.Name = "ckb_Nquan";
            this.ckb_Nquan.Size = new System.Drawing.Size(199, 25);
            this.ckb_Nquan.TabIndex = 2;
            this.ckb_Nquan.Text = "entropy quantification";
            this.ckb_Nquan.UseVisualStyleBackColor = true;
            // 
            // ckb_Nmatrix
            // 
            this.ckb_Nmatrix.AutoSize = true;
            this.ckb_Nmatrix.Location = new System.Drawing.Point(12, 39);
            this.ckb_Nmatrix.Name = "ckb_Nmatrix";
            this.ckb_Nmatrix.Size = new System.Drawing.Size(187, 25);
            this.ckb_Nmatrix.TabIndex = 1;
            this.ckb_Nmatrix.Text = "normalization matrix";
            this.ckb_Nmatrix.UseVisualStyleBackColor = true;
            // 
            // btn_export
            // 
            this.btn_export.BackColor = System.Drawing.SystemColors.Highlight;
            this.btn_export.FlatAppearance.BorderSize = 0;
            this.btn_export.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_export.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_export.ForeColor = System.Drawing.Color.White;
            this.btn_export.Location = new System.Drawing.Point(340, 260);
            this.btn_export.Name = "btn_export";
            this.btn_export.Size = new System.Drawing.Size(91, 29);
            this.btn_export.TabIndex = 6;
            this.btn_export.Text = "Export";
            this.btn_export.UseVisualStyleBackColor = false;
            this.btn_export.Click += new System.EventHandler(this.btn_export_Click);
            // 
            // btn_close
            // 
            this.btn_close.BackColor = System.Drawing.SystemColors.Highlight;
            this.btn_close.FlatAppearance.BorderSize = 0;
            this.btn_close.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_close.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_close.ForeColor = System.Drawing.Color.White;
            this.btn_close.Location = new System.Drawing.Point(197, 260);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(91, 29);
            this.btn_close.TabIndex = 7;
            this.btn_close.Text = "Close";
            this.btn_close.UseVisualStyleBackColor = false;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pib_choose);
            this.groupBox2.Controls.Add(this.txt_folder_path);
            this.groupBox2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.ForeColor = System.Drawing.Color.MediumBlue;
            this.groupBox2.Location = new System.Drawing.Point(14, 14);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox2.Size = new System.Drawing.Size(417, 94);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Enter/Select Storage Folder";
            // 
            // pib_choose
            // 
            this.pib_choose.Image = global::EWCApplicationSystem.Properties.Resources.imprtfolder;
            this.pib_choose.Location = new System.Drawing.Point(359, 32);
            this.pib_choose.Name = "pib_choose";
            this.pib_choose.Size = new System.Drawing.Size(55, 50);
            this.pib_choose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pib_choose.TabIndex = 1;
            this.pib_choose.TabStop = false;
            this.pib_choose.Click += new System.EventHandler(this.pib_choose_Click);
            // 
            // txt_folder_path
            // 
            this.txt_folder_path.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_folder_path.Location = new System.Drawing.Point(10, 32);
            this.txt_folder_path.Margin = new System.Windows.Forms.Padding(5);
            this.txt_folder_path.Multiline = true;
            this.txt_folder_path.Name = "txt_folder_path";
            this.txt_folder_path.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_folder_path.Size = new System.Drawing.Size(344, 50);
            this.txt_folder_path.TabIndex = 0;
            // 
            // ExportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(446, 302);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.btn_export);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ExportForm";
            this.Text = "Export";
            this.Load += new System.EventHandler(this.ExportForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pib_choose)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_export;
        private System.Windows.Forms.CheckBox ckb_Dindex;
        private System.Windows.Forms.CheckBox ckb_Weight;
        private System.Windows.Forms.CheckBox ckb_Nquan;
        private System.Windows.Forms.CheckBox ckb_Nmatrix;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.PictureBox pib_choose;
        private System.Windows.Forms.TextBox txt_folder_path;
    }
}