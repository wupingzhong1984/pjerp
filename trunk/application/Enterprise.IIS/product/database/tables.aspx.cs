using System;
using Enterprise.Data;
using Enterprise.Service.Base.Platform;

// ReSharper disable once CheckNamespace
namespace Enterprise.IIS.product.database
{
// ReSharper disable once InconsistentNaming
    public partial class tables : PageBase
    {
        private ProcedureService _procedureService;

        /// <summary>
        ///     角色数据服务
        /// </summary>
        protected ProcedureService ProcedureService
        {
            get { return _procedureService ?? (_procedureService = new ProcedureService()); }
            set { _procedureService = value; }
        }

        #region Protected Method
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDataGrid();
            }
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
        #endregion

        #region Private Method

        /// <summary>
        ///     绑定数据表格
        /// </summary>
        private void BindDataGrid()
        {
            //取数据源
            var list = ProcedureService.GetTables();

            //绑定数据源
            Grid1.DataSource = list;
            Grid1.DataBind();
        }
        #endregion
    }
}