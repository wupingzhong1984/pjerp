<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SalaryDriver.aspx.cs" Inherits="Enterprise.IIS.business.Reports.SalaryDriver" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>司机计件</title>
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
                <f:Region ID="Region1" Title="司机计件" Position="Center"
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
                                <f:Button ID="btnEdit" OnClick="btnReturn_Click" runat="server" Hidden="true" Text="修改"
                                    Icon="ScriptEdit" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnExport" OnClick="btnExport_Click" runat="server" Text="引出明细" Icon="PageExcel"
                                    Hidden="true" EnableAjax="false" DisableControlBeforePostBack="false" Size="Medium">
                                </f:Button>
                                <f:Button ID="Button2" OnClick="btnExports_Click" runat="server" Text="引出金额" Icon="PageExcel"
                                    Hidden="False" EnableAjax="false" DisableControlBeforePostBack="false" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnshowdetail" OnClick="btnshowdetail_Click" runat="server" Text="查看明细" Icon="PageExcel"
                                    Hidden="False" EnableAjax="false" DisableControlBeforePostBack="false" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnshowcash" OnClick="btnshowcash_Click" runat="server" Text="查看工资" Icon="PageExcel"
                                    Hidden="true" EnableAjax="false" DisableControlBeforePostBack="false" Size="Medium">
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
                                        <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="86px"
                                            runat="server" BodyPadding="5px" LabelAlign="Right">
                                            <Rows>
                                                <f:FormRow ID="FormRow1">
                                                    <Items>
                                                        <f:DropDownList runat="server" ID="ddlCompany" Label="所属公司" LabelAlign="Right"></f:DropDownList>
                                                        <f:DatePicker ID="dpkFDateBegin" runat="server" Label="开始时间" DateFormatString="yyyy-MM-dd" LabelAlign="Right">
                                                        </f:DatePicker>
                                                        <f:DatePicker ID="dpkFDateEnd" runat="server" Label="结束时间" DateFormatString="yyyy-MM-dd" LabelAlign="Right">
                                                        </f:DatePicker>
                                                        <f:DropDownList ID="ddllx" runat="server" Label="车辆类型" Required="true" ShowRedStar="True">
                                                        </f:DropDownList>

                                                        <%--<f:DropDownList runat="server" Hidden="true" ID="ddlFDriver" Label="送货司机" EnableEdit="True" EnableMultiSelect="true" LabelAlign="Right" />
                                                        <f:DropDownList runat="server" Hidden="true" ID="ddlFSupercargo" Label="押运员" EnableEdit="True" EnableMultiSelect="true" LabelAlign="Right" />--%>
                                                    </Items>
                                                </f:FormRow>

                                            </Rows>
                                        </f:Form>
                                    </Items>
                                </f:Region>
                                <f:Region ID="Region4" ShowBorder="False" ShowHeader="False" Position="Center"
                                    Layout="Fit" runat="server" BodyPadding="3px" AutoScroll="True" Hidden="true">
                                    <Items>
                                        <f:Grid ID="Grid1" ShowBorder="False" ShowHeader="False" AllowPaging="False"
                                            runat="server" EnableCheckBoxSelect="true" DataKeyNames="KeyId"
                                            OnRowCommand="Grid1_RowCommand"
                                            IsDatabasePaging="False" OnSort="Grid1_Sort" SortDirection="ASC"
                                            AllowSorting="true" EmptyText="查询无结果" EnableHeaderMenu="True"
                                            EnableSummary="true" SummaryPosition="Bottom">
                                            <Columns>
                                                <f:BoundField MinWidth="130px" ColumnID="KeyId" DataField="KeyId" HeaderText="单据号" SortField="KeyId" />
                                                <f:BoundField MinWidth="130px" ColumnID="FAuditFlag" DataField="FAuditFlag" HeaderText="审核" SortField="FAuditFlag" />
                                                <f:BoundField MinWidth="120px" DataField="FLogistics" HeaderText="物流" SortField="FLogistics" />
                                                <f:BoundField MinWidth="80px" ColumnID="FVehicleNum" DataField="FVehicleNum" HeaderText="运输车辆" SortField="FVehicleNum" />
                                                <f:BoundField MinWidth="80px" ColumnID="FDriver" DataField="FDriver" HeaderText="驾驶员" SortField="FDriver" />
                                                <f:BoundField MinWidth="80px" ColumnID="FSupercargo" DataField="FSupercargo" HeaderText="押运员" SortField="FSupercargo" />
                                                <f:BoundField MinWidth="80px" ColumnID="FBeginDate" DataField="FBeginDate" HeaderText="出发日期" DataFormatString="{0:yyyy-MM-dd}" SortField="FBeginDate" />
                                                <f:BoundField MinWidth="80px" DataField="FEndDate" HeaderText="回厂日期" SortField="FEndDate" DataFormatString="{0:yyyy-MM-dd}" TextAlign="Center" />
                                                <f:BoundField MinWidth="80px" ColumnID="FMileage" DataField="FMileage" HeaderText="公里数" SortField="FMileage" TextAlign="right" />
                                                <f:GroupField HeaderText="行程工资" TextAlign="Center" EnableLock="true">
                                                    <Columns>
                                                        <f:BoundField MinWidth="80px" ColumnID="FDriverMileageAmt" DataField="FDriverMileageAmt" HeaderText="司机" SortField="FDriverMileageAmt" TextAlign="right" />
                                                        <f:BoundField MinWidth="80px" ColumnID="FSupercargoMileageAmt" DataField="FSupercargoMileageAmt" HeaderText="押运" SortField="FSupercargoMileageAmt" TextAlign="right" />
                                                    </Columns>
                                                </f:GroupField>
                                                <f:GroupField HeaderText="过夜住宿" TextAlign="Center" EnableLock="true">
                                                    <Columns>
                                                        <f:BoundField MinWidth="80px" ColumnID="FDiffDay" DataField="FDiffDay" HeaderText="天数" SortField="FDiffDay" TextAlign="right" />
                                                        <f:BoundField MinWidth="80px" ColumnID="FDiffDayAmount" DataField="FDiffDayAmount" HeaderText="工资" SortField="FDiffDayAmount" TextAlign="right" />
                                                    </Columns>
                                                </f:GroupField>
                                                <f:GroupField HeaderText="任务" TextAlign="Center" EnableLock="true">
                                                    <Columns>
                                                        <f:BoundField MinWidth="80px" ColumnID="TaskQty" DataField="TaskQty" HeaderText="点数" SortField="TaskQty" TextAlign="right" />
                                                        <f:BoundField MinWidth="80px" ColumnID="FDriverTAmount" DataField="FDriverTAmount" HeaderText="司机工资" SortField="FDriverTAmount" TextAlign="right" />
                                                        <f:BoundField MinWidth="80px" ColumnID="FSupercargoTAmount" DataField="FSupercargoTAmount" HeaderText="押运工资" SortField="FSupercargoTAmount" TextAlign="right" />
                                                    </Columns>
                                                </f:GroupField>


                                                <f:GroupField HeaderText="Y瓶（司机）" TextAlign="Center" EnableLock="true">
                                                    <Columns>
                                                        <f:BoundField MinWidth="80px" ColumnID="FDriverYQty" DataField="FDriverYQty" HeaderText="数量" SortField="FDriverYQty" TextAlign="right" />
                                                        <f:BoundField MinWidth="80px" ColumnID="FDriverYPrice" DataField="FDriverYPrice" HeaderText="单价" SortField="FDriverYPrice" TextAlign="right" />
                                                        <f:BoundField MinWidth="80px" ColumnID="FDriverYAmount" DataField="FDriverYAmount" HeaderText="工资" SortField="FDriverYAmount" TextAlign="right" />
                                                    </Columns>
                                                </f:GroupField>
                                                <f:GroupField HeaderText="散瓶（司机）" TextAlign="Center" EnableLock="true">
                                                    <Columns>
                                                        <f:BoundField MinWidth="80px" ColumnID="FDriverQty" DataField="FDriverQty" HeaderText="数量" SortField="FDriverQty" TextAlign="right" />
                                                        <f:BoundField MinWidth="80px" ColumnID="FDriverPrice" DataField="FDriverPrice" HeaderText="单价" SortField="FDriverPrice" TextAlign="right" />
                                                        <f:BoundField MinWidth="80px" ColumnID="FDriverAmount" DataField="FDriverAmount" HeaderText="工资" SortField="FDriverAmount" TextAlign="right" />
                                                    </Columns>
                                                </f:GroupField>
                                                <f:GroupField HeaderText="散瓶（司机）" TextAlign="Center" EnableLock="true">
                                                    <Columns>
                                                        <f:BoundField MinWidth="80px" ColumnID="FDriverJQty" DataField="FDriverJQty" HeaderText="数量" SortField="FDriverJQty" TextAlign="right" />
                                                        <f:BoundField MinWidth="80px" ColumnID="FDriverJPrice" DataField="FDriverJPrice" HeaderText="单价" SortField="FDriverJPrice" TextAlign="right" />
                                                        <f:BoundField MinWidth="80px" ColumnID="FDriverJAmount" DataField="FDriverJAmount" HeaderText="工资" SortField="FDriverJAmount" TextAlign="right" />
                                                    </Columns>
                                                </f:GroupField>


                                                <f:GroupField HeaderText="Y瓶（押运）" TextAlign="Center" EnableLock="true">
                                                    <Columns>
                                                        <f:BoundField MinWidth="80px" ColumnID="FSupercargoYQty" DataField="FSupercargoYQty" HeaderText="数量" SortField="FSupercargoYQty" TextAlign="right" />
                                                        <f:BoundField MinWidth="80px" ColumnID="FSupercargoYPrice" DataField="FSupercargoYPrice" HeaderText="单价" SortField="FSupercargoYPrice" TextAlign="right" />
                                                        <f:BoundField MinWidth="80px" ColumnID="FSupercargoYAmount" DataField="FSupercargoYAmount" HeaderText="工资" SortField="FSupercargoYAmount" TextAlign="right" />
                                                    </Columns>
                                                </f:GroupField>
                                                <f:GroupField HeaderText="散瓶（押运）" TextAlign="Center" EnableLock="true">
                                                    <Columns>
                                                        <f:BoundField MinWidth="80px" ColumnID="FSupercargoQty" DataField="FSupercargoQty" HeaderText="数量" SortField="FSupercargoQty" TextAlign="right" />
                                                        <f:BoundField MinWidth="80px" ColumnID="FSupercargoPrice" DataField="FSupercargoPrice" HeaderText="单价" SortField="FSupercargoPrice" TextAlign="right" />
                                                        <f:BoundField MinWidth="80px" ColumnID="FSupercargoAmount" DataField="FSupercargoAmount" HeaderText="工资" SortField="FSupercargoAmount" TextAlign="right" />
                                                    </Columns>
                                                </f:GroupField>
                                                <f:GroupField HeaderText="散瓶（押运）" TextAlign="Center" EnableLock="true">
                                                    <Columns>
                                                        <f:BoundField MinWidth="80px" ColumnID="FSupercargoJQty" DataField="FSupercargoJQty" HeaderText="数量" SortField="FSupercargoJQty" TextAlign="right" />
                                                        <f:BoundField MinWidth="80px" ColumnID="FSupercargoJPrice" DataField="FSupercargoJPrice" HeaderText="单价" SortField="FSupercargoJPrice" TextAlign="right" />
                                                        <f:BoundField MinWidth="80px" ColumnID="FSupercargoJAmount" DataField="FSupercargoJAmount" HeaderText="工资" SortField="FSupercargoJAmount" TextAlign="right" />
                                                    </Columns>
                                                </f:GroupField>

                                                <f:BoundField MinWidth="200px" ColumnID="FMemo" DataField="FMemo" HeaderText="备注" SortField="FMemo" />


                                            </Columns>
                                        </f:Grid>
                                    </Items>
                                </f:Region>

                                <f:Region ID="Region2" ShowBorder="False" ShowHeader="False" Position="Center"
                                    Layout="Fit" runat="server" BodyPadding="3px" AutoScroll="True">
                                    <Items>
                                        <f:Grid ID="Grid2" ShowBorder="False" ShowHeader="False" AllowPaging="False"
                                            runat="server" EnableCheckBoxSelect="true" DataKeyNames="KeyId"
                                            OnRowCommand="Grid1_RowCommand"
                                            IsDatabasePaging="False" OnSort="Grid1_Sort" SortDirection="ASC"
                                            AllowSorting="true" EmptyText="查询无结果" EnableHeaderMenu="True"
                                            EnableSummary="true" SummaryPosition="Bottom">
                                            <Columns>
                                                <f:BoundField MinWidth="80px" ColumnID="drivertype" DataField="drivertype" HeaderText="计费科目" SortField="drivertype" />
                                                <f:BoundField MinWidth="80px" ColumnID="driver" DataField="driver" HeaderText="驾驶员" SortField="driver" />
                                                <f:BoundField MinWidth="80px" DataField="time" HeaderText="回厂日期" SortField="time" DataFormatString="{0:yyyy-MM-dd}" TextAlign="Center" />
                                                        <f:BoundField MinWidth="80px" ColumnID="Mileage" DataField="Mileage" HeaderText="公里数" SortField="Mileage" TextAlign="right" />
                                                        <f:BoundField MinWidth="80px" ColumnID="morethanthree" DataField="morethanthree" HeaderText="300以上公里数" SortField="morethanthree" TextAlign="right" />
                                                <f:GroupField HeaderText="行程工资" TextAlign="Center" EnableLock="true">
                                                    <Columns>
                                                        <f:BoundField MinWidth="80px" ColumnID="milageprice" DataField="milageprice" HeaderText="总公里工资" SortField="milageprice" TextAlign="right" />
                                                        <f:BoundField MinWidth="80px" ColumnID="threeprice" DataField="threeprice" HeaderText="300以上工资" SortField="threeprice" TextAlign="right" />
                                                    </Columns>
                                                </f:GroupField>
                                                <f:GroupField HeaderText="过夜住宿" TextAlign="Center" EnableLock="true">
                                                    <Columns>
                                                        <f:BoundField MinWidth="80px" ColumnID="FDiffDay" DataField="FDiffDay" HeaderText="天数" SortField="FDiffDay" TextAlign="right" />
                                                        <f:BoundField MinWidth="80px" ColumnID="FDiffDayAmount" DataField="FDiffDayAmount" HeaderText="工资" SortField="FDiffDayAmount" TextAlign="right" />
                                                    </Columns>
                                                </f:GroupField>
                                                <f:GroupField HeaderText="任务" TextAlign="Center" EnableLock="true">
                                                    <Columns>
                                                        <f:BoundField MinWidth="80px" ColumnID="UnloadingPoint" DataField="UnloadingPoint" HeaderText="点数" SortField="UnloadingPoint" TextAlign="right" />
                                                        <f:BoundField MinWidth="80px" ColumnID="pointprice" DataField="pointprice" HeaderText="点数工资" SortField="pointprice" TextAlign="right" />
                                                    </Columns>
                                                </f:GroupField>


                                                <f:GroupField HeaderText="Y瓶" TextAlign="Center" EnableLock="true">
                                                    <Columns>
                                                        <f:BoundField MinWidth="80px" ColumnID="Yout" DataField="Yout" HeaderText="数量" SortField="Yout" TextAlign="right" />
                                                        <f:BoundField MinWidth="80px" ColumnID="Yunit" DataField="Yunit" HeaderText="单价" SortField="Yunit" TextAlign="right" />
                                                        <f:BoundField MinWidth="80px" ColumnID="Yprice" DataField="Yprice" HeaderText="工资" SortField="Yprice" TextAlign="right" />
                                                    </Columns>
                                                </f:GroupField>
                                                <f:GroupField HeaderText="散瓶" TextAlign="Center" EnableLock="true">
                                                    <Columns>
                                                        <f:BoundField MinWidth="80px" ColumnID="Sout" DataField="Sout" HeaderText="数量" SortField="Sout" TextAlign="right" />
                                                        <f:BoundField MinWidth="80px" ColumnID="Sunit" DataField="Sunit" HeaderText="单价" SortField="Sunit" TextAlign="right" />
                                                        <f:BoundField MinWidth="80px" ColumnID="Sprice" DataField="Sprice" HeaderText="工资" SortField="Sprice" TextAlign="right" />
                                                    </Columns>
                                                </f:GroupField>
                                                <f:GroupField HeaderText="集格" TextAlign="Center" EnableLock="true">
                                                    <Columns>
                                                        <f:BoundField MinWidth="80px" ColumnID="Jgout" DataField="Jgout" HeaderText="数量" SortField="Jgout" TextAlign="right" />
                                                        <f:BoundField MinWidth="80px" ColumnID="jgunit" DataField="jgunit" HeaderText="单价" SortField="jgunit" TextAlign="right" />
                                                        <f:BoundField MinWidth="80px" ColumnID="JgPrice" DataField="JgPrice" HeaderText="工资" SortField="JgPrice" TextAlign="right" />
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
        function closeActiveTab() {
            parent.removeActiveTab();
        };
        F.ready(function () {

        });
    </script>
</body>
</html>
