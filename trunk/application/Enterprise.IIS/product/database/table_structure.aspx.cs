using System;
using System.Data.Objects;
using FineUI;
using Enterprise.Data;
using Enterprise.Service.Base;
using Enterprise.Service.Base.Platform;
using Enterprise.Service.Logs.SV;

namespace Enterprise.IIS.product.database
{
// ReSharper disable once InconsistentNaming
    public partial class table_structure : PageBase
    {
        /// <summary>
        ///     数据服务
        /// </summary>
        private ProcedureService _procedureService;

        /// <summary>
        ///     列表
        /// </summary>
        private ObjectResult<proc_GetTables_Result> _tables;

        /// <summary>
        ///     数据服务
        /// </summary>
        protected ProcedureService ProcedureService
        {
            get { return _procedureService ?? (_procedureService = new ProcedureService()); }
            set { _procedureService = value; }
        }

        /// <summary>
        ///     部门列表
        /// </summary>
        protected ObjectResult<proc_GetTables_Result> Tables
        {
            get
            {
                return _tables ?? (_tables = ProcedureService.GetTables());
            }
            set { _tables = value; }
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
                //加载部门树
                LoadTreeSource();
                BindDataGrid();
            }
        }

        /// <summary>
        ///     部门树加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void trDept_NodeCommand(object sender, TreeCommandEventArgs e)
        {
            BindDataGrid();
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void trDept_NodeExpand(object sender, TreeExpandEventArgs e)
        //{
        //}

        #endregion

        #region Private Method

        /// <summary>
        ///     树绑定
        /// </summary>
        private void LoadTreeSource()
        {
            trDept.Nodes.Clear();
            
            var rootNode = new TreeNode
            {
                Text = "业务数据库",
                NodeID = "N_1",
               // EnablePostBack = true,
                EnableClickEvent = true,
                Icon = Icon.Database,
                Expanded = true
            };

            trDept.Nodes.Add(rootNode);

            var log = new TreeNode
            {
                Text = "日志数据库",
                NodeID = "N_2",
               // EnablePostBack = true,
                Icon = Icon.Database,
                Expanded = true
            };

            trDept.Nodes.Add(log);

            //加载子部门信息
            LoadChildNodes(rootNode);

            var logTables =new ProcedureLogService().GetTables();

            if (logTables!=null)
            {
                foreach (var tlog in logTables)
                {
                    var cNode = new TreeNode
                    {
                        Text = tlog.descinfo,
                        NodeID = tlog.tablename,
                        Icon = Icon.Table,
                        EnableClickEvent = true,

                //        EnablePostBack = true
                    };
                    //加载子部门信息
                    log.Nodes.Add(cNode);
                }
            }

            //设置默认选择项
            trDept.SelectedNodeID = rootNode.NodeID;
            trDept_NodeCommand(null, null);
        }

        /// <summary>
        ///     绑定子节点
        /// </summary>
        /// <param name="node"></param>
        private void LoadChildNodes(TreeNode node)
        {
            if (node == null)
                return;

            if (Tables == null)
            {
                return;
            }
            node.Expanded = true;
            node.Nodes.Clear();
            foreach (var table in Tables)
            {
                var cNode = new TreeNode
                {
                    Text = table.descinfo,
                    NodeID = table.tablename,
                    Icon = Icon.Table,
                    EnableClickEvent = true,

                 //   EnablePostBack = true
                };
                //加载子部门信息
                node.Nodes.Add(cNode);
            }
        }

        /// <summary>
        ///     绑定数据表格
        /// </summary>
        private void BindDataGrid()
        {
            var node= trDept.SelectedNode.NodeID;

            if (string.IsNullOrWhiteSpace(node))
            {
                return;
            }

            if ((node=="N_1")||(node=="N_2"))
            {
                Grid1.DataSource = ProcedureService.GetTableStructure("base_account");
                Grid1.DataBind(); 
                return;
            }

            Grid1.DataSource = ProcedureService.GetTableStructure(node);
            Grid1.DataBind();
        }
        #endregion
    }
}