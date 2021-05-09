<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="primeCost.aspx.cs" Inherits="Enterprise.IIS.business.Reports.primeCost" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
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
                                <f:Label runat="server" ID="headerText" Text="生产成本" />
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
                                                        <f:DropDownList runat="server" ID="ddlFDistributionPoint" Label="作业区" LabelAlign="Right" EnableEdit="true" />
                                                        <f:DropDownList runat="server" ID="ddlFGroup" Label="班组"
                                                           LabelAlign="Right"  EnableSimulateTree="true" />
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
                                            EnableSummary="true" SummaryPosition="Bottom">
                                            <Columns>
                                                <f:BoundField MinWidth="40px" ColumnID="FINum" DataField="FINum" HeaderText="存货分类编码" SortField="FINum" />
                                                <f:BoundField MinWidth="40px" ColumnID="FName" DataField="FName" HeaderText="存货分类名称" SortField="FName" />
                                                <f:BoundField MinWidth="40px" ColumnID="FSpec" DataField="FSpec" HeaderText="规格型号" SortField="FSpec" />
                                                <f:BoundField MinWidth="40px" ColumnID="FUnit" DataField="FUnit" HeaderText="计量单位" SortField="FUnit" />
                                                <f:BoundField MinWidth="40px" ColumnID="FQty" DataField="FQty" HeaderText="数量" SortField="FQty" />
                                                <f:BoundField MinWidth="40px" ColumnID="ratio" DataField="ratio" HeaderText="换算系数" SortField="ratio" />
                                                <f:BoundField MinWidth="40px" ColumnID="volume" DataField="volume" HeaderText="体积(立方米)" SortField="volume" />
                                                <f:GroupField HeaderText="气体立方" TextAlign="Center" EnableLock="true">
                                                    <Columns>
                                                        <f:BoundField MinWidth="40px" ColumnID="H2" DataField="H2" HeaderText="H2" SortField="H2" />
                                                        <f:BoundField MinWidth="40px" ColumnID="N2" DataField="N2" HeaderText="N2" SortField="N2" />
                                                        <f:BoundField MinWidth="40px" ColumnID="Ar" DataField="Ar" HeaderText="Ar" SortField="Ar" />
                                                        <f:BoundField MinWidth="40px" ColumnID="He" DataField="He" HeaderText="He" SortField="He" />
                                                        <f:BoundField MinWidth="40px" ColumnID="CO" DataField="CO" HeaderText="CO" SortField="CO" />
                                                        <f:BoundField MinWidth="40px" ColumnID="CO2" DataField="CO2" HeaderText="CO2" SortField="CO2" />
                                                        <f:BoundField MinWidth="40px" ColumnID="O2" DataField="O2" HeaderText="O2" SortField="O2" />
                                                        <f:BoundField MinWidth="40px" ColumnID="CH4" DataField="CH4" HeaderText="CH4" SortField="CH4" />
                                                        <f:BoundField MinWidth="40px" ColumnID="Ne" DataField="Ne" HeaderText="Ne" SortField="Ne" />
                                                        <f:BoundField MinWidth="40px" ColumnID="NH3" DataField="NH3" HeaderText="NH3" SortField="NH3" />
                                                        <f:BoundField MinWidth="40px" ColumnID="PH3" DataField="PH3" HeaderText="PH3" SortField="PH3" />
                                                        <f:BoundField MinWidth="40px" ColumnID="SO2" DataField="SO2" HeaderText="SO2" SortField="SO2" />
                                                        <f:BoundField MinWidth="40px" ColumnID="C2H2" DataField="C2H2" HeaderText="C2H2" SortField="C2H2" />
                                                        <f:BoundField MinWidth="40px" ColumnID="C3H8" DataField="C3H8" HeaderText="C3H8" SortField="C3H8" />
                                                        <f:BoundField MinWidth="40px" ColumnID="SF6" DataField="SF6" HeaderText="SF6" SortField="SF6" />
                                                        <f:BoundField MinWidth="40px" ColumnID="NO" DataField="NO" HeaderText="NO" SortField="NO" />
                                                        <f:BoundField MinWidth="40px" ColumnID="SIH4" DataField="SIH4" HeaderText="SIH4" SortField="SIH4" />
                                                        <f:BoundField MinWidth="40px" ColumnID="C2H6" DataField="C2H6" HeaderText="C2H6" SortField="C2H6" />
                                                        <f:BoundField MinWidth="40px" ColumnID="Xe" DataField="Xe" HeaderText="Xe" SortField="Xe" />
                                                        <f:BoundField MinWidth="40px" ColumnID="HCL" DataField="HCL" HeaderText="HCL" SortField="HCL" />
                                                        <f:BoundField MinWidth="40px" ColumnID="CF4" DataField="CF4" HeaderText="CF4" SortField="CF4" />
                                                        <f:BoundField MinWidth="40px" ColumnID="C3H6" DataField="C3H6" HeaderText="C3H6" SortField="C3H6" />
                                                        <f:BoundField MinWidth="40px" ColumnID="Air" DataField="Air" HeaderText="Air" SortField="Air" />
                                                        <f:BoundField MinWidth="40px" ColumnID="Kr" DataField="Kr" HeaderText="Kr" SortField="Kr" />
                                                        <f:BoundField MinWidth="40px" ColumnID="N2O" DataField="N2O" HeaderText="N2O" SortField="N2O" />
                                                        <f:BoundField MinWidth="40px" ColumnID="H2S" DataField="H2S" HeaderText="H2S" SortField="H2S" />
                                                        <f:BoundField MinWidth="40px" ColumnID="C2H4" DataField="C2H4" HeaderText="C2H4" SortField="C2H4" />
                                                        <f:BoundField MinWidth="40px" ColumnID="C4H10" DataField="C4H10" HeaderText="C4H10" SortField="C4H10" />
                                                        <f:BoundField MinWidth="40px" ColumnID="CH3CL" DataField="CH3CL" HeaderText="CH3CL" SortField="CH3CL" />
                                                        <f:BoundField MinWidth="40px" ColumnID="CH3OH" DataField="CH3OH" HeaderText="CH3OH" SortField="CH3OH" />
                                                        <f:BoundField MinWidth="40px" ColumnID="CH3Br" DataField="CH3Br" HeaderText="CH3Br" SortField="CH3Br" />
                                                        <f:BoundField MinWidth="40px" ColumnID="Gsums" DataField="Gsums" HeaderText="总计" SortField="Gsums" />
                                                    </Columns>
                                                </f:GroupField>
                                                <f:GroupField HeaderText="耗材" TextAlign="Center" EnableLock="false">
                                                    <Columns>
                                                        <f:BoundField MinWidth="40px" ColumnID="NH2" DataField="NH2" HeaderText="H2" SortField="NH2" />
                                                        <f:BoundField MinWidth="40px" ColumnID="NN2" DataField="NN2" HeaderText="N2" SortField="NN2" />
                                                        <f:BoundField MinWidth="40px" ColumnID="NAr" DataField="NAr" HeaderText="Ar" SortField="NAr" />
                                                        <f:BoundField MinWidth="40px" ColumnID="NHe" DataField="NHe" HeaderText="He" SortField="NHe" />
                                                        <f:BoundField MinWidth="40px" ColumnID="NCO" DataField="NCO" HeaderText="CO" SortField="NCO" />
                                                        <f:BoundField MinWidth="40px" ColumnID="NCO2" DataField="NCO2" HeaderText="CO2" SortField="NCO2" />
                                                        <f:BoundField MinWidth="40px" ColumnID="NO2" DataField="NO2" HeaderText="O2" SortField="NO2" />
                                                        <f:BoundField MinWidth="40px" ColumnID="NCH4" DataField="NCH4" HeaderText="CH4" SortField="NCH42" />
                                                        <f:BoundField MinWidth="40px" ColumnID="NNe" DataField="NNe" HeaderText="Ne" SortField="NNe2" />
                                                        <f:BoundField MinWidth="40px" ColumnID="NNH3" DataField="NNH3" HeaderText="NH3" SortField="NNH3" />
                                                        <f:BoundField MinWidth="40px" ColumnID="NPH3" DataField="NPH3" HeaderText="PH3" SortField="NPH3" />
                                                        <f:BoundField MinWidth="40px" ColumnID="NSO2" DataField="NSO2" HeaderText="SO2" SortField="NSO2" />
                                                        <f:BoundField MinWidth="40px" ColumnID="NC2H2" DataField="NC2H2" HeaderText="C2H2" SortField="NC2H2" />
                                                        <f:BoundField MinWidth="40px" ColumnID="NC3H8" DataField="NC3H8" HeaderText="C3H8" SortField="NC3H8" />
                                                        <f:BoundField MinWidth="40px" ColumnID="NSF6" DataField="NSF6" HeaderText="SF6" SortField="NSF6" />
                                                        <f:BoundField MinWidth="40px" ColumnID="NNO" DataField="NNO" HeaderText="NO" SortField="NNO" />
                                                        <f:BoundField MinWidth="40px" ColumnID="NSIH4" DataField="NSIH4" HeaderText="SIH4" SortField="NSIH4" />
                                                        <f:BoundField MinWidth="40px" ColumnID="NC2H6" DataField="NC2H6" HeaderText="C2H6" SortField="NC2H6" />
                                                        <f:BoundField MinWidth="40px" ColumnID="NXe" DataField="NXe" HeaderText="Xe" SortField="NXe" />
                                                        <f:BoundField MinWidth="40px" ColumnID="NHCL" DataField="NHCL" HeaderText="HCL" SortField="NHCL" />
                                                        <f:BoundField MinWidth="40px" ColumnID="NCF4" DataField="NCF4" HeaderText="CF4" SortField="NCF4" />
                                                        <f:BoundField MinWidth="40px" ColumnID="NC3H6" DataField="NC3H6" HeaderText="C3H6" SortField="NC3H6" />
                                                        <f:BoundField MinWidth="40px" ColumnID="NAir" DataField="NAir" HeaderText="Air" SortField="NAir" />
                                                        <f:BoundField MinWidth="40px" ColumnID="NKr" DataField="NKr" HeaderText="Kr" SortField="NKr" />
                                                        <f:BoundField MinWidth="40px" ColumnID="NN2O" DataField="NN2O" HeaderText="N2O" SortField="NN2O" />
                                                        <f:BoundField MinWidth="40px" ColumnID="NH2S" DataField="NH2S" HeaderText="H2S" SortField="NH2S" />
                                                        <f:BoundField MinWidth="40px" ColumnID="NC2H4" DataField="NC2H4" HeaderText="C2H4" SortField="NC2H4" />
                                                        <f:BoundField MinWidth="40px" ColumnID="NC4H10" DataField="NC4H10" HeaderText="C4H10" SortField="NC4H10" />
                                                        <f:BoundField MinWidth="40px" ColumnID="NCH3CL" DataField="NCH3CL" HeaderText="CH3CL" SortField="NCH3CL" />
                                                        <f:BoundField MinWidth="40px" ColumnID="NCH3OH" DataField="NCH3OH" HeaderText="CH3OH" SortField="NCH3OH" />
                                                        <f:BoundField MinWidth="40px" ColumnID="NC3H4O" DataField="NC3H4O" HeaderText="C3H4O" SortField="NC3H4O" />
                                                        <f:BoundField MinWidth="40px" ColumnID="NCOS" DataField="NCOS" HeaderText="COS" SortField="NCOS" />
                                                        <f:BoundField MinWidth="40px" ColumnID="NC4H6" DataField="NC4H6" HeaderText="C4H6" SortField="NC4H6" />
                                                        <f:BoundField MinWidth="40px" ColumnID="NCH3Br" DataField="NCH3Br" HeaderText="CH3Br" SortField="NCH3Br" />
                                                        <f:BoundField MinWidth="40px" ColumnID="Csums" DataField="Csums" HeaderText="总计" SortField="Csums" />
                                                    </Columns>
                                                </f:GroupField>
                                                
                                                <f:BoundField MinWidth="40px" ColumnID="elect" DataField="elect" HeaderText="电" SortField="elect" />
                                                <f:BoundField MinWidth="40px" ColumnID="water" DataField="water" HeaderText="水" SortField="water" />
                                                <f:BoundField MinWidth="40px" ColumnID="workprice" DataField="workprice" HeaderText="人工制造成本" SortField="workprice" />
                                                <f:BoundField MinWidth="40px" ColumnID="makecost" DataField="makecost" HeaderText="制造成本" SortField="makecost" />
                                                <f:BoundField MinWidth="40px" ColumnID="totalCost" DataField="totalCost" HeaderText="总成本" SortField="totalCost" />
                                                <f:BoundField MinWidth="40px" ColumnID="unitCost" DataField="unitCost" HeaderText="单位成本" SortField="unitCost" />

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
