<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Import.aspx.cs" Inherits="Enterprise.IIS.business.VehicleETC.Import" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>月通卡</title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region1" ShowBorder="False" ShowHeader="False" Title="月通卡" Position="Center"
                    Layout="Fit" BoxConfigAlign="Stretch" BoxConfigPosition="Left" runat="server">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <f:Button ID="btnClose" Text="关闭"
                                    runat="server" Icon="SystemClose" Size="Medium" OnClick="btnClose_OnClick">
                                </f:Button>
                                <f:Button ID="btnSubmit" Text="保存" runat="server" Icon="SystemSaveNew" ValidateForms="SimpleForm1"
                                    OnClick="btnSubmit_Click" Size="Medium">
                                </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Items>
                        <f:RegionPanel ID="RegionPanel2" ShowBorder="false" runat="server">
                            <Regions>
                                <f:Region ID="Region3" ShowBorder="false" ShowHeader="false" Layout="Fit" BodyPadding="3px" runat="server" AutoScroll="true">
                                    <Items>
                                        <f:SimpleForm ID="SimpleForm1" BodyPadding="5px" LabelWidth="135px" EnableAjax="true"
                                            runat="server" ShowBorder="false" ShowHeader="False" LabelAlign="Right"
                                            AutoScroll="true">
                                            <Items>
                                                <f:GroupPanel runat="server" Title="模版下载" ID="GPanelRoleInfo" EnableCollapse="True">
                                                    <Items>
                                                        <f:Label ID="Label1" runat="server" Label="模板说明" Text="模板说明">
                                                        </f:Label>
                                                        <f:ContentPanel ID="ContentPanel1" ShowBorder="false" BodyPadding="10px" ShowHeader="false"
                                                            AutoScroll="true" CssClass="intro" runat="server">
                                                            <a href="~/business/template/月通卡.xls" runat="server">下载模版</a>
                                                        </f:ContentPanel>
                                                    </Items>
                                                </f:GroupPanel>
                                                <f:GroupPanel runat="server" Title="客户名称" ID="GPanelActions" EnableCollapse="True">
                                                    <Items>
                                                        <f:Panel ID="PanelActions" ShowHeader="false" Title="ICON"
                                                            ShowBorder="false" runat="server">
                                                            <Items>
                                                                <f:DropDownList runat="server" ID="ddlFVehicleNum" Label="车牌号" EnableEdit="True" LabelAlign="Right" />

                                                                <f:FileUpload runat="server" ID="fileUpload1" ShowRedStar="false" ButtonText="上传数据"
                                                                    ButtonOnly="true" Required="false" ShowLabel="true" Label="上传数据" AutoPostBack="true"
                                                                    OnFileSelected="fileUpload1_FileSelected">
                                                                </f:FileUpload>

                                                            </Items>
                                                        </f:Panel>
                                                    </Items>
                                                </f:GroupPanel>
                                                <f:GroupPanel runat="server" Title="月通卡" ID="GroupPanel1" EnableCollapse="True" AutoScroll="True">
                                                    <Items>
                                                        <f:Grid ID="Grid1" Title="月通卡" ShowBorder="false" ShowHeader="False"
                                                            runat="server" EnableCheckBoxSelect="True" DataKeyNames="id" AllowPaging="False"
                                                            IsDatabasePaging="true" AllowSorting="False" EmptyText="查询无结果"
                                                            EnableHeaderMenu="True">
                                                            <Columns>
                                                                <%--<f:BoundField MinWidth="80px" DataField="车牌号" HeaderText="车牌号" SortField="车牌号" />--%>
                                                                <f:BoundField MinWidth="140px" DataField="入口时间" HeaderText="入口时间" SortField="入口时间" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" TextAlign="Center" />
                                                                <f:BoundField MinWidth="80px" DataField="入口站" HeaderText="入口站" SortField="入口站" />
                                                                <f:BoundField MinWidth="140px" DataField="出口时间" HeaderText="出口时间" SortField="出口时间" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" TextAlign="Center" />
                                                                <f:BoundField MinWidth="80px" DataField="出口站" HeaderText="出口站" SortField="出口站" />
                                                                <f:BoundField MinWidth="80px" DataField="交易省份" HeaderText="交易省份" SortField="交易省份" />
                                                                <f:BoundField MinWidth="120px" DataField="交易性质" HeaderText="交易性质" SortField="交易性质" />
                                                                <f:BoundField MinWidth="120px" DataField="支出" HeaderText="支出" SortField="支出" TextAlign="Right" />
                                                                <f:BoundField MinWidth="120px" DataField="存入" HeaderText="存入" SortField="存入" TextAlign="Right" />
                                                                <f:BoundField MinWidth="120px" DataField="通行应返还金额" HeaderText="通行应返还金额" SortField="通行应返还金额" TextAlign="Right" />
                                                            </Columns>
                                                        </f:Grid>
                                                    </Items>
                                                </f:GroupPanel>
                                            </Items>
                                        </f:SimpleForm>
                                    </Items>
                                </f:Region>
                            </Regions>
                        </f:RegionPanel>
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>
    </form>
    <script type="text/javascript">
        var basePath = '<%= ResolveUrl("~/") %>';
        function closeActiveTab() {
            parent.removeActiveTab();
        }
    </script>
</body>
</html>
