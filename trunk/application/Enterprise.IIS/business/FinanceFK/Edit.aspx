<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Enterprise.IIS.business.FinanceFK.Edit" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>付款单</title>
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

        #component-1021 {
            background-color: #dfeaf2;
        }

        #RegionPanel1_Region1_header_hd-textEl {
            color: WindowText;
            font-size: 16px;
        }

        #RegionPanel1_Region1_RegionPanel2_Region3_SimpleForm1_FormRow1_lblFInit-inputEl {
            color: red;
        }

        #RegionPanel1_Region1_RegionPanel2_Region3_SimpleForm1_FormRow1_lblFSales-inputEl {
            color: red;
        }

        #RegionPanel1_Region1_RegionPanel2_Region3_SimpleForm1_FormRow1_lblFSalesReturn-inputEl {
            color: red;
        }

        #RegionPanel1_Region1_RegionPanel2_Region3_SimpleForm1_FormRow2_lblFSK-inputEl {
            color: red;
        }

        #RegionPanel1_Region1_RegionPanel2_Region3_SimpleForm1_FormRow2_lblFEnd-inputEl {
            color: red;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region1" Title="付款单" Position="Center"
                    ShowBorder="false" BoxConfigAlign="Stretch" Layout="Fit" runat="server" ShowHeader="True">
                    <Items>
                        <f:RegionPanel ID="RegionPanel2" ShowBorder="false" runat="server">
                            <Regions>
                                <f:Region ID="Region3" ShowBorder="false" ShowHeader="false" Split="False"
                                    Position="Top" Layout="Fit"
                                    BodyPadding="3px" runat="server" Height="100px" EnableCollapse="True">
                                    <Items>
                                        <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="80px" LabelAlign="Right"
                                            runat="server">
                                            <Rows>
                                                <f:FormRow ID="FormRow0">
                                                    <Items>
                                                        <f:TextBox ID="txtKeyId" runat="server" Label="单据号" EmptyText="单据号" Readonly="True" Enabled="False">
                                                        </f:TextBox>
                                                        <f:TextBox ID="txtCreateBy" runat="server" Label="制单人" EmptyText="制单人" Readonly="True">
                                                        </f:TextBox>
                                                        <f:DatePicker ID="txtFDate" runat="server" Required="false" Label="日期" EmptyText="日期"
                                                            ShowRedStar="false">
                                                        </f:DatePicker>
                                                        <f:TextBox runat="server" Label="供应商编码" ID="txtFCode" Required="True" />
                                                    </Items>
                                                </f:FormRow>
                                                <f:FormRow ID="FormRow1">
                                                    <Items>
                                                        <f:TriggerBox ID="tbxFCustomer" EnablePostBack="True" OnTextChanged="tbxFCustomer_OnTextChanged"
                                                            ShowLabel="true" ShowRedStar="True" Required="true" Label="供应商名称"
                                                            Readonly="false" TriggerIcon="Search" runat="server" AutoPostBack="True">
                                                        </f:TriggerBox>
                                                        <f:Label runat="server" Label="期初应付款" ID="lblFInit" Text="0.00" />
                                                        <f:Label runat="server" Label="本期应付款" ID="lblFSales" Text="0.00" />
                                                        <f:Label runat="server" Label="本期退货款" ID="lblFSalesReturn" Text="0.00" />

                                                    </Items>
                                                </f:FormRow>
                                                <f:FormRow ID="FormRow2">
                                                    <Items>
                                                        <f:Label runat="server" Label="本期已付款" ID="lblFSK" Text="0.00" />
                                                        <f:Label runat="server" Label="期未应付款" ID="lblFEnd" Text="0.00" />
                                                        <f:Label runat="server" Hidden="True" />
                                                        <f:TextBox ID="txtFMemo" MaxLength="200" MaxLengthMessage="最大长度限定200个字符以内" runat="server" Label="备注" EmptyText="备注">
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
                                        <f:Grid ID="Grid1" PageSize="20" MinHeight="220px" ShowBorder="False" ShowHeader="False" AllowPaging="False" EnableAfterEditEvent="true" OnAfterEdit="Grid1_AfterEdit"
                                            runat="server" EnableCheckBoxSelect="True" DataKeyNames="KeyId,FId" EnableSummary="True" SummaryPosition="Bottom" OnRowCommand="Grid1_RowCommand"
                                            AllowSorting="False" EmptyText="查询无结果" EnableHeaderMenu="True" AllowCellEditing="true" ClicksToEdit="1">
                                            <Toolbars>
                                                <f:Toolbar ID="Toolbar1" runat="server">
                                                    <Items>
                                                        <f:Button ID="btnSearch" OnClick="btnSearch_Click" runat="server" Text="统计应收" Icon="Zoom" Hidden="False" Size="Medium">
                                                        </f:Button>
                                                        <f:Button ID="btnAdd" runat="server" Text="新增" Icon="Add" Size="Medium" EnablePostBack="False" OnClientClick="">
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
                                                <f:GroupField HeaderText="银行账户" TextAlign="Center" EnableLock="true" SortField="mc">
                                                    <Columns>
                                                        <f:RenderField MinWidth="180px" ColumnID="FCode" DataField="FCode" FieldType="String"
                                                            HeaderText="编码">
                                                            <Editor>
                                                                <f:TriggerBox ID="tbxBankCode" OnTriggerClick="tbxBankCode_OnTriggerClick" Readonly="false" TriggerIcon="Search" runat="server" EnablePostBack="False">
                                                                </f:TriggerBox>
                                                            </Editor>
                                                        </f:RenderField>
                                                        <f:BoundField MinWidth="220px" ColumnID="FName" DataField="FName" HeaderText="开户银行" SortField="FName" />
                                                        <f:BoundField MinWidth="130px" DataField="FCardNo" HeaderText="账号" SortField="FCardNo" />

                                                        <f:RenderField MinWidth="100px" ColumnID="FDiscountAmount" DataField="FDiscountAmount" FieldType="Double" TextAlign="Right"
                                                            HeaderText="优惠">
                                                            <Editor>
                                                                <f:NumberBox ID="tbxFDiscountAmount" Required="true" runat="server">
                                                                </f:NumberBox>
                                                            </Editor>
                                                        </f:RenderField>


                                                        <f:RenderField MinWidth="100px" ColumnID="FAmt" DataField="FAmt" FieldType="Double" TextAlign="Right"
                                                            HeaderText="付款金额">
                                                            <Editor>
                                                                <f:NumberBox ID="tbxFAmt" Required="true" runat="server">
                                                                </f:NumberBox>
                                                            </Editor>
                                                        </f:RenderField>
                                                    </Columns>
                                                </f:GroupField>
                                                <f:GroupField HeaderText="承兑" TextAlign="Center" EnableLock="true" SortField="mc">
                                                    <Columns>
                                                        <f:RenderField MinWidth="100px" ColumnID="FBankNo" DataField="FBankNo" FieldType="String" TextAlign="Right"
                                                            HeaderText="行号">
                                                            <Editor>
                                                                <f:TextBox ID="txtFBankNo" runat="server">
                                                                </f:TextBox>
                                                            </Editor>
                                                        </f:RenderField>
                                                        <f:RenderField MinWidth="100px" ColumnID="FBillNo" DataField="FBillNo" FieldType="String" TextAlign="Right"
                                                            HeaderText="票据号">
                                                            <Editor>
                                                                <f:TextBox ID="txtFBillNo" runat="server">
                                                                </f:TextBox>
                                                            </Editor>
                                                        </f:RenderField>
                                                        <f:RenderField MinWidth="100px" ColumnID="FExpireDate" DataField="FExpireDate" FieldType="Date" TextAlign="Right"
                                                            HeaderText="到期时间" RendererArgument="yyyy-MM-dd" Renderer="Date">
                                                            <Editor>
                                                                <f:DatePicker ID="dpFExpireDate" runat="server">
                                                                </f:DatePicker>
                                                            </Editor>
                                                        </f:RenderField>
                                                    </Columns>
                                                </f:GroupField>
                                                <f:RenderField MinWidth="160px" ColumnID="FMemo" DataField="FMemo"
                                                    HeaderText="摘要">
                                                    <Editor>
                                                        <f:TextBox ID="tbxFMemo" Required="true" runat="server">
                                                        </f:TextBox>
                                                    </Editor>
                                                </f:RenderField>
                                            </Columns>
                                            <Listeners>
                                                <f:Listener Event="beforeedit" Handler="Gbeforeedit1" />
                                            </Listeners>
                                        </f:Grid>
                                    </Items>
                                </f:Region>
                                <f:Region ID="Region5" ShowBorder="False" ShowHeader="false" Position="Bottom"
                                    MinHeight="200px" Layout="Fit" runat="server" BodyPadding="3px" AutoScroll="True">
                                    <Items>
                                        <f:Grid ID="Grid2" ShowBorder="False" ShowHeader="False" AllowPaging="False" EnableAfterEditEvent="true" OnAfterEdit="Grid1_AfterEdit"
                                            runat="server" EnableCheckBoxSelect="False" DataKeyNames="KeyId,FId" EnableSummary="False" SummaryPosition="Bottom" OnRowCommand="Grid1_RowCommand"
                                            AllowSorting="False" EmptyText="查询无结果" EnableHeaderMenu="False" AllowCellEditing="False" ClicksToEdit="1">
                                            <Columns>
                                                <f:BoundField MinWidth="80px" ID="FDate" DataField="FDate" HeaderText="日期" SortField="FDate" DataFormatString="{0:yyyy-MM-dd}" TextAlign="Left" />
                                                <f:BoundField MinWidth="120px" DataField="KeyId" HeaderText="单据号" SortField="KeyId" />
                                                <f:BoundField MinWidth="80px" DataField="CreateBy" HeaderText="操作员" SortField="CreateBy" />
                                                <f:BoundField MinWidth="80px" ID="FAmount" DataField="FAmount" HeaderText="应付款" SortField="FAmount" />
                                                <f:BoundField MinWidth="80px" DataField="FType" HeaderText="业务类型" SortField="FType" />
                                            </Columns>
                                            <Listeners>
                                                <f:Listener Event="beforeedit" Handler="Gbeforeedit2" />
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
            Target="Parent" EnableIFrame="true" IFrameUrl="about:blank" Height="450px" Width="600px"
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
            //var grid = F(gridClientID);

            //$(grid.el.dom)
            //    .autocomplete({
            //        source: function(request, response) {
            //            $.getJSON("../../Common/AjaxProduct.ashx",
            //                request,
            //                function(data, status, xhr) {
            //                    response(data);
            //                });
            //        }
            //    });

            var txtcode = '<%= tbxFCustomer.ClientID %>';

            $('#' + txtcode + ' input').autocomplete({
                source: function (request, response) {
                    $.getJSON("../../Common/AjaxSupplier.ashx", request, function (data, status, xhr) {
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

        function Gbeforeedit1(editor, e, eop) {
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
        function Gbeforeedit2(editor, e, eop) {
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
            $.post("../../common/AjaxPrinter.ashx?oper=ajaxPrintFK",
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
