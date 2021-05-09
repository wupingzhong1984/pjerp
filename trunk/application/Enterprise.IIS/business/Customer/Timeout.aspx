<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Timeout.aspx.cs" Inherits="Enterprise.IIS.business.Customer.Timeout" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>客户管理</title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region1" ShowHeader="false" Split="true" Hidden="True"
                    Width="200px" Position="Left" Layout="Fit" runat="server">
                    <Items>
                        <f:Tree ID="trDept" Width="50px" OnNodeCommand="trDept_NodeCommand" ShowHeader="False"
                            ShowBorder="false" Icon="House" Title="客户分类" runat="server"
                            EnableArrows="true" AutoScroll="True">
                        </f:Tree>
                    </Items>
                </f:Region>
                <f:Region ID="Region2" Title="客户管理" Position="Center"
                    ShowBorder="True" BoxConfigAlign="Stretch" Layout="Fit" runat="server" ShowHeader="False">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <f:Button ID="btnSearch" runat="server" Text="查询" Icon="Zoom" OnClick="btnSearch_Click" Size="Medium">
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
                                                        <f:TextBox ID="txtaccount_number" runat="server" Label="客户代码">
                                                        </f:TextBox>
                                                        <f:TextBox ID="txtaccount_name" runat="server" Label="客户名称">
                                                        </f:TextBox>
                                                    </Items>
                                                </f:FormRow>
                                            </Rows>
                                        </f:Form>
                                    </Items>
                                </f:Region>
                                <f:Region ID="Region4" ShowBorder="False" ShowHeader="false" Position="Center"
                                    Layout="Fit" runat="server" BodyPadding="3px" AutoScroll="True">
                                    <Items>
                                        <f:Grid ID="Grid1" PageSize="20" ShowBorder="false" ShowHeader="false" AllowPaging="true"
                                            runat="server" EnableCheckBoxSelect="False" DataKeyNames="FCode" OnPageIndexChange="Grid1_PageIndexChange"
                                            IsDatabasePaging="true" OnSort="Grid1_Sort" SortDirection="ASC"
                                            AllowSorting="true" EmptyText="查询无结果" EnableHeaderMenu="True">
                                            <Columns>
                                                <f:BoundField MinWidth="80px" DataField="FCode" HeaderText="客户代码" SortField="FCode" />
                                                <f:BoundField MinWidth="220px" DataField="FName" HeaderText="客户名称" SortField="FName" />
                                                <f:BoundField MinWidth="120px" DataField="FLinkman" HeaderText="联系人" SortField="FLinkman" />
                                                <f:BoundField MinWidth="100px" DataField="FPhome" HeaderText="电话" SortField="FPhome" />
                                                <f:BoundField MinWidth="100px" DataField="FMoile" HeaderText="手机" SortField="FMoile" />
                                                <f:BoundField MinWidth="100px" DataField="FTipsDay" HeaderText="设定天数" SortField="FTipsDay" />
                                                <f:BoundField MinWidth="100px" DataField="FLastTime" HeaderText="最后交易日期" SortField="FLastTime" DataFormatString="{0:yyyy-MM-dd}"/>
                                                <f:BoundField MinWidth="100px" DataField="Fdays" HeaderText="超时天数" SortField="Fdays" />
                                            </Columns>
                                            <PageItems>
                                                <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                                </f:ToolbarSeparator>
                                                <f:ToolbarText ID="ToolbarText1" runat="server" Text="每页记录数：">
                                                </f:ToolbarText>
                                                <f:DropDownList runat="server" ID="ddlPageSize" Width="80px" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
                                                    <f:ListItem Text="20" Value="20" />
                                                    <f:ListItem Text="30" Value="30" />
                                                    <f:ListItem Text="50" Value="50" />
                                                </f:DropDownList>
                                            </PageItems>
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
            parent.addExampleTab.apply(null, ['add_fineui_customer', basePath + url, '引入客户档案', basePath + 'icon/database_add.png', true]);
        }
        function closeActiveTab() {
            parent.removeActiveTab();
        }
    </script>
</body>
</html>
