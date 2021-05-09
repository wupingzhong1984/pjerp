<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WinUnitLeaseBottle.aspx.cs" Inherits="Enterprise.IIS.Common.WinUnitLeaseBottle" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>客户租赁未还瓶统计</title>
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region2" Title="客户管理" Position="Center"
                    ShowBorder="True" BoxConfigAlign="Stretch" Layout="Fit" runat="server" ShowHeader="False">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <f:Button ID="btnClose" Text="关闭" Size="Medium" runat="server" Icon="SystemClose">
                                </f:Button>
                                <f:Button ID="Button1" runat="server" Text="查询" Size="Medium" Icon="Zoom" OnClick="btnSearch_Click">
                                </f:Button>
                                <f:Button ID="btnSubmit" Text="确定" runat="server" Size="Medium" Icon="SystemSaveNew" ValidateForms="SimpleForm1"
                                    OnClick="btnSubmit_Click">
                                </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
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
                                                        <f:TextBox ID="txtFCode" runat="server" Label="编码" AutoPostBack="True" OnTextChanged="tbxFText_OnTextChanged" >
                                                        </f:TextBox>
                                                        <f:TextBox ID="txtFName" runat="server" Label="名称" AutoPostBack="True" OnTextChanged="tbxFText_OnTextChanged" >
                                                        </f:TextBox>
                                                    </Items>
                                                </f:FormRow>
                                            </Rows>
                                        </f:Form>
                                    </Items>
                                </f:Region>
                                <f:Region ID="Region4" ShowBorder="False" ShowHeader="false" Position="Center"
                                    Layout="Fit" runat="server" BodyPadding="3px" AutoScroll="True">
                                    <Items>
                                        <f:Grid ID="Grid1" PageSize="2000" ShowBorder="false" ShowHeader="false" AllowPaging="false"
                                            runat="server" EnableCheckBoxSelect="True" DataKeyNames="FId" OnPageIndexChange="Grid1_PageIndexChange"
                                            IsDatabasePaging="false" OnSort="Grid1_Sort" SortDirection="ASC" EnableRowDoubleClickEvent="true" OnRowDoubleClick="Grid1_RowClick"
                                            AllowSorting="false" EmptyText="查询无结果" EnableHeaderMenu="True" >
                                            <Columns>
                                                <f:BoundField MinWidth="80px" DataField="FDate" HeaderText="租赁日期" SortField="FDate" DataFormatString="{0:yyyy-MM-dd}" TextAlign="Center" />
                                                <f:BoundField MinWidth="60px" DataField="FBottle" HeaderText="编码" SortField="FBottle" />
                                                <f:BoundField MinWidth="110px" DataField="FName" HeaderText="名称" SortField="FName" />
                                                <f:BoundField MinWidth="80px" DataField="FSpec" HeaderText="规格" SortField="FSpec" />
                                                <f:BoundField MinWidth="80px" DataField="FUnit" HeaderText="单位" SortField="FUnit" />
                                                <f:BoundField MinWidth="80px" DataField="ARBottleQty" HeaderText="欠瓶量" SortField="ARBottleQty" TextAlign="Right" />
                                                <f:BoundField MinWidth="80px" DataField="FPrice" HeaderText="押金/只" SortField="FPrice" TextAlign="Right" />
                                                <f:BoundField MinWidth="80px" DataField="FRentDay" HeaderText="租金/天" SortField="FRentDay" TextAlign="Right" />
                                                <f:BoundField MinWidth="80px" DataField="days" HeaderText="天数" SortField="days" TextAlign="Right" />
                                                <f:BoundField MinWidth="80px" DataField="ARRentals" HeaderText="应收租金" SortField="ARRentals" TextAlign="Right" />
                                            </Columns>
                                            <%--<PageItems>
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
                                            </PageItems>--%>
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
</body>
</html>
