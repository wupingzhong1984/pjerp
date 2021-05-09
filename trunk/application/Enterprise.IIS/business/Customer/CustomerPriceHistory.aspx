<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerPriceHistory.aspx.cs" Inherits="Enterprise.IIS.business.Customer.CustomerPriceHistory" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>查看历史变更价</title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
    <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
        <Regions>
            <f:Region ID="Region1" Title="查看历史变更价"  ShowBorder="False" BoxConfigAlign="Stretch" Layout="Region" RegionPosition="Center"  RegionSplit="true" runat="server" ShowHeader="False">
                <Toolbars>
                    <f:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <f:Button ID="btnSearch" runat="server" Text="查询" Icon="Zoom" Size="Medium" OnClick="btnSearch_Click">
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
                                BodyPadding="3px" runat="server" Height="40px" EnableCollapse="True">
                                <Items>
                                    <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="80px"
                                        runat="server" BodyPadding="5px" LabelAlign="Right">
                                        <Rows>
                                            <f:FormRow ID="FormRow1">
                                                <Items>
                                                    <f:TextBox ID="txtFName" runat="server" Label="客户名称">
                                                    </f:TextBox>
                                                    <f:TextBox ID="txtFItemName" runat="server" Label="商品名称">
                                                    </f:TextBox>
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
                                    <f:Grid ID="Grid1" PageSize="20" ShowBorder="false" ShowHeader="false" AllowPaging="true"
                                        runat="server" EnableCheckBoxSelect="True" DataKeyNames="FId" OnPageIndexChange="Grid1_PageIndexChange"
                                        IsDatabasePaging="true" OnSort="Grid1_Sort" SortDirection="ASC" 
                                        AllowSorting="true" EmptyText="查询无结果" EnableHeaderMenu="True">
                                        <Columns>
                                            <f:BoundField MinWidth="80px" DataField="FFlag" HeaderText="业务" SortField="FFlag" />
                                            <f:BoundField MinWidth="80px" DataField="FCode" HeaderText="客户代码" SortField="FCode" />
                                            <f:BoundField MinWidth="320px" DataField="FName" HeaderText="客户名称" SortField="FName" />
                                            <f:BoundField MinWidth="120px" DataField="FItemCode" HeaderText="商品代码" SortField="FItemCode" />
                                            <f:BoundField MinWidth="120px" DataField="FItemName" HeaderText="商品名称" SortField="FItemName" />
                                            <f:BoundField MinWidth="120px" DataField="FSpec" HeaderText="规格" SortField="FSpec" />
                                            <f:BoundField MinWidth="120px" DataField="FUnit" HeaderText="计数单位" SortField="FUnit" />
                                            <f:BoundField MinWidth="120px" DataField="FPrice" HeaderText="历史价" SortField="FPrice" TextAlign="Right"/>
                                            <f:BoundField MinWidth="120px" DataField="FUpdateBy" HeaderText="变更人" SortField="FUpdateBy" />
                                            <f:BoundField MinWidth="120px" DataField="FDate" HeaderText="变更时间" SortField="FDate"  DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" />
                                        </Columns>
                                        <PageItems>
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
        Target="Parent" EnableIFrame="true" IFrameUrl="about:blank" Height="430px" Width="460px"
        OnClose="Window1_Close">
    </f:Window>
         <f:Window ID="Window2" runat="server" WindowPosition="Center" IsModal="true"
        Icon="ApplicationViewDetail" EnableMaximize="true" EnableResize="true" Hidden="True"
        Target="Parent" EnableIFrame="true" IFrameUrl="about:blank" Height="430px" Width="460px"
        OnClose="Window2_Close">
    </f:Window>
    </form>
   <script type="text/javascript">
        function closeActiveTab() {
            parent.removeActiveTab();
        }
    </script>
</body>
</html>