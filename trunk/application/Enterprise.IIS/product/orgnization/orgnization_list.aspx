<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="orgnization_list.aspx.cs"
    Inherits="Enterprise.IIS.product.orgnization.orgnization_list" %>

<%@ Register Assembly="Enterprise.OrgChart" Namespace="Enterprise.OrgChart" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>组织机构</title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .alink a, a:hover, a:visited {
            text-decoration: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
        <f:RegionPanel ID="RegionPanel1" ShowBorder="False" runat="server">
            <Regions>
                <f:Region ID="Region1" ShowBorder="false" ShowHeader="false" Split="true"
                    Width="200px" Position="Left" Layout="Fit"
                    runat="server">
                    <Items>
                        <f:Tree ID="trDept" Width="50px" ShowHeader="False"
                            ShowBorder="False" Icon="House" Title="组织机构" EnableAjax="True" runat="server" OnNodeCommand="trDept_NodeCommand"
                            EnableArrows="true" AutoScroll="True">
                        </f:Tree>
                    </Items>
                </f:Region>
                <f:Region ID="Region2" ShowBorder="False" ShowHeader="false" Position="Center" Layout="Fit"
                    BoxConfigAlign="Stretch" BoxConfigPosition="Left" runat="server">
                    <Items>
                        <f:TabStrip ID="mainTabStrip" EnableTabCloseMenu="true" ShowBorder="false" runat="server">
                            <Tabs>
                                <f:Tab ID="Tab1" Title="组织机构" Layout="Fit" runat="server">
                                    <Items>
                                        <f:Grid ID="Grid1" Title="组织机构" PageSize="20" ShowBorder="False" ShowHeader="False"
                                            AllowPaging="true" runat="server" EnableCheckBoxSelect="True"
                                            DataKeyNames="id,role_name" OnPageIndexChange="Grid1_PageIndexChange" IsDatabasePaging="true"
                                            OnSort="Grid1_Sort" SortDirection="ASC" AllowSorting="true"
                                            OnRowDataBound="Grid1_RowDataBound" EmptyText="查询无结果" EnableHeaderMenu="True">
                                            <Toolbars>
                                                <f:Toolbar ID="Toolbar1" runat="server">
                                                    <Items>
                                                        <f:Button ID="btnSearch" OnClick="btnSearch_Click" runat="server" Text="查询" Icon="Zoom" Hidden="False" Size="Medium">
                                                        </f:Button>
                                                        <f:Button ID="btnAdd" runat="server" Text="新增" Size="Medium" Icon="Add" Hidden="True">
                                                        </f:Button>
                                                        <f:Button ID="btnEdit" OnClick="btnEdit_Click" Size="Medium" runat="server" Hidden="True" Text="编辑"
                                                            Icon="PageWhiteEdit">
                                                        </f:Button>
                                                        <f:Button ID="btnBatchDelete" OnClick="btnDelete_Click" Size="Medium" runat="server" Text="删除"
                                                            Icon="Delete" Hidden="True">
                                                        </f:Button>
                                                        <f:Button ID="btnExport" OnClick="btnExport_Click" Size="Medium" runat="server" Text="引出" Icon="PageExcel"
                                                            Hidden="True" EnableAjax="false" DisableControlBeforePostBack="false">
                                                        </f:Button>
                                                    </Items>
                                                </f:Toolbar>
                                            </Toolbars>
                                            <Columns>
                                                <f:BoundField Width="80px" DataField="code" HeaderText="代码" SortField="code" />
                                                <f:BoundField Width="200px" DataField="org_name" HeaderText="名称" SortField="org_name" />
                                                <f:BoundField Width="100px" DataField="code_cn" HeaderText="助记码" SortField="code_cn" />
                                                <f:BoundField Width="100px" DataField="org_type" HeaderText="类型" SortField="org_type" />
                                                <f:BoundField Width="100px" DataField="org_account_name" HeaderText="部门主管" SortField="org_account_name" />
                                                <f:BoundField Width="100px" DataField="org_office_tel" HeaderText="电话" SortField="org_office_tel" />
                                                <f:BoundField Width="100px" DataField="org_office_fax" HeaderText="传真" SortField="org_office_fax" />
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
                                </f:Tab>
                                <f:Tab ID="Tab2" Title="组织机构图" Layout="Fit" runat="server">
                                    <Items>
                                        <f:ContentPanel ID="ContentPanel3" ShowBorder="false" BodyPadding="10px" ShowHeader="false" EnableAjax="True"
                                            AutoScroll="true" CssClass="intro" runat="server">
                                            <div class="alink">
                                                <cc1:OrgChart ID="OrgChart1" runat="server" ChartStyle="Horizontal" />
                                            </div>
                                        </f:ContentPanel>
                                    </Items>
                                </f:Tab>
                            </Tabs>
                        </f:TabStrip>
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>
        <br />
        <f:Window ID="Window1" runat="server" WindowPosition="Center" IsModal="true" EnableClose="false"
            Icon="ApplicationViewDetail" EnableMaximize="true" EnableResize="true" Hidden="True"
            Target="Parent" EnableIFrame="true" IFrameUrl="about:blank" Height="430px" Width="550px"
            OnClose="Window1_Close">
        </f:Window>
    </form>
</body>
</html>
