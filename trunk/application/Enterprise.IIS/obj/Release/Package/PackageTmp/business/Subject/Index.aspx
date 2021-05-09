<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Enterprise.IIS.business.Subject.Index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
    
        <style type="text/css">
            .important .x-tree-node-text {
                color: red;
                font-weight: bold;
            }
        </style>

</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" runat="server" Layout="Fit" ShowBorder="False" ShowHeader="false"
            BodyPadding="0px">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server" Position="Top" ToolbarAlign="Left">
                    <Items>
                        <f:Button ID="btnSearch" OnClick="btnSearch_Click" runat="server" Text="查询" Icon="Zoom" Hidden="False" Size="Medium">
                        </f:Button>
                        <f:Button ID="btnAdd" OnClick="btnAdd_Click" runat="server" Text="新增" Icon="Add" Hidden="True" Size="Medium">
                        </f:Button>
                        <f:Button ID="btnEdit" OnClick="btnEdit_Click" runat="server" Hidden="True" Text="编辑"
                            Icon="PageWhiteEdit" Size="Medium">
                        </f:Button>
                        <f:Button ID="btnInvalid" OnClick="btnInvalid_Click" runat="server" Text="作废"
                            Icon="Delete" Hidden="False" Size="Medium">
                        </f:Button>
                        <f:Button ID="btnPrint" OnClick="btnPrint_Click" runat="server" Text="打印"
                            Icon="Printer" Hidden="False" Size="Medium">
                        </f:Button>
                        <f:Button ID="btnExport" OnClick="btnExport_Click" runat="server" Text="引出" Icon="PageExcel"
                            Hidden="False" EnableAjax="false" DisableControlBeforePostBack="false" Size="Medium">
                        </f:Button>
                        <f:Button ID="btnClose" EnablePostBack="False" Text="关闭" runat="server" 
                            Icon="SystemClose" Size="Medium" onClientClick="closeActiveTab();">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:TabStrip ID="TabStrip1" ShowBorder="false" TabPosition="Top" EnableTabCloseMenu="false"
                    ActiveTabIndex="0" runat="server">
                    <Tabs>
                        <f:Tab ID="Tab1" Title="【01资产类】" BodyPadding="10px" Layout="Fit"
                            runat="server">
                            <Items>
                                <f:Tree ID="trAsset" Width="120px" OnNodeCommand="trAsset_NodeCommand" ShowHeader="False"
                                    ShowBorder="false" Icon="House" Title="资产类" runat="server"
                                    EnableArrows="true" AutoLeafIdentification="false" AutoScroll="True">
                                </f:Tree>
                            </Items>
                        </f:Tab>
                        <f:Tab ID="Tab2" Title="【02负债类】" BodyPadding="10px" Layout="Fit"
                            runat="server">
                            <Items>
                                <f:Tree ID="trLiabilities" Width="120px" OnNodeCommand="trLiabilities_NodeCommand" ShowHeader="False"
                                    ShowBorder="false" Icon="House" Title="负债类" runat="server"
                                    EnableArrows="true" AutoLeafIdentification="false" AutoScroll="True">
                                </f:Tree>
                            </Items>
                        </f:Tab>
                        <f:Tab ID="Tab3" Title="【03损益类】" BodyPadding="10px" Layout="Fit"
                            runat="server">
                            <Items>
                                <f:Tree ID="trLoss" Width="120px" OnNodeCommand="trLoss_NodeCommand" ShowHeader="False"
                                    ShowBorder="false" Icon="House" Title="损益类" runat="server"
                                    EnableArrows="true" AutoLeafIdentification="false" AutoScroll="True">
                                </f:Tree>
                            </Items>
                        </f:Tab>
                        <f:Tab ID="Tab4" Title="【04成本类】" BodyPadding="10px" Layout="Fit"
                            runat="server">
                            <Items>
                                <f:Tree ID="trCost" Width="120px" OnNodeCommand="trCost_NodeCommand" ShowHeader="False"
                                    ShowBorder="false" Icon="House" Title="成本类" runat="server"
                                    EnableArrows="true" AutoLeafIdentification="false" AutoScroll="True">
                                </f:Tree>
                            </Items>
                        </f:Tab>
                        <f:Tab ID="Tab5" Title="【05所有者权益类】" BodyPadding="10px" Layout="Fit"
                            runat="server">
                            <Items>
                                <f:Tree ID="trEquity" Width="120px" OnNodeCommand="trEquity_NodeCommand" ShowHeader="False"
                                    ShowBorder="false" Icon="House" Title="成本类" runat="server"
                                    EnableArrows="true" AutoLeafIdentification="false" AutoScroll="True">
                                </f:Tree>
                            </Items>
                        </f:Tab>
                    </Tabs>
                </f:TabStrip>
            </Items>
        </f:Panel>
        <br />
        <f:Window ID="Window1" runat="server" WindowPosition="Center" IsModal="true" Hidden="True"
            Icon="ApplicationViewDetail" EnableMaximize="true" EnableResize="true" Target="Parent" EnableIFrame="true"
            IFrameUrl="about:blank" Height="300px" Width="450px" OnClose="Window1_Close">
        </f:Window>
    </form>
    
    <object id="LODOP_OB" classid="clsid:2105C259-1E0C-4534-8141-A753534CB4CA" width="0" height="0">
        <embed id="LODOP_EM" type="application/x-print-lodop" width="0" height="0"></embed>
    </object>
    <script src="../../jqueryui/js/jquery-1.8.3.min.js" type="text/javascript"> </script>
    <script src="../../jqueryui/js/jquery-ui-1.9.2.custom.js?v=4.0" type="text/javascript"> </script>
    <script type="text/javascript" language="javascript" src="../../js/LodopFuncs.js"></script>
    <script type="text/javascript">

        function closeActiveTab() {
            parent.removeActiveTab();
        };

    </script>
</body>
</html>
