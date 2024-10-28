
namespace EWCApplicationSystem
{
    partial class ChartBar
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
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.chart_column = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.credentialDialog1 = new Ookii.Dialogs.WinForms.CredentialDialog(this.components);
            this.credentialDialog2 = new Ookii.Dialogs.WinForms.CredentialDialog(this.components);
            this.treeView_Name = new System.Windows.Forms.TreeView();
            ((System.ComponentModel.ISupportInitialize)(this.chart_column)).BeginInit();
            this.SuspendLayout();
            // 
            // chart_column
            // 
            this.chart_column.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.AxisX.Interval = 1D;
            chartArea1.AxisX.IsLabelAutoFit = false;
            chartArea1.AxisX.LabelStyle.Angle = 45;
            chartArea1.Name = "ChartArea1";
            this.chart_column.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart_column.Legends.Add(legend1);
            this.chart_column.Location = new System.Drawing.Point(0, 0);
            this.chart_column.Name = "chart_column";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            series1.YValuesPerPoint = 6;
            this.chart_column.Series.Add(series1);
            this.chart_column.Size = new System.Drawing.Size(1041, 579);
            this.chart_column.TabIndex = 0;
            this.chart_column.Text = "chart1";
            title1.Name = "Title_top";
            title1.Text = "Value of";
            this.chart_column.Titles.Add(title1);
            // 
            // credentialDialog1
            // 
            this.credentialDialog1.AdditionalEntropy = null;
            this.credentialDialog1.MainInstruction = "credentialDialog1";
            // 
            // credentialDialog2
            // 
            this.credentialDialog2.AdditionalEntropy = null;
            this.credentialDialog2.MainInstruction = "credentialDialog2";
            // 
            // treeView_Name
            // 
            this.treeView_Name.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView_Name.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treeView_Name.HideSelection = false;
            this.treeView_Name.Location = new System.Drawing.Point(1040, 0);
            this.treeView_Name.Name = "treeView_Name";
            this.treeView_Name.ShowRootLines = false;
            this.treeView_Name.Size = new System.Drawing.Size(200, 579);
            this.treeView_Name.TabIndex = 4;
            this.treeView_Name.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_Name_AfterSelect);
            // 
            // ChartBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(1241, 579);
            this.Controls.Add(this.treeView_Name);
            this.Controls.Add(this.chart_column);
            this.Name = "ChartBar";
            this.ShowIcon = false;
            this.Text = "ChartBar";
            this.Load += new System.EventHandler(this.ChartBar_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart_column)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart_column;
        private Ookii.Dialogs.WinForms.CredentialDialog credentialDialog1;
        private Ookii.Dialogs.WinForms.CredentialDialog credentialDialog2;
        private System.Windows.Forms.TreeView treeView_Name;
    }
}