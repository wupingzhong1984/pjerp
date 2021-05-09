<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Enterprise.IIS._default" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title>非凡通</title>
    <meta http-equiv="Content-Type" content="text/html; charset=gbk">
    <meta name="renderer" content="webkit">
    <meta http-equiv="X-UA-Compatible" content="IE=10,chrome=1">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimum-scale=1.0, maximum-scale=1.0">

    <script type="text/javascript" src="jqueryui/js/jquery-1.8.3.min.js"></script>
    <script type="text/javascript" src="js/FusionCharts.js"></script>
    <link href="res/css/common.css" rel="stylesheet" />
    <link href="css/default.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" href="/Enterprise.ico" />


    <style type="text/css">
        .x-btn-default-toolbar-small-icon-text-top .x-btn-icon-el {
            bottom: auto;
            height: 50px;
        }

        .x-btn-default-toolbar-small::after {
            display: none;
        }

        .bgbtn2 {
            background-color: transparent;
            background-position: center center;
            background-repeat: no-repeat;
            border-width: 0;
            height: 78px;
            width: 78px;
        }

            .bgbtn2.img:hover {
                animation: tada 0.8s ease-in-out;
                -webkit-animation: tada 0.8s ease-in-out;
                -moz-animation: tada 0.8s ease-in-out;
            }

        .x-btn-default-toolbar-small-icon-text-top .x-btn-inner {
            padding-top: 50px;
        }

        .f-tree-node-el {
            line-height: 30px; /*节点高度，行高*/
            cursor: pointer;
        }
        /*树节点 节点图标与节点文字间的距离*/
        .f-tree-node a span, .f-dd-drag-ghost a span {
            text-decoration: none;
            padding: 0 0 0 5px; /*第四个参数是调整文字离图标左边的距离*分别设置上、右、下、左内边距*/
        }
        /*消息框*/
        .f-icon-error {
            background-image: url('js/exclamation.gif') !important;
        }

        .f-icon-information {
            background-image: url('icon/sound.png') !important;
        }

        .f-notification .f-window-body {
            margin: 0;
            padding: 1em !important;
            width: auto !important;
        }

        .f-notification .f-tool-close {
            display: none;
        }

        .f-notification.fixed .f-tool-close {
            display: block;
        }
        /*消息框*/
        .welcome {
            background-image: url("images/welcome.jpg");
            background-position: 30% 72%;
            background-repeat: repeat;
            height: 600px;
        }

        .bottomtable {
            width: 100%;
            font-size: 12px;
        }
        /* 修正选项卡标题中放置红色[New!]时，底部出现的一行空白线 */
        .f-theme-neptune .x-tab .x-tab-inner {
            line-height: 16px !important;
        }
        /* 主题相关样式 - neptune */
        .f-theme-neptune #header,
        .f-theme-neptune .bottomtable,
        .f-theme-neptune .x-splitter {
            background-color: #1475BB;
            color: #fff;
        }

            .f-theme-neptune #header a,
            .f-theme-neptune .bottomtable a {
                color: #fff;
            }
        /* 主题相关样式 - blue */
        .f-theme-blue #header,
        .f-theme-blue .bottomtable {
            background-color: #DFE8F6;
            color: #000;
        }

            .f-theme-blue .bottomtable a {
                color: #000;
            }
        /* 主题相关样式 - gray */
        .f-theme-gray #header,
        .f-theme-gray .bottomtable {
            background-color: #E0E0E0;
            color: #333;
        }

            .f-theme-gray #header a,
            .f-theme-gray .bottomtable a {
                color: #333;
            }
        /* 主题相关样式 - access */
        .f-theme-access #header,
        .f-theme-access .bottomtable {
            background-color: #3F4757;
            color: #fff;
        }

            .f-theme-access #header a,
            .f-theme-access .bottomtable a {
                color: #fff;
            }
            
        .x-tree-node-text {
            font-size: 18px;
            line-height: 18px;
            padding-left: 4px;
        }
        .f-theme-access .maincontent .x-panel-body {
            background-image: none;
        }

        #RegionPanel1_bottomPanel_ContentPanel-innerCt {
            /*background-image: url(./images/head_bg.jpg);*/
            /*background: rgba(172, 209, 236, 1)*/
            background-color: #1475bb;
        }

        #RegionPanel1_mainRegion_mainTabStrip_ctl00_Panel2_Panel1_Panel3_header {
            background-color: #dfeaf2;
        }

        #RegionPanel1_mainRegion_mainTabStrip_ctl00_Panel2_Panel1_Panel4_header {
            background-color: #dfeaf2;
        }

        #RegionPanel1_mainRegion_mainTabStrip_ctl00_Panel2_panelCenterRegion_panelRightRegion_header {
            background-color: #dfeaf2;
        }

        #RegionPanel1_mainRegion_mainTabStrip_ctl00_Panel2_Panel5_Panel8_header {
            background-color: #dfeaf2;
        }
        #RegionPanel1_mainRegion_mainTabStrip_ctl00_Panel2_Panel5_Panel8_header_hd-textEl {
            color: WindowText;
            font-size: 13px;
        }
        #RegionPanel1_mainRegion_mainTabStrip_ctl00_Panel2_panelCenterRegion_header {
            background-color: #dfeaf2;
        }

        #RegionPanel1_mainRegion_mainTabStrip_ctl00_Panel2_panelCenterRegion_header_hd-textEl {
            color: WindowText;
            font-size: 13px;
        }

        #RegionPanel1_mainRegion_mainTabStrip_ctl00_Panel2_panelCenterRegion_panel1_header {
            background-color: #dfeaf2;
        }

        #RegionPanel1_mainRegion_mainTabStrip_ctl00_Panel2_Panel5_Panel7_header {
            background-color: #dfeaf2;
        }

        #RegionPanel1_mainRegion_mainTabStrip_ctl00_Panel2_Panel5_Panel7_header_hd-textEl {
            color: WindowText;
            font-size: 13px;
        }

        #RegionPanel1_mainRegion_mainTabStrip_ctl00_Panel2_panelCenterRegion_panelRightRegion_header_hd-textEl {
            color: WindowText;
            font-size: 13px;
        }

        #RegionPanel1_mainRegion_mainTabStrip_ctl00_Panel2_Panel1_Panel3_header_hd-textEl {
            color: WindowText;
            font-size: 13px;
        }

        #RegionPanel1_mainRegion_mainTabStrip_ctl00_Panel2_Panel1_Panel4_header_hd-textEl {
            color: WindowText;
            font-size: 13px;
        }

        #RegionPanel1_mainRegion_mainTabStrip_ctl00_Panel2_panelCenterRegion_panel1_header_hd-textEl {
            color: WindowText;
            font-size: 13px;
        }

        #RegionPanel1_mainRegion_mainTabStrip_ctl00_Panel2_Panel6_Toolbar1-innerCt {
            background-color: white;
        }

        #RegionPanel1_mainRegion_mainTabStrip_ctl00_Panel2_panelLeftRegion_header_hd-textEl {
            color: WindowText;
            font-size: 13px;
        }

        .bgbtn {
            background-image: url(images/AddIcon.png) !important;
            background-position: center;
            background-repeat: no-repeat;
            width: 88px;
            height: 88px;
            border-width: 0;
            background-color: transparent;
        }

        .bgbtn2 {
            background-position: center;
            background-repeat: no-repeat;
            width: 88px;
            height: 88px;
            border-width: 0;
            background-color: transparent;
        }

        .x-toolbar.x-toolbar-default.x-docked-top {
            background-color: #ffffff;
            border-bottom-color: #ccc !important;
            border-bottom-width: 1px !important;
        }

        .x-btn-icon-el {
            color: #000000;
        }

        .mytable td.x-table-layout-cell {
            padding: 5px;
        }

        .mytable td.f-layout-table-cell {
            padding: 5px;
        }

        .x-btn-default-toolbar-small-icon-text-top .x-btn-icon-el {
            bottom: auto;
            height: 72px;
        }

        .x-btn-default-toolbar-small-icon-text-top .x-btn-inner {
            padding-top: 70px;
        }

        .x-toolbar.x-toolbar-default .x-btn-default-toolbar-small {
            border-color: #ffffff;
        }

        .x-btn-default-toolbar-small::after {
            display: none;
        }

        .x-toolbar.x-toolbar-default {
            background-color: #3892d3;
            border-color: #126daf;
        }

        .x-toolbar-default.x-toolbar .x-btn-default-toolbar-small {
            background: none;
        }

            .x-toolbar-default.x-toolbar .x-btn-default-toolbar-small .x-btn-inner {
                color: #000000;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server"></f:PageManager>
        <%--<f:Timer runat="server" ID="Timer1" OnTick="Timer1_OnTickOnTick" Interval="10" />--%>
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="bottomPanel" RegionPosition="Top" ShowBorder="false" ShowHeader="false" EnableCollapse="false" runat="server" Layout="Fit">
                    <Items>
                        <f:ContentPanel ShowBorder="false" ShowHeader="false"
                            ID="ContentPanel" runat="server" CssClass="luheng">
                            <div class="logo">
                                <a href="http://www.ffqs.com/" target="_blank" title="首页">
                                    <img src="./images/logo.png" alt="Logo" /></a> &nbsp;
                            </div>
                            <div class="title">
                                <a href="./default.aspx" style="color: #fff;">
                                    <asp:Literal ID="lblSystemName" runat="server"></asp:Literal></a>
                            </div>
                            <div id="currentTime" class="version">
                                <f:Button ID="btnDownloadPrinter" OnClick="btnDownloadPrinter_Click" EnableAjax="false" runat="server" Icon="PrinterConnect" Text="安装打印" CssClass="icontopaction" IconAlign="Top">
                                </f:Button>
                                <f:Button ID="btnRelease" EnablePostBack="false" Icon="Time" Text="发布历史" OnClientClick="addExampleTab('Release','product/version/index.html','发布历史','icon/Time.png');" CssClass="icontopaction" runat="server" IconAlign="Top">
                                </f:Button>
                                <f:Button ID="btnPassword" runat="server" Icon="Key" Text="修改密码" CssClass="icontopaction" EnablePostBack="False" IconAlign="Top">
                                </f:Button>
                                <f:Button ID="btnExit" runat="server" Icon="UserGo" Text="用户注销" CssClass="icontopaction" ConfirmText="您确定要注销当前用户？"
                                    OnClick="btnExit_Click" EnablePostBack="True" IconAlign="Top">
                                </f:Button>
                                &nbsp;&nbsp;
                            </div>
                        </f:ContentPanel>
                    </Items>
                </f:Region>
                <f:Region ID="leftPanel" RegionSplit="true" Width="188px" ShowHeader="true" ShowBorder="true" Title="功能管理"
                    EnableCollapse="true" Layout="Fit" Collapsed="false" RegionPosition="Left" runat="server">
                </f:Region>
                <f:Region ID="mainRegion" ShowHeader="false" Layout="Fit" Position="Center" ShowBorder="False"
                    runat="server">
                    <Items>
                        <f:TabStrip ID="mainTabStrip" EnableTabCloseMenu="true" ShowBorder="false" runat="server">
                            <Tabs>
                                <f:Tab Title="首页" Layout="Fit" Icon="House" CssClass="maincontent" runat="server">
                                    <Items>
                                        <f:Panel ID="Panel2" runat="server" ShowBorder="False"
                                            Layout="Region" Height="600px" ShowHeader="False" Title="账户信息" BodyPadding="0px">
                                            <Items>
                                                <f:Panel ID="Panel6" runat="server"
                                                    ShowBorder="False" ShowHeader="False" Height="206px" RegionPosition="Top">
                                                    <Toolbars>
                                                    </Toolbars>
                                                </f:Panel>
                                                <f:Panel ID="Panel5" BoxFlex="1" runat="server"
                                                    ShowBorder="false" ShowHeader="false" Layout="HBox" BoxConfigChildMargin="0 5 0 0">
                                                    <Items>
                                                        <f:Panel Icon="Table" ID="Panel7" BoxFlex="1" runat="server" ShowBorder="False" ShowHeader="True" Title="代办工作">
                                                            <Items>
                                                                <f:Grid ID="Grid2" Title="代办工作" PageSize="6" ShowBorder="false" ShowHeader="false"
                                                                    runat="server" EnableCheckBoxSelect="False"
                                                                    DataKeyNames="KeyId,FUrl" IsDatabasePaging="False"
                                                                    AllowSorting="False" OnRowCommand="Grid2_RowCommand"
                                                                    EmptyText="查询无结果" EnableHeaderMenu="True">
                                                                    <Columns>
                                                                        <f:LinkButtonField MinWidth="120px" CommandName="actView" DataTextField="KeyId"
                                                                             HeaderText="单据号" SortField="KeyId" TextAlign="Center" runat="server" />
                                                                        <f:BoundField MinWidth="400px" DataField="FMemo" HeaderText="摘要" SortField="FMemo" />
                                                                    </Columns>
                                                                </f:Grid>
                                                            </Items>
                                                        </f:Panel>
                                                        <f:Panel Icon="Table" ID="Panel8" BoxFlex="1" runat="server" ShowBorder="False" ShowHeader="True" Title="通知公告">
                                                            <Items>
                                                                <f:Grid ID="Grid1" Title="通知公告" PageSize="6" ShowBorder="false" ShowHeader="False"
                                                                    runat="server" EnableCheckBoxSelect="False"
                                                                    DataKeyNames="id,sequence_id" IsDatabasePaging="False"
                                                                    AllowSorting="False" OnRowCommand="Grid1_RowCommand"
                                                                    EmptyText="查询无结果" EnableHeaderMenu="True">
                                                                    <Columns>
                                                                        <f:BoundField MinWidth="100px" DataField="pubdate" TextAlign="Center" DataFormatString="{0:yyyy-MM-dd}" HeaderText="公告日期" SortField="pubdate" />
                                                                        <f:BoundField MinWidth="380px" DataField="m_content" TextAlign="Left" HeaderText="内容" SortField="m_content" />
                                                                        <f:LinkButtonField HeaderText="详情" MinWidth="80px" CommandName="actView" Text="详情" />
                                                                    </Columns>
                                                                </f:Grid>
                                                            </Items>
                                                        </f:Panel>
                                                    </Items>
                                                </f:Panel>
                                            </Items>
                                        </f:Panel>
                                    </Items>
                                </f:Tab>
                            </Tabs>
                        </f:TabStrip>
                    </Items>
                </f:Region>
                <f:Region ID="Region1" RegionPosition="Bottom" ShowBorder="false" ShowHeader="false" EnableCollapse="false" runat="server" Layout="Fit">
                    <Items>
                        <f:ContentPanel runat="server" ShowBorder="false" ShowHeader="false">
                            <table class="bottomtable">
                                <tr>
                                    <td style="width: 300px;">欢迎您：<asp:Literal ID="ltlUser" runat="server"></asp:Literal></td>
                                    <td style="text-align: center;">COPYRIGHT © 非凡气市网络科技江苏有限公司 www.ffqs.com 苏ICP备14041116号&nbsp;版本：<asp:Literal ID="ltlVersion" runat="server"></asp:Literal></td>
                                    <td style="width: 300px; text-align: right;">IP所在地：<asp:Literal ID="ltlOnline" runat="server"></asp:Literal>&nbsp;</td>
                                </tr>
                            </table>
                        </f:ContentPanel>
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>
        <f:Menu ID="menuSettings" runat="server">
            <f:MenuButton ID="btnExpandAll" IconUrl="~/icon/images/expand-all.gif" Text="展开菜单" EnablePostBack="false"
                runat="server">
            </f:MenuButton>
            <f:MenuButton ID="btnCollapseAll" IconUrl="~/icon/images/collapse-all.gif" Text="折叠菜单"
                EnablePostBack="false" runat="server">
            </f:MenuButton>
        </f:Menu>
        <f:Window ID="window1" Icon="Key" runat="server" Hidden="true"
            IsModal="true" Target="Parent" EnableMaximize="true" EnableResize="true" OnClose="Window1_Close"
            Title="修改密码" CloseAction="HidePostBack" EnableClose="True"
            EnableIFrame="true" Height="220px" Width="360px">
        </f:Window>
        <f:Window ID="window2" Icon="TimeRed" runat="server" Hidden="true"
            IsModal="true" Target="Parent" EnableMaximize="true" EnableResize="true" OnClose="Window2_Close"
            Title="超时显示客户" CloseAction="HidePostBack" EnableClose="True"
            EnableIFrame="true" Height="520px" Width="960px">
        </f:Window>
        <f:Window ID="window3" Icon="ApplicationAdd" runat="server" Hidden="true"
            IsModal="true" Target="Parent" EnableMaximize="true" EnableResize="true" OnClose="Window2_Close"
            Title="定义常用功能" CloseAction="HidePostBack" EnableClose="True"
            EnableIFrame="true" Height="480px" Width="400px">
        </f:Window>
    </form>
    <script src="./jqueryui/js/jquery-1.8.3.min.js?v=4" type="text/javascript"></script>
    <script src="./jqueryui/js/jquery-ui-1.9.2.custom.js?v=4.0.0" type="text/javascript"></script>
    <script type="text/javascript">
        var basePath = '<%= ResolveUrl("~/") %>';
        var btnExpandAllClientID = '<%= btnExpandAll.ClientID %>';
        var btnCollapseAllClientID = '<%= btnCollapseAll.ClientID %>';
        var leftPanelClientID = '<%= leftPanel.ClientID %>';
        var mainTabStripClientID = '<%= mainTabStrip.ClientID %>';
        var menuSettingsClientID = '<%= menuSettings.ClientID %>';
        function onReload() {
            location.reload();
        }
        var basePath = '<%= ResolveUrl("~/") %>';
        function openAddFineUI(url) {
            parent.addExampleTab.apply(null, ['add_fineui_tab', basePath + url, '查情', basePath + 'icon/page_find.png', true]);
        }

        function openDetailsUI(url, keyid) {
            parent.addExampleTab.apply(null, ['add_tab_' + keyid, basePath + url, '【单据' + keyid + '详情】', basePath + 'icon/page_find.png', true]);
        };

        function closeActiveTab() {
            parent.removeActiveTab();
        }
        F.ready(function () {
            $('ul.icons li').hover(function () {
                $(this).addClass('ui-state-hover');
            }, function () {
                $(this).removeClass('ui-state-hover');
            });

            var btnExpandAll = F(btnExpandAllClientID);
            var btnCollapseAll = F(btnCollapseAllClientID);
            var leftPanel = F(leftPanelClientID);
            var mainTabStrip = F(mainTabStripClientID);
            var menuSettings = F(menuSettingsClientID);
            var mainMenu = leftPanel.items.getAt(0);
            var menuType = 'accordion';
            if (mainMenu.isXType('treepanel')) {
                menuType = 'menu';
            }
            function getExpandedPanel() {
                var panel = null;
                mainMenu.items.each(function (item) {
                    if (!item.getCollapsed()) {
                        panel = item;
                    }
                });
                return panel;
            }

            // 点击展开菜单
            btnExpandAll.on('click', function () {
                if (menuType == 'menu') {
                    mainMenu.expandAll();
                } else {
                    var expandedPanel = getExpandedPanel();
                    if (expandedPanel) {
                        expandedPanel.items.getAt(0).expandAll();
                    }
                }
            });

            // 点击折叠菜单
            btnCollapseAll.on('click', function () {
                if (menuType == 'menu') {
                    mainMenu.collapseAll();
                } else {
                    var expandedPanel = getExpandedPanel();
                    if (expandedPanel) {
                        expandedPanel.items.getAt(0).collapseAll();
                    }
                }
            });

            // 初始化主框架中的树(或者Accordion+Tree)和选项卡互动，以及地址栏的更新
            F.util.initTreeTabStrip(mainMenu, mainTabStrip, null, true, false, false);

            // 添加示例标签页
            window.addExampleTab = function (id, url, text, icon, refreshWhenExist) {

                F.util.addMainTab(mainTabStrip, id, url, text, icon, null, refreshWhenExist);
            };

            // 移除选中标签页
            window.removeActiveTab = function () {
                var activeTab = mainTabStrip.getActiveTab();
                mainTabStrip.removeTab(activeTab.id);
            };

            // 添加工具图标，并在点击时显示上下文菜单
            leftPanel.addTool({
                type: 'gear',
                tooltip: '系统设置',
                handler: function (event) {
                    menuSettings.showBy(this);
                }
            });
        });

        function openWindow(pageUrl, pageName) {
            parent.addExampleTab.apply(null, ['add_tab_' + Math.random(), basePath + pageUrl, '【' + pageName + '】', basePath + 'icon/page.png', true]);
        };
    </script>
</body>
</html>
