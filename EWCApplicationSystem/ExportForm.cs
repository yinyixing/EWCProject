using EWCLibrary;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
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
    public partial class ExportForm : Form
    {
        public ExportForm()
        {
            InitializeComponent();
        }
        IExcel iExcel = new ExcelHelper();
        private void ExportForm_Load(object sender, EventArgs e)
        {
            if (GlobalMethods.ht_gyh_all.Count <= 0)
            {
                ckb_Nmatrix.Enabled = false;
            }
            if (GlobalMethods.ht_slh_fij_all.Count <= 0)
            {
                ckb_Nquan.Enabled = false;
            }
            if (GlobalMethods.ht_qzz_all.Count <= 0)
            {
                ckb_Weight.Enabled = false;
            }
            if (GlobalMethods.ht_zhzs_all.Count <= 0)
            {
                ckb_Dindex.Enabled = false;
            }
        }
        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_export_Click(object sender, EventArgs e)
        {
            try
            {
                if (txt_folder_path.Text == "")
                {
                    MessageBox.Show("No folder path has been entered/selected.");
                    return;
                }
                if (Directory.Exists(txt_folder_path.Text))
                {
                    string folderPath = txt_folder_path.Text;
                    DateTime currentTime = DateTime.Now;
                    string now_time = currentTime.ToString("yyyyMMddHHmmss");
                    if (ckb_Nmatrix.Checked)
                    {
                        export_dn(folderPath, now_time);
                    }
                    if (ckb_Nquan.Checked)
                    {
                        export_eq(folderPath, now_time);
                    }
                    if (ckb_Weight.Checked)
                    {
                        export_wd(folderPath, now_time);
                    }
                    if (ckb_Dindex.Checked)
                    {
                        export_di(folderPath, now_time);
                    }
                    MessageBox.Show("Export successful");
                }
                else
                {
                    MessageBox.Show("This folder does not exist, please re-enter/select again.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pib_choose_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Select the folder to store the result data";
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txt_folder_path.Text = fbd.SelectedPath;
            }
        }
        private void export_current_multilayer(string filename, Hashtable ht)
        {
            IWorkbook workbook = new XSSFWorkbook();
            ICollection keys_gyh = ht.Keys;
            foreach (string item in keys_gyh)
            {
                Hashtable ht2 = ht[item] as Hashtable;
                ICollection keys_gyh2 = ht2.Keys;
                foreach (string itemChild in keys_gyh2)
                {
                    iExcel.ExportToSheet(workbook, ht2[itemChild] as DataTable, itemChild.Replace("'","") + "-" + item);
                }
            }
            using (FileStream fStream = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fStream);
                fStream.Close();
            }
            GC.Collect();
        }
        private void export_current_singlelayer(string filename, Hashtable ht)
        {
            IWorkbook workbook = new XSSFWorkbook();
            ICollection keys_gyh = ht.Keys;
            foreach (string item in keys_gyh)
            { 
                DataTable  dt = ht[item] as DataTable;
                iExcel.ExportToSheet(workbook, ht[item] as DataTable, item);
            }
            using (FileStream fStream = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fStream);
                fStream.Close();
            }
            GC.Collect();
        }
        #region Export data normalization related tables
        private void export_dn(string folderName, string time)
        {
            try
            {
                string filename = folderName + "\\Result_Data normalization(Rij)_" + time + ".xlsx";
                export_current_multilayer(filename, GlobalMethods.ht_gyh_all);
            }
            catch (Exception)
            {
                return;
            }
        }
        #endregion
        #region Export Entropy Quantification Related Tables
        private void export_eq(string folderName, string time)
        {
            try
            {
                string filename = folderName + "\\Result_Entropy quantification(Fij)_" + time + ".xlsx";
                export_current_multilayer(filename, GlobalMethods.ht_slh_fij_all);
                string filename2 = folderName + "\\Result_Entropy quantification(Hi)_" + time + ".xlsx";
                export_current_multilayer(filename2, GlobalMethods.ht_slh_hi_all);
            }
            catch (Exception)
            {
                return;
            }
        }
        #endregion
        #region Export Weight determination  Related Tables
        private void export_wd(string folderName, string time)
        {
            try
            {
                string filename = folderName + "\\Result_Weight determination(Wi)_" + time + ".xlsx";
                export_current_multilayer(filename, GlobalMethods.ht_qzz_all);
            }
            catch (Exception)
            {
                return;
            }
        }
        #endregion
        #region Export Development index  Related Tables
        private void export_di(string folderName, string time)
        {
            try
            {
                string filename = folderName + "\\Result_Development index(Fj)_" + time + ".xlsx";
                export_current_multilayer(filename, GlobalMethods.ht_zhzs_all);
            }
            catch (Exception)
            {
                return;
            }
        }
        #endregion
    }
}
