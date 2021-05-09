<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TubePlan.aspx.cs" Inherits="Enterprise.IIS.business.TubeInvo.TubePlan" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>氢气进销</title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../../jqueryui/css/ui-lightness/jquery-ui-1.9.2.custom.css" />
    <style type="text/css">
        
        .colorred {
            color: green;
        }

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
                <f:Region ID="Region1" Title="氢气进销" Position="Center"
                    ShowBorder="false" BoxConfigAlign="Stretch" Layout="Fit" runat="server" ShowHeader="True">
                    <Items>
                        <f:RegionPanel ID="RegionPanel2" ShowBorder="false" runat="server">
                            <Regions>
                                <f:Region ID="Region3" ShowBorder="false" ShowHeader="false" Split="False"
                                    Position="Top" Layout="Fit"
                                    BodyPadding="3px" runat="server" Height="90px" EnableCollapse="True">
                                    <Items>
                                        <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="80px" LabelAlign="Right"
                                            runat="server">
                                            <Rows>
                                                <f:FormRow ID="FormRow0">
                                                    <Items>
                                                        <f:DatePicker ID="dateBegin" runat="server" Label="开始日期" DateFormatString="yyyy-MM-dd" LabelAlign="Right">
                                                        </f:DatePicker>
                                                        <f:DatePicker ID="dateEnd" runat="server" Label="结束日期" LabelAlign="Right">
                                                        </f:DatePicker>
                                                         <f:DropDownList ID="ddlFName" Label="商品名称" EnableEdit="true" LabelAlign="Right"  runat="server">
                                                        </f:DropDownList>
                                                       <%-- <f:TriggerBox ID="txtFName" Label="商品名称" LabelAlign="Right" OnTriggerClick="tbxFItemCode_OnTriggerClick" Readonly="false" TriggerIcon="Search" runat="server" EnablePostBack="False">
                                                        </f:TriggerBox>--%>
                                                        <f:DropDownList ID="ddlFBillType" Label="业务类型" LabelAlign="Right"  runat="server">
                                                            <f:ListItem Text="全部" Value="-1" /> 
                                                            <f:ListItem Text="进销" Value="进销" />
                                                            <f:ListItem Text="自销" Value="自销" />
                                                            <f:ListItem Text="代运" Value="代运" />
                                                        </f:DropDownList>
                                                    </Items>
                                                </f:FormRow>
                                                <f:FormRow ID="FormRow1">
                                                    <Items>
                                                        <f:DropDownList ID="ddlFVehicleNum" Label="车牌号" EnableEdit="true" LabelAlign="Right"  runat="server">
                                                        </f:DropDownList>
                                                        <f:DropDownList ID="ddlFDriver" EnableEdit="true" runat="server" Label="司机" LabelAlign="Right">
                                                        </f:DropDownList>
                                                        <f:DropDownList ID="ddlFSupercargo" EnableEdit="true" runat="server" Label="押运员" LabelAlign="Right">
                                                        </f:DropDownList>
                                                        <f:TextBox ID="txtFKeyId" runat="server" Label="单据编号" LabelAlign="Right">
                                                        </f:TextBox>
                                                    </Items>
                                                </f:FormRow>
                                                <f:FormRow ID="FormRow2">
                                                    <Items>
                                                        <f:DropDownList ID="ddlSuper" EnableEdit="true" runat="server" Label="供应商" LabelAlign="Right">
                                                        </f:DropDownList>
                                                        <f:DropDownList ID="ddlCustomer" EnableEdit="true" runat="server" Label="客户" LabelAlign="Right">
                                                        </f:DropDownList>
                                                        <%--<f:TriggerBox ID="TriggerBox4" Label="供应商" LabelAlign="Right" OnTriggerClick="tbxFItemCode_OnTriggerClick" Readonly="false" TriggerIcon="Search" runat="server" EnablePostBack="False">
                                                        </f:TriggerBox>

                                                        <f:TriggerBox ID="TriggerBox5" Label="客户" LabelAlign="Right" OnTriggerClick="tbxFItemCode_OnTriggerClick" Readonly="false" TriggerIcon="Search" runat="server" EnablePostBack="False">
                                                        </f:TriggerBox>--%>

                                                        <f:Label runat="server" Hidden="True" />
                                                        <f:Label runat="server" Hidden="True" />
                                                    </Items>
                                                </f:FormRow>
                                            </Rows>
                                        </f:Form>
                                    </Items>
                                </f:Region>
                                <f:Region ID="Region4" ShowBorder="False" ShowHeader="false" Position="Center"
                                    Layout="Fit" runat="server" BodyPadding="3px" AutoScroll="True" MinHeight="120px">
                                    <Items>
                                        <f:Grid ID="Grid1" PageSize="50" MinHeight="220px" ShowBorder="False" 
                                            EnableRowSelectEvent="True"
                                            EnableCheckBoxSelect="True"
                                            ShowHeader="False" AllowPaging="False" 
                                            runat="server" DataKeyNames="KeyId,FMarginEnd,FItemCode,FSupplierCode,F1Code"
                                            IsDatabasePaging="False" 
                                            AllowSorting="False" EmptyText="查询无结果" EnableHeaderMenu="True" 
                                            AllowCellEditing="true" ClicksToEdit="2" 
                                            OnRowDataBound="Grid1_RowDataBound"
                                            OnRowCommand="Grid1_RowCommand" 
                                            EnableAfterEditEvent="true" 
                                            OnAfterEdit="Grid1_AfterEdit">
                                            <Toolbars>
                                                <f:Toolbar ID="Toolbar1" runat="server">
                                                    <Items>
                                                        <f:Button ID="btnSearch" OnClick="btnSearch_Click" runat="server" Text="查询" Icon="Zoom" Hidden="False" Size="Medium">
                                                        </f:Button>

                                                        <f:Button ID="btnAdd" runat="server" Text="新增" Icon="Add" Size="Medium" EnablePostBack="False">
                                                        </f:Button>
                                                        
                                                        <%--<f:Button ID="btnMerge" runat="server" Text="进销并车" Icon="Car" Size="Medium" EnablePostBack="False">
                                                        </f:Button>--%>

                                                        <f:Button ID="btnSubmit" Text="保存" runat="server" Icon="SystemSaveNew"
                                                            OnClick="btnSubmit_Click" Size="Medium">
                                                        </f:Button>

                                                        <f:Button ID="btnExport" OnClick="btnExport_Click" runat="server" Text="引出" Icon="PageExcel"
                                                            Hidden="False" EnableAjax="false" DisableControlBeforePostBack="false" Size="Medium">
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
                                                
                                                <f:LinkButtonField TextAlign="Center" Icon="ScriptEdit" HeaderText="填写榜单" ConfirmTarget="Top"
                                                    ColumnID="colPound" Width="75px" CommandName="Pound" Hidden="False" SortField="mc">
                                                </f:LinkButtonField>
                                                
                                                <f:LinkButtonField TextAlign="Center" ConfirmText="确认完成榜单吗？" Icon="ScriptCodeOriginal" HeaderText="完成榜单" ConfirmTarget="Top"
                                                    ColumnID="colSubmit" Width="75px" CommandName="Submit" Hidden="False" SortField="mc">
                                                </f:LinkButtonField>
                                                
                                                <%--<f:LinkButtonField TextAlign="Center" ConfirmText="确认导气吗？" Icon="DatabaseGo" HeaderText="导气" ConfirmTarget="Top"
                                                    ColumnID="colTo" Width="55px" CommandName="To" Hidden="False" SortField="mc">
                                                </f:LinkButtonField>--%>

                                                <f:GroupField HeaderText="基本信息" TextAlign="Center" EnableLock="true" SortField="mc">
                                                    <Columns>
                                                        <f:BoundField MinWidth="160px" DataField="FDistributionPoint" HeaderText="作业区" SortField="FDistributionPoint" />
                                                        <f:BoundField MinWidth="120px" DataField="KeyId" HeaderText="单据号" SortField="KeyId" />
                                                        <f:RenderField MinWidth="100px" ColumnID="FDate" DataField="FDate" FieldType="Date" TextAlign="Right"
                                                            HeaderText="日期" RendererArgument="yyyy-MM-dd" Renderer="Date">
                                                            <Editor>
                                                                <f:DatePicker ID="tbxFDate" runat="server">
                                                                </f:DatePicker>
                                                            </Editor>
                                                        </f:RenderField>

                                                        <f:RenderField Width="160px" ColumnID="FItemName" DataField="FItemName" FieldType="String"
                                                            HeaderText="商品名称">
                                                            <Editor>
                                                                <f:DropDownList ID="tbxFItemName"  runat="server" EnableEdit="True">
                                                                </f:DropDownList>
                                                            </Editor>
                                                        </f:RenderField>

                                                        <f:RenderField Width="100px" ColumnID="FBill" DataField="FBill" FieldType="String"
                                                            HeaderText="业务类型">
                                                            <Editor>
                                                                <f:DropDownList ID="tbxFBill"  runat="server">
                                                                    <f:ListItem Text="进销" Value="进销" Selected="True"/>
                                                                    <f:ListItem Text="自销" Value="自销" />
                                                                    <f:ListItem Text="代运" Value="代运" />
                                                                </f:DropDownList>
                                                            </Editor>
                                                        </f:RenderField>

                                                        <f:RenderField Width="100px" Enabled="False" ColumnID="FVehicleNum" DataField="FVehicleNum" FieldType="String"
                                                            HeaderText="车牌号">
                                                            <Editor>
                                                                <f:DropDownList ID="tbxFVehicleNum"  runat="server" >
                                                                </f:DropDownList>
                                                            </Editor>
                                                        </f:RenderField>
                                                        
                                                        <f:BoundField MinWidth="160px" DataField="FDriver" HeaderText="司机" SortField="FDriver" />

                                                        <f:RenderField Width="100px" Enabled="False" ColumnID="FSupercargo" DataField="FSupercargo" FieldType="String"
                                                            HeaderText="押运员">
                                                            <Editor>
                                                                <f:DropDownList ID="tbxFSupercargo"  runat="server">
                                                                </f:DropDownList>
                                                            </Editor>
                                                        </f:RenderField>

                                                        <%--<f:RenderField Width="80px" ColumnID="FMargin" DataField="FMargin" FieldType="Double"
                                                            TextAlign="Right" HeaderText="装车前余量" Hidden="False">
                                                            <Editor>
                                                                <f:NumberBox ID="tbxFMargin"  runat="server">
                                                                </f:NumberBox>
                                                            </Editor>
                                                        </f:RenderField>

                                                        <f:RenderField Width="80px" ColumnID="FMarginEnd" DataField="FMarginEnd" FieldType="Double"
                                                           TextAlign="Right" HeaderText="现余量">
                                                            <Editor>
                                                                <f:NumberBox ID="tbxFMarginEnd" runat="server">
                                                                </f:NumberBox>
                                                            </Editor>
                                                        </f:RenderField>--%>
                                                        <f:BoundField MinWidth="80px" DataField="CreateBy" HeaderText="制单人" SortField="CreateBy" />
                                                    </Columns>
                                                </f:GroupField>
                                                <f:GroupField HeaderText="采购" TextAlign="Center" EnableLock="true" SortField="mc">
                                                    <Columns>

                                                        <f:RenderField MinWidth="100px" ColumnID="FPurchasedDate"
                                                            DataField="FPurchasedDate" FieldType="String" TextAlign="Left"
                                                            HeaderText="装车时间" Hidden="True">
                                                            <Editor>
                                                                <f:TextBox ID="dpFPurchasedDate" LabelAlign="Right"
                                                                    EmptyText="装车时间" runat="server">
                                                                </f:TextBox>
                                                            </Editor>
                                                        </f:RenderField>

                                                        <f:RenderField MinWidth="220px" Enabled="False" ColumnID="FSupplierName" DataField="FSupplierName" FieldType="String" TextAlign="Left"
                                                            HeaderText="供应商">
                                                            <Editor>
                                                                <f:DropDownList ID="tbxFSupplierName" runat="server"  EnableEdit="True">
                                                                </f:DropDownList>
                                                            </Editor>
                                                        </f:RenderField>

                                                        <f:RenderField MinWidth="100px" ColumnID="FPurchasedPrice" DataField="FPurchasedPrice" FieldType="Double" TextAlign="Right"
                                                            HeaderText="价格">
                                                            <Editor>
                                                                <f:NumberBox ID="tbxFPurchasedPrice"  runat="server">
                                                                </f:NumberBox>
                                                            </Editor>
                                                        </f:RenderField>
                                                        
                                                        <f:RenderField MinWidth="100px" ColumnID="FPurchasedPQty" DataField="FPurchasedPQty" FieldType="Double" TextAlign="Right"
                                                            HeaderText="计划量">
                                                            <Editor>
                                                                <f:NumberBox ID="NumberBox2"  runat="server">
                                                                </f:NumberBox>
                                                            </Editor>
                                                        </f:RenderField>

                                                        <%--<f:RenderField MinWidth="100px" ColumnID="FPurchasedQty" DataField="FPurchasedQty" FieldType="Double" TextAlign="Right"
                                                            HeaderText="磅单量">
                                                            <Editor>
                                                                <f:NumberBox ID="tbxFPurchasedPlanQty"  runat="server">
                                                                </f:NumberBox>
                                                            </Editor>
                                                        </f:RenderField>--%>
                                                        
