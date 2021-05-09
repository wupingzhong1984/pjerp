<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TubePriceEidt.aspx.cs" Inherits="Enterprise.IIS.business.TubeInvo.TubePriceEidt" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>设置运算公式</title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../../jqueryui/css/ui-lightness/jquery-ui-1.9.2.custom.css" />
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:HiddenField ID="HiddenField1" runat="server"></f:HiddenField>
        <f:Panel ID="Panel1" Layout="Fit" BodyPadding="3px" runat="server" ShowBorder="false" ShowHeader="false" AutoScroll="True">
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
                <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="90px"
                    runat="server" LabelAlign="Right">
                    <Rows>
                        <f:FormRow ID="FormRow0">
                            <Items>
                                <f:DropDownList runat="server" ID="ddlBill" Label="业务" LabelAlign="Right">
                                    <f:ListItem Text="销售" Value="销售" />
                                    <f:ListItem Text="采购" Value="采购" />
                                </f:DropDownList>
                            </Items>
                        </f:FormRow>

                        <f:FormRow ID="FormRow1">
                            <Items>
                                <f:TextBox runat="server" Label="客户代码" ID="txtFCode" Required="True"
                                    LabelAlign="Right" />
                            </Items>
                        </f:FormRow>

                        <f:FormRow ID="FormRow2">
                            <Items>
                                <f:TriggerBox ID="tbxFCustomer" EnablePostBack="True" OnTextChanged="tbxFCustomer_OnTextChanged"
                                    ShowLabel="true" ShowRedStar="True" Required="true" Label="客户名称" LabelAlign="Right"
                                    Readonly="false" TriggerIcon="Search" runat="server" AutoPostBack="True">
                                </f:TriggerBox>
                            </Items>
                        </f:FormRow>

                        <f:FormRow ID="FormRow3">
                            <Items>
                                <f:TextBox runat="server" Label="商品代码" ID="txtFItemCode" LabelAlign="Right" Required="True" />
                            </Items>
                        </f:FormRow>

                        <f:FormRow ID="FormRow4">
                            <Items>
                                <f:TriggerBox ID="tbxFName" EnablePostBack="True" OnTextChanged="tbxFName_OnTextChanged"
                                    ShowLabel="true" ShowRedStar="True" Required="true" Label="商品名称"
                                    Readonly="false" TriggerIcon="Search" runat="server" LabelAlign="Right" AutoPostBack="True">
                                </f:TriggerBox>
                            </Items>
                        </f:FormRow>

                        <f:FormRow ID="FormRow5">
                            <Items>
                                <f:TextArea runat="server" Label="计算公式" ID="txtFFormula" LabelAlign="Right" />
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow8">
                            <Items>
                                <f:TextArea runat="server" Label="备注" ID="txtFMemo" LabelAlign="Right" />
                            </Items>
                        </f:FormRow>

                        <f:FormRow ID="FormRowBr1">
                            <Items>
                                <f:ContentPanel runat="server" ShowBorder="False">
                                    <hr />
                                    <p>$W1：进厂温度、$W2：出厂温度、$W3:交付温度、$W4:接收温度</p>
                                    <p>$Y1：进厂压力、$Y2：出厂压力、$Y3:交付压力、$Y4:接收压力</p>
                                    <p>$V:水容积</p>

                                </f:ContentPanel>
                            </Items>
                        </f:FormRow>



                    </Rows>
                </f:Form>
            </Items>
        </f:Panel>
        <f:Window ID="Window2" runat="server" WindowPosition="Center" IsModal="true"
            Icon="ApplicationViewDetail" EnableMaximize="true" EnableResize="true" Hidden="True" EnableClose="true"
            Target="Parent" EnableIFrame="true" Title="客户档案"
            IFrameUrl="about:blank" Height="480px" Width="960px" OnClose="Window1_Close">
        </f:Window>


    </form>
    <object id="LODOP_OB" classid="clsid:2105C259-1E0C-4534-8141-A753534CB4CA" width="0" height="0">
        <embed id="LODOP_EM" type="application/x-print-lodop" width="0" height="0"></embed>
    </object>
    <script src="../../jqueryui/js/jquery-1.8.3.min.js" type="text/javascript"> </script>
    <script src="../../jqueryui/js/jquery-ui-1.9.2.custom.js?v=4.0" type="text/javascript"> </script>
    <script type="text/javascript" language="javascript" src="../../js/LodopFuncs.js"></script>
    <script type="text/javascript">
        var basePath = '<%= ResolveUrl("~/") %>';

        var inputselector = '.f-grid-tpl input';

        F.ready(function () {
            var txtcode = '<%= tbxFCustomer.ClientID %>';
            $('#' + txtcode + ' input').autocomplete({
                source: function (request, response) {
                    $.getJSON("../../Common/AjaxCustomer.ashx", request, function (data, status, xhr) {
                        response(data);
                    });
                }
            }, 1000);


            var txtname = '<%= tbxFName.ClientID %>';
            $('#' + txtname + ' input').autocomplete({
                source: function (request, response) {
                    $.getJSON("../../Common/AjaxProduct.ashx", request, function (data, status, xhr) {
                        response(data);
                    });
                }
            }, 1000);
        });
    </script>
</body>
</html>
