<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Enterprise.IIS.business.SalesPlanAudit.Index" %>
<%@ Import Namespace="Enterprise.Framework.Enum" %>
<%@ Import Namespace="Enterprise.IIS.Common" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>销售订单审核</title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
        <style type="text/css">
        .colorred {
            color: red;
        }
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
                <f:Region ID="Region1" ShowHeader="False" Split="True" Hidden="False" ShowBorder="False"
                    Width="220px" Position="Left" Layout="Fit" runat="server">
                    <Items>
                        <f:SimpleForm ID="SimpleForm2" BodyPadding="5px" LabelWidth="72px" EnableCollapse="False"
                            runat="server" ShowBorder="False" ShowHeader="False" LabelAlign="Right"
                            Title="查询条件">
                            <Items>
                                <f:DatePicker ID="dpFDateBegion" Label="开始日期" Required="true" runat="server">
                                </f:DatePicker>
                                <f:DatePicker ID="dpFDateEnd" Label="结束日期" Required="true" CompareControl="dpFDateBegion"
                                    CompareOperator="GreaterThan" CompareMessage="结束日期应该大于开始日期！" runat="server">
                                </f:DatePicker>
                                <f:DropDownList runat="server" ID="ddlFFlag" Label="审核状态">
                                    <f:ListItem Text="全部" Value="-1" Selected="true" />
                                    <f:ListItem Text="审核通过" Value="1"/>
                                    <f:ListItem Text="审核不通过" Value="2"/>
                                </f:DropDownList>
                                <f:TextBox runat="server" ID="txtKeyId" Label="单号" />
                                <f:TextBox runat="server" ID="txtFCode" Label="客户代码" />
                                <f:TextBox runat="server" ID="txtFName" Label="客户名称" />
                                <f:TextBox runat="server" ID="txtItemCode" Label="商品代码" />
                                <f:TextBox runat="server" ID="txtItemName" Label="商品名称" />
                                <f:TextBox runat="server" ID="txtFQty" Label="发货数量" Hidden="True"/>
                                <f:DropDownList runat="server" ID="ddlFDriver" Label="司机"  Hidden="True"></f:DropDownList>
                                <f:DropDownList runat="server" ID="ddlFShipper" Label="发货人"  Hidden="True"></f:DropDownList>
                                <f:DropDownList runat="server" ID="ddlOperator" Label="操作员"  Hidden="True"></f:DropDownList>
                            </Items>
                        </f:SimpleForm>
                    </Items>
                </f:Region>
                <f:Region ID="Region2" Title="客户管理" Position="Center"
                    ShowBorder="False" BoxConfigAlign="Stretch" Layout="Fit" runat="server" ShowHeader="False">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <f:Button ID="btnSearch" runat="server" Text="查询" Icon="Zoom" OnClick="btnSearch_Click" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnReset" runat="server" Text="重置条件" Icon="ArrowRefresh" OnClick="btnRest_Click" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnAudit" OnClick="btnAuditYes_Click" runat="server" Text="审核通过"
                                    Icon="PackageStart" Hidden="False" Size="Medium">
                                </f:Button>
                                
                                <f:Button ID="Button1" OnClick="btnAuditNo_Click" runat="server" Text="审核不通过"
                                    Icon="PackageStop" Hidden="False" Size="Medium">
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
                                <f:Region ID="Region3" ShowBorder="false" ShowHeader="False" Split="False"
                                    Position="Top" Layout="Fit"
                                    BodyPadding="0px" runat="server" Height="280px" EnableCollapse="True">
                                    <Items>
                                        <f:Grid ID="Grid1" PageSize="200" ShowBorder="False" ShowHeader="False" AllowPaging="False"
                                            runat="server" EnableCheckBoxSelect="True" DataKeyNames="KeyId" KeepCurrentSelection="True"
                                            IsDatabasePaging="False" SortDirection="ASC" OnRowDeselect="Grid1_OnRowDeselect" EnableRowDeselectEvent="True"
                                            AllowSorting="False" EmptyText="查询无结果" EnableHeaderMenu="True"  EnableSummary="true" SummaryPosition="Bottom"
                                            EnableMultiSelect="True" EnableRowClickEvent="true" OnRowClick="Grid1_RowClick"
                                            OnRowDataBound="Grid1_RowDataBound">
                                            <Columns>
                                                <f:TemplateField Width="100px" HeaderText="单据状态" TextAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAuditFlag" runat="server" Text='<%# EnumDescription.GetFieldText((GasEnumAuditFlag) Eval("FAuditFlag")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                </f:TemplateField>
                                                <f:BoundField MinWidth="220px" DataField="FName" HeaderText="客户名称" SortField="FName" />
                                                <f:BoundField MinWidth="80px" ColumnID="FCreditValue" DataField="FCreditValue" HeaderText="尚欠款" SortField="FCreditValue"  TextAlign="Right"/>
                                                <f:BoundField MinWidth="80px" ColumnID="FAmount" DataField="FAmount" HeaderText="本次销售金额" SortField="FAmount"  TextAlign="Right"/>
                                                <f:BoundField MinWidth="80px" DataField="FAuditor" HeaderText="审核人" SortField="FAuditor" />
                                                <f:BoundField MinWidth="80px" DataField="CreateBy" HeaderText="操作员" SortField="CreateBy" />
                                                <f:BoundField MinWidth="126px" ColumnID="KeyId" DataField="KeyId" HeaderText="单号" SortField="KeyId" />
                                                <f:BoundField MinWidth="110px" DataField="FDate" HeaderText="日期" SortField="FDate" DataFormatString="{0:yyyy-MM-dd}" />
                                                <f:BoundField MinWidth="160px" DataField="FMemo" HeaderText="备注" SortField="FMemo" />
                                                <f:BoundField MinWidth="80px" ColumnID="FCode" DataField="FCode" HeaderText="客户代码" SortField="FCode" />
                                            </Columns>
                                        </f:Grid>
                                    </Items>
                                </f:Region>
                                <f:Region ID="Region4" ShowBorder="False" ShowHeader="false" Position="Center"
                                    Layout="Fit" runat="server" BodyPadding="3px" AutoScroll="True">
                                    <Items>
                                        <f:Grid ID="Grid2" PageSize="200" ShowBorder="False" ShowHeader="False" AllowPaging="False"
                                            runat="server" EnableCheckBoxSelect="False" DataKeyNames="FId"
                                            IsDatabasePaging="False" AllowSorting="False" EmptyText="查询无结果" EnableHeaderMenu="True">
                                            <Columns>
                                                <f:BoundField MinWidth="80px" DataField="FINum" HeaderText="商品代码" SortField="FINum" />
                                                <f:BoundField MinWidth="220px" DataField="FItemName" HeaderText="商品名称" SortField="FItemName" />
                                                <f:BoundField MinWidth="80px" DataField="FSpec" HeaderText="规格" SortField="FSpec" />
                                                <f:BoundField MinWidth="80px" DataField="FUnit" HeaderText="单位" SortField="FUnit" />
                                                <f:BoundField MinWidth="80px" DataField="FPrice" HeaderText="单价" SortField="FPrice"  TextAlign="Right" />
                                                <f:BoundField MinWidth="80px" DataField="FQty" HeaderText="数量" SortField="FQty"  TextAlign="Right" />
                                                <f:BoundField MinWidth="80px" DataField="FAmount" HeaderText="金额" SortField="FAmount"  TextAlign="Right" />
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
            Target="Parent" EnableIFrame="true" IFrameUrl="about:blank" Height="430px" Width="760px"
            OnClose="Window1_Close">
        </f:Window>
    </form>
    <script type="text/javascript">
        var basePath = '<%= ResolveUrl("~/") %>';

        function closeActiveTab() {
            parent.removeActiveTab();
        };
    </script>
</body>
</html>

