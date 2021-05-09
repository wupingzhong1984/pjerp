<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SaleItem.aspx.cs" Inherits="Enterprise.IIS.business.Reports.SaleItem" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
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
                <f:Region ID="Region1" Title="产品销售报表" Position="Center"
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
                                        <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="110px"
                                            runat="server" BodyPadding="5px" LabelAlign="Right">
                                            <Rows>
                                                <f:FormRow ID="FormRow1">
                                                    <Items>
                                                        <f:DropDownList runat="server" ID="ddlCompany" Label="所属公司" LabelAlign="Right"></f:DropDownList>
                                                        <f:DatePicker ID="dpkFDateBegin" runat="server" Label="开始日期" DateFormatString="yyyy-MM-dd" LabelAlign="Right">
                                                        </f:DatePicker>
                                                        <f:DatePicker ID="dpkFDteEnd" runat="server" Label="结束日期" DateFormatString="yyyy-MM-dd" LabelAlign="Right">
                                                        </f:DatePicker>
                                                        
                                                    
                                                    </Items>
                                                </f:FormRow> 
                                                 <f:FormRow ID="FormRow2">
                                                    <Items>
                                                        <f:DropDownList runat="server" ID="ddlItem" Label="商品名称" EnableEdit="true" LabelAlign="Right" DataTextField="account_name" DataValueField="id"></f:DropDownList>
                                                        <f:DropDownList runat="server" ID="ddlSeller" Label="业务员" LabelAlign="Right" EnableEdit="true" DataTextField="account_name" DataValueField="id"></f:DropDownList>
                                                        <f:Label ID="label1" runat="server" Hidden="true"></f:Label>
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
                                            runat="server" EnableCheckBoxSelect="False" DataKeyNames="FName,FItemCode"
                                            OnRowCommand="Grid1_RowCommand" EnableAjax="True" EnableAjaxLoading="True"
                                            IsDatabasePaging="False" OnSort="Grid1_Sort" SortDirection="ASC"
                                            AllowSorting="true" EmptyText="查询无结果" EnableHeaderMenu="True"
                                            EnableSummary="true" SummaryPosition="Bottom">
                                            <Columns>
                                                <f:LinkButtonField MinWidth="80px"  ColumnID="FItemCode" CommandName="actView" DataTextField="FItemCode" HeaderText="商品代码" SortField="FItemCode" TextAlign="Center" runat="server" />
                                               <%-- <f:BoundField MinWidth="80px" DataField="FCode" HeaderText="客户代码" SortField="FCode" />--%>
                                                <f:BoundField MinWidth="220px" DataField="FName" HeaderText="商品名称" SortField="FName" />
                                                <f:BoundField MinWidth="80px" ColumnID="FBSpec" DataField="FBSpec" HeaderText="产品规格" SortField="FBSpec" TextAlign="Right" />
                                                <f:BoundField MinWidth="80px" ColumnID="FCateName" DataField="FCateName" HeaderText="产品类型" SortField="FCateName" TextAlign="Right" />
                                                <f:BoundField MinWidth="80px" ColumnID="fQty" DataField="fQty" HeaderText="销售数量" DataFormatString="{0:f0}" SortField="fQty"  TextAlign="Right" />
                                                <f:BoundField MinWidth="80px" ColumnID="price" DataField="price" HeaderText="均价" SortField="price" DataFormatString="{0:f2}" TextAlign="Right" />
                                                <f:BoundField MinWidth="80px" ColumnID="fAmount" DataField="fAmount" HeaderText="销售金额" SortField="fAmount" DataFormatString="{0:f2}" TextAlign="Right" />  
                                                <f:BoundField MinWidth="80px" ColumnID="Fprop" DataField="Fprop" HeaderText="销售比例(%)" SortField="Fprop" DataFormatString="{0:f2}" TextAlign="Right" />                                                                                                                                           
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
        function openDetailsUI(fcode, fconpanyid, fBDate,fEDate,fSeller) {
            var url = 'business/Reports/Finance/SaleItemDetails.aspx?FCode=' + fcode + '&fBDate=' + fBDate + '&fEDate=' + fEDate + '&fcompanyid=' + fconpanyid + '&fSeller=' + fSeller;
            parent.addExampleTab.apply(null, ['add_tab_' + fcode, basePath + url, '【详情】', basePath + 'icon/page_find.png', true]);
        };
        function closeActiveTab() {
            parent.removeActiveTab();
        };
        F.ready(function () {



        });
    </script>
</body>
</html>
