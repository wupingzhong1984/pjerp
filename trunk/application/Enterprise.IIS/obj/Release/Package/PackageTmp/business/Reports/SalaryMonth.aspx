<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SalaryMonth.aspx.cs" Inherits="Enterprise.IIS.business.Reports.SalaryMonth" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>销售月报</title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../../jqueryui/css/ui-lightness/jquery-ui-1.9.2.custom.css" />
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
                <f:Region ID="Region1" Title="销售月报" Position="Center"
                    ShowBorder="false" BoxConfigAlign="Stretch" Layout="Fit" runat="server" ShowHeader="false">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <f:Button ID="Button1" OnClick="btnSearch_Click" runat="server" Text="查询"
                                    Icon="Zoom" Hidden="False" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnResase"  runat="server" Text="重置"
                                                            Icon="ArrowRefresh" Hidden="False" Size="Medium" OnClick="btnResase_Click"></f:Button>
                                <f:Button ID="btnPrint" OnClick="btnExport_Click" runat="server" Text="打印"
                                    Icon="Printer" Hidden="False" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnExport" OnClick="btnExport_Click" runat="server" Text="引出" Icon="PageExcel"
                                    Hidden="False" EnableAjax="false" DisableControlBeforePostBack="false" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnClose" EnablePostBack="False" Text="关闭" OnClientClick="closeActiveTab();" runat="server" Icon="SystemClose" Size="Medium">
                                </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Items>
                        <f:RegionPanel ID="RegionPanel2" ShowBorder="false" runat="server">
                            <Regions>
                                <f:Region ID="Region3" ShowBorder="false" ShowHeader="false" Split="False"
                                    Position="Top" Layout="Fit"
                                    BodyPadding="3px" runat="server" Height="66px" EnableCollapse="True">
                                    <Items>
                                        <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="110px"
                                            runat="server" BodyPadding="5px">
                                            <Rows>
                                                <f:FormRow ID="FormRow1">
                                                    <Items>
                                                        <f:DropDownList runat="server" ID="ddlCompany" Label="所属公司" LabelAlign="Right"></f:DropDownList>
                                                        <f:DatePicker ID="dpkFDateBegin" runat="server" Label="销售（年/月）" DateFormatString="yyyy-MM" LabelAlign="Right">
                                                        </f:DatePicker>
                                                        <f:DropDownList runat="server" ID="ddlSeller" Label="业务员" LabelAlign="Right" DataTextField="account_name" DataValueField="id"></f:DropDownList>
                                                    </Items>
                                                </f:FormRow>
                                                <f:FormRow ID="formrow2" >
                                                    <Items>
                                                        <f:TextBox runat="server" Label="客户代码" ID="txtFCode"  Readonly="True" LabelAlign="Right" />
                                                         <f:TriggerBox ID="tbxFCustomer" EnablePostBack="True" OnTextChanged="tbxFCustomer_OnTextChanged"
                                                            ShowLabel="true" Label="客户名称" LabelAlign="Right" AutoPostBack="True"
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
                                            runat="server" EnableCheckBoxSelect="False" DataKeyNames="KeyId,FCode"
                                            OnRowCommand="Grid1_RowCommand" EnableAjax="True" EnableAjaxLoading="True"
                                            IsDatabasePaging="False" OnSort="Grid1_Sort" SortDirection="ASC"
                                            AllowSorting="true" EmptyText="查询无结果" EnableHeaderMenu="True"
                                            EnableSummary="true" SummaryPosition="Bottom">
                                            <Columns>
                                               <%--  <f:LinkButtonField MinWidth="80px"  ColumnID="FCode" CommandName="actView" DataTextField="FCode" 
                                                    HeaderText="客户代码" SortField="FCode" TextAlign="Center" runat="server" 
                                                    />--%>
                                                <f:BoundField MinWidth="80px" DataField="FCode" HeaderText="客户代码" SortField="FCode"
                                                    DataSimulateTreeLevelField="FLevel" DataFormatString="{0}"/>
                                                <f:BoundField MinWidth="220px" DataField="FName" HeaderText="客户名称" SortField="FName" />
                                                <f:BoundField MinWidth="80px" DataField="FSalesman" HeaderText="业务员" SortField="FSalesman" />
                                                <f:BoundField MinWidth="80px" ColumnID="FInit" DataField="FInit" HeaderText="本月期初" SortField="FInit" TextAlign="Right" />
                                                <f:BoundField MinWidth="80px" ColumnID="FSales" DataField="FSales" HeaderText="本月销售" SortField="FSales"  TextAlign="Right" />
                                                <f:BoundField MinWidth="80px" ColumnID="FReturned" DataField="FReturned" HeaderText="本月回款" SortField="FReturned"  TextAlign="Right" />
                                                <f:BoundField MinWidth="80px" ColumnID="FReturn" DataField="FReturn" HeaderText="本月退货" SortField="FReturn"  TextAlign="Right" />
                                                <f:BoundField MinWidth="80px" ColumnID="FDiscountAmount" DataField="FDiscountAmount" HeaderText="本月优惠" SortField="FDiscountAmount"  TextAlign="Right" />
                                                <f:BoundField MinWidth="80px" ColumnID="FEnd" DataField="FEnd" HeaderText="本月期未" SortField="FEnd"  TextAlign="Right" />
                                                <f:BoundField MinWidth="120px" ColumnID="FMemo"  DataField="FMemo" HeaderText="备注" SortField="FMemo" />
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
        function openDetailsUI(fcode, fconpanyid, fmonth) {
            var url = 'business/Reports/Finance/SalaryMonthDetails.aspx?FCode=' + fcode + '&fmonth=' + fmonth + '&fcompanyid=' + fconpanyid;
            parent.addExampleTab.apply(null, ['add_tab_' + fcode, basePath + url, '【详情】', basePath + 'icon/page_find.png', true]);
        };
        function closeActiveTab() {
            parent.removeActiveTab();
        };
        F.ready(function () {

            

        });


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