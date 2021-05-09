<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SaleItemDetails.aspx.cs" Inherits="Enterprise.IIS.business.Reports.SaleItemDetails" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
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
                <f:Region ID="Region1" Title="产品销售明细" Position="Center"
                    ShowBorder="false" BoxConfigAlign="Stretch" Layout="Fit" runat="server" ShowHeader="false">
                     <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                               
                                <f:Button ID="btnClose" EnablePostBack="false" OnClientClick="closeActiveTab();" Text="关闭"
                                    runat="server" Icon="SystemClose" Size="Medium">
                                </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Items>
                        <f:RegionPanel ID="RegionPanel2" ShowBorder="false" runat="server">
                            <Regions>                                
                                <f:Region ID="Region4" ShowBorder="False" ShowHeader="False" Position="Center"
                                    Layout="Fit" runat="server" BodyPadding="3px" AutoScroll="True">
                                    <Items>
                                        <f:Grid ID="Grid1" ShowBorder="False" ShowHeader="False" AllowPaging="False"
                                            runat="server" EnableCheckBoxSelect="False" DataKeyNames="KeyId,FCode"                                             EnableAjax="True" EnableAjaxLoading="True"
                                            IsDatabasePaging="False"  SortDirection="ASC"
                                            AllowSorting="false" EmptyText="查询无结果" EnableHeaderMenu="True"
                                            EnableSummary="true" SummaryPosition="Bottom">
                                            <Columns>
                                                <f:BoundField MinWidth="80px" DataField="FCode" HeaderText="客户代码" SortField="FCode" />
                                                <f:BoundField MinWidth="220px" DataField="customerName" HeaderText="客户名称" SortField="customerName" />
                                                <f:BoundField MinWidth="80px" ColumnID="FItemCode" DataField="FItemCode" HeaderText="商品代码" SortField="FItemCode" TextAlign="Right" />
                                                <f:BoundField MinWidth="80px" ColumnID="FName" DataField="FName" HeaderText="商品名称" SortField="FName" TextAlign="Right" />
                                                <f:BoundField MinWidth="80px" ColumnID="FBSpec" DataField="FBSpec" HeaderText="规格" SortField="FBSpec" TextAlign="Right" />
                                                <f:BoundField MinWidth="80px" ColumnID="FCateName" DataField="FCateName" HeaderText="类型" SortField="FCateName" TextAlign="Right" />
                                                <f:BoundField MinWidth="80px" ColumnID="fQty" DataField="fQty" HeaderText="销售数量" DataFormatString="{0:f0}" SortField="fQty" TextAlign="Right" />
                                                <f:BoundField MinWidth="80px" ColumnID="price" DataField="price" HeaderText="单价" DataFormatString="{0:f2}" SortField="price" TextAlign="Right" />
                                                <f:BoundField MinWidth="80px" ColumnID="fAmount" DataField="fAmount" DataFormatString="{0:f2}" HeaderText="金额" SortField="fAmount" />
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
    </form>
</body>
</html>
