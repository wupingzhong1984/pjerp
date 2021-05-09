<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="database1.aspx.cs" Inherits="Enterprise.IIS.product.webs.database1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" runat="server" />
    <f:SimpleForm ID="SimpleForm1" BodyPadding="5px" LabelWidth="180px" runat="server"
        ShowBorder="True" ShowHeader="True" Title="备份数据库">
        <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <f:Button ID="btnSubmit" ValidateForms="SimpleForm1" CssClass="inline" runat="server"
                        Text="提交表单" IconUrl="~/icon/database_add.png" OnClick="btnSubmit_Click">
                    </f:Button>
                    <f:Button ID="btnReset" Text="重置表单" EnablePostBack="false" runat="server" IconUrl="~/icon/reload.png">
                    </f:Button>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <Items>
            <f:TextBox ID="txtName" Required="true" Label="目标文件名" runat="server">
            </f:TextBox>
        </Items>
    </f:SimpleForm>
    </form>
</body>
</html>
