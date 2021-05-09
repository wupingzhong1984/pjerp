<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dictionary_edit.aspx.cs"
    Inherits="Enterprise.IIS.product.LHDictionary.dictionary_edit" %>

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
                    <f:Button ID="btnSubmit" Text="提交表单" Size="Medium" Hidden="True" runat="server" Icon="SystemSaveNew" ValidateForms="SimpleForm1"
                        OnClick="btnSubmit_Click">
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
                            <f:TextBox ID="txtdict_name" runat="server" LabelAlign="Right" Label="名称" MinLength="1" Required="True"
                                ShowRedStar="True" EmptyText="名称">
                            </f:TextBox>
                            <f:TextBox ID="txtdict_key" runat="server" LabelAlign="Right" Label="键" MinLength="1" Required="True"
                                ShowRedStar="True" EmptyText="键">
                            </f:TextBox>
                              
                            <f:TextBox ID="txtdict_value" runat="server" LabelAlign="Right" Label="值" MinLength="1" Required="True"
                                ShowRedStar="True" EmptyText="值">
                            </f:TextBox>
                            <f:NumberBox Label="排序" ID="txtdict_sort"  LabelAlign="Right" runat="server" MinValue="0" NoDecimal="true"
                                NoNegative="True" Required="True" EmptyText="排序" ShowRedStar="True" />
                            <f:TextBox ID="txtdict_desc" runat="server" LabelAlign="Right" Label="描述" EmptyText="描述">
                            </f:TextBox>
                        </Items>
                    </f:SimpleForm>
                </Items>
            </f:Panel>
        </Items>
    </f:Panel>
    </form>
</body>
</html>
