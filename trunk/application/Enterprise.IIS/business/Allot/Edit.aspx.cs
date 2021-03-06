using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Transactions;
using Enterprise.Data;
using Enterprise.Framework.File;
using Enterprise.Framework.Web;
using Enterprise.IIS.Common;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using FineUI;
using Newtonsoft.Json.Linq;

namespace Enterprise.IIS.business.AllotTrans
{
    public partial class Edit : PageBase
    {
        #region  Service
        /// <summary>
        ///     AppendToEnd
        /// </summary>
        private bool AppendToEnd
        {
            get
            {
                return ViewState["_AppendToEnd"] != null //
                    && Convert.ToBoolean(ViewState["_AppendToEnd"]);
            }
        }

        /// <summary>
        ///     排序字段
        /// </summary>
        protected string SortField
        {
            get
            {
                string sort = ViewState["SortField"] != null //
                    ? ViewState["SortField"].ToString() : "KeyId";
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
                string sort = ViewState["SortDirection"] != null //
                    ? ViewState["SortDirection"].ToString() : "ASC";
                return sort;
            }
            set { ViewState["SortDirection"] = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private CustomerService _customerService;

        /// <summary>
        ///     数据服务
        /// </summary>
        /// 
        protected CustomerService CustomerService
        {
            get { return _customerService ?? (_customerService = new CustomerService()); }
            set { _customerService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private StockOutService _stockOutService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected StockOutService StockOutService
        {
            get { return _stockOutService ?? (_stockOutService = new StockOutService()); }
            set { _stockOutService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private PassCardService _passCardService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected PassCardService PassCardService
        {
            get { return _passCardService ?? (_passCardService = new PassCardService()); }
            set { _passCardService = value; }
        }


        /// <summary>
        ///     数据服务
        /// </summary>
        private StockOutDetailsService _stockOutDetailsService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected StockOutDetailsService StockOutDetailsService
        {
            get
            {
                return _stockOutDetailsService ?? //
              (_stockOutDetailsService = new StockOutDetailsService());
            }
            set { _stockOutDetailsService = value; }
        }

        /// <summary>
        ///     Log
        /// </summary>
        private StockOutDetailsLogService _stockOutDetailsLogService;
        /// <summary>
        ///     Log
        /// </summary>
        protected StockOutDetailsLogService StockOutDetailsLogService
        {
            get { return _stockOutDetailsLogService ?? (_stockOutDetailsLogService = new StockOutDetailsLogService()); }
            set { _stockOutDetailsLogService = value; }
        }

        /// <summary>
        ///     Log
        /// </summary>
        private StockOutLogService _stockOutLogService;
        /// <summary>
        ///     Log
        /// </summary>
        protected StockOutLogService StockOutLogService
        {
            get { return _stockOutLogService ?? (_stockOutLogService = new StockOutLogService()); }
            set { _stockOutLogService = value; }
        }

        /// <summary>
        ///     客户档案
        /// </summary>
        private LHCustomer _customer;

        /// <summary>
        ///     客户档案
        /// </summary>
        protected LHCustomer Customer
        {
            get
            {
                return _customer ?? (_customer = CustomerService.FirstOrDefault(p => p.FCode == txtFCode.Text.Trim()//
              && p.FCompanyId == CurrentUser.AccountComId));
            }
            set { _customer = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private ItemsService _itemsService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected ItemsService ItemsService
        {
            get { return _itemsService ?? (_itemsService = new ItemsService()); }
            set { _itemsService = value; }
        }

        /// <summary>
        ///     
        /// </summary>
        private LHStockOut _stockOut;

        /// <summary>
        ///     
        /// </summary>
        protected LHStockOut StockOut
        {
            get
            {
                return _stockOut ?? (_stockOut = StockOutService.FirstOrDefault(p => p.KeyId == KeyId //
              && p.FCompanyId == CurrentUser.AccountComId));
            }
            set { _stockOut = value; }
        }


        /// <summary>
        ///     数据服务
        /// </summary>
        private AttachmentService _attachmentService;

        /// <summary>
        ///     数据服务
        /// </summary>
        /// 
        protected AttachmentService AttachmentService
        {
            get { return _attachmentService ?? (_attachmentService = new AttachmentService()); }
            set { _attachmentService = value; }
        }

        /// <summary>
        ///     
        /// </summary>
        private LHAttachment _attachment;

        /// <summary>
        ///     
        /// </summary>
        protected LHAttachment Attachment
        {
            get
            {
                return _attachment ?? (_attachment = AttachmentService.FirstOrDefault(p => p.KeyId == KeyId //
                    && p.FCompanyId == CurrentUser.AccountComId));
            }
            set { _attachment = value; }
        }

        /// <summary>
        ///     FCode
        /// </summary>
        protected string KeyId
        {
            get { return Request["KeyId"]; }
        }

        /// <summary>
        ///     
        /// </summary>
        public WebAction Actions
        {
            get
            {
                string s = Convert.ToString(Request["action"]);
                return (WebAction)Int32.Parse(s);
            }
        }
        #endregion

        #region Protected Method

        /// <summary>
        ///     Page_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //初始化控件数据
                InitData();

                //加载数据
                LoadData();
            }
            else
            {
                #region 弹窗加产品
                if (GetRequestEventArgument().Contains("reloadGrid:"))
                {
                    //查找所选商品代码，查访产品集合
                    string keys = GetRequestEventArgument().Split(':')[1];

                    var values = keys.Split(',');

                    string codes = String.Empty;
                    for (int i = 0; i < values.Count(); i++)
                    {
                        codes += string.Format("'{0}',", values[i]);
                    }

                    var value = codes.Substring(0, codes.Length - 1);

                    var data = SqlService.Where(string.Format("SELECT * FROM dbo.vm_SalesItem a WHERE a.FItemCode IN ({0}) and a.FCompanyId={1}", value, CurrentUser.AccountComId));

                    if (data != null && data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0)
                    {
                        var table = data.Tables[0];
                        for (int i = 0; i < table.Rows.Count; i++)
                        {
                            var details = new LHStockOutDetails();

                            decimal price = GasHelper.GeCustomerPrice(txtFCode.Text.Trim(), //
                                table.Rows[i]["FItemCode"].ToString(), CurrentUser.AccountComId);

                            details.FItemCode = table.Rows[i]["FItemCode"].ToString();
                            details.FPrice = price;
                            details.FQty = 1;
                            details.FAmount = price;
                            details.FBottleQty = 1;
                            details.FBottleOweQty = 0;
                            details.FCompanyId = CurrentUser.AccountComId;
                            details.KeyId = txtKeyId.Text.Trim();
                            details.FCateId = Convert.ToInt32(table.Rows[i]["FId"].ToString());

                            details.FBSpec = table.Rows[i]["FBSpec"].ToString();
                            details.FRecycleQty = 0;
                            details.FReturnQty = 0;
                            details.FCostPrice = 0;
                            details.FBalance = 0;
                            details.FNum = table.Rows[i]["FINum"].ToString();//U8代码

                            if (details.FCateId == 2000)
                            {
                                //默认包装物
                                details.FBottle = table.Rows[i]["FBottle"].ToString();
                            }
                            else
                            {
                                details.FBottle = details.FItemCode;
                            }

                            StockOutDetailsService.AddEntity(details);

                            //日志
                            switch (Actions)
                            {
                                case WebAction.Add:

                                    break;
                                case WebAction.Edit:

                                    break;
                            }
                        }

                        StockOutDetailsService.SaveChanges();

                        //重新绑定值
                        BindDataGrid();
                    }
                }
                #endregion

                #region 更新合计

                if (Grid1.Rows.Count > 0)
                {
                    if (GetRequestEventArgument() == "UPDATE_SUMMARY")
                    {
                        // 页面要求重新计算合计行的值
                        OutputSummaryData();

                        //写入
                        ModifiedGrid();

                        // 为了保持前后台上传，回发更新合计行值后，必须进行数据绑定或者提交更改
                        Grid1.CommitChanges();
                    }
                }

                #endregion
            }
        }

        #region OutputSummaryData
        /// <summary>
        ///     OutputSummaryData
        /// </summary>
        private void OutputSummaryData()
        {
            decimal sumFQty = 0.00M;
            decimal sumFAmount = 0.00M;
            decimal sumFRecycleQty = 0.00M;
            decimal sumFBottleQty = 0.00M;


            foreach (JObject mergedRow in Grid1.GetMergedData())
            {
                JObject values = mergedRow.Value<JObject>("values");

                sumFQty += values.Value<decimal>("FQty");
                sumFAmount += values.Value<decimal>("FAmount");
                sumFRecycleQty += values.Value<decimal>("FRecycleQty");
                sumFBottleQty += values.Value<decimal>("FBottleQty");
            }

            JObject summary = new JObject();
            summary.Add("FQty", sumFQty);
            summary.Add("FAmount", sumFAmount);
            summary.Add("FRecycleQty", sumFRecycleQty);
            summary.Add("FBottleQty", sumFBottleQty);

            Grid1.SummaryData = summary;
        }

        #endregion

        /// <summary>
        ///     btnSubmit_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool isSucceed = false;

            try
            {
                using (var trans = new TransactionScope())
                {
                    switch (Actions)
                    {
                        case WebAction.Add:
                            isSucceed = SubmintAdd();
                            break;

                        case WebAction.Edit:
                            isSucceed = SubmintEdit();
                            break;
                    }

                    trans.Complete();
                }
            }
            catch (Exception ex)
            {
                isSucceed = false;
            }
            finally
            {
                if (isSucceed)
                {
                    PageContext.RegisterStartupScript("closeActiveTab();");
                }
                else
                {
                    Alert.Show("提交失败！", MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        ///     调度生成调拨出库单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPassCardToSales_Click(object sender, EventArgs e)
        {
            bool isSucceed = false;

            try
            {
                if (string.IsNullOrEmpty(tbxFLogisticsNumber.Text.Trim()))
                {
                    Alert.Show("请选入订单单号", MessageBoxIcon.Information);

                    return;
                }

                var pamrs = new Dictionary<string, object>();
                pamrs.Clear();

                pamrs.Add("@FCode", txtFCode.Text.Trim());
                pamrs.Add("@SalesKeyId", txtKeyId.Text.Trim());
                pamrs.Add("@FCompanyId", CurrentUser.AccountComId);
                pamrs.Add("@KeyId", tbxFLogisticsNumber.Text.Trim());

                SqlService.ExecuteProcedureCommand("proc_PassCardToSales", pamrs);

                BindDataGrid();

                isSucceed = true;
            }
            catch (Exception)
            {
                isSucceed = false;
            }
            finally
            {
                if (isSucceed)
                {
                    Alert.Show("提交成功", MessageBoxIcon.Information);
                }
                else
                {
                    Alert.Show("提交失败！", MessageBoxIcon.Error);
                }
            }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void filePhoto_FileSelected(object sender, EventArgs e)
        //{
        //    if (filePhoto.HasFile)
        //    {
        //        var fileSuffix = filePhoto.ShortFileName.Substring(filePhoto.ShortFileName.LastIndexOf('.'));

        //        var sequence = SequenceService.CreateSequence("LH", CurrentUser.AccountComId);

        //        var fileName = sequence + fileSuffix;

        //        var uploadpath = Config.GetUploadpath();

        //        var tempPath = (string.Format(@"{0}/temp/{1}/", uploadpath, DateTime.Now.ToString("yyyy-MM-dd"))); //

        //        if (!DirFile.XFileExists(Server.MapPath(tempPath)))
        //        {
        //            DirFile.XCreateDir(Server.MapPath(tempPath));
        //        }

        //        filePhoto.SaveAs(Server.MapPath(tempPath + fileName));

        //        hfdImage.Text = tempPath + fileName;

        //        // 清空文件上传组件
        //        filePhoto.Reset();
        //    }

        //}

        /// <summary>
        ///     打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                PageContext.RegisterStartupScript(string.Format("LodopPrinter('{0}');", KeyId));
            }
            catch (Exception)
            {
                Alert.Show("打印失败！", MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        ///     新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                PageContext.RegisterStartupScript(
                    Window1.GetShowReference(string.Format("../../Common/WinProduct.aspx?FCode={0}",
                        txtFCode.Text.Trim()), "客户合同价"));
            }
            catch (Exception)
            {
                Alert.Show("添加失败！", MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        ///     产品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddProduct_OnClick(object sender, EventArgs e)
        {
            try
            {
                PageContext.RegisterStartupScript(
                    Window1.GetShowReference(string.Format("../../Common/WinProduct.aspx"
                        ), "产品档案"));

                //PageContext.RegisterStartupScript(
                //    Window1.GetShowReference(string.Format("../../Common/WinProductPrice.aspx?FCode={0}",
                //        txtFCode.Text.Trim()), "客户合同价"));
            }
            catch (Exception)
            {
                Alert.Show("添加失败！", MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        ///     Copy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCopy_Click(object sender, EventArgs e)
        {
            bool isSucceed = false;

            try
            {
                var pamrs = new Dictionary<string, object>();
                pamrs.Clear();

                pamrs.Add("@code", txtFCode.Text.Trim());
                pamrs.Add("@keyid", txtKeyId.Text.Trim());
                pamrs.Add("@companyid", CurrentUser.AccountComId);

                SqlService.ExecuteProcedureCommand("proc_CopyLastSales", pamrs);

                BindDataGrid();

                isSucceed = true;
            }
            catch (Exception)
            {
                isSucceed = false;
            }
            finally
            {
                if (isSucceed)
                {
                    Alert.Show("提交成功", MessageBoxIcon.Information);
                }
                else
                {
                    Alert.Show("提交失败！", MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        ///     BindDataGrid
        /// </summary>
        private void BindDataGrid()
        {
            var source = SqlService.Where(string.Format(@"SELECT * FROM dbo.vm_SalesDetails WHERE keyId='{0}' and FCompanyId={1}", txtKeyId.Text, CurrentUser.AccountComId));

            //绑定数据源
            Grid1.DataSource = source;
            Grid1.DataBind();

            var table = source.Tables[0];

            if (table != null && table.Rows.Count > 0)
            {
                decimal sumFQty = 0.00M;
                decimal sumFAmount = 0.00M;
                decimal sumFRecycleQty = 0.00M;
                decimal sumFBottleQty = 0.00M;

                sumFQty = Convert.ToDecimal(table.Compute("sum(FQty)", "true"));
                sumFAmount = Convert.ToDecimal(table.Compute("sum(FAmount)", "true"));
                sumFRecycleQty = Convert.ToDecimal(table.Compute("sum(FRecycleQty)", "true"));
                sumFBottleQty = Convert.ToDecimal(table.Compute("sum(FBottleQty)", "true"));

                var summary = new JObject
                {
                    {"FItemCode", "合计"},
                    {"FQty", sumFQty},
                    {"FAmount", sumFAmount},
                    {"FRecycleQty", sumFRecycleQty},
                    {"sumFBottleQty", sumFBottleQty},
                };

                Grid1.SummaryData = summary;
            }
            else
            {
                var summary = new JObject
                {
                    {"FItemCode", "合计"},
                    {"FQty", 0},
                    {"FAmount", 0},
                    {"FRecycleQty", 0},
                    {"sumFBottleQty", 0},
                };

                Grid1.SummaryData = summary;
            }
        }

        /// <summary>
        ///     单元格编辑与修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_AfterEdit(object sender, GridAfterEditEventArgs e)
        {
            Window1.Hidden = true;
            Window2.Hidden = true;
            Window3.Hidden = true;

            //新增行事件
            var addList = Grid1.GetNewAddedList();

            #region AddRow
            foreach (var add in addList)
            {
                var dictValues = add.Values;

                //商品代码
                var firstOrDefault = dictValues.First();

                if (firstOrDefault != null && !string.IsNullOrEmpty(firstOrDefault.ToString()))
                {
                    DataSet dataSet = GasHelper.GetSalesItem(firstOrDefault.ToString(), CurrentUser.AccountComId);

                    DataTable table = dataSet.Tables[0];

                    if (table != null && table.Rows.Count > 0)
                    {
                        decimal price = GasHelper.GeCustomerPrice(txtFCode.Text.Trim(),//
                            table.Rows[0]["FItemCode"].ToString(), CurrentUser.AccountComId);

                        table.Rows[0]["FPrice"] = price;

                        var details = new LHStockOutDetails
                        {
                            FItemCode = table.Rows[0]["FItemCode"].ToString(),
                            FPrice = price,
                            FQty = 1,
                            FAmount = price,
                            FBottleQty = 0,
                            FBottleOweQty = 0,
                            FRecycleQty = 0,
                            FCompanyId = CurrentUser.AccountComId,
                            KeyId = txtKeyId.Text.Trim(),
                            FBottle = table.Rows[0]["FBottleCode"].ToString(),
                            FCateId = Convert.ToInt32(table.Rows[0]["FId"].ToString())
                        };

                        switch (Actions)
                        {
                            case WebAction.Add:
                                break;
                            case WebAction.Edit:
                                //记录一下当前新增人操作内容
                                //var detailslog = new LHStockOutDetails_Log
                                //{
                                //    FUpdateBy = CurrentUser.AccountName,
                                //    FUpdateDate = DateTime.Now,
                                //    FItemCode = table.Rows[0]["FItemCode"].ToString(),
                                //    FPrice = price,
                                //    FQty = 1,
                                //    FAmount = price,
                                //    FBottleQty = 0,
                                //    FCompanyId = CurrentUser.AccountComId,
                                //    FBottleOweQty = 0,
                                //    KeyId = txtKeyId.Text.Trim(),
                                //    FBottle = table.Rows[0]["FBottleCode"].ToString(),
                                //    FStatus = "新增",
                                //    FMemo = string.Format(@"时间：{0} 新增人：{1}", DateTime.Now, CurrentUser.AccountName)
                                //};

                                //StockOutDetailsLogService.Add(detailslog);

                                break;
                        }

                        StockOutDetailsService.Add(details);
                    }
                }
            }
            #endregion

            if (addList.Count > 0)
                BindDataGrid();
        }

        /// <summary>
        ///     Grid1_RowCommand
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Delete" || e.CommandName == "Add")
            {
                var datakey = Convert.ToInt32(Grid1.DataKeys[e.RowIndex][1]);

                StockOutDetailsService.Delete(p => p.FId == datakey && p.FCompanyId == CurrentUser.AccountComId);

                BindDataGrid();
            }
        }

        /// <summary>
        ///     关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Window1_Close(object sender, WindowCloseEventArgs e)
        {
        }
        protected void Window2_Close(object sender, WindowCloseEventArgs e)
        {
        }
        protected void Window3_Close(object sender, WindowCloseEventArgs e)
        {
        }
        protected void txtFAddress_OnTriggerClick(object sender, EventArgs e)
        {
            //Window1.Hidden = true;
            //Window2.Hidden = true;
        }

        protected void tbxFCustomer_OnTextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbxFCustomer.Text.Trim()))
            {

                //Window1.Hidden = true;
                //Window2.Hidden = true;
                //Window3.Hidden = true;

                var custmoer = CustomerService.Where(p => p.FName == tbxFCustomer.Text.Trim() && p.FCompanyId == CurrentUser.AccountComId).FirstOrDefault();
                if (custmoer != null)
                {
                    txtFCode.Text = custmoer.FCode;
                    //txtFAddress.Text = custmoer.FAddress.Trim();
                    //txtFFreight.Text = custmoer.FFreight.ToString();
                    //txtFLinkman.Text = custmoer.FLinkman;
                    //txtFPhone.Text = custmoer.FPhome;
                }
            }
        }

        protected void tbxFLogisticsNumber_OnTextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbxFLogisticsNumber.Text.Trim()))
            {
                var plan = PassCardService.Where(p => p.KeyId == tbxFLogisticsNumber.Text.Trim()).FirstOrDefault();

                if (plan != null)
                {
                    //配送方式
                    ddlDeliveryMethod.SelectedValue = plan.FDeliveryMethod;
                    ddlFDistributionPoint.SelectedValue = plan.FDistributionPoint;
                    ddlFSalesman.SelectedValue = plan.CreateBy;
                    ddlFPoint.SelectedValue = plan.FPoint;

                    txtFCode.Text = plan.FCode;
                    tbxFCustomer.Text = plan.FName;
                    txtCreateBy.Text = plan.CreateBy;
                    ddlFSalesman.SelectedValue = plan.CreateBy;

                    var dis = new DispatchCenterService().Where(p => p.KeyId == plan.FDispatchNum).FirstOrDefault();
                    if (dis != null)
                    {
                        if (!string.IsNullOrEmpty(dis.FDriver))
                            ddlFDriver.SelectedValueArray = dis.FDriver.Split(',');
                        //if (!string.IsNullOrEmpty(passCard.FShipper))
                        //    ddlFShipper.SelectedValueArray = passCard.FShipper.Split(',');
                        if (!string.IsNullOrEmpty(dis.FSupercargo))
                            ddlFSupercargo.SelectedValueArray = dis.FSupercargo.Split(',');
                        //if (!string.IsNullOrEmpty(StockOut.FSalesman))
                        //    ddlFSalesman.SelectedValueArray = passCard.FSalesman.Split(',');

                        ddlFVehicleNum.SelectedValue = dis.FVehicleNum;
                    }

                    var pamrs = new Dictionary<string, object>();
                    pamrs.Clear();

                    pamrs.Add("@FCode", plan.FCode);
                    pamrs.Add("@SalesKeyId", txtKeyId.Text.Trim());
                    pamrs.Add("@FCompanyId", CurrentUser.AccountComId);
                    pamrs.Add("@KeyId", tbxFLogisticsNumber.Text.Trim());

                    SqlService.ExecuteProcedureCommand("proc_PassCardToSales", pamrs);

                    BindDataGrid();
                }
            }
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tbxFItemCode_OnTriggerClick(object sender, EventArgs e)
        {
        }

        #endregion

        #region Private Method

        /// <summary>
        ///     ModifiedGrid
        /// </summary>
        private void ModifiedGrid()
        {
            //编辑行事件
            var dictModified = Grid1.GetModifiedDict();
            foreach (var rowKey in dictModified.Keys)
            {
                int datakey = Convert.ToInt32(Grid1.DataKeys[rowKey][1].ToString());

                var sKeys = new StringBuilder();
                var sValues = new StringBuilder();
                foreach (var key in dictModified[rowKey].Keys)
                {
                    sKeys.AppendFormat("{0},", key);
                }

                foreach (var dictValue in dictModified[rowKey].Values)
                {
                    sValues.AppendFormat("{0},", dictValue);
                }

                var details = StockOutDetailsService.Where(p => p.FId == datakey).FirstOrDefault();
                //写入原始，通过存储过程完成明细复制
                var parmsLog = new Dictionary<string, object>();
                parmsLog.Clear();

                parmsLog.Add("@fid", datakey);
                parmsLog.Add("@opr", CurrentUser.AccountName);
                parmsLog.Add("@companyId", CurrentUser.AccountComId);
                SqlService.ExecuteProcedureCommand("proc_StockOutDetails_Log", parmsLog);

                var keys = sKeys.ToString().Split(',');
                var values = sValues.ToString().Split(',');

                for (int i = 0; i < keys.Count(); i++)
                {
                    #region 修改内容

                    var key = keys[i];
                    var value = values[i];

                    if (!string.IsNullOrEmpty(key))
                    {
                        if (details != null)
                        {
                            if (key.Equals("FPrice"))
                            {
                                details.FPrice = Convert.ToDecimal(value);
                                details.FAmount = details.FPrice * details.FQty;
                            }
                            if (key.Equals("FQty"))
                            {
                                details.FQty = Convert.ToDecimal(value);
                                details.FBottleQty = Convert.ToInt32(details.FQty);
                                details.FAmount = details.FPrice * details.FQty;
                            }
                            if (key.Equals("FBottleName"))
                            {
                                details.FBottle = GasHelper.GetBottleCodeByName(value);
                            }
                            //商品收入数
                            if (key.Equals("FReturnQty"))
                            {
                                details.FReturnQty = Convert.ToInt32(value);
                            }

                            if (key.Equals("FRecycleQty"))
                            {
                                details.FRecycleQty = Convert.ToInt32(value);
                                //写入回空单
                            }
                            if (key.Equals("FBottleQty"))
                            {
                                int result = 0;
                                int.TryParse(value, out result);

                                details.FBottleQty = result;
                            }
                            if (key.Equals("FT6Warehouse"))
                            {
                                details.FT6WarehouseNum = GasHelper.GetWarehouseByName(value);
                            }
                            if (key.Equals("FMemo"))
                            {
                                details.FMemo = value;
                            }
                            if (key.Equals("FNum"))
                            {
                                details.FNum = value;
                            }

                            var detailslog = new LHStockOutDetails_Log
                            {
                                FUpdateBy = CurrentUser.AccountName,
                                FUpdateDate = DateTime.Now,
                                FItemCode = details.FItemCode,
                                FPrice = details.FPrice,
                                FQty = details.FQty,
                                FAmount = details.FAmount,
                                FBottleQty = details.FBottleQty,
                                FBottleOweQty = details.FBottleOweQty,
                                KeyId = details.KeyId,
                                FBottle = details.FBottle,
                                FCompanyId = CurrentUser.AccountComId,
                                FRecycleQty = details.FRecycleQty,
                                FStatus = "变更",
                                FMemo = string.Format(@"时间：{0} 变更人：{1}", DateTime.Now, CurrentUser.AccountName)
                            };

                            StockOutDetailsLogService.Add(detailslog);
                        }
                    }

                    #endregion
                }

                StockOutDetailsService.SaveChanges();
            }
        }

        /// <summary>
        ///     提交编辑
        /// </summary>
        private bool SubmintEdit()
        {
            if (StockOut != null)
            {
                ModifiedGrid();
                /////////////////////////////////////////////////////////////////////////////

                StockOut.FCode = txtFCode.Text;
                StockOut.FName = tbxFCustomer.Text;
                //------------------------------------------------------
                //StockOut.FAddress = txtFAddress.Text.Trim();
                //StockOut.FCompanyId = CurrentUser.AccountComId;

                StockOut.FDate = txtFDate.SelectedDate;

                StockOut.FShipper = !ddlFShipper.SelectedValue.Equals("-1") ? ddlFShipper.SelectedText : "";
                StockOut.FVehicleNum = !ddlFVehicleNum.SelectedValue.Equals("-1") ? ddlFVehicleNum.SelectedText : "";

                //StockOut.FFreight = Convert.ToDecimal(txtFFreight.Text.Trim());
                //StockOut.FLinkman = txtFLinkman.Text.Trim();
                StockOut.FMemo = txtFMemo.Text.Trim();
                //StockOut.FPhone = txtFPhone.Text.Trim();
                StockOut.FPoint = ddlFPoint.SelectedValue;
                StockOut.FDistributionPoint = ddlFDistributionPoint.SelectedValue;
                StockOut.FSendee = ddlFSendee.SelectedValue;

                var parms = new Dictionary<string, object>();
                parms.Clear();

                parms.Add("@keyID", StockOut.KeyId);
                parms.Add("@companyId", CurrentUser.AccountComId);

                var amt =
                    Convert.ToDecimal(SqlService.ExecuteProcedureCommand("proc_SalesAmt", parms).Tables[0].Rows[0][0]);

                StockOut.FAmount = amt;

                StockOut.FSalesman = ddlFSalesman.SelectedValue;
                //StockOut.FArea = ddlFArea.SelectedValue;
                //StockOut.FAmt = string.IsNullOrEmpty(txtFAmt.Text.Trim()) ? 0 : Convert.ToDecimal(txtFAmt.Text.Trim());//
                //StockOut.FDiscountAmount = string.IsNullOrEmpty(txtFDiscountAmount.Text.Trim()) ? 0 : Convert.ToDecimal(txtFDiscountAmount.Text.Trim());
                //StockOut.FReconciliation = txtFReconciliation.Text;
                StockOut.FDeliveryMethod = ddlDeliveryMethod.SelectedValue;
                StockOut.FLogisticsNumber = tbxFLogisticsNumber.Text;
                StockOut.FSupercargo = GasHelper.GetDropDownListArrayString(ddlFSupercargo.SelectedItemArray);
                StockOut.FDriver = GasHelper.GetDropDownListArrayString(ddlFDriver.SelectedItemArray);
                //StockOut.FSubjectCode = ddlSubject.SelectedValue;
                //StockOut.FSubjectName = ddlSubject.SelectedText;
                StockOut.CreateBy = CurrentUser.AccountName;

                //---------------------------------------------------------
                //收发类型
                //StockOut.FT6ReceiveSendType = ddlT6ReceiveSendType.SelectedText;
                //StockOut.FT6ReceiveSendTypeNum = ddlT6ReceiveSendType.SelectedValue;
                //部门名称
                StockOut.FT6Department = "";
                //部门代码
                StockOut.FT6DepartmentNum = "";
                //业务员代码
                StockOut.FT6SalesmanNum = "";
                //币种
                StockOut.FT6Currency = "人民币";// ddlFT6Currency.SelectedValue;
                //汇率
                //StockOut.FT6ExchangeRate = Convert.ToDecimal(txtFT6ExchangeRate.Text);
                //销售类型
                //StockOut.FT6SaleType = ddlFT6SaleType.SelectedText;
                //StockOut.FT6SaleTypeNum = ddlFT6SaleType.SelectedValue;
                //T6同步
                //StockOut.FT6Status = "未同步";
                StockOut.FDistributionPoint = ddlFDistributionPoint.SelectedValue;
                StockOut.FAuditFlag = 1;
                StockOut.FFlag = 1;

                StockOutService.SaveChanges();

                var bottles = new Dictionary<string, object>();
                bottles.Clear();

                bottles.Add("@SalesKeyId", StockOut.KeyId);
                bottles.Add("@companyid", CurrentUser.AccountComId);
                bottles.Add("@date", Convert.ToDateTime(StockOut.FDate).ToString("yyyy-MM-dd"));

                SqlService.ExecuteProcedureCommand("proc_SalesReturnBottle", bottles);

                //收款部分
                var parmsAuto = new Dictionary<string, object>();
                parmsAuto.Clear();

                parmsAuto.Add("@KeyId", StockOut.KeyId);
                parmsAuto.Add("@companyId", CurrentUser.AccountComId);
                parmsAuto.Add("@FCate", "客户");
                parmsAuto.Add("@date", Convert.ToDateTime(StockOut.FDate).ToShortDateString());
                parmsAuto.Add("@FSKNum", string.IsNullOrEmpty(StockOut.FSKNum) ? "" : StockOut.FSKNum);

                SqlService.ExecuteProcedureCommand("proc_SKOrderAuto", parmsAuto);

                return true;
            }
            return false;
        }

        /// <summary>
        ///     提交添加
        /// </summary>
        private bool SubmintAdd()
        {
            var stock = StockOutService.Where(p => p.KeyId == txtKeyId.Text.Trim() && p.FCompanyId == CurrentUser.AccountComId).FirstOrDefault();

            if (stock != null)
            {
                ModifiedGrid();
                /////////////////////////////////////////////////////////////////////////////
                stock.FCode = txtFCode.Text;
                stock.FName = tbxFCustomer.Text;
                //--------------------------------------------------

                //stock.FAddress = txtFAddress.Text.Trim();
                stock.FFlag = 1;
                stock.FDeleteFlag = 0;
                //stock.FFreight = Convert.ToDecimal(txtFFreight.Text.Trim());
                //stock.FLinkman = txtFLinkman.Text.Trim();
                stock.FMemo = txtFMemo.Text.Trim();
                //stock.FPhone = txtFPhone.Text.Trim();

                stock.FShipper = !ddlFShipper.SelectedValue.Equals("-1") ? ddlFShipper.SelectedText : "";
                stock.FVehicleNum = !ddlFVehicleNum.SelectedValue.Equals("-1") ? ddlFVehicleNum.SelectedText : "";

                var parms = new Dictionary<string, object>();
                parms.Clear();

                parms.Add("@keyID", stock.KeyId);
                parms.Add("@companyId", CurrentUser.AccountComId);

                var amt =
                    Convert.ToDecimal(SqlService.ExecuteProcedureCommand("proc_SalesAmt", parms).Tables[0].Rows[0][0]);

                stock.FAmount = amt;

                stock.FDate = txtFDate.SelectedDate;
                stock.FSalesman = ddlFSalesman.SelectedValue;
                //stock.FArea = ddlFArea.SelectedValue;
                //stock.FAmt = string.IsNullOrEmpty(txtFAmt.Text.Trim()) ? 0 : Convert.ToDecimal(txtFAmt.Text.Trim());
                //stock.FDiscountAmount = string.IsNullOrEmpty(txtFDiscountAmount.Text.Trim()) ? 0 : Convert.ToDecimal(txtFDiscountAmount.Text.Trim());
                //stock.FReconciliation = txtFReconciliation.Text;
                stock.FDeliveryMethod = ddlDeliveryMethod.SelectedValue;
                stock.FLogisticsNumber = tbxFLogisticsNumber.Text;

                stock.FSupercargo = GasHelper.GetDropDownListArrayString(ddlFSupercargo.SelectedItemArray);
                stock.FDriver = GasHelper.GetDropDownListArrayString(ddlFDriver.SelectedItemArray);

                //stock.FSubjectCode = ddlSubject.SelectedValue;
                //stock.FSubjectName = ddlSubject.SelectedText;
                stock.FSKNum = "";
                stock.FDistributionPoint = ddlFDistributionPoint.SelectedValue;


                #region T6 对接接口
                //---------------------------------------------------------
                //收发类型
                //stock.FT6ReceiveSendType = ddlT6ReceiveSendType.SelectedText;
                //stock.FT6ReceiveSendTypeNum = ddlT6ReceiveSendType.SelectedValue;
                //部门名称
                stock.FT6Department = "";
                //部门代码
                stock.FT6DepartmentNum = "";
                //业务员代码
                stock.FT6SalesmanNum = "";
                //币种
                //stock.FT6Currency = ddlFT6Currency.SelectedValue;
                //汇率
                //stock.FT6ExchangeRate = Convert.ToDecimal(txtFT6ExchangeRate.Text);
                //销售类型
                //stock.FT6SaleType = ddlFT6SaleType.SelectedText;
                //stock.FT6SaleTypeNum = ddlFT6SaleType.SelectedValue;
                //T6同步
                stock.FT6Status = "未同步";
                stock.FT6BillStatus = "未开票";
                stock.FT6PaymentStatus = "未收款";
                //---------------------------------------------------------
                #endregion

                stock.FPoint= ddlFPoint.SelectedValue;
                stock.FDistributionPoint= ddlFDistributionPoint.SelectedValue;
                stock.FAuditFlag = 1;
                stock.FFlag = 1;
                stock.FSendee = ddlFSendee.SelectedValue;
                stock.FTime1=DateTime.Now;

                StockOutService.SaveChanges();

                if (txtKeyId.Text.Contains("TM"))
                {
                    //单据号问题
                    string newKeyId = SequenceService.CreateSequence(Convert.ToDateTime(txtFDate.SelectedDate), "AX", CurrentUser.AccountComId);
                    var orderParms = new Dictionary<string, object>();
                    orderParms.Clear();
                    orderParms.Add("@oldKeyId", txtKeyId.Text);
                    orderParms.Add("@newKeyId", newKeyId);
                    orderParms.Add("@Bill", "3");
                    orderParms.Add("@companyId", CurrentUser.AccountComId);

                    SqlService.ExecuteProcedureCommand("proc_num", orderParms);
                    txtKeyId.Text = newKeyId;

                    //新增日志
                    var billStatus = new LHBillStatus
                    {
                        KeyId = newKeyId,
                        FCompanyId = CurrentUser.AccountComId,
                        FActionName = "新增",
                        FDate = DateTime.Now,
                        FDeptId = CurrentUser.AccountOrgId,
                        FOperator = CurrentUser.AccountName,
                        FMemo = String.Format("单据号{0},{1}新增调拨出库单据。", newKeyId, CurrentUser.AccountName)
                    };

                    GasHelper.AddBillStatus(billStatus);

                    var bottles = new Dictionary<string, object>();
                    bottles.Clear();

                    bottles.Add("@SalesKeyId", newKeyId);
                    bottles.Add("@companyid", CurrentUser.AccountComId);
                    bottles.Add("@date", Convert.ToDateTime(stock.FDate).ToString("yyyy-MM-dd"));

                    SqlService.ExecuteProcedureCommand("proc_SalesReturnBottle", bottles);


                    //收款部分
                    var parmsAuto = new Dictionary<string, object>();
                    parmsAuto.Clear();

                    parmsAuto.Add("@KeyId", newKeyId);
                    parmsAuto.Add("@companyId", CurrentUser.AccountComId);
                    parmsAuto.Add("@FCate", "客户");
                    parmsAuto.Add("@date", Convert.ToDateTime(stock.FDate).ToString("yyyy-MM-dd"));
                    parmsAuto.Add("@FSKNum", stock.FSKNum);

                    SqlService.ExecuteProcedureCommand("proc_SKOrderAuto", parmsAuto);
                }

                //订单出库 
                var passCard = PassCardService.Where(p => p.KeyId == tbxFLogisticsNumber.Text.Trim()).FirstOrDefault();

                if (passCard != null)
                {
                    passCard.FOutFlag = "已出库";

                    PassCardService.SaveChanges();
                }

                if (!string.IsNullOrEmpty(hfdImage.Text))
                {
                    var attachment = new LHAttachment();
                    attachment.FCompanyId = CurrentUser.AccountComId;
                    attachment.FPath = hfdImage.Text;
                    attachment.KeyId = txtKeyId.Text;

                    AttachmentService.Add(attachment);
                }


                return true;
            }

            return false;
        }

        /// <summary>
        ///     初始化页面数据
        /// </summary>
        private void InitData()
        {
            ViewState["_AppendToEnd"] = true;

            txtCreateBy.Text = CurrentUser.AccountName;

            tbxFItemCode.OnClientTriggerClick = Window1.GetSaveStateReference(tbxFItemCode.ClientID)
                    + Window1.GetShowReference("../../Common/WinProduct.aspx");

            tbxFCustomer.OnClientTriggerClick = Window2.GetSaveStateReference(txtFCode.ClientID, tbxFCustomer.ClientID)
                    + Window2.GetShowReference("../../Common/WinCustomer.aspx");

            //txtFAddress.OnClientTriggerClick = Window3.GetSaveStateReference(txtFAddress.ClientID)
            //        + Window3.GetShowReference(string.Format(@"../../Common/WinCustomerLink.aspx"));

            tbxFBottle.OnClientTriggerClick = Window1.GetSaveStateReference(tbxFBottle.ClientID, hfdSpec.ClientID)
                    + Window1.GetShowReference("../../Common/WinBottleToGas.aspx");

            tbxFLogisticsNumber.OnClientTriggerClick = Window4.GetSaveStateReference(tbxFLogisticsNumber.ClientID)
                    + Window4.GetShowReference("../../Common/WinTask.aspx?FType=53");

            GasHelper.DropDownListDistributionPointDataBind(ddlFDistributionPoint); //

            GasHelper.DropDownListDistributionPointDataBind(ddlFPoint); //

            GasHelper.DropDownListDriverDataBind(ddlFDriver);

            GasHelper.DropDownListGodownKeeperDataBind(ddlFShipper);

            GasHelper.DropDownListGodownKeeperDataBind(ddlFSendee);

            GasHelper.DropDownListSupercargoDataBind(ddlFSupercargo);

            //GasHelper.DropDownListBottleDataBind(tbxFBottle);

            GasHelper.DropDownListVehicleNumDataBind(ddlFVehicleNum);

            //GasHelper.DropDownListAreasDataBind(ddlFArea);

            GasHelper.DropDownListSalesmanDataBind(ddlFSalesman);

            GasHelper.DropDownListDeliveryMethodDataBind(ddlDeliveryMethod);

            //GasHelper.DropDownListBankSubjectDataBind(ddlSubject);

            //GasHelper.DropDownListDataBindSaleType(ddlFT6SaleType);

            //GasHelper.DropDownListDataBindReceiveSendType(ddlT6ReceiveSendType);

            GasHelper.DropDownListWarehouseDataBind(tbxFWarehouse);

            //GasHelper.DropDownListDataBindCurrencyType(ddlFT6Currency);

            GasHelper.DropDownListDistributionPointDataBind(ddlFDistributionPoint); //

            GasHelper.DropDownListDistributionPointDataBind(ddlFPoint); //

            //ddlFT6SaleType.SelectedValue = "01";//普通销售

            //ddlT6ReceiveSendType.SelectedValue = "0202";//销售出库

            //txtFT6ExchangeRate.Text = "1";

            //删除选中单元格的客户端脚本
            string deleteScript = DeleteScript();

            //新增
            //var defaultObj = new JObject
            //{
            //    {"FItemCode", ""},
            //    {"FItemName", ""},
            //    {"FSpec", ""},
            //    {"FUnit", ""},
            //    {"FPrice", ""},
            //    {"FQty", "0"},
            //    {"FAmount", "0"},
            //    {"FBottleQty", "0"},
            //    {"FBottleOweQty", "0"},
            //    {"FCateName", ""},
            //    {"colDelete", String.Format("<a href=\"javascript:;\" onclick=\"{0}\"><img src=\"{1}\"/></a>",//
            //        deleteScript, IconHelper.GetResolvedIconUrl(Icon.Delete))},
            //};

            //// 在第一行新增一条数据
            //btnAdd.OnClientClick = Grid1.GetAddNewRecordReference(defaultObj, AppendToEnd);

            txtFDate.SelectedDate = DateTime.Now;

            //txtFFreight.Text = "0.00";

            ddlFShipper.SelectedValue = CurrentUser.AccountName;
        }

        /// <summary>
        ///     加载页面数据
        /// </summary>
        private void LoadData()
        {
            switch (Actions)
            {
                case WebAction.Add:
                    txtKeyId.Text = SequenceService.CreateSequence("TM", CurrentUser.AccountComId);
                    Region3.Title = "添加调拨出库单";

                    var temp = new LHStockOut
                    {
                        KeyId = txtKeyId.Text,

                        FFlag = 1,

                        FDeleteFlag = 1,

                        //调拨出库单
                        FType = Convert.ToInt32(GasEnumBill.Allot),

                        CreateBy = CurrentUser.AccountName,

                        FDate = txtFDate.SelectedDate,

                        FCompanyId = CurrentUser.AccountComId,

                        FStatus = Convert.ToInt32(GasEnumBillStauts.Add),

                        FProgress = Convert.ToInt32(GasEnumBillStauts.Add),

                        FCate = "客户"
                    };

                    //临时写入单据
                    StockOutService.Add(temp);

                    //合计
                    var summary = new JObject
                    {
                        {"FItemCode", "合计"},
                        {"FQty", 0},
                        {"FAmount", 0}
                    };

                    Grid1.SummaryData = summary;

                    break;
                case WebAction.Edit:
                    txtKeyId.Text = KeyId;
                    Region3.Title = "编辑调拨出库单";

                    if (StockOut != null)
                    {
                        WebControlHandler.BindObjectToControls(StockOut, SimpleForm1);
                        txtFDate.SelectedDate = StockOut.FDate;
                        tbxFCustomer.Text = StockOut.FName;
                        tbxFLogisticsNumber.Text = StockOut.FLogisticsNumber;

                        if (!string.IsNullOrEmpty(StockOut.FDriver))
                            ddlFDriver.SelectedValueArray = StockOut.FDriver.Split(',');
                        if (!string.IsNullOrEmpty(StockOut.FShipper))
                            ddlFShipper.SelectedValueArray = StockOut.FShipper.Split(',');
                        if (!string.IsNullOrEmpty(StockOut.FSupercargo))
                            ddlFSupercargo.SelectedValueArray = StockOut.FSupercargo.Split(',');
                        if (!string.IsNullOrEmpty(StockOut.FSalesman))
                            ddlFSalesman.SelectedValueArray = StockOut.FSalesman.Split(',');

                        //ddlSubject.SelectedValue = StockOut.FSubjectCode;
                        ddlFVehicleNum.SelectedValue = StockOut.FVehicleNum;
                        //ddlFArea.SelectedValue = StockOut.FArea;
                        ddlDeliveryMethod.SelectedValue = StockOut.FDeliveryMethod;

                        ddlFSendee.SelectedValue = StockOut.FSendee;

                        //ddlFT6SaleType.SelectedValue = StockOut.FT6SaleTypeNum;
                        //ddlFT6Currency.SelectedValue = StockOut.FT6Currency;
                        //ddlT6ReceiveSendType.SelectedValue = StockOut.FT6ReceiveSendTypeNum;

                        ddlFDistributionPoint.SelectedValue = StockOut.FDistributionPoint;

                        //if (Attachment != null)
                        //{
                        //    lblfile.Text = string.Format(@"<a href='../../{0}' target='_blank'>附件</a>",//
                        //        Attachment.FPath.Replace("~/", ""));
                        //}
                        //else
                        //{
                        //    lblfile.Text = string.Empty;
                        //}
                        ddlFPoint.SelectedValue = StockOut.FPoint;
                        ddlFDistributionPoint.SelectedValue = StockOut.FDistributionPoint;

                        BindDataGrid();
                    }
                    break;
            }
        }

        /// <summary>
        ///     删除选中行的脚本
        /// </summary>
        /// <returns></returns>
        private string DeleteScript()
        {
            return Confirm.GetShowReference("删除选中行？", String.Empty, MessageBoxIcon.Question, Grid1.GetDeleteSelectedRowsReference(), String.Empty);
        }

        #endregion

    }
}