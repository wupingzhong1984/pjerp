using System;
using System.Linq;
using Enterprise.Data;
using Enterprise.Framework.Web;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;
using FineUI;

namespace Enterprise.IIS.Common
{

    /// <summary>
    ///     分类管理
    /// </summary>
    public partial class WinClass : PageBase
    {

        /// <summary>
        ///     数据服务
        /// </summary>
        private ProjectItemsService _projectItemsService;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected ProjectItemsService ProjectItemsService
        {
            get { return _projectItemsService ?? (_projectItemsService = new ProjectItemsService()); }
            set { _projectItemsService = value; }
        }

        /// <summary>
        ///     FId
        /// </summary>
        protected string FId
        {
            get { return Request["FId"]; }
        }

        /// <summary>
        ///     
        /// </summary>
        protected string FSParent
        {
            get { return (Request["FSParent"]); }
        }

        /// <summary>
        ///     
        /// </summary>
        protected string FParentId
        {
            get { return (Request["FParentId"]); }
        }

        /// <summary>
        ///     当前画面操作项
        /// </summary>
        public WebAction Actions
        {
            get
            {
                string s = Convert.ToString(Request["action"]);
                return (WebAction)int.Parse(s);
            }
        }

        /// <summary>
        ///     账号信息
        /// </summary>
        private LHProjectItems _projectItems;

        /// <summary>
        ///     账号信息    
        /// </summary>
        protected LHProjectItems ProjectItems
        {
            get { return _projectItems ?? (_projectItems = ProjectItemsService//
                .FirstOrDefault(p => p.FId == FId && p.FCompanyId == CurrentUser.AccountComId)); }
            set { _projectItems = value; }
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
            var item =
                ProjectItemsService.Where(
                    p => p.FCompanyId == CurrentUser.AccountComId && p.FId == FParentId && p.FFlag == 1)
                    .FirstOrDefault();

            if (item != null)
            {
                lblParent.Text = string.Format("{0}-{1}", item.FId, item.FName);
            }
            else
            {
                var pItem=  new ProjectService().Where(
                        p => p.FCompanyId == CurrentUser.AccountComId && p.FFlag == 1 && p.FId == FSParent).FirstOrDefault();
                if (pItem != null)
                {
                    lblParent.Text = string.Format("{0}-{1}",pItem.FId, pItem.FName);
                }
            }

            switch (Actions)
            {
                case WebAction.Add:
                   

                    break;

                case WebAction.Edit:
                    txtFId.Text = ProjectItems.FId;
                    txtFName.Text = ProjectItems.FName;
                    break;
            }
        }

        #region Protected
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool isSucceed = false;

            try
            {
                switch (Actions)
                {
                    case WebAction.Add:
                        isSucceed = SubmintAdd();

                        break;

                    case WebAction.Edit:
                        isSucceed = SubmintEdit();

                        break;
                }
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
        /// 
        /// </summary>
        /// <returns></returns>
        private bool SubmintEdit()
        {
            if (ProjectItems != null)
            {
                ProjectItems.FName = txtFName.Text.Trim();
                ProjectItems.FId = txtFId.Text;
                ProjectItems.FKey = txtkey.Text.Trim();
                ProjectItems.FValue = txtvalue.Text.Trim();
                return ProjectItemsService.SaveChanges() >= 0;
            }

            return false;
        }


        /// <summary>
        ///     提交添加
        /// </summary>
        private bool SubmintAdd()
        {
            var items = new LHProjectItems()
            {
                FCompanyId = CurrentUser.AccountComId,
                FParentId = FParentId,
                FSParent = FSParent,
                FName = txtFName.Text.Trim(),
                FId = txtFId.Text,
                FFlag = 1,
                FKey = txtkey.Text.Trim(),
                FValue = txtvalue.Text.Trim()
                
            };

            return ProjectItemsService.Add(items);
        }

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