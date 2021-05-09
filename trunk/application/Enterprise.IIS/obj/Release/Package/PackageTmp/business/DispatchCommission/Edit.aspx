<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Enterprise.IIS.business.DispatchCommission.Edit" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>送货提成单</title>
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
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region1" Title="送货提成单" Position="Center"
                    ShowBorder="false" BoxConfigAlign="Stretch" Layout="Fit" runat="server" ShowHeader="False">
                    <Items>
                        <f:RegionPanel ID="RegionPanel2" ShowBorder="false" runat="server">
                            <Regions>
                                <f:Region ID="Region3" ShowBorder="false" ShowHeader="True" Title="编辑送货提成单" Split="False"
                                    Position="Top" Layout="Fit"
                                    BodyPadding="3px" runat="server" Height="160px" EnableCollapse="False">
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
                                                        <f:DatePicker ID="txtFDate" runat="server" Required="false" Label="日期" EmptyText="日期" LabelAlign="Right"
                                                            ShowRedStar="false">
                                                        </f:DatePicker>
                                                        <f:TextBox ID="txtFMemo" MaxLength="200" MaxLengthMessage="最大长度限定200个字符以内" LabelAlign="Right" runat="server" Label="备注" EmptyText="备注">
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
                                        <f:Grid ID="Grid1" PageSize="20"
                                            ShowBorder="False"
                                            ShowHeader="False"
                                            AllowPaging="False"
                                            EnableAfterEditEvent="True"
                                            OnAfterEdit="Grid1_AfterEdit"
                                            runat="server"
                                            EnableCheckBoxSelect="True"
                                            DataKeyNames="KeyId,FId"
                                            EnableSummary="True"
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
                                                        
                                                        <f:Button ID="btnAddWorkTask" OnClick="btnAddWorkTask_Click"  runat="server" Text="当日工作量" Icon="Add" Size="Medium" EnablePostBack="False">
                                                        </f:Button>
                                                        
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
                                                    ColumnID="colDelete" Width="55px" CommandName="Delete" Hidden="False" SortField="mc">
                                                </f:LinkButtonField>
                                                <f:RenderField MinWidth="100px" ColumnID="KeyId" DataField="KeyId" FieldType="String"
                                                    HeaderText="单号">
                                                    <Editor>
                                                        <f:TriggerBox ID="tbxKeyId" OnTriggerClick="tbxFItemCode_OnTriggerClick" Readonly="false" TriggerIcon="Search" runat="server" EnablePostBack="True">
                                                        </f:TriggerBox>
                                                    </Editor>
                                                </f:RenderField>
                                                <f:RenderField Width="120px" ColumnID="FDate" DataField="FDate" FieldType="Date"
                                                    Renderer="Date" RendererArgument="yyyy-MM-dd" HeaderText="日期">
                                                    <Editor>
                                                        <f:DatePicker ID="tbxFDate" Required="true" runat="server">
                                                        </f:DatePicker>
                                                    </Editor>
                                                </f:RenderField>
                                                <f:RenderField Width="160px" ColumnID="FCode" DataField="FCode" FieldType="String"
                                                     HeaderText="客户代码">
                                                    <Editor>
                                                        <f:TextBox ID="tbxFCode" Required="true" runat="server">
                                                        </f:TextBox>
                                                    </Editor>
                                                </f:RenderField>
                                                <f:RenderField Width="180px" ColumnID="FName" DataField="FName" FieldType="String"
                                                     HeaderText="客户名称">
                                                    <Editor>
                                                        <f:TextBox ID="tbxFName" Required="true" runat="server">
                                                        </f:TextBox>
                                                    </Editor>
                                                </f:RenderField>
                                                <f:RenderField Width="100px" ColumnID="FArea" DataField="FArea" FieldType="String"
                                                     HeaderText="分区">
                                                    <Editor>
                                                        <f:TextBox ID="tbxFArea" Required="true" runat="server">
                                                        </f:TextBox>
                                                    </Editor>
                                                </f:RenderField>
                                                <f:RenderField Width="160px" ColumnID="FItemName" DataField="FItemName" FieldType="String"
                                                     HeaderText="商品名称">
                                                    <Editor>
                                                        <f:TextBox ID="tbxFItemName" Required="true" runat="server">
                                                        </f:TextBox>
                                                    </Editor>
                                                </f:RenderField>

                                                <f:RenderField MinWidth="100px" ColumnID="FSp" DataField="FSp" FieldType="Int" TextAlign="Right"
                                                    HeaderText="实瓶数量">
                                                    <Editor>
                                                        <f:NumberBox ID="tbxFSp" Required="true" runat="server" DecimalPrecision="0">
                                                        </f:NumberBox>
                                                    </Editor>
                                                </f:RenderField>


                                                <f:RenderField MinWidth="100px" ColumnID="FSpPrice" DataField="FSpPrice" FieldType="Double" TextAlign="Right"
                                                    HeaderText="实瓶工资">
                                                    <Editor>
                                                        <f:NumberBox ID="tbxFSpPrice" Required="true" runat="server" DecimalPrecision="2">
                                                        </f:NumberBox>
                                                    </Editor>
                                                </f:RenderField>

                                                <f:RenderField MinWidth="100px" ColumnID="FKp" DataField="FKp" FieldType="Int" TextAlign="Right"
                                                    HeaderText="空瓶数量">
                                                    <Editor>
                                                        <f:NumberBox ID="tbxFKp" Required="true" runat="server" DecimalPrecision="0">
                                                        </f:NumberBox>
                                                    </Editor>
                                                </f:RenderField>


                                                <f:RenderField MinWidth="100px" ColumnID="FKpPrice" DataField="FKpPrice" FieldType="Double" TextAlign="Right"
                                                    HeaderText="空瓶工资">
                                                    <Editor>
                                                        <f:NumberBox ID="tbxFKpPrice" Required="true" runat="server" DecimalPrecision="2">
                                                        </f:NumberBox>
                                                    </Editor>
                                                </f:RenderField>

                                                <f:RenderField MinWidth="100px" ColumnID="FSumPrice" DataField="FSumPrice" FieldType="Double" TextAlign="Right"
                                                    HeaderText="工资总额">
                                                    <Editor>
                                                        <f:NumberBox ID="tbxFSumPrice" Required="true" runat="server" DecimalPrecision="2">
                                                        </f:NumberBox>
                                                    </Editor>
                                                </f:RenderField>

                                                <f:RenderField MinWidth="100px" ColumnID="FDriver" DataField="FDriver" FieldType="String" TextAlign="Right"
                                                    HeaderText="司机">
                                                    <Editor>
                                                        <f:DropDownList runat="server" ID="ddlFDriver" EnableEdit="True" EnableMultiSelect="true" LabelAlign="Right" />
                                                    </Editor>
                                                </f:RenderField>

                                                <f:RenderField MinWidth="100px" ColumnID="FDriverPrice" DataField="FDriverPrice" FieldType="Double" TextAlign="Right"
                                                    HeaderText="司机工资">
                                                    <Editor>
                                                        <f:NumberBox ID="tbxFDriverPrice" Required="true" runat="server" DecimalPrecision="2">
                                                        </f:NumberBox>
                                                    </Editor>
                                                </f:RenderField>

                                                <f:RenderField MinWidth="100px" ColumnID="FSupercargo" DataField="FSupercargo" FieldType="String" TextAlign="Right"
                                                    HeaderText="押运员">
                                                    <Editor>
                                                        <f:DropDownList runat="server" ID="ddlFSupercargo" EnableEdit="True" EnableMultiSelect="true" LabelAlign="Right" />
                                                    </Editor>
                                                </f:RenderField>

                                                <f:RenderField MinWidth="100px" ColumnID="FSupercargoPrice" DataField="FSupercargoPrice" FieldType="Double" TextAlign="Right"
                                                    HeaderText="押运员工资">
                                                    <Editor>
                                                        <f:NumberBox ID="tbxFSupercargoPrice" Required="true" runat="server" DecimalPrecision="2">
                                                        </f:NumberBox>
                                                    </Editor>
                                                </f:RenderField>

                                                <f:RenderField MinWidth="100px" ColumnID="FVehicleNum" DataField="FVehicleNum" FieldType="Double" TextAlign="Right"
                                                    HeaderText="车牌号">
                                                    <Editor>
                                                        <f:DropDownList runat="server" ID="ddlFVehicleNum" EnableEdit="True" LabelAlign="Right" />
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
            Target="Parent" EnableIFrame="true" IFrameUrl="about:blank" Height="480px" Width="960px"
            OnClose="Window1_Close">
        </f:Window>
        <f:Window ID="Window2" runat="server" WindowPosition="Center" IsModal="true"
            Icon="ApplicationViewDetail" EnableMaximize="true" EnableResize="true" Hidden="True" EnableClose="true"
            Target="Parent" EnableIFrame="true" Title="客户档案"
            IFrameUrl="about:blank" Height="480px" Width="960px" OnClose="Window1_Close">
        </f:Window>
        <f:Window ID="Window3" runat="server" WindowPosition="Center" IsModal="true"
            Icon="ApplicationViewDetail" EnableMaximize="true" EnableResize="true" Hidden="True" EnableClose="true"
            Target="Parent" EnableIFrame="true" Title="联系方式"
            IFrameUrl="about:blank" Height="480px" Width="960px" OnClose="Window1_Close">
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
            }, 100);
        };

        function onGridAfterEdit(editor, params) {
            updateSummary();

            var me = this, columnId = params.column.id, rowId = params.record.getId();
            if (columnId === 'FQty' || columnId === 'FPrice') {
                var fQty = parseFloat(me.f_getCellValue(rowId, 'FQty'));
                me.f_updateCellValue(rowId, 'FBottleQty', fQty);
                var fPrice = parseFloat(me.f_getCellValue(rowId, 'FPrice'));
                me.f_updateCellValue(rowId, 'FAmount', fQty * fPrice).toFixed(3);
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
