using System;
using System.Data;
using System.Data.OleDb;
using System.Transactions;
using Enterprise.Data;
using Enterprise.Framework.Extension;
using Enterprise.Framework.File;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using FineUI;

namespace Enterprise.IIS.business.Init
{
    /// <summary>
    ///     引入客户档案
    /// </summary>
    public partial class InitCustomer : PageBase
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
        ///     排序字段
        /// </summary>
        protected string SortField
        {
            get
            {
                string sort = ViewState["SortField"] != null ? ViewState["SortField"].ToString() : "客户代码";
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
        ///     引入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label1.Text = string.Format(@"引入客户档案表");
            }
        }

        #region Protected Method

        /// <summary>
        ///     关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnClose_OnClick(object sender, EventArgs e)
        {
            PageContext.RegisterStartupScript("closeActiveTab();");
        }

        /// <summary>
        ///     提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //  提配错误行数
            var errorRow = -1;
            try
            {
                using (var ts = new TransactionScope())
                {
                    var data = ViewState["dtData"] as DataTable;
                    if (data == null) return;

                    //var stringBuilder = new StringBuilder();

                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        errorRow = i + 1;

                        var item = new LHCustomer
                        {
                            FCode = data.Rows[i]["客户代码"].ToString(),
                            FName = data.Rows[i]["客户名称"].ToString(),
                            FSpell = ChineseSpell.MakeSpellCode(data.Rows[i]["客户名称"].ToString(), "",
                    SpellOptions.FirstLetterOnly).ToUpper(),
                            FPaymentMethod = data.Rows[i]["付款方式"].ToString(),
                            FLinkman = data.Rows[i]["联系人"].ToString(),
                            FPhome = data.Rows[i]["电话"].ToString(),
                            FMoile = data.Rows[i]["手机"].ToString(),
                            FAddress = data.Rows[i]["地址"].ToString(),
                            FDistric = data.Rows[i]["区域"].ToString(),

                            FIsPrint = Convert.ToInt32(data.Rows[i]["是否打印单价"]),
                            FTipsDay = Convert.ToInt32(data.Rows[i]["提醒天数"]),
                            FFreight = Convert.ToDecimal(data.Rows[i]["运输服务费"]),
                            FCredit = Convert.ToDecimal(data.Rows[i]["信用额度"]),
                            FSalesman = data.Rows[i]["业务员"].ToString(),
                            FMemo = data.Rows[i]["摘要"].ToString(),
                            FGroupNo = data.Rows[i]["客户代码"].ToString(),
                            FGroupNoFlag = "1",
                            FLevel = 0,
                           
                            //客户分类
                            FCateId="2077",
                            FSubCateId = "2077",

                            FDate = DateTime.Now,
                            FIsAllot = 0,
                            FIsInvoice = 0,
                            FCompanyId = CurrentUser.AccountComId,
                            FFlag = 1
                        };

                        CustomerService.AddEntity(item);
                    }

                    //写入数据库
                    CustomerService.SaveChanges();

                    ts.Complete();

                    Alert.Show("引入完成。", MessageBoxIcon.Information);

                }
            }
            catch (Exception ex)
            {
                Alert.Show(string.Format(@"引入失败，请排查模版内容是否完善，出错定位行：{0} ",errorRow), MessageBoxIcon.Information);
            }
        }

        protected void fileUpload1_FileSelected(object sender, EventArgs e)
        {
            if (fileUpload1.HasFile)
            {
                var fileSuffix = fileUpload1.ShortFileName.Substring(fileUpload1.ShortFileName.LastIndexOf('.'));

                var sequence = SequenceService.CreateSequence("LH",CurrentUser.AccountComId);

                var fileName = sequence + fileSuffix;

                var uploadpath = Config.GetUploadpath();

                //该目录设定死，最好不要修改
                var fileTemp = (string.Format(@"{0}/temp/{1}/", uploadpath, DateTime.Now.ToString("yyyy-MM-dd"))); //

                if (!DirFile.XFileExists(Server.MapPath(fileTemp)))
                {
                    DirFile.XCreateDir(Server.MapPath(fileTemp));
                }

                var address = fileTemp + fileName;

                fileUpload1.SaveAs(Server.MapPath(address));

                //导入
                var connStr = string.Empty;//数据连接字符串
                if (fileSuffix == ".xls")
                {
                    connStr = "Provider=Microsoft.Jet.OleDb.4.0;" + "data source=" + Server.MapPath(address) + ";Extended Properties='Excel 8.0; HDR=YES; IMEX=1'";
                }
                else if (fileSuffix == ".xlsx")
                {
                    connStr = "Provider=Microsoft.ACE.OleDb.12.0;" + "data source=" + Server.MapPath(address) + ";Extended Properties='Excel 12.0; HDR=YES; IMEX=1'";
                }

                var conn = new OleDbConnection(connStr);
                conn.Open();

                var data = new DataTable();
                const string cmdSql = "select * from [Sheet1$]";
                var da = new OleDbDataAdapter(cmdSql, conn);
                da.Fill(data);

                ViewState["dtData"] = data;
                Grid1.DataSource = data;
                Grid1.DataBind();
                Grid1.Visible = true;

                fileUpload1.Reset();
            }
        }
        #endregion
    }
}