using System;
using Enterprise.Data;
using Enterprise.Framework.Web;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using FineUI;
using Newtonsoft.Json.Linq;

namespace Enterprise.IIS.business.FinanceFK
{
    public partial class Details : PageBase
    {
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
        private FKOrderService _FKOrderService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected FKOrderService FKOrderService
        {
            get { return _FKOrderService ?? (_FKOrderService = new FKOrderService()); }
            set { _FKOrderService = value; }
        }

        #region 操作日志部分

        #endregion

        /// <summary>
        ///     
        /// </summary>
        private LHFKOrder _skOrder;

        /// <summary>
        ///     
        /// </summary>
        protected LHFKOrder FKOrder
        {
            get { return _skOrder ?? (_skOrder = FKOrderService.FirstOrDefault(p => p.keyId == KeyId&&p.FCompanyId==CurrentUser.AccountComId)); }
            set { _skOrder = value; }
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
        /// 
        /// </summary>
        private void BindDataGrid()
        {
            var source = SqlService.Where(string.Format(@"SELECT * FROM dbo.LHFKOrderBanks WHERE keyId='{0}' and FCompanyId={1}", txtKeyId.Text,CurrentUser.AccountComId));

            //绑定数据源
            Grid1.DataSource = source;
            Grid1.DataBind();

            var table = source.Tables[0];

            if (table != null && table.Rows.Count > 0)
            {
                decimal sumFAmt = 0.00M;

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    sumFAmt += Convert.ToDecimal(table.Rows[i]["FAmt"]);
                }

                var summary = new JObject
                {
                    {"FCode", "合计"},
                    {"FAmt", sumFAmt},
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

        #endregion

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
                    if (FKOrder != null)
                    {
                        WebControlHandler.BindObjectToControls(FKOrder, SimpleForm1);
                        txtFCode.Text = FKOrder.FCode;
                        tbxFCustomer.Text = FKOrder.FName;
                        txtFMemo.Text = FKOrder.FMemo;
                        BindDataGrid();
                    }
                    break;
            }
        }
        #endregion
    }
}