<%--                                                    <f:BoundField MinWidth="120px" DataField="FAmt" HeaderText="金额" SortField="FAmt" TextAlign="Right"/>--%>

                                                    </Columns>
                                                </f:GroupField>


                                                <f:GroupField HeaderText="销售" TextAlign="Center" EnableLock="true" SortField="mc">
                                                    <Columns>
                                                        <f:RenderField MinWidth="100px" ColumnID="F1Date" DataField="F1Date" 
                                                            FieldType="String" TextAlign="Left"
                                                            HeaderText="卸车时间" Hidden="True">
                                                            <Editor>
                                                                <f:TextBox ID="dpFDate1" LabelAlign="Right"
                                                                    EmptyText="卸车时间" runat="server">
                                                                </f:TextBox>
                                                            </Editor>
                                                        </f:RenderField>

                                                        <f:RenderField MinWidth="220px" ColumnID="F1Name" DataField="F1Name" FieldType="String" TextAlign="Left"
                                                            HeaderText="客户">
                                                            <Editor>
                                                                <f:DropDownList ID="tbxFName1" runat="server" EnableEdit="True">
                                                                </f:DropDownList>
                                                            </Editor>
                                                        </f:RenderField>

                                                        <f:RenderField MinWidth="100px" Enabled="False" ColumnID="F1Price" DataField="F1Price" FieldType="Double" TextAlign="Right"
                                                            HeaderText="价格">
                                                            <Editor>
                                                                <f:NumberBox ID="tbxFPrice1"  runat="server">
                                                                </f:NumberBox>
                                                            </Editor>
                                                        </f:RenderField>
                                                        
                                                        <f:RenderField MinWidth="100px" ColumnID="F1PQty" DataField="F1PQty" FieldType="Double" TextAlign="Right"
                                                            HeaderText="计划量">
                                                            <Editor>
                                                                <f:NumberBox ID="NumberBox1"  runat="server">
                                                                </f:NumberBox>
                                                            </Editor>
                                                        </f:RenderField>

                                                   <%-- <f:RenderField MinWidth="100px" ColumnID="FQty1" DataField="FQty1" FieldType="Double" TextAlign="Right"
                                                            HeaderText="磅单量">
                                                            <Editor>
                                                                <f:NumberBox ID="tbFQty1"  runat="server">
                                                                </f:NumberBox>
                                                            </Editor>
                                                        </f:RenderField>
                                                        
                                                        <f:BoundField MinWidth="120px" DataField="FAmt1" HeaderText="金额" SortField="FAmt1" TextAlign="Right"/>--%>


                                                    </Columns>
                                                </f:GroupField>
                                                
                                                <f:RenderField MinWidth="160px" ColumnID="FMemo" DataField="FMemo"
                                                    HeaderText="摘要">
                                                    <Editor>
                                                        <f:TextBox ID="tbxFMemo" runat="server">
                                                        </f:TextBox>
                                                    </Editor>
                                                </f:RenderField>
                                                
                                                <%--<f:GroupField HeaderText="导气结果" TextAlign="Center" EnableLock="true" SortField="mc">
                                                    <Columns>
                                                        <f:BoundField MinWidth="60px" DataField="XBill" HeaderText="导气业务" SortField="XBill"/>
                                                        <f:BoundField MinWidth="60px" DataField="XTo" HeaderText="导气介质" SortField="XTo"/>
                                                        <f:BoundField MinWidth="60px" DataField="XQty" HeaderText="导气数量" SortField="XQty" TextAlign="Right"/>
                                                    </Columns>
                                                </f:GroupField>--%>
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
            Target="Parent" EnableIFrame="true" IFrameUrl="about:blank" Height="450px" Width="900px"
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
        
        <f:Window ID="Window4" runat="server" WindowPosition="Center" IsModal="true"
            Icon="ApplicationViewDetail" EnableMaximize="true" EnableResize="true" Hidden="True" EnableClose="true"
            Target="Parent" EnableIFrame="true" Title="导气"
            IFrameUrl="about:blank" Height="450px" Width="350px" OnClose="Window1_Close">
        </f:Window>

    </form>
    <script src="../../jqueryui/js/jquery-1.8.3.min.js" type="text/javascript"> </script>
    <script src="../../jqueryui/js/jquery-ui-1.9.2.custom.js?v=4.0" type="text/javascript"> </script>
    <script type="text/javascript">

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
    </script>
</body>
</html>
