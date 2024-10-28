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
    public partial class errrMsgForm : Form
    {
        public errrMsgForm()
        {
            InitializeComponent();
        }
        IExcel iExcel = new ExcelHelper();
        private void errrMsgForm_Load(object sender, EventArgs e)
        {
            dgv_errMsg.DataSource = GlobalMethods.dt_error;
            dgv_errMsg.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dgv_errMsg.Refresh();
        }
        /// <summary>
        /// Export Error Table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsb_export_Click(object sender, EventArgs e)
        {
            iExcel.genericExport("ErrorMessage.xlsx", (DataTable)dgv_errMsg.DataSource, "Error Message");
        }
    }
}
