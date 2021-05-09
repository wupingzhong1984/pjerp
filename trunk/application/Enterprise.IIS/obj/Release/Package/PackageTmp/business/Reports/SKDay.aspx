<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SKDay.aspx.cs" Inherits="Enterprise.IIS.business.Reports.SKDay" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>销售日报</title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../../jqueryui/css/ui-lightness/jquery-ui-1.9.2.custom.css" />
    <style type="text/css">
        .x-grid-row-summary .x-grid-cell-inner {
            font-weight: bold;
            color: red;
        }
        .x-grid-row-summary .x-grid-cell {
            background-color: #fff !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region1" Title="销售日报" Position="Center"
                    ShowBorder="false" BoxConfigAlign="Stretch" Layout="Fit" runat="server" ShowHeader="false">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <f:Button ID="Button1" OnClick="btnSearch_Click" runat="server" Text="查询"
                                    Icon="Zoom" Hidden="False" Size="Medium">
                                </f:Button>
                                <f:Button ID="btnPrint" OnClick="btnExport_Click" runat="server" Text="打印"
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
                                    BodyPadding="3px" runat="server" Height="38px" EnableCollapse="True">
                                    <Items>
                                        <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="80px"
                                            runat="server" BodyPadding="5px" LabelAlign="Right">
                                            <Rows>
                                                <f:FormRow ID="FormRow1">
                                                    <Items>
                                                        <f:DropDownList runat="server" ID="ddlCompany" Label="所属公司" LabelAlign="Right"></f:DropDownList>
                                                        <f:DatePicker ID="dpkFDateBegin" runat="server" Label="开始时间" DateFormatString="yyyy-MM-dd" LabelAlign="Right">
                                                        </f:DatePicker>
                                                        <f:DatePicker ID="dpkFDateEnd" runat="server" Label="结束时间" DateFormatString="yyyy-MM-dd" LabelAlign="Right">
                                                        </f:DatePicker>
                                                         <f:TriggerBox ID="tbxFCustomer" EnablePostBack="True" OnTextChanged="tbxFCustomer_OnTextChanged"
                                                            ShowLabel="true" Required="True" ShowRedStar="True" Label="客户名称" LabelAlign="Right"
                                                            Readonly="false" TriggerIcon="Search" runat="server" >
                                                        </f:TriggerBox>

                                                        
                                                        <f:TextBox runat="server" Label="客户代码" ShowRedStar="True" ID="txtFCode" LabelAlign="Right" />

                                                    </Items>
                                                </f:FormRow>
                                            </Rows>
                                        </f:Form>
                                    </Items>
                                </f:Region>
                                <f:Region ID="Region4" ShowBorder="False" ShowHeader="False" Position="Center"
                                    Layout="Fit" runat="server" BodyPadding="3px" AutoScroll="True">
                                    <Items>
                                        <f:Grid ID="Grid1" ShowBorder="False" ShowHeader="False" AllowPaging="False"
                                            runat="server" EnableCheckBoxSelect="False" DataKeyNames="keyId"
                                            OnRowCommand="Grid1_RowCommand"
                                            IsDatabasePaging="False" OnSort="Grid1_Sort" SortDirection="ASC"
                                            AllowSorting="true" EmptyText="查询无结果" EnableHeaderMenu="True"
                                            EnableSummary="true" SummaryPosition="Bottom">
                                            <Columns>
                                                <f:BoundField MinWidth="130px"  ColumnID="keyId" DataField="KeyId" HeaderText="收款单" SortField="keyId" />
                                                <f:BoundField MinWidth="80px" DataField="FCode" HeaderText="客户代码" SortField="FCode" />
                                                <f:BoundField MinWidth="220px" DataField="FName" HeaderText="客户名称" SortField="FName" />
                                                <f:BoundField MinWidth="120px" DataField="FAddress" HeaderText="地址" SortField="FAddress" />
                                                <f:BoundField MinWidth="120px" DataField="FLinkman" HeaderText="联系人" SortField="FLinkman" />
                                                <f:BoundField MinWidth="120px" DataField="FPhome" HeaderText="联系电话" SortField="FPhome" />
                                                <f:BoundField MinWidth="80px" ColumnID="FAmount" DataField="FAmount" HeaderText="金额" SortField="FAmount" TextAlign="Right" />
                                                <f:BoundField MinWidth="80px" DataField="FAbstract" HeaderText="收款类型" SortField="FAbstract" />
                                                <f:BoundField MinWidth="120px" ColumnID="FMemo"  DataField="FMemo" HeaderText="备注" SortField="FMemo" />
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
        <f:Window ID="Window1" runat="server" WindowPosition="Center" IsModal="True" EnableClose="True"
            Icon="ApplicationViewDetail" Title="客户档案" EnableMaximize="True" EnableResize="True" Hidden="True"
            Target="Parent" EnableIFrame="True" IFrameUrl="about:blank" Height="580px" Width="960px"
            OnClose="Window1_Close">
        </f:Window>

        <f:Window ID="Window2" Icon="PageAttach" runat="server" Hidden="true"
            IsModal="true" Target="Parent" EnableMaximize="true" EnableResize="true" OnClose="Window2_Close"
            Title="产品档案" CloseAction="HidePostBack"
            EnableIFrame="true" Height="450px" Width="850px">
        </f:Window>

    </form>
    <script src="../../jqueryui/js/jquery-1.8.3.min.js" type="text/javascript"> </script>
    <script src="../../jqueryui/js/jquery-ui-1.9.2.custom.js?v=4.0" type="text/javascript"> </script>
    <script type="text/javascript">
        var basePath = '<%= ResolveUrl("~/") %>';
        function openDetailsUI(date1, date2, code) {
            var url = 'business/Reports/Finance/BankAccountsDetails.aspx?action=6&bDate=' + date1+"&eDate="+date2+"&code="+code;
            parent.addExampleTab.apply(null, ['add_tab_' + code, basePath + url, '明细', basePath + 'icon/page_find.png', true]);
        };
        function closeActiveTab() {
            parent.removeActiveTab();
        };
        F.ready(function () {
            var txtcode = '<%= tbxFCustomer.ClientID %>';

            $('#' + txtcode + ' input').autocomplete({
                source: function (request, response) {
                    $.getJSON("../../Common/AjaxCustomer.ashx", request, function (data, status, xhr) {
                        response(data);
                    });
                }
            });
        });
    </script>
</body>
</html>