using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Globalization;
using System.Text;
using Enterprise.IIS.Common;
using FineUI;
using Enterprise.Data;
using Enterprise.Framework.Utils;
using Enterprise.Framework.EntityRepository;
using Enterprise.Framework.Enum;
using Enterprise.Service.Base;
using Enterprise.Service.Base.Platform;

namespace Enterprise.IIS.product.employee
{
    // ReSharper disable once InconsistentNaming
    public partial class list : PageBase
    {
        /// <summary>
        ///     帐号数据服务
        /// </summary>
        private EmployeeService _employeeService;

        /// <summary>
        ///     公司组织数据服务
        /// </summary>
        private CompanyService _companyService;

        /// <summary>
        ///     部门组织数据服务
        /// </summary>
        private OrgnizationService _orgnizationService;

        /// <summary>
        ///     部门列表
        /// </summary>
        private IList<base_orgnization> _orgnizations;

        /// <summary>
        ///     排序字段
        /// </summary>
        protected string SortField
        {
            get
            {
                string sort = ViewState["SortField"] != null ? ViewState["SortField"].ToString() : "id";
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
        ///     部门组织数据服务
        /// </summary>
        protected OrgnizationService OrgnizationService
        {
            get { return _orgnizationService ?? (_orgnizationService = new OrgnizationService()); }
            set { _orgnizationService = value; }
        }

        /// <summary>
        ///     公司组织数据服务
        /// </summary>
        protected CompanyService CompanyService
        {
            get { return _companyService ?? (_companyService = new CompanyService()); }
            set { _companyService = value; }
        }

        /// <summary>
        ///     帐号数据服务
        /// </summary>
        protected EmployeeService EmployeeService
        {
            get { return _employeeService ?? (_employeeService = new EmployeeService()); }
            set { _employeeService = value; }
        }

        /// <summary>
        ///     部门列表
        /// </summary>
        protected IList<base_orgnization> Orgnizations
        {
            get
            {
                return _orgnizations ?? (_orgnizations = OrgnizationService.Where(p => p.deleteflag == 0).ToList());
            }
            set { _orgnizations = value; }
        }

        #region Protected Method

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void trDept_NodeExpand(object sender, TreeExpandEventArgs e)
        //{
        //    LoadChildNodes(e.Node);
        //}

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

                btnBatchDelete.ConfirmText = "你确定要执行删除操作吗？";

                //加载部门树
                LoadTreeSource();

                btnAdd.OnClientClick = Window1.GetShowReference(string.Format("./edit.aspx?action=1&orgid={0}",
                    trDept.SelectedNodeID), "添加职工");

                BindDataGrid();

            }
        }

