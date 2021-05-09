using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FineUI;
using Enterprise.Data;
using Enterprise.Service.Base;
using Enterprise.Service.Base.Platform;
using Enterprise.Framework.Enum;
using Enterprise.Framework.EntityRepository;


// ReSharper disable once CheckNamespace
namespace Enterprise.IIS.product.LHDictionary
{
// ReSharper disable once InconsistentNaming
    public partial class dictionary_list : PageBase
    {

        /// <summary>
        ///     字典数据服务
        /// </summary>
        private DictionaryService _dictionaryService;

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
        ///     字典数据服务
        /// </summary>
        protected DictionaryService DictionaryService
        {
            get { return _dictionaryService ?? (_dictionaryService = new DictionaryService()); }
            set { _dictionaryService = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetPermissionButtons(Toolbar1);

                btnBatchDelete.ConfirmText = "你确定要执行删除操作吗？";

                btnAdd.OnClientClick = Window1.GetShowReference(string.Format("./dictionary_edit.aspx?action=1"), "添加字典");

                BindDataGrid();
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
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            IEnumerable<int> selectIds = GetSelectIds();

            try
            {
                DictionaryService.Delete(p => selectIds.Contains(p.id));

                Alert.Show("删除成功！", MessageBoxIcon.Information);
                BindDataGrid();
            }
            catch (Exception)
            {
                Alert.Show("删除失败！", MessageBoxIcon.Warning);
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
                    int sid = Convert.ToInt32(Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0]);
                    PageContext.RegisterStartupScript(
                        Window1.GetShowReference(string.Format("./dictionary_edit.aspx?id={0}&action=2",
                            sid), "编辑字典"));
                }
            }
            catch (Exception)
            {
                Alert.Show("删除失败！", MessageBoxIcon.Warning);
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
                    base_dictionary dictionary = DictionaryService.FirstOrDefault(p => p.id == sid);
                    dictionary.enable = dictionary.enable == 1 ? 0 : 1;

                    if (DictionaryService.SaveChanges() > 0)
                    {
                        BindDataGrid();
                        Alert.Show("提交成功。", MessageBoxIcon.Information);
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

        protected void Window1_Close(object sender, WindowCloseEventArgs e)
        {
            BindDataGrid();
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
            Expression<Func<base_dictionary, bool>> predicate = BuildPredicate(out orderingSelector);

            //取数据源
            IQueryable<base_dictionary> list = DictionaryService.Where(predicate, Grid1.PageSize, Grid1.PageIndex + 1,
                orderingSelector, EnumHelper.ParseEnumByString<OrderingOrders>(SortDirection), out output);
            

            //设置页面大小
            Grid1.RecordCount = output;

            //绑定数据源
            Grid1.DataSource = list;
            Grid1.DataBind();

            ddlPageSize.SelectedValue = Grid1.PageSize.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     查询处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {

            Grid1.PageIndex = 0;
            BindDataGrid();
        }

        /// <summary>
        ///     创建查询条件表达式和排序表达式
        /// </summary>
        /// <param name="orderingSelector"></param>
        /// <returns></returns>
        private Expression<Func<base_dictionary, bool>> BuildPredicate(out dynamic orderingSelector)
        {
            // 查询条件表达式
            Expression expr = Expression.Constant(true);

            ParameterExpression parameter = Expression.Parameter(typeof(base_dictionary));
            MethodInfo methodInfo = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            expr = Expression.And(expr,
                Expression.Equal(Expression.Property(parameter, "deleteflag"), Expression.Constant(0, typeof(int?))));

            if (!string.IsNullOrWhiteSpace(txtcategory.Text.Trim()))
            {
                expr = Expression.And(expr,
                    Expression.Call(Expression.Property(parameter, "category"), methodInfo,
// ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant(txtcategory.Text.Trim())));
            }

            if (!string.IsNullOrWhiteSpace(ddlFlag.SelectedValue))
            {
                expr = Expression.And(expr,
                    Expression.Equal(Expression.Property(parameter, "enable"),
                        Expression.Constant(Int32.Parse(ddlFlag.SelectedValue), typeof(Int32?))));
            }

            Expression<Func<base_dictionary, bool>> predicate = Expression.Lambda<Func<base_dictionary, bool>>(expr, parameter);

            // 排序表达式
            orderingSelector = Expression.Lambda(Expression.Property(parameter, SortField), parameter);

            return predicate;
        }
    }
}