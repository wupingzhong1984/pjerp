using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Enterprise.Framework.File;
using FineUI;
using Enterprise.Data;
using Enterprise.Framework.Web;
using Enterprise.Service.Base;
using Enterprise.Service.Base.Platform;

// ReSharper disable once CheckNamespace
namespace Enterprise.IIS.product.menu
{
    // ReSharper disable once InconsistentNaming
    public partial class menu : PageBase
    {
        /// <summary>
        ///     选中菜单信息
        /// </summary>
        private base_menu _baseMenu;

        /// <summary>
        ///     菜单数据服务
        /// </summary>
        private MenuService _menuService;

        /// <summary>
        ///     菜单列表
        /// </summary>
        private IList<base_menu> _menus;

        /// <summary>
        ///     当前画面操作项
        /// </summary>
        public WebAction Actions
        {
            get
            {
                string s = Convert.ToString(ViewState["action"]);
                return (WebAction)int.Parse(s);
            }
            set { ViewState["action"] = value; }
        }

        /// <summary>
        ///     选中菜单ID
        /// </summary>
        protected int MenuId
        {
            get { return int.Parse(trMenu.SelectedNodeID); }
        }

        /// <summary>
        ///     选中菜单信息
        /// </summary>
        protected base_menu BaseMenu
        {
            get
            {
                if (_baseMenu == null || _baseMenu.id != MenuId)
                {
                    _baseMenu = MenuService.FirstOrDefault(p => p.id == MenuId);
                }

                return _baseMenu;
            }
        }

        /// <summary>
        ///     菜单数据服务  
        /// </summary>
        protected MenuService MenuService
        {
            get { return _menuService ?? (_menuService = new MenuService()); }
            set { _menuService = value; }
        }

        /// <summary>
        ///     菜单列表
        /// </summary>
        protected IList<base_menu> Menus
        {
            get { return _menus ?? (_menus = MenuService.Where(p => p.deleteflag == 0).ToList()); }
            set { _menus = value; }
        }

        #region Protected Method

