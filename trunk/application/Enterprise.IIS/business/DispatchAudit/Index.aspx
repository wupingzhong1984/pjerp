<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Enterprise.IIS.business.DispatchAudit.Index" %>


<%@ Import Namespace="Enterprise.Framework.Enum" %>
<%@ Import Namespace="Enterprise.IIS.Common" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>调度审核</title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .colorred {
            color: red;
        }

        .colorgreen {
            color: green;
        }

        .colorcoral {
            color: darkorchid;
        }

        .colorblueviolet {
            color: blueviolet;
        }


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
                <f:Region ID="Region1" Title="调度审核" Position="Center"
                    ShowBorder="False" BoxConfigAlign="Stretch" Layout="Fit" runat="server" ShowHeader="False">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <f:Label runat="server" ID="headerText" Text="调度审核" />
                                <f:ToolbarSeparator ID="ToolbarSeparator2" runat="server" />
                                <f:Button ID="btnSearch" OnClick="btnSearch_Click" runat="server" Text="查询" Icon="Zoom" Hidden="False" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnAuditYes" OnClick="btnAuditYes_Click" runat="server" Hidden="False" Text="通过"
                                    Icon="PackageStart" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnAuditNo" OnClick="btnAuditNo_Click" runat="server" Hidden="False" Text="不通过"
                                    Icon="PackageStop" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnReturn" OnClick="btnReturn_Click" runat="server" Hidden="False" Text="编辑出车单"
                                    Icon="ScriptEdit" Size="Medium">
                                </f:Button>
                                <f:Button ID="Button2" EnablePostBack="False" Text="关闭" OnClientClick="closeActiveTab();" runat="server" Icon="SystemClose" Size="Medium">
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
                                                        <f:DropDownList runat="server" ID="ddlLogistics" Label="物流公司"
                                                            OnSelectedIndexChanged="ddlFLogistics_SelectedIndexChanged"
                                                            AutoPostBack="true" LabelAlign="Right" />
                                                        <f:DropDownList runat="server" ID="ddlFVehicleNum" Label="车牌号" EnableEdit="True" LabelAlign="Right" />

                                                    </Items>
                                                </f:FormRow>
                                                <f:FormRow ID="FormRow2">
                                                    <Items>
                                                        <f:DropDownList runat="server" ID="ddlFAuditFlag" Label="审核状态"
                                                            LabelAlign="Right" EnableEdit="true">
                                                            <f:ListItem Text="全部" Value="-1" />
                                                            <f:ListItem Text="未审核" Value="0" />
                                                            <f:ListItem Text="审核通过" Value="1" />
                                                            <f:ListItem Text="审核不通过" Value="2" />
                                                        </f:DropDownList>
                                                        <f:DropDownList runat="server" ID="ddlDeliveryMethod" Label="配送方式"
                                                            LabelAlign="Right" EnableEdit="true" />
                                                        <f:TextBox ID="txtFKeyId" runat="server" Label="调度编号">
                                                        </f:TextBox>

                                                        <f:Label runat="server" Hidden="True" />
                                                    </Items>
                                                </f:FormRow>
                                            </Rows>
                                        </f:Form>
                                    </Items>
                                </f:Region>
                                <f:Region ID="Region4" ShowBorder="False" ShowHeader="false" Position="Center"
                                    Layout="Fit" runat="server" BodyPadding="8px" AutoScroll="True" MinHeight="160px">
                                    <Items>
                                        <f:Grid ID="Grid3" ShowBorder="false" ShowHeader="false" AllowPaging="false"
                                            runat="server" DataKeyNames="KeyId,FAuditFlag" EnableCheckBoxSelect="True" EnableMultiSelect="False"
                                            IsDatabasePaging="true" OnSort="Grid1_Sort" SortDirection="DESC"
                                            AllowSorting="False" EmptyText="查询无结果" EnableHeaderMenu="False"
                                            EnableRowClickEvent="true" OnRowClick="Grid3_RowClick" OnRowDataBound="Grid3_RowDataBound">
                                            <Columns>
                                                <f:TemplateField Width="100px" ColumnID="FFlag" HeaderText="作废标识" TextAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFFlag" runat="server" Text='<%# EnumDescription.GetFieldText((GasBillFlag) Eval("FFlag")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                </f:TemplateField>
                                                <f:BoundField MinWidth="120px" DataField="FDeliveryMethod" HeaderText="配送方式" SortField="FDeliveryMethod" runat="server" />
                                                <f:BoundField MinWidth="120px" DataField="KeyId" HeaderText="调度号" SortField="KeyId" TextAlign="Center" runat="server" />
                                                <f:BoundField MinWidth="120px" DataField="FLogistics" HeaderText="物流公司" SortField="FLogistics" TextAlign="Center" runat="server" />
                                                <f:BoundField MinWidth="80px" DataField="FDate" HeaderText="调度日期" SortField="FDate" DataFormatString="{0:yyyy-MM-dd}" TextAlign="Center" />
                                                <f:BoundField MinWidth="100px" DataField="FBeginDate" HeaderText="发车日期" SortField="FBeginDate" DataFormatString="{0:yyyy-MM-dd}" />
                                                <f:BoundField MinWidth="60px" DataField="FBeginTime" HeaderText="发车时间" SortField="FBeginTime" DataFormatString="{0:hh-mm}" />
                                                <f:BoundField MinWidth="120px" DataField="FVehicleNum" HeaderText="车牌号" SortField="FVehicleNum" TextAlign="Center" />
                                                <f:BoundField MinWidth="120px" DataField="FDriver" HeaderText="送货司机" SortField="FDriver" />
                                                <f:BoundField MinWidth="120px" DataField="FSupercargo" HeaderText="押运员" SortField="FSupercargo" />
                                                <f:BoundField MinWidth="80px" DataField="FMileage" HeaderText="里程数" SortField="FMileage" />
                                                <f:BoundField MinWidth="120px" DataField="FEndDate" HeaderText="到厂日期" SortField="FEndDate" DataFormatString="{0:yyyy-MM-dd}" />
                                                <f:BoundField MinWidth="60px" DataField="FEndTime" HeaderText="到厂时间" SortField="FEndTime" DataFormatString="{0:hh-mm}" />
                                            </Columns>
                                        </f:Grid>
                                    </Items>
                                </f:Region>
                                <f:Region ID="Region5" ShowBorder="False" ShowHeader="False" Position="Bottom"
                                    Layout="Fit" MinHeight="120px" MaxHeight="360px" runat="server" BodyPadding="8px" AutoScroll="True" Title="调度单">
                                    <Items>
                                        <f:Grid ID="Grid1" PageSize="2000" ShowBorder="false" ShowHeader="false" AllowPaging="False"
                                            runat="server" EnableCheckBoxSelect="True" DataKeyNames="KeyId"
                                            IsDatabasePaging="true" OnSort="Grid1_Sort" SortDirection="DESC" OnRowCommand="Grid1_RowCommand"
                                            AllowSorting="true" EmptyText="查询无结果" EnableHeaderMenu="True" EnableMultiSelect="False" KeepCurrentSelection="True"
                                            EnableRowClickEvent="true" OnRowClick="Grid1_RowClick">
                                            <Columns>
                                                <f:BoundField MinWidth="126px" DataField="KeyId" HeaderText="单据号" SortField="KeyId" TextAlign="Center" runat="server" />
                                                <f:BoundField MinWidth="126px" DataField="FDispatchNum" HeaderText="调度号" SortField="FDispatchNum" TextAlign="Center" runat="server" />
                                                <f:BoundField MinWidth="80px" DataField="FDate" HeaderText="日期" SortField="FDate" DataFormatString="{0:yyyy-MM-dd}" TextAlign="Center" />
                                                <f:BoundField MinWidth="126px" DataField="FDistributionPoint" HeaderText="作业区" SortField="FDistributionPoint" runat="server" />
                                                <f:BoundField MinWidth="220px" DataField="FName" HeaderText="客户名称" SortField="FName" />
                                                <f:BoundField MinWidth="260px" DataField="FAddress" HeaderText="地址" SortField="FAddress" />
                                                <%--<f:BoundField MinWidth="80px" DataField="FQty" HeaderText="配送量" SortField="FQty" TextAlign="Right" />--%>
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
                            </Regions>
                        </f:RegionPanel>
                    </Items>
                </f:Region>

                <f:Region ID="Region2" ShowHeader="False" ShowBorder="False" Split="True" Icon="ApplicationViewDetail"
                    Width="260px" Position="Right" Layout="Fit" runat="server" EnableCollapse="True">
                    <Items>
                        <f:RegionPanel ID="RegionPanel3" ShowBorder="false" runat="server">
                            <Regions>
                                <f:Region ID="Region6" ShowHeader="False" ShowBorder="False" Title="装车明细" Icon="ApplicationViewDetail"
                                    Width="260px" Position="Top" Layout="Fit" runat="server" MinHeight="330px" MaxHeight="500px" AutoScroll="True">
                                    <Items>
                                        <f:Grid PageSize="20" ShowBorder="false" ShowHeader="true" AllowPaging="False"
                                            runat="server" DataKeyNames="KeyId"
                                            IsDatabasePaging="true" OnSort="Grid1_Sort" SortDirection="DESC"
                                            AllowSorting="true" EmptyText="查询无结果" EnableHeaderMenu="True" EnableMultiSelect="False" KeepCurrentSelection="True"
                                            EnableRowClickEvent="true" Title="装车明细" ID="Grid2">
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
                                <f:Region ID="Region7" ShowHeader="False" ShowBorder="False" Title="装车明细" Icon="ApplicationViewDetail"
                                    Width="260px" Position="Center" Layout="Fit" runat="server" MinHeight="330px" MaxHeight="500px" AutoScroll="True">
                                    <Items>
                                        <f:Grid PageSize="20" ShowBorder="false" ShowHeader="true" AllowPaging="False"
                                            runat="server" DataKeyNames="KeyId"
                                            IsDatabasePaging="true" OnSort="Grid1_Sort" SortDirection="DESC"
                                            AllowSorting="true" EmptyText="查询无结果" EnableHeaderMenu="True" EnableMultiSelect="False" KeepCurrentSelection="True"
                                            EnableRowClickEvent="true" Title="气瓶明细" ID="Grid4">
                                            <Columns>
                                                <f:BoundField MinWidth="80px" ColumnID="Fname" DataField="Fname" HeaderText="品名" SortField="Fname" runat="server" />
                                                <f:BoundField MinWidth="80px" DataField="FSpec" HeaderText="规格" SortField="FSpec" runat="server" />
                                                <f:BoundField MinWidth="80px" ColumnID="FBottleQty" DataField="FBottleQty" HeaderText="数量" SortField="FBottleQty" runat="server" />
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
