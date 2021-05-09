<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProduceInReport.aspx.cs" Inherits="Enterprise.IIS.business.Reports.ProduceInReport" %>

<%@ Import Namespace="Enterprise.Framework.Enum" %>
<%@ Import Namespace="Enterprise.IIS.Common" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
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
                                <f:Button ID="btnPrint" OnClick="btnPrint_Click" runat="server" Text="打印"
                                    Icon="Printer" Hidden="False" Size="Medium">
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
                                    BodyPadding="3px" runat="server" Height="68px" EnableCollapse="True">
                                    <Items>
                                        <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="80px"
                                            runat="server" BodyPadding="5px" LabelAlign="Right">
                                            <Rows>
                                                <f:FormRow ID="FormRow1" >
                                                    <Items>
                                                        <f:DatePicker ID="dpkFDateBegin" runat="server" Label="开始日期" DateFormatString="yyyy-MM-dd" LabelAlign="Right" Required="true">
                                                        </f:DatePicker>
                                                        <f:DatePicker ID="dpkFDateEnd" runat="server" Label="结束日期" DateFormatString="yyyy-MM-dd" LabelAlign="Right" Required="true">
                                                        </f:DatePicker>
                                                        <f:TextBox ID="txtT6Code" runat="server" Label="T6编码" LabelAlign="Right">
                                                        </f:TextBox>
                                                    </Items>
                                                </f:FormRow>                                                
                                                <f:FormRow ID="FormRow2">
                                                    <Items>
                                                        <f:TextBox ID="txtFcode" runat="server" Label="产成品编码" LabelAlign="Right">
                                                        </f:TextBox>
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
                                    Layout="Fit" runat="server" BodyPadding="8px" AutoScroll="True">
                                    <Items>
                                        <f:Grid ID="Grid1" ShowBorder="False" ShowHeader="False" AllowPaging="False"
                                            runat="server" EnableCheckBoxSelect="False" 
                                            KeepCurrentSelection="True" EnableMultiSelect="True"
                                            IsDatabasePaging="False"  SortDirection="ASC"
                                            AllowSorting="true" EmptyText="查询无结果" EnableHeaderMenu="True"
                                            EnableSummary="true" SummaryPosition="Bottom">
                                            <Columns>
                                                <f:BoundField MinWidth="80px" DataField="FINum" HeaderText="T6编码" SortField="FINum" />
                                                <f:BoundField MinWidth="80px" DataField="FItemCode" HeaderText="编码" SortField="FItemCode" />
                                                <f:BoundField MinWidth="80px" DataField="FName" HeaderText="名称" SortField="FName" TextAlign="Right" />
                                                <f:BoundField MinWidth="80px" DataField="FSpec" HeaderText="规格" SortField="FSpec" TextAlign="Right" />
                                                <f:BoundField MinWidth="80px" DataField="FUnit" HeaderText="单位" SortField="FUnit" TextAlign="Right" />
                                                <f:BoundField MinWidth="80px" DataField="FQty" HeaderText="数量" SortField="FQty" TextAlign="Right" />
                                                <f:BoundField MinWidth="80px" DataField="FBottleCode" HeaderText="包装物编号" SortField="FBottleCode" TextAlign="Right" />
                                                <f:BoundField MinWidth="80px" DataField="FBName" HeaderText="包装物名称" SortField="FBName" TextAlign="Right" />
                                                <f:BoundField MinWidth="80px" DataField="FBspec" HeaderText="包装物规格" SortField="FBspec" TextAlign="Right" />
                                                </Columns>
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
