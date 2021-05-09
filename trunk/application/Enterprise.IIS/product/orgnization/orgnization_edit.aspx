<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="orgnization_edit.aspx.cs"
    Inherits="Enterprise.IIS.product.orgnization.orgnization_edit" %>

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
            <f:Toolbar ID="Toolbar1" runat="server"  Position="Bottom" ToolbarAlign="Right">
                <Items>
                    <f:Button ID="btnClose" EnablePostBack="false" Text="关闭" Size="Medium" runat="server" Icon="SystemClose">
                    </f:Button>
                    <f:Button ID="btnSubmit" Text="提交表单" runat="server" Size="Medium" Hidden="True" Icon="SystemSaveNew"
                        ValidateForms="SimpleForm1" OnClick="btnSubmit_Click">
                    </f:Button>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <Items>
            <f:Panel ID="Panel2" Layout="Fit" runat="server" ShowBorder="false" ShowHeader="false">
                <Items>
                    <f:SimpleForm ID="SimpleForm1" ShowBorder="false" ShowHeader="false"
                        AutoScroll="true" BodyPadding="5px" runat="server" EnableCollapse="True">
                        <Items>
                            <f:Label ID="lblorg_name" runat="server" Label="所属部门">
                            </f:Label>
                            <f:TextBox ID="txtcode" runat="server" Label="代码" EmptyText="代码" Required="True"
                                ShowRedStar="True">
                            </f:TextBox>
                            <f:DropDownList ID="ddlorg_type" runat="server" Label="类型" Required="true" ShowRedStar="True">
                                <f:ListItem Text="部门" Value="1"></f:ListItem>
                                <f:ListItem Text="组" Value="2"></f:ListItem>
                            </f:DropDownList>
                            <f:TextBox ID="txtorg_name" runat="server" Label="名称" MinLength="2" Required="True"
                                ShowRedStar="True" EmptyText="名称">
                            </f:TextBox>
                            <f:TextBox ID="txtorg_account_name" runat="server" Label="部门主管" EmptyText="部门主管">
                            </f:TextBox>
                            <f:TextBox ID="txtorg_office_tel" runat="server" Label="电话" EmptyText="电话">
                            </f:TextBox>
                            <f:TextBox ID="txtorg_office_fax" runat="server" Label="传真" EmptyText="传真">
                            </f:TextBox>
                            <f:TextArea ID="txtorg_desc" runat="server" Label="描述" EmptyText="描述">
                            </f:TextArea>
                        </Items>
                    </f:SimpleForm>
                </Items>
            </f:Panel>
        </Items>
    </f:Panel>
    </form>
</body>
</html>
