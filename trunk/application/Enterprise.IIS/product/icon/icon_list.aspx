<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="icon_list.aspx.cs" Inherits="Enterprise.IIS.product.icon.icon_list" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>图标管理</title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Panel1" />
    <f:Panel ID="Panel1" Title="图标管理" runat="server" ShowBorder="false" Layout="Fit" ShowHeader="False">
        <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <f:Button ID="btnAdd" runat="server" Text="新增" Icon="ADD" Hidden="True" Size="Medium">
                    </f:Button>
                    <f:Button ID="btnEdit" OnClick="btnEdit_Click" runat="server" Hidden="True" Text="编辑" Size="Medium" Icon="PageWhiteEdit">
                    </f:Button>
                    <f:Button ID="btnBatchDelete" OnClick="btnDelete_Click" runat="server" Text="删除" Size="Medium"
                        Icon="Delete" Hidden="True">
                    </f:Button>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <Items>
            <f:Grid ID="Grid1" Title="图标管理" PageSize="20" ShowBorder="False" ShowHeader="False"
                AllowPaging="true" runat="server" EnableCheckBoxSelect="True"
                DataKeyNames="id" OnPageIndexChange="Grid1_PageIndexChange" IsDatabasePaging="true"
                OnRowCommand="Grid1_RowCommand" OnSort="Grid1_Sort" SortDirection="ASC"
                AllowSorting="true"  EmptyText="查询无结果" EnableHeaderMenu="True">
                <Columns>
                    <f:ImageField Width="60px" DataImageUrlField="icon_src" HeaderText="图标" TextAlign="Center">
                    </f:ImageField>
                    <f:BoundField Width="300px" DataField="icon_src" HeaderText="图标路径" SortField="icon_src" />
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
    </f:Panel>
    <br />
    <f:Window ID="Window1" runat="server" WindowPosition="Center" IsModal="true"  Hidden="True"
        Icon="ApplicationViewDetail" EnableMaximize="true" EnableResize="true" Target="Parent" EnableIFrame="true"
        IFrameUrl="about:blank" Height="300px" Width="650px" OnClose="Window1_Close">
    </f:Window>
    </form>
</body>
</html>
