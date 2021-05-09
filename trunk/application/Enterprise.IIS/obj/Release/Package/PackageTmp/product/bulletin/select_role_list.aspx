<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="select_role_list.aspx.cs" Inherits="Enterprise.IIS.product.bulletin.select_role_list" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>角色管理</title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Panel1" />
    <f:Panel ID="Panel1" Title="角色管理" runat="server" ShowHeader="False" ShowBorder="false" Layout="Fit">
        <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <f:Button ID="btnEdit" OnClick="btnEdit_Click" Size="Medium" runat="server" Text="确认提交" Icon="PageSave">
                    </f:Button>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <Items>
            <f:RegionPanel ID="RegionPanel2" ShowBorder="false" runat="server">
                <Regions>
                    <f:Region ID="Region1" ShowBorder="false" ShowHeader="false" Split="true"
                         Position="Top" Layout="Fit"
                        BodyPadding="3px" runat="server" Height="40px" EnableCollapse="True" >
                        <Items>
                            <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False"
                                 LabelAlign="Right" runat="server" BodyPadding="5px">
                                <Rows>
                                    <f:FormRow ID="FormRow1">
                                        <Items>
                                            <f:TextBox ID="txtRole_name" runat="server" LabelAlign="Right" Label="角色名称">
                                            </f:TextBox>
                                            <f:DropDownList runat="server" ID="ddlFlag" LabelAlign="Right" Label="状态">
                                                <f:ListItem Text="全部" Value="" />
                                                <f:ListItem Text="启用" Value="1" />
                                                <f:ListItem Text="禁用" Value="0" />
                                            </f:DropDownList>
                                            <f:Button ID="btnSearch" runat="server" Text="查询" Icon="Zoom" OnClick="btnSearch_Click">
                                            </f:Button>
                                        </Items>
                                    </f:FormRow>
                                </Rows>
                            </f:Form>
                        </Items>
                    </f:Region>
                    <f:Region ID="Region2" ShowBorder="False" ShowHeader="false" Position="Center"
                        Layout="Fit" runat="server" BodyPadding="3px">
                        <Items>
                            <f:Grid ID="Grid1" PageSize="20" AllowPaging="true" runat="server"
                                EnableCheckBoxSelect="True" ShowBorder="false" ShowHeader="false"
                                DataKeyNames="id,role_name" OnPageIndexChange="Grid1_PageIndexChange" IsDatabasePaging="true"
                                OnSort="Grid1_Sort" SortDirection="ASC" AllowSorting="true"  EmptyText="查询无结果" EnableHeaderMenu="True">
                                <Columns>
                                    <f:BoundField Width="180px" DataField="role_name" HeaderText="角色名称" SortField="role_name" />
                                    <f:CheckBoxField Width="100px" RenderAsStaticField="true" DataField="role_flag" HeaderText="状态"
                                        SortField="role_flag" />
                                    <f:BoundField DataField="role_desc" Width="200px" HeaderText="描述" SortField="role_desc" />
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
    </f:Panel>
    </form>
</body>
</html>
