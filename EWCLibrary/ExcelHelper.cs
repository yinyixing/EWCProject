using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace EWCLibrary
{
    public class ExcelHelper: IExcel
    {
        /// <summary>
        /// import excel
        /// </summary>
        /// <param name="fileName">excel file path</param>
        /// <param name="sheetName">sheet name</param>
        /// <param name="isFirstRowColumn">Is the first row the title column</param>
        /// <returns></returns>
        public DataTable ReadExcelToDataTable(string fileName, string sheetName = null, bool isFirstRowColumn = true)
        {
            //Define the datatable object to be return
            DataTable data = new DataTable();
            GlobalMethods.dt_error = GlobalMethods.dt_error_field_RIT();
            int err_count = 0;
            //excel sheet
            ISheet sheet = null;
            //data start line(exclude title line)
            int startRow = 0;
            try
            {
                if (!File.Exists(fileName))
                {
                    GlobalMethods.dt_error.Rows.Add(new object[] { err_count, "file error", "This file was not found.(" + fileName + ")", "", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                    return null;
                }
                //read files according to the specified path
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                //create Excel data structure based on file flow
                IWorkbook workbook = WorkbookFactory.Create(fs);
                //if there is a specified worksheet name
                if (!string.IsNullOrEmpty(sheetName))
                {
                    sheet = workbook.GetSheet(sheetName);
                    if (sheet == null)
                    {
                        return null;
                    }
                }
                else
                {
                    //If no sheet corresponding to the specified sheetName is found, attempt to obtain the first sheet
                    sheet = workbook.GetSheetAt(0);
                }
                if (sheet != null)
                {
                    int theRowNum = 0;
                    IRow firstRow = sheet.GetRow(theRowNum);
                    if (firstRow == null)
                    {
                        GlobalMethods.dt_error.Rows.Add(new object[] { err_count, "Data irregularities", "There is no data in the first row of the table", "", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                        return null;
                    }
                    //the number of the last cell in a row,the total number of columns
                    int cellCount = firstRow.LastCellNum;
                    //if the first line is the title column name
                    if (isFirstRowColumn)
                    {
                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                        {
                            ICell cell = firstRow.GetCell(i);
                            if (cell != null)
                            {
                                //Before reading data, set the cell type to string.
                                cell.SetCellType(CellType.String);
                                string cellValue = cell.StringCellValue;
                                if (i == 0)
                                {
                                    if (cellValue != "theme")
                                    {
                                        err_count += 1;
                                        GlobalMethods.dt_error.Rows.Add(new object[] { err_count, "Data irregularities", "The first column needs to be 'theme'", cellValue, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                                    }
                                }
                                if (i == 1)
                                {
                                    if (cellValue != "dimension")
                                    {
                                        err_count += 1;
                                        GlobalMethods.dt_error.Rows.Add(new object[] { err_count, "Data irregularities", "The second column needs to be 'dimension'", cellValue, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                                    }
                                }
                                if (i == 2)
                                {
                                    if (cellValue != "subdimension")
                                    {
                                        err_count += 1;
                                        GlobalMethods.dt_error.Rows.Add(new object[] { err_count, "Data irregularities", "The third column needs to be 'subdimension'", cellValue, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                                    }
                                }
                                if (i == 3)
                                {
                                    if (cellValue != "indicatorName")
                                    {
                                        err_count += 1;
                                        GlobalMethods.dt_error.Rows.Add(new object[] { err_count, "Data irregularities", "The third column needs to be 'indicatorName'", cellValue, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                                    }
                                }
                                if (i == 4)
                                {
                                    if (cellValue != "positive-negative")
                                    {
                                        err_count += 1;
                                        GlobalMethods.dt_error.Rows.Add(new object[] { err_count, "Data irregularities", "The third column needs to be 'positive-negative'", cellValue, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                                    }
                                }
                                if (cellValue != null)
                                {
                                    DataColumn column = new DataColumn(cellValue);
                                    data.Columns.Add(column);
                                }
                            }
                        }
                        if (err_count > 0)
                        {
                            return null;
                        }
                    }
                    else
                    {
                        startRow = sheet.FirstRowNum;
                    }
                    startRow = sheet.FirstRowNum + theRowNum + 1;
                    //last row number
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null || row.GetCell(0) == null || string.IsNullOrEmpty(row.GetCell(0).ToString())) continue; //rows without data default to null　　　　　　　
                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (j == 4)//positive-negative
                            {
                                if (row.GetCell(j) != null)
                                {
                                    if (row.GetCell(j).ToString() != "positive" && row.GetCell(j).ToString() != "negative")
                                    {
                                        err_count += 1;
                                        GlobalMethods.dt_error.Rows.Add(new object[] { err_count, "Data irregularities", "The third column ('positive-negative') must be filled in positive or negative", "row " + j + " value:" + row.GetCell(j).ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                                    }
                                    else
                                    {
                                        dataRow[j] = row.GetCell(j).ToString();
                                    }
                                }
                                else
                                {
                                    err_count += 1;
                                    GlobalMethods.dt_error.Rows.Add(new object[] { err_count, "Data irregularities", "The third column ('positive-negative') must be filled in positive or negative", "row " + j + " value: null", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                                }
                            }
                            else if (j > 4 && i > 0)
                            {
                                try
                                {
                                    if (row.GetCell(j) != null) //cells without data default to null
                                    {
                                        dataRow[j] = row.GetCell(j).ToString();
                                        double isDouble = Convert.ToDouble(dataRow[j]);
                                    }
                                    else
                                    {
                                        err_count += 1;
                                        GlobalMethods.dt_error.Rows.Add(new object[] { err_count, "Data irregularities", "Only numerical values can be filled in the year/sample type field.", "column " + i + ",row " + j + " value: null", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                                    }
                                }
                                catch (Exception)
                                {
                                    err_count += 1;
                                    GlobalMethods.dt_error.Rows.Add(new object[] { err_count, "Data irregularities", "Only numerical values can be filled in the year/sample type field.", "column " + i + ",row " + j + " value: " + row.GetCell(j).ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });

                                }
                            }
                            else
                            {
                                if (row.GetCell(j) != null) //cells without data default to null
                                {
                                    dataRow[j] = row.GetCell(j).ToString();
                                }
                            }
                        }
                        data.Rows.Add(dataRow);
                    }
                    if (err_count > 0)
                    {
                        return null;
                    }
                }
                return data;
            }
            catch (Exception ex)
            {
                err_count += 1;
                GlobalMethods.dt_error.Rows.Add(new object[] { err_count, "Error reporting", "error:" + ex.Message, "", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                return null;
            }
        }
        /// <summary>
        /// Import Template Table
        /// </summary>
        public  DataTable Template_ReadExcelToDataTable(string fileName, string sheetName = null, bool isFirstRowColumn = true)
        {
            DataTable data = new DataTable();
            GlobalMethods.dt_error = GlobalMethods.dt_error_field();
            int err_count = 0;
            ISheet sheet = null;
            int startRow = 0;
            try
            {
                if (!File.Exists(fileName))
                {
                    GlobalMethods.dt_error.Rows.Add(new object[] { err_count, "file error", "This file was not found.(" + fileName + ")", "", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                    return null;
                }
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                IWorkbook workbook = WorkbookFactory.Create(fs);
                if (!string.IsNullOrEmpty(sheetName))
                {
                    sheet = workbook.GetSheet(sheetName);
                    if (sheet == null)
                    {
                        return null;
                    }
                }
                else
                {
                    sheet = workbook.GetSheetAt(0);
                }
                if (sheet != null)
                {
                    int theRowNum = 0;
                    IRow firstRow = sheet.GetRow(theRowNum);
                    if (firstRow == null)
                    {
                        GlobalMethods.dt_error.Rows.Add(new object[] { err_count, "Data irregularities", "There is no data in the first row of the table", "", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                        return null;
                    }
                    int cellCount = firstRow.LastCellNum;
                    if (isFirstRowColumn)
                    {
                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                        {
                            ICell cell = firstRow.GetCell(i);
                            if (cell != null)
                            {
                                cell.SetCellType(CellType.String);
                                string cellValue = cell.StringCellValue;
                                if (i == 0)
                                {
                                    if (cellValue != "theme")
                                    {
                                        err_count += 1;
                                        GlobalMethods.dt_error.Rows.Add(new object[] { err_count, "Data irregularities", "The first column needs to be 'theme'", cellValue, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                                    }
                                }
                                if (i == 1)
                                {
                                    if (cellValue != "dimension")
                                    {
                                        err_count += 1;
                                        GlobalMethods.dt_error.Rows.Add(new object[] { err_count, "Data irregularities", "The second column needs to be 'dimension'", cellValue, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                                    }
                                }
                                if (i == 2)
                                {
                                    if (cellValue != "subdimension")
                                    {
                                        err_count += 1;
                                        GlobalMethods.dt_error.Rows.Add(new object[] { err_count, "Data irregularities", "The third column needs to be 'subdimension'", cellValue, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                                    }
                                }
                                if (i == 3)
                                {
                                    if (cellValue != "indicatorName")
                                    {
                                        err_count += 1;
                                        GlobalMethods.dt_error.Rows.Add(new object[] { err_count, "Data irregularities", "The third column needs to be 'indicatorName'", cellValue, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                                    }
                                }
                                if (i == 4)
                                {
                                    if (cellValue != "positive-negative")
                                    {
                                        err_count += 1;
                                        GlobalMethods.dt_error.Rows.Add(new object[] { err_count, "Data irregularities", "The third column needs to be 'positive-negative'", cellValue, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                                    }
                                }
                                if (cellValue != null)
                                {
                                    DataColumn column = new DataColumn(cellValue);
                                    data.Columns.Add(column);
                                }
                            }
                        }
                        if (err_count > 0)
                        {
                            return null;
                        }
                    }
                    else
                    {
                        startRow = sheet.FirstRowNum;
                    }
                    startRow = sheet.FirstRowNum + theRowNum + 1;
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null || row.GetCell(0) == null || string.IsNullOrEmpty(row.GetCell(0).ToString())) continue;
                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (j == 4)//positive-negative
                            {
                                if (row.GetCell(j) != null)
                                {
                                    if (row.GetCell(j).ToString() != "positive" && row.GetCell(j).ToString() != "negative")
                                    {
                                        err_count += 1;
                                        GlobalMethods.dt_error.Rows.Add(new object[] { err_count, "Data irregularities", "The third column ('Direction') must be filled in positive or negative", "row " + j + " value:" + row.GetCell(j).ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                                    }
                                    else
                                    {
                                        dataRow[j] = row.GetCell(j).ToString();
                                    }
                                }
                                else
                                {
                                    err_count += 1;
                                    GlobalMethods.dt_error.Rows.Add(new object[] { err_count, "Data irregularities", "The third column ('Direction') must be filled in positive or negative", "row " + j + " value: null", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                                }
                            }
                            else
                            {
                                if (row.GetCell(j) != null)
                                {
                                    dataRow[j] = row.GetCell(j).ToString();
                                }
                            }
                            if (j > 4)
                            {
                                break;
                            }
                        }
                        data.Rows.Add(dataRow);
                    }
                    if (err_count > 0)
                    {
                        return null;
                    }
                }
                return data;
            }
            catch (Exception ex)
            {
                err_count += 1;
                GlobalMethods.dt_error.Rows.Add(new object[] { err_count, "Error reporting", "error:" + ex.Message, "", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                return null;
            }
        }
        /// <summary>
        /// DataGridView To DataTable
        /// </summary>
        /// <param name="dgv"></param>
        /// <returns></returns>
        public  DataTable ToDataTable(DataGridView dgv)
        {
            DataTable dt = new DataTable();
            for (int i = 0; i < dgv.ColumnCount; i++)
            {
                dt.Columns.Add(dgv.Columns[i].HeaderText);
            }
            //Write numerical values
            for (int r = 0; r < dgv.Rows.Count; r++)
            {
                List<object> values = new List<object>();
                for (int i = 0; i < dgv.ColumnCount; i++)
                {
                    values.Add(dgv.Rows[r].Cells[i].Value);
                }
                dt.Rows.Add(values.ToArray());
            }
            return dt;
        }
        /// <summary>
        /// indicators matrix (add in the table, the last line is empty)
        /// </summary>
        public DataTable ToDataTableIM(DataGridView dgv)
        {
            DataTable dt = new DataTable();
            for (int i = 0; i < dgv.ColumnCount; i++)
            {
                dt.Columns.Add(dgv.Columns[i].HeaderText);
            }
            //Write numerical values（(dgv.Rows.Count - 1:The datagridview will also include the last blank line added, so the number of rows is -1)）
            for (int r = 0; r < dgv.Rows.Count - 1; r++)
            {
                List<object> values = new List<object>();
                for (int i = 0; i < dgv.ColumnCount; i++)
                {
                    values.Add(dgv.Rows[r].Cells[i].Value);
                }
                dt.Rows.Add(values.ToArray());
            }
            return dt;
        }
        /// <summary>
        /// sheet table content input
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="data"></param>
        /// <param name="sheetName"></param>
        public void ExportToSheet(IWorkbook workbook, DataTable data, string sheetName)
        {
            ISheet sheet = workbook.CreateSheet(sheetName);
            IRow rowHead = sheet.CreateRow(0);
            //Fill in the header
            for (int i = 0; i < data.Columns.Count; i++)
            {
                rowHead.CreateCell(i, CellType.String).SetCellValue(data.Columns[i].ColumnName.ToString());
            }
            //Fill in the content
            for (int i = 0; i < data.Rows.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                for (int j = 0; j < data.Columns.Count; j++)
                {
                    row.CreateCell(j, CellType.String).SetCellValue(data.Rows[i][j].ToString());
                }
            }
            for (int i = 0; i < data.Columns.Count; i++)
            {
                sheet.AutoSizeColumn(i);
            }
        }
        /// <summary>
        /// Export indicator matrix table data to a fixed template Excel
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="data"></param>
        public void WriteToSheet(IWorkbook workbook, DataTable data)
        {
            ISheet sheet = workbook.GetSheetAt(0);
            IRow rowHead = sheet.CreateRow(0);
            //Fill in the header
            for (int i = 0; i < data.Columns.Count; i++)
            {
                rowHead.CreateCell(i, CellType.String).SetCellValue(data.Columns[i].ColumnName.ToString());
            }
            //Fill in the content
            for (int i = 0; i < data.Rows.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                for (int j = 0; j < data.Columns.Count; j++)
                {
                    row.CreateCell(j, CellType.String).SetCellValue(data.Rows[i][j].ToString());
                }
            }
            for (int i = 0; i < data.Columns.Count; i++)
            {
                sheet.AutoSizeColumn(i);
            }
        }
        /// <summary>
        /// Read Csv files in file stream format,load to DataTable
        /// </summary>
        /// <param name="path">csv file path</param>
        /// <param name="hasTitle">Is there a title line</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public DataTable ReadCsvByDataTable(string path, bool hasTitle = false)
        {
            DataTable dt = new DataTable();
            bool isFirst = true;
            int line_num = 0;
            using (StreamReader sr = new StreamReader(path))
            {
                line_num++;
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] values = line.Split(',');
                    if (isFirst)
                    {
                        for (int i = 0; i < values.Length; i++)
                        {
                            dt.Columns.Add();
                        }
                        isFirst = false;
                    }
                    //If there is a header, add it
                    if (hasTitle)
                    {
                        for (int i = 0; i < values.Length; i++)
                        {
                            dt.Columns[i].ColumnName = values[i];
                        }
                        hasTitle = false;
                    }
                    else
                    {
                        dt.Rows.Add(values);
                    }
                }
            }
            return dt;
        }


        /// <summary>
        /// read Excel to DataTable（only indicators in the template can be imported）
        /// </summary>
        /// <param name="fileName">excel file path</param>
        /// <param name="sheetName">sheet name</param>
        /// <returns></returns>
        public DataTable ReadExcelFilterDataTable(string fileName, DataTable dt_temp, Dictionary<string, bool> dic_Indicator, string sheetName = null)
        {
            //define the datatable object to be return
            DataTable data = new DataTable();
            GlobalMethods.dt_error = GlobalMethods.dt_error_field_RIT();
            int err_count = 0;
            //excel sheet
            ISheet sheet = null;
            //Start of data row (excluding header row)
            int startRow = 0;
            try
            {
                if (!File.Exists(fileName))
                {
                    GlobalMethods.dt_error.Rows.Add(new object[] { err_count, "file error", "This file was not found.(" + fileName + ")", "", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                    return null;
                }
                //Reads the file according to the specified path
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                //Create excel data structures from file streams
                IWorkbook workbook = WorkbookFactory.Create(fs);
                //If a sheet name is specified
                if (!string.IsNullOrEmpty(sheetName))
                {
                    sheet = workbook.GetSheet(sheetName);
                    if (sheet == null)
                    {
                        return null;
                    }
                }
                else
                {
                    //If no sheetName is specified, it tries to get the first sheet
                    sheet = workbook.GetSheetAt(0);
                }
                if (sheet != null)
                {
                    int theRowNum = 0;
                    IRow firstRow = sheet.GetRow(theRowNum);
                    if (firstRow == null)
                    {
                        GlobalMethods.dt_error.Rows.Add(new object[] { err_count, "Data irregularities", "There is no data in the first row of the table", "", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                        return null;
                    }
                    //The number of the last cell in the line,That's the total number of columns
                    int cellCount = firstRow.LastCellNum;
                    for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                    {
                        ICell cell = firstRow.GetCell(i);
                        if (cell != null)
                        {
                            //Before reading data, set the cell type to string.
                            cell.SetCellType(CellType.String);
                            string cellValue = cell.StringCellValue;
                            if (cellValue != null)
                            {
                                if (i == 0)
                                {
                                    data.Columns.Add("theme");
                                    data.Columns.Add("dimension");
                                    data.Columns.Add("subdimension");
                                    data.Columns.Add("indicatorName");
                                    data.Columns.Add("positive-negative");
                                }
                                else
                                {
                                    DataColumn column = new DataColumn(cellValue);
                                    data.Columns.Add(column);
                                }
                            }
                        }
                    }
                    if (err_count > 0)
                    {
                        return null;
                    }
                    startRow = sheet.FirstRowNum + theRowNum + 1;
                    //Last column number
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null || row.GetCell(0) == null || string.IsNullOrEmpty(row.GetCell(0).ToString())) continue;　　　　
                        DataRow dataRow = data.NewRow();
                        bool ishave = false;
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (j > 0)
                            {
                                try
                                {
                                    if (row.GetCell(j) != null)
                                    {
                                        dataRow[j + 4] = row.GetCell(j).ToString();
                                        double isDouble = Convert.ToDouble(dataRow[j + 4]);
                                    }
                                    else
                                    {
                                        err_count += 1;
                                        GlobalMethods.dt_error.Rows.Add(new object[] { err_count, "Data irregularities", "Only numerical values can be filled in the year/sample type field.", "column " + i + ",row " + j + " value: null", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                                    }
                                }
                                catch (Exception)
                                {
                                    err_count += 1;
                                    GlobalMethods.dt_error.Rows.Add(new object[] { err_count, "Data irregularities", "Only numerical values can be filled in the year/sample type field.", "column " + i + ",row " + j + " value: " + row.GetCell(j).ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });

                                }
                            }
                            else
                            {
                                if (row.GetCell(j) != null)
                                {
                                    string indicatorName = row.GetCell(j).ToString().Replace("\r", "").Replace("\n", "").Replace("\"", "");//Gets the first column of metrics
                                    try
                                    {
                                        //Matches indicators in the template
                                        if (dic_Indicator.ContainsKey(indicatorName))
                                        {
                                            DataRow[] drArr = dt_temp.Select("indicatorName='" + indicatorName + "'");
                                            dic_Indicator[indicatorName] = true;
                                            dataRow[0] = drArr[0][0].ToString();
                                            dataRow[1] = drArr[0][1].ToString();
                                            dataRow[2] = drArr[0][2].ToString();
                                            dataRow[3] = drArr[0][3].ToString();
                                            dataRow[4] = drArr[0][4].ToString();
                                            ishave = true;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        err_count += 1;
                                        GlobalMethods.dt_error.Rows.Add(new object[] { err_count, ex.Message, "column " + i + ",row " + j + " value: " + indicatorName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                                    }
                                }
                            }
                        }
                        if (ishave)
                        {
                            data.Rows.Add(dataRow);
                        }
                    }
                    int isfalse = 0;
                    foreach (var item in dic_Indicator)
                    {
                        if (item.Value == false)
                        {
                            isfalse++;
                            err_count++;
                            string systemName = "";
                            try
                            {
                                DataRow[] drArr = dt_temp.Select("indicatorName='" + item.Key + "'");
                                systemName = drArr[0][0].ToString();
                            }
                            catch (Exception)
                            {
                                systemName = "";
                            }
                            err_count += 1;
                            GlobalMethods.dt_error.Rows.Add(new object[] { err_count, "Data irregularities", "No  '" + item.Key + "'  found in the file", "", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                        }
                    }
                    if (err_count > 0)
                    {
                        return null;
                    }
                }
                return data;
            }
            catch (Exception ex)
            {
                err_count += 1;
                GlobalMethods.dt_error.Rows.Add(new object[] { err_count, "Error reporting", "error:" + ex.Message, "", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                return null;
            }
        }
        /// <summary>
        /// read csv file to DataTable（only indicators in the template can be imported）
        /// </summary>
        /// <param name="path">csv file path</param>
        /// <param name="dic_Subsystem">Template data, used to match the required indicators</param>
        /// <param name="hasTitle">Is there a title line</param>
        /// <returns></returns>
        public DataTable ReadCsvByDataTable_Filter(string path, Dictionary<string, bool> dic_Indicator, DataTable dt_error, int err_count, DataTable dt_rit, bool hasTitle = false)
        {
            try
            {
                DataTable dt = new DataTable();
                bool isFirst = true;
                int line_num = 0;
                using (StreamReader sr = new StreamReader(path))
                {
                    line_num++;
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] values = line.Split(',');
                        if (isFirst)
                        {
                            for (int i = 0; i < values.Length; i++)
                            {
                                if (i == 0)
                                {
                                    dt.Columns.Add();
                                    dt.Columns.Add();
                                    dt.Columns.Add();
                                }
                                else
                                {
                                    dt.Columns.Add();
                                }
                            }
                            isFirst = false;
                        }
                        //If there is a header, add
                        if (hasTitle)
                        {
                            for (int i = 0; i < values.Length; i++)
                            {
                                //Add the corresponding column
                                if (i == 0)
                                {
                                    dt.Columns[0].ColumnName = "Subsystem";
                                    dt.Columns[1].ColumnName = "Indicator";
                                    dt.Columns[2].ColumnName = "Direction";
                                }
                                else
                                {
                                    dt.Columns[i + 2].ColumnName = values[i];
                                }
                            }
                            hasTitle = false;
                        }
                        else
                        {
                            string Indicator = values[0].Replace("\r", "").Replace("\n", "").Replace("\"", ""); //Gets the first column of metrics 
                            //Matches indicators in the template
                            if (dic_Indicator.ContainsKey(Indicator))
                            {
                                dic_Indicator[Indicator] = true;
                                dt.Rows.Add(ChangeDataToD(values, dt_rit));
                            }
                        }
                    }
                }
                int isfalse = 0;
                foreach (var item in dic_Indicator)
                {
                    // Console.WriteLine($"Key: {item.Key}, Value: {item.Value}");
                    if (item.Value == false)
                    {
                        isfalse++;
                        err_count++;
                        string systemName = "";
                        try
                        {
                            DataRow[] drArr = dt_rit.Select("Indicator='" + item.Key + "'");
                            systemName = drArr[0][0].ToString();
                        }
                        catch (Exception)
                        {
                            systemName = "";
                        }
                        dt_error.Rows.Add(new object[] { err_count, systemName, "No  '" + item.Key + "'  found in the file", Path.GetFileNameWithoutExtension(path), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                    }
                }
                /*if (isfalse > 0)
                {
                    return null;
                }
                else
                {
                    return dt;
                }*/
                return dt;
            }
            catch (Exception ex)
            {
                err_count++;
                dt_error.Rows.Add(new object[] { err_count, "", "Error:" + ex.Message, "", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                return null;
            }
        }
        /// <summary>
        /// Scientific notation (E) converts to normal numeric values
        /// </summary>
        /// <param name="strData"></param>
        /// <returns></returns>
        public string[] ChangeDataToD(string[] values, DataTable dt_rit)
        {
            string[] new_values = new string[values.Length + 2];
            for (int i = 0; i < values.Length; i++)
            {
                try
                {
                    if (i == 0)
                    {
                        //Add the corresponding data to multiple columns
                        string Indicator = values[i].Replace("\r", "").Replace("\n", "").Replace("\"", "");
                        string filter = "Indicator='" + Indicator + "'";
                        DataRow[] drArr = dt_rit.Select(filter);
                        new_values[0] = drArr[0][0].ToString();
                        new_values[1] = Indicator;
                        new_values[2] = drArr[0][3].ToString();
                    }
                    else
                    {
                        if (values[i].ToString() == "")
                        {
                            continue;
                        }
                        else
                        {
                            Decimal dData = 0.0M;
                            if (values[i].Contains("E") || values[i].Contains("e"))
                            {
                                dData = Convert.ToDecimal(Decimal.Parse(values[i].ToString(), System.Globalization.NumberStyles.Float));
                                new_values[i + 2] = dData.ToString();
                            }
                            else
                            {
                                new_values[i + 2] = values[i];
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                    continue;
                }
            }
            return new_values;
        }

        /// <summary>
        /// General export to excel
        /// </summary>
        public void genericExport(string FileName, DataTable dtExport, string sheetName)
        {
            using (SaveFileDialog sFileDialog = new SaveFileDialog())
            {
                sFileDialog.Title = "Export";
                sFileDialog.FileName = FileName;
                sFileDialog.Filter = "Excel file(*.xlsx)|*.xlsx|Excel 文件(*.xls)|*.xls";
                sFileDialog.AddExtension = true;
                if (sFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string filename = sFileDialog.FileName;
                        IWorkbook workbook = new XSSFWorkbook();
                        ExportToSheet(workbook, dtExport, sheetName);
                        using (FileStream fStream = new FileStream(filename, FileMode.Create, FileAccess.Write))
                        {
                            workbook.Write(fStream);
                            fStream.Close();
                        }
                        GC.Collect();
                        MessageBox.Show("Export successful.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error:" + ex.Message);
                    }
                }
            }
        }
    }
}
