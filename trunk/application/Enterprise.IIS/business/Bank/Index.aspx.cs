using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Reflection;
using Enterprise.Service.Base.ERP;
using Enterprise.Data;
using Enterprise.Framework.EntityRepository;
using Enterprise.Framework.Enum;
using Enterprise.Service.Base;
using FineUI;


namespace Enterprise.IIS.business.Bank
{
    public partial class Index : PageBase
    {
        protected string SortField
        {
            get
            {
                string sort = ViewState["SortField"] != null ? ViewState["SortField"].ToString() : "FCode";
                return sort;
            }
            set { ViewState["SortField"] = value; }
        }

        protected string SortDirection
        {
            get
            {
                string sort = ViewState["SortDirection"] != null ? ViewState["SortDirection"].ToString() : "ASC";
                return sort;
            }
            set { ViewState["SortDirection"] = value; }
        }

        private SubjectService _subjectService;
        protected SubjectService SubjectService
        {
            get { return _subjectService ?? (_subjectService = new SubjectService()); }
            set { _subjectService = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //设置权限
                SetPermissionButtons(Toolbar1);

                btnOff.ConfirmText = "你确定要禁用此银行账户吗？";

                btnStart.ConfirmText = "你确定要启用此银行账户吗？";

                BindDataGrid();

                btnAdd.OnClientClick = Window1.GetShowReference(string.Format("./Edit.aspx?action=1"), "添加银行账户");
            }
        }

        #region LoadData

        /// <summary>
        /// 绑定数据表格
        /// </summary>
        private void BindDataGrid()
        {
            int output;

            dynamic orderingSelector;

            Expression<Func<LHSubject, bool>> predicate = BuildPredicate(out orderingSelector);
            //取数据源
            IQueryable<LHSubject> list = SubjectService.Where(predicate, Grid1.PageSize, Grid1.PageIndex + 1,
                orderingSelector, EnumHelper.ParseEnumByString<OrderingOrders>(SortDirection), out output);

            //设置页面大小
            Grid1.RecordCount = output;

            //设置页面大小
            SetPagerdg(output);

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
        private Expression<Func<LHSubject, bool>> BuildPredicate(out dynamic orderingSelector)
        {
            // 查询条件表达式
            Expression expr = Expression.Constant(true);

            ParameterExpression parameter = Expression.Parameter(typeof(LHSubject));

            MethodInfo methodInfo = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            expr = Expression.And(expr,
                    Expression.Call(Expression.Property(parameter, "FParentCode"), methodInfo,
                // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant("0000100004")));

            string data = DateTime.Now.ToString("yyyy-MM");

            expr = Expression.And(expr,
                    Expression.Call(Expression.Property(parameter, "FDate"), methodInfo,
                // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant(data)));

            expr = Expression.And(expr,
                 Expression.Equal(Expression.Property(parameter, "FCompanyId"), Expression.Constant(CurrentUser.AccountComId, typeof(int))));


            //功能名称
            if (!string.IsNullOrWhiteSpace(txtFName.Text.Trim()))
            {
                expr = Expression.And(expr,
                    Expression.Call(Expression.Property(parameter, "FName"), methodInfo,
                    // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant(txtFName.Text.Trim())));
            }

            if (!string.IsNullOrWhiteSpace(txtFComment.Text.Trim()))
            {
                expr = Expression.And(expr,
                    Expression.Call(Expression.Property(parameter, "FComment"), methodInfo,
                    // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant(txtFComment.Text.Trim())));
            }

            Expression<Func<LHSubject, bool>> predicate = Expression.Lambda<Func<LHSubject, bool>>(expr, parameter);

            // 排序表达式
            orderingSelector = Expression.Lambda(Expression.Property(parameter, SortField), parameter);

            return predicate;
        }

        /// <summary>
        /// 设置数据分页大小
        /// </summary>
        /// <param name="output"></param>
        private void SetPagerdg(int output)
        {
            Grid1.RecordCount = output;
        }

        /// <summary>
        ///     获取选中的ID集合
        /// </summary>
        /// <returns></returns>
        private IEnumerable<string> GetSelectIds()
        {
            int[] selections = Grid1.SelectedRowIndexArray;

            var selectIds = new String[selections.Length];

            for (int i = 0; i < selections.Length; i++)
            {
                selectIds[i] = Grid1.DataKeys[selections[i]][0].ToString();
            }
            return selectIds;
        }

        /// <summary>
        /// 查询处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tgbSearch_OnTriggerClick(object sender, EventArgs e)
        {
            BindDataGrid();
        }
        #endregion

        #region Events

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_PageIndexChange(object sender, GridPageEventArgs e)
        {
            Grid1.PageIndex = e.NewPageIndex;
            BindDataGrid();
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid1.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
            BindDataGrid();
        }

        /// <summary>
        /// RowCommand
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_Sort(object sender, GridSortEventArgs e)
        {
            SortField = string.Format(@"{0}", e.SortField);
            SortDirection = e.SortDirection;
            BindDataGrid();
        }

        protected void Window1_Close(object sender, WindowCloseEventArgs e)
        {
            BindDataGrid();
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Redirect(string.Format(@"Edit.aspx?action=1"));
        }

        /// <summary>
        ///     作废
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBatchDelete_Click(object sender, EventArgs e)
        {
            IEnumerable<string> selectIds = GetSelectIds();

            try
            {
                SubjectService.Delete(p => p.FCompanyId == CurrentUser.AccountComId && selectIds.Contains(p.FCode));

                Alert.Show("删除成功！", MessageBoxIcon.Information);

                BindDataGrid();
            }
            catch (Exception)
            {
                Alert.Show("删除失败！", MessageBoxIcon.Warning);
            }
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindDataGrid();
        }
        /// <summary>
        ///     禁用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnOff_Click(object sender, EventArgs e)
        {
            IEnumerable<string> selectIds = GetSelectIds();
            try
            {
                SubjectService.Update(p => selectIds.Contains(p.FCode), p => new LHSubject { FFlag = 0 });
                Alert.Show("禁用成功！", MessageBoxIcon.Information);
                BindDataGrid();
            }
            catch (Exception)
            {
                Alert.Show("禁用失败！", MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        ///     启用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnStart_Click(object sender, EventArgs e)
        {
            IEnumerable<string> selectIds = GetSelectIds();
            try
            {
                SubjectService.Update(p => selectIds.Contains(p.FCode), p => new LHSubject { FFlag = 1 });
                Alert.Show("启用成功！", MessageBoxIcon.Information);
                BindDataGrid();
            }
            catch (Exception)
            {
                Alert.Show("启用失败！", MessageBoxIcon.Warning);
            }
        }

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
                    string sid = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0].ToString();

                    PageContext.RegisterStartupScript(
                        Window1.GetShowReference(string.Format("./Edit.aspx?FCode={0}&action=2",
                            sid), "编辑银行账户"));
                }
            }
            catch (Exception)
            {
                Alert.Show("编辑失败！", MessageBoxIcon.Warning);
            }
        }
        #endregion
    }
}