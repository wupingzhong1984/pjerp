using System;
using System.IO;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Globalization;
using System.Text;
using Enterprise.IIS.Common;
using Enterprise.Data;
using Enterprise.Framework.EntityRepository;
using Enterprise.Framework.Enum;
using Enterprise.Framework.Utils;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using Microsoft.Office.Interop.Word;
using FineUI;

namespace Enterprise.IIS.business.Dispatch
{
    public partial class Index : PageBase
    {
        /// <summary>
        ///     排序字段
        /// </summary>
        protected string SortField
        {
            get
            {
                string sort = ViewState["SortField"] != null ? ViewState["SortField"].ToString() : "KeyId";
                return sort;
            }
            set { ViewState["SortField"] = value; }
        }

        /// <summary>
        ///     排序方向
        /// </summary>
        protected string SortDirection
        {
            get
            {
                string sort = ViewState["SortDirection"] != null ? ViewState["SortDirection"].ToString() : "DESC";
                return sort;
            }
            set { ViewState["SortDirection"] = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private DispatchService _stockOutService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected DispatchService DispatchService
        {
            get { return _stockOutService ?? (_stockOutService = new DispatchService()); }
            set { _stockOutService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private DispatchDetailsService _dispatchDetailsService;
        /// <summary>
        ///     数据服务
        /// </summary>
        protected DispatchDetailsService DispatchDetailsService
        {
            get { return _dispatchDetailsService ?? (_dispatchDetailsService = new DispatchDetailsService()); }
            set { _dispatchDetailsService = value; }
        }

        #region Protected Method

        /// <summary>
        ///     页面初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetPermissionButtons(Toolbar1);

                //单据状态
                GasHelper.DropDownListBillStatusDataBind(ddlFStatus);

                dateBegin.SelectedDate = DateTime.Now;

                dateEnd.SelectedDate = DateTime.Now;

                btnBatchDelete.ConfirmText = "你确定要执行作废操作吗？";

                BindDataGrid();
            }
        }

        /// <summary>
        ///     分页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_PageIndexChange(object sender, GridPageEventArgs e)
        {
            Grid1.PageIndex = e.NewPageIndex;
            BindDataGrid();
        }

        /// <summary>
        ///     分页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid1.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
            BindDataGrid();
        }

        /// <summary>
        ///     排序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_Sort(object sender, GridSortEventArgs e)
        {
            SortField = string.Format(@"{0}", e.SortField);
            SortDirection = e.SortDirection;
            BindDataGrid();
        }

        /// <summary>
        ///     Grid1_RowCommand
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (Grid1.SelectedRowIndexArray.Length == 0)
                {
                    Alert.Show("请至少选择一项！", MessageBoxIcon.Information);
                }
                else if (Grid1.SelectedRowIndexArray.Length > 1)
                {
                    Alert.Show("只能选择一项！", MessageBoxIcon.Information);
                }
                else
                {
                    string sid = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0].ToString();

                    if (e.CommandName == "actView")
                    {
                        PageContext.RegisterStartupScript(string.Format("openDetailsUI('{0}');", sid));
                    }
                }
            }
            catch (Exception)
            {
                Alert.Show("复制失败！", MessageBoxIcon.Warning);
            }
        }

        protected void Grid1_RowDataBound(object sender, GridRowEventArgs e)
        {
            var item = e.DataItem as LHDispatch;

            if (item != null)
                if (item.FFlag == 0)
                {
                    e.Values[3] = String.Format("<span class=\"{0}\">{1}</span>", "colorred", Convert.ToDateTime(item.FDate).ToString("yyyy-MM-dd"));
                    //e.Values[4] = String.Format("<span class=\"{0}\">{1}</span>", "colorred", item.FName);
                }
        }


        /// <summary>
        ///     作废
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBatchDelete_Click(object sender, EventArgs e)
        {
            IEnumerable<string> selectIds = GetSelectIds();

            try
            {
                //
                foreach (var ids in selectIds)
                {
                    Log(string.Format(@"作废单据号:{0}成功。", ids));

                    var status = new LHBillStatus();
                    status.FCompanyId = CurrentUser.AccountComId;
                    status.FDeptId = CurrentUser.AccountOrgId;
                    status.FDate = DateTime.Now;
                    status.FOperator = CurrentUser.AccountName;
                    status.FActionName = EnumDescription.GetFieldText(GasEnumBillStauts.Voided);
                    status.KeyId = ids;
                    status.FMemo = string.Format("单据号{0}被{1}作废处理。", ids, CurrentUser.AccountName);

                    GasHelper.AddBillStatus(status);

                    var parms = new Dictionary<string, object>();
                    parms.Clear();
                    parms.Add("@KeyId", ids);
                    parms.Add("@companyId", CurrentUser.AccountComId);
                    parms.Add("@Bill", 52);
                    SqlService.ExecuteProcedureNonQuery("proc_DeleteFlag", parms);
                }

                DispatchService.Update(p => p.FCompanyId == CurrentUser.AccountComId && selectIds.Contains(p.KeyId), p => new LHDispatch
                {
                    FFlag = 0, //
                    FStatus = Convert.ToInt32(GasEnumBillStauts.Voided), //
                    FProgress = Convert.ToInt32(GasEnumBillStauts.Voided)
                });

                Alert.Show("作废成功！", MessageBoxIcon.Information);

                BindDataGrid();
            }
            catch (Exception)
            {
                Alert.Show("作废失败！", MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        ///     复制单据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCopy_Click(object sender, EventArgs e)
        {
            try
            {
                if (Grid1.SelectedRowIndexArray.Length == 0)
                {
                    Alert.Show("请至少选择一项！", MessageBoxIcon.Information);
                }
                else if (Grid1.SelectedRowIndexArray.Length > 1)
                {
                    Alert.Show("只能选择一项！", MessageBoxIcon.Information);
                }
                else
                {
                    string sid = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0].ToString();

                    var parms = new Dictionary<string, object>();
                    parms.Clear();

                    parms.Add("@keyid", sid);
                    parms.Add("@Date", ServiceDateTime.ToShortDateString());
                    parms.Add("@companyId", CurrentUser.AccountComId);
                    string keyId = SqlService.ExecuteProcedureCommand("proc_CopySales", parms).Tables[0].Rows[0][0].ToString();

                    Alert.Show(string.Format("复制单据完成，新单据号：{0}", keyId), MessageBoxIcon.Information);

                    BindDataGrid();
                }
            }
            catch (Exception)
            {
                Alert.Show("复制失败！", MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        ///     审核单据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAudit_Click(object sender, EventArgs e)
        {
            try
            {
                if (Grid1.SelectedRowIndexArray.Length == 0)
                {
                    Alert.Show("请至少选择一项！", MessageBoxIcon.Information);
                }
                else if (Grid1.SelectedRowIndexArray.Length > 1)
                {
                    Alert.Show("只能选择一项！", MessageBoxIcon.Information);
                }
                else
                {
                    string sid = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0].ToString();

                    PageContext.RegisterStartupScript(
                        Window2.GetShowReference(string.Format("../../Common/WinAudit.aspx?KeyId={0}&action=7&Bill=1",
                            sid), "审核"));
                }
            }
            catch (Exception)
            {
                Alert.Show("复制失败！", MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        ///     编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (Grid1.SelectedRowIndexArray.Length == 0)
                {
                    Alert.Show("请至少选择一项！", MessageBoxIcon.Information);
                }
                else if (Grid1.SelectedRowIndexArray.Length > 1)
                {
                    Alert.Show("只能选择一项！", MessageBoxIcon.Information);
                }
                else
                {
                    string sid = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0].ToString();

                    PageContext.RegisterStartupScript(string.Format("openEditUI('{0}');", sid));
                }
            }
            catch (Exception)
            {
                Alert.Show("编辑失败！", MessageBoxIcon.Warning);
            }
        }


        /// <summary>
        ///     上传
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnData_Click(object sender, EventArgs e)
        {
            try
            {
                if (Grid1.SelectedRowIndexArray.Length == 0)
                {
                    Alert.Show("请至少选择一项！", MessageBoxIcon.Information);
                }
                else if (Grid1.SelectedRowIndexArray.Length > 1)
                {
                    Alert.Show("只能选择一项！", MessageBoxIcon.Information);
                }
                else
                {
                    //验证是否已作废
                    string flag = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][1].ToString();
                    if (flag.Equals("0"))
                    {
                        Alert.Show("单据已作废，不允许上传", MessageBoxIcon.Information);
                        return;
                    }

                    //验证是否已提交
                    string t6Status = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][2].ToString();
                    if (t6Status.Equals("已同步"))
                    {
                        Alert.Show("单据已同步，不允许再次上传", MessageBoxIcon.Information);
                        return;
                    }

                    string sid = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0].ToString();

                    var inInterface = new T6Interface();
                    Alert.Show(inInterface.SubmitSales(sid, CurrentUser.AccountComId), "消息提示", MessageBoxIcon.Information);

                    BindDataGrid();
                }
            }
            catch (Exception)
            {
                Alert.Show("同步失败！", MessageBoxIcon.Warning);
            }
        }


        /// <summary>
        ///     详情
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDetail_Click(object sender, EventArgs e)
        {
            try
            {
                if (Grid1.SelectedRowIndexArray.Length == 0)
                {
                    Alert.Show("请至少选择一项！", MessageBoxIcon.Information);
                }
                else if (Grid1.SelectedRowIndexArray.Length > 1)
                {
                    Alert.Show("只能选择一项！", MessageBoxIcon.Information);
                }
                else
                {
                    string sid = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0].ToString();

                    PageContext.RegisterStartupScript(string.Format("openDetailsUI('{0}');", sid));
                }
            }
            catch (Exception)
            {
                Alert.Show("编辑失败！", MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        ///     打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (Grid1.SelectedRowIndexArray.Length == 0)
                {
                    Alert.Show("请至少选择一项！", MessageBoxIcon.Information);
                }
                else if (Grid1.SelectedRowIndexArray.Length > 1)
                {
                    Alert.Show("只能选择一项！", MessageBoxIcon.Information);
                }
                else
                {
                    string sid = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0].ToString();

                    PageContext.RegisterStartupScript(string.Format("LodopPrinter('{0}');", sid));
                }
            }
            catch (Exception)
            {
                Alert.Show("打印失败！", MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        ///     打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPrintSwap_Click(object sender, EventArgs e)
        {
            try
            {
                if (Grid1.SelectedRowIndexArray.Length == 0)
                {
                    Alert.Show("请至少选择一项！", MessageBoxIcon.Information);
                }
                else if (Grid1.SelectedRowIndexArray.Length > 1)
                {
                    Alert.Show("只能选择一项！", MessageBoxIcon.Information);
                }
                else
                {
                    string sid = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0].ToString();

                    PageContext.RegisterStartupScript(string.Format("LodopPrinterSwap('{0}');", sid));
                }
            }
            catch (Exception)
            {
                Alert.Show("打印失败！", MessageBoxIcon.Warning);
            }
        }

        protected void btnPrintBlank_Click(object sender, EventArgs e)
        {
            try
            {
                PageContext.RegisterStartupScript(string.Format("LodopPrinterBlank();"));
            }
            catch (Exception)
            {
                Alert.Show("打印失败！", MessageBoxIcon.Warning);
            }
        }
        /// <summary>
        ///     查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Grid1.PageIndex = 0;
            BindDataGrid();
        }



        #region 导出WORD
        /// <summary>
        ///     书签集合
        /// </summary>
        public class WokdTags
        {
            /// <summary>
            ///   书签集合    
            /// </summary>
            public List<string> ListTags { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        public class AgentInfo
        {
            //"Logistics", "FWorkQty", "FStopQty", "Dispatcher", "FRepairQty", "FAccidentQty", "FOtherQty",
            //"FIDepartureTime", "FIRange", "FIQty", "FODepartureTime", "FORange", "FOQty", "FHeavyTruckQty", "FSumQty", "FTransport", "FTurnover"

            public string Logistics { get; set; }
            public string FWorkQty { get; set; }
            public string FStopQty { get; set; }
            public string Dispatcher { get; set; }
            public string FRepairQty { get; set; }
            public string FAccidentQty { get; set; }
            public string FOtherQty { get; set; }
            public string FIDepartureTime { get; set; }
            public string FIRange { get; set; }
            public string FIQty { get; set; }
            public string FODepartureTime { get; set; }
            public string FORange { get; set; }
            public string FOQty { get; set; }
            public string FHeavyTruckQty { get; set; }
            public string FSumQty { get; set; }
            public string FTransport { get; set; }
            public string FTurnover { get; set; }

        }

        /// <summary>
        /// 替换内容并导出
        /// </summary>
        /// <param name="templetePathandName">模板路径</param>
        /// <param name="saasPathandFile">保存路径</param>
        /// <param name="tags">书签类</param>
        /// <param name="agentinfo">数据实体类</param>
        /// <returns></returns>
        public void ExportWordForTemplete(string templetePathandName, string saasPathandFile, WokdTags tags, AgentInfo agentinfo)
        {
            //生成WORD程序对象和WORD文档对象 
            string pTemplatePath = templetePathandName; //例如"/templete.doc";
            var appWord = new Application();
            var doc = new Document();

            //打开模板文档，并指定doc的文档类型 
            object filename = "";
            try
            {
                object objTemplate = Server.MapPath(pTemplatePath);
                object objDocType = WdDocumentType.wdTypeDocument;
                object objFalse = false, objTrue = true;
                doc = appWord.Documents.Add(ref objTemplate, ref objFalse, ref objDocType, ref objTrue);
                //获取模板中所有的书签 
                //循环所有的书签，并给书签赋值 
                for (int oIndex = 0; oIndex < tags.ListTags.Count; oIndex++)
                {
                    object obDdName = tags.ListTags[oIndex];

                    string text = GetProperties(agentinfo, (string)obDdName);

                    doc.Bookmarks.get_Item(ref obDdName).Range.Text = text;
                }
                // 生成word，将当前的文档对象另存为指定的路径，然后关闭doc对象。关闭应用程序 
                filename = Server.MapPath(saasPathandFile);
                object miss = Missing.Value;
                doc.SaveAs(ref filename, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss);
                object missingValue = Type.Missing;
                object doNotSaveChanges = WdSaveOptions.wdDoNotSaveChanges;
                doc.Close(ref doNotSaveChanges, ref missingValue, ref missingValue);
                appWord.Application.Quit(ref miss, ref miss, ref miss);
                doc = null;
                appWord = null;

            }
            catch (System.Threading.ThreadAbortException)
            {
                object miss = Missing.Value;
                object missingValue = Type.Missing;
                object doNotSaveChanges = WdSaveOptions.wdDoNotSaveChanges;
                if (doc != null) doc.Close(ref doNotSaveChanges, ref missingValue, ref missingValue);
                if (appWord != null) appWord.Application.Quit(ref miss, ref miss, ref miss);
            }

            //导出
            string file = filename.ToString();
            var fi = new FileInfo(file);
            Response.Clear();
            Response.ClearHeaders();
            Response.Buffer = false;
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(Path.GetFileName(file), Encoding.UTF8));
            Response.AppendHeader("Content-Length", fi.Length.ToString(CultureInfo.InvariantCulture));
            Response.ContentType = "application/octet-stream";
            Response.WriteFile(file);
            Response.Flush();
            //Response.End();

            Response.Redirect(saasPathandFile, false);
        }
        /// <summary>
        /// 根据实体取得属性的值。
        /// </summary>
        /// <param name="ainfo"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetProperties(AgentInfo ainfo, string key)
        {
            string tStr = string.Empty;
            if (ainfo == null)
            {
                return tStr;
            }
            var properties = ainfo.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

            if (properties.Length <= 0)
            {
                return tStr;
            }
            foreach (PropertyInfo item in properties)
            {
                string name = item.Name;
                object value = item.GetValue(ainfo, null);
                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    if (name.ToLower() == key)
                    {
                        tStr = (string)value;
                        break;
                    }
                }
            }
            return tStr;
        }
        #endregion


        /// <summary>
        /// 导出本页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (Grid1.SelectedRowIndexArray.Length == 0)
                {
                    Alert.Show("请至少选择一项！", MessageBoxIcon.Information);
                }
                else if (Grid1.SelectedRowIndexArray.Length > 1)
                {
                    Alert.Show("只能选择一条进行打印！", MessageBoxIcon.Information);
                }
                else
                {

                    string keyid = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0].ToString();

                    var stock = DispatchService.FirstOrDefault(p => p.KeyId == keyid && p.FCompanyId == Company.id);
                    var print = new StringBuilder();

                    string filePath = string.Empty;

                    if (stock != null)
                    {
                        #region 最终方案

                        var parms = new Dictionary<string, object>();
                        parms.Clear();

                        //parms.Add("2|1", "上海坦申物流有限公司");
                        parms.Add("1|11", Convert.ToDateTime(stock.FDate).ToString("yyyy-MM-dd"));
                        //parms.Add("2|12", Convert.ToDateTime(stock.FDate).ToString("MM") + "月");
                        //parms.Add("2|13", Convert.ToDateTime(stock.FDate).ToString("dd"));
                        //parms.Add("2|21", stock.FDispatcher);

                        //第四行
                        parms.Add("3|4", stock.FWorkQty);
                        parms.Add("3|7", stock.FRepairQty);
                        parms.Add("3|11", stock.FIDepartureTime);
                        parms.Add("3|17", stock.FODepartureTime);//市外
                        parms.Add("2|21", stock.FHeavyTruckQty);//重车行程
                        parms.Add("2|23", stock.FTransport);//货运量吨

                        //第五行
                        parms.Add("4|4", stock.FStopQty);
                        parms.Add("4|7", stock.FAccidentQty);//事故
                        parms.Add("4|11", stock.FIRange);
                        parms.Add("4|17", stock.FORange);//市外里程
                        parms.Add("4|21", stock.FSumQty);//重车行程
                        parms.Add("4|23", stock.FTurnover);//货运量吨

                        //第六行
                        parms.Add("5|7", stock.FOtherQty);//事故
                        parms.Add("5|11", stock.FIQty);
                        parms.Add("5|17", stock.FOQty);//市外里程
                        parms.Add("5|21", stock.FOQty);//重车行程

                        //Data
                        var requestParms = new Dictionary<string, object>();
                        requestParms.Clear();

                        requestParms.Add("@KeyId", keyid);

                        var data = SqlService.ExecuteProcedureCommand("proc_DispatchDoc", requestParms);

                        NpoiUtility.ExportExcel2("LH", 7, Context, parms, data.Tables[0], //
                            Common.Template.DispatchDoc, out filePath);

                        Response.Redirect(string.Format(@"{0}", filePath), false);
                        #endregion

                        #region 方案1
                        //                        #region 表头

                        //                        print.AppendFormat(@"<!DOCTYPE html>
                        //<html lang='en'>
                        //<head>
                        //<meta charset='UTF-8'/>
                        //<title>危险货物道路运输车辆调度日志</title>
                        //</head>
                        //<body>
                        //<div style='width:350px;height:330;border:0px solid red;table:min-width:100%;td:min-width:100px;overflow:auto;display:block;'>
                        //    <table width='350px' height='330px' border='1' style='table-layout:fixed;'>
                        //        <tr>
                        //            <td style='width:5px;word-wrap:break-word;'>表22：</td>
                        //            <td colspan='18' align='center'><strong>危险货物道路运输车辆调度日志</strong></td>
                        //        </tr>
                        //        <tr>
                        //            <td height='26' align='center' style='width:5px;word-wrap:break-word;'>单位</td>
                        //            <td colspan='3' align='center'>{0}</td>
                        //            <td width='45' align='center'>日期</td>
                        //            <td colspan='9' align='center'>{1}</td>
                        //            <td width='53' align='center'>调度</td>
                        //            <td colspan='4' align='center'>{2}</td>
                        //        </tr>
                        //        <tr>
                        //            <td rowspan='4' align='center' style='width:5px;word-wrap:break-word;'>车辆技术状况</td>
                        //            <td height='26' colspan='3' align='center'>完好车数</td>
                        //            <td colspan='3' align='center'>非完好车数</td>
                        //            <td width='78' rowspan='4' align='center'>车辆运行状况</td>
                        //            <td colspan='2' align='center'>市内运行</td>
                        //            <td colspan='2' align='center'>外省市运行</td>
                        //            <td width='85' rowspan='4' align='center'>运输量统计</td>
                        //            <td width='75' rowspan='2' align='center'>重车行程</td>
                        //            <td rowspan='2' align='center'>&nbsp;</td>
                        //            <td width='99' rowspan='2' align='center'>&nbsp;</td>
                        //            <td width='152' rowspan='2' align='center'>货物运量（吨）</td>
                        //            <td width='137' rowspan='2' align='center'>&nbsp;</td>
                        //            <td width='70' rowspan='4' align='center'>&nbsp;</td>
                        //        </tr>
                        //        <tr>
                        //            <td width='102' rowspan='3' align='center'>其中</td>
                        //            <td width='90' align='center'>工作（辆）</td>
                        //            <td width='40' align='center'>{3}</td>
                        //            <td rowspan='3' align='center'>其中</td>
                        //            <td width='99' align='center'>修理（辆）</td>
                        //            <td width='67' align='center'>{4}</td>
                        //            <td width='58' align='center'>辆次</td>
                        //            <td width='44' align='center'>{5}</td>
                        //            <td width='67' align='center'>辆次</td>
                        //            <td width='67' align='center'>{6}</td>
                        //        </tr>
                        //        <tr>
                        //            <td align='center'>停驶（辆）</td>
                        //            <td align='center'>{7}</td>
                        //            <td align='center'>事故（辆）</td>
                        //            <td align='center'>{8}</td>
                        //            <td align='center'>里程</td>
                        //            <td align='center'>{9}</td>
                        //            <td align='center'>里程</td>
                        //            <td align='center'>{10}</td>
                        //            <td rowspan='2' align='center'>总行程</td>
                        //            <td rowspan='2' align='center'>{11}</td>
                        //            <td rowspan='2' align='center'>&nbsp;</td>
                        //            <td rowspan='2' align='center'>货运周转量（吨公里）</td>
                        //            <td rowspan='2' align='center'>{12}</td>
                        //        </tr>
                        //        <tr>
                        //            <td align='center'>&nbsp;</td>
                        //            <td align='center'>&nbsp;</td>
                        //            <td align='center'>其他（辆）</td>
                        //            <td align='center'>{13}</td>
                        //            <td align='center'>货运量</td>
                        //            <td align='center'>{14}</td>
                        //            <td align='center'>货运量</td>
                        //            <td align='center'>{15}</td>
                        //        </tr><tr>
                        //            <td rowspan='11' align='center'><div style='width:5px;word-wrap:break-word;'>单车 实际 调度 记录</div></td>
                        //            <td align='center'>序号</td>
                        //            <td align='center'>车号</td>
                        //            <td align='center'>车型</td>
                        //            <td align='center'>吨位</td>
                        //            <td align='center'>营运证号</td>
                        //            <td align='center'>危险类别</td>
                        //            <td align='center'>货物名称</td>
                        //            <td align='center'>包装</td>
                        //            <td align='center'>件数</td>
                        //            <td align='center'>实载吨位</td>
                        //            <td align='center'>运行起点</td>
                        //            <td align='center'>运行止点</td>
                        //            <td align='center'>里程</td>
                        //            <td align='center'>驾驶员</td>
                        //            <td align='center'>押运员</td>
                        //            <td align='center'>出车车次</td>
                        //            <td align='center'>托运单位（托运人）</td>
                        //            <td rowspan='11' align='center'>
                        //                <table cellspacing='0' cellpadding='0'>
                        //                    <col width='64' />
                        //                    <tr>
                        //                        <td width='64' rowspan='12' valign='top'>工作记事</td>
                        //                    </tr>
                        //                    <tr> </tr>
                        //                    <tr> </tr>
                        //                    <tr> </tr>
                        //                    <tr> </tr>
                        //                    <tr> </tr>
                        //                    <tr> </tr>
                        //                    <tr> </tr>
                        //                    <tr> </tr>
                        //                    <tr> </tr>
                        //                    <tr> </tr>
                        //                    <tr> </tr>
                        //                </table>
                        //            </td>
                        //        </tr>",
                        //                stock.FLogisticsName,//0
                        //                Convert.ToDateTime(stock.FDate).ToString("yyyy-MM-dd"),//1
                        //                stock.FDispatcher,//2
                        //                stock.FWorkQty,//3
                        //                stock.FRepairQty,//4
                        //                stock.FIDepartureTime,//5
                        //                stock.FODepartureTime,//6
                        //                stock.FStopQty,//7
                        //                stock.FAccidentQty,//8
                        //                stock.FIRange,//9
                        //                stock.FORange,//10
                        //                stock.FSumQty,//11
                        //                stock.FTurnover,//12
                        //                stock.FOtherQty,//13
                        //                stock.FIQty,//14
                        //                stock.FOQty,//15
                        //                stock.FHeavyTruckQty,
                        //                stock.FTransport
                        //                 );
                        //                        #endregion

                        //                        #region 表体
                        //                        var list = DispatchDetailsService.Where(p => p.KeyId == keyid);

                        //                        int i = 1;

                        //                        foreach (var item in list)
                        //                        {
                        //                            print.AppendFormat(@"<tr>
                        //            <td align='center' width='5px'>{0} </td>
                        //            <td align='center'>{1}</td>
                        //            <td align='center'>{2}</td>
                        //            <td align='center'>{3}</td>
                        //            <td align='center'>{4}</td>
                        //            <td align='center'>{5}</td>
                        //            <td align='center'>{6}</td>
                        //            <td align='center'>{7}</td>
                        //            <td align='center'>{8}</td>
                        //            <td align='center'>{9}</td>
                        //            <td align='center'>{10}</td>
                        //            <td align='center'>{11}</td>
                        //            <td align='center'>{12}</td>
                        //            <td align='center'>{13}</td>
                        //            <td align='center'>{14}</td>
                        //            <td align='center'>{15}</td>
                        //            <td align='center'>{16}</td>
                        //        </tr>",
                        //                i++,//序号0
                        //                item.FVehicleNum,//车辆1
                        //                item.FVehicleType,//车型2
                        //                item.FTonnage,//吨位3
                        //                item.FOperationCertificateNo,//证号4
                        //                item.FRiskType,//5
                        //                item.FItem,//6
                        //                "",//包7
                        //                item.FNumber,//8
                        //                item.FActual,//9
                        //                item.FFrom,//10
                        //                item.FTo,//11
                        //                item.FMileage,//12
                        //                item.FDriver,//13
                        //                item.FSupercargo,//14
                        //                item.FTimes,//15
                        //                item.FLogistics//,//16
                        //                //""
                        //        );
                        //                        }

                        //                        if (i < 10)
                        //                        {
                        //                            int m = 10 - i;
                        //                            for (int o = 0; o <= m; o++)
                        //                            {
                        //                                print.AppendFormat(@"<tr>
                        //            <td align='center'>{0}</td>
                        //            <td align='center'>&nbsp;</td>
                        //            <td align='center'>&nbsp;</td>
                        //            <td align='center'>&nbsp;</td>
                        //            <td align='center'>&nbsp;</td>
                        //            <td align='center'>&nbsp;</td>
                        //            <td align='center'>&nbsp;</td>
                        //            <td align='center'>&nbsp;</td>
                        //            <td align='center'>&nbsp;</td>
                        //            <td align='center'>&nbsp;</td>
                        //            <td align='center'>&nbsp;</td>
                        //            <td align='center'>&nbsp;</td>
                        //            <td align='center'>&nbsp;</td>
                        //            <td align='center'>&nbsp;</td>
                        //            <td align='center'>&nbsp;</td>
                        //            <td align='center'>&nbsp;</td>
                        //            <td align='center'>&nbsp;</td>
                        //        </tr>", i++);
                        //                            }
                        //                        }


                        //                        #endregion

                        //                        #region 表尾

                        //                        print.AppendFormat(@"</div></table>
                        //</body >
                        //</html > ");

                        //                        #endregion

                        #endregion
                    }

                    //Response.ClearContent();
                    //Response.AddHeader("content-disposition", string.Format(@"attachment; filename=危险货物道路运输车辆调度日志{0}.xls", SequenceGuid.GetGuidReplace()));
                    //Response.ContentType = "application/excel";
                    //Response.ContentEncoding = Encoding.UTF8;
                    //Response.Write(print);
                    //Response.End();


                    #region Word 方案2
                    //var tags = new WokdTags();
                    //var marks = new List<string> { "Logistics", "FWorkQty", "FStopQty", "Dispatcher", "FRepairQty", "FAccidentQty", "FOtherQty", "FIDepartureTime", "FIRange", "FIQty", "FODepartureTime", "FORange", "FOQty", "FHeavyTruckQty", "FSumQty", "FTransport", "FTurnover" };
                    //tags.ListTags = marks;

                    //var dispatch = DispatchService.Where(p => p.KeyId == sid).FirstOrDefault();

                    //if (dispatch != null)
                    //{
                    //    //"Logistics", "FWorkQty", "FStopQty", "Dispatcher", "FRepairQty", "FAccidentQty", 
                    //    //"FOtherQty",
                    //    //"FIDepartureTime", "FIRange", "FIQty", "FODepartureTime", "FORange", "FOQty",
                    //    // "FHeavyTruckQty", "FSumQty", "FTransport", "FTurnover"

                    //    var info = new AgentInfo
                    //    {
                    //        Logistics = dispatch.FLogistics,
                    //        FWorkQty = dispatch.FWorkQty.ToString(),
                    //        FStopQty = dispatch.FStopQty.ToString(),
                    //        Dispatcher = dispatch.FDispatcher.ToString(),
                    //        FRepairQty = dispatch.FRepairQty.ToString(),
                    //        FAccidentQty = dispatch.FAccidentQty.ToString(),
                    //        FOtherQty = dispatch.FOtherQty.ToString(),
                    //        FIDepartureTime = dispatch.FIDepartureTime.ToString(),
                    //        FIRange = dispatch.FIRange.ToString(),
                    //        FIQty = dispatch.FIQty.ToString(),
                    //        FODepartureTime = dispatch.FODepartureTime.ToString(),
                    //        FORange = dispatch.FORange.ToString(),
                    //        FOQty = dispatch.FOQty.ToString(),
                    //        FHeavyTruckQty = dispatch.FOQty.ToString(),
                    //        FSumQty = dispatch.FSumQty.ToString(),
                    //        FTransport = dispatch.FTransport.ToString(),
                    //        FTurnover = dispatch.FTurnover.ToString(),

                    //    };

                    //    ExportWordForTemplete("~/business/template/表22危险货物道路运输车辆调度日志.doc", //
                    //        string.Format(@"/upload/{0}.doc", Guid.NewGuid().ToString().Replace("-", "")), tags, info);
                    //}

                    #endregion
                }
            }
            catch (Exception ex)
            {
                Alert.Show("导出失败！", MessageBoxIcon.Warning);
            }

            //Response.ClearContent();
            //Response.AddHeader("content-disposition", string.Format(@"attachment; filename=发货单{0}.xls", SequenceGuid.GetGuidReplace()));
            //Response.ContentType = "application/excel";
            //Response.ContentEncoding = Encoding.UTF8;
            //Response.Write(Utils.GetGridTableHtml(Grid1));
            //Response.End();
        }

        /// <summary>
        /// 导出本页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnXml_Click(object sender, EventArgs e)
        {
            try
            {
                if (Grid1.SelectedRowIndexArray.Length == 0)
                {
                    Alert.Show("请至少选择一项！", MessageBoxIcon.Information);
                }
                else if (Grid1.SelectedRowIndexArray.Length > 1)
                {
                    Alert.Show("只能选择一项！", MessageBoxIcon.Information);
                }
                else
                {
                    string sid = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0].ToString();

                    var xml = new StringBuilder();

                    xml.AppendFormat(@"<?xml version='1.0' encoding='UTF-8' standalone='yes'?><DATA>");

                    var dispatch = DispatchService.Where(p => p.KeyId == sid).FirstOrDefault();

                    if (dispatch != null)
                    {
                        var parms = new Dictionary<string, object>();
                        parms.Clear();

                        parms.Add("@KeyId", sid);
                        var data = SqlService.ExecuteProcedureCommand("proc_DispachXml", parms).Tables[0];

                        for (int i = 0; i < data.Rows.Count; i++)
                        {
                            xml.AppendFormat(@"<SHEET><VHCL_TYPE>{0}</VHCL_TYPE><VHCL_NO_1>{1}</VHCL_NO_1><VHCL_NO_2>{2}</VHCL_NO_2><VHCL_DONS>{3}</VHCL_DONS><VHCL_SLOTS>{4}</VHCL_SLOTS><VHCL_MODEL>{5}</VHCL_MODEL><COMP_NAME>{6}</COMP_NAME>
<COMP_LICENSE>{7}</COMP_LICENSE><OUTWARD>{8}</OUTWARD><DRIVER>{9}</DRIVER><DRIVER_LICENSE>{10}</DRIVER_LICENSE><DRIVER_PHONE>{11}</DRIVER_PHONE><DRIVER_JOB>{12}</DRIVER_JOB><SUPERCARGO>{13}</SUPERCARGO><SUPERCARGO_LICENSE>{14}</SUPERCARGO_LICENSE>
<SUPERCARGO_PHONE>{15}</SUPERCARGO_PHONE><SUPERCARGO_JOB>{16}</SUPERCARGO_JOB><START_DATE>{17}</START_DATE><END_DATE>{18}</END_DATE><OUT_ADDRESS>{19}</OUT_ADDRESS><IN_ADDRESS>{20}</IN_ADDRESS><OUT_MILAGE>{21}</OUT_MILAGE><REMARK>{22}</REMARK><OIL_TYPE>{23}</OIL_TYPE>
<OIL_WEAR>{24}</OIL_WEAR><TRANSPORTATION_COST>{25}</TRANSPORTATION_COST><ITEM><LOAD_PROVINCE>{26}</LOAD_PROVINCE><LOAD_CITY>{27}</LOAD_CITY><LOAD_AREA>{28}</LOAD_AREA><LOAD_PORT>{29}</LOAD_PORT><LOAD_COMPNAME>{30}</LOAD_COMPNAME><LOAD_ADDRESS>{31}</LOAD_ADDRESS><LOAD_MAN>{32}</LOAD_MAN>
<LOAD_PHONE>{33}</LOAD_PHONE><MILAGE>{34}</MILAGE><GOODS_TYPE>{35}</GOODS_TYPE><GOODS_NAME>{36}</GOODS_NAME><GOODS_WEIGHT>{37}</GOODS_WEIGHT><TRANSPORT_NUM>{38}</TRANSPORT_NUM><CHEMICALS_CATEGORY>{39}</CHEMICALS_CATEGORY><LOADDOWN_NUM>{40}</LOADDOWN_NUM><DOWN_PROVINCE>{41}</DOWN_PROVINCE>
<DOWN_CITY>{42}</DOWN_CITY><DOWN_AREA>{43}</DOWN_AREA><DOWN_PORT>{44}</DOWN_PORT><DOWN_COMPNAME>{45}</DOWN_COMPNAME><DOWN_ADDRESS>{46}</DOWN_ADDRESS><DOWN_MAN>{47}</DOWN_MAN><DOWN_PHONE>{48}</DOWN_PHONE><DOWN_MILAGE>{49}</DOWN_MILAGE><DOWN_GOODS_TYPE>{50}</DOWN_GOODS_TYPE>
<DOWN_GOODS_NAME>{51}</DOWN_GOODS_NAME><DOWN_GOODS_WEIGHT>{52}</DOWN_GOODS_WEIGHT></ITEM></SHEET>",//
                          2,//VHCL_TYPE
                          data.Rows[i]["FVehicleNum"],//VHCL_NO_1
                          data.Rows[i]["FTrailerPlate"].ToString().Equals("-1") ? "" : data.Rows[i]["FTrailerPlate"],//VHCL_NO_2
                          data.Rows[i]["FTonner"],//VHCL_DONS
                          0,//VHCL_SLOTS//箱位4
                          "牵引车头",//VHCL_MODEL
                          "上海坦申物流有限公司",
                          "沪市310000005297",
                          "1",
                          data.Rows[i]["FDriver"],//DRIVER //9
                          data.Rows[i]["FDriverNo"],//DRIVER_LICENSE
                          data.Rows[i]["FDriverPhone"],//DRIVER_PHONE
                          "道路危险货物运输驾驶人员",//DRIVER_JOB
                          data.Rows[i]["FSupercargo"],//DRIVER
                          data.Rows[i]["FSupercargoNo"],//DRIVER_LICENSE
                          data.Rows[i]["FSupercargoPhone"],//DRIVER_PHONE
                          "道路危险货物运输押运人员",//DRIVER_JOB
                          data.Rows[i]["FBeginDate"],//TART_DATE
                          data.Rows[i]["FEndDate"],//END_DATE
                          data.Rows[i]["FAddress"],//OUT_ADDRESS
                          data.Rows[i]["FAddress"],//IN_ADDRESS
                          "0",//OUT_MILAGE
                          "",//REMARK
                          "1",//OIL_TYPE
                          "262",//OIL_WEAR
                          "3335",//TRANSPORTATION_COST 运费
                          "上海市",
                          "奉贤区",
                          "外环以外",
                          "",
                          "上海浦江特种气体有限公司",
                          "化学工业区才华路10号",
                          "徐泽",
                          "17321067154",
                          data.Rows[i]["FMileage"],
                          "普危",
                          data.Rows[i]["FItem"],//GOODS_NAME
                          data.Rows[i]["FActual"],
                          "1",
                          "2",
                          "1",
                          data.Rows[i]["FProvince"],//LOAD_PROVINCE
                          data.Rows[i]["FCity"],//FCity
                          data.Rows[i]["FCounty"],//LOAD_AREA
                          "",//PORT
                          data.Rows[i]["FName"],//LOAD_AREA
                          data.Rows[i]["FAddress"],//LOAD_AREA
                          "徐泽",
                          "17321067154",
                          "100",
                          "普危",
                          data.Rows[i]["FItem"],//GOODS_NAME
                          data.Rows[i]["FActual"]//FActual
                          );
                        }
                    }

                    xml.Append("</DATA>");

                    Response.ClearContent();
                    Response.AddHeader("content-disposition", string.Format(@"attachment; filename=行车日志{0}.xml", SequenceGuid.GetGuidReplace()));
                    Response.ContentType = "text/plain";
                    Response.ContentEncoding = System.Text.Encoding.UTF8;
                    Response.Write(xml.ToString());
                    Response.End();

                }
            }
            catch (Exception ex)
            {
                Alert.Show("生成日志失败！", MessageBoxIcon.Warning);
            }
        }

        protected void Window1_Close(object sender, WindowCloseEventArgs e)
        {
            BindDataGrid();
        }
        protected void Window2_Close(object sender, WindowCloseEventArgs e)
        {
            BindDataGrid();
        }
        #endregion

        #region Private Method

        /// <summary>
        ///     获取选中的ID集合
        /// </summary>
        /// <returns></returns>
        private IEnumerable<string> GetSelectIds()
        {
            int[] selections = Grid1.SelectedRowIndexArray;

            var selectIds = new string[selections.Length];

            for (int i = 0; i < selections.Length; i++)
            {
                selectIds[i] = Grid1.DataKeys[selections[i]][0].ToString();//
            }

            return selectIds;
        }

        /// <summary>
        ///     绑定数据表格
        /// </summary>
        private void BindDataGrid()
        {
            int output;

            dynamic orderingSelector;
            Expression<Func<LHDispatch, bool>> predicate = BuildPredicate(out orderingSelector);

            //取数据源
            IQueryable<LHDispatch> list = DispatchService.Where(predicate, Grid1.PageSize, Grid1.PageIndex + 1,
                orderingSelector, EnumHelper.ParseEnumByString<OrderingOrders>(SortDirection), out output);

            //设置页面大小
            Grid1.RecordCount = output;

            //绑定数据源
            Grid1.DataSource = list;
            Grid1.DataBind();

            ddlPageSize.SelectedValue = Grid1.PageSize.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     创建查询条件表达式和排序表达式
        /// </summary>
        /// <param name="orderingSelector"></param>
        /// <returns></returns>
        private Expression<Func<LHDispatch, bool>> BuildPredicate(out dynamic orderingSelector)
        {
            // 查询条件表达式
            Expression expr = Expression.Constant(true);

            ParameterExpression parameter = Expression.Parameter(typeof(LHDispatch));
            MethodInfo methodInfo = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            expr = Expression.And(expr,
                Expression.Equal(Expression.Property(parameter, "FCompanyId"), Expression.Constant(CurrentUser.AccountComId, typeof(int))));

            // 单据类型
            int ftype = Convert.ToInt32(GasEnumBill.Dispatch);
            expr = Expression.And(expr,
               Expression.Equal(Expression.Property(parameter, "FType"), Expression.Constant(ftype, typeof(int?))));

            expr = Expression.And(expr,
               Expression.Equal(Expression.Property(parameter, "FDeleteFlag"), Expression.Constant(0, typeof(int?))));

            if (dateBegin.SelectedDate != null)
            {
                expr = Expression.And(expr,
                    Expression.GreaterThanOrEqual(Expression.Property(parameter, "FDate"),
                        Expression.Constant(dateBegin.SelectedDate, typeof(DateTime?))));
            }

            if (dateEnd.SelectedDate != null)
            {
                expr = Expression.And(expr,
                    Expression.LessThanOrEqual(Expression.Property(parameter, "FDate"),
                        Expression.Constant(dateEnd.SelectedDate, typeof(DateTime?))));
            }

            // 姓名
            if (!string.IsNullOrWhiteSpace(txtFName.Text))
            {
                expr = Expression.And(expr,
                    Expression.Call(Expression.Property(parameter, "FName"), methodInfo,
                        // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant(txtFName.Text.Trim())));
            }

            // 登陆帐号
            if (!string.IsNullOrWhiteSpace(txtFItemName.Text))
            {
                expr = Expression.And(expr,
                    Expression.Call(Expression.Property(parameter, "FCode"), methodInfo,
                        // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant(txtFItemName.Text.Trim())));
            }

            Expression<Func<LHDispatch, bool>> predicate = Expression.Lambda<Func<LHDispatch, bool>>(expr, parameter);

            // 排序表达式
            orderingSelector = Expression.Lambda(Expression.Property(parameter, SortField), parameter);

            return predicate;
        }

        #endregion

    }
}