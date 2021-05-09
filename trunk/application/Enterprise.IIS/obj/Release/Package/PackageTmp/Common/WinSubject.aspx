<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WinSubject.aspx.cs" Inherits="Enterprise.IIS.Common.WinSubject" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" runat="server" Layout="Fit" ShowBorder="False" ShowHeader="false"
            BodyPadding="0px">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server" Position="Top" ToolbarAlign="Left">
                    <Items>
                        <f:Button ID="btnClose" EnablePostBack="false" Text="关闭" Size="Medium" runat="server" Icon="SystemClose">
                        </f:Button>
                        <f:Button ID="Button1" runat="server" Text="查询" Icon="Zoom" OnClick="btnSearch_Click" Size="Medium">
                        </f:Button>
                        <f:Button ID="btnConfirm" runat="server" Text="确定"
                            OnClick="btnConfirm_Click" Icon="Accept" Hidden="False" Size="Medium">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:TabStrip ID="TabStrip1" ShowBorder="false" TabPosition="Top" EnableTabCloseMenu="false"
                    ActiveTabIndex="0" runat="server">
                    <Tabs>
                        <f:Tab ID="Tab1" Title="【资产类】" BodyPadding="10px" Layout="Fit"
                            runat="server">
                            <Items>
                                <f:Tree ID="trAsset" Width="120px" OnNodeCommand="trAsset_NodeCommand" ShowHeader="False"
                                    ShowBorder="false" Icon="House" Title="资产类" runat="server"
                                    EnableArrows="true" AutoLeafIdentification="false" AutoScroll="True">
                                </f:Tree>
                            </Items>
                        </f:Tab>
                        <f:Tab ID="Tab2" Title="【负债类】" BodyPadding="10px" Layout="Fit"
                            runat="server">
                            <Items>
                                <f:Tree ID="trLiabilities" Width="120px" OnNodeCommand="trLiabilities_NodeCommand" ShowHeader="False"
                                    ShowBorder="false" Icon="House" Title="负债类" runat="server"
                                    EnableArrows="true" AutoLeafIdentification="false" AutoScroll="True">
                                </f:Tree>
                            </Items>
                        </f:Tab>
                        <f:Tab ID="Tab3" Title="【损益类】" BodyPadding="10px" Layout="Fit"
                            runat="server">
                            <Items>
                                <f:Tree ID="trLoss" Width="120px" OnNodeCommand="trLoss_NodeCommand" ShowHeader="False"
                                    ShowBorder="false" Icon="House" Title="损益类" runat="server"
                                    EnableArrows="true" AutoLeafIdentification="false" AutoScroll="True">
                                </f:Tree>
                            </Items>
                        </f:Tab>
                        <f:Tab ID="Tab4" Title="【成本类】" BodyPadding="10px" Layout="Fit"
                            runat="server">
                            <Items>
                                <f:Tree ID="trCost" Width="120px" OnNodeCommand="trCost_NodeCommand" ShowHeader="False"
                                    ShowBorder="false" Icon="House" Title="成本类" runat="server"
                                    EnableArrows="true" AutoLeafIdentification="false" AutoScroll="True">
                                </f:Tree>
                            </Items>
                        </f:Tab>
                        <f:Tab ID="Tab5" Title="【所有者权益类】" BodyPadding="10px" Layout="Fit"
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
</body>
</html>
