<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SalaryMonthDetails.aspx.cs" Inherits="Enterprise.IIS.business.Reports.SalaryMonthDetails" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>月报明细</title>
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
                <f:Region ID="Region1" Title="月报明细" Position="Center"
                    ShowBorder="false" BoxConfigAlign="Stretch" Layout="Fit" runat="server" ShowHeader="false">
                     <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                               
                                <f:Button ID="btnClose" EnablePostBack="false" OnClientClick="closeActiveTab();" Text="关闭"
                                    runat="server" Icon="SystemClose" Size="Medium">
                                </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Items>
                        <f:RegionPanel ID="RegionPanel2" ShowBorder="false" runat="server">
                            <Regions>                                
                                <f:Region ID="Region4" ShowBorder="False" ShowHeader="False" Position="Center"
                                    Layout="Fit" runat="server" BodyPadding="3px" AutoScroll="True">
                                    <Items>
                                        <f:Grid ID="Grid1" ShowBorder="False" ShowHeader="False" AllowPaging="False"
                                            runat="server" EnableCheckBoxSelect="False" DataKeyNames="KeyId,FCode"                                             EnableAjax="True" EnableAjaxLoading="True"
                                            IsDatabasePaging="False"  SortDirection="ASC"
                                            AllowSorting="false" EmptyText="查询无结果" EnableHeaderMenu="True"
                                            EnableSummary="true" SummaryPosition="Bottom">
                                            <Columns>
                                                <f:BoundField MinWidth="80px" DataField="FCode" HeaderText="客户代码" SortField="FCode" />
                                                <f:BoundField MinWidth="220px" DataField="FName" HeaderText="客户名称" SortField="FName" />
                                                <f:BoundField MinWidth="80px" ColumnID="FInit" DataField="FInit" HeaderText="本月期初" SortField="FInit" TextAlign="Right" />
                                                <f:BoundField MinWidth="80px" ColumnID="FSales" DataField="FSales" HeaderText="本月销售" SortField="FSales" TextAlign="Right" />
                                                <f:BoundField MinWidth="80px" ColumnID="FReturned" DataField="FReturned" HeaderText="本月回款" SortField="FReturned" TextAlign="Right" />
                                                <f:BoundField MinWidth="80px" ColumnID="FReturn" DataField="FReturn" HeaderText="本月退货" SortField="FReturn" TextAlign="Right" />
                                                <f:BoundField MinWidth="80px" ColumnID="FDiscountAmount" DataField="FDiscountAmount" HeaderText="本月优惠" SortField="FDiscountAmount" TextAlign="Right" />
                                                <f:BoundField MinWidth="80px" ColumnID="FEnd" DataField="FEnd" HeaderText="本月期未" SortField="FEnd" TextAlign="Right" />
                                                <f:BoundField MinWidth="120px" ColumnID="FMemo" DataField="FMemo" HeaderText="备注" SortField="FMemo" />
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
    </form>
</body>
</html>
