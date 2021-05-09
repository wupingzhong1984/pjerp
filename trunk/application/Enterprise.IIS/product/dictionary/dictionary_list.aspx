<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dictionary_list.aspx.cs"
    Inherits="Enterprise.IIS.product.LHDictionary.dictionary_list" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>字典管理</title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Panel1" />
    <f:Panel ID="Panel1" Title="字典管理" runat="server" ShowBorder="false" Layout="Fit" ShowHeader="False">
        <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <f:Button ID="btnAdd" runat="server" Text="新增" Size="Medium" Icon="ADD" Hidden="True">
                    </f:Button>
                    <f:Button ID="btnEdit" OnClick="btnEdit_Click" runat="server" Size="Medium" Hidden="True" Text="编辑" Icon="PageWhiteEdit">
                    </f:Button>
                    <f:Button ID="btnBatchDelete" OnClick="btnDelete_Click" Size="Medium" runat="server" Text="删除"
                        Icon="Delete" Hidden="True">
                    </f:Button>
                    <f:Button ID="btnEnabled" OnClick="btnEnabled_Click" Size="Medium" runat="server" Hidden="True" Text="禁用/启用"
                                Icon="Stop">
                            </f:Button>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <Items>
            <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
                <Regions>
                    <f:Region ID="Region1" ShowBorder="false" ShowHeader="false" Split="true"
                         Position="Top" Layout="Fit"
                        BodyPadding="3px" runat="server" Height="40px" EnableCollapse="True" >
                        <Items>
                            <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="80px"
                                runat="server" BodyPadding="5px">
                                <Rows>
                                    <f:FormRow ID="FormRow1">
                                        <Items>
                                           
                                            <f:TextBox ID="txtcategory" runat="server" Label="名称">
                                            </f:TextBox>
                                            <f:DropDownList runat="server" ID="ddlFlag" Label="状态">
                                                <f:ListItem Text="全部" Value="" />
                                                <f:ListItem Text="启用" Value="1" />
                                                <f:ListItem Text="禁用" Value="0" />
                                            </f:DropDownList>
                                            <f:Button ID="btnSearch" runat="server" Text="查询" Icon="Zoom" OnClick="btnSearch_Click">
                                            </f:Button>
                                        </Items>
                                    </f:FormRow>
                                </Rows>
                            </f:Form>
                        </Items>
                    </f:Region>
                    <f:Region ID="Region2" ShowBorder="False" ShowHeader="false" Position="Center"
                        Layout="Fit" runat="server" BodyPadding="3px">
                        <Items>
                            <f:Grid ID="Grid1" Title="字典管理" PageSize="20" ShowBorder="False" ShowHeader="false"
                                AllowPaging="true" runat="server" EnableCheckBoxSelect="True"
                                DataKeyNames="id" OnPageIndexChange="Grid1_PageIndexChange" IsDatabasePaging="true"
                                OnSort="Grid1_Sort" SortDirection="ASC"
                                AllowSorting="true"  EmptyText="查询无结果" EnableHeaderMenu="True">
                                <Columns>
                                    <f:BoundField Width="100px" DataField="category" HeaderText="名称" SortField="category" />    
                                    <f:BoundField Width="100px" DataField="key" HeaderText="键" SortField="key" />
                                    <f:BoundField Width="100px" DataField="value" HeaderText="值" SortField="value" />
                                    <f:BoundField Width="100px" DataField="sort" HeaderText="排序" SortField="sort" />
                                    <f:BoundField Width="100px" DataField="desc" HeaderText="描述" SortField="desc" />
                                    <f:CheckBoxField Width="60px" RenderAsStaticField="true" DataField="enable" HeaderText="是否禁用"
                                        SortField="enable" />
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
    <f:Window ID="Window1" runat="server" WindowPosition="Center" IsModal="true" Hidden="True"
        Icon="ApplicationViewDetail" EnableMaximize="true" EnableResize="true" Target="Parent" EnableIFrame="true"
        IFrameUrl="about:blank" Height="400px" Width="650px" OnClose="Window1_Close">
    </f:Window>
    </form>
</body>
</html>
