using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using Enterprise.Framework.File;
using Enterprise.Framework.Utils;
using FineUI;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using AspNet = System.Web.UI.WebControls;
using System.Web.UI;

namespace Enterprise.IIS.Common
{
    public class Utils
    {
        #region 通过NPOI导出
        /// <summary>
        ///     生成
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        public static DataTable GridConvertToDataTablse(Grid grid)
        {
            var table = new DataTable();
            foreach (GridColumn t in grid.Columns)
            {
                if (t.Visible && t.HeaderText != "序号")
                {
                    table.Columns.Add(t.HeaderText);
                }
            }
            for (int k = 0; k < grid.Rows.Count; k++)
            {
                table.Rows.Add();
                for (int i = 0; i < grid.Columns.Count; i++)
                {
                    if (grid.Columns[i].Visible && grid.Columns[i].HeaderText != "序号")
                    {
                        table.Rows[k][grid.Columns[i].HeaderText] = grid.Rows[k].Values[i];
                    }
                }
            }
            return table;
        }

        /// <summary>
        ///     创建一个Excel
        /// </summary>
        /// <returns>返回一个空表格</returns>
        public static HSSFWorkbook InitializeWorkBook()
        {
            var workBook = new HSSFWorkbook();
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();

            dsi.Company = "909994561";
            dsi.Manager = "Office Word 2003/2007";

            si.Author = "www.liyanjiang.com";
            si.Subject = "NPOI组件库";
            si.Title = "www.liyanjiang.com";

            workBook.DocumentSummaryInformation = dsi;
            workBook.SummaryInformation = si;

            return workBook;
        }

        /// <summary>
        /// 把指定的DataTable导出Excel
        /// Yakecan
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="dataTable">数据源</param>
        /// <param name="filename">Sheet的名称</param>
        /// <param name="fls">返回生成的文件路径</param>
        public static void Export(HttpContext httpContext, DataTable dataTable, string filename, out string fls)
        {
            HSSFWorkbook workbook = InitializeWorkBook();
            ISheet sheet1 = workbook.CreateSheet(filename);

            IRow titleRow = sheet1.CreateRow(0);
            titleRow.Height = (short)20 * 25;

            ICellStyle titleStyle = workbook.CreateCellStyle();
            titleStyle.Alignment = HorizontalAlignment.Center;
            titleStyle.VerticalAlignment = VerticalAlignment.Center;
            IFont font = workbook.CreateFont();
            font.FontName = "宋体";
            font.FontHeightInPoints = (short)16;
            titleStyle.SetFont(font);

            var region = new CellRangeAddress(0, 0, 0, dataTable.Columns.Count);
            sheet1.AddMergedRegion(region); 

            var titleCell = titleRow.CreateCell(0);
            titleCell.CellStyle = titleStyle;
            titleCell.SetCellValue(filename);

            IRow headerRow = sheet1.CreateRow(1);
            var headerStyle = workbook.CreateCellStyle();
            headerStyle.Alignment = HorizontalAlignment.Center;
            headerStyle.VerticalAlignment = VerticalAlignment.Center;
            headerStyle.BorderBottom = BorderStyle.Thin;
            headerStyle.BorderLeft = BorderStyle.Thin;
            headerStyle.BorderRight = BorderStyle.Thin;
            headerStyle.BorderTop = BorderStyle.Thin;
            IFont titleFont = workbook.CreateFont();
            titleFont.FontHeightInPoints = (short)11;
            titleFont.FontName = "宋体";
            headerStyle.SetFont(titleFont);
            headerRow.CreateCell(0).SetCellValue("序号");
            headerRow.GetCell(0).CellStyle = headerStyle;

            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                headerRow.CreateCell(i + 1).SetCellValue(dataTable.Columns[i].ColumnName);
                headerRow.GetCell(i + 1).CellStyle = headerStyle;
                sheet1.SetColumnWidth(i, 256 * 18);
            }

            ICellStyle bodyStyle = workbook.CreateCellStyle();
            bodyStyle.BorderBottom = BorderStyle.Thin;
            bodyStyle.BorderLeft = BorderStyle.Thin;
            bodyStyle.BorderRight = BorderStyle.Thin;
            bodyStyle.BorderTop = BorderStyle.Thin;
            for (int r = 0; r < dataTable.Rows.Count; r++)
            {
                IRow bodyRow = sheet1.CreateRow(r + 2);
                bodyRow.CreateCell(0).SetCellValue(r + 1);
                bodyRow.GetCell(0).CellStyle = bodyStyle;
                bodyRow.GetCell(0).CellStyle.Alignment = HorizontalAlignment.Left;

                for (int c = 0; c < dataTable.Columns.Count; c++)
                {
                    bodyRow.CreateCell(c + 1).SetCellValue(dataTable.Rows[r][c].ToString());
                    bodyRow.GetCell(c + 1).CellStyle = bodyStyle;
                }
            } 

            var sequence = SequenceGuid.GetGuid();

            var files = (string.Format(@"~/upload/temp/{0}/", DateTime.Now.ToString("yyyy-MM-dd"))); //

