<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockOnDayBottle.aspx.cs" Inherits="Enterprise.IIS.business.Reports.StockOnDayBottle" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>当日气体气瓶库存表</title>
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

                <f:Region ID="Region1" Title="当日气体气瓶库存表" Position="Center"
                    ShowBorder="False" BoxConfigAlign="Stretch" Layout="Fit" runat="server" ShowHeader="False">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <f:Button ID="btnSearch" OnClick="btnSearch_Click" runat="server" Text="查询" Icon="Zoom" Hidden="False" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnPrint" OnClick="btnBatchDelete_Click" runat="server" Text="打印"
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
                                    BodyPadding="3px" runat="server" Height="45px" EnableCollapse="True">
                                    <Items>
                                        <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="80px"
                                            runat="server" BodyPadding="5px" LabelAlign="Right">
                                            <Rows>
                                                <f:FormRow ID="FormRow1">
                                                    <Items>
                                                        <f:DropDownList runat="server" ID="ddlCompany" Label="所属公司" LabelAlign="Right"></f:DropDownList>
                                                        <f:DatePicker ID="dpFDate" runat="server" Label="日期" DateFormatString="yyyy-MM-dd">
                                                        </f:DatePicker>
                                                        <f:TextBox ID="txtFName" runat="server" Label="商品名称">
                                                        </f:TextBox>
                                                    </Items>
                                                </f:FormRow>
                                            </Rows>
                                        </f:Form>
                                    </Items>
                                </f:Region>
                                <f:Region ID="Region4" ShowBorder="False" ShowHeader="false" Position="Center"
                                    Layout="Fit" runat="server" BodyPadding="3px" AutoScroll="True">
                                    <Items>
                                        <f:Grid ID="Grid1" PageSize="200" ShowBorder="False" ShowHeader="False" AllowPaging="False"
                                            runat="server" EnableCheckBoxSelect="False" DataKeyNames="KeyId" OnPageIndexChange="Grid1_PageIndexChange"
                                            IsDatabasePaging="False" OnSort="Grid1_Sort" SortDirection="DESC" OnRowCommand="Grid1_RowCommand"
                                            AllowSorting="False" EmptyText="查询无结果" EnableHeaderMenu="True"
                                            EnableSummary="true" SummaryPosition="Bottom">
                                            <Columns>
                                                <f:BoundField MinWidth="30px" ColumnID="FItemCode" DataField="FItemCode" HeaderText="编码" SortField="FItemCode" />
                                                <f:BoundField MinWidth="160px" ColumnID="FName" DataField="FName" HeaderText="名称" SortField="FName" />
                                                <f:BoundField MinWidth="40px" ColumnID="FSpec" DataField="FSpec" HeaderText="规格" SortField="FSpec" />
                                                <f:BoundField MinWidth="40px" ColumnID="FUnit" DataField="FUnit" HeaderText="单位" SortField="FUnit" />
                                                <f:GroupField HeaderText="本日期初" TextAlign="Center" EnableLock="true" SortField="mc">
                                                    <Columns>
                                                        <f:BoundField Width="80px" ColumnID="FInitTotal"  DataField="FInitTotal" HeaderText="总数" SortField="FInitTotal" TextAlign="right"/>
                                                        <f:BoundField Width="80px" ColumnID="FInitGQty"  DataField="FInitGQty" HeaderText="实瓶" SortField="FInitGQty"  TextAlign="right"/>
                                                        <f:BoundField Width="80px" ColumnID="FInitBQty"  DataField="FInitBQty" HeaderText="空瓶" SortField="FInitBQty"  TextAlign="right"/>
                                                    </Columns>
                                                </f:GroupField>
                                                <f:GroupField HeaderText="本日收入" TextAlign="Center" EnableLock="true" SortField="mc">
                                                    <Columns>
                                                        <f:BoundField Width="80px" ColumnID="FInTotal"  DataField="FInTotal" HeaderText="总数" SortField="FInTotal" TextAlign="right"/>
                                                        <f:BoundField Width="80px" ColumnID="FGInQty"  DataField="FGInQty" HeaderText="实瓶" SortField="FGInQty"  TextAlign="right"/>
                                                        <f:BoundField Width="80px" ColumnID="FBInQty"  DataField="FBInQty" HeaderText="空瓶" SortField="FBInQty"  TextAlign="right"/>
                                                    </Columns>
                                                </f:GroupField>
                                                <f:GroupField HeaderText="本日发出" TextAlign="Center" EnableLock="true" SortField="mc">
                                                    <Columns>
                                                        <f:BoundField Width="80px" ColumnID="FOutTotal"  DataField="FOutTotal" HeaderText="总数" SortField="FOutTotal" TextAlign="right"/>
                                                        <f:BoundField Width="80px" ColumnID="FGOutQty"  DataField="FGOutQty" HeaderText="实瓶" SortField="FGOutQty"  TextAlign="right"/>
                                                        <f:BoundField Width="80px" ColumnID="FBOutQty"  DataField="FBOutQty" HeaderText="空瓶" SortField="FBOutQty"  TextAlign="right"/>
                                                    </Columns>
                                                </f:GroupField>
                                                <f:GroupField HeaderText="本日结存" TextAlign="Center" EnableLock="true" SortField="mc">
                                                    <Columns>
                                                        <f:BoundField Width="80px" ColumnID="FEndTotal"  DataField="FEndTotal" HeaderText="总数" SortField="FEndTotal" TextAlign="right"/>
                                                        <f:BoundField Width="80px" ColumnID="FEndGQty"  DataField="FEndGQty" HeaderText="实瓶" SortField="FEndGQty"  TextAlign="right"/>
                                                        <f:BoundField Width="80px" ColumnID="FEndBQty"  DataField="FEndBQty" HeaderText="空瓶" SortField="FEndBQty"  TextAlign="right"/>
                                                    </Columns>
                                                </f:GroupField>
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
            Target="Parent" EnableIFrame="True" IFrameUrl="about:blank" Height="450px" Width="800px"
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
        
        function closeActiveTab() {
            parent.removeActiveTab();
        }
    </script>
</body>
</html>
