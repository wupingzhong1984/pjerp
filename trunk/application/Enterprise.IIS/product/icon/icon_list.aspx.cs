using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Linq;

using FineUI;
using Enterprise.Data;
using Enterprise.Service.Base;
using Enterprise.Service.Base.Platform;
using Enterprise.Framework.Enum;
using Enterprise.Framework.EntityRepository;

// ReSharper disable once CheckNamespace
namespace Enterprise.IIS.product.icon
{
// ReSharper disable once InconsistentNaming
    public partial class icon_list : PageBase
    {

        /// <summary>
        ///     字典数据服务
        /// </summary>
        private IconService _iconService;

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
        protected IconService IconService
        {
            get { return _iconService ?? (_iconService = new IconService()); }
            set { _iconService = value; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                SetPermissionButtons(Toolbar1);

                btnBatchDelete.ConfirmText = "你确定要执行删除操作吗？";

                btnAdd.OnClientClick = Window1.GetShowReference(string.Format("./icon_edit.aspx?action=1"), "添加图标");

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
        ///     RowCommand
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            object[] keys = Grid1.DataKeys[e.RowIndex];

            int keyId = int.Parse(keys[0].ToString());
            base_icon icon = IconService.FirstOrDefault(p => p.id == keyId);
            if (icon != null)
            {
                switch (e.CommandName)
                {
                    case "Stop":
                        
                        break;
                    case "Delete":
                        if (IconService.Update(p => p.id == keyId, p => new base_icon { deleteflag = 1 }))
                        {
                            BindDataGrid();
                            Alert.Show("提交成功。", MessageBoxIcon.Information);
                        }
                        else
                        {
                            Alert.Show("提交失败，请重试！", MessageBoxIcon.Error);
                        }

                        break;
                }
            }
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
                IconService.Update(p => selectIds.Contains(p.id), p => new base_icon { deleteflag = 1 });
                Alert.Show("删除成功！", MessageBoxIcon.Information);
                BindDataGrid();
            }
            catch (Exception)
            {
                Alert.Show("删除失败！", MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 编辑
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
                        Window1.GetShowReference(string.Format("./icon_edit.aspx?id={0}&action=2",
                            sid), "编辑图标"));
                }
            }
            catch (Exception)
            {
                Alert.Show("删除失败！", MessageBoxIcon.Warning);
            }
        }

        protected void Window1_Close(object sender, WindowCloseEventArgs e)
        {
            Grid1.PageIndex = 0;
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
            Expression<Func<base_icon, bool>> predicate = BuildPredicate(out orderingSelector);

            //取数据源
            IQueryable<base_icon> list = IconService.Where(predicate, Grid1.PageSize, Grid1.PageIndex + 1,
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
        private Expression<Func<base_icon, bool>> BuildPredicate(out dynamic orderingSelector)
        {
            // 查询条件表达式
            Expression expr = Expression.Constant(true);

            ParameterExpression parameter = Expression.Parameter(typeof(base_icon));

            expr = Expression.And(expr,
                Expression.Equal(Expression.Property(parameter, "deleteflag"), Expression.Constant(0, typeof(int?))));

            //if (!trDept.SelectedNodeID.Equals("1"))
            //{
            //    expr = Expression.And(expr,
            //        Expression.Equal(Expression.Property(parameter, "account_org_id"),
            //            Expression.Constant(Int32.Parse(trDept.SelectedNodeID), typeof(Int32?))));
            //}

            // 登陆帐号
            //if (!string.IsNullOrWhiteSpace(tbxRole_name.Text.Trim()))
            //{
            //    expr = Expression.And(expr,
            //        Expression.Equal(Expression.Property(parameter, "account_number"),
            //            Expression.Constant(tbxRole_name.Text.Trim())));
            //}

            Expression<Func<base_icon, bool>> predicate = Expression.Lambda<Func<base_icon, bool>>(expr, parameter);

            // 排序表达式
            orderingSelector = Expression.Lambda(Expression.Property(parameter, SortField), parameter);

            return predicate;
        }
    }
}