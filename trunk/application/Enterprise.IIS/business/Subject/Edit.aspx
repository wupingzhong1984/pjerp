<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Enterprise.IIS.business.Subject.Edit" %>


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
                    <f:Button ID="btnSubmit" Text="提交表单" runat="server" Hidden="False" Icon="SystemSaveNew" ValidateForms="SimpleForm1" Size="Medium"
                        OnClick="btnSubmit_Click">
                    </f:Button>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <Items>
            
            <f:TextBox ID="txtUserCode" runat="server" Label="自定义编码" EmptyText="自定义编码" LabelAlign="Right">
            </f:TextBox>

            <f:TextBox ID="txtFName" runat="server" Label="科目名称" MinLength="2" Required="True"  LabelAlign="Right"
                ShowRedStar="True" EmptyText="科目名称">
            </f:TextBox>
            
            <f:DropDownList runat="server" ID="ddlLeavl" Label="操作级别" EnableEdit="True" Required="True" 
                ShowRedStar="True"  LabelAlign="Right">
                    <f:ListItem Text="添加到同级目录" Value="添加到同级目录" EnableSelect="True"/>
                    <f:ListItem Text="添加到当前子级目录" Value="添加到当前子级目录" />
            </f:DropDownList>

            <f:TextArea ID="txtFComment" runat="server" Label="描述" EmptyText="描述"  LabelAlign="Right">
            </f:TextArea>

        </Items>
    </f:SimpleForm>
    </form>
</body>
</html>
