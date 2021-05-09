<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="role_permission.aspx.cs"
    Inherits="Enterprise.IIS.product.role.role_permission" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>角色授权设置</title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
    <script src="../../jqueryui/js/jquery-1.8.3.min.js" type="text/javascript"></script>  
    <script language="javascript" type="text/javascript">
        function checkSelAll() {
            $('#RegionPanel1_Region1_RegionPanel2_Region4_trDept input:checkbox').attr("checked", true);
        }
        function checkQxAll() {
            $("#RegionPanel1_Region1_RegionPanel2_Region4_trDept input:checkbox").attr("checked", false);
        }
    </script>
    <style type="text/css">
        .x-grid-tpl .others input
        {
            vertical-align: middle;
        }
        .x-grid-tpl .others label
        {
            margin-left: 5px;
            margin-right: 15px;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
    <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
        <Regions>
            <f:Region ID="Region1" Title="数据授权配置" ShowBorder="false" ShowHeader="False" Position="Center"
                Layout="Fit" BoxConfigAlign="Stretch" BoxConfigPosition="Left" runat="server">
                <Toolbars>
                    <f:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <f:Button ID="btnSelAll" IconUrl="~/icon/comments_add.png" Size="Medium" OnClientClick="checkSelAll();"
                                Text="全选" runat="server" />
                            <f:Button ID="btnQxAll" IconUrl="~/icon/comments_delete.png" Size="Medium" OnClientClick="checkQxAll();"
                                Text="取消全选" runat="server" />
                            <f:Button ID="btnSubmit" IconUrl="~/icon/database_add.png" Size="Medium" OnClick="btnSava_Click"
                                Text="提交表单" runat="server">
                            </f:Button>
                            <f:Button ID="btnBackup" runat="server" Text="返回" Size="Medium" IconUrl="~/icon/arrow_redo.png"
                                OnClick="btnReturn_Click" />
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
                                    <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="120px"
                                        runat="server" BodyPadding="5px">
                                        <Rows>
                                            <f:FormRow ID="FormRow1">
                                                <Items>
                                                    <f:DropDownList ID="ddlDatatAccess" runat="server" Label="选择数据授权范围" Required="true"
                                                        ShowRedStar="True" AutoPostBack="True" OnSelectedIndexChanged="ddlDatatAccess_SelectedIndexChanged">
                                                        <f:ListItem Text="组织机构" Value="组织机构"></f:ListItem>
                                                        <f:ListItem Text="供理商" Value="供理商"></f:ListItem>
                                                        <f:ListItem Text="客户" Value="客户"></f:ListItem>
                                                        <f:ListItem Text="仓库" Value="仓库"></f:ListItem>
                                                    </f:DropDownList>
                                                    <f:CheckBox runat="server" Label="启用">
                                                    </f:CheckBox>
                                                    <f:TextBox ID="txtcode_cn" runat="server" Label="引入其它用户授权">
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
                                    <f:Tree ID="trDept" Width="50px" ShowHeader="False" ShowBorder="false" Icon="House"
                                        EnableMultiSelect="True" Title="组织机构" runat="server" EnableArrows="true" AutoLeafIdentification="false"
                                        Enabled="True" OnNodeCheck="Tree1_NodeCheck" AutoScroll="True">
                                    </f:Tree>
                                </Items>
                            </f:Region>
                        </Regions>
                    </f:RegionPanel>
                </Items>
            </f:Region>
        </Regions>
    </f:RegionPanel>
    <br />
    </form>
</body>
</html>
