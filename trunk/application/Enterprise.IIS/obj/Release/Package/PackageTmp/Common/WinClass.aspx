<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WinClass.aspx.cs" Inherits="Enterprise.IIS.Common.WinClass" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>分类管理</title>
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../jqueryui/css/ui-lightness/jquery-ui-1.9.2.custom.css" />
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" runat="server" Layout="Fit" ShowBorder="False" ShowHeader="false"
            BodyPadding="5px">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <f:Button ID="btnClose" EnablePostBack="false" Text="关闭" runat="server" Icon="SystemClose" Size="Medium">
                        </f:Button>
                        <f:Button ID="btnSubmit" Text="保存" runat="server" Icon="SystemSaveNew" ValidateForms="SimpleForm1" Size="Medium"
                            OnClick="btnSubmit_Click" ConfirmText="确定保存吗？">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:Panel ID="Panel2" Layout="Fit" runat="server" ShowBorder="false" ShowHeader="false">
                    <Items>
                        <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="120px"
                            runat="server" LabelAlign="Right">
                            <Rows>
                                <f:FormRow ID="FormRow1">
                                    <Items>
                                        <f:Label ID="lblParent" runat="server" Label="上级分类"></f:Label>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow2">
                                    <Items>
                                         <f:TextBox runat="server" ID="txtFId" Label="分类编码" Required="True" ShowRedStar="True"/>
                                        
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow3">
                                    <Items>
                                        <f:TextBox runat="server" ID="txtFName" Label="分类名称" Required="True" ShowRedStar="True"/>
                                        </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow4">
                                    <Items>
                                        <f:TextBox runat="server" ID="txtkey" Label="键值" Required="True" ShowRedStar="True"/>
                                        </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow5">
                                    <Items>
                                        <f:TextBox runat="server" ID="txtvalue" Label="键名" Required="True" ShowRedStar="True"/>
                                        </Items>
                                </f:FormRow>
                            </Rows>
                        </f:Form>
                    </Items>
                </f:Panel>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
