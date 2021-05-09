<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Enterprise.IIS.business.BottleTo.Edit" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>空瓶出库单</title>
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
        #RegionPanel1_Region1_header {
            background-color: #dfeaf2;
        }
        #component-1021{
            background-color: #dfeaf2;
        }
        #RegionPanel1_Region1_header_hd-textEl {
            color: WindowText;
            font-size: 16px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
        <f:HiddenField runat="server" ID="hfdCate"/>
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region1" Title="空瓶出库单" Position="Center"
                    ShowBorder="false" BoxConfigAlign="Stretch" Layout="Fit" runat="server" ShowHeader="True">
                    <Items>
                        <f:RegionPanel ID="RegionPanel2" ShowBorder="false" runat="server">
                            <Regions>
                                <f:Region ID="Region3" ShowBorder="false" ShowHeader="false" Split="False"
                                    Position="Top" Layout="Fit"
                                    BodyPadding="3px" runat="server" Height="148px" EnableCollapse="True">
                                    <Items>
                                        <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="80px"
                                            runat="server">
                                            <Rows>
                                                <f:FormRow ID="FormRow0">
                                                    <Items>
                                                        <f:TextBox ID="txtKeyId" runat="server" Label="单据号" LabelAlign="Right" EmptyText="单据号" Readonly="True"  Enabled="False">
                                                        </f:TextBox>
                                                        <f:TextBox ID="txtCreateBy" runat="server" Label="制单人" LabelAlign="Right"  EmptyText="制单人" Readonly="True">
                                                        </f:TextBox>
                                                        <f:DatePicker ID="txtFDate" runat="server" Required="false" LabelAlign="Right"  Label="日期" EmptyText="日期"
                                                            ShowRedStar="false">
                                                        </f:DatePicker>
                                                        <f:TextBox ID="txtFReconciliation" runat="server" Label="对账单号" LabelAlign="Right"  EmptyText="对账单号" />
                                                    </Items>
                                                </f:FormRow>
                                                <f:FormRow ID="FormRow1">
                                                    <Items>
                                                        <f:TextBox runat="server" Label="客户代码" ID="txtFCode" LabelAlign="Right"  Required="True"/>
                                                        <f:TriggerBox ID="tbxFCustomer" EnablePostBack="True" OnTextChanged="tbxFCustomer_OnTextChanged"
                                                            ShowLabel="true" ShowRedStar="True" Required="true" Label="客户名称"  LabelAlign="Right" 
                                                            Readonly="false" TriggerIcon="Search" runat="server"  AutoPostBack="True">
                                                        </f:TriggerBox>
                                                        <f:TriggerBox ID="txtFAddress" EnablePostBack="True" LabelAlign="Right"  OnTriggerClick="txtFAddress_OnTriggerClick"
                                                            ShowLabel="true" Label="客户地址" 
                                                            Readonly="false" TriggerIcon="Search" runat="server">
                                                        </f:TriggerBox>
                                                        <f:TextBox ID="txtFPhone" runat="server" Label="电话" EmptyText="电话" LabelAlign="Right" >
                                                        </f:TextBox>

                                                        </Items>
                                                </f:FormRow>
                                                <f:FormRow ID="FormRow2">
                                                    <Items>
                                                        <f:TextBox ID="txtFLinkman" runat="server" Label="联系人" EmptyText="联系人" LabelAlign="Right" >
                                                        </f:TextBox>
                                                        <f:TextBox ID="txtFFreight" runat="server" Label="运输服务费"  LabelAlign="Right" Text="0.00" Enabled="False">
                                                        </f:TextBox>
                                                        <f:DropDownList runat="server" ID="ddlFVehicleNum" Label="车牌号" LabelAlign="Right"  EnableEdit="True" />
                                                        <f:DropDownList runat="server" ID="ddlFDriver" Label="送货司机" LabelAlign="Right"  EnableEdit="True"  EnableMultiSelect="true" />
                                                    </Items>
                                                </f:FormRow>
                                                <f:FormRow ID="FormRow3">
                                                    <Items>
                                                        <f:DropDownList runat="server" ID="ddlFSupercargo" Label="押运员" LabelAlign="Right"  EnableEdit="True" EnableMultiSelect="true" />
                                                        <f:DropDownList runat="server" ID="ddlFShipper" Label="发货人" LabelAlign="Right"  EnableEdit="True" />
                                                        <f:DropDownList runat="server" ID="ddlFArea" Label="区域" LabelAlign="Right"  EnableEdit="True" />
                                                        <f:DropDownList runat="server" ID="ddlFSalesman" Label="业务员" LabelAlign="Right"  EnableEdit="True" /> 
                                                    </Items>
                                                </f:FormRow>
                                                <f:FormRow ID="FormRow4">
                                                    <Items>
                                                        <f:DropDownList runat="server" ID="ddlFDistributionPoint" Label="作业区" Required="True" LabelAlign="Right" ShowRedStar="True" EnableEdit="True" />
                                                        <f:DropDownList runat="server" ID="ddlDeliveryMethod" Label="配送方式" LabelAlign="Right"  Required="True" ShowRedStar="True" EnableEdit="True" />
                                                        <f:TextBox ID="txtFLogisticsNumber" runat="server" Label="物流单号" LabelAlign="Right" >
                                                        </f:TextBox>
                                                        <f:TextBox ID="txtFMemo" MaxLength="200" LabelAlign="Right" MaxLengthMessage="最大长度限定200个字符以内" runat="server" Label="备注" EmptyText="备注">
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
                                        <f:Grid ID="Grid1" PageSize="20" ShowBorder="False" ShowHeader="False" AllowPaging="False" EnableAfterEditEvent="true" OnAfterEdit="Grid1_AfterEdit"
                                            runat="server" EnableCheckBoxSelect="True" DataKeyNames="KeyId,FId" EnableSummary="True" SummaryPosition="Bottom" OnRowCommand="Grid1_RowCommand"
                                            AllowSorting="False" EmptyText="查询无结果" EnableHeaderMenu="True" AllowCellEditing="true" ClicksToEdit="1">
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
                                                <%--<f:RenderField MinWidth="100px" ColumnID="FPrice" DataField="FPrice" FieldType="Double" TextAlign="Right"
                                                    HeaderText="单价">
                                                    <Editor>
                                                        <f:TextBox ID="tbxFPrice" Required="true" runat="server">
                                                        </f:TextBox>
                                                    </Editor>
                                                </f:RenderField>--%>
                                                <f:RenderField MinWidth="100px" ColumnID="FQty" DataField="FQty" FieldType="Double" TextAlign="Right"
                                                    HeaderText="数量">
                                                    <Editor>
                                                        <f:TextBox ID="tbxFQty" Required="true" runat="server">
                                                        </f:TextBox>
                                                    </Editor>
                                                </f:RenderField>

