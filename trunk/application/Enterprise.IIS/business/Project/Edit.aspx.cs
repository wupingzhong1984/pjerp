using System;
using FineUI;
using Enterprise.Data;
using Enterprise.Framework.Web;
using Enterprise.Service.Base;
using Enterprise.Service.Base.ERP;

namespace Enterprise.IIS.business.Project
{
    public partial class Edit : PageBase
    {
        

        /// <summary>
        ///     编辑字典
        /// </summary>
        protected string ItemsId
        {
            get { return Request["id"]; }
        }

        /// <summary>
        ///     组织部门父ID
        /// 新增时，代表选中当前部门下
        /// </summary>
        protected string ParentId
        {
            get { return Request["pOrgid"]; }
        }

        /// <summary>
        ///     当前画面操作项
        /// </summary>
        public WebAction Actions
        {
            get
            {
                string s = Convert.ToString(Request["action"]);
                return (WebAction) int.Parse(s);
            }
        }

        /// <summary>
        ///     
        /// </summary>
        private ProjectService _projectService;

        /// <summary>
        ///     
        /// </summary>
        protected ProjectService ProjectService
        {
            get { return _projectService ?? (_projectService = new ProjectService()); }
            set { _projectService = value; }
        }

        /// <summary>
        ///     
        /// </summary>
        private ProjectItemsService _projectItemsService;

        /// <summary>
        ///     
        /// </summary>
        protected ProjectItemsService ProjectItemsService
        {
            get { return _projectItemsService ?? (_projectItemsService = new ProjectItemsService()); }
            set { _projectItemsService = value; }
        }

        /// <summary>
        ///  
        /// </summary>
        protected LHProject Project
        {
            get
            {
                return ProjectService.FirstOrDefault(p => p.FId == ParentId);
            }
        }

        /// <summary>
        ///     
        /// </summary>
        private LHProjectItems _projectItems;
        /// <summary>
        ///    信息    
        /// </summary>
        protected LHProjectItems ProjectItems
        {
            get
            {
                return _projectItems ??
                       (_projectItems = ProjectItemsService.FirstOrDefault(p => p.FId == ItemsId));
            }
            set { _projectItems = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetPermissionButtons(Toolbar1);

                //初始化控件数据
                InitData();

                //加载数据
                LoadData();
            }
        }

        #region Protected Method

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
        ///     提交编辑
        /// </summary>
        private bool SubmintEdit()
        {
            if (ProjectItems != null)
            {
                ProjectItems.FName = txtFName.Text.Trim();
                ProjectItems.FKey = txtFKey.Text.Trim();
                ProjectItems.FValue = txtFValue.Text.Trim();

                return ProjectItemsService.SaveChanges() >= 0;
            }
            return false;
        }

        /// <summary>
        ///     提交添加
        /// </summary>
        private bool SubmintAdd()
        {
            var items = new LHProjectItems
            {
                FFlag=1,
                FCompanyId=CurrentUser.AccountComId,
                FParentId=ParentId,
                FSParent=ParentId,
                FId = txtFId.Text.Trim(),
                FName = txtFName.Text.Trim(),
                FKey = txtFKey.Text.Trim(),
                FValue = txtFValue.Text.Trim(),
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

        /// <summary>
        ///     加载页面数据
        /// </summary>
        private void LoadData()
        {
            switch (Actions)
            {
                case WebAction.Add:
                    txtFId.Readonly = false;

                    break;
                case WebAction.Edit:
                    if (ProjectItems != null)
                    {
                        txtFId.Text = ProjectItems.FId;
                        txtFName.Text = ProjectItems.FName;
                        txtFKey.Text = ProjectItems.FKey;
                        txtFValue.Text = ProjectItems.FValue;

                        txtFId.Readonly = true;
                    }
                    break;
            }
        }

        #endregion
    }
}