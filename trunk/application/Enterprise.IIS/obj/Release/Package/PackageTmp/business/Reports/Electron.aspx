<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Electron.aspx.cs" Inherits="Enterprise.IIS.business.Reports.Electron" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>电子气本</title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../../jqueryui/css/ui-lightness/jquery-ui-1.9.2.custom.css" />
    <style type="text/css">
        .x-grid-row-summary .x-grid-cell-inner {
            font-weight: bold;
            color: red;
        }

        .x-grid-row-summary .x-grid-cell {
            background-color: #fff !important;
        }

        .colorred {
            color: red;
        }

        .colorgreen {
            color: green;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        
                        <f:Button ID="btnSearchCustomer" runat="server" Hidden="True" Text="按客户查询" Size="Medium" Icon="UserComment" OnClick="btnSearchCustomer_Click">
                        </f:Button>

                        <f:Button ID="btnSearch" runat="server" Text="查询" Icon="Zoom" OnClick="btnSearch_Click">
                        </f:Button>
                        
                        <f:Button ID="btnReset" runat="server" Text="重置条件" Icon="Reload" OnClick="btnRest_Click" Size="Medium">
                        </f:Button>
                        

                        <f:Button ID="btnExport" OnClick="btnExport_Click" runat="server" Text="引出" Icon="PageExcel"
                            Hidden="False" EnableAjax="false" DisableControlBeforePostBack="false" Size="Medium">
                        </f:Button>
                        <f:Button ID="btnClose" EnablePostBack="False" Text="关闭" runat="server" Icon="SystemClose" Size="Medium" onClientClick="closeActiveTab();">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Regions>
                <f:Region ID="Region1" ShowHeader="false" Split="true" ShowBorder="False"
                    Width="200px" Position="Left" Layout="Fit" runat="server">
                    <Items>
                        <f:Tree ID="trDept" Width="50px" OnNodeCommand="trDept_NodeCommand" ShowHeader="False"
                            ShowBorder="false" Icon="House" Title="客户" runat="server" EnableTextSelection="true"
                            EnableArrows="true" AutoScroll="True">
                        </f:Tree>
                    </Items>
                </f:Region>
                <f:Region ID="Region2" Title="客户" Position="Center"
                    ShowBorder="False" BoxConfigAlign="Stretch" Layout="Fit" runat="server" ShowHeader="False">
                    <Items>
                        <f:RegionPanel ID="RegionPanel2" ShowBorder="false" runat="server">
                            <Regions>
                                <f:Region ID="Region3" ShowBorder="false" ShowHeader="false" Split="False"
                                    Position="Top" Layout="Fit"
                                    BodyPadding="3px" runat="server" Height="68px" EnableCollapse="True">
                                    <Items>
                                        <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="80px"
                                            runat="server" BodyPadding="5px" LabelAlign="Right">
                                            <Rows>
                                                <f:FormRow ID="FormRow1">
                                                    <Items>
                                                        <f:DatePicker ID="dpkFDateBegin" runat="server" Label="年/月" DateFormatString="yyyy-MM" LabelAlign="Right">
                                                        </f:DatePicker>

                                                        <f:TextBox runat="server" Label="客户代码" ID="txtFCode" LabelAlign="Right" />
                                                        <f:TriggerBox ID="tbxFCustomer" EnablePostBack="True" OnTextChanged="tbxFCustomer_OnTextChanged"
                                                            ShowLabel="true" Label="客户名称" LabelAlign="Right" AutoPostBack="True"
                                                            Readonly="false" TriggerIcon="Search" runat="server">
                                                        </f:TriggerBox>

                                                        <f:Label runat="server" Hidden="True"/>

                                                    </Items>
                                                </f:FormRow>
                                                <f:FormRow ID="FormRow2">
                                                    <Items>
                                                        <f:DropDownList runat="server" ID="ddlFSalesman" Label="业务员" AutoPostBack="True" OnSelectedIndexChanged="ddlFSalesman_OnSelectedIndexChanged" EnableEdit="True"  LabelAlign="Right"/>
                                                        <f:DropDownList runat="server" ID="ddlBottle" Label="钢瓶档案" EnableEdit="True" LabelAlign="Right" />

                                                        <f:Label runat="server" Hidden="True"/>
                                                        <f:Label runat="server" Hidden="True"/>
                                                    
                                                    </Items>
                                                </f:FormRow>
                                                

                                            </Rows>
                                        </f:Form>
                                    </Items>
                                </f:Region>
                                <f:Region ID="Region4" ShowBorder="False" ShowHeader="false" Position="Center"
                                    Layout="Fit" runat="server" BodyPadding="3px" AutoScroll="True">
                                    <Items>
                                        <f:Grid ID="Grid1" PageSize="20" ShowBorder="false" ShowHeader="false" AllowPaging="false"
                                            runat="server" EnableCheckBoxSelect="False" DataKeyNames="FCod,FBottle" 
                                            IsDatabasePaging="false" SortDirection="ASC" OnRowDataBound="Grid1_OnRowDataBound"
                                            AllowSorting="false" EmptyText="查询无结果" EnableSummary="False" SummaryPosition="Bottom"
                                            EnableRowDoubleClickEvent="true" OnRowDoubleClick="Grid1_RowClick">
                                            <Columns>
                                                <f:BoundField MinWidth="110px"  DataField="FBottleName" HeaderText="气瓶名称" SortField="FBottleName" />
                                                <f:BoundField MinWidth="80px" DataField="FSpec" HeaderText="规格" SortField="FSpec" />
                                                <f:BoundField MinWidth="110px" TextAlign="Center" DataField="FDate1" HeaderText="回瓶日期" SortField="FDate1" DataFormatString="{0:yyyy-MM-dd}" />
                                                <f:BoundField ColumnID="FInQty" MinWidth="60px" DataField="FIQty" HeaderText="回瓶数量" SortField="FIQty" TextAlign="Right"/>
                                                <f:BoundField MinWidth="120px" TextAlign="Center" DataField="FDate2" HeaderText="提瓶日期" SortField="FDate2"  DataFormatString="{0:yyyy-MM-dd}" />
                                                <f:BoundField ColumnID="FOQty" MinWidth="60px" DataField="FOQty" HeaderText="提瓶数量" SortField="FOQty" TextAlign="Right" />
                                                
                                                <f:BoundField ColumnID="FInitQty" MinWidth="60px" DataField="FInitQty" HeaderText="期初欠瓶" SortField="FInitQty" TextAlign="Right" />
                                                <f:BoundField ColumnID="FEndQty" MinWidth="60px" DataField="FEndQty" HeaderText="期未欠瓶" SortField="FEndQty" TextAlign="Right" />


                                                <f:BoundField MinWidth="220px" DataField="FName" HeaderText="客户名称" SortField="FName" />
                                                <f:BoundField MinWidth="60px" DataField="FBottle" HeaderText="钢瓶编码" SortField="FBottle" />

                                            </Columns>
                                        </f:Grid>
                                    </Items>
                                </f:Region>
                                
                                 
                                <f:Region ID="Region5" ShowBorder="False" ShowHeader="False" Position="Bottom" Hidden="False"
                                    Layout="Fit" MinHeight="40px" MaxHeight="120px" runat="server" BodyPadding="3px" AutoScroll="True" Title="汇总合计">
                                    <Items>
                                        <f:ContentPanel runat="server">
                                            <table>
                                                <tr>
                                                    <td style="width: 160px"><f:Label runat="server" LabelAlign="Right" ID="SumIQty" Text="0.00" Label="回瓶总数"/></td>
                                                    <td style="width: 160px"><f:Label runat="server" LabelAlign="Right" ID="SumOQty" Text="0.00" Label="提瓶总数"/></td>
                                                    <td style="width: 160px"><f:Label runat="server" Hidden="True" LabelAlign="Right" ID="SumInit" Text="0.00" Label="期初欠瓶"/></td>
                                                    <td style="width: 160px"><f:Label runat="server" Hidden="True" LabelAlign="Right" ID="SumEnd" Text="0.00" Label="期未欠瓶"/></td>
                                                    <td style="width: 160px"><f:Label Hidden="True" runat="server" LabelAlign="Right" ID="SumFDiscountAmount" Text="0.00" /></td>
                                                </tr>
                                            </table>
                                        </f:ContentPanel>
                                    </Items>
                                </f:Region>


                            </Regions>
                        </f:RegionPanel>
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>
        <br />
        <f:Window ID="Window1" runat="server" WindowPosition="Center" IsModal="true" Title="原始单据"
            Icon="ApplicationViewDetail" EnableMaximize="true" EnableResize="true" Hidden="True"
            Target="Parent" EnableIFrame="true" IFrameUrl="about:blank" Height="480px" Width="380px"
            OnClose="Window1_Close">
        </f:Window>
        
    </form>
    
    
    
    <script src="../../jqueryui/js/jquery-1.8.3.min.js" type="text/javascript"> </script>
    <script src="../../jqueryui/js/jquery-ui-1.9.2.custom.js?v=4.0" type="text/javascript"> </script>
    <script type="text/javascript" language="javascript" src="../../js/LodopFuncs.js"></script>
    <script type="text/javascript">
        var basePath = '<%= ResolveUrl("~/") %>';
        
        function openDetailsUI(code, date) {

            var url = 'business/Reports/BottleReturnEdit.aspx?action=1&FCode=' + code + "&FDate=" + date;
            //var url = 'business/Reports/DayInventoryDetails.aspx?action=6&FDate=' + date + "&FCode=" + item + "&FType=" + type;

            //alert(basePath + url);
            parent.addExampleTab.apply(null, ['add_tab_', basePath + url, '回瓶', basePath + 'icon/page_find.png', true]);

            //parent.addExampleTab.apply(null, ['add_tab_2' + item, basePath + url, '添加客户回瓶单', basePath + 'icon/page_find.png', false]);
        };

        function closeActiveTab() {
            parent.removeActiveTab();
        };

        F.ready(function () {
            var txtcode = '<%= tbxFCustomer.ClientID %>';

            $('#' + txtcode + ' input').autocomplete({
                source: function (request, response) {
                    $.getJSON("../../Common/AjaxCustomer.ashx", request, function (data, status, xhr) {
                        response(data);
                    });
                }
            });
        });

    </script>
</body>
</html>
