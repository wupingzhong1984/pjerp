<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ElectronGas.aspx.cs" Inherits="Enterprise.IIS.business.Reports.ElectronGas" %>


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
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <f:Button ID="btnSearch" OnClick="btnSearch_Click" runat="server" Text="查询" Icon="Zoom" Hidden="False" Size="Medium">
                        </f:Button>
                        <f:Button ID="btnPrint" OnClick="btnBatchDelete_Click" runat="server" Text="打印"
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
            <Regions>
                <%--<f:Region ID="Region1" ShowHeader="false" Split="true" ShowBorder="False"
                    Width="200px" Position="Left" Layout="Fit" runat="server">
                    <Items>
                        <f:Tree ID="trDept" Width="50px" OnNodeCommand="trDept_NodeCommand" ShowHeader="False"
                            ShowBorder="false" Icon="House" Title="钢瓶分类" runat="server" EnableTextSelection="true"
                            EnableArrows="true" AutoScroll="True">
                        </f:Tree>
                    </Items>
                </f:Region>--%>
                <f:Region ID="Region2" Title="电子气本" Position="Center"
                    ShowBorder="False" BoxConfigAlign="Stretch" Layout="Fit" runat="server" ShowHeader="False">
                    <Items>
                        <f:RegionPanel ID="RegionPanel2" ShowBorder="false" runat="server">
                            <Regions>
                                <f:Region ID="Region3" ShowBorder="false" ShowHeader="false" Split="False"
                                    Position="Top" Layout="Fit"
                                    BodyPadding="3px" runat="server" Height="38px" EnableCollapse="True">
                                    <Items>
                                        <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="80px"
                                            runat="server" BodyPadding="5px" LabelAlign="Right">
                                            <Rows>
                                                <f:FormRow ID="FormRow1">
                                                    <Items>
                                                        <f:DropDownList runat="server" ID="ddlCompany" Label="所属公司" LabelAlign="Right"></f:DropDownList>
                                                        <f:DatePicker ID="dpFDate" runat="server" Label="日期" DateFormatString="yyyy-MM-dd">
                                                        </f:DatePicker>
                                                        <f:TextBox ID="txtFName" runat="server" Label="客户代码">
                                                        </f:TextBox>
                                                        <f:DropDownList runat="server" ID="ddlUnit" Label="单位类型" Hidden="True">
                                                            <f:ListItem Text="全部" Value="-1" Selected="true" />
                                                            <f:ListItem Text="客户" Value="客户" />
                                                            <f:ListItem Text="供应商" Value="供应商" />
                                                        </f:DropDownList>
                                                    </Items>
                                                </f:FormRow>

                                                <f:FormRow ID="FormRow2" Hidden="True">
                                                    <Items>
                                                        <f:TextBox ID="txtFBottleName" runat="server" Label="钢瓶名称">
                                                        </f:TextBox>
                                                        <f:TextBox ID="txtFBottleCode" runat="server" Label="钢瓶代码">
                                                        </f:TextBox>
                                                        <f:TextBox ID="txtFSalesman" runat="server" Label="业务员">
                                                        </f:TextBox>
                                                        <f:Label runat="server" Hidden="True" />
                                                    </Items>
                                                </f:FormRow>
                                            </Rows>
                                        </f:Form>
                                    </Items>
                                </f:Region>
                                <f:Region ID="Region4" ShowBorder="False" ShowHeader="false" Position="Center"
                                    Layout="Fit" runat="server" BodyPadding="3px" AutoScroll="True">
                                    <Items>
                                        <f:Grid ID="Grid1" PageSize="200" ShowBorder="False" ShowHeader="False" AllowPaging="False"
                                            runat="server" EnableCheckBoxSelect="False" DataKeyNames="KeyId" OnPageIndexChange="Grid1_PageIndexChange"
                                            IsDatabasePaging="False" OnSort="Grid1_Sort" SortDirection="DESC" OnRowCommand="Grid1_RowCommand"
                                            AllowSorting="False" EmptyText="查询无结果" EnableHeaderMenu="True"
                                            EnableSummary="true" SummaryPosition="Bottom" KeepCurrentSelection="True" EnableMultiSelect="True">
                                            <Columns>
                                                <f:BoundField MinWidth="30px" ColumnID="FCate" DataField="FCate" HeaderText="单位类型" SortField="FCate" />
                                                <f:BoundField MinWidth="160px" ColumnID="FName" DataField="FName" HeaderText="客户名称" SortField="FName" />
                                                <f:BoundField MinWidth="110px" ColumnID="FBottleName" DataField="FBottleName" HeaderText="钢瓶名称" SortField="FBottleName" />
                                                <f:BoundField MinWidth="40px" ColumnID="FSpec" DataField="FSpec" HeaderText="规格" SortField="FSpec" />
                                                <f:BoundField MinWidth="40px" ColumnID="FUnit" DataField="FUnit" HeaderText="单位" SortField="FUnit" />
                                                <f:BoundField Width="80px" ColumnID="FQty1" DataField="FQty1" HeaderText="客户占用" SortField="FQty1" TextAlign="right" />
                                                <f:BoundField Width="80px" ColumnID="FQty2" DataField="FQty2" HeaderText="客户存放" SortField="FQty2" TextAlign="right" />
                                                <f:BoundField Width="80px" ColumnID="FQty3" DataField="FQty3" HeaderText="存放供应商" SortField="FQty3" TextAlign="right" />
                                                <f:BoundField Width="80px" ColumnID="FQty4" DataField="FQty4" HeaderText="借进供应商" SortField="FQty4" TextAlign="right" />
                                                
                                                
                                                <f:BoundField Width="80px" ColumnID="FDepositSecurity" DataField="FDepositSecurity" HeaderText="押金" SortField="FDepositSecurity" TextAlign="right" />
                                                

                                                <f:BoundField Width="80px" ColumnID="FSalesman" DataField="FSalesman" HeaderText="业务员" SortField="FSalesman" TextAlign="right" />
                                                <f:BoundField Width="120px" ColumnID="FMemo" DataField="FMemo" HeaderText="备注" SortField="FMemo" TextAlign="right" />
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
            Icon="ApplicationViewDetail" EnableMaximize="True" EnableResize="True" Hidden="True"
            Target="Parent" EnableIFrame="True" IFrameUrl="about:blank" Height="450px" Width="800px"
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
    <script type="text/javascript" language="javascript" src="../../js/LodopFuncs.js"></script>
    <script type="text/javascript">
        var basePath = '<%= ResolveUrl("~/") %>';

        function closeActiveTab() {
            parent.removeActiveTab();
        }


        F.ready(function () {
            var txtcode = '<%= txtFName.ClientID %>';

            $('#' + txtcode + ' input').autocomplete({
                source: function (request, response) {
                    $.getJSON("../../Common/AjaxUnit.ashx", request, function (data, status, xhr) {
                        response(data);
                    });
                }
            });
        });

    </script>
</body>
</html>
