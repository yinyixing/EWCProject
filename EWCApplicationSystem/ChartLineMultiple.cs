using EWCLibrary;
using System;
using System.Collections;
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
    public partial class ChartLineMultiple : Form
    {
        public ChartLineMultiple()
        {
            InitializeComponent();
        }
        string text_name = "";
        Hashtable ht_dimension = null;
        public ChartLineMultiple(string text_name, Hashtable ht_result)
        {
            InitializeComponent();
            this.text_name = text_name;
            this.ht_dimension = ht_result;
        }
        private void ChartLineMultiple_Load(object sender, EventArgs e)
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
            if (text_name == "dn")
            {
                if (GlobalMethods.sij_rij == "Rij")
                {
                    this.Text = "Value of Rij";
                    chart_Line.Titles[0].Text = "Value of Rij";
                }
                else if (GlobalMethods.sij_rij == "Sij")
                {
                    this.Text = "Value of Sij";
                    chart_Line.Titles[0].Text = "Value of Sij";
                }
            }
            else
            {
                this.Text = text_name;
                chart_Line.Titles[0].Text = text_name;
            }
            foreach (string item in ht_dimension.Keys)
            {
                DataTable dt_one = ht_dimension[item] as DataTable;
                for (int j = 0; j < dt_one.Rows.Count; j++)
                {
                    string s_name = dt_one.Rows[j][1].ToString();
                    if (text_name == "Value of Fj")
                    {
                        s_name = dt_one.Rows[j][0].ToString();
                    }
                    treeView_Name.Nodes.Add(s_name);
                }
                break;
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
            foreach (string item in ht_dimension.Keys)
            {
                DataTable dt_sample = ht_dimension[item] as DataTable;
                DataTable dt_Column = new DataTable();
                dt_Column.Columns.Add("name", typeof(string));
                dt_Column.Columns.Add("values", typeof(double));
                int startIndex = 3;
                if (text_name == "Value of Fj")
                {
                    startIndex = 1;
                }
                for (int i = startIndex; i < dt_sample.Columns.Count; i++)
                {
                    dt_Column.Rows.Add(dt_sample.Columns[i].ColumnName, dt_sample.Rows[index][i]);
                }
                string s_name = item;
                Series pSeries = new Series(s_name);
                pSeries.Points.DataBind(dt_Column.AsEnumerable(), "name", "values", "");
                pSeries.XValueType = ChartValueType.String; 
                pSeries.ChartType = SeriesChartType.Line;   
                chart_Line.Series.Add(pSeries);
            }
        }
    }
}
