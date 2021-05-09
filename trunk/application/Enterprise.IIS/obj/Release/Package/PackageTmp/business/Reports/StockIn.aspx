<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockIn.aspx.cs" Inherits="Enterprise.IIS.business.Reports.StockIn" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>入库记录查询</title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region1" ShowHeader="False" Split="True" Hidden="False" ShowBorder="False"
                    Width="280px" Position="Left" Layout="Fit" runat="server">
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
                                <f:DropDownList runat="server" ID="ddlBill" Label="业务类型">
                                    <f:ListItem Text="全部" Value="-1" Selected="true" />
                                    <f:ListItem Text="销售退货入仓" Value="2" />
                                    <f:ListItem Text="采购入仓" Value="4" />
                                    <f:ListItem Text="生产入仓" Value="6" />
                                    <f:ListItem Text="退租入仓" Value="8" />
                                    <f:ListItem Text="盘盈入仓" Value="10" />
                                    <f:ListItem Text="空瓶回收入仓" Value="13" />
                                    <f:ListItem Text="其它入仓" Value="21" />
                                </f:DropDownList>
                                <f:TextBox runat="server" ID="txtKeyId" Label="单号" />
                                <f:TextBox runat="server" ID="txtFCode" Label="客户代码" />
                                <f:TextBox runat="server" ID="txtFName" Label="客户名称" />
                                <f:TextBox runat="server" ID="txtItemCode" Label="商品代码" />
                                <f:TextBox runat="server" ID="txtItemName" Label="商品名称" />
                                <f:DropDownList runat="server" ID="ddlOperator" Label="操作员"></f:DropDownList>
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
                                <f:Button ID="Button1" OnClick="btnExports_Click" runat="server" Text="引出入库详细" Icon="PageExcel"
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
                                            EnableMultiSelect="False" EnableRowClickEvent="true" OnRowClick="Grid1_RowClick">
                                            <Columns>
                                                <f:BoundField MinWidth="110px" DataField="FDate" HeaderText="日期" SortField="FDate" DataFormatString="{0:yyyy-MM-dd}" />
                                                <f:BoundField MinWidth="120px" DataField="KeyId" HeaderText="单据号" SortField="KeyId"  />
                                                <f:BoundField MinWidth="110px" DataField="FCode" HeaderText="客户代码" SortField="FCode" />
                                                <f:BoundField MinWidth="220px" DataField="FName" HeaderText="客户名称" SortField="FName" />
                                                <f:BoundField MinWidth="80px" DataField="CreateBy" HeaderText="操作员" SortField="CreateBy" />
                                                <f:BoundField MinWidth="80px" DataField="FTypeName" HeaderText="入仓类型" SortField="FTypeName" />
                                                <f:BoundField MinWidth="120px" DataField="FItemCode" HeaderText="商品编码" SortField="FItemCode" />
                                                <f:BoundField MinWidth="120px" DataField="FItemName" HeaderText="商品名称" SortField="FItemName" />
                                                <f:BoundField MinWidth="80px" DataField="FSpec" HeaderText="规格" SortField="FSpec" />
                                                <f:BoundField MinWidth="80px" DataField="FUnit" HeaderText="单位" SortField="FUnit" />
                                                <f:BoundField MinWidth="80px" DataField="FPrice" HeaderText="单价" SortField="FPrice"  TextAlign="Right"/>
                                                <f:BoundField MinWidth="80px" DataField="FQty" HeaderText="数量" SortField="FQty"  TextAlign="Right"/>
                                                <f:BoundField MinWidth="80px" DataField="FAmount" HeaderText="金额" SortField="FAmount" TextAlign="Right"/>
                                                

                                                <f:BoundField MinWidth="160px" DataField="FMemo" HeaderText="备注" SortField="FMemo" />
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

