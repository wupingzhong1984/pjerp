<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FMonthPurchaseDetails.aspx.cs" Inherits="Enterprise.IIS.business.Reports.FMonthPurchaseDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>调度明细出库表</title>
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
                                <f:DropDownList runat="server" ID="ddlFClass" EnableEdit="True"
                                    Label="业务类型" Required="True" ShowRedStar="True">
                                    <f:ListItem Text="全部" Value="-1" Selected="true" />
                                    <f:ListItem Text="气体采购" Value="气体采购" />
                                    <f:ListItem Text="液体采购" Value="液体采购" />
                                    <f:ListItem Text="配件采购" Value="配件采购" />
                                    <f:ListItem Text="气瓶采购" Value="气瓶采购" />
                                    <f:ListItem Text="劳保用品" Value="劳保用品" />
                                    <f:ListItem Text="车辆配件" Value="车辆配件" />
                                    <f:ListItem Text="石油采购" Value="石油采购" />
                                    <f:ListItem Text="轮胎采购" Value="轮胎采购" />
                                    <f:ListItem Text="五金设备" Value="五金设备" />
                                    <f:ListItem Text="加工配件" Value="加工配件" />
                                    <f:ListItem Text="文具采购" Value="文具采购" />
                                    <f:ListItem Text="电脑打印机" Value="电脑打印机" />
                                    <f:ListItem Text="厨房用品" Value="厨房用品" />
                                    <f:ListItem Text="消防器材" Value="消防器材" />
                                    <f:ListItem Text="印刷品" Value="印刷品" />
                                    <f:ListItem Text="工厂用品" Value="工厂用品" />
                                    <f:ListItem Text="车辆公共配件" Value="车辆公共配件" />
                                    <f:ListItem Text="粤通卡" Value="粤通卡" />
                                    <f:ListItem Text="其他" Value="其他" />
                                </f:DropDownList>
                                <f:TextBox runat="server" ID="txtKeyId" Label="单据号" />
                                <f:TextBox runat="server" ID="txtFCode" Label="客户代码" />
                                <f:TextBox runat="server" ID="txtItemCode" Label="商品代码" />
                                <f:TextBox runat="server" ID="txtItemName" Label="商品名称" />
                                <f:DropDownList runat="server" ID="ddlFVehicleNum" Label="车牌号" EnableEdit="True" LabelAlign="Right" />

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
                                                <f:BoundField MinWidth="120px" ColumnID="KeyId" DataField="KeyId" HeaderText="调度号" SortField="KeyId" />
                                                <f:BoundField MinWidth="110px" ColumnID="FCode" DataField="FCode" HeaderText="客户代码" SortField="FCode" />
                                                <f:BoundField MinWidth="220px" ColumnID="FName" DataField="FName" HeaderText="客户名称" SortField="FName" />
                                                <f:BoundField MinWidth="120px" ColumnID="FItemCode" DataField="FItemCode" HeaderText="商品代码" SortField="FItemCode" />
                                                <f:BoundField MinWidth="120px" ColumnID="FItemName" DataField="FItemName" HeaderText="商品名称" SortField="FItemName" />
                                                <f:BoundField MinWidth="120px" ColumnID="FSpec" DataField="FSpec" HeaderText="规格" SortField="FSpec" />
                                                <f:BoundField MinWidth="80px" ColumnID="FQty" DataField="FQty" HeaderText="商品入库数" SortField="FQty" TextAlign="Right" />
                                                <f:BoundField MinWidth="80px" ColumnID="FBottleQty" DataField="FBottleQty" HeaderText="容器入库数" SortField="FBottleQty" TextAlign="Right" />
                                                <f:BoundField MinWidth="80px" ColumnID="FReturnQty" DataField="FReturnQty" HeaderText="商品出库数" SortField="FReturnQty" TextAlign="Right" />
                                                <f:BoundField MinWidth="80px" ColumnID="FRecycleQty" DataField="FRecycleQty" HeaderText="容器出库数" SortField="FRecycleQty" TextAlign="Right" />
                                                <f:BoundField MinWidth="80px" ColumnID="FPrice" DataField="FPrice" HeaderText="单价" SortField="FPrice" TextAlign="Right" Hidden="true" />
                                                <f:BoundField MinWidth="80px" ColumnID="FAmount" DataField="FAmount" HeaderText="金额" SortField="FAmount" TextAlign="Right"  Hidden="true" />
                                                <f:BoundField MinWidth="110px" ColumnID="FBottle" DataField="FBottle" HeaderText="包装物编码" SortField="FBottle" />
                                                <f:BoundField MinWidth="110px" ColumnID="FBottleName" DataField="FBottleName" HeaderText="包装物" SortField="FBottleName" />
                                                <f:BoundField MinWidth="160px" ColumnID="FMemo" DataField="FMemo" HeaderText="备注" SortField="FMemo" />

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

