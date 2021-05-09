<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Enterprise.IIS.product.company.Edit" %>

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
                            <f:Label ID="lblorg_name" runat="server" Label="所属机构" LabelAlign="Right">
                            </f:Label>
                            <f:TextBox ID="txtcom_code" runat="server" Label="企业代码" EmptyText="企业代码" Required="True"
                                ShowRedStar="True" LabelAlign="Right">
                            </f:TextBox>
                            <f:TextBox ID="txtcom_name" runat="server" Label="企业名称" MinLength="2" Required="True"
                                ShowRedStar="True" EmptyText="企业名称" LabelAlign="Right">
                            </f:TextBox>
                            <f:TextBox ID="txtcom_person" runat="server" Label="法人" EmptyText="法人" LabelAlign="Right">
                            </f:TextBox>
                            <f:DropDownList runat="server" ID="ddlMonth" Label="财务月结" LabelAlign="Right" AutoPostBack="true" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged">
                                <f:ListItem Text="自然月" Value="是" />
                                <f:ListItem Text="非自然月" Value="否" />
                            </f:DropDownList>
                            <f:NumberBox runat="server" ID="txtMonthDay" Label="月结日期"  LabelAlign="Right" Hidden="True" Text="25"/>
                            <f:TextBox ID="txtcom_tel" runat="server" Label="电话" EmptyText="电话" LabelAlign="Right">
                            </f:TextBox>
                            <f:TextBox ID="txtFAddress" runat="server" Label="地址" EmptyText="地址" LabelAlign="Right">
                            </f:TextBox>
                            <f:TextArea ID="txtcom_desc" runat="server" Label="备注" EmptyText="备注" LabelAlign="Right">
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