            if (!DirFile.XFileExists(httpContext.Server.MapPath(files)))
            {
                DirFile.XCreateDir(httpContext.Server.MapPath(files));
            }
            var uploadpath = files + sequence + ".xls";

            //创建文件            
            using (var fileStream = new FileStream(httpContext.Server.MapPath(uploadpath), FileMode.Create))
            {
                workbook.Write(fileStream);
                fileStream.Close();
            }

            //返回生成文件
            fls = uploadpath;
        }

        /// <summary>
        /// 把指定的DataTable导出Excel
        /// Yakecan
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="dataTable">数据源</param>
        /// <param name="filename">Sheet的名称</param>
        /// <param name="fls">返回生成的文件路径</param>
        public static void Export(Page httpContext, DataTable dataTable, string filename, out string fls)
        {
            HSSFWorkbook workbook = InitializeWorkBook();
            ISheet sheet1 = workbook.CreateSheet(filename);

            IRow titleRow = sheet1.CreateRow(0);
            titleRow.Height = (short)20 * 25;

            ICellStyle titleStyle = workbook.CreateCellStyle();
            titleStyle.Alignment = HorizontalAlignment.Center;
            titleStyle.VerticalAlignment = VerticalAlignment.Center;
            IFont font = workbook.CreateFont();
            font.FontName = "宋体";
            font.FontHeightInPoints = (short)16;
            titleStyle.SetFont(font);

            var region = new CellRangeAddress(0, 0, 0, dataTable.Columns.Count);
            sheet1.AddMergedRegion(region);

            var titleCell = titleRow.CreateCell(0);
            titleCell.CellStyle = titleStyle;
            titleCell.SetCellValue(filename);

            IRow headerRow = sheet1.CreateRow(1);
            var headerStyle = workbook.CreateCellStyle();
            headerStyle.Alignment = HorizontalAlignment.Center;
            headerStyle.VerticalAlignment = VerticalAlignment.Center;
            headerStyle.BorderBottom = BorderStyle.Thin;
            headerStyle.BorderLeft = BorderStyle.Thin;
            headerStyle.BorderRight = BorderStyle.Thin;
            headerStyle.BorderTop = BorderStyle.Thin;
            IFont titleFont = workbook.CreateFont();
            titleFont.FontHeightInPoints = (short)11;
            titleFont.FontName = "宋体";
            headerStyle.SetFont(titleFont);
            headerRow.CreateCell(0).SetCellValue("序号");
            headerRow.GetCell(0).CellStyle = headerStyle;

            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                headerRow.CreateCell(i + 1).SetCellValue(dataTable.Columns[i].ColumnName);
                headerRow.GetCell(i + 1).CellStyle = headerStyle;
                sheet1.SetColumnWidth(i, 256 * 18);
            }

            ICellStyle bodyStyle = workbook.CreateCellStyle();
            bodyStyle.BorderBottom = BorderStyle.Thin;
            bodyStyle.BorderLeft = BorderStyle.Thin;
            bodyStyle.BorderRight = BorderStyle.Thin;
            bodyStyle.BorderTop = BorderStyle.Thin;
            for (int r = 0; r < dataTable.Rows.Count; r++)
            {
                IRow bodyRow = sheet1.CreateRow(r + 2);
                bodyRow.CreateCell(0).SetCellValue(r + 1);
                bodyRow.GetCell(0).CellStyle = bodyStyle;
                bodyRow.GetCell(0).CellStyle.Alignment = HorizontalAlignment.Left;

                for (int c = 0; c < dataTable.Columns.Count; c++)
                {
                    bodyRow.CreateCell(c + 1).SetCellValue(dataTable.Rows[r][c].ToString());
                    bodyRow.GetCell(c + 1).CellStyle = bodyStyle;
                }
            }

            var sequence = SequenceGuid.GetGuid();

            var files = (string.Format(@"~/upload/temp/{0}/", DateTime.Now.ToString("yyyy-MM-dd"))); //

            if (!DirFile.XFileExists(httpContext.Server.MapPath(files)))
            {
                DirFile.XCreateDir(httpContext.Server.MapPath(files));
            }
            var uploadpath = files + sequence + ".xls";

            //创建文件            
            using (var fileStream = new FileStream(httpContext.Server.MapPath(uploadpath), FileMode.Create))
            {
                workbook.Write(fileStream);
                fileStream.Close();
            }

