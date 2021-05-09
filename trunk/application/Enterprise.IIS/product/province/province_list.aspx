<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="province_list.aspx.cs"
    Inherits="Enterprise.IIS.product.province.province_list" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
    <f:RegionPanel ID="RegionPanel1" ShowBorder="False" runat="server">
        <Regions>
            <f:Region ID="Region1" Title="区域划分" Position="Center" ShowHeader="False"
                ShowBorder="False" BoxConfigAlign="Stretch" Layout="Fit" runat="server" BoxConfigPosition="Left">
                <Items>
                    <f:RegionPanel ID="RegionPanel2" ShowBorder="false" runat="server">
                        <Regions>
                            <f:Region ID="Region3" ShowBorder="false" ShowHeader="false" Split="false"
                                 Position="Top" Layout="Fit"
                                BodyPadding="3px" runat="server" Height="40px" EnableCollapse="True">
                                <Items>
                                    <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="80px"
                                        runat="server" BodyPadding="5px">
                                        <Rows>
                                            <f:FormRow ID="FormRow1">
                                                <Items>
                                                    <f:TextBox ID="txtcodeid" runat="server" Label="区域编码">
                                                    </f:TextBox>
                                                    <f:TextBox ID="txtcity_name" runat="server" Label="区域名称">
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
                                    <f:Grid ID="Grid1" Title="区域划分" PageSize="20" ShowBorder="false" ShowHeader="false"
                                        AllowPaging="true" runat="server" EnableCheckBoxSelect="false"
                                        DataKeyNames="id,city_name" OnPageIndexChange="Grid1_PageIndexChange" IsDatabasePaging="true"
                                        OnSort="Grid1_Sort" SortDirection="ASC" AllowSorting="true"
                                        EmptyText="查询无结果" EnableHeaderMenu="True">
                                        <Columns>
                                            <f:BoundField Width="200px" DataField="codeid" HeaderText="区域编码" SortField="codeid" />
                                            <f:BoundField Width="200px" DataField="city_name" HeaderText="区域名称" SortField="city_name" />
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
    </form>
</body>
</html>
