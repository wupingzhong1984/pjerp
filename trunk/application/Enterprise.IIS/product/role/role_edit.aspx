<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="role_edit.aspx.cs" Inherits="Enterprise.IIS.product.role.role_edit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>角色授权设置</title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
    <script src="../../jqueryui/js/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function checkSelAll() {
            $('#RegionPanel1_Region1_SimpleForm1_Grid1 input:checkbox').attr("checked", true);
        }
        function checkQxAll() {
            $("#RegionPanel1_Region1_SimpleForm1_Grid1 input:checkbox").attr("checked", false);
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
            <f:Region ID="Region1" Title="功能授权配置" ShowBorder="false" ShowHeader="False" Position="Center"
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
                    <f:SimpleForm ID="SimpleForm1" BodyPadding="5px" LabelWidth="135px" EnableAjax="true"
                        runat="server" ShowBorder="false" ShowHeader="False"
                        AutoScroll="true" >
                        <Items>
                            <f:GroupPanel runat="server" Title="基本信息" ID="GPanelRoleInfo" EnableCollapse="True">
                                <Items>
                                    <f:TextBox runat="server" Label="角色名称" ID="txtrole_name" EmptyText="角色名称" MinLength="1"
                                        Required="True" ShowRedStar="True">
                                    </f:TextBox>
                                    <f:TextBox ID="txtrole_desc" runat="server" Label="角色描述" EmptyText="角色描述">
                                    </f:TextBox>
                                </Items>
                            </f:GroupPanel>
                            <f:GroupPanel runat="server" Title="功能授权" ID="GPanelActions" EnableCollapse="True">
                                <Items>
                                    <f:Panel ID="PanelActions" ShowHeader="false" Title="ICON"
                                         ShowBorder="false" runat="server">
                                        <Items>
                                            <f:CheckBoxList ID="cboxListActions" runat="server" TableConfigColumns="8" ColumnNumber="8"
                                                DataTextField="action_name" DataValueField="id">
                                            </f:CheckBoxList>
                                        </Items>
                                    </f:Panel>
                                </Items>
                            </f:GroupPanel>
                            <f:Grid ID="Grid1" runat="server" BoxFlex="1" ShowBorder="true" ShowHeader="false"
                                EnableCheckBoxSelect="False" Title="角色授权项目" DataKeyNames="id" OnRowDataBound="Grid1_RowDataBound" EnableHeaderMenu="True">
                                <Columns>
                                    <f:BoundField Width="150px" DataField="menu_name" DataFormatString="{0}" HeaderText="模块名称"
                                        DataSimulateTreeLevelField="menu_level" />
                                    <f:TemplateField ColumnID="Others" HeaderText="权限" Width="860px">
                                        <ItemTemplate>
                                            <asp:CheckBoxList ID="cbxlActions" CssClass="others" RepeatLayout="Table" RepeatColumns="10"
                                                runat="server">
                                            </asp:CheckBoxList>
                                        </ItemTemplate>
                                    </f:TemplateField>
                                </Columns>
                            </f:Grid>
                        </Items>
                    </f:SimpleForm>
                </Items>
            </f:Region>
        </Regions>
    </f:RegionPanel>
    <br />
    </form>
    <script type="text/javascript">
        Ext.onReady(function () {
            var actionList = F('<%= cboxListActions.ClientID %>').items;
            for (var i = 0; i < actionList.length; i++) {
                var item = actionList.items[i]; ;
                item.on('check', function (s) {
                    var actions = $('#RegionPanel1_Region1_SimpleForm1_Grid1 input:checkbox');
                    for (var j = 0; j < actions.length; j++) {
                        if (actions[j].nextSibling.textContent == s.boxLabel) {
                            actions[j].checked = s.getValue();
                        }
                    }
                });
            }
        });
    </script>
</body>
</html>
