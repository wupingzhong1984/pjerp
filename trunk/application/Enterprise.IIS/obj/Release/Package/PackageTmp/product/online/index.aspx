<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="Enterprise.IIS.product.online.index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>在线用户</title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
    <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
        <Regions>
            <f:Region ID="Region1" ShowBorder="false" ShowHeader="False" Title="在线用户" Position="Center"
                Layout="Fit" BoxConfigAlign="Stretch" BoxConfigPosition="Left" runat="server">
                <Items>
                    <f:RegionPanel ID="RegionPanel2" ShowBorder="false" runat="server">
                        <Regions>
                            <f:Region ID="Region4" ShowBorder="False" ShowHeader="false" Position="Center"
                                Layout="Fit" runat="server" BodyPadding="3px">
                                <Items>
                                    <f:Grid ID="Grid1" Title="登录管理" PageSize="20" ShowBorder="False" ShowHeader="False"
                                        Icon="User" AllowPaging="true" runat="server" EnableCheckBoxSelect="False"
                                        DataKeyNames="id" OnPageIndexChange="Grid1_PageIndexChange" IsDatabasePaging="true"
                                        OnRowCommand="Grid1_RowCommand" OnSort="Grid1_Sort" SortDirection="DESC"
                                        AllowSorting="true"  EmptyText="查询无结果" EnableHeaderMenu="True">
                                        <Columns>
                                            <f:BoundField Width="350px" DataField="account_name" HeaderText="登录账号" SortField="account_name" />
                                            <f:BoundField Width="200px" DataField="account_ip_addres" HeaderText="登录地址" SortField="account_ip_addres" />
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
    </form>
</body>
</html>
