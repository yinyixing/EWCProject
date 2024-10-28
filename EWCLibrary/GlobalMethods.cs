using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EWCLibrary
{
    public static class GlobalMethods
    {
        public static Hashtable ht_original_able { get; set; }
        public static DataTable dt_temp = null; //indicator template table
        public static DataTable dt_error { get; set; }//error message table
        public static DataTable dt_sort_result = null; //sort result table
        public static bool is_avg = false;//whether to use the composite average as the source data
        public static DataTable dt_zb_avg = null;//store the average table
        public static string sij_rij = "";//normalized selection
        public static DataTable dt_weight = null;//entropy weight statistics - weight result table
        public static Dictionary<string, DataTable> dic_Sx_year = null;//table of indicator data after the specified year


        public static string[] strs_process = new string[] { "indicator", "subdimension", "dimension", "other" };
        public static DataTable dt_avg_result { get; set; }//store the results of all sample mean calculations
        public static Hashtable ht_gyh_all { get; set; }//stores all results after normalized Rij/Sij calculations
        public static DataTable dt_gyh_avg { get; set; }//store the result of the normalized calculation of the mean
        public static Hashtable ht_slh_k_all { get; set; }//store all the results after the entropy quantization k value is calculated
        public static Hashtable ht_slh_fij_all { get; set; }//store all the results after the entropy quantization fij value calculation
        public static Hashtable ht_slh_hi_all { get; set; }//store all results after the entropy quantization hi value is calculated
        public static Hashtable ht_qzz_all { get; set; }//stores all results after the normalized weight value Wi is calculated
        public static Hashtable ht_zhzs_all { get; set; }//store all the results after calculating the development index Fjφ


        /// <summary>
        /// table columns cannot be sorted
        /// </summary>
        /// <param name="dgv"></param>
        public static void Not_Sortable(DataGridView dgv)
        {
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                dgv.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        /// <summary>
        /// unified ranking of tables (indicators)
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DataTable dt_sort_indicators(DataTable dt)
        {
            dt.DefaultView.Sort = "theme asc,dimension asc,subdimension asc";
            dt = dt.DefaultView.ToTable();
            return dt;
        }

        /// <summary>
        /// unified ranking of tables
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DataTable dt_sort(DataTable dt)
        {
            dt.DefaultView.Sort = "Subsystem desc,Indicator";
            dt = dt.DefaultView.ToTable();
            return dt;
        }
        /// <summary>
        /// general error list
        /// </summary>
        /// <returns></returns>
        public static DataTable dt_error_field()
        {
            DataTable dt_error = new DataTable();
            dt_error.Columns.Add("Number", typeof(int));
            dt_error.Columns.Add("Error type", typeof(string));
            dt_error.Columns.Add("Error Reason", typeof(string));
            dt_error.Columns.Add("Error value", typeof(string));
            dt_error.Columns.Add("time", typeof(string));
            return dt_error;
        }
        /// <summary>
        /// the error table used to match the imported result data（check Representative indicators Table）
        /// </summary>
        /// <returns></returns>
        public static DataTable dt_error_field_RIT()
        {
            DataTable dt_error = new DataTable();
            dt_error.Columns.Add("Number", typeof(int));
            dt_error.Columns.Add("Subsystem", typeof(string));
            dt_error.Columns.Add("Error Reason", typeof(string));
            dt_error.Columns.Add("File Name", typeof(string));
            dt_error.Columns.Add("time", typeof(string));
            return dt_error;
        }


        /// <summary>
        /// The contents of the same cells in the first column of the table are merged
        /// </summary>
        public static void Merge_Cells(DataGridViewCellPaintingEventArgs e, DataGridView pDGV)
        {
            // Merge the same cells in column 1     
            if (e.ColumnIndex == 0 && e.RowIndex != -1)
            {
                Brush datagridBrush = new SolidBrush(pDGV.GridColor);
                SolidBrush groupLineBrush = new SolidBrush(e.CellStyle.BackColor);
                using (Pen datagridLinePen = new Pen(datagridBrush))
                {
                    // Clear cell
                    e.Graphics.FillRectangle(groupLineBrush, e.CellBounds);
                    if (e.RowIndex < pDGV.Rows.Count - 1 && pDGV.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value != null && pDGV.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value.ToString() != e.Value.ToString())
                    {
                        //Draw the bottom line
                        e.Graphics.DrawLine(datagridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right, e.CellBounds.Bottom - 1);
                        // Draw the right line
                        e.Graphics.DrawLine(datagridLinePen, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom);
                    }
                    else
                    {
                        // Draw the right line
                        e.Graphics.DrawLine(datagridLinePen, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom);
                    }

                    //Draw only the bottom line for the last entry
                    if (e.RowIndex == pDGV.Rows.Count - 1)
                    {
                        //Draw the bottom line
                        e.Graphics.DrawLine(datagridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right, e.CellBounds.Bottom - 1);
                    }
                    // Fill in the contents of the cell. Only the first cell with the same contents is filled in                     
                    if (e.Value != null)
                    {
                        if (e.RowIndex > 0 && pDGV.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value.ToString() == e.Value.ToString())
                        {
                        }
                        else
                        {
                            //Draws cell contents
                            e.Graphics.DrawString(e.Value.ToString(), e.CellStyle.Font, Brushes.Black, e.CellBounds.X + 2, e.CellBounds.Y + 5, StringFormat.GenericDefault);
                        }
                    }
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// The same cell contents in the first 3 columns of the table are merged and centered(with add)
        /// </summary>
        public static void Merge_Cells_3(DataGridViewCellPaintingEventArgs e, DataGridView pDGV)
        {
            if (e.RowIndex == pDGV.Rows.Count - 1)
            {
                return;
            }
            if (e.RowIndex >= 0 && (e.ColumnIndex == 0 || e.ColumnIndex == 1 || e.ColumnIndex == 2) && e.Value.ToString() != string.Empty)
            {
                #region
                int UpRows = 0;//Same number of rows above
                int DownRows = 0;//Same number of lines below
                int count = 0;//Total line number
                int cellwidth = e.CellBounds.Width;//Column width
                for (int i = e.RowIndex; i < pDGV.Rows.Count; i++) //Gets the number of rows below
                {
                    if (i == pDGV.Rows.Count - 1)
                    {
                        break;
                    }
                    if (pDGV.Rows[i].Cells[e.ColumnIndex].Value.ToString().Equals(e.Value.ToString()))
                    {
                        DownRows++;
                    }
                    else
                    {
                        break;
                    }
                }
                for (int i = e.RowIndex; i >= 0; i--) //Gets the number of rows above
                {
                    if (pDGV.Rows[i].Cells[e.ColumnIndex].Value.ToString().Equals(e.Value.ToString()))
                    {
                        UpRows++;
                    }
                    else
                    {
                        break;
                    }
                }
                count = UpRows + DownRows - 1;//Total line number               
                using (Brush gridBrush = new SolidBrush(pDGV.GridColor), backColorBrush = new SolidBrush(e.CellStyle.BackColor))
                {
                    using (Pen gridLinePen = new Pen(gridBrush))
                    {
                        e.Graphics.FillRectangle(backColorBrush, e.CellBounds); //Clear cell
                        if (e.Value != null)
                        {
                            int cellheight = e.CellBounds.Height;
                            SizeF size = e.Graphics.MeasureString(e.Value.ToString(), e.CellStyle.Font);
                            e.Graphics.DrawString((e.Value).ToString(), e.CellStyle.Font, Brushes.Black, e.CellBounds.X + (cellwidth - size.Width) / 2, e.CellBounds.Y - cellheight * (UpRows - 1) + (cellheight * count - size.Height) / 2, StringFormat.GenericDefault);
                        }
                        //If the data in the next row is not equal to the data in the current row, the bottom edge of the current cell is drawn
                        if (e.RowIndex < pDGV.Rows.Count - 2 && pDGV.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value.ToString() != e.Value.ToString())
                        {
                            e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
                        }
                        if (e.RowIndex == pDGV.Rows.Count - 1)
                        {
                            e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left + 2, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
                            count = 0;
                        }
                        //Draw the right line of the grid                     
                        e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom);
                        e.Handled = true;
                    }
                }
                #endregion
            }
        }
        /// <summary>
        /// The contents of the same cells in the first n columns of the table are merged and centered(Add without)
        /// </summary>
        public static void Merge_Cells_n_noAdd(DataGridViewCellPaintingEventArgs e, DataGridView pDGV, int n)
        {
            if (e.RowIndex >= 0 && (e.ColumnIndex < n) && e.Value.ToString() != string.Empty)
            {
                #region
                int UpRows = 0;
                int DownRows = 0;
                int count = 0;
                int cellwidth = e.CellBounds.Width;
                for (int i = e.RowIndex; i < pDGV.Rows.Count; i++) 
                {
                    if (pDGV.Rows[i].Cells[e.ColumnIndex].Value.ToString().Equals(e.Value.ToString()))
                    {
                        DownRows++;
                    }
                    else
                    {
                        break;
                    }
                }
                for (int i = e.RowIndex; i >= 0; i--)
                {
                    if (pDGV.Rows[i].Cells[e.ColumnIndex].Value.ToString().Equals(e.Value.ToString()))
                    {
                        UpRows++;
                    }
                    else
                    {
                        break;
                    }
                }
                count = UpRows + DownRows - 1;               
                using (Brush gridBrush = new SolidBrush(pDGV.GridColor), backColorBrush = new SolidBrush(e.CellStyle.BackColor))
                {
                    using (Pen gridLinePen = new Pen(gridBrush))
                    {
                        e.Graphics.FillRectangle(backColorBrush, e.CellBounds);
                        if (e.Value != null)
                        {
                            int cellheight = e.CellBounds.Height;
                            SizeF size = e.Graphics.MeasureString(e.Value.ToString(), e.CellStyle.Font);
                            e.Graphics.DrawString((e.Value).ToString(), e.CellStyle.Font, Brushes.Black, e.CellBounds.X + (cellwidth - size.Width) / 2, e.CellBounds.Y - cellheight * (UpRows - 1) + (cellheight * count - size.Height) / 2, StringFormat.GenericDefault);
                        }
                        //If the data in the next row is not equal to the data in the current row, the bottom edge of the current cell is drawn
                        if (e.RowIndex < pDGV.Rows.Count - 1 && pDGV.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value.ToString() != e.Value.ToString())
                        {
                            e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
                        }
                        if (e.RowIndex == pDGV.Rows.Count - 1)
                        {
                            e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left + 2, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
                            count = 0;
                        }
                        //Draw the right line of the grid                    
                        e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom);
                        e.Handled = true;
                    }
                }
                #endregion
            }
        }
        public static void Merge_Cells_n_noAdd(DataGridViewCellPaintingEventArgs e, DataGridView pDGV, bool isHaveAdd = false)
        {
            if (isHaveAdd)
            {
                if (e.RowIndex == pDGV.Rows.Count - 1)
                {
                    return;
                }
            }
            if (e.ColumnIndex == 0 && e.RowIndex != -1 || e.ColumnIndex == 1 && e.RowIndex != -1 || e.ColumnIndex == 2 && e.RowIndex != -1)
            {
                Brush datagridBrush = new SolidBrush(pDGV.GridColor);
                SolidBrush groupLineBrush = new SolidBrush(e.CellStyle.BackColor);
                using (Pen datagridLinePen = new Pen(datagridBrush))
                {
                    e.Graphics.FillRectangle(groupLineBrush, e.CellBounds);
                    if (e.RowIndex < pDGV.Rows.Count - 1 && pDGV.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value != null && pDGV.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value.ToString() != e.Value.ToString())
                    {
                        e.Graphics.DrawLine(datagridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right, e.CellBounds.Bottom - 1);
                        e.Graphics.DrawLine(datagridLinePen, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom);
                    }
                    else
                    {
                        e.Graphics.DrawLine(datagridLinePen, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom);
                    }

                    if (e.RowIndex == pDGV.Rows.Count - 1)
                    {
                        e.Graphics.DrawLine(datagridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right, e.CellBounds.Bottom - 1);
                    }

                    if (e.Value != null)
                    {
                        if (e.RowIndex > 0 && pDGV.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value.ToString() == e.Value.ToString())
                        {
                        }
                        else
                        {
                            string newvalues = e.Value.ToString().Replace(" ", "\n");
                            e.Graphics.DrawString(newvalues, e.CellStyle.Font, Brushes.MediumBlue, e.CellBounds.X + 2, e.CellBounds.Y + 5, StringFormat.GenericDefault);
                        }
                    }
                    e.Handled = true;
                }
            }
        }
    }
}
