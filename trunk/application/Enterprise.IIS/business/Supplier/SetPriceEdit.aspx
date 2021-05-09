<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetPriceEdit.aspx.cs" Inherits="Enterprise.IIS.business.Supplier.SetPriceEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../../jqueryui/css/ui-lightness/jquery-ui-1.9.2.custom.css" />
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:HiddenField ID="HiddenField1" runat="server"></f:HiddenField>
        <f:Panel ID="Panel1" Layout="Fit" runat="server" ShowBorder="false" ShowHeader="false" AutoScroll="True">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server" Position="Bottom" ToolbarAlign="Right">
                    <Items>
                        <f:Button ID="btnClose" EnablePostBack="false" Text="关闭" runat="server" Icon="SystemClose" Size="Medium">
                        </f:Button>
                        <f:Button ID="btnSubmit" Text="保存" runat="server" Icon="SystemSaveNew" ValidateForms="SimpleForm1"
                            OnClick="btnSubmit_Click" Size="Medium">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="110px"
                    runat="server">
                    <Rows>
                        <f:FormRow ID="FormRow1">
                            <Items>
                                <f:DropDownList runat="server" ID="ddlCustomer" Label="供应商名称" CompareType="String" CompareValue="-1"
                                    EnableEdit="True" CompareOperator="NotEqual" CompareMessage="请选供应商名称！" Required="True" ShowRedStar="True">
                                </f:DropDownList>
                                <f:DropDownList runat="server" ID="ddlItem" Label="商品名称" CompareType="String" CompareValue="-1"
                                    EnableEdit="True" CompareOperator="NotEqual" CompareMessage="请选商品名称！" Required="True" ShowRedStar="True">
                                </f:DropDownList>
                            </Items>
                        </f:FormRow>
                        
                        <f:FormRow ID="FormRow5">
                            <Items>
                                <f:NumberBox Label="采购单价" ID="txtFPrice" runat="server" EmptyText="采购单价" Required="True" ShowRedStar="True"/>
                                <f:Label runat="server" Hidden="True"/>
                            </Items>
                        </f:FormRow>
                        
                    </Rows>
                </f:Form>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
