<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Enterprise.IIS.business.StockIn.Edit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>入仓单</title>
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

        #RegionPanel1_Region1_RegionPanel2_Region3_SimpleForm1_FormRow0_ddlFCate-labelEl {
            color: red;
        }

        #RegionPanel1_Region1_RegionPanel2_Region3_SimpleForm1_FormRow4_ddlDeliveryMethod-labelEl {
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
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region1" Title="入仓单" Position="Center"
                    ShowBorder="false" BoxConfigAlign="Stretch" Layout="Fit" runat="server" ShowHeader="False">
                    <Items>
                        <f:RegionPanel ID="RegionPanel2" ShowBorder="false" runat="server">
                            <Regions>
                                <f:Region ID="Region3" ShowBorder="false" ShowHeader="True" Title="编辑入仓单" Split="False"
                                    Position="Top" Layout="Fit"
                                    BodyPadding="3px" runat="server" Height="132px" EnableCollapse="False">
                                    <Items>
                                        <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="80px"
                                            runat="server" LabelAlign="Right">
                                            <Rows>
                                                <f:FormRow ID="FormRow1">
                                                    <Items>
                                                        <f:TextBox ID="txtKeyId" runat="server" Label="单据号" EmptyText="单据号" Readonly="True" Enabled="False">
                                                        </f:TextBox>
                                                        <f:TextBox ID="txtCreateBy" runat="server" Label="制单人" EmptyText="制单人" Readonly="True">
                                                        </f:TextBox>
                                                        <f:DatePicker ID="txtFDate" runat="server" Required="false" Label="日期" EmptyText="日期"
                                                            ShowRedStar="false">
                                                        </f:DatePicker>
                                                        <f:DropDownList runat="server" ID="ddlFCate" Label="入仓类型" EnableEdit="True" Required="True" ShowRedStar="True" />
                                                    </Items>
                                                </f:FormRow>
                                                <f:FormRow ID="FormRow2">
                                                    <Items>
                                                        <f:DropDownList runat="server" ID="ddlT6ReceiveSendType" Label="收发类型"
                                                            Required="True" LabelAlign="Right" ShowRedStar="True" EnableSimulateTree="true" />
                                                        <f:DropDownList runat="server" ID="ddlFWarehouse" Label="仓库"
                                                            Required="True" LabelAlign="Right" ShowRedStar="True" EnableSimulateTree="true" />
                                                        <f:DropDownList runat="server" ID="ddlFSalesman" Label="业务员" LabelAlign="Right" />
                                                        <f:TextBox ID="txtFMemo" MaxLength="200" MaxLengthMessage="最大长度限定200个字符以内" runat="server" Label="备注" EmptyText="备注">
                                                        </f:TextBox>
                                                    </Items>
                                                </f:FormRow>
                                                <f:FormRow ID="FormRow3">
                                                    <Items>
                                                        <f:DropDownList runat="server" ID="ddlFGroup" Label="班组"
                                                            Required="True" LabelAlign="Right" ShowRedStar="True" EnableSimulateTree="true" />
                                                        <f:DropDownList runat="server" ID="ddlFDistributionPoint" Label="作业区" Required="True" LabelAlign="Right" ShowRedStar="True" EnableEdit="True" />
                                                        <f:Label runat="server" Hidden="true"></f:Label>
                                                        <f:Label runat="server" Hidden="true"></f:Label>
                                                    </Items>
                                                </f:FormRow>
                                            </Rows>
                                        </f:Form>
                                    </Items>
                                </f:Region>
                                <f:Region ID="Region4" ShowBorder="False" ShowHeader="false" Position="Center"
                                    Layout="Fit" runat="server" BodyPadding="3px" AutoScroll="True">
                                    <Items>
                                        <f:Grid ID="Grid1" PageSize="20" ShowBorder="False" ShowHeader="False" AllowPaging="False" EnableAfterEditEvent="true" OnAfterEdit="Grid1_AfterEdit"
                                            runat="server" EnableCheckBoxSelect="True" DataKeyNames="KeyId,FId" EnableSummary="True" SummaryPosition="Bottom" OnRowCommand="Grid1_RowCommand"
                                            AllowSorting="False" EmptyText="查询无结果" EnableHeaderMenu="True" AllowCellEditing="true" ClicksToEdit="1"                                            
                                            EnableRowDoubleClickEvent="true" 
                                            OnRowDoubleClick="Grid1_RowDoubleClick">
                                            <Toolbars>
                                                <f:Toolbar ID="Toolbar1" runat="server">
                                                    <Items>
                                                        <f:Button ID="btnAdd" runat="server" Text="新增" Icon="Add" Size="Medium" EnablePostBack="False">
                                                        </f:Button>
                                                        <f:Button ID="btnSubmit" Text="保存" runat="server" Icon="SystemSaveNew" ValidateForms="SimpleForm1"
                                                            OnClick="btnSubmit_Click" Size="Medium">
                                                        </f:Button>
                                                        <f:Button ID="btnPrint" runat="server" Text="打印" OnClick="btnPrint_Click"
                                                            Icon="Printer" Hidden="False" Size="Medium">
                                                        </f:Button>
                                                        <f:Button ID="btnClose" EnablePostBack="False" Text="关闭" OnClientClick="closeActiveTab();" runat="server" Icon="SystemClose" Size="Medium">
                                                        </f:Button>
                                                    </Items>
                                                </f:Toolbar>
                                            </Toolbars>
                                            <Columns>
                                                <f:LinkButtonField TextAlign="Center" ConfirmText="删除选中行？" Icon="Delete" HeaderText="删除" ConfirmTarget="Top"
                                                    ColumnID="colDelete" Width="45px" CommandName="Delete" Hidden="False" SortField="mc">
                                                </f:LinkButtonField>
                                                <f:RenderField MinWidth="100px" ColumnID="FItemCode" DataField="FItemCode" FieldType="String"
                                                    HeaderText="编码">
                                                    <Editor>
                                                        <f:TriggerBox ID="tbxFItemCode" OnTriggerClick="tbxFItemCode_OnTriggerClick" Readonly="false" TriggerIcon="Search" runat="server" EnablePostBack="False">
                                                        </f:TriggerBox>
                                                    </Editor>
                                                </f:RenderField>
                                                <f:BoundField MinWidth="80px" DataField="FItemName" HeaderText="名称" SortField="FItemName" />
                                                <f:BoundField MinWidth="80px" DataField="FSpec" HeaderText="规格" SortField="FSpec" />
                                                <f:BoundField MinWidth="80px" DataField="FUnit" HeaderText="单位" SortField="FUnit" TextAlign="Center" />
                                                <f:RenderField MinWidth="100px" ColumnID="FPrice" DataField="FPrice" FieldType="Double" TextAlign="Right"
                                                    HeaderText="单价" Hidden="true">
                                                    <Editor>
                                                        <f:NumberBox ID="tbxFPrice" Required="true" runat="server">
                                                        </f:NumberBox>
                                                    </Editor>
                                                </f:RenderField>
                                                <f:RenderField MinWidth="100px" ColumnID="FQty" DataField="FQty" FieldType="Double" TextAlign="Right"
                                                    HeaderText="数量">
                                                    <Editor>
                                                        <f:NumberBox ID="tbxFQty" Required="true" runat="server">
                                                        </f:NumberBox>
                                                    </Editor>
                                                </f:RenderField>
                                                <f:RenderField MinWidth="100px" ColumnID="FAmount" DataField="FAmount" FieldType="Double" TextAlign="Right"
                                                    HeaderText="金额" Hidden="true">
                                                    <Editor>
                                                        <f:NumberBox ID="tbxFAmount" Required="true" runat="server" Enabled="false">
                                                        </f:NumberBox>
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
                                                        <f:NumberBox ID="tbxFBottleQty" Required="true" runat="server">
                                                        </f:NumberBox>
                                                    </Editor>
                                                </f:RenderField>
                                                <f:RenderField MinWidth="160px" ColumnID="FMemo" DataField="FMemo"
                                                    HeaderText="备注说明">
                                                    <Editor>
                                                        <f:TextBox ID="tbxFMemo" Required="true" runat="server">
                                                        </f:TextBox>
                                                    </Editor>
                                                </f:RenderField>
                                                <f:BoundField MinWidth="120px" DataField="FBottleOweQty" HeaderText="已提瓶" SortField="FBottleOweQty" TextAlign="Right" Hidden="true" />
                                                <f:BoundField MinWidth="120px" DataField="FCateName" HeaderText="类型" SortField="FCateName" />
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
        <f:Window ID="Window1" runat="server" WindowPosition="Center" IsModal="true"
            Icon="ApplicationViewDetail" EnableMaximize="true" EnableResize="true" Hidden="True"
            Target="Parent" EnableIFrame="true" IFrameUrl="about:blank" Height="480px" Width="960px"
            OnClose="Window1_Close">
        </f:Window>
        <f:Window ID="Window2" runat="server" WindowPosition="Center" IsModal="true"
            Icon="ApplicationViewDetail" EnableMaximize="true" EnableResize="true" Hidden="True" EnableClose="true"
            Target="Parent" EnableIFrame="true" Title="选择客户列表"
            IFrameUrl="about:blank" Height="450px" Width="800px" OnClose="Window1_Close">
        </f:Window>
        <f:Window ID="Window3" runat="server" WindowPosition="Center" IsModal="true"
            Icon="ApplicationViewDetail" EnableMaximize="true" EnableResize="true" Hidden="True" EnableClose="true"
            Target="Parent" EnableIFrame="true" Title="选择客户地址"
            IFrameUrl="about:blank" Height="450px" Width="800px" OnClose="Window1_Close">
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

            ///////////////////////////
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
                    }, 100);
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
            }, 100);
        };

        function onGridAfterEdit(editor, params) {
            var me = this, columnId = params.column.id, rowId = params.record.getId();
            if (columnId === 'FQty' || columnId === 'FPrice') {
                var fQty = parseFloat(me.f_getCellValue(rowId, 'FQty'));
                me.f_updateCellValue(rowId, 'FBottleQty', fQty);
                var fPrice = parseFloat(me.f_getCellValue(rowId, 'FPrice'));
                me.f_updateCellValue(rowId, 'FAmount', fQty * fPrice);
            }
            updateSummary();
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
            $.post("../../common/AjaxPrinter.ashx?oper=ajaxPrintStockIn",
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
    </script>
</body>
</html>
