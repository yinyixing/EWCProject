
namespace EWCApplicationSystem
{
    partial class ChartLineMultiple
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.chart_Line = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.treeView_Name = new System.Windows.Forms.TreeView();
            ((System.ComponentModel.ISupportInitialize)(this.chart_Line)).BeginInit();
            this.SuspendLayout();
            // 
            // chart_Line
            // 
            this.chart_Line.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.AxisX.Interval = 1D;
            chartArea1.AxisY.IsStartedFromZero = false;
            chartArea1.Name = "ChartArea1";
            this.chart_Line.ChartAreas.Add(chartArea1);
            legend1.AutoFitMinFontSize = 8;
            legend1.ItemColumnSpacing = 100;
            legend1.Name = "Legend1";
            this.chart_Line.Legends.Add(legend1);
            this.chart_Line.Location = new System.Drawing.Point(0, -1);
            this.chart_Line.Name = "chart_Line";
            this.chart_Line.Size = new System.Drawing.Size(1185, 552);
            this.chart_Line.TabIndex = 3;
            this.chart_Line.Text = "chart1";
            title1.Alignment = System.Drawing.ContentAlignment.TopCenter;
            title1.Name = "title_top";
            title1.Text = "values";
            title2.Alignment = System.Drawing.ContentAlignment.BottomCenter;
            title2.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            title2.Name = "title_bottom";
            title2.Text = "Year";
            this.chart_Line.Titles.Add(title1);
            this.chart_Line.Titles.Add(title2);
            // 
            // treeView_Name
            // 
            this.treeView_Name.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView_Name.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treeView_Name.HideSelection = false;
            this.treeView_Name.Location = new System.Drawing.Point(1182, -1);
            this.treeView_Name.Name = "treeView_Name";
            this.treeView_Name.ShowRootLines = false;
            this.treeView_Name.Size = new System.Drawing.Size(203, 552);
            this.treeView_Name.TabIndex = 5;
            this.treeView_Name.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_Name_AfterSelect);
            // 
            // ChartLineMultiple
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(1386, 551);
            this.Controls.Add(this.treeView_Name);
            this.Controls.Add(this.chart_Line);
            this.Name = "ChartLineMultiple";
            this.ShowIcon = false;
            this.Text = "ChartLineMultiple";
            this.Load += new System.EventHandler(this.ChartLineMultiple_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart_Line)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart_Line;
        private System.Windows.Forms.TreeView treeView_Name;
    }
}