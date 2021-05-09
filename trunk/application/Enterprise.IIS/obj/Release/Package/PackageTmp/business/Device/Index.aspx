<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Enterprise.IIS.business.Device.Index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>钢瓶档案</title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <f:Button ID="btnAdd" runat="server" Text="新增" Icon="Add" Hidden="True" Size="Medium">
                        </f:Button>
                        <f:Button ID="btnEdit" OnClick="btnEdit_Click" runat="server" Hidden="True" Text="编辑"
                            Icon="PageWhiteEdit" Size="Medium">
                        </f:Button>
                        <f:Button ID="btnBatchDelete" OnClick="btnBatchDelete_Click" runat="server" Text="删除"
                            Icon="Delete" Hidden="True" Size="Medium">
                        </f:Button>
                        <f:Button ID="btnEnabled" OnClick="btnEnabled_Click" runat="server" Hidden="False"
                            Text="禁/启" Icon="Stop" Size="Medium">
                        </f:Button>
                        <f:Button ID="btnImport" OnClick="btnImport_Click" runat="server" Text="引入" Icon="PageExcel"
                            Hidden="False" EnableAjax="false" DisableControlBeforePostBack="false" Size="Medium">
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
                            ShowBorder="false" Icon="House" Title="报备分类" runat="server"
                            EnableArrows="true" AutoScroll="True">
                        </f:Tree>
                    </Items>
                </f:Region>
                <f:Region ID="Region2" Title="报备档案" Position="Center"
                    ShowBorder="False" BoxConfigAlign="Stretch" Layout="Fit" runat="server" ShowHeader="False">
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
                                                        <f:TextBox ID="txtFCode" runat="server" Label="编码">
                                                        </f:TextBox>
                                                        <f:TextBox ID="txtFName" runat="server" Label="名称">
                                                        </f:TextBox>
                                                        <f:Button ID="btnSearch" runat="server" Text="查询" Icon="Zoom" OnClick="btnSearch_Click">
                                                        </f:Button>
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
                                            runat="server" EnableCheckBoxSelect="True" DataKeyNames="FCode" OnPageIndexChange="Grid1_PageIndexChange"
                                            IsDatabasePaging="true" OnSort="Grid1_Sort" SortDirection="ASC"
                                            AllowSorting="true" EmptyText="查询无结果" EnableHeaderMenu="True">
                                            <Columns>
                                                <f:BoundField MinWidth="80px" DataField="FCode" HeaderText="编码" SortField="FCode" />
                                                <f:BoundField MinWidth="220px" DataField="FName" HeaderText="名称" SortField="FName" />
                                                <f:BoundField MinWidth="120px" DataField="FSpell" HeaderText="助记码" SortField="FSpell" />
                                                <f:BoundField MinWidth="120px" DataField="FSpec" HeaderText="规格" SortField="FSpec" />
                                                <f:BoundField MinWidth="120px" DataField="FUnit" HeaderText="单位" SortField="FUnit" />
                                                <f:BoundField MinWidth="120px" DataField="FParms" HeaderText="其它参数" SortField="FParms" />
                                                <f:BoundField MinWidth="120px" DataField="FPurchaseDate" HeaderText="购买日期" SortField="FPurchaseDate" />
                                                <f:BoundField MinWidth="120px" DataField="FInstallDate" HeaderText="安装日期" SortField="FInstallDate" />
                                                <f:BoundField MinWidth="120px" DataField="FUser" HeaderText="使有者" SortField="FUser" />
                                                <f:BoundField MinWidth="120px" DataField="FAddress" HeaderText="安装位置" SortField="FAddress" />
                                                <f:BoundField MinWidth="120px" DataField="FInspectionCycle" HeaderText="安装人" SortField="FInspectionCycle" />
                                                <f:BoundField MinWidth="120px" DataField="FDays" HeaderText="提醒天数" SortField="FDays" />
                                                <f:CheckBoxField MinWidth="100px" RenderAsStaticField="true" DataField="FFlag"
                                                    TextAlign="Center" HeaderText="是否启用" SortField="FFlag" />
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
            Target="Parent" EnableIFrame="true" IFrameUrl="about:blank" Height="480px" Width="380px"
            OnClose="Window1_Close">
        </f:Window>
    </form>
    <script type="text/javascript">
        var basePath = '<%= ResolveUrl("~/") %>';
        function openAddFineUI(url) {
            parent.addExampleTab.apply(null, ['add_fineui_bottle', basePath + url, '引入报备档案', basePath + 'icon/database_add.png', true]);
        }
        function closeActiveTab() {
            parent.removeActiveTab();
        }
    </script>
</body>
</html>
