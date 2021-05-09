<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Enterprise.IIS.business.Dispatch.Edit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>车辆调度日志</title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../../jqueryui/css/ui-lightness/jquery-ui-1.9.2.custom.css" />
    <style type="text/css">
        .x-grid-row-summary .x-grid-cell-inner {
            font-weight: bold;
            color: red;
        }
        .x-grid-row-summary .x-grid-cell {
            background-color: #dfeaf2;
        }
        #RegionPanel1_Region1_RegionPanel2_Region3_SimpleForm1_FormRow1_tbxFCustomer-labelEl {
            color: red;
        }
        #RegionPanel1_Region1_RegionPanel2_Region3_SimpleForm1_FormRow4_ddlDeliveryMethod-labelEl {
            color: red;
        }
        #RegionPanel1_Region1_RegionPanel2_Region3_SimpleForm1_FormRow5_ddlFT6SaleType-labelEl {
            color: red;
        }
        #RegionPanel1_Region1_RegionPanel2_Region3_SimpleForm1_FormRow5_ddlT6ReceiveSendType-labelEl {
            color: red;
        }
        #RegionPanel1_Region1_RegionPanel2_Region3_SimpleForm1_FormRow5_ddlDepartment-labelEl {
            color: red;
        }
        #RegionPanel1_Region1_RegionPanel2_Region3_SimpleForm1_FormRow6_ddlWarehouse-labelEl {
            color: red;
        }
        #RegionPanel1_Region1_RegionPanel2_Region3_SimpleForm1_FormRow5_ddlFT6Currency-labelEl {
            color: red;
        }
        #RegionPanel1_Region1_RegionPanel2_Region3_SimpleForm1_FormRow6_txtFT6ExchangeRate-labelEl {
            color: red;
        }
        #RegionPanel1_Region1_RegionPanel2_Region3_SimpleForm1_FormRow6_ddlFDistributionPoint-labelEl {
            color: red;
        }
        #RegionPanel1_Region1_RegionPanel2_Region3_SimpleForm1_FormRow3_ddlFShipper-labelEl {
            color: red;
        }
        #RegionPanel1_Region1_RegionPanel2_Region3_SimpleForm1_FormRow3_ddlFSalesman-labelEl {
            color: red;
        }
        #RegionPanel1_Region1_RegionPanel2_Region3_SimpleForm1_FormRow6_ddlFDistributionPoint-labelEl {
            color: red;
        }
        #RegionPanel1_Region1_RegionPanel2_Region3_header {
            background-color: #dfeaf2;
        }

        #component-1021 {
            background-color: #dfeaf2;
        }

        #RegionPanel1_Region1_RegionPanel2_Region3_header_hd-textEl {
            color: WindowText;
            font-size: 16px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
        <f:HiddenField runat="server" ID="hfdSpec" />
        <f:HiddenField runat="server" ID="hfdImage" />

        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region1" Title="车辆调度日志" Position="Center"
                    ShowBorder="false" BoxConfigAlign="Stretch" Layout="Fit" runat="server" ShowHeader="False">
                    <Items>
                        <f:RegionPanel ID="RegionPanel2" ShowBorder="false" runat="server">
                            <Regions>
                                <f:Region ID="Region3" ShowBorder="false" ShowHeader="True" Title="编辑车辆调度日志" Split="False"
                                    Position="Top" Layout="Fit"
                                    BodyPadding="3px" runat="server" Height="242px" EnableCollapse="False">
                                    <Items>
                                        <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="80px"
                                            runat="server" LabelAlign="Right">
                                            <Rows>
                                                <f:FormRow ID="FormRow0">
                                                    <Items>
                                                        <f:DatePicker ID="txtFDate" runat="server" Required="false" Label="配送日期" EmptyText="日期" LabelAlign="Right"
                                                            ShowRedStar="false">
                                                        </f:DatePicker>
                                                        <f:DropDownList runat="server" ID="ddlLogistics" Label="物流公司" 
                                                            LabelAlign="Right"/>
                                                        <f:Button ID="btnLog"  OnClick="btnLog_Click" runat="server" Text="生成日志" Size="Medium">
                                                        </f:Button>
                                                        <f:Label ID="txtKeyId" Label="单据号"  runat="server"></f:Label>
                                                    </Items>
                                                </f:FormRow>
                                                <f:FormRow ID="FormRow1">
                                                    <Items>
                                                        <f:TextBox ID="txtFWorkQty" runat="server" Label="工作（完好）" LabelAlign="Right">
                                                        </f:TextBox>
                                                        <f:TextBox ID="txtFStopQty" runat="server" Label="停驶（完好）" LabelAlign="Right">
                                                        </f:TextBox>
                                                        <f:DropDownList ID="ddlDispatcher" runat="server" Label="调度员"></f:DropDownList>
                                                        <f:Label Hidden="true" runat="server"></f:Label>
                                                    </Items>
                                                </f:FormRow>
                                                <f:FormRow ID="FormRow2">
                                                    <Items>
                                                        <f:TextBox ID="txtFRepairQty" runat="server" Label="修理（非完好）" LabelAlign="Right">
                                                        </f:TextBox>
                                                        <f:TextBox ID="txtFAccidentQty" runat="server" Label="事故（非完好）" LabelAlign="Right">
                                                        </f:TextBox>
                                                        <f:TextBox ID="txtFOtherQty" runat="server" Label="其他（非完好）" LabelAlign="Right">
                                                        </f:TextBox>
                                                        <f:TextBox ID="txtFIDepartureTime" runat="server" Label="辆次（市内）" LabelAlign="Right">
                                                        </f:TextBox>
                                                    </Items>
                                                </f:FormRow>
                                                <f:FormRow ID="FormRow3">
                                                    <Items>
                                                        <f:TextBox ID="txtFIRange" runat="server" Label="里程（市内）" LabelAlign="Right">
                                                        </f:TextBox>
                                                        <f:TextBox ID="txtFIQty" runat="server" Label="货运量（市内）" LabelAlign="Right">
                                                        </f:TextBox>
                                                        <f:TextBox ID="txtFODepartureTime" runat="server" Label="辆次（外省）" LabelAlign="Right">
                                                        </f:TextBox>
                                                        <f:TextBox ID="txtFORange" runat="server" Label="里程（外省）" LabelAlign="Right">
                                                        </f:TextBox>
                                                    </Items>
                                                </f:FormRow>
                                                <f:FormRow ID="FormRow4">
                                                    <Items>
                                                        <f:TextBox ID="txtFOQty" runat="server" Label="货运量（外省）" LabelAlign="Right">
                                                        </f:TextBox>
                                                        <f:TextBox ID="txtFHeavyTruckQty" runat="server" Label="重车行程" LabelAlign="Right">
                                                        </f:TextBox>
                                                        <f:TextBox ID="txtFSumQty" runat="server" Label="总行程" LabelAlign="Right">
                                                        </f:TextBox>
                                                        <f:TextBox ID="txtFTransport" runat="server" Label="货运量(吨)" LabelAlign="Right">
                                                        </f:TextBox>
                                                    </Items>
                                                </f:FormRow>
                                                <f:FormRow ID="FormRow5">
                                                    <Items>
                                                        <f:TextBox ID="txtFTurnover" runat="server" Label="货运周转量（吨）" LabelAlign="Right">
                                                        </f:TextBox>
                                                        <f:Label Hidden="true" runat="server"></f:Label>
                                                        <f:Label Hidden="true" runat="server"></f:Label>
                                                        <f:Label Hidden="true" runat="server"></f:Label>
                                                    </Items>
                                                </f:FormRow>
                                                <f:FormRow ID="FormRow6">
                                                    <Items>
                                                        <f:TextArea ID="txtFMemo" runat="server" Label="摘要"></f:TextArea>
                                                        
                                                    </Items>
                                                </f:FormRow>
                                            </Rows>
                                        </f:Form>
                                    </Items>
                                </f:Region>
                                <f:Region ID="Region4" ShowBorder="False" ShowHeader="false" Position="Center"
                                    Layout="Fit" runat="server" BodyPadding="3px" AutoScroll="True">
                                    <Items>
                                        <f:Grid ID="Grid1" PageSize="20"
                                            ShowBorder="False"
                                            ShowHeader="False"
                                            AllowPaging="False"
                                            EnableAfterEditEvent="False"
                                            OnAfterEdit="Grid1_AfterEdit"
                                            runat="server"
                                            EnableCheckBoxSelect="True"
                                            DataKeyNames="FId"
                                            EnableSummary="True"
                                            SummaryPosition="Bottom"
                                            OnRowCommand="Grid1_RowCommand"
                                            AllowSorting="False"
                                            EmptyText="查询无结果"
                                            EnableHeaderMenu="True"
                                            AllowCellEditing="true"
                                            EnableRowDoubleClickEvent="true" 
                                            OnRowDoubleClick="Grid1_RowDoubleClick"
                                            ClicksToEdit="1">
                                            <Toolbars>
                                                <f:Toolbar ID="Toolbar1" runat="server">
                                                    <Items>
                                                        <f:Button ID="btnSubmit" Text="保存" runat="server" Icon="SystemSaveNew" ValidateForms="SimpleForm1"
                                                            OnClick="btnSubmit_Click" Size="Medium">
                                                        </f:Button>
                                                        <%--<f:Button ID="btnPrint" runat="server" Text="打印" OnClick="btnPrint_Click"
                                                            Icon="Printer" Hidden="False" Size="Medium">
                                                        </f:Button>--%>
                                                        <f:Button ID="btnClose" EnablePostBack="False" Text="关闭" OnClientClick="closeActiveTab();" runat="server" Icon="SystemClose" Size="Medium">
                                                        </f:Button>
                                                    </Items>
                                                </f:Toolbar>
                                            </Toolbars>
                                            <Columns>

                                                <f:LinkButtonField TextAlign="Center" ConfirmText="删除选中行？" Icon="Delete" HeaderText="删除" ConfirmTarget="Top"
                                                    ColumnID="colDelete" Width="55px" CommandName="Delete" Hidden="False" SortField="FVehicleNum">
                                                </f:LinkButtonField>

                                                <f:BoundField MinWidth="130px" ColumnID="FVehicleNum" DataField="FVehicleNum" HeaderText="车号" SortField="FVehicleNum" />
                                                
                                                <f:BoundField MinWidth="80px" ColumnID="FVehicleType" DataField="FVehicleType" HeaderText="车型" SortField="FVehicleType" />
                                                <f:BoundField MinWidth="80px" DataField="FTonnage" HeaderText="吨位" SortField="FTonnage" TextAlign="Center" />
                                                <f:BoundField MinWidth="80px" DataField="FOperationCertificateNo" HeaderText="营运证号" SortField="FOperationCertificateNo" TextAlign="Center" />
                                                <f:BoundField MinWidth="80px" DataField="FRiskType" HeaderText="危险类别" SortField="FRiskType" TextAlign="Center" />
                                                <f:BoundField MinWidth="80px" DataField="FItem" HeaderText="物品名装" SortField="FItem" TextAlign="Center" />
                                                <f:BoundField MinWidth="80px" DataField="FBottleQty" HeaderText="包装" SortField="FBottleQty" TextAlign="Center" />
                                                <f:BoundField MinWidth="80px" DataField="FNumber" HeaderText="件数" SortField="FNumber" TextAlign="Center" />
                                                <f:BoundField MinWidth="80px" ColumnID="FActual" DataField="FActual" HeaderText="实载吨位" SortField="FActual" TextAlign="Center" />
                                                <f:BoundField MinWidth="80px" DataField="FFrom" HeaderText="运行起点" SortField="FFrom" TextAlign="Center" />
                                                <f:BoundField MinWidth="80px" DataField="FTo" HeaderText="运行止点" SortField="FTo" TextAlign="Center" />
                                                <f:BoundField MinWidth="80px" ColumnID="FMileage" DataField="FMileage" HeaderText="量程" SortField="FTo" TextAlign="Center" />
                                                <f:BoundField MinWidth="80px" DataField="FDriver" HeaderText="驾驶员" SortField="FDriver" TextAlign="Center" />
                                                <f:BoundField MinWidth="80px" DataField="FSupercargo" HeaderText="押运员" SortField="FSupercargo" TextAlign="Center" />
                                                <f:BoundField MinWidth="80px" ColumnID="FTimes" DataField="FTimes" HeaderText="出车次数" SortField="FTimes" TextAlign="Center" />
                                                <f:BoundField MinWidth="80px" DataField="FLogistics" HeaderText="托运单位" SortField="FLogistics" TextAlign="Center" />
                                            </Columns>
                                            <Listeners>
                                                <f:Listener Event="beforeedit" Handler="Gbeforeedit" />
                                                <f:Listener Event="edit" Handler="onGridAfterEdit" />
                                            </Listeners>
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

        <f:Window ID="Window1" runat="server" WindowPosition="Center" IsModal="true" Title="产品档案"
            Icon="ApplicationViewDetail" EnableMaximize="true" EnableResize="true" Hidden="True"
            Target="Parent" EnableIFrame="true" IFrameUrl="about:blank" Height="480px" Width="800px"
            OnClose="Window1_Close">
        </f:Window>
        <f:Window ID="Window2" runat="server" WindowPosition="Center" IsModal="true"
            Icon="ApplicationViewDetail" EnableMaximize="true" EnableResize="true" Hidden="True" EnableClose="true"
            Target="Parent" EnableIFrame="true" Title="客户档案"
            IFrameUrl="about:blank" Height="480px" Width="800px" OnClose="Window1_Close">
        </f:Window>
        <f:Window ID="Window3" runat="server" WindowPosition="Center" IsModal="true"
            Icon="ApplicationViewDetail" EnableMaximize="true" EnableResize="true" Hidden="True" EnableClose="true"
            Target="Parent" EnableIFrame="true" Title="联系方式"
            IFrameUrl="about:blank" Height="480px" Width="800px" OnClose="Window1_Close">
        </f:Window>
        <f:Window ID="Window4" runat="server" WindowPosition="Center" IsModal="true"
            Icon="ApplicationViewDetail" EnableMaximize="true" EnableResize="true" Hidden="True" EnableClose="true"
            Target="Parent" EnableIFrame="true" Title="销售订单"
            IFrameUrl="about:blank" Height="480px" Width="880px" OnClose="Window1_Close">
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

        var gridClientID = '<%= Grid1.ClientID %>';
        var inputselector = '.f-grid-tpl input';

        F.ready(function () {

        });

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
