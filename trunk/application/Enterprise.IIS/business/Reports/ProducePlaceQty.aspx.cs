using Enterprise.Data;
using Enterprise.Framework.EntityRepository;
using Enterprise.Framework.Enum;
using Enterprise.Framework.Utils;
using Enterprise.IIS.Common;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web.UI.WebControls;

namespace Enterprise.IIS.business.Reports
{
    public partial class ProducePlaceQty : PageBase
    {
        private StockOutService stockOutService;

        protected StockOutService StockOutService
        {
            get { return stockOutService ?? (stockOutService = new StockOutService()); }
            set { stockOutService = value; }
        }

        private PlaceOrderQtyService placeOrderService;

        protected PlaceOrderQtyService PlaceOrderService
        {
            get { return placeOrderService ?? (placeOrderService = new PlaceOrderQtyService()); }
            set { placeOrderService = value; }
        }
        /// <summary>
        ///     排序字段
        /// </summary>
        protected string SortField
        {
            get
            {
                string sort = ViewState["SortField"] != null ? ViewState["SortField"].ToString() : "FINum";
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
            GasHelper.DropDownListDataBindOrgnization(ddlOrgnization);
            GasHelper.DropDownListGroupDataBind(ddlFGroup);//班组
        }

        protected void BindData()
        {

            var parms = new Dictionary<string, object>();
            parms.Clear();

            parms.Add("@begintime", dpkFDateBegin.SelectedDate);
            parms.Add("@endtime", dpkFDateEnd.SelectedDate);
            parms.Add("@orgid", ddlOrgnization.SelectedValue=="-1"?"": ddlOrgnization.SelectedValue);
            parms.Add("@point", ddlFDistributionPoint.SelectedValue == "-1" ? "" : ddlFDistributionPoint.SelectedValue);
            parms.Add("@T6", txtT6Code.Text);
            parms.Add("@FGroup", ddlFGroup.SelectedValue == "-1" ? "" : ddlFGroup.SelectedValue);
            DataTable tb = SqlService.ExecuteProcedureCommand("rpt_producePrice", parms).Tables[0];

            Grid1.DataSource = tb;
            Grid1.DataBind();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            BindData();
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {

            
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

        private Expression<Func<V_ProducePlacePriceByMonth, bool>> BuildPredicate(out dynamic orderingSelector)
        {
            // 查询条件表达式
            Expression expr = Expression.Constant(true);

            ParameterExpression parameter = Expression.Parameter(typeof(V_ProducePlacePriceByMonth));
            MethodInfo methodInfo = typeof(string).GetMethod("Contains", new[] { typeof(string) });


            // 单据类型

            if (dpkFDateBegin.SelectedDate != null)
            {
                expr = Expression.And(expr,
                    Expression.Equal(Expression.Property(parameter, "FDate"),
                        Expression.Constant(DateTime.Parse(dpkFDateBegin.SelectedDate.ToString()).ToString("yyyy-MM"), typeof(string))));
            }
           

            if (ddlFDistributionPoint.SelectedValue != "-1")
            {
                expr = Expression.And(expr,
                Expression.Equal(Expression.Property(parameter, "FDistributionPoint"), Expression.Constant(ddlFDistributionPoint.SelectedValue, typeof(string))));
            }
            if (!string.IsNullOrWhiteSpace(txtT6Code.Text))
            {
                expr = Expression.And(expr,
                Expression.Equal(Expression.Property(parameter, "FDistributionPoint"), Expression.Constant(txtT6Code.Text, typeof(string))));
            }
           
            Expression<Func<V_ProducePlacePriceByMonth, bool>> predicate = Expression.Lambda<Func<V_ProducePlacePriceByMonth, bool>>(expr, parameter);

            // 排序表达式
            orderingSelector = Expression.Lambda(Expression.Property(parameter, SortField), parameter);

            return predicate;
        }
    }
}