<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PoundEdit.aspx.cs" Inherits="Enterprise.IIS.business.Liquid.PoundEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" runat="server" Layout="Fit" ShowBorder="False" ShowHeader="false"
            BodyPadding="0px">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server" Position="Bottom" ToolbarAlign="Right">
                    <Items>
                        <f:Button ID="btnClose" EnablePostBack="false" Text="关闭" runat="server" Icon="SystemClose" Size="Medium">
                        </f:Button>
                        <f:Button ID="btnSubmit" Text="提交表单" runat="server" Hidden="False" Icon="SystemSaveNew" Size="Medium"
                            ValidateForms="SimpleForm1" OnClick="btnSubmit_Click">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:TabStrip ID="TabStrip1" ShowBorder="false" TabPosition="Top" EnableTabCloseMenu="false"
                    ActiveTabIndex="0" runat="server">
                    <Tabs>
                        <f:Tab ID="Tab1" Title="基本信息" BodyPadding="10px" Layout="Fit"
                            runat="server">
                            <Items>
                                <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="100px"
                                    runat="server">
                                    <Rows>
                                        <f:FormRow ID="FormRow1">
                                            <Items>
                                                <f:Label ID="txtKeyId" runat="server" LabelAlign="Right" Label="单据号" />
                                                <f:DatePicker ID="txtFDate" runat="server" LabelAlign="Right" Label="日期"  />
                                                <f:DropDownList ID="tbxFItemName" runat="server" EnableEdit="True"
                                                    LabelAlign="Right" Label="商品名称" >
                                                </f:DropDownList>
                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow2">
                                            <Items>
                                                <f:DropDownList ID="tbxFBill" runat="server" LabelAlign="Right" Label="业务类型" >
                                                    <f:ListItem Text="进销" Value="进销" Selected="True" />
                                                    <f:ListItem Text="自销" Value="自销" />
                                                    <f:ListItem Text="代运" Value="代运" />
                                                </f:DropDownList>
                                                <f:DropDownList ID="tbxFVehicleNum" runat="server" LabelAlign="Right" Label="车牌号" AutoPostBack="true" OnSelectedIndexChanged="tbxFVehicleNum_SelectedIndexChanged">
                                                </f:DropDownList>
                                                <f:DropDownList ID="tbxFDriver"  EnableEdit="True" EnableMultiSelect="true" 
                                                     runat="server" LabelAlign="Right" Label="司机" >
                                                </f:DropDownList>
                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow3">
                                            <Items>
                                                <f:DropDownList ID="tbxFSupercargo" runat="server" LabelAlign="Right" Label="押运员" >
                                                </f:DropDownList>
                                                <f:NumberBox ID="tbxFMargin" Required="True" runat="server" LabelAlign="Right" Label="装车前余量" Text="0">
                                                </f:NumberBox>
                                                <f:TextBox ID="txtFBeginAddress" LabelAlign="Right"  Label="起点" runat="server" />
                                            </Items>
                                        </f:FormRow>
                                        
                                        <f:FormRow ID="FormRow70">
                                            <Items>
                                                <f:TextBox ID="txtFEndAddress" LabelAlign="Right"  Label="终点" runat="server" />
                                                <f:NumberBox ID="txtFBeginMileage" runat="server" LabelAlign="Right" Label="起始里程数" Text="0">
                                                </f:NumberBox>
                                                <f:NumberBox ID="txtFEndMileage" runat="server" LabelAlign="Right" Label="结束里程数" Text="0">
                                                </f:NumberBox>
                                           </Items>
                                        </f:FormRow>
                                        
                                        <f:FormRow ID="FormRow71">
                                            <Items>
                                                <f:NumberBox ID="txtFQty" runat="server" LabelAlign="Right" Label="加油量" Text="0">
                                                </f:NumberBox>
                                                <f:NumberBox ID="txtFPrice" runat="server" LabelAlign="Right" Label="单价" Text="0">
                                                </f:NumberBox>
                                                <f:NumberBox ID="txtFAmount" runat="server" LabelAlign="Right" Label="金额" Text="0">
                                                </f:NumberBox>
                                           </Items>
                                        </f:FormRow>
                                        
                                        <f:FormRow ID="FormRow72">
                                            <Items>
                                                <f:NumberBox ID="txtFOtherAmount" runat="server" LabelAlign="Right" Label="其它费用" Text="0">
                                                </f:NumberBox>
                                                <f:NumberBox ID="txtFMileage" runat="server" LabelAlign="Right" Label="总公里数" Text="0">
                                                </f:NumberBox>  
                                                <f:DropDownList runat="server" ID="ddlFSalesman" Label="业务员" EnableEdit="True" LabelAlign="Right" />
                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow4">
                                            <Items>

                                                <f:TextArea ID="txtFMemo" LabelAlign="Right" Label="备注"
                                                    EmptyText="备注" runat="server">
                                                </f:TextArea>

                                            </Items>
                                        </f:FormRow>
                                    </Rows>
                                </f:Form>
                            </Items>
                        </f:Tab>
                        <f:Tab ID="Tab2" Title="采购信息" BodyPadding="10px" Layout="Fit"
                            runat="server">
                            <Items>
                                <f:Form ID="SimpleForm2" ShowBorder="false" ShowHeader="False" LabelWidth="80px"
                                    runat="server">
                                    <Rows>
                                        <f:FormRow ID="FormRow7">
                                            <Items>
                                                <f:TextBox runat="server" Label="客户代码" ID="txtFSupplierCode"  LabelAlign="Right" />
                                                <f:TriggerBox ID="tbxFSupplier" EnablePostBack="True" OnTextChanged="tbxFSupplier_OnTextChanged"
                                                    ShowLabel="true" Label="客户名称" LabelAlign="Right" 
                                                    TriggerIcon="Search" runat="server" AutoPostBack="True">
                                                </f:TriggerBox>
                                                <%--<f:TimePicker ID="txtFPurchasedDate" LabelAlign="Right" EnableEdit="false" Label="装车时间" Increment="5"
                                                    EmptyText="装车时间" runat="server">
                                                </f:TimePicker>--%>
                                                <f:Label runat="server" Hidden="True"/>

                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow8">
                                            <Items>

                                                <f:NumberBox ID="txtFGross" runat="server" LabelAlign="Right" Label="毛重" Text="0">
                                                </f:NumberBox>

                                                <f:NumberBox ID="txtFPacked" runat="server" LabelAlign="Right" Label="皮重" Text="0">
                                                </f:NumberBox>

                                                <f:NumberBox ID="tbxFPurchasedQty" runat="server" LabelAlign="Right" Label="净重" Text="0">
                                                </f:NumberBox>

                                            </Items>
                                        </f:FormRow>

                                        <f:FormRow ID="FormRow9">
                                            <Items>
                                                <f:NumberBox ID="tbxFPurchasedPrice" runat="server" LabelAlign="Right" Label="采购价" Text="0">
                                                </f:NumberBox>
                                                
                                                <f:DatePicker ID="FDate4" runat="server" Label="装车日期" DateFormatString="yyyy-MM-dd" LabelAlign="Right">
                                                </f:DatePicker>
                                                
                                                <f:TimePicker ID="txtFPurchasedDate" LabelAlign="Right" EnableEdit="false" Label="卸车时间" Increment="5"
                                                    EmptyText="卸车时间" runat="server">
                                                </f:TimePicker>
                                            </Items>
                                        </f:FormRow>
                                    </Rows>
                                </f:Form>
                            </Items>
                        </f:Tab>

                        <f:Tab ID="Tab3" Title="销售信息" BodyPadding="10px" Layout="Fit"
                            runat="server">
                            <Items>
                                <f:Form ID="Form2" ShowBorder="false" ShowHeader="False" LabelWidth="80px"
                                    runat="server">
                                    <Rows>
                                        <f:FormRow ID="FormRow20">
                                            <Items>
                                                <f:TextBox runat="server" Label="客户代码" ID="tbxFCode1"  LabelAlign="Right" />
                                                <f:TriggerBox ID="tbxFName1" EnablePostBack="True" OnTextChanged="tbxFName1_OnTextChanged"
                                                    ShowLabel="true" Label="客户名称" LabelAlign="Right"
                                                    Readonly="false" TriggerIcon="Search" runat="server" AutoPostBack="True">
                                                </f:TriggerBox>
                                                <f:Label runat="server" Hidden="True"/>
                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow21">
                                            <Items>
                                                <f:NumberBox ID="txtFGross1" runat="server" LabelAlign="Right" Label="毛重" Text="0">
                                                </f:NumberBox>

                                                <f:NumberBox ID="txtFPacked1" runat="server" LabelAlign="Right" Label="皮重" Text="0">
                                                </f:NumberBox>

                                                <f:NumberBox ID="txtFQty1" runat="server" LabelAlign="Right" Label="净重" Text="0">
                                                </f:NumberBox>

                                            </Items>
                                        </f:FormRow>

                                        <f:FormRow ID="FormRow22">
                                            <Items>

                                                <f:NumberBox ID="txtFPrice1" runat="server" LabelAlign="Right" Label="销售价" Text="0">
                                                </f:NumberBox>
                                                
                                                <f:DatePicker ID="FDate1" runat="server" Label="卸车日期" DateFormatString="yyyy-MM-dd" LabelAlign="Right">
                                                </f:DatePicker>
                                                
                                                <f:TimePicker ID="FTime1" LabelAlign="Right" EnableEdit="false" Label="卸车时间" Increment="5"
                                                    EmptyText="卸车时间" runat="server">
                                                </f:TimePicker>

                                            </Items>
                                        </f:FormRow>

                                        <f:FormRow ID="FormRowBr1">
                                            <Items>
                                                <f:ContentPanel runat="server" ShowBorder="False">
                                                    <hr /> 
                                                </f:ContentPanel>
                                            </Items>
                                        </f:FormRow>

                                        <f:FormRow ID="FormRow23">
                                            <Items>
                                                <f:TextBox runat="server" Label="客户代码" ID="tbxFCode2"  LabelAlign="Right" />
                                                <f:TriggerBox ID="tbxFName2" EnablePostBack="True" OnTextChanged="tbxFName2_OnTextChanged"
                                                    ShowLabel="true" Label="客户名称" LabelAlign="Right"  AutoPostBack="True"
                                                    Readonly="false" TriggerIcon="Search" runat="server">
                                                </f:TriggerBox>
                                                <f:Label runat="server" Hidden="True"/>
                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow24">
                                            <Items>

                                                <f:NumberBox ID="txtFGross2" runat="server" LabelAlign="Right" Label="毛重" Text="0">
                                                </f:NumberBox>

                                                <f:NumberBox ID="txtFPacked2" runat="server" LabelAlign="Right" Label="皮重" Text="0">
                                                </f:NumberBox>

                                                <f:NumberBox ID="txtFQty2" runat="server" LabelAlign="Right" Label="净重" Text="0">
                                                </f:NumberBox>

                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow222">
                                            <Items>

                                                <f:NumberBox ID="txtFPrice2" runat="server" LabelAlign="Right" Label="销售价" Text="0">
                                                </f:NumberBox>
                                                
                                                <f:DatePicker ID="FDate2" runat="server" Label="卸车日期" DateFormatString="yyyy-MM-dd" LabelAlign="Right">
                                                </f:DatePicker>
                                                
                                                <f:TimePicker ID="FTime2" LabelAlign="Right" EnableEdit="false" Label="卸车时间" Increment="5"
                                                    EmptyText="卸车时间" runat="server">
                                                </f:TimePicker>
                                            </Items>
                                        </f:FormRow>

                                        <f:FormRow ID="FormRowBr2">
                                            <Items>
                                                <f:ContentPanel runat="server" ShowBorder="False">
                                                    <hr />
                                                </f:ContentPanel>
                                            </Items>
                                        </f:FormRow>

                                        <f:FormRow ID="FormRow33">
                                            <Items>
                                                <f:TextBox runat="server" Label="客户代码" ID="tbxFCode3"  LabelAlign="Right" />
                                                <f:TriggerBox ID="tbxFName3" EnablePostBack="True" OnTextChanged="tbxFName3_OnTextChanged"
                                                    ShowLabel="true" Label="客户名称" LabelAlign="Right"  AutoPostBack="True"
                                                    Readonly="false" TriggerIcon="Search" runat="server">
                                                </f:TriggerBox>
                                                <f:Label runat="server" Hidden="True"/>
                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow34">
                                            <Items>

                                                <f:NumberBox ID="txtFGross3" runat="server" LabelAlign="Right" Label="毛重" Text="0" EmptyText="0">
                                                </f:NumberBox>

                                                <f:NumberBox ID="txtFPacked3" runat="server" LabelAlign="Right" Label="皮重" Text="0">
                                                </f:NumberBox>

                                                <f:NumberBox ID="txtFQty3" runat="server" LabelAlign="Right" Label="净重" Text="0">
                                                </f:NumberBox>

                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow2222">
                                            <Items>

                                                <f:NumberBox ID="txtFPrice3" runat="server" LabelAlign="Right" Label="销售价" Text="0">
                                                </f:NumberBox>
                                                
                                                <f:DatePicker ID="FDate3" runat="server" Label="卸车日期" DateFormatString="yyyy-MM-dd" LabelAlign="Right">
                                                </f:DatePicker>
                                                
                                                <f:TimePicker ID="FTime3" LabelAlign="Right" EnableEdit="false" Label="卸车时间" Increment="5"
                                                    EmptyText="卸车时间" runat="server">
                                                </f:TimePicker>
                                            </Items>
                                        </f:FormRow>

                                    </Rows>
                                </f:Form>
                            </Items>
                        </f:Tab>
                    </Tabs>
                </f:TabStrip>
            </Items>
        </f:Panel>

        <f:Window ID="Window2" runat="server" WindowPosition="Center" IsModal="true"
            Icon="ApplicationViewDetail" EnableMaximize="true" EnableResize="true" Hidden="True" EnableClose="true"
            Target="Parent" EnableIFrame="true" Title="选择供货商列表"
            IFrameUrl="about:blank" Height="450px" Width="800px" OnClose="Window2_Close">
        </f:Window>

        <f:Window ID="Window1" runat="server" WindowPosition="Center" IsModal="true"
            Icon="ApplicationViewDetail" EnableMaximize="true" EnableResize="true" Hidden="True" EnableClose="true"
            Target="Parent" EnableIFrame="true" Title="选择客户列表"
            IFrameUrl="about:blank" Height="450px" Width="800px" OnClose="Window2_Close">
        </f:Window>

    </form>

    <object id="LODOP_OB" classid="clsid:2105C259-1E0C-4534-8141-A753534CB4CA" width="0" height="0">
        <embed id="LODOP_EM" type="application/x-print-lodop" width="0" height="0"></embed>
    </object>
    <script type="text/javascript" src="../../jqueryui/js/jquery-1.8.3.min.js"> </script>

    <script type="text/javascript" src="../../res/My97DatePicker/WdatePicker.js"></script>
    <script type="text/javascript" src="../../jqueryui/js/jquery-ui-1.9.2.custom.js?v=4.0"> </script>
    <script type="text/javascript" src="../../js/LodopFuncs.js"></script>
    <script type="text/javascript">
        var inputselector = '.f-grid-tpl input';

        F.ready(function () {
            var txtcode = '<%= tbxFSupplier.ClientID %>';
            $('#' + txtcode + ' input').autocomplete({
                source: function (request, response) {
                    $.getJSON("../../Common/AjaxSupplier.ashx", request, function (data, status, xhr) {
                        response(data);
                    });
                }
            });

        });


    </script>
</body>
</html>