            //返回生成文件
            fls = uploadpath;
        }
        /// <summary>
        ///     HSSFWorkbook
        /// </summary>
        public HSSFWorkbook HssfWorkbook { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        public static string GetGridTableHtml(Grid grid)
        {
            StringBuilder sb = new StringBuilder();
 
            var mht = new MultiHeaderTable();
            mht.ResolveMultiHeaderTable(grid.Columns);
            sb.Append("<meta http-equiv=\"content-type\" content=\"application/excel; charset=UTF-8\"/>");
            sb.Append("<table cellspacing=\"0\" rules=\"all\" border=\"1\" style=\"border-collapse:collapse;\">");
 
            foreach (List<object[]> rows in mht.MultiTable)
            {
                sb.Append("<tr>");
                foreach (object[] cell in rows)
                {
                    int rowspan = Convert.ToInt32(cell[0]);
                    int colspan = Convert.ToInt32(cell[1]);
                    var column = cell[2] as GridColumn;

                    if (column != null)
                        sb.AppendFormat("<th{0}{1}{2}>{3}</th>",
                            rowspan != 1 ? " rowspan=\"" + rowspan + "\"" : "",
                            colspan != 1 ? " colspan=\"" + colspan + "\"" : "",
                            colspan != 1 ? " style=\"text-align:center;\"" : "",
                            column.HeaderText);
                }
                sb.Append("</tr>");
            }
 
            foreach (GridRow row in grid.Rows)
            {
                sb.Append("<tr>");
                foreach (GridColumn column in mht.Columns)
                {
                    string html = row.Values[column.ColumnIndex].ToString();
 
                    if (column.ColumnID == "tfNumber")
                    {
                        var htmlGenericControl = row.FindControl("spanNumber") as HtmlGenericControl;
                        if (htmlGenericControl != null)
                            html = htmlGenericControl.InnerText;
                    }
                    else if (column.ColumnID == "tfGender")
                    {
                        var label = row.FindControl("labGender") as AspNet.Label;
                        if (label != null)
                            html = label.Text;
                    }

                    sb.AppendFormat("<td>{0}</td>", html);
                }
                sb.Append("</tr>");
            }
 
            sb.Append("</table>");
 
            return sb.ToString();
        }
        #endregion

        #region 多表头处理导出

        /// <summary>
        ///Response.ClearContent();
        ///Response.AddHeader("content-disposition", "attachment; filename=myexcel.xls");
        ///Response.ContentType = "application/excel";
        ///Response.ContentEncoding = System.Text.Encoding.UTF8;
        ///Response.Write(GetGridTableHtml(Grid1));
        ///Response.End();
        /// 处理多表头的类
        /// </summary>
        public class MultiHeaderTable
        {
            public List<List<object[]>> MultiTable = new List<List<object[]>>();
            public List<GridColumn> Columns = new List<GridColumn>();
            public void ResolveMultiHeaderTable(GridColumnCollection columns)
            {
                var row = new List<object[]>();
                foreach (GridColumn column in columns)
                {
                    var cell = new object[4];
                    cell[0] = 1;    // rowspan
                    cell[1] = 1;    // colspan
                    cell[2] = column;
                    cell[3] = null;
                    row.Add(cell);
                }
 
                ResolveMultiTable(row, 0);
 
                ResolveColumns(row);
            }
            private void ResolveColumns(IEnumerable<object[]> row)
            {
                foreach (object[] cell in row)
                {
                    var groupField = cell[2] as GroupField;
                    if (groupField != null && groupField.Columns.Count > 0)
                    {
                        var subrow = groupField.Columns.Select(column => new object[]
                        {
                            1, 1, column, groupField
                        }).ToList();

                        ResolveColumns(subrow);
                    }
                    else
                    {
                        Columns.Add(cell[2] as GridColumn);
                    }
                }
            }
            private void ResolveMultiTable(List<object[]> row, int level)
            {
                var nextrow = new List<object[]>();
 
                foreach (object[] cell in row)
                {
                    var groupField = cell[2] as GroupField;
                    if (groupField != null && groupField.Columns.Count > 0)
                    {
                        // 如果当前列包含子列，则更改当前列的 colspan，以及增加父列（向上递归）的colspan
                        cell[1] = Convert.ToInt32(groupField.Columns.Count);
                        PlusColspan(level - 1, cell[3] as GridColumn,groupField.Columns.Count - 1);

                        nextrow.AddRange(groupField.Columns.Select(column => new object[]
                        {
                            1, 1, column, groupField
                        }));
                    }
                }
 
                MultiTable.Add(row);
 
                // 如果当前下一行，则增加上一行（向上递归）中没有子列的列的 rowspan
                if (nextrow.Count > 0)
                {
                    PlusRowspan(level);
 
                    ResolveMultiTable(nextrow, level + 1);
                }
            }
            private void PlusRowspan(int level)
            {
                if (level < 0)
                {
                    return;
                }
                foreach (object[] cells in MultiTable[level])
                {
                    var groupField = cells[2] as GroupField;
                    if (groupField != null && groupField.Columns.Count > 0)
                    {
                    }
                    else
                    {
                        cells[0] = Convert.ToInt32(cells[0]) + 1;
                    }
                }
 
                PlusRowspan(level - 1);
            }
            private void PlusColspan(int level, GridColumn parent, int plusCount)
            {
                if (level < 0)
                {
                    return;
                }
                foreach (object[] cells in MultiTable[level])
                {
                    var column = cells[2] as GridColumn;
                    if (column == parent)
                    {
                        cells[1] = Convert.ToInt32(cells[1]) + plusCount;
                        PlusColspan(level - 1, cells[3] as GridColumn, plusCount);
                    }
                }
            }
        }
        #endregion
    }
}