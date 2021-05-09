<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Details.aspx.cs" Inherits="Enterprise.IIS.business.AllotTrans.Details" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
        <f:HiddenField runat="server" ID="hfdSpec" />
        <f:HiddenField runat="server" ID="hfdImage" />

        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region1" Title="调拨出库单" Position="Center"
                    ShowBorder="false" BoxConfigAlign="Stretch" Layout="Fit" runat="server" ShowHeader="False">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <f:Button ID="btnPrint" runat="server" Text="打印" OnClick="btnPrint_Click"
                                    Icon="Printer" Hidden="False" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnClose" EnablePostBack="false" OnClientClick="closeActiveTab();" Text="关闭"
                                    runat="server" Icon="SystemClose" Size="Medium">
                                </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Items>
                        <f:RegionPanel ID="RegionPanel2" ShowBorder="false" runat="server">
                            <Regions>
                                <f:Region ID="Region3" ShowBorder="false" ShowHeader="True" Title="调拨出库单" Split="False"
                                    Position="Top" Layout="Fit"
                                    BodyPadding="3px" runat="server" Height="160px" EnableCollapse="False">
                                    <Items>
                                        <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="80px"
                                            runat="server">
                                            <Rows>
                                                <f:FormRow ID="FormRow0">
                                                    <Items>
                                                        <f:Label ID="txtKeyId" runat="server" Label="单据号"   LabelAlign="Right" >
                                                        </f:Label>
                                                        <f:Label ID="txtCreateBy" runat="server" Label="制单人" LabelAlign="Right">
                                                        </f:Label>
                                                        <f:Label ID="txtFDate" runat="server" Label="配送日期" LabelAlign="Right">
                                                        </f:Label>
                                                        <f:Label ID="tbxFLogisticsNumber"  Label="销售订单"  runat="server">
                                                        </f:Label>
                                                    </Items>
                                                </f:FormRow>
                                                <f:FormRow ID="FormRow1">
                                                    <Items>
                                                        <f:Label runat="server" Label="客户代码" ID="txtFCode"  LabelAlign="Right" Text="PU" />
                                                        <f:Label runat="server" Label="客户名称" ID="tbxFCustomer"  LabelAlign="Right" Text="PU" />
                                                       
                                                        <f:Label runat="server" ID="ddlFVehicleNum" Label="车牌号" LabelAlign="Right" />
                                                        <f:Label runat="server" ID="ddlFDriver" Label="送货司机"  LabelAlign="Right" />
                                                    </Items>
                                                </f:FormRow>
                                                <f:FormRow ID="FormRow2">
                                                    <Items>
                                                        <f:Label runat="server" ID="ddlFSupercargo" Label="押运员"  LabelAlign="Right" />
                                                        <f:Label runat="server"  ShowRedStar="True" ID="ddlFShipper" Label="发货人"  LabelAlign="Right" />
                                                        <f:Label runat="server" ID="ddlFSalesman" Label="业务员"  LabelAlign="Right" />
                                                        <f:Label runat="server" ID="ddlDeliveryMethod" Label="配送方式" 
                                                             LabelAlign="Right"  />
                                                    </Items>
                                                </f:FormRow>
                                                <f:FormRow ID="FormRow3">
                                                    <Items>
                                                        <f:Label runat="server" ID="ddlFDistributionPoint" Label="发货站点"  LabelAlign="Right"  />
                                                        <f:Label  runat="server" ID="ddlFPoint" Label="接收站点"  LabelAlign="Right" />
                                                        <f:Label  runat="server" ID="ddlFSendee" Label="收货人" LabelAlign="Right" />
                                                        <f:Label ID="txtFMemo" LabelAlign="Right" runat="server" Label="备注"  >
                                                        </f:Label>
                                                    </Items>
                                                </f:FormRow>
                                            </Rows>
                                        </f:Form>
                                    </Items>
                                </f:Region>

                                <f:Region ID="Region2" ShowBorder="False" ShowHeader="false" Position="Center"
                                    Layout="Fit" runat="server" BodyPadding="0px" AutoScroll="True">
                                    <Items>
                                        <f:TabStrip ID="TabStrip1" AutoPostBack="true" ShowBorder="false" ActiveTabIndex="0" runat="server">
                                            <Tabs>
                                                <f:Tab Title="调拨明细" BodyPadding="0px"
                                                    Layout="Fit" runat="server">
                                                    <Items>
                                                        <f:Grid ID="Grid1" PageSize="20"
                                                            ShowBorder="False" ShowHeader="False" AllowPaging="False"
                                                            runat="server"
                                                            EnableSummary="True"
                                                            EmptyText="查询无结果"
                                                            OnPageIndexChange="Grid1_PageIndexChange">
                                                            <Columns>
                                                                <f:BoundField MinWidth="130px" DataField="FItemCode" HeaderText="编码" SortField="FItemCode" />
                                                                <f:BoundField MinWidth="130px" DataField="FT6Warehouse" HeaderText="仓库" SortField="FT6Warehouse" />
                                                

                                                                <f:BoundField MinWidth="130px" DataField="FItemName" HeaderText="名称" SortField="FItemName" />
                                                                <f:BoundField MinWidth="80px" DataField="FSpec" HeaderText="规格" SortField="FSpec" />
                                                                <f:BoundField MinWidth="80px" DataField="FUnit" HeaderText="单位" SortField="FUnit" TextAlign="Center" />
                                                                <f:BoundField MinWidth="80px" DataField="FQty" HeaderText="商品发出数" SortField="FUnit" TextAlign="Center" />
                                                                <f:BoundField MinWidth="80px" DataField="FBottleQty" HeaderText="容器发出数" SortField="FUnit" TextAlign="Center" />
                                                                <f:BoundField MinWidth="80px" DataField="FPrice" HeaderText="单价" SortField="FUnit" TextAlign="Center" />
                                                                <f:BoundField MinWidth="80px" DataField="FAmount" HeaderText="金额" SortField="FUnit" TextAlign="Center" />
                                                
                                                                <f:BoundField MinWidth="80px" DataField="FReturnQty" HeaderText="商品收入数" SortField="FUnit" TextAlign="Center" />
                                                
                                                                <f:BoundField MinWidth="80px" DataField="FRecycleQty" HeaderText="容器收入数" SortField="FUnit" TextAlign="Center" />
                                                                <f:BoundField MinWidth="80px" DataField="tbxFBottle" HeaderText="包装物" SortField="FUnit" TextAlign="Center" />
                                                
                                                                <f:BoundField MinWidth="120px" DataField="FNum" HeaderText="订单号码" SortField="FUnit" TextAlign="Center" />
                                                                <f:BoundField MinWidth="160px" DataField="FMemo" HeaderText="备注说明" SortField="FUnit" TextAlign="Center" />

                                                                <f:BoundField MinWidth="120px" Hidden="True" DataField="FBottleOweQty" HeaderText="已提瓶" SortField="FBottleOweQty" TextAlign="Right" />
                                                                <f:BoundField MinWidth="120px" Hidden="True" DataField="FCateName" HeaderText="类型" SortField="FCateName" />
                                                            </Columns>
                                                        </f:Grid>
                                                    </Items>
                                                </f:Tab>
                                                <f:Tab Title="单据状态" BodyPadding="0px"
                                                    Layout="Fit" runat="server">
                                                    <Items>
                                                        <f:Grid ID="Grid2" PageSize="20" ShowBorder="False" ShowHeader="False" AllowPaging="False"
                                                            runat="server" EnableCheckBoxSelect="False" DataKeyNames="FId" EnableSummary="True"
                                                            AllowSorting="False" EmptyText="查询无结果" EnableHeaderMenu="True" AllowCellEditing="False">
                                                            <Columns>
                                                                <f:BoundField MinWidth="80px" DataField="FDeptName" HeaderText="部门名称" SortField="FDeptName" />
                                                                <f:BoundField MinWidth="80px" DataField="FOperator" HeaderText="操作员" SortField="FOperator" />
                                                                <f:BoundField MinWidth="80px" DataField="FDate" HeaderText="时间" SortField="FDate" TextAlign="Center" DataFormatString="{0:yyyy-MM-dd}" />
                                                                <f:BoundField MinWidth="120px" DataField="FActionName" HeaderText="按扭" SortField="FActionName" />
                                                                <f:BoundField MinWidth="320px" DataField="FMemo" HeaderText="描述" SortField="FMemo" />
                                                            </Columns>
                                                        </f:Grid>
                                                    </Items>
                                                </f:Tab>
                                                <f:Tab Title="审批流程" BodyPadding="0px"
                                                    Layout="Fit" runat="server">
                                                    <Items>
                                                        <f:Grid ID="Grid3" PageSize="20" ShowBorder="False" ShowHeader="False" AllowPaging="False"
                                                            runat="server" EnableCheckBoxSelect="False" DataKeyNames="FId" EnableSummary="True"
                                                            AllowSorting="False" EmptyText="查询无结果" EnableHeaderMenu="True" AllowCellEditing="False">
                                                            <Columns>
                                                                <f:BoundField MinWidth="80px" DataField="FDeptName" HeaderText="部门名称" SortField="FDeptName" />
                                                                <f:BoundField MinWidth="80px" DataField="FOperator" HeaderText="操作员" SortField="FOperator" />
                                                                <f:BoundField MinWidth="80px" DataField="FDate" HeaderText="时间" SortField="FDate" TextAlign="Center" DataFormatString="{0:yyyy-MM-dd}" />
                                                                <f:BoundField MinWidth="320px" DataField="FMemo" HeaderText="描述" SortField="FMemo" />
                                                            </Columns>
                                                        </f:Grid>
                                                    </Items>
                                                </f:Tab>
                                                <f:Tab Title="明细变更" BodyPadding="0px"
                                                    Layout="Fit" runat="server">
                                                    <Items>
                                                        <f:Grid ID="Grid4" PageSize="20" ShowBorder="False" ShowHeader="False" AllowPaging="False"
                                                            runat="server" EnableCheckBoxSelect="False" DataKeyNames="FId" EnableSummary="True"
                                                            AllowSorting="False" EmptyText="查询无结果" EnableHeaderMenu="True" AllowCellEditing="False">
                                                            <Columns>
                                                                <f:BoundField MinWidth="60px" DataField="FStatus" HeaderText="状态" SortField="FStatus" />
                                                                <f:BoundField MinWidth="80px" DataField="FUpdateBy" HeaderText="变更人" SortField="FUpdateBy" />
                                                                <f:BoundField MinWidth="100px" DataField="FUpdateDate" HeaderText="变更时间" SortField="FUpdateDate" DataFormatString="{0:yyyy-MM-dd}" />
                                                                <f:BoundField MinWidth="60px" DataField="FItemCode" HeaderText="编码" SortField="FItemCode" />
                                                                <f:BoundField MinWidth="80px" DataField="FItemName" HeaderText="名称" SortField="FItemName" />
                                                                <f:BoundField MinWidth="80px" DataField="FSpec" HeaderText="规格" SortField="FSpec" />
                                                                <f:BoundField MinWidth="60px" DataField="FUnit" HeaderText="单位" SortField="FUnit" TextAlign="Center" />
                                                                <f:BoundField MinWidth="60px" DataField="FPrice" HeaderText="单价" SortField="FPrice"  Hidden="true"/>
                                                                <f:BoundField MinWidth="80px" DataField="FQty" HeaderText="数量" SortField="FQty"  Hidden="true"/>
                                                                <f:BoundField MinWidth="80px" DataField="FAmount" HeaderText="金额" SortField="FAmount"  Hidden="true"/>
                                                                <f:BoundField MinWidth="100px" HeaderText="包装物" DataField="FBottleName" />
                                                                <f:BoundField MinWidth="80px" DataField="FBottleQty" TextAlign="Right" HeaderText="气瓶数量" />
                                                            </Columns>
                                                        </f:Grid>
                                                    </Items>
                                                </f:Tab>
                                            </Tabs>
                                        </f:TabStrip>
                                    </Items>
                                </f:Region>

                                </Regions>
                        </f:RegionPanel>
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>
        <br />



    </form>
    <object id="LODOP_OB" classid="clsid:2105C259-1E0C-4534-8141-A753534CB4CA" width="0" height="0">
        <embed id="LODOP_EM" type="application/x-print-lodop" width="0" height="0"></embed>
    </object>
    <script src="../../jqueryui/js/jquery-1.8.3.min.js" type="text/javascript"> </script>
    <script src="../../jqueryui/js/jquery-ui-1.9.2.custom.js?v=4.0" type="text/javascript"> </script>
    <script type="text/javascript" language="javascript" src="../../js/LodopFuncs.js"></script>
    <script type="text/javascript">
        var basePath = '<%= ResolveUrl("~/") %>';

        var gridClientID = '<%= Grid1.ClientID %>';
        var inputselector = '.f-grid-tpl input';
        

        function reloadGrid(keys) {
            __doPostBack(null, 'reloadGrid:' + keys);
        };

        function closeActiveTab() {
            parent.removeActiveTab();
        };

        function Gbeforeedit(editor, e, eop) {
            var edcmp = e.column.getEditor();
            window.setTimeout(function () {
                edcmp.selectText();
            }, 300);
        };

        function onGridAfterEdit(editor, params) {
            var me = this, columnId = params.column.id, rowId = params.record.getId();
            if (columnId === 'FQty' || columnId === 'FPrice') {
                var fQty = parseFloat(me.f_getCellValue(rowId, 'FQty'));
                me.f_updateCellValue(rowId, 'FBottleQty', fQty);
                var fPrice = parseFloat(me.f_getCellValue(rowId, 'FPrice'));
                me.f_updateCellValue(rowId, 'FAmount', fQty * fPrice).toFixed(2);

                updateSummary();
            }
        };

        function updateSummary() {

            // 回发到后台更新
            __doPostBack('', 'UPDATE_SUMMARY');
        };

        //--------打印配置---------------
        var LODOP; //声明为全局变量 
        function LodopPrinter(keyid) {
            LODOP = getLodop();
            LODOP.PRINT_INIT("河南禄恒软件科技有限公司");
            var strHtml = "";
            $.post("../../common/AjaxPrinter.ashx?oper=ajaxPrintSales",
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
