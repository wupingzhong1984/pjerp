using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using FineUI;
using Enterprise.Framework.Web;
using Enterprise.Data;
using Enterprise.Service.Base;
using Enterprise.Service.Base.Platform;
using AspNet = System.Web.UI.WebControls;


namespace Enterprise.IIS.product.role
{
    /// <summary>
    /// 数据授权
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public partial class role_permission : PageBase
    {
        /// <summary>
        /// 数据授权
        /// </summary>
        private base_permission _basePermission;
        
        /// <summary>
        /// 数据授权
        /// </summary>
        protected base_permission BasePermission
        {
            get { return _basePermission ?? (_basePermission = PermissionService.FirstOrDefault(p => p.role_id == RoleId)); }
            set { _basePermission = value; }
        }

        /// <summary>
        ///     角色信息
        /// </summary>
        private base_role _baseRole;

        /// <summary>
        ///     菜单列表
        /// </summary>
        private IList<base_menu> _menus;

        /// <summary>
        ///     菜单数据服务
        /// </summary>
        private MenuService _menuService;

        /// <summary>
        ///     角色数据服务
        /// </summary>
        private RoleService _roleService;

        /// <summary>
        ///     菜单数据服务
        /// </summary>
        protected MenuService MenuService
        {
            get { return _menuService ?? (_menuService = new MenuService()); }
            set { _menuService = value; }
        }

        /// <summary>
        ///     角色数据服务
        /// </summary>
        protected RoleService RoleService
        {
            get { return _roleService ?? (_roleService = new RoleService()); }
            set { _roleService = value; }
        }

        /// <summary>
        ///     角色信息    
        /// </summary>
        protected base_role BaseRole
        {
            get { return _baseRole ?? (_baseRole = RoleService.FirstOrDefault(p => p.id == RoleId)); }
            set { _baseRole = value; }
        }

        /// <summary>
        ///     当前角色对象ID
        /// </summary>
        protected Int32 RoleId
        {
            get { return Int32.Parse(Request["id"]); }
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
        ///     菜单列表
        /// </summary>
        protected IList<base_menu> Menus
        {
            get
            {
                return _menus ?? (_menus = MenuService.All().ToList());
            }
            set { _menus = value; }
        }

        /// <summary>
        ///     公司组织数据服务
        /// </summary>
        private CompanyService _companyService;

        /// <summary>
        ///     部门组织数据服务
        /// </summary>
        private OrgnizationService _orgnizationService;

        /// <summary>
        ///     部门列表
        /// </summary>
        private IList<base_orgnization> _orgnizations;

        /// <summary>
        ///     部门组织数据服务
        /// </summary>
        protected OrgnizationService OrgnizationService
        {
            get { return _orgnizationService ?? (_orgnizationService = new OrgnizationService()); }
            set { _orgnizationService = value; }
        }

        /// <summary>
        ///     公司组织数据服务
        /// </summary>
        protected CompanyService CompanyService
        {
            get { return _companyService ?? (_companyService = new CompanyService()); }
            set { _companyService = value; }
        }

        /// <summary>
        ///     部门列表
        /// </summary>
        protected IList<base_orgnization> Orgnizations
        {
            get
            {
                return _orgnizations ?? (_orgnizations = OrgnizationService.Where(p => p.deleteflag == 0).ToList());
            }
            set { _orgnizations = value; }
        }

        /// <summary>
        /// Tree1_NodeCheck
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Tree1_NodeCheck(object sender, TreeCheckEventArgs e)
        {
            if (e.Checked)
            {
                trDept.CheckAllNodes(e.Node.Nodes);
            }
            else
            {
                trDept.UncheckAllNodes(e.Node.Nodes);
            }

        }

        /// <summary>
        /// 数据授权控制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlDatatAccess_SelectedIndexChanged(object sender, EventArgs e)
        {
            trDept.Nodes.Clear();
            var value = ddlDatatAccess.SelectedValue;
            switch (value)
            {
                case "组织机构":
                    LoadOrgnizationTreeSource();
                    break;
                case "供理商":
                    break;
                case "客户":
                    break;
                case "仓库":
                    break;
            }
        }

        /// <summary>
        /// Page_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //设置控件属性状态
                SetControlAttributeValue();

                //初始化控件数据
                InitData();

                //加载数据
                LoadData();
            }
        }

        /// <summary>
        /// SetControlAttributeValue
        /// </summary>
        private void SetControlAttributeValue()
        {
        }

        /// <summary>
        ///     初始化数据
        /// </summary>
        private void InitData()
        {
            //加载组纪机构
            LoadOrgnizationTreeSource();
        }

