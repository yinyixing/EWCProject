using EWCLibrary;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace EWCApplicationSystem
{
    /*
     * A correction to the entropy weight coefficient method by Shen et al. for accessing urban sustainability
     * https://www.sciencedirect.com/science/article/pii/S0264275120300603
     */
    public partial class EntropyWeightMethod : Form
    {
        //Multiple original tables storing multiple samples
        public static Hashtable ht_original_able = null;
        ICalculation iCalculation = new CalculationMethod();
        IExcel iExcel = new ExcelHelper();
        public EntropyWeightMethod()
        {
            InitializeComponent();
        }
        private void EntropyWeightMethod_Load(object sender, EventArgs e)
        {
            ht_original_able = new Hashtable();
            init_edit_status();
            txb_prompt.Text = "Tip: When entering the ‘Sample-Indicator-Year’ three-dimensional numerical matrix, please ensure that the indicators, years, and data volume are consistent across multiple samples!";

            #region Set datagridview
            dgv_zbjz.DefaultCellStyle.WrapMode = DataGridViewTriState.True;// Set the Style of cells to support line breaks
            dgv_zbjz.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;//Set the content of all cells to automatically adjust the height of rows
            dgv_zbjz.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;//Set each row of data to be centered vertically
            dgv_zbjz.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//Set column names to be centered

            dgv_gyh.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgv_gyh.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgv_gyh.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv_gyh.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridViewvaluefij.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridViewvaluefij.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewvaluefij.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewvaluefij.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridViewvalueH.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridViewvalueH.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewvalueH.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewvalueH.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //dataGridView_valueW.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView_valueW.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView_valueW.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView_valueW.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridView_valueW.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //dataGridView_valueF2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView_valueF2.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView_valueF2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView_valueF2.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridView_valueF2.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            #endregion

            #region Load template data
            try
            {
                string TemplateFile = Environment.CurrentDirectory + "\\Template\\Template.xlsx";
                GlobalMethods.dt_temp = GlobalMethods.dt_sort_indicators(iExcel.ReadExcelToDataTable(TemplateFile));
            }
            catch (Exception)
            {

            }
            #endregion
            treeView_sample.Nodes.Clear();
            treeView_sample.Indent = 100;//node indentation distance
            treeView_sample.ItemHeight = 50;//node spacing
            ImageList imgList = initFileNameImageList_EWM();
            treeView_sample.ImageList = imgList;//bind images
        }

        #region Button operations on the toolbar
        /// <summary>
        /// Load Template
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsb_temp_load_Click(object sender, EventArgs e)
        {
            TemplateLoad tLoad = new TemplateLoad();
            tLoad.ShowDialog();
            /*string TemplateFile = Environment.CurrentDirectory + "\\Template\\Template.xlsx";
            string fileName = Path.GetFileNameWithoutExtension(TemplateFile);
            foreach (TreeNode node in treeView_sample.Nodes)
            {
                if (node.Text == fileName)
                {
                    MessageBox.Show("The sample already exists ‘" + fileName + "’,Please do not add again or rename.");
                    return;
                }
            }
            init_load_indicators_matrix(TemplateFile);*/
        }
        /// <summary>
        /// Import Template Download
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsb_TD_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sFileDialog = new SaveFileDialog())
            {
                sFileDialog.Title = "Template Download";
                sFileDialog.FileName = "Template.xlsx"; //Set default save as file name
                sFileDialog.Filter = "Excel file(*.xlsx)|*.xlsx|Excel 文件(*.xls)|*.xls";
                sFileDialog.AddExtension = true;
                if (sFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string targetFilePath = sFileDialog.FileName;
                    string TemplateFile = Environment.CurrentDirectory + "\\Template\\Template.xlsx";
                    // copy file
                    File.Copy(TemplateFile, targetFilePath, true);
                    MessageBox.Show("Download completed.");
                }
            }
        }
        /// <summary>
        /// Import Excel data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static Dictionary<string, bool> dic_Indicator = null;//Store all indicatorName
        private void tsButton_import_Click(object sender, EventArgs e)
        {
            if (GlobalMethods.dt_temp == null)
            {
                MessageBox.Show("Template data not loaded, please perform 'Template Load' first");
                return;
            }
            OpenFileDialog pOfd = new OpenFileDialog();
            pOfd.Title = "open excel file";
            pOfd.Filter = "Excel file (*.xls;*.xlsx)|*.xls;*.xlsx";
            pOfd.Multiselect = true;//Multiple choices
            GlobalMethods.dt_error = GlobalMethods.dt_error_field();
            int err_count = 0;
            dic_Indicator = new Dictionary<string, bool>();
            if (pOfd.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < GlobalMethods.dt_temp.Rows.Count; i++)
                {
                    string IndicatorName = GlobalMethods.dt_temp.Rows[i][3].ToString();
                    dic_Indicator.Add(IndicatorName, false);
                }
                int beforeCount = treeView_sample.Nodes.Count;
                foreach (var file in pOfd.FileNames)
                {
                    string fileName = Path.GetFileNameWithoutExtension(file);
                    bool ishave = false;
                    foreach (TreeNode node in treeView_sample.Nodes)
                    {
                        if (node.Text == fileName)//Does the same sample name already exist in the left menu
                        {
                            ishave = true;
                            err_count++;
                            GlobalMethods.dt_error.Rows.Add(new object[] { err_count, "import error", "The sample already exists ‘" + fileName + "’,Please do not add again or rename.", fileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                        }
                    }
                    if (ishave == false)
                    {
                        //init_load_indicators_matrix(file);
                        DataTable dt_filter = iExcel.ReadExcelFilterDataTable(file, GlobalMethods.dt_temp, dic_Indicator);
                        if (dt_filter != null)
                        {
                            init_load_indicators_matrix(dt_filter, fileName);
                        }
                    }
                }
                int afterCount = treeView_sample.Nodes.Count;
                //Is there any change in the sample size bound to the left menu? Only when there is a change will the calculated data be cleared
                if (afterCount > beforeCount)
                {
                    clearDGV();
                }
                if (GlobalMethods.dt_error != null && GlobalMethods.dt_error.Rows.Count > 0)
                {
                    errrMsgForm emf = new errrMsgForm();
                    emf.ShowDialog();
                }
            }
        }
        private void bindTreeView(TreeView tv)
        {
            tv.Nodes.Clear();
            tv.Indent = 100;//node indentation distance
            tv.ItemHeight = 50;//node spacing
            ImageList imgList = initFileNameImageList_EWM();
            tv.ImageList = imgList;//bind images
        }
        /// <summary>
        /// Data normalization button operation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsButton_gyh_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgv_zbjz.DataSource != null && dgv_zbjz.Rows.Count > 0)
                {
                    this.tabControl1.SelectedIndex = 0;
                    this.panelintroduction.Controls.Clear();
                    Sij_Rij sr = new Sij_Rij(treeView_sample.Nodes.Count);
                    sr.ShowDialog();
                    if (sr.DialogResult == DialogResult.OK)
                    {
                        //Initialize various result tables
                        GlobalMethods.dt_avg_result = new DataTable();
                        GlobalMethods.dt_gyh_avg = new DataTable();
                        GlobalMethods.ht_gyh_all = new Hashtable();
                        GlobalMethods.ht_slh_k_all = new Hashtable();
                        GlobalMethods.ht_slh_fij_all = new Hashtable();
                        GlobalMethods.ht_slh_hi_all = new Hashtable();
                        GlobalMethods.ht_qzz_all = new Hashtable();
                        GlobalMethods.ht_zhzs_all = new Hashtable();
                        GlobalMethods.ht_original_able = ht_original_able;
                        if (GlobalMethods.is_avg)
                        {
                            //Calculate the comprehensive average and normalize it again
                            GlobalMethods.dt_avg_result = iCalculation.calculation_avg(ht_original_able);
                        }
                        #region Normalization value calculation(Just normalize the indicator layer, and the subsequent dimensions will be the same as the calculated values of the composite index, without the need for repeated normalization)
                        Hashtable ht_gyh = iCalculation.calculation_gyh(ht_original_able);
                        GlobalMethods.ht_gyh_all.Add(iCalculation.process_name(0), ht_gyh);
                        #endregion
                        iCalculation.calculation_process(ht_original_able, ht_gyh, 0);

                        /*//Tree menu after binding multi-dimensional statistics of data sources
                        bindTreeView(treeView_statistical_dimension);
                        for (int i = 0; i < GlobalMethods.strs_process.Length - 1; i++)
                        {
                            treeView_statistical_dimension.Nodes.Add(GlobalMethods.strs_process[i], GlobalMethods.strs_process[i], 3, 3);
                        }
                        treeView_statistical_dimension.SelectedNode = treeView_statistical_dimension.Nodes[0];
                        treeView_statistical_dimension.Focus();*/
                        dn_treeview();
                        if (GlobalMethods.sij_rij == "Rij")//Normalization method of Rij
                        {
                            this.panelintroduction.Controls.Add(new EWCApplicationSystem.panels.panelrij());
                        }
                        else if (GlobalMethods.sij_rij == "Sij")//Normalization method of Sij
                        {
                            this.panelintroduction.Controls.Add(new EWCApplicationSystem.panels.panelSij());
                        }
                        else
                        {
                            MessageBox.Show("No data normalization method selected!");
                        }
                    }
                }
                else
                {
                    OperatingSteps oss = new OperatingSteps("Please perform step ① first(Load/Import indicators matrix Data),Perform step ② again(Calculate Data normalization)!");
                    oss.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Data normalization Error:" + ex.Message);
            }
        }
        private void dn_treeview()
        {
            //Tree menu after binding multi-dimensional statistics of data sources
            bindTreeView(treeView_statistical_dimension);
            /*foreach (string name in GlobalMethods.ht_gyh_all.Keys)
            {
                treeView_statistical_dimension.Nodes.Add(name, name, 3, 3);
            }*/
            treeView_statistical_dimension.Nodes.Add("indicator", "indicator", 3, 3);
            /*for (int i = 0; i < GlobalMethods.strs_process.Length - 1; i++)
            {
                if (GlobalMethods.ht_gyh_all.ContainsKey(GlobalMethods.strs_process[i]))
                {
                    treeView_statistical_dimension.Nodes.Add(GlobalMethods.strs_process[i], GlobalMethods.strs_process[i], 3, 3);
                }
            }*/
            treeView_statistical_dimension.SelectedNode = treeView_statistical_dimension.Nodes[0];
            treeView_statistical_dimension.Focus();
        }
        /// <summary>
        /// Entropy quantification button operation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsButton_eq_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgv_gyh.DataSource != null && dgv_gyh.Rows.Count > 0)
                {
                    this.tabControl1.SelectedIndex = 1;
                    eq_treeview();
                    string dimension_node_name = this.treeView_statistical_dimension.SelectedNode.Name;
                    string sample_node_name = treeView_sample.SelectedNode.Text;
                    switchSLH(dimension_node_name, sample_node_name);
                }
                else
                {
                    OperatingSteps oss = new OperatingSteps("Please perform step ② first(Calculate Data normalization),Perform step ③ again(Calculate Entropy quantification)!");
                    oss.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Entropy quantification Error:" + ex.Message);
            }

        }
        private void eq_treeview()
        {
            //Tree menu after binding multi-dimensional statistics of data sources
            bindTreeView(treeView_statistical_dimension);
            /*foreach (string name in GlobalMethods.ht_slh_fij_all.Keys)
            {
                treeView_statistical_dimension.Nodes.Add(name, name, 3, 3);
            }*/
            for (int i = 0; i < GlobalMethods.strs_process.Length - 1; i++)
            {
                if (GlobalMethods.ht_slh_fij_all.ContainsKey(GlobalMethods.strs_process[i]))
                {
                    treeView_statistical_dimension.Nodes.Add(GlobalMethods.strs_process[i], GlobalMethods.strs_process[i], 3, 3);
                }
            }
            treeView_statistical_dimension.SelectedNode = treeView_statistical_dimension.Nodes[0];
            treeView_statistical_dimension.Focus();
        }
        #region <Weight determination button operation>

        private void tsButton_wd_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewvalueH.DataSource != null && dataGridViewvalueH.Rows.Count > 0)
                {
                    this.tabControl1.SelectedIndex = 2;
                    wd_treeview();
                    string dimension_node_name = this.treeView_statistical_dimension.SelectedNode.Name;
                    string sample_node_name = treeView_sample.SelectedNode.Text;
                    switchQZZ(dimension_node_name, sample_node_name);
                }
                else
                {
                    OperatingSteps oss = new OperatingSteps("Please perform step ③ first(Calculate Entropy quantification),Perform step ④ again(Calculate Weight determination)!");
                    oss.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Weight determination Error:" + ex.Message);
            }
        }
        private void wd_treeview()
        {
            //Tree menu after binding multi-dimensional statistics of data sources
            bindTreeView(treeView_statistical_dimension);
            /*foreach (string name in GlobalMethods.ht_qzz_all.Keys)
            {
                treeView_statistical_dimension.Nodes.Add(name, name, 3, 3);
            }*/
            for (int i = 0; i < GlobalMethods.strs_process.Length - 1; i++)
            {
                if (GlobalMethods.ht_qzz_all.ContainsKey(GlobalMethods.strs_process[i]))
                {
                    treeView_statistical_dimension.Nodes.Add(GlobalMethods.strs_process[i], GlobalMethods.strs_process[i], 3, 3);
                }
            }
            treeView_statistical_dimension.SelectedNode = treeView_statistical_dimension.Nodes[0];
            treeView_statistical_dimension.Focus();
        }
        #endregion
        #region <Development index button operation>
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView_valueW.DataSource != null && dataGridView_valueW.Rows.Count > 0)
                {
                    this.tabControl1.SelectedIndex = 3;
                    di_treeview();
                    string dimension_node_name = this.treeView_statistical_dimension.SelectedNode.Name;//get key
                    string sample_node_name = treeView_sample.SelectedNode.Text;
                    switchFZZS(dimension_node_name, sample_node_name);
                }
                else
                {
                    OperatingSteps oss = new OperatingSteps("Please perform step ④ first(Calculate Weight determination),Perform step ⑤ again(Calculate Development index)!");
                    oss.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Development index Error:" + ex.Message);
            }
        }
        private void di_treeview()
        {
            //Tree menu after binding multi-dimensional statistics of data sources
            bindTreeView(treeView_statistical_dimension);
            if (!GlobalMethods.ht_zhzs_all.ContainsKey("dimension") && !GlobalMethods.ht_zhzs_all.ContainsKey("subdimension"))
            {
                treeView_statistical_dimension.Nodes.Add("indicator", "theme", 3, 3);
            }
            else if (!GlobalMethods.ht_zhzs_all.ContainsKey("subdimension"))
            {
                treeView_statistical_dimension.Nodes.Add("indicator", "dimension", 3, 3);
                treeView_statistical_dimension.Nodes.Add("dimension", "theme", 3, 3);
            }
            else if (!GlobalMethods.ht_zhzs_all.ContainsKey("dimension"))
            {
                treeView_statistical_dimension.Nodes.Add("indicator", "subdimension", 3, 3);
                treeView_statistical_dimension.Nodes.Add("subdimension", "theme", 3, 3);
            }
            else
            {
                treeView_statistical_dimension.Nodes.Add("indicator", "subdimension", 3, 3);
                treeView_statistical_dimension.Nodes.Add("subdimension", "dimension", 3, 3);
                treeView_statistical_dimension.Nodes.Add("dimension", "theme", 3, 3);
            }
            /*for (int i = 0; i < GlobalMethods.strs_process.Length - 1; i++)
            {
                if (GlobalMethods.strs_process[i] == "indicator")
                {
                    treeView_statistical_dimension.Nodes.Add(GlobalMethods.strs_process[i], "subdimension", 3, 3);
                }
                else if (GlobalMethods.strs_process[i] == "subdimension")
                {
                    treeView_statistical_dimension.Nodes.Add(GlobalMethods.strs_process[i], "dimension", 3, 3);
                }
                else if (GlobalMethods.strs_process[i] == "dimension")
                {
                    treeView_statistical_dimension.Nodes.Add(GlobalMethods.strs_process[i], "theme", 3, 3);
                }
            }*/
            /*foreach (string name in GlobalMethods.ht_zhzs_all.Keys)
            {
                if (name == "indicator")
                {
                    treeView_statistical_dimension.Nodes.Add(name, "subdimension", 3, 3);
                }
                else if (name == "subdimension")
                {
                    treeView_statistical_dimension.Nodes.Add(name, "dimension", 3, 3);
                }
                else if (name == "dimension")
                {
                    treeView_statistical_dimension.Nodes.Add(name, "theme", 3, 3);
                }
            }*/
            treeView_statistical_dimension.SelectedNode = treeView_statistical_dimension.Nodes[0];
            treeView_statistical_dimension.Focus();
        }
        #endregion
        #endregion
        /// <summary>
        /// TabControl switch, change formula
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int tabIndex = tabControl1.SelectedIndex;
            if (tabIndex == 0)
            {
                this.panelintroduction.Controls.Clear();
                if (GlobalMethods.sij_rij == "Rij")
                {
                    this.panelintroduction.Controls.Add(new EWCApplicationSystem.panels.panelrij());
                }
                else if (GlobalMethods.sij_rij == "Sij")
                {
                    this.panelintroduction.Controls.Add(new EWCApplicationSystem.panels.panelSij());
                }
            }
            else if (tabIndex == 1)
            {
                this.panelintroduction.Controls.Clear();
                this.panelintroduction.Controls.Add(new EWCApplicationSystem.panels.panelHi());
            }
            else if (tabIndex == 2)
            {
                this.panelintroduction.Controls.Clear();
                this.panelintroduction.Controls.Add(new EWCApplicationSystem.panels.panelWi());
            }
            else if (tabIndex == 3)
            {
                this.panelintroduction.Controls.Clear();
                this.panelintroduction.Controls.Add(new EWCApplicationSystem.panels.panelfcsf());
            }
            //Switching between calculation result tables
            if (treeView_sample.Nodes.Count > 0 && treeView_statistical_dimension.Nodes.Count > 0)
            {
                string sample_node_name = treeView_sample.SelectedNode.Text;
                if (tabIndex == 0)
                {
                    if (dgv_gyh.DataSource != null)
                    {
                        dn_treeview();
                        string dimension_node_name = this.treeView_statistical_dimension.SelectedNode.Name;
                        switchGYH(dimension_node_name, sample_node_name);
                    }
                }
                else if (tabIndex == 1)
                {
                    if (dataGridViewvaluefij.DataSource != null)
                    {
                        eq_treeview();
                        string dimension_node_name = this.treeView_statistical_dimension.SelectedNode.Name;
                        Hashtable ht_k = GlobalMethods.ht_slh_k_all[dimension_node_name] as Hashtable;
                        string k_value = ht_k[sample_node_name].ToString();
                        switchSLH(dimension_node_name, sample_node_name);
                    }
                }
                else if (tabIndex == 2)
                {
                    if (dataGridView_valueW.DataSource != null)
                    {
                        wd_treeview();
                        string dimension_node_name = this.treeView_statistical_dimension.SelectedNode.Name;
                        switchQZZ(dimension_node_name, sample_node_name);
                    }
                }
                else if (tabIndex == 3)
                {
                    if (dataGridView_valueF2.DataSource != null)
                    {
                        di_treeview();
                        string dimension_node_name = this.treeView_statistical_dimension.SelectedNode.Name;
                        switchFZZS(dimension_node_name, sample_node_name);
                    }
                }
            }
        }
        private void groupBox1_Paint(object sender, PaintEventArgs e)
        {
            groupBox_DrawLine(sender, e);
        }
        private void groupBox3_Paint(object sender, PaintEventArgs e)
        {
            groupBox_DrawLine(sender, e);
        }
        #region datagrideview Merge columns with the same content
        private void dgv_zbjz_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            GlobalMethods.Merge_Cells_n_noAdd(e, dgv_zbjz, true);
        }
        private void dgv_gyh_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            GlobalMethods.Merge_Cells_n_noAdd(e, dgv_gyh);
        }
        private void dataGridViewvaluefij_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            GlobalMethods.Merge_Cells_n_noAdd(e, dataGridViewvaluefij);
        }
        private void dataGridViewvalueH_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            GlobalMethods.Merge_Cells_n_noAdd(e, dataGridViewvalueH);
        }
        private void dataGridView_valueW_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            GlobalMethods.Merge_Cells_n_noAdd(e, dataGridView_valueW);
        }
        #endregion

        #region Operation of buttons within the indicators matrix
        private void lbl_enable_Click(object sender, EventArgs e)
        {
            enable_editing();
        }
        private void pb_enable_Click(object sender, EventArgs e)
        {
            enable_editing();
        }
        private void lbl_close_Click(object sender, EventArgs e)
        {
            close_editing();
        }

        private void pb_close_Click(object sender, EventArgs e)
        {
            close_editing();
        }
        private void lbl_save_edit_Click(object sender, EventArgs e)
        {
            save_edit();
        }

        private void pb_save_edit_Click(object sender, EventArgs e)
        {
            save_edit();
        }

        private void lbl_export_table_Click(object sender, EventArgs e)
        {
            exportThisTable();
        }

        private void pb_export_table_Click(object sender, EventArgs e)
        {
            exportThisTable();
        }

        private void lbl_add_Column_Click(object sender, EventArgs e)
        {
            add_column();
        }
        private void pb_add_Click(object sender, EventArgs e)
        {
            add_column();
        }
        private void lbl_delete_Column_Click(object sender, EventArgs e)
        {
            delete_column();
        }
        private void pb__delete_Column_Click(object sender, EventArgs e)
        {
            delete_column();
        }
        private void lbl_delete_Click(object sender, EventArgs e)
        {
            delete_data();
        }
        private void pb_delete_Click(object sender, EventArgs e)
        {
            delete_data();
        }
        /// <summary>
        /// Table editing button (Initialization status)
        /// </summary>
        private void init_edit_status()
        {
            dgv_zbjz.ReadOnly = true;

            pb_enable.Enabled = true;
            lbl_enable.Enabled = true;
            pb_close.Enabled = false;
            lbl_close.Enabled = false;
            pb_save_edit.Enabled = false;
            lbl_save_edit.Enabled = false;
            //pb_export_table.Enabled = false;
            //lbl_export_table.Enabled = false;
            pb_add_Column.Enabled = false;
            lbl_add_Column.Enabled = false;
            pb__delete_Column.Enabled = false;
            lbl_delete_Column.Enabled = false;
            pb_delete_data.Enabled = false;
            lbl_delete_data.Enabled = false;
        }
        /// <summary>
        /// Enable Edit
        /// </summary>
        private void enable_editing()
        {
            if (dgv_zbjz.DataSource != null && dgv_zbjz.Rows.Count > 0)
            {
                dgv_zbjz.ReadOnly = false;

                pb_enable.Enabled = false;
                lbl_enable.Enabled = false;
                pb_close.Enabled = true;
                lbl_close.Enabled = true;
                pb_save_edit.Enabled = true;
                lbl_save_edit.Enabled = true;
                //pb_export_table.Enabled = true;
                //lbl_export_table.Enabled = true;
                pb_add_Column.Enabled = true;
                lbl_add_Column.Enabled = true;
                pb__delete_Column.Enabled = true;
                lbl_delete_Column.Enabled = true;
                pb_delete_data.Enabled = true;
                lbl_delete_data.Enabled = true;
            }
            else
            {
                MessageBox.Show("indicators table data not loaded, unable to enable editing!");
            }
        }
        /// <summary>
        /// close edit
        /// </summary>
        private void close_editing()
        {
            string nodeName = this.treeView_sample.SelectedNode.Text;
            DialogResult dr_close = MessageBox.Show("Do you want to save the modifications and close the editing?(Yes represents saving and closing editing,No represents not saving but closing editing,Cancel means neither saving nor exiting editing.)", "prompt", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (dr_close == DialogResult.Yes)
            {
                DataTable dt_table = GlobalMethods.dt_sort_indicators(dgv_zbjz.DataSource as DataTable);
                ht_original_able[nodeName] = dt_table;
                close_edit_status();
            }
            else if (dr_close == DialogResult.No)
            {
                close_edit_status();
            }
        }
        private void close_edit_status()
        {
            dgv_zbjz.ReadOnly = true;

            pb_enable.Enabled = true;
            lbl_enable.Enabled = true;
            pb_close.Enabled = false;
            lbl_close.Enabled = false;
            pb_save_edit.Enabled = false;
            lbl_save_edit.Enabled = false;
            //pb_export_table.Enabled = false;
            //lbl_export_table.Enabled = false;
            pb_add_Column.Enabled = false;
            lbl_add_Column.Enabled = false;
            lbl_add_Column.Enabled = false;
            pb__delete_Column.Enabled = false;
            lbl_delete_Column.Enabled = false;
            pb_delete_data.Enabled = false;
            lbl_delete_data.Enabled = false;
        }
        /// <summary>
        /// save edit
        /// </summary>
        private void save_edit()
        {
            try
            {
                DataTable dt_table = GlobalMethods.dt_sort_indicators(dgv_zbjz.DataSource as DataTable);
                ht_original_able[treeView_sample.SelectedNode.Text] = dt_table;
                MessageBox.Show("Save successful.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save error:" + ex.Message);
            }
        }
        /// <summary>
        /// Export the current table
        /// </summary>
        private void exportThisTable()
        {
            try
            {
                if (dgv_zbjz.DataSource != null && dgv_zbjz.Rows.Count > 0)
                {
                    using (SaveFileDialog sFileDialog = new SaveFileDialog())
                    {
                        sFileDialog.Title = "export table";
                        sFileDialog.FileName = treeView_sample.SelectedNode.Text + ".xlsx";
                        sFileDialog.Filter = "Excel 文件(*.xlsx)|*.xlsx|Excel 文件(*.xls)|*.xls";
                        sFileDialog.AddExtension = true;
                        if (sFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            DataTable dt_table = iExcel.ToDataTableIM(dgv_zbjz);
                            IWorkbook workbook = new XSSFWorkbook();
                            iExcel.ExportToSheet(workbook, dt_table, "sheet1");
                            using (FileStream fStream = new FileStream(sFileDialog.FileName, FileMode.Create, FileAccess.Write))
                            {
                                workbook.Write(fStream);
                                fStream.Close();
                            }
                            GC.Collect();
                            MessageBox.Show("Export table successful.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("There is no data in the indicator table!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Export table error:" + ex.Message);
            }
        }
        /// <summary>
        /// Add Column
        /// </summary>
        private void add_column()
        {
            Add_Column ac = new Add_Column();
            ac.ShowDialog();
            if (ac.DialogResult == DialogResult.OK)
            {
                string new_columnName = ac.columnName;
                //Does the column already exist
                if (dgv_zbjz.Columns[new_columnName] == null)
                {
                    DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
                    column.HeaderText = new_columnName;
                    column.Name = new_columnName;
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;//Unsorted
                    dgv_zbjz.Columns.Add(column);
                }
                else
                {
                    MessageBox.Show("Column(" + new_columnName + ") already exists,Cannot be added repeatedly!");
                }
            }
        }
        /// <summary>
        /// Delete the selected column
        /// </summary>
        private void delete_column()
        {
            int selectedColumnCount = dgv_zbjz.CurrentCell.ColumnIndex;
            //indicatorType  indicatorName  positive-negative
            if (dgv_zbjz.CurrentCell.ColumnIndex == 0 || dgv_zbjz.CurrentCell.ColumnIndex == 1 || dgv_zbjz.CurrentCell.ColumnIndex == 2 || dgv_zbjz.CurrentCell.ColumnIndex == 3 || dgv_zbjz.CurrentCell.ColumnIndex == 4)
            {
                MessageBox.Show("This column cannot be deleted!", "prompted", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string columnName = dgv_zbjz.Columns[selectedColumnCount].Name;
            DialogResult dr = MessageBox.Show("Are you sure to delete the column(" + columnName + ")?", "prompted", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                dgv_zbjz.Columns.RemoveAt(selectedColumnCount);
            }
        }
        /// <summary>
        /// Delete data
        /// </summary>
        private void delete_data()
        {
            if (dgv_zbjz.SelectedRows.Count == 0)
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
                        foreach (DataGridViewRow row in dgv_zbjz.SelectedRows)
                        {
                            if (!row.IsNewRow)
                            {
                                dgv_zbjz.Rows.Remove(row);
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
        #endregion
        /// <summary>
        /// Initialization of tables in the indicators matrix
        /// </summary>
        /// <param name="filePath"></param>
        private void init_load_indicators_matrix(string filePath)
        {
            DataTable dt_excel = iExcel.ReadExcelToDataTable(filePath);
            //Sort for ease of subsequent calculations
            dt_excel = GlobalMethods.dt_sort_indicators(dt_excel);
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            if (dt_excel == null)
            {
                if (GlobalMethods.dt_error != null && GlobalMethods.dt_error.Rows.Count > 0)
                {
                    errrMsgForm emf = new errrMsgForm();
                    emf.ShowDialog();
                    return;
                }
            }
            else
            {
                treeView_sample.Nodes.Add(fileName, fileName, 2, 2);
                ht_original_able.Add(fileName, dt_excel);
                treeView_sample.SelectedNode = treeView_sample.Nodes[treeView_sample.Nodes.Count - 1];
                treeView_sample.Focus();
            }
        }
        private void init_load_indicators_matrix(DataTable dt_excel, string fileName)
        {
            //Sort for ease of subsequent calculations
            dt_excel = GlobalMethods.dt_sort_indicators(dt_excel);
            if (dt_excel == null)
            {
                if (GlobalMethods.dt_error != null && GlobalMethods.dt_error.Rows.Count > 0)
                {
                    errrMsgForm emf = new errrMsgForm();
                    emf.ShowDialog();
                    return;
                }
            }
            else
            {
                treeView_sample.Nodes.Add(fileName, fileName, 2, 2);
                ht_original_able.Add(fileName, dt_excel);
                treeView_sample.SelectedNode = treeView_sample.Nodes[treeView_sample.Nodes.Count - 1];
                treeView_sample.Focus();
            }
        }
        /// <summary>
        /// Change the color of text and lines in groupBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void groupBox_DrawLine(object sender, PaintEventArgs e)
        {
            GroupBox gb = (GroupBox)sender;
            e.Graphics.Clear(gb.BackColor);
            //Title text color
            e.Graphics.DrawString(gb.Text, gb.Font, Brushes.Black, 10, 1);
            var vSize = e.Graphics.MeasureString(gb.Text, gb.Font);
            //Short horizontal line on the left side of the title text
            e.Graphics.DrawLine(Pens.Black, 1, vSize.Height / 2, 8, vSize.Height / 2);
            //Short horizontal line on the right side of the title text
            e.Graphics.DrawLine(Pens.Black, vSize.Width + 8, vSize.Height / 2, gb.Width - 2, vSize.Height / 2);
            /*//Custom color
            Pen pen = new Pen(Color.FromArgb(255, 0, 0));*/
            //Left vertical line of Group
            e.Graphics.DrawLine(Pens.Black, 1, vSize.Height / 2, 1, gb.Width - 2);
            //Vertical line on the right side of Group
            e.Graphics.DrawLine(Pens.Black, gb.Width - 2, vSize.Height / 2, gb.Width - 2, gb.Height - 2);
            //Group bottom horizontal line
            e.Graphics.DrawLine(Pens.Black, 1, gb.Height - 2, gb.Width - 2, gb.Height - 2);
        }
        /// <summary>
        /// Click on the column header to select the entire column
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_zbjz_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                if (e.ColumnIndex > -1)
                {
                    dgv_zbjz.SelectionMode = DataGridViewSelectionMode.ColumnHeaderSelect;
                    dgv_zbjz.Columns[e.ColumnIndex].Selected = true;//Make this column immediately selected
                }
            }
            else
            {
                //dgv_zbjz.SelectionMode = DataGridViewSelectionMode.CellSelect;//Select a single cell
                //Select the entire column
                dgv_zbjz.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgv_zbjz.Rows[e.RowIndex].Selected = true;
            }
            //dgv_zbjz.BeginEdit(false);
        }
        /// <summary>
        /// Content validation during cell editing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_zbjz_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == 4)
            {
                if (e.FormattedValue.ToString() != "positive" && e.FormattedValue.ToString() != "negative")
                {
                    if (e.FormattedValue.ToString() != "")
                    {
                        dgv_zbjz.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "Only positive or negative can be filled in under the positive-negative field!";
                    }
                }
                else
                {
                    dgv_zbjz.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = String.Empty;
                }

            }
            else if (e.ColumnIndex > 4)
            {
                double result = 0;
                if (!double.TryParse(e.FormattedValue.ToString(), out result))
                {
                    if (e.FormattedValue.ToString() != "")
                    {
                        dgv_zbjz.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "The year/sample related fields can only be filled with integers or decimals!";
                    }
                }
                else
                {
                    dgv_zbjz.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = String.Empty;
                }
            }
        }
        private void treeView_sample_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            e.DrawDefault = true;
        }
        private void treeView_statistical_dimension_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            e.DrawDefault = true;
        }
        /// <summary>
        /// treeview(Sample related node selection change event)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private TreeNode lastSelectedNode = null; //Previous node
        private void treeView_sample_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView_sample.SelectedNode == lastSelectedNode)
            {
                return;
            }
            if (lastSelectedNode == null)
            {
                lastSelectedNode = e.Node;
            }
            else
            {
                if (lbl_close.Enabled == true)
                {
                    treeView_sample.SelectedNode = lastSelectedNode;
                    MessageBox.Show("The current table is in editing status and cannot be switched. Please close editing!");
                    return;
                }
                else
                {
                    lastSelectedNode = e.Node;
                }
            }
            int scroll = this.dgv_zbjz.HorizontalScrollingOffset;
            string nodeName = this.treeView_sample.SelectedNode.Text;
            dgv_zbjz.Columns.Clear();
            dgv_zbjz.AutoGenerateColumns = true; //Set automatic column generation 
            ////sort
            //DataTable dt_excel = GlobalMethods.dt_sort_indicators(ht_original_able[nodeName] as DataTable);
            dgv_zbjz.DataSource = ht_original_able[nodeName] as DataTable;
            dgv_zbjz.Refresh();
            GlobalMethods.Not_Sortable(dgv_zbjz);
            dgv_zbjz.HorizontalScrollingOffset = scroll;
            //Switching between calculation result tables
            if (treeView_statistical_dimension.Nodes.Count > 0)
            {
                string dimension_node_name = this.treeView_statistical_dimension.SelectedNode.Name;
                string sample_node_name = treeView_sample.SelectedNode.Text;
                int tabIndex = tabControl1.SelectedIndex;
                if (tabIndex == 0)
                {
                    if (dgv_gyh.DataSource != null)
                    {
                        switchGYH(dimension_node_name, sample_node_name);
                    }
                }
                else if (tabIndex == 1)
                {
                    if (dataGridViewvaluefij != null)
                    {
                        Hashtable ht_k = GlobalMethods.ht_slh_k_all[dimension_node_name] as Hashtable;
                        string k_value = ht_k[sample_node_name].ToString();
                        switchSLH(dimension_node_name, sample_node_name);
                    }
                }
                else if (tabIndex == 2)
                {
                    if (dataGridView_valueW != null)
                    {
                        switchQZZ(dimension_node_name, sample_node_name);
                    }
                }
                else if (tabIndex == 3)
                {
                    if (dataGridView_valueF2.DataSource != null)
                    {
                        switchFZZS(dimension_node_name, sample_node_name);
                    }
                }
            }
        }

        private void treeView_statistical_dimension_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                string dimension_node_name = this.treeView_statistical_dimension.SelectedNode.Name;
                string sample_node_name = treeView_sample.SelectedNode.Text;
                try
                {
                    Hashtable ht_k = GlobalMethods.ht_slh_k_all[dimension_node_name] as Hashtable;
                    string k_value = ht_k[sample_node_name].ToString();
                }
                catch (Exception)
                {
                    MessageBox.Show("Newly added or removed samples,Please recalculate the results!");
                }
                int tabIndex = tabControl1.SelectedIndex;
                if (tabIndex == 0)
                {
                    switchGYH(dimension_node_name, sample_node_name);
                }
                else if (tabIndex == 1)
                {
                    switchSLH(dimension_node_name, sample_node_name);
                }
                else if (tabIndex == 2)
                {
                    switchQZZ(dimension_node_name, sample_node_name);
                }
                else if (tabIndex == 3)
                {
                    switchFZZS(dimension_node_name, sample_node_name);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void switchGYH(string nodeName, string sample_node_name)
        {
            //Obtain the normalized calculation result table for the selected sample and selected dimension
            Hashtable hs_gyh = GlobalMethods.ht_gyh_all[nodeName] as Hashtable;
            DataTable dt_gyh = hs_gyh[sample_node_name] as DataTable;
            if (dt_gyh != null)
            {
                dgv_gyh.Columns.Clear();
                dgv_gyh.AutoGenerateColumns = true;
                dgv_gyh.DataSource = displayDatatable(sample_node_name, nodeName, dt_gyh);
                GlobalMethods.Not_Sortable(dgv_gyh);
                dgv_gyh.Refresh();
            }
        }
        private void switchSLH(string nodeName, string sample_node_name)
        {
            //Obtain the entropy quantification result table for the selected sample and selected dimension
            //k value
            Hashtable ht_k = GlobalMethods.ht_slh_k_all[nodeName] as Hashtable;
            string k_value = ht_k[sample_node_name].ToString();
            textBoxkValue.Text = k_value;
            //Fij
            Hashtable ht_fij = GlobalMethods.ht_slh_fij_all[nodeName] as Hashtable;
            DataTable dt_fij = ht_fij[sample_node_name] as DataTable;
            dataGridViewvaluefij.Columns.Clear();
            dataGridViewvaluefij.AutoGenerateColumns = true;
            dataGridViewvaluefij.DataSource = displayDatatable(sample_node_name, nodeName, dt_fij);
            GlobalMethods.Not_Sortable(dataGridViewvaluefij);
            dataGridViewvaluefij.Refresh();
            //Hi
            Hashtable ht_hi = GlobalMethods.ht_slh_hi_all[nodeName] as Hashtable;
            DataTable dt_hi = ht_hi[sample_node_name] as DataTable;
            dataGridViewvalueH.Columns.Clear();
            dataGridViewvalueH.AutoGenerateColumns = true;
            dataGridViewvalueH.DataSource = displayDatatable(sample_node_name, nodeName, dt_hi);
            GlobalMethods.Not_Sortable(dataGridViewvalueH);
            dataGridViewvalueH.Refresh();
        }
        private void switchQZZ(string nodeName, string sample_node_name)
        {
            //wi
            Hashtable ht_qzz = GlobalMethods.ht_qzz_all[nodeName] as Hashtable;
            DataTable dt_qzz = ht_qzz[sample_node_name] as DataTable;
            dataGridView_valueW.Columns.Clear();
            dataGridView_valueW.AutoGenerateColumns = true;
            dataGridView_valueW.DataSource = displayDatatable(sample_node_name, nodeName, dt_qzz);
            GlobalMethods.Not_Sortable(dataGridView_valueW);
            dataGridView_valueW.Refresh();
        }
        private void switchFZZS(string nodeName, string sample_node_name)
        {
            //Fjφ
            Hashtable ht_Fjφ = GlobalMethods.ht_zhzs_all[nodeName] as Hashtable;
            DataTable dt_Fjφ = ht_Fjφ[sample_node_name] as DataTable;
            //dataGridView_valueF2.Columns.Clear();
            //dataGridView_valueF2.AutoGenerateColumns = true;
            dataGridView_valueF2.DataSource = dt_Fjφ;
            GlobalMethods.Not_Sortable(dataGridView_valueF2);
            //dataGridView_valueF2.Refresh();
        }
        /// <summary>
        /// Visualize the result table processing to reflect the structure
        /// </summary>
        private DataTable displayDatatable(string sample_node_name, string nodeName, DataTable dt_result)
        {
            DataTable display_gyh = new DataTable();
            DataTable source_structure = ht_original_able[sample_node_name] as DataTable;
            if (nodeName == "indicator")
            {
                for (int i = 0; i < 5; i++)
                {
                    display_gyh.Columns.Add(source_structure.Columns[i].ColumnName);
                }
                for (int i = 3; i < dt_result.Columns.Count; i++)
                {
                    display_gyh.Columns.Add(dt_result.Columns[i].ColumnName);
                }
                for (int j = 0; j < dt_result.Rows.Count; j++)
                {
                    display_gyh.Rows.Add();
                    string name_value = dt_result.Rows[j][1].ToString();
                    DataRow[] drs = source_structure.Select("indicatorName='" + name_value + "'");
                    foreach (DataRow dr in drs)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            display_gyh.Rows[j][i] = dr[i];
                        }
                        for (int k = 3; k < dt_result.Columns.Count; k++)
                        {
                            display_gyh.Rows[j][k + 2] = dt_result.Rows[j][k];
                        }
                    }
                }
                display_gyh.DefaultView.Sort = "theme,dimension,subdimension,indicatorName";
            }
            else if (nodeName == "subdimension")
            {
                DataTable dt_source_structure_filter = source_structure.Copy();
                dt_source_structure_filter.Columns.Remove("indicatorName");
                for (int i = 0; i < 4; i++)
                {
                    display_gyh.Columns.Add(dt_source_structure_filter.Columns[i].ColumnName);
                }
                for (int i = 3; i < dt_result.Columns.Count; i++)
                {
                    display_gyh.Columns.Add(dt_result.Columns[i].ColumnName);
                }
                for (int j = 0; j < dt_result.Rows.Count; j++)
                {
                    display_gyh.Rows.Add();
                    string name_value = dt_result.Rows[j][1].ToString();
                    DataRow[] drs = dt_source_structure_filter.Select("subdimension='" + name_value + "'");
                    foreach (DataRow dr in drs)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            display_gyh.Rows[j][i] = dr[i];
                        }
                        for (int k = 3; k < dt_result.Columns.Count; k++)
                        {
                            display_gyh.Rows[j][k + 1] = dt_result.Rows[j][k];
                        }
                    }
                }
                display_gyh.DefaultView.Sort = "theme,dimension,subdimension";
            }
            else if (nodeName == "dimension")
            {
                DataTable dt_source_structure_filter = source_structure.Copy();
                dt_source_structure_filter.Columns.Remove("indicatorName");
                dt_source_structure_filter.Columns.Remove("subdimension");
                for (int i = 0; i < 3; i++)
                {
                    display_gyh.Columns.Add(dt_source_structure_filter.Columns[i].ColumnName);
                }
                for (int i = 3; i < dt_result.Columns.Count; i++)
                {
                    display_gyh.Columns.Add(dt_result.Columns[i].ColumnName);
                }
                for (int j = 0; j < dt_result.Rows.Count; j++)
                {
                    display_gyh.Rows.Add();
                    string name_value = dt_result.Rows[j][1].ToString();
                    DataRow[] drs = dt_source_structure_filter.Select("dimension='" + name_value + "'");
                    foreach (DataRow dr in drs)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            display_gyh.Rows[j][i] = dr[i];
                        }
                        for (int k = 3; k < dt_result.Columns.Count; k++)
                        {
                            display_gyh.Rows[j][k] = dt_result.Rows[j][k];
                        }
                    }
                }
                display_gyh.DefaultView.Sort = "theme,dimension";
            }
            display_gyh = display_gyh.DefaultView.ToTable();
            return display_gyh;
        }
        private int get_fieldIndex()
        {
            int fieldIndex = 0;
            string dimension_node_name = this.treeView_statistical_dimension.SelectedNode.Name;
            if (dimension_node_name == "indicator")
            {
                fieldIndex = 3;
            }
            else if (dimension_node_name == "subdimension")
            {
                fieldIndex = 2;
            }
            else if (dimension_node_name == "dimension")
            {
                fieldIndex = 1;
            }
            return fieldIndex;
        }
        //Chart selection
        private void chooseChart(DataTable dt_show, string chartName, int fieldIndex, Hashtable ht_result)
        {
            if (treeView_sample.Nodes.Count > 1 && treeView_statistical_dimension.Nodes.Count > 0)
            {
                string dimensionName = treeView_statistical_dimension.SelectedNode.Name;
                ChartLineMultiple clineMulti = new ChartLineMultiple(chartName, ht_result[dimensionName] as Hashtable);
                clineMulti.Show();
            }
            else
            {
                //Determine whether it is a year value. If it is, it is "year-indicator", otherwise it is "sample-indicator", and the chart will be different.
                string fieldName = dt_show.Columns[fieldIndex + 1].ColumnName;
                if (fieldName == "positive-negative")
                {
                    fieldName = dt_show.Columns[fieldIndex + 2].ColumnName;
                }
                if (int.TryParse(fieldName, out _))
                {
                    if (treeView_sample.Nodes.Count == 1)
                    {
                        ChartLine cLine = new ChartLine(chartName, dt_show, fieldIndex);
                        cLine.Show();
                    }
                }
                else
                {
                    ChartBar cLine = new ChartBar(chartName, dt_show, fieldIndex);
                    cLine.Show();
                }
            }
        }
        private void lbl_dn_Fig_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgv_gyh.DataSource == null)
                {
                    MessageBox.Show("Please first calculate the data normalization!");
                    return;
                }
                DataTable dt_Fj = dgv_gyh.DataSource as DataTable;
                chooseChart(dt_Fj, "dn", get_fieldIndex(), GlobalMethods.ht_gyh_all);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void lbl_Fij_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewvaluefij.DataSource == null)
                {
                    MessageBox.Show("Please first calculate the Entropy quantification!");
                    return;
                }
                DataTable dt_Fij = dataGridViewvaluefij.DataSource as DataTable;
                chooseChart(dt_Fij, "Value of Fij", get_fieldIndex(), GlobalMethods.ht_slh_fij_all);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.Message);
            }
        }
        private void lbl_F2_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView_valueF2.DataSource == null)
                {
                    MessageBox.Show("Please first calculate the Development index!");
                    return;
                }
                DataTable dt_F2 = dataGridView_valueF2.DataSource as DataTable;
                chooseChart(dt_F2, "Value of Fj", 0, GlobalMethods.ht_zhzs_all);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tsb_Export_Click(object sender, EventArgs e)
        {
            ExportForm exportF = new ExportForm();
            exportF.ShowDialog();
        }

        private void treeView_sample_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)//Determine if you clicked the right button
            {
                Point ClickPoint = new Point(e.X, e.Y);
                TreeNode CurrentNode = treeView_sample.GetNodeAt(ClickPoint);
            }
        }
        /// <summary>
        /// Remove a sample
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmi_sample_remove_Click(object sender, EventArgs e)
        {
            try
            {
                string name = treeView_sample.SelectedNode.Text.ToString();//Text of storage node
                DialogResult dialogResult;
                if (lbl_close.Enabled == true)
                {
                    dialogResult = MessageBox.Show("The current table is in editing state. Are you sure you want to remove sample ‘" + name + "’?", "prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                }
                else
                {
                    dialogResult = MessageBox.Show("Are you sure you want to remove sample ‘" + name + "’?", "prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                }
                if (dialogResult == DialogResult.OK)
                {
                    if (ht_original_able.ContainsKey(name))
                    {
                        ht_original_able.Remove(name);
                    }
                    treeView_sample.Nodes.Remove(treeView_sample.SelectedNode);
                    if (treeView_sample.Nodes.Count <= 0)
                    {
                        dgv_zbjz.Columns.Clear();
                        dgv_zbjz.DataSource = null;
                        dgv_zbjz.Refresh();
                        init_edit_status();
                        lastSelectedNode = null;
                    }
                    clearDGV();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// Clear relevant display data on the interface
        /// </summary>
        private void clearDGV()
        {
            treeView_statistical_dimension.Nodes.Clear();

            dgv_gyh.Columns.Clear();
            dgv_gyh.DataSource = null;
            dgv_gyh.Refresh();

            textBoxkValue.Text = "0.123";
            dataGridViewvaluefij.Columns.Clear();
            dataGridViewvaluefij.DataSource = null;
            dataGridViewvaluefij.Refresh();
            dataGridViewvalueH.Columns.Clear();
            dataGridViewvalueH.DataSource = null;
            dataGridViewvalueH.Refresh();

            dataGridView_valueW.Columns.Clear();
            dataGridView_valueW.DataSource = null;
            dataGridView_valueW.Refresh();

            dataGridView_valueF2.Columns.Clear();
            dataGridView_valueF2.DataSource = null;
            dataGridView_valueF2.Refresh();

            //Initialize various result tables
            GlobalMethods.ht_gyh_all = new Hashtable();
            GlobalMethods.ht_slh_k_all = new Hashtable();
            GlobalMethods.ht_slh_fij_all = new Hashtable();
            GlobalMethods.ht_slh_hi_all = new Hashtable();
            GlobalMethods.ht_qzz_all = new Hashtable();
            GlobalMethods.ht_zhzs_all = new Hashtable();
        }
        /// <summary>
        /// Initialize listview image data
        /// </summary>
        /// <returns></returns>
        public static ImageList initFileNameImageList_EWM()
        {
            ImageList imageList = new ImageList();
            imageList.Images.Add(Properties.Resources.weight);//0
            imageList.Images.Add(Properties.Resources.tableimg);//1
            imageList.Images.Add(Properties.Resources.d_temp);//2
            imageList.Images.Add(Properties.Resources.multi_dimensional);//3
            return imageList;
        }
    }
}
