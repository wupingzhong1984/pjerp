<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WinSalaryDriver.aspx.cs" Inherits="Enterprise.IIS.Common.WinSalaryDriver" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>生成调度单</title>
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../jqueryui/css/ui-lightness/jquery-ui-1.9.2.custom.css" />
    <style type="text/css">
        #Panel1_Panel2_SimpleForm1_FormRow1_dptBeginDate-labelEl {
            color: red;
        }

        #Panel1_Panel2_SimpleForm1_FormRow2_ddlLogistics-labelEl {
            color: red;
        }
    </style>

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
                            OnClick="btnSubmit_Click" ConfirmText="确定配送工资吗？">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:Panel ID="Panel2" Layout="Fit" runat="server" ShowBorder="false" ShowHeader="false">
                    <Items>
                        <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="120px"
                            runat="server">
                            <Rows>
                                <f:FormRow ID="FormRow1">
                                    <Items>
                                        <f:Label ID="lblKeyId" runat="server" Label="订单号" LabelAlign="Right"></f:Label>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow2">
                                    <Items>

                                        <f:DatePicker ID="dptBeginDate" runat="server" LabelAlign="Right" Label="出发日期" EmptyText="出发日期" Required="True" ShowRedStar="True">
                                        </f:DatePicker>
                                        <f:DatePicker ID="dptEnd" runat="server" LabelAlign="Right" Label="回厂日期" EmptyText="回厂日期">
                                        </f:DatePicker>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow3">
                                    <Items>
                                        <f:DropDownList runat="server" ID="ddlLogistics" Label="物流公司"
                                            OnSelectedIndexChanged="ddlFLogistics_SelectedIndexChanged"
                                            AutoPostBack="true" LabelAlign="Right" />
                                        <f:TimePicker ID="dpBeginTime" LabelAlign="Right" EnableEdit="false" Label="出车时间" Increment="5"
                                            EmptyText="请选择时间" runat="server">
                                        </f:TimePicker>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow4">
                                    <Items>
                                        <f:DropDownList runat="server" ID="ddlFVehicleNum" DataTextField="FNum" DataValueField="FNum" LabelAlign="Right" Label="车牌号" EnableEdit="True" />
                                        <f:NumberBox ID="txtFMileage" runat="server" LabelAlign="Right" Label="里程数(km)" EmptyText="里程数(km)"></f:NumberBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow5">
                                    <Items>
                                        <f:DropDownList runat="server" ID="ddlFrom" DataTextField="FKey" DataValueField="FKey" LabelAlign="Right" Label="运行起点" EnableEdit="True" />
                                        <f:DropDownList runat="server" ID="ddlTo" DataTextField="FKey" DataValueField="FKey" LabelAlign="Right" Label="运行止点" EnableEdit="True" />
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow6">
                                    <Items>
                                        <f:DropDownList runat="server" ID="ddlFDriver" LabelAlign="Right" Label="司机" DataTextField="name" DataValueField="name" EnableEdit="True" EnableMultiSelect="true" />
                                        <f:DropDownList runat="server" ID="ddlFSupercargo" LabelAlign="Right" Label="押运员" DataTextField="name" DataValueField="name" EnableEdit="True" EnableMultiSelect="true" />
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow7">
                                    <Items>
                                        <f:NumberBox ID="txtTaskQty" runat="server" LabelAlign="Right" Label="点数" EmptyText="点数"></f:NumberBox>
                                        <f:NumberBox ID="txtFDriverYQty" runat="server" LabelAlign="Right" Label="Y瓶数" EmptyText="Y瓶数"></f:NumberBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow8">
                                    <Items>
                                        <f:NumberBox ID="txtFDriverQty" runat="server" LabelAlign="Right" Label="散瓶数" EmptyText="散瓶数"></f:NumberBox>
                                        <f:NumberBox ID="txtFDriverJQty" runat="server" LabelAlign="Right" Label="集格数" EmptyText="集格数"></f:NumberBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow9">
                                    <Items>
                                        <f:NumberBox ID="txtFDriverMileageAmt" runat="server" LabelAlign="Right" Label="司机行程工资" EmptyText="司机行程工资"></f:NumberBox>
                                        <f:NumberBox ID="txtFSupercargoMileageAmt" runat="server" LabelAlign="Right" Label="押运行程工资" EmptyText="押运行程工资"></f:NumberBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow11">
                                    <Items>
                                        <f:TextArea ID="txtFMemo" runat="server" LabelAlign="Right" Label="摘要"></f:TextArea>
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
