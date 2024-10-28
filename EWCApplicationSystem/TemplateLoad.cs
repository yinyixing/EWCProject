using EWCLibrary;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EWCApplicationSystem
{
    public partial class TemplateLoad : Form
    {
        public TemplateLoad()
        {
            InitializeComponent();
        }
        IExcel iExcel = new ExcelHelper();
        string TemplateFile = Environment.CurrentDirectory + "\\Template\\Template.xlsx";
        private void TemplateLoad_Load(object sender, EventArgs e)
        {
            try
            {
                dgv_template.RowHeadersWidth = 25;
                dgv_template.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgv_template.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dgv_template.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                DataTable dt_excel = iExcel.ReadExcelToDataTable(TemplateFile);
                //sort
                DataTable dt_tmep = GlobalMethods.dt_sort_indicators(dt_excel);
                dgv_template.DataSource = dt_tmep;
                GlobalMethods.dt_temp = dt_tmep;
                GlobalMethods.Not_Sortable(dgv_template);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Template Load Error:"+ex.Message);
            }
        }
        /// <summary>
        /// Merge cells with the same content
        /// </summary>
        private void dgv_template_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            GlobalMethods.Merge_Cells_3(e, dgv_template);
        }
        /// <summary>
        /// Import Template
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog pOfd = new OpenFileDialog();
                pOfd.Title = "Open Excel File";
                pOfd.Filter = "Excel File (*.xls;*.xlsx)|*.xls;*.xlsx";
                pOfd.Multiselect = false;//Multiple selections are not allowed
                GlobalMethods.dt_error = GlobalMethods.dt_error_field();
                int err_count = 0;
                if (pOfd.ShowDialog() == DialogResult.OK)
                {
                    string filePath = pOfd.FileName;
                    DataTable dt_import = iExcel.Template_ReadExcelToDataTable(filePath);
                    if (GlobalMethods.dt_error != null && GlobalMethods.dt_error.Rows.Count > 0)
                    {
                        errrMsgForm emf = new errrMsgForm();
                        emf.ShowDialog();
                    }
                    else
                    {
                        dgv_template.Columns.Clear();
                        dgv_template.AutoGenerateColumns = true;
                        dgv_template.DataSource = dt_import;
                        GlobalMethods.dt_temp = dt_import;
                        dgv_template.Refresh();
                        GlobalMethods.Not_Sortable(dgv_template);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Import template Error:" + ex.Message);
            }
        }
        /// <summary>
        /// delete data
        /// </summary>
        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (dgv_template.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a piece of data to delete!", "prompted", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                try
                {
                    DialogResult dir = MessageBox.Show("Are you sure you want to delete it?", "prompted", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dir == DialogResult.OK)
                    {
                        foreach (DataGridViewRow row in dgv_template.SelectedRows)
                        {
                            if (!row.IsNewRow)
                            {
                                dgv_template.Rows.Remove(row);
                            }
                        }
                        MessageBox.Show("delete success");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error:" + ex.Message);
                }
            }
        }
        /// <summary>
        /// save template
        /// </summary>
        private void btn_save_Click(object sender, EventArgs e)
        {
            DataTable dt_table = iExcel.ToDataTableIM(dgv_template);
            IWorkbook workbook = new XSSFWorkbook();
            iExcel.ExportToSheet(workbook, dt_table, "sheet1");
            using (FileStream fStream = new FileStream(TemplateFile, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fStream);
                fStream.Close();
            }
            GC.Collect();
            MessageBox.Show("Save template successful.");
        }

        private void dgv_template_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == 4)
            {
                if (e.FormattedValue.ToString() != "positive" && e.FormattedValue.ToString() != "negative")
                {
                    if (e.FormattedValue.ToString() != "")
                    {
                        dgv_template.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "Only positive or negative can be filled in under the positive-negative field!";
                    }
                }
                else
                {
                    dgv_template.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = String.Empty;
                }

            }
        }
    }
}
