<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="account_list.aspx.cs" Inherits="Enterprise.IIS.product.account.account_list" %>

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
            <f:Region ID="Region1" ShowHeader="false" Split="true" ShowBorder="False" 
                Width="200px" Position="Left" Layout="Fit" runat="server">
                <Items>
                    <f:Tree ID="trDept" Width="50px" OnNodeCommand="trDept_NodeCommand" ShowHeader="False"
                        ShowBorder="false" Icon="House" Title="组织机构" runat="server"
                        EnableArrows="true" AutoScroll="True">
                    </f:Tree>
                </Items>
            </f:Region>
            <f:Region ID="Region2" Title="帐号管理" Position="Center"
                ShowBorder="False" BoxConfigAlign="Stretch" Layout="Fit" runat="server" ShowHeader="False">
                <Toolbars>
                    <f:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <f:Button ID="btnEdit" OnClick="btnEdit_Click" runat="server" Hidden="True" Text="编辑"
                                Icon="PageWhiteEdit" Size="Medium">
                            </f:Button>
                            <f:Button ID="btnBatchDelete" OnClick="btnBatchDelete_Click" runat="server" Text="删除"
                                Icon="Delete" Hidden="True" Size="Medium">
                            </f:Button>
                            <f:Button ID="btnInitPassword" OnClick="btnInitPassword_Click" runat="server" Hidden="True"
                                Text="密码初始化" Icon="UserKey" Size="Medium">
                            </f:Button>
                            <%--<f:Button ID="btnImpower" OnClick="btnImpower_Click" runat="server" Hidden="True"
                                Text="授权" Icon="Ruby" Size="Medium">
                            </f:Button>--%>
                            <f:Button ID="btnEnabled" OnClick="btnEnabled_Click" runat="server" Hidden="True"
                                Text="禁/启" Icon="Stop" Size="Medium">
                            </f:Button>
                            <f:Button ID="btnExport" OnClick="btnExport_Click" runat="server" Text="引出" Icon="PageExcel"
                                Hidden="True" EnableAjax="false" DisableControlBeforePostBack="false" Size="Medium">
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
                                        runat="server" BodyPadding="5px" LabelAlign="Right">
                                        <Rows>
                                            <f:FormRow ID="FormRow1">
                                                <Items>
                                                    <f:TextBox ID="txtaccount_name" runat="server" Label="姓名">
                                                    </f:TextBox>
                                                    <f:TextBox ID="txtaccount_number" runat="server" Label="登陆帐号">
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
                                    <f:Grid ID="Grid1" PageSize="20" ShowBorder="false" ShowHeader="false" AllowPaging="true"
                                        runat="server" EnableCheckBoxSelect="True" DataKeyNames="id,role_name" OnPageIndexChange="Grid1_PageIndexChange"
                                        IsDatabasePaging="true" OnSort="Grid1_Sort" SortDirection="ASC"
                                        AllowSorting="true" EmptyText="查询无结果" EnableHeaderMenu="True">
                                        <Columns>
                                            <%--<f:TemplateField HeaderText="完成情况" Width="110px">
                                                <ItemTemplate>
                                                    <div style="width: 100px; border-width: 1px; border-style: solid; border-color: #999999;
                                                        height: 10px">
                                                        <div style="width: <%# Eval("id") %>px; height: 10px; background: #FF00CC">
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                            </f:TemplateField>--%>
                                            <f:BoundField MinWidth="80px" DataField="account_name" HeaderText="姓名" SortField="account_name" />
                                            <f:BoundField MinWidth="80px" DataField="code_cn" HeaderText="助记码" SortField="code_cn" />
                                            <f:BoundField MinWidth="120px" DataField="account_number" HeaderText="登录帐号" SortField="account_number" />
                                            <f:BoundField MinWidth="120px" DataField="base_orgnization.org_name" HeaderText="所属机构" SortField="account_org_id" />
                                            <f:BoundField MinWidth="120px" DataField="account_role_name" HeaderText="角色" SortField="account_role_name" />
                                            <f:ImageField MinWidth="40px" DataImageUrlField="account_sex" DataImageUrlFormatString="~/icon/user_{0}.png"
                                                TextAlign="Center" HeaderText="性别" SortField="account_sex"></f:ImageField>
                                            <f:CheckBoxField MinWidth="100px" RenderAsStaticField="true" DataField="account_flag"
                                                TextAlign="Center" HeaderText="是否启用" SortField="account_flag" />
                                            <f:BoundField MinWidth="100px" DataField="account_mobile_phone" HeaderText="移动电话" SortField="account_mobile_phone" />
                                            <f:BoundField MinWidth="100px" DataField="account_qq" HeaderText="QQ" SortField="account_qq" />
                                            <f:BoundField MinWidth="120px" DataField="account_email" HeaderText="Email" SortField="account_email" />
                                            <f:BoundField MinWidth="140px" DataField="createdon" HeaderText="创建时间" SortField="createdon" />
                                            <f:BoundField MinWidth="140px" DataField="account_last_date" HeaderText="最后一次登录时间" SortField="account_last_date" />
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
        Target="Parent" EnableIFrame="true" IFrameUrl="about:blank" Height="450px" Width="850px"
        OnClose="Window1_Close">
    </f:Window>
    </form>
</body>
</html>
