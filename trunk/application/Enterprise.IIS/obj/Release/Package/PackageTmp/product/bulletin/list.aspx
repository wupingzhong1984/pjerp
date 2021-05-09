<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="list.aspx.cs" Inherits="Enterprise.IIS.product.bulletin.list" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>系统公告</title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Panel1" />
        <f:Panel ID="Panel1" Title="系统公告" runat="server" ShowBorder="false" ShowHeader="False" Layout="Fit">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <f:Button ID="btnAdd" runat="server" Text="新增" Icon="ADD" Hidden="True" Size="Medium">
                        </f:Button>
                        <f:Button ID="btnEdit" OnClick="btnEdit_Click" runat="server" Hidden="True" Text="编辑" Size="Medium"
                            Icon="PageWhiteEdit">
                        </f:Button>
                        <f:Button ID="btnBatchDelete" OnClick="btnDelete_Click" runat="server" Text="删除" Size="Medium"
                            Icon="Delete" Hidden="True">
                        </f:Button>
                        <f:Button ID="btnSearch" runat="server" Text="查询" Icon="Zoom" OnClick="btnSearch_Click" Size="Medium">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
                    <Regions>
                        <f:Region ID="Region1" ShowBorder="false" ShowHeader="false" Split="False"
                            Position="Top" Layout="Fit"
                            BodyPadding="3px" runat="server" Height="68px" EnableCollapse="False">
                            <Items>
                                <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="130px"
                                    runat="server" BodyPadding="5px">
                                    <Rows>
                                        <f:FormRow ID="FormRow1">
                                            <Items>
                                                <f:TextBox ID="txttitle" runat="server" Label="标题">
                                                </f:TextBox>
                                                <f:DropDownList runat="server" ID="ddlFlag" Label="优先级别">
                                                    <f:ListItem Text="全部" Value="" />
                                                    <f:ListItem Text="重要" Value="重要" />
                                                    <f:ListItem Text="紧急" Value="紧急" />
                                                    <f:ListItem Text="一般" Value="一般" />
                                                </f:DropDownList>
                                            </Items>
                                        </f:FormRow>
                                        <f:FormRow ID="FormRow2">
                                            <Items>
                                                <f:DatePicker ID="dpvalidity_s" runat="server" Required="false" Label="有效开始日期" EmptyText="请选择开始日期"
                                                    ShowRedStar="false">
                                                </f:DatePicker>
                                                <f:DatePicker ID="dpvalidity_e" runat="server" Required="false" Label="有效结束日期" EmptyText="请选择结束日期"
                                                    ShowRedStar="false">
                                                </f:DatePicker>
                                            </Items>
                                        </f:FormRow>
                                    </Rows>
                                </f:Form>
                            </Items>
                        </f:Region>
                        <f:Region ID="Region2" ShowBorder="False" ShowHeader="false" Position="Center"
                            Layout="Fit" runat="server" BodyPadding="3px">
                            <Items>
                                <f:Grid ID="Grid1" Title="消息管理" PageSize="20" ShowBorder="False" ShowHeader="false"
                                    AllowPaging="true" runat="server" EnableCheckBoxSelect="True"
                                    DataKeyNames="id,sequence_id" OnPageIndexChange="Grid1_PageIndexChange" IsDatabasePaging="true"
                                    OnSort="Grid1_Sort" SortDirection="ASC" AllowSorting="true" OnRowCommand="Grid1_RowCommand"
                                    EmptyText="查询无结果" EnableHeaderMenu="True">
                                    <Columns>
                                        <f:BoundField Width="330px" DataField="title" HeaderText="标题" SortField="title" />
                                        <f:BoundField Width="130px" DataField="pubdate" HeaderText="发布时间" SortField="pubdate" />
                                        <f:BoundField Width="130px" DataField="precedence" HeaderText="优先级别" SortField="precedence" />
                                        <f:BoundField Width="130px" DataField="validity_s" HeaderText="有效开始日期" SortField="validity_s" DataFormatString="{0:yyyy-MM-dd}" />
                                        <f:BoundField Width="130px" DataField="validity_e" HeaderText="有效结束日期" SortField="validity_e" DataFormatString="{0:yyyy-MM-dd}" />
                                        <f:LinkButtonField HeaderText="详情" Width="80px" CommandName="actView" Text="详情" />

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
        </f:Panel>
        <br />
        <f:Window ID="Window1" runat="server" WindowPosition="Center" IsModal="true"
            Icon="ApplicationViewDetail" EnableMaximize="true" EnableResize="true" Hidden="True" EnableClose="True"
            Target="Parent" EnableIFrame="true"
            OnClose="Window1_Close" IFrameUrl="about:blank" Height="580px" Width="750px">
        </f:Window>
    </form>
    <script type="text/javascript">
        var basePath = '<%= ResolveUrl("~/") %>';
        function openAddFineUI(url) {
            parent.addExampleTab.apply(null, ['add_fineui_tab', basePath + url, '详情', basePath + 'icon/page_find.png', true]);
        }
        function closeActiveTab() {
            parent.removeActiveTab();
        }
    </script>
</body>
</html>
