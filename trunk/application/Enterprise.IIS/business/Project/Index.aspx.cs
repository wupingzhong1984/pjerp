using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Enterprise.Framework.Utils;
using Enterprise.IIS.Common;
using Enterprise.Framework.EntityRepository;
using Enterprise.Framework.Enum;
using Enterprise.Service.Base;
using Enterprise.Data;
using Enterprise.Service.Base.ERP;
using FineUI;

namespace Enterprise.IIS.business.Project
{

    /// <summary>
    ///     数据字典
    /// </summary>
    public partial class Index : PageBase
    {
        /// <summary>
        ///     排序字段
        /// </summary>
        protected string SortField
        {
            get
            {
                string sort = ViewState["SortField"] != null ? ViewState["SortField"].ToString() : "FId";
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
        private ProjectService _projectService;

        /// <summary>
        ///     数据服务
        /// </summary>
        private ProjectItemsService _projectItemsService;

        /// <summary>
        ///     _projects
        /// </summary>
        private IList<LHProject> _projects;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected ProjectItemsService ProjectItemsService
        {
            get { return _projectItemsService ?? (_projectItemsService = new ProjectItemsService()); }
            set { _projectItemsService = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        protected ProjectService ProjectService
        {
            get { return _projectService ?? (_projectService = new ProjectService()); }
            set { _projectService = value; }
        }

        /// <summary>
        ///     Projects
        /// </summary>
        protected IList<LHProject> Projects
        {
            get
            {
                return _projects ?? (_projects = ProjectService.Where(p => p.FFlag == 1//
                    && p.FIsTree==0//列表
                    && p.FCompanyId == CurrentUser.AccountComId).ToList());
            }
            set { _projects = value; }

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

                btnBatchDelete.OnClientClick = Grid1.GetNoSelectionAlertReference("至少选择一行！");

                btnAdd.OnClientClick = Window1.GetShowReference(string.Format("./Edit.aspx?id=0&action=1&pOrgid={0}",
                    trDept.SelectedNodeID), "添加字典");
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

            btnAdd.OnClientClick = Window1.GetShowReference(string.Format("./Edit.aspx?id=0&action=1&pOrgid={0}",
                trDept.SelectedNodeID), "添加字典");
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
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            //IEnumerable<string> selectIds = GetSelectIds();

            try
            {
                var sid = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0].ToString();
                var item = ProjectItemsService.FirstOrDefault(p => p.FId == sid);
                item.FFlag = item.FFlag == 1 ? 0 : 1;

                if (ProjectItemsService.SaveChanges() > 0)
                {
                    BindDataGrid();

                    Alert.Show("提交成功。", MessageBoxIcon.Information);
                    //记录日志
                    //Log(string.Format(@"禁用帐号{0}成功。", account.account_number));
                }
                else
                {
                    Alert.Show("提交失败，请重试！", MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Alert.Show("提交失败！", MessageBoxIcon.Warning);
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
                        Window1.GetShowReference(string.Format("./Edit.aspx?id={0}&pOrgid={1}&action=2",
                            sid, trDept.SelectedNodeID), "编辑数据字典"));
                }
            }
            catch (Exception)
            {
                Alert.Show("删除失败！", MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 导出本页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.ClearContent();
            Response.AddHeader("content-disposition", string.Format(@"attachment; filename=数据字典{0}.xls", SequenceGuid.GetGuidReplace()));
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
            var orgnization = e.DataItem as LHProjectItems;
            if (orgnization != null)
            {
                e.Values[4] = e.Values[4].ToString() == "1" ? "启用" : "禁用";
            }
        }

        #endregion

        #region Private Method

        /// <summary>
        ///     树绑定
        /// </summary>
        private void LoadTreeSource()
        {
            //加载
            LoadNodes();
        }

        /// <summary>
        ///     绑定子节点
        /// </summary>
        private void LoadNodes()
        {
            trDept.Nodes.Clear();

            foreach (var tree in Projects)
            {
                var cNode = new TreeNode
                {
                    Text = string.Format(@"{0}-{1}", tree.FId, tree.FName),
                    NodeID = tree.FId.ToString(CultureInfo.InvariantCulture),
                    EnableClickEvent = true,
                    Expanded = false
                };

                //加载子部门信息
                trDept.Nodes.Add(cNode);
            }
        }

        /// <summary>
        ///     获取选中的ID集合
        /// </summary>
        /// <returns></returns>
        private IEnumerable<string> GetSelectIds()
        {
            var selections = Grid1.SelectedRowIndexArray;

            var selectIds = new String[selections.Length];

            for (int i = 0; i < selections.Length; i++)
            {
                selectIds[i] = (Grid1.DataKeys[selections[i]][0].ToString());
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
            Expression<Func<LHProjectItems, bool>> predicate = BuildPredicate(out orderingSelector);

            //取数据源
            IQueryable<LHProjectItems> list = ProjectItemsService.Where(predicate, Grid1.PageSize, Grid1.PageIndex + 1,
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
        private Expression<Func<LHProjectItems, bool>> BuildPredicate(out dynamic orderingSelector)
        {
            // 查询条件表达式
            Expression expr = Expression.Constant(true);

            ParameterExpression parameter = Expression.Parameter(typeof(LHProjectItems));
            MethodInfo methodInfo = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            //expr = Expression.And(expr,
            //    Expression.Equal(Expression.Property(parameter, "FCompanyId"), Expression.Constant(CurrentUser.AccountComId, typeof(int?))));

            if (!string.IsNullOrWhiteSpace(trDept.SelectedNodeID))
            {
                //expr = Expression.And(expr,
                //    Expression.Equal(Expression.Property(parameter, "FSParent"),
                //        Expression.Constant(trDept.SelectedNodeID), typeof(Int32?))));

                expr = Expression.And(expr,
                    Expression.Call(Expression.Property(parameter, "FSParent"), methodInfo,
                    // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant(trDept.SelectedNodeID)));
            }

            Expression<Func<LHProjectItems, bool>> predicate = Expression.Lambda<Func<LHProjectItems, bool>>(expr, parameter);

            // 排序表达式
            orderingSelector = Expression.Lambda(Expression.Property(parameter, SortField), parameter);

            return predicate;
        }

        #endregion
    }
}