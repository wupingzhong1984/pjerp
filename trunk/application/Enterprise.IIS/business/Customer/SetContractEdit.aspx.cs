using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Transactions;
using Enterprise.Data;
using Enterprise.Framework.Web;
using Enterprise.IIS.Common;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using FineUI;
using Newtonsoft.Json.Linq;

namespace Enterprise.IIS.business.Customer
{
    /// <summary>
    ///      发货单据编辑
    /// </summary>
    public partial class SetContractEdit : PageBase
    {
        #region  Service

        /// <summary>
        ///     FCode
        /// </summary>
        protected int Id
        {
            get { return int.Parse(Request["FID"] == null ? "0" : Request["FID"]); }
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
        #endregion

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

        private ContractDetailstServie _ContractDetailstServie;
        /// <summary>
        /// 
        /// </summary>
        protected ContractDetailstServie ContractDetailstServie
        {
            get { return _ContractDetailstServie ?? (_ContractDetailstServie = new ContractDetailstServie()); }
            set { _ContractDetailstServie = value; }
        }

        private ContractServie _ContractServie;
        /// <summary>
        /// 
        /// </summary>
        protected ContractServie ContractServie
        {
            get { return _ContractServie ?? (_ContractServie =new ContractServie()); }
            set { _ContractServie = value; }
        }

        private LHContract _contract;

        protected LHContract Contract
        {
            get { return _contract ?? (_contract = ContractServie.FirstOrDefault(p => p.FId == Id)); }
            set { _contract = value; }
        }

        private LHCustomer _customer;

        public LHCustomer Customer
        {
            get { return _customer; }
            set { _customer = value; }
        }

        private LHContractDetails _contractDetails;

        protected LHContractDetails ContractDetails
        {
            get { return _contractDetails; }
            set { _contractDetails = value; }
        }

        DataTable dt = new DataTable();

        string newKeyId = string.Empty;

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
               // 初始化控件数据
                InitData();

                //加载数据
                LoadData();
            }
            else
            {
                if (GetRequestEventArgument().Contains("reloadGrid:"))
                {
                    #region 弹窗加产品
                    //查找所选商品代码，查访产品集合
                    string keys = GetRequestEventArgument().Split(':')[1];
                    var values = keys.Split(',');
                    string codes = String.Empty;
                    for (int i = 0; i < values.Count(); i++)
                    {
                        codes += string.Format("'{0}',", values[i]);
                    }
                    codes = codes.TrimEnd(',');
                    var data = SqlService.Where(string.Format("SELECT * FROM dbo.vm_SalesItem a WHERE a.FItemCode IN ({0}) and a.FCompanyId={1}", codes, CurrentUser.AccountComId));
                    if (data != null && data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0)
                    {
                        var table = data.Tables[0];
                        for (int i = 0; i < table.Rows.Count; i++)
                        {
                            LHContractDetails details = new LHContractDetails();
                            details.FOrderCode = hiddevalue.Text;
                            details.FProductID = table.Rows[i]["FItemCode"].ToString();
                            details.FPrice = 0;
                            details.FCreatedby = CurrentUser.AccountJobNumber;
                            details.FCreatedon = DateTime.Now;
                            details.Fstate = 0;
                            ContractDetailstServie.Add(details);
                        }
                    }
                    BindData();
                }
            }
        }

