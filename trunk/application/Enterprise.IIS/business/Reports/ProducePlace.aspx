<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProducePlace.aspx.cs" Inherits="Enterprise.IIS.business.Reports.ProducePlace" %>

<%@ Import Namespace="Enterprise.Framework.Enum" %>
<%@ Import Namespace="Enterprise.IIS.Common" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>领料表</title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region1" Title="领料表" Position="Center"
                    ShowBorder="false" BoxConfigAlign="Stretch" Layout="Fit" runat="server" ShowHeader="false">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <f:Button ID="Button1" OnClick="Button1_Click" runat="server" Text="查询"
                                    Icon="Zoom" Hidden="False" Size="Medium" ValidateForms="SimpleForm1">
                                </f:Button>
                                <f:Button ID="btnConfirm" OnClick="btnConfirm_Click" runat="server" Text="审核通过"
                                    Icon="PackageStart" Hidden="False" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnUnConfirm" OnClick="btnUnConfirm_Click" runat="server" Text="审核不通过"
                                    Icon="PackageDown" Hidden="False" Size="Medium">
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
                                <f:Region ID="Region3" ShowBorder="false" ShowHeader="false" Split="False"
                                    Position="Top" Layout="Fit"
                                    BodyPadding="3px" runat="server" Height="38px" EnableCollapse="True">
                                    <Items>
                                        <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="80px"
                                            runat="server" BodyPadding="5px">
                                            <Rows>
                                                <f:FormRow ID="FormRow1">
                                                    <Items>
                                                        <f:DatePicker ID="dpkFDateBegin" runat="server" Label="开始时间" DateFormatString="yyyy-MM-dd" LabelAlign="Right">
                                                        </f:DatePicker>
                                                        <f:DatePicker ID="dpkFDateEnd" runat="server" Label="结束时间" DateFormatString="yyyy-MM-dd" LabelAlign="Right">
                                                        </f:DatePicker>
                                                        <f:DropDownList runat="server" ID="ddlOrgnization" Label="领用部门" />
                                                        <f:DropDownList runat="server" ID="ddlFGroup" Label="班组"
                                                           LabelAlign="Right"  EnableSimulateTree="true" />
                                                        <f:DropDownList runat="server" ID="ddlFDistributionPoint" Label="作业区" Required="True" LabelAlign="Right" ShowRedStar="True" EnableEdit="True" />
                                                    </Items>
                                                </f:FormRow>
                                            </Rows>
                                        </f:Form>
                                    </Items>
                                </f:Region>
                                <f:Region ID="Region4" ShowBorder="False" ShowHeader="False" Position="Center"
                                    Layout="Fit" runat="server" BodyPadding="3px" AutoScroll="True">
                                    <Items>
                                        <f:Grid ID="Grid1" PageSize="200" ShowBorder="false" ShowHeader="false" AllowPaging="true"
                                            runat="server" EnableCheckBoxSelect="True" DataKeyNames="KeyId" OnPageIndexChange="Grid1_PageIndexChange"
                                            IsDatabasePaging="true" OnRowCommand="Grid1_RowCommand" OnRowDataBound="Grid1_RowDataBound"
                                            AllowSorting="true" EmptyText="查询无结果" EnableHeaderMenu="True" EnableRowClickEvent="true"
                                           >
                                            <Columns>
                                                <f:LinkButtonField Width="120px"  ColumnID="KeyId" CommandName="actView" DataTextField="KeyId" HeaderText="单据号" SortField="KeyId" TextAlign="Center" runat="server" />
                                                <f:TemplateField Width="100px" ColumnID="FFlag" HeaderText="作废标识" TextAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFFlag" runat="server" Text='<%# EnumDescription.GetFieldText((GasBillFlag) Eval("FFlag")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                </f:TemplateField>
                                                <f:TemplateField Width="100px" HeaderText="审核状态" TextAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAuditStatus" runat="server" Text='<%# EnumDescription.GetFieldText((GasEnumAuditFlag) Eval("FAuditFlag")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                </f:TemplateField>
                                                <f:BoundField MinWidth="120px" DataField="FDistributionPoint" HeaderText="作业区" SortField="FDistributionPoint" />
                                                <f:BoundField MinWidth="80px" DataField="FDate" HeaderText="领料日期" SortField="FDate" DataFormatString="{0:yyyy-MM-dd}" TextAlign="Center" />
                                                <f:BoundField MinWidth="120px" DataField="FLinkman" HeaderText="领料人" SortField="FLinkman" TextAlign="Left" />
                                                <f:BoundField MinWidth="80px" DataField="FGroup" HeaderText="班组" SortField="FGroup" />
                                                <f:BoundField MinWidth="80px" DataField="CreateBy" HeaderText="操作员" SortField="CreateBy" TextAlign="Left" />
                                                <f:BoundField MinWidth="80px" DataField="FReceiver" HeaderText="发货人" SortField="FReceiver" TextAlign="Left" />
                                                <f:BoundField MinWidth="120px" DataField="FMemo" HeaderText="备注" SortField="FMemo" />
                                                <%--<f:BoundField MinWidth="120px" DataField="FAssociatedNo" HeaderText="关联空瓶回收" SortField="FAssociatedNo" />--%>
                                                <%--<f:BoundField MinWidth="80px" DataField="FT6BillStatus" HeaderText="T6开票状态" SortField="FT6BillStatus" TextAlign="Left" />
                                                <f:BoundField MinWidth="80px" DataField="FT6PaymentStatus" HeaderText="T6收款状态" SortField="FT6PaymentStatus" TextAlign="Left" />
                                                --%>
                                                <f:BoundField MinWidth="145px" DataField="FTime1" HeaderText="操作时间" SortField="FTime1" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" TextAlign="Center" />

                                            </Columns>
                                            <PageItems>
                                                <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                                </f:ToolbarSeparator>
                                                <f:ToolbarText ID="ToolbarText1" runat="server" Text="每页记录数：">
                                                </f:ToolbarText>
                                                <f:DropDownList runat="server" ID="ddlPageSize" Width="80px" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
                                                    <f:ListItem Text="100" Value="100" />
                                                    <f:ListItem Text="200" Value="200" Selected="True" />
                                                    <f:ListItem Text="300" Value="300" />
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
        function openDetailsUI(keyid) {
            var url = 'business/Produce/Details.aspx?action=6&keyid=' + keyid;
            parent.addExampleTab.apply(null, ['add_tab_' + keyid, basePath + url, '【领料单' + keyid + '详情】', basePath + 'icon/page_find.png', true]);
        };
</script>
</body>
</html>
