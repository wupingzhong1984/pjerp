using Enterprise.Data;
using Enterprise.Framework.Web;
using Enterprise.IIS.Common;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using FineUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Enterprise.IIS.business.AllotTrans
{
    public partial class Details : PageBase
    { /// <summary>
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
        private CustomerService _customerService;

        /// <summary>
        ///     数据服务
        /// </summary>
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
        private StockOutDetailsService _stockOutDetailsService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected StockOutDetailsService StockOutDetailsService
        {
            get { return _stockOutDetailsService ?? (_stockOutDetailsService = new StockOutDetailsService()); }
            set { _stockOutDetailsService = value; }
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
        ///     
        /// </summary>
        private LHStockOut _stockOut;

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
        ///     职员档案
        /// </summary>
        protected LHStockOut StockOut
        {
            get { return _stockOut ?? (_stockOut = StockOutService.FirstOrDefault(p => p.KeyId == KeyId && p.FCompanyId == CurrentUser.AccountComId)); }
            set { _stockOut = value; }
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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //初始化控件数据
                InitData();

                //加载数据
                LoadData();
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
        }

        /// <summary>
        /// 
        /// </summary>
        private void BindDataGrid()
        {
            var source = SqlService.Where(string.Format(@"SELECT * FROM dbo.vm_SalesDetails WHERE keyId='{0}' and FCompanyId={1}", txtKeyId.Text, CurrentUser.AccountComId));

            //绑定数据源
            Grid1.DataSource = source.Tables[0];
            Grid1.DataBind();


            //----------------------------------------------------------
            var dataStatus = SqlService.Where(string.Format(@"SELECT FId
      ,KeyId
      ,a.FCompanyId
      ,FDeptId
      ,b.org_name FDeptName
      ,FOperator
      ,FDate
      ,FActionName
      ,FMemo
  FROM dbo.LHBillStatus a
  LEFT JOIN dbo.base_orgnization b ON a.FDeptId=b.id WHERE KeyId='{0}' and a.FCompanyId={1}", txtKeyId.Text, CurrentUser.AccountComId));
            //绑定数据源
            Grid2.DataSource = dataStatus;
            Grid2.DataBind();
            //----------------------------------------------------------
            var dataFlow = SqlService.Where(string.Format(@"SELECT FId
      ,KeyId
      ,a.FCompanyId
      ,FDeptId
      ,b.org_name FDeptName
      ,FOperator
      ,FDate
      ,FMemo
  FROM dbo.LHBillFlow a
  LEFT JOIN dbo.base_orgnization b ON a.FDeptId=b.id WHERE KeyId='{0}' and a.FCompanyId={1}", txtKeyId.Text, CurrentUser.AccountComId));
            //绑定数据源
            Grid3.DataSource = dataFlow;
            Grid3.DataBind();
            //----------------------------------------------------------

            var log = SqlService.Where(string.Format(@"SELECT * FROM dbo.vm_SalesDetailsLog a WHERE keyId='{0}' and a.FCompanyId={1}", txtKeyId.Text, CurrentUser.AccountComId));

            //绑定数据源
            Grid4.DataSource = log;
            Grid4.DataBind();

        }

        /// <summary>
        ///     分页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lblPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
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
        ///     关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Window1_Close(object sender, WindowCloseEventArgs e)
        {
        }

        #region Private Method

        /// <summary>
        ///     初始化页面数据
        /// </summary>
        private void InitData()
        {
        }

        /// <summary>
        ///     加载页面数据
        /// </summary>
        private void LoadData()
        {
            switch (Actions)
            {
                case WebAction.Details:
                    txtKeyId.Text = KeyId;
                    if (StockOut != null)
                    {
                        WebControlHandler.BindObjectToControls(StockOut, SimpleForm1);
                        tbxFCustomer.Text = StockOut.FName;
                        txtFMemo.Text = StockOut.FMemo;
                        ddlFDriver.Text = StockOut.FDriver;
                        ddlFShipper.Text = StockOut.FShipper;
                        ddlFSupercargo.Text = StockOut.FSupercargo;
                        ddlFVehicleNum.Text = StockOut.FVehicleNum;
                        txtFDate.Text = Convert.ToDateTime(StockOut.FDate).ToString("yyyy-MM-dd");
                        tbxFLogisticsNumber.Text = StockOut.FLogisticsNumber;
                        txtCreateBy.Text = StockOut.CreateBy;
                        txtKeyId.Text = StockOut.KeyId;
                        ddlFSupercargo.Text = StockOut.FSupercargo;
                        txtFMemo.Text = StockOut.FMemo;
                        ddlFSendee.Text = StockOut.FSendee;
                        ddlFPoint.Text = StockOut.FPoint;
                        ddlFDistributionPoint.Text = StockOut.FDistributionPoint;
                        ddlDeliveryMethod.Text = StockOut.FDeliveryMethod;
                        ddlFSalesman.Text = StockOut.FSalesman;
                        txtFCode.Text = StockOut.FCode;
                        //发货单据
                        BindDataGrid();

                    }
                    break;
            }
        }

        #endregion
    }
}