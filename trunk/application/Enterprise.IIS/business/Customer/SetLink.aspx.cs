using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Enterprise.Data;
using Enterprise.Framework.EntityRepository;
using Enterprise.Framework.Enum;
using Enterprise.Framework.Web;
using Enterprise.IIS.Common;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using FineUI;
using Newtonsoft.Json.Linq;

namespace Enterprise.IIS.business.Customer
{
    public partial class SetLink : PageBase
    {
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
        ///     数据服务
        /// </summary>
        private CustomerService _customerService;

        /// <summary>
        ///     账号数据服务
        /// </summary>
        protected CustomerService CustomerService
        {
            get { return _customerService ?? (_customerService = new CustomerService()); }
            set { _customerService = value; }
        }


        /// <summary>
        ///     数据服务
        /// </summary>
        private CustomerLinkService _customerLinkService;

        /// <summary>
        ///     账号数据服务
        /// </summary>
        protected CustomerLinkService CustomerLinkService
        {
            get { return _customerLinkService ?? (_customerLinkService = new CustomerLinkService()); }
            set { _customerLinkService = value; }
        }

        /// <summary>
        ///     客户档案
        /// </summary>
        private LHCustomer _member;

        /// <summary>
        ///     职员档案
        /// </summary>
        protected LHCustomer Customer
        {
            get { return _member ?? (_member = CustomerService.FirstOrDefault(p => p.FCode == FCode && p.FCompanyId == CurrentUser.AccountComId)); }
            set { _member = value; }
        }

        /// <summary>
        ///     FCode
        /// </summary>
        protected string FCode
        {
            get { return Request["FCode"]; }
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
        ///     btnSubmit_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool isSucceed = false;

            try
            {
                GridEdit();

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
                    PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
                }
                else
                {
                    Alert.Show("提交失败！", MessageBoxIcon.Error);
                }
            }
        }

        private void GridEdit()
        {
            #region update
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

                var details = CustomerLinkService.Where(p => p.FId == datakey && p.FCompanyId == CurrentUser.AccountComId).FirstOrDefault();
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
                            if (key.Equals("FLinkman"))
                            {
                                details.FLinkman = value;
                            }

                            if (key.Equals("FAddress"))
                            {
                                details.FAddress = value;
                            }

                            if (key.Equals("FPhome"))
                            {
                                details.FPhome = value;
                            }

                            if (key.Equals("FMoile"))
                            {
                                details.FMoile = value;
                            }

                            if (key.Equals("FMemo"))
                            {
                                details.FMemo = value;
                            }

                            if (key.Equals("FType"))
                            {
                                details.FType = value;
                            }
                        }
                    }

                    #endregion
                }


                CustomerLinkService.SaveChanges();
            }
            #endregion


            var addList = Grid1.GetNewAddedList();

            #region AddRow
            foreach (var add in addList)
            {
                var sKeys = new StringBuilder();
                var sValues = new StringBuilder();
                foreach (var key in add.Keys)
                {
                    sKeys.AppendFormat("{0},", key);
                }

                foreach (var dictValue in add.Values)
                {
                    sValues.AppendFormat("{0},", dictValue);
                }

                var keys = sKeys.ToString().Split(',');
                var values = sValues.ToString().Split(',');
                var details = new LHCustomerLink();
                for (int i = 0; i < keys.Count(); i++)
                {
                    #region 修改内容


                    details.FCompanyId = CurrentUser.AccountComId;
                    details.FCode = FCode;

                    var key = keys[i];
                    var value = values[i];

                    if (!string.IsNullOrEmpty(key))
                    {

                        if (key.Equals("FLinkman"))
                        {
                            details.FLinkman = value;
                        }

                        if (key.Equals("FAddress"))
                        {
                            details.FAddress = value;
                        }

                        if (key.Equals("FPhome"))
                        {
                            details.FPhome = value;
                        }

                        if (key.Equals("FMoile"))
                        {
                            details.FMoile = value;
                        }

                        if (key.Equals("FMemo"))
                        {
                            details.FMemo = value;
                        }

                        if (key.Equals("FType"))
                        {
                            details.FType = value;
                        }
                    }

                    #endregion
                }
                CustomerLinkService.Add(details);
            }
            #endregion
        }

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
                var datakey = Convert.ToInt32(Grid1.DataKeys[e.RowIndex][1]);

                CustomerLinkService.Delete(p => p.FId == datakey && p.FCompanyId == CurrentUser.AccountComId);

                BindDataGrid();
            }
        }


        #endregion

        #region Private Method

        /// <summary>
        ///     提交编辑
        /// </summary>
        private bool SubmintEdit()
        {
            if (Customer != null)
            {

                return CustomerService.SaveChanges() >= 0;
            }
            return false;
        }

        /// <summary>
        ///     提交添加
        /// </summary>
        private bool SubmintAdd()
        {

            try
            {
                var member = new LHCustomer();
                {

                };

                return CustomerService.Add(member);
            }
            catch (Exception ex)
            {
                throw;
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
        /// <summary>
        ///     初始化页面数据
        /// </summary>
        private void InitData()
        {

            //删除选中单元格的客户端脚本
            string deleteScript = DeleteScript();

            //新增
            var defaultObj = new JObject
            {
                {"FLinkman", ""},
                {"FAddress", ""},
                {"FPhome", ""},
                {"FMoile", ""},
                {"FMemo", ""},
                {"colDelete", String.Format("<a href=\"javascript:;\" onclick=\"{0}\"><img src=\"{1}\"/></a>",//
                    deleteScript, IconHelper.GetResolvedIconUrl(Icon.Delete))},
            };

            // 在第一行新增一条数据
            btnAdd.OnClientClick = Grid1.GetAddNewRecordReference(defaultObj, AppendToEnd);


            btnClose.OnClientClick = ActiveWindow.GetHidePostBackReference();
        }

        /// <summary>
        ///     加载页面数据
        /// </summary>
        private void LoadData()
        {

            BindDataGrid();

        }

        /// <summary>
        /// 
        /// </summary>
        private void BindDataGrid()
        {
            var list = CustomerLinkService.Where(p => p.FCode == FCode && p.FCompanyId == CurrentUser.AccountComId).ToList();

            //绑定数据源
            Grid1.DataSource = list;
            Grid1.DataBind();
        }


        #endregion

    }
}