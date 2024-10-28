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
    public partial class ChartLine : Form
    {
        public ChartLine()
        {
            InitializeComponent();
        }
        string text_name = "";
        DataTable dt_result = null;
        int fieldIndex = 0;
        public ChartLine(string text_name, DataTable dt_result, int fieldIndex)
        {
            InitializeComponent();
            this.text_name = text_name;
            this.dt_result = dt_result;
            this.fieldIndex = fieldIndex;
        }

        private void ChartLine_Load(object sender, EventArgs e)
        {
            treeView_Name.Nodes.Clear();
            treeView_Name.Indent = 100;//node indentation distance
            treeView_Name.ItemHeight = 25;//node spacing
            //Set ChartAreas properties
            chart_Line.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.NotSet; //remove the grid
            chart_Line.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.NotSet; //remove the grid
            chart_Line.ChartAreas[0].AxisX.IsMarginVisible = false;//remove excess scale lines on both sides of the x-axis
            chart_Line.ChartAreas[0].CursorY.Interval = 0.001;//set the cursor interval
            chart_Line.ChartAreas[0].CursorY.IsUserEnabled = true;//enable the cursor
            chart_Line.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;//enable the range selection
            foreach (var series in chart_Line.Series)
            {
                series.Points.Clear();
            }
            chart_Line.Series.Clear();
            if (text_name == "dn")
            {
                if (GlobalMethods.sij_rij == "Rij")
                {
                    this.Text = "Value of Rij";
                    chart_Line.Titles[0].Text = "Value of Rij";
                }
                else if (GlobalMethods.sij_rij == "Sij")
                {
                    this.Text = "Value of Rij";
                    chart_Line.Titles[0].Text = "Value of Rij";
                }
            }
            else
            {
                this.Text = text_name;
                chart_Line.Titles[0].Text = text_name;
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
            foreach (var series in chart_Line.Series)
            {
                series.Points.Clear();
            }
            chart_Line.Series.Clear();
            int index = treeView_Name.SelectedNode.Index;
            DataTable dt_result2 = dt_result.Copy();
            if (text_name != "Value of Fj")
            {
                dt_result2.Columns.Remove("positive-negative");
            }
            if (index == 0)
            {
                chart_Line.Legends[0].Enabled = true;
                for (int j = 0; j < dt_result2.Rows.Count; j++)
                {
                    DataTable dt_line = new DataTable();
                    dt_line.Columns.Add("year", typeof(string));
                    dt_line.Columns.Add("values", typeof(double));
                    for (int i = fieldIndex + 1; i < dt_result2.Columns.Count; i++)
                    {
                        dt_line.Rows.Add(dt_result2.Columns[i].ColumnName, dt_result2.Rows[j][i]);
                    }
                    string s_name = dt_result2.Rows[j][fieldIndex].ToString();
                    Series pSeries = new Series(s_name);
                    pSeries.Points.DataBind(dt_line.AsEnumerable(), "year", "values", "");
                    pSeries.XValueType = ChartValueType.String; 
                    pSeries.ChartType = SeriesChartType.Spline;
                    chart_Line.Series.Add(pSeries);
                }
            }
            else
            {
                chart_Line.Legends[0].Enabled = false;
                DataTable dt_line = new DataTable();
                dt_line.Columns.Add("year", typeof(string));
                dt_line.Columns.Add("values", typeof(double));
                for (int i = fieldIndex + 1; i < dt_result2.Columns.Count; i++)
                {
                    dt_line.Rows.Add(dt_result2.Columns[i].ColumnName, dt_result2.Rows[index - 1][i]);
                }
                string s_name = dt_result2.Rows[index - 1][fieldIndex].ToString();
                Series pSeries = new Series(s_name);
                pSeries.Points.DataBind(dt_line.AsEnumerable(), "year", "values", "");
                pSeries.XValueType = ChartValueType.String; 
                pSeries.ChartType = SeriesChartType.Spline;
                chart_Line.Series.Add(pSeries);
            }
        }
    }
}
