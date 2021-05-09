<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetPriceIndex.aspx.cs" Inherits="Enterprise.IIS.business.Customer.SetPriceIndex" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>设置客户对应产品销售价</title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region1" Title="设置客户对应产品销售价" ShowBorder="False" BoxConfigAlign="Stretch" Layout="Region" RegionPosition="Center" RegionSplit="true" runat="server" ShowHeader="False">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <f:Button ID="btnSearch" runat="server" Text="查询" Icon="Zoom" Size="Medium" OnClick="btnSearch_Click">
                                </f:Button>
                                <f:Button ID="btnAdd" runat="server" Text="新增" Icon="Add" Hidden="True" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnEdit" OnClick="btnEdit_Click" runat="server" Hidden="True" Text="编辑"
                                    Icon="PageWhiteEdit" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnBatchDelete" OnClick="btnBatchDelete_Click" runat="server" Text="删除"
                                    Icon="Delete" Hidden="True" Size="Medium">
                                </f:Button>
                             <f:Button ID="btnImport" OnClick="btnImport_Click" runat="server" Text="引入" Icon="PageExcel"
                                    Hidden="False" EnableAjax="false" DisableControlBeforePostBack="false" Size="Medium">
                                </f:Button>
                                  <f:Button ID="btnExport" OnClick="btnExport_Click" runat="server" Text="引出" Icon="PageExcel"
                                    Hidden="False" EnableAjax="false" DisableControlBeforePostBack="false" Size="Medium">
                                </f:Button>

                                <f:Button ID="btnClose" EnablePostBack="False" Text="关闭" runat="server" Icon="SystemClose" Size="Medium" OnClientClick="closeActiveTab();">
                                </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Items>
                        <f:RegionPanel ID="RegionPanel2" ShowBorder="false" runat="server">
                            <Regions>
                                <f:Region ID="Region3" ShowBorder="false" ShowHeader="false" Split="False"
                                    Position="Top" Layout="Fit"
                                    BodyPadding="3px" runat="server" Height="68px" EnableCollapse="False">
                                    <Items>
                                        <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="80px"
                                            runat="server" BodyPadding="5px" LabelAlign="Right">
                                            <Rows>
                                                <f:FormRow ID="FormRow1">
                                                    <Items>
                                                        <f:TextBox OnTextChanged="txtFName_OnTextChanged" AutoPostBack="True" ID="txtFName" runat="server" Label="客户名称">
                                                        </f:TextBox>
                                                        <f:TextBox ID="txtFItemName" OnTextChanged="txtFItemName_OnTextChanged" AutoPostBack="True" runat="server" Label="商品名称">
                                                        </f:TextBox>
                                                        <f:NumberBox ID="txtminPrice" AutoPostBack="True" OnTextChanged="txtminPrice_OnTextChanged" runat="server" Label="价格区间" Width="30px"></f:NumberBox>
                                                        <f:NumberBox ID="txtMaxPrice" AutoPostBack="True" OnTextChanged="txtMaxPrice_OnTextChanged" runat="server" Label="至" Width="20px" LabelWidth="30px"></f:NumberBox>
                                                    </Items>
                                                </f:FormRow>
                                                <f:FormRow ID="FormRow2">
                                                    <Items>
                                                        <f:DropDownList runat="server" ID="ddlFSalesman" Label="业务员" EnableEdit="True" LabelAlign="Right" />

                                                        <f:Label runat="server" Hidden="True" ></f:Label>
                                                        <f:Label runat="server" Hidden="True" ></f:Label>
                                                        <f:Label runat="server" Hidden="True" ></f:Label>
                                                    </Items>
                                                </f:FormRow>
                                            </Rows>
                                        </f:Form>
                                    </Items>
                                </f:Region>
                                <f:Region ID="Region4" ShowBorder="False" ShowHeader="false" Position="Center"
                                    Layout="Fit" runat="server" BodyPadding="3px" AutoScroll="True">
                                    <Items>
                                        <f:Grid ID="Grid1" PageSize="200" ShowBorder="false" ShowHeader="false" AllowPaging="true"
                                            runat="server" EnableCheckBoxSelect="True" DataKeyNames="FId" OnPageIndexChange="Grid1_PageIndexChange"
                                            IsDatabasePaging="true" OnSort="Grid1_Sort" SortDirection="ASC"
                                            AllowSorting="true" EmptyText="查询无结果" EnableHeaderMenu="True">
                                            <Columns>
                                                <f:BoundField MinWidth="80px" DataField="FCode" HeaderText="客户代码" SortField="FCode" />
                                                <f:BoundField MinWidth="320px" DataField="FName" HeaderText="客户名称" SortField="FName" />
                                                <f:BoundField MinWidth="120px" DataField="FItemCode" HeaderText="商品代码" SortField="FItemCode" />
                                                <f:BoundField MinWidth="120px" DataField="FItemName" HeaderText="商品名称" SortField="FItemName" />
                                                <f:BoundField MinWidth="120px" DataField="FItemSpec" HeaderText="规格" SortField="FItemSpec" />
												<f:BoundField MinWidth="120px" DataField="FUnit" HeaderText="单位" SortField="FUnit" />
                                                <f:BoundField MinWidth="120px" DataField="FPrice" HeaderText="销售价" SortField="FPrice" TextAlign="right" />
                                                <f:BoundField MinWidth="110px" DataField="FBeginDate" HeaderText="有效开始日期" SortField="FBeginDate" DataFormatString="{0:yyyy-MM-dd}" TextAlign="Center" />
                                                <%--<f:BoundField MinWidth="110px" DataField="FEndDate" HeaderText="有效结束日期" SortField="FEndDate" DataFormatString="{0:yyyy-MM-dd}" TextAlign="Center" />
                                            <f:BoundField MinWidth="120px" DataField="FCreateBy" HeaderText="制单人" SortField="FCreateBy" />
                                            <f:BoundField MinWidth="110px" DataField="FCreateDate" HeaderText="制单日期" SortField="FCreateDate" DataFormatString="{0:yyyy-MM-dd}" TextAlign="Center" />
                                             <f:BoundField MinWidth="120px" DataField="FUpdateBy" HeaderText="变更人" SortField="FUpdateBy" />
                                            <f:BoundField MinWidth="110px" DataField="FUpdateDate" HeaderText="变更日期" SortField="FUpdateDate" DataFormatString="{0:yyyy-MM-dd}" TextAlign="Center" />
                                                --%>

                                                <f:BoundField MinWidth="120px" DataField="FMemo" HeaderText="备注" SortField="FMemo" />

                                            </Columns>
                                            <PageItems>
                                                <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                                </f:ToolbarSeparator>
                                                <f:ToolbarText ID="ToolbarText1" runat="server" Text="每页记录数：">
                                                </f:ToolbarText>
                                                <f:DropDownList runat="server" ID="ddlPageSize" Width="80px" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
                                                    <f:ListItem Text="200" Value="200" />
                                                    <f:ListItem Text="300" Value="300" />
                                                    <f:ListItem Text="500" Value="500" />
                                                </f:DropDownList>
                                            </PageItems>
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
            Target="Parent" EnableIFrame="true" IFrameUrl="about:blank" Height="430px" Width="360px"
            OnClose="Window1_Close">
        </f:Window>
        <f:Window ID="Window2" runat="server" WindowPosition="Center" IsModal="true"
            Icon="ApplicationViewDetail" EnableMaximize="true" EnableResize="true" Hidden="True"
            Target="Parent" EnableIFrame="true" IFrameUrl="about:blank" Height="430px" Width="360px"
            OnClose="Window2_Close">
        </f:Window>
    </form>
    <script type="text/javascript">
        var basePath = '<%= ResolveUrl("~/") %>';
        function openAddFineUI(url) {
            parent.addExampleTab.apply(null, ['add_fineui_customer_price', basePath + url, '引入客户对应产品价格表', basePath + 'icon/database_add.png', true]);
        }
        function openAddnewFineUI(url) {
            parent.addExampleTab.apply(null, ['add_fineui_customer_price', basePath + url, '合同管理', basePath + 'icon/database_add.png', true]);
        }
        function closeActiveTab() {
            parent.removeActiveTab();
        }
    </script>
</body>
</html>
