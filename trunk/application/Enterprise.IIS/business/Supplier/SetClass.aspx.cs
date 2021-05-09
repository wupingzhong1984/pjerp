using System;
using System.Linq;
using Enterprise.Data;
using Enterprise.Framework.Web;
using Enterprise.IIS.Common;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using FineUI;
using NPOI.SS.Formula.Functions;

namespace Enterprise.IIS.business.Supplier
{
    public partial class SetClass : PageBase
    {

        /// <summary>
        ///     数据服务
        /// </summary>
        private SupplierService _supplierService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected SupplierService SupplierService
        {
            get { return _supplierService ?? (_supplierService = new SupplierService()); }
            set { _supplierService = value; }
        }

        /// <summary>
        ///     FCodes
        /// </summary>
        protected string FCodes
        {
            get { return Request["FCodes"]; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //初始化控件数据
                InitData();

                //加载数据
                LoadData();
            }
        }

        private void LoadData()
        {
            GasHelper.DropDownListSupplierCateDataBind(ddlFSubCate);
        }

        #region Protected
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool isSucceed = false;

            try
            {
                var codes = FCodes.Split(',');

                foreach (var code in codes)
                {

                    if (!string.IsNullOrEmpty(code))
                    {
                        var ftx =
                            SupplierService.FirstOrDefault(
                                p => p.FCompanyId == CurrentUser.AccountComId && p.FCode == code);

                        ftx.FSubCateId = ddlFSubCate.SelectedValue;
                        ftx.FSubCateName = ddlFSubCate.SelectedText;


                        SupplierService.SaveChanges();

                    }

                }


                isSucceed = true;
            }
            catch (Exception)
            {
                isSucceed = false;
            }
            finally
            {
                if (isSucceed)
                {
                    PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
                }
                else
                {
                    Alert.Show("提交失败！", MessageBoxIcon.Error);
                }
            }
        }

        #endregion

        #region Private Method

        /// <summary>
        ///     初始化页面数据
        /// </summary>
        private void InitData()
        {
            btnClose.OnClientClick = ActiveWindow.GetHidePostBackReference();
        }

        #endregion
    }
}