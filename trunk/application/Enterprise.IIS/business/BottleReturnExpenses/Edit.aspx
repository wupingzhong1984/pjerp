<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Enterprise.IIS.business.BottleReturnExpenses.Edit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>容器回收检查处理单</title>
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
                <f:Region ID="Region1" Title="容器回收检查处理单" Position="Center"
                    ShowBorder="false" BoxConfigAlign="Stretch" Layout="Fit" runat="server" ShowHeader="False">
                    <Items>
                        <f:RegionPanel ID="RegionPanel2" ShowBorder="false" runat="server">
                            <Regions>
                                <f:Region ID="Region3" ShowBorder="false" ShowHeader="True" Title="编辑容器回收检查处理单" Split="False"
                                    Position="Top" Layout="Fit"
                                    BodyPadding="3px" runat="server" Height="242px" EnableCollapse="False">
                                    <Items>
                                        <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="80px"
                                            runat="server">
                                            <Rows>
                                                <f:FormRow ID="FormRow0">
                                                    <Items>
                                                        <f:TextBox ID="txtKeyId" runat="server" Label="单据号" EmptyText="单据号" Readonly="True" LabelAlign="Right" Enabled="False">
                                                        </f:TextBox>
                                                        <f:TextBox ID="txtCreateBy" runat="server" Label="制单人" EmptyText="制单人" Readonly="True" LabelAlign="Right" Enabled="False">
                                                        </f:TextBox>
                                                        <f:DatePicker ID="txtFDate" runat="server" Required="false" Label="配送日期" EmptyText="日期" LabelAlign="Right"
                                                            ShowRedStar="false">
                                                        </f:DatePicker>
                                                        <f:TextBox ID="txtFReconciliation" runat="server" Label="对账单号" EmptyText="对账单号" LabelAlign="Right" />
                                                    </Items>
                                                </f:FormRow>
                                                <f:FormRow ID="FormRow1">
                                                    <Items>
                                                        <f:TextBox runat="server" Label="客户代码" ID="txtFCode" Required="True" LabelAlign="Right" />
                                                        <f:TriggerBox ID="tbxFCustomer" EnablePostBack="True" OnTextChanged="tbxFCustomer_OnTextChanged"
                                                            ShowLabel="true" ShowRedStar="True" Required="true" Label="客户名称" LabelAlign="Right"
                                                            Readonly="false" TriggerIcon="Search" runat="server" AutoPostBack="True">
                                                        </f:TriggerBox>
                                                        <f:TriggerBox ID="txtFAddress" EnablePostBack="True" OnTriggerClick="txtFAddress_OnTriggerClick"
                                                            ShowLabel="true" Label="客户地址" LabelAlign="Right" AutoPostBack="True"
                                                            Readonly="false" TriggerIcon="Search" runat="server">
                                                        </f:TriggerBox>
                                                        <f:TextBox ID="txtFPhone" runat="server" Label="电话" EmptyText="电话" LabelAlign="Right">
                                                        </f:TextBox>
                                                    </Items>
                                                </f:FormRow>
                                                <f:FormRow ID="FormRow2">
                                                    <Items>
                                                        <f:TextBox ID="txtFLinkman" runat="server" Label="联系人" EmptyText="联系人" LabelAlign="Right">
                                                        </f:TextBox>
                                                        <f:TextBox ID="txtFFreight" runat="server" Label="运输服务费" Text="" LabelAlign="Right" Readonly="True">
                                                        </f:TextBox>
                                                        <f:DropDownList runat="server" ID="ddlFVehicleNum" Label="车牌号" EnableEdit="True" LabelAlign="Right" />
                                                        <f:DropDownList runat="server" ID="ddlFDriver" Label="送货司机" EnableEdit="True" EnableMultiSelect="true" LabelAlign="Right" />
                                                    </Items>
                                                </f:FormRow>
                                                <f:FormRow ID="FormRow3">
                                                    <Items>
                                                        <f:DropDownList runat="server" ID="ddlFSupercargo" Label="押运员" EnableEdit="True" EnableMultiSelect="true" LabelAlign="Right" />
                                                        <f:DropDownList runat="server" Required="True" ShowRedStar="True" ID="ddlFShipper" Label="发货人" EnableEdit="True" LabelAlign="Right" />
                                                        <f:DropDownList runat="server" ID="ddlFReceiver" Label="收货人" EnableEdit="True" LabelAlign="Right" />

                                                        <%--<f:DropDownList runat="server" ID="ddlFArea" Label="区域" EnableEdit="True" LabelAlign="Right" />--%>
                                                        <f:DropDownList runat="server" ID="ddlFSalesman" Required="True" ShowRedStar="True" Label="业务员" EnableEdit="True" LabelAlign="Right" />
                                                    </Items>
                                                </f:FormRow>
                                                <f:FormRow ID="FormRow4">
                                                    <Items>
                                                        <f:DropDownList runat="server" ID="ddlDeliveryMethod" Label="配送方式"
                                                            Required="True" LabelAlign="Right" ShowRedStar="True" />
                                                        <f:TriggerBox ID="tbxFLogisticsNumber" EnablePostBack="True" OnTextChanged="tbxFLogisticsNumber_OnTextChanged"
                                                            ShowLabel="true" Label="销售订单" LabelAlign="Right"  Readonly="True"
                                                            TriggerIcon="Search" runat="server" AutoPostBack="True">
                                                        </f:TriggerBox>
                                                        <f:DropDownList runat="server" ID="ddlSubject" Label="银行账户" EnableEdit="False" LabelAlign="Right">
                                                        </f:DropDownList>
                                                        <f:NumberBox ID="txtFAmt" runat="server" Label="实收金额" EmptyText="实收金额" Text="" LabelAlign="Right">
                                                        </f:NumberBox>
                                                    </Items>
                                                </f:FormRow>
                                                <f:FormRow ID="FormRow5">
                                                    <Items>
                                                        <f:NumberBox ID="txtFDiscountAmount" runat="server" Label="优惠金额" EmptyText="优惠金额" Text="" LabelAlign="Right">
                                                        </f:NumberBox>
                                                        <f:DropDownList runat="server" ID="ddlFT6SaleType" Label="销售类型"
                                                            Required="True" LabelAlign="Right" ShowRedStar="True" />
                                                        <f:DropDownList runat="server" ID="ddlT6ReceiveSendType" Label="收发类型"
                                                            Required="True" LabelAlign="Right" ShowRedStar="True" EnableSimulateTree="true" />
                                                        <f:DropDownList runat="server" ID="ddlFT6Currency" Label="币种"
                                                            Required="True" LabelAlign="Right" ShowRedStar="True" />
                                                    </Items>
                                                </f:FormRow>
                                                <f:FormRow ID="FormRow6">
                                                    <Items>
                                                        <f:DropDownList runat="server" ID="ddlFDistributionPoint" Label="作业区" Required="True" LabelAlign="Right" ShowRedStar="True" EnableEdit="True" />
                                                        <f:TextBox ID="txtFT6ExchangeRate" runat="server" Label="汇率" Required="True" ShowRedStar="True" EmptyText="汇率" LabelAlign="Right">
                                                        </f:TextBox>
                                                        <f:TextBox ID="txtFMemo" MaxLength="200" MaxLengthMessage="最大长度限定200个字符以内" LabelAlign="Right" runat="server" Label="备注" EmptyText="备注">
                                                        </f:TextBox>
                                                        <f:FileUpload runat="server" ID="filePhoto" ShowRedStar="false" ButtonText="上传附件" LabelAlign="Right"
                                                            ButtonIcon="Image" Required="false" ShowLabel="True" Label="上传附件" AutoPostBack="true" ButtonOnly="True"
                                                            OnFileSelected="filePhoto_FileSelected">
                                                        </f:FileUpload>
                                                    </Items>
                                                </f:FormRow>
                                                <f:FormRow ID="FormRow7">
                                                    <Items>
                                                        <f:Label ID="lblfile" Label="附件" EncodeText="false" runat="server" LabelAlign="Right"></f:Label>
                                                        <f:DropDownList runat="server" ID="ddlFOrgName" Label="所属部门" EnableEdit="False" LabelAlign="Right">
                                                        </f:DropDownList>

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
                                            DataKeyNames="KeyId,FId"
                                            EnableSummary="False"
                                            SummaryPosition="Bottom"
                                            OnRowCommand="Grid1_RowCommand"
                                            AllowSorting="False"
                                            EmptyText="查询无结果"
                                            EnableHeaderMenu="True"
                                            AllowCellEditing="true"
                                            ClicksToEdit="1">
                                            <Toolbars>
                                                <f:Toolbar ID="Toolbar1" runat="server">
                                                    <Items>
                                                        <f:Button ID="btnAddProduct" runat="server" Text="新增" OnClick="btnAddProduct_OnClick" Icon="Add" Size="Medium">
                                                        </f:Button>
                                                        <f:Button ID="btnPassCardToSales" Hidden="True" runat="server" Text="订单To销售"
                                                            Icon="Add" Size="Medium" OnClick="btnPassCardToSales_Click">
                                                        </f:Button>
                                                        <f:Button ID="btnSubmit" Text="保存" runat="server" Icon="SystemSaveNew" ValidateForms="SimpleForm1"
                                                            OnClick="btnSubmit_Click" Size="Medium">
                                                        </f:Button>
                                                        <f:Button ID="btnCopy" runat="server" Text="复制上次订单"
                                                            OnClick="btnCopy_Click" Icon="PageCopy" Hidden="True" Size="Medium">
                                                        </f:Button>
                                                        <f:Button ID="btnPrint" runat="server" Text="打印" OnClick="btnPrint_Click"
                                                            Icon="Printer" Hidden="True" Size="Medium">
                                                        </f:Button>
                                                        <f:Button ID="btnClose" EnablePostBack="False" Text="关闭" OnClientClick="closeActiveTab();" runat="server" Icon="SystemClose" Size="Medium">
                                                        </f:Button>
                                                    </Items>
                                                </f:Toolbar>
                                            </Toolbars>
                                            <Columns>
                                                <f:LinkButtonField TextAlign="Center" ConfirmText="删除选中行？" Icon="Delete" HeaderText="删除" ConfirmTarget="Top"
                                                    ColumnID="colDelete" Width="55px" CommandName="Delete" Hidden="False" SortField="mc">
                                                </f:LinkButtonField>
                                                <f:RenderField MinWidth="100px" Hidden="True" ColumnID="FItemCode" DataField="FItemCode" FieldType="String"
                                                    HeaderText="编码">
                                                    <Editor>
                                                        <f:TriggerBox ID="tbxFItemCode" OnTriggerClick="tbxFItemCode_OnTriggerClick" Readonly="false" TriggerIcon="Search" runat="server" EnablePostBack="True">
                                                        </f:TriggerBox>
                                                    </Editor>
                                                </f:RenderField>
                                                <f:RenderField MinWidth="130px" Hidden="True" ColumnID="FT6Warehouse" DataField="FT6Warehouse" FieldType="String"
                                                    HeaderText="仓库">
                                                    <Editor>
                                                        <f:DropDownList runat="server" ID="tbxFWarehouse"></f:DropDownList>
                                                    </Editor>
                                                </f:RenderField>

                                                <f:BoundField MinWidth="130px" DataField="FItemName" HeaderText="名称" SortField="FItemName" />
                                                <f:BoundField MinWidth="80px" DataField="FSpec" HeaderText="规格" SortField="FSpec" />
                                                <f:BoundField MinWidth="80px" Hidden="True" DataField="FUnit" HeaderText="单位" SortField="FUnit" TextAlign="Center" />
                                                <f:RenderField MinWidth="100px" ColumnID="FQty" DataField="FQty" FieldType="Double" TextAlign="Right"
                                                    HeaderText="容器数量">
                                                    <Editor>
                                                        <f:TextBox ID="tbxFQty" Required="true" runat="server">
                                                        </f:TextBox>
                                                    </Editor>
                                                </f:RenderField>
                                                <f:GroupField HeaderText="无气瓶" TextAlign="Center" EnableLock="true" SortField="mc">
                                                    <Columns>
                                                        <f:RenderField MinWidth="50px" ColumnID="FBottleQty" DataField="FBottleQty" FieldType="Double" TextAlign="Right"
                                                            HeaderText="数量">
                                                            <Editor>
                                                                <f:TextBox ID="tbxFBottleQty" Required="true" runat="server">
                                                                </f:TextBox>
                                                            </Editor>
                                                        </f:RenderField>
                                                        <f:RenderField MinWidth="50px" ColumnID="FReturnQty" DataField="FReturnQty" FieldType="Int" TextAlign="Right"
                                                            HeaderText="处理费">
                                                            <Editor>
                                                                <f:TextBox ID="tbxFReturnQty" Required="true" runat="server">
                                                                </f:TextBox>
                                                            </Editor>
                                                        </f:RenderField>
                                                    </Columns>
                                                </f:GroupField>
                                                <f:GroupField HeaderText="外来瓶修理" TextAlign="Center" EnableLock="true" SortField="mc">
                                                    <Columns>
                                                        <f:RenderField MinWidth="50px" ColumnID="FRecycleQty" DataField="FRecycleQty" FieldType="Int" TextAlign="Right"
                                                            HeaderText="数量">
                                                            <Editor>
                                                                <f:TextBox ID="tbxFRecycleQty" Required="true" runat="server">
                                                                </f:TextBox>
                                                            </Editor>
                                                        </f:RenderField>
                                                        <f:RenderField MinWidth="50px" ColumnID="FPrice" DataField="FPrice" FieldType="Double" TextAlign="Right"
                                                            HeaderText="修理费">
                                                            <Editor>
                                                                <f:NumberBox ID="tbxFPrice" Required="true" runat="server" DecimalPrecision="6">
                                                                </f:NumberBox>
                                                            </Editor>
                                                        </f:RenderField>
                                                    </Columns>
                                                </f:GroupField>
                                                <f:GroupField HeaderText="外来瓶改制" TextAlign="Center" EnableLock="true" SortField="mc">
                                                    <Columns>
                                                        <f:RenderField MinWidth="50px" ColumnID="FOutTemperature" DataField="FOutTemperature" FieldType="Double" TextAlign="Right"
                                                            HeaderText="数量">
                                                            <Editor>
                                                                <f:TextBox ID="FOutTemperature" Required="true" runat="server">
                                                                </f:TextBox>
                                                            </Editor>
                                                        </f:RenderField>
                                                        <f:RenderField MinWidth="50px" ColumnID="FInTemperature" DataField="FInTemperature" TextAlign="Right"
                                                            HeaderText="改制费">
                                                            <Editor>
                                                                <f:TriggerBox ID="FInTemperature" Readonly="false" runat="server" EnablePostBack="False">
                                                                </f:TriggerBox>
                                                            </Editor>
                                                        </f:RenderField>
                                                    </Columns>
                                                </f:GroupField>
                                                <f:GroupField HeaderText="外来瓶折旧" TextAlign="Center" EnableLock="true" SortField="mc">
                                                    <Columns>
                                                        <f:RenderField MinWidth="50px" ColumnID="FInPressure" DataField="FInPressure" TextAlign="Right"
                                                            HeaderText="数量">
                                                            <Editor>
                                                                <f:TextBox ID="FInPressure" Required="true" runat="server">
                                                                </f:TextBox>
                                                            </Editor>
                                                        </f:RenderField>
                                                        <f:RenderField MinWidth="50px" ColumnID="FOutPressure" DataField="FOutPressure" TextAlign="Right"
                                                            HeaderText="折旧费">
                                                            <Editor>
                                                                <f:TextBox ID="FOutPressure" Required="true" runat="server">
                                                                </f:TextBox>
                                                            </Editor>
                                                        </f:RenderField>
                                                    </Columns>
                                                </f:GroupField>
                                                <f:RenderField MinWidth="160px" ColumnID="FMemo" DataField="FMemo"
                                                    HeaderText="备注说明">
                                                    <Editor>
                                                        <f:TextBox ID="tbxFMemo" Required="true" runat="server">
                                                        </f:TextBox>
                                                    </Editor>
                                                </f:RenderField>
                                                <f:BoundField MinWidth="120px" Hidden="True" DataField="FCateName" HeaderText="类型" SortField="FCateName" />
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
            IFrameUrl="about:blank" Height="480px" Width="980px" OnClose="Window1_Close">
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
            var txtcode = '<%= tbxFCustomer.ClientID %>';
            $('#' + txtcode + ' input').autocomplete({
                source: function (request, response) {
                    $.getJSON("../../Common/AjaxCustomer.ashx", request, function (data, status, xhr) {
                        response(data);
                    });
                }
            }, 1000);

            F('<% = Grid1.ClientID %>').on('edit', function (editor, e) {
                if (e.field == 'FItemCode') {
                    window._selectrowIndex = e.rowIdx;
                    window._selectcellIndex = e.colIdx;
                    window.setTimeout(function () {
                        $("#<% =tbxFItemCode.ClientID %> input").autocomplete({
                            source: function (request, response) {
                                $.getJSON("../../Common/AjaxProduct.ashx", request, function (data, status, xhr) {
                                    response(data);
                                });
                            }
                        });
                    }, 1000);
                }
                return true;
            });
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
            //var me = this, columnId = params.column.id, rowId = params.record.getId();
            //if (columnId === 'FQty' || columnId === 'FPrice') {
            //    var fQty = parseFloat(me.f_getCellValue(rowId, 'FQty'));
            //    me.f_updateCellValue(rowId, 'FBottleQty', fQty);
            //    var fPrice = parseFloat(me.f_getCellValue(rowId, 'FPrice'));
            //    me.f_updateCellValue(rowId, 'FAmount', fQty * fPrice).toFixed(2);

            //    updateSummary();
            //}
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
