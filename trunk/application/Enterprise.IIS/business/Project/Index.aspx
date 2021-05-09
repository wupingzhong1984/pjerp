<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Enterprise.IIS.business.Project.Index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>数据字典</title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .alink a, a:hover, a:visited {
            text-decoration: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
        <f:RegionPanel ID="RegionPanel1" ShowBorder="False" runat="server">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <f:Button ID="btnMenu" Text="字典" Enabled="False" EnablePostBack="false" runat="server" Size="Medium" Icon="Database">
                            <Menu runat="server">
                                <f:MenuButton ID="btnAddClass" runat="server" Text="新增字典" Icon="BulletBlue" Hidden="False">
                                </f:MenuButton>
                                <f:MenuButton ID="btnEditClass" runat="server" Text="编辑字典" Icon="BulletBlue" Hidden="False">
                                </f:MenuButton>
                                <f:MenuSeparator runat="server" />
                                <f:MenuButton ID="btnAddTree" runat="server" Text="新增树字典" Icon="BulletBlue" Hidden="False">
                                </f:MenuButton>
                                <f:MenuButton ID="btnEditTree" runat="server" Text="编辑树字典" Icon="BulletBlue" Hidden="False">
                                </f:MenuButton>
                            </Menu>
                        </f:Button>
                        <f:Button ID="btnAdd" runat="server" Text="新增" Size="Medium" Icon="Add" Hidden="True">
                        </f:Button>
                        <f:Button ID="btnEdit" OnClick="btnEdit_Click" Size="Medium" runat="server" Hidden="True" Text="编辑"
                            Icon="PageWhiteEdit">
                        </f:Button>
                        <f:Button ID="btnBatchDelete" OnClick="btnDelete_Click" Size="Medium" runat="server" Text="删除"
                            Icon="Delete" Hidden="True">
                        </f:Button>
                        <f:Button ID="btnStop" OnClick="btnDelete_Click" Size="Medium" runat="server" Text="启/禁"
                            Icon="Stop" Hidden="False">
                        </f:Button>
                        <f:Button ID="btnExport" OnClick="btnExport_Click" Size="Medium" runat="server" Text="引出" Icon="PageExcel"
                            Hidden="True" EnableAjax="false" DisableControlBeforePostBack="false">
                        </f:Button>
                        <f:Button ID="btnClose" EnablePostBack="False" Text="关闭" runat="server" Icon="SystemClose" Size="Medium" onClientClick="closeActiveTab();">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Regions>
                <f:Region ID="Region1" ShowBorder="false" ShowHeader="false" Split="true"
                    Width="200px" Position="Left" Layout="Fit"
                    runat="server">
                    <Items>
                        <f:Tree ID="trDept" Width="50px" ShowHeader="False"
                            ShowBorder="False" Icon="House" Title="数据字典" EnableAjax="True" runat="server" OnNodeCommand="trDept_NodeCommand"
                            EnableArrows="true" AutoScroll="True">
                        </f:Tree>
                    </Items>
                </f:Region>
                <f:Region ID="Region2" ShowBorder="False" ShowHeader="false" Position="Center" Layout="Fit"
                    BoxConfigAlign="Stretch" BoxConfigPosition="Left" runat="server" AutoScroll="True">
                    <Items>
                        <f:Grid ID="Grid1" Title="数据字典" PageSize="20" ShowBorder="False" ShowHeader="False"
                            AllowPaging="true" runat="server" EnableCheckBoxSelect="True"
                            DataKeyNames="FId,FName" OnPageIndexChange="Grid1_PageIndexChange" IsDatabasePaging="true"
                            OnSort="Grid1_Sort" SortDirection="ASC" AllowSorting="true"
                            OnRowDataBound="Grid1_RowDataBound" EmptyText="查询无结果" EnableHeaderMenu="True">

                            <Columns>
                                <f:BoundField MinWidth="80px" DataField="FId" HeaderText="代码" SortField="FId" />

                                <f:BoundField MinWidth="200px" DataField="FName" HeaderText="名称" SortField="FName" />
                                <f:BoundField MinWidth="100px" DataField="FKey" HeaderText="FKey" SortField="FKey" />
                                <f:BoundField MinWidth="100px" DataField="FValue" HeaderText="FValue" SortField="FValue" />
                                <f:BoundField MinWidth="100px" DataField="FFlag" HeaderText="状态" SortField="FFlag" />
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
        <br />
        <f:Window ID="Window1" runat="server" WindowPosition="Center" IsModal="true" EnableClose="True"
            Icon="ApplicationViewDetail" EnableMaximize="true" EnableResize="true" Hidden="True"
            Target="Parent" EnableIFrame="true" IFrameUrl="about:blank" Height="430px" Width="550px"
            OnClose="Window1_Close">
        </f:Window>
    </form>
    <script type="text/javascript">
        var basePath = '<%= ResolveUrl("~/") %>';
        function closeActiveTab() {
            parent.removeActiveTab();
        };
    </script>
</body>
</html>
