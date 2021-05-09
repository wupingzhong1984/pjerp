using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Globalization;
using System.Text;
using Enterprise.Data;
using Enterprise.Framework.EntityRepository;
using Enterprise.Framework.Enum;
using Enterprise.Framework.Utils;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using FineUI;

namespace Enterprise.IIS.Common
{
    public partial class WinBank : PageBase
    {
        /// <summary>
        ///     排序字段
        /// </summary>
        protected string SortField
        {
            get
            {
                string sort = ViewState["SortField"] != null ? ViewState["SortField"].ToString() : "FCode";
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

        private SubjectService _subjectService;
        protected SubjectService SubjectService
        {
            get { return _subjectService ?? (_subjectService = new SubjectService()); }
            set { _subjectService = value; }
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
                //关闭
                btnClose.OnClientClick = ActiveWindow.GetHidePostBackReference();

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
        ///     确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                if (Grid1.SelectedRowIndexArray.Length == 0)
                {
                    Alert.Show("请至少选择一项！", MessageBoxIcon.Information);
                }
                else
                {
                    int[] selections = Grid1.SelectedRowIndexArray;

                    var keys = new StringBuilder();

                    foreach (int t in selections)
                    {
                        keys.AppendFormat("{0},", Grid1.DataKeys[t][0]);
                    }

                    // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                    string values = keys.ToString().Substring(0, keys.Length - 1);

                    PageContext.RegisterStartupScript(//
                        string.Format(@"F.getActiveWindow().window.reloadGrid('{0}');", //
                         values) + ActiveWindow.GetHidePostBackReference());
                }
            }
            catch (Exception)
            {
                Alert.Show("添加失败！", MessageBoxIcon.Warning);
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
            Response.AddHeader("content-disposition", string.Format(@"attachment; filename=产品档案{0}.xls", SequenceGuid.GetGuidReplace()));
            Response.ContentType = "application/excel";
            Response.ContentEncoding = Encoding.UTF8;
            Response.Write(Utils.GetGridTableHtml(Grid1));
            Response.End();
        }

        protected void Window1_Close(object sender, WindowCloseEventArgs e)
        {
            BindDataGrid();
        }

        protected void Grid1_RowClick(object sender, GridRowClickEventArgs e)
        {
            string sid = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0].ToString();

            PageContext.RegisterStartupScript(//
                        string.Format(@"F.getActiveWindow().window.reloadGrid('{0}');", //
                         sid) + ActiveWindow.GetHidePostBackReference());
        }

        #endregion

        #region Private Method

        /// <summary>
        ///     获取选中的ID集合
        /// </summary>
        /// <returns></returns>
        private IEnumerable<string> GetSelectIds()
        {
            int[] selections = Grid1.SelectedRowIndexArray;

            var selectIds = new string[selections.Length];

            for (int i = 0; i < selections.Length; i++)
            {
                selectIds[i] = Grid1.DataKeys[selections[i]][0].ToString();
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
        /// 设置数据分页大小
        /// </summary>
        /// <param name="output"></param>
        private void SetPagerdg(int output)
        {
            Grid1.RecordCount = output;
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
            expr = Expression.And(expr,
                Expression.Equal(Expression.Property(parameter, "FCompanyId"), //
                Expression.Constant(CurrentUser.AccountComId, typeof(int))));

            expr = Expression.And(expr,
                     Expression.Call(Expression.Property(parameter, "FDate"), methodInfo,
                // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                         Expression.Constant(DateTime.Now.ToString("yyyy-MM"))));

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
        #endregion
    }
}