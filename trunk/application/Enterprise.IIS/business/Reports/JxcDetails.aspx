<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JxcDetails.aspx.cs" Inherits="Enterprise.IIS.business.Reports.JxcDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>商品进销存明细表</title>
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

                <f:Region ID="Region1" Title="商品进销存明细表" Position="Center"
                    ShowBorder="false" BoxConfigAlign="Stretch" Layout="Fit" runat="server" ShowHeader="False">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <f:Button ID="btnSearch" OnClick="btnSearch_Click" runat="server" Text="查询" Icon="Zoom" Hidden="False" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnPrint" OnClick="btnPrint_Click" runat="server" Text="打印"
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
                                                        <f:TextBox ID="txtFName" runat="server" Label="商品名称">
                                                        </f:TextBox>
                                                        <f:Label runat="server" Hidden="True"/>
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
                                            runat="server" EnableCheckBoxSelect="False" DataKeyNames="FName"
                                            IsDatabasePaging="False" SortDirection="DESC"
                                            AllowSorting="False" EmptyText="查询无结果" EnableHeaderMenu="True"
                                            EnableSummary="true" SummaryPosition="Bottom">
                                            <Columns>
                                                <f:BoundField ColumnID="FName" MinWidth="220px" DataField="FName" HeaderText="品名" SortField="FName" />
                                               <f:BoundField ColumnID="FSpec" MinWidth="80px" DataField="FSpec" HeaderText="规格" SortField="FSpec" />
                                                 <f:BoundField ColumnID="FUltQty" MinWidth="100px" DataField="FUltQty" HeaderText="上月结存" SortField="FUltQty" TextAlign="right"/>
                                                 <f:GroupField HeaderText="本月收入" TextAlign="Center" EnableLock="true" SortField="mc">
                                                    <Columns>
                                                        <f:BoundField Width="80px" ColumnID="FCZ" DataField="FCZ" HeaderText="充装" SortField="FCZ" TextAlign="right"/>
                                                        <f:BoundField Width="80px" ColumnID="FWG"  DataField="FWG" HeaderText="采购" SortField="FWG"  TextAlign="right"/>
                                                        <f:BoundField Width="80px" ColumnID="FXSTH"  DataField="FXSTH" HeaderText="退货" SortField="FXSTH"  TextAlign="right"/>
                                                        <f:BoundField Width="80px" ColumnID="FPY"  DataField="FPY" HeaderText="盘盈" SortField="FPY"  TextAlign="right"/>
                                                        <f:BoundField Width="80px" ColumnID="FPQ"  DataField="FPQ" HeaderText="配气" SortField="FPQ"  TextAlign="right"/>
                                                    </Columns>
                                                </f:GroupField>
                                                <f:BoundField ColumnID="FITotal" MinWidth="100px" DataField="FITotal" HeaderText="合计" SortField="FITotal" TextAlign="right"/>
                                                <f:GroupField ColumnID="mc" HeaderText="本月支出" TextAlign="Center" EnableLock="true" SortField="mc">
                                                    <Columns>
                                                        <f:BoundField Width="80px" ColumnID="FXS"  DataField="FXS" HeaderText="销售" SortField="FXS" TextAlign="right"/>
                                                        <f:BoundField Width="80px" ColumnID="FDB"  DataField="FDB" HeaderText="调拨" SortField="FDB"  TextAlign="right"/>
                                                        <f:BoundField Width="80px" ColumnID="FZY"  DataField="FZY" HeaderText="自用" SortField="FZY"  TextAlign="right"/>
                                                        <f:BoundField Width="80px" ColumnID="FZS"  DataField="FZS" HeaderText="增送" SortField="FZS"  TextAlign="right"/>
                                                        <f:BoundField Width="80px" ColumnID="FPK"  DataField="FPK" HeaderText="盘亏" SortField="FPK"  TextAlign="right"/>
                                                    </Columns>
                                                </f:GroupField>
                                                <f:BoundField ColumnID="FOTotal" MinWidth="100px" DataField="FOTotal" HeaderText="合计" SortField="FOTotal" TextAlign="right"/>
                                                <f:BoundField ColumnID="FEnd" MinWidth="100px" DataField="FEnd" HeaderText="本月结存" SortField="FEnd" TextAlign="right"/>
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
        
        function closeActiveTab() {
            parent.removeActiveTab();
        }
    </script>
</body>
</html>
