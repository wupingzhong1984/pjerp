using System;

using FineUI;
using Enterprise.Data;
using Enterprise.Framework.Web;
using Enterprise.Service.Base;
using Enterprise.Service.Base.Platform;


// ReSharper disable once CheckNamespace
namespace Enterprise.IIS.product.LHDictionary
{
// ReSharper disable once InconsistentNaming
    public partial class dictionary_edit : PageBase
    {

        /// <summary>
        ///     字典信息
        /// </summary>
        private base_dictionary _baseDictionary;

        /// <summary>
        ///     字典数据服务
        /// </summary>
        private DictionaryService _dictionaryService;

        /// <summary>
        ///     字典数据服务
        /// </summary>
        protected DictionaryService DictionaryService
        {
            get { return _dictionaryService ?? (_dictionaryService = new DictionaryService()); }
            set { _dictionaryService = value; }
        }

        /// <summary>
        ///     当前角色对象ID
        /// </summary>
        protected int Key
        {
            get { return int.Parse(Request["id"]); }
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
        ///      字典信息    
        /// </summary>
        protected base_dictionary Dictionary
        {
            get { return _baseDictionary ?? (_baseDictionary = DictionaryService.FirstOrDefault(p => p.id == Key)); }
            set { _baseDictionary = value; }
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
            var isSucceed = false;

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
            if (Dictionary != null)
            {
                Dictionary.category = txtdict_name.Text.Trim();
                Dictionary.key = txtdict_key.Text.Trim();
                Dictionary.value = txtdict_value.Text.Trim();
                Dictionary.sort = int.Parse(txtdict_sort.Text.Trim());
                Dictionary.desc = txtdict_desc.Text.Trim();
                Dictionary.deleteflag = 0;
                Dictionary.enable = 1;

                return DictionaryService.SaveChanges() >= 0;
            }
            return false;
        }

        /// <summary>
        ///     提交添加
        /// </summary>
        private bool SubmintAdd()
        {
            var dictionary = new base_dictionary
            {
                category = txtdict_name.Text.Trim(),
                key = txtdict_key.Text.Trim(),
                value = txtdict_value.Text.Trim(),
                sort = int.Parse(txtdict_sort.Text.Trim()),
                desc = txtdict_desc.Text.Trim(),
                deleteflag = 0,
                enable = 1
            };

            return DictionaryService.Add(dictionary);
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
                    
                    break;
                case WebAction.Edit:
                    if (Dictionary != null)
                    {
                        txtdict_key.Text=Dictionary.key;
                        txtdict_value.Text=Dictionary.value;
                        txtdict_sort.Text=Dictionary.sort.ToString();
                        txtdict_desc.Text = Dictionary.desc;
                        txtdict_name.Text = Dictionary.category;
                    }
                    break;
            }
        }
        #endregion
    }
}