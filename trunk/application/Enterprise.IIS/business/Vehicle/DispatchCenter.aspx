<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DispatchCenter.aspx.cs" Inherits="Enterprise.IIS.business.Vehicle.DispatchCenter" %>

<%@ Import Namespace="Enterprise.Framework.Enum" %>
<%@ Import Namespace="Enterprise.IIS.Common" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>调度控制</title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .x-grid-row-summary .x-grid-cell-inner {
            font-weight: bold;
            color: red;
        }

        .x-grid-row-summary .x-grid-cell {
            background-color: #fff !important;
        }

        #RegionPanel1_Region1_RegionPanel2_Region4_Grid1_ctl04-titleEl {
            text-align: center;
        }

        #RegionPanel1_Region1_RegionPanel2_Region4_Grid1_ctl05-titleEl {
            text-align: center;
        }

        #RegionPanel1_Region1_RegionPanel2_Region4_Grid1_ctl06-titleEl {
            text-align: center;
        }

        #RegionPanel1_Region1_RegionPanel2_Region4_Grid1_ctl07-titleEl {
            text-align: center;
        }

        #RegionPanel1_Region1_RegionPanel2_Region4_Grid1_ctl08-titleEl {
            text-align: center;
        }

        #RegionPanel1_Region1_RegionPanel2_Region4_Grid1_ctl14-titleEl {
            text-align: center;
        }

        #RegionPanel1_Region1_header {
            background-color: #dfeaf2;
        }

        #RegionPanel1_Region1_Toolbar1_headerText-inputEl {
            color: WindowText;
            font-size: 16px;
            font-weight: bold;
        }

        #RegionPanel1_Region2_Toolbar2_Label1-inputEl {
            color: WindowText;
            font-size: 16px;
            font-weight: bold;
        }

        #RegionPanel1_Region1_RegionPanel2_Region5_header {
            background-color: #dfeaf2;
        }

        #RegionPanel1_Region1_RegionPanel2_Region5_header_hd-textEl {
            color: WindowText;
            font-size: 16px;
            font-weight: bold;
        }

        #RegionPanel1_Region2_header {
            background-color: #dfeaf2;
        }

        #RegionPanel1_Region2_header_hd-textEl {
            color: WindowText;
            font-size: 16px;
            font-weight: bold;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
        <f:Timer runat="server" ID="Timer1" OnTick="Timer1_OnTickOnTick" Interval="20000" />

        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region1" Title="调度控制" Position="Center"
                    ShowBorder="False" BoxConfigAlign="Stretch" Layout="Fit" runat="server" ShowHeader="False">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <f:Label runat="server" ID="headerText" Text="车辆调度" />
                                <f:ToolbarSeparator ID="ToolbarSeparator2" runat="server" />
                                <f:Button ID="btnSearch" OnClick="btnSearch_Click" runat="server" Text="查询" Icon="Zoom" Hidden="False" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnDispatch" OnClick="btnDispatch_Click" runat="server" Hidden="False" Text="生成调度单"
                                    Icon="PageWhiteEdit" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnReturn" OnClick="btnReturn_Click" runat="server" Hidden="False" Text="修改"
                                    Icon="ScriptEdit" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnResase" runat="server" Text="重置"
                                    Icon="ArrowRefresh" Hidden="False" Size="Medium" OnClick="btnResase_Click">
                                </f:Button>
                                <f:Button ID="btnExport" OnClick="btnExport_Click" runat="server" Text="引出" Icon="PageExcel"
                                    Hidden="False" EnableAjax="False" DisableControlBeforePostBack="False" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnClose" EnablePostBack="False" Text="关闭" OnClientClick="closeActiveTab();" runat="server" Icon="SystemClose" Size="Medium">
                                </f:Button>
                            </Items>
                        </f:Toolbar>
                        
                        <f:Toolbar ID="Toolbar3" runat="server">
                            <Items>
                                <f:Button ID="btnTubePrint" runat="server" Text="排管车出门单" OnClick="btnTubePrint_Click"
                                    Icon="Printer" Hidden="False" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnPrint" runat="server" Text="货车出门单" OnClick="btnPrint_Click"
                                    Icon="Printer" Hidden="False" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnPrintDispatch" runat="server" Text="货车运输记录单" OnClick="btnPrintDispatch_Click"
                                    Icon="Printer" Hidden="False" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnAddTask" runat="server" Text="追加任务单" OnClick="btnAddTask_Click"
                                    Icon="TableAdd" Hidden="False" Size="Medium">
                                </f:Button>
                                
                                <f:Button ID="btnBatchDelete" OnClick="btnBatchDelete_Click" runat="server" Text="作废调度单"
                                    Icon="Delete" Size="Medium" Hidden="True">
                                </f:Button>
                            </Items>
                        </f:Toolbar>
                        
                    </Toolbars>
                    <Items>
                        <f:RegionPanel ID="RegionPanel2" ShowBorder="false" runat="server">
                            <Regions>
                                <f:Region ID="Region3" ShowBorder="false" ShowHeader="false" Split="False"
                                    Position="Top" Layout="Fit"
                                    BodyPadding="3px" runat="server" Height="70px" EnableCollapse="True">
                                    <Items>
                                        <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="80px"
                                            runat="server" BodyPadding="5px" LabelAlign="Right">
                                            <Rows>
                                                <f:FormRow ID="FormRow1">
                                                    <Items>
                                                        <f:DatePicker ID="dateBegin" runat="server" Label="开始日期">
                                                        </f:DatePicker>
                                                        <f:DatePicker ID="dateEnd" runat="server" Label="结束日期">
                                                        </f:DatePicker>
                                                        <f:DropDownList runat="server" ID="ddlFDistributionPoint" Label="作业区"
                                                            Required="True" LabelAlign="Right" ShowRedStar="True" EnableEdit="True"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddlFDistributionPoint_SelectedIndexChanged" />
                                                    </Items>
                                                </f:FormRow>
                                                <f:FormRow ID="FormRow2">
                                                    <Items>
                                                        <f:RadioButtonList ID="rbtList" Label="配车状态" runat="server"
                                                            AutoPostBack="true" OnSelectedIndexChanged="rbtList_SelectedIndexChanged">
                                                            <f:RadioItem Text="全部" Value="全部" />
                                                            <f:RadioItem Text="未配车" Value="未配车" Selected="true" />
                                                            <f:RadioItem Text="已配车" Value="已配车" />
                                                        </f:RadioButtonList>
                                                        <f:DropDownList runat="server" ID="ddlDeliveryMethod" Label="配送方式"
                                                            Required="True" LabelAlign="Right" ShowRedStar="True" />
                                                        <f:TextBox ID="txtFKeyId" runat="server" Label="预调度编号">
                                                        </f:TextBox>
                                                    </Items>
                                                </f:FormRow>
                                            </Rows>
                                        </f:Form>
                                    </Items>
                                </f:Region>
                                <f:Region ID="Region4" ShowBorder="False" ShowHeader="false" Position="Center"
                                    Layout="Fit" runat="server" BodyPadding="8px" AutoScroll="True" MinHeight="220px">
                                    <Items>
                                        <f:Grid ID="Grid1" PageSize="2000" ShowBorder="false" ShowHeader="false" AllowPaging="False"
                                            runat="server" EnableCheckBoxSelect="True" DataKeyNames="KeyId,FInitDispatch"
                                            IsDatabasePaging="true" OnSort="Grid1_Sort" SortDirection="DESC" OnRowCommand="Grid1_RowCommand"
                                            AllowSorting="true" EmptyText="查询无结果" EnableHeaderMenu="True" EnableMultiSelect="True" KeepCurrentSelection="True"
                                            EnableRowClickEvent="true" OnRowClick="Grid1_RowClick">
                                            <Columns>
                                                <f:BoundField MinWidth="60px" DataField="FDeliveryMethod" HeaderText="配送方式" SortField="FDeliveryMethod" />
                                                <f:BoundField MinWidth="126px" DataField="KeyId" HeaderText="单据号" SortField="KeyId" TextAlign="Center" runat="server" />
                                                <f:BoundField MinWidth="126px" DataField="FDispatchNum" HeaderText="调度号" SortField="FDispatchNum" TextAlign="Center" runat="server" />
                                                <f:BoundField MinWidth="126px" DataField="FInitDispatch" HeaderText="预调度号" SortField="FInitDispatch" TextAlign="Center" runat="server" />
                                                <f:BoundField MinWidth="80px" DataField="FDate" HeaderText="日期" SortField="FDate" DataFormatString="{0:yyyy-MM-dd}" TextAlign="Center" />
                                                <f:BoundField MinWidth="126px" DataField="FDistributionPoint" HeaderText="作业区" SortField="FDistributionPoint" runat="server" />
                                                <f:BoundField MinWidth="80px" DataField="FFullloadFlag" HeaderText="满载审核结果" SortField="FFullloadFlag" runat="server" />
                                                <f:BoundField MinWidth="80px" DataField="FFullloadRatio" HeaderText="满载率" SortField="FFullloadRatio" runat="server" />
                                                <f:BoundField MinWidth="220px" DataField="FName" HeaderText="客户名称" SortField="FName" />
                                                <f:BoundField MinWidth="260px" DataField="FAddress" HeaderText="地址" SortField="FAddress" />
                                                <f:BoundField MinWidth="80px" DataField="FQty" HeaderText="配送量" SortField="FQty" TextAlign="Right" />
                                                <f:BoundField MinWidth="120px" DataField="FVehicleNum" HeaderText="车牌号" SortField="FVehicleNum" TextAlign="Center" />
                                                <f:BoundField MinWidth="120px" DataField="FDriver" HeaderText="送货司机" SortField="FDriver" />
                                                <f:BoundField MinWidth="120px" DataField="FSupercargo" HeaderText="押运员" SortField="FSupercargo" />
                                                <f:BoundField MinWidth="120px" DataField="FMemo" HeaderText="备注" SortField="FMemo" />
                                                <f:BoundField MinWidth="120px" DataField="FLinkman" HeaderText="联系人" SortField="FLinkman" />
                                                <f:BoundField MinWidth="80px" DataField="FPhone" HeaderText="电话" SortField="FPhone" />
                                            </Columns>
                                        </f:Grid>
                                    </Items>
                                </f:Region>
                                <f:Region ID="Region5" ShowBorder="False" ShowHeader="False" Position="Bottom"
                                    Layout="Fit" MinHeight="120px" MaxHeight="160px" runat="server" BodyPadding="8px" AutoScroll="True" Title="调度单">
                                    <Items>
                                        <f:Grid ID="Grid3" ShowBorder="false" ShowHeader="false" AllowPaging="false"
                                            runat="server" DataKeyNames="KeyId" EnableCheckBoxSelect="True" EnableMultiSelect="False"
                                            IsDatabasePaging="true" OnSort="Grid1_Sort" SortDirection="DESC"
                                            AllowSorting="False" EmptyText="查询无结果" EnableHeaderMenu="True">
                                            <Columns>
                                                <f:TemplateField Width="100px" ColumnID="FFlag" HeaderText="作废标识" TextAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFFlag" runat="server" Text='<%# EnumDescription.GetFieldText((GasBillFlag) Eval("FFlag")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                </f:TemplateField>
                                                <f:BoundField MinWidth="120px" DataField="KeyId" HeaderText="调度号" SortField="KeyId" TextAlign="Center" runat="server" />
                                                <f:BoundField MinWidth="120px" DataField="FLogistics" HeaderText="物流公司" SortField="FLogistics" TextAlign="Center" runat="server" />
                                                <f:BoundField MinWidth="80px" DataField="FDate" HeaderText="调度日期" SortField="FDate" DataFormatString="{0:yyyy-MM-dd}" TextAlign="Center" />
                                                <f:BoundField MinWidth="100px" DataField="FBeginDate" HeaderText="发车日期" SortField="FBeginDate" DataFormatString="{0:yyyy-MM-dd}" />
                                                <f:BoundField MinWidth="60px" DataField="FBeginTime" HeaderText="发车时间" SortField="FBeginTime" DataFormatString="{0:hh-mm}" />
                                                <f:BoundField MinWidth="120px" DataField="FVehicleNum" HeaderText="车牌号" SortField="FVehicleNum" TextAlign="Center" />
                                                <f:BoundField MinWidth="120px" DataField="FDriver" HeaderText="送货司机" SortField="FDriver" />
                                                <f:BoundField MinWidth="120px" DataField="FSupercargo" HeaderText="押运员" SortField="FSupercargo" />
                                                <f:BoundField MinWidth="120px" DataField="FEndDate" HeaderText="到厂日期" SortField="FEndDate" DataFormatString="{0:yyyy-MM-dd}" />
                                                <f:BoundField MinWidth="60px" DataField="FEndTime" HeaderText="到厂时间" SortField="FEndTime" DataFormatString="{0:hh-mm}" />
                                            </Columns>
                                        </f:Grid>
                                    </Items>
                                </f:Region>
                            </Regions>
                        </f:RegionPanel>
                    </Items>
                </f:Region>

                <f:Region ID="Region2" ShowHeader="False" ShowBorder="False" Split="True" Title="装车明细" Icon="ApplicationViewDetail"
                    Width="260px" Position="Right" Layout="Fit" runat="server" EnableCollapse="True">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar2" runat="server" ToolbarAlign="Left">
                            <Items>
                                <f:Label runat="server" ID="Label1" Text="装车明细" />
                                <%--<f:Button ID="Button1" EnablePostBack="False"  Text="关闭" OnClientClick="closeActiveTab();" runat="server" Icon="SystemClose" Size="Medium">
                                </f:Button>--%>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Items>
                        <f:Grid ID="Grid2" PageSize="20" ShowBorder="False" ShowHeader="False" AllowPaging="False" Title="装车明细"
                            runat="server" EnableCheckBoxSelect="False" DataKeyNames="FId"
                            IsDatabasePaging="False" OnSort="Grid1_Sort" SortDirection="DESC"
                            AllowSorting="False" EmptyText="查询无结果" EnableHeaderMenu="True"
                            EnableSummary="true" SummaryPosition="Flow">
                            <Columns>
                                <f:BoundField MinWidth="80px" ColumnID="FName" DataField="FName" HeaderText="品名" SortField="FName" runat="server" />
                                <f:BoundField MinWidth="80px" ColumnID="FQty" DataField="FQty" HeaderText="数量" SortField="FQty" runat="server" />
                                <f:BoundField MinWidth="80px" DataField="FSpec" HeaderText="规格" SortField="FSpec" runat="server" />
                                <f:BoundField MinWidth="80px" DataField="FUnit" HeaderText="单位" SortField="FUnit" runat="server" />
                                <f:BoundField MinWidth="120px" DataField="FMemo" HeaderText="备注" SortField="FMemo" />

                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>
        <br />
        <f:Window ID="Window1" runat="server" WindowPosition="Center" IsModal="True" EnableClose="True"
            Icon="ApplicationViewDetail" EnableMaximize="True" EnableResize="True" Hidden="True"
            Target="Parent" EnableIFrame="True" IFrameUrl="about:blank" Height="680px" Width="960px"
            OnClose="Window1_Close">
        </f:Window>

        <f:Window ID="Window2" Icon="PageAttach" runat="server" Hidden="true"
            IsModal="true" Target="Parent" EnableMaximize="true" EnableResize="true" OnClose="Window2_Close"
            Title="Popup Window 2" CloseAction="HidePostBack"
            EnableIFrame="true" Height="450px" Width="650px">
        </f:Window>

    </form>
    <object id="LODOP_OB" classid="clsid:2105C259-1E0C-4534-8141-A753534CB4CA" width="0" height="0">
        <embed id="LODOP_EM" type="application/x-print-lodop" width="0" height="0"></embed>
    </object>
    <script src="../../jqueryui/js/jquery-1.8.3.min.js" type="text/javascript"> </script>
    <script src="../../jqueryui/js/jquery-ui-1.9.2.custom.js?v=4.0" type="text/javascript"> </script>
    <script type="text/javascript" language="javascript" src="../../js/LodopFuncs.js"></script>
    <script type="text/javascript">
        var basePath = '<%= ResolveUrl("~/") %>';
        function closeActiveTab() {
            parent.removeActiveTab();
        }
        //--------打印配置---------------
        var LODOP; //声明为全局变量 
        function LodopPrinter(keyid) {
            LODOP = getLodop();
            LODOP.PRINT_INIT("河南禄恒软件科技有限公司");
            var strHtml = "";
            $.post("../../common/AjaxPrinter.ashx?oper=ajaxPrintVehicle",
                { "keyid": keyid },
                function (result) {
                    if (result != "") {

                        LODOP.ADD_PRINT_HTM(1, 1, "100%", "100%", result);
                        LODOP.PREVIEW();
                    }
                    else {
                        alert("打印失败！");
                    }
                }, "html");
        };
        function LodopTubePrinter(keyid) {
            LODOP = getLodop();
            LODOP.PRINT_INIT("河南禄恒软件科技有限公司");
            var strHtml = "";
            $.post("../../common/AjaxPrinter.ashx?oper=ajaxPrintTubeVehicle",
                { "keyid": keyid },
                function (result) {
                    if (result != "") {

                        LODOP.ADD_PRINT_HTM(1, 1, "100%", "100%", result);
                        LODOP.PREVIEW();
                    }
                    else {
                        alert("打印失败！");
                    }
                }, "html");
        };
        function LodopPrinterDispatch(keyid) {
            LODOP = getLodop();
            LODOP.PRINT_INIT("河南禄恒软件科技有限公司");
            var strHtml = "";
            $.post("../../common/AjaxPrinter.ashx?oper=ajaxPrintDispatch",
                { "keyid": keyid },
                function (result) {
                    if (result != "") {

                        LODOP.ADD_PRINT_HTM(1, 1, "100%", "100%", result);
                        LODOP.PREVIEW();
                    }
                    else {
                        alert("打印失败！");
                    }
                }, "html");
        };
    </script>
</body>
</html>
