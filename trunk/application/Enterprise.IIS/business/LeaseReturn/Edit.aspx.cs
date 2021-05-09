using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Enterprise.Data;
using Enterprise.Framework.Web;
using Enterprise.IIS.Common;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using FineUI;
using Newtonsoft.Json.Linq;


namespace Enterprise.IIS.business.LeaseReturn
{
    public partial class Edit : PageBase
    {
        /// <summary>
        ///     AppendToEnd
        /// </summary>
        private bool AppendToEnd
        {
            get
            {
                return ViewState["_AppendToEnd"] != null && Convert.ToBoolean(ViewState["_AppendToEnd"]);
            }
        }

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
                string sort = ViewState["SortDirection"] != null ? ViewState["SortDirection"].ToString() : "ASC";
                return sort;
            }
            set { ViewState["SortDirection"] = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private ViewUnitService _viewUnitService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected ViewUnitService ViewUnitService
        {
            get { return _viewUnitService ?? (_viewUnitService = new ViewUnitService()); }
            set { _viewUnitService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private LeaseReturnService _leaseReturnService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected LeaseReturnService LeaseReturnService
        {
            get { return _leaseReturnService ?? (_leaseReturnService = new LeaseReturnService()); }
            set { _leaseReturnService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private LeaseReturnDetailsLogService _logService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected LeaseReturnDetailsLogService LeaseReturnDetailsLogService
        {
            get { return _logService ?? (_logService = new LeaseReturnDetailsLogService()); }
            set { _logService = value; }
        }


        /// <summary>
        ///     数据服务
        /// </summary>
        private LeaseDetailsService _leaseDetailsService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected LeaseDetailsService LeaseDetailsService
        {
            get { return _leaseDetailsService ?? (_leaseDetailsService = new LeaseDetailsService()); }
            set { _leaseDetailsService = value; }
        }


        /// <summary>
        ///     数据服务
        /// </summary>
        private LeaseReturnDetailsService _leaseReturnDetailsService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected LeaseReturnDetailsService LeaseReturnDetailsService
        {
            get { return _leaseReturnDetailsService ?? (_leaseReturnDetailsService = new LeaseReturnDetailsService()); }
            set { _leaseReturnDetailsService = value; }
        }

        /// <summary>
        ///     客户档案
        /// </summary>
        private vm_Unit _vmUnit;

        /// <summary>
        ///     客户档案
        /// </summary>
        protected vm_Unit Customer
        {
            get { return _vmUnit ?? (_vmUnit = ViewUnitService.FirstOrDefault(p => p.FCode == txtFCode.Text && p.FCompanyId == CurrentUser.AccountComId)); }
            set { _vmUnit = value; }
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
        private LHLeaseReturn _lease;

        /// <summary>
        ///     职员档案
        /// </summary>
        protected LHLeaseReturn LeaseReturn
        {
            get { return _lease ?? (_lease = LeaseReturnService.FirstOrDefault(p => p.KeyId == KeyId)); }
            set { _lease = value; }
        }

        /// <summary>
        ///     FCode
        /// </summary>
        protected string KeyId
        {
            get { return Request["KeyId"]; }
        }

        /// <summary>
        ///     当前画面操作项
        /// </summary>
        public WebAction Actions
        {
            get
            {
                string s = Convert.ToString(Request["action"]);
                return (WebAction)Int32.Parse(s);
            }
        }

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
                #region 退租明细
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

                    var data = SqlService.Where(string.Format("SELECT * FROM dbo.vm_LeaseBottleAR a WHERE a.FId IN ({0}) and FCompanyId={1}", value, CurrentUser.AccountComId));

                    if (data != null && data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0)
                    {
                        var table = data.Tables[0];
                        for (int i = 0; i < table.Rows.Count; i++)
                        {
                            var details = new LHLeaseReturnDetails();

                            details.FBottle = table.Rows[i]["FBottle"].ToString();
                            details.FPrice = Convert.ToDecimal(table.Rows[i]["FPrice"]);
                            details.FBottleQty = Convert.ToInt32(table.Rows[i]["ARBottleQty"]);//
                            details.FCompanyId = CurrentUser.AccountComId;
                            details.KeyId = txtKeyId.Text.Trim();
                            //每天租赁费
                            details.FRentDay = Convert.ToDecimal(table.Rows[i]["FRentDay"]);
                            //押金
                            details.FDepositSecurity = Convert.ToDecimal(table.Rows[i]["FDepositSecurity"]);

                            details.FDays = Convert.ToInt32(table.Rows[i]["days"]);

                            details.FPaymentRentals = Convert.ToDecimal(table.Rows[i]["ARRentals"]);

                            //租赁明细Id
                            details.FDetailsFId = Convert.ToInt32(table.Rows[i]["FId"]);
                            LeaseReturnDetailsService.AddEntity(details);

                            //日志
                            switch (Actions)
                            {
                                case WebAction.Add:
                                    break;
                                case WebAction.Edit:
                                    //记录一下当前新增人操作内容
                                    var detailslog = new LHLeaseReturnDetails_Log
                                    {
                                        FUpdateBy = CurrentUser.AccountName,
                                        FUpdateDate = DateTime.Now,
                                        FBottle = details.FBottle,
                                        FPrice = details.FPrice,
                                        FBottleQty = details.FBottleQty,
                                        KeyId = txtKeyId.Text.Trim(),
                                        FStatus = "新增",
                                        FCompanyId = CurrentUser.AccountComId,
                                        FDays = details.FDays,
                                        FRentDay = details.FRentDay,
                                        FPaymentRentals = details.FPaymentRentals,
                                        FDepositSecurity = details.FDepositSecurity,
                                        FMemo = string.Format(@"时间：{0} 操作人：{1}", DateTime.Now, CurrentUser.AccountName)
                                    };

                                    LeaseReturnDetailsLogService.Add(detailslog);

                                    break;
                            }
                        }

                        LeaseReturnDetailsService.SaveChanges();

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
            decimal sumFBottleQty = 0.00M;
            decimal sumFDepositSecurity = 0.00M;
            decimal sumFPaymentRentals = 0.00M;

            foreach (JObject mergedRow in Grid1.GetMergedData())
            {
                JObject values = mergedRow.Value<JObject>("values");

                sumFBottleQty += values.Value<decimal>("FBottleQty");
                sumFDepositSecurity += values.Value<decimal>("FDepositSecurity");
                sumFPaymentRentals += values.Value<decimal>("FPaymentRentals");
            }

            JObject summary = new JObject();
            summary.Add("FBottleQty", sumFBottleQty);
            summary.Add("FDepositSecurity", sumFDepositSecurity);
            summary.Add("FPaymentRentals", sumFPaymentRentals);
            Grid1.SummaryData = summary;
        }

        #endregion

        /// <summary>
        ///      
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tbxFBottle_OnTriggerClick(object sender, EventArgs e)
        {
            Window1.Hidden = true;
            Window2.Hidden = true;
            Window3.Hidden = true;
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
                PageContext.RegisterStartupScript(string.Format("LodopPrinter('{0}');", KeyId));
            }
            catch (Exception)
            {
                Alert.Show("打印失败！", MessageBoxIcon.Warning);
            }
        }
        /// <summary>
        ///     文本改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tbxFCustomer_OnTextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbxFCustomer.Text.Trim()))
            {
                Window1.Hidden = true;
                Window2.Hidden = true;
                Window3.Hidden = true;

                var custmoer = ViewUnitService.Where(p => p.FName == tbxFCustomer.Text.Trim() && p.FCompanyId == CurrentUser.AccountComId).FirstOrDefault();
                if (custmoer != null)
                {
                    txtFCode.Text = custmoer.FCode;
                    txtFAddress.Text = custmoer.FAddress.Trim();
                    txtFLinkman.Text = custmoer.FLinkman;
                    txtFPhone.Text = custmoer.FPhome;
                }
            }
        }

        protected void txtFAddress_OnTriggerClick(object sender, EventArgs e)
        {
            Window1.Hidden = true;
            Window2.Hidden = true;
        }

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
            catch (Exception)
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
        ///     分页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_PageIndexChange(object sender, GridPageEventArgs e)
        {
            Grid1.PageIndex = e.NewPageIndex;
            //BindDataGrid();
        }

        /// <summary>
        /// 
        /// </summary>
        private void BindDataGrid()
        {
            var source = SqlService.Where(string.Format(@"SELECT * FROM dbo.vm_LeaseReturnDetails WHERE keyId='{0}' and FCompanyId={1}", txtKeyId.Text, CurrentUser.AccountComId));

            //绑定数据源
            Grid1.DataSource = source;
            Grid1.DataBind();

            var table = source.Tables[0];

            if (table != null && table.Rows.Count > 0)
            {
                decimal sumFBottleQty = 0.00M;
                decimal sumFPaymentRentals = 0.00M;
                decimal sumFDepositSecurity = 0.00M;


                for (int i = 0; i < table.Rows.Count; i++)
                {
                    sumFBottleQty += Convert.ToDecimal(table.Rows[i]["FBottleQty"]);
                    sumFPaymentRentals += Convert.ToDecimal(table.Rows[i]["FPaymentRentals"]);
                    sumFDepositSecurity += Convert.ToDecimal(table.Rows[i]["FDepositSecurity"]);
                }

                var summary = new JObject
                {
                    {"FBottle", "合计"},
                    {"FBottleQty", sumFBottleQty},
                    {"FPaymentRentals", sumFPaymentRentals},
                    {"FDepositSecurity", sumFDepositSecurity},
                };

                Grid1.SummaryData = summary;
            }
        }

        /// <summary>
        ///     分页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
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
                    Window1.GetShowReference(string.Format("../../Common/WinUnitLeaseBottle.aspx?unitcode={0}&FDate={1}",
                        txtFCode.Text.Trim(), Convert.ToDateTime(txtFDate.SelectedDate).ToString("yyyy-MM-dd")), "退租"));
            }
            catch (Exception)
            {
                Alert.Show("添加失败！", MessageBoxIcon.Warning);
            }
        }


        /// <summary>
        ///     排序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_Sort(object sender, GridSortEventArgs e)
        {
            SortField = String.Format(@"{0}", e.SortField);
            SortDirection = e.SortDirection;
            //BindDataGrid();
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_AfterEdit(object sender, GridAfterEditEventArgs e)
        {
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
                var parms = new Dictionary<string, object>();
                parms.Clear();
                parms.Add("@keyID", txtKeyId.Text);
                parms.Add("@companyId", CurrentUser.AccountComId);

                SqlService.ExecuteProcedureCommand("proc_LeaseReturnAmt", parms);

                var datakey = Convert.ToInt32(Grid1.DataKeys[e.RowIndex][1]);
                LeaseReturnDetailsService.Delete(p => p.FId == datakey);

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
        #endregion

        #region Private Method

        /// <summary>
        ///     ModifiedGrid
        /// </summary>
        private void ModifiedGrid()
        {
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

                var details = LeaseReturnDetailsService.Where(p => p.FId == datakey && p.FCompanyId == CurrentUser.AccountComId).FirstOrDefault();

                //写入原始，通过存储过程完成明细复制
                var parms = new Dictionary<string, object>();
                parms.Clear();

                parms.Add("@fid", datakey);
                parms.Add("@opr", CurrentUser.AccountName);
                parms.Add("@companyId", CurrentUser.AccountComId);
                SqlService.ExecuteProcedureCommand("proc_LeaseReturnDetails_Log", parms);

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
                            if (key.Equals("FBottleQty"))
                            {

                                var parmsQty = new Dictionary<string, object>();
                                parmsQty.Clear();

                                parmsQty.Add("@FCode", txtFCode.Text);
                                parmsQty.Add("@FId", datakey);
                                parmsQty.Add("@companyId", CurrentUser.AccountComId);
                                var dataQty = SqlService.ExecuteProcedureCommand("proc_LeaseBottleARByFCode", parmsQty).Tables[0];

                                //int arBottleQty = 0;
                                //if (dataQty != null && dataQty.Rows.Count > 0)
                                //    arBottleQty = Convert.ToInt32(dataQty.Rows[0]["ARBottleQty"]);

                                //验证当前回收量是否大于本次应回收量
                                //if (Convert.ToInt32(value) > arBottleQty)
                                //{
                                //    Alert.ShowInParent("本次回收量不能大余应回收量，请输入合法值。", MessageBoxIcon.Information);
                                //    return;
                                //}

                                details.FBottleQty = Convert.ToInt32(value);
                                details.FPaymentRentals = details.FBottleQty * details.FDays * details.FRentDay;
                                details.FDepositSecurity = details.FBottleQty * details.FPrice;
                            }

                            if (key.Equals("FRentDay"))
                            {//租金/天
                                details.FRentDay = Convert.ToDecimal(value);
                            }

                            if (key.Equals("FPrice"))
                            {//押金/个
                                details.FPrice = Convert.ToDecimal(value);
                                details.FPaymentRentals = details.FBottleQty * details.FDays * details.FRentDay;
                                details.FDepositSecurity = details.FBottleQty * details.FPrice;
                            }

                            if (key.Equals("FAmount"))
                            {//押金
                                details.FPaymentRentals = details.FBottleQty * details.FDays * details.FRentDay;
                                details.FDepositSecurity = details.FBottleQty * details.FPrice;
                                break;

                            }

                            var detailslog = new LHLeaseReturnDetails_Log
                            {
                                FUpdateBy = CurrentUser.AccountName,
                                FUpdateDate = DateTime.Now,
                                FBottle = details.FBottle,
                                FPrice = details.FPrice,
                                FBottleQty = details.FBottleQty,
                                FPaymentRentals = details.FPaymentRentals,
                                FDepositSecurity = details.FDepositSecurity,
                                KeyId = details.KeyId,
                                FDays = details.FDays,
                                FRentDay = details.FRentDay,
                                FStatus = "变更",
                                FCompanyId = CurrentUser.AccountComId,
                                FMemo = string.Format(@"时间：{0} 变更人：{1}", DateTime.Now, CurrentUser.AccountName)
                            };

                            LeaseReturnDetailsLogService.Add(detailslog);
                        }
                    }
                    #endregion

                }

                LeaseReturnDetailsService.SaveChanges();
            }
        }

        /// <summary>
        ///     提交编辑
        /// </summary>
        private bool SubmintEdit()
        {
            if (LeaseReturn != null)
            {
                ModifiedGrid();

                LeaseReturn.FCode = txtFCode.Text;
                LeaseReturn.FName = tbxFCustomer.Text;
                LeaseReturn.FAddress = txtFAddress.Text.Trim();
                LeaseReturn.FCompanyId = CurrentUser.AccountComId;

                //StockOut.FDriver = !ddlFDriver.SelectedValue.Equals("-1") ? ddlFDriver.SelectedText : "";
                LeaseReturn.FShipper = !ddlFShipper.SelectedValue.Equals("-1") ? ddlFShipper.SelectedText : "";
                //StockOut.FSupercargo = !ddlFSupercargo.SelectedValue.Equals("-1") ? ddlFSupercargo.SelectedText : "";
                LeaseReturn.FVehicleNum = !ddlFVehicleNum.SelectedValue.Equals("-1") ? ddlFVehicleNum.SelectedText : "";


                LeaseReturn.FFlag = 1;
                //LeaseReturn.FFreight = Convert.ToDecimal(txtFFreight.Text.Trim());
                LeaseReturn.FLinkman = txtFLinkman.Text.Trim();
                LeaseReturn.FMemo = txtFMemo.Text.Trim();
                LeaseReturn.FPhone = txtFPhone.Text.Trim();

                LeaseReturn.FSupercargo = GasHelper.GetDropDownListArrayString(ddlFSupercargo.SelectedItemArray);
                LeaseReturn.FDriver = GasHelper.GetDropDownListArrayString(ddlFDriver.SelectedItemArray);

                LeaseReturn.FPaymentRentals = string.IsNullOrEmpty(txtFPaymentRentals.Text.Trim()) ? 0 : Convert.ToDecimal(txtFPaymentRentals.Text.Trim());
                LeaseReturn.FDepositSecurity = string.IsNullOrEmpty(txtFDepositSecurity.Text.Trim()) ? 0 : Convert.ToDecimal(txtFDepositSecurity.Text.Trim());

                LeaseReturn.FSubjectCodeIn = ddlSubjectIn.SelectedValue;
                LeaseReturn.FSubjectNameIn = ddlSubjectIn.SelectedText;
                LeaseReturn.FSubjectCodeOut = ddlSubjectOut.SelectedValue;
                LeaseReturn.FSubjectNameOut = ddlSubjectOut.SelectedText;
                LeaseReturn.FCate = hfdClass.Text;
                LeaseReturn.CreateBy = CurrentUser.AccountName;

                LeaseReturnService.SaveChanges();

                var parms = new Dictionary<string, object>();
                parms.Clear();
                parms.Add("@keyID", LeaseReturn.KeyId);
                parms.Add("@companyId", CurrentUser.AccountComId);

               var data= SqlService.ExecuteProcedureCommand("proc_LeaseReturnAmt", parms).Tables[0];
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    int ids = Convert.ToInt32(data.Rows[i]["FDetailsFId"]);
                    int qty = Convert.ToInt32(data.Rows[i]["Qty"]);
                    var details = LeaseDetailsService.FirstOrDefault(p => p.FId == ids);
                    details.FReturnQty = details.FReturnQty + qty;
                    LeaseDetailsService.SaveChanges();
                }


                //生成财务凭证
                var parmsAuto = new Dictionary<string, object>();
                parmsAuto.Clear();

                parmsAuto.Add("@KeyId", LeaseReturn.KeyId);
                parmsAuto.Add("@companyId", CurrentUser.AccountComId);
                parmsAuto.Add("@FCate", hfdClass.Text);
                parmsAuto.Add("@date", Convert.ToDateTime(LeaseReturn.FDate).ToString("yyyy-MM-dd"));
                parmsAuto.Add("@FSKNum", LeaseReturn.FSKNum);
                parmsAuto.Add("@FAbstract", "退租");

                SqlService.ExecuteProcedureCommand("proc_SFKLeaseReturnAuto", parmsAuto);

                return true;
            }
            return false;
        }

        /// <summary>
        ///     提交添加
        /// </summary>
        private bool SubmintAdd()
        {
            var leaseReturn = LeaseReturnService.Where(p => p.KeyId == txtKeyId.Text.Trim() && p.FCompanyId == CurrentUser.AccountComId).FirstOrDefault();

            if (leaseReturn != null)
            {
                ModifiedGrid();

                leaseReturn.FCode = txtFCode.Text;
                leaseReturn.FName = tbxFCustomer.Text;
                leaseReturn.FAddress = txtFAddress.Text.Trim();
                leaseReturn.FCompanyId = CurrentUser.AccountComId;
                leaseReturn.FFlag = 1;
                leaseReturn.FDeleteFlag = 0;
                leaseReturn.FLinkman = txtFLinkman.Text.Trim();
                leaseReturn.FMemo = txtFMemo.Text.Trim();
                leaseReturn.FPhone = txtFPhone.Text.Trim();
                leaseReturn.FShipper = !ddlFShipper.SelectedValue.Equals("-1") ? ddlFShipper.SelectedText : "";
                leaseReturn.FVehicleNum = !ddlFVehicleNum.SelectedValue.Equals("-1") ? ddlFVehicleNum.SelectedText : "";
                leaseReturn.FSupercargo = GasHelper.GetDropDownListArrayString(ddlFSupercargo.SelectedItemArray);
                leaseReturn.FDriver = GasHelper.GetDropDownListArrayString(ddlFDriver.SelectedItemArray);
                leaseReturn.FPaymentRentals = string.IsNullOrEmpty(txtFPaymentRentals.Text.Trim()) ? 0 : Convert.ToDecimal(txtFPaymentRentals.Text.Trim());
                leaseReturn.FDepositSecurity = string.IsNullOrEmpty(txtFDepositSecurity.Text.Trim()) ? 0 : Convert.ToDecimal(txtFDepositSecurity.Text.Trim());
                
                leaseReturn.FSubjectCodeIn = ddlSubjectIn.SelectedValue;
                leaseReturn.FSubjectNameIn = ddlSubjectIn.SelectedText;

                leaseReturn.FSubjectCodeOut = ddlSubjectOut.SelectedValue;
                leaseReturn.FSubjectNameOut = ddlSubjectOut.SelectedText;
                leaseReturn.FSKNum = "";
                leaseReturn.FCate = hfdClass.Text;
                leaseReturn.FFKNum = "";
                LeaseReturnService.SaveChanges();


                var parms = new Dictionary<string, object>();
                parms.Clear();
                parms.Add("@keyID", leaseReturn.KeyId);
                parms.Add("@companyId", CurrentUser.AccountComId);
                var data= SqlService.ExecuteProcedureCommand("proc_LeaseReturnAmt", parms).Tables[0];

                for (int i = 0; i < data.Rows.Count; i++)
                {

                    int ids = Convert.ToInt32(data.Rows[i]["FDetailsFId"]);
                    int qty = Convert.ToInt32(data.Rows[i]["Qty"]);
                    var details = LeaseDetailsService.FirstOrDefault(p => p.FId == ids);
                    details.FReturnQty = details.FReturnQty + qty;
                    LeaseDetailsService.SaveChanges();
                }

                if (txtKeyId.Text.Contains("TM"))
                {
                    //单据号问题
                    string newKeyId = SequenceService.CreateSequence(Convert.ToDateTime(txtFDate.SelectedDate), "TZ", CurrentUser.AccountComId);
                    var orderParms = new Dictionary<string, object>();
                    orderParms.Clear();
                    orderParms.Add("@oldKeyId", txtKeyId.Text);
                    orderParms.Add("@newKeyId", newKeyId);
                    orderParms.Add("@Bill", "8");
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
                        FMemo = String.Format("单据号{0},{1}新增租赁归还单据。", newKeyId, CurrentUser.AccountName)
                    };
                    GasHelper.AddBillStatus(billStatus);

                    //生成财务凭证
                    var parmsAuto = new Dictionary<string, object>();
                    parmsAuto.Clear();

                    parmsAuto.Add("@KeyId", newKeyId);
                    parmsAuto.Add("@companyId", CurrentUser.AccountComId);
                    parmsAuto.Add("@FCate", hfdClass.Text);
                    parmsAuto.Add("@date", Convert.ToDateTime(leaseReturn.FDate).ToString("yyyy-MM-dd"));
                    parmsAuto.Add("@FSKNum", leaseReturn.FSKNum);
                    parmsAuto.Add("@FAbstract", "退租");

                    SqlService.ExecuteProcedureCommand("proc_SFKLeaseReturnAuto", parmsAuto);
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

            //tbxFBottle.OnClientTriggerClick = Window1.GetSaveStateReference(tbxFBottle.ClientID)
            //        + Window1.GetShowReference("../../Common/WinUnitLeaseBottle.aspx");

            tbxFCustomer.OnClientTriggerClick = Window2.GetSaveStateReference(txtFCode.ClientID, tbxFCustomer.ClientID, hfdClass.ClientID)
                    + Window2.GetShowReference("../../Common/WinUnit.aspx");

            txtFAddress.OnClientTriggerClick = Window3.GetSaveStateReference(txtFAddress.ClientID)
                    + Window3.GetShowReference(string.Format(@"../../Common/WinCustomerLink.aspx"));

            //区域
            //GasHelper.DropDownListCustomerDataBind(ddlCustomer);

            GasHelper.DropDownListSalesmanDataBind(ddlFDriver);

            GasHelper.DropDownListSalesmanDataBind(ddlFShipper);

            GasHelper.DropDownListSalesmanDataBind(ddlFSupercargo);

            GasHelper.DropDownListVehicleNumDataBind(ddlFVehicleNum);

            GasHelper.DropDownListBankSubjectDataBind(ddlSubjectOut);

            GasHelper.DropDownListBankSubjectDataBind(ddlSubjectIn);

            //删除选中单元格的客户端脚本
            //string deleteScript = DeleteScript();

            //选择产品填加
            //string searchScript = SearchScript();

            ////新增
            //var defaultObj = new JObject
            //{
            //    {"FItemCode", ""},
            //    {"FItemName", ""},
            //    {"FSpec", ""},
            //    {"FUnit", ""},
            //    {"FPrice", ""},
            //    {"FQty", "0"},
            //    {"FBottleQty", "0"},
            //    {"FRentDay", "0"},
            //    {"FDays", "0"},
            //    {"FPaymentRentals", "0"},
            //    {"FDepositSecurity", "0"},
            //    {"FCateName", ""},
            //    {"colDelete", String.Format("<a href=\"javascript:;\" onclick=\"{0}\"><img src=\"{1}\"/></a>",//
            //        deleteScript, IconHelper.GetResolvedIconUrl(Icon.Delete))},
                
            //    //{"colSearch", String.Format("<a href=\"javascript:;\" onclick=\"{0}\"><img src=\"{1}\"/></a>", //
            //    //    searchScript, IconHelper.GetResolvedIconUrl(Icon.SystemSearch))}
                
            //};

            // 在第一行新增一条数据
            //btnAdd.OnClientClick = Grid1.GetAddNewRecordReference(defaultObj, AppendToEnd);

            txtFDate.SelectedDate = DateTime.Now;

        }

        /// <summary>
        ///     加载页面数据
        /// </summary>
        private void LoadData()
        {
            switch (Actions)
            {
                case WebAction.Add:
                    Region1.Title = "添加气瓶退租单";
                    txtKeyId.Text = SequenceService.CreateSequence("TM", CurrentUser.AccountComId);
                    var temp = new LHLeaseReturn
                    {
                        KeyId = txtKeyId.Text,

                        FFlag = 1,

                        FDeleteFlag = 1,

                        //租金收款单
                        FType = Convert.ToInt32(GasEnumBill.LeaseReturn),

                        CreateBy = CurrentUser.AccountName,

                        FDate = txtFDate.SelectedDate,

                        FCompanyId = CurrentUser.AccountComId,

                        FStatus = Convert.ToInt32(GasEnumBillStauts.Add),

                        FProgress = Convert.ToInt32(GasEnumBillStauts.Add),

                    };

                    //临时写入单据
                    LeaseReturnService.Add(temp);

                    break;
                case WebAction.Edit:
                    Region1.Title = "编辑气瓶退租单";
                    txtKeyId.Text = KeyId;
                    if (LeaseReturn != null)
                    {
                        txtFCode.Text = LeaseReturn.FCode;
                        tbxFCustomer.Text = LeaseReturn.FName;

                        txtFAddress.Text = LeaseReturn.FAddress;
                        //txtFFreight.Text = LeaseReturn.FFreight.ToString();

                        txtFLinkman.Text = LeaseReturn.FLinkman;
                        txtFMemo.Text = LeaseReturn.FMemo;
                        txtFPhone.Text = LeaseReturn.FPhone;

                        ddlFDriver.Text = LeaseReturn.FDriver;
                        ddlFShipper.SelectedValue = LeaseReturn.FShipper;
                        ddlFSupercargo.Text = LeaseReturn.FSupercargo;
                        ddlFVehicleNum.SelectedValue = LeaseReturn.FVehicleNum;
                        hfdClass.Text = LeaseReturn.FCate;

                        ddlSubjectIn.SelectedValue = LeaseReturn.FSubjectCodeIn;
                        ddlSubjectOut.SelectedValue = LeaseReturn.FSubjectCodeOut;
                        txtFDepositSecurity.Text = LeaseReturn.FDepositSecurity.ToString();
                        txtFPaymentRentals.Text = LeaseReturn.FPaymentRentals.ToString();


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