        private void BindData()
        {
            var source = SqlService.Where(string.Format(@"SELECT * FROM dbo.vm_ContractDetails WHERE FOrderCode='{0}'", hiddevalue.Text));

            //绑定数据源
            Grid1.DataSource = source;
            Grid1.DataBind();
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
                switch (Actions)
                {
                    case WebAction.Add:
                        isSucceed = SubmintAdd();
                        break;

                    case WebAction.Edit:
                        isSucceed = SubmintEdit();
                        break;
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
        ///     单元格编辑与修改
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
                var datakey = Convert.ToInt32(Grid1.DataKeys[e.RowIndex][0]);

                ContractDetailstServie.Delete(p => p.FId == datakey);
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

        /// <summary>
        ///     
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tbxFProductID_OnTriggerClick(object sender, EventArgs e)
        {
            //Window1.Hidden = true;
           

        }


        #endregion
        #endregion
        #region Private Method

        /// <summary>
        ///     提交编辑
        /// </summary>
        private bool SubmintEdit()
        {
            try
            {
                using (TransactionScope socp = new TransactionScope())
                {
                    Dictionary<int, Dictionary<string, object>> modifiedDict = Grid1.GetModifiedDict();
                    
                    LHContract contract = Contract;
                    contract.FOrderCode = hiddevalue.Text;
                    contract.FContractCode = txtFContractCode.Text.Trim();
                    contract.FContractDate = dpFContractDate.SelectedDate;
                    contract.FConacter = ddlFCtroler.SelectedValue;
                    if (!string.IsNullOrEmpty(ddlFAccType.SelectedValue))
                    {
                        contract.FAccType = ddlFAccType.SelectedValue;
                    }
                    if (!string.IsNullOrEmpty(ddlFBillType.SelectedValue))
                    {
                        contract.FBillType = ddlFBillType.SelectedValue;
                    }
                    contract.FBeginDate = dpFBeginDate.SelectedDate;
                    contract.FEndDate = dpFEndDate.SelectedDate;
                    contract.FCustomer = this.txtFCustomer.Text;
                    contract.FContext = txtFContext.Text.Trim();
                    contract.FConacter = txtFConacter.Text.Trim();
                    contract.FTel = txtFTel.Text.Trim();
                    contract.FUpdateby = CurrentUser.AccountJobNumber;
                    contract.FUpdateon = DateTime.Now;

                    ContractServie.SaveChanges();
                    ModifiedGrid();
                    //UpdateDataRow(modifiedDict);
                    socp.Complete();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 
        ///     提交添加
        /// </summary>
        private bool SubmintAdd()
        {
            
            try
            {
                LHContract iscontract = ContractServie.FirstOrDefault(p => p.FOrderCode == hiddevalue.Text);
                if (iscontract != null)
                {
                    Alert.Show("该数据已经存在,不能重复保存");
                }
                using (TransactionScope socp = new TransactionScope())
                {
                   
                    LHContract contract = new LHContract();
                    contract.FOrderCode = hiddevalue.Text;
                    contract.FContractCode = txtFContractCode.Text.Trim();
                    contract.FContractDate = dpFContractDate.SelectedDate;
                    contract.FContractName = txtFContractName.Text.Trim();
                    if (!string.IsNullOrEmpty(ddlFAccType.SelectedValue))
                    {
                        contract.FAccType = ddlFAccType.SelectedValue;
                    }
                    if (!string.IsNullOrEmpty(ddlFBillType.SelectedValue))
                    {
                        contract.FBillType = ddlFBillType.SelectedValue;
                    }
                    contract.FConacter = ddlFCtroler.SelectedValue;  
                    contract.FBeginDate = dpFBeginDate.SelectedDate;
                    contract.FEndDate = dpFEndDate.SelectedDate;
                    contract.FCustomer = this.txtFCustomer.Text.Trim();
                    contract.FContext = txtFContext.Text.Trim();
                    contract.FConacter = txtFConacter.Text.Trim();
                    contract.FTel = txtFTel.Text.Trim();
                    contract.FCreatedby = CurrentUser.AccountJobNumber;
                    contract.FCreatedon = DateTime.Now;
                    contract.FDeleteflag = 0;
                    contract.FCompanyId = CurrentUser.AccountComId;
                    ContractServie.Add(contract);
                    var dictModified = Grid1.GetModifiedDict();
                    ModifiedGrid();
;                    socp.Complete();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        private void ModifiedGrid()
        {
            //编辑行事件
            var dictModified = Grid1.GetModifiedDict();
            //if (Grid1 != null && Grid1.Rows.Count > 0)
            //{
            //    for (int i = 0; i < Grid1.Rows.Count; i++)
            //    {
            //        int datakey = Convert.ToInt32(Grid1.Rows[i].DataKeys[0].ToString());
            //        var details = ContractDetailstServie.Where(p => p.FID == datakey).FirstOrDefault();
            //        details.FPrice = Convert.ToDecimal(((TextBox)Grid1.Rows[i].FindControl("tbxFPrice")).Text);
            //        ContractDetailstServie.SaveChanges();
            //    }
            //}
            foreach (var rowKey in dictModified.Keys)
            {
                int datakey = Convert.ToInt32(Grid1.DataKeys[rowKey][0].ToString());

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
                var keys = sKeys.ToString().Split(',');
                var values = sValues.ToString().Split(',');
                var details = ContractDetailstServie.Where(p => p.FId == datakey).FirstOrDefault();
                for (int i = 0; i < keys.Count(); i++)
                {
                    var key = keys[i];
                    var value = values[i];
                    if (key.Equals("FPrice"))
                    {
                        details.FPrice = Convert.ToDecimal(value);
                    }
                }
                ContractDetailstServie.SaveChanges();
            }
        }

        private void UpdateDataRow(Dictionary<int, Dictionary<string, object>> modifiedDict)
        {

            Dictionary<string, object> rowDict;
            foreach (int rowIndex in modifiedDict.Keys)
            {
                int rowId = Convert.ToInt32(Grid1.DataKeys[rowIndex][0]);
                rowDict = modifiedDict[rowIndex];
                ContractDetails = ContractDetailstServie.FirstOrDefault(p => p.FId == rowId);
                ContractDetails.FProductID = rowDict["FProductID"].ToString();
                ContractDetails.FPrice = Convert.ToDecimal(rowDict["FPrice"]);
                ContractDetails.Fstate = 1;
                ContractDetails.FUpdatedby = CurrentUser.AccountJobNumber;
                ContractDetails.FUpdatedon = DateTime.Now;
                ContractDetailstServie.SaveChanges();
                LHContractDetails lhcontract = new LHContractDetails();
                lhcontract.FOrderCode = ContractDetails.FOrderCode;
                lhcontract.FProductID = rowDict["FProductID"].ToString();
                lhcontract.FPrice = Convert.ToDecimal(rowDict["FPrice"]);
                lhcontract.Fstate = 0;
                lhcontract.FCreatedby = CurrentUser.AccountJobNumber;
                lhcontract.FCreatedon = DateTime.Now;
                ContractDetailstServie.Add(lhcontract);
            }

        }

        /// <summary>
        ///     初始化页面数据
        /// </summary>
        private void InitData()
        {
            this.tbxFProductID.OnClientTriggerClick = Window1.GetSaveStateReference(tbxFProductID.ClientID)
                    + Window1.GetShowReference("../../Common/WinProduct.aspx");
            tbxFCustomer.OnClientTriggerClick = Window2.GetSaveStateReference(this.txtFCustomer.ClientID, tbxFCustomer.ClientID)
                    + Window2.GetShowReference("../../Common/WinCustomer.aspx");
            GasHelper.DropDownListSurveyorDataBind(ddlFCtroler);
            //GasHelper.DropDownListCustomerDataBind(ddlFCustomer);
            GasHelper.DropDownListBillStatusDataBind(ddlFAccType, "1030");
            GasHelper.DropDownListBillStatusDataBind(ddlFBillType, "1031");
            //删除选中单元格的客户端脚本
            string deleteScript = DeleteScript();
            //新增
            var defaultObj = new JObject
            {             
                {"FProductID", ""},  
                {"FProductName", ""},                
                {"FSpec", ""},
                {"FUnit", ""},
                {"FPrice", "0"},
                {"colDelete", String.Format("<a href=\"javascript:;\" onclick=\"{0}\"><img src=\"{1}\"/></a>",//
                    deleteScript, IconHelper.GetResolvedIconUrl(Icon.Delete))},
            };
            
            // 在第一行新增一条数据
            btnAdd.OnClientClick = Grid1.GetAddNewRecordReference(defaultObj, false);
            //BindData();
        }

        /// <summary>
        ///     加载页面数据
        /// </summary>
        private void LoadData()
        {
            switch (Actions)
            {
                case WebAction.Add:
                    hiddevalue.Text = SequenceService.CreateSequence(DateTime.Now, "CC", CurrentUser.AccountComId);
                    BindData();
                    break;
                case WebAction.Edit:
                    
                    if (Contract != null)
                    {
                        Customer = CustomerService.FirstOrDefault(p => p.FCode == Contract.FCustomer && p.FCompanyId == CurrentUser.AccountComId);
                        if (CurrentUser != null)
                        {
                            tbxFCustomer.Text = Customer.FName;
                        }
                        hiddevalue.Text = Contract.FOrderCode;
                        txtFContractCode.Text = Contract.FContractCode;
                        dpFContractDate.SelectedDate = Contract.FContractDate;
                        txtFContractName.Text = Contract.FContractName;
                        ddlFCtroler.SelectedValue = Contract.FCtroler;
                        //txtFContractName.Text = Contract.FContractName;
                        if (!string.IsNullOrEmpty(ddlFAccType.SelectedValue))
                        {
                            ddlFAccType.SelectedValue = Contract.FAccType;
                        }
                        if (!string.IsNullOrEmpty(ddlFBillType.SelectedValue))
                        {
                            ddlFBillType.SelectedValue = Contract.FBillType;
                        }
                        dpFBeginDate.SelectedDate = Contract.FBeginDate;
                        dpFEndDate.SelectedDate = Contract.FEndDate;
                        this.txtFCustomer.Text = Contract.FCustomer;
                        
                        txtFContext.Text = Contract.FContext;
                        txtFConacter.Text = Contract.FConacter;
                        txtFTel.Text = Contract.FTel;
                        BindData();
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

        #region 上传文件
        private void UpLoadfile()
        {
            string fileName = fileUpload.ShortFileName;
            if (fileUpload.HasFile)
            {
                var file = (string.Format(@"~/upload/"+this.txtFCustomer.Text+"/"));
                //fileName = "烤盘信息表.xls";
                fileName = txtFContractCode.Text + System.IO.Path.GetExtension(fileName);
                if (!FileHelper.IsExistFile(file))
                {
                    FileHelper.CreateDirectory(file);
                }
                if (FileHelper.IsExistFile(file + fileName))
                {
                    FileHelper.DeleteFile(file + fileName);
                }
                fileUpload.SaveAs(Server.MapPath(file + fileName));
                
            }
        }
        #endregion

        protected void fileUpload_FileSelected(object sender, EventArgs e)
        {
            if (fileUpload.HasFile)
            {
                UpLoadfile();
            }
        }

       

        #endregion

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
                    this.txtFCustomer.Text = custmoer.FCode;                    
                }
            }
        }

    }
}

