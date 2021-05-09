<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="select_account_list.aspx.cs" Inherits="Enterprise.IIS.product.bulletin.select_account_list" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>帐号管理</title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
    <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
        <Regions>
            <f:Region ID="Region1" ShowHeader="false" Split="true" 
                Width="180px" Position="Left" Layout="Fit" runat="server">
                <Items>
                    <f:Tree ID="trDept" Width="120px" OnNodeCommand="trDept_NodeCommand" ShowHeader="False"
                        ShowBorder="false" Icon="House" Title="组织机构" runat="server"
                        EnableArrows="true" AutoLeafIdentification="false" AutoScroll="True">
                    </f:Tree>
                </Items>
            </f:Region>
            <f:Region ID="Region2" Title="用户管理" Position="Center" ShowHeader="False"
                ShowBorder="True" BoxConfigAlign="Stretch" Layout="Fit" runat="server">
                <Toolbars>
                    <f:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <f:Button ID="btnEdit" OnClick="btnEdit_Click" runat="server" Text="确认提交"  Size="Medium"
                                Icon="PageSave">
                            </f:Button>
                            <f:Button ID="btnSearch" runat="server" Text="查询" Icon="Zoom" Size="Medium" OnClick="btnSearch_Click"></f:Button>
                        </Items>
                    </f:Toolbar>
                </Toolbars>
                <Items>
                    <f:RegionPanel ID="RegionPanel2" ShowBorder="false" runat="server">
                        <Regions>
                            <f:Region ID="Region3" ShowBorder="false" ShowHeader="false" Split="true"
                                 Position="Top" Layout="Fit"
                                BodyPadding="3px" runat="server" Height="40px" EnableCollapse="True">
                                <Items>
                                    <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="80px"
                                       LabelAlign="Right"  runat="server" BodyPadding="5px">
                                        <Rows>
                                            <f:FormRow ID="FormRow1">
                                                <Items>
                                                    <f:TextBox ID="txtaccount_name" runat="server" Label="姓名" LabelAlign="Right">
                                                    </f:TextBox>
                                                    <f:TextBox ID="txtaccount_number" runat="server" Label="登陆帐号" LabelAlign="Right">
                                                    </f:TextBox>
                                                </Items>
                                            </f:FormRow>
                                        </Rows>
                                    </f:Form>
                                </Items>
                            </f:Region>
                            <f:Region ID="Region4" ShowBorder="False" ShowHeader="false" Position="Center"
                                Layout="Fit" runat="server" BodyPadding="3px">
                                <Items>
                                    <f:Grid ID="Grid1" PageSize="20" ShowBorder="false" ShowHeader="false" AllowPaging="true"
                                        runat="server" EnableCheckBoxSelect="True" DataKeyNames="id,role_name" OnPageIndexChange="Grid1_PageIndexChange"
                                        IsDatabasePaging="true" OnSort="Grid1_Sort" SortDirection="ASC"
                                        AllowSorting="true" EmptyText="查询无结果" EnableHeaderMenu="True">
                                        <Columns>
                                            <%--<f:BoundField Width="100px" DataField="account_org_name" HeaderText="所属部门" SortField="account_org_name" />--%>
                                            <f:BoundField Width="80px" DataField="account_name" HeaderText="姓名" SortField="account_name" />
                                            <f:BoundField Width="80px" DataField="account_number" HeaderText="登录帐号" SortField="account_number" />
                                            <f:BoundField Width="80px" DataField="account_role_name" HeaderText="角色" SortField="account_role_name" />
                                            <f:CheckBoxField Width="100px" RenderAsStaticField="true" DataField="account_flag"
                                                TextAlign="Center" HeaderText="是否禁用" SortField="account_flag" />
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
<%--    <f:Window ID="Window1" runat="server" WindowPosition="Center" IsModal="true"
        EnableMaximize="true" EnableResize="true" Target="Parent" EnableIFrame="true"
        IFrameUrl="about:blank" Height="450px" Width="650px" OnClose="Window1_Close">
    </f:Window>--%>
    </form>
</body>
</html>
