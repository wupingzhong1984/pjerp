<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="log_error.aspx.cs" Inherits="Enterprise.IIS.product.log.log_error" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>导常日志</title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
    <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
        <Regions>
            <f:Region ID="Region1" ShowBorder="false" ShowHeader="False" Title="导常日志" Position="Center" Layout="Fit"
                BoxConfigAlign="Stretch" BoxConfigPosition="Left" runat="server">
                <Items>
                    <f:RegionPanel ID="RegionPanel2" ShowBorder="false" runat="server">
                        <Regions>
                            <f:Region ID="Region3" ShowBorder="false" ShowHeader="false" Split="False"
                                 Position="Top" Layout="Fit"
                                BodyPadding="3px" runat="server" Height="40px" EnableCollapse="False" >
                                <Items>
                                    <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="80px"
                                        runat="server" BodyPadding="5px">
                                        <Rows>
                                            <f:FormRow ID="FormRow1">
                                                <Items>
                                                    <f:TextBox ID="txtaccount_name" runat="server" Label="登录账号" Required="false" ShowRedStar="false"
                                                        EmptyText="登录账号">
                                                    </f:TextBox>
                                                    <f:DatePicker ID="dpBeginDate" runat="server" Required="false" Label="开始日期" EmptyText="请选择开始日期"  ShowRedStar="false"></f:DatePicker> 
                                                    <f:DatePicker ID="dpEndDate" runat="server" Required="false" Label="结束日期" EmptyText="请选择结束日期"  ShowRedStar="false"></f:DatePicker>
                                                    <f:Button ID="btnSearch" runat="server" Icon="Zoom" Text="查询" OnClick="btnSearch_OnClick">
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
                                    <f:Grid ID="Grid1" Title="导常日志" PageSize="20" ShowBorder="false" ShowHeader="false"
                                        Icon="User" AllowPaging="true" runat="server" EnableCheckBoxSelect="True"
                                        DataKeyNames="id" OnPageIndexChange="Grid1_PageIndexChange" IsDatabasePaging="true"
                                        OnRowCommand="Grid1_RowCommand" OnSort="Grid1_Sort" SortDirection="DESC"
                                        AllowSorting="true"  EmptyText="查询无结果" EnableHeaderMenu="True">
                                        <Columns>
                                            <%--<f:BoundField Width="100px" DataField="id" HeaderText="编号" SortField="id" />--%>
                                            <f:BoundField Width="100px" DataField="account_name" HeaderText="登录帐号" SortField="account_name" />
                                            <f:BoundField Width="300px" DataField="action_desc" HeaderText="错误描述" SortField="action_desc" />
                                            <f:BoundField Width="200px" DataField="account_ip" HeaderText="登录IP" SortField="account_ip" />
                                            <f:BoundField Width="200px" DataField="action_on" HeaderText="登录时间" SortField="action_on" />
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
