<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerMonthProc.aspx.cs" Inherits="Enterprise.IIS.business.Reports.CustomerMonthProc" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>客户每月应收款统计</title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .x-grid-row-summary .x-grid-cell-inner {
            font-weight: bold;
            color: red;
        }

        .x-grid-row-summary .x-grid-cell {
            background-color: #fff !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>

                <f:Region ID="Region1" Title="客户每月应收款统计" Position="Center"
                    ShowBorder="false" BoxConfigAlign="Stretch" Layout="Fit" runat="server" ShowHeader="False">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>

                                <f:Button ID="Button1" OnClick="btnSearch_Click" runat="server" Text="查询"
                                    Icon="Zoom" Hidden="False" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnResase" runat="server" Text="重置"
                                                            Icon="PageRefresh" Hidden="False" Size="Medium" OnClick="btnResase_Click">
                                </f:Button>
                                <f:Button ID="btnPrint" OnClick="btnExport_Click" runat="server" Text="打印"
                                    Icon="Printer" Hidden="False" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnExport" OnClick="btnExport_Click" runat="server" Text="引出" Icon="PageExcel"
                                    Hidden="False" EnableAjax="false" DisableControlBeforePostBack="false" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnClose" EnablePostBack="False" Text="关闭" runat="server" Icon="SystemClose" Size="Medium" OnClientClick="closeActiveTab();">
                                </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Items>
                        <f:RegionPanel ID="RegionPanel2" ShowBorder="false" runat="server">
                            <Regions>
                                <f:Region ID="Region3" ShowBorder="false" ShowHeader="false" Split="False"
                                    Position="Top" Layout="Fit"
                                    BodyPadding="3px" runat="server" Height="68px" EnableCollapse="True">
                                    <Items>
                                        <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="80px"
                                            runat="server" BodyPadding="5px">
                                            <Rows>
                                                <f:FormRow ID="FormRow1">
                                                    <Items>
                                                        <f:DropDownList runat="server" ID="ddlCompany" Label="所属公司" LabelAlign="Right"></f:DropDownList>
                                                        <f:DatePicker ID="dpkFDate" runat="server" Label="年月" DateFormatString="yyyyMM" LabelAlign="Right">
                                                        </f:DatePicker>
                                                        <f:DropDownList runat="server" ID="ddlSeller" Label="业务员" LabelAlign="Right" DataTextField="account_name" DataValueField="id"></f:DropDownList>
                                                    </Items>
                                                </f:FormRow>
                                                <f:FormRow ID="FormRow2">
                                                    <Items>
                                                        <f:TextBox runat="server" Label="客户代码" ID="txtFCode" Readonly="True" LabelAlign="Right" />
                                                        <f:TriggerBox ID="tbxFCustomer" EnablePostBack="True" OnTextChanged="tbxFCustomer_OnTextChanged"
                                                            ShowLabel="true" Label="客户名称" LabelAlign="Right"
                                                            Readonly="false" TriggerIcon="Search" runat="server">
                                                        </f:TriggerBox>
                                                        <f:Label runat="server" Hidden="True"/>
                                                    </Items>
                                                </f:FormRow>
                                            </Rows>
                                        </f:Form>
                                    </Items>
                                </f:Region>
                                <f:Region ID="Region4" ShowBorder="False" ShowHeader="False" Position="Center"
                                    Layout="Fit" runat="server" BodyPadding="3px" AutoScroll="True">
                                    <Items>
                                        <f:Grid ID="Grid1" ShowBorder="False" ShowHeader="False" AllowPaging="False"
                                            runat="server" EnableCheckBoxSelect="False" DataKeyNames="FCode"
                                            IsDatabasePaging="False" OnSort="Grid1_Sort" SortDirection="DESC"
                                            AllowSorting="true" EmptyText="查询无结果" EnableHeaderMenu="True"
                                            EnableSummary="true" SummaryPosition="Bottom">
                                            <Columns>
                                                <f:BoundField MinWidth="80px" ColumnID="FCode" DataField="FCode" HeaderText="编码" SortField="FCode" />
                                                <f:BoundField MinWidth="220px" ColumnID="FName" DataField="FName" HeaderText="名称" SortField="FName" />
                                                <f:BoundField MinWidth="120px" ColumnID="FInitAR" DataField="FInitAR" HeaderText="期初应收款" SortField="FInitAR" TextAlign="Center" />
                                                <f:BoundField MinWidth="120px" ColumnID="FPeriodAR" DataField="FPeriodAR" HeaderText="本期应收款" SortField="FPeriodAR" TextAlign="Center" />
                                                <f:BoundField MinWidth="120px" ColumnID="FPeriodReturn" DataField="FPeriodReturn" HeaderText="本期退货款" SortField="FPeriodReturn" TextAlign="Center" />
                                                <f:BoundField MinWidth="120px" ColumnID="FPeriodRecover" DataField="FPeriodRecover" HeaderText="本期收回应收款" SortField="FPeriodRecover" TextAlign="Center" />
                                                <f:BoundField MinWidth="120px" ColumnID="FDiscountAmount" DataField="FDiscountAmount" HeaderText="本期优惠" SortField="FDiscountAmount" TextAlign="Center" />
                                                <f:BoundField MinWidth="120px" ColumnID="FFinalAR" DataField="FFinalAR" HeaderText="期未应收款" SortField="FFinalAR" TextAlign="Center" />
                                            </Columns>
                                        </f:Grid>
                                    </Items>
                                </f:Region>
                            </Regions>
                        </f:RegionPanel>
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>
        <br />
        <f:Window ID="Window1" runat="server" WindowPosition="Center" IsModal="True" EnableClose="True"
            Icon="ApplicationViewDetail" Title="客户档案" EnableMaximize="True" EnableResize="True" Hidden="True"
            Target="Parent" EnableIFrame="True" IFrameUrl="about:blank" Height="580px" Width="960px"
            OnClose="Window1_Close">
        </f:Window>

        <f:Window ID="Window2" Icon="PageAttach" runat="server" Hidden="true"
            IsModal="true" Target="Parent" EnableMaximize="true" EnableResize="true" OnClose="Window2_Close"
            Title="Popup Window 2" CloseAction="HidePostBack"
            EnableIFrame="true" Height="350px" Width="450px">
        </f:Window>

    </form>
    <script src="../../jqueryui/js/jquery-1.8.3.min.js" type="text/javascript"> </script>
    <script src="../../jqueryui/js/jquery-ui-1.9.2.custom.js?v=4.0" type="text/javascript"> </script>
    <script type="text/javascript">
        var basePath = '<%= ResolveUrl("~/") %>';
        function openAddFineUI(keyid) {
            var url = 'business/Sales/Details.aspx?action=6&keyid=' + keyid;
            parent.addExampleTab.apply(null, ['add_tab_' + keyid, basePath + url, '详情', basePath + 'icon/page_find.png', true]);
        };
        function closeActiveTab() {
            parent.removeActiveTab();
        };
        F.ready(function () {
            var txtcode = '<%= tbxFCustomer.ClientID %>';

            $('#' + txtcode + ' input').autocomplete({
                source: function (request, response) {
                    $.getJSON("../../Common/AjaxCustomer.ashx", request, function (data, status, xhr) {
                        response(data);
                    });
                }
            });
        });
    </script>
</body>
</html>
