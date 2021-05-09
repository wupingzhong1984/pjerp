<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReconciliationSales.aspx.cs" Inherits="Enterprise.IIS.business.Reports.ReconciliationSales" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>客户代码对账单</title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <f:Button ID="btnSearch" runat="server" Text="查询" Icon="Zoom" OnClick="btnSearch_Click">
                        </f:Button>
                        <f:Button ID="btnExport" OnClick="btnExport_Click" runat="server" Text="引出" Icon="PageExcel"
                            Hidden="False" EnableAjax="false" DisableControlBeforePostBack="false" Size="Medium">
                        </f:Button>
                        <f:Button ID="btnPrint" runat="server" Text="打印" OnClick="btnPrint_Click"
                                    Icon="Printer" Hidden="False" Size="Medium">
                        </f:Button>
                        <f:Button ID="btnClose" EnablePostBack="False" Text="关闭" runat="server" Icon="SystemClose" Size="Medium" onClientClick="closeActiveTab();">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Regions>
                <f:Region ID="Region1" ShowHeader="false" Split="true" ShowBorder="False"
                    Width="200px" Position="Left" Layout="Fit" runat="server">
                    <Items>
                        <f:Tree ID="trDept" Width="50px" OnNodeCommand="trDept_NodeCommand" ShowHeader="False"
                            ShowBorder="false" Icon="House" Title="客户" runat="server" EnableTextSelection="true"
                            EnableArrows="true" AutoScroll="True">
                        </f:Tree>
                    </Items>
                </f:Region>
                <f:Region ID="Region2" Title="客户" Position="Center"
                    ShowBorder="False" BoxConfigAlign="Stretch" Layout="Fit" runat="server" ShowHeader="False">
                    <Items>
                        <f:RegionPanel ID="RegionPanel2" ShowBorder="false" runat="server">
                            <Regions>
                                <f:Region ID="Region3" ShowBorder="false" ShowHeader="false" Split="False"
                                    Position="Top" Layout="Fit"
                                    BodyPadding="3px" runat="server" Height="40px" EnableCollapse="True">
                                    <Items>
                                        <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="80px"
                                            runat="server" BodyPadding="5px">
                                            <Rows>
                                                <f:FormRow ID="FormRow1">
                                                    <Items>
                                                        <f:DropDownList runat="server" ID="ddlCompany" Label="公司" LabelAlign="Right"></f:DropDownList>
                                                        <f:DatePicker ID="dpkFDateBegin" runat="server" Label="年月" DateFormatString="yyyy-MM" LabelAlign="Right">
                                                        </f:DatePicker>
                                                        <f:DropDownList runat="server" ID="ddlFSalesman" Label="业务员" EnableEdit="True"  LabelAlign="Right"  
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddlFSalesman_SelectedIndexChanged"/>
                                                        <f:TextBox ID="txtFName" runat="server" Label="客户代码" LabelAlign="Right">
                                                        </f:TextBox>
                                                    </Items>
                                                </f:FormRow>
                                            </Rows>
                                        </f:Form>
                                    </Items>
                                </f:Region>
                                <f:Region ID="Region4" ShowBorder="False" ShowHeader="false" Position="Center"
                                    Layout="Fit" runat="server" BodyPadding="3px" AutoScroll="True">
                                    <Items>
                                        <f:Grid ID="Grid1" PageSize="20" ShowBorder="false" ShowHeader="false" AllowPaging="false"
                                            runat="server" EnableCheckBoxSelect="True" DataKeyNames="KeyId"
                                            IsDatabasePaging="false" SortDirection="ASC"
                                            AllowSorting="false" EmptyText="查询无结果" EnableHeaderMenu="True">
                                            <Columns>
                                                <f:BoundField MinWidth="80px" DataField="FDate" HeaderText="日期" SortField="FDate" />
                                                <f:BoundField MinWidth="80px" DataField="FType" HeaderText="业务类型" SortField="FType" />
                                                <f:BoundField MinWidth="120px" DataField="KeyId" HeaderText="单据号码" SortField="KeyId" />
                                                <f:BoundField MinWidth="120px" DataField="FItemName" HeaderText="商品名称" SortField="FItemName" />
                                                <f:BoundField MinWidth="100px" DataField="FSpec" HeaderText="规格/型号" SortField="FSpec" />
                                                <f:BoundField MinWidth="60px" DataField="FUnit" HeaderText="单位" SortField="FUnit" />
                                                <f:BoundField MinWidth="80px" DataField="FQty" HeaderText="数量" SortField="FQty" TextAlign="right"/>
                                                <f:BoundField MinWidth="80px" DataField="FPrice" HeaderText="单价" SortField="FPrice" TextAlign="right"/>
                                                <f:BoundField MinWidth="100px" DataField="FAmount" HeaderText="金额" SortField="FAmount" TextAlign="right"/>
                                                <f:BoundField MinWidth="100px" DataField="FAmt" HeaderText="未期余额" SortField="FAmt" TextAlign="right"/>
                                                <f:BoundField MinWidth="80px" DataField="FNum" HeaderText="订单号码" SortField="FNum" />
                                            </Columns>
                                        </f:Grid>
                                    </Items>
                                </f:Region>
                                <%--<f:Region ID="Region5" ShowBorder="False" ShowHeader="False" Position="Bottom"
                                    Layout="Fit" MinHeight="100px" MaxHeight="120px" runat="server" BodyPadding="3px" AutoScroll="True" Title="调度单">
                                    <Items>
                                        <f:ContentPanel runat="server">
                                            <table>
                                                <tr>
                                                    <td style="width: 160px"><f:Label runat="server" LabelAlign="Right" ID="FSum" Text="0.00" Label="总金额"/></td>
                                                    <td style="width: 160px"><f:Label runat="server" LabelAlign="Right" ID="FSalesSum" Text="0.00" Label="销售总金额"/></td>
                                                    <td style="width: 160px"><f:Label runat="server" LabelAlign="Right" ID="FRetrunSum" Text="0.00" Label="退货总金额"/></td>
                                                    <td style="width: 160px"><f:Label runat="server" LabelAlign="Right" ID="FLeaseARSum" Text="0.00" Label="应收总押金额"/></td>
                                                    <td style="width: 160px"><f:Label runat="server" LabelAlign="Right" ID="FLeaseSKSum" Text="0.00" Label="已收总押金额"/></td>
                                                </tr>
                                                <tr>
                                                    <td><f:Label runat="server" LabelAlign="Right" ID="FLeaseReturnSum" Text="0.00" Label="已退押金额"/></td>
                                                    <td><f:Label runat="server" LabelAlign="Right" ID="FLeaseSum" Text="0.00" Label="尚欠押金款"/></td>
                                                    

                                                    <td><f:Label runat="server" LabelAlign="Right" ID="FSKSum" Text="0.00" Label="已收款"/></td>
                                                    <td><f:Label runat="server" LabelAlign="Right" ID="FARSum" Text="0.00" Label="尚欠款"/></td>
                                                    
                                                </tr>
                                            </table>
                                        </f:ContentPanel>
                                    </Items>
                                </f:Region>--%>
                            </Regions>
                        </f:RegionPanel>
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>
        <br />
        <f:Window ID="Window1" runat="server" WindowPosition="Center" IsModal="true" Title="原始单据"
            Icon="ApplicationViewDetail" EnableMaximize="true" EnableResize="true" Hidden="True"
            Target="Parent" EnableIFrame="true" IFrameUrl="about:blank" Height="480px" Width="380px"
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
        var basePath = '<%= ResolveUrl("~/") %>';
        
        function closeActiveTab() {
            parent.removeActiveTab();
        }

        //--------打印配置---------------
        var LODOP; //声明为全局变量 
        function LodopPrinter(code, firstDay,end,date) {
            LODOP = getLodop();
            LODOP.PRINT_INIT("河南禄恒软件科技有限公司");
            var strHtml = "";
            $.post("../../common/AjaxPrinter.ashx?oper=ajaxPrintReconciliationSales",
				  { "FCode": code, "FirstDay": firstDay,"end":end,"date":date },
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
    </script>
</body>
</html>
