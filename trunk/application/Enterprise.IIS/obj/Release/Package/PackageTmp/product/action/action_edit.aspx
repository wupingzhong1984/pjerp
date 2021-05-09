<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="action_edit.aspx.cs" Inherits="Enterprise.IIS.product.action.action_edit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" runat="server"  AutoSizePanelID="SimpleForm1" />
    <f:SimpleForm ID="SimpleForm1" runat="server" BodyPadding="5px" ShowHeader="False"
        Title="功能管理">
        <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server"  Position="Bottom" ToolbarAlign="Right">
                <Items>
                    <f:Button ID="btnClose" EnablePostBack="false" Text="关闭" runat="server" Icon="SystemClose" Size="Medium">
                    </f:Button>
                    <f:Button ID="btnSubmit" Text="提交表单" runat="server" Hidden="True" Icon="SystemSaveNew" ValidateForms="SimpleForm1" Size="Medium"
                        OnClick="btnSubmit_Click">
                    </f:Button>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <Items>
            <f:TextBox ID="txtaction_name" runat="server" Label="功能名称" MinLength="2" Required="True"
                ShowRedStar="True" EmptyText="功能名称" LabelAlign="Right">
            </f:TextBox>
            <f:TextBox ID="txtaction_en" runat="server" Label="英文名称" MinLength="2" Required="True"
                ShowRedStar="True" EmptyText="英文名称" LabelAlign="Right">
            </f:TextBox>
            <f:TextBox ID="txtaction_desc" runat="server" Label="描述" Required="false" ShowRedStar="false"
                EmptyText="描述" LabelAlign="Right">
            </f:TextBox>
        </Items>
    </f:SimpleForm>
    </form>
</body>
</html>
