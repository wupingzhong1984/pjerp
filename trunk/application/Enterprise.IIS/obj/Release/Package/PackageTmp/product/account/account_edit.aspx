<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="account_edit.aspx.cs" Inherits="Enterprise.IIS.product.account.account_edit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
    <f:Panel ID="Panel1" runat="server" Layout="Fit" ShowBorder="False" ShowHeader="false"
        BodyPadding="5px">
        <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server" Position="Bottom" ToolbarAlign="Right">
                <Items>
                    <f:Button ID="btnClose" EnablePostBack="false" Text="关闭" runat="server" Icon="SystemClose"
                        Size="Medium">
                    </f:Button>
                    <f:Button ID="btnSubmit" Text="提交表单" runat="server" Hidden="True" Icon="SystemSaveNew"
                        ValidateForms="SimpleForm1" OnClick="btnSubmit_Click" Size="Medium">
                    </f:Button>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <Items>
            <f:Panel ID="Panel2" Layout="Fit" runat="server" ShowBorder="false" ShowHeader="false">
                <Items>
                    <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="80px"
                        runat="server">
                        <Rows>
                            <f:FormRow ID="FormRow1">
                                <Items>
                                    <f:Label ID="txtaccount_org_name" runat="server" Label="所属部门" LabelAlign="Right">
                                    </f:Label>
                                    <f:TextBox ID="txtaccount_name" runat="server" Label="姓名" MinLength="2" Required="True"
                                        ShowRedStar="True" EmptyText="姓名" LabelAlign="Right">
                                    </f:TextBox>
                                    <f:TextBox ID="txtaccount_number" runat="server" Label="登录帐号" MinLength="3" Required="True" Enabled="False"
                                        ShowRedStar="True" EmptyText="登录帐号" AutoPostBack="true" OnTextChanged="txtaccount_number_TextChanged" LabelAlign="Right">
                                    </f:TextBox>
                                </Items>
                            </f:FormRow>
                            <f:FormRow ID="FormRow2">
                                <Items>
                                    <f:DropDownList ID="ddlaccount_sex" runat="server" Label="性别" Required="true" ShowRedStar="True" LabelAlign="Right">
                                        <f:ListItem Text="男" Value="男"></f:ListItem>
                                        <f:ListItem Text="女" Value="女"></f:ListItem>
                                    </f:DropDownList>
                                    <f:DropDownList ID="ddlaccount_role_name" runat="server" Label="角色名称" Required="true"
                                        ShowRedStar="True" LabelAlign="Right">
                                    </f:DropDownList>
                                    <f:DropDownList ID="ddlaccount_flag" runat="server" Label="登录有效" Required="true"
                                        ShowRedStar="True" LabelAlign="Right">
                                        <f:ListItem Text="禁用" Value="0"></f:ListItem>
                                        <f:ListItem Text="启用" Value="1"></f:ListItem>
                                    </f:DropDownList>
                                </Items>
                            </f:FormRow>
                            <f:FormRow ID="FormRow3">
                                <Items>
                                    <f:TextBox ID="txtaccount_mobile_phone" runat="server" Label="手机号码" MinLength="11"
                                        EmptyText="手机号码" LabelAlign="Right">
                                    </f:TextBox>
                                    <f:TextBox ID="txtaccount_qq" runat="server" Label="QQ" MinLength="11" EmptyText="QQ" LabelAlign="Right">
                                    </f:TextBox>
                                    <f:TextBox ID="txtaccount_email" runat="server" Label="电子邮箱" MinLength="1" Hidden="true" Text="info@ffqs.com" EmptyText="电子邮箱" LabelAlign="Right">
                                    </f:TextBox>
                                </Items>
                            </f:FormRow>
                            <f:FormRow ID="FormRow4"  Hidden="true">
                                <Items>
                                    <f:TextBox ID="txtaccount_password" runat="server" Label="密码" TextMode="Password"
                                        MinLength="6" EmptyText="密码" LabelAlign="Right">
                                    </f:TextBox>
                                    <f:DropDownList ID="ddlaccount_post" runat="server" Label="职员类别" DataTextField="key"
                                        DataValueField="value" LabelAlign="Right"  Hidden="True">
                                    </f:DropDownList>
                                    <f:DropDownList ID="ddlaccount_major" runat="server" Label="职务" DataTextField="key"
                                        DataValueField="value" LabelAlign="Right"   Hidden="True">
                                    </f:DropDownList>
                                </Items>
                            </f:FormRow>
                        </Rows>
                    </f:Form>
                </Items>
            </f:Panel>
        </Items>
    </f:Panel>
    </form>
   <script language="javascript" type="text/javascript">
       Ext.onReady(function () {
           //var map = new Ext.util.KeyMap({
           //    target: 'adRuleView',
           //    binding: [
           //        {
           //            key: Ext.EventObject.ESC,
           //            fn: function() { brUnitRuleGrid.addRuleWindow.close(); }
           //        }
           //    ]
           //});
           Ext.get("text").on("keypress", function (e) {
               if (e.getKey() == Ext.EventObject.ESC) {
               }
           });
       });
   </script>

</body>
</html>