        /// <summary>
        ///     部门树加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void trDept_NodeCommand(object sender, TreeCommandEventArgs e)
        {
            BindDataGrid();
            if (trDept.SelectedNodeID != "1")
            {
                btnAdd.OnClientClick = Window1.GetShowReference(string.Format("./edit.aspx?action=1&orgid={0}",
                    trDept.SelectedNodeID), "添加职工");
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
        ///     删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBatchDelete_Click(object sender, EventArgs e)
        {
            IEnumerable<int> selectIds = GetSelectIds();
            try
            {
                Log(string.Format(@"删除帐号记录ID:{0}成功。", selectIds));
                EmployeeService.Update(p => selectIds.Contains(p.id), p => new base_employee { deleteflag = 1 });
                Alert.Show("删除成功！", MessageBoxIcon.Information);
                BindDataGrid();
            }
            catch (Exception)
            {
                Alert.Show("删除失败！", MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 注册帐号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddAcount_Click(object sender, EventArgs e)
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
                    int sid = Convert.ToInt32(Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0]);

                    var employee = EmployeeService.Where(p => p.id == sid).FirstOrDefault();

                    if (employee != null)
                    {

                        var user =
                            new AccountService().Where(p => p.account_number == employee.job_number).FirstOrDefault();

                        if (user != null)
                        {
                            Alert.Show("帐户已被注册，请核实身份！", MessageBoxIcon.Warning);
                            return;
                        }

                        int listCount = new AccountService().Count(p => p.account_org_id == employee.orgnization_id);

                        var account = new base_account
                        {
                            account_name = employee.name,
                            account_number = employee.job_number,
                            account_org_id = employee.orgnization_id,
                            // account_org_name = employee.orgnization_name,
                            account_sex = employee.sex,
                            code_cn = employee.code_cn,
                            account_flag = 1,
                            //account_mobile_phone = employee.office_phone,
                            account_qq = employee.qq,
                            //account_email = employee.email,
                            //account_password = EncryptUtil.Encrypt("123456"),

                            account_password = "ffqs654321",
                            account_sort = listCount + 1,
                            createdby_id = CurrentUser.Id,
                            createdby_name = CurrentUser.AccountName,
                            createdon = DateTime.Now,
                            deleteflag = 0,
                            FCompanyId = CurrentUser.AccountComId,
                            FFlag = 1
                        };

                        new AccountService().Add(account);
                        Alert.Show("注册帐号成功！", MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception)
            {
                Alert.Show("注册帐号失败！", MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 办理离职
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDimission_Click(object sender, EventArgs e)
        {
            IEnumerable<int> selectIds = GetSelectIds();
            try
            {
                EmployeeService.Update(p => selectIds.Contains(p.id), p => new base_employee { flag = 1, flag_date = DateTime.Now });

                BindDataGrid();

                Alert.Show("办理离职成功！", MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                Alert.Show("办理离职失败！", MessageBoxIcon.Warning);
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
                    int sid = Convert.ToInt32(Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0]);
                    PageContext.RegisterStartupScript(
                        Window1.GetShowReference(string.Format("./edit.aspx?id={0}&orgid={1}&action=2",
                            sid, trDept.SelectedNodeID), "编辑职工"));
                }
            }
            catch (Exception)
            {
                Alert.Show("编辑失败！", MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        ///     禁用/启用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnabled_Click(object sender, EventArgs e)
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
                    int sid = Convert.ToInt32(Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0]);
                    var account = EmployeeService.FirstOrDefault(p => p.id == sid);
                    account.flag = account.flag == 1 ? 0 : 1;

                    if (EmployeeService.SaveChanges() > 0)
                    {
                        BindDataGrid();
                        Alert.Show("提交成功。", MessageBoxIcon.Information);
                        //记录日志
                        Log(string.Format(@"禁用帐号{0}成功。", account.id));
                    }
                    else
                    {
                        Alert.Show("提交失败，请重试！", MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception)
            {
                Alert.Show("删除失败！", MessageBoxIcon.Warning);
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

        /// <summary>
        /// 导出本页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.ClearContent();
            Response.AddHeader("content-disposition", string.Format(@"attachment; filename=职工{0}.xls", SequenceGuid.GetGuidReplace()));
            Response.ContentType = "application/excel";
            Response.ContentEncoding = Encoding.UTF8;
            Response.Write(Utils.GetGridTableHtml(Grid1));
            Response.End();
        }

        protected void Window1_Close(object sender, WindowCloseEventArgs e)
        {
            BindDataGrid();
        }

        #endregion

        #region Private Method

        /// <summary>
        ///     树绑定
        /// </summary>
        private void LoadTreeSource()
        {
            trDept.Nodes.Clear();

            base_company company = CompanyService.FirstOrDefault(p => p.deleteflag == 0 && p.id == CurrentUser.AccountComId);
            var rootNode = new TreeNode
            {
                Text = company.com_name,
                NodeID = company.id.ToString(CultureInfo.InvariantCulture),
                EnableClickEvent = true,
                Expanded = true
            };

            trDept.Nodes.Add(rootNode);

            trDept.SelectedNodeID = company.id.ToString();

            //加载子部门信息
            LoadChildNodes(rootNode);

        }

        /// <summary>
        ///     绑定子节点
        /// </summary>
        /// <param name="node"></param>
        private void LoadChildNodes(TreeNode node)
        {
            var nodeId = Int32.Parse(node.NodeID);

            if (OrgnizationService.Count(p => p.org_parent_id == nodeId && p.deleteflag == 0) == 0)
            {
                node.Leaf = true;
            }
            else
            {
                node.Expanded = true;
                node.Nodes.Clear();
                foreach (
                    base_orgnization orgnization in
                        OrgnizationService.Where(p => p.org_parent_id == nodeId && p.deleteflag == 0))
                {
                    var cNode = new TreeNode
                    {
                        Text = string.Format(@"{0}-{1}", orgnization.code, orgnization.org_name),
                        NodeID = orgnization.id.ToString(CultureInfo.InvariantCulture),
                        EnableClickEvent = true,
                        Expanded = false
                    };

                    //加载子部门信息
                    node.Nodes.Add(cNode);

                    LoadChildNodes(cNode);
                }
            }
        }

        /// <summary>
        ///     获取选中的ID集合
        /// </summary>
        /// <returns></returns>
        private IEnumerable<int> GetSelectIds()
        {
            int[] selections = Grid1.SelectedRowIndexArray;

            var selectIds = new int[selections.Length];

            for (int i = 0; i < selections.Length; i++)
            {
                selectIds[i] = Int32.Parse(Grid1.DataKeys[selections[i]][0].ToString());
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
            Expression<Func<base_employee, bool>> predicate = BuildPredicate(out orderingSelector);

            //取数据源
            IQueryable<base_employee> list = EmployeeService.Where(predicate, Grid1.PageSize, Grid1.PageIndex + 1,
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
        private Expression<Func<base_employee, bool>> BuildPredicate(out dynamic orderingSelector)
        {
            // 查询条件表达式
            Expression expr = Expression.Constant(true);

            ParameterExpression parameter = Expression.Parameter(typeof(base_employee));
            MethodInfo methodInfo = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            expr = Expression.And(expr,
                Expression.Equal(Expression.Property(parameter, "deleteflag"), Expression.Constant(0, typeof(int?))));

            expr = Expression.And(expr,
            Expression.Equal(Expression.Property(parameter, "FCompanyId"),
            Expression.Constant(CurrentUser.AccountComId, typeof(int?))));


            if (trDept.SelectedNodeID != "1")
            {
                expr = Expression.And(expr,
                    Expression.Equal(Expression.Property(parameter, "orgnization_id"),
                        Expression.Constant(Int32.Parse(trDept.SelectedNodeID), typeof(Int32?))));
            }

            // 姓名
            if (!string.IsNullOrWhiteSpace(txtaccount_name.Text))
            {
                expr = Expression.And(expr,
                    Expression.Call(Expression.Property(parameter, "name"), methodInfo,
                    // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant(txtaccount_name.Text.Trim())));
            }

            if (!string.IsNullOrWhiteSpace(txtcode_cn.Text))
            {
                expr = Expression.And(expr,
                    Expression.Call(Expression.Property(parameter, "code_cn"), methodInfo,
                    // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant(txtcode_cn.Text.Trim())));
            }


            //if (!rbtList.SelectedValue.Equals("-1"))
            //{
            //    int flag = Convert.ToInt32(rbtList.SelectedValue);

            //    expr = Expression.And(expr,
            //    Expression.Equal(Expression.Property(parameter, "fflag"),
            //    Expression.Constant(flag, typeof(int?))));
            //}


            Expression<Func<base_employee, bool>> predicate = Expression.Lambda<Func<base_employee, bool>>(expr, parameter);

            // 排序表达式
            orderingSelector = Expression.Lambda(Expression.Property(parameter, SortField), parameter);

            return predicate;
        }

        #endregion
    }
}