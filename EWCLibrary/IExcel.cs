using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EWCLibrary
{
    public interface IExcel
    {
        /// <summary>
        /// import excel
        /// </summary>
        /// <param name="fileName">excel file path</param>
        /// <param name="sheetName">sheet name</param>
        /// <param name="isFirstRowColumn">Is the first row the title column</param>
        /// <returns></returns>
        DataTable ReadExcelToDataTable(string fileName, string sheetName = null, bool isFirstRowColumn = true);
        /// <summary>
        /// Import Template Table
        /// </summary>
        DataTable Template_ReadExcelToDataTable(string fileName, string sheetName = null, bool isFirstRowColumn = true);
        /// <summary>
        /// DataGridView To DataTable
        /// </summary>
        /// <param name="dgv"></param>
        /// <returns></returns>
        DataTable ToDataTable(DataGridView dgv);
        /// <summary>
        /// indicators matrix (add in the table, the last line is empty)
        /// </summary>
        DataTable ToDataTableIM(DataGridView dgv);
        /// <summary>
        /// sheet table content input
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="data"></param>
        /// <param name="sheetName"></param>
        void ExportToSheet(IWorkbook workbook, DataTable data, string sheetName);
        /// <summary>
        /// Export indicator matrix table data to a fixed template Excel
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="data"></param>
        void WriteToSheet(IWorkbook workbook, DataTable data);
        /// <summary>
        /// Read Csv files in file stream format,load to DataTable
        /// </summary>
        /// <param name="path">csv file path</param>
        /// <param name="hasTitle">Is there a title line</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        DataTable ReadCsvByDataTable(string path, bool hasTitle = false);
        /// <summary>
        /// read Excel to DataTable（only indicators in the template can be imported）
        /// </summary>
        /// <param name="fileName">excel file path</param>
        /// <param name="sheetName">sheet name</param>
        /// <returns></returns>
        DataTable ReadExcelFilterDataTable(string fileName, DataTable dt_temp, Dictionary<string, bool> dic_Indicator, string sheetName = null);
        /// <summary>
        /// read  csv file to DataTable（only indicators in the template can be imported）
        /// </summary>
        /// <param name="path">csv file path</param>
        /// <param name="dic_Subsystem">Template data, used to match the required indicators</param>
        /// <param name="hasTitle">Is there a title line</param>
        /// <returns></returns>
        DataTable ReadCsvByDataTable_Filter(string path, Dictionary<string, bool> dic_Indicator, DataTable dt_error, int err_count, DataTable dt_rit, bool hasTitle = false);
        /// <summary>
        /// Scientific notation (E) converts to normal numeric values
        /// </summary>
        /// <param name="strData"></param>
        /// <returns></returns>
        string[] ChangeDataToD(string[] values, DataTable dt_rit);
        /// <summary>
        /// General export to excel
        /// </summary>
        void genericExport(string FileName, DataTable dtExport, string sheetName);
    }
}