<%--                                                <f:RenderField MinWidth="100px" ColumnID="FAmount" DataField="FAmount" FieldType="Double" TextAlign="Right"
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
                                                </f:RenderField>--%>
                                                <f:RenderField MinWidth="160px" ColumnID="FMemo" DataField="FMemo"
                                                    HeaderText="备注说明">
                                                    <Editor>
                                                        <f:TextBox ID="tbxFMemo" Required="true" runat="server">
                                                        </f:TextBox>
                                                    </Editor>
                                                </f:RenderField>
                                                <%--<f:BoundField MinWidth="120px" DataField="FBottleOweQty" HeaderText="已提瓶" SortField="FBottleOweQty" TextAlign="Right" />
                                                <f:BoundField MinWidth="120px" DataField="FCateName" HeaderText="类型" SortField="FCateName" />--%>
                                            </Columns>
                                            <Listeners>
                                                <f:Listener Event="beforeedit" Handler="Gbeforeedit" />
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
        
        var gridClientID = '<%= Grid1.ClientID %>';
        var inputselector = '.f-grid-tpl input';


        F.ready(function () {

            ///////////////////////////
            //编辑之前的事件
            F('<% = Grid1.ClientID %>').on('edit', function (editor, e) {
                //列名
                if (e.field == 'FItemCode') {
                    //列号
                    window._selectrowIndex = e.rowIdx;
                    window._selectcellIndex = e.colIdx;
                    window.setTimeout(function () {
                        //新增input获取焦点事件，获取焦点后更改则重置grid
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

            var txtcode = '<%= tbxFCustomer.ClientID %>';

            $('#' + txtcode + ' input').autocomplete({
                source: function (request, response) {
                    $.getJSON("../../Common/AjaxCustomer.ashx", request, function (data, status, xhr) {
                        response(data);
                    });
                }
            });

            //window.setInterval(txtcode, 3000);
        });

        function reloadGrid(keys) {
            __doPostBack(null, 'reloadGrid:' + keys);
        };

        function closeActiveTab() {
            parent.removeActiveTab();
        };

        function Gbeforeedit(editor, e, eop) {
            //得到选择器控件
            var edcmp = e.column.getEditor();

            //如果是个text
            if (edcmp.getXType() == "textfield") {
                //选中文字，注意延迟，
                window.setTimeout(function () {
                    edcmp.selectText();
                }, 100);
            }
        };

        //--------打印配置---------------
        var LODOP; //声明为全局变量 
        function LodopPrinter(keyid) {
            LODOP = getLodop();
            LODOP.PRINT_INIT("河南禄恒软件科技有限公司");
            var strHtml = "";
            $.post("../../common/AjaxPrinter.ashx?oper=ajaxPrintPurchase",
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
