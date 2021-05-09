using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Web.UI;
using FineUI;
using Enterprise.Service.Base;
using Enterprise.Service.Base.Platform;
using Enterprise.Data;

namespace Enterprise.IIS.product.bulletin
{
// ReSharper disable once InconsistentNaming
    public partial class select_orgnization_list : PageBase
    {
        /// <summary>
        ///     帐号数据服务
        /// </summary>
        private AccountService _accountService;

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
        ///     排序字段
        /// </summary>
        protected string SortField
        {
            get
            {
                string sort = ViewState["SortField"] != null ? ViewState["SortField"].ToString() : "id";
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
                string sort = ViewState["SortDirection"] != null ? ViewState["SortDirection"].ToString() : "ASC";
                return sort;
            }
            set { ViewState["SortDirection"] = value; }
        }

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
        ///     帐号数据服务
        /// </summary>
        protected AccountService AccountService
        {
            get { return _accountService ?? (_accountService = new AccountService()); }
            set { _accountService = value; }
        }

        /// <summary>
        ///     部门列表
        /// </summary>
        protected IList<base_orgnization> Orgnizations
        {
            get
            {
                return _orgnizations ?? (_orgnizations = OrgnizationService.Where(p => p.deleteflag == 0 && p.FCompanyId == CurrentUser.AccountComId).ToList());
            }
            set { _orgnizations = value; }
        }

        /// <summary>
        ///     页面初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //加载部门树
                LoadTreeSource();
            }
        }

        /// <summary>
        ///     部门树加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void trDept_NodeCommand(object sender, TreeCommandEventArgs e)
        {

        }

        protected void trDept_NodeCheck(object sender, TreeCheckEventArgs e)
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
        ///     树绑定
        /// </summary>
        private void LoadTreeSource()
        {
            trDept.Nodes.Clear();

            base_company company = CompanyService.FirstOrDefault(p => p.deleteflag == 0 && p.id == CurrentUser.AccountComId);
            var rootNode = new TreeNode
            {
                Text = company.com_name,
                NodeID = company.id.ToString(CultureInfo.InvariantCulture),
                Expanded = true,
                EnableClickEvent = true,
                EnableCheckBox=true
            };

            trDept.Nodes.Add(rootNode);

            //加载子部门信息
            LoadChildNodes(rootNode);

            //设置默认选择项
            trDept.SelectedNodeID = rootNode.NodeID;
            
            //trDept_NodeCommand(null, null);
        }

        /// <summary>
        ///     绑定子节点
        /// </summary>
        /// <param name="node"></param>
        private void LoadChildNodes(TreeNode node)
        {
            var nodeId = Int32.Parse(node.NodeID);

            if (OrgnizationService.Count(p => p.org_parent_id == nodeId && p.deleteflag == 0 && p.FCompanyId == CurrentUser.AccountComId) == 0)
            {
                node.Leaf = true;
            }
            else
            {
                node.Expanded = true;
                node.Nodes.Clear();
                foreach (
                    base_orgnization orgnization in
                        OrgnizationService.Where(p => p.org_parent_id == nodeId && p.deleteflag == 0 && p.FCompanyId == CurrentUser.AccountComId))
                {
                    var cNode = new TreeNode
                    {
                        Text = orgnization.org_name,
                        NodeID = orgnization.id.ToString(CultureInfo.InvariantCulture),
                        Expanded = true,
                        EnableClickEvent = true,
                        EnableCheckBox = true
                    };
                    //加载子部门信息
                    node.Nodes.Add(cNode);

                    //加载子部门信息
                    LoadChildNodes(cNode);
                }
            }
        }
         
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {

               var nodeIDs=  trDept.GetCheckedNodeIDs();
               if (nodeIDs.Length == 0)
               {
                   Alert.Show("请至少选择一项！", MessageBoxIcon.Information);
               }
               else
               {
                   var accounts = new StringBuilder();
                   foreach (var node in nodeIDs)
                   {
                       if (!string.IsNullOrWhiteSpace(node))
                       {
                           accounts.Append(node + ",");
                       }
                   }
                   PageContext.RegisterStartupScript(ActiveWindow.GetWriteBackValueReference(accounts.ToString().TrimEnd(',')) + ActiveWindow.GetHideReference());
               }
            }
            catch (Exception)
            {
                Alert.Show("操作失败！", MessageBoxIcon.Warning);
            }
        }
    }
}