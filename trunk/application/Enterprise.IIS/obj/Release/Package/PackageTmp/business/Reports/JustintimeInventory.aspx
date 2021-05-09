<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JustintimeInventory.aspx.cs" Inherits="Enterprise.IIS.business.Reports.JustintimeInventory" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>商品实时库存统计</title>
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

                <f:Region ID="Region1" Title="商品实时库存统计" Position="Center"
                    ShowBorder="false" BoxConfigAlign="Stretch" Layout="Fit" runat="server" ShowHeader="False">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <f:Button ID="Button1" OnClick="btnSearch_Click" runat="server" Text="查询"
                                    Icon="Zoom" Hidden="False" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnPrint" runat="server" Text="打印"
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
                                    BodyPadding="3px" runat="server" Height="42px" EnableCollapse="True">
                                    <Items>
                                        <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="80px"
                                            runat="server" BodyPadding="5px"  LabelAlign="Right">
                                            <Rows>
                                                <f:FormRow ID="FormRow1">
                                                    <Items>
                                                        <f:DropDownList runat="server" ID="ddlCompany" Label="所属公司" LabelAlign="Right"></f:DropDownList>
                                                        <f:DatePicker ID="dateBegin" runat="server" Label="年月" DateFormatString="yyyyMM">
                                                        </f:DatePicker>
                                                        <f:TextBox ID="txtFName" runat="server" Label="商品名称">
                                                        </f:TextBox>
                                                        <f:DropDownList ID="ddlType" runat="server" Label="产品类型" DataTextField="FName" DataValueField="FId" EnableEdit="true"></f:DropDownList>
                                                    </Items>
                                                </f:FormRow>
                                            </Rows>
                                        </f:Form>
                                    </Items>
                                </f:Region>
                                <f:Region ID="Region4" ShowBorder="False" ShowHeader="false" Position="Center"
                                    Layout="Fit" runat="server" BodyPadding="3px" AutoScroll="True">
                                    <Items>
                                        <f:Grid ID="Grid1" PageSize="20000" ShowBorder="false" ShowHeader="false" AllowPaging="False"
                                            runat="server" EnableCheckBoxSelect="False" DataKeyNames="FCode"
                                            IsDatabasePaging="False" OnSort="Grid1_Sort" SortDirection="DESC"
                                            AllowSorting="False" EmptyText="查询无结果" EnableHeaderMenu="True"
                                            EnableSummary="true" SummaryPosition="Bottom">
                                            <Columns>
                                                <f:BoundField MinWidth="80px" ColumnID="FCode" DataField="FCode" HeaderText="编码" SortField="FCode" />
                                                <f:BoundField MinWidth="220px" ColumnID="FName" DataField="FName" HeaderText="名称" SortField="FName" />
                                                <f:BoundField MinWidth="80px" ColumnID="FSpec" DataField="FSpec" HeaderText="规格" SortField="FSpec" TextAlign="Center" />
                                                <f:BoundField MinWidth="120px" ColumnID="FUnit" DataField="FUnit" HeaderText="单位" SortField="FUnit" TextAlign="Center" />
                                                <f:BoundField MinWidth="120px"  ColumnID="FInitQty" DataField="FInitQty" HeaderText="本月期初" SortField="FInitQty" TextAlign="Center" />
                                                <f:BoundField MinWidth="120px" ColumnID="FInQty" DataField="FInQty" HeaderText="本月收入" SortField="FInQty" TextAlign="Center" />
                                                <f:BoundField MinWidth="120px" ColumnID="FOutQty" DataField="FOutQty" HeaderText="本月支出" SortField="FOutQty" TextAlign="Center" />
                                                <f:BoundField MinWidth="120px"  ColumnID="FNowQty" DataField="FNowQty" HeaderText="本月库存" SortField="FNowQty" TextAlign="Right" />
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
        function openAddFineUI(keyid) {
            var url = 'business/Sales/Details.aspx?action=6&keyid=' + keyid;
            parent.addExampleTab.apply(null, ['add_tab_' + keyid, basePath + url, '详情', basePath + 'icon/page_find.png', true]);
        }
        function closeActiveTab() {
            parent.removeActiveTab();
        }
    </script>
</body>
</html>
