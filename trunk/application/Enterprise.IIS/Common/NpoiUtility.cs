using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using Enterprise.Framework.File;
using Enterprise.Framework.Utils;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;


namespace Enterprise.IIS.Common
{
    /// <summary>
    /// 导出EXCEL
    /// </summary>
    public class NpoiUtility
    {
        /// <summary>
        /// 通过NPOI，读模版表格，生成报表
        /// </summary>
        /// <param name="fileName">报表名称</param>
        /// <param name="rowIndex">从模版中的第几行插入操作</param>
        /// <param name="httpContext">上下文件</param>
        /// <param name="dataSource">数据源</param>
        /// <param name="templatePath">模版文件</param>
        /// <param name="filePath">生成后的文件位置</param>
        /// <returns></returns>
        public static bool ExportExcel(string fileName, Int32 rowIndex, HttpContext httpContext, //
            DataTable dataSource, string templatePath, out string filePath)
        {
            InitializeWorkbook(httpContext, templatePath);

            var sheet = _hssfworkbook.GetSheetAt(0);
            try
            {
                foreach (DataRow row in dataSource.Rows)
                {
                    #region 填充内容
                    var iRow = sheet.GetRow(rowIndex);
                    foreach (DataColumn column in dataSource.Columns)
                    {
                        if (null == iRow)
                            continue;

                        var iCell = iRow.GetCell(column.Ordinal);

                        if (null == iCell)
                            continue;

                        string rowValue = row[column].ToString();
                        switch (column.DataType.ToString())
                        {
                            case "System.String"://字符串类型   
                                iCell.SetCellValue(rowValue);

                                break;
                            case "System.DateTime"://日期类型   
                                DateTime valueTime;
                                DateTime.TryParse(rowValue, out valueTime);
                                iCell.SetCellValue(valueTime.ToString());

                                break;
                            case "System.Boolean"://布尔型   
                                bool value;
                                bool.TryParse(rowValue, out value);
                                iCell.SetCellValue(value);

                                break;
                            case "System.Int16"://整型   
                            case "System.Int32":
                            case "System.Int64":
                            case "System.Byte":
                                int intValue;
                                int.TryParse(rowValue, out intValue);
                                iCell.SetCellValue(intValue);

                                break;
                            case "System.Decimal"://浮点型   
                            case "System.Double":
                                double doubValue;
                                double.TryParse(rowValue, out doubValue);
                                iCell.SetCellValue(doubValue);

                                break;
                            case "System.DBNull"://空值处理   
                                iCell.SetCellValue("");

                                break;
                            default:
                                iCell.SetCellValue("");

                                break;
                        }
                    }
                    #endregion

                    rowIndex++;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            sheet.ForceFormulaRecalculation = true;

            //生成sequence
            var sequence = SequenceGuid.GetGuid();

            //上传文件路径
            var uploadpath = ConfigurationManager.AppSettings["上传文件路径"];

            //该目录设定死，最好不要修改
            var fileTemp = (string.Format(@"{0}/temp/{1}/", uploadpath, DateTime.Now.ToString("yyyy-MM-dd"))); //

            //目录不存在，生成新文件夹
            if (!DirFile.XFileExists(httpContext.Server.MapPath(fileTemp)))
            {
                DirFile.XCreateDir(httpContext.Server.MapPath(fileTemp));
            }

            //拼接生成后的文件路径
            uploadpath = string.Format(@"{0}/{1}{2}.xls", fileTemp, fileName, sequence);

            //创建文件            
            using (var fileStream = new FileStream(httpContext.Server.MapPath(uploadpath), FileMode.Create))
            {
                //通过流方式写入到EXCEL中
                _hssfworkbook.Write(fileStream);

                //关闭输入流
                fileStream.Close();
            }

            //输出生成后的文件路径
            filePath = uploadpath;

            return true;
        }

        /// <summary>
        /// 通过NPOI，读模版表格，生成报表(针对不规则表格处理)
        /// </summary>
        /// <param name="fileName">报表名称</param>
        /// <param name="context">上下文件</param>
        /// <param name="parms">第几行几列存储的内容,格式:1|1</param>
        /// <param name="templatePath">模版文件</param>
        /// <param name="filePath">生成后的文件位置</param>
        /// <returns></returns>
        public static bool ExportExcel(string fileName, HttpContext context, //
            Dictionary<string, object> parms, string templatePath, out string filePath)
        {
            InitializeWorkbook(context, templatePath);

            var sheet = _hssfworkbook.GetSheetAt(0);

            foreach (var o in parms)
            {
                var key = o.Key;
                var value = o.Value.ToString();

                var row = Convert.ToInt32(key.Split('|')[0]);
                var col = Convert.ToInt32(key.Split('|')[1]);

                var dataRow = sheet.GetRow(row);
                var newCell = dataRow.GetCell(col);
                if (newCell == null)
                    continue;
                newCell.SetCellValue(value);
            }

            var sequence = SequenceGuid.GetGuid();

            //上传文件路径
            var uploadpath = ConfigurationManager.AppSettings["上传文件路径"];

            //该目录设定死，最好不要修改
            var fileTemp = (string.Format(@"{0}/temp/{1}/", uploadpath, DateTime.Now.ToString("yyyy-MM-dd"))); //

            if (!DirFile.XFileExists(context.Server.MapPath(fileTemp)))
            {
                DirFile.XCreateDir(context.Server.MapPath(fileTemp));
            }

            //
            uploadpath = string.Format(@"{0}/{1}{2}.xls", fileTemp, fileName, sequence);

            //创建文件            
            using (var fileStream = new FileStream(context.Server.MapPath(uploadpath), FileMode.Create))
            {
                _hssfworkbook.Write(fileStream);
                fileStream.Close();
            }

            filePath = uploadpath;

            return true;
        }


        /// <summary>
        /// 通过NPOI，读模版表格，生成报表(针对不规则表格处理)
        /// </summary>
        /// <param name="fileName">报表名称</param>
        /// <param name="context">上下文件</param>
        /// <param name="parms">第几行几列存储的内容,格式:1|1</param>
        /// <param name="templatePath">模版文件</param>
        /// <param name="filePath">生成后的文件位置</param>
        /// <returns></returns>
        public static bool ExportExcel2(string fileName,Int32 rowIndex,  HttpContext context, //
            Dictionary<string, object> parms, DataTable dataSource, string templatePath, out string filePath)
        {
            InitializeWorkbook(context, templatePath);

            var sheet = _hssfworkbook.GetSheetAt(0);

            //指定单元格
            foreach (var o in parms)
            {
                var key = o.Key;
                var value = o.Value.ToString();

                var row = Convert.ToInt32(key.Split('|')[0]);
                var col = Convert.ToInt32(key.Split('|')[1]);

                var dataRow = sheet.GetRow(row);
                var newCell = dataRow.GetCell(col);
                if (newCell == null)
                    continue;
                newCell.SetCellValue(value);
            }

            //指定行写入
            foreach (DataRow row in dataSource.Rows)
            {
                #region 填充内容
                var iRow = sheet.GetRow(rowIndex);
                foreach (DataColumn column in dataSource.Columns)
                {
                    if (null == iRow)
                        continue;

                    var iCell = iRow.GetCell(column.Ordinal);

                    if (null == iCell)
                        continue;

                    string rowValue = row[column].ToString();
                    switch (column.DataType.ToString())
                    {
                        case "System.String"://字符串类型   
                            iCell.SetCellValue(rowValue);

                            break;
                        case "System.DateTime"://日期类型   
                            DateTime valueTime;
                            DateTime.TryParse(rowValue, out valueTime);
                            iCell.SetCellValue(valueTime.ToString());

                            break;
                        case "System.Boolean"://布尔型   
                            bool value;
                            bool.TryParse(rowValue, out value);
                            iCell.SetCellValue(value);

                            break;
                        case "System.Int16"://整型   
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intValue;
                            int.TryParse(rowValue, out intValue);
                            iCell.SetCellValue(intValue);

                            break;
                        case "System.Decimal"://浮点型   
                        case "System.Double":
                            double doubValue;
                            double.TryParse(rowValue, out doubValue);
                            iCell.SetCellValue(doubValue);

                            break;
                        case "System.DBNull"://空值处理   
                            iCell.SetCellValue("");

                            break;
                        default:
                            iCell.SetCellValue("");

                            break;
                    }
                }
                #endregion

                rowIndex++;
            }

            var sequence = SequenceGuid.GetGuid();

            //上传文件路径
            var uploadpath = ConfigurationManager.AppSettings["上传文件路径"];

            //该目录设定死，最好不要修改
            var fileTemp = (string.Format(@"{0}/temp/{1}/", uploadpath, DateTime.Now.ToString("yyyy-MM-dd"))); //

            if (!DirFile.XFileExists(context.Server.MapPath(fileTemp)))
            {
                DirFile.XCreateDir(context.Server.MapPath(fileTemp));
            }

            //
            uploadpath = string.Format(@"{0}/{1}{2}.xls", fileTemp, fileName, sequence);

            //创建文件            
            using (var fileStream = new FileStream(context.Server.MapPath(uploadpath), FileMode.Create))
            {
                _hssfworkbook.Write(fileStream);
                fileStream.Close();
            }

            filePath = uploadpath;

            return true;
        }

        /// <summary>
        /// // 摘要:
        //     High level representation of a workbook. This is the first object most users
        //     will construct whether they are reading or writing a workbook. It is also
        //     the top level object for creating new sheets/etc.
        /// </summary>
        static HSSFWorkbook _hssfworkbook;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="templatePaht"></param>
        static void InitializeWorkbook(HttpContext httpContext, string templatePaht)
        {
            //read the template via FileStream, it is suggested to use FileAccess.Read to prevent file lock.
            var file = new FileStream(httpContext.Server.MapPath(templatePaht), FileMode.Open, FileAccess.Read);

            _hssfworkbook = new HSSFWorkbook(file);

            //create a entry of DocumentSummaryInformation
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "www.liyanjiang.com";
            _hssfworkbook.DocumentSummaryInformation = dsi;

            //create a entry of SummaryInformation
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = "www.liyanjiang.com";
            _hssfworkbook.SummaryInformation = si;
        }
    }
}