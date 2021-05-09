<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LeaseDetails.aspx.cs" Inherits="Enterprise.IIS.business.Reports.LeaseDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>气瓶租赁明细表</title>
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
                <f:Region ID="Region1" ShowHeader="False" Split="True" Hidden="False" ShowBorder="False"
                    Width="240px" Position="Left" Layout="Fit" runat="server">
                    <Items>
                        <f:SimpleForm ID="SimpleForm2" BodyPadding="5px" LabelWidth="80px" EnableCollapse="False"
                            runat="server" ShowBorder="False" ShowHeader="False" LabelAlign="Right"
                            Title="查询条件">
                            <Items>
                                <f:DatePicker ID="dpFDateBegion" Label="开始日期" Required="true" runat="server">
                                </f:DatePicker>
                                <f:DatePicker ID="dpFDateEnd" Label="结束日期" Required="true" CompareControl="DatePicker1"
                                    CompareOperator="GreaterThan" CompareMessage="结束日期应该大于开始日期！" runat="server">
                                </f:DatePicker>
                                <f:TextBox runat="server" ID="txtKeyId" Label="单据号" />
                                <f:TextBox runat="server" ID="txtFCode" Label="客户代码" />
                                <f:TextBox runat="server" ID="txtFName" Label="客户名称" />
                                <f:TextBox runat="server" ID="txtItemCode" Label="商品代码" />
                            </Items>
                        </f:SimpleForm>
                    </Items>
                </f:Region>
                <f:Region ID="Region2" Title="客户管理" Position="Center"
                    ShowBorder="False" BoxConfigAlign="Stretch" Layout="Fit" runat="server" ShowHeader="False">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <f:Button ID="btnSearch" runat="server" Text="查询" Icon="Zoom" OnClick="btnSearch_Click" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnReset" runat="server" Text="重置" Icon="ArrowRefresh" OnClick="btnRest_Click" Size="Medium">
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
                                <f:Region ID="Region3" ShowBorder="false" ShowHeader="False" Split="False"
                                    Position="Center" Layout="Fit"
                                    BodyPadding="0px" runat="server" Height="280px" EnableCollapse="True">
                                    <Items>
                                        <f:Grid ID="Grid1" PageSize="200" ShowBorder="False" ShowHeader="False" AllowPaging="False"
                                            runat="server" EnableCheckBoxSelect="True" DataKeyNames="KeyId,FType"
                                            IsDatabasePaging="False" SortDirection="ASC"
                                            AllowSorting="False" EmptyText="查询无结果" EnableHeaderMenu="True"
                                            EnableMultiSelect="False" EnableRowClickEvent="true" OnRowClick="Grid1_RowClick"
                                            EnableSummary="true" SummaryPosition="Bottom">
                                            <Columns>
                                                <f:BoundField MinWidth="110px" ColumnID="FDate" DataField="FDate" HeaderText="日期" SortField="FDate" DataFormatString="{0:yyyy-MM-dd}" />
                                                <f:BoundField MinWidth="120px" ColumnID="KeyId" DataField="KeyId" HeaderText="单号" SortField="KeyId" />
                                                <f:BoundField MinWidth="110px" ColumnID="FCode" DataField="FCode" HeaderText="客户代码" SortField="FCode" />
                                                <f:BoundField MinWidth="220px" ColumnID="FName" DataField="FName" HeaderText="客户名称" SortField="FName" />
                                                <f:BoundField MinWidth="120px" ColumnID="FBottle" DataField="FBottle" HeaderText="商品代码" SortField="FBottle" />
                                                <f:BoundField MinWidth="120px" ColumnID="FBottleName" DataField="FBottleName" HeaderText="商品名称" SortField="FBottleName" />
                                                <f:BoundField MinWidth="80px" ColumnID="FSpec" DataField="FSpec" HeaderText="规格" SortField="FSpec" />
                                                <f:BoundField MinWidth="60px" ColumnID="FUnit" DataField="FUnit" HeaderText="单位" SortField="FUnit" />
                                                
                                                
                                                <f:BoundField MinWidth="80px" ColumnID="FPrice" DataField="FPrice" HeaderText="单价" SortField="FPrice" TextAlign="Right" />
                                                <f:BoundField MinWidth="80px" ColumnID="FBottleQty" DataField="FBottleQty" HeaderText="数量" SortField="FBottleQty" TextAlign="Right" />
                                                <f:BoundField MinWidth="80px" ColumnID="FAmount" DataField="FAmount" HeaderText="金额" SortField="FAmount" TextAlign="Right" />
                                                <f:BoundField MinWidth="80px" ColumnID="FRentDay" DataField="FRentDay" HeaderText="日租金" SortField="FRentDay" TextAlign="Right" />
                                                
                                                <f:BoundField MinWidth="160px" ColumnID="FMemo" DataField="FMemo" HeaderText="备注" SortField="FMemo" />

                                            </Columns>
                                        </f:Grid>
                                    </Items>
                                </f:Region>
                                <%--<f:Region ID="Region4" ShowBorder="False" ShowHeader="false" Position="Center"
                                    Layout="Fit" runat="server" BodyPadding="3px" AutoScroll="True">
                                    <Items>
                                        <f:Grid ID="Grid2" PageSize="200" ShowBorder="False" ShowHeader="False" AllowPaging="False"
                                            runat="server" EnableCheckBoxSelect="False" DataKeyNames="FId"
                                            IsDatabasePaging="False" AllowSorting="False" EmptyText="查询无结果" EnableHeaderMenu="True">
                                            <Columns>
                                                <f:BoundField MinWidth="80px" DataField="FItemCode" HeaderText="编码" SortField="FItemCode" />
                                                <f:BoundField MinWidth="220px" DataField="FItemName" HeaderText="名称" SortField="FItemName" />
                                                <f:BoundField MinWidth="80px" DataField="FSpec" HeaderText="规格" SortField="FSpec" />
                                                <f:BoundField MinWidth="80px" DataField="FUnit" HeaderText="单位" SortField="FUnit" />
                                                <f:BoundField MinWidth="80px" DataField="FQty" HeaderText="气体数量" SortField="FQty" />
                                                <f:BoundField MinWidth="80px" DataField="FBottleQty" TextAlign="Right" HeaderText="气瓶数量" />
                                            </Columns>
                                        </f:Grid>
                                    </Items>
                                </f:Region>--%>
                            </Regions>
                        </f:RegionPanel>
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>
        <br />
        <f:Window ID="Window1" runat="server" WindowPosition="Center" IsModal="true"
            Icon="ApplicationViewDetail" EnableMaximize="true" EnableResize="true" Hidden="True"
            Target="Parent" EnableIFrame="true" IFrameUrl="about:blank" Height="430px" Width="760px"
            OnClose="Window1_Close">
        </f:Window>
    </form>
    <script type="text/javascript">
        var basePath = '<%= ResolveUrl("~/") %>';

        function closeActiveTab() {
            parent.removeActiveTab();
        };
    </script>
</body>
</html>

