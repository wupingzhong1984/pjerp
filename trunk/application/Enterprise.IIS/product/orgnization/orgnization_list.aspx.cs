using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Enterprise.Framework.Utils;
using Enterprise.IIS.Common;
using FineUI;
using Enterprise.Framework.EntityRepository;
using Enterprise.Framework.Enum;
using Enterprise.Service.Base;
using Enterprise.Service.Base.Platform;
using Enterprise.Data;
using Enterprise.OrgChart;

// ReSharper disable once CheckNamespace
namespace Enterprise.IIS.product.orgnization
{
    // ReSharper disable once InconsistentNaming
    public partial class orgnization_list : PageBase
    {
        /// <summary>
        ///     公司组织数据服务
        /// </summary>
        private CompanyService _companyService;

        /// <summary>
        ///     部门组织数据服务
        /// </summary>
        private OrgnizationService _orgnizationService;

        private EmployeeService _employeeService;
        /// <summary>
        /// 
        /// </summary>
        protected EmployeeService EmployeeService
        {
            get { return _employeeService ?? (_employeeService = new EmployeeService()); }
            set { _employeeService = value; }
        }

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
        ///     部门列表
        /// </summary>
        protected IList<base_orgnization> Orgnizations
        {
            get
            {
                return _orgnizations ?? (_orgnizations = OrgnizationService.Where(p => p.deleteflag == 0 && p.FCompanyId == CurrentUser.AccountComId).ToList());
            }
            set { _orgnizations = value; }
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

                btnBatchDelete.ConfirmText = "你确定要执行删除操作吗？";

                //加载部门树
                LoadTreeSource();
                
                BindDataGrid();

                InitOrgChart();

                btnBatchDelete.OnClientClick = Grid1.GetNoSelectionAlertReference("至少选择一行！");
                btnAdd.OnClientClick = Window1.GetShowReference(string.Format("./orgnization_edit.aspx?id=0&action=1&pOrgid={0}",
                    trDept.SelectedNodeID), "添加部门");
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

            btnAdd.OnClientClick = Window1.GetShowReference(string.Format("./orgnization_edit.aspx?id=0&action=1&pOrgid={0}",
                trDept.SelectedNodeID), "添加部门");
        }

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
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            IEnumerable<int> selectIds = GetSelectIds();

            try
            {
                OrgnizationService.Update(p => selectIds.Contains(p.id), p => new base_orgnization { deleteflag = 1 });
                Alert.Show("删除成功！", MessageBoxIcon.Information);
                BindDataGrid();
                LoadTreeSource();
            }
            catch (Exception)
            {
                Alert.Show("删除失败！", MessageBoxIcon.Warning);
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
                        Window1.GetShowReference(string.Format("./orgnization_edit.aspx?id={0}&pOrgid={1}&action=2",
                            sid, trDept.SelectedNodeID), "编辑部门"));
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
            Response.AddHeader("content-disposition", string.Format(@"attachment; filename=组织机构{0}.xls", SequenceGuid.GetGuidReplace()));
            Response.ContentType = "application/excel";
            Response.ContentEncoding = Encoding.UTF8;
            Response.Write(Utils.GetGridTableHtml(Grid1));
            Response.End();
        }

        protected void Window1_Close(object sender, WindowCloseEventArgs e)
        {
        }

        protected void Grid1_RowDataBound(object sender, GridRowEventArgs e)
        {
            var orgnization = e.DataItem as base_orgnization;
            if (orgnization != null)
            {
                e.Values[3] = e.Values[3].ToString() == "1" ? "部门" : "组";
            }
        }

        #endregion

        #region Private Method

        /// <summary>
        ///     树绑定
        /// </summary>
        private void LoadTreeSource()
        {
            trDept.Nodes.Clear();

            base_company company = CompanyService.FirstOrDefault(p => p.deleteflag == 0 && p.id== CurrentUser.AccountComId );
            var rootNode = new TreeNode
            {
                Text = company.com_name,
                NodeID = company.id.ToString(CultureInfo.InvariantCulture),
                Expanded = true,
                EnableClickEvent = true,
            };

            trDept.Nodes.Add(rootNode);

            //加载子部门信息
            LoadChildNodes(rootNode);

            //设置默认选择项
            trDept.SelectedNodeID = rootNode.NodeID;

           
        }

