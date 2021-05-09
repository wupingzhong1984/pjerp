<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="action_list.aspx.cs" Inherits="Enterprise.IIS.product.action.action_list" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>功能管理</title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
    <f:RegionPanel ID="RegionPanel1" ShowBorder="False" runat="server">
        <Regions>
            <f:Region ID="Region1" Title="功能管理" Position="Center" ShowHeader="False"
              ShowBorder="False"  BoxConfigAlign="Stretch" Layout="Fit" runat="server">
                <Toolbars>
                    <f:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <f:Button ID="btnAdd" runat="server" Text="新增" Icon="Add" Hidden="True" Size="Medium">
                            </f:Button>
                            <f:Button ID="btnEdit" OnClick="btnEdit_Click" runat="server" Hidden="True" Text="编辑"
                                Icon="PageWhiteEdit" Size="Medium">
                            </f:Button>
                            <f:Button ID="btnBatchDelete" OnClick="btnDelete_Click" Size="Medium" runat="server" Text="删除" Icon="Delete" Hidden="True">
                            </f:Button>
                        </Items>
                    </f:Toolbar>
                </Toolbars>
                <Items>
                    <f:RegionPanel ID="RegionPanel2" ShowBorder="false" runat="server">
                        <Regions>
                            <f:Region ID="Region3" ShowBorder="false" ShowHeader="false" Split="true"
                                 Position="Top" Layout="Fit"
                                BodyPadding="3px" runat="server" Height="40px" EnableCollapse="True" >
                                <Items>
                                    <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="80px"
                                        runat="server" BodyPadding="5px">
                                        <Rows>
                                            <f:FormRow ID="FormRow1">
                                                <Items>
                                                    <f:TextBox ID="txtaction_name" runat="server" Label="功能名称">
                                                    </f:TextBox>
                                                    <f:TextBox ID="txtaction_en" runat="server" Label="英文名称">
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
                                Layout="Fit" runat="server" BodyPadding="3px">
                                <Items>
                                    <f:Grid ID="Grid1" Title="功能管理" PageSize="20" ShowBorder="false" ShowHeader="false"
                                        AllowPaging="true" runat="server" EnableCheckBoxSelect="True"
                                        DataKeyNames="id,action_name" OnPageIndexChange="Grid1_PageIndexChange" IsDatabasePaging="true"
                                        OnRowCommand="Grid1_RowCommand" OnSort="Grid1_Sort" SortDirection="ASC"
                                        AllowSorting="true"  EmptyText="查询无结果" EnableHeaderMenu="True">
                                        <Columns>
                                            <f:BoundField Width="100px" DataField="action_name" HeaderText="功能名称" SortField="action_name" />
                                            <f:BoundField Width="100px" DataField="action_en" HeaderText="英文名称" SortField="action_en" />
                                            <f:BoundField Width="460px" DataField="action_desc" HeaderText="描述" SortField="action_desc" />
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
    <f:Window ID="Window1" runat="server" WindowPosition="Center" IsModal="true"  Hidden="True"
        Icon="ApplicationViewDetail" EnableMaximize="true" EnableResize="true" Target="Parent" EnableIFrame="true"
        IFrameUrl="about:blank" Height="300px" Width="650px" OnClose="Window1_Close">
    </f:Window>
    </form>
</body>
</html>
