<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Details.aspx.cs" Inherits="Enterprise.IIS.business.Production.Details" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>生产</title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../../jqueryui/css/ui-lightness/jquery-ui-1.9.2.custom.css" />
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
                <f:Region ID="Region1" Title="生产" Position="Center"
                    ShowBorder="false" BoxConfigAlign="Stretch" Layout="Fit" runat="server" ShowHeader="False">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <f:Button ID="btnPrint" runat="server" Text="打印" OnClick="btnPrint_Click"
                                    Icon="Printer" Hidden="False" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnClose" EnablePostBack="false" OnClientClick="closeActiveTab();" Text="关闭" runat="server" Icon="SystemClose" Size="Medium">
                                </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Items>
                        <f:RegionPanel ID="RegionPanel2" ShowBorder="false" runat="server">
                            <Regions>
                                <f:Region ID="Region3" ShowBorder="false" ShowHeader="false" Split="False"
                                    Position="Top" Layout="Fit"
                                    BodyPadding="3px" runat="server" Height="118px" EnableCollapse="True">
                                    <Items>
                                        <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="90px"
                                            runat="server">
                                            <Rows>
                                                <f:FormRow ID="FormRow0">
                                                    <Items>
                                                        <f:Label ID="txtKeyId" runat="server" Label="单据号" LabelAlign="Right" >
                                                        </f:Label>
                                                        <f:Label ID="txtCreateBy" runat="server" Label="制单人" LabelAlign="Right" >
                                                        </f:Label>
                                                        <f:Label ID="txtFDate" runat="server" Label="生产日期" LabelAlign="Right" >
                                                        </f:Label>
                                                        <f:Label ID="txtFBatchNumber" runat="server" Label="批次号" LabelAlign="Right">
                                                        </f:Label>
                                                    </Items>
                                                </f:FormRow>
                                                <f:FormRow ID="FormRow1">
                                                    <Items>
                                                        <f:Label runat="server" ID="ddlFProducer" LabelAlign="Right" Label="生产员" />
                                                        <f:Label runat="server" ID="ddlFSurveyor" LabelAlign="Right" Label="检测员"/>
                                                        <f:Label runat="server" Label="商品代码" ID="txtFCode"  LabelAlign="Right" />
                                                        <f:Label ID="tbxFName"  Label="商品名称" runat="server" LabelAlign="Right" >
                                                        </f:Label>
                                                    </Items>
                                                </f:FormRow>
                                                <f:FormRow ID="FormRow2">
                                                    <Items>
                                                        <f:Label runat="server" ID="txtFQty" Label="生产数量" Text="0" LabelAlign="Right" />
                                                        <f:Label ID="DropDownList1" LabelAlign="Right"  Label="包装物" runat="server">
                                                        </f:Label>
                                                        <f:Label runat="server" ID="txtFBottleQty"  Label="包装物数量" LabelAlign="Right" Text="0"/>
                                                        <f:Label ID="txtFMemo" LabelAlign="Right"  runat="server" Label="备注">
                                                        </f:Label>
                                                    </Items> 
                                                </f:FormRow>
                                                <f:FormRow ID="FormRow3">
                                                    <Items>
                                                        <f:Label runat="server" ID="txtFCostPrice" Label="组装成本" LabelAlign="Right" Text="0"/>
                                                        <f:Label runat="server" ID="txtFBalance"  Label="整合差额" LabelAlign="Right" Text="0"/>
                                                        <f:Label runat="server" Hidden="True"/>
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
                                        <f:TabStrip ID="TabStrip1" AutoPostBack="true" ShowBorder="false" ActiveTabIndex="0" runat="server">
                                            <Tabs>
                                                <f:Tab Title="原料明细" BodyPadding="0px"
                                                    Layout="Fit" runat="server">
                                                    <Items>
                                                        <f:Grid ID="Grid1" PageSize="20" ShowBorder="False" ShowHeader="False" AllowPaging="False"
                                                            runat="server" EnableCheckBoxSelect="False" DataKeyNames="KeyId,FId" EnableSummary="True" SummaryPosition="Bottom"
                                                            AllowSorting="False" EmptyText="查询无结果" EnableHeaderMenu="True" AllowCellEditing="False" ClicksToEdit="2">
                                                            <Columns>
                                                                <f:RenderField MinWidth="100px" ColumnID="FItemCode" DataField="FItemCode" FieldType="String"
                                                                    HeaderText="编码">
                                                                    <Editor>
                                                                        <f:TextBox ID="tbxFItemCode" Required="true" runat="server">
                                                                        </f:TextBox>
                                                                    </Editor>
                                                                </f:RenderField>
                                                                <f:BoundField MinWidth="80px" DataField="FName" HeaderText="名称" SortField="FName" />
                                                                <f:BoundField MinWidth="80px" DataField="FSpec" HeaderText="规格" SortField="FSpec" />
                                                                <f:BoundField MinWidth="80px" DataField="FUnit" HeaderText="单位" SortField="FUnit" TextAlign="Center" />
                                                                <f:RenderField MinWidth="100px" ColumnID="FPrice" DataField="FPrice" FieldType="Double" TextAlign="Right"
                                                                    HeaderText="单价">
                                                                    <Editor>
                                                                        <f:TextBox ID="tbxFPrice" Required="true" runat="server">
                                                                        </f:TextBox>
                                                                    </Editor>
                                                                </f:RenderField>
                                                                <f:RenderField MinWidth="100px" ColumnID="FQty" DataField="FQty" FieldType="Double" TextAlign="Right"
                                                                    HeaderText="数量">
                                                                    <Editor>
                                                                        <f:TextBox ID="tbxFQty" Required="true" runat="server">
                                                                        </f:TextBox>
                                                                    </Editor>
                                                                </f:RenderField>
                                                                <f:RenderField MinWidth="100px" ColumnID="FAmount" DataField="FAmount" FieldType="Double" TextAlign="Right"
                                                                    HeaderText="金额">
                                                                    <Editor>
                                                                        <f:TextBox ID="tbxFAmount" Required="true" runat="server" Enabled="false">
                                                                        </f:TextBox>
                                                                    </Editor>
                                                                </f:RenderField>
                                                                <f:RenderField MinWidth="160px" ColumnID="FMemo" DataField="FMemo"
                                                                    HeaderText="备注说明">
                                                                    <Editor>
                                                                        <f:TextBox ID="tbxFMemo" Required="true" runat="server">
                                                                        </f:TextBox>
                                                                    </Editor>
                                                                </f:RenderField>
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
                                                                <f:BoundField MinWidth="80px" DataField="FStatus" HeaderText="状态" SortField="FStatus" />
                                                                <f:BoundField MinWidth="80px" DataField="FUpdateBy" HeaderText="变更人" SortField="FUpdateBy" />
                                                                <f:BoundField MinWidth="100px" DataField="FUpdateDate" HeaderText="变更时间" SortField="FUpdateDate" DataFormatString="{0:yyyy-MM-dd}" />
                                                                <%--<f:BoundField MinWidth="360px" DataField="FMemo" HeaderText="备注说明" />--%>
                                                                <f:BoundField MinWidth="80px" DataField="FItemCode" HeaderText="编码" SortField="FItemCode" />
                                                                <f:BoundField MinWidth="80px" DataField="FItemName" HeaderText="名称" SortField="FItemName" />
                                                                <f:BoundField MinWidth="80px" DataField="FSpec" HeaderText="规格" SortField="FSpec" />
                                                                <f:BoundField MinWidth="80px" DataField="FUnit" HeaderText="单位" SortField="FUnit" TextAlign="Center" />
                                                                <f:BoundField MinWidth="80px" DataField="FPrice" HeaderText="单价" SortField="FPrice" />
                                                                <f:BoundField MinWidth="80px" DataField="FQty" HeaderText="数量" SortField="FQty" />
                                                                <f:BoundField MinWidth="80px" DataField="FAmount" HeaderText="金额" SortField="FAmount" />
                                                                <f:BoundField MinWidth="130px" HeaderText="包装物" DataField="FBottleName" />
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
        <f:Window ID="Window1" runat="server" WindowPosition="Center" IsModal="true"
            Icon="ApplicationViewDetail" EnableMaximize="true" EnableResize="true" Hidden="True"
            Target="Parent" EnableIFrame="true" IFrameUrl="about:blank" Height="480px" Width="960px"
            OnClose="Window1_Close">
        </f:Window>
    </form>
    <object id="LODOP_OB" classid="clsid:2105C259-1E0C-4534-8141-A753534CB4CA" width="0" height="0">
        <embed id="LODOP_EM" type="application/x-print-lodop" width="0" height="0"></embed>
    </object>
    <script src="../../jqueryui/js/jquery-1.8.3.min.js" type="text/javascript"> </script>
    <script src="../../jqueryui/js/jquery-ui-1.9.2.custom.js?v=4.0" type="text/javascript"> </script>
    <script type="text/javascript" language="javascript" src="../../js/LodopFuncs.js"></script>
    <script type="text/javascript">
        function reloadGrid(keys) {
            __doPostBack(null, 'reloadGrid:' + keys);
        };

        var basePath = '<%= ResolveUrl("~/") %>';
        function closeActiveTab() {
            parent.removeActiveTab();
        }

        //查看是否安装了LodopPrinter
        function CheckIsInstall() {
            try {
                var LODOP = getLodop();
                if (LODOP.VERSION) {
                    if (LODOP.CVERSION)
                        alert("当前有C-Lodop云打印可用!\n C-Lodop版本:" + LODOP.CVERSION + "(内含Lodop" + LODOP.VERSION + ")");
                    else
                        alert("本机已成功安装了Lodop控件！\n 版本号:" + LODOP.VERSION);
                };
            } catch (err) {
            }
        };


        //--------打印配置---------------
        var LODOP; //声明为全局变量 
        function LodopPrinter(keyid) {
            LODOP = getLodop();
            LODOP.PRINT_INIT("河南禄恒软件科技有限公司");
            var strHtml = "";
            $.post("../../common/AjaxPrinter.ashx?oper=ajaxPrintProduction",
				  { "keyid": keyid },
              function (result) {
                  if (result != "") {

                      LODOP.ADD_PRINT_HTM(1, 1, "100%", "100%", result);
                      LODOP.PREVIEW();
                      //LODOP.PRINT();
                  }
                  else {
                      alert("打印失败！");
                  }
              }, "html");
        };

        //
    </script>
</body>
</html>
