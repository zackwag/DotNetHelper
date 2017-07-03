using System;
using System.Data;
using System.IO;
using System.Linq;
using ClosedXML.Excel;
using Helper.Extensions;

namespace Helper.Data
{
    public static class ExcelHelper
    {
        public static DataTable ExecuteDataTable(XLWorkbook workBook)
        {
            try
            {
                return ExecuteDataTable(workBook.Worksheet(1));
            }
            catch (Exception ex)
            {
                throw new Exception("error on ExecuteDataTable method, message: " + ex.Message);
            }
        }

        public static DataTable ExecuteDataTable(IXLWorksheet workSheet)
        {
            try
            {
                var dt = new DataTable();

                var firstRow = true;

                foreach (var row in workSheet.Rows())
                {
                    //Use the first row to add columns to DataTable.
                    if (firstRow)
                    {
                        row.Cells().Each(c => dt.Columns.Add(c.Value.ToString()));
                        firstRow = false;
                    }
                    else
                    {
                        //Add rows to DataTable.
                        dt.Rows.Add();

                        var i = 0;

                        foreach (var cell in row.Cells())
                        {
                            dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
                            i++;
                        }
                    }
                }

                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception("error on ExecuteDataTable method, message: " + ex.Message);
            }
        }

        public static XLWorkbook LoadWorkbook(string filePath)
        {
            try
            {
                return new XLWorkbook(filePath);
            }
            catch (Exception ex)
            {
                throw new Exception("error on LoadWorkbook method, message: " + ex.Message);
            }
        }

        public static XLWorkbook LoadWorkbook(Stream stream)
        {
            try
            {
                return new XLWorkbook(stream);
            }
            catch (Exception ex)
            {
                throw new Exception("error on LoadWorkbook method, message: " + ex.Message);
            }
        }

        public static XLWorkbook LoadWorkbook(DataTable dataSource)
        {
            try
            {
                using (var workBook = new XLWorkbook())
                {
                    workBook.AddWorksheet(LoadWorksheet(dataSource));
                    return workBook;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error on Export method, message: " + ex.Message);
            }
        }

        public static XLWorkbook LoadWorkbook(DataSet dataSource)
        {
            try
            {
                using (var workBook = new XLWorkbook())
                {
                    foreach (var dt in dataSource.Tables.Cast<DataTable>())
                    {
                        workBook.AddWorksheet(LoadWorksheet(dt, dt.TableName));
                    }

                    return workBook;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error on Export method, message: " + ex.Message);
            }
        }

        public static void SaveToDisk(XLWorkbook workbook, string fileName)
        {
            try
            {
                workbook.SaveAs(fileName);
            }
            catch (Exception ex)
            {
                throw new Exception("error on Export method, message: " + ex.Message);
            }
        }

        public static Stream SaveToMemory(XLWorkbook workbook)
        {
            try
            {
                var ms = new MemoryStream();

                workbook.SaveAs(ms);

                return ms;
            }
            catch (Exception ex)
            {
                throw new Exception("error on SaveToMemory method, message: " + ex.Message);
            }
        }

        private static IXLWorksheet LoadWorksheet(DataTable dataSource, string sheetName = "Sheet1")
        {
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add(sheetName);
                    var hasHeader = dataSource.Columns.Count > 0;

                    foreach (var c in Enumerable.Range(0, dataSource.Columns.Count))
                    {
                        var cell = worksheet.Cell(1, c + 1);
                        cell.Value = dataSource.Columns[c].ColumnName;
                        cell.Style.Font.Bold = true;
                    }

                    foreach (var r in Enumerable.Range(0, dataSource.Rows.Count))
                    {
                        foreach (var c in Enumerable.Range(0, dataSource.Rows[r].ItemArray.Length))
                        {
                            var cell = worksheet.Cell(hasHeader ? r + 2 : r + 1, c + 1);
                            var cellVal = dataSource.Rows[r].ItemArray[c];

                            if (!cellVal.IsNull())
                            {
                                switch (dataSource.Rows[r].ItemArray[c].GetType().Name)
                                {
                                    case "String":
                                        cell.Value = "'" + cellVal;
                                        cell.Style.Alignment.WrapText = false;
                                        break;
                                    case "DateTime":
                                        cell.Style.NumberFormat.NumberFormatId = 14; //14 == mm/dd/yy;
                                        goto default;
                                    default:
                                        cell.Value = cellVal;
                                        break;
                                }
                            }
                            else
                                cell.Value = string.Empty;
                        }
                    }

                    return workbook.Worksheets.First();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error on LoadWorksheet method, message: " + ex.Message);
            }
        }

        public static void DeleteSourceFile(string filePath)
        {
            try
            {
                if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                    File.Delete(filePath);
            }
            catch (Exception ex)
            {
                throw new Exception("error on DeleteSourceFile method, message: " + ex.Message);
            }
        }
    }
}
