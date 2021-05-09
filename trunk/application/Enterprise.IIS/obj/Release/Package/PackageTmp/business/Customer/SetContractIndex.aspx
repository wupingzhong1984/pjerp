<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetContractIndex.aspx.cs" Inherits="Enterprise.IIS.business.Customer.SetContractIndex" %>

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
                <f:Region ID="Region1" Title="设置客户合同" ShowBorder="False" BoxConfigAlign="Stretch" Layout="Region" RegionSplit="true" runat="server" ShowHeader="False">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <f:Button ID="btnSearch" runat="server" Text="查询" Icon="Zoom" Size="Medium" OnClick="btnSearch_Click">
                                </f:Button>
                                <f:Button ID="btnAdd" runat="server" Text="新增" Icon="Add" Hidden="False" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnEdit" OnClick="btnEdit_Click" runat="server" Hidden="False" Text="编辑"
                                    Icon="PageWhiteEdit" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnBatchDelete" OnClick="btnBatchDelete_Click" runat="server" Text="删除"
                                    Icon="Delete" Hidden="False" Size="Medium">
                                </f:Button>
                                 <f:Button ID="btnSubmit" OnClick="btnSubmit_Click" runat="server" Text="生效"
                                    Icon="Accept" Hidden="False" Size="Medium">
                                </f:Button>
                                <%--<f:Button ID="btnView" OnClick="btnView_Click" runat="server" Text="查看合同"
                                    Icon="ApplicationViewTile" Hidden="False" Size="Medium">
                                </f:Button>--%>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Items>
                        <f:RegionPanel ID="RegionPanel2" ShowBorder="false" runat="server">
                            <Regions>
                                <f:Region ID="Region3" ShowBorder="false" ShowHeader="false" Split="False"
                                    Position="Top" Layout="Fit"
                                    BodyPadding="3px" runat="server" Height="40px" EnableCollapse="True">
                                    <Items>
                                        <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="80px"
                                            runat="server" BodyPadding="5px">
                                            <Rows>
                                                <f:FormRow ID="FormRow1">
                                                    <Items>
                                                        <f:TextBox ID="txtFName" runat="server" Label="客户名称"  Width="80px">
                                                        </f:TextBox>
                                                        <f:TextBox ID="txtFItemName" runat="server" Label="商品名称">
                                                        </f:TextBox>
                                                         <%--<f:DatePicker ID="dpkFDateBegin" runat="server" Label="开始时间" DateFormatString="yyyy-MM-dd">
                                                        </f:DatePicker>
                                                        <f:DatePicker ID="dpkFDateEnd" runat="server" Label="结束时间" DateFormatString="yyyy-MM-dd">
                                                        </f:DatePicker>--%>
                                                        <f:NumberBox ID="txtminPrice" runat="server" Label="价格区间" Width="80px"></f:NumberBox>
                                                        <f:NumberBox ID="txtMaxPrice" runat="server" Width="80px" Label="至"></f:NumberBox>

                                                    </Items>
                                                </f:FormRow>
                                            </Rows>
                                        </f:Form>
                                    </Items>
                                </f:Region>
                                <f:Region ID="Region4" ShowBorder="False" ShowHeader="false" Position="Top" Height="270px"
                                    Layout="Fit" runat="server" BodyPadding="3px" AutoScroll="True">
                                    <Items>
                                        <f:Grid ID="Grid1" PageSize="20" ShowBorder="false" ShowHeader="false" AllowPaging="true" EnableRowSelectEvent="true" EnableMultiSelect="false"
                                            runat="server" EnableCheckBoxSelect="True" DataKeyNames="FId,FOrderCode"  IsDatabasePaging="true" OnSort="Grid1_Sort" SortDirection="ASC"
                                            AllowSorting="true" EmptyText="查询无结果" EnableHeaderMenu="True" OnRowClick="Grid1_RowClick" EnableRowClickEvent="true">
                                            <Columns>
                                                <f:BoundField MinWidth="120px" DataField="FOrderCode" HeaderText="编号" SortField="FOrderCode" />
                                                <f:BoundField MinWidth="120px" DataField="FContractCode" HeaderText="合同编号" SortField="FContractCode" />
                                                <f:BoundField MinWidth="120px" DataField="customername" HeaderText="客户名称" SortField="customername" />
                                              <%--  <f:BoundField MinWidth="120px" DataField="FAccType" HeaderText="结算方式" SortField="FAccType" />
                                                <f:BoundField MinWidth="120px" DataField="FBillType" HeaderText="票据类型" SortField="FBillType" />--%>
                                                <f:BoundField MinWidth="100px" DataField="FBeginDate" HeaderText="开始时间" SortField="FBeginDate" DataFormatString="{0:yyyy-MM-dd}" />
                                                <f:BoundField MinWidth="100px" DataField="FEndDate" HeaderText="结束时间" SortField="FEndDate" DataFormatString="{0:yyyy-MM-dd}" />
                                                <f:BoundField MinWidth="120px" DataField="UserName" HeaderText="变更人" SortField="UserName" />
                                                <f:BoundField MinWidth="100px" DataField="FCreatedon" HeaderText="时间" SortField="FCreatedon" DataFormatString="{0:yyyy-MM-dd}" />
                                                
                                                <f:BoundField MinWidth="160px" DataField="FContext" HeaderText="备注" SortField="FContext" />
                                                <%--<f:BoundField MinWidth="120px" DataField="FCreatedon" HeaderText="上传文件" SortField="FItemCode" />--%>
                                            </Columns>
                                           
                                        </f:Grid>
                                    </Items>
                                </f:Region>
                                <f:Region ShowBorder="False" ShowHeader="false" Position="Bottom"
                                    Layout="Fit" runat="server" BodyPadding="3px" AutoScroll="True" Height="190px">
                                    <Items>
                                        <f:Grid ID="Grid2"  ShowBorder="false" ShowHeader="false" AllowPaging="false"
                                            runat="server" EnableCheckBoxSelect="True" DataKeyNames="FID" SortDirection="ASC"
                                            AllowSorting="true" EmptyText="查询无结果" EnableHeaderMenu="True">
                                            <Columns>
                                                <f:BoundField MinWidth="120px" DataField="FProductID" HeaderText="商品代码" SortField="FProductID" />
                                                <f:BoundField MinWidth="120px" DataField="ProductName" HeaderText="商品名称" SortField="ProductName" />
                                                <f:BoundField MinWidth="120px" DataField="FUnit" HeaderText="单位" SortField="FUnit" />
                                                <f:BoundField MinWidth="120px" DataField="FSpec" HeaderText="规格" SortField="FSpec" />
                                                <f:BoundField MinWidth="120px" DataField="FPrice" HeaderText="单价" SortField="FPrice" />
                                                <f:BoundField MinWidth="120px" DataField="statename" HeaderText="状态" SortField="statename" />
                                                <%--<f:BoundField MinWidth="120px" DataField="FCreatedon" HeaderText="上传文件" SortField="FItemCode" />--%>
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
            Target="Parent" EnableIFrame="true" IFrameUrl="about:blank" Height="530px" Width="760px"
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
