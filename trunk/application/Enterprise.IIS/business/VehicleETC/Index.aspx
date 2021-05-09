<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Enterprise.IIS.business.VehicleETC.Index" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>月通卡</title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <f:Button ID="btnSearch" runat="server" Text="查询" Icon="Zoom" OnClick="btnSearch_Click" Size="Medium">
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
            <Regions>
                <f:Region ID="Region1" Title="本厂月期初产品情况" Position="Center"
                    ShowBorder="False" BoxConfigAlign="Stretch" Layout="Fit" runat="server" ShowHeader="False">
                    <Items>
                        <f:RegionPanel ID="RegionPanel2" ShowBorder="false" runat="server">
                            <Regions>
                                <f:Region ID="Region3" ShowBorder="false" ShowHeader="false" Split="False"
                                    Position="Top" Layout="Fit"
                                    BodyPadding="3px" runat="server" Height="36px" EnableCollapse="True">
                                    <Items>
                                        <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="80px"
                                            runat="server" BodyPadding="5px" LabelAlign="Right">
                                            <Rows>
                                                <f:FormRow ID="FormRow1">
                                                    <Items>
                                                        <f:DropDownList runat="server" Hidden="True" ID="ddlCompany" Label="所属公司" LabelAlign="Right"></f:DropDownList>
                                                        
                                                        <f:DatePicker ID="dateBegin" runat="server" Label="开始日期" LabelAlign="Right">
                                                        </f:DatePicker>
                                                        <f:DatePicker ID="dateEnd" runat="server" Label="结束日期" LabelAlign="Right">
                                                        </f:DatePicker>
                                                        <f:DropDownList runat="server" ID="ddlFVehicleNum" Label="车牌号" EnableEdit="True"  LabelAlign="Right"/>
                                                        
                                                    </Items>
                                                </f:FormRow>
                                            </Rows>
                                        </f:Form>
                                    </Items>
                                </f:Region>
                                <f:Region ID="Region4" ShowBorder="False" ShowHeader="false" Position="Center"
                                    Layout="Fit" runat="server" BodyPadding="3px" AutoScroll="True">
                                    <Items>
                                        <f:Grid ID="Grid1" PageSize="20" ShowBorder="False" ShowHeader="False" AllowPaging="true"
                                            runat="server" EnableCheckBoxSelect="False" DataKeyNames="FId" OnPageIndexChange="Grid1_PageIndexChange"
                                            IsDatabasePaging="true" OnSort="Grid1_Sort" SortDirection="ASC"
                                            AllowSorting="true" EmptyText="查询无结果" EnableHeaderMenu="True">
                                            <Columns>
                                                
                                                <f:BoundField MinWidth="80px" DataField="FVehicleNum" HeaderText="车牌号" SortField="FVehicleNum" />
                                                <f:BoundField MinWidth="140px" DataField="FDateI" HeaderText="入口时间" SortField="FDateI" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" TextAlign="Center" />
                                                <f:BoundField MinWidth="80px" DataField="FPortalEntry" HeaderText="入口站" SortField="FPortalEntry" />
                                                <f:BoundField MinWidth="140px" DataField="FDateO" HeaderText="出口时间" SortField="FDateO" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" TextAlign="Center" />
                                                <f:BoundField MinWidth="80px" DataField="FPortalExit" HeaderText="出口站" SortField="FPortalExit" />
                                                <f:BoundField MinWidth="80px" DataField="FProvince" HeaderText="交易省份" SortField="FProvince" />
                                                <f:BoundField MinWidth="120px" DataField="FNature" HeaderText="交易性质" SortField="FNature" />
                                                <f:BoundField MinWidth="120px" DataField="FExpenditure" HeaderText="支出（元）" SortField="FExpenditure" TextAlign="Right" />
                                                <f:BoundField MinWidth="120px" DataField="FDeposit" HeaderText="存入（元）" SortField="FDeposit" TextAlign="Right" />
                                                <f:BoundField MinWidth="120px" DataField="FRebate" HeaderText="通行应返还金额（元）" SortField="FRebate" TextAlign="Right" />
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
    </form>
    <script type="text/javascript">
        var basePath = '<%= ResolveUrl("~/") %>';
        function openAddFineUI(url) {
            parent.addExampleTab.apply(null, ['add_fineui_pars', basePath + url, '引入月通卡信息', basePath + 'icon/database_add.png', true]);
        }

        function closeActiveTab() {
            parent.removeActiveTab();
        }
    </script>
</body>
</html>
