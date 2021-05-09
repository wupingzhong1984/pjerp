<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InitCustomerList.aspx.cs" Inherits="Enterprise.IIS.business.Init.InitCustomerList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>设置客户对应产品销售价</title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
    <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
        <Regions>
            <f:Region ID="Region1" Title="设置客户对应产品销售价" Position="Center"
                ShowBorder="True" BoxConfigAlign="Stretch" Layout="Fit" runat="server" ShowHeader="False">
                <Toolbars>
                    <f:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <f:Button ID="btnImport" OnClick="btnImport_Click" runat="server" Text="引入" Icon="PageExcel"
                                    Hidden="False" EnableAjax="false" DisableControlBeforePostBack="false" Size="Medium">
                                </f:Button>
                            <f:Button ID="btnExport" OnClick="btnExport_Click" runat="server" Text="引出" Icon="PageExcel"
                                Hidden="False" EnableAjax="false" DisableControlBeforePostBack="false" Size="Medium">
                            </f:Button>
                        </Items>
                    </f:Toolbar>
                </Toolbars>
                <Items>
                    <f:RegionPanel ID="RegionPanel2" ShowBorder="false" runat="server">
                        <Regions>
                            <f:Region ID="Region3" ShowBorder="false" ShowHeader="false" Split="False"
                                 Position="Top" Layout="Fit"
                                BodyPadding="3px" runat="server" Height="40px" EnableCollapse="True">
                                <Items>
                                    <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="80px"
                                        runat="server" BodyPadding="5px">
                                        <Rows>
                                            <f:FormRow ID="FormRow1">
                                                <Items>
                                                    <f:TextBox ID="txtFName" runat="server" Label="客户名称">
                                                    </f:TextBox>
                                                    <f:Button ID="btnSearch" runat="server" Text="查询" Icon="Zoom" OnClick="btnSearch_Click">
                                                    </f:Button>
                                                </Items>
                                            </f:FormRow>
                                        </Rows>
                                    </f:Form>
                                </Items>
                            </f:Region>
                            <f:Region ID="Region4" ShowBorder="False" ShowHeader="false" Position="Center"
                                Layout="Fit" runat="server" BodyPadding="3px" AutoScroll="True">
                                <Items>
                                    <f:Grid ID="Grid1" ShowBorder="False" ShowHeader="False" AllowPaging="False"
                                        runat="server" EnableCheckBoxSelect="False" DataKeyNames="FCode"
                                        IsDatabasePaging="False" OnSort="Grid1_Sort" SortDirection="ASC"
                                        AllowSorting="False" EmptyText="查询无结果" EnableHeaderMenu="True">
                                        <Columns>
                                            <f:BoundField MinWidth="80px" DataField="FDate" HeaderText="年月" SortField="FDate" />
                                            <f:BoundField MinWidth="80px" DataField="FCode" HeaderText="客户代码" SortField="FCode" />
                                            <f:BoundField MinWidth="220px" DataField="FName" HeaderText="客户名称" SortField="FName" />
                                            <f:BoundField MinWidth="120px" DataField="FLinkman" HeaderText="联系人" SortField="FLinkman" />
                                            <f:BoundField MinWidth="120px" DataField="FPhome" HeaderText="电话" SortField="FPhome" />
                                            <f:BoundField MinWidth="120px" DataField="FAmt" HeaderText="应收金额" SortField="FAmt" />
                                        </Columns>
                                    </f:Grid>
                                </Items>
                            </f:Region>
                        </Regions>
                    </f:RegionPanel>
                </Items>
            </f:Region>
        </Regions>
    </f:RegionPanel>
    <br />
    <f:Window ID="Window1" runat="server" WindowPosition="Center" IsModal="true"
        Icon="ApplicationViewDetail" EnableMaximize="true" EnableResize="true" Hidden="True"
        Target="Parent" EnableIFrame="true" IFrameUrl="about:blank" Height="430px" Width="760px"
        OnClose="Window1_Close">
    </f:Window>
    </form>
   <script type="text/javascript">
        var basePath = '<%= ResolveUrl("~/") %>';
        function openAddFineUI(url) {
            parent.addExampleTab.apply(null, ['add_fineui_customer_price', basePath + url, '引入客户对应产品价格表', basePath + 'icon/database_add.png', true]);
        }
        function closeActiveTab() {
            parent.removeActiveTab();
        }
    </script>
</body>
</html>