        /// <summary>
        /// 选中当前树的节点IDS
        /// </summary>
        /// <returns></returns>
        private string GetSelectedNodeIds()
        {
            var nodes = trDept.GetCheckedNodes();
            if (nodes.Length > 0)
            {
                var ids = new StringBuilder();
                foreach (TreeNode node in nodes)
                {
                    ids.AppendFormat("{0},", node.NodeID);
                }
                return ids.ToString();
            }
            return string.Empty;
        }

        /// <summary>
        ///     树绑定
        /// </summary>
        private void LoadOrgnizationTreeSource()
        {
            trDept.Nodes.Clear();

            base_company company = CompanyService.FirstOrDefault(p => p.deleteflag == 0);
            var rootNode = new TreeNode
            {
                Text = company.com_name,
                NodeID = company.id.ToString(CultureInfo.InvariantCulture),
                //AutoPostBack = true,
                EnableCheckBox = true,
                Icon = Icon.BulletFeed,
                Expanded = true
            };

            //设置是否选中
            SetNodeChecked(rootNode);

            trDept.Nodes.Add(rootNode);

            //加载子部门信息
            LoadChildNodes(rootNode);

            //设置默认选择项
            trDept.SelectedNodeID = rootNode.NodeID;
        }

        /// <summary>
        ///     绑定子节点
        /// </summary>
        /// <param name="node"></param>
        private void LoadChildNodes(TreeNode node)
        {
            var nodeId = Int32.Parse(node.NodeID);

            if (OrgnizationService.Count(p => p.org_parent_id == nodeId && p.deleteflag == 0) == 0)
            {
                node.Leaf = true;
            }
            else
            {
                node.Expanded = true;
                node.Nodes.Clear();
                foreach (
                    base_orgnization orgnization in
                        OrgnizationService.Where(p => p.org_parent_id == nodeId && p.deleteflag == 0))
                {
                    var cNode = new TreeNode
                    {
                        Text = string.Format(@"{0}-{1}", orgnization.code, orgnization.org_name),
                        NodeID = orgnization.id.ToString(CultureInfo.InvariantCulture),
                        EnableCheckBox = true,
                        Expanded = true,
                        //AutoPostBack = true,
                        Icon = Icon.BulletFeed
                    };
                    //加载子部门信息
                    node.Nodes.Add(cNode);

                    //设置是否选中
                    SetNodeChecked(cNode);
                    //
                    LoadChildNodes(cNode);
                }
            }
        }

        /// <summary>
        ///     加载所有数据列表
        /// </summary>
        private void LoadData()
        {
            
        }

        private void SetNodeChecked(TreeNode node)
        {
            //取前权限信息
            if (BasePermission != null)
            {
                if (!string.IsNullOrWhiteSpace(BasePermission.orgnization_ids))
                {
                    var ids = BasePermission.orgnization_ids.Split(',');
                    if (ids.Contains(node.NodeID))
                        node.Checked = true;
                    }
            }
        }

        /// <summary>
        ///     提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSava_Click(object sender, EventArgs e)
        {
            var value = ddlDatatAccess.SelectedValue;
            //取当前选中的节点IDS
            var ids = GetSelectedNodeIds();

            if (string.IsNullOrWhiteSpace(ids))
            {
                Alert.Show("至少选择一项。", MessageBoxIcon.Warning);
                return;
            }
            //验证部分
            if (BasePermission != null)
            {
                switch (value)
                {
                    case "组织机构":
                        BasePermission.orgnization_ids = ids;
                        break;
                    case "供理商":
                        BasePermission.supplier_ids = ids;
                        break;
                    case "客户":
                        BasePermission.customer_ids = ids;
                        break;
                    case "仓库":
                        BasePermission.storage_ids = ids;
                        break;
                }
                if (PermissionService.SaveChanges() >= 0)
                {
                    Alert.Show("提交成功。", MessageBoxIcon.Information);
                }
                else
                {
                    Alert.Show("提交失败！", MessageBoxIcon.Error);
                }
            }
            else
            {
                switch (value)
                {
                    case "组织机构":
                        BasePermission = new base_permission
                        {
                            orgnization_ids = ids,
                            role_id =RoleId, 
                            deleteflag = 0
                        };
                        break;
                    case "供理商":
                        BasePermission = new base_permission
                        {
                            supplier_ids = ids,
                            role_id = RoleId, 
                            deleteflag = 0
                        };
                        break;
                    case "客户":
                        BasePermission = new base_permission
                        {
                            customer_ids = ids,
                            role_id = RoleId, 
                            deleteflag = 0
                        };
                        break;
                    case "仓库":
                        BasePermission = new base_permission
                        {
                            storage_ids = ids,
                            role_id = RoleId, 
                            deleteflag = 0
                        };
                        break;
                }

                if (PermissionService.Add(BasePermission))
                {
                    Alert.Show("提交成功。", MessageBoxIcon.Information);
                }
                else
                {
                    Alert.Show("提交失败！", MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        ///     返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request["ReturnUrl"] ?? "role_list.aspx");
        }
    }
}