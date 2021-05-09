using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Globalization;
using System.Text;
using Enterprise.IIS.Common;
using Enterprise.Data;
using Enterprise.Framework.EntityRepository;
using Enterprise.Framework.Enum;
using Enterprise.Framework.Utils;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using FineUI;

namespace Enterprise.IIS.business.TubeInvo
{
    public partial class TubePrice : PageBase
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
        private ViewTubePriceService _tubePriceService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected ViewTubePriceService TubePriceService
        {
            get { return _tubePriceService ?? (_tubePriceService = new ViewTubePriceService()); }
            set { _tubePriceService = value; }
        }

        /// <summary>
        ///     数据服务_视图
        /// </summary>
        private CustomerPriceService _customerPriceService;

        /// <summary>
        ///     数据服务_视图
        /// </summary>
        protected CustomerPriceService CustomerPriceService
        {
            get { return _customerPriceService ?? (_customerPriceService = new CustomerPriceService()); }
            set { _customerPriceService = value; }
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

                btnAdd.OnClientClick = Window1.GetShowReference(string.Format("./TubePriceEidt.aspx?action=1"), "设置计算公式");

                //btnView.OnClientClick = Window2.GetShowReference(string.Format("./SetContractIndex.aspx"), "查看客户合同");

                btnBatchDelete.ConfirmText = "你确定要执行删除操作吗？";

                //GasHelper.DropDownListSalesmanDataBind(ddlFSalesman);

                BindDataGrid();
            }
        }
        protected void txtFName_OnTextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtFName.Text.Trim()))
                BindDataGrid();
        }

        protected void txtFItemName_OnTextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtFItemName.Text.Trim()))
                BindDataGrid();
        }

        protected void txtminPrice_OnTextChanged(object sender, EventArgs e)
        {
            BindDataGrid();
        }

        protected void txtMaxPrice_OnTextChanged(object sender, EventArgs e)
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

                var service = new TubePriceService();

                service.Delete(p => selectIds.Contains(p.FId));
                
                Alert.Show("删除成功！", MessageBoxIcon.Information);
                BindDataGrid();
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
                    string sid = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0].ToString();
                    PageContext.RegisterStartupScript(
                        Window1.GetShowReference(string.Format("./TubePriceEidt.aspx?FId={0}&action=2",
                            sid), "设置计算公式"));
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
            Response.AddHeader("content-disposition", string.Format(@"attachment; filename=设置计算公式{0}.xls", SequenceGuid.GetGuidReplace()));
            Response.ContentType = "application/excel";
            Response.ContentEncoding = Encoding.UTF8;
            Response.Write(Utils.GetGridTableHtml(Grid1));
            Response.End();
        }

        /// <summary>
        ///     引入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnImport_Click(object sender, EventArgs e)
        {
            //PageContext.RegisterStartupScript(string.Format("openAddFineUI('./business/Init/InitCustomerPrice.aspx');"));
        }

        protected void Window1_Close(object sender, WindowCloseEventArgs e)
        {
            BindDataGrid();
        }

        protected void Window2_Close(object sender, WindowCloseEventArgs e)
        {
            BindDataGrid();
        }
        #endregion

        #region Private Method

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
                selectIds[i] = Convert.ToInt32(Grid1.DataKeys[selections[i]][0].ToString());
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
            Expression<Func<vm_TubePrice, bool>> predicate = BuildPredicate(out orderingSelector);

            //取数据源
            IQueryable<vm_TubePrice> list = TubePriceService.Where(predicate, Grid1.PageSize, Grid1.PageIndex + 1,
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
        private Expression<Func<vm_TubePrice, bool>> BuildPredicate(out dynamic orderingSelector)
        {
            // 查询条件表达式
            Expression expr = Expression.Constant(true);

            ParameterExpression parameter = Expression.Parameter(typeof(vm_TubePrice));
            MethodInfo methodInfo = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            //expr = Expression.And(expr,
            //    Expression.Equal(Expression.Property(parameter, "FCompanyId"), Expression.Constant(CurrentUser.AccountComId, typeof(int?))));

            if (!string.IsNullOrWhiteSpace(txtFName.Text))
            {
                expr = Expression.And(expr,
                    Expression.Call(Expression.Property(parameter, "FName"), methodInfo,
                    // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant(txtFName.Text.Trim())));
            }

            if (!string.IsNullOrWhiteSpace(txtFItemName.Text))
            {
                expr = Expression.And(expr,
                    Expression.Call(Expression.Property(parameter, "FItemName"), methodInfo,
                    // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant(txtFItemName.Text.Trim())));
            }

            if (!ddlBill.SelectedValue.Equals("-1"))
            {
                expr = Expression.And(expr,
                    Expression.Call(Expression.Property(parameter, "FBill"), methodInfo,
                    // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant(ddlBill.SelectedValue.Trim())));
            }

            Expression<Func<vm_TubePrice, bool>> predicate = Expression.Lambda<Func<vm_TubePrice, bool>>(expr, parameter);

            // 排序表达式
            orderingSelector = Expression.Lambda(Expression.Property(parameter, SortField), parameter);

            return predicate;
        }

        #endregion
    }
}