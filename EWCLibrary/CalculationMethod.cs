using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace EWCLibrary
{
   public  class CalculationMethod: ICalculation
    {
        #region Multi dimensional unified recursive computation
        public DataTable dt_Data_Target_ColumnName(DataTable calculateSource, int startYearFieldNum)
        {
            DataTable calulateTarget = new DataTable();
            calulateTarget.Columns.Add("UpperDimension");
            calulateTarget.Columns.Add("Name");
            calulateTarget.Columns.Add("positive-negative");
            for (int i = startYearFieldNum; i < calculateSource.Columns.Count; i++)
            {
                calulateTarget.Columns.Add(calculateSource.Columns[i].ColumnName);
            }
            return calulateTarget;
        }
        public  DataTable dt_Hi_ColumnName()
        {
            DataTable calulateTarget = new DataTable();
            calulateTarget.Columns.Add("UpperDimension");
            calulateTarget.Columns.Add("Name");
            calulateTarget.Columns.Add("positive-negative");
            calulateTarget.Columns.Add("H");
            return calulateTarget;
        }
        public  DataTable dt_Wi_ColumnName()
        {
            DataTable calulateTarget = new DataTable();
            calulateTarget.Columns.Add("UpperDimension");
            calulateTarget.Columns.Add("Name");
            calulateTarget.Columns.Add("positive-negative");
            calulateTarget.Columns.Add("Weight");
            return calulateTarget;
        }
        public  DataTable dt_Fjφ_ColumnName(DataTable calculateSource, int startYearFieldNum)
        {
            DataTable calulateTarget = new DataTable();
            calulateTarget.Columns.Add("Name");
            for (int i = startYearFieldNum; i < calculateSource.Columns.Count; i++)
            {
                calulateTarget.Columns.Add(calculateSource.Columns[i].ColumnName);
            }
            return calulateTarget;
        }


        /// <summary>
        /// Unified recursive calculation of Data normalization, entropy quantization, Weight value, and Development index for multiple samples
        /// </summary>
        /// <param name="ht_original_able"></param>
        /// <param name="count"></param>
        public void calculation_process(Hashtable ht_original_able, Hashtable ht_gyh, int count)
        {
            /* #region [calculation data normalization] Extract it out, only calculate the indicator level
             Hashtable ht_gyh = calculation_gyh(ht_original_able);
             ht_gyh_all.Add(process_name(count), ht_gyh);
             #endregion*/

            #region [calculation entropy quantization]
            SLH_RESULT slhResult = calculation_slh(ht_original_able, ht_gyh, count);
            GlobalMethods.ht_slh_k_all.Add(process_name(count), slhResult.ht_slh_k);
            GlobalMethods.ht_slh_fij_all.Add(process_name(count), slhResult.ht_slh_fij);
            GlobalMethods.ht_slh_hi_all.Add(process_name(count), slhResult.ht_slh_hi);
            #endregion

            #region [calculation Weight value]
            Hashtable ht_weight = calculation_qzz(ht_original_able, slhResult.ht_slh_hi);
            GlobalMethods.ht_qzz_all.Add(process_name(count), ht_weight);
            #endregion

            #region [calculation Development index]
            Hashtable ht_zhzs = calculation_zhzs(ht_gyh, ht_weight);
            GlobalMethods.ht_zhzs_all.Add(process_name(count), ht_zhzs);
            //After the calculation development index，Unified data format for result tables, facilitating recursive calculations again
            Hashtable new_ht_original_able = new Hashtable();
            bool isTheme = false;
            ICollection keys = GlobalMethods.ht_original_able.Keys;
            foreach (string sampleName in keys)
            {
                //Obtain the source table of a single sample
                DataTable dt_original_table_sample = GlobalMethods.ht_original_able[sampleName] as DataTable;
                //Obtain the result table for a single sample
                DataTable dt_zhzs_result_sample = ht_zhzs[sampleName] as DataTable;
                DataTable new_calculateSource = new DataTable();
                new_calculateSource.Columns.Add("UpperDimension");
                new_calculateSource.Columns.Add("Name");
                new_calculateSource.Columns.Add("positive-negative");
                for (int i = 1; i < dt_zhzs_result_sample.Columns.Count; i++)
                {
                    new_calculateSource.Columns.Add(dt_zhzs_result_sample.Columns[i].ColumnName);
                }
                //Multiple upward statistical data (full dimension, zero dimension, zero sub dimension, zero dimension+zero sub dimension)
                //Determine if there is empty data in the dimension fields
                bool hasNull_dimension = dt_original_table_sample.AsEnumerable().Any(row => row["dimension"].ToString() == "");
                //Is there empty data in the subdimension fields
                bool hasNull_subdimension = dt_original_table_sample.AsEnumerable().Any(row => row["subdimension"].ToString() == "");
                if (hasNull_dimension && hasNull_subdimension)//zero dimension+zero sub dimension:There are empty data in both the dimension and subdimension fields
                {
                    isTheme = true;
                }
                else if (hasNull_dimension && hasNull_subdimension == false)//zero dimension：There is empty data in the dimension field
                {
                    for (int i = 0; i < dt_zhzs_result_sample.Rows.Count; i++)
                    {
                        new_calculateSource.Rows.Add();
                        string categoryIndex = dt_zhzs_result_sample.Rows[i][0].ToString();
                        for (int j = 0; j < 4; j++)
                        {
                            string ColumnsName = dt_original_table_sample.Columns[j].ColumnName;
                            //Query all values of a column
                            DataRow[] dr_filter = dt_original_table_sample.Select(ColumnsName + "='" + categoryIndex + "'");
                            if (dr_filter.Length > 0)
                            {
                                string UpperDimension = "";
                                string Name = "";
                                if (ColumnsName == "theme")
                                {
                                    isTheme = true;
                                    break;
                                }
                                else if (ColumnsName == "subdimension")
                                {
                                    isTheme = false;
                                    count = 1;
                                    DataRow dr_single = dr_filter[0];
                                    Name = dr_single[2].ToString();
                                    UpperDimension = dr_single[0].ToString();
                                }
                                new_calculateSource.Rows[i][0] = UpperDimension;
                                new_calculateSource.Rows[i][1] = Name;
                                new_calculateSource.Rows[i][2] = "positive";
                                for (int k = 1; k < dt_zhzs_result_sample.Columns.Count; k++)
                                {
                                    new_calculateSource.Rows[i][k + 2] = dt_zhzs_result_sample.Rows[i][k];
                                }
                                break;
                            }
                        }
                    }
                }
                else if (hasNull_dimension == false && hasNull_subdimension)//zero sub dimension：There is empty data in the subdimension field
                {
                    for (int i = 0; i < dt_zhzs_result_sample.Rows.Count; i++)
                    {
                        new_calculateSource.Rows.Add();
                        string categoryIndex = dt_zhzs_result_sample.Rows[i][0].ToString();
                        for (int j = 0; j < 4; j++)
                        {
                            string ColumnsName = dt_original_table_sample.Columns[j].ColumnName;
                            //Query all values of a column
                            DataRow[] dr_filter = dt_original_table_sample.Select(ColumnsName + "='" + categoryIndex + "'");
                            if (dr_filter.Length > 0)
                            {
                                string UpperDimension = "";
                                string Name = "";
                                if (ColumnsName == "theme")
                                {
                                    isTheme = true;
                                    break;
                                }
                                else if (ColumnsName == "dimension")
                                {
                                    isTheme = false;
                                    count = 2;
                                    DataRow dr_single = dr_filter[0];
                                    Name = dr_single[1].ToString();
                                    UpperDimension = dr_single[0].ToString();
                                }
                                new_calculateSource.Rows[i][0] = UpperDimension;
                                new_calculateSource.Rows[i][1] = Name;
                                new_calculateSource.Rows[i][2] = "positive";
                                for (int k = 1; k < dt_zhzs_result_sample.Columns.Count; k++)
                                {
                                    new_calculateSource.Rows[i][k + 2] = dt_zhzs_result_sample.Rows[i][k];
                                }
                                break;
                            }
                        }
                    }
                }
                else //full dimension:data is available in the dimension and subdimension fields
                {
                    for (int i = 0; i < dt_zhzs_result_sample.Rows.Count; i++)
                    {
                        new_calculateSource.Rows.Add();
                        string categoryIndex = dt_zhzs_result_sample.Rows[i][0].ToString();
                        for (int j = 0; j < 4; j++)
                        {
                            string ColumnsName = dt_original_table_sample.Columns[j].ColumnName;
                            //Query all values for a column
                            DataRow[] dr_filter = dt_original_table_sample.Select(ColumnsName + "='" + categoryIndex + "'");
                            if (dr_filter.Length > 0)
                            {
                                string UpperDimension = "";
                                string Name = "";
                                if (ColumnsName == "theme")
                                {
                                    isTheme = true;
                                    break;
                                }
                                else if (ColumnsName == "dimension")
                                {
                                    isTheme = false;
                                    count = 2;
                                    DataRow dr_single = dr_filter[0];
                                    Name = dr_single[1].ToString();
                                    UpperDimension = dr_single[0].ToString();
                                }
                                else if (ColumnsName == "subdimension")
                                {
                                    isTheme = false;
                                    count = 1;
                                    DataRow dr_single = dr_filter[0];
                                    Name = dr_single[2].ToString();
                                    UpperDimension = dr_single[1].ToString();
                                }
                                new_calculateSource.Rows[i][0] = UpperDimension;
                                new_calculateSource.Rows[i][1] = Name;
                                new_calculateSource.Rows[i][2] = "positive";
                                for (int k = 1; k < dt_zhzs_result_sample.Columns.Count; k++)
                                {
                                    new_calculateSource.Rows[i][k + 2] = dt_zhzs_result_sample.Rows[i][k];
                                }
                                break;
                            }
                        }
                    }
                }
                new_ht_original_able.Add(sampleName, new_calculateSource);
            }
            if (isTheme)
            {
                //ht_zhzs_fj_all = calculation_zhzs_zzs(ht_gyh, ht_weight);
                return;
            }
            else
            {
                //count++;
                calculation_process(new_ht_original_able, new_ht_original_able, count);
            }
            #endregion
        }
        ///Format the source datatable
        private DataTable dt_init_original(DataTable calculateSource)
        {
            if (calculateSource.Columns.Contains("theme"))//Format each table
            {
                //Determines if empty data exists in the dimension column
                bool hasNull_dimension = calculateSource.AsEnumerable().Any(row => row["dimension"].ToString() == "");
                //Whether empty data exists in the subdimension column
                bool hasNull_subdimension = calculateSource.AsEnumerable().Any(row => row["subdimension"].ToString() == "");
                if (hasNull_subdimension && hasNull_dimension)
                {
                    calculateSource.Columns.RemoveAt(1);//Delete column dimension
                    calculateSource.Columns.RemoveAt(1);//Delete column subdimension
                }
                else if (hasNull_subdimension)
                {
                    calculateSource.Columns.RemoveAt(0);//Delete column theme
                    calculateSource.Columns.RemoveAt(1);//Delete column subdimension
                }
                else
                {
                    calculateSource.Columns.RemoveAt(0);//Delete column theme
                    calculateSource.Columns.RemoveAt(0);//Delete column dimension
                }
            }
            return calculateSource;
        }
        /// calculation data normalization
        public Hashtable calculation_gyh(Hashtable ht_original_able)
        {
            try
            {
                Hashtable ht_gyh_all = new Hashtable();
                // Gets a collection of keys
                ICollection keys = ht_original_able.Keys;
                DataTable OriginalTable = new DataTable();
                foreach (string sampleName in keys)
                {
                    OriginalTable = ht_original_able[sampleName] as DataTable;
                    DataTable calculateSource = dt_init_original(OriginalTable.Copy());
                    DataTable dt_Data_normalization = dt_Data_Target_ColumnName(calculateSource, 3);
                    if (GlobalMethods.sij_rij == "Rij")//Normalization method of Rij
                    {
                        //The i-th indicator, the maximum value of all samples in a certain year
                        double maxxij;
                        //The i-th indicator, the minimum value of all samples in a certain year
                        double minxij;
                        for (int i = 0; i < calculateSource.Rows.Count; i++)
                        {
                            List<double> indicatorList = new List<double>();
                            foreach (string singleName in keys)
                            {
                                DataTable sourceTable = ht_original_able[singleName] as DataTable;
                                int startNum = 3;
                                if (OriginalTable.Columns.Contains("theme"))
                                {
                                    startNum = 5;
                                }
                                for (int j = startNum; j < OriginalTable.Columns.Count; j++)
                                {
                                    indicatorList.Add(Convert.ToDouble(OriginalTable.Rows[i][j]));
                                }
                            }
                            maxxij = indicatorList.Max();
                            minxij = indicatorList.Min();

                            dt_Data_normalization.Rows.Add();
                            for (int j = 0; j < calculateSource.Columns.Count; j++)
                            {
                                if (j <= 2)
                                {
                                    dt_Data_normalization.Rows[i][j] = calculateSource.Rows[i][j];
                                }
                                else
                                {
                                    //normalized value
                                    if (calculateSource.Rows[i]["positive-negative"].ToString() == "positive")
                                    {
                                        double rij = handle_NaN(Convert.ToDouble(calculateSource.Rows[i][j]) / maxxij);
                                        dt_Data_normalization.Rows[i][j] = rij;
                                    }
                                    else if (calculateSource.Rows[i]["positive-negative"].ToString() == "negative")
                                    {
                                        double rij = handle_NaN(minxij / Convert.ToDouble(calculateSource.Rows[i][j]));
                                        dt_Data_normalization.Rows[i][j] = rij;
                                    }
                                    else
                                    {
                                        MessageBox.Show("Data error, missing positive-negative information, program terminal, please check your input metrics");
                                        return null;
                                    }
                                }
                            }
                        }
                        ht_gyh_all.Add(sampleName, dt_Data_normalization);
                    }
                    else if (GlobalMethods.sij_rij == "Sij")//Normalization method of Sij
                    {
                        //The i-th indicator, the maximum value of all samples in a certain year
                        double maxxij;
                        //The i-th indicator,the maximum value of all samples in a certain year
                        double minxij;
                        for (int i = 0; i < calculateSource.Rows.Count; i++)
                        {
                            List<double> indicatorList = new List<double>();
                            foreach (string singleName in keys)
                            {
                                DataTable sourceTable = ht_original_able[singleName] as DataTable;
                                int startNum = 3;
                                if (OriginalTable.Columns.Contains("theme"))
                                {
                                    startNum = 5;
                                }
                                /*//The maximum and minimum values within a single sample
                                 for (int j = startNum; j < OriginalTable.Columns.Count; j++)
                                {
                                    indicatorList.Add(Convert.ToDouble(OriginalTable.Rows[i][j]));
                                }*/
                                //The maximum and minimum values compared among all samples
                                foreach (string contrast in keys)
                                {
                                    DataTable ptable = ht_original_able[contrast] as DataTable;
                                    for (int c = startNum; c < ptable.Columns.Count; c++)
                                    {
                                        indicatorList.Add(Convert.ToDouble(ptable.Rows[i][c]));
                                    }
                                }
                            }
                            maxxij = indicatorList.Max();
                            minxij = indicatorList.Min();

                            dt_Data_normalization.Rows.Add();
                            for (int j = 0; j < calculateSource.Columns.Count; j++)
                            {
                                if (j <= 2)  //Directly assigning values to non year fields
                                {
                                    dt_Data_normalization.Rows[i][j] = calculateSource.Rows[i][j];
                                }
                                else  //Normalization value calculation for year field
                                {
                                    //normalized value
                                    if (calculateSource.Rows[i]["positive-negative"].ToString() == "positive")
                                    {
                                        double Sij = handle_NaN((Convert.ToDouble(calculateSource.Rows[i][j]) - minxij) / (maxxij - minxij));
                                        if (Sij == 0)
                                        {
                                            dt_Data_normalization.Rows[i][j] = 0.0001;
                                        }
                                        else
                                        {
                                            dt_Data_normalization.Rows[i][j] = Sij;
                                        }
                                    }
                                    else if (calculateSource.Rows[i]["positive-negative"].ToString() == "negative")
                                    {
                                        double Sij = handle_NaN((maxxij - Convert.ToDouble(calculateSource.Rows[i][j])) / (maxxij - minxij));
                                        if (Sij == 0)
                                        {
                                            dt_Data_normalization.Rows[i][j] = 0.0001;
                                        }
                                        else
                                        {
                                            dt_Data_normalization.Rows[i][j] = Sij;
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Data error, missing positive-negative information, program terminal, please check your input metrics");
                                        return null;
                                    }
                                }
                            }
                        }
                        ht_gyh_all.Add(sampleName, dt_Data_normalization);
                    }
                }

                if (GlobalMethods.sij_rij == "Rij")
                {
                    if (GlobalMethods.is_avg)
                    {
                        GlobalMethods.dt_gyh_avg = calculate_gyh_Rij_avg(dt_init_original(GlobalMethods.dt_avg_result));
                    }
                }
                else if (GlobalMethods.sij_rij == "Sij")
                {
                    if (GlobalMethods.is_avg)
                    {
                        GlobalMethods.dt_gyh_avg = calculate_gyh_Sij_avg(dt_init_original(GlobalMethods.dt_avg_result));
                    }
                }
                return ht_gyh_all;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return null;
            }
        }
        /// calculation entropy quantization
        public class SLH_RESULT
        {
            public Hashtable ht_slh_k { get; set; }
            public Hashtable ht_slh_fij { get; set; }
            public Hashtable ht_slh_hi { get; set; }
        }
        public  SLH_RESULT calculation_slh(Hashtable ht_original_able, Hashtable ht_gyh, int count)
        {
            SLH_RESULT slhResult = new SLH_RESULT();
            slhResult.ht_slh_k = new Hashtable();
            slhResult.ht_slh_fij = new Hashtable();
            slhResult.ht_slh_hi = new Hashtable();
            // Get a collection of keys
            ICollection keys = ht_original_able.Keys;
            foreach (string sampleName in keys)
            {
                DataTable OriginalTable = ht_original_able[sampleName] as DataTable;
                if (GlobalMethods.is_avg == true && count == 0)
                {
                    OriginalTable = GlobalMethods.dt_avg_result;
                }
                DataTable calculateSource = dt_init_original(OriginalTable.Copy());
                DataTable dt_Fij = dt_Data_Target_ColumnName(calculateSource, 3);
                DataTable dt_Hi = dt_Hi_ColumnName();
                double value_k = handle_NaN(1 / Math.Log(calculateSource.Columns.Count - 3, Math.E));
                slhResult.ht_slh_k.Add(sampleName, value_k);
                DataTable sample_single_gyh = new DataTable();
                sample_single_gyh = ht_gyh[sampleName] as DataTable;
                if (GlobalMethods.is_avg == true && count == 0)
                {
                    sample_single_gyh = GlobalMethods.dt_gyh_avg;
                }
                for (int i = 0; i < sample_single_gyh.Rows.Count; i++)
                {
                    double sumRij = 0;
                    dt_Fij.Rows.Add();
                    dt_Hi.Rows.Add();
                    for (int j = 3; j < sample_single_gyh.Columns.Count; j++)
                    {
                        sumRij += Convert.ToDouble(sample_single_gyh.Rows[i][j]);
                    }
                    double sumfij = 0;
                    for (int j = 0; j < sample_single_gyh.Columns.Count; j++)
                    {
                        if (j <= 2)
                        {
                            dt_Fij.Rows[i][j] = calculateSource.Rows[i][j];
                            dt_Hi.Rows[i][j] = calculateSource.Rows[i][j];
                        }
                        else
                        {
                            double fij = handle_NaN(Convert.ToDouble(sample_single_gyh.Rows[i][j]) / sumRij);
                            sumfij += handle_NaN(fij * Math.Log(1 / fij, Math.E));
                            dt_Fij.Rows[i][j] = fij;

                        }
                    }
                    dt_Hi.Rows[i][3] = handle_NaN(value_k * sumfij);
                }
                slhResult.ht_slh_fij.Add(sampleName, dt_Fij);
                slhResult.ht_slh_hi.Add(sampleName, dt_Hi);
            }
            return slhResult;
        }
        /// calculation Weight value
        public Hashtable calculation_qzz(Hashtable ht_original_able, Hashtable ht_slh_hi)
        {
            Hashtable ht_wehght = new Hashtable();
            // Get a collection of keys
            ICollection keys = ht_original_able.Keys;
            foreach (string sampleName in keys)
            {
                DataTable OriginalTable = ht_original_able[sampleName] as DataTable;
                DataTable calculateSource = dt_init_original(OriginalTable.Copy());
                DataTable dt_Weight = dt_Wi_ColumnName();
                double weith = 0;
                List<string> trycontain = new List<string>();
                DataTable dt_single_hi = ht_slh_hi[sampleName] as DataTable;
                for (int i = 0; i < dt_single_hi.Rows.Count; i++)
                {
                    if (!trycontain.Contains(dt_single_hi.Rows[i][0].ToString()))
                    {
                        trycontain.Add(dt_single_hi.Rows[i][0].ToString());
                    }
                }
                for (int t = 0; t < trycontain.Count; t++)
                {
                    double sumHi = 0;
                    for (int i = 0; i < dt_single_hi.Rows.Count; i++)
                    {
                        if (trycontain[t].Equals(dt_single_hi.Rows[i][0].ToString()))
                        {
                            sumHi += (1 - Convert.ToDouble(dt_single_hi.Rows[i][3]));
                        }

                    }
                    for (int i = 0; i < dt_single_hi.Rows.Count; i++)
                    {
                        if (trycontain[t].Equals(dt_single_hi.Rows[i][0].ToString()))
                        {
                            if (Convert.ToDouble(dt_single_hi.Rows[i][3]) == 1)
                            {
                                weith = 0;
                            }
                            else
                            {
                                weith = handle_NaN((1 - Convert.ToDouble(dt_single_hi.Rows[i][3])) / sumHi);
                            }
                            dt_Weight.Rows.Add();
                            dt_Weight.Rows[i][0] = calculateSource.Rows[i][0];
                            dt_Weight.Rows[i][1] = calculateSource.Rows[i][1];
                            dt_Weight.Rows[i][2] = calculateSource.Rows[i][2];
                            dt_Weight.Rows[i][3] = weith;
                        }

                    }
                }
                ht_wehght.Add(sampleName, dt_Weight);
            }
            return ht_wehght;
        }
        /// calculation Development index
        public Hashtable calculation_zhzs(Hashtable ht_gyh, Hashtable ht_weight)
        {
            #region calculation Development index
            Hashtable ht_flzs = new Hashtable();
            // Gets a collection of keys
            ICollection keys = ht_gyh.Keys;
            foreach (string sampleName in keys)
            {
                DataTable dt_gyh = ht_gyh[sampleName] as DataTable;
                DataTable dt_wi = ht_weight[sampleName] as DataTable;
                DataTable dt_Fjφ = dt_Fjφ_ColumnName(dt_gyh, 3);

                List<string> trycontain = new List<string>();
                for (int i = 0; i < dt_gyh.Rows.Count; i++)
                {
                    if (!trycontain.Contains(dt_gyh.Rows[i][0].ToString()))
                    {
                        trycontain.Add(dt_gyh.Rows[i][0].ToString());
                    }
                }
                for (int c = 0; c < trycontain.Count; c++)
                {
                    dt_Fjφ.Rows.Add();
                    dt_Fjφ.Rows[c][0] = trycontain[c];
                    for (int j = 3; j < dt_gyh.Columns.Count; j++)
                    {
                        double categoryIndex = 0;
                        for (int i = 0; i < dt_gyh.Rows.Count; i++)
                        {
                            if (trycontain[c].Equals(dt_gyh.Rows[i][0].ToString()))
                            {
                                categoryIndex += Convert.ToDouble(dt_wi.Rows[i][3]) * Convert.ToDouble(dt_gyh.Rows[i][j]);

                            }

                        }
                        dt_Fjφ.Rows[c][j - 2] = categoryIndex;
                    }
                }
                ht_flzs.Add(sampleName, dt_Fjφ);
            }
            return ht_flzs;
            #endregion
        }
        /// Calculate the overall index
        public Hashtable calculation_zhzs_zzs(Hashtable ht_gyh, Hashtable ht_weight)
        {
            #region Calculate the overall index
            Hashtable ht_zzs_all = new Hashtable();
            ICollection keys = ht_gyh.Keys;
            foreach (string sampleName in keys)
            {
                DataTable dt_gyh = ht_gyh[sampleName] as DataTable;
                DataTable dt_wi = ht_weight[sampleName] as DataTable;
                //overall index Value Table
                DataTable dt_Fj = new DataTable();
                dt_Fj.Columns.Add("index");
                for (int j = 3; j < dt_gyh.Columns.Count; j++)
                {
                    dt_Fj.Columns.Add(dt_gyh.Columns[j].ColumnName);
                }
                dt_Fj.Rows.Add();

                for (int j = 3; j < dt_gyh.Columns.Count; j++)
                {
                    double indexj = 0;
                    for (int i = 0; i < dt_gyh.Rows.Count; i++)
                    {
                        double w = Convert.ToDouble(dt_wi.Rows[i][3]);
                        double n = Convert.ToDouble(dt_gyh.Rows[i][j]);
                        indexj += Convert.ToDouble(dt_wi.Rows[i][3]) * Convert.ToDouble(dt_gyh.Rows[i][j]);
                    }
                    dt_Fj.Rows[0][j - 2] = indexj;
                }
                dt_Fj.Rows[0][0] = " Name";
                ht_zzs_all.Add(sampleName, dt_Fj);
            }
            return ht_zzs_all;
            #endregion
        }
        /// <summary>
        /// Calculate the average value
        /// </summary>
        public DataTable calculation_avg(Hashtable ht_original_able)
        {
            Hashtable ht_avg_table = new Hashtable();
            //Merge multiple indicator data tables into one table
            int tb_num = 0;
            DataTable dt_all = new DataTable();
            ICollection keys = ht_original_able.Keys;
            foreach (string sampleName in keys)
            {
                DataTable dts = ht_original_able[sampleName] as DataTable;
                tb_num += 1;
                if (tb_num == 1)
                {
                    dt_all = dts.Copy();
                }
                else
                {
                    foreach (DataRow drs in dts.Rows)
                    {
                        DataRow dr = dt_all.NewRow();
                        dr.ItemArray = drs.ItemArray;
                        dt_all.Rows.Add(dr);
                    }
                }
            }
            //Calculate the average value based on indicator grouping
            foreach (DataColumn column in dt_all.Columns)
            {
                //Define a prefix for the header, otherwise subsequent grouping statistics cannot be performed (datatable Compute), and the numeric header cannot be recognized.
                column.ColumnName = "_" + column.ColumnName;
            }
            DataTable dt_avg_result = dt_all.Clone();
            for (int i = 0; i < dt_avg_result.Columns.Count; i++)
            {
                if (i > 4)
                {
                    //Changing the field type to double, string type cannot calculate the average value.
                    dt_avg_result.Columns[i].DataType = typeof(double);
                }
            }
            DataTable dt_Time = dt_all.DefaultView.ToTable(true, dt_all.Columns[3].ColumnName);
            for (int i = 0; i < dt_Time.Rows.Count; i++)
            {
                string filter = dt_all.Columns[3].ColumnName + "='" + dt_Time.Rows[i][0] + "'";
                DataRow[] rows = dt_all.Select(filter);
                //Temp is used to store filtered data
                DataTable temp = dt_avg_result.Clone();
                foreach (DataRow row in rows)
                {
                    temp.Rows.Add(row.ItemArray);
                }
                DataRow dr = dt_avg_result.NewRow();
                DataRow[] drArr = dt_all.Select("_indicatorName='" + dt_Time.Rows[i][0].ToString() + "'");
                dr[0] = drArr[0][0].ToString();
                dr[1] = drArr[0][1].ToString();
                dr[2] = drArr[0][2].ToString();
                dr[3] = dt_Time.Rows[i][0].ToString();
                dr[4] = drArr[0][4].ToString();
                for (int k = 0; k < dt_all.Columns.Count; k++)
                {
                    if (k > 4)
                    {
                        string ComputeStr = "AVG(" + dt_all.Columns[k].ColumnName + ")";
                        dr[k] = temp.Compute(ComputeStr, "");
                    }
                }
                dt_avg_result.Rows.Add(dr);
            }
            foreach (DataColumn column in dt_avg_result.Columns)
            {
                //Remove the _ symbol added to the column name for calculation
                column.ColumnName = column.ColumnName.Remove(0, 1);
            }
            GlobalMethods.dt_zb_avg = GlobalMethods.dt_sort_indicators(dt_avg_result);
            return GlobalMethods.dt_zb_avg;
        }
        /// <summary>
        /// Table of Average Results Rij Normalization Method
        /// </summary>
        private DataTable calculate_gyh_Rij_avg(DataTable calculateSource)
        {
            //Extract indicator values
            DataTable normalizationTable = new DataTable();
            for (int i = 0; i < calculateSource.Columns.Count; i++)
            {
                normalizationTable.Columns.Add(calculateSource.Columns[i].ColumnName);
            }
            //The i-th indicator, the maximum value of all samples in a certain year
            double maxxij;
            //The i-th indicator, the minimum value of all samples in a certain year
            double minxij;
            for (int i = 0; i < calculateSource.Rows.Count; i++)
            {
                List<double> indicatorList = new List<double>();
                for (int j = 5; j < calculateSource.Columns.Count; j++)
                {
                    indicatorList.Add(Convert.ToDouble(calculateSource.Rows[i][j]));
                }
                maxxij = indicatorList.Max();
                minxij = indicatorList.Min();

                normalizationTable.Rows.Add();
                for (int j = 0; j < calculateSource.Columns.Count; j++)
                {
                    if (j <= 2)
                    {
                        normalizationTable.Rows[i][j] = calculateSource.Rows[i][j];
                    }
                    else
                    {
                        if (calculateSource.Rows[i]["positive-negative"].ToString() == "positive")
                        {
                            //normalized value
                            double rij = Convert.ToDouble(calculateSource.Rows[i][j]) / maxxij;
                            normalizationTable.Rows[i][j] = rij;
                        }
                        else if (calculateSource.Rows[i]["positive-negative"].ToString() == "negative")
                        {
                            double rij = minxij / Convert.ToDouble(calculateSource.Rows[i][j]);
                            normalizationTable.Rows[i][j] = rij;
                        }
                        else
                        {
                            MessageBox.Show("Data error, missing positive-negative information, program terminal, please check your input metrics");
                            return null;
                        }
                    }

                }

            }
            return normalizationTable;
        }
        /// <summary>
        /// Table of Average Results Sij Normalization Method
        /// </summary>
        private DataTable calculate_gyh_Sij_avg(DataTable calculateSource)
        {
            DataTable normalizationTable = new DataTable();
            for (int i = 0; i < calculateSource.Columns.Count; i++)
            {
                normalizationTable.Columns.Add(calculateSource.Columns[i].ColumnName);
            }
            //The i-th indicator, the maximum value of all samples in a certain year
            double maxxij = 0;
            //The i-th indicator, the minimum value of all samples in a certain year
            double minxij = 0;
            for (int i = 0; i < calculateSource.Rows.Count; i++)
            {
                List<double> indicatorList = new List<double>();
                for (int j = 3; j < calculateSource.Columns.Count; j++)
                {
                    indicatorList.Add(Convert.ToDouble(calculateSource.Rows[i][j]));
                }
                maxxij = indicatorList.Max();
                minxij = indicatorList.Min();

                normalizationTable.Rows.Add();
                for (int j = 0; j < calculateSource.Columns.Count; j++)
                {
                    if (j <= 2)
                    {
                        normalizationTable.Rows[i][j] = calculateSource.Rows[i][j];
                    }
                    else
                    {
                        if (calculateSource.Rows[i]["positive-negative"].ToString() == "positive")
                        {
                            double cs = Convert.ToDouble(calculateSource.Rows[i][j]);
                            //normalized value
                            double Sij = (Convert.ToDouble(calculateSource.Rows[i][j]) - minxij) / (maxxij - minxij);
                            if (Sij == 0)
                            {
                                normalizationTable.Rows[i][j] = 0.0001;
                            }
                            else
                            {
                                normalizationTable.Rows[i][j] = Sij;
                            }

                        }
                        else if (calculateSource.Rows[i]["positive-negative"].ToString() == "negative")
                        {
                            double cs = Convert.ToDouble(calculateSource.Rows[i][j]);
                            double Sij = (maxxij - Convert.ToDouble(calculateSource.Rows[i][j])) / (maxxij - minxij);
                            if (Sij == 0)
                            {
                                normalizationTable.Rows[i][j] = 0.0001;
                            }
                            else
                            {
                                normalizationTable.Rows[i][j] = Sij;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Data error, missing positive-negative information, program terminal, please check your input metrics");
                            return null;
                        }
                    }
                }
            }
            return normalizationTable;
        }
        ///Judging the progress of the process
        public string process_name(int count)
        {
            string name = ""; //Single step process name
            name = GlobalMethods.strs_process[count];
            return name;
        }
        //Dealing with data issues
        private double handle_NaN(double value)
        {
            if (double.IsNaN(value))//Determine whether it is NaN
            {
                return 0.0;
            }
            else
            {
                return value;
            }
        }
        #endregion
    }
}
