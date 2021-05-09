using Enterprise.Data;
using Enterprise.Framework.EntityRepository;
using Enterprise.Framework.Enum;
using Enterprise.Framework.Utils;
using Enterprise.IIS.Common;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using FineUI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Enterprise.IIS.business.Reports
{
    public partial class ProducePlace : PageBase
    {
        /// <summary>
        ///     排序字段
        /// </summary>
        protected string SortField
        {
            get
            {
                string sort = ViewState["SortField"] != null ? ViewState["SortField"].ToString() : "KeyId";
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
                string sort = ViewState["SortDirection"] != null ? ViewState["SortDirection"].ToString() : "DESC";
                return sort;
            }
            set { ViewState["SortDirection"] = value; }
        }

        /// <summary>
        ///     数据服务
        /// </summary>
        private StockOutService _stockOutService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected StockOutService StockOutService
        {
            get { return _stockOutService ?? (_stockOutService = new StockOutService()); }
            set { _stockOutService = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }


        protected void LoadData()
        {
            GasHelper.DropDownListDistributionPointDataBind(ddlFDistributionPoint);
            GasHelper.DropDownListGroupDataBind(ddlFGroup);//班组
            GasHelper.DropDownListDataBindOrgnization(ddlOrgnization);
        }

        protected void BindData()
        {
            
            dynamic orderingSelector;
            int output;
            Expression<Func<LHStockOut, bool>> predicate = BuildPredicate(out  orderingSelector);

            //取数据源
            IQueryable<LHStockOut> list = StockOutService.Where(predicate, Grid1.PageSize, Grid1.PageIndex + 1,
                orderingSelector, EnumHelper.ParseEnumByString<OrderingOrders>(SortDirection), out  output);

            //设置页面大小
            Grid1.RecordCount = output;

            //绑定数据源
            Grid1.DataSource = list;
            Grid1.DataBind();

            ddlPageSize.SelectedValue = Grid1.PageSize.ToString(CultureInfo.InvariantCulture);
        }

        protected void Grid1_PageIndexChange(object sender, GridPageEventArgs e)
        {
            Grid1.PageIndex = e.NewPageIndex;
            BindData();
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid1.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
            BindData();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            BindData();
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {


        }
        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
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

                    if (e.CommandName == "actView")
                    {
                        PageContext.RegisterStartupScript(string.Format("openDetailsUI('{0}');", sid));
                    }
                }
            }
            catch (Exception)
            {
                Alert.Show("复制失败！", MessageBoxIcon.Warning);
            }
        }

        protected void Grid1_RowDataBound(object sender, GridRowEventArgs e)
        {
            var item = e.DataItem as LHPassCard;

            if (item != null)
            {

                if (item.FAuditFlag == 0)
                {
                    for (int i = 0; i < e.Values.Length; i++)
                    {
                        if (!e.Values[i].ToString().Contains("#@TPL@"))
                            e.Values[i] = String.Format("<span class=\"{0}\">{1}</span>", "colorcoral", e.Values[i].ToString().Contains("#@TPL@") ? "已作废" : e.Values[i]);
                    }
                }

                if (item.FAuditFlag == 1)
                {
                    for (int i = 0; i < e.Values.Length; i++)
                    {
                        if (!e.Values[i].ToString().Contains("#@TPL@"))
                            e.Values[i] = String.Format("<span class=\"{0}\">{1}</span>", "colorgreen", e.Values[i].ToString().Contains("#@TPL@") ? "已作废" : e.Values[i]);
                    }
                    
                }
            }
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.ClearContent();
            Response.AddHeader("content-disposition", string.Format(@"attachment; filename=领料表单{0}.xls", SequenceGuid.GetGuidReplace()));
            Response.ContentType = "application/excel";
            Response.ContentEncoding = Encoding.UTF8;
            Response.Write(Utils.GetGridTableHtml(Grid1));
            Response.End();
        }

        private Expression<Func<LHStockOut, bool>> BuildPredicate(out dynamic orderingSelector)
        {
            // 查询条件表达式
            Expression expr = Expression.Constant(true);

            ParameterExpression parameter = Expression.Parameter(typeof(LHStockOut));
            MethodInfo methodInfo = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            expr = Expression.And(expr,
                Expression.Equal(Expression.Property(parameter, "FCompanyId"), Expression.Constant(CurrentUser.AccountComId, typeof(int))));

            // 单据类型
            int ftype = Convert.ToInt32(GasEnumBill.picking);
            expr = Expression.And(expr,
               Expression.Equal(Expression.Property(parameter, "FType"), Expression.Constant(ftype, typeof(int?))));

            expr = Expression.And(expr,
               Expression.Equal(Expression.Property(parameter, "FDeleteFlag"), Expression.Constant(0, typeof(int?))));

            expr = Expression.And(expr,
               Expression.Equal(Expression.Property(parameter, "FFlag"), Expression.Constant(1, typeof(int?))));
            if (dpkFDateBegin.SelectedDate != null)
            {
                expr = Expression.And(expr,
                    Expression.GreaterThanOrEqual(Expression.Property(parameter, "FDate"),
                        Expression.Constant(dpkFDateBegin.SelectedDate, typeof(DateTime?))));
            }

            if (dpkFDateEnd.SelectedDate != null)
            {
                expr = Expression.And(expr,
                    Expression.LessThanOrEqual(Expression.Property(parameter, "FDate"),
                        Expression.Constant(dpkFDateEnd.SelectedDate, typeof(DateTime?))));
            }

            if (ddlFDistributionPoint.SelectedValue != "-1")
            {
                expr = Expression.And(expr,
                    Expression.Equal(Expression.Property(parameter, "FDistributionPoint"),
                        // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant(ddlFDistributionPoint.SelectedValue)));
            }

            if (ddlFGroup.SelectedValue!="-1")
            {
                expr = Expression.And(expr,
                    Expression.Equal(Expression.Property(parameter, "FGroup"), 
                        // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant(ddlFGroup.SelectedValue)));
            }

            if (ddlOrgnization.SelectedValue != "-1")
            {
                expr = Expression.And(expr,
                    Expression.Call(Expression.Property(parameter, "FCode"), methodInfo,
                        // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        Expression.Constant(ddlOrgnization.SelectedValue)));
            }



            Expression<Func<LHStockOut, bool>> predicate = Expression.Lambda<Func<LHStockOut, bool>>(expr, parameter);

            // 排序表达式
            orderingSelector = Expression.Lambda(Expression.Property(parameter, SortField), parameter);

            return predicate;
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            IEnumerable<string> selectIds = GetSelectIds();

            try
            {
                Log(string.Format(@"审核单据号:{0}成功。", selectIds));
                

                StockOutService.Update(p => p.FCompanyId == CurrentUser.AccountComId && selectIds.Contains(p.KeyId), p => new LHStockOut
                {
                    FAuditFlag=1
                });
                
                Alert.Show("审核成功！", MessageBoxIcon.Information);
                BindData();
            }
            catch (Exception)
            {
                Alert.Show("审核失败！", MessageBoxIcon.Warning);
            }
        }

        protected void btnUnConfirm_Click(object sender, EventArgs e)
        {
            IEnumerable<string> selectIds = GetSelectIds();

            try
            {
                Log(string.Format(@"审核单据号:{0}成功。", selectIds));


                StockOutService.Update(p => p.FCompanyId == CurrentUser.AccountComId && selectIds.Contains(p.KeyId), p => new LHStockOut
                {
                    FAuditFlag = 2
                });

                Alert.Show("审核成功！", MessageBoxIcon.Information);
                BindData();
            }
            catch (Exception)
            {
                Alert.Show("审核失败！", MessageBoxIcon.Warning);
            }

        }

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

    }
}