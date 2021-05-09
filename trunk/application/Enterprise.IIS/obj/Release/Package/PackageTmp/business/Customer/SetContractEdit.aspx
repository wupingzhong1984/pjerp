<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetContractEdit.aspx.cs" Inherits="Enterprise.IIS.business.Customer.SetContractEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>合同管理</title>
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
                <f:Region ID="Region1" Title="合同管理" Position="Center"
                    ShowBorder="True" BoxConfigAlign="Stretch" Layout="Fit" runat="server" ShowHeader="False">
                    <Items>
                        <f:RegionPanel ID="RegionPanel2" ShowBorder="false" runat="server">
                            <Regions>
                                <f:Region ID="Region3" ShowBorder="false" ShowHeader="True" Title="合同管理" Split="False"
                                    Position="Top" Layout="Fit"
                                    BodyPadding="3px" runat="server" Height="270px" EnableCollapse="False">
                                    <Items>
                                        <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="80px"
                                            runat="server">
                                            <Rows>
                                                <f:FormRow ID="FormRow2">
                                                    <Items>
                                                        <f:TextBox ID="txtFContractCode" runat="server" Label="合同编号" EmptyText="合同编号">
                                                        </f:TextBox>
                                                        <f:DatePicker ID="dpFContractDate" runat="server" Required="false" Label="合同日期" EmptyText="合同日期"
                                                            ShowRedStar="false">
                                                        </f:DatePicker>
                                                    </Items>
                                                </f:FormRow>
                                                <f:FormRow ID="FormRow3">
                                                    <Items>
                                                        <f:TextBox ID="txtFContractName" runat="server" Label="合同名称" EmptyText="合同名称">
                                                        </f:TextBox>
                                                        <f:DropDownList runat="server" ID="ddlFCtroler" Label="经办人" CompareType="String" CompareValue="-1"
                                                            EnableEdit="True" CompareOperator="NotEqual" CompareMessage="经办人" Required="True" ShowRedStar="True">
                                                        </f:DropDownList>
                                                    </Items>
                                                </f:FormRow>
                                                <f:FormRow ID="FormRow1" Hidden="true">
                                                    <Items>
                                                        <f:DropDownList runat="server" ID="ddlFAccType" Label="结算方式" CompareType="String" CompareValue="-1"
                                                            EnableEdit="false" CompareOperator="NotEqual" CompareMessage="结算方式" >
                                                        </f:DropDownList>
                                                        <f:DropDownList runat="server" ID="ddlFBillType" Label="票据类型" CompareType="String" CompareValue="-1"
                                                            EnableEdit="false" CompareOperator="NotEqual" CompareMessage="票据类型" >
                                                        </f:DropDownList>
                                                    </Items>
                                                </f:FormRow>
                                                <f:FormRow ID="FormRow4">
                                                    <Items>
                                                        <f:DatePicker ID="dpFBeginDate" runat="server" Required="false" Label="有效期开始" EmptyText="有效期开始"
                                                            ShowRedStar="false">
                                                        </f:DatePicker>
                                                        <f:DatePicker ID="dpFEndDate" runat="server" Required="false" Label="有效期结束" EmptyText="有效期结束"
                                                            ShowRedStar="false">
                                                        </f:DatePicker>
                                                    </Items>
                                                </f:FormRow>
                                                <f:FormRow ID="FormRow6">
                                                    <Items>
                                                        <%--<f:DropDownList runat="server" ID="ddlFCustomer" Label="客户名称" CompareType="String" CompareValue="-1"
                                                            EnableEdit="True" CompareOperator="NotEqual" CompareMessage="客户名称" Required="True" ShowRedStar="True">
                                                        </f:DropDownList>--%>
                                                         <f:TextBox runat="server" Label="客户代码" ID="txtFCustomer"  Required="True" LabelAlign="Right" />
                                                        <f:TriggerBox ID="tbxFCustomer" EnablePostBack="True" OnTextChanged="tbxFCustomer_OnTextChanged"
                                                            ShowLabel="true" ShowRedStar="True" Required="true" Label="客户名称" LabelAlign="Right"
                                                            Readonly="false" TriggerIcon="Search" runat="server">
                                                        </f:TriggerBox>
                                                    </Items>
                                                </f:FormRow>
                                                <f:FormRow ID="FormRow5">
                                                    <Items>
                                                        <f:TextBox ID="txtFConacter" runat="server" Label="联系人" EmptyText="联系人">
                                                        </f:TextBox>
                                                        <f:TextBox ID="txtFTel" runat="server" Label="电话" EmptyText="电话">
                                                        </f:TextBox>
                                                    </Items>
                                                </f:FormRow>
                                                <f:FormRow ID="FormRow7">
                                                    <Items>
                                                        <f:TextBox ID="txtFContext" runat="server" Label="备注" EmptyText="备注">
                                                        </f:TextBox>
                                                    </Items>
                                                </f:FormRow>
                                                <f:FormRow ID="FormRow8">
                                                    <Items>
                                                       <f:FileUpload runat="server" ID="fileUpload" Label="上传附件" OnFileSelected="fileUpload_FileSelected" AutoPostBack="true"></f:FileUpload>
                                                    </Items>
                                                </f:FormRow>
                                            </Rows>
                                        </f:Form>
                                    </Items>
                                </f:Region>
                                <f:Region ID="Region4" ShowBorder="False" ShowHeader="false" Position="Center"
                                    Layout="Fit" runat="server" BodyPadding="3px" AutoScroll="True"  Height="120px">
                                    <Items>
                                        <f:Grid ID="Grid1" PageSize="20"
                                            ShowBorder="False"
                                            ShowHeader="False"
                                            AllowPaging="False"
                                            EnableAfterEditEvent="True"
                                            OnAfterEdit="Grid1_AfterEdit"
                                            runat="server"
                                            EnableCheckBoxSelect="True"
                                            DataKeyNames="FID"
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
                                                        <f:Button ID="btnAdd" runat="server" Text="新增" Icon="Add" Size="Medium" EnablePostBack="False">
                                                        </f:Button>
                                                        <f:Button ID="btnSubmit" Text="保存" runat="server" Icon="SystemSaveNew" ValidateForms="SimpleForm1"
                                                            OnClick="btnSubmit_Click" Size="Medium">
                                                        </f:Button>
                                                      
                                                    </Items>
                                                </f:Toolbar>
                                            </Toolbars>
                                            <Columns>
                                                <f:LinkButtonField TextAlign="Center" ConfirmText="删除选中行？" Icon="Delete" HeaderText="删除" ConfirmTarget="Top"
                                                    ColumnID="colDelete" Width="55px" CommandName="Delete" Hidden="False" SortField="mc">
                                                </f:LinkButtonField>
                                                <f:RenderField MinWidth="100px" ColumnID="FProductID" DataField="FProductID" FieldType="String"
                                                    HeaderText="编码">
                                                    <Editor>
                                                        <f:TriggerBox ID="tbxFProductID"  OnTriggerClick="tbxFProductID_OnTriggerClick"  Readonly="false" TriggerIcon="Search" runat="server">
                                                        </f:TriggerBox>
                                                    </Editor>
                                                </f:RenderField>
                                                <f:BoundField MinWidth="130px" DataField="FProductName" HeaderText="名称" SortField="FProductName" />
                                                <f:BoundField MinWidth="80px" DataField="FSpec" HeaderText="规格" SortField="FSpec" />                                                
                                                <f:BoundField MinWidth="80px" DataField="FUnit" HeaderText="单位" SortField="FUnit" />                                                                                                 
                                                <f:RenderField MinWidth="100px" ColumnID="FPrice" DataField="FPrice" FieldType="Double" TextAlign="Right"
                                                    HeaderText="单价">
                                                    <Editor>
                                                        <f:NumberBox ID="tbxFPrice" Required="true" NoDecimal="false" runat="server">
                                                        </f:NumberBox>
                                                    </Editor>
                                                </f:RenderField>                                               
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
            Target="Parent" EnableIFrame="true" IFrameUrl="about:blank" Height="480px" Width="960px"
            OnClose="Window1_Close">
        </f:Window>
        <f:Window ID="Window2" runat="server" WindowPosition="Center" IsModal="true"
            Icon="ApplicationViewDetail" EnableMaximize="true" EnableResize="true" Hidden="True" EnableClose="true"
            Target="Parent" EnableIFrame="true" Title="客户档案"
            IFrameUrl="about:blank" Height="480px" Width="960px" OnClose="Window1_Close">
        </f:Window>
        <f:HiddenField ID="hiddevalue" runat="server"></f:HiddenField>
    </form>
    <script src="../../jqueryui/js/jquery-1.8.3.min.js" type="text/javascript"> </script>
    <script src="../../jqueryui/js/jquery-ui-1.9.2.custom.js?v=4.0" type="text/javascript"> </script>
    <script type="text/javascript">
        var basePath = '<%= ResolveUrl("~/") %>';
        F.ready(function () {
            var txtcode = '<%= tbxFCustomer.ClientID %>';
                $('#' + txtcode + ' input').autocomplete({
                    source: function (request, response) {
                        $.getJSON("../../Common/AjaxCustomer.ashx", request, function (data, status, xhr) {
                            response(data);
                        });
                    }
                }, 1000);
            });
        var gridClientID = '<%= Grid1.ClientID %>';
        var inputselector = '.f-grid-tpl input';

        ///////////////////////////
        F('<% = Grid1.ClientID %>').on('edit', function (editor, e) {
            if (e.field == 'FItemCode') {
                window._selectrowIndex = e.rowIdx;
                window._selectcellIndex = e.colIdx;
                window.setTimeout(function () {
                    $("#<% =tbxFProductID.ClientID %> input").autocomplete({
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
            ///////////////////////////
    
        function reloadGrid(keys) {
            __doPostBack(null, 'reloadGrid:' + keys);
        };
            function closeActiveTab() {
                parent.removeActiveTab();
            };

    </script>
</body>
</html>
