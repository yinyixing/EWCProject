
namespace EWCApplicationSystem
{
    partial class TemplateLoad
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
            this.btn_import = new System.Windows.Forms.Button();
            this.btn_delete = new System.Windows.Forms.Button();
            this.dgv_template = new System.Windows.Forms.DataGridView();
            this.btn_save = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_template)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_import
            // 
            this.btn_import.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btn_import.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btn_import.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_import.Location = new System.Drawing.Point(12, 12);
            this.btn_import.Name = "btn_import";
            this.btn_import.Size = new System.Drawing.Size(108, 27);
            this.btn_import.TabIndex = 0;
            this.btn_import.Text = "Import template";
            this.btn_import.UseVisualStyleBackColor = false;
            this.btn_import.Click += new System.EventHandler(this.button1_Click);
            // 
            // btn_delete
            // 
            this.btn_delete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btn_delete.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btn_delete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_delete.Location = new System.Drawing.Point(138, 12);
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.Size = new System.Drawing.Size(108, 27);
            this.btn_delete.TabIndex = 1;
            this.btn_delete.Text = "Delete Data";
            this.btn_delete.UseVisualStyleBackColor = false;
            this.btn_delete.Click += new System.EventHandler(this.btn_delete_Click);
            // 
            // dgv_template
            // 
            this.dgv_template.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_template.BackgroundColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.dgv_template.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgv_template.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_template.GridColor = System.Drawing.SystemColors.Control;
            this.dgv_template.Location = new System.Drawing.Point(1, 45);
            this.dgv_template.Name = "dgv_template";
            this.dgv_template.RowHeadersWidth = 82;
            this.dgv_template.RowTemplate.Height = 23;
            this.dgv_template.Size = new System.Drawing.Size(1020, 438);
            this.dgv_template.TabIndex = 2;
            this.dgv_template.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgv_template_CellPainting);
            this.dgv_template.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgv_template_CellValidating);
            // 
            // btn_save
            // 
            this.btn_save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_save.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btn_save.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btn_save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_save.Location = new System.Drawing.Point(901, 12);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(108, 27);
            this.btn_save.TabIndex = 3;
            this.btn_save.Text = "Save template";
            this.btn_save.UseVisualStyleBackColor = false;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // TemplateLoad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(1021, 482);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.dgv_template);
            this.Controls.Add(this.btn_delete);
            this.Controls.Add(this.btn_import);
            this.Name = "TemplateLoad";
            this.ShowIcon = false;
            this.Text = "Template Load";
            this.Load += new System.EventHandler(this.TemplateLoad_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_template)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_import;
        private System.Windows.Forms.Button btn_delete;
        private System.Windows.Forms.DataGridView dgv_template;
        private System.Windows.Forms.Button btn_save;
    }
}