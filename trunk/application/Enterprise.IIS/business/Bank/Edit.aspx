<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Enterprise.IIS.business.Bank.Edit" %>


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
            <f:TextBox ID="txtFName" runat="server" Label="开户银行" MinLength="2" Required="True"
                ShowRedStar="True" EmptyText="银行账户" LabelAlign="Right">
            </f:TextBox>
            <f:TextBox ID="txtUserCode" runat="server" Label="自定义编码" EmptyText="自定义编码" LabelAlign="Right" Required="True" ShowRedStar="True" >
            </f:TextBox>
            <f:TextArea ID="txtFComment" runat="server" Label="账号" EmptyText="账号" LabelAlign="Right">
            </f:TextArea>
            <f:DropDownList ID="ddlFAbstract" Label="业务性质" LabelAlign="Right" Required="true" runat="server" >
                   <f:ListItem Text="现金" Value="现金" Selected="True"/>
                   <f:ListItem Text="银行" Value="银行" />
             </f:DropDownList>

        </Items>
    </f:SimpleForm>
    </form>
</body>
</html>
