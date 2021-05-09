<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Enterprise.IIS.business.Warehouse.Edit" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>仓库信息</title>
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
                    <f:SimpleForm ID="SimpleForm1" ShowBorder="false" ShowHeader="false" LabelAlign="Right"
                        AutoScroll="true" BodyPadding="5px" runat="server" EnableCollapse="True">
                        <Items>
                            
                            <f:TextBox ID="txtFCode" runat="server" Label="仓库编码" EmptyText="仓库编码" Required="True"
                                ShowRedStar="True">
                            </f:TextBox>
                            <f:TextBox ID="txtFName" runat="server" Label="仓库名称" MinLength="2" Required="True"
                                ShowRedStar="True" EmptyText="仓库名称">
                            </f:TextBox>
                            <f:TextBox ID="txtFLinkman" runat="server" Label="库管员" EmptyText="库管员">
                            </f:TextBox>
                            <f:TextBox ID="txtFPhome" runat="server" Label="电话" EmptyText="电话">
                            </f:TextBox>
                            <f:TextBox ID="txtFAddress" runat="server" Label="地址" EmptyText="地址">
                            </f:TextBox>
                            <f:TextArea ID="txtFMemo" runat="server" Label="备注" EmptyText="备注">
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
