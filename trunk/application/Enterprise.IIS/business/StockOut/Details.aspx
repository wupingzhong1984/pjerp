<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Details.aspx.cs" Inherits="Enterprise.IIS.business.StockOut.Details" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>发货单</title>
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
                <f:Region ID="Region1" Title="发货单" Position="Center"
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
                                <f:Region ID="Region3" ShowBorder="false" ShowHeader="false" Split="False"
                                    Position="Top" Layout="Fit"
                                    BodyPadding="5px" runat="server" Height="98px" EnableCollapse="True">
                                    <Items>
                                        <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="110px"
                                            runat="server">
                                            <Rows>
                                                <f:FormRow ID="FormRow0">
                                                    <Items>
                                                        <f:Label ID="txtKeyId" runat="server" Label="单据号">
                                                        </f:Label>
                                                        <f:Label ID="txtCreateBy" runat="server" Label="制单人">
                                                        </f:Label>
                                                        <f:Label ID="txtFDate" runat="server" Label="日期">
                                                        </f:Label>
                                                        <f:Label runat="server" ID="ddlFCate" Label="出仓类型"/>
                                                    </Items>
                                                </f:FormRow>
                                                <f:FormRow ID="FormRow5">
                                                    <Items>
                                                        <f:Label ID="txtFMemo" runat="server" Label="备注">
                                                        </f:Label>
                                                    </Items>
                                                </f:FormRow>
                                            </Rows>
                                        </f:Form>
                                    </Items>
                                </f:Region>
                                <f:Region ID="Region4" ShowBorder="False" ShowHeader="false" Position="Center"
                                    Layout="Fit" runat="server" BodyPadding="0px" AutoScroll="True">
                                    <Items>
                                        <f:TabStrip ID="TabStrip1" AutoPostBack="true" ShowBorder="false" ActiveTabIndex="0" runat="server">
                                            <Tabs>
                                                <f:Tab Title="销售明细" BodyPadding="0px"
                                                    Layout="Fit" runat="server">
                                                    <Items>
                                                        <f:Grid ID="Grid1" PageSize="20" ShowBorder="False" ShowHeader="False" AllowPaging="False"
                                                            runat="server" EnableCheckBoxSelect="False" DataKeyNames="KeyId,FId" EnableSummary="True" SummaryPosition="Bottom" OnRowCommand="Grid1_RowCommand"
                                                            AllowSorting="False" EmptyText="查询无结果" EnableHeaderMenu="True" AllowCellEditing="False" ClicksToEdit="2">
                                                            <Columns>
                                                                <f:RenderField MinWidth="100px" ColumnID="FItemCode" DataField="FItemCode" FieldType="String"
                                                                    HeaderText="编码">
                                                                    <Editor>
                                                                        <f:TextBox ID="tbxFItemCode" Required="true" runat="server">
                                                                        </f:TextBox>
                                                                    </Editor>
                                                                </f:RenderField>
                                                                <f:BoundField MinWidth="80px" DataField="FItemName" HeaderText="名称" SortField="FItemName" />
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
                                                                <f:RenderField MinWidth="130px" ColumnID="FBottleName" DataField="FBottleName"
                                                                    HeaderText="包装物">
                                                                    <Editor>
                                                                        <f:DropDownList ID="tbxFBottle" Required="true" runat="server" Enabled="True" EnableEdit="True">
                                                                        </f:DropDownList>
                                                                    </Editor>
                                                                </f:RenderField>
                                                                <f:RenderField MinWidth="100px" ColumnID="FBottleQty" DataField="FBottleQty" FieldType="Double" TextAlign="Right"
                                                                    HeaderText="气瓶数量">
                                                                    <Editor>
                                                                        <f:TextBox ID="tbxFBottleQty" Required="true" runat="server">
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
                                                                <f:BoundField MinWidth="120px" DataField="FBottleOweQty" HeaderText="已提瓶" SortField="FBottleOweQty" TextAlign="Right" />
                                                                <f:BoundField MinWidth="120px" DataField="FCateName" HeaderText="类型" SortField="FCateName" />
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
                                                                <%--<f:BoundField MinWidth="360px" DataField="FMemo" HeaderText="备注说明" />--%>
                                                                <f:BoundField MinWidth="60px" DataField="FItemCode" HeaderText="编码" SortField="FItemCode" />
                                                                <f:BoundField MinWidth="80px" DataField="FItemName" HeaderText="名称" SortField="FItemName" />
                                                                <f:BoundField MinWidth="80px" DataField="FSpec" HeaderText="规格" SortField="FSpec" />
                                                                <f:BoundField MinWidth="60px" DataField="FUnit" HeaderText="单位" SortField="FUnit" TextAlign="Center" />
                                                                <f:BoundField MinWidth="60px" DataField="FPrice" HeaderText="单价" SortField="FPrice" />
                                                                <f:BoundField MinWidth="80px" DataField="FQty" HeaderText="数量" SortField="FQty" />
                                                                <f:BoundField MinWidth="80px" DataField="FAmount" HeaderText="金额" SortField="FAmount" />
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
        };

        //查看是否安装了LodopPrinter
        //function CheckIsInstall() {
        //    try {
        //        var LODOP = getLodop();
        //        if (LODOP.VERSION) {
        //           if (LODOP.CVERSION)
        //                alert("当前有C-Lodop云打印可用!\n C-Lodop版本:" + LODOP.CVERSION + "(内含Lodop" + LODOP.VERSION + ")");
        //            else
        //                alert("本机已成功安装了Lodop控件！\n 版本号:" + LODOP.VERSION);
        //        };
        //    } catch (err) {
        //    }
        //};

        //--------打印配置---------------
        var LODOP; //声明为全局变量 
        function LodopPrinter() {
            LODOP = getLodop();
            LODOP.PRINT_INIT("河南禄恒软件科技有限公司");
            LODOP.ADD_PRINT_RECT(10, 8, 776, 228, 3, 1);

            //边框离纸张顶端10px(px是绝对值长度，等于1/96英寸,下同)距左边55px、宽360px、高220px、
            //框为实线(0-实线 1-破折线 2-点线 3-点划线 4-双点划线)、线宽为1px

            LODOP.SET_PRINT_STYLE("FontSize", 18);
            //离纸张顶端53px、距左边187px、宽75px、高20px、内容为“”
            LODOP.ADD_PRINT_TEXT(20, 360, 776, 20, "发货单");
            LODOP.SET_PRINT_STYLE("FontSize", 11);


            LODOP.ADD_PRINT_TEXT(50, 10, 300, 20, "地址：中国北京社会科学院附近东大街西胡同");
            LODOP.ADD_PRINT_TEXT(80, 10, 300, 20, "电话：010-88811888");

            //LODOP.ADD_PRINT_HTM(10, 55, "100%", "100%", strHtml);

            LODOP.PREVIEW();


            //LODOP.PRINT();
        };
        //
    </script>
</body>
</html>