        protected void filePhoto_FileSelected(object sender, EventArgs e)
        {
            if (filePhoto.HasFile)
            {
                var fileSuffix = filePhoto.ShortFileName.Substring(filePhoto.ShortFileName.LastIndexOf('.'));

                var sequence = SequenceService.CreateSequence("LH", CurrentUser.AccountComId);

                var fileName = sequence + fileSuffix;

                var uploadpath = Config.GetUploadpath();

                var tempPath = (string.Format(@"{0}/temp/{1}/", uploadpath, DateTime.Now.ToString("yyyy-MM-dd"))); //

                if (!DirFile.XFileExists(Server.MapPath(tempPath)))
                {
                    DirFile.XCreateDir(Server.MapPath(tempPath));
                }

                var iconPath = (string.Format(@"~/icon/"));

                if (!DirFile.XFileExists(Server.MapPath(iconPath)))
                {
                    DirFile.XCreateDir(Server.MapPath(iconPath));
                }

                filePhoto.SaveAs(Server.MapPath(tempPath + fileName));


                ImageHelper.LocalImage2Thumbs(Server.MapPath(tempPath + fileName),
                   Server.MapPath(iconPath + fileName), 70, 80, "WH");

                //imgPhoto.ImageUrl = iconPath + fileName;

                hdfImage.Text = iconPath + fileName;

                // 清空文件上传组件
                filePhoto.Reset();
            }

        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetPermissionButtons(Toolbar1);

                btnDelete.ConfirmText = "你确定要执行删除操作吗？";

                LoadTreeSource();

                LoadIcons();

                LoadActions();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void p_rbtnICON_SelectedIndexChanged(object sender, EventArgs e)
        {
            Image1.ImageUrl = p_rbtnICON.SelectedItem.Value;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void trMenu_NodeExpand(object sender, TreeExpandEventArgs e)
        //{
        //}

        /// <summary>
        ///     加载左则数据列表
        /// </summary>
        private void LoadTreeSource()
        {
            trMenu.Nodes.Clear();

            var rootNode = new TreeNode
            {
                Text = "系统菜单",
                NodeID = "0",
                Expanded = true,
              //  EnablePostBack = true,
                CommandName = "0",
                EnableClickEvent = true
            };

            trMenu.Nodes.Add(rootNode);

            //加载子部门信息
            LoadChildNodes(rootNode, 1);

            //设置默认选择项
            trMenu.SelectedNodeID = rootNode.NodeID;
            trMenu_NodeCommand(null, null);
        }

        /// <summary>
        ///     绑定子节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="level"></param>
        private void LoadChildNodes(TreeNode node, int level)
        {
            foreach (base_menu menu in Menus.Where(p => p.menu_parent_id.ToString() == node.NodeID).OrderBy(p => p.menu_sort))
            {
                var cNode = new TreeNode
                {
                    Text = menu.menu_name,
                    NodeID = menu.id.ToString(CultureInfo.InvariantCulture),
                    IconUrl = menu.menu_class,
                    CommandName = level.ToString(CultureInfo.InvariantCulture),
                   // EnablePostBack = true,
                   // SingleClickExpand = true
                    //Expanded=true,
                    EnableClickEvent=true
                    
                };

                //加载子部门信息
                node.Nodes.Add(cNode);

                //递归加载部门组织
                LoadChildNodes(cNode, 2);
            }
        }

        /// <summary>
        ///     部门树加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void trMenu_NodeCommand(object sender, TreeCommandEventArgs e)
        {
            //设置权限
            SetPermissionButtons(Toolbar1);

            //附加编加权限
            ViewState["action"] = ApplicationConstant.EDIT;

            string depth = trMenu.SelectedNode.CommandName;
            switch (depth)
            {
                case "0":
                    txtorg_sort.Hidden = true;
                    GPanelMenuType.Hidden = true;
                    GPanelICON.Hidden = true;
                    GPanelActions.Hidden = true;
                    btnSubmit.Hidden = true;
                    btnDelete.Hidden = !trMenu.SelectedNode.Leaf;
                    btnAdd.Hidden = false;

                    txtmenu_name.Text = "系统菜单";
                    txtmenu_url.Text = "#";
                    break;
                case "1":
                    txtorg_sort.Hidden = false;
                    GPanelMenuType.Hidden = true;
                    GPanelICON.Hidden = false;
                    GPanelActions.Hidden = true;
                    btnSubmit.Hidden = false;

                    btnDelete.Hidden = !trMenu.SelectedNode.Leaf;
                    btnAdd.Hidden = false;

                    txtmenu_name.Text = BaseMenu.menu_name;
                    txtmenu_url.Text = BaseMenu.menu_url;
                    txtorg_sort.Text = BaseMenu.menu_sort.ToString();
                    Image1.ImageUrl = BaseMenu.menu_class;
                    p_rbtnICON.SelectedValue = BaseMenu.menu_class;

                    break;
                case "2":
                    txtorg_sort.Hidden = false;
                    GPanelMenuType.Hidden = false;
                    GPanelICON.Hidden = false;
                    GPanelActions.Hidden = false;
                    btnSubmit.Hidden = false;
                    btnAdd.Hidden = true;
                    btnDelete.Hidden = false;

                    txtmenu_name.Text = BaseMenu.menu_name;
                    txtmenu_url.Text = BaseMenu.menu_url;
                    txtorg_sort.Text = BaseMenu.menu_sort.ToString();
                    Image1.ImageUrl = BaseMenu.menu_class;
                    p_rbtnICON.SelectedValue = BaseMenu.menu_class;

                    btnDelete.Hidden = !trMenu.SelectedNode.Leaf;


                    rbtnListmenu_is_frame_view.SelectedValue = BaseMenu.menu_is_frame_view.ToString();
                    rbtnListmenu_is_view.SelectedValue = BaseMenu.menu_is_view.ToString();

                    if (string.IsNullOrWhiteSpace(BaseMenu.menu_actions))
                    {
                        foreach (CheckItem item in cboxListActions.Items)
                        {
                            item.Selected = false;
                        }
                    }
                    else
                    {
                        foreach (CheckItem item in cboxListActions.Items)
                        {
                            item.Selected = BaseMenu.menu_actions.Contains("|" + item.Value);
                        }
                    }

                    break;
            }
        }

        /// <summary>
        ///     提交部门信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string depth = trMenu.SelectedNode.CommandName;
            switch (Actions)
            {
                case WebAction.Add:
                    switch (depth)
                    {
                        case "0":
                            AddMenu1();
                            break;
                        case "1":
                            AddMenu2();
                            break;
                        case "2":
                            break;
                    }
                    break;
                case WebAction.Edit:
                    switch (depth)
                    {
                        case "0":
                            break;
                        case "1":
                            EditMenu1();
                            break;
                        case "2":
                            EditMenu2();
                            break;
                    }
                    break;
            }
        }

        /// <summary>
        ///     添加菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            //附加新增权限
            ViewState["action"] = ApplicationConstant.ADD;

            string depth = trMenu.SelectedNode.CommandName;
            switch (depth)
            {
                case "0":
                    txtorg_sort.Hidden = false;
                    GPanelMenuType.Hidden = true;
                    GPanelICON.Hidden = false;
                    GPanelActions.Hidden = true;
                    btnSubmit.Hidden = false;

                    txtorg_sort.Hidden = true;
                    txtmenu_name.Text = string.Empty;
                    txtmenu_url.Text = "#";

                    foreach (RadioItem item in p_rbtnICON.Items)
                    {
                        item.Selected = false;
                    }

                    break;
                case "1":
                    txtorg_sort.Hidden = false;
                    GPanelMenuType.Hidden = false;
                    GPanelICON.Hidden = false;
                    GPanelActions.Hidden = false;
                    btnSubmit.Hidden = false;
                    txtorg_sort.Hidden = true;

                    txtmenu_name.Text = string.Empty;
                    txtmenu_url.Text = string.Empty;

                    foreach (CheckItem item in cboxListActions.Items)
                    {
                        item.Selected = false;
                    }

                    break;
                case "2":
                    break;
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (MenuService.Delete(BaseMenu))
            {
                LoadTreeSource();
                Alert.Show("删除成功。", MessageBoxIcon.Information);
            }
            else
            {
                Alert.Show("删除失败！", MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Private Method

        /// <summary>
        ///     加载功能
        /// </summary>
        private void LoadActions()
        {
            var list = new AcitonService().Where(p => p.deleteflag == 0).ToList().Select(
                p => new base_aciton
                {
                    action_name = p.action_name,
                    action_en = p.action_en + "|" + p.id.ToString(CultureInfo.InvariantCulture)
                });
            cboxListActions.DataSource = list;
            cboxListActions.DataBind();
        }

        /// <summary>
        ///     加载ICON
        /// </summary>
        private void LoadIcons()
        {
            var iconList = new IconService().Where(p => p.deleteflag == 0);

            foreach (var icon in iconList)
            {
                string text = String.Format("<img style=\"vertical-align:bottom;\" Width=\"16px\" Height=\"16px\" src=\"../../{0}\" />&nbsp;", icon.icon_src.Replace("~/", ""));
                p_rbtnICON.Items.Add(new RadioItem(text, icon.icon_src));
            }
        }

        /// <summary>
        ///     添加一级菜单
        /// </summary>
        private void AddMenu1()
        {
            int list = MenuService.Count(p => p.menu_parent_id == MenuId);
            var menu = new base_menu
            {
                menu_class_icon = hdfImage.Text,
                menu_name = txtmenu_name.Text.Trim(),
                menu_url = "#",
                menu_class = p_rbtnICON.SelectedValue.Trim(),
                menu_sort = list + 1,
                menu_parent_id = int.Parse(trMenu.SelectedNodeID),
                menu_level = 1,
                menu_is_view = 1,
                menu_is_frame_view = 1,
                deleteflag = 0
            };

            if (MenuService.Add(menu))
            {
                Alert.Show("提交成功。", MessageBoxIcon.Information);
                LoadTreeSource();
            }
            else
            {
                Alert.Show("提交失败！", MessageBoxIcon.Error);
            }
        }

        /// <summary>
        ///     添加二级菜单
        /// </summary>
        private void AddMenu2()
        {
            //组织当前附加的工能列表项（规则：action_name|action_id,....）
            var sb = new StringBuilder();
            //功能
            foreach (CheckItem item in cboxListActions.SelectedItemArray)
            {
                string actionName = item.Text;
                string actionId = item.Value;

                sb.AppendFormat("{0}|{1},", actionName, actionId);
            }
            int list = MenuService.Count(p => p.menu_parent_id == MenuId);
            var menu = new base_menu
            {
                menu_class_icon = hdfImage.Text,
                menu_name = txtmenu_name.Text.Trim(),
                menu_url = txtmenu_url.Text.Trim(),
                menu_class = p_rbtnICON.SelectedValue.Trim(),
                menu_actions = sb.ToString(),
                menu_is_view = int.Parse(rbtnListmenu_is_view.SelectedValue),
                menu_is_frame_view = int.Parse(rbtnListmenu_is_frame_view.SelectedValue),
                menu_sort = list + 1,
                menu_parent_id = MenuId,
                menu_level = 2,
                deleteflag = 0
            };

            if (MenuService.Add(menu))
            {
                Alert.Show("提交成功。", MessageBoxIcon.Information);
                LoadTreeSource();
            }
            else
            {
                Alert.Show("提交失败！", MessageBoxIcon.Error);
            }
        }

        /// <summary>
        ///     保存一级菜单
        /// </summary>
        private void EditMenu1()
        {
            BaseMenu.menu_sort = int.Parse(txtorg_sort.Text.Trim());
            BaseMenu.menu_name = txtmenu_name.Text.Trim();
            BaseMenu.menu_url = "#";
            BaseMenu.menu_class = p_rbtnICON.SelectedValue.Trim();
            BaseMenu.menu_class_icon = hdfImage.Text;
            if (MenuService.SaveChanges() > 0)
            {
                LoadTreeSource();
                Alert.Show("提交成功。", MessageBoxIcon.Information);
            }
            else
            {
                Alert.Show("提交失败！", MessageBoxIcon.Error);
            }
        }

        /// <summary>
        ///     保存二级菜单
        /// </summary>
        private void EditMenu2()
        {
            BaseMenu.menu_sort = int.Parse(txtorg_sort.Text.Trim());
            BaseMenu.menu_name = txtmenu_name.Text.Trim();
            BaseMenu.menu_url = txtmenu_url.Text.Trim();
            BaseMenu.menu_class = p_rbtnICON.SelectedValue.Trim();
            BaseMenu.menu_class_icon = hdfImage.Text;
            //组织当前附加的工能列表项（规则：action_name|action_id,....）
            var sb = new StringBuilder();
            //功能
            foreach (CheckItem item in cboxListActions.SelectedItemArray)
            {
                string actionName = item.Text;
                string actionId = item.Value;

                sb.AppendFormat("{0}|{1},", actionName, actionId);
            }
            BaseMenu.menu_actions = sb.ToString();
            BaseMenu.menu_is_view = int.Parse(rbtnListmenu_is_view.SelectedValue);
            BaseMenu.menu_is_frame_view = int.Parse(rbtnListmenu_is_frame_view.SelectedValue);

            if (MenuService.SaveChanges() > 0)
            {
                LoadTreeSource();
                Alert.Show("提交成功。", MessageBoxIcon.Information);
            }
            else
            {
                Alert.Show("提交失败！", MessageBoxIcon.Error);
            }
        }
        #endregion
    }
}