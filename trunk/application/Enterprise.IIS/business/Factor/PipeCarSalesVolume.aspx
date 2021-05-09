<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PipeCarSalesVolume.aspx.cs" Inherits="Enterprise.IIS.business.Factor.PipeCarSalesVolume" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
     <form runat="server" id="form1">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region1" Title="生产成本" Position="Center"
                    ShowBorder="False" BoxConfigAlign="Stretch" Layout="Fit" runat="server" ShowHeader="False" Icon="Page">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <f:Label runat="server" ID="headerText" Text="排管销售对账表" />
                                <f:ToolbarSeparator ID="ToolbarSeparator2" runat="server" />
                                <f:Button ID="btnSearch" OnClick="btnSearch_Click" runat="server" Text="查询" Icon="Zoom" Hidden="False" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnExport" OnClick="btnExport_Click" runat="server" Text="引出" Icon="PageExcel"
                                    Hidden="False" EnableAjax="false" DisableControlBeforePostBack="false" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnClose" EnablePostBack="False" Text="关闭" runat="server" Icon="SystemClose" Size="Medium" OnClientClick="closeActiveTab();">
                                </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Items>
                        <f:RegionPanel ID="RegionPanel2" ShowBorder="false" runat="server">
                            <Regions>
                                <f:Region ID="Region3" ShowBorder="false" ShowHeader="false" Split="False"
                                    Position="Top" Layout="Fit"
                                    BodyPadding="3px" runat="server" Height="66px" EnableCollapse="True">
                                    <Items>
                                        <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="80px"
                                            runat="server" BodyPadding="5px">
                                            <Rows>
                                                <f:FormRow ID="FormRow1">
                                                    <Items>
                                                        <f:DatePicker ID="dateBegin" runat="server" Label="开始日期" DateFormatString="yyyy-MM-dd" LabelAlign="Right">
                                                        </f:DatePicker>
                                                        <f:DatePicker ID="dateEnd" runat="server" Label="结束日期" DateFormatString="yyyy-MM-dd" LabelAlign="Right">
                                                        </f:DatePicker>
                                                        <f:DropDownList runat="server" ID="ddlFDistributionPoint" Label="作业区" Required="True" LabelAlign="Right" ShowRedStar="True" EnableEdit="True" />
                                                       
                                                    </Items>
                                                </f:FormRow>
                                                
                                                <f:FormRow ID="FormRow2">
                                                    <Items>
                                                         <f:TextBox runat="server" Label="客户代码" ID="txtFCode" Required="True" LabelAlign="Right" />
                                                       <f:TriggerBox ID="tbxFCustomer" EnablePostBack="True" OnTextChanged="tbxFCustomer_OnTextChanged"
                                                            ShowLabel="true" ShowRedStar="True" Required="true" Label="客户名称" LabelAlign="Right"
                                                            Readonly="false" TriggerIcon="Search" runat="server" AutoPostBack="True">
                                                        </f:TriggerBox>
                                                        <f:DropDownList runat="server" ID="ddlDeliveryMethod" Label="配送方式"
                                                            Required="True" LabelAlign="Right" ShowRedStar="false" />
                                                         <f:TextBox runat="server" Label="单价金额" ID="txtPrice" Required="false" LabelAlign="Right" />
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
                                            runat="server" EnableCheckBoxSelect="False"
                                            IsDatabasePaging="False" SortDirection="ASC"
                                            AllowSorting="true" EmptyText="查询无结果" EnableHeaderMenu="True"
                                            EnableSummary="true" SummaryPosition="Bottom" OnRowDataBound="Grid1_RowDataBound" >
                                            <Columns>
                                                <f:BoundField MinWidth="40px" ColumnID="FDistributionPoint" DataField="FDistributionPoint" HeaderText="提货点" SortField="FDistributionPoint" />
                                                <f:BoundField MinWidth="40px" ColumnID="FDate" DataField="FDate" HeaderText="日期" SortField="FDate"  />
                                                <f:BoundField MinWidth="40px" ColumnID="FDevice" DataField="FDevice" HeaderText="牌号" SortField="FDevice" />
                                                <f:BoundField MinWidth="40px" ColumnID="tank" DataField="tank" HeaderText="罐号" SortField="tank" />
                                                <f:BoundField MinWidth="40px" ColumnID="waterspace" DataField="waterspace" HeaderText="水溶积" SortField="waterspace" />
                                                <f:BoundField MinWidth="40px" ColumnID="FPayTemperature" DataField="FPayTemperature" HeaderText="满温" SortField="FPayTemperature" />
                                                <f:BoundField MinWidth="40px" ColumnID="FPayPressure" DataField="FPayPressure" HeaderText="满压" SortField="FPayPressure" />
                                                
                                                <f:BoundField MinWidth="40px" ColumnID="FReceiveTemperature" DataField="FReceiveTemperature" HeaderText="余温" SortField="FReceiveTemperature" />
                                                <f:BoundField MinWidth="40px" ColumnID="FReceivePressure" DataField="FReceivePressure" HeaderText="余压" SortField="FReceivePressure" />
                                                <f:BoundField MinWidth="40px" ColumnID="bulking" DataField="bulking" HeaderText="总体积" SortField="bulking" DataFormatString="{0:F}" />
                                                <f:BoundField MinWidth="40px" ColumnID="FPrice" DataField="FPrice" HeaderText="单价" SortField="FPrice" DataFormatString="{0:F}" />
                                                <f:BoundField MinWidth="40px" ColumnID="cost" DataField="cost" HeaderText="总金额" SortField="cost" DataFormatString="{0:F2}" />
                                                
                                                <f:BoundField MinWidth="40px" ColumnID="KeyId" DataField="KeyId" HeaderText="单号" SortField="KeyId" />

                                                <f:BoundField MinWidth="40px" ColumnID="FCode" DataField="FCode" HeaderText="产品编码" SortField="FCode" Hidden="true" />
                                                <f:BoundField MinWidth="40px" ColumnID="FMemo" DataField="FMemo" HeaderText="备注" SortField="FMemo" Hidden="true" />

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
         <f:Window ID="Window2" runat="server" WindowPosition="Center" IsModal="true"
            Icon="ApplicationViewDetail" EnableMaximize="true" EnableResize="true" Hidden="True" EnableClose="true"
            Target="Parent" EnableIFrame="true" Title="客户档案"
            IFrameUrl="about:blank" Height="480px" Width="800px" OnClose="Window1_Close">
        </f:Window>
    </form>
    <script src="../../jqueryui/js/jquery-1.8.3.min.js" type="text/javascript"> </script>
    <script src="../../jqueryui/js/jquery-ui-1.9.2.custom.js?v=4.0" type="text/javascript"> </script>
    <script type="text/javascript">
        var basePath = '<%= ResolveUrl("~/") %>';
        function closeActiveTab() {
            parent.removeActiveTab();
        };
        F.ready(function () {

        });
    </script>

</body>
</html>
