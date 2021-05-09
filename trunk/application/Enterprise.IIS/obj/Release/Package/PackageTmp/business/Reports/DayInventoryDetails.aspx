﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DayInventoryDetails.aspx.cs" Inherits="Enterprise.IIS.business.Reports.DayInventoryDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>详情</title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .x-grid-row-summary .x-grid-cell-inner {
            font-weight: bold;
            color: red;
        }
        .x-grid-row-summary .x-grid-cell {
            background-color: #fff !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region1" Title="详情表" Position="Center"
                    ShowBorder="False" BoxConfigAlign="Stretch" Layout="Fit" runat="server" ShowHeader="False">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <f:Button ID="Button1" OnClick="btnSearch_Click" runat="server" Text="查询"
                                    Icon="Zoom" Hidden="False" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnPrint" OnClick="btnExport_Click" runat="server" Text="打印"
                                    Icon="Printer" Hidden="False" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnExport" OnClick="btnExport_Click" runat="server" Text="引出" Icon="PageExcel"
                                    Hidden="False" EnableAjax="false" DisableControlBeforePostBack="false" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnClose" EnablePostBack="False" Text="关闭" OnClientClick="closeActiveTab();" runat="server" Icon="SystemClose" Size="Medium">
                                </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Items>
                        <f:RegionPanel ID="RegionPanel2" ShowBorder="false" runat="server">
                            <Regions>
                                <f:Region ID="Region3" ShowBorder="false" ShowHeader="false" Split="False"
                                    Position="Top" Layout="Fit"
                                    BodyPadding="3px" runat="server" Height="38px" EnableCollapse="True">
                                    <Items>
                                        <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="80px"
                                            runat="server" BodyPadding="5px">
                                            <Rows>
                                                <f:FormRow ID="FormRow1">
                                                    <Items>
                                                        <f:DropDownList runat="server" ID="ddlCompany" Label="所属公司" LabelAlign="Right"></f:DropDownList>
                                                        <f:DatePicker ID="dpkFDateBegin" LabelAlign="Right" runat="server" Label="开始时间" DateFormatString="yyyy-MM-dd">
                                                        </f:DatePicker>
                                                        <f:Label runat="server" Hidden="True"/>
                                                        <f:Label runat="server" Hidden="True"/>
                                                    </Items>
                                                </f:FormRow>
                                            </Rows>
                                        </f:Form>
                                    </Items>
                                </f:Region>
                                <f:Region ID="Region4" ShowBorder="False" ShowHeader="False" Position="Center"
                                    Layout="Fit" runat="server" BodyPadding="3px" AutoScroll="True">
                                    <Items>
                                        <f:Grid ID="Grid1" ShowBorder="False" ShowHeader="False" AllowPaging="False"
                                            runat="server" EnableCheckBoxSelect="False" DataKeyNames="KeyId" 
                                            OnSort="Grid1_Sort" EmptyText="查询无结果" 
                                            EnableSummary="true" SummaryPosition="Bottom">
                                            <Columns>
                                                <f:BoundField MinWidth="130px"  ColumnID="FCode" DataField="FCode" HeaderText="客户代码" SortField="FCode" />
                                                <f:BoundField MinWidth="130px"  ColumnID="FName" DataField="FName" HeaderText="客户名称" SortField="FName" />
                                                <f:BoundField MinWidth="230px"  ColumnID="KeyId" DataField="KeyId" HeaderText="单据号" SortField="KeyId" />
                                                <f:BoundField MinWidth="80px" ColumnID="FItemCode" DataField="FItemCode" HeaderText="商品代码" SortField="FItemCode" />
                                                <f:BoundField MinWidth="80px" ColumnID="FItemName" DataField="FItemName" HeaderText="商品名称" SortField="FItemName" />
                                                
                                                <f:BoundField MinWidth="80px" ColumnID="FBottle" DataField="FBottle" HeaderText="包装物代码" SortField="FBottle" />
                                                <f:BoundField MinWidth="80px" ColumnID="FBottleName" DataField="FBottleName" HeaderText="包装物代码" SortField="FBottleName" />


                                                <f:BoundField MinWidth="80px" ColumnID="FQty"  DataField="FQty" HeaderText="产品数量" SortField="FQty" TextAlign="Right" />
                                                <f:BoundField MinWidth="80px" ColumnID="FBottleQty"  DataField="FBottleQty" HeaderText="包装物数量" SortField="FBottleQty" TextAlign="Right" />
                                                
                                                

                                                <f:BoundField MinWidth="230px"  ColumnID="FMemo" DataField="FMemo" HeaderText="备注说明" SortField="FMemo" />

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
        <f:Window ID="Window1" runat="server" WindowPosition="Center" IsModal="True" EnableClose="True"
            Icon="ApplicationViewDetail" Title="客户档案" EnableMaximize="True" EnableResize="True" Hidden="True"
            Target="Parent" EnableIFrame="True" IFrameUrl="about:blank" Height="580px" Width="960px"
            OnClose="Window1_Close">
        </f:Window>
        
        <f:Window ID="Window2" Icon="PageAttach" runat="server" Hidden="true"
            IsModal="true" Target="Parent" EnableMaximize="true" EnableResize="true" OnClose="Window2_Close"
            Title="Popup Window 2" CloseAction="HidePostBack"
            EnableIFrame="true" Height="350px" Width="450px">
        </f:Window>

    </form>
    <script src="../../jqueryui/js/jquery-1.8.3.min.js" type="text/javascript"> </script>
    <script src="../../jqueryui/js/jquery-ui-1.9.2.custom.js?v=4.0" type="text/javascript"> </script>
    <script type="text/javascript">
        var basePath = '<%= ResolveUrl("~/") %>';
        function openAddFineUI(keyid) {
            var url = 'business/Sales/Details.aspx?action=6&keyid=' + keyid;
            parent.addExampleTab.apply(null, ['add_tab_' + keyid, basePath + url, '详情', basePath + 'icon/page_find.png', true]);
        };
        function closeActiveTab() {
            parent.removeActiveTab();
        };
        <%--F.ready(function () {
            var txtcode = '<%= tbxFCustomer.ClientID %>';

            $('#' + txtcode + ' input').autocomplete({
                source: function (request, response) {
                    $.getJSON("../../Common/AjaxCustomer.ashx", request, function (data, status, xhr) {
                        response(data);
                    });
                }
            });
        });--%>
    </script>
</body>
</html>