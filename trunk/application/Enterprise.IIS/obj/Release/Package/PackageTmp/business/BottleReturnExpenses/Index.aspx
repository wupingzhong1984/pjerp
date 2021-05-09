<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Enterprise.IIS.business.BottleReturnExpenses.Index" %>

<%@ Import Namespace="Enterprise.Framework.Enum" %>
<%@ Import Namespace="Enterprise.IIS.Common" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>容器回收检查处理单</title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .colorred {
             color: red;
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
            font-weight: bold
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region1" Title="容器回收检查处理单" Position="Center"
                    ShowBorder="False" BoxConfigAlign="Stretch" Layout="Fit" runat="server" ShowHeader="False" Icon="Page">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <f:Label runat="server" ID="headerText" Text="容器回收检查处理单"/>
                                <f:ToolbarSeparator ID="ToolbarSeparator2" runat="server" />
                                <f:Button ID="btnSearch" OnClick="btnSearch_Click" runat="server" Text="查询" Icon="Zoom" Hidden="False" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnAdd" runat="server" Text="新增" OnClientClick="openAddUI()" Icon="Add" Hidden="True" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnEdit" OnClick="btnEdit_Click" runat="server" Hidden="True" Text="编辑"
                                    Icon="PageWhiteEdit" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnBatchDelete" OnClick="btnBatchDelete_Click" runat="server" Text="作废"
                                    Icon="Delete" Hidden="True" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnPrint" runat="server" Text="打印" OnClick="btnPrint_Click"
                                    Icon="Printer" Hidden="True" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnExport" OnClick="btnExport_Click" runat="server" Text="引出" Icon="PageExcel"
                                    Hidden="False" EnableAjax="false" DisableControlBeforePostBack="false" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnClose" EnablePostBack="False" Text="关闭" runat="server" Icon="SystemClose" Size="Medium" onClientClick="closeActiveTab();">
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
                                                        <f:DatePicker ID="dateEnd" runat="server" Label="结束日期"  DateFormatString="yyyy-MM-dd" LabelAlign="Right">
                                                        </f:DatePicker>
                                                        <f:DropDownList runat="server" ID="ddlFDistributionPoint" Label="作业区" LabelAlign="Right" EnableEdit="true"/>
                                                    </Items>
                                                </f:FormRow>
                                                <f:FormRow ID="FormRow2">
                                                    <Items>
                                                        <f:TextBox ID="txtFName" runat="server"  Label="客户名称" LabelAlign="Right">
                                                        </f:TextBox>
                                                        <f:TextBox ID="txtFCode" runat="server" Label="客户代码" LabelAlign="Right">
                                                        </f:TextBox>
                                                        <f:TextBox ID="txtFKeyId" runat="server" Label="单据编号" LabelAlign="Right">
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
                                        <f:Grid ID="Grid1" PageSize="200" ShowBorder="false" ShowHeader="false" AllowPaging="true"
                                            runat="server" EnableCheckBoxSelect="True" DataKeyNames="KeyId,FFlag,FT6Status,FShipper,FReceiver" OnPageIndexChange="Grid1_PageIndexChange"
                                            IsDatabasePaging="true" OnSort="Grid1_Sort" SortDirection="DESC" OnRowCommand="Grid1_RowCommand"
                                            AllowSorting="true" EmptyText="查询无结果" EnableHeaderMenu="True" EnableRowClickEvent="true"
                                            OnRowDataBound="Grid1_RowDataBound">
                                            <Columns>
                                                <f:LinkButtonField Width="120px"  ColumnID="KeyId" CommandName="actView" DataTextField="KeyId" HeaderText="单据号" SortField="KeyId" TextAlign="Center" runat="server" />
                                                <f:TemplateField Width="100px" ColumnID="FFlag" HeaderText="作废标识" TextAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFFlag" runat="server" Text='<%# EnumDescription.GetFieldText((GasBillFlag) Eval("FFlag")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                </f:TemplateField>
                                                <f:TemplateField Width="100px" HeaderText="单据状态" TextAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatus" runat="server" Text='<%# EnumDescription.GetFieldText((GasEnumBillStauts) Eval("FStatus")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                </f:TemplateField>
                                                <f:BoundField Width="80px" DataField="FT6Status" HeaderText="上传" SortField="FT6Status" TextAlign="Center" />
                                                <f:BoundField MinWidth="120px" DataField="FDistributionPoint" HeaderText="作业区" SortField="FDistributionPoint" />
                                                <f:BoundField MinWidth="80px" DataField="FDate" HeaderText="回收日期" SortField="FDate" DataFormatString="{0:yyyy-MM-dd}" TextAlign="Center" />
                                                <f:BoundField MinWidth="220px" DataField="FName" HeaderText="客户名称" SortField="FName" />
                                                <f:BoundField MinWidth="60px" DataField="FDeliveryMethod" HeaderText="配送方式" SortField="FDeliveryMethod" />
                                                <f:BoundField MinWidth="120px" DataField="FVehicleNum" HeaderText="车牌号" SortField="FVehicleNum" TextAlign="Left" />
                                                <f:BoundField MinWidth="120px" DataField="FDriver" HeaderText="送货司机" SortField="FDriver" TextAlign="Left" />
                                                <f:BoundField MinWidth="120px" DataField="FSupercargo" HeaderText="押运员" SortField="FSupercargo" TextAlign="Left" />
                                                <f:BoundField MinWidth="120px" DataField="FShipper" HeaderText="发货人" SortField="FShipper" TextAlign="Left" />
                                                <f:BoundField MinWidth="120px" DataField="FReceiver" HeaderText="收货人" SortField="FReceiver" TextAlign="Left" />
                                                <f:BoundField MinWidth="80px" DataField="CreateBy" HeaderText="操作员" SortField="CreateBy" TextAlign="Left" />
                                                <f:BoundField MinWidth="80px" DataField="FSalesman" HeaderText="业务员" SortField="FSalesman" TextAlign="Left" />
                                                <f:BoundField MinWidth="120px" DataField="FMemo" HeaderText="备注" SortField="FMemo" />
                                                <f:BoundField MinWidth="145px" DataField="FTime1" HeaderText="操作时间" SortField="FTime1" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" TextAlign="Center" />
                                            </Columns>
                                            <PageItems>
                                                <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                                </f:ToolbarSeparator>
                                                <f:ToolbarText ID="ToolbarText1" runat="server" Text="每页记录数：">
                                                </f:ToolbarText>
                                                <f:DropDownList runat="server" ID="ddlPageSize" Width="80px" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
                                                    <f:ListItem Text="100" Value="100" />
                                                    <f:ListItem Text="200" Value="200" Selected="True" />
                                                    <f:ListItem Text="300" Value="300" />
                                                </f:DropDownList>
                                            </PageItems>
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
            EnableIFrame="true" Height="450px" Width="480px">
        </f:Window>

    </form>
    <object id="LODOP_OB" classid="clsid:2105C259-1E0C-4534-8141-A753534CB4CA" width="0" height="0">
        <embed id="LODOP_EM" type="application/x-print-lodop" width="0" height="0"></embed>
    </object>
    
    <script type="text/javascript" src="../../js/UX_TimePickerField.js"></script>
    <script type="text/javascript" src="../../js/UX_DateTimePicker.js"></script>
    <script type="text/javascript" src="../../js/UX_DateTimeMenu.js"></script>
    <script type="text/javascript" src="../../js/UX_DateTimeField.js"></script>


    <script src="../../jqueryui/js/jquery-1.8.3.min.js" type="text/javascript"> </script>
    <script src="../../jqueryui/js/jquery-ui-1.9.2.custom.js?v=4.0" type="text/javascript"> </script>
    <script type="text/javascript" language="javascript" src="../../js/LodopFuncs.js"></script>
    <script type="text/javascript">
        var basePath = '<%= ResolveUrl("~/") %>';
        function openDetailsUI(keyid) {
            var url = 'business/BottleReturnExpenses/Edit.aspx?action=2&keyid=' + keyid;
            parent.addExampleTab.apply(null, ['add_tab_' + keyid, basePath + url, '【容器回收检查处理单' + keyid + '详情】', basePath + 'icon/page_edit.png', true]);
        };
        function openAddUI() {
            var url = 'business/BottleReturnExpenses/Edit.aspx?action=1';
            parent.addExampleTab.apply(null, ['add_tab_00001', basePath + url, '添加容器回收检查处理单', basePath + 'icon/page_add.png', true]);
        };
        function openEditUI(keyid) {
            var url = 'business/BottleReturnExpenses/Edit.aspx?KeyId=' + keyid + '&action=2';
            parent.addExampleTab.apply(null, ['add_tab_00001', basePath + url, '【编辑容器回收检查处理单' + keyid + '】', basePath + 'icon/page_edit.png', true]);
        };
        function closeActiveTab() {
            parent.removeActiveTab();
        };
        //--------打印配置---------------
        var LODOP; //声明为全局变量 
        function LodopPrinter(keyid) {
            LODOP = getLodop();
            LODOP.PRINT_INIT("河南禄恒软件科技有限公司");
            var strHtml = "";
            $.post("../../Common/AjaxPrinter.ashx?oper=ajaxPrintSales",
				  { "keyid": keyid },
              function (result) {
                  if (result != "") {
                      LODOP.ADD_PRINT_HTM(0, 1, "100%", "100%", result);
                      LODOP.PREVIEW();
                  }
                  else {
                      alert("打印失败！");
                  }
              }, "html");
        };

        function LodopPrinterSwap(keyid) {
            LODOP = getLodop();
            LODOP.PRINT_INIT("河南禄恒软件科技有限公司");
            var strHtml = "";
            $.post("../../common/AjaxPrinter.ashx?oper=ajaxPrintSwap",
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

        function LodopPrinterBlank() {
            LODOP = getLodop();
            LODOP.PRINT_INIT("河南禄恒软件科技有限公司");
            var strHtml = "";
            $.post("../../common/AjaxPrinter.ashx?oper=ajaxPrintBlank",
				  { "keyid": 'KeyId' },
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
