using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Reflection;
using FineUI;
using Enterprise.Data;
using Enterprise.Framework.EntityRepository;
using Enterprise.Framework.Enum;
using Enterprise.Service.Base;
using Enterprise.Service.Base.Platform;



// ReSharper disable once CheckNamespace
namespace Enterprise.IIS.product.action
{
// ReSharper disable once InconsistentNaming
    public partial class action_list : PageBase
    {
        protected string SortField
        {
            get
            {
                string sort = ViewState["SortField"] != null ? ViewState["SortField"].ToString() : "ID";
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

        private AcitonService _mainAction;
        protected AcitonService MainAction
        {
            get { return _mainAction ?? (_mainAction = new AcitonService()); }
            set { _mainAction = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //设置权限
                SetPermissionButtons(Toolbar1); 

                btnBatchDelete.ConfirmText = "你确定要执行删除操作吗？";

                BindDataGrid();

                btnAdd.OnClientClick = Window1.GetShowReference(string.Format("./action_edit.aspx?action=1"), "添加功能");
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

            Expression<Func<base_aciton, bool>> predicate = BuildPredicate(out orderingSelector);
            //取数据源
            IQueryable<base_aciton> list = MainAction.Where(predicate, Grid1.PageSize, Grid1.PageIndex + 1,
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
        private Expression<Func<base_aciton, bool>> BuildPredicate(out dynamic orderingSelector)
        {
            // 查询条件表达式
            Expression expr = Expression.Constant(true);

            ParameterExpression parameter = Expression.Parameter(typeof(base_aciton));

            MethodInfo methodInfo = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            expr = Expression.And(expr,
                Expression.Equal(Expression.Property(parameter, "deleteflag"), Expression.Constant(0, typeof(int?))));

            
            //功能名称
            if (!string.IsNullOrWhiteSpace(txtaction_name.Text.Trim()))
            {
                expr = Expression.And(expr,
                    Expression.Call(Expression.Property(parameter, "action_name"), methodInfo,
// ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant(txtaction_name.Text.Trim())));
            }

            Expression<Func<base_aciton, bool>> predicate = Expression.Lambda<Func<base_aciton, bool>>(expr, parameter);

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
            string commandName = e.CommandName;
            object[] keys = Grid1.DataKeys[e.RowIndex];

            int keyId = int.Parse(keys[0].ToString());
            var role = MainAction.FirstOrDefault(p => p.id == keyId);
            if (role != null)
            {
                switch (commandName)
                {
                    case "Edit":
                        HttpContext.Current.Response.Redirect(string.Format(@"action_edit.aspx?id={0}&action=2", keyId));
                        break;
                    case "Delete":
                        if (MainAction.Update(p=>p.id==keyId,p=>new base_aciton {deleteflag = 1}))
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
            HttpContext.Current.Response.Redirect(string.Format(@"action_edit.aspx?action=1"));
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindDataGrid();
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            IEnumerable<int> selectIds = GetSelectIds();
            try
            {
                MainAction.Update(p => selectIds.Contains(p.id), p => new base_aciton { deleteflag = 1 });
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
                        Window1.GetShowReference(string.Format("./action_edit.aspx?id={0}&action=2",
                            sid), "编辑功能"));
                }
            }
            catch (Exception)
            {
                Alert.Show("删除失败！", MessageBoxIcon.Warning);
            }
        }
        #endregion
    }
}