        /// <summary>
        ///     绑定子节点
        /// </summary>
        /// <param name="node"></param>
        private void LoadChildNodes(TreeNode node)
        {
            if (node == null)
                return;

            var nodeId = Int32.Parse(node.NodeID);

            if (OrgnizationService.Count(p => p.org_parent_id == nodeId && p.deleteflag == 0) == 0)
            {
                node.Leaf = true;
            }
            else
            {
                node.Expanded = true;
                node.Nodes.Clear();
                foreach (base_orgnization orgnization in OrgnizationService.Where(p => p.org_parent_id == nodeId && p.deleteflag == 0).OrderBy(p=>p.code))
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
            var selections = Grid1.SelectedRowIndexArray;

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
            Expression<Func<base_orgnization, bool>> predicate = BuildPredicate(out orderingSelector);

            //取数据源
            IQueryable<base_orgnization> list = OrgnizationService.Where(predicate, Grid1.PageSize, Grid1.PageIndex + 1,
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
        private Expression<Func<base_orgnization, bool>> BuildPredicate(out dynamic orderingSelector)
        {
            // 查询条件表达式
            Expression expr = Expression.Constant(true);

            ParameterExpression parameter = Expression.Parameter(typeof(base_orgnization));

            expr = Expression.And(expr,
            Expression.Equal(Expression.Property(parameter, "FCompanyId"), Expression.Constant(CurrentUser.AccountComId, typeof(int?))));

            expr = Expression.And(expr,
                Expression.Equal(Expression.Property(parameter, "deleteflag"), Expression.Constant(0, typeof(int?))));

            if (!string.IsNullOrWhiteSpace(trDept.SelectedNodeID))
            {
                expr = Expression.And(expr,
                    Expression.Equal(Expression.Property(parameter, "org_parent_id"),
                        Expression.Constant(Int32.Parse(trDept.SelectedNodeID), typeof(Int32?))));
            }

            Expression<Func<base_orgnization, bool>> predicate = Expression.Lambda<Func<base_orgnization, bool>>(expr, parameter);

            // 排序表达式
            orderingSelector = Expression.Lambda(Expression.Property(parameter, SortField), parameter);

            return predicate;
        }

        /// <summary>
        /// 初始化组织结构图
        /// </summary>
        private void InitOrgChart()
        {
            var company = CompanyService.FirstOrDefault(p => p.deleteflag == 0);
            var orgNode = new OrgNode
            {
                Text = company.com_name,
                ID = company.id.ToString(CultureInfo.InvariantCulture),
                Description = company.com_name,
                UnderDesc = string.Format(@"<font size='2.5px'>董事长:{0}</br>电话：{1}</br>公司总人数：{2}(位)</br></font>",//
                        company.com_chairman, company.com_tel, EmployeeService.Where(p => p.deleteflag == 0).Count())
            };

            //加载子部门信息
            LoadChildNodes(orgNode);

            OrgChart1.Node = orgNode;
            OrgChart1.ChartStyle = Orientation.Vertical; //:Orientation.Horizontal;//
        }

        /// <summary>
        ///     绑定子节点
        /// </summary>
        /// <param name="node"></param>
        private void LoadChildNodes(OrgNode node)
        {
            if (node == null)
                return;

            var nodeId = Int32.Parse(node.ID);

            if (OrgnizationService.Count(p => p.org_parent_id == nodeId && p.deleteflag == 0) == 0)
            {
            }
            else
            {
                foreach (base_orgnization orgnization in OrgnizationService.Where(p => p.org_parent_id == nodeId && p.deleteflag == 0))
                {
                    var key = orgnization.id;
                    var cNode = new OrgNode
                    {
                        Text = string.Format(@"{0}-{1}", orgnization.code, orgnization.org_name),
                        ID = orgnization.id.ToString(CultureInfo.InvariantCulture),
                        Description = orgnization.org_name,
                        UnderDesc = string.Format(@"<font size='2.5px'>部门主管:{0}</br>电话：{1}</br>部门总人数：{2}(位)</br></font>",//
                        orgnization.org_account_name, orgnization.org_office_tel, EmployeeService.Where(p => p.orgnization_id == key && p.deleteflag == 0).Count())
                    };
                    //加载子部门信息
                    node.Nodes.Add(cNode);

                    //递归加截
                    LoadChildNodes(cNode);
                }
            }
        }
        #endregion
    }
}