namespace BEL.FeedbackWorkflow.Common
{
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;
    using System;

    /// <summary>
    /// Excel Helper
    /// </summary>
    public class ExcelHelper
    {
        /// <summary>
        /// Upload Excel Files By Byte Array and Extension
        /// </summary>
        /// <param name="postedFile">posted File</param>
        /// <param name="extension">File extension</param>
        /// <returns>Data Table</returns>
        public DataTable UploadExcelFile(byte[] postedFile, string extension)
        {
            if (postedFile != null)
            {
                using (Stream str = new MemoryStream(postedFile))
                {
                    string fileExtension = System.IO.Path.GetExtension(extension).ToLower();
                    ////IWorkbook workbook = null;
                    ISheet sheet;
                    if (fileExtension == ".xls" || fileExtension == ".csv")
                    {
                        HSSFWorkbook workbook = new HSSFWorkbook(str);
                        sheet = workbook.GetSheetAt(0);
                    }
                    else if (fileExtension == ".xlsx")
                    {
                        ////NPOI.OpenXml4Net.OPC.OPCPackage pkg = NPOI.OpenXml4Net.OPC.OPCPackage.Open(str);
                        XSSFWorkbook workbook = new XSSFWorkbook(str);
                        sheet = workbook.GetSheetAt(0);
                    }
                    else
                    {
                        return null;
                    }
                    return this.ConvertExcelSheetToDataTable(sheet, true);

                    ////DataSet ds = null;
                    ////using (ds = new DataSet())
                    ////{
                    ////    DataTable dt = this.ConvertExcelSheetToDataTable(sheet, true);
                    ////    ds.Tables.Add(dt);
                    ////}
                    ////return ds;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Converts Excel Sheet to Data Table
        /// </summary>
        /// <param name="sheet">the sheet</param>
        /// <param name="removeHeaderRowFromTable">the remove Header Row From Table</param>
        /// <returns>Data Table</returns>
        private DataTable ConvertExcelSheetToDataTable(ISheet sheet, bool removeHeaderRowFromTable)
        {
            DataTable table = null;
            using (table = new DataTable())
            {
                table.TableName = sheet.SheetName;
                IRow headerRow = sheet.GetRow(0);
                if (headerRow != null)
                {
                    int cellCount = headerRow.LastCellNum;

                    for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                    {
                        DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                        table.Columns.Add(column);
                    }

                    ////int rowCount = sheet.LastRowNum;
                    for (int i = sheet.FirstRowNum; i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row != null)
                        {
                            DataRow dataRow = table.NewRow();
                            for (int j = 0; j < row.LastCellNum; j++)
                            {
                                if (row.GetCell(j) != null)
                                {
                                    dataRow[j] = row.GetCell(j);
                                }
                            }

                            table.Rows.Add(dataRow);
                        }
                    }

                    if (removeHeaderRowFromTable)
                    {
                        if (table != null && table.Rows.Count != 0)
                        {
                            table.Rows.RemoveAt(0);
                        }
                    }
                }

                ////Remove all the blank rows
                List<DataRow> list = table.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field is System.DBNull || string.Compare((field as string).Trim(), string.Empty) == 0)).ToList();
                if (list.Any())
                {
                    table = list.CopyToDataTable();
                }
                else
                {
                    table.Clear();
                }

                sheet = null;
            }
            return table;
        }
    }
}