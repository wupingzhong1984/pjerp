<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Enterprise.IIS.business.FinanceSKChecked.Edit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>核销收款单</title>
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
                <f:Region ID="Region1" Title="核销收款单" Position="Center"
                    ShowBorder="false" BoxConfigAlign="Stretch" Layout="Fit" runat="server" ShowHeader="True">
                    <Items>
                        <f:RegionPanel ID="RegionPanel2" ShowBorder="false" runat="server">
                            <Regions>
                                <f:Region ID="Region3" ShowBorder="false" ShowHeader="false" Split="False"
                                    Position="Top" Layout="Fit"
                                    BodyPadding="3px" runat="server" Height="96px" EnableCollapse="True">
                                    <Items>
                                        <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="80px" LabelAlign="Right"
                                            runat="server">
                                            <Rows>
                                                <f:FormRow ID="FormRow0">
                                                    <Items>
                                                        <f:TextBox ID="txtKeyId" runat="server" Label="单据号" EmptyText="单据号" Readonly="True"  Enabled="False">
                                                        </f:TextBox>
                                                        <f:TextBox ID="txtCreateBy" runat="server" Label="制单人" EmptyText="制单人" Readonly="True">
                                                        </f:TextBox>
                                                        <f:DatePicker ID="txtFDate" runat="server" Required="false" Label="日期" EmptyText="日期"
                                                            ShowRedStar="false">
                                                        </f:DatePicker>
                                                        <f:TextBox runat="server" Label="客户代码" ID="txtFCode"  Required="True" />
                                                    </Items>
                                                </f:FormRow>
                                                <f:FormRow ID="FormRow1">
                                                    <Items>
                                                        <f:TriggerBox ID="tbxFCustomer" EnablePostBack="True" OnTextChanged="tbxFCustomer_OnTextChanged"
                                                            ShowLabel="true" ShowRedStar="True" Required="true" Label="客户代码"
                                                            Readonly="false" TriggerIcon="Search" runat="server"  AutoPostBack="True">
                                                        </f:TriggerBox>
                                                        <f:Label runat="server" Label="期初应收款" ID="lblFInit" Text="0.00"/>
                                                        <f:Label runat="server" Label="本期应收款" ID="lblFSales" Text="0.00"/>
                                                        <f:Label runat="server" Label="本期退货款" ID="lblFSalesReturn" Text="0.00"/>
                                                        
                                                    </Items>
                                                </f:FormRow>
                                                <f:FormRow ID="FormRow2">
                                                    <Items>
                                                        <f:Label runat="server" Label="本期回收款" ID="lblFSK" Text="0.00"/>
                                                        <f:Label runat="server" Label="期未应收款" ID="lblFEnd" Text="0.00" />
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
                                    Layout="Fit" runat="server" BodyPadding="3px" AutoScroll="True" MinHeight="200px"  MaxHeight="200px">
                                    <Items>
                                        <f:Grid ID="Grid1" PageSize="20" Height="120px" ShowBorder="False" ShowHeader="False" AllowPaging="False" EnableAfterEditEvent="False" OnAfterEdit="Grid1_AfterEdit"
                                            runat="server" EnableCheckBoxSelect="True" DataKeyNames="KeyId,FId,FNoCheckedAmt" EnableSummary="True" SummaryPosition="Bottom" OnRowCommand="Grid1_RowCommand"
                                            AllowSorting="False" EmptyText="查询无结果" EnableHeaderMenu="True" ClicksToEdit="1">
                                            <Toolbars>
                                                <f:Toolbar ID="Toolbar1" runat="server">
                                                    <Items>
                                                        <f:Button ID="btnSearch" OnClick="btnSearch_Click" runat="server" Text="分析核销" Icon="Zoom" Hidden="False" Size="Medium">
                                                        </f:Button>
                                                        <f:Button ID="btnSubmit" Text="保存" runat="server" Icon="SystemSaveNew" ValidateForms="SimpleForm1"
                                                            OnClick="btnSubmit_Click" Size="Medium">
                                                        </f:Button>
                                                        <f:Button ID="btnPrint" runat="server" Text="打印"
                                                            Icon="Printer" Hidden="False" Size="Medium">
                                                        </f:Button>
                                                        <f:Button ID="btnClose" EnablePostBack="False" Text="关闭" OnClientClick="closeActiveTab();" runat="server" Icon="SystemClose" Size="Medium">
                                                        </f:Button>
                                                    </Items>
                                                </f:Toolbar>
                                            </Toolbars>
                                            <Columns>
                                                <f:BoundField MinWidth="120px" ColumnID="keyId" DataField="keyId" HeaderText="收款单号" SortField="keyId" />
                                                <f:BoundField MinWidth="200px" ColumnID="BName" DataField="BName" HeaderText="开户银行" SortField="BName" />
                                                <f:BoundField MinWidth="180px" DataField="FComment" HeaderText="账号" SortField="FComment" />
                                                <f:BoundField MinWidth="80px" DataField="BAmt" HeaderText="收款金额" SortField="BAmt" />
                                                <f:BoundField MinWidth="80px" DataField="FCheckedAmt" HeaderText="已核销" SortField="FCheckedAmt" />
                                                <f:BoundField MinWidth="80px" DataField="FNoCheckedAmt" HeaderText="未核销" SortField="FNoCheckedAmt" />
                                                
                                                
                                                <%--<f:RenderField MinWidth="80px" ColumnID="FNoCheckedAmt" DataField="FNoCheckedAmt"
                                                    HeaderText="核销金额">
                                                    <Editor>
                                                        <f:NumberBox ID="txtFNoCheckedAmt" Required="true" runat="server">
                                                        </f:NumberBox>
                                                    </Editor>
                                                </f:RenderField>--%>
                                            </Columns>
                                        </f:Grid>
                                    </Items>
                                </f:Region>
                                <f:Region ID="Region5" ShowBorder="False" ShowHeader="false" Position="Bottom"
                                    MinHeight="200px"  MaxHeight="200px" Layout="Fit" runat="server" BodyPadding="3px" AutoScroll="True">
                                    <Items>
                                        <f:Grid ID="Grid2" 
                                            ShowBorder="False" 
                                            ShowHeader="False" 
                                            AllowPaging="False" 
                                            EnableCheckBoxSelect="True" 
                                            EnableHeaderMenu="True"  
                                            runat="server"
                                            DataKeyNames="KeyId,FNoCheckedAmt"
                                            SummaryPosition="Bottom" 
                                            AllowSorting="False" 
                                            EmptyText="查询无结果">
                                            <Columns>
                                                <f:BoundField MinWidth="80px" ID="FDate" DataField="FDate" HeaderText="日期" SortField="FDate" DataFormatString="{0:yyyy-MM-dd}" TextAlign="Left" />
                                                <f:BoundField MinWidth="120px" DataField="KeyId" HeaderText="单据号" SortField="KeyId" />
                                                <f:BoundField MinWidth="80px" DataField="CreateBy" HeaderText="操作员" SortField="CreateBy" />
                                         
                                                <f:BoundField MinWidth="80px" ID="FAmount" DataField="FAmount" HeaderText="销售款" SortField="FAmount" />
                                                <f:BoundField MinWidth="80px" ID="CheckedFAmt" DataField="CheckedFAmt" HeaderText="已核部分" SortField="CheckedFAmt" />
                                                <f:BoundField MinWidth="80px" ID="FNoCheckedAmt" DataField="FNoCheckedAmt" HeaderText="未核部分" SortField="FNoCheckedAmt" />

                                                <f:BoundField MinWidth="80px" DataField="FType" HeaderText="业务类型" SortField="FType" />
                                            </Columns>
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
    <script src="../../jqueryui/js/jquery-1.8.3.min.js" type="text/javascript"> </script>
    <script src="../../jqueryui/js/jquery-ui-1.9.2.custom.js?v=4.0" type="text/javascript"> </script>
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
    </script>
</body>
</html>
