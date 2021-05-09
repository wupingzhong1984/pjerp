<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SalaryDay.aspx.cs" Inherits="Enterprise.IIS.business.Reports.SalaryDay" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>销售日报</title>
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
                <f:Region ID="Region1" Title="销售日报" Position="Center"
                    ShowBorder="false" BoxConfigAlign="Stretch" Layout="Fit" runat="server" ShowHeader="false">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <f:Button ID="Button1" OnClick="btnSearch_Click" runat="server" Text="查询"
                                    Icon="Zoom" Hidden="False" Size="Medium">
                                </f:Button>
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
                                    BodyPadding="3px" runat="server" Height="68px" EnableCollapse="True">
                                    <Items>
                                        <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="80px"
                                            runat="server" BodyPadding="5px" LabelAlign="Right">
                                            <Rows>
                                                <f:FormRow ID="FormRow1">
                                                    <Items>
                                                        <f:DropDownList runat="server" ID="ddlCompany" Label="所属公司" LabelAlign="Right"></f:DropDownList>
                                                        <f:DatePicker ID="dpkFDateBegin" runat="server" Label="开始时间" DateFormatString="yyyy-MM-dd" LabelAlign="Right">
                                                        </f:DatePicker>
                                                        <f:DatePicker ID="dpkFDateEnd" runat="server" Label="结束时间" DateFormatString="yyyy-MM-dd" LabelAlign="Right">
                                                        </f:DatePicker>
                                                        <f:DropDownList runat="server" ID="dllFType" Label="销售业务">
                                                            <f:ListItem Text="全部" Value="全部" Selected="true" />
                                                            <f:ListItem Text="瓶气销售" Value="瓶气销售" />
	                                                        <f:ListItem Text="液体销售" Value="液体销售" />
                                                        </f:DropDownList>
                                                    </Items>
                                                </f:FormRow>
                                                <f:FormRow ID="FormRow2">
                                                    <Items>
                                                        
                                                        <f:TextBox runat="server" Label="客户代码" ID="txtFCode" Readonly="True" LabelAlign="Right" />

                                                        <f:TriggerBox ID="tbxFCustomer" EnablePostBack="True" OnTextChanged="tbxFCustomer_OnTextChanged"
                                                            ShowLabel="true" Label="客户名称" LabelAlign="Right"
                                                            Readonly="false" TriggerIcon="Search" runat="server">
                                                        </f:TriggerBox>
                                                        <f:TextBox runat="server" Label="商品代码" ID="txtItemCode" LabelAlign="Right" Readonly="True" />
                                                        <f:TriggerBox ID="tbxFItemName" EnablePostBack="True" OnTextChanged="tbxFItemName_OnTextChanged"
                                                            ShowLabel="true" Label="商品名称" Readonly="false" TriggerIcon="Search" runat="server" LabelAlign="Right">
                                                        </f:TriggerBox>
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
                                            runat="server" EnableCheckBoxSelect="False" DataKeyNames="KeyId"
                                            OnRowCommand="Grid1_RowCommand"
                                            IsDatabasePaging="False" OnSort="Grid1_Sort" SortDirection="ASC"
                                            AllowSorting="true" EmptyText="查询无结果" EnableHeaderMenu="True"
                                            EnableSummary="true" SummaryPosition="Bottom">
                                            <Columns>
                                                <f:BoundField MinWidth="130px" ColumnID="KeyId" DataField="KeyId" HeaderText="发货单号" SortField="KeyId" />
                                                <f:BoundField MinWidth="80px" DataField="FCode" HeaderText="客户代码" SortField="FCode" />
                                                <f:BoundField MinWidth="220px" DataField="FName" HeaderText="客户名称" SortField="FName" />
                                                <f:BoundField MinWidth="120px" DataField="FItemCode" HeaderText="商品代码" SortField="FItemCode" />
                                                <f:BoundField MinWidth="120px" DataField="CName" HeaderText="商品名称" SortField="CName" />
                                                <f:BoundField MinWidth="80px" DataField="FPrice" HeaderText="发货单价" SortField="FPrice" TextAlign="Right" />
                                                <f:BoundField MinWidth="80px" ColumnID="FQty" DataField="FQty" HeaderText="销售数量" SortField="FQty" TextAlign="Right" />
                                                <f:BoundField MinWidth="80px" ColumnID="FAmount" DataField="FAmount" HeaderText="销售金额" SortField="FAmount" TextAlign="Right" />
                                                <f:BoundField MinWidth="80px" ColumnID="FAmt" DataField="FAmt" HeaderText="实收金额" SortField="FAmt"  TextAlign="Right" />
                                                <f:BoundField MinWidth="80px" DataField="FSpec" HeaderText="规格" SortField="FSpec" />
                                                <f:BoundField MinWidth="80px" DataField="FUnit" HeaderText="单位" SortField="FUnit" />
                                                <f:BoundField MinWidth="120px" ColumnID="FMemo" DataField="FMemo" HeaderText="备注" SortField="FMemo" />
                                            </Columns>
                                            <Listeners>
                                                <f:Listener Event="dataload" Handler="onGridDataLoad" />
                                            </Listeners>
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
            Title="产品档案" CloseAction="HidePostBack"
            EnableIFrame="true" Height="450px" Width="850px">
        </f:Window>

    </form>
    <script src="../../jqueryui/js/jquery-1.8.3.min.js" type="text/javascript"> </script>
    <script src="../../jqueryui/js/jquery-ui-1.9.2.custom.js?v=4.0" type="text/javascript"> </script>
    <script type="text/javascript">
        var basePath = '<%= ResolveUrl("~/") %>';
        function openDetailsUI(date1, date2, code) {
            var url = 'business/Reports/Finance/BankAccountsDetails.aspx?action=6&bDate=' + date1 + "&eDate=" + date2 + "&code=" + code;
            parent.addExampleTab.apply(null, ['add_tab_' + code, basePath + url, '明细', basePath + 'icon/page_find.png', true]);
        };
        function closeActiveTab() {
            parent.removeActiveTab();
        };
        F.ready(function () {

        });
        function onGridDataLoad(event) {
            this.mergeColumns(['KeyId', 'FCode', 'FName', 'FAmt']);
        };
    </script>
</body>
</html>
