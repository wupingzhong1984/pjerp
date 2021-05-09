<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WinSalesList.aspx.cs" Inherits="Enterprise.IIS.Common.WinSalesList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>发货单</title>
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>

                <f:Region ID="Region1" Title="发货单" Position="Center"
                    ShowBorder="True" BoxConfigAlign="Stretch" Layout="Fit" runat="server" ShowHeader="False">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <f:Button ID="btnSearch" OnClick="btnSearch_Click" runat="server" Text="查询" Icon="Zoom" Hidden="False" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnSubmit" OnClick="btnSubmit_Click" runat="server" Text="确认" Icon="Add" Hidden="False" Size="Medium">
                                </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Items>
                        <f:RegionPanel ID="RegionPanel2" ShowBorder="false" runat="server">
                            <Regions>
                                <f:Region ID="Region3" ShowBorder="false" ShowHeader="false" Split="False"
                                    Position="Top" Layout="Fit"
                                    BodyPadding="3px" runat="server" Height="70px" EnableCollapse="True">
                                    <Items>
                                        <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="80px"
                                            runat="server" BodyPadding="5px">
                                            <Rows>
                                                <f:FormRow ID="FormRow1">
                                                    <Items>
                                                        <f:DatePicker ID="dateBegin" runat="server" Label="开始日期">
                                                        </f:DatePicker>
                                                        <f:DatePicker ID="dateEnd" runat="server" Label="结束日期">
                                                        </f:DatePicker>
                                                        <f:DropDownList runat="server" ID="ddlFStatus" Label="业务状态" EnableEdit="True" />
                                                    </Items>
                                                </f:FormRow>
                                                <f:FormRow ID="FormRow2">
                                                    <Items>
                                                        <f:TextBox ID="txtFName" runat="server" Label="客户名称">
                                                        </f:TextBox>
                                                        <f:TextBox ID="txtFItemName" runat="server" Label="商品名称">
                                                        </f:TextBox>
                                                        <f:TextBox ID="txtFKeyId" runat="server" Label="单据编号">
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
                                        <f:Grid ID="Grid1" PageSize="20000" ShowBorder="False" ShowHeader="False" AllowPaging="False"
                                            runat="server" EnableCheckBoxSelect="True" DataKeyNames="KeyId" OnPageIndexChange="Grid1_PageIndexChange"
                                            IsDatabasePaging="true" OnSort="Grid1_Sort" SortDirection="DESC" EnableRowDoubleClickEvent="true" OnRowDoubleClick="Grid1_RowClick"
                                            AllowSorting="true" EmptyText="查询无结果" EnableHeaderMenu="True">
                                            <Columns>
                                                <f:BoundField MinWidth="80px" DataField="KeyId" HeaderText="单据号" SortField="KeyId" TextAlign="Center" runat="server" />
                                                <f:BoundField MinWidth="80px" DataField="FDate" HeaderText="销售日期" SortField="FDate" DataFormatString="{0:yyyy-MM-dd}" TextAlign="Center" />
                                                <f:BoundField MinWidth="80px" DataField="FCode" HeaderText="客户代码" SortField="FCode" />
                                                <f:BoundField MinWidth="220px" DataField="FName" HeaderText="客户名称" SortField="FName" />
                                                <f:BoundField MinWidth="80px" DataField="CreateBy" HeaderText="操作员" SortField="CreateBy" TextAlign="Center" />
                                                <f:BoundField MinWidth="120px" DataField="FVehicleNum" HeaderText="车牌号" SortField="FVehicleNum" TextAlign="Center" />
                                                <f:BoundField MinWidth="120px" DataField="FDriver" HeaderText="送货司机" SortField="FDriver" TextAlign="Center" />
                                                <f:BoundField MinWidth="120px" DataField="FSupercargo" HeaderText="押运员" SortField="FSupercargo" TextAlign="Center" />
                                                <f:BoundField MinWidth="120px" DataField="FShipper" HeaderText="发货人" SortField="FShipper" TextAlign="Center" />
                                                <f:BoundField MinWidth="120px" DataField="FFreight" HeaderText="运输服务费" SortField="FFreight" TextAlign="Right" />
                                                <f:BoundField MinWidth="120px" DataField="FAmount" HeaderText="金额" SortField="FAmount" TextAlign="Right" />
                                                <f:BoundField MinWidth="120px" DataField="FMemo" HeaderText="备注" SortField="FMemo" />
                                            </Columns>
                                            <%--<PageItems>
                                                <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                                </f:ToolbarSeparator>
                                                <f:ToolbarText ID="ToolbarText1" runat="server" Text="每页记录数：">
                                                </f:ToolbarText>
                                                <f:DropDownList runat="server" ID="ddlPageSize" Width="80px" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
                                                    <f:ListItem Text="20" Value="20" />
                                                    <f:ListItem Text="30" Value="30" />
                                                    <f:ListItem Text="50" Value="50" />
                                                </f:DropDownList>
                                            </PageItems>--%>
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
            Target="Parent" EnableIFrame="True" IFrameUrl="about:blank" Height="580px" Width="960px"
            OnClose="Window1_Close">
        </f:Window>
    </form>
    <script type="text/javascript">
        var basePath = '<%= ResolveUrl("~/") %>';
        function closeActiveTab() {
            parent.removeActiveTab();
        }
    </script>
</body>
</html>
