<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Enterprise.IIS.business.Address.Index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>寄件</title>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <f:Button ID="btnSearch" runat="server" Text="查询" Size="Medium" Hidden="False" Icon="Zoom" OnClick="btnSearch_Click">
                        </f:Button>
                        <f:Button ID="btnAdd" runat="server" Text="新增" Icon="Add" Size="Medium">
                        </f:Button>
                        <f:Button ID="btnEdit" OnClick="btnEdit_Click" runat="server" Hidden="True" Text="编辑"
                            Icon="PageWhiteEdit" Size="Medium">
                        </f:Button>
                        <f:Button ID="btnPrint" runat="server" Text="打印" OnClick="btnPrint_Click"
                            Icon="Printer" Hidden="False" Size="Medium">
                        </f:Button>
                        <f:Button ID="btnClose" EnablePostBack="False" Text="关闭" runat="server" Icon="SystemClose" Size="Medium" OnClientClick="closeActiveTab();">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Regions>
                <f:Region ID="Region2" Title="寄件" Position="Center" ShowBorder="False"
                    BoxConfigAlign="Stretch" Layout="Fit" runat="server" ShowHeader="False">
                    <Items>
                        <f:RegionPanel ID="RegionPanel2" ShowBorder="false" runat="server">
                            <Regions>
                                <f:Region ID="Region3" ShowBorder="false" ShowHeader="false" Split="False"
                                    Position="Top" Layout="Fit"
                                    BodyPadding="3px" runat="server" Height="40px" EnableCollapse="True">
                                    <Items>
                                        <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="False" LabelWidth="80px"
                                            runat="server" BodyPadding="5px" LabelAlign="Right">
                                            <Rows>
                                                <f:FormRow ID="FormRow1">
                                                    <Items>
                                                        <f:TextBox ID="txtFSpell" runat="server" Label="助记码">
                                                        </f:TextBox>
                                                        <f:TextBox ID="txtFName" runat="server" Label="客户名称">
                                                        </f:TextBox>
                                                        <f:Label runat="server" Hidden="True" />
                                                        <f:Label runat="server" Hidden="True" />
                                                    </Items>
                                                </f:FormRow>
                                            </Rows>
                                        </f:Form>
                                    </Items>
                                </f:Region>
                                <f:Region ID="Region4" ShowBorder="False" ShowHeader="false" Position="Center"
                                    Layout="Fit" runat="server" BodyPadding="3px" AutoScroll="True">
                                    <Items>
                                        <f:Grid ID="Grid1" PageSize="20" ShowBorder="false" ShowHeader="false" AllowPaging="true"
                                            runat="server" EnableCheckBoxSelect="True" DataKeyNames="FId" OnPageIndexChange="Grid1_PageIndexChange"
                                            IsDatabasePaging="true" OnSort="Grid1_Sort" SortDirection="ASC"
                                            AllowSorting="true" EmptyText="查询无结果" EnableHeaderMenu="True">
                                            <Columns>
                                                <f:BoundField MinWidth="80px" DataField="FCode" HeaderText="客户代码" SortField="FCode" />
                                                <f:BoundField MinWidth="180px" DataField="FName" HeaderText="客户名称" SortField="FName" />
                                                <f:BoundField MinWidth="120px" DataField="FLinkman" HeaderText="联系人" SortField="FLinkman" />
                                                <f:BoundField MinWidth="80px" DataField="FPhome" HeaderText="电话" SortField="FPhome" />
                                                <f:BoundField MinWidth="80px" DataField="FMoile" HeaderText="手机" SortField="FMoile" />
                                                <f:BoundField MinWidth="160px" DataField="FAddress" HeaderText="邮寄地址" SortField="FAddress" />
                                                <f:BoundField MinWidth="80px" DataField="FZip" HeaderText="邮编" SortField="FZip" />
                                                <f:BoundField MinWidth="148px" DataField="FDate" HeaderText="打印时间" SortField="FDate" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" TextAlign="Center" />
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
        <f:Window ID="Window1" runat="server" WindowPosition="Center" IsModal="true"
            Icon="ApplicationViewDetail" EnableMaximize="true" EnableResize="true" Hidden="True"
            Target="Parent" EnableIFrame="true" IFrameUrl="about:blank" Height="530px" Width="960px"
            OnClose="Window1_Close">
        </f:Window>
        <f:Window ID="Window2" runat="server" WindowPosition="Center" IsModal="true" Title="分类"
            Icon="FeedEdit" EnableMaximize="true" EnableResize="true" Hidden="True"
            Target="Parent" EnableIFrame="true" IFrameUrl="about:blank" Height="220px" Width="380px"
            OnClose="Window1_Close">
        </f:Window>

    </form>
    <object id="LODOP_OB" classid="clsid:2105C259-1E0C-4534-8141-A753534CB4CA" width="0" height="0">
        <embed id="LODOP_EM" type="application/x-print-lodop" width="0" height="0"></embed>
    </object>
    <script src="../../jqueryui/js/jquery-1.8.3.min.js" type="text/javascript"> </script>
    <script src="../../jqueryui/js/jquery-ui-1.9.2.custom.js?v=4.0" type="text/javascript"> </script>
    <script type="text/javascript" language="javascript" src="../../js/LodopFuncs.js"></script>
    <script type="text/javascript">
        var basePath = '<%= ResolveUrl("~/") %>';

        function closeActiveTab() {
            parent.removeActiveTab();
        }

        //--------打印配置---------------
        var LODOP; //声明为全局变量 
        function LodopPrinter(keyid) {
            LODOP = getLodop();
            LODOP.PRINT_INIT("河南禄恒软件科技有限公司");
            var strHtml = "";
            $.post("../../common/AjaxPrinter.ashx?oper=ajaxPrintPost",
                { "FId": keyid },
                function (result) {
                    if (result != "") {

                        LODOP.ADD_PRINT_HTM(0, 1, "100%", "100%", result);
                        LODOP.PREVIEW();
                        //LODOP.PRINT();
                    }
                    else {
                        alert("打印失败！");
                    }
                }, "html");
        };
    </script>
</body>
</html>
