<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MonthStockGas.aspx.cs" Inherits="Enterprise.IIS.business.Reports.MonthStockGas" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>气体存货库存</title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .x-grid-row-summary .x-grid-cell-inner {
            font-weight: bold;
            color: red;
        }
        .x-grid-row-summary .x-grid-cell {
            background-color: #dfeaf2;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region1" Title="气体存货库存" Position="Center"
                    ShowBorder="false" BoxConfigAlign="Stretch" Layout="Fit" runat="server" ShowHeader="False">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <f:Button ID="btnSearch" ValidateForms="SimpleForm1" OnClick="btnSearch_Click" runat="server" Text="查询" Icon="Zoom" Hidden="False" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnPrint" OnClick="btnPrint_Click" runat="server" Text="打印"
                                    Icon="Printer" Hidden="True" Size="Medium">
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
                                    BodyPadding="3px" runat="server" Height="38px" EnableCollapse="True">
                                    <Items>
                                        <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="80px"
                                            runat="server" BodyPadding="5px" LabelAlign="Right">
                                            <Rows>
                                                <f:FormRow ID="FormRow1">
                                                    <Items>
                                                        <f:DropDownList runat="server" ID="ddlCompany" Label="所属公司" LabelAlign="Right"></f:DropDownList>
                                                        <f:DatePicker ID="dpFDate" runat="server" Label="日期" DateFormatString="yyyy-MM-dd">
                                                        </f:DatePicker>
                                                        <f:DropDownList runat="server" ID="ddlFDistributionPoint" Label="作业区" Required="True" LabelAlign="Right" ShowRedStar="True" EnableEdit="True" />
                                                        <f:Label runat="server" Hidden="True" />
                                                    </Items>
                                                </f:FormRow>
                                            </Rows>
                                        </f:Form>
                                    </Items>
                                </f:Region>
                                <f:Region ID="Region4" ShowBorder="False" ShowHeader="false" Position="Center"
                                    Layout="Fit" runat="server" BodyPadding="3px" AutoScroll="True">
                                    <Items>
                                        <f:Grid ID="Grid1" ShowBorder="False" ShowHeader="False" AllowPaging="False"
                                            runat="server" EnableCheckBoxSelect="False" DataKeyNames="FCode"
                                            IsDatabasePaging="False" SortDirection="DESC" 
                                            EnableRowDoubleClickEvent="true" OnRowDoubleClick="Grid1_RowDoubleClick" EnableAjax="True" EnableAjaxLoading="True"
                                            AllowSorting="False" EmptyText="查询无结果" EnableHeaderMenu="True"
                                            EnableSummary="true" SummaryPosition="Bottom">
                                            <Columns>
                                                <f:BoundField ColumnID="FCode" MinWidth="100px" DataField="FCode" HeaderText="代码" SortField="FCode" />
                                                <f:BoundField ColumnID="FINum" MinWidth="100px" DataField="FINum" HeaderText="T6代码" SortField="FINum" />
                                                <f:BoundField ColumnID="FName" MinWidth="220px" DataField="FName" HeaderText="气体" SortField="FName" />
                                                <f:BoundField ColumnID="FSpec" MinWidth="80px" DataField="FSpec" HeaderText="规格" SortField="FSpec" />
                                                <f:BoundField MinWidth="120px" DataField="FRack" HeaderText="库位货架" SortField="FRack" />
                                                <f:BoundField ColumnID="lastMonth" MinWidth="100px" DataField="lastMonth" HeaderText="上月库存" SortField="FInit" TextAlign="right" />
                                                <f:BoundField ColumnID="FInit" MinWidth="100px" DataField="FInit" HeaderText="当日期初库存" SortField="FInit" TextAlign="right" />
                                                <f:GroupField HeaderText="当日收入" TextAlign="Center" EnableLock="true" SortField="mc">
                                                    <Columns>
                                                        <f:BoundField Width="80px" ColumnID="FPurchaseIQty" DataField="FPurchaseIQty" HeaderText="采购入库" SortField="FPurchaseIQty" TextAlign="right" />
                                                        <f:BoundField Width="80px" ColumnID="FSalesIQty" DataField="FSalesIQty" HeaderText="销售退货" SortField="FSalesIQty" TextAlign="right" />
                                                        <f:BoundField Width="80px" ColumnID="FProfitQty" DataField="FProfitQty" HeaderText="盘盈" SortField="FProfitQty" TextAlign="right" />
                                                        <f:BoundField Width="80px" ColumnID="FAllotIQty" DataField="FAllotIQty" HeaderText="调拨入库" SortField="FAllotIQty" TextAlign="right" />
                                                        <f:BoundField Width="80px" ColumnID="FIQty" DataField="FIQty" HeaderText="其它入库" SortField="FIQty" TextAlign="right" />
                                                    </Columns>
                                                </f:GroupField>
                                                <f:BoundField ColumnID="FSumIQty" MinWidth="100px" DataField="FSumIQty" HeaderText="小计" SortField="FSumIQty" TextAlign="right" />
                                                <f:GroupField ColumnID="mc" HeaderText="当日支出" TextAlign="Center" EnableLock="true" SortField="mc">
                                                    <Columns>
                                                        <f:BoundField Width="80px" ColumnID="FSalesOQty" DataField="FSalesOQty" HeaderText="销售出库" SortField="FSalesOQty" TextAlign="right" />
                                                        <f:BoundField Width="80px" ColumnID="FAllotOQty" DataField="FAllotOQty" HeaderText="调拨出库" SortField="FAllotOQty" TextAlign="right" />
                                                        <f:BoundField Width="80px" ColumnID="FOQty" DataField="FOQty" HeaderText="其它出库" SortField="FOQty" TextAlign="right" />
                                                        <f:BoundField Width="80px" ColumnID="FPurchaseOQty" DataField="FPurchaseOQty" HeaderText="采购退货" SortField="FPurchaseOQty" TextAlign="right" />
                                                        <f:BoundField Width="80px" ColumnID="FLossessQty" DataField="FLossessQty" HeaderText="盘亏" SortField="FLossessQty" TextAlign="right" />
                                                    </Columns>
                                                </f:GroupField>
                                                <f:BoundField ColumnID="FSumOQty" MinWidth="100px" DataField="FSumOQty" HeaderText="小计" SortField="FSumOQty" TextAlign="right" />
                                                <f:BoundField ColumnID="FEndQty" MinWidth="100px" DataField="FEndQty" HeaderText="当日结存" SortField="FEndQty" TextAlign="right" />
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
            Icon="ApplicationViewDetail" EnableMaximize="True" EnableResize="True" Hidden="True"
            Target="Parent" EnableIFrame="True" IFrameUrl="about:blank" Height="580px" Width="960px"
            OnClose="Window1_Close">
        </f:Window>

        <f:Window ID="Window2" Icon="PageAttach" runat="server" Hidden="true"
            IsModal="true" Target="Parent" EnableMaximize="true" EnableResize="true" OnClose="Window2_Close"
            Title="Popup Window 2" CloseAction="HidePostBack"
            EnableIFrame="true" Height="350px" Width="450px">
        </f:Window>

    </form>
    <script type="text/javascript">
        var basePath = '<%= ResolveUrl("~/") %>';
        function openDetailsUI(fcode, fconpanyid, fmonth,fpoint) {
            var url = 'business/Reports/MonthStockGasDetails.aspx?FCode=' + fcode + '&FDate=' + fmonth + '&FCompanyId=' + fconpanyid+'&FPoint='+ fpoint;
            parent.addExampleTab.apply(null, ['add_tab_' + fcode, basePath + url, '【气体存货流水清单】', basePath + 'icon/page_find.png', true]);
        };
        function closeActiveTab() {
            parent.removeActiveTab();
        }
    </script>
</body>
</html>
