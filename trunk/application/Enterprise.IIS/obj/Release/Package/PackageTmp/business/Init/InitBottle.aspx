<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InitBottle.aspx.cs" Inherits="Enterprise.IIS.business.Init.InitBottle" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>引入钢瓶档案</title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region1" ShowBorder="False" ShowHeader="False" Title="引入钢瓶档案" Position="Center"
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
                                <f:Region ID="Region3" ShowBorder="false" ShowHeader="false" Layout="Fit" BodyPadding="3px" runat="server">
                                    <Items>
                                        <f:SimpleForm ID="SimpleForm1" BodyPadding="5px" LabelWidth="135px" EnableAjax="true"
                                            runat="server" ShowBorder="false" ShowHeader="False"
                                            AutoScroll="true">
                                            <Items>
                                                <f:GroupPanel runat="server" Title="模版下载" ID="GPanelRoleInfo" EnableCollapse="True">
                                                    <Items>
                                                        <f:Label ID="Label1" runat="server" Label="模板说明" Text="模板说明">
                                                        </f:Label>
                                                        <f:ContentPanel ID="ContentPanel1" ShowBorder="false" BodyPadding="10px" ShowHeader="false"
                                                            AutoScroll="true" CssClass="intro" runat="server">
                                                            <a href="~/business/template/钢瓶档案.xls" runat="server">下载模版</a>
                                                        </f:ContentPanel>
                                                    </Items>
                                                </f:GroupPanel>
                                                <f:GroupPanel runat="server" Title="功能授权" ID="GPanelActions" EnableCollapse="True">
                                                    <Items>
                                                        <f:Panel ID="PanelActions" ShowHeader="false" Title="ICON"
                                                            ShowBorder="false" runat="server">
                                                            <Items>
                                                                <f:FileUpload runat="server" ID="fileUpload1" ShowRedStar="false" ButtonText="上传数据"
                                                                    ButtonOnly="true" Required="false" ShowLabel="true" Label="上传数据" AutoPostBack="true"
                                                                    OnFileSelected="fileUpload1_FileSelected">
                                                                </f:FileUpload>
                                                            </Items>
                                                        </f:Panel>
                                                    </Items>
                                                </f:GroupPanel>
                                                <f:Grid ID="Grid1" Title="引入钢瓶档案" ShowBorder="false" ShowHeader="False"
                                                    runat="server" EnableCheckBoxSelect="True" DataKeyNames="id" AllowPaging="False"
                                                    IsDatabasePaging="true" AllowSorting="False" EmptyText="查询无结果"
                                                    EnableHeaderMenu="True">
                                                    <Columns>
                                                        <f:BoundField Width="100px" DataField="商品代码" HeaderText="商品代码" SortField="商品代码" />
                                                        <f:BoundField Width="300px" DataField="商品名称" HeaderText="商品名称" SortField="商品名称" />
                                                        <f:BoundField Width="100px" DataField="规格" HeaderText="规格" SortField="规格" />
                                                        <f:BoundField Width="100px" DataField="计量单位" HeaderText="计量单位" SortField="计量单位" />
                                                        <f:BoundField Width="100px" DataField="采购单价" HeaderText="采购单价" SortField="采购单价" />
                                                        <f:BoundField Width="100px" DataField="发货单价" HeaderText="发货单价" SortField="发货单价" />
                                                        <f:BoundField Width="100px" DataField="摘要" HeaderText="摘要" SortField="摘要" />
                                                    </Columns>
                                                </f:Grid>
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
