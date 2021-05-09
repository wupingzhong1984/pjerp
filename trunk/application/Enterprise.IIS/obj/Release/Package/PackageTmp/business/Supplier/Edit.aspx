<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Enterprise.IIS.business.Supplier.Edit" %>

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
                <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="110px" BodyPadding="3px" LabelAlign="Right"
                    runat="server">
                    <Rows>
                        <f:FormRow ID="FormRow1">
                            <Items>
                                <%--<f:DropDownList runat="server" ID="ddlDistric" Label="所属区域" CompareType="String" CompareValue="-1"
                                    EnableEdit="True" CompareOperator="NotEqual" CompareMessage="请选区域！" Required="True" ShowRedStar="True" 
                                    DataTextField="FKey" DataValueField="FValue">
                                </f:DropDownList>--%>
                                <f:TextBox ID="txtFCode" runat="server" Label="供应商代码" EmptyText="供应商代码" Required="True"
                                    ShowRedStar="True">
                                </f:TextBox>
                                <f:TextBox ID="txtFName" runat="server" Label="供应商名称" MinLength="1" Required="True"
                                    ShowRedStar="True" EmptyText="供应商名称" AutoPostBack="True">
                                </f:TextBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow2">
                            <Items>
                                <f:TextBox ID="txtFLinkman" runat="server" Label="联系人" MinLength="1" Required="False"
                                    ShowRedStar="False" EmptyText="联系人" AutoPostBack="True">
                                </f:TextBox>
                                <f:TextBox ID="txtFAddress" runat="server" Label="地址" MinLength="1" Required="False"
                                    ShowRedStar="False" EmptyText="地址" AutoPostBack="True">
                                </f:TextBox>
                            </Items>
                        </f:FormRow>
                        <%--<f:FormRow ID="FormRow3">
                            <Items>
                                
                                <f:DatePicker ID="txtFDate" runat="server" Label="建档日期" EmptyText="建档日期" Required="True"
                                    ShowRedStar="True">
                                </f:DatePicker>
                            </Items>
                        </f:FormRow>--%>
                        <f:FormRow ID="FormRow4">
                            <Items>
                                <f:TextBox ID="txtFPhome" runat="server" Label="电话" MinLength="1" Required="False"
                                    ShowRedStar="False" EmptyText="电话" AutoPostBack="True">
                                </f:TextBox>
                                <f:TextBox ID="txtFMoile" runat="server" Label="手机" MinLength="1" Required="False"
                                    ShowRedStar="False" EmptyText="手机" AutoPostBack="True">
                                </f:TextBox>
                            </Items>
                        </f:FormRow>
                        <%--<f:FormRow ID="FormRow5">
                            <Items>
                                <f:DropDownList runat="server" ID="ddlFIsPrint" Label="是否发票"
                                    AutoPostBack="False" DataTextField="key"
                                    DataValueField="value" Required="True" ShowRedStar="True">
                                </f:DropDownList>
                                <f:NumberBox Label="提醒天数" ID="txtFTipsDay" runat="server" EmptyText="提醒天数" Required="True" ShowRedStar="True"/>
                            </Items>
                        </f:FormRow>--%>
                        <%--<f:FormRow ID="FormRow6">
                            <Items>
                                <f:NumberBox Label="运输服务费" ID="txtFFreight" runat="server" EmptyText="运输服务费" Required="True" ShowRedStar="True"/>
                                <f:DropDownList runat="server" ID="ddlFPaymentMethod" Label="结算方式" Required="True" ShowRedStar="True" DataTextField="FKey" DataValueField="FValue">
                                </f:DropDownList>
                            </Items>
                        </f:FormRow>--%>
                        <f:FormRow ID="FormRow8">
                            <Items>
                                <f:DropDownList runat="server" ID="ddlFSalesman" Label="业务员" Required="True" ShowRedStar="True" DataTextField="name" DataValueField="name">
                                </f:DropDownList>
                                <f:Label runat="server" Hidden="True"/>

                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow9">
                            <Items>
                                
                                <f:TextArea Label="备注" ID="txtFMemo" runat="server" EmptyText="备注" />
                            </Items>
                        </f:FormRow>
                    </Rows>
                </f:Form>
            </Items>
        </f:Panel>
    </form>
    <%--    <script src="../../jqueryui/js/jquery-1.8.3.min.js" type="text/javascript"> </script>
    <script src="../../jqueryui/js/jquery-ui-1.9.2.custom.js?v=4.0" type="text/javascript"> </script>
    <script type="text/javascript">
        function onReady() {
            var txtcate = '<%= txtcontact_pcz.ClientID %>';
            $('#' + txtcate).autocomplete({
                source: function (request, response) {
                    $.getJSON("../company/province_data.ashx", request, function (data, status, xhr) {
                        response(data);
                    });
                }
            });
            //window.setInterval(readIDCard, 3000);
        };

    </script>--%>
</body>
</html>
