using System;
using System.Collections.Generic;
using System.Text;
using Enterprise.IIS.Common;
using Enterprise.Data;
using Enterprise.Framework.Utils;
using FineUI;
using Newtonsoft.Json.Linq;

namespace Enterprise.IIS.business.Reports
{
    public partial class ParsMonthDetails : PageBase
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

                dpFDate.SelectedDate = DateTime.Now;

                GasHelper.DropDownListCompanyDataBind(ddlCompany);

                BindDataGrid();
            }
        }

        /// <summary>
        ///     查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
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
            Response.AddHeader("content-disposition", string.Format(@"attachment; filename=商品进销存明细表{0}.xls", SequenceGuid.GetGuidReplace()));
            Response.ContentType = "application/excel";
            Response.ContentEncoding = Encoding.UTF8;
            Response.Write(Utils.GetGridTableHtml(Grid1));
            Response.End();
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
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
        ///     绑定数据表格
        /// </summary>
        private void BindDataGrid()
        {
            var parms = new Dictionary<string, object>();
            parms.Clear();

            parms.Add("@FDate", dpFDate.SelectedDate);
            parms.Add("@companyid", ddlCompany.SelectedValue);
            var list = SqlService.ExecuteProcedureCommand("rpt_jxcDetails", parms).Tables[0];

            if (list != null && list.Rows.Count > 0)
            {
                //绑定数据源
                Grid1.DataSource = list;
                Grid1.DataBind();

                //合计
                var summary = new JObject
                    {
                        {"FName", "合计"},
                        {"FUltQty", list.Compute("sum(FUltQty)","true").ToString()},
                        {"FCZ", list.Compute("sum(FCZ)","true").ToString()},
                        {"FWG", list.Compute("sum(FWG)","true").ToString()},
                        {"FXSTH", list.Compute("sum(FXSTH)","true").ToString()},
                        {"FPY", list.Compute("sum(FPY)","true").ToString()},
                        {"FPQ", list.Compute("sum(FPQ)","true").ToString()},
                        {"FXS", list.Compute("sum(FXS)","true").ToString()},
                        {"FDB", list.Compute("sum(FDB)","true").ToString()},
                        {"FZY", list.Compute("sum(FZY)","true").ToString()},
                        {"FZS", list.Compute("sum(FZS)","true").ToString()},
                        {"FPK", list.Compute("sum(FPK)","true").ToString()},
                        {"FITotal", list.Compute("sum(FITotal)","true").ToString()},
                        {"FOTotal", list.Compute("sum(FOTotal)","true").ToString()},
                        {"FEnd", list.Compute("sum(FEnd)","true").ToString()},
                    };

                Grid1.SummaryData = summary;
            }
            else
            {
                //合计
                var summary = new JObject
                    {
                        {"FName", "合计"},
                        {"FUltQty", "0"},
                        {"FCZ",  "0"},
                        {"FWG",  "0"},
                        {"FXSTH",  "0"},
                        {"FPY",  "0"},
                        {"FPQ",  "0"},
                        {"FXS",  "0"},
                        {"FDB", "0"},
                        {"FZY",  "0"},
                        {"FZS",  "0"},
                        {"FPK",  "0"},
                        {"FITotal","0"},
                        {"FOTotal", "0"},
                        {"FEnd",  "0"},
                    };

                Grid1.SummaryData = summary;
            }
        }

        #endregion
    }
}