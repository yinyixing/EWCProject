using EWCLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace EWCApplicationSystem
{
    public partial class ChartBar : Form
    {
        public ChartBar()
        {
            InitializeComponent();
        }
        string text_name = "";
        DataTable dt_result = null;
        int fieldIndex = 0;
        public ChartBar(string text_name, DataTable dt_result, int fieldIndex)
        {
            InitializeComponent();
            this.text_name = text_name;
            this.dt_result = dt_result;
            this.fieldIndex = fieldIndex;
        }
        private void ChartBar_Load(object sender, EventArgs e)
        {
            treeView_Name.Nodes.Clear();
            treeView_Name.Indent = 100;//node indentation distance
            treeView_Name.ItemHeight = 25;//node spacing
            chart_column.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.NotSet; //remove the grid
            chart_column.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.NotSet; //remove the grid
            chart_column.ChartAreas[0].AxisX.IsMarginVisible = false;//remove excess scale lines on both sides of the x-axis
            //chart_column.Legends[0].Enabled = false;//cancel legend
            chart_column.ChartAreas[0].CursorY.Interval = 0.001;//set the cursor interval
            chart_column.ChartAreas[0].CursorY.IsUserEnabled = true;//enable the cursor
            chart_column.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;//enable the range selection

            if (text_name == "dn")
            {
                if (GlobalMethods.sij_rij == "Rij")
                {
                    this.Text = "Value of Rij";
                    chart_column.Titles[0].Text = "Value of Rij";
                }
                else if (GlobalMethods.sij_rij == "Sij")
                {
                    this.Text = "Value of Sij";
                    chart_column.Titles[0].Text = "Value of Sij";
                }
            }
            else
            {
                this.Text = text_name;
                chart_column.Titles[0].Text = text_name;
            }
            treeView_Name.Nodes.Add("ALL");
            for (int j = 0; j < dt_result.Rows.Count; j++)
            {
                string s_name = dt_result.Rows[j][fieldIndex].ToString();
                treeView_Name.Nodes.Add(s_name);
            }
            treeView_Name.SelectedNode = treeView_Name.Nodes[0];
            treeView_Name.Focus();
        }

        private void treeView_Name_AfterSelect(object sender, TreeViewEventArgs e)
        {
            foreach (var series in chart_column.Series)
            {
                series.Points.Clear();
            }
            chart_column.Series.Clear();
            int index = treeView_Name.SelectedNode.Index;
            DataTable dt_result2 = dt_result.Copy();
            if (text_name != "Value of Fj(φ)")
            {
                dt_result2.Columns.Remove("positive-negative");
            }
            if (index == 0)
            {
                chart_column.Legends[0].Enabled = true;

                for (int j = 0; j < dt_result.Rows.Count; j++)
                {
                    DataTable dt_Column = new DataTable();
                    dt_Column.Columns.Add("name", typeof(string));
                    dt_Column.Columns.Add("values", typeof(double));
                    for (int i = fieldIndex + 1; i < dt_result2.Columns.Count; i++)
                    {
                        dt_Column.Rows.Add(dt_result2.Columns[i].ColumnName, dt_result2.Rows[j][i]);
                    }
                    string s_name = dt_result2.Rows[j][fieldIndex].ToString();
                    Series pSeries = new Series(s_name);
                    pSeries.Points.DataBind(dt_Column.AsEnumerable(), "name", "values", "");
                    pSeries.XValueType = ChartValueType.String; 
                    pSeries.ChartType = SeriesChartType.Column;   
                    chart_column.Series.Add(pSeries);
                    this.chart_column.Series[0].Palette = ChartColorPalette.Bright;
                }
            }
            else
            {
                chart_column.Legends[0].Enabled = false;
                DataTable dt_Column = new DataTable();
                dt_Column.Columns.Add("name", typeof(string));
                dt_Column.Columns.Add("values", typeof(double));
                for (int i = fieldIndex + 1; i < dt_result2.Columns.Count; i++)
                {
                    dt_Column.Rows.Add(dt_result2.Columns[i].ColumnName, dt_result2.Rows[index - 1][i]);
                }
                string s_name = dt_result2.Rows[index - 1][fieldIndex].ToString();
                Series pSeries = new Series(s_name);
                pSeries.Points.DataBind(dt_Column.AsEnumerable(), "name", "values", "");
                pSeries.XValueType = ChartValueType.String; 
                pSeries.ChartType = SeriesChartType.Column;   
                chart_column.Series.Add(pSeries);
                this.chart_column.Series[0].Palette = ChartColorPalette.Bright;
            }
        }
    }
}
